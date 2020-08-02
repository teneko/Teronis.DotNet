using System;
using System.IO;
using CommandLine;
using Teronis.ModuleInitializer.AssemblyLoader.Utils;
using Teronis.ModuleInitializer.AssemblyLoader;
using static Teronis.ModuleInitializer.AssemblyLoader.Utils.ExceptionUtils;

namespace Teronis.ModuleInitializer.AssemblyLoader
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<ReadFullAssemblyNameCommand, InjectAssemblyLoaderCommandOptions>(args)
                .MapResult<ReadFullAssemblyNameCommand, InjectAssemblyLoaderCommandOptions, int>(
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
                        try {
                            AssemblyLoaderInjector.Default.InjectAssemblyLoader(injectOptions.InjectionTargetAssemblyPath,
                                injectOptions.SourceAssemblyPathToBeLoaded);
                        } catch (ArgumentNullException error) {
                            Console.Error.WriteLine($"The assembly path cannot be null. ({error.ParamName})");
                            return 1;
                        } catch (FileNotFoundException error) {
                            Console.Error.WriteLine($"The assembly file '{error.FileName}' was not found.");
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
