using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Data
{
    public class DutchMappingProfile:Profile
    {
        public DutchMappingProfile()
        {
            // reverse map will make it valid
            CreateMap<Order, OrderViewModel>()
                .ForMember(m => m.OrderId,
                    ex => ex.MapFrom(o=>o.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
        }
    }
}
