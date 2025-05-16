using Core.Entities;


namespace Entities.DTOs.Users
{
    public class ConfirmPasswordResetDto : IDto
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }
}

