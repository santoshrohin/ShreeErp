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

public partial class Masters_VIEW_ViewProductionToStore : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    //static string fieldName;
    DataTable dtFilter = new DataTable();
    ProductionToStore_BL productionStore_BL = null;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='52'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadProduction();
                    LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store-View", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region LoadProduction
    private void LoadProduction()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = CommonClasses.Execute("SELECT  PS_PERSON_NAME,dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO, CONVERT(varchar,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE, 106) AS PS_GIN_DATE, '' AS P_NAME,  0  AS I_CODENO,   0  AS I_NAME,0 AS PS_BATCH_NO, CAST(sum(dbo.PRODUCTION_TO_STORE_DETAIL.PSD_QTY) AS numeric(10, 3)) AS PSD_QTY, (CASE WHEN PS_TYPE = 1 THEN 'Normal' ELSE 'Assembly' END)  AS PS_TYPE, dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME FROM dbo.PRODUCTION_TO_STORE_MASTER INNER JOIN  dbo.PRODUCTION_TO_STORE_DETAIL ON dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE = dbo.PRODUCTION_TO_STORE_DETAIL.PSD_PS_CODE   WHERE  (dbo.PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE = '" + Session["CompanyCode"] + "') AND (dbo.PRODUCTION_TO_STORE_MASTER.ES_DELETE = '0') group by dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE,   PS_TYPE,dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME ORDER BY dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO DESC");
            /*11082018 :- Add Item Code and Item Name*/
            dt = CommonClasses.Execute("SELECT  PS_PERSON_NAME,dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO, CONVERT(varchar,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE, 106) AS PS_GIN_DATE, '' AS P_NAME,I_CODENO, I_NAME,0 AS PS_BATCH_NO, CAST(sum(dbo.PRODUCTION_TO_STORE_DETAIL.PSD_QTY) AS numeric(20, 3)) AS PSD_QTY, (CASE WHEN PS_TYPE = 1 THEN 'Normal' ELSE 'Assembly' END)  AS PS_TYPE, dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME FROM dbo.PRODUCTION_TO_STORE_MASTER INNER JOIN  dbo.PRODUCTION_TO_STORE_DETAIL ON dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE = dbo.PRODUCTION_TO_STORE_DETAIL.PSD_PS_CODE INNER JOIN ITEM_MASTER ON PSD_I_CODE=I_CODE WHERE (dbo.PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE = '" + Session["CompanyCode"] + "') AND ITEM_MASTER.ES_DELETE=0 AND (dbo.PRODUCTION_TO_STORE_MASTER.ES_DELETE = '0') group by dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE,   PS_TYPE,dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME,I_CODENO,I_NAME ORDER BY dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO DESC");
            if (dt.Rows.Count > 0)
            {
                dgProductionStore.Enabled = true;
                dgProductionStore.DataSource = dt;
                dgProductionStore.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store - View", "LoadProduction", Ex.Message);
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
            CommonClasses.SendError("Production To Store", "txtString_TextChanged", Ex.Message);
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
                //dtfilter = CommonClasses.Execute("SELECT PS_CODE,PS_GIN_NO,convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,P_NAME,I_CODENO,I_NAME,BT_NO as PS_BATCH_NO,cast(PSD_QTY as numeric(10,3)) as PSD_QTY,(case when PS_TYPE=1 then 'As Per Mat.Req' else 'Direct' end) as PS_TYPE,PS_PERSON_NAME FROM PRODUCTION_TO_STORE_MASTER,PARTY_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,BATCH_MASTER WHERE PS_CODE=PSD_PS_CODE and PSD_I_CODE=I_CODE and  PS_P_CODE=P_CODE and PS_CM_COMP_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND PRODUCTION_TO_STORE_MASTER.ES_DELETE='0' and (upper(PS_GIN_NO) like upper('%" + str + "%') OR upper(PS_GIN_DATE) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_CODENO) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%') or upper(BT_NO) like upper('%" + str + "%')) and BT_CODE=PS_BATCH_NO order by PS_GIN_NO DESC");
                /*11082018 :- Add Item Code and Item Name*/
                dtfilter = CommonClasses.Execute("SELECT  PS_PERSON_NAME, dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO, CONVERT(varchar,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE, 106) AS PS_GIN_DATE,0 AS P_NAME , I_CODENO,I_NAME, 0 AS PS_BATCH_NO, CAST(sum(dbo.PRODUCTION_TO_STORE_DETAIL.PSD_QTY) AS numeric(20, 3)) AS PSD_QTY, (CASE WHEN PS_TYPE = 1 THEN 'Norml' ELSE 'Assembly' END) AS PS_TYPE, dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME FROM dbo.PRODUCTION_TO_STORE_MASTER INNER JOIN  dbo.PRODUCTION_TO_STORE_DETAIL ON dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE = dbo.PRODUCTION_TO_STORE_DETAIL.PSD_PS_CODE INNER JOIN ITEM_MASTER ON PSD_I_CODE=I_CODE WHERE (dbo.PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE = '" + Session["CompanyCode"] + "') AND ITEM_MASTER.ES_DELETE=0 AND (dbo.PRODUCTION_TO_STORE_MASTER.ES_DELETE = '0') and (upper(dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO) like upper('%" + str + "%') OR upper(dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE) like upper('%" + str + "%') or upper(dbo.ITEM_MASTER.I_CODENO) like upper('%" + str + "%') or upper(dbo.ITEM_MASTER.I_NAME) like upper('%" + str + "%')) group by dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE, PS_TYPE,dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME,I_CODENO,I_NAME ORDER BY dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO DESC");

            else
                /*11082018 :- Add Item Code and Item Name*/
                dtfilter = CommonClasses.Execute("SELECT  PS_PERSON_NAME,dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO, CONVERT(varchar,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE, 106) AS PS_GIN_DATE, '' AS P_NAME,I_CODENO, I_NAME,0 AS PS_BATCH_NO, CAST(sum(dbo.PRODUCTION_TO_STORE_DETAIL.PSD_QTY) AS numeric(20, 3)) AS PSD_QTY, (CASE WHEN PS_TYPE = 1 THEN 'Normal' ELSE 'Assembly' END)  AS PS_TYPE, dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME FROM dbo.PRODUCTION_TO_STORE_MASTER INNER JOIN  dbo.PRODUCTION_TO_STORE_DETAIL ON dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE = dbo.PRODUCTION_TO_STORE_DETAIL.PSD_PS_CODE INNER JOIN ITEM_MASTER ON PSD_I_CODE=I_CODE WHERE (dbo.PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE = '" + Session["CompanyCode"] + "') AND ITEM_MASTER.ES_DELETE=0 AND (dbo.PRODUCTION_TO_STORE_MASTER.ES_DELETE = '0') group by dbo.PRODUCTION_TO_STORE_MASTER.PS_CODE, dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO,dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_DATE,   PS_TYPE,dbo.PRODUCTION_TO_STORE_MASTER.PS_PERSON_NAME,I_CODENO,I_NAME ORDER BY dbo.PRODUCTION_TO_STORE_MASTER.PS_GIN_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgProductionStore.Enabled = true;
                dgProductionStore.DataSource = dtfilter;
                dgProductionStore.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgProductionStore.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_GIN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_GIN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_BATCH_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_PERSON_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProductionStore.DataSource = dtFilter;
                    dgProductionStore.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "LoadStatus", ex.Message);
        }
    }
    #endregion LoadStatus

    #region LoadFilter
    public void LoadFilter()
    {
        try
        {
            if (dgProductionStore.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgProductionStore.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_GIN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_GIN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_BATCH_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PS_PERSON_NAME", typeof(String)));


                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProductionStore.DataSource = dtFilter;
                    dgProductionStore.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "LoadFilter", ex.Message);
        }
    }
    #endregion LoadFilter

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/ProductionToStore.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Production To Store-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region dgProductionStore_RowDeleting
    protected void dgProductionStore_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgProductionStore.Rows[e.RowIndex].FindControl("lblPS_CODE"))).Text))
            {

                string ps_code = ((Label)(dgProductionStore.Rows[e.RowIndex].FindControl("lblPS_CODE"))).Text;
                productionStore_BL = new ProductionToStore_BL(Convert.ToInt32(ps_code));
                // bool flag = CommonClasses.Execute1("UPDATE PRODUCTION_TO_STORE_MASTER SET ES_DELETE = 1 WHERE PS_CODE='" + Convert.ToInt32(ps_code) + "'");
                if (productionStore_BL.Delete())
                {
                    CommonClasses.WriteLog("Production To Store", "Delete", "Production To Store", ps_code, Convert.ToInt32(ps_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Deleted.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //DataTable dtoldqty = CommonClasses.Execute("select CL_CODE,CL_I_CODE,CL_CQTY,CL_DOC_CODE from PROD_AGAIN_WORKORD where CL_CODE='" + Convert.ToInt32(ps_code) + "' and CL_DOC_TYPE='PRODUCTON'");


                    ////Updating Old Qty In PROD_AGAIN_WORKORD as well as Stock In dbo.ITEM_MASTER
                    //for (int r = 0; r < dtoldqty.Rows.Count; r++)
                    //{
                    //    //subtracting old Qty From Previous Consume Qty
                    //    flag = CommonClasses.Execute1("update PROD_AGAIN_WORKORD set CL_CON_QTY=CL_CON_QTY+'" + dtoldqty.Rows[r]["CL_CQTY"] + "' where CL_CODE='" + dtoldqty.Rows[r]["CL_DOC_CODE"] + "' and CL_I_CODE = '" + dtoldqty.Rows[r]["CL_I_CODE"] + "' and CL_DOC_TYPE='WORKORD'");

                    //    if (flag == true)
                    //    {
                    //        //subtracting old Qty From Stock

                    //        flag = CommonClasses.Execute1("UPDATE ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+'" + dtoldqty.Rows[r]["CL_CQTY"] + "' where I_CODE='" + dtoldqty.Rows[r]["CL_I_CODE"].ToString() + "'");
                    //    }
                    //    if (flag == true)
                    //    {
                    //        //Deleteing previous production entry from PROD_AGAIN_WORKORD
                    //        flag = CommonClasses.Execute1("DELETE FROM PROD_AGAIN_WORKORD WHERE CL_CODE='" + dtoldqty.Rows[r]["CL_CODE"] + "' and CL_I_CODE='" + dtoldqty.Rows[r]["CL_I_CODE"] + "' and CL_DOC_CODE='" + dtoldqty.Rows[r]["CL_DOC_CODE"] + "'");

                    //    }

                }
                LoadProduction();
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights to Delete";
        }



    }

    #endregion

    #region dgProductionStore_PageIndexChanging
    protected void dgProductionStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgProductionStore.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store- View", "dgProductionStore_PageIndexChanging", Ex.Message);
        }

    }
    #endregion

    #region dgProductionStore_RowCommand
    protected void dgProductionStore_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string ps_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/ProductionToStore.aspx?c_name=" + type + "&ps_code=" + ps_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights to view";
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string ps_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/ProductionToStore.aspx?c_name=" + type + "&ps_code=" + ps_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another user";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights to view";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "ISSUE";
                        string MatReq_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/RoportForms/ADD/ProductionTOStorePrint.aspx?MatReq_Code=" + MatReq_Code + "&print_type=" + type, false);
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
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store-View", "dgProductionStore_RowCommand", Ex.Message);
        }
    }
    #endregion


    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PRODUCTION_TO_STORE_MASTER where PS_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Production To Store - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

}
