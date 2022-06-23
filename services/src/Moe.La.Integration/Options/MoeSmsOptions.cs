namespace Moe.La.Integration.Options
{
    public class MoeSmsOptions
    {
        /// <summary>
        /// Service endpoint URL.
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
        /// The name of the sender.
        /// </summary>
        /// <remarks>Eg. MOE</remarks>
        public string Sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CheckActivation { get; set; }
    }
}
