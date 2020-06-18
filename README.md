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

![Read information](http://www.plantuml.com/plantuml/proxy?src=https://raw.githubusercontent.com/sygnowskip/modular-monolith/master/Docs/read-information.puml?token=ACHEMPNMCX3B2YQ57HMS7H265OS2I)

### Update information

![Update information](http://www.plantuml.com/plantuml/proxy?src=https://raw.githubusercontent.com/sygnowskip/modular-monolith/master/Docs/update-information.puml?token=ACHEMPKGT5NXU5SY7IU33AC65OS3U)

### Outbox processor

![Outbox processor](http://www.plantuml.com/plantuml/proxy?src=https://raw.githubusercontent.com/sygnowskip/modular-monolith/master/Docs/outbox-processor.puml?token=ACHEMPJCQH4UN6QRZ3OUQ5265OSYI)

### Asynchronous processing 

![Asynchronous processing - react for domain events and execute action on different aggregate](http://www.plantuml.com/plantuml/proxy?src=https://raw.githubusercontent.com/sygnowskip/modular-monolith/master/Docs/asynchronous-processing-command.puml?token=ACHEMPM5CFHZAV23RWKWIJK65OSUU)

![Asynchronous processing - update read models based on domain events](http://www.plantuml.com/plantuml/proxy?src=https://raw.githubusercontent.com/sygnowskip/modular-monolith/master/Docs/asynchronous-processing-read-model.puml?token=ACHEMPK3VKDTNKZXWFTN3U265OSWS)

## Known issues

Commonly used `OwnsOne` function produces overly complicated SQL https://github.com/dotnet/efcore/issues/18299