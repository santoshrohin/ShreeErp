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

public partial class Transactions_VIEW_ViewPDIDetails : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='125'");
                    //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadInvoice();
                    if (dgInvoiceDettail.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgInvoiceDettail.DataSource = dtFilter;
                            dgInvoiceDettail.DataBind();
                            dgInvoiceDettail.Enabled = false;
                        }
                    }
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "Page_Load", Ex.Message);
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
            //dgInvoiceDettail.Focus();
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "txtString_TextChanged", Ex.Message);
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
                Response.Redirect("~/Transactions/ADD/PDIDetails.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("INSPECTION PDI DETAIL", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceDettail_RowDeleting
    protected void dgInvoiceDettail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINSPDI_CODE"))).Text))
            {

                try
                {
                    string inv_code = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINSPDI_CODE"))).Text;
                    string inv_no = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_NO"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE INSPECTION_PDI_DETAIL SET ES_DELETE = 1 WHERE INSPDI_INSM_CODE='" + Convert.ToInt32(inv_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("INSPECTION PDI DETAIL", "Delete", "INSPECTION PDI DETAIL", inv_no, Convert.ToInt32(inv_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                    LoadInvoice();
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("INSPECTION PDI DETAIL View", "dgInvoiceDettail_RowDeleting", Ex.Message);
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

    #region dgInvoiceDettail_RowCommand
    protected void dgInvoiceDettail_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Response.Redirect("~/Transactions/ADD/PDIDetails.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                        Response.Redirect("~/Transactions/ADD/PDIDetails.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                int Type = 1;
                int index = Convert.ToInt32(e.CommandArgument.ToString());
               // string INSPDI_CODE = ((Label)(dgInvoiceDettail.Rows[index].FindControl("lblINSPDI_CODE"))).Text;//suja Invoice No. code
                Response.Redirect("~/RoportForms/ADD/PDIInspectionPrint.aspx?Type=" + Type + "&inv_code=" + index + "&IWM_CODE=1", false);
            }
            #endregion  
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "dgInvoiceDettail_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceDettail_PageIndexChanging
    protected void dgInvoiceDettail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInvoiceDettail.PageIndex = e.NewPageIndex;
            LoadInvoice();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "dgInvoiceDettail_PageIndexChanging", ex.Message);
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
            DataTable dt = CommonClasses.Execute("select MODIFY from INSPECTION_PDI_DETAIL where INSPDI_INSM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadInvoice
    private void LoadInvoice()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select DISTINCT P_NAME,INSPDI_INSM_CODE,INM_NO from INSPECTION_PDI_DETAIL,INVOICE_MASTER,PARTY_MASTER where INM_CODE=INSPDI_INSM_CODE and INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE and INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_INSM_CODE=INM_CODE order by INM_NO desc");
            dgInvoiceDettail.DataSource = dt;
            dgInvoiceDettail.DataBind();
            if (dgInvoiceDettail.Rows.Count > 0)
            {
                dgInvoiceDettail.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "LoadInvoice", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select DISTINCT P_NAME,INSPDI_INSM_CODE,INM_NO from INSPECTION_PDI_DETAIL,INVOICE_MASTER,PARTY_MASTER where INM_CODE=INSPDI_INSM_CODE and INSPDI_I_CODE <0 and INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_INSM_CODE=INM_CODE and (INM_NO like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by INM_NO desc");
            else
                dtfilter = CommonClasses.Execute("select DISTINCT P_NAME,INSPDI_INSM_CODE,INM_NO from INSPECTION_PDI_DETAIL,INVOICE_MASTER,PARTY_MASTER where INM_CODE=INSPDI_INSM_CODE and INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE and INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_INSM_CODE=INM_CODE order by INM_NO desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgInvoiceDettail.DataSource = dtfilter;
                dgInvoiceDettail.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                   
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgInvoiceDettail.DataSource = dtFilter;
                    dgInvoiceDettail.DataBind();
                    //dgInvoiceDettail.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("INSPECTION PDI DETAIL View", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #endregion
}