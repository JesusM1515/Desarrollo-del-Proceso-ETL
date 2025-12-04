using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtraerComentarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialCommentsController : ControllerBase
    {
        private readonly ISocialComments _socialCommentsService;
        private readonly ILogger<SocialCommentsController> _logger;

        public SocialCommentsController(ISocialComments socialCommentsService,
                                        ILogger<SocialCommentsController> logger)
        {
            _socialCommentsService = socialCommentsService;
            _logger = logger;
        }

        [HttpGet]
        [Route("allSocialComments")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Peticion para obtener todos los Social Comments");

            var result = await _socialCommentsService.GetAllSocialComments();

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Error al obtener Social Comments: {Message}", result.Message);
                return NotFound(new { message = result.Message });
            }

            _logger.LogInformation("Social Comments obtenidos correctamente");
            return Ok(result);
        }
    }
}
