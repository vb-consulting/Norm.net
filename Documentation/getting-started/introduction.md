---
title: Introduction
order: 1
nextUrl: /docs/getting-started/installation/
nextTitle: Installation
---

# Introduction

`Norm` is a Micro-ORM library for efficient data access in the `.NET` ecosystem. 

It supports .NET Standard 2.1 or higher and .NET Core 3.0 and higher (.NET Core 3, .NET 5, 6, 7, 8, etc). See the full [compatibility table](/#compatibility).

Micro-ORM libraries implement one-way mapping - from your database commands and queries - to  `.NET` types and structures.

That means that **all database commands and queries must be handwritten in SQL** - and `Norm` is here to help you efficiently map the results into the `.NET` types and structures.

`Norm` is a **set of extensions** over the [`System.Data.Common.DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) object.

That means that if the connection implements `System.Data.Common.DbConnection` type - the database that implements that connection is supported. And that includes, for  example:

- [`SqlConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection) for SQL Server databases.
- [`NpgsqlConnection`](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html) for PostgreSQL databases.
- [`SqliteConnection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqliteconnection) for SQLite databases.
- [`MySqlConnection`](https://mysqlconnector.net/api/mysqlconnector/mysqlconnectiontype/) for MySQL databases.
- Any database provider that implements [`System.Data.Common.DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection)
