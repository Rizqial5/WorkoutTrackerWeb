using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MockQueryable;
using MockQueryable.FakeItEasy;
using WorkoutTracker.Backend.Controllers;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Services.Interfaces;
using WorkoutTracker.Backend.Utilities;
using Xunit;
using Xunit.Abstractions;
using Moq;

public class WorkoutPlanControllerTest
{
    private readonly ITestOutputHelper _testOutputHelper;
   
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRedisCacheService _redisCacheService;
    private readonly ILogger<WorkoutPlansController> _logger;
    private readonly ApplicationDbContext fakeDbContext;

    public WorkoutPlanControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

        fakeDbContext = A.Fake<ApplicationDbContext>(x =>
            x.WithArgumentsForConstructor(() => new ApplicationDbContext(options)));

        _userManager = A.Fake<UserManager<IdentityUser>>();
        _redisCacheService = A.Fake<IRedisCacheService>();
        _logger = A.Fake<ILogger<WorkoutPlansController>>();
    }

    [Fact]
    public async Task GetWorkoutPlans_ReturnsDataWithUserLogin()
    {

        // Arrange

        var identityUser = new IdentityUser { Id = "User1", UserName = "TestUser" };
        var workoutPlans = GenerateWorkoutPlansList(identityUser);

        A.CallTo(() => fakeDbContext.WorkoutPlans).Returns(workoutPlans.AsQueryable().BuildMockDbSet());

        // Fake UserManager
        var fakeUser = identityUser;
        A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>.Ignored))
            .Returns(Task.FromResult(fakeUser));

        // Fake CacheService
        A.CallTo(() => _redisCacheService.GetCacheAsync<List<WorkoutPlanResponse>>(A<string>.Ignored))
            .Returns(Task.FromResult<List<WorkoutPlanResponse>>(null)); // Simulating no cache hit

        

        var controller = new WorkoutPlansController(fakeDbContext, _userManager, _redisCacheService, _logger);

        // Act
        var result = await controller.GetWorkoutPlans();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<WorkoutPlanResponse>>(okResult.Value);

        
        // Verifying the response
          // We expect one response due to the single fake WorkoutPlan
        var planResponse = response.First();

        Assert.Equal("Plan A", planResponse.PlanName);
        Assert.Equal("TestUser", planResponse.User); // Check fake user login
    }

    private static List<WorkoutPlans> GenerateWorkoutPlansList(IdentityUser identityUser)
    {
        var workoutPlans = new List<WorkoutPlans>
        {
            new WorkoutPlans
            {
                PlanId = 1,
                PlanName = "Plan A",
                UserId = identityUser.Id,
                User = identityUser,
                ScheduledTime = new List<SchedulePlans>(),
                ExerciseSets = new List<ExerciseSet>()
            },
            new WorkoutPlans
            {
                PlanId = 2,
                PlanName = "Plan B",
                UserId = identityUser.Id,
                User = identityUser,
                ScheduledTime = new List<SchedulePlans>(),
                ExerciseSets = new List<ExerciseSet>()
            }
        };
        return workoutPlans;
    }
}
