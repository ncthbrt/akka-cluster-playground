using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;

namespace Playground.Worker
{
    public class WorkerService
    {
        private readonly string _actorSystemName;
        private readonly Config _akkaConfig;

        public WorkerService(string actorSystemName, Config akkaConfig)
        {
            _actorSystemName = actorSystemName;
            _akkaConfig = akkaConfig;
        }       
        
        private ActorSystem _system;
        public Task TerminationHandle => _system.WhenTerminated;

        public void Start(CancellationToken token)
        {
            _system = ActorSystem.Create(_actorSystemName, _akkaConfig);
            token.Register(StopAsync);
        }
        
        private async void StopAsync()
        {
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }       
    }
}
