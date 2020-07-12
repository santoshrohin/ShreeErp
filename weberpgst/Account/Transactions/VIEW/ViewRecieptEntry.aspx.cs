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

public partial class Account_Transactions_VIEW_ViewRecieptEntry : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string redirect = "~/Account/Transactions/VIEW/ViewRecieptEntry.aspx";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    static string AddRecipetURL = "../../../Account/Transactions/Add/RecieptEntry.aspx";

    #endregion

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadInward();

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Recipet Payment", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region LoadInward
    private void LoadInward()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select RCP_CODE,RCP_P_CODE,RCP_NO,convert(varchar,RCP_DATE,106) as RCP_DATE,RCP_CHEQUE_NO,P_NAME,RCP_AMOUNT from RECIEPT_MASTER,PARTY_MASTER where RECIEPT_MASTER.ES_DELETE=0 and P_CODE=RCP_P_CODE  and  RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by RCP_CODE desc ");

            if (dt.Rows.Count == 0)
            {
                if (dgDetailPO.Rows.Count == 1)
                {
                    Response.Redirect(redirect, false);
                }
                if (dgDetailPO.Rows.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_P_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_CHEQUE_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("RCP_AMOUNT", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailPO.DataSource = dtFilter;
                        dgDetailPO.DataBind();
                    }

                }
            }
            else
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dt;
                dgDetailPO.DataBind();
            }
            //for (int i = 0; i < dgDetailPO.Rows.Count; i++)
            //{ 
            //    if(dgDetailPO.Rows[i].
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Recipet Payment", "LoadRECIEPT", Ex.Message);
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
            CommonClasses.SendError("Recipet Payment", "txtString_TextChanged", Ex.Message);
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
            {
                //    dtfilter = CommonClasses.Execute("SELECT RCP_CODE,RCP_P_CODE,RCP_NO,convert(varchar,RCP_DATE,106) as RCP_DATE,RCP_CHEQUE_NO,convert(varchar,RCP_CHEQUE_DATE,106) as RCP_CHEQUE_DATE,P_NAME FROM RECIEPT_MASTER,LEDGER_MASTER WHERE RECIEPT_MASTER.ES_DELETE=0 and LM_CODE=RCP_P_CODE and RECIEPT_MASTER.RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND RECIEPT_MASTER.ES_DELETE='0' and ((upper(RCP_NO) like upper('%" + str + "%') OR CONVERT(VARCHAR, RCP_DATE, 106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(RCP_CHEQUE_NO) like upper('%" + str + "%') OR upper(RCP_CHEQUE_DATE) like upper('%" + str + "%') OR upper(RCP_P_CODE) like upper('%" + str + "%')))order by RCP_CODE desc");
                dtfilter = CommonClasses.Execute("SELECT RCP_CODE,RCP_P_CODE,RCP_NO,convert(varchar,RCP_DATE,106) as RCP_DATE,RCP_CHEQUE_NO,convert(varchar,RCP_CHEQUE_DATE,106) as RCP_CHEQUE_DATE,P_NAME,RCP_AMOUNT FROM RECIEPT_MASTER,PARTY_MASTER WHERE RECIEPT_MASTER.ES_DELETE=0 and P_CODE=RCP_P_CODE and RECIEPT_MASTER.RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND RECIEPT_MASTER.ES_DELETE='0' and (RCP_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, RCP_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR RCP_CHEQUE_NO like upper('%" + str + "%') OR RCP_CHEQUE_DATE like upper('%" + str + "%') OR RCP_P_CODE like upper('%" + str + "%')) order by RCP_CODE desc");
            }
            else
            {
                //dtfilter = CommonClasses.Execute("select RCP_CODE,RCP_P_CODE,RCP_NO,convert(varchar,RCP_DATE,106) as RCP_DATE,RCP_CHEQUE_NO,convert(varchar,RCP_CHEQUE_DATE,106) as RCP_CHEQUE_DATE,P_NAME FROM RECIEPT_MASTER,LEDGER_MASTER where RECIEPT_MASTER.ES_DELETE=0 and LM_CODE=RCP_P_CODE and RECIEPT_MASTER.RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND RECIEPT_MASTER.ES_DELETE='0' order by RCP_CODE desc ");
                dtfilter = CommonClasses.Execute("select RCP_CODE,RCP_P_CODE,RCP_NO,convert(varchar,RCP_DATE,106) as RCP_DATE,RCP_CHEQUE_NO,convert(varchar,RCP_CHEQUE_DATE,106) as RCP_CHEQUE_DATE,P_NAME,RCP_AMOUNT FROM RECIEPT_MASTER,PARTY_MASTER where RECIEPT_MASTER.ES_DELETE=0 and P_CODE=RCP_P_CODE and RECIEPT_MASTER.RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND RECIEPT_MASTER.ES_DELETE='0' order by RCP_CODE desc ");
            }
            if (dtfilter.Rows.Count > 0)
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dtfilter;
                dgDetailPO.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_CHEQUE_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RCP_AMOUNT", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Recipet Payment", "LoadStatus", ex.Message);
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
                Response.Redirect(AddRecipetURL + "?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "you have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Recipet Payment", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblRCP_CODE"))).Text))
            {

                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblRCP_CODE"))).Text;

                    //if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_RCP_CODE", "AND ES_DELETE=0", cpom_code))
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You can't delete this record, it's Inspection is already done";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //    return;
                    //}
                    //else
                    //{

                    bool flag = CommonClasses.Execute1("UPDATE RECIEPT_MASTER SET ES_DELETE = 1 WHERE RCP_CODE='" + Convert.ToInt32(cpom_code) + "'");
                    if (flag == true)
                    {
                        DataTable dtq = CommonClasses.Execute("SELECT RCPD_CODE,RCPD_TYPE,RCPD_RCP_CODE,RCPD_REF_CODE,RCPD_INVOICE_CODE,RCPD_AMOUNT,RCPD_ADJ_AMOUNT FROM RECIEPT_DETAIL where RCPD_RCP_CODE=" + cpom_code + " ");

                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                            if (dtq.Rows[i]["RCPD_TYPE"].ToString() == "1")
                            {
                                CommonClasses.Execute("update INVOICE_MASTER set INM_RECIEVED_AMT=ISNULL(INM_RECIEVED_AMT,0)-" + dtq.Rows[i]["RCPD_AMOUNT"] + " where INM_CODE='" + dtq.Rows[i]["RCPD_INVOICE_CODE"] + "'");
                            }
                            if (dtq.Rows[i]["RCPD_TYPE"].ToString() == "0")
                            {
                                CommonClasses.Execute("update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)-" + dtq.Rows[i]["RCPD_AMOUNT"] + " where CNM_CODE='" + dtq.Rows[i]["RCPD_INVOICE_CODE"] + "'");
                            }
                            if (dtq.Rows[i]["RCPD_TYPE"].ToString() == "2")
                            {
                                CommonClasses.Execute("update DEBIT_NOTE_MASTER set DNM_PAID_AMT=ISNULL(DNM_PAID_AMT,0)-" + dtq.Rows[i]["RCPD_AMOUNT"] + " where DNM_CODE='" + dtq.Rows[i]["RCPD_INVOICE_CODE"] + "'");
                            }

                            if (dtq.Rows[i]["RCPD_TYPE"].ToString() == "3")
                            {
                                CommonClasses.Execute("update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)-" + dtq.Rows[i]["RCPD_AMOUNT"] + " where BPM_CODE='" + dtq.Rows[i]["RCPD_INVOICE_CODE"] + "'");
                            }

                        }
                        flag = CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + cpom_code + "' and ACCNT_DOC_TYPE='RECIEPTENTRY'");
                        flag = CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + cpom_code + "'   and ACCNT_DOC_TYPE='ADJRECIEPTENTRY'");
                        LoadInward();
                        CommonClasses.WriteLog("Recipet Payment ", "Delete", "Recipet Payment", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }


                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Recipet Payment", "dgDetailPO_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }


            LoadInward();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You have no rights to delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            return;
        }
    }
    #endregion

    #region dgDetailPO_RowEditing
    protected void dgDetailPO_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string cpom_code = ((Label)(dgDetailPO.Rows[e.NewEditIndex].FindControl("lblRCP_CODE"))).Text;
                string type = "MODIFY";

                if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_RCP_CODE", "AND ES_DELETE=0", cpom_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Material Inspection ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
                else
                {
                    Response.Redirect(AddRecipetURL + "?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                // ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Recipet Payment", "dgPODetail_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowCommand
    protected void dgDetailPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect(AddRecipetURL + "?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
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
                        string cpom_code = e.CommandArgument.ToString();

                        if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_RCP_CODE", "AND ES_DELETE=0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Inspection is already done";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                        else
                        {
                            Response.Redirect(AddRecipetURL + "?c_name=" + type + "&cpom_code=" + cpom_code, false);

                        }

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            else if (e.CommandName.Equals("Delete"))
            {

            }
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string Type = "IWIM";
                    string PrintType = "Single";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Account/ReportForms/ADD/RecieptReport.aspx?Title=" + Title + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            else if (e.CommandName.Equals("PrintMult"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string Type = "IWIM";
                    string PrintType = "Mult";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/RoportForms/VIEW/ViewInwardReports.aspx?Title=" + Title + "&cpom_code=" + cpom_code + "&Type=" + Type + "&PType=" + PrintType, false);
                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);

                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Recipet Payment-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from RECIEPT_MASTER where RCP_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("RECIEPT Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Recipet Payment -View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgDetailPO_PageIndexChanging
    protected void dgDetailPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailPO.PageIndex = e.NewPageIndex;
            LoadInward();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Masters/ADD/AccountDefault.aspx", false);

    }
}
