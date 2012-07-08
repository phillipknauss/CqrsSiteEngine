using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventing
{
    public static class TypeExtensions
    {
        public static string GetContractName(this Type self)
        {
            return self.FullName;
        }
    }
}
