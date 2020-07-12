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


public partial class Admin_MasterRegister_ADD_AddUserMaster : System.Web.UI.Page
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
            bool ChkUserNameAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            string User_Name = Request.QueryString[2].ToString();

            if (ChkUserNameAll == true)
            {
                GenerateReport(Title, User_Name, "All");
            }
            if (ChkUserNameAll == false)
            {
                //string Comp = Request.QueryString[2];
                GenerateReport(Title, User_Name, "NotAll");
            }
            
           
        }
        catch (Exception)
        {

        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string Title, string User_Name, string Comp1)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        if (Comp1 == "All")
        {
           // Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE  ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.I_CAT_CODE='-2147483646'";
            Query = "SELECT UM_CODE,UM_NAME,UM_EMAIL,UM_LEVEL,UM_USERNAME FROM USER_MASTER WHERE ES_DELETE=0";
        }
        else
        {
           // Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE  ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.I_CAT_CODE='-2147483646' and ITEM_MASTER.I_CODE=" + User_Name + "";
            Query = "SELECT UM_CODE,UM_NAME,UM_EMAIL,UM_LEVEL,UM_USERNAME FROM USER_MASTER WHERE  ES_DELETE=0 AND UM_CODE='"+User_Name+"'";
        }
       
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {


            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptUserMaster.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptUserMaster.rpt");
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
            CommonClasses.SendError("User Master View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/MasterRegister/VIEW/ViewUserMaster.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }

    }
}
