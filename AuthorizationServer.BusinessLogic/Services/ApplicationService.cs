using System.Security.Cryptography;
using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Context;
using AuthorizationServer.DataAccess.Dtos.Application;
using AuthorizationServer.DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationServer.BusinessLogic.Services;

public class ApplicationService : IApplicationService
{
    private readonly AppDbContext _context;

    private readonly IPasswordService _passwordService;
    
    public ApplicationService(AppDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }
    
    public async Task<Result<ApplicationDto>> CreateApplication(CreateApplicationDto dto)
    {
        // 1. Генерируем ClientId (из названия или UUID)
        var clientId = await GenerateClientId(dto.Name);
    
        // 2. Генерируем ClientSecret
        var clientSecret = GenerateClientSecret();
    
        // 3. Хешируем ClientSecret перед сохранением
        var hashedSecret = _passwordService.Hash(clientSecret);
    
        // 4. Создаем приложение
        var application = new Application
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ClientId = clientId,
            ClientSecret = hashedSecret,  // сохраняем хеш!
            RedirectUrls = dto.RedirectUrls,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
        };
    
        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
    
        // 5. ВАЖНО! Возвращаем НЕ хешированный ClientSecret ТОЛЬКО ОДИН РАЗ!
        return Result.Ok(new ApplicationDto
        {
            Id = application.Id,
            Name = application.Name,
            ClientId = clientId,
            ClientSecret = clientSecret,
            RedirectUrls = application.RedirectUrls,
            CreatedAt = application.CreatedAt,
        });
    }

    public async Task<Result<ApplicationDto>> GetApplicationById(Guid id)
    {
        var application = await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
        if (application is null)
            return Result.Fail(new Error("Приложение не найдено"));

        return Result.Ok(new ApplicationDto()
        {
            Id = application.Id,
            Name = application.Name,
            ClientId = application.ClientId,
            RedirectUrls = application.RedirectUrls,
            CreatedAt = application.CreatedAt,
        });
    }

    public async Task<Result<List<ApplicationDto>>> GetAllApplications()
    {
        var applications = await _context.Applications.ToListAsync();
        var dtos = new List<ApplicationDto>();
        foreach (var application in applications)
        {
            dtos.Add(new ApplicationDto()
            {
                Id = application.Id,
                Name = application.Name,
                ClientId = application.ClientId,
                RedirectUrls = application.RedirectUrls,
                CreatedAt = application.CreatedAt,
            });
        }

        return Result.Ok(dtos);
    }

    private async Task<string> GenerateClientId(string name)
    {
        var slug = name
            .ToLower()
            .Replace(" ", "-")
            .Replace("_", "-");
        
        var exists = await _context.Applications.AnyAsync(a => a.ClientId == slug);
    
        if (exists)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    
        return slug;
    }
    
    private string GenerateClientSecret()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "")
            .Substring(0, 48);
    }
}