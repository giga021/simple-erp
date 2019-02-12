# About
Simple-ERP is example of implementing few microservices using modern concepts like:

 - Event sourcing - data is stored in event store 
 - CQRS - there are two separate models (write - event store and read - SQL database)
 - Domain driven design - service modeled using concepts from DDD like bounded context, ubiquitous language, aggregates, entities, value objects, domain services, repositories...

It models workflow from accounting like adding to, removing from and changing journal entry.

# Technology
 - .NET Core 2.2
 - EventStore 4.1
 - MySQL 5.7
 - RabbitMQ 3 (with MassTransit as service bus)
 - IdentityServer4
 - EntityFramework 2.2
 - React 16
 - Docker

# Running
In order to run it you should have Visual Studio 2017 and Docker installed, everything else will be automatically installed when started.

Demo credentials
Username: **user1**
Password: **User.1**