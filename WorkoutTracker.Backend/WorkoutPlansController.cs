using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkoutPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutPlans>>> GetWorkoutPlans()
        {
            return await _context.WorkoutPlans.ToListAsync();
        }

        // GET: api/WorkoutPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlans>> GetWorkoutPlans(int id)
        {
            var workoutPlans = await _context.WorkoutPlans.FindAsync(id);

            if (workoutPlans == null)
            {
                return NotFound();
            }

            return workoutPlans;
        }

        // PUT: api/WorkoutPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutPlans(int id, WorkoutPlans workoutPlans)
        {
            if (id != workoutPlans.PlanId)
            {
                return BadRequest();
            }

            _context.Entry(workoutPlans).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutPlansExists(id))
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

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlans>> PostWorkoutPlans([FromBody] WorkoutPlansPostDTO workoutPlansPost)
        {
            var workoutPlans = new WorkoutPlans { PlanName = workoutPlansPost.PlansName };

            var exercises = await _context.ExerciseDatas
                .Where(e => workoutPlansPost.ExercisesCollection.Contains(e.ExerciseId))
                .ToListAsync();

            workoutPlans.Exercises = exercises;
            //return Ok(workoutPlans);

            _context.WorkoutPlans.Add(workoutPlans);
            await _context.SaveChangesAsync();

            var response = new
            {
                workoutPlans.PlanId,
                workoutPlans.PlanName,
                Exercises = workoutPlans.Exercises
                    .Select(e => new { e.ExerciseId, e.Name })
                    .ToList()
            };

            return CreatedAtAction("GetWorkoutPlans", new { id = workoutPlans.PlanId },response);
        }

        // DELETE: api/WorkoutPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlans(int id)
        {
            var workoutPlans = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlans == null)
            {
                return NotFound();
            }

            _context.WorkoutPlans.Remove(workoutPlans);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkoutPlansExists(int id)
        {
            return _context.WorkoutPlans.Any(e => e.PlanId == id);
        }
    }
}
