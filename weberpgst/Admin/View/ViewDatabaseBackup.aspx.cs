using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Win32.TaskScheduler;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_View_ViewDatabaseBackup : System.Web.UI.Page
{
    # region Variables   
    static int mlCode = 0;
    static string right = "";
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {

                       // string dbName = ConfigurationManager.ConnectionStrings["dbName"].ConnectionString;
                        //string dbUser = ConfigurationManager.ConnectionStrings["dbLogin"].ConnectionString;
                        //string dbPass = ConfigurationManager.ConnectionStrings["dbPassword"].ConnectionString;
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Database Backup", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region BackupDatabase
    public bool BackupDatabase()
    {
        bool flag = false;
        try
        {
            // store the file inside ~/App_Data/uploads folder
            var dbPath = Server.MapPath("~/App_Data/Utility/SimyaBackup.bat");
            var dir = Server.MapPath("~/App_Data/Utility/");
            var dirForBak = Server.MapPath("~/App_Data/Utility/7z.exe");
            var BackupPath = Server.MapPath("~/App_Data/Utility/Backup/");
            var BackupPathForsql = Server.MapPath("~/App_Data/Utility/Backup/");
            string name = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
            FileInfo file = new FileInfo(dir + "SimyaBackup.bat");
            FileInfo file1 = new FileInfo(dir + "SimyaBackup.sql");
            if (file.Exists)
            {
                file.Delete();
            }
            //if (file1.Exists)
            //{
            //    file1.Delete();
            //}

            string connectionString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ConnectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            string database = builder.InitialCatalog;
            string dbPass = builder.Password;
            string dbUser=builder.UserID;
           
            //string dbName = ConfigurationManager.ConnectionStrings["dbName"].ConnectionString;
            //string dbUser = ConfigurationManager.ConnectionStrings["dbLogin"].ConnectionString;
            //string dbPass = ConfigurationManager.ConnectionStrings["dbPassword"].ConnectionString;
            
            //using (var batfile = new StreamWriter(dir + "\\SimyaBackup.bat", true))
            //{
            //   // batfile.WriteLine("sqlcmd -S SIMYAUSERKUNKULE \\SM_CNTRCT -U sa -P abcd1234 -i SimyaBackup.sql");
            //    batfile.WriteLine("sqlcmd -S " + name + "\\" + database + " -U " + dbUser + " -P " + dbPass + " -i SimyaBackup.sql");
            //    //batfile.WriteLine("sqlcmd -S SIMYAUSERKUNKULE \\SM_CNTRCT -U " + dbUser + " -P " + dbPass + " -i SimyaBackup.sql");
            //    batfile.WriteLine("\"" + dirForBak + "\" a \"" + BackupPath + "\\SimyaDB_%date%_%time:~0,2%-%time:~3,2%_.zip\"" + " \""+BackupPath + "\\db_backup_*.bak\"");
            //    batfile.WriteLine("del " + "\""+BackupPath + "\\db_backup_*.bak\"");
            //}
            string BackupScript = "DECLARE  @pathName NVARCHAR(512) SET @pathName = '" + BackupPathForsql + "\\db_backup_' + Convert(varchar(8), GETDATE(), 112) + '.bak' BACKUP DATABASE [" + database + "] TO  DISK = @pathName WITH NOFORMAT, NOINIT, NAME = N'db_backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            using (var sqlfile = new StreamWriter(dir + "\\SimyaBackup.sql", true))
            {
                sqlfile.WriteLine("DECLARE  @pathName NVARCHAR(512) SET @pathName = '" + BackupPathForsql + "\\db_backup_' + Convert(varchar(8), GETDATE(), 112) + '.bak' BACKUP DATABASE [" + database + "] TO  DISK = @pathName WITH NOFORMAT, NOINIT, NAME = N'db_backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10");
            }
            if (CommonClasses.Execute1(BackupScript))
            {
                flag = true;
            }
            //TaskService ts = new TaskService();
            //TimeTrigger tt = new TimeTrigger();
            //tt.StartBoundary = DateTime.Now;
            //tt.Enabled = true;
            //tt.Repetition.Duration = TimeSpan.MaxValue;
           // tt.Repetition.Duration = TimeSpan.FromHours(Convert.ToDouble(txtTimeDuration.Text));
            //tt.Repetition.Duration = TimeSpan.FromHours(0.017);
           // tt.Repetition.Interval = TimeSpan.FromMinutes(Min);
           // tt.Repetition.Interval = TimeSpan.FromMinutes(300);
            //tt.Repetition. = TimeSpan.FromMinutes(1);
            //TaskDefinition td = ts.NewTask();
            //td.Triggers.Add(tt);
            //td.Actions.Add(new ExecAction(dbPath, null, dir));
           // string taskName = "backup16082014"+DateTime.Now.Millisecond;
            //string taskName = "JKCCBackup1263";

           // ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.Create, null, null, TaskLogonType.InteractiveToken, null);
           
           
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "BackupDatabase", ex.Message.ToString());
        }
        return flag;
    }

    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (BackupDatabase())
        {
            //txtTimeDuration.Text = "0.00";
            ShowMessage("#Avisos", "Backup Processed", CommonClasses.MSG_Warning);
            return;
        }
        else
        {
            ShowMessage("#Avisos", "Backup Not Processed", CommonClasses.MSG_Warning);
            return;
        }

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Database Backup", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Database Backup", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    //public void BackupDatabase1()
    //{
    //    try
    //    {
    //        // store the file inside ~/App_Data/uploads folder
    //        var dbPath = Server.MapPath("~/App_Data/Utility/SimyaBackup.bat");
    //        var dir = Server.MapPath("~/App_Data/Utility/");
    //        var dirForBak = Server.MapPath("~/App_Data/Utility/7z.exe");
    //        var BackupPath = Server.MapPath("~/App_Data/Utility/Backup/");
    //        var BackupPathForsql = Server.MapPath("~/App_Data/Utility/Backup/");
    //        string name = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();
    //        FileInfo file = new FileInfo(dir + "SimyaBackup.bat");
    //        FileInfo file1 = new FileInfo(dir + "SimyaBackup.sql");
    //        if (file.Exists)
    //        {
    //            file.Delete();
    //        }
    //        if (file1.Exists)
    //        {
    //            file1.Delete();
    //        }


    //        using (var batfile = new StreamWriter(dir + "\\SimyaBackup.bat", true))
    //        {
    //            batfile.WriteLine("sqlcmd -S " + name + "\\SM_CNTRCT -U sa -P abcd1234 -i SimyaBackup.sql");
    //            batfile.WriteLine("" + dirForBak + " a " + BackupPath + "\\SimyaDB_%date%_%time:~0,2%-%time:~3,2%_.zip " + BackupPath + "\\db_backup_*.bak");
    //            batfile.WriteLine("del " + BackupPath + "\\db_backup_*.bak");
    //        }

    //        using (var sqlfile = new StreamWriter(dir + "\\SimyaBackup.sql", true))
    //        {
    //            sqlfile.WriteLine("DECLARE  @pathName NVARCHAR(512) SET @pathName = '" + BackupPathForsql + "\\db_backup_' + Convert(varchar(8), GETDATE(), 112) + '.bak' BACKUP DATABASE [SM_CNTRCT] TO  DISK = @pathName WITH NOFORMAT, NOINIT, NAME = N'db_backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10");
    //        }
    //        TaskService ts = new TaskService();
    //        TimeTrigger tt = new TimeTrigger();
    //        tt.StartBoundary = DateTime.Now;
    //        tt.Enabled = true;
    //        //tt.Repetition.Duration = TimeSpan.MaxValue;
    //        //tt.Repetition.Duration = TimeSpan.FromHours(24);
    //        tt.Repetition.Duration = TimeSpan.FromHours(Convert.ToDouble(txtTimeDuration.Text));
    //        //tt.Repetition.Interval = TimeSpan.FromMinutes(Min);
    //        //tt.Repetition.Interval = TimeSpan.FromMinutes(300);
    //        //tt.Repetition. = TimeSpan.FromMinutes(1);
    //        TaskDefinition td = ts.NewTask();
    //        td.Triggers.Add(tt);
    //        td.Actions.Add(new ExecAction(dbPath, null, dir));
    //        string taskName = "dbbackuputil";
    //        ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.Create, null, null, TaskLogonType.InteractiveToken, null);
    //    }
    //    catch (Exception ex)
    //    {           

    //    }
       
    //}
}
