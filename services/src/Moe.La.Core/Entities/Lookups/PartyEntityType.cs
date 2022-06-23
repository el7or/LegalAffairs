namespace Moe.La.Core.Entities
{
    /// <summary>
    /// وصف نوع الاطراف (فرد-شركة -....)    
    /// </summary>
    public class PartyEntityType : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}