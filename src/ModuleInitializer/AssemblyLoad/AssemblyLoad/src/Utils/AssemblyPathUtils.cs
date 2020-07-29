using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Pdb;

namespace Teronis.NetCoreApp.ModuleInitializerInjector.Utils
{
    public static class AssemblyPathUtils
    {
        /// <summary>
        /// Tries to gets the PDB file path that belongs to the assembly file path.
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assembly file.</param>
        /// <returns>The path of the pdb-file if it exists, otherwise null.</returns>
        public static string? GetPdbFilePathOrDefault(string assemblyFilePath)
        {
            if (assemblyFilePath == null) {
                throw new ArgumentNullException(nameof(assemblyFilePath));
            }

            var path = Path.ChangeExtension(assemblyFilePath, ".pdb");

            return File.Exists(path)
                ? path
                : null;
        }

        public static AssemblyDefinition ReadAssemblyFromPath(string assemblyPath, bool readSymbols)
        {
            assemblyPath = assemblyPath ?? throw new ArgumentNullException(nameof(assemblyPath));
            var readerParameters = new ReaderParameters(ReadingMode.Immediate);

            if (readSymbols) {
                readerParameters.ReadSymbols = true;
                readerParameters.SymbolReaderProvider = new PdbReaderProvider();
            }

            return AssemblyDefinition.ReadAssembly(assemblyPath);
        }

        public static string ReadAssemblyFullName(string assemblyPath)
        {
            using var assemblyDefinition = ReadAssemblyFromPath(assemblyPath, false);
            return assemblyDefinition.FullName;
        }
    }
}
