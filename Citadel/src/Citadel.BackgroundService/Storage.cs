using Citadel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Citadel.BackgroundService
{
    public class Storage : IStorage
    {
        public Task EnsureCreateAsync(IDbConnection dbConnection)
        {
            throw new NotImplementedException();
        }
    }
}
