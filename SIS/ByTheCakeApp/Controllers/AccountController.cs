using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByTheCake.App.Services;
using ByTheCake.App.Services.Interfaces;
using ByTheCakeApp.Data;
using ByTheCakeApp.Models;
using SIS.HTTP.Cookies;
using SIS.HTTP.Requests.Interfaces;
using SIS.HTTP.Responses.Interfaces;
using SIS.WebServer.Results;

namespace ByTheCake.App.Controllers
{
    public class AccountController : BaseController
    {
        private ByTheCakeDbContext dbContext;
        private IHashService hashService;

        public AccountController()
        {
            this.dbContext = new ByTheCakeDbContext();
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return View("Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Username length cannot be less than 4 characters!");
            }

            if (this.dbContext.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError("User with the same name already exists!");
            }

            if (password.Length < 6)
            {
                return this.BadRequestError("Password length cannot be less than 6 characters!");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match!");
            }

            // Hash Password
            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            // Create User
            var user = new User()
            {
                Name = username,
                Username = username,
                Password = hashedPassword
            };

            this.dbContext.Users.Add(user);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            // Login
            this.DoLogin(request);

            // Redirect
            return new RedirectResult("/");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return View("Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            var user = this.dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(username);

            var response = new RedirectResult("/");
            response.Cookies.Add(new HttpCookie(".auth-cakes", cookieContent, 7));
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return new RedirectResult("/");
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            var response = new RedirectResult("/");
            response.AddCookie(cookie);
            return response;
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            var username = this.GetUsername(request);
            if (string.IsNullOrWhiteSpace(username))
            {
                return this.BadRequestError(
                    "<h1>You need to login first!</h1><br/><a href=\"/login\"><button>Login</button></a>");
            }
            var user = this.dbContext.Users.FirstOrDefault(u => u.Username == username);
            var registeredOn = user.DateOfRegistration;
            var ordersCount = user.Orders.Count;

            var viewBag = new Dictionary<string, string>();
            viewBag["username"] = username;
            viewBag["registeredOn"] = registeredOn.ToString("dd-MM-yyyy");
            viewBag["ordersCount"] = ordersCount.ToString();

            return View("UserProfile", viewBag);
        }

        public IHttpResponse ShowOrders(IHttpRequest request)
        {
            var username = this.GetUsername(request);
            if (string.IsNullOrWhiteSpace(username))
            {
                return this.BadRequestError(
                    "<h1>You need to login first!</h1><br/><a href=\"/login\"><button>Login</button></a>");
            }

            var sb = new StringBuilder();
            sb.Append("<table>")
                .Append("<th>Order Id</th><th>Created On</th><th>Sum</th>");

            var orders = this.dbContext.Users
                .FirstOrDefault(u => u.Username == username)
                .Orders
                .Select(o =>
                    $"<tr><td><a href=\"/orderDetails?id={o.Id}\">{o.Id}</a></td><td>{o.DateOfCreation:dd-MM-yyyy}</td><td>${o.OrderProducts.Select(op => op.Product.Price).DefaultIfEmpty(0).Sum():f2}</td></tr>"
                )
                .ToArray();

            sb.Append(string.Join(string.Empty, orders))
                .Append("</table>");

            var viewBag = new Dictionary<string, string>()
            {
                ["Orders"] = sb.ToString()
            };

            return View("MyOrders", viewBag);
        }
    }
}
