[![Build Status](https://dev.azure.com/teroneko/Teronis.DotNet/_apis/build/status/NuGetPushArtifacts?branchName=develop)](https://dev.azure.com/teroneko/Teronis.DotNet/_build/latest?definitionId=5&branchName=master)
[![Custom badge](https://buildstats.info/nuget/Teronis.MSBuild.Packaging.ProjectBuildInPackage?includePreReleases=true)](https://www.nuget.org/packages?q=Teroneko+Teronis)
![Custom badge](https://img.shields.io/endpoint?url=https%3A%2F%2Fgist.githubusercontent.com%2Fteroneko%2Fa807e920ca2ee8d3e5749366d3528486%2Fraw%2F05805ebd5a26fb58cabb26a42bd6baf467822fd7%2Fpreview-badge.json)

# Teronis.DotNet

Teronis.DotNet consists of many projects. These are projects to solve common problems you may appear in daily programming.

## Resources

:package: [NuGet Packages](https://www.nuget.org/packages?q=Teronis)
<br />:briefcase: Project Introductions

- [Teronis.Nullable](/src/Nullable) &nbsp; | &nbsp; :package: [NuGet Package](https://www.nuget.org/packages/Teronis.Nullable)
  <br/>_Use .NET Core 3.0's new nullable attributes in older target frameworks._
- [Teronis.MSBuild.Packaging.ProjectBuildInPackage](/src/MSBuild/Packaging/ProjectBuildInPackage) &nbsp; | &nbsp; :package: [NuGet Package](https://www.nuget.org/packages/Teronis.MSBuild.Packaging.ProjectBuildInPackage)
  <br/>_Allow project reference content to be added to the NuGet-package during pack process._
- [Teronis.AspNetCore.Components.NUnit](src/AspNetCore/Components/NUnit/0) &nbsp; | &nbsp; :package: [NuGet Package](https://www.nuget.org/packages/Teronis.AspNetCore.Components.NUnit)
  <br/>_Create and execute NUnit test cases in Blazor programmatically and display NUnit XML report._
- [Teronis.Microsoft.JSInterop](src/Microsoft/JSInterop/0) &nbsp; | &nbsp; :package: [NuGet Package](https://www.nuget.org/packages/Teronis.Microsoft.JSInterop)
  <br/>_Create specialized IJSObjectReference facades_
- [Teronis.Microsoft.JSInterop.Dynamic](src/Microsoft/JSInterop/Dynamic/0) &nbsp; | &nbsp; :package: [NuGet Package](https://www.nuget.org/packages/Teronis.Microsoft.JSInterop.Dynamic)
  <br/>_Create specialized IJSObjectReference dynamic proxy facades_
- ... (TBD)
  
:book: [Source Documentation](https://teneko.de/docs/Teronis.DotNet/Microsoft.Extensions.DependencyInjection.html)
<br/>:1234: [Repository Structure](/docs/ProjectStructure.md)

## Building

The build script `./build.(cmd|ps1|sh)` is a small application to assist in restoring, compiling, testing and packing projects that can be found in this repository.

## Contributing

Feel free to open an issue if you encounter any problems. Pull requests should be only applied if they are well described. I will do my best to answer as fast as I can. :slightly_smiling_face:

## Versioning

I use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/teroneko/Teronis.DotNet/tags).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Icons

<div>Warning icon made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
