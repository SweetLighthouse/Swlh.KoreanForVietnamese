using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Application.Dtos.AuthenticationDtos;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Application.Services;

public class AccountService(MainDbContext context)
{
    public async Task<bool> AddAccountAsync(RegisterDto dto)
    {
        if (await context.Accounts.AnyAsync(a => a.Username == dto.Username)) return false;
        var account = new Account
        {
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Email = dto.Email,
            Role = Role.User
        };
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Account?> FindAsync(LoginDto dto)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Username == dto.UsernameOrEmail || a.Email == dto.UsernameOrEmail);
        if (account == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.Password)) return null;
        return account;
    }
}
