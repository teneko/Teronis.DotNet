[![Build Status](https://dev.azure.com/teroneko/Teronis.DotNet/_apis/build/status/NuGetPushArtifacts?branchName=develop)](https://dev.azure.com/teroneko/Teronis.DotNet/_build/latest?definitionId=5&branchName=master)
[![Custom badge](https://buildstats.info/nuget/Teronis.MSBuild.Packaging.ProjectBuildInPackage?includePreReleases=true)](https://www.nuget.org/packages?q=Teroneko+Teronis)
![Custom badge](https://img.shields.io/endpoint?url=https%3A%2F%2Fgist.githubusercontent.com%2Fteroneko%2Fa807e920ca2ee8d3e5749366d3528486%2Fraw%2F05805ebd5a26fb58cabb26a42bd6baf467822fd7%2Fpreview-badge.json)

# Teronis.DotNet

Teronis.DotNet is a collection of many subprojects. It has grown since I begun 2018 to summarize the code I use across many projects.

## List of Projects

All my projects are uploaded to [NuGet](https://www.nuget.org/packages?q=Teroneko+Teronis). You can view them here on [GitHub](https://github.com/teroneko/Teronis.DotNet/packages) too.

Here a list of the project folders:
- [Teronis.AspNetCore.Identity](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Identity/Identity) Teronis ASP.NET Core Identity provides you a better integration for ASP.NET Core Identity.
- [Teronis.AspNetCore.Identity.Abstractions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Identity/Identity.Abstractions) Teronis ASP.NET Core Identity provides you a better integration for ASP.NET Core Identity.
- [Teronis.AspNetCore.Identity.Bearer](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Identity/Bearer) Teronis ASP.NET Core Identity Bearer provides you a bearer token integration for ASP.NET Core Identity.
- [Teronis.AspNetCore.Identity.Bearer.Abstractions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Identity/Bearer.Abstractions) Teronis ASP.NET Core Identity Bearer provides you a bearer token integration for ASP.NET Core Identity.
- [Teronis.AspNetCore.Mvc](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Mvc/Mvc) An extension to the native function range of Microsoft.AspNetCore.Mvc.Core.
- [Teronis.GitVersion.CommandLine.Library](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/GitVersion/CommandLine/Library) A wrapper to use the executable of GitVersion (https://chocolatey.org/packages/GitVersion.Portable) in C# programmatically.
- [Teronis.Microsoft.AspNetCore.Authorization](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/AspNetCore/Authorization/Microsoft) Provides extension methods for Microsoft.AspNetCore.Authorization.
- [Teronis.ModuleInitializer.AssemblyLoader](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0) Provides an assembly loader injector that injects IL code representing calls to foreign assembly ModuleInitializer.Initialize() methods in an target assembly that forces these foreign assembly to load AND initialize.
- [Teronis.ModuleInitializer.AssemblyLoader.Executable](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0.Executable) Provides an assembly loader injector executable that can inject IL code representing calls to foreign assembly ModuleInitializer.Initialize() methods in an target assembly that forces these foreign assembly to load AND initialize.
- [Teronis.ModuleInitializer.AssemblyLoader.MSBuild](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0.MSBuild) Provides the MSBuild interface for using the assembly loader injector exectuable.
- [Teronis.MSBuild.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Core/Core) Some special targets and tasks. See comments in source code.
- [Teronis.MSBuild.GitVersionCache](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/GitVersionCache/0) GitVersionCache is an improved implementation to GitVersionTask. It does support concurrency and nested GitVersion.yml relative to your .git folder.
- [Teronis.MSBuild.Packaging.Pack](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/Pack) An addition to NuGet.Build.Tasks.Pack that adds more package types beside of existing ones like DotNetCli and Dependency.
      You just need to set-up PackSourceAs and PackageSourceReference and depending on scenario creating a synthetic project.
- [Teronis.MSBuild.ProjectBuildInPackage](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/ProjectBuildInPackage) Allows project reference content to be added to the NuGet-package during pack process.
- [Teronis.NetStandard.Collections](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/Collections) Provides classes that define generic collections.
- [Teronis.NetStandard.Collections.Changes](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/Changes) Provides algorithm to calculate collection differences between two collection.
- [Teronis.NetStandard.Collections.Synchronization](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/Synchronization) Provides classes for collection synchronization.
- [Teronis.NetStandard.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Core/Core) A collection of utilities, extensions and classes for the .NET Standard implementation.
- [Teronis.NetStandard.DataModeling.TreeColumn](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/DataModeling/TreeColumn) Provides TreeColumnSeeker for complex data structures.
- [Teronis.NetStandard.Drawing](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Drawing/Drawing) Some simplifications for System.Drawing.Common.
- [Teronis.NetStandard.EntityFrameworkCore.Query](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/EntityFrameworkCore/Query) Provides expression builder to increase query ability in Entity Framework Core.
- [Teronis.NetStandard.Json](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Json/Json) An extension to the native function range of Newtonsoft.Json.
- [Teronis.NetStandard.Linq.Expressions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Linq/Expressions/Expressions) Provides expression builder, expression visitor and everything around LINQ Expressions.
- [Teronis.Windows.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/Windows/Core) A collection of utilities, extensions, classes and platform invokes for the Windows platform.
- [Teronis.Windows.Forms](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/Windows/Forms) A collection of utilities, extensions and classes for Windows Forms.
- [Teronis.Windows.PresentationFoundation](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/Windows/PresentationFoundation) A collection of utilities, extensions and classes for Windows Presentation Foundation.

## Build Script

The build script `./build.cmd` is a small application to assist in restoring, building, testing and packing all projects that can be found in this repository.

```
Teronis.Build 1.0.0+9e43e3dee9c14e083f9abc72ed59918200116e21
Teroneko

  restore    Restores projects

  build      Builds projects

  pack       Packs projects

  test       Tests projects

  azure      Restores, builds, tests and packs projects

  help       Display more information on a specific command.

  version    Display version information.
```

## Contributing

I appreciate any kind of feedback, so feel free to open an issue or sending me a pull request.

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Icons

<div>Warning icon made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
