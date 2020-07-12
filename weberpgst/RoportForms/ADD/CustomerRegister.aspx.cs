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

public partial class RoportForms_ADD_CustomerRegister : System.Web.UI.Page
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
           // bool chkAllSector = Convert.ToBoolean(Request.QueryString[3].ToString());

            if (ChkAll == true )
            {
                GenerateReport(Title, "All");
            }
            if (ChkAll == false )
            {
                string val = Request.QueryString[2];
                GenerateReport(Title, val);
            }
            //if (ChkAll == true && chkAllSector == true)
            //{
            //    GenerateReport(Title, "All", "All");
            //}
            //if (ChkAll == false && chkAllSector == true)
            //{
            //    string val = Request.QueryString[2];
            //    GenerateReport(Title, val, "All");
            //}
            //if (ChkAll == true && chkAllSector == false)
            //{
            //    string val1 = Request.QueryString[2];
            //    GenerateReport(Title, "All", val1);
            //}
            //if (ChkAll == false && chkAllSector == false)
            //{
            //    string val = Request.QueryString[2];
            //    GenerateReport(Title, val, val);

            //}
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string val/*, string val1*/)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        if (val == "All" )
        {
            Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,P_EMAIL,P_NOTE , ISNULL(P_CREDITDAYS,'') AS P_CREDITDAYS FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE = '0' ";
           // Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,SECTOR_MASTER.SCT_DESC,P_EMAIL,P_NOTE FROM PARTY_MASTER INNER JOIN SECTOR_MASTER ON PARTY_MASTER.P_SCT_CODE = SECTOR_MASTER.SCT_CODE where PARTY_MASTER.ES_DELETE=0 ";
        }
        if (val != "All" )
        {
            Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,P_EMAIL,P_NOTE , ISNULL(P_CREDITDAYS,'') AS P_CREDITDAYS FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE = 0 AND P_CODE = '" + val + "'";
           // Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,SECTOR_MASTER.SCT_DESC,P_EMAIL,P_NOTE FROM PARTY_MASTER INNER JOIN SECTOR_MASTER ON PARTY_MASTER.P_SCT_CODE = SECTOR_MASTER.SCT_CODE where PARTY_MASTER.ES_DELETE=0 and P_CODE='" + val + "'";
        }
       
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            rptname.Load(Server.MapPath("~/Reports/rptCustomerMaster.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptCustomerMaster.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            CrystalReportViewer1.ReportSource = rptname;
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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
