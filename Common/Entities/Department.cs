namespace MISA.Web08.QLTS.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// mã phòng ban
        /// </summary>
        public string department_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string department_name { get; set; }

        public string description { get; set; }

        public int is_parent { get; set; }

        public Guid organization_id { get; set; }

        public string created_by { get; set; }

        public DateTime created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }
}
