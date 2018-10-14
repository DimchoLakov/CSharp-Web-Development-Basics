using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByTheCake.App.Extensions;
using ByTheCakeApp.Data;
using ByTheCakeApp.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace ByTheCake.App.Controllers
{
    public class CakeController : BaseController
    {
        private const decimal DefaultProductPrice = 5;

        private ByTheCakeDbContext dbContext;

        public CakeController()
        {
            this.dbContext = new ByTheCakeDbContext();
        }

        public IHttpResponse AddCake(IHttpRequest request)
        {
            return View("AddCake");
        }

        public IHttpResponse DoAddCake(IHttpRequest request)
        {
            var productName = request.FormData["product"].ToString();
            productName = productName.Replace("+", " ");
            var price = decimal.TryParse(request.FormData["price"].ToString(), out decimal result) ? result : DefaultProductPrice;
            var url = request.FormData["url"].ToString().UrlDecode();

            Uri uriResult;
            bool isUrlValid = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            var product = new Product()
            {
                Name = productName,
                Price = price,
                ImageUrl = url
            };

            this.dbContext.Products.Add(product);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                this.ServerError(e.Message);
            }

            return View("/Index");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            var allProducts = this.dbContext
                .Products
                .Select(p => $"<div><a href=\"/cakeById?id={p.Id}\">{p.Name}</a> ${p.Price:f2} <input type=\"submit\" name=\"{p.Id}\" value=\"Order\" /></div><br />")
                .ToArray();

            var productsAsString = string.Join(string.Empty, allProducts);

            var viewBag = new Dictionary<string, string>()
            {
                ["Cakes"] = productsAsString
            };

            return View("Search", viewBag);
        }

        public IHttpResponse CakeById(IHttpRequest request)
        {
            var isIdValid = int.TryParse(request.QueryData["id"].ToString(), out int id);
            if (!isIdValid)
            {
                return this.BadRequestError("Cake Not Found!");
            }

            var product = this.dbContext
                .Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Cake Not Found!");
            }

            var viewBag = new Dictionary<string, string>()
            {
                ["Name"] = product.Name,
                ["Price"] = product.Price.ToString("f2"),
                ["ImageUrl"] = product.ImageUrl
            };

            return View("CakeById", viewBag);
        }

        public IHttpResponse OrderDetails(IHttpRequest request)
        {
            var viewBag = new Dictionary<string, string>();
            var isIdValid = int.TryParse(request.QueryData["id"].ToString(), out int id);
            if (!isIdValid)
            {
                return this.BadRequestError("Id not valid!");
            }

            var order = this.dbContext.Orders.FirstOrDefault(o => o.Id == id);

            var sb = new StringBuilder();
            sb.Append("<table>")
                .Append("<th>Product Name</th><th>Price</th>");

            var orderDetails = order
                .OrderProducts
                .Select(op =>
                    $"<tr><td><a href=\"/cakeById?id={op.ProductId}\">{op.Product.Name}</a></td><td>${op.Product.Price:f2}</td></tr><br/><div><i>Created On: {op.Order.DateOfCreation:dd-MM-yyyy}</i></div>"
                )
                .ToArray();

            var result = string.Join(string.Empty, orderDetails);

            sb.Append(result)
                .Append("</table>");

            viewBag["Order"] = sb.ToString();

            return View("OrderDetails", viewBag);
        }
    }
}
