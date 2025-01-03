using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Services.Interfaces;
using WorkoutTracker.Backend.Utilities;

namespace WorkoutTracker.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ExerciseSetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRedisCacheService _cacheService;
        private readonly ILogger<ExerciseSetsController> _logger;

        public ExerciseSetsController(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager, 
            IRedisCacheService cacheService,
            ILogger<ExerciseSetsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _cacheService = cacheService;
            _logger = logger;
        }

        // GET: api/ExerciseSets
        [HttpGet]
        [EndpointDescription(
            "Displays all exercise sets created by the user."
        )]
        public async Task<ActionResult<IEnumerable<ExerciseSet>>> GetExerciseSets()
        {

            var user = await GetUserAsync();
            
            //Caching
            var cacheKeyExerciseAll = $"ExerciseSets:User:{user.Id}";
            var cacheData = await _cacheService.GetCacheAsync<List<ExerciseSetResponse>>(cacheKeyExerciseAll);

            if (cacheData != null)
            {
                _logger.LogInformation("Retrieve Data from Cache");
                _logger.LogInformation($"User cache for {cacheKeyExerciseAll}");

                return Ok(cacheData);
            }
            //---------------------------

            var exerciseSets = await _context.ExerciseSets
                .Where(es => es.UserId == user.Id)
                .Include(es => es.Exercise)
                .ToListAsync();

            

            var response = exerciseSets.Select(ResponsesHelper.ExerciseSetResponse);

            await _cacheService.SetCacheAsync(cacheKeyExerciseAll, response, TimeSpan.FromMinutes(1));

            return Ok(response);
        }

        private async Task<IdentityUser?> GetUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user;
        }

        // GET: api/ExerciseSets/5
        [HttpGet("{id}")]
        [EndpointDescription(
            "Displays exercise sets created by the user through id input."
        )]
        public async Task<ActionResult<ExerciseSet>> GetExerciseSet(int id)
        {
            var user = await GetUserAsync();

            //Caching
            var cacheKeyExercise = $"ExerciseSet:User:{user.Id}";
            var cacheData = await _cacheService.GetCacheAsync<ExerciseSetResponse>(cacheKeyExercise);

            if (cacheData != null)
            {
                _logger.LogInformation("Retrieve Data from Cache");
                _logger.LogInformation($"User cache for {cacheKeyExercise}");

                return Ok(cacheData);
            }
            //---------------------------

            var exerciseSet = await _context!.ExerciseSets!
                .Where(es => es.UserId == user.Id)
                .Include(es => es.Exercise)
                .FirstOrDefaultAsync(es => es.ExerciseSetId == id);

            if (exerciseSet == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);

            await _cacheService.SetCacheAsync(cacheKeyExercise, response, TimeSpan.FromMinutes(1));

            return Ok(response);
        }

        // PUT: api/ExerciseSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EndpointDescription(
            "Modifies exercise sets based on the input ID."
        )]
        public async Task<IActionResult> PutExerciseSet(int id, CreateExerciseSetRequest request)
        {
            var user = await GetUserAsync();
            var exerciseSet = await _context.ExerciseSets
                .Where(es => es.UserId == user.Id)
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

            var cacheKeyExercise = $"ExerciseSet:User:{user.Id}";

            await ClearAllCache(user);

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);
            
            await _cacheService.SetCacheAsync(cacheKeyExercise, response, TimeSpan.FromMinutes(1));

            return Ok(response);
        }

        // POST: api/ExerciseSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EndpointDescription(
            "Input new exercise set based on exercise data"
        )]
        public async Task<ActionResult<ExerciseSet>> PostExerciseSet(CreateExerciseSetRequest exerciseSetRequest)
        {
            var user = await GetUserAsync();

            var exercise = await _context.ExerciseDatas
                    .FindAsync(exerciseSetRequest.ExerciseId);

            if (exercise == null) return BadRequest("Exercise Not Found in ExerciseData");

            var exerciseSet = new ExerciseSet
            {
                ExerciseSetName = exerciseSetRequest.ExerciseSetName,
                Repetitions = exerciseSetRequest.Repetitions,
                UserId = user.Id,
                Set = exerciseSetRequest.Set,
                Weight = exerciseSetRequest.Weight,
                ExerciseId = exerciseSetRequest.ExerciseId,
                
            };
            

            _context.ExerciseSets.Add(exerciseSet);

            await _context.SaveChangesAsync();

            await ClearAllCache(user);

            var response = ResponsesHelper.ExerciseSetResponse(exerciseSet);

            return CreatedAtAction("GetExerciseSet", new { id = exerciseSet.ExerciseSetId }, response);
        }

        

        // DELETE: api/ExerciseSets/5
        [HttpDelete("{id}")]
        [EndpointDescription(
            "Delete exercise set based on id input"
        )]
        public async Task<IActionResult> DeleteExerciseSet(int id)
        {
    
            var user = await GetUserAsync();
            var exerciseSet = await _context.ExerciseSets
                .Where(es => es.UserId == user.Id)
                .FirstOrDefaultAsync(es => es.ExerciseSetId == id);


            if (exerciseSet == null)
            {
                return NotFound();
            }

            _context.ExerciseSets.Remove(exerciseSet);
            await _context.SaveChangesAsync();

            await ClearAllCache(user);

            return Ok("Data deleted successfully");
        }

        private async Task ClearAllCache(IdentityUser user)
        {
            var cacheKeyExerciseAll = $"ExerciseSets:User:{user.Id}";
            var cacheKeyExercise = $"ExerciseSet:User:{user.Id}";

            _logger.LogInformation("Clear All Cache");

            await _cacheService.DeleteCacheAsync<ExerciseSetResponse>(cacheKeyExercise);
            await _cacheService.DeleteCacheAsync<List<ExerciseSetResponse>>(cacheKeyExerciseAll);
        }


    }
}
