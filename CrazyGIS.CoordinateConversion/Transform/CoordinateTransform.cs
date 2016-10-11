using System;
using System.Collections.Generic;
using CrazyGIS.CoordinateConversion.Models;
using System.Linq;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 坐标转换
	/// </summary>
	public class CoordinateTransform
	{
		private SevenParameters sevenParams; // 七参数
		private CoordinateSystemType sourceCS; // 源坐标系统
		private CoordinateSystemType targetCS; // 目标坐标系统
		private double sourceMeridian; // 源中央经线
		private double targetMeridian; // 目标中央经线
		private CoordinateType sourceCT; // 源坐标类型
		private CoordinateType targetCT; // 目标坐标类型

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="parameters">转换参数</param>
		public CoordinateTransform(ConversionParameters parameters)
		{
			if(parameters == null || parameters.ConversionSevenParams == null)
			{
				throw new Exception("转换参数或转换七参数为空");
			}

			this.sevenParams = parameters.ConversionSevenParams;
			this.sourceCS = parameters.SourceCoordinateSystem;
			this.targetCS = parameters.TargetCoordinateSystem;
			this.sourceMeridian = parameters.SourceCenterMeridian;
			this.targetMeridian = parameters.TargetCenterMeridian;
			this.sourceCT = parameters.SourceCoordinateType;
			this.targetCT = parameters.TargetCoordinateType;
		}

		/// <summary>
		/// 源坐标转目标坐标
		/// </summary>
		/// <param name="sourceCoordinate">源坐标</param>
		/// <returns>目标坐标</returns>
		public ICoordinate SourceToTarget(ICoordinate sourceCoordinate)
		{
			if(sourceCoordinate == null)
			{
				return null;
			}
			// 两个中间变量
			SpherePoint spTemp = null;
			PlanePoint ppTemp = null;
			// 如果源是平面坐标
			// 1.平面坐标转高斯坐标;2.高斯反算,转为球面坐标
			if(sourceCT == CoordinateType.Plane)
			{
				GaussKrugerTransform gauss_source = new GaussKrugerTransform(sourceCS);
				ppTemp = gauss_source.PlaneToGauss((PlanePoint)sourceCoordinate);
				spTemp = gauss_source.GaussKrugerReverse(ppTemp, sourceMeridian);
			}
			else
			{
				spTemp = (SpherePoint)sourceCoordinate.Clone();
			}

			// 大地坐标转空间直角坐标
			GeodeticTransform geo_source = new GeodeticTransform(sourceCS, sourceMeridian);
			ppTemp = geo_source.GeodeticToThreeDimensions(spTemp);

			// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
			BursaWolfTransform bursa_source = new BursaWolfTransform(sevenParams);
			ppTemp = (PlanePoint)bursa_source.Transform(ppTemp);

			// 空间直角坐标转大地坐标
			GeodeticTransform geo_target = new GeodeticTransform(targetCS, targetMeridian);
			spTemp = geo_target.ThreeDimensionsToGeodetic(ppTemp);


			ICoordinate result = null;
			// 如果目标是平面坐标
			// 1.高斯正算,转为高斯坐标;2.高斯坐标转平面坐标
			if (targetCT == CoordinateType.Plane)
			{
				GaussKrugerTransform gauss_target = new GaussKrugerTransform(targetCS);
				ppTemp = gauss_target.GaussKrugerForward(spTemp, targetMeridian);
				ppTemp = gauss_target.GaussToPlane(ppTemp);

				result = ppTemp.Clone();
			}
			else
			{
				result = spTemp.Clone();
			}

			return result;
		}

		/// <summary>
		/// 目标坐标转源坐标
		/// </summary>
		/// <param name="targetCoordinate">目标坐标</param>
		/// <returns>源坐标</returns>
		public ICoordinate TargetToSource(ICoordinate targetCoordinate)
		{
			if (targetCoordinate == null)
			{
				return null;
			}
			// 两个中间变量
			SpherePoint spTemp = null;
			PlanePoint ppTemp = null;
			// 如果目标是平面坐标
			// 1.平面坐标转高斯坐标;2.高斯反算,转为球面坐标
			if (targetCT == CoordinateType.Plane)
			{
				GaussKrugerTransform gauss_target = new GaussKrugerTransform(targetCS);
				ppTemp = gauss_target.PlaneToGauss((PlanePoint)targetCoordinate);
				spTemp = gauss_target.GaussKrugerReverse(ppTemp, targetMeridian);
			}
			else
			{
				spTemp = (SpherePoint)targetCoordinate.Clone();
			}

			// 大地坐标转空间直角坐标
			GeodeticTransform geo_target = new GeodeticTransform(targetCS, targetMeridian);
			ppTemp = geo_target.GeodeticToThreeDimensions(spTemp);

			// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
			BursaWolfTransform bursa_target = new BursaWolfTransform(sevenParams.Reverse());
			ppTemp = (PlanePoint)bursa_target.Transform(ppTemp);

			// 空间直角坐标转大地坐标
			GeodeticTransform geo_source = new GeodeticTransform(sourceCS, sourceMeridian);
			spTemp = geo_source.ThreeDimensionsToGeodetic(ppTemp);


			ICoordinate result = null;
			// 如果源是平面坐标
			// 1.高斯正算,转为高斯坐标;2.高斯坐标转平面坐标
			if (sourceCT == CoordinateType.Plane)
			{
				GaussKrugerTransform gauss_source = new GaussKrugerTransform(sourceCS);
				ppTemp = gauss_source.GaussKrugerForward(spTemp, sourceMeridian);
				ppTemp = gauss_source.GaussToPlane(ppTemp);

				result = ppTemp.Clone();
			}
			else
			{
				result = spTemp.Clone();
			}

			return result;
		}

		/// <summary>
		/// 源坐标转目标坐标(批量)
		/// </summary>
		/// <param name="sourceCoordinates">源坐标集合</param>
		/// <returns>目标坐标集合</returns>
		public List<ICoordinate> SourceToTargetBatch(IEnumerable<ICoordinate> sourceCoordinates)
		{
			if(sourceCoordinates == null || sourceCoordinates.Count() == 0)
			{
				return null;
			}
			List<ICoordinate> result = new List<ICoordinate>();
			// 两个中间变量
			SpherePoint spTemp = null;
			PlanePoint ppTemp = null;
			// 高斯转换
			GaussKrugerTransform gauss_source = new GaussKrugerTransform(sourceCS);
			GaussKrugerTransform gauss_target = new GaussKrugerTransform(targetCS);
			// 大地坐标转换
			GeodeticTransform geo_source = new GeodeticTransform(sourceCS, sourceMeridian);
			GeodeticTransform geo_target = new GeodeticTransform(targetCS, targetMeridian);
			// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
			BursaWolfTransform bursa_source = new BursaWolfTransform(sevenParams);

			foreach(ICoordinate sourceCoordinate in sourceCoordinates)
			{
				// 如果源是平面坐标
				// 1.平面坐标转高斯坐标;2.高斯反算,转为球面坐标
				if (sourceCT == CoordinateType.Plane)
				{
					ppTemp = gauss_source.PlaneToGauss((PlanePoint)sourceCoordinate);
					spTemp = gauss_source.GaussKrugerReverse(ppTemp, sourceMeridian);
				}
				else
				{
					spTemp = (SpherePoint)sourceCoordinate.Clone();
				}

				// 大地坐标转空间直角坐标
				ppTemp = geo_source.GeodeticToThreeDimensions(spTemp);

				// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
				ppTemp = (PlanePoint)bursa_source.Transform(ppTemp);

				// 空间直角坐标转大地坐标
				spTemp = geo_target.ThreeDimensionsToGeodetic(ppTemp);

				// 如果目标是平面坐标
				// 1.高斯正算,转为高斯坐标;2.高斯坐标转平面坐标
				if (targetCT == CoordinateType.Plane)
				{
					ppTemp = gauss_target.GaussKrugerForward(spTemp, targetMeridian);
					ppTemp = gauss_target.GaussToPlane(ppTemp);

					result.Add(ppTemp.Clone());
				}
				else
				{
					result.Add(spTemp.Clone());
				}
			}

			return result;
		}

		/// <summary>
		/// 目标坐标转源坐标(批量)
		/// </summary>
		/// <param name="targetCoordiantes">目标坐标集合</param>
		/// <returns>源坐标集合</returns>
		public List<ICoordinate> TargetToSourceBatch(IEnumerable<ICoordinate> targetCoordiantes)
		{
			if (targetCoordiantes == null || targetCoordiantes.Count() == 0)
			{
				return null;
			}
			List<ICoordinate> result = new List<ICoordinate>();
			// 两个中间变量
			SpherePoint spTemp = null;
			PlanePoint ppTemp = null;
			// 高斯转换
			GaussKrugerTransform gauss_source = new GaussKrugerTransform(sourceCS);
			GaussKrugerTransform gauss_target = new GaussKrugerTransform(targetCS);
			// 大地坐标转换
			GeodeticTransform geo_source = new GeodeticTransform(sourceCS, sourceMeridian);
			GeodeticTransform geo_target = new GeodeticTransform(targetCS, targetMeridian);
			// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
			BursaWolfTransform bursa_target = new BursaWolfTransform(sevenParams.Reverse());

			foreach(ICoordinate targetCoordinate in targetCoordiantes)
			{
				// 如果目标是平面坐标
				// 1.平面坐标转高斯坐标;2.高斯反算,转为球面坐标
				if (targetCT == CoordinateType.Plane)
				{
					ppTemp = gauss_target.PlaneToGauss((PlanePoint)targetCoordinate);
					spTemp = gauss_target.GaussKrugerReverse(ppTemp, targetMeridian);
				}
				else
				{
					spTemp = (SpherePoint)targetCoordinate.Clone();
				}

				// 大地坐标转空间直角坐标
				ppTemp = geo_target.GeodeticToThreeDimensions(spTemp);

				// 七参数模型，对空间直角坐标进行转换，转换后同样是空间直角坐标
				ppTemp = (PlanePoint)bursa_target.Transform(ppTemp);

				// 空间直角坐标转大地坐标
				spTemp = geo_source.ThreeDimensionsToGeodetic(ppTemp);

				// 如果源是平面坐标
				// 1.高斯正算,转为高斯坐标;2.高斯坐标转平面坐标
				if (sourceCT == CoordinateType.Plane)
				{
					ppTemp = gauss_source.GaussKrugerForward(spTemp, sourceMeridian);
					ppTemp = gauss_source.GaussToPlane(ppTemp);

					result.Add(ppTemp.Clone());
				}
				else
				{
					result.Add(spTemp.Clone());
				}
			}

			return result;
		}
	}
}
