namespace SBE.Core.Utils
{
    static class FileHelper
    {
        internal static string SourcePath { private get; set; }

        internal static string GetOutputFileName(string name, string extension, string assembly)
        {
            return $"{SourcePath}\\{assembly}.{name}.sbe.{extension}";
        }
    }
}
