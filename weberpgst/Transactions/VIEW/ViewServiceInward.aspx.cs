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

public partial class Transactions_VIEW_ViewServiceInward : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='39'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadServiceInward();

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region LoadServiceInward
    private void LoadServiceInward()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select SIM_CODE,SIM_P_CODE,SIM_NO,convert(varchar,SIM_DATE,106) as SIM_DATE,SIM_CHALLAN_NO,P_NAME from SERVICE_INWARD_MASTER,PARTY_MASTER where SERVICE_INWARD_MASTER.ES_DELETE=0 and P_CODE=SIM_P_CODE  and  SIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  and P_TYPE='2'  AND SIM_TYPE='SIWM'    order by SIM_CODE desc ");

            if (dt.Rows.Count == 0)
            {
                if (dgDetailPO.Rows.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SIM_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SIM_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SIM_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SIM_P_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SIM_CHALLAN_NO", typeof(string)));
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
            CommonClasses.SendError("Service Inward", "LoadServiceInward", Ex.Message);
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
            CommonClasses.SendError("Service Inward", "txtString_TextChanged", Ex.Message);
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
                //    dtfilter = CommonClasses.Execute("SELECT IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME FROM SERVICE_INWARD_MASTER,LEDGER_MASTER WHERE SERVICE_INWARD_MASTER.ES_DELETE=0 and LM_CODE=IWM_P_CODE and SERVICE_INWARD_MASTER.IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SERVICE_INWARD_MASTER.ES_DELETE='0' and ((upper(IWM_NO) like upper('%" + str + "%') OR CONVERT(VARCHAR, IWM_DATE, 106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(IWM_CHALLAN_NO) like upper('%" + str + "%') OR upper(IWM_CHAL_DATE) like upper('%" + str + "%') OR upper(IWM_P_CODE) like upper('%" + str + "%')))order by IWM_CODE desc");
                dtfilter = CommonClasses.Execute("SELECT SIM_CODE,SIM_P_CODE,SIM_NO,convert(varchar,SIM_DATE,106) as SIM_DATE,SIM_CHALLAN_NO,convert(varchar,SIM_CHAL_DATE,106) as SIM_CHAL_DATE,P_NAME FROM SERVICE_INWARD_MASTER,PARTY_MASTER WHERE SERVICE_INWARD_MASTER.ES_DELETE=0 and P_CODE=SIM_P_CODE and SERVICE_INWARD_MASTER.SIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SERVICE_INWARD_MASTER.ES_DELETE='0' and (SIM_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, SIM_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR SIM_CHALLAN_NO like upper('%" + str + "%') OR SIM_CHAL_DATE like upper('%" + str + "%') OR SIM_P_CODE like upper('%" + str + "%'))   and P_TYPE='2' AND SIM_TYPE='SIWM'   order by SIM_CODE desc");
            }
            else
            {
                //dtfilter = CommonClasses.Execute("select SIM_CODE,SIM_P_CODE,SIM_NO,convert(varchar,SIM_DATE,106) as SIM_DATE,SIM_CHALLAN_NO,convert(varchar,SIM_CHAL_DATE,106) as SIM_CHAL_DATE,P_NAME FROM SERVICE_INWARD_MASTER,LEDGER_MASTER where SERVICE_INWARD_MASTER.ES_DELETE=0 and LM_CODE=SIM_P_CODE and SERVICE_INWARD_MASTER.SIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SERVICE_INWARD_MASTER.ES_DELETE='0' order by SIM_CODE desc ");
                dtfilter = CommonClasses.Execute("select SIM_CODE,SIM_P_CODE,SIM_NO,convert(varchar,SIM_DATE,106) as SIM_DATE,SIM_CHALLAN_NO,convert(varchar,SIM_CHAL_DATE,106) as SIM_CHAL_DATE,P_NAME FROM SERVICE_INWARD_MASTER,PARTY_MASTER where SERVICE_INWARD_MASTER.ES_DELETE=0 and P_CODE=SIM_P_CODE and SERVICE_INWARD_MASTER.SIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SERVICE_INWARD_MASTER.ES_DELETE='0' and P_TYPE='2'  AND SIM_TYPE='SIWM'     order by SIM_CODE desc ");
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
                    dtFilter.Columns.Add(new System.Data.DataColumn("SIM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SIM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SIM_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SIM_P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SIM_CHALLAN_NO", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Inward", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/ServiceMaterialInward.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Service Inward", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblSIM_CODE"))).Text))
            {

                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblSIM_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_SIM_CODE", "AND ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it's Inspection is already done";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else
                    {

                        bool flag = CommonClasses.Execute1("UPDATE SERVICE_INWARD_MASTER SET ES_DELETE = 1 WHERE SIM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            DataTable dtq = CommonClasses.Execute("SELECT SID_REV_QTY,SID_I_CODE,SID_CPOM_CODE FROM SERVICE_INWARD_DETAIL where SID_SIM_CODE=" + cpom_code + " ");

                            for (int i = 0; i < dtq.Rows.Count; i++)
                            {
                                CommonClasses.Execute("update SERVICE_PO_DETAILS set SRPOD_INW_QTY=SRPOD_INW_QTY-" + dtq.Rows[i]["SID_REV_QTY"] + " where SRPOD_I_CODE='" + dtq.Rows[i]["SID_I_CODE"] + "' and SRPOD_SPOM_CODE='" + dtq.Rows[i]["SID_CPOM_CODE"] + "'");
                                //  CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtq.Rows[i]["SID_REV_QTY"] + " where I_CODE='" + dtq.Rows[i]["SID_I_CODE"] + "'");

                            }

                            //flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + cpom_code + "' and STL_DOC_TYPE='IWIM'");
                            //if (flag == true)
                            //{
                            //    flag = CommonClasses.Execute1("DELETE FROM GIN_STOCK_LEDGER WHERE GL_DOC_ID='" + cpom_code + "' and GL_DOC_TYPE='IWIM'");
                            //}


                            //CommonClasses.WriteLog("Material Inward ", "Delete", "Material Inward", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]),(Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            CommonClasses.WriteLog("Service Inward ", "Delete", "Service Inward", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        }
                    }

                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Service Inward", "dgDetailPO_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }


            LoadServiceInward();
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

                string cpom_code = ((Label)(dgDetailPO.Rows[e.NewEditIndex].FindControl("lblSIM_CODE"))).Text;
                string type = "MODIFY";

                if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_SIM_CODE", "AND ES_DELETE=0", cpom_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Material Inspection ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
                else
                {
                    Response.Redirect("~/Transactions/ADD/ServiceMaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            CommonClasses.SendError("Service Inward", "dgPODetail_RowEditing", Ex.Message);
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
                        Response.Redirect("~/Transactions/ADD/ServiceMaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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

                        if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_SIM_CODE", "AND ES_DELETE=0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Inspection is already done";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/ServiceMaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

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
                    string Type = "SIWM";
                    string PrintType = "Single";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/RoportForms/ADD/RptInspectionGIN_IWIM.aspx?Title=" + Title + "&cpom_code=" + cpom_code + "&Type=" + Type + "&PType=" + PrintType, false);
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
            else if (e.CommandName.Equals("PrintMult"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string Type = "SIWM";
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
            CommonClasses.SendError("Service Inward-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SERVICE_INWARD_MASTER where SIM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Inward Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Service Inward -View", "ShowMessage", Ex.Message);
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
            LoadServiceInward();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);

    }
}
