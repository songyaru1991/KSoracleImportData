using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace sftpUplod
{
    class SqlHelp
    {
        public DataTable QuerySqlServerDB(string sqlStr)
        {
            String connsql = "server=192.168.78.234;database=FGHR;user id=user ;password=user;Connect Timeout=60";
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connsql;
                    conn.Open(); // 打开数据库连接

                    //String sql = "select distinct bc from KQ_RECORD_BC "; // 查询语句
                    //String sql = "select ygbh,CONVERT(varchar(12) , fdate, 111 ),bc  from KQ_RECORD_BC WHERE ygbh is not null and ygbh!='' and fdate<='2017-08-20' "; // 查询语句
                    // ygbh,CONVERT(varchar(12) , fdate, 111 ),bc
                    SqlDataAdapter myda = new SqlDataAdapter(sqlStr, conn); // 实例化适配器

                    DataTable dt = new DataTable(); // 实例化数据表
                    myda.Fill(dt); // 保存数据 

                    //dataGridView1.DataSource = dt; // 设置到DataGridView中

                    string strFileName = Directory.GetCurrentDirectory() + '\\' + "Log";
                    if (Directory.Exists(strFileName))
                    {
                        Directory.CreateDirectory(strFileName);
                    }
                    string s = strFileName + '\\' + "SqlServerCount" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".csv";
                    SaveCSV(dt, s);
                    //WriteLog("OK:" + dt.Rows.Count.ToString());

                    conn.Close(); // 关闭数据库连接
                    return dt;
                }
            }
            catch (Exception ex)
            {
                WriteLog(DateTime.Now + "-->Query SqlServer Error:" + ex.Message);
                return null;
            }

        }

        //public DataTable QuerySqlServerDB112(string sqlStr)
        //{
        //    String connsql = "server=192.168.64.67;database=stxt001041228;user id=user_lzj ;password=User_lzj1;";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            conn.ConnectionString = connsql;
        //            conn.Open(); // 打开数据库连接

        //            //String sql = "select distinct bc from KQ_RECORD_BC "; // 查询语句
        //            //String sql = "select ygbh,CONVERT(varchar(12) , fdate, 111 ),bc  from KQ_RECORD_BC WHERE ygbh is not null and ygbh!='' and fdate<='2017-08-20' "; // 查询语句
        //            // ygbh,CONVERT(varchar(12) , fdate, 111 ),bc
        //            SqlDataAdapter myda = new SqlDataAdapter(sqlStr, conn); // 实例化适配器

        //            DataTable dt = new DataTable(); // 实例化数据表
        //            myda.Fill(dt); // 保存数据 

        //            //dataGridView1.DataSource = dt; // 设置到DataGridView中

        //            string strFileName = Directory.GetCurrentDirectory() + '\\' + "Log";
        //            if (Directory.Exists(strFileName))
        //            {
        //                Directory.CreateDirectory(strFileName);
        //            }
        //            string s = strFileName + '\\' + "SqlServerCount" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".csv";
        //            SaveCSV(dt, s);
        //            //WriteLog("OK:" + dt.Rows.Count.ToString());

        //            conn.Close(); // 关闭数据库连接
        //            return dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(DateTime.Now + "-->Query SqlServer Error:" + ex.Message);
        //        return null;
        //    }

        //}

        /// <summary>
        /// 记录LOG
        /// </summary>
        /// <param name="strLog"></param>
        private void WriteLog(String strLog)
        {
            FileStream ostrm;
            StreamWriter writer;
            String strFolderLog;
            String strFileLog;

            try
            {
                //取得當前目錄,如C:\EMESNET\EMES_Service\Service_ATE_Manager\
                strFolderLog = String.Format(@"{0}Log\", System.AppDomain.CurrentDomain.BaseDirectory);
                strFileLog = String.Format("{0:yyyyMMddHH}.txt", DateTime.Now);

                //if (!Directory.Exists(strFolderLog))
                //Directory.CreateDirectory(strFolderLog);

                //存在Log目錄時才紀錄，哪天不需要Log時把目錄刪除即可
                if (Directory.Exists(strFolderLog))
                {
                    //Log檔目錄按年月日分開，不然久了會積很多
                    strFolderLog += String.Format(@"{0:yyyy}\{0:MM}\{0:dd}\", DateTime.Now);
                    if (!Directory.Exists(strFolderLog))
                        Directory.CreateDirectory(strFolderLog);

                    if (!File.Exists(strFolderLog + strFileLog))
                    {
                        ostrm = new FileStream(strFolderLog + strFileLog, FileMode.Create, FileAccess.Write);
                    }
                    else
                    {
                        ostrm = new FileStream(strFolderLog + strFileLog, FileMode.Append, FileAccess.Write);
                    }

                    writer = new StreamWriter(ostrm);

                    //設定訊息輸出至資料流
                    Console.SetOut(writer);

                    Console.WriteLine(strLog);
                    Console.WriteLine("");

                    writer.Close();
                    ostrm.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("WriteLog Error: " + ex.Message);
               // MessageBox.Show("WriteLog Error: " + ex.Message);
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
