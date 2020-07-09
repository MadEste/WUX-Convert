using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Prototype_Fixes
{
    public class Namespaces
    {
        public static String[] GetValidNames()
        {
            StringReader sr = new StringReader(Resources.validNames);
            var strs = sr.ReadToEnd();
            var strArr = strs.Split('\n');
            return strArr;
        }
    } 
}
