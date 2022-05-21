﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RandomBooks.Data.Models;
using RandomBooks.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RandomBooks.API.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly DataContext _ctx;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(DataContext ctx, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
        }
        else if (!VerifyPasswordHash(password, user.Password))
        {
            response.Success = false;
            response.Message = "Wrong password.";
        }
        else response.Data = CreateToken(user);

        return response;
    }

    public async Task<ServiceResponse<string>> Register(User user, string password)
    {
        if (await UserExists(user.Username))
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "User already exists."
            };

        CreatePasswordHash(password, out byte[] passwordHash);

        user.Password = passwordHash;

        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();

        return new ServiceResponse<string> { Data = user.Username, Message = "Registration successfull." };
    }

    public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
    {
        var user = await _ctx.Users.FindAsync(userId);
        if (user == null)
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "User not found."
            };

        CreatePasswordHash(newPassword, out byte[] passwordHash);

        user.Password = passwordHash;

        await _ctx.SaveChangesAsync();

        return new ServiceResponse<bool>
        {
            Data = true,
            Message = "Password has been changed."
        };
    }

    #region Helper Methods
    #region Password Hashing
    private void CreatePasswordHash(string password, out byte[] passwordHash)
    {
        using var hmac = new HMACSHA512(new byte[16]);
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash)
    {
        using var hmac = new HMACSHA512(new byte[16]);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
    #endregion

    public async Task<bool> UserExists(string username)
    {
        if (await _ctx.Users.AnyAsync(user => user.Username.Equals(username)))
            return true;

        return false;
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JWTToken").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    #region Get User information
    public int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public string GetUserUsername() =>
        _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

    public async Task<User> GetUserByUsername(string username) =>
        await _ctx.Users.FirstOrDefaultAsync(x => x.Username.Equals(username));
    #endregion
    #endregion
}
