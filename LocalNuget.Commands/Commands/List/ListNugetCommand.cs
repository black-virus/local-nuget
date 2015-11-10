using System.Linq;
using AutoMapper;
using LocalNuget.Models;
using LocalNuget.Core.Commands;
using LocalNuget.Core.Results;
using LocalNuget.Storage;

namespace LocalNuget.Commands.List
{
    public class ListNugetCommand : ILineCommand
    {
        // Fields
        private readonly IStorage storage;
        private readonly IResultBus<PackageInfoModel> resultBus;

        // Constructors
        public ListNugetCommand(IStorage storage, IResultBus<PackageInfoModel> resultBus)
        {
            this.storage = storage;
            this.resultBus = resultBus;
        }

        //Methods
        public void Execute()
        {
            resultBus.SetResult(storage.List().Select(Mapper.Map<StoragePackage, PackageInfoModel>).ToArray());
        }

    }

}
