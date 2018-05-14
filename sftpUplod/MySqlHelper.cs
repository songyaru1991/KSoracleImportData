using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace sftpUplod
{

    class MySqlHelper
    {

       
        public DataTable QueryDBC(string sqlStr)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["conMySqlDbCSBG"].ConnectionString.ToString();
                string connectionString = "";
                using (MySqlConnection sqlconn = new MySqlConnection(connectionString))
                {
                    sqlconn.Open();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(sqlStr, sqlconn);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "datatable1");
                    DataTable dt1 = ds1.Tables[0];
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
                return null;
            }
        }

        public DataTable QueryDbAsbg(string sqlStr)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["conMySqlDbASBG"].ConnectionString.ToString();
                string connectionString = "";
                using (MySqlConnection sqlconn = new MySqlConnection(connectionString))
                {
                    //sqlconn.Open();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(sqlStr, sqlconn);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "datatable1");
                    DataTable dt1 = ds1.Tables[0];
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
                return null;
            }
        }

        public DataTable QueryDB65_230(string sqlStr)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["conMySqlDbASBG"].ConnectionString.ToString();
                string connectionString = "";
                using (MySqlConnection sqlconn = new MySqlConnection(connectionString))
                {
                    //sqlconn.Open();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(sqlStr, sqlconn);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "datatable1");
                    DataTable dt1 = ds1.Tables[0];
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
                return null;
            }
        }

        public DataTable Query155_200(string sqlStr)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["conMySqlDbCSBG"].ConnectionString.ToString();
                string connectionString = "server=10.64.155.200;userid=root;password=mysql;database=mysql;charset=utf8";
                using (MySqlConnection sqlconn = new MySqlConnection(connectionString))
                {
                    //sqlconn.Open();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(sqlStr, sqlconn);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "datatable1");
                    DataTable dt1 = ds1.Tables[0];
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
                return null;
            }
        }

        private void WriteLog(String strLog)
        {
            DateTime currenttime = System.DateTime.Now;
            //create log folder
            string sLogPath = System.Environment.CurrentDirectory + "\\sftplog";
            if (!Directory.Exists(sLogPath))
            {
                Directory.CreateDirectory(sLogPath);
            }
            //write log to local
            StreamWriter sw = null;
            try
            {
                string sTitle = string.Format("{0:yyyyMMdd}", currenttime);
                string LogStr = currenttime + " " + strLog;
                sw = new StreamWriter(sLogPath + "\\" + sTitle + ".txt", true);
                sw.WriteLine(LogStr);
                //sw.WriteLine('\r');
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
