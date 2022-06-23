namespace Moe.La.Core.Models.Integration.Moeen
{
    public class RulingInfoStructureModel
    {
        /// <summary>
        /// معرف الدائرة مصدرة الحكم حسب معرفات ديوان المظالم
        /// </summary>
        public string CircuitID { get; set; }

        /// <summary>
        /// اسم الدائرة مصدرة الحكم
        /// </summary>
        public string CircuitDescription { get; set; }

        /// <summary>
        /// تاريخ الحكم
        /// </summary>
        public GHDateStructureModel RulingDate { get; set; }

        /// <summary>
        /// منطوق الحكم
        /// </summary>
        public string RulingSpoken { get; set; }

        /// <summary>
        /// التاريخ المحدد لاستلام الحكم
        /// </summary>
        public GHDateStructureModel RulingDeliveryDate { get; set; }
    }
}
