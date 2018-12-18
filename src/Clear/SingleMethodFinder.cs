using System;
using System.Linq;
using System.Reflection;

namespace Clear
{
    public class SingleMethodFinder
    {
        private readonly Type _type;

        public SingleMethodFinder(Type type)
        {
            _type = type;
        }

        public bool TryFindMethodToExecute(string command, out MethodInfo methodToExecute)
        {
            var methods = _type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            methodToExecute = methods.Where(m => string.Equals(m.Name, command, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return methodToExecute != null;
        }
    }
}
