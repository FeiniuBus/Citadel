namespace Citadel.Client
{
    public class HttpServiceDescriptor
    {
        public HttpServiceDescriptor(string serviceName, string host, string path, string method)
        {
            ServiceName = serviceName;
            Host = host;
            Path = path;
            Method = method;
        }

        public string ServiceName { get; }
        public string Host { get; }
        public string Path { get; }
        public string Method { get; }
    }
}
