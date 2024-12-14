using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Utilities;

namespace WorkoutTracker.Backend.Controllers
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
            var workoutPlans =await WorkoutPlansEnumerable().ToListAsync();
                

            var response = workoutPlans.Select(ResponsesHelper.WorkoutPlanResponse);

            return Ok(response);   
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

        //[HttpPut("{id}/done")]
        //public async Task<IActionResult> SetDoneWorkoutPlans(int id)
        //{
        //    var workoutPlan = await WorkoutPlansEnumerable().FirstOrDefaultAsync(wp => wp.PlanId == id);

        //    if (workoutPlan == null) return NotFound(new { Message = "Workout Plan Not Found" });
        //    if (workoutPlan.PlanStatus == PlanStatus.Done) return BadRequest("Workout plans Has already set to done");

        //    workoutPlan.PlanStatus = PlanStatus.Done;

        //    await _context.SaveChangesAsync();

        //    var response = $"{workoutPlan.PlanName} set to {workoutPlan.PlanStatus}"; 

        //    return Ok(response);
        //}


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
