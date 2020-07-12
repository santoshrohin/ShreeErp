using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ADD_CashBookEntry : System.Web.UI.Page
{

    #region General Declaration
    static int mlCode = 0;
    static string right = "";
    static string ViewURL = "../../../Account/Masters/View/ViewCashBookEntry.aspx";
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='131'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadCountry();
                ViewState["mlCode"] = mlCode;
                txtDate.Attributes.Add("readonly", "readonly");
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
                else
                {
                    txtDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                    txtDate.Attributes.Add("readonly", "readonly");
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Cash Book Entry", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Cash Book Entry", "btnCancel_Click", ex.Message.ToString());
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
            if (txtWithdrawal.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtDeposit.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtRemark.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlBank.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (dllLedger.SelectedIndex == 0)
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
            CommonClasses.SendError("Cash Book Entry", "CheckValid", Ex.Message);
        }

        return flag;
    }

    private void CancelRecord()
    {
        if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
        {
            CommonClasses.RemoveModifyLock("CASH_BOOK_ENTRY", "MODIFY", "C_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
        }
        Response.Redirect(ViewURL, false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtWithdrawal.Text = "0.00";
            txtDeposit.Text = "0.00";
            ddlBank.SelectedIndex = -1;
            dllLedger.SelectedIndex = -1;
            txtRemark.Text = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "ClearFied", Ex.Message);
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
                dt = CommonClasses.Execute("select L_CODE,L_NAME from ACCOUNT_LEDGER where ES_DELETE=0 and L_CM_ID='" + (string)Session["CompanyId"] + "'");
                ddlBank.DataSource = dt;
                ddlBank.DataTextField = "L_NAME";
                ddlBank.DataValueField = "L_CODE";
                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new ListItem("Select Ledger", "0"));

                dllLedger.DataSource = dt;
                dllLedger.DataTextField = "L_NAME";
                dllLedger.DataValueField = "L_CODE";
                dllLedger.DataBind();
                dllLedger.Items.Insert(0, new ListItem("Select Ledger", "0"));

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
            CommonClasses.SendError("Cash Book Entry", "LoadCountry", Ex.Message);

        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM CASH_BOOK_ENTRY where C_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
            if (dt.Rows.Count > 0)
            {
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["C_DATE"].ToString()).ToString("dd/MMM/yyyy");
                ddlBank.SelectedValue = dt.Rows[0]["C_NAME"].ToString();
                txtDeposit.Text = dt.Rows[0]["C_DEPOSIT"].ToString();
                dllLedger.SelectedValue = dt.Rows[0]["C_L_CODE"].ToString();
                txtWithdrawal.Text = dt.Rows[0]["C_WITHDRAWAL"].ToString();
                txtRemark.Text = dt.Rows[0]["C_REMARK"].ToString();
                txtOn_Account.Text = dt.Rows[0]["C_ON_ACC"].ToString();
                txtCheque_No.Text = dt.Rows[0]["C_CHEQUE_NO"].ToString();
            }
            if (str == "VIEW")
            {
                txtWithdrawal.Enabled = false;
                txtDeposit.Enabled = false;
                txtRemark.Enabled = false;
                txtDate.Enabled = false;
                ddlBank.Enabled = false;
                dllLedger.Enabled = false;
                btnSubmit.Visible = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CASH_BOOK_ENTRY", "MODIFY", "C_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "ViewRec", Ex.Message);
        }

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceReason = txtRemark.Text.Trim();


            StrReplaceReason = StrReplaceReason.Replace("'", "''");

            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                //DataTable dtExist = CommonClasses.Execute(" SELECT * FROM ACCOUNT_LEDGER where  L_NAME='" + StrReplaceLedgerName + "' AND ES_DELETE=0 "); ;
                //if (dtExist.Rows.Count > 0)
                //{
                //    ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    txtLedgerName.Focus();
                //    return true;
                //}

                if (CommonClasses.Execute1("INSERT INTO CASH_BOOK_ENTRY (C_CM_ID,  C_DATE, C_NAME, C_DEPOSIT, C_WITHDRAWAL, C_REMARK, C_L_CODE,C_ON_ACC,C_CHEQUE_NO) VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlBank.SelectedValue.ToString() + "','" + txtDeposit.Text + "','" + txtWithdrawal.Text + "','" + StrReplaceReason + "','" + dllLedger.SelectedValue + "','" + txtOn_Account.Text.Trim() + "','" + txtCheque_No.Text.Trim() + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(C_CODE) from CASH_BOOK_ENTRY");
                    CommonClasses.WriteLog("Cash Book Entry", "Save", "Cash Book Entry", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect(ViewURL, false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtDate.Focus();
                }
            }
            #endregion Insert

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE CASH_BOOK_ENTRY SET C_DATE ='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "', C_NAME ='" + ddlBank.SelectedValue + "', C_DEPOSIT ='" + txtDeposit.Text + "', C_WITHDRAWAL ='" + txtWithdrawal.Text + "' , C_REMARK ='" + StrReplaceReason + "', C_L_CODE ='" + dllLedger.SelectedValue + "' , C_ON_ACC='" + txtOn_Account.Text.Trim() + "', C_CHEQUE_NO='" + txtCheque_No.Text.Trim() + "' WHERE     (C_CODE = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "')      "))
                {
                    CommonClasses.RemoveModifyLock("CASH_BOOK_ENTRY", "MODIFY", "C_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                    CommonClasses.WriteLog("Cash Book Entry", "Update", "Cash Book Entry", ViewState["mlCode"].ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect(ViewURL, false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtDate.Focus();
                }
            }
            #endregion Modify
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Cash Book Entry", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Cash Book Entry", "ShowMessage", Ex.Message);
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
                txtWithdrawal.Enabled = false;
                txtDeposit.Enabled = false;
                txtRemark.Enabled = false;
                txtDate.Enabled = false;
                ddlBank.Enabled = false;
                dllLedger.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Cash Book Entry", "GetValues", ex.Message);
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
            CommonClasses.SendError("Cash Book Entry", "Setvalues", ex.Message);
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
        Response.Redirect(ViewURL, false);
    }
    #endregion



}