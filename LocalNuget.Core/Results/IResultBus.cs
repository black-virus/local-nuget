﻿namespace LocalNuget.Core.Results
{

    public interface IResultBus<in TResult>
    {
        void SetResult(TResult result);
    }

}
