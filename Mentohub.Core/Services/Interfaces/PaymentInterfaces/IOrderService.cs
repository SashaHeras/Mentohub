﻿using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(string userID);

        bool DeleteOrder(string orderId);

        Order GetOrder(string orderId);

        Task<OrderDTO> GetActiveUserOrder(string userID);
    }
}