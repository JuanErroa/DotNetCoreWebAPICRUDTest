using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public JobsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Jobs
        //Returns all Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jobs>>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        // GET: api/Jobs/5
        //Returns specific Job by it's Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Jobs>> GetJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // PUT: api/Jobs/5
        //Update a Job by it's Id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobs(int id, Jobs job)
        {
            if (id != job.Job)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        //Create a New Job
        [HttpPost]
        public async Task<ActionResult<Jobs>> PostJobs(Jobs job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobs", new { id = job.Job }, job);
        }

        // DELETE: api/Jobs/5
        //Delete a Job by it's Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jobs>> DeleteJobs(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return job;
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Job == id);
        }
    }
}
