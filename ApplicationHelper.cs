using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for ApplicationHelper
/// </summary>
/// 
public class ApplicationHelper
{
    public ApplicationHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region "Variable Declaration"

    public const string CNST_SELECT_STRING = "SELECT";
    public static string RECORDADDED = "N";
    public static   int GrievId;
    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
    public static string con = System.Configuration.ConfigurationSettings.AppSettings.Get("connStr").ToString();
    public static SqlConnection cn = new SqlConnection(con);

    #endregion

    #region "Create Connection"

    //Purpose            - Returns the connection
    //Created date       - 12 Sept 2020
    //Author             - Vishal

    public SqlConnection Prop_GetConnection
    {
        get { return cn; }
        set { cn = value; }
    }

    #endregion 

    #region "Open Connection"
    //Purpose            - Opens the connection 
    //Created date       - 19 Sept 2020
    //Author             - Vishal
    public static SqlConnection OpenConnection()
    {
        try 
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            return cn;        
        }
        catch (SqlException ex)
        { throw ex; }
    }

    #endregion

    #region "Close Connection"
    //Purpose            - Closes the connection 
    //Created date       - 19 Sept 2020
    //Author             - Vishal
    public static SqlConnection CloseConnection()
    {
        try 
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();            
            }
            return cn;
        }
        catch (SqlException ex)
        { throw ex; }
    }

    #endregion

    #region "Create Parameter"

    //Purpose            - To Create SqlParameters for Stored Procedures
    //InParameters       - paramName,paramvalue,ParamSize,paramDbType,paramdirection
    //OutParameters      - Sqlparameter
    //Author             - Vishal
    //created date       - 12 Sept 2020

    public static SqlParameter Createparameter(string parName, string parValue, SqlDbType dbType, int size,ParameterDirection parDirection)
    {
        SqlParameter sqlParameter = new SqlParameter();
        try
        {
            sqlParameter.ParameterName = parName;
            sqlParameter.SqlValue = parValue;
            sqlParameter.SqlDbType = dbType;
            sqlParameter.Size = size;
            sqlParameter.Direction = parDirection;
            return sqlParameter;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlParameter = null;
        }
    }

    public static SqlParameter Createparameter(string parName, byte[] parValue, SqlDbType dbType, ParameterDirection parDirection)
    {
        SqlParameter sqlParameter = new SqlParameter();
        try
        {
            sqlParameter.ParameterName = parName;
            sqlParameter.SqlValue = parValue;
            sqlParameter.SqlDbType = dbType;
            //sqlParameter.Size  = SqlDbType.;
            sqlParameter.Direction = parDirection;
            return sqlParameter;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlParameter = null;
        }
    }


    public static SqlParameter Createparameter(string parName, Guid parValue, SqlDbType dbType, int size, ParameterDirection parDirection)
    {
        SqlParameter sqlParameter = new SqlParameter();
        try
        {
            sqlParameter.ParameterName = parName;
            sqlParameter.SqlValue = parValue;
            sqlParameter.SqlDbType = dbType;
            sqlParameter.Size = size;
            sqlParameter.Direction = parDirection;
            return sqlParameter;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlParameter = null;
        }
    }

    #endregion

    #region "Execute  NonQuery"
    //Purpose            - To return result of Query
    //InParameters       -connection,commandType,CommandText,SqlParameter
    //OutParameters      - datareader object
    //Author             -Vishal
    //created date       -12 Sept 2020

    public int ExecuteNonQuery(SqlConnection con, CommandType commandType, string commandText)
    {
        SqlCommand cmd = new SqlCommand();
        int i = 0;
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            //con.Open();


            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        { throw ex; }
        finally { con.Close(); }
    }

    public  int ExecuteNonQuery(SqlConnection con, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        SqlCommand cmd = new SqlCommand();        
        int i = 0;
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            //con.Open();

            while (i < parameters.Length)
            {
                cmd.Parameters.Add(parameters[i]);
                i += 1;
            }

            
            //int outvalue = Convert.ToInt16( cmd.Parameters["@POID"].Value);
            return  cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        { throw ex; }
        finally { con.Close(); }
    }

    //public long[] ExecuteNonQueryOutParam(SqlConnection con, CommandType commandType, string commandText, params SqlParameter[] parameters)
    //{
    //    SqlCommand cmd = new SqlCommand();
    //    long[] OutValue = new long[2];
    //    int i = 0;
    //    try
    //    {
    //        if (con.State == ConnectionState.Closed)
    //        {
    //            con.Open();
    //        }
    //        cmd.Connection = con;
    //        cmd.CommandType = commandType;
    //        cmd.CommandText = commandText;
    //        //con.Open();

    //        while (i < parameters.Length)
    //        {
    //            cmd.Parameters.Add(parameters[i]);
    //            i += 1;
    //        }

    //        OutValue[0] = cmd.ExecuteNonQuery();
    //        OutValue[1] = Convert.ToInt64  (cmd.Parameters["@OutParam"].Value);

    //        //int outvalue = Convert.ToInt16( cmd.Parameters["@POID"].Value);
    //        return OutValue;
    //    }
    //    catch (Exception ex)
    //    { throw ex; }
    //    finally { con.Close(); }
    //}

    public string[] ExecuteNonQueryOutParam(SqlConnection con, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        SqlCommand cmd = new SqlCommand();
        string[] OutValue = new string[2];
        int i = 0;
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            //con.Open();

            while (i < parameters.Length)
            {
                cmd.Parameters.Add(parameters[i]);
                i += 1;
            }

            OutValue[0] = cmd.ExecuteNonQuery().ToString();
            OutValue[1] = cmd.Parameters["@OutParam"].Value.ToString();

            //int outvalue = Convert.ToInt16( cmd.Parameters["@POID"].Value);
            return OutValue;
        }
        catch (Exception ex)
        { throw ex; }
        finally { con.Close(); }
    }

    #endregion

    #region "Execute dataReader"
    //Purpose            - To return datareader
    //InParameters       -connection,commandType,CommandText,SqlParameter
    //OutParameters      - datareader object
    //Author             -Vishal
    //created date       -12 Sept 2020

    public SqlDataReader ExecuteDataReader(SqlConnection con, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        int i = 0;
        SqlCommand cmd = new SqlCommand();

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = con;
            cmd.CommandTimeout = 0;
            while (i < parameters.Length)
            {
                cmd.Parameters.Add(parameters[i]);
                i++;
            }
            return cmd.ExecuteReader();
        }
        catch (Exception  ex)
        {
            throw ex;
        }
        finally
        {
            
            cmd.Dispose();
        }
    }

    public SqlDataReader ExecuteDataReader(SqlConnection con, CommandType commandType, string commandText)
    {
        int i = 0;
        SqlCommand cmd = new SqlCommand();

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.Connection = con;
            cmd.CommandTimeout = 0;
            
            return cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            //con.Close();
            cmd.Dispose();
        }
    }
    #endregion    

    #region "Execute Scalar"
    //Purpose            - To Return Single Integer Value
    //InParameters       - paramName,paramvalue,ParamSize,paramDbType,paramdirection
    //OutParameters      - Sqlparameter
    //Author             - Vishal
    //created date       - 12 Sept 2020

    public int ExecuteScalar(SqlConnection con, CommandType commandType, string commandText)
    {
        SqlCommand cmd = new SqlCommand();
        int j = 0;

        try
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.CommandTimeout = 0;
            
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        { throw ex; }
        finally
        { cmd.Dispose();
            con.Close(); 
        }
    }

    public int ExecuteScalar(SqlConnection con, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        SqlCommand cmd = new SqlCommand();
        int j = 0;

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            cmd.CommandTimeout = 0;
            while (j < parameters.Length)
            {
                cmd.Parameters.Add(parameters[j]);
                j++;
            }
            return Convert.ToInt32 (cmd.ExecuteScalar());             
        }
        catch (Exception ex)
        { throw ex; }
        finally { con.Close(); }
    }
    #endregion

    #region "Execute Dataset"

    //Purpose            - To return dataset
    //InParameters       - connection,commandType,CommandText,SqlParameter
    //OutParameters      - dataset object
    //Author             - Vishal
    //created date       - 12 Sept 2020

    //public static DataSet ExecuteDataset(SqlConnection cn, CommandType commandType, string commandText, params SqlParameter[] parameters)
    //{
    //    DataSet ds = new DataSet();
    //    SqlDataAdapter da = new SqlDataAdapter(commandText,cn);
    //    int j = 0;

    //    try
    //    {
    //        da.SelectCommand.CommandType = commandType;
    //        da.SelectCommand.CommandTimeout = 0;

    //        while (j < parameters.Length)
    //        {
    //            da.SelectCommand.Parameters.Add(parameters[j]);
    //            j += 1;
    //        }
    //        da.Fill(ds);
    //        return ds;
    //    }
    //    catch (Exception ex)
    //    { throw ex; }
    //    //finally { ds = null; }
    //}

    public static DataSet ExecuteDataset(string cn, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        SqlConnection con = new SqlConnection(cn);
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return ExecuteDataset(con, commandType, commandText, parameters);
        }
        catch (Exception ex)
        { throw ex; }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    public static DataSet ExecuteDataset(string cn, string query)
    {
        DataSet dscc = new DataSet();
        try 
        {
            
            SqlDataAdapter da = new SqlDataAdapter(query, cn);
            da.Fill(dscc);
            return dscc;
        }
        catch (Exception ex)
        { throw ex; }
        finally 
        {
            dscc = null;
        }
    }


    //connectionString - a valid connection string for a SqlConnection
    //spName - the name of the stored procedure
    //parameterValues - an array of objects to be assigned as the input values of the stored procedure
    //Returns: a dataset containing the resultset generated by the command

    public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
    {
        SqlParameter[] commandParameters = null;

        if (parameterValues != null && parameterValues.Length > 0)
        {
            commandParameters = GetSpParameterSet(connectionString, spName);

            //assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, parameterValues);

            //call the overload that takes an array of SqlParameters
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            //otherwise we can just call the SP without params
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    
    // Parameters:
    // connectionString - a valid connection string for a SqlConnection
    // commandType - the CommandType (stored procedure, text, etc.)
    // commandText - the stored procedure name or T-SQL command
    // Returns: a dataset containing the resultset generated by the command

    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
    {
        //pass through the call providing null for the set of SqlParameters
        return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    

    // -connection - a valid SqlConnection
    // -commandType - the CommandType (stored procedure, text, etc.)
    // -commandText - the stored procedure name or T-SQL command
    // -commandParameters - an array of SqlParamters used to execute the command
    // Returns: a dataset containing the resultset generated by the command

    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //create a command and prepare it for execution
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        SqlDataAdapter da = default(SqlDataAdapter);

        try
        {


            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            da = new SqlDataAdapter(cmd);

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            //detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            //return the dataset
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            connection.Close();
        }
        return ds;
    }

    #endregion 

    #region "AssignParameterValues"

    //This method assigns an array of values to an array of SqlParameters.
    //Parameters:
    // -commandParameters - array of SqlParameters to be assigned values
    // -array of objects holding the values to be assigned

    public  static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        int i = 0;
        int j = 0;

        if ((commandParameters == null) & (parameterValues == null))
        {
            //do nothing if we get no data
            return;
        }
        // we must have the same number of values as we pave parameters to put them in
        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("Parameter count does not match Parameter Value count.");
        }
        //value array
        j = commandParameters.Length - 1;
        for (i = 0; i <= j; i++)
        {
            commandParameters[i].Value = parameterValues[i];
        }
    }

