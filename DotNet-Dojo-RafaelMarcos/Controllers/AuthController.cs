using System;
using System.Collections.Generic;
using System.Linq;
using DotNet_Dojo_RafaelMarcos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNet_Dojo_RafaelMarcos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly User[] Users = new[]
        {
            new User(){ Name = "Rafael Trevizan", Email = "rafael.trevizan@mblabs.com.br", Password= "123123", Role = "Admin" },
            new User(){ Name = "Marcos Junior", Email = "marcos.vasconcellos@mblabs.com.br", Password= "123456", Role = "AppUser" },
        };

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public JWT.Token Auth([FromBody] UserDto user)
        {
            if (Users.Any(UserExist(user))) {
                return JWT.BuildUserResponse(Users.First(UserExist(user)));
            }

            return null;
        }
        
        [HttpGet]
        [Authorize]
        public IEnumerable<User> List()
        {
            return Users;
        }

        private static Func<User, bool> UserExist(UserDto user)
        {
            return x => x.Email == user.Email && x.Password == user.Password;
        }
    }
}
