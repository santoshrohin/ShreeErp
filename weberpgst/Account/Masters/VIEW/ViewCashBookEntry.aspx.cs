using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_VIEW_ViewCashBookEntry : System.Web.UI.Page
{

    #region Variables
    static string right = "";
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='131'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                    //{

                    LoadState();
                    dgState.Enabled = true;
                    if (dgState.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("C_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("C_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("C_DEPOSIT", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("C_WITHDRAWAL", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("C_REMARK", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("L_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("LEDGER_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgState.DataSource = dtFilter;
                            dgState.DataBind();
                            dgState.Enabled = false;
                        }
                    }
                    //}
                    //else
                    //{
                    //    Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");

                    //}
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Account/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Cash Book Entry - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadState
    private void LoadState()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT CASH_BOOK_ENTRY.C_CODE, CASH_BOOK_ENTRY.C_NO, CONVERT(VARCHAR, C_DATE,106) AS C_DATE, CASH_BOOK_ENTRY.C_NAME, CASH_BOOK_ENTRY.C_DEPOSIT, CASH_BOOK_ENTRY.C_WITHDRAWAL, CASH_BOOK_ENTRY.C_REMARK, CASH_BOOK_ENTRY.C_L_CODE, ACCOUNT_LEDGER.L_NAME, AL.L_NAME AS LEDGER_NAME FROM ACCOUNT_LEDGER INNER JOIN CASH_BOOK_ENTRY ON ACCOUNT_LEDGER.L_CODE = CASH_BOOK_ENTRY.C_NAME INNER JOIN ACCOUNT_LEDGER AS AL ON CASH_BOOK_ENTRY.C_L_CODE = AL.L_CODE  WHERE     (CASH_BOOK_ENTRY.ES_DELETE = 0) AND  C_CM_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ACCOUNT_LEDGER.ES_DELETE=0 order by  C_CODE DESC");

            dgState.DataSource = dt;
            dgState.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "LoadState", Ex.Message);
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
            CommonClasses.SendError("Cash Book Entry", "txtString_TextChanged", Ex.Message);
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

            if (txtString.Text.Trim() != "")
                dtfilter = CommonClasses.Execute(" SELECT     CASH_BOOK_ENTRY.C_CODE, CASH_BOOK_ENTRY.C_NO, CONVERT(VARCHAR, C_DATE,106) AS C_DATE, CASH_BOOK_ENTRY.C_NAME, CASH_BOOK_ENTRY.C_DEPOSIT,                       CASH_BOOK_ENTRY.C_WITHDRAWAL, CASH_BOOK_ENTRY.C_REMARK, CASH_BOOK_ENTRY.C_L_CODE, ACCOUNT_LEDGER.L_NAME, AL.L_NAME AS LEDGER_NAME  FROM         ACCOUNT_LEDGER INNER JOIN                      CASH_BOOK_ENTRY ON ACCOUNT_LEDGER.L_CODE = CASH_BOOK_ENTRY.C_NAME INNER JOIN                      ACCOUNT_LEDGER AS AL ON CASH_BOOK_ENTRY.C_L_CODE = AL.L_CODE  WHERE     (CASH_BOOK_ENTRY.ES_DELETE = 0) AND C_CM_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ACCOUNT_LEDGER.ES_DELETE=0   and (AL.L_NAME like upper('%" + str + "%') OR C_DEPOSIT like upper('%" + str + "%')   OR C_WITHDRAWAL like upper('%" + str + "%')   OR C_REMARK like upper('%" + str + "%')  OR ACCOUNT_LEDGER.L_NAME like upper('%" + str + "%')) order by C_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT     CASH_BOOK_ENTRY.C_CODE, CASH_BOOK_ENTRY.C_NO,  CONVERT(VARCHAR, C_DATE,106) AS C_DATE, CASH_BOOK_ENTRY.C_NAME, CASH_BOOK_ENTRY.C_DEPOSIT,                       CASH_BOOK_ENTRY.C_WITHDRAWAL, CASH_BOOK_ENTRY.C_REMARK, CASH_BOOK_ENTRY.C_L_CODE, ACCOUNT_LEDGER.L_NAME, AL.L_NAME AS LEDGER_NAME  FROM         ACCOUNT_LEDGER INNER JOIN                       CASH_BOOK_ENTRY ON ACCOUNT_LEDGER.L_CODE = CASH_BOOK_ENTRY.C_NAME INNER JOIN                       ACCOUNT_LEDGER AS AL ON CASH_BOOK_ENTRY.C_L_CODE = AL.L_CODE WHERE     (CASH_BOOK_ENTRY.ES_DELETE = 0) AND C_CM_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ACCOUNT_LEDGER.ES_DELETE=0  order by C_CODE DESC");
            if (dtfilter.Rows.Count > 0)
            {
                dgState.Enabled = true;
                dgState.DataSource = dtfilter;
                dgState.DataBind();
            }
            else
            {
                dtFilter.Clear();


                if (dtFilter.Columns.Count == 0)
                {

                    dtFilter.Columns.Add(new System.Data.DataColumn("C_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("C_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("C_DEPOSIT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("C_WITHDRAWAL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("C_REMARK", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("L_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LEDGER_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgState.DataSource = dtFilter;
                    dgState.DataBind();
                    dgState.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Cash Book Entry", "LoadStatus", ex.Message);
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
                Response.Redirect("../../../Account/Masters/Add/CashBookEntry.aspx?c_name=" + type, false);

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgState_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgState.Rows[e.RowIndex].FindControl("lblL_CODE"))).Text))
                {

                    string s_code = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblL_CODE"))).Text;
                    string s_name = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblL_NAME"))).Text;

                    //if (CommonClasses.CheckUsedInTran("CITY_MASTER", "CITY_SM_CODE", "AND ES_DELETE=0", s_code))
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You can't delete this record becouse it has used in city master";

                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //    //ShowMessage("#Avisos", "You cant delete this record it has used in city master", CommonClasses.MSG_Warning);
                    //}
                    //else
                    //{
                    bool flag = CommonClasses.Execute1("UPDATE CASH_BOOK_ENTRY SET ES_DELETE = 1 WHERE C_CODE='" + Convert.ToInt32(s_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Cash Book Entry", "Delete", "Cash Book Entry", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        // ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);



                    }
                    //}
                    //}
                    LoadState();
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
            CommonClasses.SendError("Cash Book Entry", "dgState_RowDeleting", Ex.Message);
        }
    }
    protected void dgState_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string s_code = ((Label)(dgState.Rows[e.NewEditIndex].FindControl("lblL_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Account/Masters/Add/CashBookEntry.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
            CommonClasses.SendError("Cash Book Entry", "dgState_RowEditing", Ex.Message);
        }
    }
    protected void dgState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgState.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "dgBillPassing_PageIndexChanging", Ex.Message);
        }
    }
    protected void dgState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {

                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string s_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Account/Masters/Add/CashBookEntry.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
                        Response.Redirect("~/Account/Masters/Add/CashBookEntry.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string Cash_Code= e.CommandArgument.ToString();
                    Response.Redirect("~/Account/ReportForms/ADD/CashBookPrint.aspx?Title=" + Title + "&Cash_Code=" + Cash_Code, false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cash Book Entry", "dgState_RowCommand", Ex.Message);
        }
    }
    protected void dgState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            // CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ACCOUNT_LEDGER where L_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Cash Book Entry", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Cash Book Entry", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}