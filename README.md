# Teronis.DotNet

Teronis.DotNet is a colletion of many subprojects. It has grown since I begun 2015 to summarize
the code I use across many projects.

## List of Projects

All my projects are uploaded to [NuGet.org](https://www.nuget.org/packages?q=Teroneko).

Here a list of project source folders that are currently published:
- [MSBuild.Core](src\MSBuild\Core\Core\src) Some special targets and tasks. See comments in source
code.
- [MSBuild.GitVersionCache](src\MSBuild\Packaging\GitVersionCache\GitVersionCache\src)
GitVersionCache is an improved implementation to GitVersionTask. It does support concurrency and 
nested GitVersion.yml relative to your .git folder.
- [MSBuild.ProjectBuildInPackage](src\MSBuild\Packaging\ProjectBuildInPackage\src)
A collection of utilities, extensions, tools and classes for the .NET Core implementation.
- [NetCoreApp.Core](src\NetCoreApp\Core\Core\src) A collection of utilities, extensions, tools and
classes for the .NET Core implementation.
- [NetCoreApp.Identity](src\NetCoreApp\Identity\Identity\src) Teronis Identity provides you a bearer
token integration for ASP.NET Core Identity.
- [NetCoreApp.Mvc](src\NetCoreApp\Mvc\Mvc\src) An extension to the native function range of
Microsoft.AspNetCore.Mvc.Core.
- [NetCoreApp.WinForms](src\NetCoreApp\WinForms\WinForms\src) A collection of utilities, extensions,
tools and classes for WinForms.
- [NetCoreApp.Wpf](src\NetCoreApp\Wpf\Wpf\src) A collection of utilities, extensions, tools and
classes for WPF.
- [NetStandard.Abstractions](src\NetStandard\Abstractions\Abstractions\src) Some abstractions.
- [NetStandard.Autofac](src\NetStandard\Autofac\Autofac\src) An extension to the native function
range of AutoFac.
- [NetStandard.Core](src\NetStandard\Core\Core\src) A collection of utilities, extensions, tools and
classes for the .NET Standard implementation.
- [NetStandard.Drawing](src\NetStandard\Drawing\Drawing\src) Some simplifications for
System.Drawing.Common.
- [NetStandard.EntityFrameworkCore](src\NetStandard\EntityFrameworkCore\EntityFrameworkCore\src)
An extension to the native function range of EntityFrameworkCore.
- [NetStandard.Json](src\NetStandard\Json\Json\src) An extension to the native function range of
Newtonsoft.Json.
- [NetStandard.GitVersion](src\NetStandard\Tools\GitVersion\GitVersion\src) A wrapper to use the
executable of GitVersion (https://chocolatey.org/packages/GitVersion.Portable) in C# programmatically.

## Running the Tests

Run specific test in Visual Studio or run all test by typing `./build.cmd test` to the console while being in the root directory.

## Contributing

You are free to send pull requests.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details