using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Transactions_VIEW_ViewCreditNote : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    DataTable dtFilter = new DataTable();
    static string Code = "";
    static string type = "";
    #endregion

    #region Event

    #region btnPrint_Click
    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }
    #endregion btnPrint_Click

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadCreditNote();
                    if (dgCreditNote.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CNM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CNM_SERIAL_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CNM_DATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCreditNote.DataSource = dtFilter;
                            dgCreditNote.DataBind();
                            dgCreditNote.Enabled = false;
                        }
                    }
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice View", "txtString_TextChanged", Ex.Message);
        }

    }
    #endregion

    #region  btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/CreditNote.aspx?c_name=" + type, false);
            }
            // else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "You Have No Rights To Add";
            //    return;
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgCreditNote_RowDeleting
    protected void dgCreditNote_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCreditNote.Rows[e.RowIndex].FindControl("lblCNM_CODE"))).Text))
            {
                try
                {
                    string inv_code = ((Label)(dgCreditNote.Rows[e.RowIndex].FindControl("lblCNM_CODE"))).Text;
                    string inv_no = ((Label)(dgCreditNote.Rows[e.RowIndex].FindControl("lblCNM_SERIAL_NO"))).Text;
                    string P_CODE = ((Label)(dgCreditNote.Rows[e.RowIndex].FindControl("lblP_CODE"))).Text;

                    DataTable dtcheckused = CommonClasses.Execute(" SELECT * FROM  RECIEPT_MASTER,RECIEPT_DETAIL where RCP_CODE=RCPD_RCP_CODE  AND RCPD_TYPE=0   AND RCP_P_CODE='" + P_CODE + "'  AND RCPD_INVOICE_CODE='" + inv_code + "' ORDER BY RCP_CODE DESC");
                    if (dtcheckused.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used In Transaction ,You Can Not Delete.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }



                    bool flag = CommonClasses.Execute1("UPDATE CREDIT_NOTE_MASTER SET ES_DELETE = 1 WHERE CNM_CODE='" + Convert.ToInt32(inv_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("CREDIT_NOTE_MASTER", "Delete", "CREDIT_NOTE_MASTER", inv_no, Convert.ToInt32(inv_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        //DataTable dt = CommonClasses.Execute("select isnull(CNM_IS_SUPPLIMENT,0) as CNM_IS_SUPPLIMENT from CREDIT_NOTE_MASTER where CNM_CODE='" + Convert.ToInt32(inv_code) + "'");
                        //if (dt.Rows.Count > 0)
                        //{
                        //    if (Convert.ToBoolean(dt.Rows[0]["CNM_IS_SUPPLIMENT"]))
                        //    {
                        //    }
                        //    else
                        //    {
                        //        DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + inv_code + " ");
                        //        for (int i = 0; i < dtq.Rows.Count; i++)
                        //        {
                        //            CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                        //            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                        //        }
                                flag = CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + inv_code + "' and ACCNT_DOC_TYPE='CRDENTRY'");
                         //   }
                        //}
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                    LoadCreditNote();
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Tax Invoice View", "dgCreditNote_RowDeleting", Ex.Message);
                }
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgCreditNote_RowCommand
    protected void dgCreditNote_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string inv_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/CreditNote.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
                }
                else
                {
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
                        string inv_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/CreditNote.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        //string type = rbReportType.SelectedValue.ToString();  //&rptType=" + rptType + "
                        string inv_code = e.CommandArgument.ToString();
                        Code = inv_code;
                        type = "Single";
                        Response.Redirect("~/RoportForms/ADD/CreditNotePrint.aspx?inv_code=" + inv_code + "&type=" + type, false);
                        //type = "Single";
                        //Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?inv_code=" + inv_code + "&type=" + type, false);
                        //popUpPanel5.Visible = true;
                        //ModalPopupPrintSelection.Show();
                        //return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("PrintMult"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    //string type = "MODIFY";
                    //string inv_code = e.CommandArgument.ToString();
                    //type = "Mult";
                    //Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?inv_code=" + inv_code + "&type=" + type, false);
                    //popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.Show();
                    //return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "dgCreditNote_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCreditNote_PageIndexChanging
    protected void dgCreditNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCreditNote.PageIndex = e.NewPageIndex;
            LoadCreditNote();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice View", "dgCreditNote_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    #endregion

    #region User Defined Method

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from CREDIT_NOTE_MASTER where CNM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadCreditNote
    private void LoadCreditNote()
    {
        try
        {
            DataTable dt = new DataTable();

            //dt = CommonClasses.Execute("select P_NAME,INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE from INVOICE_MASTER,PARTY_MASTER where INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE  and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_INVOICE_TYPE= '0' AND ISNULL(INM_TYPE,0)='TAXINV' order by INM_CODE DESC");
            dt = CommonClasses.Execute("select P_NAME,CNM_CODE,CNM_SERIAL_NO,convert(varchar,CNM_DATE,106) as CNM_DATE,P_CODE from CREDIT_NOTE_MASTER,PARTY_MASTER where CREDIT_NOTE_MASTER.ES_DELETE=0 and CNM_CUST_CODE=P_CODE  and CNM_CM_CODE=" + (string)Session["CompanyCode"] + " order by CNM_CODE DESC");
            dgCreditNote.DataSource = dt;
            dgCreditNote.DataBind();
            if (dgCreditNote.Rows.Count > 0)
            {
                dgCreditNote.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadCreditNote", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT CNM_CODE,CNM_SERIAL_NO,convert(varchar,CNM_DATE,106) as CNM_DATE,P_NAME,P_CODE FROM CREDIT_NOTE_MASTER,PARTY_MASTER WHERE CNM_CUST_CODE=P_CODE and CNM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND CREDIT_NOTE_MASTER.ES_DELETE='0' and (CNM_SERIAL_NO like upper('%" + str + "%') OR convert(varchar,CNM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by CNM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT CNM_CODE,CNM_SERIAL_NO,convert(varchar,CNM_DATE,106) as CNM_DATE,P_NAME,P_CODE FROM CREDIT_NOTE_MASTER,PARTY_MASTER WHERE CNM_CUST_CODE=P_CODE and CNM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND CREDIT_NOTE_MASTER.ES_DELETE='0' order by CNM_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgCreditNote.DataSource = dtfilter;
                dgCreditNote.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CNM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CNM_SERIAL_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CNM_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCreditNote.DataSource = dtFilter;
                    dgCreditNote.DataBind();
                    //dgCreditNote.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #endregion

    #region ddlPrintOpt_SelectedIndexChanged_Commented
    //protected void ddlPrintOpt_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //if (ddlPrintOpt.SelectedValue == "1")
    //    //{
    //    ModalPopupPrintSelection.Show();
    //    lblPrintCopy.Visible = true;
    //    chkPrintCopy1.Visible = true;
    //    chkPrintCopy2.Visible = true;
    //    chkPrintCopy3.Visible = true;
    //    return;
    //    //}
    //    //if (ddlPrintOpt.SelectedValue == "2")
    //    //{
    //    //    ModalPopupPrintSelection.Show();
    //    //    lblPrintCopy.Visible = false;
    //    //    chkPrintCopy1.Visible = false;
    //    //    chkPrintCopy2.Visible = false;
    //    //    chkPrintCopy3.Visible = false;
    //    //    chkPrintCopy4.Visible = false;
    //    //    return;
    //    //}
    //    //else
    //    //{

    //    //}
    //}
    //#endregion ddlPrintOpt_SelectedIndexChanged

    //#region chk1_CheckedChanged
    //protected void chk1_CheckedChanged(object sender, EventArgs e)
    //{
    //    ModalPopupPrintSelection.Show();
    //}
    #endregion chk1_CheckedChanged

    #region btnOk_Click_Commented
    //protected void btnOk_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlPrintOpt.SelectedValue == "0")
    //        {
    //            PanelMsg.Visible = true;
    //            lblmsg.Text = "Please Select print Option";
    //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
    //            ModalPopupPrintSelection.Show();
    //            return;
    //        }
    //        if (chkPrintCopy1.Checked == true)
    //        {
    //        }
    //        else if (chkPrintCopy2.Checked == true)
    //        {
    //        }
    //        else if (chkPrintCopy3.Checked == true)
    //        {
    //        }
    //        else if (chkPrintCopy4.Checked == true)
    //        {
    //        }
    //        else
    //        {
    //            PanelMsg.Visible = true;
    //            lblmsg.Text = "Please Select Print No. Of Copies";
    //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
    //            ModalPopupPrintSelection.Show();
    //            return;
    //        }

    //        //if (ddlPrintOpt.SelectedValue != "0")
    //        //{
    //        //    if (ddlPrintOpt.SelectedValue == "2")
    //        //    {
    //        //        Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&code=" + Code + "&type=" + type + "", false);
    //        //    }
    //        //}
    //        //if (ddlPrintOpt.SelectedValue == "1")
    //        //{
    //        if (chkPrintCopy1.Checked == true)
    //        {
    //            Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + chkPrintCopy1.Text + "&code=" + Code + "&type=" + type + "", false);
    //        }
    //        if (chkPrintCopy2.Checked == true)
    //        {
    //            Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint2=" + chkPrintCopy2.Text + "&code=" + Code + "&type=" + type + "", false);
    //        }
    //        if (chkPrintCopy3.Checked == true)
    //        {
    //            Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint3=" + chkPrintCopy3.Text + "&code=" + Code + "&type=" + type + "", false);
    //        }
    //        if (chkPrintCopy4.Checked == true)
    //        {
    //            Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint4=" + chkPrintCopy4.Text + "&code=" + Code + "&type=" + type + "", false);
    //        }
    //        //}
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Invoice Print ", "btnOk_Click", Ex.Message);
    //    }
    //}
    #endregion
}
