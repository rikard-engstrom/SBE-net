using SBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SBE.Core.Services
{
    static partial class ScenarioService
    {
        sealed class TestMethodInfo
        {
            public string AssemblyName { get; internal set; }
            public KeyValuePair<string, string>[] NamedArgumets { get; internal set; }
        }

        static class ReflectionService
        {
            static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();
            static readonly Dictionary<string, string> TypeAssembly = new Dictionary<string, string>();
            static readonly Dictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();

            internal static TestMethodInfo GetTestMethodInfo(ITestOutcomeEvent e)
            {
                RegisterTypeAndAssembly(e.TestClassFullName);
                var method = Types[e.TestClassFullName].GetMethod(e.TestMethodName);
                var parmeters = method.GetParameters().Select(p => p.Name).ToArray();

                return new TestMethodInfo
                {
                    AssemblyName = Assemblies[TypeAssembly[e.TestClassFullName]].GetName().Name,
                    NamedArgumets = GetNamedArgumets(parmeters, e.TestArguments).ToArray(),
                };
            }

            private static IEnumerable<KeyValuePair<string, string>> GetNamedArgumets(string[] parameters, object[] arguments)
            {
                for (int i = 0; i < arguments?.Length; i++)
                {
                    if (parameters[i] == "exampleTags")
                    {
                        continue;
                    }

                    yield return new KeyValuePair<string, string>(parameters[i], arguments[i]?.ToString());
                }
            }

            private static void RegisterTypeAndAssembly(string classFullName)
            {
                if (TypeAssembly.ContainsKey(classFullName))
                {
                    return;
                }

                var assembly = GetAssemblyByClassName(classFullName);
                if (!Assemblies.ContainsKey(assembly.FullName))
                {
                    Assemblies.Add(assembly.FullName, assembly);
                }

                TypeAssembly.Add(classFullName, assembly.FullName);

                var type = assembly.GetType(classFullName);

                Types.Add(classFullName, type);
            }

            private static Assembly GetAssemblyByClassName(string className)
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(x => IsMatch(x, className))
                                    .OrderByDescending(x => x.GetName().Name.Length)
                                    .FirstOrDefault();
                return assembly;
            }

            private static bool IsMatch(Assembly assembly, string className)
            {
                var assemblyName = assembly.GetName();
                return className.StartsWith(assemblyName.Name)
                        || className.StartsWith(assemblyName.Name.Replace("-", "_"));
            }
        }
    }
}
