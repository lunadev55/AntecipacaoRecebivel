using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AntecipacaoRecebivel.Tests.Controllers;
public class NotaFiscalControllerTests
{
    private readonly Mock<INotaFiscalService> _mockNotaFiscalService;
    private readonly Mock<ILogger<NotaFiscalController>> _mockLogger;
    private readonly NotaFiscalController _controller;

    public NotaFiscalControllerTests()
    {
        _mockNotaFiscalService = new Mock<INotaFiscalService>();
        _mockLogger = new Mock<ILogger<NotaFiscalController>>();
        _controller = new NotaFiscalController(_mockNotaFiscalService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenValidRequest()
    {
        // Arrange
        var notaFiscalDto = new NotaFiscalDto
        {
            Numero = 123,
            EmpresaId = Guid.NewGuid(),
            Valor = 10000,
            DataVencimento = DateTime.UtcNow.AddDays(30)
        };

        var notaFiscalId = Guid.NewGuid(); 
        _mockNotaFiscalService.Setup(s => s.CreateNotaFiscal(It.IsAny<NotaFiscalDto>()))
            .ReturnsAsync(notaFiscalId);        

        // Act
        var result = await _controller.Create(notaFiscalDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode); 
        Assert.Equal(nameof(_controller.Create), result.ActionName);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var notaFiscalDto = new NotaFiscalDto();
        _controller.ModelState.AddModelError("Numero", "Required");

        // Act
        var result = await _controller.Create(notaFiscalDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode); 
    }

    [Fact]
    public async Task Create_ReturnsInternalServerError_OnException()
    {
        // Arrange
        var notaFiscalDto = new NotaFiscalDto
        {
            Numero = 123,
            EmpresaId = Guid.NewGuid(),
            Valor = 10000,
            DataVencimento = DateTime.UtcNow.AddDays(30)
        };

        _mockNotaFiscalService.Setup(s => s.CreateNotaFiscal(notaFiscalDto))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(notaFiscalDto) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Ocorreu um erro na requisicao.", result.Value);
    }

    [Fact]
    public async Task GetNotasFiscaisByEmpresaId_ReturnsOk_WithNotasFiscais()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        var notasFiscais = new List<NotaFiscalDto>
        {
            new NotaFiscalDto { Numero = 123, EmpresaId = empresaId, Valor = 10000, DataVencimento = DateTime.UtcNow.AddDays(30) },
            new NotaFiscalDto { Numero = 456, EmpresaId = empresaId, Valor = 20000, DataVencimento = DateTime.UtcNow.AddDays(60) }
        };

        _mockNotaFiscalService.Setup(s => s.GetNotasFiscaisByEmpresaId(empresaId))
            .ReturnsAsync(notasFiscais);

        // Act
        var result = await _controller.GetNotasFiscaisByEmpresaId(empresaId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode); 
        var returnedNotas = Assert.IsType<List<NotaFiscalDto>>(result.Value);
        Assert.Equal(2, returnedNotas.Count);
    }
}
