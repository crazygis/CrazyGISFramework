using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.Graphical.Models;

namespace CrazyGIS.Graphical
{
    public class Polygon
    {
        public Polygon()
        {

        }

		#region 公共方法

		/// <summary>
		/// 判断点是否在多边形内(适用于任意多边形)
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolygon">多边形点集合(按照顺时针或逆时针顺序存储多边形的点)</param>
		/// <returns>true or false</returns>
		public bool InPolygon(PlanePoint targetPoint, List<PlanePoint> targetPolygon)
        {
			return this.IsPointInPolygon(targetPoint, targetPolygon);
        }

		/// <summary>
		/// 计算多边形的中心点
		/// </summary>
		/// <param name="polygonPoints">多边形顶点集合(封闭，最后一个点和起始点相同)</param>
		/// <returns></returns>
		public PlanePoint GetPolygonCenterPoint(List<PlanePoint> polygonPoints)
		{
			return this.getCenterPoint(polygonPoints);
		}

		/// <summary>
		/// 计算多边形的面积
		/// </summary>
		/// <param name="polygonPoints">多边形顶点集合(封闭，最后一个点和起始点相同)</param>
		/// <returns></returns>
		public double GetPolygonArea(List<PlanePoint> polygonPoints)
		{
			return this.getArea(polygonPoints);
		}

		#endregion

		#region 计算点是否在多边形内的辅助方法

