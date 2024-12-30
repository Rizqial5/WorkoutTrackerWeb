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
    public class ExerciseDatasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseDatasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDataResponse>>> GetExerciseDatas()
        {
            var exerciseDatas = await _context.ExerciseDatas.ToListAsync();

            var responses = exerciseDatas.Select(ResponsesHelper.ExerciseDataResponse);

            return Ok(responses);


        }

        // GET: api/ExerciseDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDataResponse>> GetExerciseData(int id)
        {
            var exerciseData = await _context.ExerciseDatas.FindAsync(id);

            if (exerciseData == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.ExerciseDataResponse(exerciseData);

            return Ok(response);
        }

        // PUT: api/ExerciseDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExerciseData(int id, ExerciseData exerciseData)
        {
            if (id != exerciseData.ExerciseId)
            {
                return BadRequest();
            }

            _context.Entry(exerciseData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseDataExists(id))
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

        // POST: api/ExerciseDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExerciseData>> PostExerciseData(ExerciseData exerciseData)
        {
            _context.ExerciseDatas.Add(exerciseData);
            await _context.SaveChangesAsync();

            var response = ResponsesHelper.ExerciseDataResponse(exerciseData);

            return CreatedAtAction("GetExerciseData", new { id = exerciseData.ExerciseId }, response);
        }

        // DELETE: api/ExerciseDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseData(int id)
        {
            var exerciseData = await _context.ExerciseDatas.FindAsync(id);
            if (exerciseData == null)
            {
                return NotFound();
            }

            _context.ExerciseDatas.Remove(exerciseData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseDataExists(int id)
        {
            return _context.ExerciseDatas.Any(e => e.ExerciseId == id);
        }
    }
}
