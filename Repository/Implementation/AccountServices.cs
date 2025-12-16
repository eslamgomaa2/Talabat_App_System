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
        private readonly ApplicationDbContext _DbContext;
        public AccountServices(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt, IEmailService emailService, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailService = emailService;
            _DbContext = applicationDbContext;
        }

        public async Task<AuthenticationResponse> Login(AuthenticationRequest request)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var User = await _userManager.FindByEmailAsync(request.Email!);
            if (User is null || !await _userManager.CheckPasswordAsync(User, request.Password!))
            {
                response.Message = "Email or Password is not Correct";
                return response;
            }
            JwtSecurityToken Jwt = await GenerateJwtSecurityToken(User);
            response.Id = User.Id;
            response.UserName = User.UserName;
            response.Email = User.Email;
            response.IsAuthenticated = true;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(Jwt);
            IList<string> roleslist = await _userManager.GetRolesAsync(User);
            response.Roles = roleslist.ToList();
            return response;

        }

        public async Task<AuthenticationResponse> RegisterUserAscustomer(Register register)

        {
            return await RegisterUserAs(register, Roles.Customer);


        }
        public async Task<AuthenticationResponse> RegisterUserAsDriver(Register register)

        {
           return await RegisterUserAs(register, Roles.Driver);



        }
        public async Task<AuthenticationResponse> RegisterUserAsRestaurantOwner(Register register)

        {
            return await RegisterUserAs(register, Roles.RestaurantOwner);


        }
        public async Task ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email!);
            if (user is null)
            {
                throw new Exception("User Not Found");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            //var random = new Random();
            //var numericToken = random.Next(100000, 999999).ToString();
            var mail = new Email
            {
                MailTo = forgotPasswordRequest.Email,
                Subject = $"Reset token is{token}",
                Body = $"Reset password"
            };
            await _emailService.SendEmail(mail.MailTo!, mail.Subject, mail.Body);

        }



        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var response = new AuthenticationResponse();
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email!);
            if (user is null)
            {
                response.Message = "user not found";
                return response;
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token!, resetPasswordRequest.Password!);
            if (result.Succeeded)
            {
                response.Message = "password is changed";
                return response;
            }
            response.Message = "there is an errro";
            return response;


        }
        private async Task<JwtSecurityToken> GenerateJwtSecurityToken(ApplicationUser User)
        {
            var userclaim = await _userManager.GetClaimsAsync(User);
            var roles = await _userManager.GetRolesAsync(User);
            var roleClaims = roles.Select(role => new Claim("roles", role));
            var claims = new[]
            {

        new Claim(ClaimTypes.NameIdentifier, User.Id),
        new Claim(ClaimTypes.Name, User.UserName!),
        new Claim(ClaimTypes.Email, User.Email!),

            }
            .Union(userclaim)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key!));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        public async Task<AuthenticationResponse> RegisterUserAs(Register register, Roles role)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var validInput = ValidUserInput(register);
            if (!validInput.IsAuthenticated)
            {
                response.Message = validInput.Message;
                return response;
            }
            var userexist = await CheckUserExist(register.Email!, register.UserName!);
            if (!userexist.IsAuthenticated) 
            {
                return userexist;
            }
            var newuser = CreateApplicationUSer(register);
            var result = await _userManager.CreateAsync(newuser, register.Password!);
            if (!result.Succeeded)
            {

                response.Message = string.Join(" ", result.Errors.Select(e => e.Description));
                return response;
            }

            await _userManager.AddToRoleAsync(newuser, role.ToString());
            if (role is Roles.Driver)
            {
                var driver = new Driver
                {
                    Name = $"{register.FName} {register.LName}",
                    PhoneNumber = register.PhoneNUmber,
                    VehicleRegistration = register.VehicleRegistration,
                    VehicleType = register.VehicleType,
                    CreatedAt = DateTime.UtcNow,
                };
                await _DbContext.Drivers.AddAsync(driver); 
                await _DbContext.SaveChangesAsync(); 
            }
            if (role is Roles.RestaurantOwner)
            {
                var owner = new Resaurant_Owner
                {
                    FName = register.FName,
                    LName = register.LName,
                    Email = register.Email,
                    Phone_Numbber= register.UserName,
                    UserId=newuser.Id,
                    
                };
                await _DbContext.Resaurant_Owners.AddAsync(owner);
                await _DbContext.SaveChangesAsync();
            }


            response.Message = "User Created Successfully";
            response.IsAuthenticated = true;

            return response;

        }

        private async Task<AuthenticationResponse> CheckUserExist(string email, string username)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            var useremailExist = await _userManager.FindByEmailAsync(email!);
            var usernameExist = await _userManager.FindByNameAsync(username!);
            if (useremailExist != null || usernameExist != null)
            {
                response.Message = $"Username{username} or email{email} is lAlready Exist";
                return response;
            }
            response.Message = "User is not Exist, you can register now";
            response.IsAuthenticated = true;
            return response;
        }
        private AuthenticationResponse ValidUserInput(Register request)
        {
            var errors = new List<string>();
            AuthenticationResponse response = new AuthenticationResponse();
            if (string.IsNullOrEmpty(request.FName))
            {
                errors.Add("First Name is required");

            }
            if (string.IsNullOrEmpty(request.LName))
            {
                errors.Add("Last Name is required");

            }
            if (string.IsNullOrEmpty(request.UserName))
            {
                errors.Add("UserName is required");

            }
            if (string.IsNullOrEmpty(request.Email))
            {
                errors.Add("Email is required");

            }
            if (string.IsNullOrEmpty(request.PhoneNUmber))
            {
                errors.Add("PhoneNumber is required");

            }
            if (string.IsNullOrEmpty(request.Password))
            {
                errors.Add("PassWord is required");

            }
            if (errors.Any())
            {
                response.Message = string.Join(", ", errors);
                return response;
            }
            return new AuthenticationResponse { IsAuthenticated = true };
        }
        private ApplicationUser CreateApplicationUSer(Register request)
        {
            return new ApplicationUser()
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
    }
}
