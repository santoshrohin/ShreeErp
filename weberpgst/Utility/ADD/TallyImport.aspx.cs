using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Text;
using System.IO;


public partial class Account_ReportForms_VIEW_TallyImport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    public void TruncateTallyAccountTable()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataSet ds = new DataSet();
        ds = null;



        SqlParameter[] par = new SqlParameter[1];
        par[0] = new SqlParameter("@date", 1);



        DL_DBAccess.Insertion_Updation_Delete("TruncateTallyAccountTable", par);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            myLabel.Text = "Import Started......";
            lblmsz.Text = "Import Started......";
        }
        catch (Exception ex)
        {
            
            
        }
        //importResller();
        

    }
    public void importResller()
    {
        string SaveLocation = Server.MapPath("~/UpLoadPath/Accounts/Book1.xlsx");
        // Create the connection object

        System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
                            "Provider=Microsoft.ACE.OLEDB.12.0; " +
                             "data source='" + SaveLocation + "';" +
                                "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");

        try
        {


            List<string> CountryList = new List<string>();
            

            // Open connection
            myConnection.Open();

            // Create OleDbCommand object and select data from worksheet Sheet1
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Bills Receivable$]", myConnection);

            // Create new OleDbDataAdapter
            OleDbDataAdapter oleda = new OleDbDataAdapter();

            oleda.SelectCommand = cmd;

            // Create a DataSet which will hold the data extracted from the worksheet.
            DataSet ds = new DataSet();

            // Fill the DataSet from the data extracted from the worksheet.
            oleda.Fill(ds, "Outstanding");
            DataTable dt = ds.Tables["Outstanding"];
            //Austria
            DataTable selectedTable = dt.AsEnumerable()
                            .Where(r => r.Field<string>("F11") == "Austria")
                            .CopyToDataTable();
            var distinctValues = selectedTable.AsEnumerable()
                        .Select(row => new
                        {
                            CountryList = row.Field<string>("F11"),
                            Types = row.Field<string>("F2"),
                            City = row.Field<string>("F9"),
                            State = row.Field<string>("F10"),
                            Certificates = row.Field<string>("F19"),
                            
                            PartnerType = row.Field<string>("F20")
                        })
                        .Distinct();

            foreach (var item in distinctValues)
            {
                
                CountryList.Add(item.CountryList);

            }
            
            




        }
        catch (Exception ex)
        {
            
            throw;
        }

    }
    public void imporSuupliertexcel()
    {




        string SaveLocation = Server.MapPath("~/UpLoadPath/Accounts/supout.xlsx");
        // Create the connection object

        System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
                            "Provider=Microsoft.ACE.OLEDB.12.0; " +
                             "data source='" + SaveLocation + "';" +
                                "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");

        //OleDbConnection oledbConn = new OleDbConnection(myConnection);
        try
        {
            // Open connection
            myConnection.Open();

            // Create OleDbCommand object and select data from worksheet Sheet1
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Bills Payable$]", myConnection);

            // Create new OleDbDataAdapter
            OleDbDataAdapter oleda = new OleDbDataAdapter();

            oleda.SelectCommand = cmd;

            // Create a DataSet which will hold the data extracted from the worksheet.
            DataSet ds = new DataSet();

            // Fill the DataSet from the data extracted from the worksheet.
            oleda.Fill(ds, "Outstanding");
            DataTable dt = ds.Tables["Outstanding"];
            string partyname = "";
            DateTime date;
            DateTime duedate;
            string refno;
            int Ovrduedays=0;
            float amt;
            foreach (DataRow dc in dt.Rows)
            {
                //if (dc[0].ToString() == "Date" && dc[1].ToString() == "Ref. No." && dc[2].ToString() == "Party's Name" && dc[3].ToString() == "Pending" && dc[4].ToString() == "Due on" && dc[5].ToString() == "Overdue")
                ////if (dc[0].ToString() == "Date" )
                //{
                if (dc[0].ToString() == "" && dc[1].ToString() == "" && dc[2].ToString() != "" && dc[3].ToString() == "" && dc[4].ToString() == "" && dc[5].ToString() == "")
                {
                    partyname = dc[2].ToString();
                    

                }
                //if (dc[0].ToString() != "" && dc[1].ToString() != "" && dc[2].ToString() == "" && dc[3].ToString() != "" && dc[4].ToString() != "" && dc[5].ToString() != "")
                if (dc[0].ToString() != "" && dc[1].ToString() != "" && dc[2].ToString() == "" && dc[3].ToString() != "" && dc[4].ToString() != "")
                {
                    date = Convert.ToDateTime(dc[0].ToString());
                    refno = dc[1].ToString();
                    amt = float.Parse(dc[3].ToString());
                    duedate = Convert.ToDateTime(dc[4].ToString());
                    if (dc[5].ToString() !="")
                    {
                        Ovrduedays = Convert.ToInt32(dc[5].ToString());    
                    }
                    

                    insertintotable(date, refno, amt, duedate, Ovrduedays, partyname, "Supp");



                }


                //}

            }
            myLabel.Text = "Import Completed";
            lblmsz.Text = "Import Completed......";
            // Bind the data to the GridView
            //GridView1.DataSource = ds.Tables[0].DefaultView;
            //GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // Close connection
            myConnection.Close();
        }
    }


    public void imporCustomerexcel()
    {

        


        string SaveLocation = Server.MapPath("~/UpLoadPath/Accounts/custout.xlsx");
        // Create the connection object

        System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection(
                            "Provider=Microsoft.ACE.OLEDB.12.0; " +
                             "data source='" + SaveLocation + "';" +
                                "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\" ");

        //OleDbConnection oledbConn = new OleDbConnection(myConnection);
        try
        {
            // Open connection
            myConnection.Open();

            // Create OleDbCommand object and select data from worksheet Sheet1
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Bills Receivable$]", myConnection);

            // Create new OleDbDataAdapter
            OleDbDataAdapter oleda = new OleDbDataAdapter();

            oleda.SelectCommand = cmd;

            // Create a DataSet which will hold the data extracted from the worksheet.
            DataSet ds = new DataSet();

            // Fill the DataSet from the data extracted from the worksheet.
            oleda.Fill(ds, "Outstanding");
            DataTable dt = ds.Tables["Outstanding"];
            string partyname = "";
            DateTime date;
            DateTime duedate;
            string refno;
            int Ovrduedays=0;
            float amt;
            foreach (DataRow dc in dt.Rows)
            {
                //if (dc[0].ToString() == "Date" && dc[1].ToString() == "Ref. No." && dc[2].ToString() == "Party's Name" && dc[3].ToString() == "Pending" && dc[4].ToString() == "Due on" && dc[5].ToString() == "Overdue")
                ////if (dc[0].ToString() == "Date" )
                //{
                if (dc[0].ToString() == "" && dc[1].ToString() == "" && dc[2].ToString() != "" && dc[3].ToString() == "" && dc[4].ToString() == "" && dc[5].ToString() == "")
                {
                    partyname = dc[2].ToString();


                }
                if (dc[0].ToString() != "" && dc[1].ToString() != "" && dc[2].ToString() == "" && dc[3].ToString() != "" && dc[4].ToString() != "")
                {
                    date = Convert.ToDateTime(dc[0].ToString());
                    refno = dc[1].ToString();
                    amt = float.Parse(dc[3].ToString());
                    duedate = Convert.ToDateTime(dc[4].ToString());
                    if (dc[5].ToString() != "")
                    {
                        Ovrduedays = Convert.ToInt32(dc[5].ToString());
                    }

                    //Ovrduedays = Convert.ToInt32(dc[5].ToString());

                    insertintotable(date, refno, amt, duedate, Ovrduedays, partyname, "Cust");



                }


                //}

            }
            myLabel.Text = "Import Completed";
            lblmsz.Text = "Import Completed......";
            // Bind the data to the GridView
            //GridView1.DataSource = ds.Tables[0].DefaultView;
            //GridView1.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // Close connection
            myConnection.Close();
        }
    }



    public void insertintotable(DateTime date, string refno, float amt, DateTime duedate, int Ovrduedays, string partyname, string type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataSet ds = new DataSet();
        ds = null;



        SqlParameter[] par = new SqlParameter[7];
        par[0] = new SqlParameter("@date", date);
        par[1] = new SqlParameter("@refno", refno);
        par[2] = new SqlParameter("@amt", amt);
        par[3] = new SqlParameter("@duedate", duedate);
        par[4] = new SqlParameter("@Ovrduedays", Ovrduedays);
        par[5] = new SqlParameter("@partyname", partyname);
        par[6] = new SqlParameter("@Type", type);


        DL_DBAccess.Insertion_Updation_Delete("insertintotallyouttable", par);
    }

   

    public DataTable GetDataTableFromExcel(string path, bool hasHeader )
        {
            hasHeader = true;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    public void insertintoBanktable(DateTime importdate, string partyname, string vouchertype, string transactiontype, string InstrumentNumber, DateTime InstrumentDate, DateTime BankDate, float DebitAmount, float CreditAmount)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataSet ds = new DataSet();
        ds = null;

        string impodate =String.Format("{0:MM/dd/yyyy}", importdate);
        string InstrumentDat=String.Format("{0:MM/dd/yyyy}", InstrumentDate);
        string BankDat=String.Format("{0:MM/dd/yyyy}", BankDate) ;
        SqlParameter[] par = new SqlParameter[9];
        par[0] = new SqlParameter("@importdate", impodate);
        par[1] = new SqlParameter("@partyname", partyname);
        par[2] = new SqlParameter("@vouchertype", vouchertype);
        par[3] = new SqlParameter("@transactiontype", transactiontype);
        par[4] = new SqlParameter("@InstrumentNumber", InstrumentNumber);
        par[5] = new SqlParameter("@InstrumentDate",InstrumentDat );
        par[6] = new SqlParameter("@BankDate", BankDat);
        par[7] = new SqlParameter("@DebitAmount", DebitAmount);
        par[8] = new SqlParameter("@CreditAmount", CreditAmount);


        DL_DBAccess.Insertion_Updation_Delete("insertintobankdetails", par);
    }
    protected void btnBankImport_Click(object sender, EventArgs e)
    {
        string SaveLocation = Server.MapPath("~/UpLoadPath/Accounts/Bank1.xls");
        DataTable dt = new DataTable();
        dt = GetDataTableFromExcel(SaveLocation, true);

        
        int i = 0;
        foreach (DataRow dc in dt.Rows)
        {
            if (i>6)
            {
                if (dc[0].ToString() != "")
                {
                    insertintoBanktable(Convert.ToDateTime(dc[0].ToString() == "" ? null : dc[0].ToString()), dc[1].ToString(), dc[2].ToString(), dc[3].ToString(), dc[4].ToString(), Convert.ToDateTime(dc[5].ToString() == "" ? null : dc[5].ToString()), Convert.ToDateTime(dc[6].ToString() == "" ? null : dc[6].ToString()), float.Parse(dc[7].ToString() == "" ? "0" : dc[7].ToString()), float.Parse(dc[8].ToString() == "" ? "0" : dc[8].ToString()));    
                }
                
            }
            i = i + 1;
        }
        
    }
    protected void btnTransaction_Click(object sender, EventArgs e)
    {



        TruncateTallyAccountTable();
        imporSuupliertexcel();
        imporCustomerexcel();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Label1.Text = "File Uploading Started";
        StringBuilder sb = new StringBuilder();

        if (FileUpload1.HasFile)
        {
            try
            {
                string imageFolder = @"../UpLoadPath/Accounts" + "/" + FileUpload1.FileName;

                // upload path
                string imagePathString = @"UpLoadPath/Accounts" + "/" + FileUpload1.FileName;
                string s = Server.MapPath("");


                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Accounts/" + FileUpload1.FileName));
                sb.AppendFormat(" Uploading file: {0}", FileUpload1.FileName);



                //Showing the file information
                sb.AppendFormat("<br/> Save As: {0}", FileUpload1.PostedFile.FileName);
                sb.AppendFormat("<br/> File type: {0}", FileUpload1.PostedFile.ContentType);
                sb.AppendFormat("<br/> File length: {0}", FileUpload1.PostedFile.ContentLength);
                sb.AppendFormat("<br/> File name: {0}", FileUpload1.PostedFile.FileName);

                Label1.Text = "File Uploading Done  " + sb.ToString();
            }
            catch (Exception ex)
            {
                sb.Append("<br/> Error <br/>");
                sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
            }
        }
        else
        {
            lblmessage.Text = sb.ToString();
        }
    }

}
