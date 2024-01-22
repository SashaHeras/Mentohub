﻿using Mentohub.Core.Infrastructure;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Repositories.Interfaces.PaymentInterfaces
{
    public interface IOrderItemRepository : ISingletoneService, IRepository<OrderItem>
    {
        ICollection<OrderItem> GetOrderItems();

        OrderItem GetOrderItem(int id);
    }
}