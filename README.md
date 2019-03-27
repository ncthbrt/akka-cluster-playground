# Akka.net Cluster Playground

## Set-up
This repository contains a docker-compose playground designed for learning Akka.Net Clustering techniques.
The application has a WebApi node, two identical worker nodes (you could add more if you wanted), just modify the compose
file, and an instance of Petabridge Lighthouse, which is a dedicated seed node for the akka cluster.

To get started, please make sure you have the latest version of Docker installed on your machine.

If you are on Jetbrains Rider, it is helpful to have the Docker tools installed. 
If you are on Visual Studio, ensure that you have the latest container tools installed. You will need to right-click and set docker-compose.yml as the startup project. If you run into any problems,
you can consult this [troubleshooting guide](https://docs.microsoft.com/en-us/visualstudio/containers/vs-azure-tools-docker-troubleshooting-docker-errors?view=vs-2017)
Using either of these two IDEs should allow you to simply click Start and have this clustered environment running.  

Alternatively if you prefer to use the terminal, you can start this project by calling `docker-compose up` from the 
project root directory, and likewise `docker-compose down` to stop the environment.

## The Project
Playground.Web is a Aspnet Core REST API. To view the available API endpoints, browse to http://localhost:5050/swagger
while your solution is running. 

The Web API communicates with the Worker node which is in a clustered configuration. This example has two different 
types of Clustered Actors, a example of a ClusterSingletonActor (`TicketCounterActor.cs`) and a type of 
ClusterShardedActor (`AnimalActor.cs`).

The manner in which the actors start up is controlled by the Cluster extension, with the actors being registered with 
the system in the worker node in `WorkerService.cs`, and are referenced in the Api via actor proxy which is started in
`WebService.cs`. The controllers use these proxies to send messages to the actual actors in the worker node. 

Of note in both `WebService` and `WorkerService` is the use of graceful shutdown, which ensures that a node is gracefully
stopped, leading to a more reliable system. 

This project implements some of Petabridge's `ActorMetaData` patterns to reduce the likelihood of typos. 
Refer to `DistributedSingletonActorMetaData`, `DistributedShardedActorMetaData` and `ActorPaths` for more details.


