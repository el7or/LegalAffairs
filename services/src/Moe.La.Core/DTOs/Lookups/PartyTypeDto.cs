namespace Moe.La.Core.Dtos
{
    public class PartyTypeListItemDto : BaseDto<int>
    {
        public string Name { get; set; }
    }

    public class PartyTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
