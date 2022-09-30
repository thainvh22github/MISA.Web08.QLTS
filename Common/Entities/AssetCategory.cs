namespace MISA.Web08.QLTS.Common.Entities
{
    public class AssetCategory
    {
        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// mã loại tài sản
        /// </summary>
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        public Guid organization_id { get; set; }

        public float depreciation_rate { get; set; }

        public int life_time { get; set; }

        public string description { get; set; }

        public string created_by { get; set; }

        public DateTime created_date { get; set; }

        public string modified_by { get; set; }

        public DateTime modified_date { get; set; } 

    }
}
