using System.Threading.Tasks;

namespace Citadel.BackgroundService
{
    public  class Bootstraper
    {
        private readonly ConsumerClient _consumerClient;

        public Bootstraper(ConsumerClient consumerClient)
        {
            _consumerClient = consumerClient;
        }

        public async Task BootstrapAsync()
        {
            await _consumerClient.InitClientAsync();
            await _consumerClient.ConsumeAsync();
        }
    }
}
