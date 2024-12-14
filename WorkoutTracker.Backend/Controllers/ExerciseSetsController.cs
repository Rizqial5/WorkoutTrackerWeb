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
    public class ExerciseSetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseSetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseSet>>> GetExerciseSets()
        {
            var exerciseSets = await _context.ExerciseSets
                .Include(es => es.Exercise)
                .ToListAsync();

            var response = exerciseSets.Select(ResponsesHelper.ExerciseSetResponse);

            return Ok(response);
        }

        // GET: api/ExerciseSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseSet>> GetExerciseSet(int id)
        {
            var exerciseSet = await _context!.ExerciseSets!
                .Include(es => es.Exercise)
                .FirstOrDefaultAsync(es => es.ExerciseSetId == id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);

            return Ok(response);
        }

        // PUT: api/ExerciseSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExerciseSet(int id, CreateExerciseSetRequest request)
        {
            var exerciseSet = await _context.ExerciseSets
                .Include(es => es.Exercise)
                .FirstOrDefaultAsync(es => es.ExerciseSetId == id);

            if (exerciseSet == null) return NotFound("Exercise Set not found");
            
            var exercise = await _context.ExerciseDatas.FindAsync(request.ExerciseId);

            if (exercise == null) return NotFound("Exercise not found in Exercise Data");

            exerciseSet.ExerciseSetName = request.ExerciseSetName;
            exerciseSet.ExerciseId = request.ExerciseId;
            exerciseSet.Set = request.Set;
            exerciseSet.Repetitions = request.Repetitions;
            exerciseSet.Weight = request.Weight;

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);

            return Ok(response);
        }

        // POST: api/ExerciseSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExerciseSet>> PostExerciseSet(CreateExerciseSetRequest exerciseSetRequest)
        {
            

            var exercise = await _context.ExerciseDatas
                    .FindAsync(exerciseSetRequest.ExerciseId);

            if (exercise == null) return BadRequest("Exercise Not Found in ExerciseData");

            var exerciseSet = new ExerciseSet
            {
                ExerciseSetName = exerciseSetRequest.ExerciseSetName,
                Repetitions = exerciseSetRequest.Repetitions,
                Set = exerciseSetRequest.Set,
                Weight = exerciseSetRequest.Weight,
                ExerciseId = exerciseSetRequest.ExerciseId,
                
            };
            

            _context.ExerciseSets.Add(exerciseSet);

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);

            return CreatedAtAction("GetExerciseSet", new { id = exerciseSet.ExerciseSetId }, response);
        }

        

        // DELETE: api/ExerciseSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseSet(int id)
        {
            var exerciseSet = await _context.ExerciseSets.FindAsync(id);
            if (exerciseSet == null)
            {
                return NotFound();
            }

            _context.ExerciseSets.Remove(exerciseSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
