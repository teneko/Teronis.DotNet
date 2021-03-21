[![Build Status](https://dev.azure.com/teroneko/Teronis.DotNet/_apis/build/status/NuGetPushArtifacts?branchName=develop)](https://dev.azure.com/teroneko/Teronis.DotNet/_build/latest?definitionId=5&branchName=master)
[![Custom badge](https://buildstats.info/nuget/Teronis.MSBuild.Packaging.ProjectBuildInPackage?includePreReleases=true)](https://www.nuget.org/packages?q=Teroneko+Teronis)
![Custom badge](https://img.shields.io/endpoint?url=https%3A%2F%2Fgist.githubusercontent.com%2Fteroneko%2Fa807e920ca2ee8d3e5749366d3528486%2Fraw%2F05805ebd5a26fb58cabb26a42bd6baf467822fd7%2Fpreview-badge.json)

# About

Teronis.DotNet is a collection of some of my projects I've come up since 2018.

## Packages

All my projects are uploaded to [NuGet](https://www.nuget.org/packages?q=Teronis).

# Documentation

You can view the documentation of the projects at [https://teroneko.de/docs/Teronis.DotNet/](https://teroneko.de/docs/Teronis.DotNet/Microsoft.Extensions.DependencyInjection.html)

## Build Script

The build script `./build.cmd`|`./build.ps1`|`./build.sh` is a small application to assist in restoring, compiling, testing and packing all projects that can be found in this repository.

```
$ ./build.cmd --help
PowerShell Desktop version 5.1.18362.1171
Microsoft (R) .NET Core SDK version 5.0.103

███╗   ██╗██╗   ██╗██╗  ██╗███████╗
████╗  ██║██║   ██║██║ ██╔╝██╔════╝
██╔██╗ ██║██║   ██║█████╔╝ █████╗
██║╚██╗██║██║   ██║██╔═██╗ ██╔══╝
██║ ╚████║╚██████╔╝██║  ██╗███████╗
╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝

NUKE Execution Engine version 5.0.2 (Windows,.NETCoreApp,Version=v2.1)

Targets (with their direct dependencies):

  Clean
  Restore
  Compile (default)    -> Restore
  Test                 -> Compile
  Pack                 -> Compile

Parameters:

  --configuration      Configuration to build - Default is 'Debug' (local) or
                       'Release' (server).

  --continue           Indicates to continue a previously failed build attempt.
  --help               Shows the help text for this build assembly.
  --host               Host for execution. Default is 'automatic'.
  --target             List of targets to be executed. Default is 'Compile'.
  --no-logo            Disables displaying the NUKE logo.
  --plan               Shows the execution plan (HTML).
  --root               Root directory during build execution.
  --skip               List of targets to be skipped. Empty list skips all
                       dependencies.
  --verbosity          Logging verbosity during build execution. Default is
                       'Normal'.
```
*The build project has been created with [Nuke](https://github.com/nuke-build/nuke). Please visit and stare their awesome project. <3

## Contributing

Feel free to open an issue if you encounter any problems. Pull requests should be only applied if they are well described. I will do my best to answer as fast as I can. :)

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Icons

<div>Warning icon made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
