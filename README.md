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

![Read information](http://www.plantuml.com/plantuml/png/NP31IiGm48RlynG_lUyBU90LriEULXSNpsCxIY1DIcO2zksTk1NRGYxvv-ERFpcgSR4wHgAtO57GNOK7XFIPoyWMSkg8kBTSS5CfD3aT-Z1nCZozd3Vtrofbfe6DFrWNqo6dEEClwy2A5byyFIqXKJFZyDY4UC2xknIMyIK6pQEisv0pZjvtEVpyBNqrKRe7Fjdh7Rea54-nwK9xSAszYjMIz1V-L_lk_MofqrxWerFGAVAyBWm_jpFHKTBWt_m3)

### Update information

![Update information](http://www.plantuml.com/plantuml/png/ZP31QiD034Jl-WgHUt-Wu9BGzD1JGqdeMROLUs5NQrRQalnzPMeJV2ZqOcpcpNWqUpKgLbC4E0UBZBlBr9Cn1hdcaiX2bXq0TPOBNfGBJCw7BamaXeVJ--RxDQT4qc-POIWyU2HuBhMaZCVb1M1EnAUNEGYVySjvHSlOZIG3e-R4DWOPm7N7rdn7tsxSLME5fi5p8L7gvghOvYezbJis3Ioq6nybt7gOfdXVVv457fg3QpNdbY7K_FadTAfj33fYdnC5GVxcvOIFQcs-FNpR-fzijOXYOQtH_dV4CjBMkYup3-dYCUWKwRw8cAze8-DlBC2UfQyf_W00)

### Outbox processor

![Outbox processor](http://www.plantuml.com/plantuml/png/VP11IiOm44RtSugFV-y5X4WNWgjYbLvWqWmOh4dCJ4HlRw4r2odTXO_tU646DQxMFbQYETegk3qty_aRfUP5L7Ez4GKszgo2n-D1PPFY4Widmua2LkpZzBVHPvCcU6NleuqlHCvXhfoK5uivmUUM0jSVE4V_LDmzz3BkyII6bahpQzGt2P0lIQOdKezjqhJZ7RuiZ5pV3nAiwAchFzCWAMnt_0K0)

### Asynchronous processing 

![Asynchronous processing - react for domain events and execute action on different aggregate](http://www.plantuml.com/plantuml/png/VP5XQkim48J_wnI3_-S23_n8U2_GFwLjI0-mjZQEG9PSxQeaj--cSPecq88nOlJjpWnQYbBHEaRdja4Zerd9CVNxabEkWgdadaL26h10OUeLkrpWyqWXWTyvgO2I1n-uhyem8kG4-tpOxRWO01g6mWCfDywzLQwC3NLTqCSNDvbvwCD4XZNhqphiRLta8Wr8y2azd9KR-5yUHvDljDN5ntbIwaWO_-U3S-xxAYp-VcN07snEnGHvRdIFNioCN9zhYcjRR0ibcVjskIPFvOYsTJDjWzT4DhgQfdX4R_K6DYjtlJFmajEurycVH57x_mWzrou6sTyzX6OyLUtouJft6t_3MaiI59OQzRSIRid9rp5-0000)

![Asynchronous processing - update read models based on domain events](http://www.plantuml.com/plantuml/png/POz1IWGn44NtTOfFzdq15pA2WXiH1pr0TQTq0abLcqg4kRtfSHIPsJvU_PVil75jlMIYrzIo8DpvKPT3DRNkMAijufxq0nFw6ha9gd14iIZPCRDBX2cY5KuA-H9j7eW-ktJ1dkSvjQSNMaSaBMbbRGWFcuG3QynIFO0T9_JuGsXKU1j6s8_KP9gF-6KdUyFFWzwV8T7_UqotVs7SRFBI77P-7DwhbKl_Qd7SlLt-5gP4Ez7OI_u6)

## Known issues

Commonly used `OwnsOne` function produces overly complicated SQL https://github.com/dotnet/efcore/issues/18299