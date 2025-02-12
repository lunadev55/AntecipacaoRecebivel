using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AntecipacaoRecebivel.Tests.Controllers;
public class AntecipacaoRecebivelControllerTests
{
    private readonly Mock<IAntecipacaoRecebivelService> _mockService;
    private readonly Mock<ILogger<AuthController>> _mockLogger;
    private readonly AntecipacaoRecebivelController _controller;

    public AntecipacaoRecebivelControllerTests()
    {
        _mockService = new Mock<IAntecipacaoRecebivelService>();
        _mockLogger = new Mock<ILogger<AuthController>>();
        _controller = new AntecipacaoRecebivelController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CalcularAntecipacaoDeRecebiveis_ReturnsBadRequest_WhenModelStateInvalid()
    {
        // Arrange
        var requisicao = new CalculoCarrinhoRequisicaoDto();
        _controller.ModelState.AddModelError("EmpresaId", "EmpresaId é obrigatório");

        // Act
        var result = await _controller.CalcularAntecipacaoDeRecebiveis(requisicao);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CalcularAntecipacaoDeRecebiveis_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var requisicao = new CalculoCarrinhoRequisicaoDto { EmpresaId = Guid.NewGuid() };

        _mockService.Setup(s => s.CalcularAntecipacao(requisicao))
            .ThrowsAsync(new Exception("Erro ao calcular antecipação"));

        // Act
        var result = await _controller.CalcularAntecipacaoDeRecebiveis(requisicao);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Contains("Erro ao calcular antecipação", badRequestResult.Value.ToString());
    }
}
