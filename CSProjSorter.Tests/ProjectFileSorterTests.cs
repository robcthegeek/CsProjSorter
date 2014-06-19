using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSProjSorter.Tests.Samples;
using Xunit;

namespace CSProjSorter.Tests
{
    public class ProjectFileSorterTests
    {
        [Fact]
        public void Sort_EmptyString_ReturnsEmptyString()
        {
            var sorter = new ProjectFileSorter(string.Empty);
            var result = sorter.Sort();
            Assert.Equal(string.Empty, result);
        }

        [Fact(Skip = "Need to get 'Remove' working")]
        public void Sort_SimpleProject_SortsReferences()
        {
            var source = SampleFile.Simple;
            var sorter = new ProjectFileSorter(source);
            var result = sorter.Sort();
            var idxA = result.IndexOf("ReferenceA");
            var idxB = result.IndexOf("ReferenceB");
            var idxC = result.IndexOf("ReferenceC");

            Assert.True(idxC > idxB, "Reference C doesn't seem to be after Reference B.");
            Assert.True(idxB > idxA, "Reference B doesn't seem to be after Reference A.");
        }

        [Fact(Skip = "Need to get 'Remove' working")]
        public void Sort_SimpleProject_SortsCompiles()
        {
            var source = SampleFile.Simple;
            var sorter = new ProjectFileSorter(source);
            var result = sorter.Sort();
            var idxA = result.IndexOf("CompileA");
            var idxB = result.IndexOf("CompileB");
            var idxC = result.IndexOf("CompileC");

            Assert.True(idxC > idxB, "Compile C doesn't seem to be after Compile B.");
            Assert.True(idxB > idxA, "Compile B doesn't seem to be after Compile A.");
        }
        
        [Fact(Skip = "Need to get 'Remove' working")]
        public void Sort_SimpleProject_SortsEmbeddedResources()
        {
            var source = SampleFile.Simple;
            var sorter = new ProjectFileSorter(source);
            var result = sorter.Sort();
            var idxA = result.IndexOf("EmbeddedResourceA");
            var idxB = result.IndexOf("EmbeddedResourceB");
            var idxC = result.IndexOf("EmbeddedResourceC");

            Assert.True(idxC > idxB, "EmbeddedResource C doesn't seem to be after EmbeddedResource B.");
            Assert.True(idxB > idxA, "EmbeddedResource B doesn't seem to be after EmbeddedResource A.");
        }
        
        [Fact(Skip = "Need to get 'Remove' working")]
        public void Sort_ProjectWithDuplicates_RemovesDuplicates()
        {
            var source = SampleFile.Duplicates;
            var sorter = new ProjectFileSorter(source);
            var result = sorter.Sort();
            var refB = Regex.Matches(result, "ReferenceB").Count;
            var comB = Regex.Matches(result, "CompileB").Count;

            Assert.Equal(1, refB);
            Assert.Equal(1, comB);
        }
        
        [Fact]
        public void Sort_ProjectWithMultipleReferenceItemGroups_CollapsesToOne()
        {
            var source = SampleFile.MultipleItemGroups;
            var sorter = new ProjectFileSorter(source);
            var result = sorter.Sort();
            var is2ndRef = Regex.IsMatch(result, @"<Reference[\w\W\s\b]+<\/ItemGroup>[\w\W\s\b]+<Reference", RegexOptions.IgnoreCase);
            Assert.False(is2ndRef);
        }
    }
}