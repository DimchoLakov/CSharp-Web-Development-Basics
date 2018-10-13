using System;
using System.Collections.Generic;
using System.Linq;
using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Interfaces;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Interfaces;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        private IHashService hashService;
        private IRunesDbContext dbContext;

        public UserController()
        {
            this.hashService = new HashService();
            this.dbContext = new IRunesDbContext();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return View("Login", request);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return View("Register", request);
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            var user = this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return View("Login", request);
            }

            var view = this.View("Index", request);

            this.SignInUser(username, view, request);

            return View("Index", request);
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(email) || password != confirmPassword)
            {
                return View("/users/register", request);
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            var user = new User()
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.dbContext.Users.Add(user);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestError(e.Message, request);
            }

            return View("Index", request);
        }
    }
}
