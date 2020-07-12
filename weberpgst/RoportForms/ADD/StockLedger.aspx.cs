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

public partial class RoportForms_ADD_StockLedger : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

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
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

            string i_name = Request.QueryString[5].ToString();
            bool ChkCatAll = Convert.ToBoolean(Request.QueryString[7].ToString());
            string category = Request.QueryString[8].ToString();
            string detail = Request.QueryString[6].ToString();
            if (ChkAll == true && ChkComp == true && ChkCatAll == true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail, category, "All");
            }
            if (ChkAll != true && ChkComp == true && ChkCatAll == true)
            {
                GenerateReport(Title, "ONE", "All", From, To, i_name, detail, category, "All");
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll == true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "All");
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "ONE");
            }
            if (ChkAll != true && ChkComp != true && ChkCatAll == true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail, category, "All");
            }
            if (ChkAll != true && ChkComp == true && ChkCatAll != true)
            {
                GenerateReport(Title, "ONE", "ALL", From, To, i_name, detail, category, "ONE");
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "ONE");
            }
            if (ChkAll == true && ChkComp == true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail, category, "ONE");
            }

            if (ChkAll != true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail, category, "ONE");
            }

        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name, string detail, string category, string chkCat)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        //DataTable dtMain = new DataTable();
        //dtMain.Columns.Add("EMCode");
        //dtMain.Columns.Add("EMName");
        //dtMain.Columns.Add("Date");
        //dtMain.Columns.Add("Particulars");
        //dtMain.Columns.Add("LoanGiven");
        //dtMain.Columns.Add("Installment");
        //dtMain.Columns.Add("Balance");
        //Query = "select I_CODENO,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
        //Hiten Stock Query Added 
        Query = "select ITEM_MASTER.I_CODENO,ISNULL( STL.STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL.STL_DOC_DATE) AS STL_DOC_DATE,STL.STL_CODE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,  (CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT'    WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD'  WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD'  WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.'  WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'PRODTOSTORECONSUME' THEN 'PRODUCTION CONSUMTION ' ELSE STL_DOC_TYPE  END) AS STL_DOC_TYPE,ITEM_MASTER.I_CODE AS STL_I_CODE,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc FROM STOCK_LEDGER AS STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE WHERE  (ITEM_MASTER.I_ACTIVE_IND = 1)";

        if (chkCat != "All")
        {
            Query = Query + " AND I_CAT_CODE='" + category + "'";
        }

        if (chkdate == "All" && chkComp == "All")
        {
            Query = Query;
        }
        if (chkdate != "All")
        {
            Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
        }

        if (i_name != "")
        {
            Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
        }

        Query = Query + "GROUP BY I_CODENO, STL_DOC_NUMBER ,STL_DOC_DATE,I_CODE, STL_CODE, STL_DOC_TYPE, STL_I_CODE, I_NAME,   (CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT'    WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD'  WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD'  WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.'  WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' ELSE STL_DOC_TYPE END)  order by rtrim(ltrim(I_CODENO)),(STL.STL_DOC_DATE) ,STL.STL_CODE";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptStockLedger.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptStockLedger.rpt");
            rptname.Refresh();
            //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            //rptname.SetParamererValue("",);
            //rptname.SetParamererValue("",Session["ShowBal"].ToString());
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedger.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
