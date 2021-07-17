# CHANGELOG

[Improvement] Replacing read model entities (EF Core) with Dapper for querying purposes

Pros:
* entity and it's configuration is no longer needed (and database view, to avoid issues with multiple entities mapped to single table in EF Core between read and write models)
* objects returned by queries executed with Dapper are untracked by default (in EF Core you need to remember about calling `.AsNoTracking()`)
* possibility to write optimal queries with raw SQL

Cons:
* maintenance of raw SQL queries in C# code
