---
title: Installation
order: 2
nextUrl: /docs/getting-started/first-use/
nextTitle: First Use
prevUrl: /docs/getting-started/introduction/
prevTitle: Introduction
---

# Installation

## .NET Command-Line-Interface

Run this command from the command line to install the latest version:

```bash
dotnet add package Norm.net --version {version}
```

## Visual Studio Package Manager Console (PowerShell)

Run this command from the Visual Studio Package Manager Console (PowerShell) to install the latest version:

```powershell
NuGet\Install-Package Norm.net -Version {version}
```

## Package Reference

Add this line to your `csproj`` project file inside `<ItemGroup>` section to install the latest version:

```xml
<PackageReference Include="Norm.net" Version="{version}" />
```

## Script & Interactive

In your C# Interactive console or another scripting engine (such as `csrepl`, for example) - execute the following command to install the latest version:

```yaml
#r "nuget: Norm.net, {version}
```

