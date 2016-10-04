using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace CrazyGIS.GeoPackage
{
	public class TileManager
	{

		/** 不同的连接字符串，生成不同的connection，并使用连接字符串名称来标识**/
		private Dictionary<string, SQLiteConnection> connections = new Dictionary<string, SQLiteConnection>();
		// 连接字符串名称与视图名称的对应关系
		private Dictionary<string, string> views = new Dictionary<string, string>();


		/** 通过单例模式实现只有一个TileManager **/

		#region 单例模式实现

		private volatile static TileManager tileManager;

		private readonly static object locker = new object();
		private TileManager() { }

		public static TileManager GetInstance()
		{
			if (tileManager == null)
			{
				lock (locker)
				{
					if (tileManager == null)
					{
						tileManager = new TileManager();
					}
				}
			}

			return tileManager;
		}

		#endregion

		#region 接口方法
		/// <summary>
		/// 获取TileImage
		/// </summary>
		/// <param name="connectionStringName">config配置文件中的连接字符串名称</param>
		/// <param name="level">层级</param>
		/// <param name="row">行号</param>
		/// <param name="column">列号</param>
		/// <returns>TileImage对象</returns>
		public TileImage GetTileImage(string connectionStringName, int level, int row, int column)
		{
			if(string.IsNullOrEmpty(connectionStringName))
			{
				return this.GetTileImage(level, row, column);
			}
			try
			{
				SQLiteConnection connection = this.getSQLiteConnection(connectionStringName);
				if (connection == null)
				{
					return null;
				}
				string viewName = this.views[connectionStringName];
				byte[] data = this.getTileData(connection, viewName, level, row, column);

				TileImage tileImage = new TileImage(level, row, column, data);
				return tileImage;
			}
			catch (Exception e)
			{
				throw new Exception("读取GeoPackage异常", e);
			}
		}

		/// <summary>
		/// 获取TileImage
		/// </summary>
		/// <param name="level">层级</param>
		/// <param name="row">行号</param>
		/// <param name="column">列号</param>
		/// <returns>TileImage对象</returns>
		public TileImage GetTileImage(int level, int row, int column)
		{
			try
			{
				string connectionStringName = this.getConnectionStringName();
				SQLiteConnection connection = this.getSQLiteConnection(connectionStringName);
				if (connection == null)
				{
					return null;
				}
				string viewName = this.views[connectionStringName];
				byte[] data = this.getTileData(connection, viewName, level, row, column);

				TileImage tileImage = new TileImage(level, row, column, data);
				return tileImage;
			}
			catch(Exception e)
			{
				throw new Exception("读取GeoPackage异常", e);
			}
		}

		/// <summary>
		/// 获取瓦片的二进制数据
		/// </summary>
		/// <param name="connectionStringName">config配置文件中的连接字符串名称</param>
		/// <param name="level">层级</param>
		/// <param name="row">行号</param>
		/// <param name="column">列号</param>
		/// <returns>瓦片二进制数据</returns>
		public byte[] GetTileData(string connectionStringName, int level, int row, int column)
		{
			if (string.IsNullOrEmpty(connectionStringName))
			{
				return this.GetTileData(level, row, column);
			}
			try
			{
				SQLiteConnection connection = this.getSQLiteConnection(connectionStringName);
				if (connection == null)
				{
					return null;
				}
				string viewName = this.views[connectionStringName];
				byte[] data = this.getTileData(connection, viewName, level, row, column);

				return data;
			}
			catch (Exception e)
			{
				throw new Exception("读取GeoPackage异常", e);
			}
		}

		/// <summary>
		/// 获取瓦片的二进制数据
		/// </summary>
		/// <param name="level">层级</param>
		/// <param name="row">行号</param>
		/// <param name="column">列号</param>
		/// <returns>瓦片二进制数据</returns>
		public byte[] GetTileData(int level, int row, int column)
		{
			try
			{
				string connectionStringName = this.getConnectionStringName();
				SQLiteConnection connection = this.getSQLiteConnection(connectionStringName);
				if (connection == null)
				{
					return null;
				}
				string viewName = this.views[connectionStringName];
				byte[] data = this.getTileData(connection, viewName, level, row, column);
				
				return data;
			}
			catch (Exception e)
			{
				throw new Exception("读取GeoPackage异常", e);
			}
		}

		#endregion

		#region 私有方法

		/// <summary>
		/// 获取连接字符串名称
		/// </summary>
		/// <returns></returns>
		private string getConnectionStringName()
		{
			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[0];
			if (settings != null)
			{
				return settings.Name;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获取连接字符串
		/// </summary>
		/// <returns></returns>
		private string getConnectionString()
		{
			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[0];
			if (settings != null)
			{
				return settings.ConnectionString;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获取连接字符串
		/// </summary>
		/// <param name="connectionStringName"></param>
		/// <returns></returns>
		private string getConnectionString(string connectionStringName)
		{
			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (settings != null)
			{
				return settings.ConnectionString;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获取SQLiteConnection
		/// </summary>
		/// <returns></returns>
		private SQLiteConnection getSQLiteConnection(string connectionStringName)
		{
			SQLiteConnection sqliteConnection = null;
			if(!connections.ContainsKey(connectionStringName))
			{
				lock(locker)
				{
					if (!connections.ContainsKey(connectionStringName))
					{
						string connectionString = this.getConnectionString(connectionStringName);
						sqliteConnection = new SQLiteConnection(connectionString);
						if(sqliteConnection != null)
						{
							string viewName = this.getView(sqliteConnection);
							if(!string.IsNullOrEmpty(viewName))
							{
								// connection 与 view 要一一对应： 增加一个connection，同时增加一个view
								connections.Add(connectionStringName, sqliteConnection);
								views.Add(connectionStringName, viewName);
							}
						}
					}
				}
			}
			else
			{
				sqliteConnection = connections[connectionStringName];
			}

			return sqliteConnection;
		}

		/// <summary>
		/// 获取GeoPackage的视图名称
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		private string getView(SQLiteConnection connection)
		{
			string viewName = null;
			if(connection == null)
			{
				return viewName;
			}
			if(connection.State == ConnectionState.Closed)
			{
				connection.Open();
			}
			using (SQLiteCommand command = connection.CreateCommand())
			{
				command.CommandText = "SELECT name FROM sqlite_master WHERE type='view' limit 1";
				command.CommandType = CommandType.Text;

				using (DbDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						viewName = reader["name"].ToString();
					}
				}
			}
			return viewName;
		}

		/// <summary>
		/// 获取瓦片的二进制数据
		/// </summary>
		/// <param name="connection">SQLiteConnection</param>
		/// <param name="viewName">GeoPackage视图名称</param>
		/// <param name="level">层级</param>
		/// <param name="row">行号</param>
		/// <param name="column">列号</param>
		/// <returns>瓦片的二进制数据</returns>
		private byte[] getTileData(SQLiteConnection connection, string viewName, int level, int row, int column)
		{
			if(connection == null || string.IsNullOrEmpty(viewName))
			{
				return null;
			}

			byte[] result = null;

			using (SQLiteCommand command = connection.CreateCommand())
			{
				StringBuilder sql = new StringBuilder();
				sql.Append("SELECT * FROM ");
				sql.Append(viewName);
				sql.Append(" WHERE ZOOM_LEVEL = :TileLevel AND TILE_ROW = :TileRow AND TILE_COLUMN = :TileColumn");

				SQLiteParameter parameter = null;
				parameter = new SQLiteParameter("TileLevel", level);
				command.Parameters.Add(parameter);
				parameter = new SQLiteParameter("TileRow", row);
				command.Parameters.Add(parameter);
				parameter = new SQLiteParameter("TileColumn", column);
				command.Parameters.Add(parameter);

				command.CommandText = sql.ToString();
				command.CommandType = CommandType.Text;

				using (DbDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						result = reader["TILE_DATA"] as byte[];
					}
				}
			}

			return result;
		}

		#endregion
	}
}
