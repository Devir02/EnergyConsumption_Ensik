using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyConsumption.Repository.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        IAccountRepository Accounts { get; }
        IReadingRepository Readings { get; }
        int Complete();
    }
}
