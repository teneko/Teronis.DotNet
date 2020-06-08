using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;

namespace Teronis.GitVersionCache.BuildTasks.Models
{
    public class Test : Task
    {
        public override bool Execute()
        {
            Log.LogError(" # 5 3 HALLO ES FUNKTTKJDFJ");
            return true;
        }
    }
}
