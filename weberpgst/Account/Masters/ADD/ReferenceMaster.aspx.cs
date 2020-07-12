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

public partial class Account_Masters_ADD_ReferenceMaster : System.Web.UI.Page
{
    #region General Declaration
    ReferenceMaster_BL BL_ReferenceMaster = null;
    static int mlCode = 0;
    static string right = "";
    static string URL = "../../../Account/Masters/View/ViewReferenceMaster.aspx";
    #endregion

    #region Events

    #region Page_Load
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
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                //right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_ReferenceMaster = new ReferenceMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_ReferenceMaster = new ReferenceMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtReferenceName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Reference Master", "PageLoad", ex.Message);
                }
            }

        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            //{
            SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Reference Master", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

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
            CommonClasses.SendError("Reference Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtReferenceDesc.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtReferenceName.Text.Trim() == "")
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
            CommonClasses.SendError("Reference Master", "CheckValid", Ex.Message);
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
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("NEWREFERENCE_MASTER", "MODIFY", "REF_CODE", mlCode);
            }
            Response.Redirect(URL, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    //#region btnClose_Click
    //protected void btnClose_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (mlCode != 0 && mlCode != null)
    //        {
    //            CommonClasses.RemoveModifyLock("NEWREFERENCE_MASTER", "MODIFY", "REF_CODE", mlCode);
    //        }
    //        Response.Redirect("~/Masters/VIEW/ViewReferenceMaster.aspx", false);
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Reference Master", "btnClose_Click", Ex.Message);
    //    }
    //}
    //#endregion btnClose_Click
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_ReferenceMaster = new ReferenceMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_ReferenceMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("NEWREFERENCE_MASTER", "MODIFY", "REF_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reference Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtReferenceName.Text = BL_ReferenceMaster.REF_NAME;
            txtReferenceDesc.Text = BL_ReferenceMaster.REF_DESC;
            if (str == "VIEW")
            {

                txtReferenceName.Enabled = false;
                txtReferenceDesc.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master", "GetValues", ex.Message);
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
            BL_ReferenceMaster.REF_NAME = txtReferenceName.Text.ToUpper();
            BL_ReferenceMaster.REF_DESC = txtReferenceDesc.Text.ToUpper();
            BL_ReferenceMaster.REF_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master", "Setvalues", ex.Message);
        }
        return res;
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
                BL_ReferenceMaster = new ReferenceMaster_BL();
                if (Setvalues())
                {
                    if (BL_ReferenceMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(REF_CODE) from NEWREFERENCE_MASTER");
                        CommonClasses.WriteLog("Reference Master", "Save", "Reference Master", BL_ReferenceMaster.REF_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(URL, false);
                    }
                    else
                    {
                        if (BL_ReferenceMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ReferenceMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            //PanelMsg.Visible = true;
                            //lblmsg.Text = BL_ReferenceMaster.Msg;
                            BL_ReferenceMaster.Msg = "";
                        }
                        txtReferenceName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_ReferenceMaster = new ReferenceMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_ReferenceMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("NEWREFERENCE_MASTER", "MODIFY", "REF_CODE", mlCode);
                        CommonClasses.WriteLog("Reference Master", "Update", "Reference Master", BL_ReferenceMaster.REF_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(URL, false);
                    }
                    else
                    {
                        if (BL_ReferenceMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ReferenceMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_ReferenceMaster.Msg = "";
                        }
                        txtReferenceName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reference Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Reference Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


}