using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.Graphical.Models;

namespace CrazyGIS.Graphical
{
	public class Polyline
	{
		public Polyline()
		{

		}

		#region 公共方法

		/// <summary>
		/// 获取目标点距离目标线最近的点
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		public PlanePoint GetLineNearstPoint(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			return this.getNearstPoint(targetPoint, targetPolyline);
		}

		/// <summary>
		/// 根据目标点，截取线段的后半段
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		public List<PlanePoint> InterceptionPolylineAfterPoint(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			return this.InterceptionPolylineAfter(targetPoint, targetPolyline);
		}

		#endregion

		#region 计算点到线的最近距离的点坐标  辅助方法
		/// <summary>
		/// 获取目标点距离目标线最近的点
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		private PlanePoint getNearstPoint(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			// 线的端点个数必须大于等于2
			if(targetPolyline.Count < 2)
			{
				return null;
			}
			double? nearstDistance = null;
			PlanePoint nearstPoint = null;
			for (int i = 0; i < targetPolyline.Count - 1; i++)
			{
				PlanePoint nPoint = this.pointToLineSegmentNearstPoint(targetPoint, targetPolyline[i], targetPolyline[i + 1]);
				double distance = this.getLength(targetPoint, nPoint);
				if(nearstDistance == null || nearstDistance.Value > distance)
				{
					nearstDistance = distance;
					nearstPoint = nPoint;
				}
			}

			return nearstPoint;
		}

		/// <summary>
		/// 计算点到线段的最近点坐标
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="lineStartPoint">线段起点</param>
		/// <param name="lineEndPoint">线段终点</param>
		/// <returns></returns>
		private PlanePoint pointToLineSegmentNearstPoint(PlanePoint targetPoint, PlanePoint lineStartPoint, PlanePoint lineEndPoint)
		{
			double deltaX = lineEndPoint.x - lineStartPoint.x;
			double deltaY = lineEndPoint.y - lineStartPoint.y;
			double som = deltaX * deltaX + deltaY * deltaY;
			double u = ((targetPoint.x - lineStartPoint.x) * deltaX + (targetPoint.y - lineStartPoint.y) * deltaY) / som;
			if (u > 1)
			{
				u = 1;
			}
			else if (u < 0)
			{
				u = 0;
			}

			double resultX = lineStartPoint.x + u * deltaX;
			double resultY = lineStartPoint.y + u * deltaY;

			PlanePoint result = new PlanePoint();
			result.x = resultX;
			result.y = resultY;

            return result;
		}

		/// <summary>
		/// 获取两点之间的长度
		/// </summary>
		/// <param name="point1">起点</param>
		/// <param name="point2">终点</param>
		/// <returns>长度值</returns>
		private double getLength(PlanePoint point1, PlanePoint point2)
		{
			double deltaX = point2.x - point1.x;
			double deltaY = point2.y - point1.y;
			// 两点之间的距离公式：( (x2-x1)^2 + (y2-y1)^2 )^(1/2)
			double length = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

			return length;
		}

		#endregion

		#region 截取线段  辅助方法

		/// <summary>
		/// 根据目标点，截取线段的后半段
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		private List<PlanePoint> InterceptionPolylineAfter(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			// 最近点下一点的索引值
			int nextPointIndex = this.getNearstNextPointIndex(targetPoint, targetPolyline);
			if(nextPointIndex < 0)
			{
				return null;
			}
			PlanePoint nearstPoint = this.getNearstPoint(targetPoint, targetPolyline);
			// 添加最近的点
			List<PlanePoint> resultPolyline = new List<PlanePoint>();
			resultPolyline.Add(nearstPoint);

			// 循环添加线中的符合条件的点
			for (int i = nextPointIndex; i < targetPolyline.Count; i++)
			{
				resultPolyline.Add(targetPolyline[i]);
			}
			
			return resultPolyline;
		}

		/// <summary>
		/// 根据目标点，截取线段的前半段
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		private List<PlanePoint> interceptionPolylineFront(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			// 最近点下一点的索引值
			int nextPointIndex = this.getNearstNextPointIndex(targetPoint, targetPolyline);
			if (nextPointIndex < 0)
			{
				return null;
			}
			PlanePoint nearstPoint = this.getNearstPoint(targetPoint, targetPolyline);
			// 添加最近的点
			List<PlanePoint> resultPolyline = new List<PlanePoint>();
			resultPolyline.Add(nearstPoint);

			// 循环添加线中的符合条件的点
			for (int i = nextPointIndex; i < targetPolyline.Count; i++)
			{
				resultPolyline.Add(targetPolyline[i]);
			}

			return resultPolyline;
		}

		/// <summary>
		/// 获取目标点距离目标线最近的点的下一个点坐标的索引
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolyline">目标线</param>
		/// <returns></returns>
		private int getNearstNextPointIndex(PlanePoint targetPoint, List<PlanePoint> targetPolyline)
		{
			// 线的端点个数必须大于等于2
			if (targetPolyline.Count < 2)
			{
				return -1;
			}
			double? nearstDistance = null;
			int index = -1;
			for (int i = 0; i < targetPolyline.Count - 1; i++)
			{
				PlanePoint nPoint = this.pointToLineSegmentNearstPoint(targetPoint, targetPolyline[i], targetPolyline[i + 1]);
				double distance = this.getLength(targetPoint, nPoint);
				if (nearstDistance == null || nearstDistance.Value > distance)
				{
					nearstDistance = distance;
					index = i + 1;
                }
			}

			return index;
		}

		#endregion
	}
}
