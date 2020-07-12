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


public partial class Masters_ADD_VehicleMaster : System.Web.UI.Page
{
    #region General Declaration
    VehicleMaster_BL BL_VehicleMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    //
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='133'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadVehicle();

                if (Request.QueryString[0].Equals("VIEW"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("VIEW");
                }
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("MOD");
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Vehicle Master", "Page_Load", Ex.Message);
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }
    #endregion
    #region btnClose_Click
    //protected void btnClose_Click(object sender, EventArgs e)
    //{
    //    txtVehicleName.Text = "";
    //    ddlTransport.SelectedIndex = 0;
    //    if (mlCode != 0 && mlCode != null)
    //    {
    //        CommonClasses.RemoveModifyLock("VEHICLE_MASTER", "MODIFY", "VM_CODE", mlCode);
    //    }
    //    Response.Redirect("~/Admin/View/ViewVehicleMaster.aspx", false);
    //}
    #endregion btnClose_Click

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();

                }
                else
                {
                    CancelRecord();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        // SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtVehicleName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlTransport.SelectedIndex <= 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    private void CancelRecord()
    {
        txtVehicleName.Text = "";
        ddlTransport.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("VEHICLE_MASTER", "MODIFY", "VM_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewVehicleMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtVehicleName.Text = "";
            ddlTransport.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "ClearFied", Ex.Message);
        }
    }
    #endregion


    //newly Added 


    #region LoadVehicle
    private void LoadVehicle()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select T_CODE,T_NAME from TRANSAPORT_MASTER where ES_DELETE=0 and T_CM_COMP_ID='" + (string)Session["CompanyId"] + "'");
                ddlTransport.DataSource = dt;
                ddlTransport.DataTextField = "T_NAME";
                ddlTransport.DataValueField = "T_CODE";
                ddlTransport.DataBind();
                ddlTransport.Items.Insert(0, new ListItem("Select Transport", "0"));
            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "LoadVehicle", Ex.Message);

        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {


        try
        {
            BL_VehicleMaster = new VehicleMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_VehicleMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("VEHICLE_MASTER", "MODIFY", "VM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "ViewRec", Ex.Message);
        }

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceStateName = txtVehicleName.Text;

            StrReplaceStateName = StrReplaceStateName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_VehicleMaster = new VehicleMaster_BL();
                if (Setvalues())
                {
                    if (BL_VehicleMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(VM_CODE) from VEHICLE_MASTER");
                        CommonClasses.WriteLog("Vehicle Master", "Save", "Vehicle Master", BL_VehicleMaster.VM_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewVehicleMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_VehicleMaster.Msg != "")
                        {
                            //ShowMessage("#Avisos", BL_VehicleMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            lblmsg.Text = BL_VehicleMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            BL_VehicleMaster.Msg = "";
                        }
                        ddlTransport.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_VehicleMaster = new VehicleMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_VehicleMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("VEHICLE_MASTER", "MODIFY", "VM_CODE", mlCode);
                        CommonClasses.WriteLog("Vehicle Master", "Update", "Vehicle Master", BL_VehicleMaster.VM_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewVehicleMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_VehicleMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_VehicleMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            BL_VehicleMaster.Msg = "";
                        }
                        ddlTransport.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "SaveRec", ex.Message);
        }
        return result;
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

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlTransport.SelectedValue = BL_VehicleMaster.VM_T_CODE.ToString();
                txtVehicleName.Text = BL_VehicleMaster.VM_NAME;
                txtVehicleName.Enabled = false;
                ddlTransport.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                ddlTransport.SelectedValue = BL_VehicleMaster.VM_T_CODE.ToString();
                txtVehicleName.Text = BL_VehicleMaster.VM_NAME;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_VehicleMaster.VM_T_CODE = Convert.ToInt32(ddlTransport.SelectedValue);
            BL_VehicleMaster.VM_NAME = txtVehicleName.Text;
            BL_VehicleMaster.VM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion






    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtVehicleName.Text = "";
        ddlTransport.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("VEHICLE_MASTER", "MODIFY", "VM_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewVehicleMaster.aspx", false);
    }
    #endregion


}
