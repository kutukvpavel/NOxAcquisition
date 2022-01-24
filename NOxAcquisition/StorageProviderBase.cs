using System;
using NOxAcquisition;

public abstract class StorageProviderBase : IDisposable
{
    protected RegisterSet _regs;

    public StorageProviderBase(RegisterSet regs)
    {
        _regs = regs;
    }

    public abstract void Store();

    public abstract void Dispose();
}