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
        public Task<Jobs> GetJobById(int? id);
        public Task EditJob(Jobs job);
        public Task<string> DeleteJob(int? id);
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


        public async Task<Jobs> GetJobById(int? id)
        {
            if (id == null)
                return null;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiBaseUrl + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Jobs job = JsonConvert.DeserializeObject<Jobs>(apiResponse);
                    return job;
                }
            }
        }

        public async Task EditJob(Jobs editedJob)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(editedJob), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(apiBaseUrl + editedJob.Job, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var santi = JsonConvert.DeserializeObject<Jobs>(apiResponse);
                }
            }
        }


        public async Task<string> DeleteJob(int? id)
        {
            if (id.HasValue)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(apiBaseUrl + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        return "Ok";
                    }
                }
            }
            return null;
        }

    }
}
