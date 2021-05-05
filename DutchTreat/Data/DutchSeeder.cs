using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            StoreUser user = await _userManager.FindByEmailAsync("SeifEddine@saibr.com");

            if (user == null)
            {
                // Create new user
                user = new StoreUser()
                {
                    FirstName = "Seif",
                    LastName = "Eddine",
                    Email = "SeifEddine@saibr.com",
                    UserName = "SeifEddine@saibr.com"

                };
                var result = await _userManager.CreateAsync(user, "Pa$$W0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            if (!_context.Products.Any())
            {
                // Need to seed dev data
                var FilePath = Path.Combine("Data/art.json");
                var json = File.ReadAllText(FilePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _context.Products.AddRange(products);

                var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                 _context.SaveChanges();
            }
        }
    }
}
