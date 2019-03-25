using System;
using Akka.Actor;
using Akka.Cluster.Tools.Singleton;
using LanguageExt;

namespace Playground.Shared
{
    public class DistributedSingletonActorMetaData : DistributedActorMetaData
    {
        
        public DistributedSingletonActorMetaData(string name, Option<string> role) : base(name, role)
        {
        }

        public Props ProxyProps(ActorSystem system)
        {
            var config = system.Settings.Config.GetConfig("akka.cluster.singleton-proxy");
            var identificationInterval = config.GetTimeSpan("singleton-identification-interval", new TimeSpan?(), true);
            var bufferSize = config.GetInt("buffer-size", 0);
            
            return ClusterSingletonProxy.Props(
                "user/singleton-manager/"+Name,
                 new ClusterSingletonProxySettings(
                    Name,
                    Role.IfNoneUnsafe(() => null),
                    identificationInterval,
                    bufferSize
                )
            );
        }
    }
}
