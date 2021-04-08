# Teronis.Microsoft.JSInterop [![Nuget](https://img.shields.io/nuget/v/Teronis.Microsoft.JSInterop)][NuGet Package]

[NuGet Package]: https://www.nuget.org/packages/Teronis.Microsoft.JSInterop

_Create specialized IJSObjectReference facades_

[:running: Getting Started](#getting-started) &nbsp; | &nbsp; [:package: NuGet Package][NuGet Package]

## About

Teronis.Microsoft.JSInterop extends the functionality of Microsoft.JSInterop. It does so by introducing a base interface called `IJSObjectReferenceFacade` that intends to wrap an instance of `IJSObjectReference`. It's purpose is to provide the possiblity to hook any call to `InvokeAsync` or `InvokeVoidAsync` of the instance of `IJSObjectReference`. So simply said, any call is forwarded to an interceptor that is also part of `IJSObjectReferenceFacade`. Upon the interface `IJSObjectReferenceFacade` specialized `IJSObjectReference` facades has been crafted. Later on this interface with the intend to hook a call paves the way to create dynamic proxies.

## Installation

Package Managaer

```
Install-Package Teronis.Microsoft.JSInterop -Version <version>
```

.NET CLI

```
dotnet add package Teronis.Microsoft.JSInterop --version <version>
```

## Getting Started

Because this project serves as a kompendium of Teronis.Microsoft.JSInterop.Abstractions, Teronis.Microsoft.JSInterop.Core and Teronis.Microsoft.JSInterop.Locality.WebAssets it is very simple to get started.

### IJSModule Facade

This facade represents a module. It is equivalent to `IJSRuntime.InvokeAsync<IJSObjectReference>("import", <name or path>)`.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSModule();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSModuleActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSModuleActivator`.

### IJSLocalObject Facade

This facade represents a local or global object. That means nothing different than it can be a nested object of an object or a global non-pathed object like `window`. Because there is no in-built possibility to get an object of an object, the project Teronis.Microsoft.JSInterop.Locality.WebAssets provides the interop to handle this.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSLocalObject();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSLocalObjectActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSLocalObjectActivator`.

### IJSCustomFacade Facade

Like its name tells it offers you to wrap a facade from above into a custom written facade.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddCustomFacade();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSCustomFacadeActivator>();
```

> :warning: Currently there are default interceptors and value assigners added that requires you to call `services.AddJSFacadeHub`. When you are not doing it you are running into an exception of missing services when these interceptor and value assigners are created on the fly when you firstly request the `IJSCustomFacadeActivator`.

### IJSFacadeHub<T> (not a facade)

Like its name tells it serves as HUB of all available facade activators. It also opens you to initialize the properties and fields of an object. The hub serves then as container of the holded disposables.

```
// Register Services
public void ConfigureServices(IServiceCollection services) =>
    services.AddJSFacadeHub();

// Get Started
public object ProvideService(IServiceProvider serviceProvider) =>
    serviceProvider.GetRequiredService<IJSFacadeHub<JSFacadeActivators>>(); // JSFacadeActivators holds all available activators.
```

## Examples

The above activators are good quick-starters. For bigger scenarios I am currently lacking of comprehensive examples so I have to forward you to some good and self-explanatory unit tests.

- Create an object (component) that holds modules and global objects that get initialized when activating `IJSFacadeHub<T>` by invoking `IJSCustomFacadeActivator.CreateInstance`:
  [/src/Microsoft/JSInterop/0/test/0/Facade/JSFacadeHubComponentTests.cs](/src/Microsoft/JSInterop/0/test/0/Facade/JSFacadeHubComponentTests.cs)