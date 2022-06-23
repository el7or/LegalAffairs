using System;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// Represents the relationship between a branch and its departments.
    /// </summary>
    public class BranchesDepartments
    {
        /// <summary>
        /// The branch ID.
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// The branch object.
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// The department ID.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// The department object.
        /// </summary>
        /// 
        public Department Department { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

    }
}
