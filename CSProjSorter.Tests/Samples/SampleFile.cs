using System.IO;

namespace CSProjSorter.Tests.Samples
{
    public class SampleFile
    {
        private static string Load(string fileName)
        {
            var path = string.Format(@"Samples\{0}.csproj", fileName);

            try
            {
                var file = File.ReadAllText(path);
                return file;
            }
            catch (FileNotFoundException fileNotFound)
            {
                var message = string.Format("Unable to load sample project file '{0}' - did you mark the file as 'Copy to Output Directory'?", fileNotFound.FileName);
                throw new FileNotFoundException(message, path, fileNotFound);
            }
        }

        public static string Simple
        {
            get
            {
                return Load("Simple");
            }
        }
    }
}