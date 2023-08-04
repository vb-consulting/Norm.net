---
title: Introduction
---

# Introduction

`Norm` is a Micro-ORM library for efficient data access in the `.NET` ecosystem (see the [compatibility table](/#compatibility).)

Micro-ORM library implements one-way database mapping only - from your database commands and queries to  `.NET` types and structures.

`Norm` is implemented as a set of extensions over the [`DbConnection`](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection) object.

## Installation

### .NET Command-Line-Interface

Run this command from the command line to install the latest version:

```bash
dotnet add package Norm.net --version {version}
```

### Visual Studio Package Manager Console (PowerShell)

Run this command from the Visual Studio Package Manager Console (PowerShell) to install the latest version:

```powershell
NuGet\Install-Package Norm.net -Version {version}
```

### Package Reference

Add this line into your `csproj` project file inside `<ItemGroup>` section to install the latest version:

```xml
<PackageReference Include="Norm.net" Version="{version}" />
```

### Script & Interactive

In your C# Interactive console or other scripting engine (such as `csharprepl`), execute following command to install the latest version:

```yaml
#r "nuget: Norm.net, {version}
```

## First use

```csharp
using Norm;

connection.Read("select id, name from table");
```
