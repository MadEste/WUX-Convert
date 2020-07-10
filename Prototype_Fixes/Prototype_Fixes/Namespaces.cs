using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Prototype_Fixes
{
    class Namespaces
    {
        internal static string[] GetValidNames()
        {
            StringReader sr = new StringReader(Resources.validNames);
            var strs = sr.ReadToEnd();
            var strArr = strs.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );
            return strArr;
        }

        internal static String[] GetInvalidNames()
        {
            StringReader sr = new StringReader(Resources.invalidNames);
            var strs = sr.ReadToEnd();
            var strArr = strs.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );
            return strArr;
        }
    } 
}
