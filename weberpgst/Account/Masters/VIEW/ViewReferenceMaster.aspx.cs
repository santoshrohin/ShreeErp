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

public partial class Account_Masters_VIEW_ViewReferenceMaster : System.Web.UI.Page
{
    #region " Var "
    ReferenceMaster_BL BL_ReferenceMaster = null;
    static string right = "";
    static string fieldName;
    static string URL = "../../../Account/Masters/Add/ReferenceMaster.aspx";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='10'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadReference();
                    if (dgReferenceMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("REF_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("REF_NAME", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("REF_DESC", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgReferenceMaster.DataSource = dtFilter;
                            dgReferenceMaster.DataBind();
                            dgReferenceMaster.Enabled = false;
                        }
                    }
                    txtString.Focus();

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadReference
    private void LoadReference()
    {
        try
        {
            BL_ReferenceMaster = new ReferenceMaster_BL();
            BL_ReferenceMaster.REF_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_ReferenceMaster.FillGrid(dgReferenceMaster);
            if (dgReferenceMaster.Rows.Count > 0)
            {
                dgReferenceMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master-View", "LoadReference", Ex.Message);
        }
    }
    #endregion

    #region dgReferenceMaster_RowDeleting
    protected void dgReferenceMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgReferenceMaster.Rows[e.RowIndex].FindControl("lblREF_CODE"))).Text))
                {
                    BL_ReferenceMaster = new ReferenceMaster_BL();
                    string REF_code = ((Label)(dgReferenceMaster.Rows[e.RowIndex].FindControl("lblREF_CODE"))).Text;
                    string REF_name = ((Label)(dgReferenceMaster.Rows[e.RowIndex].FindControl("lblREF_NAME"))).Text;
                    string REF_desc = ((Label)(dgReferenceMaster.Rows[e.RowIndex].FindControl("lblREF_DESC"))).Text;
                    BL_ReferenceMaster.REF_CODE = Convert.ToInt32(REF_code);
                    if (CommonClasses.CheckUsedInTran("RECIEPT_DETAIL", "RCPD_INVOICE_CODE", "AND ES_DELETE=0", REF_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        // ShowMessage("#Avisos", "You cant delete this record it has used in Components", CommonClasses.MSG_Warning);
                    }
                    else
                    {
                        bool flag = BL_ReferenceMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Reference Master", "Delete", "Reference Master", REF_name, Convert.ToInt32(REF_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        }

                        LoadReference();
                    }

                }

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgReferenceMaster_RowEditing
    protected void dgReferenceMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string REF_code = ((Label)(dgReferenceMaster.Rows[e.NewEditIndex].FindControl("lblREF_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect(""+URL+"?c_name=" + type + "&REF_code=" + REF_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "dgReferenceMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgReferenceMaster_PageIndexChanging
    protected void dgReferenceMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgReferenceMaster.PageIndex = e.NewPageIndex;
        LoadReference();
    }
    #endregion

    #region dgReferenceMaster_RowUpdating
    protected void dgReferenceMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region dgReferenceMaster_RowCommand
    protected void dgReferenceMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {

                    string type = "VIEW";
                    string REF_code = e.CommandArgument.ToString();
                    Response.Redirect(""+URL+"?c_name=" + type + "&REF_code=" + REF_code, false);

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string REF_code = e.CommandArgument.ToString();
                        Response.Redirect(""+URL+"?c_name=" + type + "&REF_code=" + REF_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_Reference_MASTER where REF_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "ModifyLog", Ex.Message);
        }

        return false;
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
            CommonClasses.SendError("Reference Master - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT REF_CODE,REF_NAME,REF_DESC FROM ITEM_Reference_MASTER WHERE REF_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (REF_NAME like upper('%" + str + "%') or REF_DESC like upper('%" + str + "%')) order by REF_NAME,REF_DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT REF_CODE,REF_NAME,REF_DESC FROM ITEM_Reference_MASTER WHERE REF_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by REF_NAME,REF_DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgReferenceMaster.DataSource = dtfilter;
                dgReferenceMaster.DataBind();
                dgReferenceMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("REF_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("REF_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("REF_DESC", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgReferenceMaster.DataSource = dtFilter;
                    dgReferenceMaster.DataBind();
                    dgReferenceMaster.Enabled = false;
                }

            }


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master - View", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect(""+URL+"?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
