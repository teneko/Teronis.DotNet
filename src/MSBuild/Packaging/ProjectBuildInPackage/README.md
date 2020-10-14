# Teronis.MSBuild.Packaging.ProjectBuildInPackage

## Usage
* Install ```Teronis.MSBuild.Packaging.ProjectBuildInPackage``` nuget package, ie. ```Install-Package Teronis.MSBuild.Packaging.ProjectBuildInPackage```
* Modify the project that will be packed: add ```PrivateAssets="all"``` property into each ```ProjectReference``` item that is to be included into the nuget package
* Pack/publish, ie. using Visual Studio's "Publish" component

### Example
Before
```
  <ItemGroup>
    <ProjectReference Include="..\SomeReallyGoodProject\SomeReallyGoodProject.csproj" />
  </ItemGroup>
```
After
```
  <ItemGroup>
    <ProjectReference Include="..\SomeReallyGoodProject\SomeReallyGoodProject.csproj" PrivateAssets="all" />
  </ItemGroup>
```
