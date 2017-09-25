using SBE.Core.Models;

namespace SBE.Core.OutputGenerators
{
    public interface IOutputGenerator
    {
        void Generate(SbeAssembly[] assemblies);
    }
}
