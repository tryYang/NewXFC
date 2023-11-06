using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

using System.Data;
using XFC.Model;
using System.Collections;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace XFC.Helper
{
    internal class OledbHelper:IDisposable
    {
       
        // 将 JSON 字符串解析为 JObject
        static string connstr;
        static OleDbCommand command;
        public static OleDbConnection connection;
        public  string sqlstring;
        public OledbHelper()
        {
            string projectpath = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(projectpath,"config.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            JObject jsonObject = JObject.Parse(jsonContent);
            connstr = (string)jsonObject["connstr"];
            connection=new OleDbConnection(connstr);
            connection.Open();
        }
        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand(sqlstring,connection);            
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;

        }
        public OleDbDataReader GetDataReader()
        {
            OleDbCommand cmd = new OleDbCommand(sqlstring, connection);
            
            OleDbDataReader reader=cmd.ExecuteReader();
            return reader;


        }
        public int ExecuteCommand() {

            using (OleDbCommand cmd = new OleDbCommand(sqlstring, connection))
            {
                int result = cmd.ExecuteNonQuery();
                return result;
                
            }
                
       }

        public static void InsertData<T>(T data)
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                string columnNames = string.Join(", ", type.GetFields().Select(prop => '[' + prop.Name + ']'));
                string paramNames = string.Join(", ", type.GetFields().Select(prop => '?'));

                string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";

                MessageBox.Show(insertQuery);
                using (OleDbCommand command = new OleDbCommand(insertQuery, connection))
                {
                    foreach (var fields in type.GetFields())
                    {

                        command.Parameters.AddWithValue("?", fields.GetValue(data));
                    }
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
                
                
            

        }
        public void Save2Table(object obj)
        {
            Type type = obj.GetType();
            // 获取类名  
            string className = type.Name;

            // 获取字段名  
            FieldInfo[] fields = type.GetFields();

            string query = @"Insert into @tablename ({0}) Value ({1})";

            
            OleDbCommand cmd= new OleDbCommand(query, connection);
        }
        public int GetMaxID()
        {
            int result =0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sqlstring, connection);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                    else
                    {
                        result = 1;
                    }
                }
            } catch (Exception exception)
            {
                
            }
            return result;
          
        }
        public void Dispose() =>connection?.Dispose();
    }
}
