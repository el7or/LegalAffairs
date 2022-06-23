namespace Moe.La.Core.Enums
{
    /// <summary>
    /// The sending options for a given request.
    /// </summary>
    public enum SendingTypes
    {
        /// <summary>
        /// The request will be sent to a given user.
        /// </summary>
        User = 1,

        /// <summary>
        /// The request will be send to a given role.
        /// </summary>
        Role = 2,

        /// <summary>
        /// The request will be sent to a given department.
        /// </summary>
        Department = 3
    }
}
