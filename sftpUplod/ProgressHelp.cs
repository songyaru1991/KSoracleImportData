using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.IO;

namespace sftpUplod
{
    class ProgressHelp
    {
        public DataTable QueryProgress(string sqlStr)
        {
            string ControlString1 = "DSN=KSHR;UID=csruser;PWD=csruser";
            OdbcConnection odbcCon = new OdbcConnection(ControlString1);
            try
            {
                //string SqlStr = "select * from PUB.deptcsr WHERE SwipeCardTime>='2017/8/16 00:00:00'";
                OdbcDataAdapter odbcAdapter = new OdbcDataAdapter(sqlStr, odbcCon);
                DataTable ds = new DataTable();
                odbcAdapter.Fill(ds);
                string strFileName = Directory.GetCurrentDirectory() + '\\' + "Log";
                //if (Directory.Exists(strFileName))
                //{
                //    string s = strFileName + '\\' + "ProgressEMPCLASSR" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".csv";
                //    SaveCSV(ds, s);
                //}
                
                return ds;
            }
            catch (System.Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query Progress Error:" + ex.Message);
                return null;
            }
            finally
            {
                odbcCon.Close();
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

        public static void SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                        || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }
    }
}
