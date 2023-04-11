using MySql.Data.MySqlClient;
using System;
using System.Data;

public class cls_database
{
	private static DataTable table1;

	public static DataTable dtt = new DataTable();

	private static void RecordErro(string str_err)
	{
	}

	private static DataTable column_table()
	{
		if (cls_database.table1 == null)
		{
			cls_database.table1 = cls_database.ReadTable("select * from code_table");
			return cls_database.table1;
		}
		return cls_database.table1;
	}

	public static string conn = "Database=cx-center-new;Data Source=127.0.0.1;User Id=root;Password=Qian913761489!@#;pooling=false;CharSet=utf8;port=3306";
    public static string GetConnectionString()
	{
        return conn;

    }

	public static MySqlConnection GetConnection()
	{
		return new MySqlConnection(cls_database.GetConnectionString());
	}

	public static DataSet GetDataSet(string str_sql)
	{
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, connection);
		MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(MySqlCommand);
		DataSet dataSet = new DataSet();
		connection.Open();
		MySqlTransaction MySqlTransaction = connection.BeginTransaction();
		MySqlCommand.Transaction = MySqlTransaction;
		try
		{
			MySqlDataAdapter.Fill(dataSet);
		}
		catch (Exception ex)
		{
			cls_database.RecordErro(ex.Message);
			MySqlTransaction.Rollback();
		}
		finally
		{
			connection.Close();
		}
		return dataSet;
	}

	public static DataTable GetDataTable(string str_sql)
	{
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, connection);
		MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(MySqlCommand);
		DataTable dataTable = new DataTable();
		connection.Open();
		MySqlTransaction MySqlTransaction = connection.BeginTransaction();
		MySqlCommand.Transaction = MySqlTransaction;
		try
		{
			MySqlDataAdapter.Fill(dataTable);
		}
		catch (Exception ex)
		{
			cls_database.RecordErro(ex.Message);
			MySqlTransaction.Rollback();
		}
		finally
		{
			connection.Close();
		}
		return dataTable;
	}

	public static DataSet Query(string SQLString)
	{
		MySqlConnection connection = cls_database.GetConnection();
		DataSet dataSet = new DataSet();
		try
		{
			connection.Open();
			MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(SQLString, connection);
			MySqlDataAdapter.Fill(dataSet, "ds");
		}
		catch (MySqlException ex)
		{
			throw new Exception(ex.Message);
		}
		finally
		{
			connection.Close();
		}
		return dataSet;
	}

	public static DataSet Query(string SQLString, params MySqlParameter[] cmdParms)
	{
		DataSet result;
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			MySqlCommand MySqlCommand = new MySqlCommand();
			cls_database.PrepareCommand(MySqlCommand, connection, null, SQLString, cmdParms);
			using (MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(MySqlCommand))
			{
				DataSet dataSet = new DataSet();
				try
				{
					MySqlDataAdapter.Fill(dataSet, "ds");
					MySqlCommand.Parameters.Clear();
				}
				catch (MySqlException ex)
				{
					cls_database.RecordErro(ex.Message);
				}
				result = dataSet;
			}
		}
		return result;
	}

	public static MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
	{
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand();
		MySqlDataReader result;
		try
		{
			cls_database.PrepareCommand(MySqlCommand, connection, null, SQLString, cmdParms);
			MySqlDataReader MySqlDataReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			MySqlCommand.Parameters.Clear();
			result = MySqlDataReader;
		}
		catch (MySqlException ex)
		{
			cls_database.RecordErro(ex.Message);
			result = null;
		}
		return result;
	}

	public static MySqlDataReader GetReader(string str_sql, MySqlConnection sqlcon_conn1)
	{
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, sqlcon_conn1);
		return MySqlCommand.ExecuteReader();
	}

	public static int ExecuteSQL(string str_sql)
	{
		int result = -1;
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, connection);
		connection.Open();
		MySqlTransaction MySqlTransaction = connection.BeginTransaction();
		MySqlCommand.Transaction = MySqlTransaction;
		try
		{
			result = MySqlCommand.ExecuteNonQuery();
			MySqlTransaction.Commit();
		}
		catch (Exception)
		{
			MySqlTransaction.Rollback();
		}
		finally
		{
			connection.Close();
		}
		return result;
	}

	public static bool yeornouser(string id)
	{
		return cls_database.selectSQL("select 1 from users where id ='" + id + "'") == 1;
	}
    public static bool isimglanmu(string id)
    {
        return cls_database.selectSQL("select 1 from dict where id ='" + id + "' and isimg=1") == 1;
    }
	public static bool yeorno(string id)
	{
		return cls_database.selectSQL("select 1 from info where id ='" + id + "'") == 1;
	}

	public static bool yeornolanmu(string id)
	{
		return cls_database.selectSQL("select 1 from dict where id ='" + id + "'") == 1;
	}
    

    public static int selectSQL(string str_sql)
	{
		DataTable dataTable = new DataTable();
		MySqlConnection connection = cls_database.GetConnection();
		try
		{
			connection.Open();
			MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(str_sql, connection);
			MySqlDataAdapter.Fill(dataTable);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		finally
		{
			connection.Close();
		}
		return dataTable.Rows.Count;
	}

	public static string zhixingsql(string str_sql)
	{
		string result = "";
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, connection);
		connection.Open();
		MySqlTransaction MySqlTransaction = connection.BeginTransaction();
		MySqlCommand.Transaction = MySqlTransaction;
		try
		{
			MySqlCommand.ExecuteNonQuery();
			MySqlTransaction.Commit();
		}
		catch (Exception ex)
		{
			result = ex.Message;
			MySqlTransaction.Rollback();
		}
		finally
		{
			connection.Close();
		}
		return result;
	}

	public static int ExecuteSql(string SQLString, params MySqlParameter[] cmdParms)
	{
		int result;
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			using (MySqlCommand MySqlCommand = new MySqlCommand())
			{
				try
				{
					cls_database.PrepareCommand(MySqlCommand, connection, null, SQLString, cmdParms);
					int num = MySqlCommand.ExecuteNonQuery();
					MySqlCommand.Parameters.Clear();
					result = num;
				}
				catch (MySqlException ex)
				{
					cls_database.RecordErro(ex.Message);
					result = -1;
				}
			}
		}
		return result;
	}

	public static DataTable ReadTable(string SQLString, MySqlParameter[] cmdParms)
	{
		DataTable dataTable = new DataTable();
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			using (MySqlCommand MySqlCommand = new MySqlCommand())
			{
				try
				{
					cls_database.PrepareCommand(MySqlCommand, connection, SQLString, cmdParms);
					MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(MySqlCommand);
					MySqlDataAdapter.Fill(dataTable);
					MySqlCommand.Parameters.Clear();
				}
				catch (MySqlException ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}
		return dataTable;
	}

	private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, string cmdText, MySqlParameter[] cmdParms)
	{
		if (conn.State != ConnectionState.Open)
		{
			conn.Open();
		}
		cmd.Connection = conn;
		cmd.CommandText = cmdText;
		cmd.CommandType = CommandType.Text;
		if (cmdParms != null)
		{
			for (int i = 0; i < cmdParms.Length; i++)
			{
				MySqlParameter MySqlParameter = cmdParms[i];
				if ((MySqlParameter.Direction == ParameterDirection.InputOutput || MySqlParameter.Direction == ParameterDirection.Input) && MySqlParameter.Value == null)
				{
					MySqlParameter.Value = DBNull.Value;
				}
				cmd.Parameters.Add(MySqlParameter);
			}
		}
	}

	public static DataTable ReadTable(string strSql)
	{
		DataTable dataTable = new DataTable();
		MySqlConnection connection = cls_database.GetConnection();
		try
		{
			connection.Open();
			MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(strSql, connection);
			MySqlDataAdapter.Fill(dataTable);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		finally
		{
			connection.Close();
		}
		return dataTable;
	}

	public static DataTable ReadTabletran(string strSql, ref string errinfo)
	{
		DataTable dataTable = new DataTable();
		MySqlConnection connection = cls_database.GetConnection();
		try
		{
			connection.Open();
			MySqlDataAdapter MySqlDataAdapter = new MySqlDataAdapter(strSql, connection);
			MySqlDataAdapter.Fill(dataTable);
		}
		catch (Exception ex)
		{
			errinfo = ex.Message;
		}
		finally
		{
			connection.Close();
		}
		return dataTable;
	}

	public static int ExecutePRO(string str_pro_name, string str_name, string str_name_value, string output_name, ref string returnValue)
	{
		int result = -1;
		MySqlConnection MySqlConnection = new MySqlConnection(cls_database.GetConnectionString());
		MySqlCommand MySqlCommand = new MySqlCommand();
		MySqlCommand.Connection = MySqlConnection;
		MySqlConnection.Open();
		MySqlCommand.CommandText = str_pro_name;
		MySqlCommand.CommandType = CommandType.StoredProcedure;
		string[] array = str_name.Split(new char[]
		{
			','
		});
		string[] array2 = str_name_value.Split(new char[]
		{
			'!'
		});
		string[] array3 = output_name.Split(new char[]
		{
			','
		});
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i] == ""))
			{
				MySqlCommand.Parameters.Add(new MySqlParameter(array[i], array2[i]));
			}
		}
		for (int j = 0; j < array3.Length; j++)
		{
			if (!(array3[j] == ""))
			{
				MySqlParameter MySqlParameter = new MySqlParameter(array3[j], MySqlDbType.VarChar, 400);
				MySqlParameter.Direction = ParameterDirection.Output;
				MySqlCommand.Parameters.Add(MySqlParameter);
			}
		}
		MySqlTransaction MySqlTransaction = MySqlConnection.BeginTransaction();
		MySqlCommand.Transaction = MySqlTransaction;
		try
		{
			result = MySqlCommand.ExecuteNonQuery();
			for (int k = 0; k < array3.Length; k++)
			{
				if (!(array3[k] == ""))
				{
					returnValue = returnValue + "," + MySqlCommand.Parameters[array3[k]].Value.ToString();
				}
			}
			if (returnValue.StartsWith(","))
			{
				returnValue = returnValue.Substring(1);
			}
			MySqlTransaction.Commit();
		}
		catch (Exception ex)
		{
			returnValue = ex.Message;
			MySqlTransaction.Rollback();
		}
		finally
		{
			MySqlConnection.Close();
		}
		return result;
	}

	public static int ExecuteSP(string spName, params MySqlParameter[] cmdParms)
	{
		int result;
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			using (MySqlCommand MySqlCommand = new MySqlCommand())
			{
				try
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					MySqlCommand.Connection = connection;
					MySqlCommand.CommandText = spName;
					MySqlCommand.CommandType = CommandType.StoredProcedure;
					for (int i = 0; i < cmdParms.Length; i++)
					{
						MySqlCommand.Parameters.Add(cmdParms[i]);
					}
					int num = MySqlCommand.ExecuteNonQuery();
					MySqlCommand.Parameters.Clear();
					result = num;
				}
				catch (MySqlException ex)
				{
					cls_database.RecordErro(ex.Message);
					result = -2;
				}
			}
		}
		return result;
	}

	public static MySqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
	{
		MySqlConnection connection = cls_database.GetConnection();
		MySqlDataReader result = null;
		connection.Open();
		MySqlCommand MySqlCommand = cls_database.BuildQueryCommand(connection, storedProcName, parameters);
		MySqlCommand.CommandType = CommandType.StoredProcedure;
		try
		{
			result = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch (Exception ex)
		{
			cls_database.RecordErro(ex.Message);
			connection.Close();
		}
		return result;
	}

	public static object GetSingle(string SQLString, params MySqlParameter[] cmdParms)
	{
		object result;
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			using (MySqlCommand MySqlCommand = new MySqlCommand())
			{
				try
				{
					cls_database.PrepareCommand(MySqlCommand, connection, null, SQLString, cmdParms);
					object obj = MySqlCommand.ExecuteScalar();
					MySqlCommand.Parameters.Clear();
					if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
					{
						result = null;
					}
					else
					{
						result = obj;
					}
				}
				catch (MySqlException ex)
				{
					cls_database.RecordErro(ex.Message);
					throw ex;
				}
			}
		}
		return result;
	}

	public static object GetSingle(string SQLString)
	{
		object result;
		using (MySqlConnection connection = cls_database.GetConnection())
		{
			using (MySqlCommand MySqlCommand = new MySqlCommand())
			{
				try
				{
					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}
					MySqlCommand.Connection = connection;
					MySqlCommand.CommandText = SQLString;
					MySqlCommand.CommandType = CommandType.Text;
					object obj = MySqlCommand.ExecuteScalar();
					MySqlCommand.Parameters.Clear();
					if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
					{
						result = null;
					}
					else
					{
						result = obj;
					}
				}
				catch (MySqlException ex)
				{
					cls_database.RecordErro(ex.Message);
					result = null;
				}
			}
		}
		return result;
	}

	public static bool Exists(string strSql, params MySqlParameter[] cmdParms)
	{
		object single = cls_database.GetSingle(strSql, cmdParms);
		int result;
		if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
		{
			result = 0;
		}
		else
		{
			result = int.Parse(single.ToString());
		}
		return result != 0;
	}

	public static object GetScalar(string str_sql)
	{
		MySqlConnection connection = cls_database.GetConnection();
		MySqlCommand MySqlCommand = new MySqlCommand(str_sql, connection);
		connection.Open();
		object result = MySqlCommand.ExecuteScalar();
		connection.Close();
		return result;
	}

	private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
	{
		if (conn.State != ConnectionState.Open)
		{
			conn.Open();
		}
		cmd.Connection = conn;
		cmd.CommandText = cmdText;
		if (trans != null)
		{
			cmd.Transaction = trans;
		}
		cmd.CommandType = CommandType.Text;
		if (cmdParms != null)
		{
			for (int i = 0; i < cmdParms.Length; i++)
			{
				MySqlParameter MySqlParameter = cmdParms[i];
				if ((MySqlParameter.Direction == ParameterDirection.InputOutput || MySqlParameter.Direction == ParameterDirection.Input) && MySqlParameter.Value == null)
				{
					MySqlParameter.Value = DBNull.Value;
				}
				cmd.Parameters.Add(MySqlParameter);
			}
		}
	}

	private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName, IDataParameter[] parameters)
	{
		MySqlCommand MySqlCommand = new MySqlCommand(storedProcName, connection);
		MySqlCommand.CommandType = CommandType.StoredProcedure;
		for (int i = 0; i < parameters.Length; i++)
		{
			MySqlParameter MySqlParameter = (MySqlParameter)parameters[i];
			if (MySqlParameter != null)
			{
				if ((MySqlParameter.Direction == ParameterDirection.InputOutput || MySqlParameter.Direction == ParameterDirection.Input) && MySqlParameter.Value == null)
				{
					MySqlParameter.Value = DBNull.Value;
				}
				MySqlCommand.Parameters.Add(MySqlParameter);
			}
		}
		return MySqlCommand;
	}

	

	

	public static bool isbiglanmu(string id)
	{
		return cls_database.GetDataTable("select parentid from dict where id=" + id + " and parentid is null").Rows.Count == 1;
	}
    public static bool hassmalllanmu(string id)
    {
        return cls_database.GetDataTable("select id from dict where parentid="+id+"").Rows.Count == 0;
    }

	public static bool islanmu(string id)
	{
		return cls_database.GetDataTable("select 1 from dict where id=" + id).Rows.Count == 1;
	}

	public static bool islanmuinfo(string id)
	{
		return cls_database.GetDataTable("select 1 from info where id=" + id).Rows.Count == 1;
	}

	public static bool iskeshi(string id)
	{
		return cls_database.GetDataTable("select 1 from dict where id=" + id).Rows.Count == 1;
	}

	public static bool isluntan(string id)
	{
		return cls_database.GetDataTable("select 1 from luntan where id=" + id).Rows.Count == 1;
	}

	

	public static DataTable biglanmu(DataTable dt)
	{
		for (int i = 0; i < dt.Rows.Count; i++)
		{
            if (dt.Rows[i]["parentid"].ToString() == "" && cls_database.selectSQL("select id from dict where parentid=" + dt.Rows[i]["id"] + "")>0)
			{
				if (!cls_database.dtt.Columns.Contains(dt.Rows[i]["id"].ToString()))
				{
					cls_database.dtt.Columns.Add(dt.Rows[i]["id"].ToString());
				}
			}
			else
			{
				cls_database.biglanmu(cls_database.GetDataTable("select * from dict where id ='" + dt.Rows[i]["parentid"] + "'"));
			}
		}
		return cls_database.dtt;
	}
}
