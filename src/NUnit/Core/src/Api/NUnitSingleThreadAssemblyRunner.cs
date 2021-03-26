// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;

namespace Teronis.NUnit.Api
{
    /// <summary>
    /// <para></para>
    /// Creates a runner with following hardcoded settings:
    /// <br/>[FrameworkPackageSettings.NumberOfTestWorkers] = 0,
    /// <br/>[FrameworkPackageSettings.SynchronousEvents] = true,
    /// <br/>[FrameworkPackageSettings.RunOnMainThread] = true
    /// <para>
    /// Uses <see cref="NUnitTestAssemblyRunner"/> internally.
    /// </para>
    /// </summary>
    public class NUnitSingleThreadAssemblyRunner
    {
        private NUnitTestAssemblyRunner runner;
        private Dictionary<string, object> settings;

        public NUnitSingleThreadAssemblyRunner(IEnumerable<KeyValuePair<string, object>>? additionalSettings)
        {
            runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());

            settings = new Dictionary<string, object>() {
                // https://github.com/nunit/nunit/issues/2922
                [FrameworkPackageSettings.NumberOfTestWorkers] = 0,
                [FrameworkPackageSettings.SynchronousEvents] = true,
                [FrameworkPackageSettings.RunOnMainThread] = true
            };

            if (!(additionalSettings is null)) {
                foreach (var setting in additionalSettings) {
                    settings.Add(setting.Key, setting.Value);
                }
            }
        }

        public NUnitSingleThreadAssemblyRunner()
            : this(additionalSettings: null) { }

        public void LoadTestsInAssembly(Assembly assembly) =>
            runner.Load(assembly, settings);

        public ITestResult? RunTests(ITestListener? listener = null, ITestFilter? filter = null)
        {
            listener ??= FunctionlessTestListener.Instance;
            filter ??= FunctionlessTestFilter.Instance;
            return runner.Run(listener, filter);
        }

        private class FunctionlessTestListener : ITestListener
        {
            public static FunctionlessTestListener Instance = new FunctionlessTestListener();

            public void SendMessage(TestMessage message) { }
            public void TestOutput(TestOutput output) { }
            public void TestFinished(ITestResult result) { }
            public void TestStarted(ITest test) { }
        }

        private class FunctionlessTestFilter : ITestFilter
        {
            public static FunctionlessTestFilter Instance = new FunctionlessTestFilter();

            public bool Pass(ITest test) => true;
            public bool IsExplicitMatch(ITest test) => true;
            public TNode ToXml(bool recursive) => throw new NotImplementedException();
            public TNode AddToXml(TNode parentNode, bool recursive) => throw new NotImplementedException();
        }
    }
}
