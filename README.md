# Modular monolith

## Why?

Modular monolith is a monolith with strictly defined boundaries between each module. Communication between them should be based on contracts that each independent module is providing. Most of the communications should be done in an asynchronous way, but synchronous communication is also possible.

Modular monolith is a good way to start developing a new project within the unexplored domain and with constantly changed requirements at the beginning of the project - it simplifies the deployment (as we have only one unit to deploy), it allows us to change our domain cheaper and faster (it's simpler to move some code within the single project between modules or rearrange the modules, than changing microservice or their boundaries within multiple microservices).

Modular monolith is also a good way to refactor "big ball of mud" projects. Extracting local modules within project is always simpler than starting a microservices revolution.

Thankfully to our principles and concepts, we're always ready to move an independent module to a microservice (in case of e.g. scalability requirements).

## Common concepts / patterns

### Domain-Driven Design

We will use the Domain-Driven Design concept in our modular monolith project (it is a quite "hot" topic in current IT and there are a lot of books/blogs about it, so I won't describe it in detail).

### CQRS

CQRS stands for Command Query Responsibility Segregation. At its heart is the notion that you can use a different model to update information than the model you use to read information.

### Transactional outbox

A command typically needs to update the aggregate and publish domain events atomically. The database update and sending of the message must be atomic in order to avoid data inconsistencies and bugs. When you save data as part of one transaction, you also save domain events that you later want to process as part of the same transaction. The second element of this pattern is a separate process that periodically checks the contents of the Outbox messages and processes them (with at-least-once delivery pattern).

### Asynchronous communications / processing

We will use asynchronous communications with RabbitMQ as our base communication protocol betweens independent modules.

## Use cases

### Read information

UI -> API -> Queries -> Read only database (No lock / No tracking, Eventual consistency)

### Update information

UI -> API -> Command -> Aggregate -> Domain Events (Single transaction)

### Outbox processor

Domain Events -> RabbitMQ (At least one times published)

### Asynchronous processing 

RabbitMQ -> Event Handlers (At least one delivery)
Event Handlers -> Commands (Reaction in the same or different domain to some events)
Event Handlers -> Updating read only database

## Known issues

Commonly used `OwnsOne` function produces overly complicated SQL https://github.com/dotnet/efcore/issues/18299