namespace Citadel.Data
{
    public class DbConnectionFactoryOptions
    {
        public string ConnectionString { get; set; }
        public string Schema { get; set; } = "public";
    }
}
