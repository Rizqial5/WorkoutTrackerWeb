using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Utilities;

namespace WorkoutTracker.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkoutPlansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/WorkoutPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutPlans>>> GetWorkoutPlans()
        {

            var workoutPlans = await WorkoutPlansList();


            var response = workoutPlans.Select(ResponsesHelper.WorkoutPlanResponse);

            return Ok(response);   
        }

        private async Task<List<WorkoutPlans>> WorkoutPlansList()
        {
            var user = await _userManager.GetUserAsync(User);

            var workoutPlans = await _context.WorkoutPlans
                .Where(wp => wp.UserId == user.Id)
                .Include(wp => wp.ScheduledTime)
                .Include(wp => wp.ExerciseSets)
                .ThenInclude(s => s.Exercise)
                .ToListAsync();
            return workoutPlans;
        }

        

        private IIncludableQueryable<WorkoutPlans, ExerciseData?> WorkoutPlansEnumerable()
        {
           

            var workoutPlans = _context.WorkoutPlans
                .Include(wp => wp.ScheduledTime)
                .Include(wp => wp.ExerciseSets)
                .ThenInclude(s => s.Exercise);
                
            return workoutPlans;
        }

        // GET: api/WorkoutPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlans>> GetWorkoutPlans(int id)
        {
            var workoutPlans = await WorkoutPlansEnumerable().FirstOrDefaultAsync(wp => wp.PlanId == id);

            if (workoutPlans == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlans);

            return Ok(response);
        }

        [HttpGet("{id}/report")]
        public async Task<ActionResult<WorkoutPlans>> GetPlanReport(int id)
        {
            var workoutPlans = await _context.WorkoutPlans
                .Include(wp => wp.ExerciseSets)
                .Include(wp => wp.ScheduledTime.Where(sp=> sp.PlanStatus == PlanStatus.Done))
                .FirstOrDefaultAsync(wp => wp.PlanId == id);

            if (workoutPlans == null) return NotFound("Workout Plan not found");
            
            int countWorkoutPlanDone = workoutPlans.ScheduledTime.Count();

            var response = new
            {
                workoutPlans.PlanName,
                ExerciseName = workoutPlans.ExerciseSets.Select(es => new
                {
                    es.ExerciseSetName

                }).ToList(),
                TotalWorkoutDone = countWorkoutPlanDone,
                ScheduledPlans = workoutPlans.ScheduledTime.Select(sp => new
                {
                    sp.ScheduleTime,
                    sp.PlanStatus
                }).ToList()


            };

            return Ok(response);

        }


        // PUT: api/WorkoutPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutPlans(int id, WorkoutPlansPostRequest workoutRequest)
        {
            var workoutPlan = await WorkoutPlansEnumerable().FirstOrDefaultAsync(wp => wp.PlanId == id);

            if (workoutPlan == null) return NotFound(new { Message = "Workout Plan Not Found" });

            workoutPlan.PlanName = workoutRequest.PlansName;

            var newExercises = _context.ExerciseSets
                .Where(e => workoutRequest.ExercisesSetExercisesCollection.Contains(e.ExerciseId));

            foreach (var exercise in newExercises)
            {
                if (workoutPlan!.ExerciseSets!.All(e => e.ExerciseSetId != exercise.ExerciseSetId))
                {
                    workoutPlan.ExerciseSets.Add(exercise);
                }
            }

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlan);


            return Ok(response);
        }

        
        

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlans>> PostWorkoutPlans([FromBody] WorkoutPlansPostRequest workoutPlansRequest)
        {
            var workoutPlans = new WorkoutPlans { PlanName = workoutPlansRequest.PlansName };

            var exercises = await _context.ExerciseSets
                .Include(exerciseSet => exerciseSet.Exercise!)
                .Where(e => workoutPlansRequest.ExercisesSetExercisesCollection.Contains(e.ExerciseSetId))
                .ToListAsync();

            workoutPlans.ExerciseSets = exercises;
            //workoutPlans.PlanStatus = PlanStatus.Pending;

            _context.WorkoutPlans.Add(workoutPlans);
            await _context.SaveChangesAsync();

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlans);

            return CreatedAtAction("GetWorkoutPlans", new { id = workoutPlans.PlanId }, response);
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

            return Ok("Data deleted successfully");
        }


    }
}
