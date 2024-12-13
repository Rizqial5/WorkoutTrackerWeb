using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulePlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchedulePlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SchedulePlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulePlans>>> GetSchedulePlans()
        {
            return await _context.SchedulePlans.ToListAsync();
        }

        // GET: api/SchedulePlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchedulePlans>> GetSchedulePlans(int id)
        {
            var schedulePlans = await _context.SchedulePlans.FindAsync(id);

            if (schedulePlans == null)
            {
                return NotFound();
            }

            return schedulePlans;
        }

        // PUT: api/SchedulePlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedulePlans(int id, SchedulePlans schedulePlans)
        {
            if (id != schedulePlans.Id)
            {
                return BadRequest();
            }

            _context.Entry(schedulePlans).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchedulePlansExists(id))
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

        // POST: api/SchedulePlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SchedulePlans>> PostSchedulePlans(SchedulePlansRequest request)
        {

            var workoutPlans = await _context.WorkoutPlans.FindAsync(request.WorkoutPlansId);

            if (workoutPlans == null) return NotFound("Workout Plan not Found");

            var schedulePlans = new SchedulePlans
            {
                ScheduleTime = request.DateTime,
                WorkoutPlansId = request.WorkoutPlansId
            };
            

            _context.SchedulePlans.Add(schedulePlans);

            await _context.SaveChangesAsync();

            var response = new SchedulePlansResponse
            {
                Id = schedulePlans.Id,
                PlannedDateTime = schedulePlans.ScheduleTime,
                WorkoutPlanResponse = new WorkoutPlanResponse
                {
                    PlanId = schedulePlans.WorkoutPlansId,
                    PlanName = schedulePlans.WorkoutPlan.PlanName,
                    ExerciseSets = null,
                    ScheduledTime = null
                    
                }
            };

            return CreatedAtAction("GetSchedulePlans", new { id = schedulePlans.Id }, response);
        }

        // DELETE: api/SchedulePlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedulePlans(int id)
        {
            var schedulePlans = await _context.SchedulePlans.FindAsync(id);
            if (schedulePlans == null)
            {
                return NotFound();
            }

            _context.SchedulePlans.Remove(schedulePlans);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SchedulePlansExists(int id)
        {
            return _context.SchedulePlans.Any(e => e.Id == id);
        }
    }
}
