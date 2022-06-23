using System;

namespace Moe.La.Core.Constants
{
    /// <summary>
    /// The application constants.
    /// </summary>
    public static class ApplicationConstants
    {
        /// <summary>
        /// The system administrator user used for actions created by the system.
        /// </summary>
        public static readonly Guid SystemAdministratorId = new Guid("f243f134-6761-4bdf-9016-fcf497765c2e");

        /// <summary>
        /// موقع مخزن الملفات
        /// </summary>
        public static readonly string UploadLocation = "Uploads";

        /// <summary>
        /// موقع مخزن صور النماذج
        /// </summary>
        public static readonly string TemplateImageLocation = "images/templates/";
    }
}
