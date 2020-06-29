# Teronis.DotNet

Teronis.DotNet is a colletion of many subprojects. It has grown since I begun 2018 to summarize the code I use across many projects.

## List of Projects

All my projects are uploaded to [NuGet.org](https://www.nuget.org/packages?q=Teroneko+Teronis).

Here a list of project folders that are currently published:
- [MSBuild.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Core/Core) Some special targets and tasks. See comments in source
code.
- [MSBuild.GitVersionCache](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/GitVersionCache/GitVersionCache)
GitVersionCache is an improved implementation to GitVersionTask. It does support concurrency and 
nested GitVersion.yml relative to your .git folder.
- [MSBuild.ProjectBuildInPackage](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/MSBuild/Packaging/ProjectBuildInPackage)
A collection of utilities, extensions, tools and classes for the .NET Core implementation.
- [NetCoreApp.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Core/Core) A collection of utilities, extensions, tools and
classes for the .NET Core implementation.
- [NetCoreApp.Identity](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Identity/Identity) Teronis Identity provides you a bearer
token integration for ASP.NET Core Identity.
- [NetCoreApp.Mvc](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Mvc/Mvc) An extension to the native function range of
Microsoft.AspNetCore.Mvc.Core.
- [NetCoreApp.WinForms](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/WinForms/WinForms) A collection of utilities, extensions,
tools and classes for WinForms.
- [NetCoreApp.Wpf](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetCoreApp/Wpf/Wpf) A collection of utilities, extensions, tools and
classes for WPF.
- [NetStandard.Abstractions](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Abstractions/Abstractions) Some abstractions.
- [NetStandard.Autofac](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Autofac/Autofac) An extension to the native function
range of AutoFac.
- [NetStandard.Core](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Core/Core) A collection of utilities, extensions, tools and
classes for the .NET Standard implementation.
- [NetStandard.Drawing](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Drawing/Drawing) Some simplifications for
System.Drawing.Common.
- [NetStandard.EntityFrameworkCore](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/EntityFrameworkCore/EntityFrameworkCore)
An extension to the native function range of EntityFrameworkCore.
- [NetStandard.Json](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Json/Json) An extension to the native function range of
Newtonsoft.Json.
- [NetStandard.Tools.GitVersion](https://github.com/teroneko/Teronis.DotNet/tree/develop/src/NetStandard/Tools/GitVersion/GitVersion) A wrapper to use the
executable of GitVersion (https://chocolatey.org/packages/GitVersion.Portable) in C# programmatically.

## Running the Tests

Run specific test in Visual Studio or run all test by typing `./build.cmd test` to the console while being in the root directory.

## Contributing

You are free to send pull requests.

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details