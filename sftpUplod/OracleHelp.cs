using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Windows.Forms;
namespace sftpUplod
{
    class OracleHelp
    {

        public DataTable OrcaleQuery(string sqlStr)
        {
            try
            {
                //Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.64.224)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RTODB)));Persist Security Info=True;User ID=usyn;Password=mis_syn
                string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.78.154)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SSGSDB)));Persist Security Info=True;User ID=swipe;Password=a2#ks#ssgs";
                using (OracleConnection con = new OracleConnection(connString))
                {
                    con.Open();
                    // OracleCommand com = new OracleCommand(sqlStr, con);

                    OracleDataAdapter Oda = new OracleDataAdapter(sqlStr, con);
                    DataSet ds1 = new DataSet();
                    Oda.Fill(ds1, "datatable1");
                    DataTable dt1 = ds1.Tables[0];
                    return dt1;
                }
            }
            catch (Exception ex) {

                WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
                return null;

            }
            
           

        }
        public DataTable OrcaleQuery_ONE(string sqlStr)
        {
            try
            {
                string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.78.154)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SSGSDB)));Persist Security Info=True;User ID=swipe;Password=a2#ks#ssgs";
                using (OracleConnection con = new OracleConnection(connString))
                {
                    con.Open();
                    // OracleCommand com = new OracleCommand(sqlStr, con);

                    OracleDataAdapter Oda = new OracleDataAdapter(sqlStr, con);
                    DataSet ds1 = new DataSet();
                    Oda.Fill(ds1, "datatable1");
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
        //public void oracleInsert(string sqlStr) {

        //    try
        //    {
        //        string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.144.187)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SCARD)));Persist Security Info=True;User ID=swipe;Password=mis_swipe;";
        //        using (OracleConnection con = new OracleConnection(connString))
        //        {
        //            con.Open();
        //            OracleCommand com = new OracleCommand(sqlStr, con);
        //            com.ExecuteNonQuery();
        //        }

        //    }
        //    catch (Exception ex) {

        //        WriteLog(DateTime.Now + "-->Query 111 Error:" + ex.Message);
        //    }
        
        
        
        //}
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
