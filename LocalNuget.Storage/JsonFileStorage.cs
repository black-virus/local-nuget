using System.Collections.Generic;
using System.IO;
using System.Linq;
using LocalNuget.Models;
using LocalNuget.Settings;
using Newtonsoft.Json;

namespace LocalNuget.Storage
{
    public class JsonFileStorage : IStorage
    {

        #region Fields

        private readonly FileInfo file;
        private readonly List<StoragePackage> innerStorage;

        #endregion

        #region Constructors

        public JsonFileStorage(ISettings settings)
        {
            file = new FileInfo(Path.Combine(settings.WorkDirectory, "storage.json"));
            if (file.Exists)
            {
                using (var fs = file.OpenRead())
                using (var sw = new StreamReader(fs))
                {
                    innerStorage = new List<StoragePackage>(JsonConvert.DeserializeObject<StoragePackage[]>(sw.ReadToEnd()));
                }
            }
            else
            {
                file.Create().Dispose();
                file.Refresh();
                innerStorage = new List<StoragePackage>();
            }
        }

        #endregion

        #region Methods

        public void Add(string name, string csProjFile, string nuspecFile)
        {
            if (innerStorage.Any(package => package.Name.Equals(name)))
                throw new StorageLocalNugetException(StorageLocalNugetExceptionReason.PackageAlreadyExist);
            innerStorage.Add(new StoragePackage
            {
                Name = name,
                CsProjectFile = csProjFile,
                NuspecProjectFile = nuspecFile
            });
            Store();
        }

        public StoragePackage Get(string packageName)
        {
            return innerStorage.FirstOrDefault(package => package.Name.Equals(packageName));
        }

        public IEnumerable<StoragePackage> List()
        {
            return innerStorage.AsReadOnly();
        }

        public void Remove(string name)
        {
            innerStorage.RemoveAll(package => package.Name.Equals(name));
        }

        private void Store()
        {
            using (var fs = file.OpenWrite())
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(JsonConvert.SerializeObject(innerStorage.ToArray()));
            }
        }

        #endregion

    }
}
