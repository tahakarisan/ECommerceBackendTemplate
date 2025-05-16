using Core.Entities;

namespace Entities.DTOs.Users
{
    public class ChangePasswordDto : IDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
