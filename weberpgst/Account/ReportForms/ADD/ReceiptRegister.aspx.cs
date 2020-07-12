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

public partial class RoportForms_ADD_ReceiptRegister : System.Web.UI.Page
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
            bool ALL_DATE = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool All_CUST = Convert.ToBoolean(Request.QueryString[2].ToString());

            string FromDate = Request.QueryString[3].ToString();
            string ToDate = Request.QueryString[4].ToString();
            int p_name = Convert.ToInt32(Request.QueryString[5].ToString());
            string StrCondition = Request.QueryString[6].ToString();
            GenerateReport(Title, ALL_DATE, All_CUST, FromDate, ToDate, p_name, StrCondition);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, bool ALL_DATE, bool All_CUST, string FromDate, string ToDate, int p_name, string StrCondition)
    {
        try
        {

            DataTable dtReceiptRegister = new DataTable();
            dtReceiptRegister = CommonClasses.Execute("select RCP_CODE,P_CODE,P_NAME,isnull(RCPD_AMOUNT,0) as RCPD_AMOUNT, isnull(RCPD_ADJ_AMOUNT,0) as RCPD_ADJ_AMOUNT ,isnull(RCP_AMOUNT,0) as  RCP_AMOUNT,INM_DATE,INM_NO,RCP_REMARK,RCP_NO,RCP_DATE from RECIEPT_MASTER RCP INNER JOIN RECIEPT_DETAIL RCPD ON RCP.RCP_CODE=RCPD.RCPD_RCP_CODE INNER JOIN PARTY_MASTER P ON RCP.RCP_P_CODE=P.P_CODE inner join INVOICE_MASTER on P.P_CODE=INM_P_CODE AND INM_CODE=RCPD.RCPD_INVOICE_CODE where " + StrCondition + " RCP.ES_DELETE=0 AND RCPD.ES_DELETE=0 AND P.ES_DELETE=0");
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            #region Receipt_Register_Comment
            //SqlParameter[] par = new SqlParameter[5];
            //par[0] = new SqlParameter("@FromDate", FromDate);
            //par[1] = new SqlParameter("@ToDate", ToDate);
            //par[2] = new SqlParameter("@Party_code", "");
            //par[3] = new SqlParameter("@StrCondition", StrCondition);
            //par[4] = new SqlParameter("@All_CUST", All_CUST);
            //ds = DL_DBAccess.SelectDataDataset("Receipt_Register", par, "dtReceiptRegister");
            #endregion Receipt_Register_Comment

            #region CrystalReport
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Account/Reports/ReceiptRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/ReceiptRegister.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dtReceiptRegister);

            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtDate", " FromDate " + FromDate + " ToDate " + ToDate);
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
