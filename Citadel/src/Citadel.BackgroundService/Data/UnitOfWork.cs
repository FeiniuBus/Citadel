using Citadel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Citadel.Abstracts;
using Citadel.Data;

namespace Citadel.BackgroundService.Data
{
    public class UnitOfWork : BaseUnitOfWork
    {
        public UnitOfWork(WorkUnitOptions options, IDbConnectionFactory dbConnectionFactory) : base(options, dbConnectionFactory)
        {
        }

    }
}
