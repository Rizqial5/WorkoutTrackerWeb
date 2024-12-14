using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Utilities;

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
            var schedulePlans = await _context.SchedulePlans
                .Include(sp => sp.WorkoutPlan).ToListAsync();

            var response = schedulePlans.Select(ResponsesHelper.SchedulePlansResponse);
            
            

            return Ok(response);
        }

        // GET: api/SchedulePlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchedulePlans>> GetSchedulePlans(int id)
        {
            var schedulePlans = await _context.SchedulePlans
                .Include(sp=>sp.WorkoutPlan)
                .FirstOrDefaultAsync(sp=> sp.Id == id);

            if (schedulePlans == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.SchedulePlansResponse(schedulePlans);

            return Ok(response);
        }

        // PUT: api/SchedulePlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedulePlans(int id, SchedulePlansRequest request)
        {
            var schedulePlans = await _context.SchedulePlans
                .Include(sp => sp.WorkoutPlan)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (schedulePlans == null) return NotFound("Schedule Plans Not Found");

            var workPlans = await _context.WorkoutPlans.FindAsync(request.WorkoutPlansId);

            if (workPlans == null) return NotFound();

            schedulePlans.ScheduleTime = request.DateTime;
            schedulePlans.WorkoutPlansId = request.WorkoutPlansId;

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.SchedulePlansResponse(schedulePlans);


            return Ok(response);
        }

        // POST: api/SchedulePlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SchedulePlans>> PostSchedulePlans(SchedulePlansRequest request)
        {

            var workoutPlans = await _context.WorkoutPlans.FindAsync(request.WorkoutPlansId);

            if (workoutPlans == null) return NotFound("Workout Plan not Found");

            workoutPlans.PlanStatus = PlanStatus.Active;

            var schedulePlans = new SchedulePlans
            {
                ScheduleTime = request.DateTime,
                WorkoutPlansId = request.WorkoutPlansId
            };

            

            _context.SchedulePlans.Add(schedulePlans);

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.SchedulePlansResponse(schedulePlans);

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

            return Ok("Data deleted successfully");
        }

    }
}
