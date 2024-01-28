using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.Payment
{
    public class OrderItemDTO
    {
        public int? ID { get; set; }

        public int? Pos { get; set; }

        public decimal? Price { get; set; }

        public decimal? Total { get; set; }

        public Nullable<decimal> SubTotal { get; set; }

        public Nullable<decimal> Discount { get; set; }

        public Nullable<bool> HasDiscount { get; set; }

        public string? OrderID { get; set; }

        public int? CourseID { get; set; }
    }
}
