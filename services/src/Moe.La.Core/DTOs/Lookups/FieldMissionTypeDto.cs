namespace Moe.La.Core.Dtos
{
    public class FieldMissionTypeListItemDto : BaseDto<int>
    {
        public string Name { get; set; }
    }

    public class FieldMissionTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
