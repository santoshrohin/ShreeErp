using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Masters_VIEW_ViewServiceMaster : System.Web.UI.Page
{
    string c_type = "";
    static string right = "";
    DataTable dtFilter = new DataTable();

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


                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='119'");
                    right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                    LoadItem();
                    if (dgServiceTypeMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        dgServiceTypeMaster.Enabled = false;
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("S_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("S_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("S_ACTIVE_IND", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgServiceTypeMaster.DataSource = dtFilter;
                            dgServiceTypeMaster.DataBind();
                        }
                    }
                    //LoadStatus();


                    //LoadFilter();
                }

                txtString.Focus();

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Type Master", "Page_Load", Ex.Message);
        }
    }

    #region LoadItem
    private void LoadItem()
    {
        try
        {
            string strSql = "";
            strSql = " SELECT SERVICE_TYPE_MASTER.S_CODE, SERVICE_TYPE_MASTER.S_CODENO, SERVICE_TYPE_MASTER.S_NAME, EXCISE_TARIFF_MASTER.E_TARIFF_NO,case when S_ACTIVE_IND=1 then '1' else '0' end as S_ACTIVE_IND FROM SERVICE_TYPE_MASTER INNER JOIN EXCISE_TARIFF_MASTER ON SERVICE_TYPE_MASTER.S_E_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE (SERVICE_TYPE_MASTER.ES_DELETE = 0) AND (EXCISE_TARIFF_MASTER.ES_DELETE = 0)";
            DataTable dtfilter = CommonClasses.Execute(strSql);

            if (dtfilter.Rows.Count > 0)
            {
                dgServiceTypeMaster.DataSource = dtfilter;
                dgServiceTypeMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Type Master", "LoadItem", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Type Master", "btnCancel_Click", ex.Message);
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

        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "btnSearch_Click", ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        string strSql = "";
        strSql = " SELECT SERVICE_TYPE_MASTER.S_CODE, SERVICE_TYPE_MASTER.S_CODENO, SERVICE_TYPE_MASTER.S_NAME, EXCISE_TARIFF_MASTER.E_TARIFF_NO,case when S_ACTIVE_IND=1 then '1' else '0' end as S_ACTIVE_IND FROM SERVICE_TYPE_MASTER INNER JOIN EXCISE_TARIFF_MASTER ON SERVICE_TYPE_MASTER.S_E_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE (SERVICE_TYPE_MASTER.ES_DELETE = 0) AND (EXCISE_TARIFF_MASTER.ES_DELETE = 0)";
        DataTable dtfilter = CommonClasses.Execute(strSql);
        if (dtfilter.Rows.Count > 0)
        {
            dtfilter.CaseSensitive = false;
            dtfilter.DefaultView.RowFilter = "S_CODENO Like '%" + txtString.Text.ToUpper() + "%' OR S_NAME Like '%" + txtString.Text.ToUpper() + "%' OR E_TARIFF_NO Like '%" + txtString.Text.ToUpper() + "%'";
            dgServiceTypeMaster.DataSource = dtfilter;
            dgServiceTypeMaster.DataBind();
        }
        else
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("S_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("S_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("S_ACTIVE_IND", typeof(String)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgServiceTypeMaster.DataSource = dtFilter;
            dgServiceTypeMaster.DataBind();
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
                Response.Redirect("~/Masters/ADD/ServiceTypeMaster.aspx?c_name=" + type + "", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You no rights to Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Type Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgServiceTypeMaster_RowDeleting
    protected void dgServiceTypeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgServiceTypeMaster.Rows[e.RowIndex].FindControl("lblS_CODE"))).Text))
            {

                string i_code = ((Label)(dgServiceTypeMaster.Rows[e.RowIndex].FindControl("lblS_CODE"))).Text;
                string i_name = ((Label)(dgServiceTypeMaster.Rows[e.RowIndex].FindControl("lblS_NAME"))).Text;
                string i_codeno = ((Label)(dgServiceTypeMaster.Rows[e.RowIndex].FindControl("lblS_CODENO"))).Text;

                if (CommonClasses.CheckUsedInTran("SERVICE_PO_MASTER,SERVICE_PO_DETAILS", "SRPOD_I_CODE", "AND SRPOD_SPOM_CODE=SRPOM_CODE and SERVICE_PO_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Service Purchase Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                //if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER,CUSTPO_DETAIL", "CPOD_I_CODE", "AND CPOD_CPOM_CODE=CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0", i_code))
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "You cant delete this record it has used in Sales Order";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    return;
                //}
                //if (CommonClasses.CheckUsedInTran("ISSUE_MASTER,ISSUE_MASTER_DETAIL ", "IMD_I_CODE", " AND ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE AND ISSUE_MASTER.ES_DELETE=0", i_code))
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "You cant delete this record it has used in Issue To Production";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    return;
                //}
                //if (CommonClasses.CheckUsedInTran("PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL ", "PSD_I_CODE", " AND PS_CODE=PSD_PS_CODE AND PRODUCTION_TO_STORE_MASTER.ES_DELETE=0", i_code))
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "You cant delete this record it has used in  Production TO store";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    return;
                //}
                bool flag = CommonClasses.Execute1("UPDATE SERVICE_TYPE_MASTER SET ES_DELETE = 1 WHERE S_CODE='" + Convert.ToInt32(i_code) + "'");
                if (flag == true)
                {
                    CommonClasses.WriteLog("Service Type Master", "Delete", "Service Type Master", i_name, Convert.ToInt32(i_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Deleted Successfully";
                    LoadItem();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

        }
        else
        {
            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgServiceTypeMaster_PageIndexChanging
    protected void dgServiceTypeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgServiceTypeMaster.PageIndex = e.NewPageIndex;
            //LoadItem();
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "dgServiceTypeMaster_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from SERVICE_TYPE_MASTER where S_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region dgServiceTypeMaster_RowCommand
    protected void dgServiceTypeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/ServiceTypeMaster.aspx?c_name=" + type + "&i_code=" + i_code + "", false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

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
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/ServiceTypeMaster.aspx?c_name=" + type + "&i_code=" + i_code + "", false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Type Master", "dgServiceTypeMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

}
