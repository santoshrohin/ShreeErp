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

public partial class Account_ADD_AccountLedgerMaster : System.Web.UI.Page
{
    #region General Declaration
    StateMaster_BL BL_StateMaster = null;
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='130'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadCountry();
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
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Account Ledger Master", "Page_Load", Ex.Message);
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
    //    txtStateName.Text = "";
    //    ddlACType.SelectedIndex = 0;
    //    if (mlCode != 0 && mlCode != null)
    //    {
    //        CommonClasses.RemoveModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
    //    }
    //    Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
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
            CommonClasses.SendError("Account Ledger Master", "btnCancel_Click", ex.Message.ToString());
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
            if (txtLedgerName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtOpeningBal.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlACType.SelectedIndex <= 0)
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
            CommonClasses.SendError("Account Ledger Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    private void CancelRecord()
    {
        if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
        {
            CommonClasses.RemoveModifyLock("ACCOUNT_LEDGER", "MODIFY", "L_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
        }
        
        Response.Redirect("../../../Account/Masters/View/ViewAccountLedgerMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtLedgerName.Text = "";
            txtOpeningBal.Text = "0.00";
            ddlACType.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Account Ledger Master", "ClearFied", Ex.Message);
        }
    }
    #endregion


    //newly Added 


    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                //dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID='" + (string)Session["CompanyId"] + "'");
                //ddlACType.DataSource = dt;
                //ddlACType.DataTextField = "COUNTRY_NAME";
                //ddlACType.DataValueField = "COUNTRY_CODE";
                //ddlACType.DataBind();
                //ddlACType.Items.Insert(0, new ListItem("Select Country", "0"));

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
            CommonClasses.SendError("Account Ledger Master", "LoadCountry", Ex.Message);

        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {


        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM ACCOUNT_LEDGER where L_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
            if (dt.Rows.Count > 0)
            {
                txtLedgerName.Text = dt.Rows[0]["L_NAME"].ToString();
                ddlACType.SelectedValue = dt.Rows[0]["L_ACCTYPE"].ToString();
                txtOpeningBal.Text = dt.Rows[0]["L_OPBAL"].ToString();

            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ACCOUNT_LEDGER", "MODIFY", "L_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Account Ledger Master", "ViewRec", Ex.Message);
        }

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceLedgerName = txtLedgerName.Text.Trim();


            StrReplaceLedgerName = StrReplaceLedgerName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dtExist = CommonClasses.Execute(" SELECT * FROM ACCOUNT_LEDGER where  L_NAME='" + StrReplaceLedgerName + "' AND ES_DELETE=0 "); ;
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtLedgerName.Focus();
                    return true;
                }

                if (CommonClasses.Execute1("INSERT INTO ACCOUNT_LEDGER (L_CM_ID, L_NAME, L_ACCTYPE, L_OPBAL)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + StrReplaceLedgerName + "','" + ddlACType.SelectedValue.ToString() + "','" + txtOpeningBal.Text + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(L_CODE) from ACCOUNT_LEDGER");

                    CommonClasses.WriteLog("Account Ledger Master", "Save", "Account Ledger Master", StrReplaceLedgerName, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    
                    Response.Redirect("../../../Account/Masters/View/ViewAccountLedgerMaster.aspx", false);
                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtLedgerName.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                DataTable dtExist = CommonClasses.Execute(" SELECT * FROM ACCOUNT_LEDGER where  L_NAME='" + StrReplaceLedgerName + "' AND  L_CODE!='" + ViewState["mlCode"].ToString() + "' AND ES_DELETE=0 "); ;
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtLedgerName.Focus();
                    return true;
                }


                if (CommonClasses.Execute1("UPDATE    ACCOUNT_LEDGER SET L_NAME ='" + StrReplaceLedgerName + "', L_ACCTYPE ='" + ddlACType.SelectedValue + "', L_OPBAL ='" + txtOpeningBal.Text + "' WHERE     (L_CODE = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "')      "))
                {
                    CommonClasses.RemoveModifyLock("ACCOUNT_LEDGER", "MODIFY", "L_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                    CommonClasses.WriteLog("Account Ledger Master", "Update", "Account Ledger Master", StrReplaceLedgerName, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    
                    Response.Redirect("../../../Account/Masters/View/ViewAccountLedgerMaster.aspx", false);
                }
                else
                {

                }


            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Account Ledger Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Account Ledger Master", "ShowMessage", Ex.Message);
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

                ddlACType.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Account Ledger Master", "GetValues", ex.Message);
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
            CommonClasses.SendError("Account Ledger Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion


    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
        {
            CommonClasses.RemoveModifyLock("ACCOUNT_LEDGER", "MODIFY", "L_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
        }
        
        Response.Redirect("../../../Account/Masters/View/ViewAccountLedgerMaster.aspx", false);
    }
    #endregion


}