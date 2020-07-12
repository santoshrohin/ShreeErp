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
public partial class RoportForms_ADD_IssueToProductionRegister : System.Web.UI.Page
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
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());

            bool ChkAllitem = Convert.ToBoolean(Request.QueryString[2].ToString());

            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();

            string i_name = Request.QueryString[5].ToString();
            string group = Request.QueryString[6].ToString();
            string way = Request.QueryString[7].ToString();
            string StrCond = Request.QueryString[8].ToString();

            i_name = i_name.Replace("'", "''");


            #region Detail
            if (way == "Direct")
            {
                if (group == "Datewise")
                {

                }
                if (group == "ItemWise")
                {

                }

            }
            #endregion

            #region Summary
            if (way == "AsperReq")
            {
                if (group == "Datewise")
                {

                }
                if (group == "ItemWise")
                {

                    //GenerateReport(Title, "All", "ONE", From, To, i_name, group, way);

                }


            }
            #endregion

            GenerateReport(Title, From, To, group, way, StrCond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string strcond)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            // Query = "SELECT CR_GIN_NO,CONVERT(VARCHAR,CR_GIN_DATE,106) AS CR_GIN_DATE,CR_P_CODE,P_NAME,CR_NET_AMT,CD_I_CODE,I_NAME,I_UOM_NAME,CD_UOM,CD_PO_CODE,CPOM_PONO,cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER where CUSTREJECTION_MASTER.CR_P_CODE=P_CODE and CUSTREJECTION_DETAIL.CD_CR_CODE=CR_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE";

            if (way == "Direct")
            {
                Query = "SELECT DISTINCT ITEM_MASTER.I_CODENO, ITEM_MASTER.I_CODE, ISSUE_MASTER.IM_CODE, ISSUE_MASTER.IM_MATERIAL_REQ, ISSUE_MASTER.IM_NO, CONVERT(varchar, ISSUE_MASTER.IM_DATE, 106) AS IM_DATE, (CASE WHEN IM_TYPE = 1 THEN 'As Per Material Req' ELSE 'Direct' END) AS IM_TYPE  ,IM_ISSUEBY,IM_REQBY, ISNULL(MATERIAL_REQUISITION_MASTER.MR_BATCH_NO, '') AS MR_BATCH_NO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, ISNULL(ITEM_MASTER.I_INV_RATE, 0.00) AS I_CURRENT_BAL, ISSUE_MASTER_DETAIL.IMD_REQ_QTY, ISSUE_MASTER_DETAIL.IMD_ISSUE_QTY, ISSUE_MASTER_DETAIL.IMD_REMARK FROM  MATERIAL_REQUISITION_MASTER RIGHT OUTER JOIN ISSUE_MASTER INNER JOIN ISSUE_MASTER_DETAIL INNER JOIN ITEM_MASTER ON ISSUE_MASTER_DETAIL.IMD_I_CODE = ITEM_MASTER.I_CODE AND ISSUE_MASTER_DETAIL.IMD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ISSUE_MASTER_DETAIL.IMD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND ISSUE_MASTER_DETAIL.IMD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE ON ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE ON  MATERIAL_REQUISITION_MASTER.MR_CODE = ISSUE_MASTER.IM_MATERIAL_REQ WHERE  " + strcond + " (ISSUE_MASTER.ES_DELETE = 0) AND (ISSUE_MASTER.IM_COMP_ID = " + Session["CompanyCode"] + ") AND    (ISSUE_MASTER_DETAIL.ES_DELETE = 0) and IM_TYPE=2 ";
                // Query = "SELECT distinct(I_CODENO) as I_CODENO,ISSUE_MASTER.IM_CODE,IM_MATERIAL_REQ,IM_NO,convert(varchar,IM_DATE,106)as IM_DATE,(case when IM_TYPE=1 then 'As Per Material Req' else 'Direct' end) as IM_TYPE,isnull(MR_BATCH_NO,'') as MR_BATCH_NO ,I_NAME,I_UOM_NAME,I_CURRENT_BAL,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK FROM ISSUE_MASTER LEFT OUTER JOIN MATERIAL_REQUISITION_MASTER ON ISSUE_MASTER.IM_MATERIAL_REQ = MATERIAL_REQUISITION_MASTER.MR_CODE,ITEM_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_UNIT_MASTER,ISSUE_MASTER_DETAIL where ISSUE_MASTER.ES_DELETE=0 and IM_COMP_ID=1 and I_CODE=IMD_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and ISSUE_MASTER_DETAIL.ES_DELETE=0 ";
            }
            else
            {
                Query = "  SELECT distinct(I_CODENO) as I_CODENO,ITEM_MASTER.I_CODE,ISSUE_MASTER.IM_CODE,IM_MATERIAL_REQ,IM_NO,convert(varchar,IM_DATE,106)as IM_DATE,(case when IM_TYPE=1 then 'As Per Material Req' else 'Direct' end) as IM_TYPE  ,IM_ISSUEBY,IM_REQBY,isnull(MR_BATCH_NO,'') as MR_BATCH_NO ,I_NAME,I_UOM_NAME,isnull(I_CURRENT_BAL,0.00)as I_CURRENT_BAL,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK FROM  MATERIAL_REQUISITION_MASTER , ISSUE_MASTER INNER JOIN ISSUE_MASTER_DETAIL INNER JOIN ITEM_MASTER ON ISSUE_MASTER_DETAIL.IMD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ISSUE_MASTER_DETAIL.IMD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE ON  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE    where " + strcond + " ISSUE_MASTER.ES_DELETE=0 and IM_COMP_ID=" + Session["CompanyCode"] + " and I_CODE=IMD_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and ISSUE_MASTER_DETAIL.ES_DELETE=0 and MATERIAL_REQUISITION_MASTER.MR_CODE = ISSUE_MASTER.IM_MATERIAL_REQ ";
                //Query = "SELECT distinct(I_CODENO) as I_CODENO,ISSUE_MASTER.IM_CODE,IM_MATERIAL_REQ,IM_NO,convert(varchar,IM_DATE,106)as IM_DATE,(case when IM_TYPE=1 then 'As Per Material Req' else 'Direct' end) as IM_TYPE,isnull(MR_BATCH_NO,'') as MR_BATCH_NO ,I_NAME,I_UOM_NAME,I_CURRENT_BAL,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK FROM ISSUE_MASTER , MATERIAL_REQUISITION_MASTER,ITEM_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_UNIT_MASTER,ISSUE_MASTER_DETAIL where ISSUE_MASTER.ES_DELETE=0 and IM_COMP_ID=1 and I_CODE=IMD_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and ISSUE_MASTER_DETAIL.ES_DELETE=0 and  ISSUE_MASTER.IM_MATERIAL_REQ = MATERIAL_REQUISITION_MASTER.MR_CODE ";

            }
            #region MyRegion
            /*
            #region Detail
            if (way == "Direct")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and ITEM_MASTER.I_CODE='" + i_name + "' ";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }

                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and ITEM_MASTER.I_CODE='" + i_name + "'";

                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }

                }
                #endregion

            }
            #endregion

            #region AsPerReq
            if (way == "AsperReq")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and ITEM_MASTER.I_CODE='" + i_name + "' ";
                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {

                    if (date1 == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && item != "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and ITEM_MASTER.I_CODE='" + i_name + "'";

                    }
                    if (date1 != "All" && item == "All")
                    {
                        Query = Query + " and IM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                }
                #endregion


            }
            #endregion


            */

            #endregion
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIssueToProductionDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIssueToProductionDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    //if (way == "AsPerReg")
                    //{
                    //    rptname.SetParameterValue("txtType", "1");

                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtType", "0");

                    //}
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIssueToProductionItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIssueToProductionItemWise.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    //if (way == "Summary")
                    //{
                    //    rptname.SetParameterValue("txtType", "1");

                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtType", "0");

                    //}
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
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

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewIssueToProductionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

}
