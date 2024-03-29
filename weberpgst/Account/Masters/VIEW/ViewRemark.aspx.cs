﻿using System;
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

public partial class Account_Masters_VIEW_ViewRemark : System.Web.UI.Page
{
    #region Variables
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='88'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadStatus(txtString);

                }
                txtString.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master ", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/AccountDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgProcessMaster_RowDeleting
    protected void dgProcessMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblR_CODE"))).Text))
                {
                    string Process_CODE = ((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblR_CODE"))).Text;
                    string Process_NAME = ((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblR_DESC"))).Text;

                    if (CommonClasses.CheckUsedInTran("CASH_BOOK_ENTRY", "C_REMARK_CODE", "AND ES_DELETE=0 ", Process_CODE))
                    {
                        //ShowMessage("#Avisos", "You cant delete this record it has used in State Master", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Cashbook Entry";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE REMARK_MASTER SET ES_DELETE=1 where R_CODE='" + Process_CODE + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Remark Master", "Delete", "Remark Master", Process_NAME, Convert.ToInt32(Process_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                        }
                    }
                }
                LoadProcess();
            }



            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "dgProcessMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgProcessMaster_RowEditing
    protected void dgProcessMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string Process_CODE = ((Label)(dgProcessMaster.Rows[e.NewEditIndex].FindControl("lblProcess_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("../../../Account/Masters/Add/RemarkMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);
            }



            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "dgProcessMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgProcessMaster_PageIndexChanging
    protected void dgProcessMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgProcessMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
            dgProcessMaster.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgProcessMaster_RowUpdating
    protected void dgProcessMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region dgProcessMaster_RowCommand
    protected void dgProcessMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {


                    string type = "VIEW";
                    string Process_CODE = e.CommandArgument.ToString();
                    Response.Redirect("../../../Account/Masters/Add/RemarkMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);

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
                        string Process_CODE = e.CommandArgument.ToString();
                        Response.Redirect("../../../Account/Masters/Add/RemarkMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }



                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "dgProcessMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("../../../Account/Masters/Add/RemarkMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Remark Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion


    #endregion

    #region Methods

    #region LoadProcess
    private void LoadProcess()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT R_CODE, R_DESC, IS_ACTIVE, R_CM_ID FROM REMARK_MASTER where ES_DELETE=0");

            dgProcessMaster.DataSource = dt;
            dgProcessMaster.DataBind();
            dgProcessMaster.Enabled = true;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "LoadProcess", Ex.Message);
        }
    }
    #endregion


    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from Process_MASTER where Process_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Remark Master", "ShowMessage", Ex.Message);
            return false;
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

            if (txtString.Text.Trim() != "")
                dtfilter = CommonClasses.Execute("SELECT R_CODE, R_DESC, IS_ACTIVE, R_CM_ID  FROM REMARK_MASTER where ES_DELETE=0 AND  R_CM_ID='" + Session["CompanyId"] + "' and (R_DESC like upper('%" + str + "%')) order by R_DESC ");
            else
                dtfilter = CommonClasses.Execute("SELECT R_CODE, R_DESC, IS_ACTIVE, R_CM_ID  FROM REMARK_MASTER where ES_DELETE=0 AND R_CM_ID='" + Session["CompanyId"] + "'  order by R_DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgProcessMaster.Enabled = true;
                dgProcessMaster.DataSource = dtfilter;
                dgProcessMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("R_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("R_DESC", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProcessMaster.DataSource = dtFilter;
                    dgProcessMaster.DataBind();
                    //dgProcessMaster.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master", "LoadStatus", ex.Message);
        }
    }

    #endregion
    #endregion
}
