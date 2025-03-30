using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;

public partial class Utility_VIEW_ViewDatabaseBackup : System.Web.UI.Page
{
    private string filePrefix = "DB_A2EA4B_QUANTIC";
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if any backup message is set
        if (!string.IsNullOrEmpty(Session["BackupStatus"] as string))
        {
            lblBackupStatus.Text = Session["BackupStatus"].ToString();
            PanelMsg.Visible = true;
            Session["BackupStatus"] = null; // Clear the session message
        }
    }

    protected void btnBackup_Click(object sender, EventArgs e)
    {
        try
        {
            // Get the FTP server details (replace with actual FTP server info)
            //string ftpServerUrl = "win8167.site4now.net"; // Replace with your FTP server URL
            string ftpServerUrl = "ftp://win8167.site4now.net";
            string ftpUsername = "santoshrohini-001"; // Replace with your FTP username
            string ftpPassword = "Navnath$1704"; // Replace with your FTP password

            // Build the file name based on today's date (format: DB_A2EA4B_QUANTIC_{month}_{day}_{year}.bak)
            string currentDate = DateTime.Now.ToString("M_d_yyyy"); // Example: 3_6_2025
            
            string remoteFileName = string.Format("{0}_{1}.bak", filePrefix, currentDate); // Construct the file name using the variable


            // Full FTP URL to the file
            string ftpFilePath = string.Format("{0}/DB/{1}", ftpServerUrl, remoteFileName);

            // Local file path to save the downloaded backup
            string localFolderPath = Server.MapPath("~/UpLoadPath/DB/");
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath); // Create the DB folder if it doesn't exist
            }

            string localFilePath = Path.Combine(localFolderPath, remoteFileName);

            // Download the file from FTP server
            DownloadFileFromFtp(ftpFilePath, localFilePath, ftpUsername, ftpPassword);

            // Store a success message in session
            Session["BackupStatus"] = string.Format("Database backup successful. File saved at: {0}", localFilePath);
            lblBackupStatus.Text = "Database backup successful!";
            PanelMsg.Visible = true;
        }
        catch (Exception ex)
        {
            // Handle the error and show a failure message
            Session["BackupStatus"] = string.Format("Database backup failed: {0}", ex.Message);
            lblBackupStatus.Text = "Backup failed!";
            PanelMsg.Visible = true;
            DownloadFileToLocalMachine();
        }
    }
    private void DownloadFileToLocalMachine()
    {
        // Specify the file path relative to your application directory
        string currentDate = DateTime.Now.ToString("M_d_yyyy");
        string remoteFileName = string.Format("{0}_{1}.bak", filePrefix, currentDate); // File name for today

        string serverRelativeFilePath = "~/UpLoadPath/DB/" + remoteFileName;

        // Convert the relative path to a physical file path on the server
        string localFilePath = Server.MapPath(serverRelativeFilePath);  // This gives the physical file path on the server

        if (File.Exists(localFilePath))
        {
            // Clear the response to prepare for sending the file
            Response.Clear();

            // Set the correct content type for a file download
            Response.ContentType = "application/octet-stream";

            // Set the content disposition to attachment to force download
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(localFilePath));

            // Send the file to the client
            Response.TransmitFile(localFilePath);

            // End the response to complete the download
            Response.End();
        }
        else
        {
            // If the file doesn't exist, show an error message
            lblBackupStatus.Text = "Backup file not found.";
            PanelMsg.Visible = true;
        }
    }

    private void DownloadFileFromFtp(string ftpFilePath, string localFilePath, string ftpUsername, string ftpPassword)
    {
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpFilePath);
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        ftpRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = false;

        using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
        using (Stream ftpStream = ftpResponse.GetResponseStream())
        using (FileStream localFileStream = new FileStream(localFilePath, FileMode.Create))
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                localFileStream.Write(buffer, 0, bytesRead);
            }
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string currentDate = DateTime.Now.ToString("M_d_yyyy");
        string remoteFileName = string.Format("{0}_{1}.bak", filePrefix, currentDate); // File name for today


        string localFolderPath = Server.MapPath("~/UpLoadPath/DB/");
        string localFilePath = Path.Combine(localFolderPath, remoteFileName);

        if (File.Exists(localFilePath))
        {
            // Trigger file download
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(localFilePath));
            Response.TransmitFile(localFilePath);
            Response.End();
        }
        else
        {
            lblBackupStatus.Text = "Backup file not found.";
            PanelMsg.Visible = true;
        }
    }
}
