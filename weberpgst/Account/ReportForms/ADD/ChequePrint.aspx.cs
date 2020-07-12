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

public partial class RoportForms_ADD_ChequePrint : System.Web.UI.Page
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
            int paym_code = Convert.ToInt32(Request.QueryString[1].ToString());

            GenerateReport(Title, paym_code);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, int paym_code)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@paym_code", paym_code);
            DataTable dtCashBookPrint = CommonClasses.Execute("select PAYM_CODE,PAYM_NO,PAYM_DATE,PAYM_P_CODE,P_NAME,PAYM_CHEQUE_NO,PAYM_CHEQUE_DATE,PAYM_AMOUNT from PAYMENT_MASTER  inner join PARTY_MASTER on PAYM_P_CODE=P_CODE WHERE PAYMENT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND PAYM_CODE ='" + paym_code + "'");
            // ds = DL_DBAccess.SelectDataDataset("Reciept_Report", par, "dtPaymentAdvice");

            #region Print
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            //CrystalDecisions.Shared.PageMargins PageMargins1 = new CrystalDecisions.Shared.PageMargins
            //{
            //    topMargin = 250,
            //    leftMargin = 250,
            //    rightMargin = 250,
            //    bottomMargin = 250
            //};

            //rptname.PrintOptions.ApplyPageMargins(PageMargins1);


            rptname.Load(Server.MapPath("~/Account/Reports/ChequePrint.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/ChequePrint.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dtCashBookPrint);

            //rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            //rptname.SetParameterValue("txtAddress", Session["CompanyAdd1"].ToString());
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
