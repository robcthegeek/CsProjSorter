using System;
using System.Linq;
using CsQuery;

namespace CSProjSorter
{
    public class ProjectFileSorter
    {
        private readonly string _content;

        public ProjectFileSorter(string content)
        {
            _content = content;
        }

        public string Sort()
        {
            if (string.IsNullOrWhiteSpace(_content))
                return string.Empty;

            var dom = new CQ(_content);
            var root = dom.Select("Project");

            foreach (var element in root.Children())
            {
                if (element.RequiresSort())
                {
                    element.SortChildren();
                }
                
            }

            return dom.Render();
        }
    }
}