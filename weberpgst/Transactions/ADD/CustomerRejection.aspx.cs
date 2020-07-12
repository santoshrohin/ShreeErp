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
using System.Collections.Generic;


public partial class Transactions_ADD_CustomerRejection : System.Web.UI.Page
{
    #region Variable
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    DataTable dtInw = new DataTable();

    static int mlCode = 0;
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    static string right = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
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
                    //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='16'");
                    //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    try
                    {
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        BindTable.Clear();
                        optLstType_SelectedIndexChanged(null, null);
                        LoadTaxName();
                        ViewState["mlCode"] = mlCode;
                        ViewState["str"] = str;
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

                            LoadCustomer();
                            //LoadICode();
                            //LoadIName();
                            //LoadPO();
                            // LoadTax();
                            // LoadCurr();
                            //optLstType.SelectedIndex = 0;


                            //ItemCode,ItemName,Unit,UnitCode,PO_Code,PO_No,Rate,OriginalQty,ChallanQty,ReceivedQty,Amount

                            LoadFilter();

                            txtChallanDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtGINDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                            txtChallanDate.Attributes.Add("readonly", "readonly");
                            txtGINDate.Attributes.Add("readonly", "readonly");

                            txtInvDate.Attributes.Add("readonly", "readonly");
                        }

                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Rejection", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "Page_Load", ex.Message.ToString());
        }
    }

    private void LoadFilter()
    {
        if (dgCustomerRejection.Rows.Count == 0)
        {
            dtInwardDetail.Clear();

            if (dtInwardDetail.Columns.Count == 0)
            {
                dgCustomerRejection.Enabled = false;
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("UnitCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_Code", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_No", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("OriginalQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ChallanQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ReceivedQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Amount", typeof(String)));

                dtInwardDetail.Columns.Add(new System.Data.DataColumn("E_BASIC", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("E_EDU_CESS", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("E_H_EDU", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ExisDuty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("EduCess", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("SHECess", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO1", typeof(String)));

                dtInwardDetail.Rows.Add(dtInwardDetail.NewRow());
                dgCustomerRejection.DataSource = dtInwardDetail;
                dgCustomerRejection.DataBind();
            }
        }
    }
    #endregion

    private void LoadTaxName()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlTaxName.DataSource = dt;
            ddlTaxName.DataTextField = "ST_TAX_NAME";
            ddlTaxName.DataValueField = "ST_CODE";
            ddlTaxName.DataBind();
            ddlTaxName.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            ddlTaxName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }
    }



    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetInvoiceNo(string prefixText)
    //{
    //    DataTable dt = new DataTable();
    //    dt = CommonClasses.Execute("select INM_NO from INVOICE_MASTER, PARTY_MASTER where INM_P_CODE=P_CODE and P_CODE=" + ddlCustomer.SelectedValue + " and and INM_NO like '%"+txtInvoiceNo.Text+"%'");
    //    List<string> INV_NO = new List<string>();
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        INV_NO.Add(dt.Rows[i][1].ToString());
    //    }
    //    return INV_NO;
    //}
    public void LoadInVNo(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();
            if (ddlCustomer.SelectedIndex != 0)
            {
                if (txtString.Text != "")
                    dtfilter = CommonClasses.Execute("select INM_NO from INVOICE_MASTER, PARTY_MASTER where INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and P_CODE=" + ddlCustomer.SelectedValue + " and INM_NO like '%" + str + "%'");
                //dtfilter = CommonClasses.Execute("select CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and (P_NAME like upper('%" + str + "%') OR convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%') OR CR_GIN_NO like upper('%" + str + "%') OR CR_CHALLAN_NO like upper('%" + str + "%') OR convert(varchar,CR_CHALLAN_DATE,106) like upper('%" + str + "%') OR CR_P_CODE like upper('%" + str + "%')) order by CR_GIN_NO desc");
                else
                    dtfilter = CommonClasses.Execute("select INM_NO from INVOICE_MASTER, PARTY_MASTER where INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and P_CODE=" + ddlCustomer.SelectedValue + "");
                //dtfilter = CommonClasses.Execute("select CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by CR_GIN_NO desc");
                //lstview.DataSource = dtfilter;
                //lstview.DataTextField = "INM_NO";
                //lstview.DataValueField = "INM_NO";
                //lstview.DataBind();
                lstview.Items.Clear();
                if (dtfilter.Rows.Count != 0)
                {
                    lstview.Visible = true;
                    for (int i = 0; i < dtfilter.Rows.Count; i++)
                    {
                        lstview.Items.Add(dtfilter.Rows[i][0].ToString());
                    }
                }
                else
                    lstview.Visible = false;
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadStatus", ex.Message);
        }
    }
    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            //            dt = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO,IND_RATE from CUSTPO_MASTER,INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where CPOM_CODE=IND_CPOM_CODE and I_CODE=IND_I_CODE and IND_INM_CODE=INM_CODE and INM_DATE='" + txtInvDate.Text + "' and INM_NO=" + txtInvoiceNo.Text + " and I_CODE=" + ddlItemCode.SelectedValue);
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO,CPOD_RATE,CPOD_ORD_QTY,IND_REFUNDABLE_QTY from CUSTPO_MASTER,INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE and CPOM_CODE=IND_CPOM_CODE and I_CODE=CPOD_I_CODE and I_CODE=IND_I_CODE and IND_INM_CODE=INM_CODE  and INM_NO=" + txtInvoiceNo.Text + " and I_CODE=" + ddlItemCode.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txtOrigionalQty.Text = dt.Rows[0]["IND_REFUNDABLE_QTY"].ToString();
                }
                else
                {

                }
            }
            else
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOM_PONO,CPOM_P_CODE from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE and I_CODE=" + ddlItemCode.SelectedValue + "");
                txtOrigionalQty.Text = "";
            }

            ddlPONo.Items.Clear();
            ddlPONo.SelectedIndex = -1;
            ddlPONo.SelectedValue = null;
            ddlPONo.ClearSelection();


            txtChallanQty.Text = "0.000";
            txtReceivedQty.Text = "0.000";

            if (dt.Rows.Count > 0)
            {
                txtRate.Text = dt.Rows[0]["CPOD_RATE"].ToString();
                ddlPONo.DataSource = dt;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();
                ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadPO", Ex.Message);
        }

    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            //BL_Customer = new CustomerPO_BL(ml_code);
            //BL_Customer.CPOM_CM_COMP_ID = Convert.ToInt32(Session["CompanyCode"]);
            LoadCustomer();

            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGINDate.Attributes.Add("readonly", "readonly");

            dtInwardDetail.Clear();

            dt = CommonClasses.Execute("SELECT CR_CODE,CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,cast((CR_BASIC_EXCISE) as numeric(20,2)) AS CR_BASIC_EXCISE,cast((CR_EDU_CESS) as numeric(20,2)) AS CR_EDU_CESS,cast((CR_H_EDU_CESS) as numeric(20,2)) AS CR_H_EDU_CESS,cast((CR_SALES_TAX) as numeric(20,2)) AS CR_SALES_TAX,cast((CR_GRAND_TOTAL) as numeric(20,2)) AS CR_GRAND_TOTAL,CR_ST_CODE FROM CUSTREJECTION_MASTER where CR_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["CR_CODE"].ToString()); ;
                optLstType.SelectedIndex = (Convert.ToBoolean(dt.Rows[0]["CR_TYPE"].ToString()) == false) ? 0 : 1;
                if (optLstType.SelectedIndex == 1)
                {
                    txtInvoiceNo.Enabled = false;
                    txtInvDate.Enabled = false;
                }
                else
                {
                    txtInvoiceNo.Enabled = true;
                    txtInvDate.Enabled = true;
                }
                optLstType_SelectedIndexChanged(null, null);
                txtGINNo.Text = dt.Rows[0]["CR_GIN_NO"].ToString();
                txtChallanNo.Text = dt.Rows[0]["CR_CHALLAN_NO"].ToString();
                txtGINDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_GIN_DATE"]).ToString("dd MMM yyyy");
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_CHALLAN_DATE"]).ToString("dd MMM yyyy");

                ddlCustomer.SelectedValue = dt.Rows[0]["CR_P_CODE"].ToString();
                //if (dt.Rows[0]["CR_INV_NO"].ToString() != "0" && dt.Rows[0]["CR_INV_NO"].ToString() == "")

                txtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_INV_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceNo.Text = dt.Rows[0]["CR_INV_NO"].ToString();
                //txtBasicExcise.Text = string.Format("{0:0.00}",dt.Rows[0]["CR_BASIC_EXCISE"].ToString());
                txtBasicExcise.Text = dt.Rows[0]["CR_BASIC_EXCISE"].ToString();
                txtHigherEduCess.Text = dt.Rows[0]["CR_H_EDU_CESS"].ToString();
                txtEduCess.Text = (dt.Rows[0]["CR_EDU_CESS"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_EDU_CESS"].ToString());
                ddlTaxName.SelectedValue = (dt.Rows[0]["CR_ST_CODE"].ToString() == "" ? "0" : dt.Rows[0]["CR_ST_CODE"].ToString());
                txtSalesTax.Text = (dt.Rows[0]["CR_SALES_TAX"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_SALES_TAX"].ToString());
                txtGrandAmt.Text = (dt.Rows[0]["CR_GRAND_TOTAL"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_GRAND_TOTAL"].ToString());
                LoadICode();
                LoadIName();

                dtInwardDetail = CommonClasses.Execute("SELECT CD_I_CODE as ItemCode,I_NAME as ItemName,I_UOM_NAME as Unit, CD_UOM as UnitCode,CD_PO_CODE as PO_Code,CPOM_PONO as PO_No, cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount,isnull(CD_EXC_PER,0) AS E_BASIC,isnull(CD_EDU_CESS_PER,0) AS E_EDU_CESS, isnull(CD_H_EDU_CESS,0) AS E_H_EDU,ISNULL(CD_CENTRAL_TAX_AMT,0) as ExisDuty,isnull(CD_STATE_TAX_AMT,0) as EduCess,isnull(CD_INTEGRATED_TAX_AMT,0) as SHECess, isnull(CD_E_TARRIF_CODE,0) as E_TARIFF_NO1 FROM CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE and CD_CR_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");



                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgCustomerRejection.Enabled = true;
                    dgCustomerRejection.DataSource = dtInwardDetail;
                    dgCustomerRejection.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                    GetTotal();
                    txtNetAmount_TextChanged(null, null);
                }
            }
            if (optLstType.SelectedValue == "1")
            {
                txtInvoiceNo.Enabled = true;
            }
            else
            {

            }
            if (ViewState["str"].ToString() == "VIEW")
            {
                ddlPONo.Enabled = false;
                txtGINDate.Enabled = false;
                txtInvDate.Enabled = false;
                txtInvoiceNo.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtRate.Enabled = false;
                txtUnit.Enabled = false;
                txtNetAmount.Enabled = false;
                txtChallanQty.Enabled = false;
                txtReceivedQty.Enabled = false;
                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                txtChallanNo.Enabled = false;
                txtChallanDate.Enabled = false;
                dgCustomerRejection.Enabled = false;

            }
            else if (ViewState["str"].ToString() == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection Transaction", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            // dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO from INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where INM_NO=" + txtInvoiceNo.Text + " and IND_INM_CODE=INM_CODE  AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE=0 and I_CODE=IND_I_CODE");
            else
                dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE");

            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            //ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                dt = CommonClasses.Execute("select I_CODE,I_NAME from INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where INM_NO=" + txtInvoiceNo.Text + " and IND_INM_CODE=INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV'  and I_CODE=IND_I_CODE");
            else
                dt = CommonClasses.Execute("select I_CODE,I_NAME from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE     AND CPOD_I_CODE=I_CODE");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            //ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadIName();
        LoadICode();
    }
    protected void ddlItemCode_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                LoadPO();
                if (ddlPONo.Items.Count > 0)
                {
                    if (txtInvoiceNo.Text != "" && txtInvDate.Text != "")
                        ddlPONo.SelectedIndex = 1;
                }
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

                if (dt1.Rows.Count > 0)
                {
                    txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                }
                else
                {
                    txtUnit.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                LoadPO();
                if (ddlPONo.Items.Count > 0)
                {
                    //if (txtInvoiceNo.Text != "" && txtInvDate.Text != "")
                    ddlPONo.SelectedIndex = 1;
                }
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtitemcode = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_NAME=" + ddlItemName.Text + "");

                if (dt1.Rows.Count > 0)
                {
                    txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                }
                else
                {
                    txtUnit.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            //            dt = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO,IND_RATE from CUSTPO_MASTER,INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where CPOM_CODE=IND_CPOM_CODE and I_CODE=IND_I_CODE and IND_INM_CODE=INM_CODE and INM_DATE='" + txtInvDate.Text + "' and INM_NO=" + txtInvoiceNo.Text + " and I_CODE=" + ddlItemCode.SelectedValue);
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO,CPOD_RATE,CPOD_ORD_QTY,IND_REFUNDABLE_QTY from CUSTPO_MASTER,INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE and CPOM_CODE=IND_CPOM_CODE and I_CODE=CPOD_I_CODE and I_CODE=IND_I_CODE and IND_INM_CODE=INM_CODE  and INM_NO=" + txtInvoiceNo.Text + " and I_CODE=" + ddlItemCode.SelectedValue + " and CPOM_CODE="+ddlPONo.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txtOrigionalQty.Text = dt.Rows[0]["IND_REFUNDABLE_QTY"].ToString();
                }
                else
                {

                }
            }
            else
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOM_PONO,CPOM_P_CODE from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE and I_CODE=" + ddlItemCode.SelectedValue + " and CPOM_CODE=" + ddlPONo.SelectedValue);
                txtOrigionalQty.Text = "";
            }

           


            txtChallanQty.Text = "0.000";
            txtReceivedQty.Text = "0.000";

            if (dt.Rows.Count > 0)
            {
                txtRate.Text = dt.Rows[0]["CPOD_RATE"].ToString();
                
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadPO", Ex.Message);
        }
    }
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {

    }
    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {

        try
        {
            #region validation
            if (txtGINDate.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter GIN Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter GIN Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtGINDate.Focus();
                return;
            }
            if (txtChallanNo.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter Challan No", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanNo.Focus();
                return;
            }
            if (txtChallanDate.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter Challan Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanDate.Focus();
                return;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer";
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
            if (Convert.ToDouble(txtReceivedQty.Text) == 0)
            {
                //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Received Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReceivedQty.Focus();
                return;
            }


            if (txtChallanQty.Text == "0.00")
            {
                //ShowMessage("#Avisos", "Please Enter Challan Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
                return;
            }
            if (optLstType.SelectedValue == "1")
            {

                if (txtInvoiceNo.Text == "")
                {
                    //ShowMessage("#Avisos", "Please Enter Invoice No", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Invoice No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInvoiceNo.Focus();
                    return;
                }
                if (txtInvDate.Text == "")
                {
                    //ShowMessage("#Avisos", "Please Enter Invoice Date", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Invoice Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInvDate.Focus();
                    return;
                }
            }
            if (optLstType.SelectedValue == "2")
            {
                if (ddlPONo.SelectedIndex == 0)
                {
                    //ShowMessage("#Avisos", "Select PO", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPONo.Focus();
                    return;
                }
            }
            if (optLstType.SelectedValue == "1")
            {
                if (ddlPONo.SelectedIndex == 0)
                {
                    //ShowMessage("#Avisos", "Select PO", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPONo.Focus();
                    return;
                }
            }

            Double Subamount = Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtChallanQty.Text)), 2);
            double E_BASIC = 0;
            double E_EDU_CESS = 0;
            double E_H_EDU = 0;
            double ExisDuty = 0;
            double EduCess = 0;
            double SHECess = 0;
            int E_CODE = 0;
            //if (!chkExcInclusive.Checked)
            //{
            dt = CommonClasses.Execute("select E_CODE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU from EXCISE_TARIFF_MASTER,ITEM_MASTER where I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString());//Central
                E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString());//state
                E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString());//Integrated
                E_CODE = Convert.ToInt32(dt.Rows[0]["E_CODE"].ToString());//ECode
            }
            //}
            //E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"]);
            
            DataTable dtGSTcal = CommonClasses.Execute("select P_CODE,P_NAME,P_SM_CODE,CM_STATE from PARTY_MASTER,COMPANY_MASTER where P_TYPE=1 and CM_ID=P_CM_COMP_ID and PARTY_MASTER.ES_DELETE=0 and P_CODE='" + ddlCustomer.SelectedValue + "'");
            if (dtGSTcal.Rows.Count > 0)
            {
                int PState = Convert.ToInt32(dtGSTcal.Rows[0]["P_SM_CODE"]);
                int CState = Convert.ToInt32(dtGSTcal.Rows[0]["CM_STATE"]);
                if (PState == CState)
                {
                    ExisDuty = Subamount * Convert.ToDouble(dt.Rows[0]["E_BASIC"]) / Convert.ToDouble(100);
                    EduCess = Subamount * Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"]) / Convert.ToDouble(100);
                }
                else
                {
                    SHECess = Subamount * Convert.ToDouble(dt.Rows[0]["E_H_EDU"]) / Convert.ToDouble(100);
                }
            }


            if (dgCustomerRejection.Rows.Count > 0)
            {
                for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            // ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region datatable structure
            PanelMsg.Visible = false;
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");

                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                ((DataTable)ViewState["dt2"]).Columns.Add("UnitCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_Code");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_No");

                ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
                ((DataTable)ViewState["dt2"]).Columns.Add("OriginalQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("ChallanQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("ReceivedQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Amount");

                ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
                ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
                ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
                ((DataTable)ViewState["dt2"]).Columns.Add("ExisDuty");
                ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
                ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");
                ((DataTable)ViewState["dt2"]).Columns.Add("E_TARIFF_NO1");

            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            // dr["ShortName"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["Unit"] = txtUnit.Text;
            dr["UnitCode"] = lblUnit.Text;
            if (txtOrigionalQty.Text == "")
            {
                txtOrigionalQty.Text = "0.000";
            }
            else
            {

            }
            if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "")
            {
                dr["OriginalQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtOrigionalQty.Text)), 3));
            }
            else
            {
                dr["OriginalQty"] = "0.000";

            }
            if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
            {
                //ShowMessage("#Avisos", "Received Qty should be less than or equal to Challan Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
            }
            else
                dr["ChallanQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtChallanQty.Text)), 3));

            if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "")
            {
                if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
                {
                    //ShowMessage("#Avisos", "Challan Qty should be less than or equal to Original Qty", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Challan Qty should be less than or equal to Original Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtChallanQty.Focus();
                    return;
                }
            }
            dr["ReceivedQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtReceivedQty.Text)), 3));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            if (ddlPONo.SelectedIndex > 0)
            {
                dr["PO_Code"] = ddlPONo.SelectedValue;
                dr["PO_No"] = ddlPONo.SelectedItem;
            }

            dr["Amount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtChallanQty.Text)), 2));
            dr["ExisDuty"] = Math.Round(ExisDuty, 2);
            dr["EduCess"] = Math.Round(EduCess, 2);
            dr["SHECess"] = Math.Round(SHECess, 2);
            dr["E_BASIC"] = E_BASIC;
            dr["E_EDU_CESS"] = E_EDU_CESS;
            dr["E_H_EDU"] = E_H_EDU;
            dr["E_TARIFF_NO1"] = E_CODE;
            #endregion


            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.Visible = true;
                dgCustomerRejection.DataSource = ((DataTable)ViewState["dt2"]);
                dgCustomerRejection.DataBind();
            }
            #endregion



            GetTotal();
            txtNetAmount_TextChanged(null, null);
            CalGrandTotal();
            clearDetail();
            ddlPONo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {

        }

    }
    #endregion


    #region GetTotal
    private void GetTotal()
    {
        //double decTotal = 0;
        //double EBasicPer = 0;
        //double EEduCessPer = 0;
        //double EHEduCessPer = 0;
        //double EBasic = 0;
        //double EEduCess = 0;
        //double EHEduCess = 0;
        //if (dgCustomerRejection.Rows.Count > 0)
        //{


        //    for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
        //    {
        //        string QED_AMT = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblAmount"))).Text;
        //        string Icode = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblItemCode"))).Text;
        //        double Amount = Convert.ToDouble(QED_AMT);

        //        if (i == 0)
        //        {
        //            DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + Icode + "");
        //            if (dtExcisePer.Rows.Count > 0)
        //            {




        //                DataTable dtCompState = new DataTable();
        //                DataTable dtCustState = new DataTable();
        //                dtCompState = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_CODE='" + Session["CompanyCode"] + "' AND ISNULL(CM_DELETE_FLAG,0)='0' AND CM_ID= '" + Session["CompanyId"] + "'");
        //                dtCustState = CommonClasses.Execute("SELECT P_SM_CODE,* FROM CUSTPO_MASTER INNER JOIN PARTY_MASTER ON CUSTPO_MASTER.CPOM_P_CODE=PARTY_MASTER.P_CODE WHERE CUSTPO_MASTER.CPOM_CODE='" + ddlPONo.SelectedValue + "' AND CUSTPO_MASTER.CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CUSTPO_MASTER.ES_DELETE=0");

        //                if (dtCompState.Rows[0]["CM_STATE"].ToString() == dtCustState.Rows[0]["P_SM_CODE"].ToString())
        //                {
        //                    EHEduCessPer = 0;
        //                    EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
        //                    EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());

        //                    txtSHEExcPer.Text = "0.00";
        //                    txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(EBasicPer, 2));
        //                    txtducexcper.Text = string.Format("{0:0.00}", Math.Round(EEduCessPer, 2));
        //                }
        //                else
        //                {
        //                    EBasicPer = 0;
        //                    EEduCessPer = 0;

        //                    EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
        //                    txtBasicExcPer.Text = "0.00";
        //                    txtducexcper.Text = "0.00";
        //                    txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(EHEduCessPer, 2));
        //                }

        //                //txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(EBasicPer, 2));
        //                //txtducexcper.Text = string.Format("{0:0.00}", Math.Round(EEduCessPer, 2));
        //                //txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(EHEduCessPer, 2));
        //            }
        //        }


        //        EBasic = EBasic + Math.Round(((Amount * EBasicPer) / 100), 2);
        //        EEduCess = EEduCess + Math.Round(Amount * EEduCessPer / 100, 2);
        //        EHEduCess = EHEduCess + Math.Round(Amount * EHEduCessPer / 100, 2);
        //        //ExcBasic = ExcBasic + Convert.ToDouble(Basic);
        //        //Excedu = Excedu + Convert.ToDouble(EduCess);
        //        //ExcSH = ExcSH + Convert.ToDouble(SH);

        //        decTotal = decTotal + Amount;

        //    }
        //}

        

        double decTotal = 0;
        double CGST_AMT = 0, SGST_AMT = 0, IGST_AMT = 0;
        if (dgCustomerRejection.Rows.Count > 0)
        {
            for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblAmount"))).Text;

                double Amount = Convert.ToDouble(QED_AMT);

                decTotal = decTotal + Amount;

                string CGST = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblExisDuty"))).Text;
                string SGST = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblEduCess"))).Text;
                string IGST = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblSHECess"))).Text;

                CGST_AMT = CGST_AMT + Convert.ToDouble(CGST);
                SGST_AMT = SGST_AMT + Convert.ToDouble(SGST);
                IGST_AMT = IGST_AMT + Convert.ToDouble(IGST);
            }
        }
        double Totaltax = CGST_AMT + SGST_AMT + IGST_AMT;
        txtBasicExcise.Text = CGST_AMT.ToString();
        txtEduCess.Text = SGST_AMT.ToString();
        txtHigherEduCess.Text = IGST_AMT.ToString();
        txtNetAmount.Text = string.Format("{0:0.00}", decTotal);
        txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(decTotal + Totaltax), 2);

        if (dgCustomerRejection.Enabled)
        {
            //string ICode = ((Label)(dgInvoiceAddDetail.Rows[0].FindControl("lblIND_I_CODE"))).Text;
            //DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + ICode + "");
            //if (dtExcisePer.Rows.Count > 0)
            //{
            //    EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
            //    EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
            //    EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
            //}
        }
        else
        {
            txtNetAmount.Text = "0.00";
            //txtDiscAmt.Text = "0.00";
            txtBasicExcise.Text = "0.00";
            txtEduCess.Text = "0.00";
            txtHigherEduCess.Text = "0.00";

        }


        //if (txtNetAmount.Text == "")
        //{
        //    txtNetAmount.Text = "0.00";
        //}



        //if (txtBasicExcise.Text == "")
        //{
        //    txtBasicExcise.Text = "0.00";
        //}
        //if (txtBasicExcPer.Text == "")
        //{
        //    txtBasicExcPer.Text = "0.00";
        //}

        //if (txtducexcper.Text == "")
        //{
        //    txtducexcper.Text = "0.00";
        //}
        //if (txtSHEExcPer.Text == "")
        //{
        //    txtSHEExcPer.Text = "0.00";
        //}


        ////if (txtFreight.Text == "")
        ////{
        ////    txtFreight.Text = "0";
        ////}
        if (txtSalesTaxPer.Text == "")
        {
            txtSalesTaxPer.Text = "0.00";
        }

        if (txtSalesTax.Text == "")
        {
            txtSalesTax.Text = "0.00";
        }


        ////if (txtOtherAmount.Text == "")
        ////{
        ////    txtOtherAmount.Text = "0";
        ////}
        //if (txtGrandAmt.Text == "")
        //{
        //    txtGrandAmt.Text = "0.00";
        //}
        //txtBasicExcise.Text = string.Format("{0:0.00}", Math.Round(EBasic, 2));
        //txtEduCess.Text = string.Format("{0:0.00}", Math.Round(EEduCess, 2));
        //txtHigherEduCess.Text = string.Format("{0:0.00}", Math.Round(EHEduCess, 2));

        ////if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
        ////{
        ////    DataTable dtSTAX = new DataTable();
        ////    dtSTAX = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from INVOICE_MASTER,SALES_TAX_MASTER where ST_CODE=INM_T_CODE AND INM_NO=" + txtInvoiceNo.Text);
        ////    if (dtSTAX.Rows.Count > 0)
        ////    {
        ////        txtSalesTax.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtNetAmount.Text)+Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(dtSTAX.Rows[0]["ST_SALES_TAX"].ToString()) / 100)));
        ////        txtSalesTaxPer.Text = dtSTAX.Rows[0]["ST_SALES_TAX"].ToString();
        ////        ddlTaxName.SelectedValue = dtSTAX.Rows[0]["ST_CODE"].ToString();
        ////    }
        ////}
        ////else
        ////{
        ////    DataTable dtSTAX = new DataTable();
        ////    if (ddlTaxName.SelectedIndex > 0)
        ////    {
        ////        dtSTAX = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from SALES_TAX_MASTER where ST_CODE=" + ddlTaxName.SelectedValue);
        ////        txtSalesTaxPer.Text = dtSTAX.Rows[0]["ST_SALES_TAX"].ToString();
        ////        txtSalesTax.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text)) * Convert.ToDouble(txtSalesTaxPer.Text) / 100), 2));
        ////    }
        ////}




        ////txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text) + Convert.ToDouble(txtEduCess.Text) + Convert.ToDouble(txtHigherEduCess.Text) + Convert.ToDouble(txtSalesTax.Text) + Convert.ToDouble(txtPackAmt.Text)) - Convert.ToDouble(txtDiscAmt.Text)), 2);
        ////if (optLstType.SelectedIndex == 0)
        ////{
        ////txtBasicExcise.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * 0.12));
        ////    txtEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text) * 0.01));
        ////    txtHigherEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text) * 0.02));
        ////    txtSalesTax.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * 0.125));
        ////}
        //CalGrandTotal();
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            txtUnit.Text = "";
            txtOrigionalQty.Text = "0.00";
            txtRate.Text = "0.00";
            txtReceivedQty.Text = "0.00";
            txtChallanQty.Text = "0.00";
            //ddlPONo.SelectedIndex = 0;
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        //if (ddlTaxName.SelectedIndex == 0)
        //{
        //    //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "Select Tax";
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //    ddlCustomer.Focus();
        //    return;
        //}


        if (dgCustomerRejection.Rows.Count != 0)
        {
            SaveRec();
        }
        else
        {
            //ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found In Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            //dgCustomerRejection.DataSource = dt2;
            //dgCustomerRejection.DataBind();
            if (Request.QueryString[0].Equals("INSERT"))
            {
                string strSql = "";
                int ginNo = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(CR_GIN_NO),0) as CR_GIN_NO FROM CUSTREJECTION_MASTER WHERE CR_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    ginNo = Convert.ToInt32(dt.Rows[0]["CR_GIN_NO"]);
                    ginNo = ginNo + 1;
                }
                //string ginNo = (Convert.ToInt32(CommonClasses.GetMaxId("Select Max(isnull(CR_GIN_NO,0)) from CUSTREJECTION_MASTER"))+1).ToString();
                if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                {
                    strSql = "INSERT INTO CUSTREJECTION_MASTER(CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,CR_BASIC_EXCISE,CR_EDU_CESS,CR_H_EDU_CESS,CR_CM_COMP_ID,MODIFY,ES_DELETE,CR_GRAND_TOTAL,CR_SALES_TAX,CR_ST_CODE,CR_CM_CODE)VALUES('" + optLstType.SelectedIndex + "'," + ginNo + ",'" + txtGINDate.Text + "','" + txtChallanNo.Text + "','" + txtChallanDate.Text + "'," + txtInvoiceNo.Text + ",'" + txtInvDate.Text + "'," + ddlCustomer.SelectedValue + "," + txtNetAmount.Text + "," + txtBasicExcise.Text + "," + txtEduCess.Text + "," + txtHigherEduCess.Text + ",'" + Convert.ToInt32(Session["CompanyId"]) + "',0,0," + txtGrandAmt.Text + "," + txtSalesTax.Text + "," + ddlTaxName.SelectedValue + "," + Convert.ToInt32(Session["CompanyCode"]) + ")";

                }
                else
                {
                    strSql = "INSERT INTO CUSTREJECTION_MASTER(CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,CR_BASIC_EXCISE,CR_EDU_CESS,CR_H_EDU_CESS,CR_CM_COMP_ID,MODIFY,ES_DELETE,CR_GRAND_TOTAL,CR_SALES_TAX,CR_ST_CODE,CR_CM_CODE)VALUES('" + optLstType.SelectedIndex + "'," + ginNo + ",'" + txtGINDate.Text + "','" + txtChallanNo.Text + "','" + txtChallanDate.Text + "','','" + System.DateTime.Now.ToString("dd MMM yyyy") + "'," + ddlCustomer.SelectedValue + "," + txtNetAmount.Text + "," + txtBasicExcise.Text + "," + txtEduCess.Text + "," + txtHigherEduCess.Text + ",'" + Convert.ToInt32(Session["CompanyId"]) + "',0,0," + txtGrandAmt.Text + "," + txtSalesTax.Text + "," + ddlTaxName.SelectedValue + "," + Convert.ToInt32(Session["CompanyCode"]) + ")";
                }
                if (CommonClasses.Execute1(strSql))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(CR_CODE) from CUSTREJECTION_MASTER");
                    for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO CUSTREJECTION_DETAIL(CD_CR_CODE,CD_I_CODE,CD_UOM,CD_PO_CODE,CD_RATE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_AMOUNT,CD_EXC_PER,CD_EDU_CESS_PER,CD_H_EDU_CESS,CD_CENTRAL_TAX_AMT,CD_STATE_TAX_AMT,CD_INTEGRATED_TAX_AMT,CD_E_TARRIF_CODE) VALUES(" + Code + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblPO_Code")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblAmount")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblSHECess")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_TARIFF_NO1")).Text + ")");
                        if (optLstType.SelectedValue == "1")
                        {
                            if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0" && ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text != "0.000")
                            {
                                double N1 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                                double N2 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                                double N3 = N1 - N2;
                                CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N2 + " WHERE IND_I_CODE IN (SELECT IND_I_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " and IND_I_CODE=" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + ")");
                            }

                        }
                        //  CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N2 + " WHERE IND_I_CODE IN (SELECT IND_I_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " and IND_I_CODE=" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + ")");
                        CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+ " + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + " WHERE I_CODE ='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "'  ");

                        CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + ginNo + "','IWIC','" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + "')");
                    }
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    CommonClasses.WriteLog("Customer Rejection", "Save", "Customer Rejection", ginNo.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                    Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
                }
                else
                {
                    //ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE CUSTREJECTION_MASTER SET CR_TYPE='" + optLstType.SelectedIndex + "',CR_GIN_NO=" + txtGINNo.Text + ",CR_GIN_DATE='" + txtGINDate.Text + "',CR_CHALLAN_NO=" + txtChallanNo.Text + ",CR_CHALLAN_DATE='" + txtChallanDate.Text + "',CR_INV_NO=" + txtInvoiceNo.Text + ",CR_INV_DATE='" + txtInvDate.Text + "',CR_P_CODE=" + ddlCustomer.SelectedValue + ",CR_NET_AMT=" + txtNetAmount.Text + ",CR_BASIC_EXCISE=" + txtBasicExcise.Text + ",CR_EDU_CESS=" + txtEduCess.Text + ",CR_H_EDU_CESS=" + txtHigherEduCess.Text + ",CR_GRAND_TOTAL=" + txtGrandAmt.Text + ", CR_SALES_TAX = " + txtSalesTax.Text + ", CR_ST_CODE = " + ddlTaxName.SelectedValue + "  where CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    DataTable DtRej = new DataTable();

                    DtRej = CommonClasses.Execute("SELECT CD_I_CODE,CD_PO_CODE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND  CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND CR_INV_NO='" + txtInvoiceNo.Text + "' AND CR_INV_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'");
                    for (int p = 0; p < DtRej.Rows.Count; p++)
                    {
                        if (optLstType.SelectedValue == "1")
                        {
                            CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY+ " + DtRej.Rows[p]["CD_RECEIVED_QTY"].ToString() + " WHERE IND_I_CODE ='" + DtRej.Rows[p]["CD_I_CODE"].ToString() + "' AND  IND_INM_CODE IN (SELECT IND_INM_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " and IND_I_CODE=" + DtRej.Rows[p]["CD_I_CODE"].ToString() + ")");
                        }
                        CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL- " + DtRej.Rows[p]["CD_RECEIVED_QTY"].ToString() + " WHERE I_CODE ='" + DtRej.Rows[p]["CD_I_CODE"].ToString() + "'  ");

                    }

                    result = CommonClasses.Execute1("DELETE FROM CUSTREJECTION_DETAIL WHERE CD_CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND STL_DOC_TYPE='IWIC'");

                    if (result)
                    {

                        for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                        {
                            //CommonClasses.Execute1("INSERT INTO CUSTREJECTION_DETAIL(CD_CR_CODE,CD_I_CODE,CD_UOM,CD_PO_CODE,CD_RATE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_AMOUNT)VALUES(" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblPO_Code")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblAmount")).Text + ")");
                            CommonClasses.Execute1("INSERT INTO CUSTREJECTION_DETAIL(CD_CR_CODE,CD_I_CODE,CD_UOM,CD_PO_CODE,CD_RATE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_AMOUNT,CD_EXC_PER,CD_EDU_CESS_PER,CD_H_EDU_CESS,CD_CENTRAL_TAX_AMT,CD_STATE_TAX_AMT,CD_INTEGRATED_TAX_AMT,CD_E_TARRIF_CODE) VALUES(" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblPO_Code")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblAmount")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblSHECess")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblE_TARIFF_NO1")).Text + ")");
                            if (optLstType.SelectedValue == "1")
                            {
                                if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0" && ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text != "")
                                {
                                    double N1 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                                    double N2 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                                    double N3 = N1 - N2;
                                    CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N2 + " WHERE IND_I_CODE='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "' AND IND_INM_CODE IN (SELECT IND_INM_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " )");

                                }
                            }
                            CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+ " + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + " WHERE I_CODE ='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "'  ");

                            CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtGINNo.Text + "','IWIC','" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + "')");

                        }

                        //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");

                        CommonClasses.RemoveModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Customer Rejection", "Update", "Customer Rejection", txtGINNo.ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
                }
                else
                {
                    // ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invalid Update";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

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
                   // ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            // SaveRec();
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Customer Rejection", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtGINDate.Text == "")
            {
                flag = false;
            }
            else if (txtChallanNo.Text == "")
            {
                flag = false;
            }
            else if (txtChallanDate.Text == "")
            {
                flag = false;
            }
            else if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (dgCustomerRejection.Rows.Count > 0)
            {
                flag = true;
            }
            else
            {

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    //#region ShowMessage
    //public bool ShowMessage(string DiveName, string Message, string MessageType)
    //{
    //    try
    //    {
    //        if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
    //        {
    //            Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
    //        }
    //        return true;
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Customer Rejection ", "ShowMessage", Ex.Message);
    //        return false;
    //    }
    //}
    //#endregion

    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dtCheckInv = CommonClasses.Execute("select * from INVOICE_MASTER where INM_P_CODE=" + ddlCustomer.SelectedValue + "    AND INM_TYPE='TAXINV' and INM_NO=" + txtInvoiceNo.Text.Trim() + "");
        if (dtCheckInv.Rows.Count <= 0)
        {
            //ShowMessage("#Avisos", "Please enter valid invoice no", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter valid invoice no";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //txtInvoiceNo.Focus();
        }
        else
        {
            PanelMsg.Visible = false;
            LoadIName();
            LoadICode();
        }
        //LoadInVNo(txtInvoiceNo);
    }


    protected void txtInvDate_TextChanged(object sender, EventArgs e)
    {
        DataTable dtCheckInvDae = CommonClasses.Execute("select * from INVOICE_MASTER where INM_P_CODE=" + ddlCustomer.SelectedValue + " and INM_NO=" + txtInvoiceNo.Text.Trim() + " and INM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'");
        if (dtCheckInvDae.Rows.Count <= 0)
        {
            //ShowMessage("#Avisos", "Please enter valid invoice Date", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter valid invoice Date";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtInvDate.Focus();
        }
        else
        {
            LoadICode();
            LoadIName();


        }
    }

    protected void optLstType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkSelection();
    }


    public void checkSelection()
    {
        if (optLstType.SelectedIndex == 0)
        {
            txtInvDate.Enabled = true;
            txtInvoiceNo.Enabled = true;
            //ddlPONo.Enabled = false;
            ddlTaxName.Enabled = false;
            txtBasicExcPer.Enabled = false;
            txtSHEExcPer.Enabled = false;
            txtducexcper.Enabled = false;
        }
        else
        {
            txtInvDate.Enabled = false;
            txtInvoiceNo.Enabled = false;
            ddlTaxName.Enabled = true;
            txtBasicExcPer.Enabled = true;
            txtSHEExcPer.Enabled = true;
            txtducexcper.Enabled = true;
            ddlPONo.Enabled = true;
            txtInvDate.Text = "";
            txtInvoiceNo.Text = "";
        }
    }
    protected void dgCustomerRejection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //dgCustomerRejection.DataSource = dt2;
            // dgCustomerRejection.DataBind();
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgCustomerRejection.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgCustomerRejection.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgCustomerRejection.DataSource = ((DataTable)ViewState["dt2"]);
                dgCustomerRejection.DataBind();

                GetTotal();
                clearDetail();
                if (dgCustomerRejection.Rows.Count == 0)
                    LoadFilter();
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged1(null, null);
                //if (ddlPONo.SelectedIndex == 2)
                //{
                //    LoadCurr();
                //}
                ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_Code"))).Text;
                //txtTaxName.Text = ((Label)(row.FindControl("lblTaxName"))).Text;
                //txtTaxPer.Text = ((Label)(row.FindControl("lblTaxPer"))).Text;
                txtReceivedQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblReceivedQty"))).Text);
                txtRate.Text = string.Format("{0:0.00}", ((Label)(row.FindControl("lblRate"))).Text);
                txtChallanQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblChallanQty"))).Text);
                txtOrigionalQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblOriginalQty"))).Text);
                //double amount = Convert.ToDouble(((Label)(row.FindControl("lblRate"))).Text) * Convert.ToDouble(((Label)(row.FindControl("lblOrderQty"))).Text);
                //txtChallanDate.Text = ((Label)(row.FindControl("lblCustItemCode"))).Text;
                //txtCustItemCode.Text = ((Label)(row.FindControl("lblCustItemCode"))).Text;
                //txtCustItemName.Text = ((Label)(row.FindControl("lblCustItemName"))).Text;
                //ddlTaxCategory.SelectedValue = ((Label)(row.FindControl("lblTaxCatCode"))).Text;
                //rbtStatus.SelectedIndex = Convert.ToInt32(((Label)(row.FindControl("lblStatusInd"))).Text);
                txtUnit.Text = ((Label)(row.FindControl("lblUnit"))).Text;

            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void dgCustomerRejection_Deleting(object sender, GridViewDeleteEventArgs e)
    {
        //dt = CommonClasses.Execute("Select E_BASIC,E_EDU_CESS,E_H_EDU from ITEM_MASTER, EXCISE_TARIFF_MASTER where I_E_CODE = E_CODE and i_code=" + ((Label)dgCustomerRejection.Rows[e.RowIndex].FindControl("lblItemCode")).Text + "");

        ////if(txtNetAmount.Text!="0.00" || txtNetAmount.Text!="")
        //if (dt.Rows.Count > 0)
        //{
        //    //txtEduCess.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.02).ToString();
        //    //txtHigherEduCess.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.01).ToString();
        //    //txtBasicExcise.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.12).ToString();
        //    txtBasicExcise.Text = string.Format("{0:0.00}",(Convert.ToDouble(txtBasicExcise.Text)-(Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString()) / 100))));
        //    txtBasicExcPer.Text = dt.Rows[0]["E_BASIC"].ToString();
        //    txtEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text)-(Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString()) / 100))));
        //    txtducexcper.Text = dt.Rows[0]["E_EDU_CESS"].ToString();
        //    txtHigherEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text)-(Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString()) / 100))));
        //    txtSHEExcPer.Text = dt.Rows[0]["E_H_EDU"].ToString();

        //}
    }
    protected void txtNetAmount_TextChanged(object sender, EventArgs e)
    {
        //if (ddlItemName.SelectedIndex != 0)
        //{
        //    dt = CommonClasses.Execute("Select E_BASIC,E_EDU_CESS,E_H_EDU from ITEM_MASTER, EXCISE_TARIFF_MASTER where I_E_CODE = E_CODE and i_code=" + ddlItemName.SelectedValue + "");

        //    //if(txtNetAmount.Text!="0.00" || txtNetAmount.Text!="")
        //    if (dt.Rows.Count > 0)
        //    {
        //        //txtEduCess.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.02).ToString();
        //        //txtHigherEduCess.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.01).ToString();
        //        //txtBasicExcise.Text = (Convert.ToDouble(txtNetAmount.Text) * 0.12).ToString();
        //        txtBasicExcise.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString()) / 100)));
        //        txtBasicExcPer.Text = dt.Rows[0]["E_BASIC"].ToString();
        //        txtEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString()) / 100)));
        //        txtducexcper.Text = dt.Rows[0]["E_EDU_CESS"].ToString();
        //        txtHigherEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString()) / 100)));
        //        txtSHEExcPer.Text = dt.Rows[0]["E_H_EDU"].ToString();

        //    }
        //}       

        //dt.Clear();
        //if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
        //{
        //    dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from INVOICE_MASTER,SALES_TAX_MASTER where ST_CODE=INM_T_CODE AND INM_NO=" + txtInvoiceNo.Text);
        //    if (dt.Rows.Count > 0)
        //    {
        //        txtSalesTax.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["ST_SALES_TAX"].ToString()) / 100)));
        //        txtSalesTaxPer.Text = dt.Rows[0]["ST_SALES_TAX"].ToString();
        //        ddlTaxName.SelectedValue = dt.Rows[0]["ST_CODE"].ToString();
        //    }
        //}
        //CalGrandTotal();
    }
    public void CalGrandTotal()
    {

        txtGrandAmt.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text == "" ? "0.00" : txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text == "" ? "0.00" : txtBasicExcise.Text) + Convert.ToDouble(txtHigherEduCess.Text == "" ? "0.00" : txtHigherEduCess.Text) + Convert.ToDouble(txtEduCess.Text == "" ? "0.00" : txtEduCess.Text)));
    }
    protected void txtGrandAmt_TextChanged(object sender, EventArgs e)
    {
        // txtGrandAmt.Text = (Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text) + Convert.ToDouble(txtHigherEduCess.Text) + Convert.ToDouble(txtEduCess.Text) + Convert.ToDouble(txtSalesTax.Text)).ToString();
    }
    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);

        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
        {
            //ShowMessage("#Avisos", "Received Qty should be less than or equal to Challan Qty", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtChallanQty.Focus();
        }
        if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "" && txtOrigionalQty.Text != "")
        {
            if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
            {
                //ShowMessage("#Avisos", "Challan Qty should be less than or equal to Original Qty", CommonClasses.MSG_Warning);

                PanelMsg.Visible = true;
                lblmsg.Text = "Challan Qty should be less than or equal to Original Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
            }
        }
    }
    protected void txtReceivedQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtReceivedQty.Text);

        txtReceivedQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
        {
            //ShowMessage("#Avisos", "Received Qty should be less than or equal to Challan Qty", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtReceivedQty.Focus();
        }
    }
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
            if ( no > 15)
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

    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);

        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        txtBasicExcise.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100)));
        //txtEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString()) / 100)));
        //txtducexcper.Text = dt.Rows[0]["E_EDU_CESS"].ToString();
        //txtHigherEduCess.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString()) / 100)));
        //txtSHEExcPer.Text = dt.Rows[0]["E_H_EDU"].ToString();
        //txtBasicExcise.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString()) / 100)));
        //txtBasicExcPer.Text = dt.Rows[0]["E_BASIC"].ToString();
        CalGrandTotal();

    }
    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);

        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        txtEduCess.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(txtducexcper.Text) / 100)));
        CalGrandTotal();
    }
    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSHEExcPer.Text);

        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        txtHigherEduCess.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(txtSHEExcPer.Text) / 100)));
        CalGrandTotal();
    }
    protected void ddlTaxName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text == "0" || txtInvoiceNo.Text == "")
        {
            DataTable dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ST_CODE=" + ddlTaxName.SelectedValue);
            txtSalesTax.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text) + Convert.ToDouble(txtEduCess.Text) + Convert.ToDouble(txtHigherEduCess.Text)) * (Convert.ToDouble(dt.Rows[0]["ST_SALES_TAX"].ToString()) / 100)));
            txtSalesTaxPer.Text = dt.Rows[0]["ST_SALES_TAX"].ToString();
            ddlTaxName.SelectedValue = dt.Rows[0]["ST_CODE"].ToString();
        }
        CalGrandTotal();
    }
    //protected void txtSalesTax_TextChanged(object sender, EventArgs e)
    //{
    //    CalGrandTotal();
    //}
    //protected void txtHigherEduCess_TextChanged(object sender, EventArgs e)
    //{
    //    CalGrandTotal();
    //}
    //protected void txtEduCess_TextChanged(object sender, EventArgs e)
    //{
    //    CalGrandTotal();
    //}
    //protected void txtBasicExcise_TextChanged(object sender, EventArgs e)
    //{
    //    CalGrandTotal();
    //}

}
