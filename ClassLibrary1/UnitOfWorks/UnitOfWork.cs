using DataAccess.EFCore.Repositories;
using Domain.Interfaces;
using EnergyConsumption.Data;
using EnergyConsumption.Data.Repositories;
using EnergyConsumption.Repository;
using EnergyConsumption.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EFCore.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnergyConsumptionContext _context;
        public IReadingRepository Readings { get; private set; }
        public IAccountRepository Accounts { get; private set; }
        public UnitOfWork(EnergyConsumptionContext context)
        {
            _context = context;
            Readings = new ReadingRepository(_context);
            Accounts = new AccountRepository(_context);
        }
       
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
