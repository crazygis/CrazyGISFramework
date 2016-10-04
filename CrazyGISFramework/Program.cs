using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.GeoPackage;
using CrazyGIS.Graphical;
using GeoAPI;
using CrazyGIS.CoordinateConversion;
using CrazyGIS.CoordinateConversion.Models;

namespace CrazyGISFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			ConversionParameters parameters = new ConversionParameters();
			// 七参数
			SevenParameters sevenP = new SevenParameters();
			sevenP.XaxisDeviation = 0;
			sevenP.XaxisRotateRadian = 0;
			sevenP.YaxisDeviation = 0;
			sevenP.YaxisRotateRadian = 0;
			sevenP.ZaxisDeviation = 0;
			sevenP.ZaxisRotateRadian = 0;
			sevenP.ScaleParameter = 0;

			parameters.ConversionSevenParams = sevenP;
			parameters.SourceCoordinateSystem = CoordinateSystemType.WGS84;
			parameters.TargetCoordinateSystem = CoordinateSystemType.CGCS2000;
			parameters.SourceCenterMeridian = 120;
			parameters.TargetCenterMeridian = 120;
			parameters.SourceCoordinateType = CoordinateType.Sphere;
			parameters.TargetCoordinateType = CoordinateType.Sphere;

			ConversionService conversionService = new ConversionService(parameters);

			ICoordinate source = new SpherePoint(120, 32);
			Console.WriteLine("源：" + source.xAxis + "," + source.yAxis);
			ICoordinate target = conversionService.SourceToTarget(source);
			Console.WriteLine("目标：" + target.xAxis + "," + target.yAxis);

			source = conversionService.TargetToSource(target);
			Console.WriteLine("反算出来的源：" + source.xAxis + "," + source.yAxis);
		}
	}
}
