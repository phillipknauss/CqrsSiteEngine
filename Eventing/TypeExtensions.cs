using System;

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
