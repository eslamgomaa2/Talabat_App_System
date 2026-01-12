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
using Google.Apis.Auth;

namespace Repository.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountServices(
            UserManager<ApplicationUser> userManager,
            IOptions<Jwt> jwt,
            IEmailService emailService,
            ApplicationDbContext dbContext,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailService = emailService;
            _dbContext = dbContext;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthenticationResponse> Login(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password!))
                return new AuthenticationResponse { Message = "Email or Password is not correct" };

            var jwtToken = await GenerateJwtSecurityToken(user);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.Id);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiryDate,
                Roles = roles.ToList()
            };
        }

        public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request)
        {
            var refreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken!);
            if (refreshToken == null)
                return new AuthenticationResponse { Message = "Invalid or expired refresh token" };

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user == null)
                return new AuthenticationResponse { Message = "User not found" };

            var jwtToken = await GenerateJwtSecurityToken(user);
            var newRefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.Id);

            // Revoke old token and replace with new one
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken.Token, newRefreshToken.Token);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiryDate,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> RevokeToken(string token)
        {
            var refreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(token);
            if (refreshToken == null)
                return false;

            await _refreshTokenService.RevokeRefreshTokenAsync(token);
            return true;
        }

            private async Task<JwtSecurityToken> GenerateJwtSecurityToken(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var userroles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        public async Task<AuthenticationResponse> LoginWithGoogle(ExternalLoginRequest request, Roles role = Roles.Customer)
        {
            try
            {
                
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.IdToken!,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _jwt.Audience }
                    });

                
                var user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    // Create new user from Google payload
                    user = new ApplicationUser
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FName = payload.GivenName ?? "",
                        LName = payload.FamilyName ?? "",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        return new AuthenticationResponse
                        {
                            Message = string.Join(" ", result.Errors.Select(e => e.Description))
                        };
                    }

                   
                    await _userManager.AddToRoleAsync(user, role.ToString());

                    
                    if (role == Roles.Driver)
                    {
                        var driver = new Driver
                        {
                            Name = $"{user.FName} {user.LName}",
                            PhoneNumber = user.PhoneNumber ?? "",
                            CreatedAt = DateTime.UtcNow
                        };
                        await _dbContext.Drivers.AddAsync(driver);
                    }
                    else if (role == Roles.RestaurantOwner)
                    {
                        var owner = new Resaurant_Owner
                        {
                            FName = user.FName,
                            LName = user.LName,
                            Email = user.Email,
                            Phone_Numbber = user.UserName
                        };
                        await _dbContext.Resaurant_Owners.AddAsync(owner);
                    }

                    await _dbContext.SaveChangesAsync();
                }

                
                var jwtToken = await GenerateJwtSecurityToken(user);
                var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(user);

                return new AuthenticationResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAuthenticated = true,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiryDate,
                    Roles = roles.ToList()
                };
            }
            catch (InvalidOperationException)
            {
                return new AuthenticationResponse { Message = "Invalid or expired Google token" };
            }
            catch (Exception ex)
            {
                return new AuthenticationResponse { Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
