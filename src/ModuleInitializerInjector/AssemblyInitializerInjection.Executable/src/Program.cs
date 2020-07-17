using System;
using System.IO;
using CommandLine;
using Teronis.NetCoreApp.AssemblyLoadInjection;
using Teronis.NetCoreApp.AssemblyLoadInjection.Executable;
using Teronis.NetCoreApp.ModuleInitializerInjector.Utils;
using static Teronis.AssemblyInitializerInjection.Utils.ExceptionUtils;

namespace Teronis.NetCoreApp.ModuleInitInjector
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<ReadFullAssemblyNameCommand, InjectAssemblyInitializerCommandOptions>(args)
                .MapResult<ReadFullAssemblyNameCommand, InjectAssemblyInitializerCommandOptions, int>(
                    fullNameOptions => {
                        try {
                            var assemblyFullName = AssemblyPathUtils.ReadAssemblyFullName(fullNameOptions.AssemblyPath);
                            Console.Out.WriteLine(assemblyFullName);
                        } catch (ArgumentNullException) {
                            Console.Error.WriteLine("The assembly path cannot be null.");
                            return 1;
                        } catch (FileNotFoundException) {
                            Console.Error.WriteLine("The assembly file was not found.");
                            return 1;
                        } catch (Exception error) {
                            Console.Error.WriteLine(GetMessageWithPrependedType(error, "An error occured while trying to read the full assembly name."));
                            return 1;
                        }

                        return 0;
                    },
                    injectOptions => {
                        var assemlyNameToBeLoaded = injectOptions.AssemlyNameToBeLoaded;
                        var assemlyNameFromPathToBeLoaded = injectOptions.AssemlyNameFromPathToBeLoaded;

                        if (assemlyNameToBeLoaded == null && assemlyNameFromPathToBeLoaded == null) {
                            Console.Error.WriteLine($"You have to set --{InjectAssemblyInitializerCommandOptions.AssemblyNameOptionLongName} " +
                                $"or --{InjectAssemblyInitializerCommandOptions.AssemblyNameFromPathOptionLongName}.");

                            return 1;
                        } else if (assemlyNameToBeLoaded != null && assemlyNameFromPathToBeLoaded != null) {
                            Console.Error.WriteLine($"You can only set --{InjectAssemblyInitializerCommandOptions.AssemblyNameOptionLongName} " +
                                $"or --{InjectAssemblyInitializerCommandOptions.AssemblyNameFromPathOptionLongName}.");

                            return 1;
                        }

                        try {
                            AssemblyInitializerInjector.Default.InjectAssemblyInitializer(injectOptions.InjectionTargetAssemblyPath,
                                injectOptions.AssemlyNameToBeLoaded);
                        } catch (ArgumentNullException) {
                            Console.Error.WriteLine("The assembly path cannot be null.");
                            return 1;
                        } catch (FileNotFoundException) {
                            Console.Error.WriteLine("The assembly file was not found.");
                            return 1;
                        } catch (AggregateException error) {
                            foreach (var innerError in error.InnerExceptions) {
                                Console.Error.WriteLine(GetMessageWithPrependedType(innerError));
                            }

                            return 1;
                        } catch (Exception error) {
                            Console.Error.WriteLine(GetMessageWithPrependedType(error, "An error occured while trying to inject the " +
                                $"full assembly name to injection target assembly."));

                            return 1;
                        }

                        return 0;
                    },
                    errors => {
                        foreach (var error in errors) {
                            Console.Error.WriteLine(error.ToString());
                        }

                        return 1;
                    });
        }
    }
}
