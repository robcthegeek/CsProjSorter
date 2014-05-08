using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CsQuery;

namespace CSProjSorter
{
    public static class CsQueryExtensions
    {
        public static bool RequiresSort(this IDomObject node)
        {
            var name = node.NodeName;

            if (string.IsNullOrWhiteSpace(name))
                return false;

            var sortable = new[] { "ITEMGROUP" };
            var result = sortable.Contains(name);
            return result;
        }

        public static void SortChildren(this IDomObject node)
        {
            if (node.ChildNodes.Count == 0)
                return;

            var buffer = new Dictionary<string, IDomObject>();

            foreach (var child in node.ChildNodes)
            {
                var value = GetValueBasedOnNode(child);
                if (!string.IsNullOrWhiteSpace(value) && !buffer.ContainsKey(value))
                {
                    buffer.Add(value, child);
                }
            }

            node.ChildNodes.Clear();

            var sorted = buffer.OrderBy(x => x.Key);
            foreach (var kvp in sorted)
            {
                node.ChildNodes.Add(kvp.Value);
            }
        }

        private static string GetValueBasedOnNode(IDomObject node)
        {
            if (node.NodeName == "REFERENCE")
                return node.Attributes["Include"];

            if (node.NodeName == "COMPILE")
                return node.Attributes["Include"];

            if (node.NodeName == "EMBEDDEDRESOURCE")
                return node.Attributes["Include"];

            return null;
        }
    }
}