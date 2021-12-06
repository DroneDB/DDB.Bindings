using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDB.Bindings.Model
{
    public enum MergeStrategy
    {
        DontMerge = 0,
        KeepTheirs = 1,
        KeepOurs = 2
    }
}
