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


public partial class RoportForms_VIEW_ViewRequirment : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='131'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            txtFromDate.Text = Convert.ToDateTime(System.DateTime.Now).ToString("MMM yyyy");
            txtFromDate.Enabled = false;
            ddlComponent.Enabled = false;
            chkAllComp.Checked = true;
            LoadComponent();
           
        }
    }
    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO+' '+I_NAME  AS I_NAME FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL,ITEM_MASTER where CS_CODE=CSD_CS_CODE AND CSD_I_CODE=I_CODE  AND CUSTOMER_SCHEDULE.ES_DELETE=0 AND DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Month + "' AND   DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Year + "' AND I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO+' '+I_NAME ");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_NAME";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requirment", "LoadComponent", Ex.Message);
        }

    }
    #endregion


    #region ddlComponent_SelectedIndexChanged
    protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = false;
        }
        else
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = true;
            ddlComponent.Focus();
        }
    }
    #endregion

    #region LoadComponent by Category
    private void LoadComponent(string CAT_CODE)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE='" + CAT_CODE + "' and ES_DELETE=0  ORDER BY I_NAME");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_CODENO";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requirment", "LoadComponent", Ex.Message);
        }

    }
    #endregion


    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception)
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
            CommonClasses.SendError("Purchase Requirment", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            From = txtFromDate.Text;
            string i_code = "";
            string str1 = "DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Month + "' AND   DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(txtFromDate.Text).Year + "'  AND ";
            if (ddlComponent.SelectedValue != "0")
            {
                str1 =str1+ " CSD_I_CODE='"+ ddlComponent.SelectedValue.ToString()+"'  AND " ;
            }
            Response.Redirect("~/RoportForms/ADD/Requirment.aspx?Title=" + Title + "&FromDate=" + From + "&cond=" + str1 + " ", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requirment", "btnShow_Click", Ex.Message);
        }
    }
    #endregion


}
