using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class Branch
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The branch name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parent branch ID.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// The parent branch object.
        /// </summary>
        public Branch Parent { get; set; }

        /// <summary>
        /// A list of the branch children branches.
        /// </summary>
        public IList<Branch> Children { get; set; } = new List<Branch>();

        /// <summary>
        /// A list of the branch departments.
        /// </summary>
        public IList<BranchesDepartments> BranchDepartments { get; set; } = new List<BranchesDepartments>();

    }
}
