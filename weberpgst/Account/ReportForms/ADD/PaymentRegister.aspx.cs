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

public partial class RoportForms_ADD_PaymentRegister : System.Web.UI.Page
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
            dtReceiptRegister = CommonClasses.Execute("select PAYM_CODE,P_CODE,P_NAME,isnull(PAYMD_AMOUNT,0) as PAYMD_AMOUNT, isnull(PAYMD_ADJ_AMOUNT,0) as PAYMD_ADJ_AMOUNT ,isnull(PAYM_AMOUNT,0) as  PAYM_AMOUNT 	 ,INM_DATE,INM_NO,PAYM_REMARK,PAYM_NO,PAYM_DATE from PAYMENT_MASTER PAYM INNER JOIN PAYMENT_DETAIL PAYMD ON PAYM.PAYM_CODE=PAYMD.PAYMD_PAYM_CODE INNER JOIN PARTY_MASTER P ON PAYM.PAYM_P_CODE=P.P_CODE inner join INVOICE_MASTER on P.P_CODE=INM_P_CODE AND INM_CODE=PAYMD.PAYMD_INVOICE_CODE where " + StrCondition + " PAYM.ES_DELETE=0 AND PAYMD.ES_DELETE=0 AND P.ES_DELETE=0");
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            #region Payment_Register_Comment
            //SqlParameter[] par = new SqlParameter[5];
            //par[0] = new SqlParameter("@FromDate", FromDate);
            //par[1] = new SqlParameter("@ToDate", ToDate);
            //par[2] = new SqlParameter("@Party_code", "");
            //par[3] = new SqlParameter("@StrCondition", StrCondition);
            //par[4] = new SqlParameter("@All_CUST", All_CUST);
            //ds = DL_DBAccess.SelectDataDataset("Payment_Register", par, "dtReceiptRegister");
            #endregion Payment_Register_Comment

            #region CrystalReport
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Account/Reports/PaymentRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/PaymentRegister.rpt");

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