#endregion

    #region "PrepareCommand"

    // -command - the SqlCommand to be prepared
    // -connection - a valid SqlConnection, on which to execute this command
    // -transaction - a valid SqlTransaction, or 'null'
    // -commandType - the CommandType (stored procedure, text, etc.)
    // -commandText - the stored procedure name or T-SQL command
    // -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
    {

        //if the provided connection is not open, we will open it
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        //associate the connection with the command
        command.Connection = connection;

        //set the command text (stored procedure name or SQL statement)
        command.CommandText = commandText;

        //if we were provided a transaction, assign it.
        if ((transaction != null))
        {
            command.Transaction = transaction;
        }
        //set the command type
        command.CommandType = commandType;
        //command.CommandTimeout = CType(GetConfigValue(CNST_CONFIG_CommandTimeout), Integer)

        //attach the command parameters if they are provided
        if ((commandParameters != null))
        {
            AttachParameters(command, commandParameters);
        }

        return;
    }
    #endregion

    #region "AttachParameters"
    // This method is used to attach array of SqlParameters to a SqlCommand.
    // This method will assign a value of DbNull to any parameter with a direction of
    // InputOutput and a value of null.
    // This behavior will prevent default values from being used, but
    // this will be the less common case than an intended pure output parameter (derived as InputOutput)
    // where the user provided no input value.
    // Parameters:
    // -command - The command to which the parameters will be added
    // -commandParameters - an array of SqlParameters tho be added to command
    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
        //SqlParameter p = default(SqlParameter);
        foreach (SqlParameter p in commandParameters)
        {
            //check for derived output value with no value assigned
            if (p.Direction == ParameterDirection.InputOutput & p.Value == null)
            {
                p.Value = null;
            }
            command.Parameters.Add(p);
        }
    }
    #endregion

    #region "GetSPParameter Set"

    public static SqlParameter[] GetSpParameterSet(string connection,string spName)
    {
        return GetSpParameterSet(connection,spName ,false );
    }

    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        SqlParameter[] cachedParameters = null;
        string hashKey = null;

        hashKey = connectionString + ":" + spName + (includeReturnValueParameter == true ? ":include ReturnValue Parameter" : "");

        cachedParameters = (SqlParameter[])paramCache[hashKey];

        if ((cachedParameters == null))
        {
            paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter);

            cachedParameters = (SqlParameter[])paramCache[hashKey];
        }
        if ((cachedParameters != null))
        {
            paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter);

            cachedParameters = (SqlParameter[])paramCache[hashKey];
        }
        return CloneParameters(cachedParameters);
    }
    #endregion

    #region "CloneSpParameters"

    public static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
    {
        int i = 0;
        int j = originalParameters.Length - 1;
        SqlParameter[] clonedParameters = new SqlParameter[j + 1];

        for (i = 0; i <= j; i++)
        {
            clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
        }

        return clonedParameters;
    }
    #endregion

    #region "DiscoverSpParameterSet"

    private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter, params object[] parameterValues)
    {
        SqlConnection cn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(spName, cn);
        SqlParameter[] discoveredParameters = null;
        try
        {
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(cmd);
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }
            discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);
        }
        finally
        {
            cmd.Dispose();
            cn.Dispose();
        }
        return discoveredParameters;
    }
    #endregion

    #region "CommonInsert"
    //' @Desription   - Sub will insert common "All" in all the Tables passed.
    //' @Param        - ByRef DataTable : dtltemp : Name of the Table to which the Record 
    //'                 will be added.
    //'                 ByVal String : strSelectText : String to be appended after "Select".
    //' @Return       - None


    public static void CommonInsertAllInTable(DataTable dtltemp, string strSelectText, string colId, string colName)
    {
        DataRow dtrNewRow = dtltemp.NewRow();

        dtrNewRow[colId] = -1;
        dtrNewRow[colName] = CNST_SELECT_STRING;
        dtltemp.Rows.InsertAt(dtrNewRow, 0);
    }

    public static void CommonInsertAllInTable(DataTable dtltemp, string strSelectText)
    {
        DataRow dtrNewRow = dtltemp.NewRow();

        dtrNewRow["ID"] = -1;
        dtrNewRow["Name"] = CNST_SELECT_STRING;
        dtltemp.Rows.InsertAt(dtrNewRow, 0);
    }

    public static void CommonInsertAllInTable1(DataTable dtltemp, string strSelectText)
    {
        DataRow dtrNewRow = dtltemp.NewRow();

        dtrNewRow["ID"] = -2;
        dtrNewRow["Name"] = "ALL";
        dtltemp.Rows.InsertAt(dtrNewRow, 1);
    }

    #endregion

    #region "GetList"

    // Desription   - To get the list for look up 
    // Param        - ByVal : strSpName, ByVal :prmSpParameters : Array of parameters for the GetList function
    // Return       - Dataset
    // Exception    - Exception thrown by the method (If any)

    public static DataSet GetList(string strSpName, params object[] prmSpParameters)
    {
        int intPrmCount = 0;
        try
        {
            intPrmCount = prmSpParameters.Length - 1;
            return ExecuteDataset(con, strSpName, prmSpParameters);
        }
        catch (Exception ex)
        { throw ex;}        
    }

    #endregion

    #region "CreateParamArray"
    //' @Desription   - Return a values for each parameter in the parmaneter array
    //' @Param        - parameter array
    //' @Return       - the reversed sort expression string
    //' @Exception    - Exception thrown by the method (If any)

    public static object[] CreateParamArray(params object[] prmGetParameters)
    {
        return GetDbObject(prmGetParameters);
    }

    #endregion

    #region "SetString"
    //Desription   - This function will set the string for string builder
    //Param        -                  
    //Return       - None
    //Exception    - Exception thrown by the method (If any)
    //Author       -  Vishal   
    //Date Created - 

    private static object SetString(int intLoopCtr, int intCount, StringBuilder strParameter, string strAppendString)
    {
        if ((intLoopCtr == 0))
        {
            //strParameter.Append("( " & strAppendString & ", ")====Previous
            return strParameter.Append("( " + strAppendString + ", ");
        }
        else if ((intLoopCtr == intCount))
        {
            //strParameter.Append(strAppendString & " )") =====previous
            return strParameter.Append(strAppendString + " )");
        }
        else
        {
            //strParameter.Append(strAppendString & ", ") ====Previous"
            return strParameter.Append(strAppendString + ", ");

        }
    }
    #endregion


    #region "GetDBObject"
    //' @Desription   - This function will check each variable passed in as parameter and depending on its 'Type' 
    //'                 will call routine to Set 'NULL' or the 'Value' for the object.      
    //' @Param        - ByRef Object() : objLst : Array of objects for Null Check
    //' @Return       - Object()
    //' @Exception    - Exception thrown by the method (If any)

    public static object[] GetDbObject(object[] objLst)
    {
        long lngTemp = -1;
        int intTemp = -1;
        double dblTemp = 0.0;
        string strTemp = "";
        DateTime datTemp = DateTime.Now;
        object[] objOutLst = new object[objLst.Length];
        int iLoopCtr = 0;
        StringBuilder strParameter = new StringBuilder();
        //strParameter = null;

        for (iLoopCtr = 0; iLoopCtr <= objLst.Length - 1; iLoopCtr++)
        {
            // nothing is passed as paramter
            if ((objLst[iLoopCtr] == null))
            {
                objOutLst[iLoopCtr] = System.DBNull.Value;
                SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
            }
            else
            {
                // the given paramter is of type string
                if ((objLst[iLoopCtr].GetType().IsInstanceOfType(strTemp)))
                {

                    strTemp = ((string)objLst[iLoopCtr]).Trim();
                    // check if not value is passed or a null is passed in string
                    if ((strTemp.Equals("") | strTemp.Equals("null")))
                    {
                        objOutLst[iLoopCtr] = System.DBNull.Value;
                        SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
                    }
                    else
                    {
                        // string parameter contains a value
                        // check for existence of quotes
                        // Replace any single quote in string with two single quotes.
                        //strTemp = Replace(strTemp, "'", "''")
                        objOutLst[iLoopCtr] = strTemp;
                        SetString(iLoopCtr, objLst.Length - 1, strParameter, "'" + strTemp + "'");

                    }
                }
                else
                {
                    if ((objLst[iLoopCtr].GetType().IsInstanceOfType(lngTemp)))
                    {

                        lngTemp = (long)objLst[iLoopCtr];
                        if ((lngTemp == 0 | lngTemp == -1))
                        {
                            objOutLst[iLoopCtr] = System.DBNull.Value;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
                        }
                        else
                        {
                            objOutLst[iLoopCtr] = lngTemp;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, lngTemp.ToString());

                        }
                    }
                    else if ((objLst[iLoopCtr].GetType().IsInstanceOfType(intTemp)))
                    {

                        intTemp = (int)objLst[iLoopCtr];
                        if ((intTemp == 0 | intTemp == -1))
                        {
                            objOutLst[iLoopCtr] = System.DBNull.Value;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
                        }
                        else
                        {
                            objOutLst[iLoopCtr] = intTemp;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, intTemp.ToString());

                        }
                    }
                    else if ((objLst[iLoopCtr].GetType().IsInstanceOfType(datTemp)))
                    {

                        datTemp = (DateTime)objLst[iLoopCtr];
                        if (((datTemp.Month == 1) & (datTemp.Day == 1) & (datTemp.Year == 1)))
                        {
                            objOutLst[iLoopCtr] = System.DBNull.Value;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
                        }
                        else
                        {
                            objOutLst[iLoopCtr] = datTemp;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, datTemp.ToString());
                        }
                    }
                    else if ((objLst[iLoopCtr].GetType().IsInstanceOfType(dblTemp)))
                    {

                        dblTemp = (double)objLst[iLoopCtr];
                        if ((dblTemp == 0))
                        {
                            objOutLst[iLoopCtr] = System.DBNull.Value;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, "NULL");
                        }
                        else
                        {
                            objOutLst[iLoopCtr] = dblTemp;
                            SetString(iLoopCtr, objLst.Length - 1, strParameter, dblTemp.ToString());
                        }
                    }
                }
            }
        }
        return objOutLst;
    }
    
    #endregion

    
    public string[] getMusterDays(int custid, int paperid, int month, int year, char  yesno)
    {
        
        string[] strArr;

        string query = "select Muster_day from Client_Item_Master where Item_id = " + paperid + " And Client_id=" + custid + " And Is_Paper='Y'  and Status = '" + yesno + "' and Month(muster_date)=" + month + " and Year(muster_date)= " + year;

        SqlDataReader dr = ExecuteDataReader(cn, CommandType.Text, query);

        string days = "";
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                days = dr["Muster_day"].ToString();
            }

            dr.Dispose();
        }
            strArr = days.Split(',');
        

        return strArr;

    }
}
