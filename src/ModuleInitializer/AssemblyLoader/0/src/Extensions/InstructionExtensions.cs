// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Teronis.ModuleInitializer.AssemblyLoader.Extensions
{
    public static class InstructionExtensions
    {
        public static void Replace(this Collection<Instruction> collection, Instruction replacingInstruction, IEnumerable<Instruction> insertingInstructions)
        {
            var instructionIndex = collection.IndexOf(replacingInstruction);
            collection.RemoveAt(instructionIndex);

            foreach (var instruction in insertingInstructions) {
                collection.Insert(instructionIndex, instruction);
                instructionIndex++;
            }
        }
    }
}
