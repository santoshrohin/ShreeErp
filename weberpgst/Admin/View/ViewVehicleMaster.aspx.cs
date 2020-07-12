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

public partial class Masters_VIEW_ViewVehicleMaster : System.Web.UI.Page
{
    #region Variables
    VehicleMaster_BL BL_VehicleMaster = null;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='133'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadVehicle();
                    dgVehicle.Enabled = true;
                    if (dgVehicle.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("VM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("VM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgVehicle.DataSource = dtFilter;
                            dgVehicle.DataBind();
                            dgVehicle.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Vehicle Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadVehicle
    private void LoadVehicle()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = CommonClasses.Execute("SELECT VM_CODE,VM_NAME,T_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE=0 order by VM_NAME,T_NAME");
            dt = CommonClasses.Execute("SELECT VM.VM_CODE,VM.VM_NAME,T.T_NAME FROM VEHICLE_MASTER VM ,TRANSAPORT_MASTER T WHERE T.T_CODE=VM.VM_T_CODE and VM.VM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and VM.ES_DELETE=0 order by VM.VM_NAME,T_NAME");
            dgVehicle.DataSource = dt;
            dgVehicle.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "LoadVehicle", Ex.Message);
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
            CommonClasses.SendError("Vehicle Master", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT VM_CODE,VM_NAME,T_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE='0' and (VM_NAME like upper('%" + str + "%') OR T_NAME like upper('%" + str + "%')) order by VM_NAME,T_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT VM_CODE,VM_NAME,T_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE='0' order by VM_NAME,T_NAME ");

            if (dtfilter.Rows.Count > 0)
            {
                dgVehicle.Enabled = true;
                dgVehicle.DataSource = dtfilter;
                dgVehicle.DataBind();
            }
            else
            {
                dtFilter.Clear();


                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("VM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));


                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgVehicle.DataSource = dtFilter;
                    dgVehicle.DataBind();
                    dgVehicle.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Admin/Add/VehicleMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgVehicle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgVehicle.Rows[e.RowIndex].FindControl("lblVM_CODE"))).Text))
                {

                    string s_code = ((Label)(dgVehicle.Rows[e.RowIndex].FindControl("lblVM_CODE"))).Text;
                    string s_name = ((Label)(dgVehicle.Rows[e.RowIndex].FindControl("lblSTATE_NAME"))).Text;

                    if (CommonClasses.CheckUsedInTran("CITY_MASTER", "CITY_VM_CODE", "AND ES_DELETE=0", s_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record becouse it has used in city master";

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "You cant delete this record it has used in city master", CommonClasses.MSG_Warning);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE STATE_MASTER SET ES_DELETE = 1 WHERE VM_CODE='" + Convert.ToInt32(s_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Vehicle Master", "Delete", "Vehicle Master", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            // ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);



                        }
                    }
                    //}
                    LoadVehicle();
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
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //    //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                //   return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "dgVehicle_RowDeleting", Ex.Message);
        }
    }
    protected void dgVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string s_code = ((Label)(dgVehicle.Rows[e.NewEditIndex].FindControl("lblVM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/VehicleMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
            CommonClasses.SendError("Vehicle Master", "dgVehicle_RowEditing", Ex.Message);
        }
    }
    protected void dgVehicle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void dgVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/VehicleMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/VehicleMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
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
            CommonClasses.SendError("Vehicle Master", "dgVehicle_RowCommand", Ex.Message);
        }
    }
    protected void dgVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from STATE_MASTER where VM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Vehicle Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

}
