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
public partial class Transactions_VIEW_ViewCustomerSchedule : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='131'");
                    //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadSchedule();
                    if (dgCustomerSchedule.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CS_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CS_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CS_MONTH", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCustomerSchedule.DataSource = dtFilter;
                            dgCustomerSchedule.DataBind();
                            dgCustomerSchedule.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
            //txtString.Focus();
            //dgCustomerSchedule.Focus();
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule", "txtString_TextChanged", Ex.Message);
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
                Response.Redirect("~/Transactions/ADD/CustomerSchedule.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Customer Schedule", "btnAddNew_Click", Ex.Message);
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

    #region dgCustomerSchedule_RowDeleting
    protected void dgCustomerSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblCS_CODE"))).Text))
            {

                try
                {
                    string inv_code = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblCS_CODE"))).Text;
                    string inv_no = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblINM_NO"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE INVOICE_MASTER SET ES_DELETE = 1 WHERE CS_CODE='" + Convert.ToInt32(inv_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Tax Invoice", "Delete", "Tax Invoice", inv_no, Convert.ToInt32(inv_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        DataTable dt = CommonClasses.Execute("select isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT from INVOICE_MASTER where CS_CODE='" + Convert.ToInt32(inv_code) + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["INM_IS_SUPPLIMENT"]))
                            {

                            }
                            else
                            {
                                DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_CS_CODE=" + inv_code + " ");
                                for (int i = 0; i < dtq.Rows.Count; i++)
                                {
                                    CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");

                                }
                                flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + inv_code + "' and STL_DOC_TYPE='TAXINV'");
                            }
                        }

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                    LoadSchedule();
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Customer Schedule", "dgCustomerSchedule_RowDeleting", Ex.Message);
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

    #region dgCustomerSchedule_RowCommand
    protected void dgCustomerSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Response.Redirect("~/Transactions/ADD/CustomerSchedule.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                        Response.Redirect("~/Transactions/ADD/CustomerSchedule.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                        //Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?inv_code=" + inv_code + "&type=" + type, false);
                        popUpPanel5.Visible = true;
                        ModalPopupPrintSelection.Show();
                        return;
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
                    string inv_code = e.CommandArgument.ToString();
                    type = "Mult";
                    //Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?inv_code=" + inv_code + "&type=" + type, false);
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule", "dgCustomerSchedule_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_PageIndexChanging
    protected void dgCustomerSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerSchedule.PageIndex = e.NewPageIndex;
            LoadSchedule();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Schedule", "dgCustomerSchedule_PageIndexChanging", ex.Message);
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
            DataTable dt = CommonClasses.Execute("select MODIFY from INVOICE_MASTER where CS_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Schedule", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Customer Schedule", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadSchedule
    private void LoadSchedule()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select P_NAME,CS.CS_CODE,CS.CS_NO,convert(varchar,CS.CS_MONTH,106) as CS_MONTH from CUSTOMER_SCHEDULE CS,PARTY_MASTER where CS.ES_DELETE=0 and CS.CS_P_CODE=P_CODE  and CS.CS_CM_CODE='" + (string)Session["CompanyCode"] + "' order by CS_CODE DESC");
            dgCustomerSchedule.DataSource = dt;
            dgCustomerSchedule.DataBind();
            if (dgCustomerSchedule.Rows.Count > 0)
            {
                dgCustomerSchedule.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule", "LoadSchedule", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT CS_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and (INM_NO like upper('%" + str + "%') OR convert(varchar,INM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) and INM_INVOICE_TYPE ='0' AND ISNULL(INM_TYPE,0)='TAXINV' order by CS_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT CS_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and INM_INVOICE_TYPE = '0' AND ISNULL(INM_TYPE,0)='TAXINV' order by CS_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerSchedule.DataSource = dtfilter;
                dgCustomerSchedule.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CS_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CS_MONTH", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                    //dgCustomerSchedule.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Schedule", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #endregion

    #region ddlPrintOpt_SelectedIndexChanged
    protected void ddlPrintOpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlPrintOpt.SelectedValue == "1")
        //{
        ModalPopupPrintSelection.Show();
        lblPrintCopy.Visible = true;
        chkPrintCopy1.Visible = true;
        chkPrintCopy2.Visible = true;
        chkPrintCopy3.Visible = true;
        chkPrintCopy4.Visible = true;
        return;
        //}
        //if (ddlPrintOpt.SelectedValue == "2")
        //{
        //    ModalPopupPrintSelection.Show();
        //    lblPrintCopy.Visible = false;
        //    chkPrintCopy1.Visible = false;
        //    chkPrintCopy2.Visible = false;
        //    chkPrintCopy3.Visible = false;
        //    chkPrintCopy4.Visible = false;
        //    return;
        //}
        //else
        //{

        //}
    }
    #endregion ddlPrintOpt_SelectedIndexChanged

    #region chk1_CheckedChanged
    protected void chk1_CheckedChanged(object sender, EventArgs e)
    {
        ModalPopupPrintSelection.Show();
    }
    #endregion chk1_CheckedChanged

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPrintOpt.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select print Option";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ModalPopupPrintSelection.Show();
                return;
            }
            if (chkPrintCopy1.Checked == true)
            {
            }
            else if (chkPrintCopy2.Checked == true)
            {
            }
            else if (chkPrintCopy3.Checked == true)
            {
            }
            else if (chkPrintCopy4.Checked == true)
            {
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Print No. Of Copies";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ModalPopupPrintSelection.Show();
                return;
            }

            //if (ddlPrintOpt.SelectedValue != "0")
            //{
            //    if (ddlPrintOpt.SelectedValue == "2")
            //    {
            //        Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&code=" + Code + "&type=" + type + "", false);
            //    }
            //}
            //if (ddlPrintOpt.SelectedValue == "1")
            //{
            if (type == "Single")
            {
                if (chkPrintCopy1.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + chkPrintCopy1.Text + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy2.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint2=" + chkPrintCopy2.Text + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy3.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint3=" + chkPrintCopy3.Text + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy4.Checked == true)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerSchedulePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint4=" + chkPrintCopy4.Text + "&code=" + Code + "&type=" + type + "", false);
                }
            }
            else
            {
                if (chkPrintCopy1.Checked == true)
                {
                    //Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?inv_code=" + inv_code + "&type=" + type, false);
                    Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + 1 + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy2.Checked == true)
                {
                    Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + 2 + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy3.Checked == true)
                {
                    Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + 3 + "&code=" + Code + "&type=" + type + "", false);
                }
                if (chkPrintCopy4.Checked == true)
                {
                    Response.Redirect("~/RoportForms/VIEW/ViewInvoiceReport.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint1=" + 4 + "&code=" + Code + "&type=" + type + "", false);
                }
            }
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Invoice Print ", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

}
