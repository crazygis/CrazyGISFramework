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
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

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
			byte[] data = null;
			try
			{
				data = webClient.DownloadData(UrlImg);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		static void UriTest()
		{
			Uri uri = new Uri("http://www.crazygis.com/Tiles/nanjing/maritime/12/657x3399.png");
			Uri uri2 = new Uri(@"D:\test.png");
			Console.WriteLine(uri.AbsoluteUri);
			Console.WriteLine(uri2.AbsoluteUri);
			byte[] data = null;
			try
			{
				Image image = Image.FromFile(@"D:\test1.png");
				data = imageToBytes(image);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				data = null;
			}



			//string message = "http://t{0-7}.tianditu.com/DataServer?T=img_c&x={x}&y={y}&l={z}";
			//foreach (Match mch in Regex.Matches(message, "{([a-z])-([a-z])}|{(\\d)-(\\d)}"))
			//{
			//	string result = mch.Value.Trim();
			//	string a = result.Substring(1, 1);
			//	string b = result.Substring(3, 1);

			//	int start = Encoding.ASCII.GetBytes(a)[0];
			//	int end = Encoding.ASCII.GetBytes(b)[0];

			//	for(int i = start; i <= end; i++)
			//	{
			//		byte[] array = new byte[1];
			//		array[0] = (byte)(Convert.ToInt32(i));
			//		string domain = Encoding.ASCII.GetString(array);
			//		string url = message.Replace(result, domain);
			//		Console.WriteLine(url);
			//	}

			//	//Console.WriteLine(a);
			//	//Console.WriteLine(start);
			//	//Console.WriteLine(b);
			//	//Console.WriteLine(end);
			//}

			ConcurrentQueue<string> test = new ConcurrentQueue<string>();
			//Interlocked.Increment(ref successCount);

			//int counter = 0;

			//Task.Factory.StartNew(() =>
			//	Parallel.ForEach(items,
			//		new ParallelOptions
			//		{
			//			MaxDegreeOfParallelism = 4
			//		},
			//		item => {
			//			DoSomething(item);
			//			Interlocked.Increment(ref counter);
			//		});
			//);
		}

		static byte[] imageToBytes(Image image)
		{
			ImageFormat format = image.RawFormat;
			using (MemoryStream ms = new MemoryStream())
			{
				if (format.Equals(ImageFormat.Jpeg))
				{
					image.Save(ms, ImageFormat.Jpeg);
				}
				else if (format.Equals(ImageFormat.Png))
				{
					image.Save(ms, ImageFormat.Png);
				}
				else if (format.Equals(ImageFormat.Bmp))
				{
					image.Save(ms, ImageFormat.Bmp);
				}
				else if (format.Equals(ImageFormat.Gif))
				{
					image.Save(ms, ImageFormat.Gif);
				}
				else if (format.Equals(ImageFormat.Icon))
				{
					image.Save(ms, ImageFormat.Icon);
				}
				byte[] buffer = new byte[ms.Length];
				//Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(buffer, 0, buffer.Length);
				return buffer;
			}
		}
	}
}
