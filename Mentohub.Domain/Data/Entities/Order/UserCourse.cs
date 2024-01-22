using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.Entities.Order
{
    public class UserCourse
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual CurrentUser? СurrentUser { get; set; }

        public int OrderItemId { get; set; }

        public string OrderPaymentId { get; set; }

        public virtual OrderPayment OrderPayment { get; set; }

        public virtual OrderItem OrderItem { get; set; }
    }
}
