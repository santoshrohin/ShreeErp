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

public partial class Masters_ADD_PoTypeMaster : System.Web.UI.Page
{
    # region Variables
    //UserMaster_BL BL_UserMaster = null;
    static int mlCode = 0;
    static string right = "";
    # endregion

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
                        CommonClasses.SendError("Po Type Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Po Type Master", "Page_Load", ex.Message);
        }
    } 
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM PO_TYPE_MASTER WHERE PO_T_CODE=" + mlCode + " AND PO_T_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtShortName.Text = dt.Rows[0]["PO_T_SHORT_NAME"].ToString();
                txtDescription.Text = dt.Rows[0]["PO_T_DESC"].ToString();
                txtFirstLetter.Text = dt.Rows[0]["PO_T_FIRST_LETTER"].ToString();

                if (str == "VIEW")
                {
                    txtDescription.Enabled = false;
                    txtFirstLetter.Enabled = false;
                    txtShortName.Enabled = false;
                   
                    btnSubmit.Visible = false;
                }
            }

            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PO_TYPE_MASTER", "MODIFY", "PO_T_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Po Type Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    } 
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select PO_T_CODE,PO_T_SHORT_NAME FROM PO_TYPE_MASTER WHERE lower(PO_T_SHORT_NAME)= lower('" + txtShortName.Text.Trim() + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO PO_TYPE_MASTER (PO_T_COMP_ID,PO_T_SHORT_NAME,PO_T_DESC,PO_T_FIRST_LETTER)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtShortName.Text.Trim() + "','"+txtDescription.Text+"','"+txtFirstLetter.Text+"')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PO_T_CODE) from PO_TYPE_MASTER");
                        CommonClasses.WriteLog("PO TYPE MASTER", "Save", "PO TYPE MASTER", txtDescription.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewPoTypeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtShortName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Short Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {


                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PO_TYPE_MASTER WHERE ES_DELETE=0 AND PO_T_CODE!= '" + mlCode + "' AND lower(PO_T_SHORT_NAME) = lower('" + txtShortName.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PO_TYPE_MASTER SET PO_T_SHORT_NAME='" + txtShortName.Text + "',PO_T_DESC='" + txtDescription.Text + "',PO_T_FIRST_LETTER='"+txtFirstLetter.Text+"' WHERE PO_T_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PO_TYPE_MASTER", "MODIFY", "PO_T_CODE", mlCode);
                        CommonClasses.WriteLog("Po Type Master", "Update", "Po Type Master", txtShortName.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewPoTypeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                        txtShortName.Focus();
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "Short Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                    txtShortName.Focus();
                }


            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Po Type Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PO_Type_Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
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
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Po Type Master", "btnCancel_Click", ex.Message.ToString());
        }
    } 
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PO_TYPE_MASTER", "MODIFY", "PO_T_CODE", mlCode);
            }

            Response.Redirect("~/Masters/VIEW/ViewPoTypeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Po Type Master", "btnCancel_Click", ex.Message);
        }
    } 
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtShortName.Text.Trim() == "")
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
            CommonClasses.SendError("PO Type Master", "CheckValid", Ex.Message);
        }

        return flag;
    } 
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    } 
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    } 
    #endregion
}
