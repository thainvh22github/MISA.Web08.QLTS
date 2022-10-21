using MISA.Web08.QLTS.Common.Attributes;
using MISA.Web08.QLTS.DL;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Contructor

        public BaseBL(IBaseDL<T> baseDL) 
        {
            _baseDL = baseDL;
        }

        /// <summary>
        /// Binding format style cho file excel
        /// </summary>
        /// <param name="workSheet">Sheet cần binding format</param>
        /// <param name="record">Danh sách bản ghi</param>
        /// Author:NVHThai (17/10/2022)
        private void BindingFormatForExcel(ExcelWorksheet workSheet, IEnumerable<T> records)
        {
            // Lấy ra các property có attribute name là ExcelColumnNameAttribute 
            var excelColumnProperties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true).Length > 0);

            // Lấy ra tên column cuối cùng (tính cả số thứ tự)
            var lastColumnName = (char)('A' + (excelColumnProperties.Count()+1));

            // Tạo phần tiêu đề cho file excel
            using (var range = workSheet.Cells[$"A1:{lastColumnName}1"])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 16;
                range.Style.Font.Name = "Arial";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Value = "DANH SÁCH TÀI SẢN";
            }
            // Gộp các ô từ A2 đến ô cuối cùng của dòng 2
            workSheet.Cells[$"A2:{lastColumnName}2"].Merge = true;

            // Style chung cho tất cả bảng
            using (var range = workSheet.Cells[$"A3:{lastColumnName}{records.Count() + 3}"])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Times New Roman";
                range.Style.Font.Size = 11;
                range.AutoFitColumns();
            }

            // Lấy ra các property có attribute name là ExcelColumnNameAttribute và đổ vào header của table
            int columnIndex = 1;
            workSheet.Cells[3, columnIndex].Value = "Số thứ tự";
            columnIndex++;
            foreach (var property in excelColumnProperties)
            {
                var excelColumnName = (property.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true)[0] as ExcelColumnNameAttribute).ColumnName;
                workSheet.Cells[3, columnIndex].Value = excelColumnName;
                columnIndex++;
            }
            workSheet.Cells[3, columnIndex].Value = "Giá trị còn lại";
            columnIndex++;

            // Style cho header của table
            using (var range = workSheet.Cells[$"A3:{lastColumnName}3"])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 10;
                range.Style.Font.Name = "Arial";
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Đổ dữ liệu từ list nhân viên vào các côt tương ứng
            int rowIndex = 4;
            int stt = 1; // Số thứ tự
            float cost = 0; 
            float lossYear = 0; 
            foreach (var record in records)
            {
                columnIndex = 1;
                workSheet.Cells[rowIndex, columnIndex].Value = stt;
                columnIndex++;
                foreach (var property in excelColumnProperties)
                {
                    // Lấy ra giá trị của property
                    var propertyValue = property.GetValue(record);
                    // Trả về đối số kiểu cơ bản của kiểu nullable đã chỉ định.
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var value = "";
                    switch (propertyType.Name)
                    {
                        case "DateTime":
                            value = (propertyValue as DateTime?)?.ToString("dd/MM/yyyy"); // Định dạng ngày tháng
                            workSheet.Cells[rowIndex, columnIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            break;
                        default:
                            value = propertyValue?.ToString();
                            break;
                    }
                    if(columnIndex == 6)
                    {
                        cost = (float)propertyValue;
                    }
                    if(columnIndex == 8)
                    {
                        lossYear = (float)propertyValue;
                    }
                    // Đổ dữ liệu vào cột
                    workSheet.Cells[rowIndex, columnIndex].Value = value;
                    workSheet.Column(columnIndex).AutoFit();
                    columnIndex++;
                }
                workSheet.Cells[rowIndex, columnIndex].Value = (cost - lossYear).ToString();
                columnIndex++;
                rowIndex++;
                stt++;
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>Đối tượng Stream chứa file excel</returns>
        /// Author:NVHThai (17/10/2022)
        public Stream ExportExcel()
        {
            var employees = _baseDL.GetAllRecords();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Danh sách tài sản");
            package.Workbook.Properties.Author = "Nguyễn Vũ Hải Thái";
            package.Workbook.Properties.Title = "Danh sách tài sản";
            BindingFormatForExcel(workSheet, employees);
            package.Save();
            stream.Position = 0; // Đặt con trỏ về đầu file để đọc
            return package.Stream;
        }

        /// <summary>
        /// lấy danh sách toàn bộ bản ghi
        /// <returns>Lấy danh sách toàn bản ghi</returns>
        /// Author: NVHThai (16/09/2022)
        /// </summary>
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Lấy danh sách bản ghi có điều kiện
        /// </summary>
        /// <param name="keword">Tên hoặc mã</param>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<T> GetFillterRecords(string? keword)
        {
            return _baseDL.GetFillterRecords(keword);
        }

        #endregion
    }
}
