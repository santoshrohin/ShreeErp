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


public partial class Transactions_VIEW_ViewServicePurchaseOrder : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='120'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    dgDetailServicePO.Enabled = false;
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_PO_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_ORDER_QTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_INW_QTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_POST", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailServicePO.DataSource = dtFilter;
                        dgDetailServicePO.DataBind();
                    }

                    LoadServicePO();



                }
                txtString.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service PO Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadServicePO
    private void LoadServicePO()
    {
        try
        {
            DataTable dt = new DataTable();


            //dt = CommonClasses.Execute("select SRPOM_AMOUNT, P_NAME,SRPOM_CODE,SRPOM_PO_NO,convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER ,PARTY_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by SRPOM_PO_NO DESC");
            dt = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SRPOM_CODE ORDER BY SRPOM_CODE) AS rn,SRPOM_CODE,SRPOM_PONO AS SRPOM_PO_NO,P_NAME,SRPOD_I_CODE,S_NAME, convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,cast(SRPOD_ORDER_QTY as numeric(20,3)) as SRPOD_ORDER_QTY,cast(SRPOD_ORDER_QTY-SRPOD_INW_QTY as numeric(20,3)) as SRPOD_INW_QTY,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER,PARTY_MASTER,SERVICE_PO_DETAILS,SERVICE_TYPE_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and SRPOM_CODE=SRPOD_SPOM_CODE  and SRPOD_I_CODE=S_CODE and P_ACTIVE_IND=1 AND SRPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SRPOM_POTYPE=0 ) select * from RESULT where rn=1 order by SRPOM_PO_NO DESC");
            if (dt.Rows.Count > 0)
            {
                dgDetailServicePO.Enabled = true;
                dgDetailServicePO.DataSource = dt;
                dgDetailServicePO.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgDetailServicePO.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {

                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_PO_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_ORDER_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_INW_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_POST", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailServicePO.DataSource = dtFilter;
                    dgDetailServicePO.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO Transaction", "LoadServicePO", Ex.Message);
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
                Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO Transaction", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Service PO Transaction", "txtString_TextChanged", Ex.Message);
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
                //(upper(SRPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%')  OR convert(varchar,SRPOM_DATE,106) like upper('%" + str + "%') or upper(SRPOD_ORDER_QTY) like upper('%" + str + "%') or upper(SRPOD_INW_QTY) like upper('%" + str + "%'))
                // dtfilter = CommonClasses.Execute("select cast(SRPOM_AMOUNT as numeric(10,2)) as SRPOM_AMOUNT, P_NAME,SRPOM_CODE,SRPOM_PO_NO,convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER ,PARTY_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 AND SRPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and (upper(SRPOM_PO_NO) like upper('%" + str + "%') OR convert(varchar,SRPOM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR SRPOM_AMOUNT like '%" + str + "%' ) order by SRPOM_PO_NO DESC");
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SRPOM_CODE ORDER BY SRPOM_CODE) AS rn,SRPOM_CODE,SRPOM_PONO AS SRPOM_PO_NO,P_NAME,SRPOD_I_CODE,S_NAME, convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,cast(SRPOD_ORDER_QTY as numeric(20,3)) as SRPOD_ORDER_QTY,cast(SRPOD_ORDER_QTY-SRPOD_INW_QTY as numeric(20,3)) as SRPOD_INW_QTY,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER,PARTY_MASTER,SERVICE_PO_DETAILS,SERVICE_TYPE_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and SRPOM_CODE=SRPOD_SPOM_CODE  and SRPOD_I_CODE=S_CODE and P_ACTIVE_IND=1 AND SRPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + "  AND SRPOM_POTYPE=0 and (upper(SRPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(S_NAME) like upper('%" + str + "%')  OR convert(varchar,SRPOM_DATE,106) like upper('%" + str + "%') or upper(SRPOD_ORDER_QTY) like upper('%" + str + "%') or upper(SRPOD_INW_QTY) like upper('%" + str + "%'))) select * from RESULT where rn=1 order by SRPOM_PO_NO DESC");

            else
                //dtfilter = CommonClasses.Execute("select cast(SRPOM_AMOUNT as numeric(20,2)) as SRPOM_AMOUNT, P_NAME,SRPOM_CODE,SRPOM_PO_NO,convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER ,PARTY_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 AND SRPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by SRPOM_PO_NO DESC");
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SRPOM_CODE ORDER BY SRPOM_CODE) AS rn,SRPOM_CODE,SRPOM_PONO AS SRPOM_PO_NO,P_NAME,SRPOD_I_CODE,S_NAME, convert(varchar,SRPOM_DATE,106) as SRPOM_DATE,cast(SRPOD_ORDER_QTY as numeric(20,3)) as SRPOD_ORDER_QTY,cast(SRPOD_ORDER_QTY-SRPOD_INW_QTY as numeric(20,3)) as SRPOD_INW_QTY,case when SRPOM_POST =1 then 'Yes' else 'No' end as SRPOM_POST from SERVICE_PO_MASTER,PARTY_MASTER,SERVICE_PO_DETAILS,SERVICE_TYPE_MASTER where SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_P_CODE=P_CODE and P_TYPE=2 and SRPOM_CODE=SRPOD_SPOM_CODE  and SRPOD_I_CODE=S_CODE and P_ACTIVE_IND=1 AND  SRPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SRPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SRPOM_POTYPE=0 ) select * from RESULT where rn=1 order by SRPOM_PO_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgDetailServicePO.Enabled = true;
                dgDetailServicePO.DataSource = dtfilter;
                dgDetailServicePO.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dgDetailServicePO.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_PO_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_ORDER_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOD_INW_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_POST", typeof(String)));


                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailServicePO.DataSource = dtFilter;
                    dgDetailServicePO.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Suppiler PO Transaction", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region dgDetailServicePO_RowEditing
    protected void dgDetailServicePO_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string cpom_code = ((Label)(dgDetailServicePO.Rows[e.NewEditIndex].FindControl("lblCPOM_CODE"))).Text;
            string type = "MODIFY";


            Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Po Transactoin", "dgDetailServicePO_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgDetailServicePO_RowDeleting
    protected void dgDetailServicePO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgDetailServicePO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text))
                {
                    string cpom_code = ((Label)(dgDetailServicePO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text;

                    /*if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "and IWD_IWM_CODE=IWM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Inward";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }*/
                    //else
                    {
                        DataTable dt = CommonClasses.Execute("select isnull(SRPOM_POST,0) as SRPOM_POST,isnull(SRPOM_P_AMEND_CODE,0) as SRPOM_P_AMEND_CODE from SERVICE_PO_MASTER where SRPOM_CODE='" + cpom_code + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            //if (Convert.ToInt32(dt.Rows[0]["SRPOM_P_AMEND_CODE"]) != 0)
                            //{
                            //    PanelMsg.Visible = true;
                            //    lblmsg.Text = "You Can't Delete Because These PO Is Amended";
                            //    return;
                            //}
                            if (Convert.ToBoolean(dt.Rows[0]["SRPOM_POST"].ToString()) == true)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You Can't Delete, Because This PO Is Posted";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                return;
                            }

                            else
                            {
                                bool flag = CommonClasses.Execute1("UPDATE SERVICE_PO_MASTER SET ES_DELETE = 1 WHERE SRPOM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                                if (flag == true)
                                {
                                    CommonClasses.WriteLog("Service Purchase Order ", "Delete", "Service Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "Record deleted successfully";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                                }
                            }
                        }
                    }
                    LoadServicePO();
                }
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

        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO Transaction", "dgDetailPO_RowDeleting", Ex.Message);
        }



    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SERVICE_PO_MASTER where SRPOM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Service PO Master-View", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region dgDetailServicePO_RowCommand
    protected void dgDetailServicePO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
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
            #endregion

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                    {
                        if (!ModifyLog(e.CommandArgument.ToString()))
                        {
                            string type = "MODIFY";
                            string cpom_code = e.CommandArgument.ToString();
                            DataTable dt = CommonClasses.Execute("select isnull(SRPOM_POST,0) as SRPOM_POST,isnull(SRPOM_P_AMEND_CODE,0) as SRPOM_P_AMEND_CODE  from SERVICE_PO_MASTER where SRPOM_CODE='" + cpom_code + "' ");
                            if (dt.Rows.Count > 0)
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["SRPOM_POST"].ToString()) == true)
                                {
                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "This PO is posted and can not be modified";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                    return;
                                }
                                //else if (Convert.ToInt32(dt.Rows[0]["SRPOM_P_AMEND_CODE"]) != 0)
                                //{
                                //    PanelMsg.Visible = true;
                                //    lblmsg.Text = "You Can't Modify Because These PO Is Amend";
                                //    return;
                                //}
                                else
                                {
                                    if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "AND IWD_CPOM_CODE=SRPOM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                                    {
                                        PanelMsg.Visible = true;
                                        lblmsg.Text = "You can't delete this record, it is used in Inward Master";
                                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                        return;
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can't Modify Because it is cancelled";
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
            #endregion

            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                    {
                        if (!ModifyLog(e.CommandArgument.ToString()))
                        {
                            string cpom_code = e.CommandArgument.ToString();
                            string PoType = "";
                            DataTable dtAuth = CommonClasses.Execute("Select SRPOM_AUTHR_FLAG,SRPOM_TYPE,PO_T_SHORT_NAME from SERVICE_PO_MASTER,PO_TYPE_MASTER  where  PO_T_CODE=SRPOM_TYPE and SRPOM_CODE='" + cpom_code + "'");
                            string AuthoFlag = "";
                            if (dtAuth.Rows[0]["SRPOM_AUTHR_FLAG"].ToString() == "False")
                            {
                                AuthoFlag = "true";
                            }
                            //if (dtAuth.Rows[0]["SRPOM_TYPE"].ToString() == "-2147483647")
                            if (dtAuth.Rows[0]["PO_T_SHORT_NAME"].ToString().Contains("IMPORT"))
                            {
                                PoType = "Export PO";
                            }
                            else
                            {
                                PoType = "Domestic PO";
                            }
                            Response.Redirect("~/RoportForms/ADD/ServiceOrderPrint.aspx?cpom_code=" + cpom_code + "&AuthoType=" + AuthoFlag + "&PoType=" + PoType, false);
                        }
                        //else
                        //{
                        //    PanelMsg.Visible = true;
                        //    lblmsg.Text = "Record used by another person";

                        //    return;
                        //}

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can't Modify Because it is cancelled";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }

            #endregion

            #region Authorize
            else if (e.CommandName.Equals("Authorize"))
            {
                if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    string qe_code = ((Label)dgDetailServicePO.Rows[index].FindControl("lblCPOM_CODE")).Text;

                    CommonClasses.Execute("update SERVICE_PO_MASTER set SRPOM_AUTHR_FLAG=1 where SRPOM_CODE=" + qe_code + "");
                    CommonClasses.WriteLog("Service Purchase Order ", "Authorize", "Service Purchase Order", qe_code, Convert.ToInt32(qe_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Can't Modify Because it is cancelled";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            #endregion

            #region Post
            else if (e.CommandName.Equals("Post"))
            {
                if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string cpom_code = e.CommandArgument.ToString();
                        if (CommonClasses.Execute1("update SERVICE_PO_MASTER set SRPOM_POST=1 where SRPOM_CODE='" + cpom_code + "'"))
                        {
                            CommonClasses.WriteLog("Service Purchase Order ", "Post", "Service Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                            LoadServicePO();
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Posted";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Posted";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                    }
                    //else
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "Record used by another person";
                    //    return;
                    //}
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Can't Modify Because it is cancelled";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion

            #region Amend
            else if (e.CommandName.Equals("Amend"))
            {
                //string cpom_code = e.CommandArgument.ToString();
                //DataTable dt = CommonClasses.Execute("select isnull(SRPOM_POST,0) as SRPOM_POST  from SERVICE_PO_MASTER where SRPOM_CODE='" + cpom_code + "' ");
                //if (dt.Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dt.Rows[0]["SRPOM_POST"].ToString()) == false)
                //    {
                //        PanelMsg.Visible = true;
                //        lblmsg.Text = "First Post This PO";
                //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //        return;
                //    }
                //    else
                //    {
                //        if (!ModifyLog(e.CommandArgument.ToString()))
                //        {
                //            string type = "AMEND";
                //            Session["AMEND"] = "AMEND";
                //            Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                //        }
                //        //else
                //        //{
                //        //    PanelMsg.Visible = true;
                //        //    lblmsg.Text = "Record used by another person";
                //        //    return;
                //        //}
                //    }
                //}
                if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                {
                    string cpom_code = e.CommandArgument.ToString();
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        DataTable dt = CommonClasses.Execute("select SRPOD_SPOM_CODE,SRPOM_IS_SHORT_CLOSE, sum(SRPOD_ORDER_QTY) AS SRPOD_ORDER_QTY,sum(SRPOD_INW_QTY) AS SRPOD_INW_QTY FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SRPOM_CODE=SRPOD_SPOM_CODE AND SRPOM_CODE='" + cpom_code + "' AND SRPOM_POST=1 Group BY SRPOD_SPOM_CODE,SRPOM_IS_SHORT_CLOSE");
                        if (dt.Rows.Count > 0)
                        {
                            double ORDER_QTY = Convert.ToDouble(dt.Rows[0]["SRPOD_ORDER_QTY"].ToString());
                            double INW_QTY = Convert.ToDouble(dt.Rows[0]["SRPOD_INW_QTY"].ToString());
                            string IS_SHORT_CLOSE = dt.Rows[0]["SRPOM_IS_SHORT_CLOSE"].ToString();
                            //if (ORDER_QTY - INW_QTY == 0 || INW_QTY == 0 || IS_SHORT_CLOSE == "True")
                            //if (IS_SHORT_CLOSE == "True" || INW_QTY == 0)
                            //{
                            string type1 = "AMEND";
                            Session["AMEND"] = "AMEND";
                            Response.Redirect("~/Transactions/ADD/ServicePurchaseOrder.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code, false);
                            return;
                            //}
                            //else if (INW_QTY > 0)
                            //{
                            //    PanelMsg.Visible = true;
                            //    lblmsg.Text = "Couldn't  Amend, This is Open Purchase order.";
                            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            //    return;
                            //}


                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "First Post This PO";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
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
                    lblmsg.Text = "You Can't Modify Because it is cancelled";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion

            #region ShortClose
            else if (e.CommandName.Equals("ShortClose"))
            {
                if ((CommonClasses.Execute("select count(*) as counted from SERVICE_PO_MASTER where SRPOM_CANCEL_PO=1 and ES_DELETE=0 and SRPOM_CODE='" + e.CommandArgument.ToString() + "'")).Rows[0][0].ToString() == "0")
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string cpom_code = e.CommandArgument.ToString();
                        DataTable dt = CommonClasses.Execute("select isnull(SRPOM_IS_SHORT_CLOSE,0) as SRPOM_IS_SHORT_CLOSE,SRPOM_POST  from SERVICE_PO_MASTER where SRPOM_CODE='" + cpom_code + "' AND SRPOM_IS_SHORT_CLOSE=0");
                        if (dt.Rows.Count > 0)
                        {
                            if ((dt.Rows[0]["SRPOM_POST"]).ToString() == "True")
                            {
                                //DataTable DtInw = CommonClasses.Execute("SELECT MAX (SRPOD_INW_QTY) as SRPOD_INW_QTY FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SRPOM_CODE=SRPOD_SPOM_CODE AND SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE='" + cpom_code + "'");
                                //double INW_QTY = Convert.ToDouble(DtInw.Rows[0]["SRPOD_INW_QTY"].ToString());
                                //if (INW_QTY > 0)
                                //{
                                bool Res = CommonClasses.Execute1("UPDATE SERVICE_PO_MASTER SET SRPOM_IS_SHORT_CLOSE=1 WHERE SRPOM_CODE='" + cpom_code + "'");
                                if (Res == true)
                                {
                                    CommonClasses.WriteLog("Service Purchase Order ", "ShortClose", "Service Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "Short Close Successfully";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                    return;
                                }

                                //}
                                //else
                                //{
                                //    PanelMsg.Visible = true;
                                //    lblmsg.Text = "Can Not Short Close Pending for Inward";
                                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                //    return;
                                //}

                            }
                            else
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "First Post This PO";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;

                            }
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Allready Short Closed";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Can't Modify Because it is cancelled";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion

            #region CancelPO
            else if (e.CommandName.Equals("CancelPO"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    //if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "AND IWD_CPOM_CODE=SRPOM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You Can't Cancel This PO, It Is Used In Inward Master";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //    return;
                    //}
                    //else
                    //{
                    if (CommonClasses.Execute1("update SERVICE_PO_MASTER set SRPOM_CANCEL_PO=1 where SRPOM_CODE='" + cpom_code + "'"))
                    {
                        CommonClasses.WriteLog("Service Purchase Order ", "CancelPO", "Service Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        LoadServicePO();
                        PanelMsg.Visible = true;
                        lblmsg.Text = "PO Canceled";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "PO Canceled";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    //}
                }
            }
            #endregion


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Po Transaction", "dgDetailServicePO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgDetailServicePO_PageIndexChanging
    protected void dgDetailServicePO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailServicePO.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
