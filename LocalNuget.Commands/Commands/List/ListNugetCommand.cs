using System.IO;
using System.Linq;
using AutoMapper;
using LocalNuget.Models;
using LocalNuget.Core.Commands;
using LocalNuget.Core.Results;
using LocalNuget.Settings;
using LocalNuget.Storage;

namespace LocalNuget.Commands.List
{
    public class ListNugetCommand : ILineCommand
    {
        // Fields
        private readonly IStorage storage;
        private readonly IResultBus<PackageInfoModel> resultBus;
        private readonly ISettings settings;

        // Constructors
        public ListNugetCommand(IStorage storage, IResultBus<PackageInfoModel> resultBus, ISettings settings)
        {
            this.storage = storage;
            this.resultBus = resultBus;
            this.settings = settings;
        }

        //Methods
        public void Execute()
        {
            var result = storage.List().Select(Convert).ToArray();
            resultBus.SetResult(result);
        }

        private PackageInfoModel Convert(StoragePackage package)
        {
            var model = Mapper.Map<StoragePackage, PackageInfoModel>(package);
            var nuspecProjectFile = new FileInfo(package.NuspecProjectFile);
            var nuspecWorkFile = new FileInfo(Path.Combine(settings.WorkDirectory, nuspecProjectFile.Name));
            if (!nuspecProjectFile.Exists)
                throw CommandException.NuspecInProjectExistException(nuspecProjectFile.FullName);
            if (!nuspecWorkFile.Exists)
                throw CommandException.NuspecInWorkExistException(nuspecWorkFile.FullName);
            model.NuspecInProject = nuspecProjectFile.FullName;
            model.NuspecInWork = nuspecWorkFile.FullName;
            model.Created = nuspecWorkFile.CreationTime;
            if (nuspecProjectFile.CreationTime < model.Created) model.Created = nuspecProjectFile.CreationTime;
            model.Updated = nuspecWorkFile.LastWriteTime;
            if (nuspecProjectFile.LastWriteTime > model.Updated) model.Updated = nuspecProjectFile.LastWriteTime;
            return model;
        }

    }

}
