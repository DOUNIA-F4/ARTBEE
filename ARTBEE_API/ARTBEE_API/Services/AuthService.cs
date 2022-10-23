using ARTBEE_API.Helper;
using ARTBEE_API.Interfaces;
using ARTBEE_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Myrmec;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ARTBEE_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Auth> SignUp(SignUp signUp, IFormFile image, string role)
        {
            User user = new();
            MemoryStream imageMs = new();
            List<string> imageResults = new();
            Sniffer sniffer = new();
            List<Record> supportedImages = new()
            {
                new Record("jpg,jpeg", "ff,d8,ff,db"),
                new Record("png", "89,50,4e,47,0d,0a,1a,0a"),
            };
            sniffer.Populate(supportedImages);

            if (await _userManager.FindByEmailAsync(signUp.Email) is not null || await _userManager.FindByNameAsync(signUp.UserName) is not null)
            {
                return new Auth { Message = "Email Already registered" };
            }

            if (image is null)
            {
                var defaultImg =
                    "Assets\\defualt-profile-photo.jpg";
                //user.ProfilePhoto = await File.ReadAllBytesAsync(defaultImg);
                user = new()
                {
                    UserName = signUp.UserName,
                    Email = signUp.Email,
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    PhoneNumber = signUp.Phone,
                    //Address = signUp.Address,
                    DoB = signUp.DoB,
                    ProfilePhoto = await File.ReadAllBytesAsync(defaultImg),
                    //Certificate = certificateMs.ToArray(),
                    Country = signUp.Country,
                    //Status = signUp.Status

                };
            }
            else
            {
                byte[] imageHead = FileHead.ReadFileHead(image);

                if (imageHead != null)
                {
                    imageResults = sniffer.Match(imageHead);
                }

                if (imageResults.Count == 0)
                {
                    return new Auth { Message = "Something went wrong" };
                }

                await image.CopyToAsync(imageMs);

                if (imageMs.Length < 1048576)
                {
                    user = new()
                    {
                        UserName = signUp.UserName,
                        //UserName = signUp.UserName,
                        Email = signUp.Email,
                        FirstName = signUp.FirstName,
                        LastName = signUp.LastName,
                        DoB = signUp.DoB,
                        //Address = signUp.Address,
                        Country = signUp.Country,
                        PhoneNumber = signUp.Phone,
                        ProfilePhoto = imageMs.ToArray(),
                        //Certificate = certificateMs.ToArray(),
                        //Status = signUp.Status
                    };
                }
                else
                {
                    return new Auth { Message = "Max size is 1 MB" };
                }
            }

            var data = await _userManager.CreateAsync(user, signUp.Password);

            if (!data.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in data.Errors)
                {
                    errors += ("  ", error.Description);

                }

                return new Auth { Message = errors };
            }

            //await _userManager.AddToRoleAsync(usr, "Patient");
            await _userManager.AddToRoleAsync(user, role);

            var jwtSecurityToken = await CreateJwtToken(user);

            return new Auth
            {
                Email = user.Email,
                Expire = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }

        public Task<Auth> Login(Login login)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UserToRoleAssign(UserToRole userRole)
        {
            var usr = await _userManager.FindByIdAsync(userRole.UserId);

            if (usr is null || !await _roleManager.RoleExistsAsync(userRole.roleName))
            {
                return "Something went wrong";
            }

            //return usr is null || !await _roleManager.RoleExistsAsync(userRole.roleName) ? "Something went wrong" : 

            if (await _userManager.IsInRoleAsync(usr, userRole.roleName))
            {
                return "the user is already assigned to this role";
            }

            var assigning = await _userManager.AddToRoleAsync(usr, userRole.roleName);

            return assigning.Succeeded ? string.Empty : "something wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            //var photo = Convert.ToBase64String(user.ProfilePhoto);

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id),
                    //new Claim("photo", photo)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
