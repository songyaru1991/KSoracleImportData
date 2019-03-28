using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using ICSharpCode.SharpZipLib.Zip;
using System.Data.Odbc;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;



namespace sftpUplod
{
    public partial class Form1 : Form
    {

        DataTable DT3 = new DataTable();
        DateTime dt = new DateTime();
       
       

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //if(File.Exists(strFilePath))
            //{
            //    txtReadPath.Text=inifile.IniReadValue("Path","ReadPath",strFilePath);
            //    txtBackupPath.Text = inifile.IniReadValue("Path", "BackupPath", strFilePath);
            //    //txtTime.Text = inifile.IniReadValue("QueryTime", "QTime", strFilePath);
            //}
            //Wip To Progress
            WriteLog("Raw To Progress===================" + '\r', 1);

            

            #region 抛转资料到Oracle中       

           // InsertOracle_dept_relation_Progress("CSBG");
           // InsertOracle_employee_Progress3("CSBG");

           // Oracle_testswipecardtimeTOprogress_CARDSR();
            #endregion
           

         //  this.Close();


        }

        /// <summary>
        /// 开始按钮逻辑.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            

            if (btnStart.Text.Equals("Start"))
            {
                //检查路径是否正确
                //if (!CheckPath()) return;

                btnStart.Text = "Pause";
                WriteLog("timer1_Tick定时器开始工作:" + '\r', 1);
                timer1.Enabled = true;
            }
            else
            {
                btnStart.Text = "Start";
                WriteLog("Pause Read..." + '\r',1);
                timer1.Enabled = false;
            }
        }




        /// <summary>
        /// 輸出訊息在畫面上,並寫入Log檔...
        /// </summary>
        /// <param name="intColorType">0:Gray,1:Blue,2:Red</param>
        private void WriteLog(String sMessage, Int32 intColorType = 0)
        {
            DateTime currenttime = System.DateTime.Now;
            string sDateTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", currenttime);
            RichTxtLog.AppendText(sDateTime + " => " + sMessage);
            //RichTxtLog.Text=sMessage+'\r'+RichTxtLog.Text;
            RichTxtLog.Select(RichTxtLog.TextLength - sMessage.Length, RichTxtLog.TextLength);
            switch (intColorType)
            {
                //case 0:
                //    //一般訊息
                //    RichTxtLog.SelectionColor = Color.Gray;
                //    break;
                case 1:
                    //上傳成功
                    RichTxtLog.SelectionColor = Color.Green;
                    break;
                case 2:
                    //出錯
                    RichTxtLog.SelectionColor = Color.Red;
                    break;
            }
            ////保留最新的100筆訊息
            //String[] lines = new String[100];
            //if (RichTxtLog.Lines.Length > lines.Length)
            //{
            //    Array.Copy(RichTxtLog.Lines, lines, lines.Length);
            //    this.RichTxtLog.Lines = lines;
            //}

            if (RichTxtLog.Lines.Length > 100)
            {
                string[] sLines = RichTxtLog.Lines;
                string[] sNewLines = new string[sLines.Length - 100];

                Array.Copy(sLines, 100, sNewLines, 0, sNewLines.Length);
                RichTxtLog.Lines = sNewLines;
            }

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
                string LogStr = currenttime.ToString("HH:mm:ss:fffffff") + " " + sMessage;
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

        private void RichTxtLog_TextChanged(object sender, EventArgs e)
        {

            RichTxtLog.SelectionStart = RichTxtLog.Text.Length; //Set the current caret position at the end     
            RichTxtLog.ScrollToCaret();
            //RichTxtLog.ScrollToCaret(); //Now scroll it automatically
        }

        


        /// <summary>
        /// 定时器.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
           
         //   timer1.Enabled = false;
            if (DateTime.Now.ToString("HH:mm") == "03:30")
            //|| DateTime.Now.ToString("HH:mm") == "09:00" || DateTime.Now.ToString("HH:mm") == "13:00" || DateTime.Now.ToString("HH:mm") == "17:00"
            {
                InsertOracle_empClass0330("CSBG");
     

                InsertOracle_LMT_DEPT_Progress("CSBG");

                InsertOracle_dept_relation_Progress("CSBG");
            }

            if (DateTime.Now.ToString("HH:mm") == "15:30")
            //|| DateTime.Now.ToString("HH:mm") == "09:00" || DateTime.Now.ToString("HH:mm") == "13:00" || DateTime.Now.ToString("HH:mm") == "17:00"
            {

                InsertOracle_empClass1530("CSBG");
             
                InsertOracle_LMT_DEPT_Progress("CSBG");

                InsertOracle_dept_relation_Progress("CSBG");
            }

            if (DateTime.Now.ToString("HH:mm") == "10:00")
            {
                Oracle_testswipecardtimeTOprogress_CARDSR();

            }
            if (DateTime.Now.ToString("HH:mm") == "10:20")
            {
                Oracle_rawToProgress();
            }

            if (DateTime.Now.Minute == 30 || DateTime.Now.Minute == 00)
            {
                InsertOracle_employee_Progress3("CSBG");

            }
            if ((Int32)DateTime.Now.DayOfWeek == 0 && DateTime.Now.ToString("HH:mm") == "14:06")
            {

              //  InsertMysql_empClassNull("CSBG");

            }
            



           
        }

        #region test
        private void InsertOrcale_empClass() {
            try
            {
                //(ID,EMP_DATE,CLASS_NO,UPDATE_TIME)
                string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.144.187)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SCARD)));Persist Security Info=True;User ID=swipe;Password=mis_swipe;";
                OracleConnection con = new OracleConnection(connString);
                //con.Open();
                //OracleHelp oracleHelp = new OracleHelp();

                string InsertSql = "insert into emp_class values('112233',to_date('2020/8/18','yyyy/mm/dd'),'4',to_date('2020/8/7','yyyy/mm/dd'))";
                OracleCommand com = new OracleCommand(InsertSql, con);
                com.Connection.Open();
               // oracleHelp.oracleInsert(InsertSql);
                com.ExecuteNonQuery();
            
                WriteLog("连接成功" + '\r');
                con.Close();
     
            }
            catch (Exception ex)
            {
                WriteLog("连接失败" + ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            //連接progress數據庫
            SqlHelp sqlHelp = new SqlHelp();
            ProgressHelp ProHelp = new ProgressHelp();

            string SQL = "SELECT EMP_CD, START_DATE, CLASS_CD from PUB.EMPCLASSR";
            DT3 = ProHelp.QueryProgress(SQL);
            //遍历数据显示在datagridView上
            foreach(DataRow item in DT3.Rows){
            
                dataGridView1.Rows.Add(item["EMP_CD"].ToString(),
                                       item["START_DATE"].ToString(),
                                       item["CLASS_CD"].ToString()); 
               
                string date = item["START_DATE"].ToString();
               // Console.WriteLine(date);
                //DateTime dt;
                //2017/10/26 0:00:00
                //DateTimeFormatInfo dtFormat = new System.GlobalizationDateTimeFormatInfo();
               
                DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();                       
                dtFormat.ShortDatePattern = "yyyy/MM/dd hh:mm:ss";

                //dt = Convert.ToDateTime("2011/05/26", dtFormat);
                if(date  == DateTime.Now.ToString("yy/MM/dd hh:mm:ss")){

                }
                string dateDiff = null;

                try
                {

                    TimeSpan ts1 = new TimeSpan(dt.Ticks);

                    TimeSpan ts2 = new TimeSpan(dt.Ticks);

                    TimeSpan ts = ts1.Subtract(ts2).Duration();

                    dateDiff = ts.Days.ToString() + "天";

                }

                catch
                {

                }




            }

            if (DT3 == null)
            {
                WriteLog("-->Query ProgressEmpClassSumError" + '\r', 2);
                return;
            }
            else if (DT3.Rows.Count == 0)
            {
                WriteLog("-->Query ProgressEmpClassSum:0" + '\r', 2);
                return;
            }
            WriteLog("-->Query ProgressEmpClassSum:" + DT3.Rows.Count.ToString() + '\r', 1);




        }
        #endregion

        //下午三点半抛数据到Oracle
        private void InsertOracle_empClass1530(string strBG)
        {
            //如果Mysql有当天资料但是班别为空，则update，如果Mysql不存在当天资料则Insert
            //Update和Insert明天资料。
            SqlHelp sqlHelp = new SqlHelp();
            ProgressHelp ProHelp = new ProgressHelp();
            OracleHelp oraclehelp = new OracleHelp();
            //mysql查询语句         
            //oracle查询语句
            string strSqlOracleEmp = " select ID, EMP_DATE, CLASS_NO FROM emp_class where emp_date>=to_date(to_char(SYSDATE - 1,'YYYY/MM/DD'),'YYYY/MM/DD')and emp_date<to_date(to_char(sysdate+2,'YYYY/MM/DD') ,'YYYY/MM/DD') order by ID, EMP_DATE"; 
            //string strSqlMySqlEmp = "select ID, emp_date, class_no FROM swipecard.emp_class where  emp_date=date_add(CURDATE(),interval 1 day) order by ID, emp_date";
            if (strBG == "CSBG")
            {
                #region CSBG
                DataTable DT1 = oraclehelp.OrcaleQuery(strSqlOracleEmp);
                if (DT1 == null)
                {
                    WriteLog("-->Query 155ClassError" + '\r', 2);
                    return;
                }

                List<string> ClassArray3 = new List<string>();
                foreach (DataRow dr in DT1.Rows)
                {
                    ClassArray3.Add(dr[0].ToString() + Convert.ToDateTime(dr[1].ToString()).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + dr[2].ToString());
                }
                List<string> ClassArray2 = new List<string>();
                foreach (DataRow dr in DT1.Rows)
                {
                    ClassArray2.Add(dr[0].ToString() + Convert.ToDateTime(dr[1].ToString()).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                }
                WriteLog("-->Query 155ClassSum:" + DT1.Rows.Count.ToString() + '\r', 1);

                //String sql = "SELECT DISTINCT 編號  FROM V_RYBZ_TX"; // 查询语句
                ////String sql = "select ygbh,fdate,bc from KQ_RECORD_BC where fdate>=CONVERT(varchar(100), GETDATE()-3, 111) and fdate<CONVERT(varchar(100), GETDATE()+2, 111) and ( " + strEMP + " )"; // 查询语句
                //DataTable DT2 = sqlHelp.QuerySqlServerDB(sql);
                //if (DT2 == null)
                //{
                //    WriteLog("-->Query SqlServerdeptError" + '\r', 2);
                //    return;
                //}
                //else if (DT2.Rows.Count == 0)
                //{
                //    WriteLog("-->Query SqlServerdeptSum:0" + '\r', 2);
                //    return;
                //}
                //string strEMP = "";
                //foreach (DataRow dr in DT2.Rows)
                //{
                //    strEMP += " EMP_CD='" + dr[0].ToString() + "' OR";
                //}
                //strEMP = strEMP.Substring(0, strEMP.Length - 2);
                //WriteLog("-->SelectSqlServerEmpSum:" + DT2.Rows.Count.ToString() + '\r', 1);

                string SQL = "SELECT EMP_CD, START_DATE, CLASS_CD from PUB.EMPCLASSR ";
                DataTable DT3 = ProHelp.QueryProgress(SQL);
                if (DT3 == null)
                {
                    WriteLog("-->Query ProgressEmpClassSumError" + '\r', 2);
                    return;
                }
                else if (DT3.Rows.Count == 0)
                {
                    WriteLog("-->Query ProgressEmpClassSum:0" + '\r', 2);
                    return;
                }
                WriteLog("-->Query ProgressEmpClassSum:" + DT3.Rows.Count.ToString() + '\r', 1);

                //String connsql = "server=192.168.60.111;userid=root;password=foxlink;database=;charset=utf8";
                //MySqlConnection MySqlConn = new MySqlConnection(connsql);
                //MySqlConn.Open();
                //MySqlTransaction tx = MySqlConn.BeginTransaction();
                //MySqlCommand cmd = new MySqlCommand();
                ////cmd.CommandTimeout = 30;
                //cmd.Connection = MySqlConn;
                //cmd.Transaction = tx;
                //连接oracle的数据库
                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;
                int UpdateSumk = 0;
                int InsertSumk = 0;
                string strSQLUpdate = "";
                string strSQLInsert = "insert all";
                try
                {
                    foreach (DataRow dr in DT3.Rows)
                    {
                        //WriteLog(dr[0].ToString() + '\r', 1);
                        TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(dr[1].ToString());
                        int d = ts.Days;
                        string strClass = dr[2].ToString().Substring(0, dr[2].ToString().Length - 1);
                        List<string> list = strClass.Split(new string[] { "," }, StringSplitOptions.None).ToList<string>();
                        for (int i = 0; i < 2; i++)
                        {
                            if ((d + i) >= list.Count) continue;
                            if (!ClassArray3.Contains(dr[0].ToString() + DateTime.Now.AddDays(i).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + list[d + i]))
                            {
                                if (ClassArray2.Contains(dr[0].ToString() + DateTime.Now.AddDays(i).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)))
                                {
                                    string sa = list[d + i] == "\\" ? "\\\\" : list[d + i];
                                    if (i == 0)
                                    {
                                        //strSQLUpdate = "update emp_class SET class_no ='" + sa + "',update_time=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') where ID='" + dr[0].ToString() + "'and emp_date='" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'";
                                       
                                        strSQLUpdate = "update emp_class SET class_no ='" + sa + "',update_time=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') where (class_no ='' or class_no='502') and ID='" + dr[0].ToString() + "'and emp_date=to_date('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'YYYY/MM/DD')";
                                    }
                                    else
                                    { strSQLUpdate = "update emp_class SET class_no ='" + sa + "',update_time=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') where ID='" + dr[0].ToString() + "' and emp_date=to_date('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'YYYY/MM/DD')"; }
                                    oracmd.CommandText = strSQLUpdate;
                                    int it = oracmd.ExecuteNonQuery();
                                    if (it > 0) UpdateSumk++;
                                    //WriteLog("-->Update111_EmpClassOK:" + k + strSQLUpdate + '\r', 1);
                                }
                                else
                                {

                                    ++InsertSumk;
                                    string sb = list[d + i] == "\\" ? "\\\\" : list[d + i];
                                    //strSQLInsert += " ('" + dr[0].ToString() + "','"
                                    //                     + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "','"
                                    //                     + sb + "',"
                                    //                     + "curdate()" + "),";
                                    strSQLInsert += " into emp_class (id,emp_date,class_no,update_time) values('" + dr[0].ToString() + "',"
                                                        + "to_date" + "('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'yyyy/MM/dd'),'"
                                                        + sb + "',"
                                                        + "to_date(to_char(SYSDATE,'yyyy/MM/dd'),'yyyy/MM/dd '))";
                                   // insert into CUSLOGS(STARTTIME) values(to_date('2009-5-21 18:55:49','yyyy/mm/dd HH24:MI:SS'))
                                    if (InsertSumk > 0 && InsertSumk % 20 == 0)
                                    {
                                        //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                                        strSQLInsert += " SELECT 1 FROM DUAL";
                                        oracmd.CommandText = strSQLInsert;
                                        oracmd.ExecuteNonQuery();

                                        strSQLInsert = "insert all";
                                    }

                                }
                            }
                        }

                    }
                    if (InsertSumk % 20 != 0)
                    {
                        //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                        strSQLInsert += " SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();

                        //tx.Commit();
                    }
                    //else
                    //{

                    //    tx.Commit();
                    //}
                    //如果emp_class有資料，但是班別為空，則給默認班別39
                    //strSQLUpdate = "update swipecard.emp_class set class_no='39' where emp_date>=curdate() and emp_date<date_add(CURDATE(),interval 2 day) and class_no=''";
                    strSQLUpdate = "update emp_class set class_no='502' where emp_date>=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') and emp_date<to_date(to_char(sysdate+2,'YYYY/MM/DD') ,'YYYY/MM/DD') and class_no is null";
                    oracmd.CommandText = strSQLUpdate;
                    oracmd.ExecuteNonQuery();


                    ortx.Commit();

                    WriteLog("-->Update_ORACLE_EmpClassOK:" + UpdateSumk + '\r', 1);
                    WriteLog("-->insert_ORACLE_EmpClassOK:" + InsertSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert_ORACLE_EmpClassError,Rollback :" + ex.Message + '\r', 2);
                }
                finally
                {
                    ortx.Dispose();
                    OracleConn.Close();
                }
                #endregion
            }
           

        }
        //凌晨三点半抛数据到Oracle
        private void InsertOracle_empClass0330(string strBG)
        {
            //今天和明天的都会Update和Insert.
            SqlHelp sqlHelp = new SqlHelp();
            OracleHelp oracleHelp = new OracleHelp();
            ProgressHelp ProHelp = new ProgressHelp();
          
            //HH24:MI:SS
            //查询oracle数据库
            string strSqlOracleEmp = " select ID, EMP_DATE, CLASS_NO FROM emp_class where emp_date>=to_date(to_char(SYSDATE - 1,'YYYY/MM/DD'),'YYYY/MM/DD')and emp_date<to_date(to_char(sysdate+2,'YYYY/MM/DD') ,'YYYY/MM/DD') order by ID, EMP_DATE"; 
            //string strSqlMySqlEmp = "select ID, emp_date, class_no FROM swipecard.emp_class where emp_date>=curdate() and emp_date<date_add(CURDATE(),interval 2 day) order by ID, emp_date";
            //string strSqlOracleEmp = "select * from emp_class";
       
            if (strBG == "CSBG")
            {
                #region CSBG
                //60.111

               //ataTable DT1 = MySqlHelp.QueryDBC(strSqlMySqlEmp);    
                //oracle数据库中的表存入DT1表中
                DataTable DT1 = oracleHelp.OrcaleQuery(strSqlOracleEmp);
                //沒数据的时候返回的
                if (DT1 == null)
                {
                    WriteLog("-->Query 155ClassError" + '\r', 2);
                    return;
                }

                //存字符串的数组(数据库中的所有数据)
                List<string> ClassArray3 = new List<string>();
                //遍历oracle数据库中的emp_class表
                foreach (DataRow dr in DT1.Rows)
                {
                    //把数据库中的数据存到数组中
                    ClassArray3.Add(dr[0].ToString() + Convert.ToDateTime(dr[1].ToString()).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + dr[2].ToString());
                }
                //存字符串的数组(只存工号和更新的日期)
                List<string> ClassArray2 = new List<string>();
                foreach (DataRow dr in DT1.Rows)
                {
                    //把数据库中的工号和更新日期存到数组中
                    ClassArray2.Add(dr[0].ToString() + Convert.ToDateTime(dr[1].ToString()).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                }
                WriteLog("-->Query 155ClassSum:" + DT1.Rows.Count.ToString() + '\r', 1);

              

                //查询出progress数据库中的数据
                string SQL = "SELECT EMP_CD, START_DATE, CLASS_CD from PUB.EMPCLASSR ";
                //查询的数据存入table中
                DataTable DT3 = ProHelp.QueryProgress(SQL);
                if (DT3 == null)
                {
                    //没有数据直接返回
                    WriteLog("-->Query ProgressEmpClassSumError" + '\r', 2);
                    return;
                }
                else if (DT3.Rows.Count == 0)
                {
                    WriteLog("-->Query ProgressEmpClassSum:0" + '\r', 2);
                    return;
                }
                WriteLog("-->Query ProgressEmpClassSum:" + DT3.Rows.Count.ToString() + '\r', 1);
                
                //String connsql = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.144.187)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SCARD)));Persist Security Info=True;User ID=swipe;Password=mis_swipe";
                //MySqlConnection MySqlConn = new MySqlConnection(connsql);
                //MySqlConn.Open();
                //MySqlTransaction tx = MySqlConn.BeginTransaction();
                //MySqlCommand cmd = new MySqlCommand();
                ////cmd.CommandTimeout = 30;
                //cmd.Connection = MySqlConn;
                //cmd.Transaction = tx;

                //连接oracle的数据库
                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;
                ////这个参数需要指定每次批插入的记录数
                //int recc = 20;
                //oracmd.ArrayBindCount = recc;
                ////在这个命令行中,用到了参数,参数我们很熟悉,但是这个参数在传值的时候

                ////用到的是数组,而不是单个的值,这就是它独特的地方

                //oracmd.CommandText = "insert into dept values(:id, :emp_date, :class_no, :update_time)";
                //OracleConn.Open();

                ////下面定义几个数组,分别表示三个字段,数组的长度由参数直接给出

                //int[] id = new int[recc];

                //string[] emp_date = new string[recc];

                //string[] class_no = new string[recc];
                //string[] update_time = new string[recc];
              
                //更新的数据
                int UpdateSumk = 0;
                //插入的数据
                int InsertSumk = 0;
                string strSQLUpdate = "";
                //插入语句
                //string strSQLInsert = "insert into emp_class VALUES (id,emp_date,class_no,update_time)";
                //string strSQLInsert = "insert into emp_class (id,emp_date,class_no,update_time) values";
                string strSQLInsert = "insert all";
                
                try
                {
                    foreach (DataRow dr in DT3.Rows)
                    {
                       // WriteLog(dr[0].ToString() + '\r', 1);
                       
                        //更新日期为10/28  今天为11/2
                        TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(dr[1].ToString());
                        //相差的天数  5d
                        int d = ts.Days;
                        //比较天数
                        string strClass = dr[2].ToString().Substring(0, dr[2].ToString().Length - 1);
                        //把逗号去掉都存入数组中
                        List<string> list = strClass.Split(new string[] { "," }, StringSplitOptions.None).ToList<string>();
                        
                        for (int i = 0; i < 2; i++)
                        {
                            if ((d + i) >= list.Count) continue; 
                            //如果表中没有数据库的数据
                            if (!ClassArray3.Contains(dr[0].ToString() + DateTime.Now.AddDays(i).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + list[d + i]))
                            {
                                //没有除了班别的//ClassArray2.Contains(dr[0].ToString() + DateTime.Now.AddDays(i).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo))
                                if (ClassArray2.Contains(dr[0].ToString() + DateTime.Now.AddDays(i).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo)))
                                {
                                    //更新两条数据两条数据  不能有重复的
                                    string sa = list[d + i] == "\\" ? "\\\\" : list[d + i];
                                   // strSQLUpdate = "update emp_class SET class_no ='" + sa + "',update_time=curdate() where ID='" + dr[0].ToString() + "'and emp_date='" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'";
                                    strSQLUpdate = "update emp_class SET class_no ='" + sa + "',update_time=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') where ID='" + dr[0].ToString() + "' and emp_date=to_date('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'YYYY/MM/DD')";
                                    
                                    //oracle数据库更新语句
                                    oracmd.CommandText = strSQLUpdate;
                                    oracmd.ExecuteNonQuery();
                                    UpdateSumk++;
                                    //WriteLog("-->Update111_EmpClassOK:" + k + strSQLUpdate + '\r', 1);
                                }
                                else
                                {

                                    //插入两条数据
                                    // ('00002',to_date('2017/11/02','YYYY/MM/DD'),'5',to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD'))
                                   
                                    // ('00002',to_date('2017/11/02','YYYY/MM/DD'),'5',to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD'))
                                    ++InsertSumk;
                                    string sb = list[d + i] == "\\" ? "\\\\" : list[d + i];
                                    //emp_class (id,emp_date,class_no,update_time)
                                    strSQLInsert += " into emp_class (id,emp_date,class_no,update_time) values('" + dr[0].ToString() + "',"
                                                         + "to_date" + "('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," +"'YYYY/MM/DD'),'"
                                                         + sb + "',"
                                                         + "to_date(to_char(SYSDATE,'YYYY/MM/DD') ,'YYYY/MM/DD'))";
      
                                    
                                    //strSQLInsert += "('" + dr[0].ToString() + "',"
                                    //                     + "to_date" + "('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'YYYY/MM/DD'),'"
                                    //                     + sb + "',"
                                    //                     + "to_date(to_char(SYSDATE,'YYYY/MM/DD')" + ",'YYYY/MM/DD'))";

                                    if (InsertSumk > 0 && InsertSumk % 20 == 0){
                                        //strSQLInsert.Length - 1
                                        //strSQLInsert.Substring(0, 5046)
                                        strSQLInsert = strSQLInsert + " SELECT 1 FROM DUAL";
                                        oracmd.CommandText = strSQLInsert;
                                        oracmd.ExecuteNonQuery();

                                        strSQLInsert = "insert all";
                                    }

                                }
                            }
                        }

                    }
                    if (InsertSumk % 20 != 0)
                    {
                        strSQLInsert = strSQLInsert + " SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();
                        //ortx.Commit();
                    }
                    //else
                    //{
                    //    ortx.Commit();
                    //}
                    //如果emp_class有資料，但是班別為空，則給默認班別4
                    //如果emp_class有資料，但是班別為空，則給默認班別39
                    strSQLUpdate = "update emp_class set class_no='502' where emp_date>=to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') and emp_date<to_date(to_char(sysdate+2,'YYYY/MM/DD') ,'YYYY/MM/DD') and class_no is null";
                    oracmd.CommandText = strSQLUpdate;
                    oracmd.ExecuteNonQuery(); ;
                    ortx.Commit();

                    
                    

                    WriteLog("-->Update155_EmpClassOK:" + UpdateSumk + '\r', 1);
                    WriteLog("-->insert155_EmpClassOK:" + InsertSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert155_EmpClassError,Rollback :" + ex.Message + '\r', 2);
                }
                finally
                {
                    ortx.Dispose();
                    OracleConn.Close();
                }
                #endregion
            }
           

        }
        //抛转Progress数据库中pub.deptment2表的数据到oracle的SWIPE.DEPT_RELATION中
        private void InsertOracle_dept_relation_Progress(string strBG)
        {

            if (strBG == "CSBG")
            {
                OracleHelp oracleHelp = new OracleHelp();
                ProgressHelp progressHelp = new ProgressHelp();
                //查询oracle数据库
                string strSqlOracleEmp = "SELECT PARENT_DEPT, DEPID, DEPT_LEVEL, COSTID,DEPT_NAME  FROM SWIPE.DEPT_RELATION ";

                DataTable DT1 = oracleHelp.OrcaleQuery(strSqlOracleEmp);
                //沒数据的时候返回的
                if (DT1 == null)
                {
                    WriteLog("-->Query Oracle_ClassError" + '\r', 2);
                    return;
                }

                //查询progress数据库
                //\"start()\""
                //\"ning\""
                String sql = @"SELECT DEPT_UP, DEPT_CD,""LEVEL"", EXP_DEPT,DEPT_NAME FROM pub.deptment2 ";
                
                DataTable DT2 = progressHelp.QueryProgress(sql);
                if (DT2 == null)
                {
                    WriteLog("-->Query Progress_deptment2_SumError" + '\r', 2);
                    return;
                }
                else if (DT2.Rows.Count == 0)
                {
                    WriteLog("-->Query Progress_deptment2_Sum:0" + '\r', 2);
                    return;
                }

                //连接oracle的数据库
                //data source=192.168.144.187/SCARD ;User Id=swipe;Password=mis_swipe;"
                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;



                //先删除oracle数据库中SWIPE.DEPT_RELATION表的数据
                string strSQLDelete = "DELETE FROM DEPT_RELATION";
                oracmd.CommandText = strSQLDelete;
                oracmd.ExecuteNonQuery();
                int InsertSumk = 0;
                string strSQLInsert = "insert all";
                //把Progress数据库中的数据导入到oracle中
                try
                {
                    foreach (DataRow dr in DT2.Rows)
                    {
                        //dr["DEPT_NAME"].ToString()
                        //insert all INTO DEPT_RELATION (PARENT_DEPT,DEPID, DEPT_LEVEL, COSTID) VALUES('1592','1589','5','1589')
                        ++InsertSumk;
                        //strSQLInsert += " INTO DEPT_RELATION (PARENT_DEPT,DEPID, DEPT_LEVEL, COSTID) VALUES('" + dr["DEPT_UP"].ToString() + "','"
                        //                              + dr["DEPT_CD"].ToString() + "','"
                        //                              + dr["LEVEL"].ToString() + "','"
                        //                              + dr["EXP_DEPT"].ToString() + "')";


                        string DEPT_NAMEStr = "";
                        if (dr["DEPT_NAME"].ToString().IndexOf("'") != -1) //判断字符串是否含有单引号,如：DEPT_NAME为 X'成型生產一課                      
                            DEPT_NAMEStr = dr["DEPT_NAME"].ToString().Replace("'", "''");
                        else
                            DEPT_NAMEStr = dr["DEPT_NAME"].ToString();

                        strSQLInsert += " INTO SWIPE.DEPT_RELATION (PARENT_DEPT,DEPID, DEPT_LEVEL, COSTID,DEPT_NAME) VALUES('" + dr["DEPT_UP"].ToString() + "','"
                        + dr["DEPT_CD"].ToString() + "','"
                        + dr["LEVEL"].ToString() + "','"
                        + dr["EXP_DEPT"].ToString() + "','" + DEPT_NAMEStr + "')";

                        if (InsertSumk > 0 && InsertSumk % 10 == 0)
                        {
                            //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                            strSQLInsert += " SELECT 1 FROM DUAL";
                            oracmd.CommandText = strSQLInsert;
                            oracmd.ExecuteNonQuery();
                            //ortx.Commit();
                            //strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate)  VALUES ";
                            strSQLInsert = "insert all";
                           
                        }
                       
                    }

                    //WriteLog("-->InsertSumk:" + InsertSumk + '\r', 1);

                    if (InsertSumk %  10 != 0)
                    {
                        // strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                        strSQLInsert += " SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();
                        ortx.Commit();


                    
                    }
                    else
                    {
                        ortx.Commit();
                    }

                
                    WriteLog("-->insert_dept_relation_OK:" + InsertSumk + '\r', 1);

                }
                catch (Exception ex)
                {
                    
                    ortx.Rollback();
                    WriteLog("-->insert_dept_relation_Error,Rollback :" + ex.Message + '\r' + strSQLInsert, 2);
                }
                finally
                {
                    OracleConn.Close();
                }

            }


        }

  
        //更新Oracle数据库中星期天班别为默认班别502的为星期六的班别
        private void InsertMysql_empClassNull(string strBG)
        {
            //SqlHelp sqlHelp = new SqlHelp();
            OracleHelp oracleHelp = new OracleHelp();
            int UpdateSumk = 0;

            //string sql = "select id,class_no from swipecard.emp_class where emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')-6),'%Y-%m-%d')";
            if (strBG == "CSBG")
            {   //获取当前日期对应的周六的班别
                // string sql = "select id,class_no from swipecard.emp_class where emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')+1),'%Y-%m-%d') and id in (select id from swipecard.emp_class where emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')),'%Y-%m-%d') and class_no='502')";// and class_no <> '502'";
                string sql = "select id,class_no from emp_class where to_char(emp_date-1,'d')='5' and id in (select id from emp_class where to_char(emp_date-1,'d')='6' and to_char(sysdate,'d')='6' and class_no='502')";
                DataTable DT5 = oracleHelp.OrcaleQuery(sql);
                //获取当前日期对应得周日班别
                //string SunSql = "select id,class_no from swipecard.emp_class where emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')-7),'%Y-%m-%d')";
                // string SunSql = "select id,class_no from swipecard.emp_class where emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')),'%Y-%m-%d') and class_no='502'";
                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;
                try
                {
                    //删除周日为502的班别
                    //string strSQL = "delete from swipecard.emp_class where  emp_date=DATE_FORMAT(subdate(curdate(),date_format(curdate(),'%w')),'%Y-%m-%d') and class_no='502'";
                    string strSQL = "delete from emp_class where to_char(emp_date-1,'d')='6' and to_char(sysdate,'d')='6' and class_no='502'";

                    //WriteLog(DateTime.Now + "-->delete:" + " " + '\r', 1);
                    oracmd.CommandText = strSQL;
                    oracmd.ExecuteNonQuery();

                    string strSQLInsert = "insert all ";

                    //循环插入星期天的班别，class_no为周六的班别
                   // strSQL = "insert into emp_class VALUES ";
                    foreach (DataRow dr5 in DT5.Rows)
                    {
                        // sa = dr[1].Equals(DBNull.Value) ? "NULL" : "'" + dr[1].ToString() + "'";
                        // sb = dr[2].Equals(DBNull.Value) ? "NULL" : "'" + dr[2].ToString() + "'";
                        //sc = dr[3].Equals(DBNull.Value) ? "NULL" : "'" + dr[3].ToString() + "'";
                        strSQLInsert += "into emp_class values  ('" + dr5["id"].ToString() + "',"
                                         + " to_date(to_char(sysdate,'yyyy/mm/dd'),'yyyy/mm/dd') " + ","
                                         + dr5["class_no"].ToString() + ","
                                         + "to_date(to_char(sysdate,'yyyy/mm/dd'),'yyyy/mm/dd')" + ")";
                        UpdateSumk++;
                    }
                    //去除最后的逗号
                    strSQLInsert += "select 1 from dual";
                    oracmd.CommandText = strSQLInsert;
                    oracmd.ExecuteNonQuery();
                    ortx.Commit();
                    WriteLog("-->insert155_emp_class:" + UpdateSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert155_emp_class,Rollback :" + ex.Message + '\r', 2);
                }
                

            }

            }
            //Progress中PUB.deptcsr表的数据到LMT_DEPT

        private void InsertOracle_LMT_DEPT_Progress(string strBG) {
            if (strBG == "CSBG")
            {
                OracleHelp oracleHelp = new OracleHelp();
                ProgressHelp progressHelp = new ProgressHelp();
                //查询oracle数据库
                string strSqlOracleEmp = "SELECT DEPID, DEPTNAME,COSTID,DEPTNAME2 FROM LMT_DEPT";

                DataTable DT1 = oracleHelp.OrcaleQuery(strSqlOracleEmp);
                //沒数据的时候返回的
                if (DT1 == null)
                {
                    WriteLog("-->Query Oracle_ClassError" + '\r', 2);
                    return;
                }
                //查询progress数据库

                String sql = "SELECT bmbh,bmmc,kbmbh,kbmmc FROM PUB.deptcsr";
                DataTable DT2 = progressHelp.QueryProgress(sql);
                if (DT2 == null)
                {
                    WriteLog("-->Query Progress_deptment2_SumError" + '\r', 2);
                    return;
                }
                else if (DT2.Rows.Count == 0)
                {
                    WriteLog("-->Query Progress_deptment2_Sum:0" + '\r', 2);
                    return;
                }

                //连接oracle的数据库
                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;

                //先删除oracle数据库中SWIPE.DEPT_RELATION表的数据
                string strSQLDelete = "DELETE FROM LMT_DEPT";
                oracmd.CommandText = strSQLDelete;
                oracmd.ExecuteNonQuery();

                int InsertSumk = 0;
                string strSQLInsert = "insert all";
                //把Progress数据库中的数据导入到oracle中
                try {
                    foreach (DataRow dr in DT2.Rows) {
                        ++InsertSumk;
                        strSQLInsert += " INTO LMT_DEPT (DEPID,DEPTNAME, COSTID, DEPTNAME2) VALUES('" + dr["bmbh"].ToString() + "','"
                                                     + dr["bmmc"].ToString() + "','"
                                                     + dr["kbmbh"].ToString() + "','"
                                                     + dr["kbmmc"].ToString() + "')";
                        if (InsertSumk > 0 && InsertSumk % 10 == 0)
                        {
                            //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                            strSQLInsert += " SELECT 1 FROM DUAL";
                            oracmd.CommandText = strSQLInsert;
                            oracmd.ExecuteNonQuery();
                            //ortx.Commit();
                            //strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate)  VALUES ";
                            strSQLInsert = "insert all";
                        }
                    }
                    if (InsertSumk % 10 != 0)
                    {
                        // strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                        strSQLInsert += " SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();
                        ortx.Commit();
                    }
                    else
                    {
                        ortx.Commit();
                    }


                    WriteLog("-->insert_LMT_DEPT_OK:" + InsertSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert_LMT_DEPT_Error,Rollback :" + ex.Message + '\r', 2);
                }
            }
        
        
        
        
        }
        //從新圖的sqlserver數據庫更新新員工資料
        private void InsertMySql_testemployee_SqlServer(string strBG)
        {
            SqlHelp sqlHelp = new SqlHelp();
            ProgressHelp ProHelp = new ProgressHelp();
            OracleHelp oracleHelp = new OracleHelp();
            if (strBG == "CSBG")
            {
                #region CSBG
                //60.111
                string strSqlMySqlEmp = "SELECT ID, NAME, DEPID, DEPNAME, DIRECT, CARDID,COSTID, ISONWORK FROM CSR_EMPLOYEE_ONE";
                DataTable DT1 = oracleHelp.OrcaleQuery(strSqlMySqlEmp);
                if (DT1 == null)
                {
                    WriteLog("-->Query 155TestemployeeError" + '\r', 2);
                    return;
                }
                
                DT1.PrimaryKey = new DataColumn[] { DT1.Columns["ID"] };//设置第一列为主键 

                String sql = "SELECT zgbh, icid FROM mf_employee where lrzx in ('PQ','MQ','PN','IR','AI')"; // 查询语句
                //String sql = "select ygbh,fdate,bc from KQ_RECORD_BC where fdate>=CONVERT(varchar(100), GETDATE()-3, 111) and fdate<CONVERT(varchar(100), GETDATE()+2, 111) and ( " + strEMP + " )"; // 查询语句
                DataTable DT2 = sqlHelp.QuerySqlServerDB(sql);
                if (DT2 == null)
                {
                    WriteLog("-->Query SqlServerdeptError" + '\r', 2);
                    return;
                }
                else if (DT2.Rows.Count == 0)
                {
                    WriteLog("-->Query SqlServerdeptSum:0" + '\r', 2);
                    return;

                }
                DT2.PrimaryKey = new DataColumn[] { DT2.Columns["id"] }; //设置第一列为主列

                WriteLog("-->SelectSqlServerSum:" + DT2.Rows.Count.ToString() + '\r', 1);

                //string SQL = "SELECT EMP_CD, EMP_NAME, DEPT_CD, DEPT_NAME, D_I_CD,'cardid', EXP_DEPT, EMP_STATUS FROM PUB.EMPR";
                //DataTable DT3 = ProHelp.QueryProgress(SQL);
                //if (DT3 == null)
                //{
                //    WriteLog("-->Query ProgressEMPRSumError" + '\r', 2);
                //    return;
                //}
                //else if (DT3.Rows.Count == 0)
                //{
                //    WriteLog("-->Query ProgressEMPRSum:0" + '\r', 2);
                //    return;
                //}

                //WriteLog("-->Query ProgressEMPRSum:" + DT3.Rows.Count.ToString() + '\r', 1);

                //String connsql = "server=192.168.60.111;userid=root;password=foxlink;database=;charset=utf8";
                ////String connsql = "server=10.64.155.200;userid=root;password=mysql;database=mysql;charset=utf8";
                //MySqlConnection MySqlConn = new MySqlConnection(connsql);
                //MySqlConn.Open();
                //MySqlTransaction tx = MySqlConn.BeginTransaction();
                //MySqlCommand cmd = new MySqlCommand();
                ////cmd.CommandTimeout = 300;
                //cmd.Connection = MySqlConn;
                //cmd.Transaction = tx;

                //连接oracle的数据库
                String connsql = "data source=10.72.1.172/SCARD ;User Id=swipe;Password=mis_swipe;";
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;
                int UpdateSumk = 0;
                int InsertSumk = 0;
                string strSQLUpdate = "";
                //string strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate) VALUES ";
                string strSQLInsert = "insert all";
                try
                {
                    //foreach (DataRow dr in DT1.Rows)
                    //{
                    //    DataRow MSdr = DT2.Rows.Find(dr["ID"].ToString());
                    //    if (MSdr == null)
                    //    {
                    //        strSQLUpdateIswork = "update swipecard.testemployee SET isOnWork =1,updateDate =curdate() where isOnWork=0 and ID = '" + dr["ID"].ToString() + "'";
                    //        cmd.CommandText = strSQLUpdateIswork;
                    //        int i = cmd.ExecuteNonQuery();
                    //        if (i > 0) { WriteLog("-->Update111_" + dr["ID"].ToString() + " Iswork=1OK:" + UpdateSumk + strSQLUpdate + '\r', 1); }
                    //    }

                    //}
                    foreach (DataRow dr in DT2.Rows)
                    {
                        if (dr["cardid"].ToString() == "") continue;
                        DataRow Tempdr = DT1.Rows.Find(dr["id"].ToString());
                        //如果Tempdr为空说明Mysql中没有此员工，需要insert
                        //如果Tempdr不为空说明Mysql中存在此员工，继续判断是要Update还是Insert
                        if (Tempdr != null)
                        {
                            //此判断判断员工是否在职
                            if (Convert.ToInt32(Tempdr["isOnWork"].ToString()) == 0)
                            {
                                //如果Oracle数据库中的人员资料与新图数据库中的资料不一样就要更新
                                if (Tempdr["ID"].ToString() != dr["id"].ToString() || Tempdr["Name"].ToString() != dr["name"].ToString() ||
                                    Tempdr["depid"].ToString() != dr["depid"].ToString() || Tempdr["depname"].ToString() != dr["depname"].ToString() ||
                                    Tempdr["Direct"].ToString() != dr["direct"].ToString() || Tempdr["cardid"].ToString() != dr["cardid"].ToString() ||
                                    Tempdr["costID"].ToString() != dr["costid"].ToString())
                                {
                                    //if (dr["EMP_STATUS"].ToString() == string.Empty)
                                    //{ Istatus = 0; }
                                    //else if (Convert.ToInt32(dr["EMP_STATUS"].ToString()) == 0)
                                    //{ Istatus = 1; }
                                    //else if (Convert.ToInt32(dr["EMP_STATUS"].ToString()) == 1)
                                    //{ Istatus = 0; }
                                    strSQLUpdate = "update CSR_EMPLOYEE_ONE SET Name = '" + dr["name"].ToString() +
                                                                                  "',depid = '" + dr["depid"].ToString() +
                                                                                  "',depname = '" + dr["depname"].ToString() +
                                                                                  "',Direct = '" + dr["direct"].ToString() +
                                                                                  "',cardid = '" + dr["cardid"].ToString() +
                                                                                  "',costID = '" + dr["costid"].ToString() +
                                                                                  "',updateDate =to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') WHERE ID = '" + dr["id"].ToString() + "' ";
                                    //strSQLUpdate = "update CSR_EMPLOYEE_ONE SET Name = '" + dr["EMP_NAME"].ToString() +
                                    //                                          "',depid = '" + dr["DEPT_CD"].ToString() +
                                    //                                          "',depname = '" + dr["DEPT_NAME"].ToString() +
                                    //                                          "',Direct = '" + dr["D_I_CD"].ToString() +
                                    //                                          "',cardid = '" + strCardID +
                                    //                                          "',costID = '" + dr["EXP_DEPT"].ToString() +
                                    //                                          "',isOnWork = " + Istatus + "  ,updateDate =to_date(to_char(SYSDATE,'YYYY/MM/DD'),'YYYY/MM/DD') WHERE ID = '" + dr["EMP_CD"].ToString() + "' ";
                                    oracmd.CommandText = strSQLUpdate;
                                    oracmd.ExecuteNonQuery();
                                    UpdateSumk++;
                                    //WriteLog("-->Update111_EmpOK:" + UpdateSumk + strSQLUpdate + '\r', 1);
                                }
                            }
                        }
                        else
                        {
                            //如果Tempder为空，说明Mysql中要Insert
                            ++InsertSumk;
                            //if (dr["cardid"].ToString() == null)
                            //{
                            //    strSQLInsert += " INTO CSR_EMPLOYEE_ONE (ID,NAME,DEPID,DEPNAME,DIRECT,CARDID,COSTID,UPDATEDATE) VALUES ('" + dr["id"].ToString() + "','"
                            //                          + dr["name"].ToString() + "','"
                            //                          + dr["depid"].ToString() + "','"
                            //                          + dr["depname"].ToString() + "','"
                            //                          + dr["direct"].ToString() + "','"
                            //                          + "null" + "','"
                            //                          + dr["costid"].ToString() + "',"
                            //                          + "to_date(to_char(SYSDATE,'YYYY/MM/DD')" + ",'YYYY/MM/DD'))";
                            //}
                            //else {

                            
                                strSQLInsert += " INTO CSR_EMPLOYEE_ONE (ID,NAME,DEPID,DEPNAME,DIRECT,CARDID,COSTID,UPDATEDATE) VALUES ('" + dr["id"].ToString() + "','"
                                                          + dr["name"].ToString() + "','"
                                                          + dr["depid"].ToString() + "','"
                                                          + dr["depname"].ToString() + "','"
                                                          + dr["direct"].ToString() + "','"
                                                          + dr["cardid"].ToString() + "','"
                                                          + dr["costid"].ToString() + "',"
                                                          + "to_date(to_char(SYSDATE,'YYYY/MM/DD')" + ",'YYYY/MM/DD'))";
                            
                            //}

                            //strSQLInsert += " INTO CSR_EMPLOYEE_ONE (ID,NAME,DEPID,DEPNAME,DIRECT,CARDID,COSTID,UPDATEDATE) VALUES('" + dr["EMP_CD"].ToString() + "','"
                            //                      + dr["EMP_NAME"].ToString() + "','"
                            //                      + dr["DEPT_CD"].ToString() + "','"
                            //                      + dr["DEPT_NAME"].ToString() + "','"
                            //                      + dr["D_I_CD"].ToString() + "','"
                            //                      + strCardID + "','"
                            //                      + dr["EXP_DEPT"].ToString() + "',"
                            //                      + "to_date(to_char(SYSDATE,'YYYY/MM/DD')" + ",'YYYY/MM/DD'))";
                            //WriteLog("-->Insert111_EmpOK:" + InsertSumk + " ('" + dr["EMP_CD"].ToString() + "','"
                            //                      + dr["EMP_NAME"].ToString() + "','"
                            //                      + dr["DEPT_CD"].ToString() + "','"
                            //                      + dr["DEPT_NAME"].ToString() + "','"
                            //                      + dr["D_I_CD"].ToString() + "','"
                            //                      + SqlServerdr["kh"].ToString() + "','"
                            //                      + dr["EXP_DEPT"].ToString() + "',"
                            //                      + "curdate()" + '\r', 1);
                            if (InsertSumk > 0 && InsertSumk % 10 == 0)
                            {
                                //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                                strSQLInsert += "SELECT 1 FROM DUAL";
                                oracmd.CommandText = strSQLInsert;
                                oracmd.ExecuteNonQuery();

                                //strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate)  VALUES ";
                                strSQLInsert = "insert all";
                            }
                        }

                    }
                    if (InsertSumk % 10 != 0)
                    {
                        //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                        strSQLInsert += "SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();
                        ortx.Commit();
                    }
                    else
                    {
                        ortx.Commit();
                    }

                    WriteLog("-->Update155_EmployeeOK:" + UpdateSumk + '\r', 1);
                    WriteLog("-->insert155_EmployeeOK:" + InsertSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert155_EmpClassError,Rollback :" + ex.Message + '\r', 2);
                }
                finally
                {
                    ortx.Dispose();
                    OracleConn.Close();
                }
                #endregion

            }
           

        }
        //從Progress數據庫更新員工資料
        private void InsertOracle_employee_Progress3(string strBG)
        {
            SqlHelp sqlHelp = new SqlHelp();
            OracleHelp oraclehelp = new OracleHelp();
            ProgressHelp ProHelp = new ProgressHelp();

            if (strBG == "CSBG")
            {
                #region CSBG
                //60.111
                //string strSqlMySqlEmp = "select  ID, Name, depid, depname, Direct, cardid, costID, isOnWork from swipecard.testemployee";
                //查询oracle中的数据
                string strSqlMySqlEmp = "SELECT ID, NAME, DEPID, DEPNAME, DIRECT, CARDID,COSTID, ISONWORK FROM SWIPE.CSR_EMPLOYEE";
                //DataTable DT1 = MySqlHelp.QueryDBC(strSqlMySqlEmp);
                DataTable DT1 = oraclehelp.OrcaleQuery_ONE(strSqlMySqlEmp);
                if (DT1 == null)
                {
                    WriteLog("-->Query 155employeeError" + '\r', 2);
                    return;
                }
                DT1.PrimaryKey = new DataColumn[] { DT1.Columns["ID"] };//设置第一列为主键 
                //sqlserver查询语句
                String sql = "SELECT zgbh, icid FROM mf_employee where lrzx in ('IR','AI','PQ','MQ','PN')"; // 查询语句
              //  String sql = "SELECT zgbh, icid FROM mf_employee where icid <>'' ";//where lrzx in ('IR','AI','PQ','MQ','PN')"; // 查询语句
                //String sql = "select ygbh,fdate,bc from KQ_RECORD_BC where fdate>=CONVERT(varchar(100), GETDATE()-3, 111) and fdate<CONVERT(varchar(100), GETDATE()+2, 111) and ( " + strEMP + " )"; // 查询语句
             

                  DataTable DT2 = sqlHelp.QuerySqlServerDB(sql);
                if (DT2 == null)
                {
                    WriteLog("-->Query SqlServerdeptError" + '\r', 2);
                    return;
                }

               // else if (DT2.Rows.Count == 0)
               // {
                //    WriteLog("-->Query SqlServerdeptSum:0" + '\r', 2);
                 //   return;

               // }
                  DT2.PrimaryKey = new DataColumn[] { DT2.Columns["zgbh"] }; //设置第一列为主列

                 WriteLog("-->SelectSqlServerSum:" + DT2.Rows.Count.ToString() + '\r', 1);
                //查询progress中的数据
                 string SQL = "SELECT emp.EMP_CD, emp.EMP_NAME, emp.DEPT_CD, dept.DEPT_NAME, emp.D_I_CD,emp.CRN, emp.EXP_DEPT, emp.EMP_STATUS FROM PUB.EMPR emp, PUB.DEPTMENT2 dept WHERE emp.DEPT_CD=dept.dept_cd";// and emp.out_plant_date is null";

                //string SQL = "SELECT EMP_CD, EMP_NAME, DEPT_CD, DEPT_NAME, D_I_CD, CRN, EXP_DEPT, EMP_STATUS FROM pub.EMPR WHERE SUBSTR(EXP_DEPT_NAME,1,2)='通訊'";
                //string SQL = "SELECT EMP_CD, EMP_NAME, DEPT_CD, DEPT_NAME, D_I_CD, CRN, EXP_DEPT, EMP_STATUS FROM pub.EMPR ";
                DataTable DT3 = ProHelp.QueryProgress(SQL);
                if (DT3 == null)
                {
                    WriteLog("-->Query ProgressEmployeeSumError" + '\r', 2);
                    return;
                }
                else if (DT3.Rows.Count == 0)
                {
                    WriteLog("-->Query ProgressEmployeeSum:0" + '\r', 2);
                    return;
                }
                DT3.PrimaryKey = new DataColumn[] { DT3.Columns["EMP_CD"] };//设置第一列为主键 
                WriteLog("-->Query ProgressEmployeeSum:" + DT3.Rows.Count.ToString() + '\r', 1);



                String connsql = "data source=192.168.78.154/SSGSDB ;User Id=swipe;Password=a2#ks#ssgs;";

              //  String connsql = "data source= 10.72.1.172/scard ;User Id=swipe;Password=mis_swipe;";
               
                OracleConnection OracleConn = new OracleConnection(connsql);
                OracleConn.Open();
                OracleTransaction ortx = OracleConn.BeginTransaction();
                OracleCommand oracmd = new OracleCommand();
                oracmd.Connection = OracleConn;
                oracmd.Transaction = ortx;

                int UpdateSumk = 0;
                int InsertSumk = 0;
                int Istatus = 0;
                string strSQLUpdate = "";
                //string strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate) VALUES ";
                string strSQLInsert = "insert all";
                try
                {
   
                    foreach (DataRow dr in DT3.Rows)
                    {
                       
                        string strCardID = "";
                        //DataRow[] Tempdr = DT1.Select("ID='" + dr["EMP_CD"].ToString() + "'");
                          DataRow SqlServerdr = DT2.Rows.Find(dr["EMP_CD"].ToString());
                          //如果SqlServerdr为空说明Sqlserver中不存在此员工，cardID设置为空,不为空则把sqlserver中的cardID给strCardID
                          if (SqlServerdr != null)
                          {
                              if (SqlServerdr["icid"].ToString() != string.Empty)
                              {

                                  strCardID = SqlServerdr["icid"].ToString();
                              }
                              else
                              {
                                  strCardID = dr["CRN"].ToString();
                              }
                          }
                          else
                          {
                              strCardID = dr["CRN"].ToString();
                             // Sqlserver中不存在此员工,则此员工为离职状态
                              if (dr["CRN"].ToString() == null)
                              {
                                  String strSQLUpdateIswork = "update CSR_EMPLOYEE SET isOnWork =1,updateDate =sysdate where isOnWork=0 and ID = '" + dr["EMP_CD"].ToString() + "'";
                                  oracmd.CommandText = strSQLUpdateIswork;
                                  oracmd.ExecuteNonQuery();
                                  UpdateSumk++;
                              }
                             

                          }

                      //  strCardID = dr["CRN"].ToString();
                          if (strCardID == "") continue;
                        DataRow Tempdr = DT1.Rows.Find(dr["EMP_CD"].ToString());
                        //如果SqlServerdr不为空说明sqlserver中存在此员工，继续判断是要Update还是Insert
                        //如果Tempdr不为空说明Oracle中存在此员工，继续判断是否需要Update
                        if (dr["EMP_STATUS"].ToString() == string.Empty)
                        { Istatus = 0; }
                        else
                        {
                            switch (Convert.ToInt32(dr["EMP_STATUS"].ToString()))
                            {
                                case 1:                              
                                    Istatus = 0;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    Istatus = 1;
                                    break;
                                default:
                                    Istatus = 0;
                                    break;
                            }
                        }
                        if (Tempdr != null)
                        {
                            if (Tempdr["ID"].ToString() != dr["EMP_CD"].ToString() || Tempdr["Name"].ToString() != dr["EMP_NAME"].ToString() ||
                                Tempdr["depid"].ToString() != dr["DEPT_CD"].ToString() || Tempdr["depname"].ToString() != dr["DEPT_NAME"].ToString() ||
                                Tempdr["Direct"].ToString() != dr["D_I_CD"].ToString() || Tempdr["cardid"].ToString() != strCardID ||
                                Tempdr["costID"].ToString() != dr["EXP_DEPT"].ToString() || Tempdr["isOnWork"].ToString() != Istatus.ToString())
                            {
                                //員工狀態  "1"是在職 "2"停薪 "3" 離職 4 "自離  "5 開除 6  資遣7 試用不合格 8 廠區異動
                                //if (dr["EMP_STATUS"].ToString() == string.Empty)
                                //{ Istatus = 0; }
                                //else if (Convert.ToInt32(dr["EMP_STATUS"].ToString()) == 0)
                                //{ Istatus = 1; }
                                //else if (Convert.ToInt32(dr["EMP_STATUS"].ToString()) == 1)
                                //{ Istatus = 0; }

                                //strSQLUpdate = "update swipecard.testemployee SET Name = '" + dr["EMP_NAME"].ToString() +
                                //                                              "',depid = '" + dr["DEPT_CD"].ToString() +
                                //                                              "',depname = '" + dr["DEPT_NAME"].ToString() +
                                //                                              "',Direct = '" + dr["D_I_CD"].ToString() +
                                //                                              "',cardid = '" + strCardID +
                                //                                              "',costID = '" + dr["EXP_DEPT"].ToString() +
                                //                                              "',isOnWork = " + Istatus + "  ,updateDate =curdate() WHERE ID = '" + dr["EMP_CD"].ToString() + "' ";
                                strSQLUpdate = "update CSR_EMPLOYEE SET Name = '" + dr["EMP_NAME"].ToString() +
                                                                              "',depid = '" + dr["DEPT_CD"].ToString() +
                                                                              "',depname = '" + dr["DEPT_NAME"].ToString() +
                                                                              "',Direct = '" + dr["D_I_CD"].ToString() +
                                                                              "',cardid = '" + strCardID +
                                                                              "',costID = '" + dr["EXP_DEPT"].ToString() +
                                                                              "',isOnWork = " + Istatus + "  ,updateDate =sysdate WHERE ID = '" + dr["EMP_CD"].ToString() + "' ";
                                oracmd.CommandText = strSQLUpdate;
                                oracmd.ExecuteNonQuery();
                                UpdateSumk++;
                                //WriteLog("-->Update111_EmpOK:" + UpdateSumk + strSQLUpdate + '\r', 1);
                            }
                        }
                        else
                        {
                            //如果Tempder为空，说明Mysql中要Insert
                            ++InsertSumk;

                            //strSQLInsert += " into emp_class (id,emp_date,class_no,update_time) values('" + dr[0].ToString() + "',"
                            //                            + "to_date" + "('" + DateTime.Now.AddDays(i).ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'," + "'YYYY/MM/DD'),'"
                            //                            + sb + "',"
                            //                            + "to_date(to_char(SYSDATE,'YYYY/MM/DD')" + ",'YYYY/MM/DD'))";
                            //string strSQLInsert = "insert into CSR_EMPLOYEE_ONE (ID,NAME,DEPID,DEPNAME,DIRECT,CARID,COSTID,UPDATEDATE) VALUES";
      
                            //strSQLInsert += " ('" + dr["EMP_CD"].ToString() + "','"
                            //                      + dr["EMP_NAME"].ToString() + "','"
                            //                      + dr["DEPT_CD"].ToString() + "','"
                            //                      + dr["DEPT_NAME"].ToString() + "','"
                            //                      + dr["D_I_CD"].ToString() + "','"
                            //                      + strCardID + "','"
                            //                      + dr["EXP_DEPT"].ToString() + "',"
                            //                      + "curdate()" + "),";
                            strSQLInsert += " INTO CSR_EMPLOYEE (ID,NAME,DEPID,DEPNAME,DIRECT,CARDID,COSTID,UPDATEDATE) VALUES('" + dr["EMP_CD"].ToString() + "','"
                                                  + dr["EMP_NAME"].ToString() + "','"
                                                  + dr["DEPT_CD"].ToString() + "','"
                                                  + dr["DEPT_NAME"].ToString() + "','"
                                                  + dr["D_I_CD"].ToString() + "','"
                                                  + strCardID + "','"
                                                  + dr["EXP_DEPT"].ToString() + "',"
                                                  + "SYSDATE)";
                            //ORA-01400: 无法将 NULL 插入 ("SWIPE"."CSR_EMPLOYEE_ONE"."CARDID")

                            //WriteLog("-->Insert111_EmpOK:" + InsertSumk + " ('" + dr["EMP_CD"].ToString() + "','"
                            //                      + dr["EMP_NAME"].ToString() + "','"
                            //                      + dr["DEPT_CD"].ToString() + "','"
                            //                      + dr["DEPT_NAME"].ToString() + "','"
                            //                      + dr["D_I_CD"].ToString() + "','"
                            //                      + SqlServerdr["kh"].ToString() + "','"
                            //                      + dr["EXP_DEPT"].ToString() + "',"
                            //                      + "curdate()" + '\r', 1);

                            //列“costID”不属于表 datatable1。

                            if (InsertSumk > 0 && InsertSumk % 10 == 0)
                            {
                                //strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                                strSQLInsert += " SELECT 1 FROM DUAL";
                                oracmd.CommandText = strSQLInsert;
                                oracmd.ExecuteNonQuery();
                                //ortx.Commit();
                                //strSQLInsert = "insert into swipecard.testemployee (ID,Name,depid,depname,Direct,cardid,costID,updateDate)  VALUES ";
                                strSQLInsert = "insert all";
                            }
                        }
                        //}
                    }
                    if (InsertSumk % 10 != 0)
                    {
                       // strSQLInsert = strSQLInsert.Substring(0, strSQLInsert.Length - 1);
                        strSQLInsert += " SELECT 1 FROM DUAL";
                        oracmd.CommandText = strSQLInsert;
                        oracmd.ExecuteNonQuery();
                        ortx.Commit();
                    }
                    else
                    {
                        ortx.Commit();
                    }

                    WriteLog("-->Update155_EmployeeOK:" + UpdateSumk + '\r', 1);
                    WriteLog("-->insert155_EmployeeOK:" + InsertSumk + '\r', 1);
                }
                catch (Exception ex)
                {
                    ortx.Rollback();
                    WriteLog("-->insert155_EmployeeError,Rollback :" + ex.Message + '\r', 2);
                }
                finally
                {
                    ortx.Dispose();
                    OracleConn.Close();
                }
                #endregion
            }


        }

        private void Oracle_rawToProgress()
        {
            string sql = @"SELECT id,
       cardid,
       swipecardtime,
       sysdate update_time 
  FROM swipe.raw_record
 WHERE swipecardtime >=
          to_date(to_char(TRUNC(sysdate-1),'yyyy/mm/dd')|| ' 10:00:00','yyyy/mm/dd HH24:MI:SS')
       AND swipecardtime <
              to_date(to_char(TRUNC(sysdate),'yyyy/mm/dd')|| ' 10:00:00','yyyy/mm/dd HH24:MI:SS') AND record_status!='4' and record_status!='8'";
            string result = "NG";

            OracleHelp OraclelHelp = new OracleHelp();
            ProgressHelp ProHelp = new ProgressHelp();

            #region CSBG
            DataTable DT2 = OraclelHelp.OrcaleQuery(sql);
            if (DT2 == null)
            {
                WriteLog("-->Query OracleRawError" + '\r', 2);
                return;
            }//result = "NG: Query OracleRawError"; }
            else if (DT2.Rows.Count == 0) { WriteLog("-->Query OracleRawSum:0" + '\r', 2); return; }//result = "NG: Query OracleRawSum:0"; }
            WriteLog("-->Query OracleRawSum:" + DT2.Rows.Count.ToString() + '\r', 1);

            string ControlString1 = "DSN=KSHR;UID=csruser;PWD=csruser";
            OdbcConnection odbcCon = new OdbcConnection(ControlString1);
            odbcCon.Open();
            OdbcTransaction tx = odbcCon.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = odbcCon;
            cmd.Transaction = tx;
            try
            {
                string sa = "";
                string sb = "";
                int InsertSumk = 0;
                foreach (DataRow dr in DT2.Rows)
                {
                    sa = dr["id"].Equals(DBNull.Value) ? "NULL" : "'" + dr["id"].ToString() + "'";
                    sb = dr["cardid"].Equals(DBNull.Value) ? "NULL" : "'" + dr["cardid"].ToString() + "'";
                    string strSQL = "insert into pub.CARDSR2 (EMP_CD,CardID,SwipeCardTime,UPDATE_TIME)  VALUES  (" + sa + ","
                                                                                                                   + sb + ",'"
                                                                                                                   + Convert.ToDateTime(dr["swipecardtime"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + "','"
                                                                                                                   + Convert.ToDateTime(dr["update_time"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + "')";

                    cmd.CommandText = strSQL;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        ++InsertSumk;
                    }
                    catch (System.Exception ex)
                    {
                        string s = ex.Message.Trim().Substring(ex.Message.Trim().Length - 6, 5);
                        if (s != "16949")
                        {
                            WriteLog("-->OracleRawInsertError " + ex.Message + " " + strSQL + '\r', 2);
                            // strInsertError += "OracleRawInsertError" + ex.Message + '\r';
                        }
                        continue;
                    }
                   
                }
                tx.Commit();
                //   result = "OK: OracleRawToProgress";
                WriteLog("-->OracleRawToProgressOK,共insert进CARDSR2中"+InsertSumk+"笔原始刷卡記錄" + '\r', 1);
            }
            catch (System.Exception ex)
            {
                WriteLog("-->OracleRawToProgressErrorRollback" + ex.Message + '\r', 2);
                //   result = "NG: OracleRawToProgressErrorRollback" + ex.Message;
                tx.Rollback();
            }
            finally
            {
                tx.Dispose();
                odbcCon.Close();

            }
            #endregion

            // return result;



        }


        private void Oracle_testswipecardtimeTOprogress_CARDSR()
        {
            //            string sql = @"SELECT cardtime.ID , cardtime.RecordID, cardtime.CardID, cardtime.name, cardtime.SwipeCardTime, cardtime.SwipeCardTime2, cardtime.CheckState, cardtime.PROD_LINE_CODE, cardtime.WorkshopNo, cardtime.PRIMARY_ITEM_NO, cardtime.RC_NO, cardtime.Shift, cardtime.OvertimeState, cardtime.overtimeType, cardtime.overtimeCal 
            //              FROM  swipecard.testswipecardtime cardtime
            //             WHERE   (   (DATE_FORMAT(cardtime.swipecardtime, '%Y%m%d') =
            //                               DATE_SUB(CURDATE(), INTERVAL 1 DAY))
            //                        OR (    DATE_FORMAT(cardtime.swipecardtime2, '%Y%m%d') =
            //                                   DATE_SUB(CURDATE(), INTERVAL 1 DAY)
            //                            AND cardtime.SwipeCardTime IS NULL)
            //                        OR (    DATE_FORMAT(cardtime.swipecardtime2, '%Y%m%d') = CURDATE()
            //                            AND shift = 'N'))";
            string sql = @"SELECT  T.EMP_ID,
         T.SWIPECARDTIME,
         T.SWIPECARDTIME2,
         T.CHECKSTATE,
         T.PROD_LINE_CODE,
         T.WORKSHOPNO,
         T.PRIMARY_ITEM_NO,
         T.RC_NO,
         T.SHIFT
  FROM SWIPE.CSR_SWIPECARDTIME T WHERE T.SWIPE_DATE=TO_CHAR(SYSDATE-2,'YYYY-MM-DD')";

            OracleHelp OraclelHelp = new OracleHelp();
            ProgressHelp ProHelp = new ProgressHelp();
            string result = "NG";
            #region CSBG

            //string sql = "select emp.ID,cardtime.* from swipecard.testemployee emp, swipecard.testswipecardtime cardtime where emp.cardid = cardtime.CardID and cardtime.SwipeCardTime>='2017/8/15 00:00' and  cardtime.SwipeCardTime<'2017/8/16 00:00'";
            //string sql = "select emp.ID,cardtime.* from swipecard.testemployee emp, swipecard.testswipecardtime cardtime where emp.cardid = cardtime.CardID and ((DATE_FORMAT(swipecardtime, '%Y%m%d')=curdate()-1) or (DATE_FORMAT(swipecardtime2, '%Y%m%d')=curdate()-1 and cardtime.Shift='N' and cardtime.SwipeCardTime is null))";
            DataTable DT2 = OraclelHelp.OrcaleQuery(sql);
            if (DT2 == null)
            {
                WriteLog("-->Query OracleCardTimeError" + '\r', 2);
                return;
            }//result = "NG: Query OracleCardTimeError"; }
            else if (DT2.Rows.Count == 0)
            {
                WriteLog("-->Query OracleCardTimeSum:0" + '\r', 2);
                return;
            }//result = "NG: Query OracleCardTimeSum:0"; }

            //DT2.PrimaryKey = new DataColumn[] { DT2.Columns["RecordID"] };//设置RecordID为主键 .
            WriteLog("-->Query OracleCardTimeSum:" + DT2.Rows.Count.ToString() + '\r', 1);

            //DataTable DT3 = ProHelp.QueryProgress(sql2);
            //if (DT3 == null)
            //{
            //    WriteLog("-->Query ProgressCardsrError" + '\r', 2);
            //    return;
            //}
            //DT3.PrimaryKey = new DataColumn[] { DT3.Columns["RecordID"] };//设置RecordID为主键 

            //WriteLog("-->Query ProgressEmpClassSum:" + DT3.Rows.Count.ToString() + '\r', 1);

            string ControlString1 = "DSN=KSHR;UID=csruser;PWD=csruser";
            OdbcConnection odbcCon = new OdbcConnection(ControlString1);
            odbcCon.Open();
            OdbcTransaction tx = odbcCon.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = odbcCon;
            cmd.Transaction = tx;
            try
            {

                string sa = "";
                string sb = "";
                string ss = "";
                int InsertSumk = 0;
                foreach (DataRow dr in DT2.Rows)
                {
                    //DataRow Progressdr = DT3.Rows.Find(dr["RecordID"].ToString());
                    //if (Progressdr == null)
                    //{
                    //ss = dr[0].Equals(DBNull.Value) ? "NULL" : dr[0].ToString();
                    sa = dr[1].Equals(DBNull.Value) ? "NULL" : "'" + Convert.ToDateTime(dr[1].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                    sb = dr[2].Equals(DBNull.Value) ? "NULL" : "'" + Convert.ToDateTime(dr[2].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                    string strSQL = "insert into pub.CARDSR (EMP_CD,swipecardtime,swipecardtime2,checkstate,prod_line_code,workshopno,primary_item_no,rc_no,shift)  VALUES  ('"

                                                                         + dr[0].ToString() + "',"

                                                                         + sa + ","
                                                                         + sb + ",'"
                                                                         + dr[3].ToString() + "','"
                                                                         + dr[4].ToString() + "','"
                                                                         + dr[5].ToString() + "','"
                                                                         + dr[6].ToString() + "','"
                                                                         + dr[7].ToString() + "','"

                                                                         + dr[8].ToString() + "')";

                    cmd.CommandText = strSQL;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        ++InsertSumk;
                    }
                    catch (System.Exception ex)
                    {
                        WriteLog("-->OracleCardTimeInsertError " + strSQL + '\r', 2);
                        // strInsertError += "OracleCardTimeInsertError " + ex.Message + '\r';
                        continue;
                    }
                    //}

                }


                tx.Commit();
                // result = "OK: OracleCardTimeToProgress";
                WriteLog("-->OracleCardTimeToProgressOK,共insert进CARDSR中"+InsertSumk+"笔刷卡記錄" + '\r', 1);



            }
            catch (System.Exception ex)
            {
                WriteLog("-->OracleCardTimeToProgressErrorRollback" + ex.Message + '\r', 2);
                //  result = "NG: OracleCardTimeToProgressErrorRollback" + ex.Message;
                tx.Rollback();
            }
            finally
            {
                tx.Dispose();
                odbcCon.Close();

            }
            // return result;
            #endregion
        }
  

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
    
}

