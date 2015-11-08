// ReSharper disable UnusedMemberInSuper.Global
using LocalNuget.Core.Results;

namespace LocalNuget.Core.Commands
{

    public interface ILineCommandResult<TResult>
    {

        IResultBus<TResult> ResultBus { get; set; }

    }

}
