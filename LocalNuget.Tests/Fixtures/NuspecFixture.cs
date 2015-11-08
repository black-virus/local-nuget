using System.IO;
using System.Reflection;
using static System.IO.Path;

namespace LocalNuget.Tests.Fixtures
{
    public class NuspecFixture
    {

        public static string CsProjLocation { get; } = @"C:\00_Praca\99_WorkingCopy\Local NUGET\LocalNuget.Tests\LocalNuget.Tests.csproj";
        public static string CsProjLocation2 { get; } = @"C:\00_Praca\99_WorkingCopy\blackhouse\blackhouse.csproj";

        public NuspecFixture()
        {
            ClearNuspecs();
        }

        public void ClearNuspecs()
        {
            ClearNuspecs(GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            ClearNuspecs(GetDirectoryName(CsProjLocation));
            ClearNuspecs(GetDirectoryName(CsProjLocation2));
            ClearStorage();
        }

        private void ClearStorage()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var dir = new DirectoryInfo(GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var files = dir.GetFiles("storage.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                file.Delete();
            }
        }

        private void ClearNuspecs(string workDirectory)
        {
            var dir = new DirectoryInfo(workDirectory);
            foreach (var nuspecFile in dir.GetFiles("*.nuspec", SearchOption.AllDirectories))
            {
                nuspecFile.Delete();
            }
        }

    }
}
