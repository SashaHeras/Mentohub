using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Data.DTO.Payment
{
    public class UserCourseDTO
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int CourseId { get; set; }

        public string UserId { get; set; }

        public int? OrderItemId { get; set; }

        public string? OrderPaymentId { get; set; }
    }
}
