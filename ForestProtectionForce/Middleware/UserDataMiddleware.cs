using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ForestProtectionForce.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserDataMiddleware
    {
        private readonly RequestDelegate _next;

        public UserDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userData = context.Request.Headers["X-User-Data"].ToString();
            bool t = context.Request.Headers.ContainsKey("x-user-data");
            // Retrieve the user data from the custom header
            if (context.Request.Headers.TryGetValue("x-user-data", out var xUserDataValues))
            {
                string xUserData = xUserDataValues.FirstOrDefault();
                // Use the value of xUserData as needed
            }
            // You can access the user data as needed, e.g. deserialize it into an object, etc.
            var user = JsonConvert.DeserializeObject<UserData>(userData);
            // Here, User is a model class representing your user data

            // Continue with the request pipeline
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserDataMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserDataMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserDataMiddleware>();
        }
    }
}
