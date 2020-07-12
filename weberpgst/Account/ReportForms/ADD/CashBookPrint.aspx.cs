using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data.Sql;

public partial class RoportForms_ADD_CashBookPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            int P_Code = Convert.ToInt32(Request.QueryString[1].ToString());

            GenerateReport(Title, P_Code);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, int P_Code)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@P_Code", P_Code);
            DataTable dtCashBookPrint = CommonClasses.Execute("SELECT CSE.C_CODE,CSE.C_DATE,CSE.C_DEPOSIT,CSE.C_WITHDRAWAL,CSE.C_REMARK,isnull(CSE.C_ON_ACC,'') as C_ON_ACC,isnull(CSE.C_CHEQUE_NO,'') as C_CHEQUE_NO, AB.L_NAME AS [BANK_CASH_NAME],AL.L_NAME AS [LEDGER_NAME] FROM CASH_BOOK_ENTRY CSE INNER JOIN ACCOUNT_LEDGER AB on CSE.C_NAME=AB.L_CODE inner join ACCOUNT_LEDGER AL on CSE.C_L_CODE=AL.L_CODE WHERE CSE.ES_DELETE=0 AND AL.ES_DELETE=0 AND C_CODE='" + P_Code + "'");
            // ds = DL_DBAccess.SelectDataDataset("Reciept_Report", par, "dtPaymentAdvice");

            #region Print
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Account/Reports/CashBookPrint.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/CashBookPrint.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dtCashBookPrint);

            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtAddress", Session["CompanyAdd1"].ToString());
            //rptname.SetParameterValue("txtCompanyPhone", Session["CompanyPhone"].ToString());
            //rptname.SetParameterValue("txtCompanyFax", Session["CompanyFax"].ToString());
            //rptname.SetParameterValue("txtCompanyEmail", Session["CompanyEmail"].ToString());
            CrystalReportViewer1.ReportSource = rptname;

            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = " No Record Found! ";
            //}
            #endregion

        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/View/ViewOutstandingReport.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion btnCancel_Click
}
