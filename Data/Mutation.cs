using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductQL.Data;
using ProductQL.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQL
{
    public class Mutation
    {
        //create
        public async Task<AddUserPayload> AddUserAsync(
        AddUserInput input,
        [Service] ProductQLContext context)
        {
            var user = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                Username = input.Username,
                Password = input.Password
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new AddUserPayload(user);
        }

        //update
        public async Task<AddUserPayload> UpdateUserAsync(int id,
        AddUserInput input,
        [Service] ProductQLContext context)
        {
            var result = context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.FullName = input.FullName;
                result.Email = input.Email;
                result.Username = input.Username;
                result.Password = input.Password;
                await context.SaveChangesAsync();
            }

            return new AddUserPayload(result);
        }

        // delete
        [Authorize]
        public async Task<AddUserPayload> DeleteUserAsync(int id,
            [Service] ProductQLContext context)
        {
            var result = context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (result != null)
            {
                context.Users.Remove(result);
                await context.SaveChangesAsync();
            }

            return new AddUserPayload(result);
        }

        //create
        public async Task<AddProductPayload> AddProductAsync(
        AddProductInput input,
        [Service] ProductQLContext context)
        {
            var product = new Product
            {
                Name = input.Name,
                Stock = input.Stock,
                Price = input.Price,
                Created = System.DateTime.Now
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return new AddProductPayload(product);
        }

        //update
        public async Task<AddProductPayload> UpdateProductAsync(int id,
        AddProductInput input,
        [Service] ProductQLContext context)
        {
            var result = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.Name = input.Name;
                result.Stock = input.Stock;
                result.Price = input.Price;
                result.Created = System.DateTime.Now;
                await context.SaveChangesAsync();
            }

            return new AddProductPayload(result);
        }

        // delete
        public async Task<AddProductPayload> DeleteProductAsync(int id,
            [Service] ProductQLContext context)
        {
            var result = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (result != null)
            {
                context.Products.Remove(result);
                await context.SaveChangesAsync();
            }

            return new AddProductPayload(result);
        }

        public async Task<UserData> RegisterUserAsync(
            RegisterUser input,
            [Service] ProductQLContext context)
        {
            var user = context.Users.Where(o => o.Username == input.UserName).FirstOrDefault();
            if (user != null)
            {
                return await Task.FromResult(new UserData());
            }
            var newUser = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                Username = input.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password)
            };

            var ret = context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return await Task.FromResult(new UserData
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                FullName = newUser.FullName
            });
        }
        public async Task<UserToken> LoginAsync(
            LoginUser input,
            [Service] IOptions<TokenSettings> tokenSettings,
            [Service] ProductQLContext context)
        {
            var user = context.Users.Where(o => o.Username == input.Username).FirstOrDefault();
            if (user == null)
            {
                return await Task.FromResult(new UserToken(null, null, "Username or password was invalid"));
            }
            bool valid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);
            if (valid)
            {
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                var expired = DateTime.Now.AddHours(3);
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenSettings.Value.Issuer,
                    audience: tokenSettings.Value.Audience,
                    expires: expired,
                    signingCredentials: credentials
                );

                return await Task.FromResult(
                    new UserToken(new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expired.ToString(), null));
                //return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return await Task.FromResult(new UserToken(null, null, Message: "Username or password was invalid"));
        }
    }
}
