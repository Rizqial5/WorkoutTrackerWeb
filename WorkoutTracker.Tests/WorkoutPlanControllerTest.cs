using System.Security.Claims;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkoutTracker.Backend.Controllers;
using WorkoutTracker.Backend.Models;
using WorkoutTracker.Backend.Services.Interfaces;

namespace WorkoutTracker.Tests
{
    public class WorkoutPlanControllerTest
    {
        private readonly WorkoutPlansController _controller;
        private readonly ApplicationDbContext _fakeContext;
        private readonly UserManager<IdentityUser> _fakeUserManager;
        private readonly IRedisCacheService _fakeCacheService;
        private readonly ILogger<WorkoutPlansController> _fakeLogger;

        public WorkoutPlanControllerTest()
        {
            // Create mocks using FakeItEasy
            _fakeContext = A.Fake<ApplicationDbContext>();
            _fakeUserManager = A.Fake<UserManager<IdentityUser>>();
            _fakeCacheService = A.Fake<IRedisCacheService>();
            _fakeLogger = A.Fake<ILogger<WorkoutPlansController>>();
            
            // Instantiate controller
            _controller = new WorkoutPlansController(_fakeContext, _fakeUserManager, _fakeCacheService, _fakeLogger);

            // Mock HTTP Context with User Claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "TestUserId") }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

    }
}
