using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecipacaoRecebivel.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _notaFiscalService;
        private readonly ILogger<NotaFiscalController> _logger;

        public NotaFiscalController(INotaFiscalService notaFiscalService, ILogger<NotaFiscalController> logger)
        {
            _notaFiscalService = notaFiscalService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotaFiscalDto notaFiscalDto)
        {
            _logger.LogInformation("Recebeu requisicao para criar uma nova Nota Fiscal Numero: {0}", notaFiscalDto.Numero);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Modelo Invalido para criacao de Nota Fiscal.");
                return BadRequest(ModelState);
            }

            try
            {
                var nF = await _notaFiscalService.GetNotaFiscalByNumero(notaFiscalDto.Numero);
                if (nF != null)
                {
                    _logger.LogInformation("Nota Fiscal Numero {0} já existe no sistema.", notaFiscalDto.Numero);
                    return StatusCode(409, "Já existe uma nota fiscal com esse número no sistema, por favor utilize um número diferente.");
                }
                var notaFiscalId = await _notaFiscalService.CreateNotaFiscal(notaFiscalDto);
                _logger.LogInformation("Nota Fiscal Numero {0} criada com sucesso.", notaFiscalDto.Numero);           
                return CreatedAtAction(nameof(Create), new { id = notaFiscalId }, notaFiscalDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro na criacao da Nota Fiscal numero: {0}", notaFiscalDto.Numero);
                return StatusCode(500, "Ocorreu um erro na requisicao.");
            }          
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetNotasFiscaisByEmpresaId(Guid empresaId)
        {
            _logger.LogInformation("Recebeu requisicao para buscar Notas Fisicas por EmpresaId: {0}", empresaId);           

            var notaFiscal = await _notaFiscalService.GetNotasFiscaisByEmpresaId(empresaId);       
            return Ok(notaFiscal);
        }
    }
}
