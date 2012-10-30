using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace FW.CommonFunction
{
    /// <summary>
    /// DBOperatorBase 的摘要说明。
    /// </summary>
    public class SqlHelper : IDisposable
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// 配置节点名称
        /// </summary>
        public static string AppSettingsConnectionString = "SqlConnectionString";

        private string connectionString;
        private SqlConnection connection;
        private SqlTransaction transaction;

        //参数ArrayList集合
        private ArrayList parameters = new ArrayList();

        //默认为true, 自动事务
        private bool bAutoTransaction;

        //默认为true, 自动连接数据库、自动断开数据库
        private bool bAutoConnection;

        /// <summary>
        /// 设置SQL超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 属性，返回 AddParameter 方法增加的所有属性集合
        /// </summary>
        public ArrayList Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// 数据库连接对象。
        /// </summary>
        public SqlConnection Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// 属性，返回当前连接（Connection）的事务对象。
        /// 说明：调用BeginTransaction方法后，有效。
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return this.transaction; }
        }

        /// <summary>
        /// 是否为自动事务，默认是
        /// </summary>
        public bool AutoTransaction
        {
            get { return this.bAutoTransaction; }
            set { this.bAutoTransaction = value; }
        }

        /// <summary>
        /// 是否采用自动连接数据库、断开数据库，默认是
        /// </summary>
        public bool AutoConnection
        {
            get { return this.bAutoConnection; }
            set { this.bAutoConnection = value; }
        }

        #region DBOperatorBase构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SqlHelper()
        {
            this.bAutoTransaction = false;
            this.bAutoConnection = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="intTimeout">SQL语句执行超时时间</param>
        public SqlHelper(int intTimeout)
        {
            this.bAutoTransaction = false;
            this.bAutoConnection = true;

            this.Timeout = intTimeout;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="autoTransaction">是否自动管理事务</param>
        /// <param name="autoConnection">是否自动管理连接</param>
        public SqlHelper(bool autoTransaction, bool autoConnection)
        {
            this.bAutoTransaction = autoTransaction;
            this.bAutoConnection = autoConnection;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public SqlHelper(string connectionString)
        {
            this.bAutoTransaction = false;
            this.bAutoConnection = true;
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="autoTransaction">是否自动管理事务</param>
        /// <param name="autoConnection">是否自动管理连接</param>
        public SqlHelper(string connectionString, bool autoTransaction, bool autoConnection)
        {
            this.bAutoTransaction = autoTransaction;
            this.bAutoConnection = autoConnection;
            this.connectionString = connectionString;
        }
        #endregion

        #region 数据库连接打开/关闭

        /// <summary>
        /// 数据库连接，如果连接不上数据库服务器，则抛出异常
        /// </summary>
        public void OpenConnection()
        {
            if (this.connectionString == null || this.connectionString == String.Empty)
            {
                //System.Configuration.AppSettingsReader objConfig = new System.Configuration.AppSettingsReader();
                //this.connectionString = objConfig.GetValue(AppSettingsConnectionString, typeof(string)).ToString();

                this.connectionString = new ConnectionInfo().GetSqlConnectionString();
            }
            OpenConnection(this.connectionString);
        }

        /// <summary>
        /// 数据库连接，如果连接不上数据库服务器，则抛出异常
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>打开数据库的连接</returns>
        public void OpenConnection(string connectionString)
        {
            try
            {
                if (this.connection == null)
                    this.connection = new SqlConnection(connectionString);
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine(string.Format("打开数据库连接失败, 连接字符串:{0}", connectionString));
                throw ex;
            }
        }

        /// <summary>
        /// 关闭数据库连接，不抛出异常
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                this.connection.Close();
                this.connection.Dispose();
            }
            catch (Exception)
            { }
            finally
            {
                this.connection = null;
            }
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (this.connection == null)
            {
                this.OpenConnection();
            }
            this.transaction = this.connection.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (this.transaction != null)
                    this.transaction.Commit();
            }
            finally
            {
                this.transaction = null;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (this.transaction != null)
                    this.transaction.Rollback();
            }
            finally
            {
                this.transaction = null;
            }
        }
        #endregion

        #region 创建一个SqlCommand对象
        /// <summary>
        /// 属性，创建一个SqlCommand对象（当前连接Connection进行创建，
        /// 并且根据是否开启事务，自动给Command.Transaction对象赋值。）
        /// 说明：调用OpenConnection方法后有效，否则会引起异常。
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand()
        {
            SqlCommand cmd = this.connection.CreateCommand();
            if (this.Timeout > 0)
            {
                cmd.CommandTimeout = this.Timeout;
            }
            cmd.Transaction = this.Transaction;
            return cmd;
        }
        #endregion

        #region 增加/清空参数

        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数
        /// </summary>
        /// <param name="param">SqlParameter对象</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter AddParameter(SqlParameter param)
        {
            if (this.parameters == null)
                this.parameters = new ArrayList();

            this.parameters.Add(param);
            return param;
        }
        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数。
        /// </summary>
        /// <param name="parameters">SqlParameter对象</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter[] AddParameter(params  SqlParameter[] parameters)
        {
            if (this.parameters == null)
                this.parameters = new ArrayList();
            foreach (SqlParameter param in parameters)
            {
                this.parameters.Add(param);
            }

            return parameters;
        }
        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数。
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="val">参数值</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter AddParameter(string parameterName, object val)
        {
            return this.AddParameter(parameterName, SqlDbType.VarChar, 4000, val, ParameterDirection.Input);
        }
        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数。
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="val">参数值</param>
        /// <param name="direction">参数输出方向</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter AddParameter(string parameterName, object val, ParameterDirection direction)
        {
            return this.AddParameter(parameterName, SqlDbType.VarChar, 4000, val, direction);
        }
        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数。
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小（小于等于0，表示采用默认值）</param>
        /// <param name="val">参数值</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter AddParameter(string parameterName, SqlDbType dbType, int size, object val)
        {
            return AddParameter(parameterName, dbType, size, val, ParameterDirection.Input);
        }

        /// <summary>
        /// 增加参数
        /// 说明：增加参数后，执行所有数据库操作命令（如Fill、ExecuteNonQuery等）时，将使用增加的参数。
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">参数大小（小于等于0，表示采用默认值）</param>
        /// <param name="val">参数值</param>
        /// <param name="direction">参数输出方向</param>
        /// <returns>返回SqlParameter对象</returns>
        public SqlParameter AddParameter(string parameterName, SqlDbType dbType, int size, object val, ParameterDirection direction)
        {
            if (this.parameters == null)
                this.parameters = new ArrayList();

            SqlParameter sqlPar = new SqlParameter();
            sqlPar.ParameterName = parameterName;
            sqlPar.SqlDbType = dbType;
            if (size > 0)
                sqlPar.Size = size;
            sqlPar.Direction = direction;
            if (val != null)
            {
                sqlPar.Value = val;
            }
            this.parameters.Add(sqlPar);
            return sqlPar;
        }

        /// <summary>
        /// 清空AddParameter方法增加的所有参数
        /// </summary>
        public void ClearParameter()
        {
            if (this.parameters == null)
                this.parameters = new ArrayList();
            else
                this.parameters.Clear();
        }
        #endregion

        #region 执行ExecuteNonQuery(insert、update、delete)SQL语句
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>返回受影响行数</returns>
        public int ExecuteNonQuery(string strSQL)
        {
            return ExecuteNonQuery(strSQL, CommandType.Text, null);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <returns>返回受影响行数</returns>
        public int ExecuteNonQuery(string strSQL, CommandType commandType)
        {
            return ExecuteNonQuery(strSQL, commandType, null);
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSQL">SQL查询语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="parameters">参数类型ArrayList，允许为空</param>
        /// <returns>返回受影响行数</returns>
        public int ExecuteNonQuery(string strSQL, CommandType commandType, ArrayList parameters)
        {
            int ret = 0;
            SqlCommand command = null;
            bool bInAutoConnection = false;
            bool bInAutoTransaction = false;

            try
            {
                //判断是否自动连接数据库
                if (this.AutoConnection)
                {
                    if (this.connection == null || this.connection.State != ConnectionState.Open)
                    {
                        OpenConnection();
                        bInAutoConnection = true;
                    }
                }

                command = this.connection.CreateCommand();
                if (this.Timeout > 0)
                {
                    command.CommandTimeout = this.Timeout;
                }

                //判断是否自动开始事务
                if (this.AutoTransaction && this.transaction == null)
                {
                    BeginTransaction();
                    bInAutoTransaction = true;
                }
                if (this.transaction != null)
                    command.Transaction = this.transaction;

                command.CommandText = strSQL;
                command.CommandType = commandType;

                //传参数
                if (parameters != null && parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[parameters.Count];
                    for (int i = 0, j = parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)parameters[i]).Clone();
                    }

                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                else if (this.parameters != null && this.parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[this.parameters.Count];
                    for (int i = 0, j = this.parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)this.parameters[i]).Clone();
                    }
                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                ret = command.ExecuteNonQuery();

                //判断是否自动结束事务
                if (bInAutoTransaction)
                {
                    this.CommitTransaction();
                    bInAutoTransaction = false;
                }
            }
            catch (Exception ex)
            {
                if (bInAutoTransaction)
                {
                    bInAutoTransaction = false;
                    this.RollbackTransaction();
                }
                if (commandType == CommandType.Text)
                {
                    //发生异常，显示替换参数后的完成 SQL 语句
                    if (parameters != null && parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, parameters);
                    else if (this.parameters != null && this.parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, this.parameters);

                    TraceLog.WriteLine(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL));

                    throw new Exception(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL), ex);
                }
                else
                {
                    string sParameters = "";
                    if (command != null)
                    {
                        string sep = "";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            if (param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.NChar)
                                sep = "'";
                            else
                                sep = "";

                            if (sParameters != String.Empty)
                                sParameters += ", ";
                            sParameters += param.ParameterName + "=" + sep + param.Value + sep;
                        }
                        if (sParameters != String.Empty)
                            sParameters = "(" + sParameters + ")";
                    }

                    TraceLog.WriteLine(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters));
                    throw new Exception(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters), ex);
                }
            }
            finally
            {
                try
                {
                    command.Dispose();
                }
                catch (Exception)
                { }

                //判断是否自动断开连接
                if (bInAutoConnection)
                    CloseConnection();
            }
            return ret;
        }


        #endregion

        #region 执行ExecuteScalar()SQL语句
        /// <summary>
        /// 根据SQL查询语句，返回一个查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <returns>返回SQL结果集中的第一行、第一列数据值</returns>
        public object ExecuteScalar(string strSQL)
        {
            return ExecuteScalar(strSQL, CommandType.Text, null);
        }
        /// <summary>
        /// 根据SQL查询语句，返回一个查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <returns>返回SQL结果集中的第一行、第一列数据值</returns>
        public object ExecuteScalar(string strSQL, CommandType commandType)
        {
            return ExecuteScalar(strSQL, commandType, null);
        }
        /// <summary>
        /// 根据SQL查询语句，返回一个查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="parameters">参数列表类型ArrayList，允许为空</param>
        /// <returns>返回SQL结果集中的第一行、第一列数据值</returns>
        public object ExecuteScalar(string strSQL, CommandType commandType, ArrayList parameters)
        {
            object ret;
            SqlCommand command = null;
            bool bInAutoConnection = false;
            bool bInAutoTransaction = false;

            try
            {
                //判断是否自动连接数据库
                if (this.AutoConnection)
                {
                    if (this.connection == null || this.connection.State != ConnectionState.Open)
                    {
                        OpenConnection();
                        bInAutoConnection = true;
                    }
                }

                command = this.connection.CreateCommand();
                if (this.Timeout > 0)
                {
                    command.CommandTimeout = this.Timeout;
                }

                //判断是否自动开启事务
                if (this.AutoTransaction && this.transaction == null)
                {
                    BeginTransaction();
                    bInAutoTransaction = true;
                }
                if (this.transaction != null)
                    command.Transaction = this.transaction;

                command.CommandText = strSQL;
                command.CommandType = commandType;

                //传参数
                if (parameters != null && parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[parameters.Count];
                    for (int i = 0, j = parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)parameters[i]).Clone();
                    }

                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                else if (this.parameters != null && this.parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[this.parameters.Count];
                    for (int i = 0, j = this.parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)this.parameters[i]).Clone();
                    }
                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                ret = command.ExecuteScalar();

                //判断是否自动结束事务
                if (bInAutoTransaction)
                {
                    this.CommitTransaction();
                    bInAutoTransaction = false;
                }
            }
            catch (Exception ex)
            {
                if (bInAutoTransaction)
                {
                    bInAutoTransaction = false;
                    this.RollbackTransaction();
                }

                if (commandType == CommandType.Text)
                {
                    //发生异常，显示替换参数后的完成 SQL 语句
                    if (parameters != null && parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, parameters);
                    else if (this.parameters != null && this.parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, this.parameters);
                    TraceLog.WriteLine(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL));
                    throw new Exception(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL), ex);
                }
                else
                {
                    string sParameters = "";
                    if (command != null)
                    {
                        string sep = "";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            if (param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.NChar)
                                sep = "'";
                            else
                                sep = "";

                            if (sParameters != String.Empty)
                                sParameters += ", ";
                            sParameters += param.ParameterName + "=" + sep + param.Value + sep;
                        }
                        if (sParameters != String.Empty)
                            sParameters = "(" + sParameters + ")";
                    }
                    TraceLog.WriteLine(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters));
                    throw new Exception(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters), ex);
                }
            }
            finally
            {
                try
                {
                    if (command != null)
                        command.Dispose();
                }
                catch (Exception)
                { }

                //判断是否自动断开连接
                if (bInAutoConnection)
                    CloseConnection();
            }
            return ret;
        }
        #endregion

        #region 执行SQL，返回SqlDataReader
        /// <summary>
        /// 执行SQL，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>返回一个只读的对象（SqlDataReader）</returns>
        public SqlDataReader ExecuteReader(string strSQL)
        {
            return ExecuteReader(strSQL, CommandType.Text, null);
        }
        /// <summary>
        /// 执行SQL，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <returns>返回一个只读的对象（SqlDataReader）</returns>
        public SqlDataReader ExecuteReader(string strSQL, CommandType commandType)
        {
            return ExecuteReader(strSQL, commandType, null);
        }

        /// <summary>
        /// 执行SQL，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="parameters">允许为空 参数类型ArrayList，允许为空</param>
        /// <returns>返回一个只读的对象（SqlDataReader）</returns>
        public SqlDataReader ExecuteReader(string strSQL, CommandType commandType, ArrayList parameters)
        {
            SqlDataReader ret = null;
            SqlCommand command = null;
            bool bInAutoTransaction = false;

            try
            {
                //判断是否自动连接数据库
                if (this.AutoConnection)
                {
                    if (this.connection == null || this.connection.State != ConnectionState.Open)
                    {
                        OpenConnection();
                    }
                }

                command = this.connection.CreateCommand();
                if (this.Timeout > 0)
                {
                    command.CommandTimeout = this.Timeout;
                }

                //判断是否自动开启事务
                if (this.AutoTransaction && this.transaction == null)
                {
                    BeginTransaction();
                    bInAutoTransaction = true;
                }
                if (this.transaction != null)
                    command.Transaction = this.transaction;

                command.CommandText = strSQL;
                command.CommandType = commandType;

                //传参数
                if (parameters != null && parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[parameters.Count];
                    for (int i = 0, j = parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)parameters[i]).Clone();
                    }

                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                else if (this.parameters != null && this.parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[this.parameters.Count];
                    for (int i = 0, j = this.parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)this.parameters[i]).Clone();
                    }
                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                ret = command.ExecuteReader(CommandBehavior.CloseConnection);

                //判断是否自动结束事务
                if (bInAutoTransaction)
                {
                    this.CommitTransaction();
                    bInAutoTransaction = false;
                }
            }
            catch (Exception ex)
            {
                if (bInAutoTransaction)
                {
                    bInAutoTransaction = false;
                    this.RollbackTransaction();
                }

                if (commandType == CommandType.Text)
                {
                    //发生异常，显示替换参数后的完成 SQL 语句
                    if (parameters != null && parameters.Count > 0)
                    {
                        strSQL = this.GetReplaceSql(strSQL, parameters);
                    }
                    else if (this.parameters != null && this.parameters.Count > 0)
                    {
                        strSQL = this.GetReplaceSql(strSQL, this.parameters);
                    }

                    TraceLog.WriteLine(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL));

                    throw new Exception(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL), ex);
                }
                else
                {
                    string sParameters = "";
                    if (command != null)
                    {
                        string sep = "";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            if (param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.NChar)
                            {
                                sep = "'";
                            }
                            else
                            {
                                sep = "";
                            }

                            if (sParameters != String.Empty)
                            {
                                sParameters += ", ";
                            }
                            sParameters += param.ParameterName + "=" + sep + param.Value + sep;
                        }
                        if (sParameters != String.Empty)
                        {
                            sParameters = "(" + sParameters + ")";
                        }
                    }

                    TraceLog.WriteLine(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters));

                    throw new Exception(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters), ex);
                }
            }
            finally
            {
                try
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
                catch (Exception)
                {
                }
            }
            return ret;
        }
        #endregion

        #region 执行SQL，返回DataTable
        /// <summary>
        /// 执行SQL，返回DataTable
        /// </summary>
        /// <param name="strSQL">SQL查询语句</param>
        /// <returns>返回查询结果集</returns>
        public DataTable FillDataTable(string strSQL)
        {
            DataSet data = new DataSet();
            Fill(strSQL, CommandType.Text, null, data, null);
            if (data.Tables.Count > 0)
            {
                return data.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// 执行SQL，返回DataTable(分页存储过程DoSplitPage不要调用该方法)
        /// </summary>
        /// <param name="strSQL">SQL查询语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <returns>返回查询结果集</returns>
        public DataTable FillDataTable(string strSQL, CommandType commandType)
        {
            DataSet data = new DataSet();
            Fill(strSQL, commandType, null, data, null);
            if (data.Tables.Count > 0)
            {
                return data.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        #endregion

        #region 执行SQL，填充DataSet

        /// <summary>
        /// 执行SQL，返回一个Dataset
        /// </summary>
        /// <param name="strSQL">SQL查询语句</param>
        /// <returns>返回查询结果集</returns>
        public DataSet Fill(string strSQL)
        {
            DataSet data = new DataSet();
            Fill(strSQL, CommandType.Text, null, data, null);
            return data;
        }

        /// <summary>
        /// 执行SQL，返回一个Dataset
        /// </summary>
        /// <param name="strSQL">SQL查询语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <returns>返回查询结果集</returns>
        public DataSet Fill(string strSQL, CommandType commandType)
        {
            DataSet data = new DataSet();
            Fill(strSQL, commandType, null, data, null);
            return data;
        }
        /// <summary>
        /// 根据SQL查询语句，返回查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="dataSet">输出参数，DataSet数据集</param>
        /// <returns>返回受影响行数</returns>
        public int Fill(string strSQL, CommandType commandType, DataSet dataSet)
        {
            return Fill(strSQL, commandType, null, dataSet, null);
        }

        /// <summary>
        /// 根据SQL查询语句，返回查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="srcTable">DataSet数据集映射表不能为空</param>
        /// <param name="dataSet">输出参数，DataSet数据集</param>
        /// <returns>返回受影响行数</returns>
        public int Fill(string strSQL, CommandType commandType, string srcTable, DataSet dataSet)
        {
            if (srcTable == null || srcTable == String.Empty)
            {
                TraceLog.WriteLine("参数srcTable不能为空！");
                throw new Exception("参数srcTable不能为空！");
            }
            return Fill(strSQL, commandType, srcTable, dataSet, null);
        }

        /// <summary>
        /// 根据SQL查询语句，返回查询结果
        /// </summary>
        /// <param name="strSQL">strSQL语句</param>
        /// <param name="commandType">CommandType类型</param>
        /// <param name="srcTable">DataSet数据集映射表名，不能为空</param>
        /// <param name="dataSet">输出参数，DataSet数据集</param>
        /// <param name="parameters">参数类型为ArrayList，允许为空</param>
        /// <returns>返回受影响行数</returns>
        public int Fill(string strSQL, CommandType commandType, string srcTable, DataSet dataSet, ArrayList parameters)
        {
            int ret;
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            bool bInAutoConnection = false;
            bool bInAutoTransaction = false;

            try
            {
                //判断是否自动连接数据库
                if (this.AutoConnection)
                {
                    if (this.connection == null || this.connection.State != ConnectionState.Open)
                    {
                        OpenConnection();
                        bInAutoConnection = true;
                    }
                }

                command = this.connection.CreateCommand();
                if (this.Timeout > 0)
                {
                    command.CommandTimeout = this.Timeout;
                }

                //判断是否自动开启事务
                if (this.AutoTransaction && this.transaction == null)
                {
                    BeginTransaction();
                    bInAutoTransaction = true;
                }
                if (this.transaction != null)
                    command.Transaction = this.transaction;

                command.CommandText = strSQL;
                command.CommandType = commandType;
                adapter = new SqlDataAdapter(command);

                //传参数
                if (parameters != null && parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[parameters.Count];
                    for (int i = 0, j = parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)parameters[i]).Clone();
                    }

                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }
                else if (this.parameters != null && this.parameters.Count > 0)
                {
                    SqlParameter[] ClonedParameters = new SqlParameter[this.parameters.Count];
                    for (int i = 0, j = this.parameters.Count; i < j; i++)
                    {
                        ClonedParameters[i] = (SqlParameter)((ICloneable)this.parameters[i]).Clone();
                    }
                    this.ClearParameter();
                    foreach (SqlParameter param in ClonedParameters)
                    {
                        command.Parameters.Add(param);
                        this.AddParameter(param);
                    }
                }

                if (srcTable != null)
                    ret = adapter.Fill(dataSet, srcTable);
                else
                    ret = adapter.Fill(dataSet);

                if (bInAutoTransaction)
                {
                    this.CommitTransaction();
                    bInAutoTransaction = false;
                }
            }
            catch (Exception ex)
            {

                if (bInAutoTransaction)
                {
                    bInAutoTransaction = false;
                    this.RollbackTransaction();
                }

                if (commandType == CommandType.Text)
                {
                    //发生异常，显示替换参数后的完成 SQL 语句
                    if (parameters != null && parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, parameters);
                    else if (this.parameters != null && this.parameters.Count > 0)
                        strSQL = this.GetReplaceSql(strSQL, this.parameters);
                    TraceLog.WriteLine(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL));
                    throw new Exception(String.Format("{0}<br><br><B>SQL==</B>{1}", ex.Message, strSQL), ex);
                }
                else
                {
                    string sParameters = "";
                    if (command != null)
                    {
                        string sep = "";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            if (param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.NChar)
                                sep = "'";
                            else
                                sep = "";

                            if (sParameters != String.Empty)
                                sParameters += ", ";
                            sParameters += param.ParameterName + "=" + sep + param.Value + sep;
                        }
                        if (sParameters != String.Empty)
                            sParameters = "(" + sParameters + ")";
                    }
                    TraceLog.WriteLine(string.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters));
                    throw new Exception(String.Format("{0}<br><br><B>存储过程名称==</B>{1}{2}", ex.Message, strSQL, sParameters), ex);
                }
            }
            finally
            {
                try
                {
                    if (adapter != null)
                        adapter.Dispose();
                }
                catch (Exception)
                { }

                try
                {
                    if (command != null)
                        command.Dispose();
                }
                catch (Exception)
                { }

                //判断是否自动断开连接
                if (bInAutoConnection)
                    CloseConnection();
            }
            return ret;
        }
        #endregion

        #region 格式化SQL语句
        /// <summary>
        /// 格式化strSQL语句的条件，替为“tab”、“回车”符为“空格”
        /// </summary>
        /// <param name="str">strSQL语句</param>
        /// <returns>格式化后strSQL语句</returns>
        public string FormateSQLCondtion(string str)
        {
            string str1 = "\t";
            string str2 = "\r";
            string str3 = "\n";
            str = str.Replace(str1, "").Replace(str2, "").Replace(str3, "");
            return str;
        }
        #endregion

        #region 根据参数名，得到参数的值
        /// <summary>
        /// 根据parameterName 返回参数值
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>object 参数值</returns>
        public object GetParameterValue(string parameterName)
        {
            try
            {
                parameterName = parameterName.ToLower();
                SqlParameter param = null;
                foreach (SqlParameter p in this.parameters)
                {
                    if (p.ParameterName.ToLower() == parameterName)
                        param = p;
                }
                if (param == null)
                    return null;

                return param.Value;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 返回替换参数后的SQL语句
        /// <summary>
        /// 根据带参数SQL语句、和参数集合，返回参数替换后的SQL语句
        /// </summary>
        /// <param name="strSQL">带参数定义SQL语句</param>
        /// <param name="parms">参数列表</param>
        /// <returns>返回简单替换参数后的SQL语句</returns>
        private string GetReplaceSql(string strSQL, ArrayList parms)
        {
            try
            {
                string sep = "";
                StringBuilder sbRet = new StringBuilder(strSQL);
                foreach (SqlParameter param in parms)
                {
                    if (param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.NChar)
                        sep = "'";
                    else
                        sep = "";
                    sbRet.Replace("@" + param.ParameterName, sep + (param.Value == null ? "NULL" : param.Value.ToString()) + sep);
                }
                return sbRet.ToString();
            }
            catch
            {
                return strSQL;
            }
        }
        #endregion


        #region IDisposable 成员

        /// <summary>
        /// 释放连接资源
        /// </summary>
        public void Dispose()
        {
            CloseConnection();
        }

        #endregion
    }
}
