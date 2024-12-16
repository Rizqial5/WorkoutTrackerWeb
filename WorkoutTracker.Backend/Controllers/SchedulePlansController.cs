using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Utilities;

namespace WorkoutTracker.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SchedulePlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SchedulePlansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/SchedulePlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulePlans>>> GetSchedulePlans()
        {

            var user = await _userManager.GetUserAsync(User);

            var schedulePlans = await _context.SchedulePlans
                .Where(sp=> sp.UserId == user.Id)
                .Include(sp => sp.WorkoutPlan)
                .ToListAsync();

            var response = schedulePlans.Select(ResponsesHelper.SchedulePlansResponse);
            
            

            return Ok(response);
        }

        // GET: api/SchedulePlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchedulePlans>> GetSchedulePlans(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var schedulePlans = await _context.SchedulePlans
                .Where(sp=> sp.UserId == user.Id)
                .Include(sp=>sp.WorkoutPlan)
                .FirstOrDefaultAsync(sp=> sp.Id == id);

            if (schedulePlans == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.SchedulePlansResponse(schedulePlans);

            return Ok(response);
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<SchedulePlans>>> GetSortedPlans()
        {
            var user = await _userManager.GetUserAsync(User);

            var schedulePlans = await _context.SchedulePlans
                .Where(sp => sp.UserId == user.Id)
                .Include(sp => sp.WorkoutPlan)
                .OrderBy(sp => sp.ScheduleTime)
                .ToListAsync();

            var response = schedulePlans.Select(ResponsesHelper.SchedulePlansResponse);

            return Ok(response);

        }

        // PUT: api/SchedulePlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedulePlans(int id, SchedulePlansRequest request)
        {
            var user = await _userManager.GetUserAsync(User);

            var schedulePlans = await _context.SchedulePlans
                .Where(sp => sp.UserId == user.Id)
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

        [HttpPut("{id}/done")]
        public async Task<IActionResult> SetDoneWorkoutPlans(int id)
        {
            var user = await _userManager.GetUserAsync(User);


            var schedulePlan = await _context.SchedulePlans
                .Where(sp => sp.UserId == user.Id)
                .Include(sp=> sp.WorkoutPlan)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (schedulePlan == null) return NotFound(new { Message = "Schedule Plan Not Found" });
            if (schedulePlan.PlanStatus == PlanStatus.Done) return BadRequest("Schedule plans Has already set to done");

            schedulePlan.PlanStatus = PlanStatus.Done;

            await _context.SaveChangesAsync();

            var response = $"{schedulePlan.WorkoutPlan.PlanName} set to {schedulePlan.PlanStatus}";

            return Ok(response);
        }

        // POST: api/SchedulePlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SchedulePlans>> PostSchedulePlans(SchedulePlansRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var workoutPlans = await _context.WorkoutPlans.FindAsync(request.WorkoutPlansId);

            if (workoutPlans == null) return NotFound("Workout Plan not Found");

            //workoutPlans.PlanStatus = PlanStatus.Active;

            var schedulePlans = new SchedulePlans
            {
                ScheduleTime = request.DateTime,
                PlanStatus = PlanStatus.Active,
                WorkoutPlansId = request.WorkoutPlansId,
                UserId = user.Id
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
            var user = await _userManager.GetUserAsync(User);

            var schedulePlans = await _context.SchedulePlans
                .Where(sp => sp.UserId == user.Id)
                .FirstOrDefaultAsync(sp => sp.Id == id);

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
