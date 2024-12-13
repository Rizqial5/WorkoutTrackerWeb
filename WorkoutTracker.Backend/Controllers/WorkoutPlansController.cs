﻿using System;
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
                .Include(wp => wp.ExerciseSets)
                    .ThenInclude(s=>s.Exercise)
                .Select(wp => new
                {
                    wp.PlanId,
                    wp.PlanName,
                    ExerciseSets = wp.ExerciseSets
                        .Select(e => new
                        {
                            e.ExerciseSetId, 
                            Exercise = new
                            {
                                e.Exercise!.Name,
                                e.Exercise!.CategoryWorkout,
                                e.Exercise!.MuscleGroup
                            },
                            e.Repetitions,
                            e.Set,
                            
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
                .Include(wp => wp.ExerciseSets)
                    .ThenInclude(exerciseSet => exerciseSet.Exercise!)
                .FirstOrDefault(wp => wp.PlanId == id);

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

            var response = new
            {
                workoutPlan.PlanId,
                workoutPlan.PlanName,
                Exercises = workoutPlan.ExerciseSets
                    .Select(e => new 
                       {
                        e.ExerciseSetId,
                        Exercise = new
                        {
                            e.Exercise!.Name,
                            e.Exercise!.CategoryWorkout,
                            e.Exercise!.MuscleGroup
                        },
                        e.Repetitions,
                        e.Set,
                    })
                    .ToList()
            };


            return Ok(response);
        }

        [HttpPut("schedule/{id}")]
        public async Task<ActionResult> SetScheduleWorkout(int id, DateTime setDateTime)
        {
            var workoutPlan = _context.WorkoutPlans
                .Include(wp => wp.ExerciseSets)
                .FirstOrDefault(wp => wp.PlanId == id);

            if (workoutPlan == null) return NotFound(new { Message = "Workout Plan Not Found" });

            workoutPlan.ScheduledTime = setDateTime;

            await _context.SaveChangesAsync();

            var response = new
            {
                workoutPlan.PlanId,
                workoutPlan.PlanName,
                workoutPlan.ScheduledTime
            };

            return Ok(response);

        }

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlans>> PostWorkoutPlans([FromBody] WorkoutPlansPostRequest workoutPlansRequest)
        {
            var workoutPlans = new WorkoutPlans { PlanName = workoutPlansRequest.PlansName };

            var exercises = await _context.ExerciseSets
                .Where(e => workoutPlansRequest.ExercisesSetExercisesCollection.Contains(e.ExerciseSetId))
                .Include(exerciseSet => exerciseSet.Exercise!)
                .ToListAsync();

            workoutPlans.ExerciseSets = exercises;

            _context.WorkoutPlans.Add(workoutPlans);
            await _context.SaveChangesAsync();

            var response = new
            {
                workoutPlans.PlanId,
                workoutPlans.PlanName,
                Exercises = workoutPlans.ExerciseSets
                    .Select(e => new
                    {
                        e.ExerciseSetId,
                        Exercise = new
                        {
                            e.Exercise!.Name,
                            e.Exercise!.CategoryWorkout,
                            e.Exercise!.MuscleGroup
                        },
                        e.Repetitions,
                        e.Set,
                    })
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