		/// <summary>
		/// 判断点是否在多边形内(适用于任意多边形)
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="targetPolygon">多边形点集合(按照顺时针或逆时针顺序存储多边形的点)</param>
		/// <returns>true or false</returns>
		private bool IsPointInPolygon(PlanePoint targetPoint, List<PlanePoint> targetPolygon)
		{
			// 多边形的点必须大于等于3,最后一个点和起点相同,所以长度需要大于等于4
			if (targetPolygon.Count < 4)
			{
				return false;
			}
			// 点在第一条线段的左边(1),右边(-1),线上(0)
			int first = 0;
			// 点在其他线段的左边(1),右边(-1),线上(0)
			int other = 0;
			// 角度合计
			double angleSummation = 0;

			// 循环多边形的点
			for (int i = 0, j = i + 1; i < targetPolygon.Count - 1; i++, j++)
			{
				PlanePoint lineStartPoint = targetPolygon[i]; // 线段起点
				PlanePoint lineEndPoint = targetPolygon[j];   // 线段终点
														 // 首先判断点是否在线段上，如果是则直接返回true
				if (this.pointAtSegment(targetPoint, lineStartPoint, lineEndPoint))
				{
					return true;
				}
				// 角度
				double angle = this.getTriangleTopAngle(targetPoint, lineStartPoint, lineEndPoint);
				// 位置
				int position = this.pointLeftPolyline(targetPoint, lineStartPoint, lineEndPoint);

				if (first == 0)  // 如果点在第一条线上，则将点相对于下一条线的位置作为标记
				{
					first = position;
				}
				else    // 点相对于当前线的位置
				{
					other = position;
				}

				if (first == position)  // 如果点相对于第一条线的位置和当前线的位置一致，则角度相加。
				{
					angleSummation += angle;
				}
				else                   // 如果点相对于第一条线的位置和当前线的位置不一致，则角度相减。(处理凹多边形的情况)
				{
					angleSummation -= angle;
				}
			}

			// 如果角度之和等于360度，表示点在多边形内
			if (Math.Round(angleSummation, 5) == 360)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 判断点相对于线的方位
		/// </summary>
		/// <param name="targetPoint">目标点</param>
		/// <param name="lineStartPoint">线的起点</param>
		/// <param name="lineEndPoint">线的终点</param>
		/// <returns>1:左边，0：线上，-1：右边</returns>
		private int pointLeftPolyline(PlanePoint targetPoint, PlanePoint lineStartPoint, PlanePoint lineEndPoint)
        {
            double x0 = targetPoint.x;
			double y0 = targetPoint.y;
			double x1 = lineStartPoint.x;
			double y1 = lineStartPoint.y;
			double x2 = lineEndPoint.x;
			double y2 = lineEndPoint.y;
			double temp = (x2 - x1) * (y0 - y1) - (y2 - y1) * (x0 - x1);
            int result = 0;
            if (temp > 0)  // 点在线左边
            {
                result = 1;
            }
            else if (temp == 0)  // 点在线上
            {
                result = 0;
            }
            else                // 点在线右边
            {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// 点是否在线段上
        /// </summary>
        /// <param name="targetPoint">目标点</param>
        /// <param name="lineStartPoint">线段起点</param>
        /// <param name="lineEndPoint">线段终点</param>
        /// <returns></returns>
        private bool pointAtSegment(PlanePoint targetPoint, PlanePoint lineStartPoint, PlanePoint lineEndPoint)
        {
			// 目标点对应的边的长度
			double topPoint_Side_Length = this.getLength(lineStartPoint, lineEndPoint);
			// lineStartPoint对应的边的长度
			double point1_Side_Length = this.getLength(targetPoint, lineEndPoint);
			// lineEndPoint对应的边的长度
			double point2_Side_Length = this.getLength(targetPoint, lineStartPoint);

            if ((point1_Side_Length + point2_Side_Length) == topPoint_Side_Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取三角形顶点所在的角度值
        /// </summary>
        /// <param name="topPoint">三角形顶点坐标</param>
        /// <param name="point1">三角形另一点</param>
        /// <param name="point2">三角形另一点</param>
        /// <returns>角度值</returns>
        private double getTriangleTopAngle(PlanePoint topPoint, PlanePoint point1, PlanePoint point2)
        {
			// 计算顶点角的余弦值
			double cosValue = this.getAngleCos(topPoint, point1, point2);
			// 计算角度值
			double angle = Math.Acos(cosValue) * 180 / Math.PI;

            return angle;
        }
        
        /// <summary>
        /// 获取角度的余弦值
        /// </summary>
        /// <param name="topPoint">顶点</param>
        /// <param name="point1">起点</param>
        /// <param name="point2">终点</param>
        /// <returns>余弦值</returns>
        private double getAngleCos(PlanePoint topPoint, PlanePoint point1, PlanePoint point2)
        {
            // 顶点对应的边的长度
            double topPoint_Side_Length = this.getLength(point1, point2);
			// point1对应的边的长度
			double point1_Side_Length = this.getLength(topPoint, point2);
			// point2对应的边的长度
			double point2_Side_Length = this.getLength(topPoint, point1);
			// 顶点余弦公式：cosA=(b^2+c^2-a^2)/(2*b*c)
			double cosValue = (Math.Pow(point1_Side_Length, 2) + Math.Pow(point2_Side_Length, 2) - Math.Pow(topPoint_Side_Length, 2)) / (2 * point1_Side_Length * point2_Side_Length);

            return cosValue;
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

		#region 计算多边形中心点的辅助方法

		/// <summary>
		/// 计算多边形的中心点
		/// </summary>
		/// <param name="polygonPoints">多边形顶点集合(封闭，最后一个点和起始点相同)</param>
		/// <returns></returns>
		private PlanePoint getCenterPoint(List<PlanePoint> polygonPoints)
		{
			// 多边形的点必须大于等于3,最后一个点和起点相同,所以长度需要大于等于4
			if (polygonPoints.Count < 4)
			{
				return null;
			}

			double centerX = 0;
			double centerY = 0;

			// 循环多边形的顶点(因最后一个点与第一个点相同，故排除在外)
			for (int i = 0; i < polygonPoints.Count - 1; i++)
			{
				PlanePoint p1 = polygonPoints[i];
				PlanePoint p2 = polygonPoints[i + 1];

				centerX += (p1.x + p2.x) * (p1.x * p2.y - p2.x * p1.y);
				centerY += (p1.y+p2.y) * (p1.x * p2.y - p2.x * p1.y);
			}

			double polygonArea = this.getArea(polygonPoints);
			centerX /= (6 * polygonArea);
			centerY /= (6 * polygonArea);

			PlanePoint centerPoint = new PlanePoint();
			centerPoint.x = centerX;
			centerPoint.y = centerY;

			return centerPoint;
		}

		#endregion

		#region 计算多边形面积的辅助方法

		/// <summary>
		/// 计算多边形的面积
		/// </summary>
		/// <param name="polygonPoints">多边形顶点集合(封闭，最后一个点和起始点相同)</param>
		/// <returns></returns>
		private double getArea(List<PlanePoint> polygonPoints)
		{
			// 多边形的点必须大于等于3,最后一个点和起点相同,所以长度需要大于等于4
			if (polygonPoints.Count < 4)
			{
				return 0;
			}

			double area = 0;

			// 循环多边形的顶点(因最后一个点与第一个点相同，故排除在外)
			for(int i = 0; i < polygonPoints.Count - 1; i++)
			{
				PlanePoint p1 = polygonPoints[i];
				PlanePoint p2 = polygonPoints[i + 1];

				double subArea = p1.x * p2.y - p2.x * p1.y;
				area += subArea;
			}

			area /= 2;

			return area;
		}
		#endregion
	}
}
