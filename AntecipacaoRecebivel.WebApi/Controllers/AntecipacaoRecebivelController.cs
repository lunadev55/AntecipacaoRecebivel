using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecipacaoRecebivel.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AntecipacaoRecebivelController : ControllerBase
    {        
        private readonly IAntecipacaoRecebivelService _antecipacaoRecebivelService;
        private readonly ILogger<AuthController> _logger;

        public AntecipacaoRecebivelController(IAntecipacaoRecebivelService antecipacaoRecebivelService, ILogger<AuthController> logger)
        {
            _antecipacaoRecebivelService = antecipacaoRecebivelService;
            _logger = logger;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalcularAntecipacaoDeRecebiveis([FromBody] CalculoCarrinhoRequisicaoDto requisicao)
        {
            _logger.LogInformation("Requisicao para calcular Carrinho de Notas da empresa: {0}", requisicao.EmpresaId);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Modelo Invalido para criacao de Empresa.");
                return BadRequest(ModelState);
            }                

            try
            {
                var result = await _antecipacaoRecebivelService.CalcularAntecipacao(requisicao);
                _logger.LogInformation("Antecipacao calculada com Sucesso.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao Calcular Antecipacao de Recebiveis da empresa {0}", requisicao.EmpresaId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
