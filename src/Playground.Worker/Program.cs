using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Playground.Shared;

namespace Playground.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configTemplate = File.ReadAllText("./akka.conf");
            var systemName = "playground";
            var akkaConfig = 
                ConfigurationTemplating.WithEnvironmentVariables(configTemplate, ConfigurationFactory.Default())
                    .WithFallback(DistributedPubSub.DefaultConfig())
                    .WithFallback(ConfigurationFactory.Default());            
            var workerService = new WorkerService(systemName, akkaConfig);
            var tokenSource = new CancellationTokenSource();
            AppDomain.CurrentDomain.ProcessExit += (_, __) => tokenSource.Cancel();
            workerService.Start(tokenSource.Token);
            await workerService.TerminationHandle;
        }
    }
}
