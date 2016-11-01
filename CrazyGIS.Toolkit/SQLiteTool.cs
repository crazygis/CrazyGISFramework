using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace CrazyGIS.Toolkit
{
	public class SQLiteTool
	{
		/// <summary>
		/// 获取数据库连接对象
		/// </summary>
		/// <param name="dbFileFullName">数据库文件完整路径</param>
		/// <returns></returns>
		public static SQLiteConnection GetConnection(string dbFileFullName)
		{
			SQLiteConnection sqliteConnection = null;
			try
			{
				string connectionString = "Data Source=" + dbFileFullName + ";Pooling=true;FailIfMissing=false;";
				sqliteConnection = new SQLiteConnection(connectionString);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				sqliteConnection = null;
			}

			return sqliteConnection;
		}

		/// <summary>
		/// 执行SQL语句
		/// </summary>
		/// <param name="connection">数据库连接对象</param>
		/// <param name="sql">SQL语句</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public static bool ExecuteNonQuery(SQLiteConnection connection, string sql, Dictionary<string, object> parameters = null)
		{
			bool result = false;

			using (SQLiteCommand command = connection.CreateCommand())
			{
				command.CommandText = sql;
				command.CommandType = CommandType.Text;

				if (parameters != null && parameters.Count > 0)
				{
					SQLiteParameter parameter = null;
					foreach (string paramKey in parameters.Keys)
					{
						object paramValue = parameters[paramKey];

						parameter = new SQLiteParameter(paramKey, paramValue);
						command.Parameters.Add(parameter);
					}
				}

				int rowCount = command.ExecuteNonQuery();
				result = rowCount != 0;
			}

			return result;
		}
	}
}
