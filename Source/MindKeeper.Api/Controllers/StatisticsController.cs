using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Statistics;
using MindKeeper.Domain.EntitiesComposed;
using MindKeeper.Shared.Models.ApiModels.Statistics;
using MindKeeper.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticsService _statisticsService;
        private readonly IMapper _mapper;

        public StatisticsController(
            ILogger<StatisticsController> logger,
            IStatisticsService statisticsService,
            IMapper mapper)
        {
            _logger = logger;
            _statisticsService = statisticsService;
            _mapper = mapper;
        }

        [HttpGet("Achievements")]
        [ResponseCache(Duration = 5)]
        public async Task<Response<AchievementsResult>> Achievements()
        {
            var achievements = await _statisticsService.GetAchievements();
            var result = _mapper.Map<List<AchievementsResult.Achivement>>(achievements);

            var response = new Response<AchievementsResult>(
                new AchievementsResult
                {
                    Achivements = result
                }
            );

            return response;
        }

        [HttpGet("Achievements/User/")]
        public async Task<Response<AchievementsResult>> AchievementsByUser([FromQuery] Guid? userId = null)
        {
            var id = userId ?? User.GetUserId();
            
            var achievements = await _statisticsService.GetAchievements(id);
            var result = _mapper.Map<List<AchievementsResult.Achivement>>(achievements);

            var response = new Response<AchievementsResult>(
                new AchievementsResult
                {
                    Achivements = result
                }
            );

            return response;
        }

        [HttpGet("Stats/User/")]
        public async Task<Response<StatsUserResult>> StatsByUser([FromQuery] Guid? userId = null)
        {
            var id = userId ?? User.GetUserId();
            StatsUser stats = await _statisticsService.GetUserStats(id);

            var result = _mapper.Map<StatsUserResult>(stats);

            var response = new Response<StatsUserResult>(result);
            return response;
        }

        [HttpGet("Stats/System")]
        [ResponseCache(Duration = 5)]
        public async Task<Response<StatsSystemResult>> StatsBySystem()
        {
            StatsSystem stats = await _statisticsService.GetSystemStats();
            var result = _mapper.Map<StatsSystemResult>(stats);

            var response = new Response<StatsSystemResult>(result);
            return response;
        }
    }
}
