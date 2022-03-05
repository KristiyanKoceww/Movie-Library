using Microsoft.AspNetCore.Identity;
using MovieLibrary.Data.Common.Repositories;
using MovieLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services.Data.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> appUserRepository;
        private readonly IRepository<IdentityUserClaim<string>> claimRepository;

        public UsersService(
            IRepository<ApplicationUser> appUserRepository,
            IRepository<IdentityUserClaim<string>> claimRepository)
        {
            this.appUserRepository = appUserRepository;
            this.claimRepository = claimRepository;
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

        public async Task CreateAsync(CreateUserModel createUserModel)
        {
            var userName = this.appUserRepository.All().Where(x => x.UserName == createUserModel.UserName).FirstOrDefault();
            if (userName != null)
            {
                throw new Exception("Username already taken by another user.Please enter new username.");
            }

            if (string.IsNullOrWhiteSpace(createUserModel.Password) || string.IsNullOrWhiteSpace(createUserModel.UserName))
            {
                throw new Exception("Password and username are required");
            }

            var user = new ApplicationUser()
            {
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Email = createUserModel.Email,
                UserName = createUserModel.UserName,
                PasswordHash = ComputeSha256Hash(createUserModel.Password),
                Gender = createUserModel.Gender,
            };

            this.BuildClaims(user);

            await this.appUserRepository.AddAsync(user);
            await this.appUserRepository.SaveChangesAsync();
        }

        public ApplicationUser Authenticate(string username, string password)
        {
            var dbUser = this.appUserRepository.All().Where(x => x.UserName == username).FirstOrDefault();

            if (dbUser == null)
            {
                return null;
            }

            var userPassHash = ComputeSha256Hash(password);

            if (!(dbUser.UserName == username && dbUser.PasswordHash == userPassHash))
            {
                throw new Exception("Invalid username or password!");
            }

            return dbUser;
        }

        public ApplicationUser GetById(string userId)
        {
            var user = this.appUserRepository.All().Where(x => x.Id == userId).Select(x => new ApplicationUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Gender = x.Gender,
                UserName = x.UserName,
                Movies = x.Movies.Select(x => new Movie()
                {
                    Categories = x.Categories,
                    ImageUrl = x.ImageUrl,
                    MovieNotes = x.MovieNotes,
                    Title = x.Title,
                    Year = x.Year,
                    Votes = x.Votes,
                    Id = x.Id,
                }).ToList(),
            }).FirstOrDefault();

            if (user is not null)
            {
                return user;
            }
            else
            {
                throw new Exception("No user found  by this id");
            }
        }

        private void BuildClaims(ApplicationUser user)
        {
            var claim = new IdentityUserClaim<string>
            {
                UserId = user.Id,
                ClaimType = "NameIdentifier",
                ClaimValue = user.UserName,
            };

            user.Claims.Add(claim);
            this.claimRepository.AddAsync(claim);

            this.appUserRepository.SaveChangesAsync();
            this.claimRepository.SaveChangesAsync();
        }
    }
}
