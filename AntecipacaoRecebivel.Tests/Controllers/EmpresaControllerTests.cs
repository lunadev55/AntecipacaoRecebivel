using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AntecipacaoRecebivel.Tests.Controllers;
public class EmpresaControllerTests
{
    private readonly Mock<IEmpresaService> _mockEmpresaService;
    private readonly Mock<ILogger<EmpresaController>> _mockLogger;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _mockEmpresaService = new Mock<IEmpresaService>();
        _mockLogger = new Mock<ILogger<EmpresaController>>();
        _controller = new EmpresaController(_mockEmpresaService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateEmpresa_ReturnsCreated_WhenValidRequest()
    {
        // Arrange
        var empresaDto = new EmpresaDto
        {
            Nome = "Empresa Teste",
            Cnpj = "12345678000199",
            FaturamentoMensal = 50000,
            RamoAtuacao = "Products"
        };

        var empresaId = Guid.NewGuid();
        _mockEmpresaService.Setup(s => s.CreateEmpresa(It.IsAny<EmpresaDto>()))
            .ReturnsAsync(empresaId);

        // Act
        var result = await _controller.CreateEmpresa(empresaDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(_controller.CreateEmpresa), createdResult.ActionName);
        Assert.Equal(empresaId, createdResult.Value);
    }

    [Fact]
    public async Task CreateEmpresa_ReturnsBadRequest_WhenModelStateInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

        var empresaDto = new EmpresaDto
        {
            Cnpj = "12345678000199",
            FaturamentoMensal = 50000,
            RamoAtuacao = "Products"
        };

        // Act
        var result = await _controller.CreateEmpresa(empresaDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetEmpresaById_ReturnsOk_WhenEmpresaExists()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        var empresaDto = new EmpresaDto
        {
            Nome = "Empresa Teste",
            Cnpj = "12345678000199",
            FaturamentoMensal = 50000,
            RamoAtuacao = "Products"
        };

        _mockEmpresaService.Setup(s => s.GetEmpresaById(empresaId))
            .ReturnsAsync(empresaDto);

        // Act
        var result = await _controller.GetEmpresaById(empresaId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(empresaDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmpresaById_ReturnsNotFound_WhenEmpresaDoesNotExist()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        _mockEmpresaService.Setup(s => s.GetEmpresaById(empresaId))
            .ReturnsAsync((EmpresaDto)null);

        // Act
        var result = await _controller.GetEmpresaById(empresaId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}
