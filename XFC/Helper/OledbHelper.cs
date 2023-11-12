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
using System.Diagnostics;

namespace XFC.Helper
{
    internal class OledbHelper:IDisposable
    {
       
        // 将 JSON 字符串解析为 JObject
         string connstr;
        static OleDbCommand command;
        public  OleDbConnection connection;
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
            try
            {
                
                OleDbCommand cmd = new OleDbCommand(sqlstring, connection);
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {

            }
          return ds;
           

        }
        public DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable();
            try
            {

                using (OleDbCommand cmd = new OleDbCommand(sqlstring, connection))
                {

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }

            return dataTable;
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
        /// <summary>
        /// 【Threshold】
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {

            using (OleDbCommand cmd = new OleDbCommand(sqlstring, connection))
            {
                object result = cmd.ExecuteScalar();
                return result;

            }

        }

        public  void InsertData<T>(T data)
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                string columnNames = string.Join(", ", type.GetProperties().Select(prop => '[' + prop.Name + ']'));
                string paramNames = string.Join(", ", type.GetProperties().Select(prop =>"'"+ prop.GetValue(data)+"'"));

                string sqlstring = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
                
                using (OleDbCommand command = new OleDbCommand(sqlstring, connection))
                {
                    
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("插入数据失败");
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
