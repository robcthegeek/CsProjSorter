using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CsQuery;
using CsQuery.Implementation;

namespace CSProjSorter
{
    public class ProjectFileSorter
    {
        private readonly string _content;
        private readonly string[] _elementsToSort =
        {
            "ITEMGROUP REFERENCE",
            "ITEMGROUP COMPILE",
            "ITEMGROUP EMBEDDEDRESOURCE"
        };

        public ProjectFileSorter(string content)
        {
            _content = content;
        }

        public string Sort()
        {
            if (string.IsNullOrWhiteSpace(_content))
                return string.Empty;

            var dom = new CQ(_content);
            var root = dom.First(x => x.NodeName == "PROJECT");

            var buffer = new Dictionary<string, IEnumerable<IDomObject>>();

            // TODO: Get (and Remove all ITEMGROUP > Reference)
            foreach (var selector in _elementsToSort)
            {
                var nodes = dom[selector].Remove();
                var sorted = SortByNodeValue(nodes); // TODO: Actually Sort Them                
                var deDuped = DeDupeByNodeValue(sorted);
                var newEl = new CQ("<ItemGroup></ItemGroup>");
                newEl.Append(deDuped);
                root.AppendChild(newEl);
            }

            // TODO: Delete any ITEMGROUP > {SORTABLE} Fields
            // TODO: Re-Insert the ITEMGROUP > {SORTABLE} AFTER the last PROPERTYGROUP

            var result = dom.Render();
            return result;
        }

        private IEnumerable<IDomObject> DeDupeByNodeValue(IEnumerable<IDomObject> sorted)
        {
            var buffer = new List<IDomObject>();

            foreach (var node in sorted)
            {
                var value = GetValueBasedOnNode(node);
                
                if (buffer.Any(x => GetValueBasedOnNode(x) == value))
                    continue;
                
                buffer.Add(node);
            }

            return buffer;
        }

        private IOrderedEnumerable<IDomObject> SortByNodeValue(IEnumerable<IDomObject> nodes)
        {
            var sorted = nodes.OrderBy(GetValueBasedOnNode);
            return sorted;
        }

        public static void SortChildren(IDomObject node)
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