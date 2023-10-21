using AdventOfCode.Core.Components;
using System.Reflection;

namespace AdventOfCode.Core.Extensions
{
    /// <summary>
    /// Provides extensions for assembly type.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Perform an assembly scan to get all defined <see cref="IDay"/>
        /// within an assembly.
        /// </summary>
        /// <param name="assembly">Assembly to perform the scan on</param>
        /// <returns>An enumeration of all <see cref="IDay"/> implementations.</returns>
        public static IEnumerable<Type> GetDayImplementations(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsClass
                && !t.IsAbstract
                && t.IsAssignableTo(typeof(IDay)));
        }
    }
}
