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
            var workoutPlans= await _context.WorkoutPlans
                .Include(wp => wp.Exercises)
                .Select(wp => new
                {
                    wp.PlanId,
                    wp.PlanName,
                    Exercises = wp.Exercises
                        .Select(e => new
                        {
                            e.ExerciseId, 
                            e.Name, 
                            e.CategoryWorkout, 
                            e.MuscleGroup
                        })

                }).ToListAsync();

            return Ok(workoutPlans);   
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
        public async Task<IActionResult> PutWorkoutPlans(int id, WorkoutPlansPostRequest workoutRequest)
        {
            var workoutPlan = _context.WorkoutPlans
                .Include(wp => wp.Exercises)
                .FirstOrDefault(wp => wp.PlanId == id);

            if (workoutPlan == null) return NotFound(new { Message = "Workout Plan Not Found" });

            workoutPlan.PlanName = workoutRequest.PlansName;

            var newExercises = _context.ExerciseDatas
                .Where(e => workoutRequest.ExercisesCollection.Contains(e.ExerciseId));

            foreach (var exercise in newExercises)
            {
                if (workoutPlan!.Exercises!.All(e => e.ExerciseId != exercise.ExerciseId))
                {
                    workoutPlan.Exercises.Add(exercise);
                }
            }

            await _context.SaveChangesAsync();

            var response = new
            {
                workoutPlan.PlanId,
                workoutPlan.PlanName,
                Exercises = workoutPlan.Exercises
                    .Select(e => new 
                       { 
                            e.ExerciseId, 
                            e.Name,
                            e.CategoryWorkout,
                            e.MuscleGroup
                        })
                    .ToList()
            };


            return Ok(response);
        }

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlans>> PostWorkoutPlans([FromBody] WorkoutPlansPostRequest workoutPlansRequest)
        {
            var workoutPlans = new WorkoutPlans { PlanName = workoutPlansRequest.PlansName };

            var exercises = await _context.ExerciseDatas
                .Where(e => workoutPlansRequest.ExercisesCollection.Contains(e.ExerciseId))
                .ToListAsync();

            workoutPlans.Exercises = exercises;

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

            return NoContent();
        }


    }
}
