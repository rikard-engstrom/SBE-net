using System;
using System.Linq;

namespace SBE.Core.Models
{
    static class TagsService
    {
        internal static string GetEpicName(string[] tags)
        {
            return tags?.FirstOrDefault(tag => tag.StartsWith("@Epic:", StringComparison.OrdinalIgnoreCase))
                    ?? string.Empty;
        }

        internal static bool IsCurrentIteration(string[] tags)
        {
            return tags?.Any(tag => tag.Equals("current")) ?? false;
        }

        internal static string SortTag(string[] tags)
        {
            return tags?.FirstOrDefault(tag => tag.StartsWith("sort:", StringComparison.OrdinalIgnoreCase));
        }
    }
}
