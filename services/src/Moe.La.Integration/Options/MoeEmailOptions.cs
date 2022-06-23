namespace Moe.La.Integration.Options
{
    public class MoeEmailOptions
    {
        /// <summary>
        /// Send email endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Service account username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Service account password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email from account.
        /// </summary>
        public string From { get; set; }
    }
}
