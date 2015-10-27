using System;
using System.Data;

using System.Data.OracleClient;

namespace CommLB.wmj.AllDataProdivers.Command
{
    /// <summary>
    /// OracleDataProvider 的摘要说明
    /// </summary>
    internal class OracleDataProvider 
    {
        private System.Data.OracleClient.OracleConnection oracleConnection;
        private System.Data.OracleClient.OracleCommand oracleCommand;
        private string connectionString;
        public OracleDataProvider()
            : this(null)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public OracleDataProvider(string connectionString)
        {
            if (connectionString == null || connectionString.Trim() == string.Empty)
            {
                System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
                this.connectionString = (string)(configurationAppSettings.GetValue("oracleConnectionString", typeof(string)));
            }
            else
            {
                this.connectionString = connectionString;
            }
        }

        /// <summary>
        /// Oracle 连接字符串 "User Id=southfence;Data Source=FENCEORA;Password=southfence;Persist Security Info=true;"    
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        /// <summary>
        /// 返回一个带有连接字符串的Oracle Connection.
        /// </summary>
        /// <returns>OracleConnection</returns>
        private OracleConnection GetOracleConnection()
        {
            try
            {
                return new OracleConnection(this.connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。对于其他所有类型的语句，返回值为 -1
        /// </summary>
        /// <param name="Sql">UPDATE、INSERT 和 DELETE 语句</param>
        public int ExecuteNonQuery(string sql)
        {
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return -1;
                int rv = -1;
                OracleTransaction oracleTransaction = null;
                try
                {
                    if (oracleConnection.State == System.Data.ConnectionState.Closed)
                        oracleConnection.Open();
                    oracleCommand = new OracleCommand(sql, oracleConnection);
                    oracleTransaction = oracleConnection.BeginTransaction();
                    oracleCommand.Transaction = oracleTransaction;
                    rv = oracleCommand.ExecuteNonQuery();
                    oracleTransaction.Commit();
                }
                catch (Exception ex)
                {
                    oracleTransaction.Rollback();
                    rv = -1;
                    throw ex;
                }

                return rv;
            }
        }

        /// <summary>
        /// 执行查询，并将查询返回的结果集中第一行的第一列作为 .NET Framework 数据类型返回。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">SELECT 语句</param>
        /// <returns>.NET Framework 数据类型形式的结果集第一行的第一列；如果结果集为空或结果为 REF CURSOR，则为空引用</returns>
        public object ExecuteScalar(string sql)
        {
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;
                try
                {
                    if (oracleConnection.State == System.Data.ConnectionState.Closed)
                        oracleConnection.Open();
                    oracleCommand = new OracleCommand(sql, oracleConnection);
                    return oracleCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        ///  执行单Sql语句查询，并将查询返回的结果作为一个数据集返回
        /// </summary>
        /// <param name="selectSql">SELECT 语句</param>
        /// <returns>数据集 DataSet</returns>
        public DataSet RetriveDataSet(string sql)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("抱歉，SQL 语句为空...");
            }
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;
                using (OracleDataAdapter da = new OracleDataAdapter(sql, oracleConnection))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行Sql数组语句查询，并将查询返回的结果作为一个数据集返回
        /// </summary>
        /// <param name="sql">Select 语句数组</param>
        /// <param name="tableName">TableName</param>
        /// <returns>数据集 DataSet</returns>
        public DataSet RetriveDataSet(string[] sqls, params string[] tableNames)
        {
            if (sqls == null || sqls.Length == 0)
            {
                throw new Exception("抱歉，SQL 语句为空...");
            }
            int sqlLength;
            sqlLength = sqls.Length;
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;
                DataSet ds = new DataSet();
                int tableNameLength = tableNames.Length;
                for (int i = 0; i < sqlLength; i++)
                {
                    using (OracleDataAdapter da = new OracleDataAdapter(sqls[i], oracleConnection))
                    {
                        try
                        {
                            if (i < tableNameLength)
                                da.Fill(ds, tableNames[i]);
                            else
                                da.Fill(ds, "table" + i);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                return ds;
            }
        }

        public DataSet RetriveDataSet(string[,] sql_tableNames)
        {
            if (sql_tableNames.GetLength(0) != 2)
            {
                throw new Exception("抱歉，SQL语句-表名 参数必须为二维数组...");
            }
            int length = sql_tableNames.GetLength(1);
            if (length <= 0)
            {
                throw new Exception("抱歉，SQL语句-表名 为空...");
            }

            string[] sqls = new string[length];
            string[] tableNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                sqls[i] = sql_tableNames[i, 0];
                tableNames[i] = sql_tableNames[i, 1];
            }

            return RetriveDataSet(sqls, tableNames);
        }

        /// <summary>
        /// 更新数据集. 
        /// 过程:客户层(dataSet.GetChanges()) -- 修改 --> 数据服务层(hasChangesDataSet.update()) -- 更新--> 数据层(hasChangesDataSet) ...
        ///  数据层(hasChangesDataSet) -- 新数据 --> 数据服务层 (hasChangesDataSet) -- 合并 -- > 客户层(dataSet.Merge(hasChangesDataSet))
        /// </summary>
        /// <param name="hasChangeDataSet"></param>
        /// <returns></returns>
        public DataSet UpdateDataSet(string sql, DataSet hasChangesDataSet)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("抱歉，SQL 语句为空...");
            }
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;
                using (OracleDataAdapter da = new OracleDataAdapter(sql, oracleConnection))
                {
                    try
                    {
                        OracleCommandBuilder cb = new OracleCommandBuilder(da);
                        da.Update(hasChangesDataSet);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return hasChangesDataSet;
                }
            }
        }

        /// <summary>
        /// 将一组 UPDATE、INSERT 和 DELETE 语句以事务执行
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns>是否执行成功</returns>
        public bool ExecuteTransaction(string[] sqls)
        {
            if (sqls == null || sqls.Length == 0)
            {
                throw new Exception("抱歉，SQL 语句为空...");
            }
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return false;
                OracleTransaction oracleTransaction = null;
                try
                {
                    if (oracleConnection.State == System.Data.ConnectionState.Closed)
                        oracleConnection.Open();
                    oracleCommand = oracleConnection.CreateCommand();
                    oracleTransaction = oracleConnection.BeginTransaction();
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.Transaction = oracleTransaction;

                    for (int i = 0; i < sqls.Length; i++)
                    {
                        oracleCommand.CommandText = sqls[i];
                        oracleCommand.ExecuteNonQuery();
                    }
                    oracleTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    if (oracleTransaction != null)
                        oracleTransaction.Rollback();

                    throw ex;
                }
            }
        }
        public bool ExecuteTransaction(string[] sqls, object[][] myParams)
        {
            throw new Exception("抱歉，未实现...");
        }

        /// <summary>
        ///  执行Sql数组语句查询，并将查询返回的结果作为一个数据读取器返回
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>OracleDataReader</returns>
        public OracleDataReader RetriveDataReader(string sql)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("抱歉，SQL 语句为空...");
            }
            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;
                using (oracleCommand = new OracleCommand(sql, oracleConnection))
                {
                    try
                    {
                        OracleDataReader oracleDataReader = oracleCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                        return oracleDataReader;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        /*
        /// <summary>
        /// 执行一个查询式的存贮过程,返回得到的数据集
        /// </summary>
        /// <param name="proceName">存贮过程名称</param>
        /// <param name="myParams">所有属性值</param>
        /// <returns></returns>
        public DataSet ExecStoredProcedure(string proceName, object[] myParams)
        {
            if (proceName == null || proceName == string.Empty)
            {
                throw new Exception("抱歉，存贮过程名称为空...");
            }

            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    return null;

                DataSet ds = new DataSet();

                try
                {
                    if (oracleConnection.State == System.Data.ConnectionState.Closed)
                        oracleConnection.Open();
                    oracleCommand = oracleConnection.CreateCommand();
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.CommandText = proceName;
                    if (myParams != null)
                    {
                        for (int i = 0; i < myParams.Length; i++)
                            oracleCommand.Parameters.Add((OracleParameter)myParams[i]);
                    }
                    using (OracleDataAdapter da = new OracleDataAdapter(oracleCommand))
                    {
                        int returnValue = da.Fill(ds);
                        if (returnValue < 0)
                            throw new Exception("存储过程执行错误:" + (returnValue >= -14 ?
                                ((StoreProcReturn)returnValue).ToString() : "ErrCode:" + returnValue));
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        */
        /*
        /// <summary>
        /// 执行一个非查询式的存贮过程
        /// </summary>
        /// <param name="proceName">存贮过程名称</param>
        /// <param name="myParams">所有属性值</param>
        /// <returns>存储过程return值</returns>
        public int ExecNonQueryStoredProcedure(string proceName, ref object[] myParams)
        {
            if (proceName == null || proceName == string.Empty)
            {
                throw new Exception("抱歉，存贮过程名称为空...");
            }

            using (oracleConnection = this.GetOracleConnection())
            {
                if (oracleConnection == null)
                    throw new Exception("抱歉，数据库连接没有初始化...");

                try
                {
                    if (oracleConnection.State == System.Data.ConnectionState.Closed)
                        oracleConnection.Open();
                    oracleCommand = oracleConnection.CreateCommand();
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.CommandText = proceName;
                    if (myParams != null)
                    {
                        for (int i = 0; i < myParams.Length; i++)
                            oracleCommand.Parameters.Add((OracleParameter)myParams[i]);
                    }
                    int returnValue = oracleCommand.ExecuteNonQuery();
                    if (returnValue < 0)
                        throw new Exception("存储过程执行错误:" + (returnValue >= -14 ?
                            ((StoreProcReturn)returnValue).ToString() : "ErrCode:" + returnValue));
                    return returnValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        */
        public void Dispose()
        {
            this.connectionString = null;
            if (this.oracleCommand != null)
                this.oracleCommand.Dispose();
            if (this.oracleConnection != null)
                this.oracleConnection.Dispose();
        }
    }
}


