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
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
public partial class RoportForms_ADD_CreditNotePrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Credit_Code = "";
    string reportType = "";
    string toNo = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        Credit_Code = Request.QueryString[0];
        string Title = Request.QueryString[1];
        //Cond = Request.QueryString[1];
        GenerateReport(Credit_Code);
    }
    #endregion Page_Init

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtCreditNote = new DataTable();
        DataTable dtTemp = new DataTable();
        DataSet dsCreditNote = new DataSet();
        ReportDocument rptname = null;
        rptname = new ReportDocument();
        try
        {
            dtCreditNote = CommonClasses.Execute("SELECT DISTINCT(I_CODE),CNM_SERIAL_NO,ISNULL(CNM_GSTIN_NO,'') as CNM_GSTIN_NO,ISNULL(CNM_EWAY_BILL_NO,'') as CNM_EWAY_BILL_NO,CONVERT(VARCHAR,CNM_DATE,103) AS CNM_DATE ,CNM_CUST_ADDRESS,CNM_CUST_STATE_NAME,CNM_CUST_STATE_CODE, CAST(ISNULL(CNM_GRAND_TOTAL,0) AS NUMERIC(20,2)) AS CNM_GRAND_TOTAL,CAST(ISNULL(CNM_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS CNM_CENTRAL_TAX_AMT, CAST(ISNULL(CNM_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS CNM_STATE_UNION_TAX_AMT,CAST(ISNULL(CNM_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS CNM_INTEGRATED_TAX_AMT, CAST(ISNULL(CNM_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS CNM_CESS_TAX_AMT,CAST(ISNULL(CNM_NET_AMOUNT,0) AS NUMERIC(20,2)) AS CNM_NET_AMOUNT, CAST(ISNULL(CND_QTY,0) AS NUMERIC (20,2)) AS CND_QTY, CAST(ISNULL(CND_RATE,0) AS NUMERIC(20,2)) AS CND_RATE,CAST(ISNULL(CND_AMOUNT,0) AS NUMERIC(20,2)) AS CND_AMOUNT,	CAST(ISNULL(CND_CENTRAL_TAX,0) AS NUMERIC(20,2)) AS CND_CENTRAL_TAX, CAST(ISNULL(CND_STATE_UNION_TAX,0) AS NUMERIC(20,2)) AS CND_STATE_UNION_TAX, CAST(ISNULL(CND_INTEGRATED_TAX,0) AS NUMERIC(20,2)) AS CND_INTEGRATED_TAX,CAST(ISNULL(CND_CESS_TAX,0) AS NUMERIC(20,2)) AS CND_CESS_TAX,CAST(ISNULL(CND_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS CND_CENTRAL_TAX_AMT, CAST(ISNULL(CND_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS CND_STATE_UNION_TAX_AMT,	CAST(ISNULL(CND_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS CND_INTEGRATED_TAX_AMT, 	CAST(ISNULL(CND_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS CND_CESS_TAX_AMT, P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,ISNULL(P_LBT_NO,'') AS P_GST_NO ,P_PIN_CODE,P_PARTY_CODE,0 as E_TARIFF_NO ,SM_NAME,SM_STATE_CODE ,I_CODENO,I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME FROM CREDIT_NOTE_MASTER,CREDIT_NOTE_DETAILS,PARTY_MASTER ,ITEM_MASTER,ITEM_UNIT_MASTER,STATE_MASTER WHERE  CREDIT_NOTE_MASTER.CNM_CODE=CREDIT_NOTE_DETAILS.CND_CNM_CODE  AND CREDIT_NOTE_MASTER.CNM_CUST_CODE=PARTY_MASTER.P_CODE AND ITEM_MASTER.I_CODE=CREDIT_NOTE_DETAILS.CND_ITEM_CODE AND   ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER.SM_CODE=P_SM_CODE AND CND_CNM_CODE='" + Credit_Code + "' ");

            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");
            if (dtComp.Rows.Count > 0)
            {
                if (dtCreditNote.Rows.Count > 0)
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCreditNote.rpt"));   
                    rptname.FileName = Server.MapPath("~/Reports/rptCreditNote.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtCreditNote);
                    
                    rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    rptname.SetParameterValue("txtCompGSTIN", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("CreditNote", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewCreditNote.aspx", false);
    }
    #endregion
}
