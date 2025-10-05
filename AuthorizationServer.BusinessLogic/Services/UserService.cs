using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Context;
using AuthorizationServer.DataAccess.Dtos;
using AuthorizationServer.DataAccess.Entities;
using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    private readonly IPasswordService _passwordService;

    public UserService(AppDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }
    
    public async Task<Result<UserDto>> CreateUser(UserDto dto)
    {
        var isUserExists = await _context.Users.AnyAsync(u => u.Name == dto.Name);
        if (isUserExists)
            return Result.Fail(new Error("Пользователь с таким именем уже существует"));
        
        var user = new User()
        {
            Name = dto.Name,
            PasswordHash = _passwordService.Hash(dto.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var userDto = user.Adapt<UserDto>();
        
        return Result.Ok(userDto);
    }

    public async Task<Result<UserDto>> GetUserById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) 
            return Result.Fail(new Error("Пользователь не найден"));

        var userDto = user.Adapt<UserDto>();
        
        return Result.Ok(userDto);
    }

    public async Task<Result<List<UserDto>>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        var usersDtoList = users.Adapt<List<UserDto>>();
        return Result.Ok(usersDtoList);
    }

    public async Task<Result<UserDto>> UpdateUser(UserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
        if (user is null) 
            return Result.Fail(new Error("Пользователь не найден"));

        if (dto.Name is not null)
            user.Name = dto.Name;

        if (dto.Password is not null)
            user.PasswordHash = _passwordService.Hash(dto.Password);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var userDto = user.Adapt<UserDto>();
        
        return Result.Ok(userDto);
    }

    public async Task<Result> DeleteUser(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return Result.Fail(new Error("Пользователь не найден"));
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return Result.Ok();
    }
}