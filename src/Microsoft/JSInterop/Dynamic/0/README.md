# Teronis.Microsoft.JSInterop.Dynamic [![Nuget](https://img.shields.io/nuget/v/Teronis.Microsoft.JSInterop.Dynamic)][NuGet Package]

[NuGet Package]: https://www.nuget.org/packages/Teronis.Microsoft.JSInterop.Dynamic

_Create specialized IJSObjectReference dynamic proxy facades_

[:running: Getting Started](#getting-started) &nbsp; | &nbsp; [:package: NuGet Package][NuGet Package]

## About

Teronis.Microsoft.JSInterop.Dynamic Extends the functionality of Microsoft.JSInterop and Teronis.Microsoft.JSInterop. First recommend you to read [Teronis.Microsoft.JSInterop](/src/Microsoft/JSInterop/0). After you know what activators are available I can tell you that every activator has now a dynamic proxy activator too. These activators offers you to create interface proxies.

## Installation

Package Managaer

```
Install-Package Teronis.Microsoft.JSInterop.Dynamic -Version <version>
```

.NET CLI

```
dotnet add package Teronis.Microsoft.JSInterop.Dynamic --version <version>
```

## Getting Started

Because this project serves as a kompendium of Teronis.Microsoft.JSInterop.Abstractions, Teronis.Microsoft.JSInterop.Core, Teronis.Microsoft.JSInterop.Locality.WebAssets and Teronis.Microsoft.JSInterop.Dynamic.Core it is very simple to get started.

### __Teronis.Microsoft.JSInteropFacades__

Please take a look at the available facades and their individual activator of [Teronis.Microsoft.JSInterop](/src/Microsoft/JSInterop/0/README.md#getting-started).

### IJSObjectReferenceFacade Dynamic Facade

This is the entry point of all available dynamic proxy activators listed below. It creates a dynamic proxy that at least implements IJSObjectReferenceFacade.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSDynamicProxy();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSDynamicProxyActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSDynamicFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSDynamicLocalObjectActivator`.

### IJSModule Dynamic Facade

The dynamic proxy facade implements at least IJSModule.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSDynamicModule();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSDynamicModuleActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSDynamicFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSDynamicModuleActivator`.

### IJSLocalObject Dynamic Facade

The dynamic proxy facade implements at least IJSLocalObject.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSDynamicLocalObject();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSDynamicLocalObjectActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSDynamicFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSDynamicLocalObjectActivator`.

### IJSFacadeHub<T> (not a dynamic facade)

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSDynamicFacadeHub();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSFacadeHub<JSDynamicFacadeActivators>>(); // JSDynamicFacadeActivators holds all available dynamic and non-dynamic activators.
```

## Examples

The above activators are good quick-starters. For bigger scenarios I am currently lacking of comprehensive examples so I have to forward you to some good and self-explanatory unit tests.

- Create an object (component) that holds dynamic modules and dynamic global objects that get initialized when activating `IJSFacadeHub<T>` by invoking `IJSCustomFacadeActivator.CreateInstance(object component)`:
  </br>[/src/Microsoft/JSInterop/Dynamic/0/test/0/Facade/JSDynamicFacadeHubComponentTests.cs](/src/Microsoft/JSInterop/Dynamic/0/test/0/Facade/JSDynamicFacadeHubComponentTests.cs)
- Create local dynamic proxy facades:
  </br>[/src/Microsoft/JSInterop/Dynamic/0/test/0/Locality](/src/Microsoft/JSInterop/Dynamic/0/test/0/Locality)
- Advanced dynamic proxy usage cases:
  </br>[/src/Microsoft/JSInterop/Dynamic/0/test/0/Dynamic](/src/Microsoft/JSInterop/Dynamic/0/test/0/Dynamic)