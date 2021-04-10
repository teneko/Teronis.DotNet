﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// <auto-generated>
//   This code file has automatically been added by the "Teronis.Nullable" NuGet package (https://www.nuget.org/packages/Nullable).
//   Please see https://github.com/manuelroemer/Nullable for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The StaticAnalyseAttributes-folder and the [..]Attribute[.ExcludeFromCodeCoverage].cs-files don't appear in your project.
//   * The added files are immutable and can therefore not be modified by coincidence.
//   * Updating/Uninstalling the package will work flawlessly.
// </auto-generated>

#if !NULLABLE_ATTRIBUTES_DISABLE
#nullable enable
#pragma warning disable

namespace System.Diagnostics.CodeAnalysis
{
    using global::System;

#if DEBUG
    /// <summary>
    ///     Specifies that the output will be non-<see langword="null"/> if the
    ///     named parameter is non-<see langword="null"/>.
    /// </summary>
#endif
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
        AllowMultiple = true,
        Inherited = false
    )]
#if !NULLABLE_ATTRIBUTES_INCLUDE_IN_CODE_COVERAGE
    [DebuggerNonUserCode]
#endif
    internal sealed partial class NotNullIfNotNullAttribute : Attribute
    {
#if DEBUG
        /// <summary>
        ///     Gets the associated parameter name.
        ///     The output will be non-<see langword="null"/> if the argument to the
        ///     parameter specified is non-<see langword="null"/>.
        /// </summary>
#endif
        public string ParameterName { get; }

#if DEBUG
        /// <summary>
        ///     Initializes the attribute with the associated parameter name.
        /// </summary>
        /// <param name="parameterName">
        ///     The associated parameter name.
        ///     The output will be non-<see langword="null"/> if the argument to the
        ///     parameter specified is non-<see langword="null"/>.
        /// </param>
#endif
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}

#pragma warning restore
#nullable restore
#endif // NULLABLE_ATTRIBUTES_DISABLE
