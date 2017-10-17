using Citadel;
using Citadel.Data;
using Citadel.Postgre;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CitadelExtension
    {
        public static void UsePostgreSQL(this CitadelOptions options, Action<DbConnectionFactoryOptions> configure)
        {
            var extension = new PostgreExtension(configure);
            options.RegisterExtension(extension);
        }
    }
}
