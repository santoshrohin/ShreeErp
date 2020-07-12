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


public partial class Masters_ADD_AddProjectCodeMaster : System.Web.UI.Page
{
    # region Variables
    //UserMaster_BL BL_UserMaster = null;
    static int mlCode = 0;
    static string right = "";
    # endregion

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
                    try
                    {

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
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Project Code Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            // BL_UserMaster = new UserMaster_BL(mlCode);

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM PROJECT_CODE_MASTER WHERE PROCM_CODE=" + mlCode + " AND PROCM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtProCode.Text = dt.Rows[0]["PROCM_NAME"].ToString();

                if (str == "VIEW")
                {
                    txtProCode.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }

            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtProCode.Text == "")
        {
            ShowMessage("#Avisos", "The Field 'Tally Name' is Required", CommonClasses.MSG_Warning);
            return;
            //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

        }
        SaveRec();
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {

            if (Request.QueryString[0].Equals("INSERT"))
            {

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select PROCM_CODE,PROCM_NAME FROM PROJECT_CODE_MASTER WHERE lower(PROCM_NAME)= lower('" + txtProCode.Text.Trim() + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {

                    if (CommonClasses.Execute1("INSERT INTO PROJECT_CODE_MASTER (PROCM_COMP_ID,PROCM_NAME)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtProCode.Text.Trim() + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PROCM_CODE) from PROJECT_CODE_MASTER");
                        CommonClasses.WriteLog("PROJECT_CODE_MASTER", "Save", "PROJECT_CODE_MASTER", txtProCode.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                    }
                    else
                    {

                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);

                        txtProCode.Focus();
                    }
                }
                else
                {


                    ShowMessage("#Avisos", "Project Code Already Exists", CommonClasses.MSG_Warning);

                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {


                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PROJECT_CODE_MASTER WHERE ES_DELETE=0 AND PROCM_CODE!= '" + mlCode + "' AND lower(PROCM_NAME) = lower('" + txtProCode.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PROJECT_CODE_MASTER SET PROCM_NAME='" + txtProCode.Text + "' WHERE PROCM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", mlCode);
                        CommonClasses.WriteLog("Project Code Master", "Update", "Project Code Master", txtProCode.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);

                        txtProCode.Focus();
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "Project Code Already Exists", CommonClasses.MSG_Warning);
                    txtProCode.Focus();
                }


            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
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
            CommonClasses.SendError("Project Code Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", mlCode);
            }

            Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "btnCancel_Click", ex.Message);
        }
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtProCode.Text.Trim() == "")
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
            CommonClasses.SendError("Project Code Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click
}