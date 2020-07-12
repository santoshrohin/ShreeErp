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
public partial class RoportForms_ADD_CustomerRejectionRegister : System.Web.UI.Page
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
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRejectionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];


            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();

            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();

            string Condition = Request.QueryString[5].ToString();


            GenerateReport(Title, From, To, group, way, Condition);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //            Query = "SELECT Distinct CPOD_I_CODE,CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT FROM PARTY_MASTER INNER JOIN CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN LOG_MASTER ON CUSTPO_MASTER.CPOM_CODE = LOG_MASTER.LG_DOC_CODE ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_MASTER ON CUSTPO_DETAIL.CPOD_I_CODE = ITEM_MASTER.I_CODE where CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";
            //Query = "SELECT Distinct CPOD_I_CODE,CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU FROM PARTY_MASTER ,CUSTPO_MASTER,CUSTPO_DETAIL,LOG_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE=E_CODE and CPOM_CODE = CPOD_CPOM_CODE AND CPOM_CODE = LG_DOC_CODE AND P_CODE = CPOM_P_CODE AND CPOD_I_CODE = I_CODE AND CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";
            // Query = "SELECT CD_I_CODE,I_NAME,I_UOM_NAME,CD_UOM,CD_PO_CODE,CPOM_PONO,cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount FROM CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE";
            Query = "SELECT CR_GIN_NO, CR_GIN_DATE,CR_CHALLAN_DATE,CR_CHALLAN_NO,CR_INV_DATE,CR_INV_NO,CR_TYPE,CR_P_CODE,P_NAME,CR_NET_AMT,CD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CD_UOM,CD_PO_CODE,CPOM_PONO,cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER where  " + Condition + "    CUSTREJECTION_MASTER.CR_P_CODE=P_CODE and CUSTREJECTION_DETAIL.CD_CR_CODE=CR_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE";



            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectItemWise.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectCustWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectCustWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate","From "+ From + " To " + To);
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
}
