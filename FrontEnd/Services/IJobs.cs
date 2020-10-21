using ClosedXML.Excel;
using FrontEnd.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Task<MemoryStream> GenerateXlsx();
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


        public async Task<MemoryStream> GenerateXlsx()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var workbook = new XLWorkbook();

                var SheetName = "Jobs List";
                var worksheet = workbook.Worksheets.Add(SheetName);

                worksheet.Cell(1, 1).Value = "Job";
                worksheet.Cell(1, 2).Value = "Job Title";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Created At";
                worksheet.Cell(1, 5).Value = "Expires At";

                var jobList = await GetJobList();
                for (int i = 0; i < jobList.Count(); i++)
                {
                    worksheet.Cell(i + 2, 1).Value = jobList.ElementAt(i).Job;
                    worksheet.Cell(i + 2, 2).Value = jobList.ElementAt(i).JobTitle;
                    worksheet.Cell(i + 2, 3).Value = jobList.ElementAt(i).Description;
                    worksheet.Cell(i + 2, 4).Value = jobList.ElementAt(i).CreatedAt;
                    worksheet.Cell(i + 2, 5).Value = jobList.ElementAt(i).ExpiresAt;
                }

                worksheet.Range(1, 1, 1, 5).Style.Font.Bold = true;
                worksheet.Range(1, 1, 1, 5).Style.Font.FontColor = XLColor.White;
                worksheet.Range(1, 1, 1, 5).Style.Fill.BackgroundColor = XLColor.FromArgb(66, 135, 245);
                worksheet.Range(1, 1, 1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                worksheet.Column(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Column(4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Column(5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 70;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 12;

                int lastRow = worksheet.LastRowUsed().RowNumber();

                worksheet.Range("A1:E" + lastRow).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:E" + lastRow).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:E" + lastRow).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:E" + lastRow).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                workbook.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
    }
}
