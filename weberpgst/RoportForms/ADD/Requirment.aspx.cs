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

public partial class RoportForms_ADD_Requirment : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    DataRow dr;
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string Cond = Request.QueryString[2].ToString();
            GenerateReport(Title, From, Cond);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Cond)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
     

        DateTime dtdate = new DateTime();
        dtdate = Convert.ToDateTime(From);
        DataTable dtResult = new DataTable();
        //Query = "select I_CODENO,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
        //Hiten Stock Query Added 
        //Query = "select ITEM_MASTER.I_CODENO,ISNULL( STL.STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL.STL_DOC_DATE) AS STL_DOC_DATE,STL.STL_CODE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,  (CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT'    WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD'  WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD'  WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.'  WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'PRODTOSTORECONSUME' THEN 'PRODUCTION CONSUMTION ' ELSE STL_DOC_TYPE  END) AS STL_DOC_TYPE,ITEM_MASTER.I_CODE AS STL_I_CODE,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc FROM STOCK_LEDGER AS STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE WHERE  (ITEM_MASTER.I_ACTIVE_IND = 1)";
        DataTable dtSchedule = new DataTable();
        #region datatable structure
        if (dtResult.Columns.Count == 0)
        {
            dtResult.Columns.Add("RawICode");
            dtResult.Columns.Add("I_CODENO");
            dtResult.Columns.Add("I_NAME");
            dtResult.Columns.Add("ReqQty");
            dtResult.Columns.Add("AMT");
        }
        #endregion

        dtSchedule = CommonClasses.Execute("SELECT CSD_I_CODE,SUM(CSD_QTY) AS CSD_QTY  FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL where "+Cond+" CUSTOMER_SCHEDULE.CS_CODE=CSD_CS_CODE AND CUSTOMER_SCHEDULE.ES_DELETE=0      GROUP BY CSD_I_CODE");
        for (int i = 0; i < dtSchedule.Rows.Count; i++)
        {

            DataTable dt = new DataTable();
            SqlParameter[] Params = 
			    { 
				    new SqlParameter("@FGICODE",dtSchedule.Rows[i]["CSD_I_CODE"].ToString()),
				    new SqlParameter("@FGQTY",dtSchedule.Rows[i]["CSD_QTY"].ToString()),
                    new SqlParameter("@OPDate",dtdate.ToString("01/MMM/yyyy"))
			    };
            dt = DL_DBAccess.SelectData("REQUIREMENT", Params);
            if (dt.Rows.Count > 0)
            {
                dtResult.Rows.Add(dt.Rows[0]["RawICode"].ToString(), dt.Rows[0]["I_CODENO"].ToString(),   dt.Rows[0]["I_NAME"].ToString(), dt.Rows[0]["ReqQty"].ToString(), dt.Rows[0]["AMT"].ToString());
            }
        }

        if (dtResult.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/Requirment.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/Requirment.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtResult);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", " Purchase Requirment For the Month Of " + Convert.ToDateTime(From).ToString("MMM/yyyy") );
            CrystalReportViewer1.ReportSource = rptname;

        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
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
            CommonClasses.SendError("Purchase Requirment", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewRequirment.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requirment", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}