# Akka.net Cluster Playground

## Set-up
This repository contains a docker-compose playground designed for learning Akka.Net Clustering techniques.
The application has a WebApi node, two identical worker nodes, and an instance of Petabridge Lighthouse, which is a dedicated seed node for the akka cluster.
You could add more worker nodes if you wanted, just modify the compose file.

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


## Exercises

### Prerequisites

This is a short training course on using Akka.NET effectively in a distributed fashion. By the end of this course you should have an understanding of the basic concepts of distributed akka, understand how to debug and what effect modifying the config has. In order to get started you will need to complete the following tasks:

a) You will need to be able to use Docker Compose for local development.

b) Clone the Akka cluster playground repo (https://github.com/ncthbrt/akka-cluster-playground) and open the solution in an IDE. The solution models a Zoo with each animal represented by an actor.

c) Select the docker.compose file as the startup project.

d) Build and run the application.

e) The API documentation is made available using Swagger (https://swagger.io/). To view the docs, with the application running navigate to http://localhost:5050/swagger/index.html


### Exercise 1: Petabridge Command Line

Petabridge.cmd (https://cmd.petabridge.com/) is a way to manage Akka.NET clusters and applications on .NET Core. We integrate this tool with our actor solutions so that we can easily manage our Akka resources using a CLI.

The Playground repo already has Petabridge integrated with the Worker cluster. To complete this exercise, do the following tasks:

a) Add the Petabridge.cmd tool to the Web cluster as well.

b) Log into the Petabridge.cmd tool on local and view the cluster
tip: run >pbm 127.0.0.1:{port}
the ports of the nodes are specified in the docker-compose.

c) Make a few animals so we have some actors. Refer to http://localhost:5050/swagger/index.html

d) Check which node has the singleton and DOWN the worker that has the singleton and see what changes in the cluster.
tip: on the petabridge console, >actor hierarchy will show you whether the current node you are on has the singleton.

e) Look at the logs on the worker container, and see how the singleton migrates to the other worker.

### Exercise 2: Creating sharded actors

In the solution, we have a sharded actor Playground.Worker.Actors.AnimalActor and a singleton actor Playground.Worker.Actors.TicketCounterActor. We would like to keep track of visitors to the Zoo by adding a new actor for each visitor, and requiring that they have a ticket. This way, we can deny entry to the zoo once it gets too many visitors. This task will introduce you to creating sharded actors, and will require you to reference the worker cluster from itself. You always need a proxy to access a singleton, so you will also learn how to set up a singleton.

a) Create a new sharded actor "Visitor" on the on the worker cluster. Look at the Animal actor for guidance.

b) Implement a proxy on the worker cluster singleton to allow it to access itself. This is already done in the web cluster.

c) Add logic that requests a ticket from the ticket counter actor when creating a visitor. There is a limited number of tickets available, and when these run out, the visitor should be denied entrance into the zoo.

d) Wire up the visitor to the controller. When GET-ing a visitor, the user should get feedback as to whether the visitor has a ticket for the zoo or not.

### Exercise 3: Docker.compose

Open up the docker-compose.yml file. We use this to specify how we provision and network new nodes.

a) Create a third worker node. Make sure it's networked properly.

b) Add another lighthouse and add it to the seed node. Make sure they start in the same order each time using the depends_on field.
tip: ALL seed node specs need to be updated: in akka.conf (arrays) as well as in docker-compose (comma-seperated strings).

### Exercise 4: Topology changes

This last exercise involves putting several of the previous pieces together to see how changes can affect the system topology. Note that autodowning is not necessarily recommended practice, but this exercise will illustrate how it all works. See https://getakka.net/articles/clustering/split-brain-resolver.html for a preferred alternative.

a) Stop one of the containers with docker stop <containername>.

b) Connect to the petabridge.cmd on one of the other nodes.

c) Check the status of the cluster to see that the one that was shutdown badly is marked as unreachable.

d) Use petabridge.cmd to down the node manually. This should stop the error spam from the other nodes.

e) Add a line to the config line for autodowning unresponsive nodes after a timeout. 
Repeat steps a) to d), and notice that the node is removed automatically.

f) Now fix it permanently in code by uncommenting the coordinated shutdown line in the StopAsync() method of the WorkerService.
Repeat steps a) to d) and notice that the coordinated shutdown now brings down the node automatically without having to wait. This makes the system more responsive to topology changes.