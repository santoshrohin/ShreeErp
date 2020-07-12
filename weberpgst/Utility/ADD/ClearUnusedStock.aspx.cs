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

public partial class Utility_ADD_ClearUnusedStock : System.Web.UI.Page
{
    static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                ViewState["dt"] = null;
                LoadUnsedStock();
            }
        }
    }

    private void LoadUnsedStock()
    {
        dt = CommonClasses.Execute("SELECT I_CODENO,I_NAME  FROM ITEM_MASTER WHERE (I_TEMP_CURRENT_BAL <> 0) AND (ES_DELETE = 0)");
        ViewState["dt"] = dt;
        if (((DataTable)ViewState["dt"]).Rows.Count>0)
        {
            dgInvDetails.DataSource = ((DataTable)ViewState["dt"]);
            dgInvDetails.DataBind(); 
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "No record found";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            return;
        }
    }

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (((DataTable)ViewState["dt"]).Rows.Count > 0)
            {
                //foreach (DataRow item in ((DataTable)ViewState["dt"]).Rows)
                //{

                if (CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_TEMP_CURRENT_BAL=0 WHERE (I_TEMP_CURRENT_BAL <> 0) AND (ES_DELETE = 0) "))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Cleared');", true);
                    Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Not cleared";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                   
                    return;
                }
                //}

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record not found";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
               
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Clear Unused Stock", "btnSubmit_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Clear Unused Stock", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Clear Unused Stock", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion
    protected void dgInvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInvDetails.PageIndex = e.NewPageIndex;
            LoadUnsedStock();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "dgInvoiceDettail_PageIndexChanging", ex.Message);
        }
    }
}
