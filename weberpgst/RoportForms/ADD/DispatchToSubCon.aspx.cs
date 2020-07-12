using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Transactions_ADD_DispatchToSubCon : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtInvoiceDetail = new DataTable();
    static int mlCode = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    //static double EBasicPer = 0;
    //static double EEduCessPer = 0;
    //static double EHEduCessPer = 0;
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    static string right = "";

    #endregion

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
                    try
                    {
                        //EBasicPer = 0;
                        //EEduCessPer = 0;
                        //EHEduCessPer = 0;
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                        ViewState["mlCode"] = "";

                        ViewState["Index"] = "";

                        ViewState["mlCode"] = mlCode;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        dt2.Clear();
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Clear();

                        LoadCustomer();
                        LoadICode();
                        LoadIName();
                        LoadProcess();



                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadFilter();

                            dgInvoiceAddDetail.Enabled = false;
                            txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtIssuedate.Text = System.DateTime.Now.ToString("dd MMM yyyy");


                            txtDate.Attributes.Add("readonly", "readonly");
                            txtIssuedate.Attributes.Add("readonly", "readonly");

                        }
                        txtInvoiceNo.Focus();
                        dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
        }
    }


    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");

        //if (ddlCustomer.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "Select Customer Name";
        //    ddlCustomer.Focus();
        //    return;

        //}
        //if (txtDate.Text == "")
        //{
        //    //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "Select Invoice Date";
        //    txtDate.Focus();
        //    return;
        //}
        if (dgInvoiceAddDetail.Enabled == false)
        {

            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //PanelMsg.Visible = true;
            //lblmsg.Text = "Record Not Exist";
            return;
        }

        if (dgInvoiceAddDetail.Rows.Count > 0)
        {
            SaveRec();
        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //PanelMsg.Visible = true;
            //lblmsg.Text = "Record Not Exist";
        }

    }

    #endregion btnSubmit_Click

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
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
            CommonClasses.SendError("Tax Invoice", "btnCancel_Click", ex.Message.ToString());
        }
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

            CommonClasses.SendError("Tax Invocie", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        // CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDate.Text == "")
            {
                flag = false;
            }

            //else if (dgInvoiceAddDetail.Enabled)
            //{
            //    flag = true;
            //}
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,CUSTPO_MASTER where CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1   ORDER BY P_NAME ASC-- and CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC");

            dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,SUPP_PO_MASTER where SPOM_P_CODE=P_CODE and SUPP_PO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and SPOM_CANCEL_PO=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 AND  SPOM_POTYPE=1 AND SPOM_POTYPE=1 ORDER BY P_NAME");

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

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 order by I_CODENO ASC");
            //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND  (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) > 0 or SPOM_CODE=SPOD_SPOM_CODE) and SPOD_I_CODE=I_CODE and  SPOM_CODE=SPOD_SPOM_CODE and SPOM_P_CODE=" + ddlCustomer.SelectedValue + " and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 and SPOM_POTYPE=1 order by I_CODENO ASC");
            dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND  (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) > 0 or SPOM_CODE=SPOD_SPOM_CODE) and SPOD_I_CODE=I_CODE and  SPOM_CODE=SPOD_SPOM_CODE and SPOM_P_CODE=" + ddlCustomer.SelectedValue + " and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 and SPOM_POTYPE=1 order by I_CODENO ASC");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        //try
        //{
        //    DataTable dtPO = new DataTable();
        //    if (Request.QueryString[0].Equals("MODIFY"))
        //    {
        //        dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and  CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE)   ");
        //    }
        //    else
        //    {
        //        dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0  ");
        //    }

        //    ddlPONo.DataSource = dtPO;
        //    ddlPONo.DataTextField = "CPOM_PONO";
        //    ddlPONo.DataValueField = "CPOM_CODE";
        //    ddlPONo.DataBind();
        //    ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));

        //}
        //catch (Exception Ex)
        //{
        //    CommonClasses.SendError("Tax Invoice", "LoadPO", Ex.Message);
        //}
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct I_CODE,I_NAME from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and  CPOD_I_CODE=I_CODE   and CPOM_CODE=CPOD_CPOM_CODE  and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 order by I_NAME ASC");

            //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND  (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) > 0 or SPOM_CODE=SPOD_SPOM_CODE) and SPOD_I_CODE=I_CODE and  SPOM_CODE=SPOD_SPOM_CODE and SPOM_P_CODE=" + ddlCustomer.SelectedValue + " and SPOM_CANCEL_PO=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_POTYPE=1 order by I_NAME ASC");
            dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND  (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) > 0 or SPOM_CODE=SPOD_SPOM_CODE) and SPOD_I_CODE=I_CODE and  SPOM_CODE=SPOD_SPOM_CODE and SPOM_P_CODE=" + ddlCustomer.SelectedValue + " and SPOM_CANCEL_PO=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_POTYPE=1 order by I_NAME ASC");

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvoiceAddDetail.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ACT_WGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PROCESS_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PROCESS_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_CON_QTY", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvoiceAddDetail.DataSource = dtFilter;
                dgInvoiceAddDetail.DataBind();
                dgInvoiceAddDetail.Enabled = false;
            }
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
            CommonClasses.SendError("Tax Invoice", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,INVOICE_DETAIL,BOM_MASTER,BOM_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0 or IND_CPOM_CODE=SPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    else
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM  SUPP_PO_MASTER,SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and ( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                }
                else
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,INVOICE_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0 or IND_CPOM_CODE=SPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    }
                    else
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and ( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    }
                }
                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "SPOM_PO_NO";
                ddlPONo.DataValueField = "SPOM_CODE";
                ddlPONo.DataBind();
                //ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));


                ddlPONo_SelectedIndexChanged(null, null);


                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                  //  txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                    txtVQty.Text = "";
                }
                else
                {
                    txtUOM.Text = "";
                    txtVQty.Text = "";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    private void LoadProcess()
    {
        try
        {
            DataTable dtProcess = CommonClasses.Execute("SELECT PROCESS_CODE,PROCESS_NAME FROM PROCESS_MASTER WHERE ES_DELETE=0 AND PROCESS_CM_COMP_ID='" + (string)Session["CompanyId"] + "'");
            if (dtProcess.Rows.Count > 0)
            {
                ddlProcessType.DataSource = dtProcess;
                ddlProcessType.DataTextField = "PROCESS_NAME";
                ddlProcessType.DataValueField = "PROCESS_CODE";
                ddlProcessType.DataBind();
            }

        }
        catch (Exception)
        {
        }
    }


    #region ddlPONo_SelectedIndexChanged
    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                DataTable dt1 = new DataTable();
                DataTable dtQty = new DataTable();
                DataTable dtQty1 = new DataTable();
                double dispatchQty = 0;
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    dt1 = CommonClasses.Execute(" SELECT SPOD_CONV_RATIO,SPOD_PROCESS_CODE,(SPOD_ORDER_QTY*BD_VQTY)+(SPOD_ORDER_QTY* BD_SCRAPQTY) AS SPOD_ORDER_QTY ,I_INV_RATE AS SPOD_RATE,ITEM_MASTER.I_UOM_CODE AS  SPOD_UOM_CODE,I_UOM_NAME  FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_UNIT_MASTER,BOM_MASTER,BOM_DETAIL ,ITEM_MASTER  WHERE SPOD_SPOM_CODE=SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SPOD_RATE_UOM  AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND I_CODE=BD_I_CODE AND SPOM_CODE='" + ddlPONo.SelectedValue + "' AND BD_I_CODE='" + ddlItemCode.SelectedValue + "'");
                    dtQty = CommonClasses.Execute(" SELECT ISNULL((SPOD_ORDER_QTY)*BD_VQTY + (SPOD_ORDER_QTY)*BD_SCRAPQTY,0) as Qty FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL where SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND  BD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "'");
                    dtQty1 = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) AS DISPATCHQTY FROM CHALLAN_STOCK_LEDGER where CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + ddlItemCode.SelectedValue + "' AND CL_DOC_TYPE='OutSUBINM'");
                    if (dtQty1.Rows.Count > 0)
                    {
                        dispatchQty = Convert.ToDouble(dtQty1.Rows[0]["DISPATCHQTY"].ToString());
                    }
                }
                else
                {
                    dt1 = CommonClasses.Execute("SELECT SPOD_CONV_RATIO,SPOD_PROCESS_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_UOM_CODE,I_UOM_NAME FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_UNIT_MASTER WHERE SPOD_SPOM_CODE=SPOM_CODE and I_UOM_CODE=SPOD_RATE_UOM AND SPOM_CODE='" + ddlPONo.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "'");
                    dtQty = CommonClasses.Execute("SELECT ISNULL((SPOD_ORDER_QTY-ISNULL(SPOD_DISPACH,0)),0) as Qty FROM SUPP_PO_DETAILS where SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "'");
                    //dtQty1 = CommonClasses.Execute(" SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) AS DISPATCHQTY FROM CHALLAN_STOCK_LEDGER where CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + ddlItemCode.SelectedValue + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //if (dtQty1.Rows.Count > 0)
                    //{
                    //    dispatchQty = Convert.ToDouble(dtQty1.Rows[0]["DISPATCHQTY"].ToString());
                    //}
                }


                if (dt1.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["SPOD_RATE"]);
                    ddlProcessType.SelectedValue = dt1.Rows[0]["SPOD_PROCESS_CODE"].ToString();
                    txtActWght.Text = dt1.Rows[0]["SPOD_CONV_RATIO"].ToString();
                    txtPoUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                }
                else
                {
                    txtActWght.Text = "0.00";
                    txtRate.Text = "0.00";
                }
                if (dtQty.Rows.Count > 0)
                {
                    if (ddlConsPatt.SelectedIndex == 0)
                    {
                        DataTable dtCheckOpen = CommonClasses.Execute(" SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL where SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND  BD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "'");
                        if (dtCheckOpen.Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dtCheckOpen.Rows[0][0]) != 0)
                            {
                                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dtQty.Rows[0]["Qty"]) - dispatchQty);
                            }
                            else
                            {
                                txtPendingQty.Text = "0.0";
                            }
                        }
                        else
                        {
                            txtPendingQty.Text = "0.0";
                        }
                    }
                    else
                    {
                        DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_ORDER_QTY>0");
                        if (dtCheckOpen.Rows.Count > 0)
                        {
                            txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dtQty.Rows[0]["Qty"]) - dispatchQty);
                        }
                        else
                        {
                            txtPendingQty.Text = "0.0";
                        }
                    }
                    //txtVQty.Text = string.Format("{0:0.000}", dtQty.Rows[0]["Qty"]);
                    //txtVQty_OnTextChanged(null, null);
                    txtVQty.Text = " ";
                }
                else
                {
                    txtPendingQty.Text = "0.000";
                    txtVQty.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlPONo_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region LoadICode
    private void LoadICodeMaterial()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 order by I_CODENO ASC");

            //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND  (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) > 0 or SPOM_CODE=SPOD_SPOM_CODE) and SPOD_I_CODE=I_CODE and  SPOM_CODE=SPOD_SPOM_CODE and SPOM_P_CODE=" + ddlCustomer.SelectedValue + " and SPOM_POTYPE=1 order by I_CODENO ASC");
            dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where SPOM_CODE=SPOD_SPOM_CODE AND SPOM_POTYPE=1  AND BD_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND SPOD_I_CODE=BM_I_CODE AND SPOM_P_CODE=" + ddlCustomer.SelectedValue + "    order by  I_CODENO ASC ");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion
    #region LoadIName
    private void LoadINameMaterial()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct I_CODE,I_NAME from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and  CPOD_I_CODE=I_CODE   and CPOM_CODE=CPOD_CPOM_CODE  and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 order by I_NAME ASC");
            dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where SPOM_CODE=SPOD_SPOM_CODE AND SPOM_POTYPE=1  AND BD_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND SPOD_I_CODE=BM_I_CODE AND SPOM_P_CODE=" + ddlCustomer.SelectedValue + "    order by  I_NAME ASC ");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadIName", Ex.Message);
        }

    }
    #endregion


    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlConsPatt.SelectedIndex.ToString() == "0")
            {
                LoadICodeMaterial();
                LoadINameMaterial();
            }
            else if (ddlConsPatt.SelectedIndex.ToString() == "1")
            {
                LoadICode();
                LoadIName();
            }
            else if (ddlConsPatt.SelectedIndex.ToString() == "2")
            {
                LoadICode();
                LoadIName();
            }

        }
        catch (Exception)
        {
        }
    }
    #endregion


    #region ddlConsPatt_SelectedIndexChanged
    protected void ddlConsPatt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCustomer_SelectedIndexChanged(null, null);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,INVOICE_DETAIL,BOM_MASTER,BOM_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0 or IND_CPOM_CODE=SPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    else
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM  SUPP_PO_MASTER,SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and ( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                }
                else
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,INVOICE_DETAIL WHERE SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and (( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0 or IND_CPOM_CODE=SPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    }
                    else
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(SPOM_CODE),SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND SUPP_PO_MASTER.ES_DELETE=0 and ( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>=0   and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_POTYPE=1");
                    }
                }
                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "SPOM_PO_NO";
                ddlPONo.DataValueField = "SPOM_CODE";
                ddlPONo.DataBind();
                //ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));


                ddlPONo_SelectedIndexChanged(null, null);


                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    //txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                    txtVQty.Text = "";
                }
                else
                {
                    txtUOM.Text = "";
                    txtVQty.Text = "";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion



    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtDate.Attributes.Add("readonly", "readonly");
            txtIssuedate.Attributes.Add("readonly", "readonly");

            dtInvoiceDetail.Clear();
            DataTable dtMast = CommonClasses.Execute("SELECT INM_CODE,INM_NO,INM_DATE,INM_P_CODE,INM_VEH_NO,INM_ISSUE_DATE,INM_C_DAYS,INM_NATURE_PRO,INM_PREPARE_BY,INM_INVOICE_TYPE,INM_REMARK FROM INVOICE_MASTER where ES_DELETE=0 and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

            if (dtMast.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dtMast.Rows[0]["INM_CODE"]); ;

                txtInvoiceNo.Text = dtMast.Rows[0]["INM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = dtMast.Rows[0]["INM_P_CODE"].ToString();
                txtVehNo.Text = dtMast.Rows[0]["INM_VEH_NO"].ToString();
                txtCreditDays.Text = dtMast.Rows[0]["INM_C_DAYS"].ToString();
                txtPreparedBy.Text = dtMast.Rows[0]["INM_PREPARE_BY"].ToString();
                txtSpecialInst.Text = dtMast.Rows[0]["INM_REMARK"].ToString();
                txtNatureProcess.Text = dtMast.Rows[0]["INM_NATURE_PRO"].ToString();


                ddlConsPatt.SelectedIndex = Convert.ToInt32(dtMast.Rows[0]["INM_INVOICE_TYPE"].ToString());
                ddlCustomer_SelectedIndexChanged(null, null);
                btnSubmit.Visible = true;
                btnInsert.Visible = true;

                //dtInvoiceDetail = CommonClasses.Execute("select CPOD_I_CODE as ItemCode,I_CODENO as ShortName,cast((CPOD_ORD_QTY*CPOD_RATE) as numeric(20,2)) as Amount,CPOD_AMT,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as UnitCode,I_UOM_NAME as Unit,cast(CPOD_ORD_QTY as numeric(20,2))  as OrderQty,cast(CPOD_RATE as  numeric(10,2)) as Rate,CPOD_DESC as Description FROM CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER WHERE CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_MASTER.ES_DELETE=0  and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' order by CPOD_I_CODE");
                //dtInvoiceDetail = CommonClasses.Execute("select DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,'0.000' as STOCK_QTY,cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as IND_EX_AMT,IND_E_CESS_AMT as IND_E_CESS_AMT,IND_SH_CESS_AMT as IND_SH_CESS_AMT from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where IND_INM_CODE=INM_CODE and IND_CPOM_CODE=CPOM_CODE AND IND_CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOD_DISPACH > 0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");
                if (Convert.ToInt32(dtMast.Rows[0]["INM_INVOICE_TYPE"].ToString()) == 0)
                {
                    //dtInvoiceDetail = CommonClasses.Execute("SELECT DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,SPOM_PO_NO as PO_NO,I_CURRENT_BAL as STOCK_QTY,cast(( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_DISPACH,0)) as numeric(10,3)) as PEND_QTY, cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(0 as numeric(20,2)) as AMT,IND_PROCESS_CODE,PROCESS_NAME AS IND_PROCESS_NAME,IND_CON_QTY from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER,PROCESS_MASTER  WHERE INM_CODE=IND_INM_CODE and IND_CPOM_CODE=SPOM_CODE AND  SPOM_CODE=SPOD_SPOM_CODE  AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE AND PROCESS_CODE=IND_PROCESS_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    dtInvoiceDetail = CommonClasses.Execute("SELECT DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,SPOM_PO_NO as PO_NO,I_CURRENT_BAL as STOCK_QTY,0 as PEND_QTY, cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(0 as numeric(20,2)) as AMT,IND_PROCESS_CODE,PROCESS_NAME AS IND_PROCESS_NAME,IND_CON_QTY from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER,PROCESS_MASTER  WHERE INM_CODE=IND_INM_CODE and IND_CPOM_CODE=SPOM_CODE AND  SPOM_CODE=SPOD_SPOM_CODE  AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE AND PROCESS_CODE=IND_PROCESS_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                }
                else
                {
                    dtInvoiceDetail = CommonClasses.Execute("SELECT DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,SPOM_PO_NO as PO_NO,I_CURRENT_BAL as STOCK_QTY,cast(( ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_DISPACH,0)) as numeric(10,3)) as PEND_QTY, cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(0 as numeric(20,2)) as AMT,IND_PROCESS_CODE,PROCESS_NAME AS IND_PROCESS_NAME,IND_CON_QTY from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER,PROCESS_MASTER  WHERE INM_CODE=IND_INM_CODE and IND_CPOM_CODE=SPOM_CODE AND  SPOM_CODE=SPOD_SPOM_CODE  AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE AND PROCESS_CODE=IND_PROCESS_CODE   AND  IND_I_CODE=SPOD_I_CODE  and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                }

                //LoadICode();
                //LoadIName();
                LoadPO();
                if (dtInvoiceDetail.Rows.Count != 0)
                {
                    dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                    dgInvoiceAddDetail.DataBind();
                    ViewState["dt2"] = dtInvoiceDetail;
                    dgInvoiceAddDetail.Enabled = true;
                    //txtTotalAmt.Text = string.Format("{0:0.00}", sum1);
                }

            }

            if (str == "VIEW")
            {
                //ddlPOType.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtDate.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtAmount.Enabled = false;
                ddlPONo.Enabled = false;
                // txtDescription.Enabled=false;
                // txtEDUPer.Enabled=false;
                //  txtHEDUPer.Enabled=false;
                txtVQty.Enabled = false;
                txtRate.Enabled = false;



                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                dgInvoiceAddDetail.Enabled = false;
                chkIsSuppliement.Enabled = false;
                //txtPackDesc.Enabled = false;

            }
            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                chkIsSuppliement.Enabled = false;
                ddlConsPatt.Enabled = false;
                CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ViewRec", Ex.Message);
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

    //#region txtVQty_OnTextChanged
    //protected void txtVQty_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txtVQty.Text == "")
    //        {
    //            txtVQty.Text = "0.000";
    //        }
    //        else
    //        {
    //            string totalStr = DecimalMasking(txtVQty.Text);

    //            txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    //        }
    //        if (txtRate.Text == "")
    //        {
    //            txtRate.Text = "0.00";
    //        }
    //        else
    //        {
    //            string totalStr = DecimalMasking(txtRate.Text);

    //            txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

    //            //txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
    //        }
    //        if (txtPendingQty.Text != "")
    //        {
    //            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
    //            {
    //                //ShowMessage("#Avisos", "Invoice Qty Is Not Greater Than Pending Qty", CommonClasses.MSG_Warning);
    //                PanelMsg.Visible = true;
    //                lblmsg.Text = "Please Enter Correct Invoice Qty";
    //                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    //                txtVQty.Text = "0.00";
    //                txtVQty.Focus();
    //                return;
    //            }
    //            if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtVQty.Text))
    //            {
    //                //ShowMessage("#Avisos", "Invoice Qty Is Not Greater Than Pending Qty", CommonClasses.MSG_Warning);
    //                PanelMsg.Visible = true;
    //                lblmsg.Text = "Please Enter Invoice Qty Less than stock";
    //                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    //                txtVQty.Text = "0.00";
    //                txtVQty.Focus();
    //                return;
    //            }
    //        }



    //txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
    //if (txtNoPackaeg.Text == "")
    //{
    //    txtNoPackaeg.Text = "0";
    //}
    //if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
    //{
    //    double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
    //    txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
    //}
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Tax Incvoice", "txtVQty_OnTextChanged", Ex.Message);
    //    }
    //}
    //#endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVQty.Text == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);

                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
                //txtVQty.Text = string.Format("{0:0.00}",Convert.ToDouble(txtVQty.Text));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);

                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                // txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            //      txtAmount.Text = amount.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    //#region CalcExise
    //void CalcExise()
    //{
    //    #region CalExcise
    //    double EBasic = 0;
    //    double EEduCess = 0;
    //    double EHEduCess = 0;

    //    if (txtBasicExcAmt.Text == "")
    //    {
    //        txtBasicExcAmt.Text = "0.00";
    //    }
    //    if (txtEdueceAmt.Text == "")
    //    {
    //        txtEdueceAmt.Text = "0.00";
    //    }
    //    if (txtSHEExcAmt.Text == "")
    //    {
    //        txtSHEExcAmt.Text = "0.00";
    //    }
    //    if (txtPackAmt.Text == "")
    //    {
    //        txtPackAmt.Text = "0.00";
    //    }
    //    if (txtDiscAmt.Text == "")
    //    {
    //        txtDiscAmt.Text = "0.00";
    //    }
    //    double EBasicOn = (Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtPackAmt.Text)) - Convert.ToDouble(txtDiscAmt.Text);
    //    EBasic = (EBasicOn * EBasicPer) / 100;
    //    EEduCess = Math.Round(EBasic * EEduCessPer / 100, 2);
    //    EHEduCess = Math.Round(EBasic * EHEduCessPer / 100, 2);
    //    txtBasicExcAmt.Text = string.Format("{0:0.00}",Math.Round(EBasic,2));
    //    txtEdueceAmt.Text = string.Format("{0:0.00}", EEduCess);
    //    txtSHEExcAmt.Text = string.Format("{0:0.00}", EHEduCess);

    //    #endregion
    //}
    //#endregion

    //#region txtNoPackaeg_TextChanged
    //protected void txtNoPackaeg_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtNoPackaeg.Text == "")
    //    {
    //        txtNoPackaeg.Text = "0";
    //    }
    //    // string totalStr = DecimalMasking(txtNoPackaeg.Text);

    //    //txtNoPackaeg.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    //    if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
    //    {
    //        if (txtVQty.Text != "")
    //        {
    //            double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
    //            txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
    //        }
    //    }
    //}
    //#endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {

            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");


            if (ddlCustomer.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;

            }
            if (ddlItemCode.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            //if (ddlPONo.SelectedIndex == 0)
            //{
            //    // ShowMessage("#Avisos", "Select PO Number", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Select PO Number";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlPONo.Focus();
            //    return;
            //}
            if (txtVQty.Text != "" || Convert.ToDouble(txtVQty.Text) != 0)
            {
                //ShowMessage("#Avisos", "The Field 'Invoice Qty' Is Required", CommonClasses.MSG_Warning);
                if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtVQty.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter Disp. Qty Less Than Stock Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtVQty.Focus();
                    return;
                }

            }
            if (txtVQty.Text == "" || Convert.ToDouble(txtVQty.Text) == 0)
            {
                //ShowMessage("#Avisos", "The Field 'Invoice Qty' Is Required", CommonClasses.MSG_Warning);                 
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtVQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
            {//SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' 
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL  WHERE SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {

                        //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Focus();
                        return;
                    }
                }
                else
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {

                        //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Focus();
                        return;
                    }
                }
            }
            //if (txtBatchNo.Text == "")
            //{
            //   // ShowMessage("#Avisos", "The Field 'Batch Number' Is Required", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Enetr Batch Number";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtBatchNo.Focus();
            //    return;
            //}
            //if (txtNoPackaeg.Text=="0")
            //{
            //   // ShowMessage("#Avisos", "No Of Packages Can Not Be 0", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Enter No of Packages";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtNoPackaeg.Focus();
            //    return;
            //}
            //if (txtStockQty.Text == "" || txtStockQty.Text == "0.000")
            //{
            //    //ShowMessage("#Avisos", "The Field 'Pack Qty' Is Required", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please check Stock  Qty";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtStockQty.Focus();
            //    return;
            //}

            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_I_CODE"))).Text;
                    string PO_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblPO_CODE"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && PO_CODE == ddlPONo.SelectedValue.ToString())
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);

                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            return;
                        }

                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString() && PO_CODE == ddlPONo.SelectedValue.ToString())
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);

                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            return;
                        }

                    }

                }
            }

            #region Add Data table coloums
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("UOM");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("STOCK_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PEND_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("INV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("ACT_WGHT");
                ((DataTable)ViewState["dt2"]).Columns.Add("RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("AMT");


                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PROCESS_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PROCESS_NAME");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_CON_QTY");


                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_SUBHEADING");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_BACHNO");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_NO_PACK");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_PAK_QTY");

                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_EX_AMT");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_CESS_AMT");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_SH_CESS_AMT");
                // ((DataTable)ViewState["dt2"]).Columns.Add("IND_PACK_DESC");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IND_I_CODE"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem.ToString();
            dr["IND_I_NAME"] = ddlItemName.SelectedItem.ToString();
            dr["UOM"] = txtUOM.Text;
            dr["PO_CODE"] = ddlPONo.SelectedValue;
            dr["PO_NO"] = ddlPONo.SelectedItem.ToString();
            dr["STOCK_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtStockQty.Text == "" ? "0.00" : txtStockQty.Text) - Convert.ToDouble(txtVQty.Text)));
            //dr["PEND_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtPendingQty.Text) - Convert.ToDouble(txtVQty.Text)));
            if (Convert.ToDouble(txtPendingQty.Text) == 0)
            {
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL  WHERE SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {
                        dr["PEND_QTY"] = string.Format("{0:0.000}", ((Convert.ToDouble(txtPendingQty.Text == "" ? "0.00" : txtPendingQty.Text)) - Convert.ToDouble(txtVQty.Text)));
                    }
                    else
                    {
                        dr["PEND_QTY"] = 0;
                    }
                }
                else
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {
                        dr["PEND_QTY"] = string.Format("{0:0.000}", ((Convert.ToDouble(txtPendingQty.Text == "" ? "0.00" : txtPendingQty.Text)) - Convert.ToDouble(txtVQty.Text)));
                    }
                    else
                    {
                        dr["PEND_QTY"] = 0;
                    }
                }
            }
            else
            {
                dr["PEND_QTY"] = ((Convert.ToDouble(txtPendingQty.Text == "" ? "0.00" : txtPendingQty.Text)) - Convert.ToDouble(txtVQty.Text)).ToString();
            }
            dr["INV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtVQty.Text == "" ? "0.00" : txtVQty.Text)));
            dr["ACT_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtActWght.Text == "" ? "0.00" : txtActWght.Text)));
            dr["RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text == "" ? "0.00" : txtRate.Text)));
            dr["AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)));

            dr["IND_PROCESS_CODE"] = ddlProcessType.SelectedValue;
            dr["IND_PROCESS_CODE"] = ddlProcessType.SelectedValue;
            dr["IND_PROCESS_NAME"] = ddlProcessType.SelectedItem.Text;
            dr["IND_CON_QTY"] = txtConvertQty.Text;




            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    dgInvoiceAddDetail.Enabled = true;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                dgInvoiceAddDetail.Enabled = true;
            }
            #endregion


            #region Binding data to Grid
            dgInvoiceAddDetail.Visible = true;
            dgInvoiceAddDetail.Enabled = true;
            dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
            dgInvoiceAddDetail.DataBind();
            #endregion

            #region Clear Controles

            ClearControles();
            #endregion


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ClearControles
    void ClearControles()
    {
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlPONo.SelectedIndex = 0;
        txtUOM.Text = "";
        txtStockQty.Text = "0.000";
        txtPendingQty.Text = "0.000";
        txtVQty.Text = "0.000";
        txtActWght.Text = "0.00";
        txtRate.Text = "0.00";
        txtAmount.Text = "0.00";
        txtSubHeading.Text = "";
        //txtBatchNo.Text = "";
        //txtNoPackaeg.Text = "0";
        //txtQtyPerPack.Text = "0.00";
        //txtPackDesc.Text = "";
        str = "";
    }
    #endregion

    #region dgInvoiceAddDetail_RowCommand
    protected void dgInvoiceAddDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInvoiceAddDetail.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInvoiceAddDetail.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
                dgInvoiceAddDetail.DataBind();

                if (dgInvoiceAddDetail.Rows.Count == 0)
                {
                    dgInvoiceAddDetail.Enabled = false;
                    LoadFilter();

                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                //ddlItemCode_SelectedIndexChanged(null, null);
                txtUOM.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;

                ddlPONo_SelectedIndexChanged(null, null);
                txtStockQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(((Label)(row.FindControl("lblSTOCK_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text)), 3));

                double pendQty = Convert.ToDouble(((Label)(row.FindControl("lblPEND_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text);
                txtPendingQty.Text = string.Format("{0:0.000}", Math.Round(pendQty, 3));
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text), 3));
                txtActWght.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblACT_WGHT"))).Text), 2));
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRATE"))).Text), 2));

                ddlProcessType.SelectedValue = ((Label)(row.FindControl("lblProcessCode"))).Text;


                txtConvertQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblConQty"))).Text), 3));

                //txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAMT"))).Text), 2));
                //txtSubHeading.Text = ((Label)(row.FindControl("lblSubHeading"))).Text;
                //txtBatchNo.Text = ((Label)(row.FindControl("lblBatch"))).Text;
                //txtNoPackaeg.Text = (Convert.ToInt32(((Label)(row.FindControl("lblNoPack"))).Text)).ToString();
                //txtQtyPerPack.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblPakQty"))).Text), 2));
                //txtPackDesc.Text = ((Label)(row.FindControl("lblIND_PACK_DESC"))).Text;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceAddDetail_Deleting
    protected void dgInvoiceAddDetail_Deleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            int IsSuppliement = 0;
            if (chkIsSuppliement.Checked)
            {
                IsSuppliement = 1;
            }
            else
            {
                IsSuppliement = 0;
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Inv_No = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 AND INM_TYPE='OutSUBINM'");
                if (dt.Rows.Count > 0)
                {
                    Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                    Inv_No = Inv_No + 1;
                }

                //if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER (INM_CM_CODE,INM_NO,INM_DATE,INM_INVOICE_TYPE,INM_P_CODE,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_TAX_TCS,INM_TAX_TCS_AMT,INM_PACK_AMT,INM_G_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,INM_ACCESSIBLE_AMT,INM_TAXABLE_AMT,INM_ROUNDING_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','1','" + ddlCustomer.SelectedValue + "','" + txtNetAmount.Text + "','" + txtDiscPer.Text + "','" + txtDiscAmt.Text + "','" + txtBasicExcPer.Text + "','" + txtBasicExcAmt.Text + "','" + txtducexcper.Text + "','" + txtEdueceAmt.Text + "','" + txtSHEExcPer.Text + "','" + txtSHEExcAmt.Text + "','" + txtSalesTaxPer.Text + "','" + txtSalesTaxAmount.Text + "','0','" + txtTCSAmt.Text + "','" + txtPackAmt.Text + "','" + txtGrandAmt.Text + "','" + ddlTaxName.SelectedValue + "','" + txtstroreloc.Text + "','" + txtVechicleNo.Text + "','" + txtTransport.Text + "','" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "','" + txtRemark.Text + "','" + txtLRNo.Text + "','" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "','" + txtAccessableAmt.Text + "','" + txtTaxableAmt.Text + "','" + txtRoundingAmt.Text + "','" + txtOtherCharges.Text + "','" + txtFreight.Text + "','" + txtIncurance.Text + "','" + txtTransportAmt.Text + "','" + txtOctri.Text + "','" + IsSuppliement + "','" + txtIssuetime.Text + "','" + txtRemoveltime.Text + "')"))
                if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER(INM_CM_CODE,INM_NO,INM_DATE,INM_INVOICE_TYPE,INM_TYPE,INM_P_CODE,INM_VEH_NO,INM_ISSUE_DATE,INM_C_DAYS,INM_NATURE_PRO,INM_PREPARE_BY,INM_REMARK)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlConsPatt.SelectedIndex + "','OutSUBINM','" + ddlCustomer.SelectedValue + "','" + txtVehNo.Text + "','" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "','" + txtCreditDays.Text + "','" + txtNatureProcess.Text + "','" + txtPreparedBy.Text + "','" + txtSpecialInst.Text + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(INM_CODE) from INVOICE_MASTER");
                    for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                    {
                        //result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL(IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC) values ('" + Code + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["AMT"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_SUBHEADING"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_BACHNO"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_NO_PACK"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_PAK_QTY"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_EX_AMT"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CESS_AMT"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_SH_CESS_AMT"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_PACK_DESC"] + "')");
                        result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL(IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_CON_QTY,IND_PROCESS_CODE,IND_ACT_WEIGHT) values ('" + Code + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_CON_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PROCESS_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["ACT_WGHT"] + "')");
                        //CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH + " +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " where CPOD_CPOM_CODE='" +  ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");

                        if (!chkIsSuppliement.Checked)
                        {
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE SUPP_PO_DETAILS SET SPOD_DISPACH = ISNULL(SPOD_DISPACH,0)  + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE SPOD_SPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and SPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','OutSUBINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "')");
                            }
                            //Removing Stock
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }

                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_P_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlCustomer.SelectedValue + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PROCESS_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "',0,0,'" + Code + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutSUBINM',0)");
                            }
                        }
                    }

                    CommonClasses.WriteLog("DISPATCH TO SUB CONTRACTOR", "Save", "DISPATCH TO SUB CONTRACTOR", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);



                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    txtInvoiceNo.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_CM_CODE=" + Session["CompanyCode"] + ",INM_NO='" + txtInvoiceNo.Text + "', INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',INM_P_CODE='" + ddlCustomer.SelectedValue + "',INM_VEH_NO='" + txtVehNo.Text + "',INM_ISSUE_DATE='" + txtIssuedate.Text + "',INM_C_DAYS='" + txtCreditDays.Text + "',INM_NATURE_PRO='" + txtNatureProcess.Text + "',INM_PREPARE_BY='" + txtPreparedBy.Text + "',INM_REMARK='" + txtSpecialInst.Text + "' where INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {

                    DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");

                    for (int i = 0; i < dtq.Rows.Count; i++)
                    {
                        CommonClasses.Execute("UPDATE SUPP_PO_DETAILS set SPOD_DISPACH = ISNULL(SPOD_DISPACH,0)  - " + dtq.Rows[i]["IND_INQTY"] + " where SPOD_SPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and SPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                        CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");

                    }
                    result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and STL_DOC_TYPE='OutSUBINM'");
                    result = CommonClasses.Execute1("DELETE FROM CHALLAN_STOCK_LEDGER WHERE CL_DOC_ID = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND CL_DOC_TYPE='OutSUBINM'");

                    if (result)
                    {

                        for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                        {

                            result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_CON_QTY,IND_PROCESS_CODE,IND_ACT_WEIGHT) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_CON_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PROCESS_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["ACT_WGHT"] + "')");

                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE SUPP_PO_DETAILS SET SPOD_DISPACH = ISNULL(SPOD_DISPACH,0)  + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE SPOD_SPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and SPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                //result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "')");
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','OutSUBINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "')");

                            }
                            //Removing Stock
                            if (result == true)
                            {
                                //result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }

                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_P_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlCustomer.SelectedValue + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PROCESS_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "',0,0,'" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutSUBINM',0)");
                            }

                        }

                        //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");

                        CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Tax Invoice", "Update", "Tax Invoice", Convert.ToString(Convert.ToInt32(ViewState["mlCode"].ToString())), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    //   txtPONo.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region txtDate_TextChanged
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {

        }
        else
        {
            if (Convert.ToDateTime(txtDate.Text) >= DateTime.Now)
            {

            }
            else
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }

        txtIssuedate.Text = txtDate.Text;
        //txtremovaldate.Text = txtDate.Text;
    }
    #endregion

    #region txtIssuedate_TextChanged
    protected void txtIssuedate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {

        }
        else
        {
            if (Convert.ToDateTime(txtIssuedate.Text) >= DateTime.Now)
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtIssuedate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }
    }
    #endregion

    #region txtVQty_TextChanged
    protected void txtVQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string StrMask = DecimalMasking(txtVQty.Text);
            txtVQty.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(StrMask), 2)); ;
            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
            {//SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlCustomer.SelectedValue + "' 
                if (ddlConsPatt.SelectedIndex == 0)
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL  WHERE SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {

                        //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Focus();
                        return;
                    }
                }
                else
                {
                    DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + ddlPONo.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_ORDER_QTY>0");
                    if (dtCheckOpen.Rows.Count > 0)
                    {

                        //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Focus();
                        return;
                    }
                }
            }



            if (txtActWght.Text.Trim() == "")
            {
                txtActWght.Text = "0.00";
            }
            if (Convert.ToDouble(txtActWght.Text) > 0)
            {
                txtConvertQty.Text = (Convert.ToDouble(txtVQty.Text) * Convert.ToDouble(txtActWght.Text)).ToString();
            }
            else
            {
                txtConvertQty.Text = txtVQty.Text;
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
