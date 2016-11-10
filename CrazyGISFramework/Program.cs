using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.CoordinateConversion;
using CrazyGIS.Graphical;
using CrazyGIS.Graphical.Models;
using System.IO;
using CrazyGIS.Toolkit;
using System.Data.SQLite;
using System.Data;
using CrazyGIS.TilePackage.Database;
using System.Net;

namespace CrazyGISFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			//TilePackageTest();

			//double dpi = 591658710.909131 / (2 * Math.PI * 6378137 * 1.40625 / 360 / 0.0254);
			//Console.WriteLine(dpi);
			//double scale = (96 * 2 * Math.PI * 6378137 * 1.40625 / 360 / 0.0254);
			//Console.WriteLine(scale);

			//double resolution = 156543.033928 * 360 / (2 * Math.PI * 6378137);
			//Console.WriteLine(resolution);

			UriTest();
		}

		static void SevenParamsConversion()
		{
			//ConversionParameters parameters = new ConversionParameters();
			//// 七参数
			//SevenParameters sevenP = new SevenParameters();
			//sevenP.XaxisDeviation = 0;
			//sevenP.XaxisRotateRadian = 0;
			//sevenP.YaxisDeviation = 0;
			//sevenP.YaxisRotateRadian = 0;
			//sevenP.ZaxisDeviation = 0;
			//sevenP.ZaxisRotateRadian = 0;
			//sevenP.ScaleParameter = 0;

			//parameters.ConversionSevenParams = sevenP;
			//parameters.SourceCoordinateSystem = CoordinateSystemType.WGS84;
			//parameters.TargetCoordinateSystem = CoordinateSystemType.CGCS2000;
			//parameters.SourceCenterMeridian = 120;
			//parameters.TargetCenterMeridian = 120;
			//parameters.SourceCoordinateType = CoordinateType.Sphere;
			//parameters.TargetCoordinateType = CoordinateType.Sphere;

			//ConversionService conversionService = new ConversionService(parameters);

			//CrazyGIS.CoordinateConversion.Models.ICoordinate source = 
			//	new CrazyGIS.CoordinateConversion.Models.SpherePoint(120, 32);
			//Console.WriteLine("源：" + source.xAxis + "," + source.yAxis);
			//CrazyGIS.CoordinateConversion.Models.ICoordinate target = conversionService.SourceToTarget(source);
			//Console.WriteLine("目标：" + target.xAxis + "," + target.yAxis);

			//source = conversionService.TargetToSource(target);
			//Console.WriteLine("反算出来的源：" + source.xAxis + "," + source.yAxis);
		}

		static void IconInputDatabase()
		{
			string fileFolder = @"D:\Code\CJ\水上公安\HarborPoliceSolution\HarborPoliceWebApp\Content\police";
			string dbFileName = @"D:\Code\CJ\水上公安\HarborPoliceSolution\HarborPoliceWebApp\App_Data\HarborPolice.db";
			string sql = "INSERT INTO icon (id,data,enabled,time,remark) VALUES(:id,:data,:enabled,:time,:remark)";

			SQLiteConnection connection = SQLiteTool.GetConnection(dbFileName);
			if(connection == null)
			{
				Console.WriteLine("sqlite connection error");
				return;
			}

			if (connection.State == ConnectionState.Closed)
			{
				connection.Open();
			}

			DirectoryInfo directoryInfo = new DirectoryInfo(fileFolder);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				if(fileInfo.Extension == ".png" || fileInfo.Extension == ".jpg")
				{
					byte[] imageData = ImageTool.ImageToBytes(fileInfo.FullName);
					Dictionary<string, object> paramters = new Dictionary<string, object>();
					paramters.Add("id", Guid.NewGuid().ToString().ToLower());
					paramters.Add("data", imageData);
					paramters.Add("enabled", 1);
					paramters.Add("time", DateTime.Now);
					paramters.Add("remark", null);

					bool success = SQLiteTool.ExecuteNonQuery(connection, sql, paramters);
					if(success)
					{
						Console.WriteLine("success:" + fileInfo.FullName);
					}
					else
					{
						Console.WriteLine("failure:" + fileInfo.FullName);
					}
				}
			}

			connection.Close();

		}

		static void TilePackageTest()
		{
			TilePackageCreator creator = new TilePackageCreator();
			creator.Create("D:\\test.tpkg");
			Console.WriteLine("OK");
		}

		static void UrlToBytesTest()
		{
			// http://www.crazygis.com/Tiles/nanjing/maritime/12/657x3399.png
			// http://t3.tianditu.com/DataServer?T=cva_c&x=3399&y=660&l=12
			// http://t3.tianditu.com/DataServer?T=cva_c&x=3401&y=660&l=12
			string UrlImg = "http://www.crazygis.com/Tiles/nanjing/maritime/12/657x3399.png";
			WebClient webClient = new WebClient();
			webClient.Credentials = CredentialCache.DefaultCredentials;
			//以数组的形式下载指定文件  
			try
			{
				byte[] byteData = webClient.DownloadData(UrlImg);

				Console.WriteLine("success");

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		static void UriTest()
		{
			Uri uri = new Uri("http://www.crazygis.com/Tiles/nanjing/maritime/{z}/657x3399.png");
			Uri uri2 = new Uri(@"D:\test.tpkg");
			Console.WriteLine(uri.AbsoluteUri);

		}
	}
}
