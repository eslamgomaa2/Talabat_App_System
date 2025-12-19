using Domin.DTOS.Auth_DTO;
using Domin.Enum;
using Domin.Helper;
using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repository.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _dbContext;

        public AccountServices(
            UserManager<ApplicationUser> userManager,
            IOptions<Jwt> jwt,
            IEmailService emailService,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        
        public async Task<AuthenticationResponse> Login(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password!))
                return new AuthenticationResponse { Message = "Email or Password is not correct" };

            var jwtToken = await GenerateJwtSecurityToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Roles = roles.ToList()
            };
        }

        private async Task<JwtSecurityToken> GenerateJwtSecurityToken(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var userroles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            }
            .Union(userclaims)
            .Union(userroles.Select(role => new Claim("roles", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: creds
            );
        }
      

        
        public async Task<AuthenticationResponse> RegisterUserAscustomer(Register register)
            => await RegisterUserAs(register, Roles.Customer);

        public async Task<AuthenticationResponse> RegisterUserAsDriver(Register register)
            => await RegisterUserAs(register, Roles.Driver);

        public async Task<AuthenticationResponse> RegisterUserAsRestaurantOwner(Register register)
            => await RegisterUserAs(register, Roles.RestaurantOwner);

        public async Task<AuthenticationResponse> RegisterUserAs(Register register, Roles role)
        {
            var validation = ValidateUserInput(register);
            if (!validation.IsAuthenticated)
                return validation;

            var exists = await CheckUserExist(register.Email!, register.UserName!);
            if (!exists.IsAuthenticated)
                return exists;

            var newUser = CreateApplicationUser(register);
            var result = await _userManager.CreateAsync(newUser, register.Password!);

            if (!result.Succeeded)
                return new AuthenticationResponse { Message = string.Join(" ", result.Errors.Select(e => e.Description)) };

             await _userManager.AddToRoleAsync(newUser, role.ToString());

            
            if (role == Roles.Driver)
            {
                var driver = new Driver
                {
                    Name = $"{register.FName} {register.LName}",
                    PhoneNumber = register.PhoneNUmber,
                    VehicleRegistration = register.VehicleRegistration,
                    VehicleType = register.VehicleType,
                    CreatedAt = DateTime.UtcNow
                    
                };
                await _dbContext.Drivers.AddAsync(driver);
            }
            else if (role == Roles.RestaurantOwner)
            {
                var owner = new Resaurant_Owner
                {
                    FName = register.FName,
                    LName = register.LName,
                    Email = register.Email,
                    Phone_Numbber = register.UserName,
                    
                };
                await _dbContext.Resaurant_Owners.AddAsync(owner);
            }

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                Message = "User created successfully",
                IsAuthenticated = true
            };
        }

        private AuthenticationResponse ValidateUserInput(Register request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.FName)) errors.Add("First Name is required");
            if (string.IsNullOrWhiteSpace(request.LName)) errors.Add("Last Name is required");
            if (string.IsNullOrWhiteSpace(request.UserName)) errors.Add("UserName is required");
            if (string.IsNullOrWhiteSpace(request.Email)) errors.Add("Email is required");
            if (string.IsNullOrWhiteSpace(request.PhoneNUmber)) errors.Add("PhoneNumber is required");
            if (string.IsNullOrWhiteSpace(request.Password)) errors.Add("Password is required");

            return errors.Any()
                ? new AuthenticationResponse { Message = string.Join(", ", errors) }
                : new AuthenticationResponse { IsAuthenticated = true };
        }

        private async Task<AuthenticationResponse> CheckUserExist(string email, string username)
        {
            var emailExists = await _userManager.FindByEmailAsync(email);
            var usernameExists = await _userManager.FindByNameAsync(username);

            if (emailExists != null || usernameExists != null)
                return new AuthenticationResponse { Message = $"Username '{username}' or Email '{email}' already exists" };

            return new AuthenticationResponse { Message = "User can register", IsAuthenticated = true };
        }

        private ApplicationUser CreateApplicationUser(Register request)
        {
            return new ApplicationUser
            {
                FName = request.FName,
                LName = request.LName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNUmber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
        }
       

        #region Password Management
        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null) throw new Exception("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendEmail(
                request.Email!,
                $"Reset token is {token}",
                "Reset password"
            );
        }

        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null) return new AuthenticationResponse { Message = "User not found" };

            var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);
            return result.Succeeded
                ? new AuthenticationResponse { Message = "Password changed successfully" }
                : new AuthenticationResponse { Message = "Failed to reset password" };
        }
        #endregion
    }
}
