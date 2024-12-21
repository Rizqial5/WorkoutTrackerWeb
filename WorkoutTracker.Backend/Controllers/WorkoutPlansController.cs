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
using WorkoutTracker.Backend.Services.Interfaces;
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
        private readonly IRedisCacheService _cacheService;
        private readonly ILogger<WorkoutPlansController> _logger;

        
        
        public WorkoutPlansController(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager,
            IRedisCacheService cacheService,
            ILogger<WorkoutPlansController> logger)
        {
            _context = context;
            _userManager = userManager;
            _cacheService = cacheService;
            _logger = logger;
        }

        // GET: api/WorkoutPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutPlans>>> GetWorkoutPlans()
        {

            var user = await _userManager.GetUserAsync(User);

            var cacheKeyAll = $"WorkoutPlans:User:{user.Id}";
            
            var cacheData = await _cacheService.GetCacheAsync<List<WorkoutPlanResponse>>(cacheKeyAll);

            

            if (cacheData != null)
            {
                _logger.LogInformation("Retrieve Data from Cache");
                _logger.LogInformation($"User cache for {cacheKeyAll}");
                return Ok(cacheData);
            }



            var workoutPlans = await WorkoutPlansList();

            var workoutPlanList = await workoutPlans.ToListAsync();



            var response = workoutPlanList.Select(ResponsesHelper.WorkoutPlanResponse);

            _logger.LogInformation("Add Data to cache");

            await _cacheService.SetCacheAsync(cacheKeyAll, response, TimeSpan.FromMinutes(1));

            return Ok(response);   
        }

        private async Task<IIncludableQueryable<WorkoutPlans, ExerciseData?>> WorkoutPlansList()
        {
            var user = await _userManager.GetUserAsync(User);

            var workoutPlans = _context.WorkoutPlans
                .Where(wp => wp.UserId == user.Id)
                .Include(wp => wp.ScheduledTime.Where(sp=>sp.UserId == user.Id))
                .Include(wp => wp.ExerciseSets.Where(es=>es.UserId == user.Id))
                .ThenInclude(s => s.Exercise);
                
            return workoutPlans;
        }

        

       

        // GET: api/WorkoutPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutPlans>> GetWorkoutPlans(int id)
        {

            var cacheKeyId = $"WorkoutPlan:{id}";
            var cacheData = await _cacheService.GetCacheAsync<WorkoutPlanResponse>(cacheKeyId);
            if (cacheData != null)
            {
                _logger.LogInformation("Retrieve Data from Cache");
                return Ok(cacheData);
            }

            var workoutPlans = await WorkoutPlansList();

            var workoutPlanList = await workoutPlans.FirstOrDefaultAsync(wp => wp.PlanId == id);

            if (workoutPlanList == null)
            {
                return NotFound();
            }

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlanList);

            _logger.LogInformation("Update data in Cache");
            await _cacheService.SetCacheAsync(cacheKeyId, response, TimeSpan.FromMinutes(1));

            return Ok(response);
        }

        [HttpGet("{id}/report")]
        public async Task<ActionResult<WorkoutPlans>> GetPlanReport(int id)
        {

            var cacheKeyId = $"WorkoutPlanReport:{id}";
            var cacheData = await _cacheService.GetCacheAsync<WorkoutPlanReport>(cacheKeyId);
            if (cacheData != null)
            {
                _logger.LogInformation("Retrieve Data from Cache");
                return Ok(cacheData);
            }

            var user = await _userManager.GetUserAsync(User);

            var workoutPlans = await _context.WorkoutPlans
                .Where(wp=> wp.UserId == user.Id)
                .Include(wp => wp.ExerciseSets)
                .Include(wp => wp.ScheduledTime.Where(sp=> sp.PlanStatus == PlanStatus.Done))
                .FirstOrDefaultAsync(wp => wp.PlanId == id);

            if (workoutPlans == null) return NotFound("Workout Plan not found");
            
            int countWorkoutPlanDone = workoutPlans.ScheduledTime.Count();

            var response = new WorkoutPlanReport
            {
                PlanName = workoutPlans.PlanName,
                ExerciseListName = workoutPlans.ExerciseSets.Select(es => es.ExerciseSetName).ToList(),
                TotalWorkoutDone = countWorkoutPlanDone,
                ListPastSchedule = workoutPlans.ScheduledTime.Select(sp => new ScheduleDone
                {
                    DateSchedule = sp.ScheduleTime,
                    PlanStatus = sp.PlanStatus
                }).ToList()


            };

            _logger.LogInformation("Update data in Cache");
            await _cacheService.SetCacheAsync(cacheKeyId, response, TimeSpan.FromMinutes(1));

            return Ok(response);

        }


        // PUT: api/WorkoutPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutPlans(int id, WorkoutPlansPostRequest workoutRequest)
        {
            var user = await _userManager.GetUserAsync(User);

            var workoutPlan = await WorkoutPlansList();

            var workoutPlansList = await workoutPlan.FirstOrDefaultAsync(wp => wp.PlanId == id && wp.UserId == user.Id );

            if (workoutPlansList == null) return NotFound(new { Message = "Workout Plan Not Found" });

            workoutPlansList.PlanName = workoutRequest.PlansName;

            var newExercises = _context.ExerciseSets
                .Where(e => workoutRequest.ExercisesSetExercisesCollection.Contains(e.ExerciseId));

            foreach (var exercise in newExercises)
            {
                if (workoutPlansList!.ExerciseSets!.All(e => e.ExerciseSetId != exercise.ExerciseSetId))
                {
                    workoutPlansList.ExerciseSets.Add(exercise);
                }
            }

            await _context.SaveChangesAsync();

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlansList);

            var CacheKeyPlan = $"WorkoutPlan:User:{user.Id}";

            await ClearAllCache(user);
            await _cacheService.SetCacheAsync(CacheKeyPlan, response, TimeSpan.FromMinutes(2));
            


            return Ok(response);
        }

        
        

        // POST: api/WorkoutPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkoutPlans>> PostWorkoutPlans([FromBody] WorkoutPlansPostRequest workoutPlansRequest)
        {

            var user = await _userManager.GetUserAsync(User);

            var workoutPlans = new WorkoutPlans
            {
                PlanName = workoutPlansRequest.PlansName,
                UserId = user.Id
            };

            var exercises = await _context.ExerciseSets
                .Include(exerciseSet => exerciseSet.Exercise!)
                .Where(e => 
                    workoutPlansRequest.ExercisesSetExercisesCollection.Contains(e.ExerciseSetId) && e.UserId == workoutPlans.UserId)
                .ToListAsync();

            if (exercises == null) return BadRequest("Exercise Set Not found or User dont have Exercise Set");

            workoutPlans.ExerciseSets = exercises;
            //workoutPlans.PlanStatus = PlanStatus.Pending;

            _context.WorkoutPlans.Add(workoutPlans);
            await _context.SaveChangesAsync();

            var response = ResponsesHelper.WorkoutPlanResponse(workoutPlans);
            await ClearAllCache(user);


            return CreatedAtAction("GetWorkoutPlans", new { id = workoutPlans.PlanId }, response);
        }

        private async Task ClearAllCache(IdentityUser user)
        {
            var CacheKeyAll = $"WorkoutPlans:User:{user.Id}";
            var CacheKeyPlan = $"WorkoutPlan:User:{user.Id}";
            var cacheKeyReport = $"WorkoutPlanReport:User:{user.Id}";

            _logger.LogInformation("Clear All Cache");

            await _cacheService.DeleteCacheAsync<WorkoutPlanResponse>(CacheKeyPlan);
            await _cacheService.DeleteCacheAsync<WorkoutPlanReport>(cacheKeyReport);
            await _cacheService.DeleteCacheAsync<List<WorkoutPlanResponse>>(CacheKeyAll);
        }

        // DELETE: api/WorkoutPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlans(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var workoutPlans =
                await _context.WorkoutPlans.FirstOrDefaultAsync(wp => wp.PlanId == id && wp.UserId == user.Id);

            

            if (workoutPlans == null)
            {
                return NotFound();
            }

            _context.WorkoutPlans.Remove(workoutPlans);
            await _context.SaveChangesAsync();

            await ClearAllCache(user);
            

            return Ok("Data deleted successfully");
        }


    }
}
