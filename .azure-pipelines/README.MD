# Local Feed Credentials
You need to set the following environment variables ([Azure Artifacts Credential Provider](https://github.com/microsoft/artifacts-credprovider#environment-variables)) for being able to access local feeds in nuget, dotnet or msbuild:
```
NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED=true
# In Azure DevOps Variables UI you can left off the single quotes
VSS_NUGET_EXTERNAL_FEED_ENDPOINTS='{"endpointCredentials":[{"endpoint":"https://pkgs.dev.azure.com/<username>/<project>/_packaging/<project_scope>/nuget/v3/index.json","username":"optional","password":"<private_access_token>"}]}'
```
You have to explicit set them in each task ([secrets](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops&tabs=yaml%2Cbatch#secret-variables)) you are in need of the credentials:
```
# You need to provide "-ApiKey <anystring>" where <anystring> is any string
- script: nuget push **\*.nupkg ... -ApiKey optional
      env:
        NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED: true
        # $(VSS_NUGET_EXTERNAL_FEED_ENDPOINTS) refers to the variable in Azure DevOps Variables UI
        VSS_NUGET_EXTERNAL_FEED_ENDPOINTS: '$(VSS_NUGET_EXTERNAL_FEED_ENDPOINTS)'
      displayName: Publish NuGet Packages
```