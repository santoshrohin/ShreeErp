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

public partial class RoportForms_ADD_TrayStockReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/TrayStockReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery challan Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            //bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            //bool ChkAllUser = Convert.ToBoolean(Request.QueryString[4].ToString());

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //string p_name = Request.QueryString[6].ToString();
            //string i_name = Request.QueryString[7].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();
            //i_name = i_name.Replace("'", "''");

            GenerateReport(Title, From, To, group, way, Cond, Type);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition, string Type)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            // Query = "Select DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME as I_UOM_NAME,DCM_TYPE,DCD_ORD_QTY from DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER where   " + Condition + "  AND  DCM_TYPE='DLC' AND  DCM_CODE=DCD_DCM_CODE and DCD_I_CODE=I_CODE and DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCD_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE";
            //Query = "  Select DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME as I_UOM_NAME,DCM_TYPE,DCD_ORD_QTY ,SUM(DND_REC_QTY) AS DND_REC_QTY ,SUM(DND_SCRAP_QTY) AS  DND_SCRAP_QTY from DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER , DC_RETURN_MASTER,DC_RETURN_DETAIL where " + Condition + " DCM_TYPE='DLCT' AND  DCM_CODE=DCD_DCM_CODE and DCD_I_CODE=I_CODE and DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCD_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND DNM_CODE=DND_DNM_CODE AND DC_RETURN_MASTER.ES_DELETE=0 AND DNM_PARTY_DC_NO =DCM_CODE AND DCD_I_CODE=DND_I_CODE  GROUP BY DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME ,DCM_TYPE,DCD_ORD_QTY";
            Query = "SELECT   (SELECT ISNULL(SUM(STL_DOC_QTY),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE<'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OPENING_QTY,  ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME ,(SELECT ISNULL(SUM(ABS(STL_DOC_QTY)),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND STL_DOC_TYPE='DCTOUT') AS ISSUE_QTY,    (SELECT ISNULL(SUM(STL_DOC_QTY),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND STL_DOC_TYPE='DCTIN') AS IN_QTY, ( SELECT ISNULL(SUM(DND_SCRAP_QTY),0) AS DND_SCRAP_QTY  FROM DC_RETURN_DETAIL,DC_RETURN_MASTER where DNM_CODE=DND_DNM_CODE AND DND_I_CODE=I_CODE AND DNM_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AS SCRAP FROM ITEM_MASTER INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE WHERE  " + Condition + "   (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = - 2147483633)";

            //if (Type == "1")
            //{
            //    Query = Query + " HAVING ISNULL(DCD_ORD_QTY,0) -(ISNULL(SUM(DND_REC_QTY),0)+ISNULL(SUM(DND_SCRAP_QTY),0))>0 ";
            //}


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();


                rptname.Load(Server.MapPath("~/Reports/rptTrayMIS.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptTrayMIS.rpt");
                //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", " From " + From + " To " + To);

                CrystalReportViewer1.ReportSource = rptname;


            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
