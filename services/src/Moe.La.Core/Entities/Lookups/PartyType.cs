using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class PartyType : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Party> Parties { get; set; }
    }
}