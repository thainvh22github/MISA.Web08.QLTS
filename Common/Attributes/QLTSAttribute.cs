namespace MISA.Web08.QLTS.Common.Attributes
{

    /// <summary>
    /// Attribute dùng để xác định 1 property là khóa chính 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {

    }

    /// <summary>
    /// Attribure dùng để xác định 1 property không được để trống
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNotNullOrEmptyAttribute : Attribute
    {
        #region Field

        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        public string ErrorMessage;

        #endregion

        #region Constructor

        public IsNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        #endregion
    }

    /// <summary>
    /// Attribute tạo tên cột phục vụ cho việc Export Excel
    /// </summary> 
    /// Created by: TCDN AnhDV (05/10/2022)
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Tên cột
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">Tên cột</param>
        /// Created by: TCDN AnhDV (05/10/2022)
        public ExcelColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }

}
