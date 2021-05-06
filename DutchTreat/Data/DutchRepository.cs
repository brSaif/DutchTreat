using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;

        public DutchRepository(DutchContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _ctx.Products
                .OrderBy(p => p.Title)
                .ToList();
        }
        
        public IEnumerable<Product> GetProductsByCatagory(string category)
        {
            return _ctx.Products
                .OrderBy(p => p.Category)
                .ToList();
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                        .Include(c => c.Items)
                        .ThenInclude(c => c.Product)
                        .ToList(); 
            }
            else
            {
                return _ctx.Orders.ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                    .Where(u => u.User.UserName == username)
                    .Include(c => c.Items)
                    .ThenInclude(c => c.Product)
                    .ToList();
            }
            else
            {
                return _ctx.Orders.ToList();
            }
        }

        public Order GetOrderById(string username,int id)
        {
            return _ctx.Orders
                .Include(c => c.Items)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(c => c.Id == id && c.User.UserName == username );
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public bool SaveAll()
        {

            return _ctx.SaveChanges() > 0;
        }
    }
}
