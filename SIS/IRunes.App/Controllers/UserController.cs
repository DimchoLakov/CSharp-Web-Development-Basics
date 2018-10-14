using System;
using System.Collections.Generic;
using System.Linq;
using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        private IHashService hashService;
        private IRunesDbContext dbContext;
        private Dictionary<string, string> viewBag;

        public UserController()
        {
            this.hashService = new HashService();
            this.dbContext = new IRunesDbContext();
            this.viewBag = new Dictionary<string, string>();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var username = this.GetUsernameFromSession(request);
                this.viewBag.Add("Username", username);
                return View("Index", request, viewBag);
            }
            return View("Login", request);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return View("Index", request);
            }
            return View("Register", request);
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            if (this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var user = this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return View("Index", request);
            }

            var view = this.View("Index", request);

            this.SignInUser(username, view, request);
            
            viewBag.Add("Username", username);

            return View("Index", request, viewBag);
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

            return this.DoLogin(request);

            //return View("Index", request);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            this.LogoutUser(request);
            return View("Index", request);
        }
    }
}
