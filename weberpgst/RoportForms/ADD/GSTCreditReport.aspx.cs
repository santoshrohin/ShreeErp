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

public partial class RoportForms_ADD_GSTCreditReport : System.Web.UI.Page
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
            string Condition = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string Party = Request.QueryString[4].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            if (Condition != "")
            {
               // Condition = "WHERE " + Condition;
                //string sub = Condition.Substring(0, Condition.Length-3);
                //Condition = sub;
            }

            GenerateReport(Title, Condition, From, To, Party);
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

        double srpRcvQty = 0;
        dt.Clear();

        dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME,P_NAME,IWM_NO,IWD_SQTY,IWD_RATE,convert(varchar,IWM_DATE,106) as IWM_DATE,SPOM_PONO,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE from ITEM_MASTER,SUPP_PO_MASTER,PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where " + Condition + " PARTY_MASTER.ES_DELETE=0 AND I_ACTIVE_IND=1 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE and I_CODE=IWD_I_CODE and INWARD_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND P_ACTIVE_IND=1 AND IWD_MODVAT_FLG=0 AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=IWD_CPOM_CODE AND P_TYPE=2");
       
        //Query done without Item if required
        //dt = CommonClasses.Execute("select distinct P_NAME,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,SPOM_PONO from SUPP_PO_MASTER,PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where  PARTY_MASTER.ES_DELETE=0 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE and INWARD_MASTER.ES_DELETE=0 AND P_ACTIVE_IND=1 AND IWD_MODVAT_FLG=0 AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=IWD_CPOM_CODE");
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptPendingGINForCredit.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptPendingGINForCredit.rpt");
            rptname.Refresh();

            rptname.SetDataSource(dt);
            rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
            rptname.SetParameterValue("Title", "Pending GIN For GST Credit Entries");
            rptname.SetParameterValue("Date", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/GSTCreditReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
