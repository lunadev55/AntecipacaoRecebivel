using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecipacaoRecebivel.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly ILogger<EmpresaController> _logger;

        public EmpresaController(IEmpresaService empresaService, ILogger<EmpresaController> logger)
        {
            _empresaService = empresaService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpresa([FromBody] EmpresaDto empresaDto)
        {
            _logger.LogInformation("Recebeu solicitação para criar uma nova empresa: {Nome}", empresaDto.Nome);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Modelo de solicitação inválido para criação de empresa.");
                return BadRequest(ModelState);
            }

            try
            {
                var empresaId = await _empresaService.CreateEmpresa(empresaDto);                
                _logger.LogInformation("Empresa {Nome} criada com sucesso.", empresaDto.Nome);                
                return CreatedAtAction(nameof(CreateEmpresa), new { id = empresaId }, empresaId);                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro na criacao da empresa: {0}", empresaDto.Nome);
                return StatusCode(500, "Ocorreu um erro na request.");
            }         
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmpresaById(Guid id)
        {
            _logger.LogInformation("Recebeu solicitação para buscar empresa com ID: {0}", id);

            var empresa = await _empresaService.GetEmpresaById(id);          
            if (empresa == null)
            {
                _logger.LogWarning("Empresa com Id {0} nao encontrada.", id);
                return NotFound("Empresa não encontrada!");
            }

            _logger.LogError("Empresa {Nome} encontrada com sucesso.", empresa.Nome);
            return Ok(empresa);
        }
    }
}