using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Citadel.BackgroundService;

namespace BackgroundService.Sample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly BackgroundClient _backgroundClient;
        public ValuesController(BackgroundClient backgroundClient)
        {
            _backgroundClient = backgroundClient;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            string text = DateTime.Now.ToString();
            var cls = new BackgroundServiceTestLib.Class1();
            await _backgroundClient.Enqueue(JobInfo.CreateDelayJob(() => cls.Run(text) , TimeSpan.FromMinutes(1)));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
