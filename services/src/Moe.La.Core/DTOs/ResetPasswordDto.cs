namespace Moe.La.Core.Dtos
{
    public class ResetPasswordDto
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
