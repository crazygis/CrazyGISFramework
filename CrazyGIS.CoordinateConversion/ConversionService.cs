using System;
using System.Collections.Generic;
using CrazyGIS.CoordinateConversion.Models;
using CrazyGIS.CoordinateConversion.Transform;

namespace CrazyGIS.CoordinateConversion
{
	/// <summary>
	/// 转换服务
	/// </summary>
	public class ConversionService
	{
		private CoordinateTransform coordinateTransform;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="conversionParams">转换参数</param>
		public ConversionService(ConversionParameters conversionParams)
		{
			if (conversionParams == null || conversionParams.ConversionSevenParams == null)
			{
				throw new Exception("转换参数或转换七参数为空");
			}

			coordinateTransform = new CoordinateTransform(conversionParams);
		}

		/// <summary>
		/// 源坐标转目标坐标
		/// </summary>
		/// <param name="sourceCoordinate">源坐标</param>
		/// <returns>目标坐标</returns>
		public ICoordinate SourceToTarget(ICoordinate sourceCoordinate)
		{
			return coordinateTransform.SourceToTarget(sourceCoordinate);
		}

		/// <summary>
		/// 目标坐标转源坐标
		/// </summary>
		/// <param name="targetCoordinate">目标坐标</param>
		/// <returns>源坐标</returns>
		public ICoordinate TargetToSource(ICoordinate targetCoordinate)
		{
			return coordinateTransform.TargetToSource(targetCoordinate);
		}

		/// <summary>
		/// 源坐标转目标坐标(批量)
		/// </summary>
		/// <param name="sourceCoordinates">源坐标集合</param>
		/// <returns>目标坐标集合</returns>
		public List<ICoordinate> SourceToTargetBatch(List<ICoordinate> sourceCoordinates)
		{
			return coordinateTransform.SourceToTargetBatch(sourceCoordinates);
		}

		/// <summary>
		/// 目标坐标转源坐标(批量)
		/// </summary>
		/// <param name="targetCoordiantes">目标坐标集合</param>
		/// <returns>源坐标集合</returns>
		public List<ICoordinate> TargetToSourceBatch(List<ICoordinate> targetCoordiantes)
		{
			return coordinateTransform.TargetToSourceBatch(targetCoordiantes);
		}
	}
}
