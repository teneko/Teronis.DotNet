# What to consider?

The source should import conditionally something like `Sdk.props`.
You may now in position to conditionally disable able it for
example with `Condition="'$(DisableSdkProps)' != 'true'"`.
The `Sdk.props` should contains something like:

```
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Sdk Name="Microsoft.NET.Sdk" />
</Project>
```