using AuthorizationServer.BusinessLogic.Services;
using FluentAssertions;

namespace AuthorizationServer.Tests.Unit.Services;

public class PasswordServiceTests
{
    [Fact]
    public void Hash_ShouldReturnDifferentHashForSamePassword()
    {
        // Arrange
        const string password = "password";
        var passwordService = new PasswordService();
        
        // Act
        var password1 = passwordService.Hash(password);
        var password2 = passwordService.Hash(password);
        
        // Assert
        password1.Should().NotBe(password2);
    }

    [Fact]
    public void Verify_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        const string password = "password";
        var passwordService = new PasswordService();
        var hashedPassword = passwordService.Hash(password);
        
        // Act
        var result = passwordService.Verify(password, hashedPassword);
        
        // Assert
        result.Should().BeTrue();
    }
}