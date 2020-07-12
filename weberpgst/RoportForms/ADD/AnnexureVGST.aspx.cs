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

public partial class RoportForms_ADD_AnnexureVGST : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            //bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string Condition = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            
            string Party = Request.QueryString[4].ToString();
            // string rpttype = Request.QueryString[6].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

                GenerateReport(Title,  Condition, From, To,Party);
            
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title, string Condition, string From, string To, string Party)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        dt = CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER_REPORT");
        dt.Clear();
        //dt = CommonClasses.Execute(" INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME, (SELECT IWD_SQTY FROM INWARD_DETAIL WHERE IWD_I_CODE = CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIAP') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND CHL.CL_CH_NO = COP.CL_CH_NO AND COP.CL_DATE <= '" + Convert.ToDateTime(From) + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND CHL.CL_CH_NO = COP.CL_CH_NO) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE  AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND  IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIAP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY  , ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where   IWM_CODE=CL_DOC_ID  AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,  ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where   IWM_CODE=CL_DOC_ID  AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN  ITEM_MASTER  on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where   CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'    AND    P_TYPE=2   AND CL_DOC_TYPE IN ('IWIAP','OutSUBINM')  ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        //dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME, (SELECT IWD_SQTY FROM INWARD_DETAIL WHERE IWD_I_CODE = CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From) + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY,ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN ITEM_MASTER on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND P_TYPE=1 AND CL_DOC_TYPE IN ('OutJWINM','IWIFP') ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        //dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME,    (SELECT IND_INQTY FROM INVOICE_DETAIL WHERE IND_I_CODE = CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY,ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN ITEM_MASTER on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND P_TYPE=1 AND CL_DOC_TYPE IN ('OutJWINM','IWIFP')   ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE) SELECT  CAST(CHL.CL_CH_NO AS int) AS CL_CH_NO, CHL.CL_DATE, CHL.CL_P_CODE, PARTY_MASTER.P_NAME, CHL.CL_I_CODE, RAW.I_CODENO, RAW.I_NAME,  RAWUNIT.I_UOM_NAME, CHL.CL_DOC_NO, CHL.CL_DOC_ID, CHL.CL_DOC_DATE, CHL.CL_CQTY, CHL.CL_DOC_TYPE, CHL.CL_ASSY_CODE AS IWD_I_CODE,  ITEM_MASTER.I_CODENO AS FINI_I_CODENO, ITEM_MASTER.I_NAME AS FINI_I_NAME, ASS_U.I_UOM_NAME AS FINI_UOM_NAME, (SELECT IND_INQTY FROM INVOICE_DETAIL WHERE IND_I_CODE = CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM') as IWD_SQTY, (SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_ASSY_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_ASSY_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL, (SELECT (IND_INQTY * BD_VQTY ) AS BD_VQTY FROM INVOICE_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_ASSY_CODE AND BM_I_CODE = IND_I_CODE AND IND_I_CODE=CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY ,0 AS IWM_CHALLAN_NO,0 AS IWM_INWARD_TYPE   FROM ITEM_UNIT_MASTER AS RAWUNIT INNER JOIN ITEM_MASTER AS RAW ON RAWUNIT.I_UOM_CODE = RAW.I_UOM_CODE INNER JOIN CHALLAN_STOCK_LEDGER AS CHL LEFT OUTER JOIN ITEM_MASTER ON ITEM_MASTER.I_CODE = CHL.CL_I_CODE LEFT OUTER JOIN ITEM_UNIT_MASTER AS ASS_U ON ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON CHL.CL_P_CODE = PARTY_MASTER.P_CODE ON RAW.I_CODE = CHL.CL_ASSY_CODE WHERE     (CHL.CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (CHL.CL_DOC_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AND (PARTY_MASTER.P_TYPE = 1) AND (CHL.CL_DOC_TYPE IN ('OutJWINM', 'IWIFP'))  ORDER BY CHL.CL_P_CODE, CHL.CL_I_CODE, CHL.CL_DOC_DATE, CL_CH_NO");
        dt.Clear();

        //SqlParameter[] par = new SqlParameter[2];
        //par[0] = new SqlParameter("@fromdate", Convert.ToDateTime(From));
        //par[1] = new SqlParameter("@todate", Convert.ToDateTime(To));
        ////par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
        //DL_DBAccess.Insertion_Updation_Delete("prGetOPCLBalance", par);

        double srpRcvQty = 0;
        dt.Clear();
        dt = CommonClasses.Execute("SELECT CL_CH_NO,CL_DATE,CL_P_CODE,CHALLAN_STOCK_LEDGER_REPORT.P_NAME,CL_I_CODE,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_DOC_NO,CL_DOC_DATE,CL_CQTY ,CL_DOC_TYPE,'' AS FINI_I_CODENO ,'' AS FINI_I_NAME ,0 AS IWD_SQTY , OP_BAL , CL_BAL,' ' AS IWM_CHALLAN_NO, 0 AS IWM_INWARD_TYPE,P_LBT_NO,E_TARIFF_NO FROM CHALLAN_STOCK_LEDGER_REPORT,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER where " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0 AND CL_DOC_TYPE='IWIFP'  and ITEM_MASTER.I_E_CODE=E_CODE UNION SELECT 0 AS CL_CH_NO,CL_DOC_DATE AS CL_DATE,CL_P_CODE,PARTY_MASTER.P_NAME,0 AS CL_I_CODE,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_DOC_NO,CL_DOC_DATE,SUM(CL_CQTY) AS CL_CQTY,CL_DOC_TYPE,FINI_I_CODENO,FINI_I_NAME ,ISNULL(IWD_SQTY,0) AS IWD_SQTY,OP_BAL,CL_BAL,IWM_CHALLAN_NO,IWM_INWARD_TYPE,P_LBT_NO,E_TARIFF_NO FROM CHALLAN_STOCK_LEDGER_REPORT,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER where " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0 AND CL_DOC_TYPE='OutJWINM'  and ITEM_MASTER.I_E_CODE=E_CODE  GROUP BY CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,FINI_I_CODENO,FINI_I_NAME,OP_BAL,CL_BAL,IWM_CHALLAN_NO,IWM_INWARD_TYPE,IWD_SQTY,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_P_CODE,PARTY_MASTER.P_NAME,P_LBT_NO,E_TARIFF_NO ORDER BY CL_DOC_DATE,CL_DOC_NO");

        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/AnnexureVGST.rpt")); //rptVendorStockBOMDetails1  rptCustomerStockBOMDetails1
            rptname.FileName = Server.MapPath("~/Reports/AnnexureVGST.rpt"); //rptVendorStockBOMDetails1  rptCustomerStockBOMDetails1
            rptname.Refresh();

            rptname.SetDataSource(dt);
            rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
            //rptname.SetParameterValue("Date", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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
            CommonClasses.SendError("Annexure V Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewAnnexureVGST.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Annexure V Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
