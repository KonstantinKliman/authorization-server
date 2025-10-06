// using AuthorizationServer.BusinessLogic.Interfaces.Services;
// using AuthorizationServer.BusinessLogic.Services;
// using AuthorizationServer.DataAccess.Context;
// using AuthorizationServer.DataAccess.Dtos;
// using AuthorizationServer.DataAccess.Entities;
// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Moq;
//
// namespace AuthorizationServer.Tests.Unit.Services;
//
// public class UserServiceTests
// {
//     [Fact]
//     public async Task CreateUser_WhenUserDoesntExists_ShouldCreateUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         var userDto = new UserDto
//         {
//             Name = "TestUser",
//             Password = "password123"
//         };
//
//         // Act
//         var result = await userService.CreateUser(userDto);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//         result.Value.Name.Should().Be("TestUser");
//         result.Value.Password.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task CreateUser_WhenUserExists_ShouldNotCreateUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//         await context.Users.AddAsync(new User
//         {
//             Id = Guid.NewGuid(),
//             Name = "TestUser",
//             PasswordHash = "hashed_password",
//             CreatedAt = DateTime.UtcNow
//         });
//
//         await context.SaveChangesAsync();
//
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//         var userDto = new UserDto
//         {
//             Name = "TestUser",
//             Password = "password123"
//         };
//
//         // Act
//         var result = await userService.CreateUser(userDto);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.IsFailed.Should().BeTrue();
//     }
//
//     [Fact]
//     public async Task GetUserById_WhenUserExists_ShouldReturnUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//         var guid = Guid.NewGuid();
//         await context.Users.AddAsync(new User
//         {
//             Id = guid,
//             Name = "TestUser",
//             PasswordHash = "hashed_password",
//             CreatedAt = DateTime.UtcNow
//         });
//
//         await context.SaveChangesAsync();
//
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         // Act
//         var result = await userService.GetUserById(guid);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//         result.Value.Id.Should().Be(guid);
//         result.Value.Name.Should().Be("TestUser");
//         result.Value.Password.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task GetUserById_WhenUserNotExists_ShouldNotReturnUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//         var guid = Guid.NewGuid();
//
//         // Act
//         var result = await userService.GetUserById(guid);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.IsFailed.Should().BeTrue();
//         result.Errors.Should().NotBeEmpty();
//         result.Errors[0].Message.Should().Contain("не найден");
//     }
//
//     [Fact]
//     public async Task GetAllUsers_WhenUserExists_ShouldReturnUserList()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var users = new List<User>()
//         {
//             new()
//             {
//                 Id = Guid.NewGuid(), Name = "TestUser1", PasswordHash = "hashed_password", CreatedAt = DateTime.UtcNow
//             },
//             new()
//             {
//                 Id = Guid.NewGuid(), Name = "TestUser2", PasswordHash = "hashed_password", CreatedAt = DateTime.UtcNow
//             }
//         };
//
//         await context.Users.AddRangeAsync(users);
//         await context.SaveChangesAsync();
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         // Act
//         var result = await userService.GetAllUsers();
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//         result.Value.Should().BeOfType<List<UserDto>>();
//         result.Value.Should().HaveCount(2);
//         result.Value.Should().Contain(u => u.Name == "TestUser1");
//         result.Value.Should().Contain(u => u.Name == "TestUser2");
//     }
//
//     [Fact]
//     public async Task UpdateUser_WhenUserNotExists_ShouldNotReturnUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         var userDto = new UserDto()
//         {
//             Id = Guid.NewGuid(),
//             Name = "TestUserName",
//             Password = "new_hashed_password",
//         };
//
//         // Act
//         var result = await userService.UpdateUser(userDto);
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.Errors[0].Message.Should().Contain("не найден");
//     }
//
//     [Fact]
//     public async Task UpdateUser_WhenUserExistsAndDtoNameIsNull_ShouldNotUpdateName()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var guid = Guid.NewGuid();
//
//         var oldUser = new User()
//         {
//             Id = guid,
//             Name = "TestUser",
//             PasswordHash = "hashed_password"
//         };
//         
//         await context.Users.AddAsync(oldUser);
//         await context.SaveChangesAsync();
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         var userDto = new UserDto()
//         {
//             Id = guid,
//             Password = "new_hashed_password",
//         };
//         
//         // Act
//         var result = await userService.UpdateUser(userDto);
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.Value.Id.Should().Be(oldUser.Id);
//         result.Value.Name.Should().Be(oldUser.Name);
//     }
//
//     [Fact]
//     public async Task UpdateUser_WhenPasswordIsNull_ShouldNotUpdatePassword()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var guid = Guid.NewGuid();
//         var oldPasswordHash = "old_hashed_password";
//
//         var user = new User()
//         {
//             Id = guid,
//             Name = "TestUser",
//             PasswordHash = oldPasswordHash
//         };
//         
//         await context.Users.AddAsync(user);
//         await context.SaveChangesAsync();
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         var userDto = new UserDto()
//         {
//             Id = guid,
//             Name = "NewTestName",
//         };
//         
//         // Act
//         var result = await userService.UpdateUser(userDto);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//
//         var updatedUser = await context.Users.FindAsync(guid);
//         updatedUser.PasswordHash.Should().Be(oldPasswordHash);
//     }
//
//     [Fact]
//     public async Task UpdateUser_WhenNameAndPasswordIsNotNull_ShouldUpdateUser()
//     {
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//         
//         var guid = Guid.NewGuid();
//         var user = new User()
//         {
//             Id = guid,
//             Name = "TestUser",
//             PasswordHash = "hashed_password"
//         };
//         
//         await context.Users.AddAsync(user);
//         await context.SaveChangesAsync();
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("new_hashed_password");
//         
//         var userService = new UserService(context, mockPasswordService.Object);
//
//         var userDto = new UserDto()
//         {
//             Id = guid,
//             Name = "NewTestUserName",
//             Password = "new_hashed_password"
//         };
//         
//         // Act
//         var result = await userService.UpdateUser(userDto);
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//         
//         var updatedUser = await context.Users.FindAsync(guid);
//         updatedUser.Name.Should().Be("NewTestUserName");
//         updatedUser.PasswordHash.Should().Be("new_hashed_password");
//     }
//
//     [Fact]
//     public async Task DeleteUser_WhenUserNotExists_ShouldNotDeleteUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//         
//         var userService = new UserService(context, mockPasswordService.Object);
//         
//         // Act
//         var result = await userService.DeleteUser(Guid.NewGuid());
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.IsFailed.Should().BeTrue();
//         result.Errors.Should().NotBeEmpty();
//         result.Errors[0].Message.Should().Contain("не найден");
//     }
//     
//     [Fact]
//     public async Task DeleteUser_WhenUserExists_ShouldDeleteUser()
//     {
//         // Arrange
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;
//
//         using var context = new AppDbContext(options);
//
//         var guid = Guid.NewGuid();
//         var user = new User()
//         {
//             Id = guid,
//             Name = "TestUser",
//             PasswordHash = "hashed_password"
//         };
//
//         await context.Users.AddAsync(user);
//         await context.SaveChangesAsync();
//         
//         var mockPasswordService = new Mock<IPasswordService>();
//         mockPasswordService
//             .Setup(x => x.Hash(It.IsAny<string>()))
//             .Returns("hashed_password");
//         
//         var userService = new UserService(context, mockPasswordService.Object);
//         
//         // Act
//         var result = await userService.DeleteUser(guid);
//         
//         // Assert
//         result.Should().NotBeNull();
//         result.IsSuccess.Should().BeTrue();
//     }
// }