using AntecipacaoRecebivel.Domain.Entities.Auth;
using AntecipacaoRecebivel.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AntecipacaoRecebivel.Tests.Controllers;
public class AuthControllerTests
{
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<AuthController>> _mockLogger;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            _mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            null, null, null, null
        );
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<AuthController>>();

        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("supersecretkey123456");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("testIssuer");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("testAudience");

        _controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockConfiguration.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Registrar_ReturnsOk_WhenUserCreatedSuccessfully()
    {
        // Arrange
        var request = new RegisterRequest { Username = "testuser", Password = "Password123!" };
        var identityUser = new IdentityUser { UserName = request.Username };

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Registrar(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Contains("Usuário registrado com sucesso", okResult.Value.ToString());
    }

    [Fact]
    public async Task Registrar_ReturnsBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var request = new RegisterRequest { Username = "testuser", Password = "Password123!" };
        var identityErrors = new IdentityError[] { new IdentityError { Description = "Invalid username" } };

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        var result = await _controller.Registrar(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }   

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequest { Username = "unknownuser", Password = "Password123!" };

        _mockUserManager.Setup(u => u.FindByNameAsync(request.Username))
            .ReturnsAsync((IdentityUser)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var request = new LoginRequest { Username = "testuser", Password = "WrongPassword" };
        var identityUser = new IdentityUser { UserName = request.Username };

        _mockUserManager.Setup(u => u.FindByNameAsync(request.Username))
            .ReturnsAsync(identityUser);

        _mockSignInManager.Setup(s => s.PasswordSignInAsync(identityUser, request.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorizedResult.StatusCode);
    }
}
