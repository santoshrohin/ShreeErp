using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class Account_ADD_CashBookRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string StrCond = Request.QueryString[3].ToString();

            string group = Request.QueryString[4].ToString();
            GenerateReport(Title, From, To, StrCond, group);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string StrCond, string group)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            // Query = "SELECT BPM_INV_NO,BPM_INV_DATE,BPM_BASIC_AMT,BPM_EXCIES_AMT,BPM_ECESS_AMT,BPM_HECESS_AMT,BPM_TAX_AMT,BPM_DISCOUNT_AMT,BPM_DISC_AMT,BPM_FREIGHT,BPM_PACKING_AMT,BPM_OCTRO_AMT,BPM_G_AMT,P_NAME FROM BILL_PASSING_MASTER, PARTY_MASTER WHERE BPM_P_CODE=P_CODE AND BILL_PASSING_MASTER.ES_DELETE=0";
            Query = " SELECT CASH_BOOK_ENTRY.C_CODE, CASH_BOOK_ENTRY.C_DATE, CASH_BOOK_ENTRY.C_NAME, ACCOUNT_LEDGER.L_NAME, CASH_BOOK_ENTRY.C_DEPOSIT, CASH_BOOK_ENTRY.C_WITHDRAWAL, CASH_BOOK_ENTRY.C_REMARK, CASH_BOOK_ENTRY.C_L_CODE, ACCOUNT_LEDGER_1.L_NAME AS LEDGER_NAME FROM CASH_BOOK_ENTRY INNER JOIN ACCOUNT_LEDGER ON CASH_BOOK_ENTRY.C_NAME = ACCOUNT_LEDGER.L_CODE INNER JOIN ACCOUNT_LEDGER AS ACCOUNT_LEDGER_1 ON CASH_BOOK_ENTRY.C_L_CODE = ACCOUNT_LEDGER_1.L_CODE WHERE  " + StrCond + " CASH_BOOK_ENTRY.ES_DELETE=0 ";  //AND C_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);

            #region Crystal_Report
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                string head = "";
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCashBookRegisterDate.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCashBookRegisterDate.rpt");
                    head = "Cash Book Receipt Datewise";
                }
                else
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCashBookRegisterBook.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCashBookRegisterBook.rpt");
                    head = "Cash Book Receipt BookWise";
                }

                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                rptname.SetParameterValue("txtTitle", head);
                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion

        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Account/VIEW/ViewCashBookRegister.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
}