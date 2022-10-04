using MISA.Web08.QLTS.Common.Attributes;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class Assets
    {
        /// <summary>
        /// ID tài sản
        /// </summary>
        [PrimaryKey]
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        [IsNotNullOrEmpty("Mã tài sản không được để trống")]
        public string? fixed_asset_code { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [IsNotNullOrEmpty("Tên tài sản không được để trống")]
        public string? fixed_asset_name { get; set; }


        /// <summary>
        /// ID loại tài sản
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        [IsNotNullOrEmpty("Mã phòng ban không được để trống")]
        public string? department_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string? department_name { get; set; }

        /// <summary>
        /// ID phòng ban
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [IsNotNullOrEmpty("Mã loại tài sản không được để trống")]
        public string? fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? fixed_asset_category_name { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>
        [IsNotNullOrEmpty("Ngày mua không được để trống")]
        public DateTime? purchase_date { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        [IsNotNullOrEmpty("Nguyên giá không được để trống")]
        public float? cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [IsNotNullOrEmpty("Số lượng không được để trống")]
        public int? quantity { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn
        /// </summary>
        [IsNotNullOrEmpty("Tỷ lệ hao mòn không được để trống")]
        public float? depreciation_rate { get; set; }

        /// <summary>
        /// Năm theo dõi
        /// </summary>
        public int? tracked_year { get; set; }


        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [IsNotNullOrEmpty("Số năm sử dụng không được để trống")]
        public int? life_time { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>
        [IsNotNullOrEmpty("Ngày bắt đầu sử dụng không được để trống")]
        public DateTime? production_date { get; set; }

        /// <summary>
        /// Người tạo bảng
        /// </summary>
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày tạo bảng
        /// </summary>
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người sửa bảng
        /// </summary>
        public string? modified_by { get; set; }

        /// <summary>
        /// Ngày sửa bảng
        /// </summary>
        public DateTime? modified_date { get; set; }

        /// <summary>
        /// Giá trị hao mòn năm
        /// </summary>
        public float? loss_year { get; set; } 
    }
}
