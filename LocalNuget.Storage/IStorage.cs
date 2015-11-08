using System.Collections.Generic;
using LocalNuget.Models;

namespace LocalNuget.Storage
{
    public interface IStorage
    {
        void Add(string name, string csProjFile, string nuspecFile);
        StoragePackage Get(string test);
        IEnumerable<StoragePackage> List();
        void Remove(string name);
    }
}
