using System.Linq;
using AutoMapper;
using LocalNuget.Models;
using LocalNuget.Core.Commands;
using LocalNuget.Core.Results;
using LocalNuget.Storage;

namespace LocalNuget.Commands.List
{
    public class ListNugetCommand : ILineCommand, ILineCommandResult<PackageInfoModel[]>
    {
        // Fields
        private readonly IStorage storage;

        // Properties
        public IResultBus<PackageInfoModel[]> ResultBus { get; set; }

        // Constructors
        public ListNugetCommand(IStorage storage)
        {
            this.storage = storage;
        }

        //Methods
        public void Execute()
        {
            ResultBus.SetResult(storage.List().Select(Mapper.Map<StoragePackage, PackageInfoModel>).ToArray());
        }

    }

}
