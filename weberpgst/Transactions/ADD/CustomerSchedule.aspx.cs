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

public partial class Transactions_ADD_CustomerSchedule : System.Web.UI.Page
{
    static int mlCode = 0;
    DataTable dt = new DataTable();
    DataTable dtDetail = new DataTable();
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static string right = "";
    public static string str = "";
    static DataTable dt2 = new DataTable();
    static string ItemUpdateIndex = "-1";
    public static int Index = 0;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='60'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    ViewState["BindTable"] = BindTable;
                    ViewState["dt2"] = dt2;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["Index"] = Index;
                    try
                    {
                        ((DataTable)ViewState["BindTable"]).Clear();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        if (Request.QueryString[0].Equals("INSERT"))
                        {
                            txtScheduleDate.Attributes.Add("readonly", "readonly");
                            str = "";
                            LoadCustomer();
                            BlankGridView();
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            ((DataTable)ViewState["dt2"]).Columns.Clear();
                            txtScheduleDate.Text = CommonClasses.GetCurrentTime().Date.ToString("dd MMM yyyy");
                            //btnSubmit.Visible = false;
                        }
                        txtScheduleDate.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Schedule", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Schedule", "Page_Load", ex.Message.ToString());
        }
    }

    #region BlankGridView
    private void BlankGridView()
    {
        DataTable dtFilter = new DataTable();
        if (dgCustomerSchedule.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CSD_QTY", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgCustomerSchedule.DataSource = dtFilter;
                dgCustomerSchedule.DataBind();
                dgCustomerSchedule.Enabled = false;
            }
        }
    }
    #endregion BlankGridView

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,CUSTPO_MASTER where CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='1' and P_ACTIVE_IND=1   ORDER BY P_NAME ASC");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtScheduleDate.Attributes.Add("readonly", "readonly");
            LoadCustomer();
            dtDetail.Clear();
            //dt = CommonClasses.Execute("Select SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + mlCode + "");
            dt = CommonClasses.Execute("select P_CODE,P_NAME,CS.CS_CODE,CS.CS_NO,convert(varchar,CS.CS_MONTH,106) as CS_MONTH from CUSTOMER_SCHEDULE CS,PARTY_MASTER where CS.ES_DELETE=0 and CS.CS_P_CODE=P_CODE  and CS.CS_CM_CODE='" + (string)Session["CompanyCode"] + "' and CS_CODE=" + mlCode + "");
            if (dt.Rows.Count > 0)
            {
                ddlCustomer.SelectedValue = dt.Rows[0]["P_CODE"].ToString();
                txtScheduleNo.Text = dt.Rows[0]["CS_NO"].ToString();
                txtScheduleDate.Text = Convert.ToDateTime(dt.Rows[0]["CS_MONTH"]).ToString("dd MMM yyyy");
                dtDetail = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME+' '+I_CODENO as ItemName,CSD_QTY from CUSTOMER_SCHEDULE CS INNER JOIN CUSTOMER_SCHEDULE_DETAIL CSD ON CS.CS_CODE=CSD.CSD_CS_CODE INNER JOIN ITEM_MASTER I ON CSD.CSD_I_CODE=I.I_CODE where CS.ES_DELETE=0 and I.ES_DELETE=0 AND CS.CS_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and CS_CODE='" + mlCode + "' order by ItemName");
                if (dtDetail.Rows.Count > 0)
                {
                    dgCustomerSchedule.DataSource = dtDetail;
                    dgCustomerSchedule.DataBind();
                    ViewState["dt2"] = dtDetail;
                }
            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                txtScheduleDate.Enabled = false;
                dgCustomerSchedule.Enabled = false;
            }
            else if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", Convert.ToInt32(mlCode));
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ViewRec", Ex.Message);
        }
    }
    #endregion ViewRec

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion
    #region btnCancel_Click

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (mlCode != 0 && mlCode != null)
        //{
        //    CommonClasses.RemoveModifyLock("ISSUE_MASTER", "MODIFY", "IM_CODE", mlCode);
        //}

        //((DataTable)ViewState["dt2"]).Rows.Clear();
        //Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false)
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
            CommonClasses.SendError("Issue To Production", "btnCancel_Click", ex.Message.ToString());
        }

    }

    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtScheduleDate.Text == "")
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
            CommonClasses.SendError("Issue To Production", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec(); 
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Issue To Production", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions//VIEW/ViewCustomerSchedule.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "CancelRecord", Ex.Message);
        }
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
            CommonClasses.SendError("Supplier Purchase Order ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlCustomer.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Customer Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlCustomer.Focus();
            return;
        }
        #endregion
        if (dgCustomerSchedule.Enabled == true)
        {
            if (txtScheduleDate.Text == "")
            {
                // ShowMessage("#Avisos", "Select Po Type", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Customer Schedule Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtScheduleDate.Focus();
                return;
            }
            if (dgCustomerSchedule.Rows.Count > 0)
            {
                SaveRec();
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found In Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }
    #endregion btnSubmit_Click

    #region Numbering
    string Numbering()
    {
        int GenScheduleNo;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("Select isnull(max(CS_NO),0) as CS_NO FROM CUSTOMER_SCHEDULE WHERE CS_CM_CODE= '" + (string)Session["CompanyCode"] + "' and ES_DELETE=0");
        if (dt.Rows[0]["CS_NO"] == null || dt.Rows[0]["CS_NO"].ToString() == "")
        {
            GenScheduleNo = 1;
        }
        else
        {
            GenScheduleNo = Convert.ToInt32(dt.Rows[0]["CS_NO"]) + 1;
        }
        return GenScheduleNo.ToString();
    }
    #endregion

    bool SaveRec()
    {
        bool result = false;
        try
        {
            DateTime dtDate = Convert.ToDateTime(txtScheduleDate.Text);
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Doc_no = Convert.ToInt32(Numbering());
                DataTable dt = new DataTable();
                DataTable dtExist = CommonClasses.Execute("SELECT * FROM CUSTOMER_SCHEDULE WHERE ES_DELETE=0 and CS_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND CS_P_CODE='" + ddlCustomer.SelectedValue + "' AND DATEPART(mm,CS_MONTH)='" + dtDate.Month + "' AND DATEPART(YYYY,CS_MONTH)='" + dtDate.Year + "'");
                if (dtExist.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record already Exist for this Month , You can modify..";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtScheduleDate.Focus();
                }
                else
                {
                    if (CommonClasses.Execute1("INSERT INTO CUSTOMER_SCHEDULE(CS_CM_CODE,CS_MONTH,CS_P_CODE,CS_NO) VALUES ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDateTime(txtScheduleDate.Text).ToString("dd MMM yyyy") + "','" + ddlCustomer.SelectedValue + "','" + Doc_no + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CS_CODE) from CUSTOMER_SCHEDULE");
                        double TotScheduleQty = 0;
                        for (int i = 0; i < dgCustomerSchedule.Rows.Count; i++)
                        {
                            string CSD_QTY = string.Format("{0:0.000}", (Convert.ToDouble(((TextBox)dgCustomerSchedule.Rows[i].FindControl("lblCSD_QTY")).Text)));
                            result = CommonClasses.Execute1("INSERT INTO CUSTOMER_SCHEDULE_DETAIL(CSD_CS_CODE,CSD_I_CODE,CSD_QTY)VALUES('" + Code + "','" + ((Label)dgCustomerSchedule.Rows[i].FindControl("lblI_CODE")).Text + "','" + CSD_QTY + "')");
                            TotScheduleQty = TotScheduleQty + Convert.ToDouble(CSD_QTY);
                        }
                        if (result == true)
                        {
                            CommonClasses.Execute("Update CUSTOMER_SCHEDULE set CS_TOTAL_SCHEDULE=" + TotScheduleQty + " where CS_CODE='" + Code + "'");
                        }
                        CommonClasses.WriteLog("Customer Schedule", "Save", "Customer Schedule", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewCustomerSchedule.aspx", false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Could not saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtScheduleDate.Focus();
                    }
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE CUSTOMER_SCHEDULE SET CS_MONTH	=	'" + Convert.ToDateTime(txtScheduleDate.Text).ToString("dd MMM yyyy") + "' where CS_CODE='" + mlCode + "' and CS_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM CUSTOMER_SCHEDULE_DETAIL WHERE CSD_CS_CODE='" + mlCode + "'");
                    if (result)
                    {
                        double TotScheduleQty = 0;
                        for (int i = 0; i < dgCustomerSchedule.Rows.Count; i++)
                        {
                            string CSD_QTY = string.Format("{0:0.000}", (Convert.ToDouble(((TextBox)dgCustomerSchedule.Rows[i].FindControl("lblCSD_QTY")).Text)));
                            result = CommonClasses.Execute1("INSERT INTO CUSTOMER_SCHEDULE_DETAIL(CSD_CS_CODE,CSD_I_CODE,CSD_QTY)VALUES('" + mlCode + "','" + ((Label)dgCustomerSchedule.Rows[i].FindControl("lblI_CODE")).Text + "','" + CSD_QTY + "')");
                            TotScheduleQty = TotScheduleQty + Convert.ToDouble(CSD_QTY);
                        }
                        if (result == true)
                        {
                            CommonClasses.Execute("Update CUSTOMER_SCHEDULE set CS_TOTAL_SCHEDULE=" + TotScheduleQty + " where CS_CODE='" + mlCode + "'");
                        }
                        CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
                        CommonClasses.WriteLog("STOCK_ADJUSTMENT_MASTER", "Update", "STOCK_ADJUSTMENT_MASTER", txtScheduleNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewCustomerSchedule.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtScheduleDate.Focus();
                }
            }
            #endregion MODIFY

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("STOCK_ADJUSTMENT_MASTER", "SaveRec", ex.Message);
        }
        return result;
    }

    protected void dgCustomerSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region dgCustomerSchedule_RowCommand
    protected void dgCustomerSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgCustomerSchedule.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                // int rowindex = row.RowIndex;
                //dgSupplierPurchaseOrder.DeleteRow(Convert.ToInt32(ViewState["Index"].ToString()));
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgCustomerSchedule.DataSource = ((DataTable)ViewState["dt2"]);
                dgCustomerSchedule.DataBind();
                if (dgCustomerSchedule.Rows.Count == 0)
                    BlankGridView();
            }

            if (e.CommandName == "Modify")
            {
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule", "dgCustomerSchedule_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            // int n = no - 1;
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {

                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {

            }


            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion



    #region txtScheduleDate_TextChanged
    protected void txtScheduleDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtScheduleDate.Text) >= DateTime.Now)
            {
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtScheduleDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }
    }
    #endregion txtScheduleDate_TextChanged

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedIndex != 0)
            {
                DataTable dtItem_Bind = new DataTable();
                dtItem_Bind = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME+' '+I_CODENO as ItemName,0 as CSD_QTY from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0  order by ItemName");
                if (dtItem_Bind.Rows.Count > 0)
                {
                    //btnSubmit.Visible = true;
                    dgCustomerSchedule.DataSource = dtItem_Bind;
                    dgCustomerSchedule.DataBind();
                    dgCustomerSchedule.Enabled = true;
                }
                else
                    BlankGridView();
            }
        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError(" Customer Order Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion
}

