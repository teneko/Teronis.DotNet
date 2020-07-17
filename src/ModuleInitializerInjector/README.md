# Teronis.ModuleInitializerInjector[.[..]]

This projects aims the problem to auto-load a third party assembly in a customer assembly at compile time.
This happens by injecting/extending a module initializer with its content making a call to `Assembly.Load`.

## Involved Projects

### AssemblyInitializerInjection.Executable

Provides an executable that can be run to inject the assembly load related module initializer.

#### Usage

TODO

### Teronis.AssemblyInitializerInjection

Provides the core logic to inject the assembly load related module initializer.

Commonly Used Types:
<br />AssemblyInitializerInjector

## Thanks to

@Coen Goedegebure (https://www.coengoedegebure.com/module-initializers-in-dotnet/)
<br />@Adriano Carlos Verona (https://github.com/adrianoc/cecilifier)
<br />@Simon Cropp (https://github.com/Fody/ModuleInit)

for teaching me.