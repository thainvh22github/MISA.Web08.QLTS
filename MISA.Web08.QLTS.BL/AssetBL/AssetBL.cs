using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Attributes;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;
using MISA.Web08.QLTS.API.Entities;
using OfficeOpenXml;

namespace MISA.Web08.QLTS.BL
{
    public class AssetBL : BaseBL<Assets>, IAssetBL
    {

        #region Field

        private IAssetDL _assetDL;

        #endregion

        #region Contructor

        public AssetBL(IAssetDL assetDL) : base(assetDL)
        {
            _assetDL = assetDL;
        }

        /// <summary>
        /// Validate nghiệp vụ dữ liệu truyền vào
        /// </summary>
        /// <param name="asset">thông tin tài sản</param>
        /// <returns>Đối tượng ServiceResponse mỗ tả thành công hay thất bại</returns>
        /// Author: NVHThai (12/10/2022)
        private ServiceResponse ValidateDetail(Assets asset)
        {
            var validateFailures = new List<string>();

            if ((decimal)asset.loss_year > asset.cost)
            {
                validateFailures.Add(Resource.validateDetailLossYear);
            }
            
            float depreciationRate = (float)asset.depreciation_rate;
            int liftTime = (int)asset.life_time;
            if(liftTime != 0)
            {
                float checkDepreciationRate = 1 / (float)liftTime * 100;
                float result = (float)Math.Round(checkDepreciationRate, 2);
                if(result != depreciationRate)
                {
                    validateFailures.Add(Resource.validateDetailDepreciationRate);
                }
            }
            else
            {
                if(depreciationRate != 0)
                {
                    validateFailures.Add(Resource.validateDetailDepreciationRate);
                }
            }
            if (validateFailures.Count > 0)
            {
                validateFailures.Add(Resource.MoreInfo);
                return new ServiceResponse
                {
                    Success = (int)StatusResponse.Failed,
                    Data = new ErrorResult(
                    AssetErrorCode.InvalidInput,
                    Resource.UserMsg_ValidateFailed,
                    Resource.UserMsg_ValidateFailed,
                    validateFailures)
                };
            }
            return new ServiceResponse { Success = (int)StatusResponse.Done };


        }

        /// <summary>
        /// Validate trống dữ liệu truyền lên từ API
        /// </summary>
        /// <param name="asset">Đối tượng tài sản cần validate</param>
        /// <returns>Đối tượng ServiceResponse mỗ tả thành công hay thất bại</returns>
        /// Author: NVHThai (04/10/2022)
        private ServiceResponse ValidateRequestData(Assets asset)
        {
            
            // Validate dữ liệu đầu vào
            var properties = typeof(Assets).GetProperties();
            var validateFailures = new List<string>();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                var propertyValue = property.GetValue(asset);
                var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
                }
            }

            if (validateFailures.Count > 0)
            {
                validateFailures.Add(Resource.MoreInfo);
                return new ServiceResponse
                {
                    Success = (int)StatusResponse.Failed,
                    Data = new ErrorResult(
                    AssetErrorCode.InvalidInput,
                    Resource.UserMsg_ValidateFailed,
                    Resource.UserMsg_ValidateFailed,
                    validateFailures)
                };
            }
            return new ServiceResponse { Success = (int)StatusResponse.Done };
        }

        #endregion
        
        /// <summary>
        /// lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public Assets GetAssetByID(Guid assetID)
        {
            return _assetDL.GetAssetByID(assetID);
        }

        /// <summary>
        /// Kiểm tra xem id tài sản này đã chứ từ bằng mã nào
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Mã chứng từ</returns>
        /// Author: NVHThai (3/11/2022)
        public string checkAssetIsActive(Guid assetID)
        {
            return _assetDL.checkAssetIsActive(assetID);
        }

        /// <summary>
        /// Hàm tìm kiếm và phân trang
        /// </summary>
        /// <param name="keword">tìm kiếm theo mã tài sản và tên tài sản</param>
        /// <param name="assetCategoryID">lọc theo id loại tài sản</param>
        /// <param name="departmentID">lọc theo id phòng ban</param>
        /// <param name="limit">số trang trong 1 bản ghi</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID, Guid? departmentID, int limit, int offset)
        {
            return _assetDL.FilterAssets(keword, assetCategoryID, departmentID, limit, offset);
        }

        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        public string GetNewAssetCode()
        {
            return _assetDL.GetNewAssetCode();
        }

        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset">Thông tin tài sản muốn thêm</param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        public ServiceResponse InsertAsset(Assets asset)
        {
            var validateResult = ValidateRequestData(asset);
            if (validateResult != null && validateResult.Success == (int)StatusResponse.Done)
            {
                var validateResultDetail = ValidateDetail(asset);
                if (validateResultDetail != null && validateResultDetail.Success == (int)StatusResponse.Done)
                {
                    var newAssetID = _assetDL.InsertAsset(asset);

                    if (newAssetID.assetID != Guid.Empty && newAssetID.numberOfAffectedRows != 0)
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusResponse.Done,
                            Data = newAssetID
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusResponse.Exception,
                            Data = new ErrorResult(
                            AssetErrorCode.Exception,
                            Resource.DevMeg_ValidateFailed,
                            Resource.UserMsg_ValidateFailed,
                            Resource.MoreInfo)
                        };
                    }
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Failed,
                        Data = validateResultDetail.Data
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = (int)StatusResponse.Failed,
                    Data = validateResult.Data
                };
            }
            
        }


        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="assetID">ID tài sản cần sửa</param>
        /// <param name="asset">Thông tin tài sản cần sửa</param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        public ServiceResponse UpdateAsset(Guid assetID, Assets asset)
        {
            var validateResult = ValidateRequestData(asset);

            if (validateResult != null && validateResult.Success == (int)StatusResponse.Done)
            {
                var validateResultDetail = ValidateDetail(asset);
                if (validateResultDetail != null && validateResultDetail.Success == (int)StatusResponse.Done)
                {
                    var newAssetID = _assetDL.UpdateAsset(assetID, asset);

                    if (newAssetID.assetID != Guid.Empty && newAssetID.numberOfAffectedRows != -1)
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusResponse.Done,
                            Data = newAssetID
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusResponse.Exception,
                            Data = new ErrorResult(
                            AssetErrorCode.Exception,
                            Resource.DevMeg_ValidateFailed,
                            Resource.UserMsg_ValidateFailed,
                            Resource.MoreInfo)
                        };
                    }
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Failed,
                        Data = validateResultDetail.Data
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = (int)StatusResponse.Failed,
                    Data = validateResult.Data
                };
            }
        }

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID">ID tài sản cần xóa</param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteAsset(Guid assetID)
        {
            return _assetDL.DeleteAsset(assetID);
        }

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs">Danh sách ID tài sản cần xóa</param>
        /// <returns>Số tài sản bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteMutipleAssets(List<string> assetList)
        {
            return _assetDL.DeleteMutipleAssets(assetList);
        }
       
    }
}
