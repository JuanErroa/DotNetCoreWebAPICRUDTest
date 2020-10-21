using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FrontEnd.Controllers
{
    public class JobsController : Controller
    {
        private IJobs jobsService;
        public JobsController(IJobs _jobsService)
        {
            jobsService = _jobsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetJobList(DTParameters parameters)
        {
            var data = await jobsService.GetJobList();

            int totalRows = data.Count();
            if (!string.IsNullOrEmpty(parameters.Search.Value)) //filter
            {
                data = data.Where(e => e.JobTitle.ToString().ToLower().Contains(parameters.Search.Value.ToLower()) ||
                     e.Description.ToString().ToLower().Contains(parameters.Search.Value.ToLower())
                     ).ToList();
            }
            int totalRowsFiltered = data.Count();

            string sort = UppercaseFirst(parameters.SortOrder); //sorting
            if (sort != null)
            {
                try
                {
                    data = data.AsQueryable().OrderBy(sort).ToList();
                }
                catch (System.Linq.Dynamic.Core.Exceptions.ParseException) { }
            }

            data = data.Skip(parameters.Start).Take(parameters.Length).ToList(); //Paging

            return Json(new { data = data, draw = parameters.Draw, recordsTotal = totalRows, recordsFiltered = totalRowsFiltered });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Jobs newJob)
        {
            if (!ModelState.IsValid)
            {
                return View(newJob);
            }

            await jobsService.CreateJob(newJob);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            Jobs job = await jobsService.GetJobById(Id);
            if (job == null)
                return RedirectToAction(nameof(Index));

            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Jobs editedJob)
        {
            if (!ModelState.IsValid)
            {
                return View(editedJob);
            }

            await jobsService.EditJob(editedJob);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<string> RemoveJob(int? id)
        {
            if (!id.HasValue)
                return null;

            return JsonConvert.SerializeObject(await jobsService.DeleteJob(id));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateJobsXlsx(int? id)
        {
            var stream = await jobsService.GenerateXlsx();
            return this.File(
                    fileContents: stream.ToArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "Jobs.xlsx"
                );
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
