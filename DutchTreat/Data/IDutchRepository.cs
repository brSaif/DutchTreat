using System.Collections.Generic;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCatagory(string cat);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(string username,int id);  
        void AddEntity(object model);
        bool SaveAll();
        
    }
}