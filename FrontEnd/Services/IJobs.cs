using FrontEnd.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public interface IJobs
    {
        public Task<IEnumerable<Jobs>> GetJobList();
    }

    public class JobsServices : IJobs
    {
        private String apiBaseUrl;

        public JobsServices(IConfiguration configuration)
        {
            apiBaseUrl = configuration.GetValue<string>("WebAPIBaseUrl");
        }

        public async Task<IEnumerable<Jobs>> GetJobList()
        {
            IEnumerable<Jobs> jobsList = new List<Jobs>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiBaseUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    jobsList = JsonConvert.DeserializeObject<IEnumerable<Jobs>>(apiResponse);
                }
            }

            return jobsList;
        }

    }
}
