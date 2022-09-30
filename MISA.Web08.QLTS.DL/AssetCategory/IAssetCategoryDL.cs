using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Web08.QLTS.Common.Entities;
namespace MISA.Web08.QLTS.Common.Entities;

public interface IAssetCategoryDL
{
    public IEnumerable<AssetCategory> GetAllAssetsCategory();
}
