using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkoutTracker.Backend.Controllers;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Services.Interfaces;
using WorkoutTracker.Backend.Utilities;
using Xunit;
using Xunit.Abstractions;

public class WorkoutPlanControllerTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public WorkoutPlanControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetWorkoutPlans_ReturnsDataWithUserLogin()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Membuat instansi WorkoutPlans secara manual tanpa menggunakan FakeItEasy
        var workoutPlan = new WorkoutPlans
        {
            PlanId = 1,
            PlanName = "Plan A",
            UserId = "User1",
            User = new IdentityUser { Id = "User1", UserName = "TestUser" },
            ExerciseSets = new List<ExerciseSet>(),
            ScheduledTime = new List<SchedulePlans>()
        };
        context.WorkoutPlans.Add(workoutPlan);
        await context.SaveChangesAsync();

        // Fake UserManager
        var mockUserManager = A.Fake<UserManager<IdentityUser>>();
        var fakeUser = new IdentityUser { Id = "User1", UserName = "TestUser" };
        A.CallTo(() => mockUserManager.GetUserAsync(A<ClaimsPrincipal>.Ignored))
            .Returns(Task.FromResult(fakeUser));

        // Fake CacheService
        var mockCacheService = A.Fake<IRedisCacheService>();
        A.CallTo(() => mockCacheService.GetCacheAsync<List<WorkoutPlanResponse>>(A<string>.Ignored))
            .Returns(Task.FromResult<List<WorkoutPlanResponse>>(null)); // Simulating no cache hit

        // Fake Logger
        var mockLogger = A.Fake<ILogger<WorkoutPlansController>>();

        var controller = new WorkoutPlansController(context, mockUserManager, mockCacheService, mockLogger);

        // Act
        var result = await controller.GetWorkoutPlans();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<IEnumerable<WorkoutPlanResponse>>(okResult.Value);

        
        // Verifying the response
        Assert.Single(response);  // We expect one response due to the single fake WorkoutPlan
        var planResponse = response.First();
        var planResponseList = response.ToList();

        Assert.Equal("Plan A", planResponse.PlanName);
        Assert.Equal("TestUser", planResponse.User);

        foreach (var item in planResponseList)
        {
            _testOutputHelper.WriteLine($"Plan Name: {item.PlanName}, User: {item.User}");
        }

        // Output to check what's returned
        _testOutputHelper.WriteLine($"Plan Name: {planResponse.PlanName}, User: {planResponse.User}");
    }

}
