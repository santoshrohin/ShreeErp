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

public partial class Transactions_VIEW_ViewCustomerRejection : System.Web.UI.Page
{

    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            //home.Attributes["class"] = "active";
            //HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
            //home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='21'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();


                    LoadCustomerRejection();
                    //if (dgCustomerRejection.Rows.Count == 0)
                    //{
                    //    dtFilter.Clear();

                    //    if (dtFilter.Columns.Count == 0)
                    //    {
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_TYPE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_NO", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_DATE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("CR_P_CODE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    //        dtFilter.Rows.Add(dtFilter.NewRow());
                    //        dgCustomerRejection.DataSource = dtFilter;
                    //        dgCustomerRejection.DataBind();
                    //    }
                    //}
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "Page_Load", Ex.Message);
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomerRejection
    private void LoadCustomerRejection()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by CR_GIN_NO desc");
            if (dt.Rows.Count == 0)
            {
                if (dt.Rows.Count == 0)
                {
                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dgCustomerRejection.Enabled = false;
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_TYPE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_P_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgCustomerRejection.DataSource = dtFilter;
                        dgCustomerRejection.DataBind();
                    }
                }
            }
            else
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.DataSource = dt;
                dgCustomerRejection.DataBind();

            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadCustomerRejection", Ex.Message);
        }
    }
    #endregion


    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "txtString_TextChanged", Ex.Message);
        }
    }

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("select CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and (P_NAME like upper('%" + str + "%') OR convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%') OR CR_GIN_NO like upper('%" + str + "%') OR CR_CHALLAN_NO like upper('%" + str + "%') OR convert(varchar,CR_CHALLAN_DATE,106) like upper('%" + str + "%') OR CR_P_CODE like upper('%" + str + "%')) order by CR_GIN_NO desc");
            else
                dtfilter = CommonClasses.Execute("select CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by CR_GIN_NO desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.DataSource = dtfilter;
                dgCustomerRejection.DataBind();
            }
            else
            {

                dtFilter.Clear();
                dgCustomerRejection.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerRejection.DataSource = dtFilter;
                    dgCustomerRejection.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadStatus", ex.Message);
        }
    }

    #endregion

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/CustomerRejection.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Transaction", "btnAddNew_Click", Ex.Message);
        }
    }
    protected void dgCustomerRejection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerRejection.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Transaction", "dgCustomerRejection_PageIndexChanging", Ex.Message);
        }
    }
    protected void dgCustomerRejection_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string CR_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/CustomerRejection.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
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
                        string CR_CODE = e.CommandArgument.ToString();
                        int cnt = 0;
                        //DataTable dtcheck = CommonClasses.Execute("select CPOD_CR_CODE,CPOD_WO_FLAG from CUSTPO_DETAIL where CPOD_CR_CODE=" + CR_CODE + "");
                        //for (int n = 0; n < dtcheck.Rows.Count; n++)
                        //{
                        //    if (dtcheck.Rows[n]["CPOD_WO_FLAG"].ToString() == "True")
                        //    {
                        //        cnt = 1;
                        //    }
                        //    else
                        //    {
                        //        cnt = 0;
                        //    }
                        //}
                        //if (cnt == 1)
                        //{
                        //    ShowMessage("#Avisos", "Work Order for perticular Customer Order is generated, can't modify the record", CommonClasses.MSG_Erro);
                        //}
                        //else
                        //{
                        //    Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
                        //}

                        //if (CommonClasses.CheckUsedInTran("WORK_ORDER_MASTER", "WO_CR_CODE", "AND ES_DELETE=0", CR_CODE))
                        //{  PanelMsg.Visible = true;
                        // lblmsg.Text = "You can't modity this record, it is used in Work Order";

                        //    ShowMessage("#Avisos", "You can't modity this record, it is used in Work Order", CommonClasses.MSG_Warning);
                        //    return;
                        //}
                        //else
                        //{
                        Response.Redirect("~/Transactions/ADD/CustomerRejection.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);

                        //}
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
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                //if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                //{
                if (!ModifyLog(e.CommandArgument.ToString()))
                {

                    //string type = "MODIFY";
                    string CR_CODE = e.CommandArgument.ToString();
                    //string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/CustomerRejectionRPTForm.aspx?CR_CODE=" + CR_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                    return;
                }

                //}
                //else
                //{
                //    ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);
                //    return;
                //}
            }
            //if (e.CommandName.Equals("PrintWRK"))
            //{
            //    //if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            //    //{
            //    if (!ModifyLog(e.CommandArgument.ToString()))
            //    {

            //        //string type = "MODIFY";
            //        string CR_CODE = e.CommandArgument.ToString();
            //        string type1 = "wrkorder";
            //        Response.Redirect("~/RoportForms/ADD/CustomerPORPTForm.aspx?CR_CODE=" + CR_CODE + "&print_type=" + type1, false);
            //    }
            //    else
            //    {
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "Record Used By Another Person";

            //        // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
            //        return;
            //    }

            //    //}
            //    //else
            //    //{
            //    //    ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);
            //    //    return;
            //    //}
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "GridView1_RowCommand", Ex.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from CUSTREJECTION_MASTER where CR_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Rejection", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion
    protected void dgCustomerRejection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCustomerRejection.Rows[e.RowIndex].FindControl("lblCR_CODE"))).Text))
            {

                try
                {
                    string CR_CODE = ((Label)(dgCustomerRejection.Rows[e.RowIndex].FindControl("lblCR_CODE"))).Text;

                    DataTable dtRejDet = CommonClasses.Execute("SELECT CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_I_CODE,CR_INV_NO from CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER where CD_CR_CODE=CR_CODE and CD_CR_CODE='" + CR_CODE + "'");

                    if (dtRejDet.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtRejDet.Rows.Count; k++)
                        {
                            if (dtRejDet.Rows[k]["CR_INV_NO"].ToString() != "0" && dtRejDet.Rows[k]["CD_ORIGIONAL_QTY"].ToString() != "0.000")
                            {
                                double N1 = Convert.ToDouble(dtRejDet.Rows[k]["CD_ORIGIONAL_QTY"].ToString());
                                double N2 = Convert.ToDouble(dtRejDet.Rows[k]["CD_RECEIVED_QTY"].ToString());
                                double N3 = N1 - N2;
                                CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N3 + " WHERE IND_INM_CODE IN (SELECT INM_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + Convert.ToInt32(dtRejDet.Rows[k]["CR_INV_NO"].ToString()) + " and IND_I_CODE=" + dtRejDet.Rows[k]["CD_I_CODE"].ToString() + ")");

                            }
                            CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL- " + dtRejDet.Rows[k]["CD_RECEIVED_QTY"].ToString() + " WHERE I_CODE ='" + dtRejDet.Rows[k]["CD_I_CODE"].ToString() + "'  ");

                        }
                    }



                    bool flag = CommonClasses.Execute1("UPDATE CUSTREJECTION_MASTER SET ES_DELETE = 1 WHERE CR_CODE='" + Convert.ToInt32(CR_CODE) + "'");
                    if (flag == true)
                    {


                        CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(CR_CODE).ToString() + "' AND STL_DOC_TYPE='IWIC'");

                        CommonClasses.WriteLog("CUSTREJECTION_MASTER", "Delete", "CUSTREJECTION_MASTER", CR_CODE, Convert.ToInt32(CR_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }


                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("CUSTREJECTION_MASTER", "dgCustomerRejection_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                return;
            }

            LoadCustomerRejection();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            return;
        }
    }
    protected void dgCustomerRejection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string CR_CODE = ((Label)(dgCustomerRejection.Rows[e.NewEditIndex].FindControl("lblCR_CODE"))).Text;
                string type = "MODIFY";

                // if (CommonClasses.CheckUsedInTran("WORK_ORDER_MASTER", "WO_CR_CODE", "AND ES_DELETE=0", CR_CODE))
                // {
                //     PanelMsg.Visible = true;
                //     lblmsg.Text = "You Have No Rights To Delete";
                // ShowMessage("#Avisos", "You can't modify this record, it is used in Work Order", CommonClasses.MSG_Warning);
                // }
                // else
                // {
                Response.Redirect("~/Transactions/ADD/CustomerRejection.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
                //  }
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
            CommonClasses.SendError("Customer Po Transactoin", "dgCustomerMaster_RowEditing", Ex.Message);
        }
    }
}
