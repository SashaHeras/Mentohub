﻿using Mentohub.Domain.Data.DTO.Payment;
using Mentohub.Domain.Data.Entities.Order;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IOrderItemSevice : IService
    {
        OrderItem GetOrderItem(int id);
        void DeleteOrderItem(int id);
        List<OrderItemDTO> GetOrderItems(string id);
    }
}
