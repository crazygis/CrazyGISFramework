using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.GeoPackage
{
	public class GeoPackageManager
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public GeoPackageManager()
		{

		}

		#region 接口方法

		/// <summary>
		/// 创建GeoPackage数据库
		/// </summary>
		/// <param name="gpkgFullName">数据库文件名称(含完整路径)</param>
		/// <returns></returns>
		public bool CreateGeoPackage(string gpkgFullName)
		{
			return true;
		}

		#endregion

	}
}
