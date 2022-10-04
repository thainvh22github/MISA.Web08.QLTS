namespace MISA.Web08.QLTS.Common.Entities
{
    public class AssetCategory
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid organization_id { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        public float depreciation_rate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int life_time { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string description { get; set; }

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
