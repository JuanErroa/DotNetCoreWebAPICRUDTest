using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

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
