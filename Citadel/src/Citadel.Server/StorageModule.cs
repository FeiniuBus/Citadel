using Citadel.Data;
using Citadel.Extensions;
using Citadel.Logging;
using Citadel.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Citadel.Server
{
    public class StorageModule 
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger _logger;

        public StorageModule(IDbConnectionFactory dbConnectionFactory, ILogger<StorageModule> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public void StartUp(IServiceProvider serviceProvider)
        {
            var storages = serviceProvider.GetServices<IStorage>();
            if(storages?.Any() == true)
            {
                var connection = _dbConnectionFactory.CreateDbConnection();
                connection.Open();
                foreach (var storage in storages)
                {
                    try
                    {
                        storage.EnsureCreateAsync(connection).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(new EventLog(nameof(StorageModule), "StartUp", "EnsureCreateAsync", new { Errors = e.GetErrors() }));
                    }
                }
            }
        }
    }
}
