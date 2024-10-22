using _2.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1.API.Controllers.Mantenimiento
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoController : ControllerBase
    {
        private readonly IMantenimientoApplication _mantenimientoApplication;
        public MantenimientoController(IMantenimientoApplication mantenimientoApplication)
        {
            _mantenimientoApplication = mantenimientoApplication;            
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Mantenimiento()
        {
            var response = await _mantenimientoApplication.GetEstatusMantenimiento();
            return StatusCode(response.CodigoEstatus,response);
        }
    }
}
