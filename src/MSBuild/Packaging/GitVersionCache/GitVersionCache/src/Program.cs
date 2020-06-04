using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GitVersion.MSBuildTask;
using Microsoft.Extensions.DependencyInjection;
using Teronis.GitVersionCache.BuildTasks;

namespace Teronis.GitVersionCache
{

    class Program
    {
        static Program()
        {

        }

        public static void Main()
        {
            var test = ModuleInitializer.ExecutingAssemblyDirectory;
            //serviceCollection.Insert()


            var test2 = new BuildTaskExecutor(null);

            //gitVersionTask.Execute();
            ;
            //Assembly.LoadFile()


        }
    }
}
