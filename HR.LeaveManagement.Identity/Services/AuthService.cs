﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HR.LeaveManagement.Identity.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IOptions<JwtSettings> _jwtSettings;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AuthService(UserManager<ApplicationUser> userManager,
			IOptions<JwtSettings> jwtSettings,
			SignInManager<ApplicationUser> signInManager)
        {
			this._userManager = userManager;
			this._jwtSettings = jwtSettings;
			this._signInManager = signInManager;
		}
        public async Task<AuthResponse> Login(AuthRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				throw new NotFoundException($"User with {request.Email} not found.", request.Email);
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
			if (result.Succeeded)
			{
				throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
			}

			JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
			var response = new AuthResponse
			{
				Id = user.Id,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				Email = request.Email,
				UserName = user.UserName
			};
			return response;
		}

		

		public async Task<RegistrationResponse> Register(RegistrationRequest request)
		{
			var user = new ApplicationUser
			{
				Email = request.Email,
				FirstName = request.FirstName,
				LastName = request.LastName,
				UserName = request.UserName,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "Employee");
				return new RegistrationResponse() {UserId = user.Id};
			}
			else
			{
				StringBuilder str = new StringBuilder();
				foreach (var err in result.Errors)
				{
					str.AppendFormat("•{0}\n", err.Description);
				}
				throw new BadRequestException($"{str}");
			}
		}
		private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);

			var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

			var Claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}
				.Union(userClaims)
				.Union(roleClaims);

			var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
			var signingCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Value.Issuer,
				audience: _jwtSettings.Value.Audience,
				claims: Claims,
				expires: DateTime.Now.AddMinutes(_jwtSettings.Value.DurationInMinutes),
				signingCredentials: signingCredentials);
			return jwtSecurityToken;
		}
	}


}
