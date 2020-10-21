using FrontEnd.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public interface IJobs
    {
        public Task<IEnumerable<Jobs>> GetJobList();
        public Task CreateJob(Jobs job);
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

        public async Task CreateJob(Jobs newJob)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newJob), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(apiBaseUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var jobAdded = JsonConvert.DeserializeObject<Jobs>(apiResponse);
                }
            }
        }

    }
}
