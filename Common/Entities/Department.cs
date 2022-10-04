namespace MISA.Web08.QLTS.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string department_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Phòng ban này có phải là cha không?
        /// </summary>
        public int is_parent { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid organization_id { get; set; }

        /// <summary>
        /// Người tạo bảng
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// Ngày tạo bảng
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// Người sửa bảng
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// Ngày sửa bảng
        /// </summary>
        public DateTime modified_date { get; set; }
    }
}
