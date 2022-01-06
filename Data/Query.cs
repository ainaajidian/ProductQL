using HotChocolate;
using ProductQL.Models;
using System.Linq;

namespace ProductQL
{
    public class Query
    {
        public IQueryable<UserData> GetUser([Service] ProductQLContext context) =>
        context.Users.Select(u => new UserData()
        { 
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Username = u.Username
        });
            
        public IQueryable<UserData> GetUserById(int id, [Service] ProductQLContext context) =>
         context.Users.Select(u => new UserData()
         {
             Id = u.Id,
             FullName = u.FullName,
             Email = u.Email,
             Username = u.Username
         }).Where(u => u.Id == id).AsQueryable();

        public IQueryable<Product> GetProduct([Service] ProductQLContext context) =>
        context.Products;

        public IQueryable<Product> GetProductById(int id, [Service] ProductQLContext context) =>
         context.Products.Where(p => p.Id == id).AsQueryable();
    }
}
