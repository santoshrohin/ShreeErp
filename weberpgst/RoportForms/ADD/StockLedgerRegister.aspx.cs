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


public partial class RoportForms_ADD_StockLedgerRegister : System.Web.UI.Page
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
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            string Type = Request.QueryString[6].ToString();
            bool ChkCatAll = Convert.ToBoolean(Request.QueryString[7].ToString());
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

            string i_name = Request.QueryString[5].ToString();
            string detail = Request.QueryString[6].ToString();
            string category = Request.QueryString[8].ToString();
            string WithVal = Request.QueryString[9].ToString();

            GenerateReport(Title, ChkAll.ToString(), ChkComp.ToString(), From, To, i_name, Type, category, ChkCatAll.ToString(), WithVal);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name, string Type, string category, string chkCat, string WithVal)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        #region Detail
        if (Type == "Detail")
        {
            Query = "select I_CODENO,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
            if (chkCat.ToUpper() != "TRUE")
            {
                Query = Query + " AND I_CAT_CODE='" + category + "'";
            }
            if (chkComp.ToUpper() != "TRUE")
            {
                Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
            }
            // Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptStockLedgerRegister.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerRegister.rpt");
                rptname.Refresh();
                //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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

        #region Summary
        else if (Type == "Summary")
        {
            //Query = "select I_CODENO,I_NAME,sum(isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0)) as ADD_QTY,sum(isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0)) as REM_QTY,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
            //Hiten Added Query
            Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc,I_INV_RATE,I_UWEIGHT from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";
            //Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";
            if (chkCat.ToUpper() != "TRUE")
            {
                Query = Query + " AND I_CAT_CODE='" + category + "'";
            }
            if (chkComp.ToUpper() != "TRUE")
            {
                Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
            }
            if (chkdate.ToUpper() != "TRUE")
            {
                // Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ";
            }
            Query = Query + " ) as A GROUP BY I_CODENO,I_NAME,ADD_QTY,REM_QTY,OpenStoc,I_INV_RATE,I_UWEIGHT";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (WithVal == "WithAmt")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockLedgerDetailRegisterWithVal.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerDetailRegisterWithVal.rpt");
                    rptname.Refresh();
                    //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtWithVal", "WithVal");
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;
                }
                else
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockLedgerDetailRegister.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerDetailRegister.rpt");
                    rptname.Refresh();
                    //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
                    rptname.SetDataSource(dt);
                   // rptname.SetParameterValue("txtWithVal", "");
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        #endregion

        #region MIS
        else
        {
            //Query = "select I_CODENO,I_NAME,sum(isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0)) as ADD_QTY,sum(isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0)) as REM_QTY,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
            //Hiten Added Query
            //Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='PRODTOSTORE' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS PROD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='OutSUBINM' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS DISPATCH_SUB,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='IWIAP' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS SUB_INW,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='TAXINV' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS INVOICE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='STCADJ' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS STOCK_ADJ_ADD,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='STCADJ' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS STOCK_ADJ_LESS,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='ISSPROD' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ISSUE_PROD,ISNULL(CAST((SELECT ABS(SUM(STL_DOC_QTY)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') ) AS numeric(10, 3)), 0) AS CLOSE_STOCK from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";

            Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='PRODTOSTORE' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS PROD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='OutSUBINM' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS DISPATCH_SUB,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='IWIAP' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS SUB_INW,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='TAXINV' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS INVOICE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='STCADJ' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS STOCK_ADJ_ADD,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='STCADJ' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS STOCK_ADJ_LESS,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='ISSPROD' AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ISSUE_PROD,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='IWIC' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS CUST_REJ,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='IWIFP' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS PROCESS_INWARD,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='OutJWINM' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS LABOUR_SALE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='DCIN' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS DC_INWARD,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='DCOUT' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS DC_OUT,ISNULL(CAST((SELECT ABS(SUM(STL_DOC_QTY )) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE =ITEM_MASTER.I_CODE) AND STL_DOC_TYPE='SubContractorRejection' AND  (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REJ_MATERIAL, ISNULL(CAST((SELECT (SUM(STL_DOC_QTY)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') ) AS numeric(10, 3)), 0) AS CLOSE_STOCK from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";

            //Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";
            //Query = "SELECT * FROM (select ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE where (ITEM_MASTER.I_ACTIVE_IND = 1)";
            //if (chkCat.ToUpper() != "TRUE")
            //{
            //Query = Query + " AND I_CAT_CODE='-2147483648'";
            //}
            if (chkComp.ToUpper() != "TRUE")
            {
                Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
            }

            if (chkdate.ToUpper() != "TRUE")
            {
                Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ";
            }
            Query = Query + " ) as A GROUP BY I_CODENO, I_NAME,PROD_QTY,DISPATCH_SUB,SUB_INW,INVOICE,CLOSE_STOCK,OpenStoc,STOCK_ADJ_ADD,STOCK_ADJ_LESS,ISSUE_PROD,CUST_REJ,LABOUR_SALE,PROCESS_INWARD,DC_INWARD,DC_OUT,REJ_MATERIAL";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptStockLedgerMIS.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerMIS.rpt");
                rptname.Refresh();
                //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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
}
