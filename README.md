# Teronis.DotNet

Teronis.DotNet is a collection of many subprojects. It has grown since I begun 2018 to summarize the code I use across many projects.

## List of Projects

All my projects are uploaded to [NuGet](https://www.nuget.org/packages?q=Teroneko+Teronis). You can view them here on [GitHub](https://github.com/teroneko/Teronis.DotNet/packages) too.

Here a list of the project folders:
- [ModuleInitializer.AssemblyLoader](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0) Provides the assembly loader injector.
- [ModuleInitializer.AssemblyLoader.Executable](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0.Executable) Provides a tool that injects IL code representing calls to foreign assembly ModuleInitializer.Initialize() methods in an target assembly that forces these foreign assembly to load AND initialize.
- [ModuleInitializer.AssemblyLoader.MSBuild](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/ModuleInitializer/AssemblyLoader/0.MSBuild) Provides the MSBuild interface for using the assembly loader injector exectuable.
- [MSBuild.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Core/Core) Some special targets and tasks. See comments in source code.
- [MSBuild.GitVersionCache](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/GitVersionCache/0) GitVersionCache is an improved implementation to GitVersionTask. It does support concurrency and nested GitVersion.yml relative to your .git folder.
- [MSBuild.Packaging.Pack](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/Pack) An additional to NuGet.Build.Tasks.Pack that adds more package types beside of existing ones like DotNetCli and Dependency.
You just need to set-up PackSourceAs and PackageSourceReference and depending on scenario creating a synthetic project.
- [MSBuild.ProjectBuildInPackage](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/ProjectBuildInPackage) A collection of utilities, extensions, tools and classes for the .NET Core implementation.
- [NetCoreApp.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Core/Core) A collection of utilities, extensions, tools and classes for the .NET Core implementation.
- [NetCoreApp.Identity](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Identity/Identity) Teronis Identity provides you a better integration for ASP.NET Core Identity.
- [NetCoreApp.Identity.Bearer](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Identity/Bearer) Teronis Identity Bearer provides you a bearer token integration for ASP.NET Core Identity.
- [NetCoreApp.Identity.EntityFrameworkCore](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Identity/EntityFrameworkCore) Teronis Identity integration that uses ASP.NET Core Identity.
- [NetCoreApp.Mvc](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Mvc/Mvc) An extension to the native function range of Microsoft.AspNetCore.Mvc.Core.
- [NetCoreApp.WinForms](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/WinForms/WinForms) A collection of utilities, extensions, tools and classes for WinForms.
- [NetCoreApp.Wpf](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Wpf/Wpf) A collection of utilities, extensions, tools and classes for WPF.
- [NetStandard.Abstractions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Abstractions/Abstractions) Some abstractions.
- [NetStandard.Autofac](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Autofac/Autofac) An extension to the native function range of AutoFac.
- [NetStandard.Collections](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/Collections) Provides classes that define generic collections.
- [NetStandard.Collections.CollectionChanging](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/CollectionChanging) Provides algorithm to calculate collection differences between two collection.
- [NetStandard.Collections.Synchronization](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Collections/Synchronization) Provides classes for collection synchronization.
- [NetStandard.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Core/Core) A collection of utilities, extensions, tools and classes for the .NET Standard implementation.
- [NetStandard.DataModel.TreeColumn](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/DataModel/TreeColumn) Provides TreeColumnSeeker for complex data structures.
- [NetStandard.Drawing](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Drawing/Drawing) Some simplifications for System.Drawing.Common.
- [NetStandard.EntityFrameworkCore](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/EntityFrameworkCore/EntityFrameworkCore) An extension to the native function range of EntityFrameworkCore.
- [NetStandard.EntityFrameworkCore.Query](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/EntityFrameworkCore/Query) Provides expression builder to increase query ability in Entity Framework Core.
- [NetStandard.Json](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Json/Json) An extension to the native function range of Newtonsoft.Json.
- [NetStandard.Linq.Expressions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Linq/Expressions/Expressions) Provides expression builder, expression visitor and everything around LINQ Expressions.
- [NetStandard.Tools.GitVersion](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Tools/GitVersion/GitVersion) A wrapper to use the executable of GitVersion (https://chocolatey.org/packages/GitVersion.Portable) in C# programmatically.

## Running the Tests

Run specific test in Visual Studio or run all test by typing `./build.cmd test` to the console while being in the root directory.

## Contributing

You are free to send pull requests.

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details