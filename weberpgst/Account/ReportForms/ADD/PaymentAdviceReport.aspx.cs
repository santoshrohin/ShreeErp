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

public partial class RoportForms_ADD_PaymentAdviceReport : System.Web.UI.Page
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

            ds = DL_DBAccess.SelectDataDataset("Payment_Advice", par, "dtPaymentAdvice");

            #region Print
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Account/Reports/PaymentAdvice.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/PaymentAdvice.rpt");

            rptname.Refresh();
            rptname.SetDataSource(ds);

            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtAddress", Session["CompanyAdd1"].ToString());
            rptname.SetParameterValue("txtCompanyPhone", Session["CompanyPhone"].ToString());
            rptname.SetParameterValue("txtCompanyFax", Session["CompanyFax"].ToString());
            rptname.SetParameterValue("txtCompanyEmail", Session["CompanyEmail"].ToString());
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
