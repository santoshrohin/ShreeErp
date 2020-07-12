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

public partial class Admin_View_ViewSubCategoryMaster : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='32'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                    {
                        LoadSubCategory();
                        dgSubCategory.Enabled = true;
                        if (dgSubCategory.Rows.Count == 0)
                        {
                            dtFilter.Clear();
                            if (dtFilter.Columns.Count == 0)
                            {
                                dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_CODE", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_DESC", typeof(string)));

                                dtFilter.Rows.Add(dtFilter.NewRow());
                                dgSubCategory.DataSource = dtFilter;
                                dgSubCategory.DataBind();
                                dgSubCategory.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadSubCategory
    private void LoadSubCategory()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM ITEM_SUBCATEGORY_MASTER WHERE SCAT_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 order by SCAT_DESC ");

            dgSubCategory.DataSource = dt;
            dgSubCategory.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SubCategory Master", "LoadUser", Ex.Message);
        }
    }
    #endregion LoadSubCategory

    #region btnInsert
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Admin/Add/SubCategoryMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";

            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master - View", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT  * FROM ITEM_SUBCATEGORY_MASTER WHERE ES_DELETE='FALSE' AND SCAT_CM_COMP_ID = '" + Session["CompanyId"] + "' and lower(SCAT_DESC) like lower('%" + str + "%') order by SCAT_DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT  * FROM ITEM_SUBCATEGORY_MASTER WHERE ES_DELETE='FALSE' AND SCAT_CM_COMP_ID = '" + Session["CompanyId"] + "' order by SCAT_DESC ");


            if (dtfilter.Rows.Count > 0)
            {
                dgSubCategory.DataSource = dtfilter;
                dgSubCategory.DataBind();
                dgSubCategory.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_DESC", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSubCategory.DataSource = dtFilter;
                    dgSubCategory.DataBind();
                    dgSubCategory.Enabled = false;
                }
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master - View", "LoadStatus", Ex.Message);
        }
    }
    #endregion

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSubCategory.PageIndex = e.NewPageIndex;
            LoadSubCategory();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/SubCategoryMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
                    return;
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/SubCategoryMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
        catch (Exception exc)
        {
            CommonClasses.SendError("SubCategory Master", "GridView1_RowCommand", exc.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            //lblmsg.Visible = false;
            //lblmsg.Text = "";
            DataTable dt = CommonClasses.Execute("SELECT MODIFY FROM ITEM_SUBCATEGORY_MASTER WHERE SCAT_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";

                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }

        return false;
    }
    #endregion

    #region GridView1_RowDeleting
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_CODE"))).Text))
                {
                    string um_code = ((Label)(dgSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_CODE"))).Text;
                    string um_name = ((Label)(dgSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_DESC"))).Text;

                    if (CommonClasses.CheckUsedInTran("EXCISE_TARIFF_MASTER", "((E_TALLY_BASIC=" + um_code + " or E_TALLY_SPECIAL=" + um_code + " or E_TALLY_EDU=" + um_code + " or E_TALLY_H_EDU=" + um_code + ") and 1", "AND ES_DELETE=0", "1)"))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE ITEM_SUBCATEGORY_MASTER SET ES_DELETE = 1 WHERE SCAT_CODE='" + Convert.ToInt32(um_code) + "'");

                        if (flag == true)
                        {
                            CommonClasses.WriteLog("SubCategory Master", "Delete", "SubCategory Master", um_name, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadSubCategory();
                    }
                }
            }
            else
            {
                ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region GridView1_RowEditing
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgSubCategory.Rows[e.NewEditIndex].FindControl("lblSCAT_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/SubCategoryMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
