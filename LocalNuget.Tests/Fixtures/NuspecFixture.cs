using System;
using System.IO;
using System.Reflection;
using System.Threading;
using static System.IO.Path;

namespace LocalNuget.Tests.Fixtures
{
    public class NuspecFixture : IDisposable
    {

        private const string BaseCsProjLocation = @"C:\00_Praca\99_WorkingCopy\Local NUGET\LocalNuget.Tests\LocalNuget.Tests.csproj";
        private const string BaseCsProjLocation2 = @"C:\00_Praca\99_WorkingCopy\blackhouse\blackhouse.csproj";


        private string WorkDir { get; set; }
        public string CsProjLocation { get; private set; }
        public string CsProjLocation2 { get; private set; }

        public void AttachToWorkDirectory(string workDirectory)
        {
            WorkDir = workDirectory;
            var dir = new DirectoryInfo(Combine(workDirectory, "projects"));
            if (dir.Exists) return;
            dir.Create();
            var dirCs1 = dir.CreateSubdirectory("CS1");
            var dirCs2 = dir.CreateSubdirectory("CS2");
            var proj1File = new FileInfo(BaseCsProjLocation);
            var proj2File = new FileInfo(BaseCsProjLocation2);
            CsProjLocation = proj1File.CopyTo(Combine(dirCs1.FullName, proj1File.Name)).FullName;
            CsProjLocation2 = proj2File.CopyTo(Combine(dirCs2.FullName, proj2File.Name)).FullName;
            ClearNuspecs();
        }

        public void ClearNuspecs()
        {
            ClearNuspecs(WorkDir);
            if (!string.IsNullOrEmpty(CsProjLocation))
                ClearNuspecs(GetDirectoryName(CsProjLocation));
            if (!string.IsNullOrEmpty(CsProjLocation2))
                ClearNuspecs(GetDirectoryName(CsProjLocation2));
            ClearStorage();
        }

        private void ClearStorage()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var dir = new DirectoryInfo(WorkDir);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            var tries = 10;
            while (tries > 0)
            {
                ClearNuspecs();
                ClearStorage();
                Thread.Sleep(1000);
                tries--;
            }
        }
    }
}
