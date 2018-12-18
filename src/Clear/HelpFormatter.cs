using System;
using System.Reflection;
using System.Text;

namespace Clear
{
    public class HelpFormatter
    {
        public string GetCommands(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var output = new StringBuilder();

            foreach (var method in methods)
            {
                output.Append(method.Name + " (");
                var ps = method.GetParameters();

                for (int i = 0; i < ps.Length; i++)
                {
                    output.Append(ps[i].ParameterType.Name);
                    output.Append(" ");
                    output.Append(ps[i].Name);

                    if (i < (ps.Length - 1))
                    {
                        output.Append(", ");
                    }
                }
                output.AppendLine(")");

                //var da = m.GetCustomAttribute<DisplayAttribute>();
                //if (da != null)
                //{
                //    Console.WriteLine($"\t\t{da.GetShortName()} - {da.GetName()} - {da.GetDescription()} - {da.GetPrompt()}");
                //}
            }

            return output.ToString();
        }
    }
}
