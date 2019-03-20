# Akka.net Cluster Playground

## Set-up
This repository contains a docker-compose playground designed for learning Akka.Net Clustering techniques.
The application has a WebApi node, two identical worker nodes (you could add more if you wanted), just modify the compose
file, and an instance of Petabridge Lighthouse, which is a dedicated seed node for the akka cluster.

To get started, please make sure you have the latest version of Docker installed on your machine.

If you are on Jetbrains Rider, it is helpful to have the Docker tools installed. 
If you are on Visual Studio, ensure that you have the latest container tools installed, and if you run into any problems,
you can consult this [troubleshooting guide](https://docs.microsoft.com/en-us/visualstudio/containers/vs-azure-tools-docker-troubleshooting-docker-errors?view=vs-2017)
Using either of these two IDEs should allow you to simply click Start and have this clustered environment running.  

Alternatively if you prefer to use the terminal, you can start this project by calling `docker-compose up` from the 
project root directory, and likewise `docker-compose down` to stop the environment.

## The Project
Playground.Web is a Aspnet Core REST API. It uses the cluster client to communicate with the worker nodes.