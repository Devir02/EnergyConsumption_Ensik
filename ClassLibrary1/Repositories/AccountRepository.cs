using System.Globalization;
using System.Data;
using EnergyConsumption.Repository.Entities;
using System.Text.RegularExpressions;
using EnergyConsumption.Repository.Interfaces;
using EnergyConsumption.Repository;
using DataAccess.EFCore.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.IO;
using Domain.Interfaces;

namespace EnergyConsumption.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>,IAccountRepository
    {
        public AccountRepository(EnergyConsumptionContext context) : base(context)
        {
        }

        

    }
}
