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

public partial class Account_Masters_ADD_RemarkMaster : System.Web.UI.Page
{
    #region General Declaration
    static int mlCode = 0;
    static string right = "";
    #endregion

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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='88'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    ViewState["mlCode"] = mlCode;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtProcessName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Remark Master", "PageLoad", ex.Message);
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
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            // {
            if (txtProcessName.Text == "")
            {
                ShowMessage("#Avisos", "The Field 'Process Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }
            SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Remark Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Remark Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion


    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();


    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtProcessName.Text.Trim() == "")
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
            CommonClasses.SendError("Remark Master", "CheckValid", Ex.Message);
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
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("REMARK_MASTER", "MODIFY", "R_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("../../../Account/Masters/View/ViewRemark.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Methods


    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(" SELECT * FROM REMARK_MASTER WHERE ES_DELETE=0 AND R_CODE='" + ViewState["mlCode"].ToString() + "'");

            txtProcessName.Text = dt.Rows[0]["R_DESC"].ToString();
            chkactive.Checked = Convert.ToBoolean(dt.Rows[0]["IS_ACTIVE"].ToString());
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REMARK_MASTER", "MODIFY", "R_CODE",Convert.ToInt32( ViewState["mlCode"].ToString()));
            }
            else
            {
                txtProcessName.Enabled = false;
                chkactive.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Remark Master", "ViewRec", Ex.Message);
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

                txtProcessName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master", "GetValues", ex.Message);
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


            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master", "Setvalues", ex.Message);
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
            string StrReplaceSctorName = txtProcessName.Text;


            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            DataTable dtExist = new DataTable();



            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtExist = CommonClasses.Execute("  SELECT * FROM REMARK_MASTER WHERE ES_DELETE=0 AND R_DESC='" + txtProcessName.Text.Trim().Replace("'", "''") + "' AND R_CM_ID='" + Session["CompanyId"].ToString() + "'"); ;
                if (dtExist.Rows.Count > 0)
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    return true;
                }
                if (CommonClasses.Execute1("INSERT INTO REMARK_MASTER (R_DESC, IS_ACTIVE, R_CM_ID)VALUES ('" + txtProcessName.Text.Trim().Replace("'", "''") + "','" + chkactive.Checked + "','" + Session["CompanyId"].ToString() + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(R_CODE) from REMARK_MASTER");
                    CommonClasses.WriteLog("Remark Master", "Save", "Remark Master", txtProcessName.Text.Trim().Replace("'", "''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("../../../Account/Masters/View/ViewRemark.aspx", false);
                }
                else
                {

                    //ShowMessage("#Avisos", BL_ProcessMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);



                    txtProcessName.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtExist = CommonClasses.Execute("  SELECT * FROM REMARK_MASTER WHERE ES_DELETE=0 AND R_DESC='" + txtProcessName.Text.Trim().Replace("'", "''") + "' AND R_CODE!='" +  ViewState["mlCode"].ToString()  + "' AND R_CM_ID='" + Session["CompanyId"].ToString() + "'"); ;
                if (dtExist.Rows.Count > 0)
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    return true;
                }
                if (CommonClasses.Execute1("UPDATE REMARK_MASTER SET R_DESC ='" + txtProcessName.Text.Trim().Replace("'", "''") + "', IS_ACTIVE ='" + chkactive.Checked + "'  where R_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("REMARK_MASTER", "MODIFY", "P_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                    CommonClasses.WriteLog("Remark Master", "Update", "Remark Master", txtProcessName.Text.Trim().Replace("'", "''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("../../../Account/Masters/View/ViewRemark.aspx", false);
                }
                else
                {

                    lblmsg.Text = "Record Not save ";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtProcessName.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remark Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Remark Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    #endregion
}
