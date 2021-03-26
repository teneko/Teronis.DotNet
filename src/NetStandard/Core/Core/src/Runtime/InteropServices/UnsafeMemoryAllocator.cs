// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Teronis.Runtime.InteropServices
{
    public static class UnsafeMemoryAllocator
    {
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int c, int count);

        public unsafe static void* New<T>(int elementCount) where T : struct =>
            Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) * elementCount).ToPointer();

        public unsafe static void* NewAndInit<T>(int elementCount) where T : struct
        {
            var sizeInBytes = Marshal.SizeOf(typeof(T)) * elementCount;
            void* newArrayPtr = Marshal.AllocHGlobal(sizeInBytes).ToPointer();
            MemSet(new IntPtr(newArrayPtr), 0, sizeInBytes);
            return newArrayPtr;
        }

        public unsafe static void Free(void* unmanagedPointer) =>
            Marshal.FreeHGlobal(new IntPtr(unmanagedPointer));

        public unsafe static void* Resize<T>(void* oldPointer, int newElementCount) where T : struct =>
            Marshal.ReAllocHGlobal(
                new IntPtr(oldPointer),
                new IntPtr(Marshal.SizeOf(typeof(T)) * newElementCount)).ToPointer();
    };
}
