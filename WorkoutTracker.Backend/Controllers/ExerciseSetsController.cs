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
            return await _context.ExerciseSets.ToListAsync();
        }

        // GET: api/ExerciseSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseSet>> GetExerciseSet(int id)
        {
            var exerciseSet = await _context.ExerciseSets.FindAsync(id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            return exerciseSet;
        }

        // PUT: api/ExerciseSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExerciseSet(int id, ExerciseSet exerciseSet)
        {
            if (id != exerciseSet.ExerciseSetId)
            {
                return BadRequest();
            }

            _context.Entry(exerciseSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseSetExists(id))
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

        // POST: api/ExerciseSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExerciseSet>> PostExerciseSet(ExerciseSet exerciseSet)
        {
            _context.ExerciseSets.Add(exerciseSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExerciseSet", new { id = exerciseSet.ExerciseSetId }, exerciseSet);
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

        private bool ExerciseSetExists(int id)
        {
            return _context.ExerciseSets.Any(e => e.ExerciseSetId == id);
        }
    }
}
