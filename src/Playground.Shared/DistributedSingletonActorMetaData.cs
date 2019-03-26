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

        public IActorRef StartProxy(ActorSystem system)
        {
            return system.ActorOf(
                ProxyProps(system),
                ProxyName(Name));
        }


        public IActorRef Start(ActorSystem system, Props actorProps)
        {
            return system.ActorOf(
                ManagerProps(system, actorProps),
                Name);
        }


        public ClusterSingletonManagerSettings ManagerSettings(ActorSystem system)
        {
            var config = system.Settings.Config.GetConfig("akka.cluster.singleton");
            var handoverInternval = config.GetTimeSpan("hand-over-retry-interval", new TimeSpan?(), true);                                    
            return new ClusterSingletonManagerSettings(
                Name,
                Role.IfNoneUnsafe(() => null),
                Akka.Cluster.Cluster.Get(system).DowningProvider.DownRemovalMargin,
                handoverInternval
            ).WithSingletonName(Name);            
        }

        public ClusterSingletonProxySettings ProxySettings(ActorSystem system)
        {
            var config = system.Settings.Config.GetConfig("akka.cluster.singleton-proxy");
            var identificationInterval = config.GetTimeSpan("singleton-identification-interval", new TimeSpan?(), true);
            var bufferSize = config.GetInt("buffer-size", 0);
            return new ClusterSingletonProxySettings(
                Name,
                Role.IfNoneUnsafe(() => null),
                identificationInterval,
                bufferSize
            );            
        }

        public Props ProxyProps(ActorSystem system)
        {                        
            return ClusterSingletonProxy.Props(
                "/user/"+Name,
                 ProxySettings(system)
            );
        }

        public string ProxyName(string actorName)
        {
            return $"{actorName}Proxy";
        }
        
        public Props ManagerProps(ActorSystem system, Props childProps)
        {
            return ClusterSingletonManager.Props(
                childProps,
                PoisonPill.Instance,
                ManagerSettings(system)
            );
        }
    }
}
