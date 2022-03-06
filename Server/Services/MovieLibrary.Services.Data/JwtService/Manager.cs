namespace MovieLibrary.Services.Data.JwtService
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;

    public class Manager
    {
        private readonly IRepository<RefreshToken> refreshTokenRepository;
        private readonly IRepository<ApplicationUser> appUserRepository;
        private readonly ILogger<Manager> logger;

        private readonly JwtAuthService jwtAuthService;
        private readonly JwtTokenConfig jwtTokenConfig;

        public Manager(
            ILogger<Manager> logger,
            JwtAuthService jwtAuthService,
            JwtTokenConfig jwtTokenConfig,
            IRepository<RefreshToken> refreshTokenRepository,
            IRepository<ApplicationUser> appUserRepository)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.appUserRepository = appUserRepository;
            this.logger = logger;
            this.jwtAuthService = jwtAuthService;
            this.jwtTokenConfig = jwtTokenConfig;
        }

        public static string ComputeSha256Hash(string password)
        {
            // Create a SHA256
            using SHA256 sha256Hash = SHA256.Create();

            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            this.logger.LogInformation($"Validating user [{userName}]", userName);

            SignInResult result = new();

            if (string.IsNullOrWhiteSpace(userName))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return result;
            }

            var user = this.appUserRepository.All().Where(f => f.UserName == userName && f.PasswordHash == ComputeSha256Hash(password)).Select(x => new ApplicationUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Gender = x.Gender,
                UserName = x.UserName,
                Roles = x.Roles.Select(x => new IdentityUserRole<string>
                {
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                }).ToList(),
                Claims = x.Claims.Select(x => new IdentityUserClaim<string>
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                }).ToList(),
                Logins = x.Logins.Select(x => new IdentityUserLogin<string>
                {
                    UserId = x.UserId,
                    LoginProvider = x.LoginProvider,
                    ProviderDisplayName = x.ProviderDisplayName,
                    ProviderKey = x.ProviderKey,
                }).ToList(),
                Movies = x.Movies.Select(x => new Movie()
                {
                    Title = x.Title,
                    Lenght = x.Lenght,
                    Id = x.Id,
                    Description = x.Description,
                    Year = x.Year,
                    Categories = x.Categories,
                    ImageUrl = x.ImageUrl,
                    MovieNotes = x.MovieNotes.Select(x => new MovieNote()
                    {
                        Id = x.Id,
                        Note = x.Note,
                        UserId = x.UserId,
                    }).ToList(),
                    Votes = x.Votes,
                }).ToList(),
                Votes = x.Votes.Select(x => new Vote()
                {
                    Id = x.Id,
                    Value = x.Value,
                    MovieId = x.MovieId,
                    Movie = x.Movie,
                }).ToList(),
            }).FirstOrDefault();

            if (user != null)
            {
                var claims = this.BuildClaims(user);
                result.User = user;
                result.AccessToken = this.jwtAuthService.BuildToken(claims);
                result.RefreshToken = this.jwtAuthService.BuildRefreshToken();

                await this.refreshTokenRepository.AddAsync(
                     new RefreshToken
                     {
                         UserId = user.Id,
                         Token = result.RefreshToken,
                         IssuedAt = DateTime.Now,
                         ExpiresAt = DateTime.Now.AddMinutes(this.jwtTokenConfig.RefreshTokenExpiration),
                     });

                await this.refreshTokenRepository.SaveChangesAsync();

                result.Success = true;
            }

            return result;
        }

        public async Task<SignInResult> RefreshToken(string accessToken, string refreshToken)
        {
            ClaimsPrincipal claimsPrincipal = this.jwtAuthService.GetPrincipalFromToken(accessToken);
            SignInResult result = new();

            if (claimsPrincipal == null)
            {
                return result;
            }

            string id = claimsPrincipal.Claims.First(c => c.Type == "id").Value;
            var user = this.appUserRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (user == null)
            {
                return result;
            }

            var token = await this.refreshTokenRepository.All()
                    .Where(f => f.UserId == user.Id
                            && f.Token == refreshToken
                            && f.ExpiresAt >= DateTime.Now)
                    .FirstOrDefaultAsync();

            if (token == null)
            {
                return result;
            }

            var claims = this.BuildClaims(user);

            result.User = user;
            result.AccessToken = this.jwtAuthService.BuildToken(claims);
            result.RefreshToken = this.jwtAuthService.BuildRefreshToken();

            this.refreshTokenRepository.Delete(token);
            await this.refreshTokenRepository.AddAsync(
                 new RefreshToken
                 {
                     UserId = user.Id,
                     Token = result.RefreshToken,
                     IssuedAt = DateTime.Now,
                     ExpiresAt = DateTime.Now.AddMinutes(this.jwtTokenConfig.RefreshTokenExpiration),
                 });

            await this.refreshTokenRepository.SaveChangesAsync();

            result.Success = true;

            return result;
        }

        private Claim[] BuildClaims(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

            return claims;
        }
    }
}
