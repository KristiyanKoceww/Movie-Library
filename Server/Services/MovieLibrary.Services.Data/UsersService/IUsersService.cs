namespace MovieLibrary.Services.Data.UsersService
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Models;

    public interface IUsersService
    {
        Task CreateAsync(CreateUserModel createUserModel);

        ApplicationUser GetById(string userId);

        ApplicationUser Authenticate(string username, string password);
    }
}
