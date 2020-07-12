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
using System.Data.SqlClient;
using System.IO;


public partial class Transactions_ADD_SupplierPurchaseOrder : System.Web.UI.Page
{
    #region Variable
    DirectoryInfo ObjSearchDir;
    string fileName = "";
    string path1 = "";
    static int File_Code1 = 0;
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtPODetail = new DataTable();
    int Active = 0;
    static int File_Code = 0;
    string path = "";

    static int mlCode = 0;
    DataRow dr;
    public static DataTable dt2 = new DataTable();
    public static DataTable dt3 = new DataTable();
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    Double DescAmount;

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

                    ViewState["fileName"] = fileName;
                    ViewState["mlCode"] = mlCode;
                    ViewState["Index"] = Index;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    LoadType();
                    LoadCustomer();
                    LoadProCode();
                    try
                    {
                        ViewState["mlCode"] = mlCode;
                        if (dt2 != null)
                        {
                        }
                        else
                        {
                            dt2 = new DataTable();
                        }
                        if (dt3 == null)
                        {
                            dt3 = new DataTable();
                        }
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        ViewState["dt3"] = dt3;
                        if (((DataTable)ViewState["dt3"]).Rows.Count > 0)
                        {
                            ((DataTable)ViewState["dt3"]).Rows.Clear();
                        }
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
                        else if (Request.QueryString[0].Equals("AMEND"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("AMEND");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {

                            LoadPoType();
                            LoadSupplier();
                            LoadICode();
                            LoadIName();
                            LoadTax();
                            LoadUnit();
                            LoadGST();
                            LoadRateUOM();
                            LoadCurr();
                            //CreateDataTable();
                            chkActiveInd.Checked = true;
                            BlankGridView();
                            txtConversionRetio.Enabled = false;
                            //CarryForward();
                            txtPoDate.Attributes.Add("readonly", "readonly");
                            txtPoDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
                            txtSupplierRefDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                            txtValid.Attributes.Add("readonly", "readonly");
                            txtValid.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                        }
                        ddlPOType.Focus();
                        //imgUpload.Attributes["onchange"] = "UploadFile(this)";
                    }
                    catch (Exception ex)
                    {
                        //CommonClasses.SendError("Supplier Order", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
        }
        #region AttachDoc
        if (IsPostBack && imgUpload.PostedFile != null)
        {
            if (imgUpload.PostedFile.FileName.Length > 0)
            {
                fileName = imgUpload.PostedFile.FileName;
                ViewState["fileName"] = fileName;
                Upload(null, null);
            }
        }
        #endregion
    }
    #endregion Page_Load

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (imgUpload.PostedFile.ContentLength > 0)
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName"].ToString()));
            }
            else
            {
                imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            }
            LnkDoc.Visible = true;
            LnkDoc.Text = ViewState["fileName"].ToString();
        }
    }
    #endregion Upload

    #region CarryForward
    public void CarryForward()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TOP 1 * from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' order by SPOM_CODE DESC");
            if (dt.Rows.Count > 0)
            {
                txtPaymentTerm.Text = dt.Rows[0]["SPOM_PAY_TERM1"].ToString();
                txtDeliverySchedule.Text = dt.Rows[0]["SPOM_DEL_SHCEDULE"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SPOM_DELIVERED_TO"].ToString();
                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SPOM_GUARNTY"].ToString();
                txtPacking.Text = dt.Rows[0]["SPOM_PACKING"].ToString();
                txtNote.Text = dt.Rows[0]["SPOM_NOTES"].ToString();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Order", "CarryForward", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadType
    private void LoadType()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(PO_T_CODE) ,PO_T_DESC from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by SO_T_DESC");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_DESC";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Purchase Order", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Save"))
        //{
        //if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtPoDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtPoDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtSupplierRefDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtSupplierRefDate.Text))))
        //{
        if (dgSupplierPurchaseOrder.Rows.Count != 0)
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select PO Type", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select PO Type";
                ddlPOType.Focus();
                return;
            }
            if (ddlProjectCode.SelectedValue == "0")
            {
                ShowMessage("#Avisos", "Select Project Code", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select PO Type";
                ddlProjectCode.Focus();
                return;
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                 
                if (Convert.ToDateTime(txtPoDate.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]))
                {
                    ShowMessage("#Avisos", "Please Select Date in current financial year", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtPoDate.Focus();
                    return;
                }
            }
            if (Convert.ToDateTime(txtPoDate.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
            {
                ShowMessage("#Avisos", "Please Select Date in current financial year ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
                return;
            }

            if (ddlSupplier.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Supplier";
                ddlSupplier.Focus();
                return;
            }
            /*19052018 Comment*/
            //if (ddlCustomer.SelectedIndex == 0)
            //{
            //    //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Select Customer";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlCustomer.Focus();
            //    return;
            //}
            if (txtPoDate.Text == "")
            {
                ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtPoDate.Focus();
            }
            //if (ddlPOType.SelectedValue == "-2147483647" && ddlCurrency.SelectedIndex==0)
            //{

            //    ShowMessage("#Avisos", "Select Currancy Name", CommonClasses.MSG_Warning);
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlPOType.Focus();
            //    return;
            //}

            if (txtPaymentTerm.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Please Enter PO Payment Terms", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtPaymentTerm.Focus();
                return;
            }
            if (dgSupplierPurchaseOrder.Enabled && dgSupplierPurchaseOrder.Rows.Count > 0)
            {

                SaveRec();

            }
            else
            {
                ShowMessage("#Avisos", "Insert Item Detail", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Record Not Exist In Grid View";
                return;
            }


        }

        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);

        }
        //}
        //else
        //{
        //    ShowMessage("#Avisos", "Please Select Current Year Date ", CommonClasses.MSG_Warning);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

        //    //PanelMsg.Visible = true;
        //    //lblmsg.Text = "Record Not Exist In Grid View";
        //    return;
        //}

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //if (mlCode != 0 && mlCode != null)
            //{
            //    CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", mlCode);
            //}

            //dt2.Rows.Clear();
            //Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);

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
            CommonClasses.SendError("Supplier Purchase Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    private void CreateDataTable()
    {
        #region datatable structure
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode1");

            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");

            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("Round");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("NetTotal");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("ConversionRatio");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcInclusive");
            ((DataTable)ViewState["dt2"]).Columns.Add("Excdatepass");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcGapRate");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).
Columns.Add("SHECess");

            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("round1");
            ((DataTable)ViewState["dt2"]).Columns.Add("PurVatCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("SerTCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_ITEM_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("ActiveInd");

            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_INW_QTY");
        }
        #endregion
        ViewState["NewTable"] = dt2;
    }
    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        Double Per = 100;
        try
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Po Type", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select PO Type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlPOType.Focus();
                return;
            }
            if (ddlSupplier.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Supplier";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlSupplier.Focus();
                return;
            }
            /*Comment : 19052018*/
            //if (ddlCustomer.SelectedIndex == 0)
            //{
            //    //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Select Customer";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlCustomer.Focus();
            //    return;
            //}

            if (txtPoDate.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Po Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
                return;
            }
            if (ddlItemCode.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            //if (txtOrderQty.Text.Trim() == "" || txtOrderQty.Text == "0.000")
            if (txtOrderQty.Text.Trim() == "")
            {
                //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Order Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtMissItemName.Text) && Convert.ToDouble(txtOrderQty.Text) != 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should not less than Inward Qty " + txtMissItemName.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            if (txtRate.Text == "" || txtRate.Text == "0.00")
            {
                //ShowMessage("#Avisos", "Please Enter Rate", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRate.Focus();
                return;
            }
            if (ddlRateUOM.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Rate UOM", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Rate Unit";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRateUOM.Focus();
                return;
            }
            if (chkActiveInd.Checked == true)
            {
                Active = 1;
            }
            else
            {
                Active = 0;
            }

            //if (ddlSalesTax.SelectedIndex == 0)
            //{
            //    //ShowMessage("#Avisos", "Select  Sales Tax Category", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Select Sales Tax";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlSalesTax.Focus();
            //    return;
            //}
            if (txtConversionRetio.Text == "" || txtConversionRetio.Text == "0.0")
            {

                if (ddlStockUOM.SelectedItem.Text != ddlRateUOM.SelectedItem.Text)
                {
                    //ShowMessage("#Avisos", "Enter Conversion Ratio", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Conversion Ratio";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                }
            }
            if (txtDescAmount.Text == "")
            {
                DescAmount = 0.00;
            }
            else
            {
                DescAmount = Convert.ToDouble(txtDescAmount.Text);
            }
            if (txtDescPerc.Text == "")
            {
                txtDescPerc.Text = "0";
            }
            else if (Convert.ToDouble(txtDescPerc.Text) > Per)
            {
                //ShowMessage("#Avisos", "Percentage not greter than 100", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Percentage cannot be greater than 100";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDescPerc.Focus();
                return;
            }
            //if (txtExcisePer.Text.Trim() == "")
            //{
            //    txtExcisePer.Text = "0.00";
            //}
            //if (chkExcInclusive.Checked == false && Convert.ToDouble(txtExcisePer.Text) <= 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter Excise % ";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtExcisePer.Focus();
            //    return;
            //}
            //dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from SALES_TAX_MASTER where ST_CODE=" + ddlSalesTax.SelectedValue + " and ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "");
            Double Tax = 0;

            Double Subamount = (Convert.ToDouble(txtTotalAmount.Text.ToString()) - DescAmount);

            double E_BASIC = 0;
            double E_EDU_CESS = 0;
            double E_H_EDU = 0;
            double ExisDuty = 0;
            double EduCess = 0;
            double SHECess = 0;
            //if (!chkExcInclusive.Checked)
            //{
            dt = CommonClasses.Execute("select E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU from EXCISE_TARIFF_MASTER,ITEM_MASTER where I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString());//Central
                E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString());//state
                E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString());//Integrated
            }
            //}
            E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"]);
            //DataTable dtGSTcal = CommonClasses.Execute("SELECT P_CODE, P_NAME, P_SM_CODE, CM_STATE , ISNULL( P_LBT_IND ,0) AS P_LBT_IND  FROM PARTY_MASTER,COMPANY_MASTER WHERE P_TYPE = 2 AND CM_ID = P_CM_COMP_ID AND PARTY_MASTER.ES_DELETE = 0 AND P_CODE='" + ddlSupplier.SelectedValue + "'");
            DataTable dtGSTcal = CommonClasses.Execute("SELECT P_CODE, P_NAME, P_SM_CODE, CM_STATE , ISNULL( P_LBT_IND ,0) AS P_LBT_IND  FROM PARTY_MASTER,COMPANY_MASTER WHERE CM_ID = P_CM_COMP_ID AND PARTY_MASTER.ES_DELETE = 0 AND P_CODE='" + ddlSupplier.SelectedValue + "'");
            if (dtGSTcal.Rows.Count > 0)
            {
                if (dtGSTcal.Rows[0]["P_LBT_IND"].ToString() == "True" || dtGSTcal.Rows[0]["P_LBT_IND"].ToString() == "1")
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
                else
                {
                    ExisDuty = 0;
                    EduCess = 0;
                    SHECess = 0;
                }
            }
            //Double SalesTax = (((Convert.ToDouble(txtTotalAmount.Text.ToString())) + Subamount + ExisDuty + EduCess + SHECess) * Tax) / (Convert.ToDouble(100));
            // NetTotal = Subamount + ExisDuty + EduCess + SHECess + SalesTax;

            if (dgSupplierPurchaseOrder.Rows.Count > 0)
            {
                for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode"))).Text;
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
                            ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                ViewState["ItemUpdateIndex"] = "-1";
            }

            dt2 = (DataTable)ViewState["NewTable"];

            DataTable dtsample = (DataTable)ViewState["dt2"];

            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                for (int i = ((DataTable)ViewState["dt2"]).Rows.Count - 1; i >= 0; i--)
                {
                    if (((DataTable)ViewState["dt2"]).Rows[i][0] == DBNull.Value)
                        ((DataTable)ViewState["dt2"]).Rows[i].Delete();
                }
                ((DataTable)ViewState["dt2"]).AcceptChanges();
            }

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemCode.SelectedValue;
            dr["ItemCode1"] = ddlItemCode.SelectedItem;

            dr["ItemName1"] = ddlItemName.SelectedValue;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["StockUOM1"] = ddlStockUOM.SelectedValue;
            dr["StockUOM"] = ddlStockUOM.SelectedItem;
            dr["OrderQty"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 2));
            dr["Rate"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtRate.Text)), 3));

            dr["RateUOM1"] = ddlRateUOM.SelectedValue;
            dr["RateUOM"] = ddlRateUOM.SelectedItem;


            //dr["Round"] = ChkRound.Checked;
            dr["TotalAmount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtOrderQty.Text)), 2));
            // dr["NetTotal"] = string.Format("{0:0.00}", Math.Round(NetTotal), 2);

            dr["DiscPerc"] = txtDescPerc.Text;
            dr["DiscAmount"] = string.Format("{0:0.00}", Math.Round(DescAmount), 2);
            if (txtConversionRetio.Text == "")
            {
                dr["ConversionRatio"] = 0.0;
            }
            else
            {
                dr["ConversionRatio"] = Convert.ToDouble(txtConversionRetio.Text);
            }
            dr["ExcInclusive"] = chkExcInclusive.Checked;
            dr["SalesTax1"] = ddlSalesTax.SelectedValue;
            dr["SalesTax"] = ddlSalesTax.SelectedItem;
            dr["Specification"] = txtSpecification.Text;
            dr["SPOD_ITEM_NAME"] = txtMissItemName.Text.Trim();

            dr["ActiveInd"] = Active;
            dr["E_BASIC"] = E_BASIC;
            dr["E_EDU_CESS"] = E_EDU_CESS;
            dr["E_H_EDU"] = E_H_EDU;
            if (txtMissItemName.Text.Trim() == "")
            {
                txtMissItemName.Text = "0";
            }
            dr["SPOD_INW_QTY"] = txtMissItemName.Text;
            dr["DocName"] = ViewState["fileName"].ToString();
            dr["E_TARIFF_NO"] = ddlGSTCODE.SelectedItem;
            dr["E_TARIFF_NO1"] = ddlGSTCODE.SelectedValue;
            dr["ExisDuty"] = Math.Round(ExisDuty, 2);
            dr["EduCess"] = Math.Round(EduCess, 2);
            dr["SHECess"] = Math.Round(SHECess, 2);
            // dt[""]=;
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
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

                // ViewState["NewTable"] = ((DataTable)ViewState["dt2"]);
            }
            #endregion

            #region Binding data to Grid
            dgSupplierPurchaseOrder.Visible = true;
            dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
            //dgSupplierPurchaseOrder.DataSource = (DataTable)ViewState["NewTable"]; 
            dgSupplierPurchaseOrder.DataBind();
            dgSupplierPurchaseOrder.Enabled = true;


            #endregion

            ddlItemCode.Enabled = true;
            ddlItemName.Enabled = true;

            GetTotal();
            clearDetail();
            ddlRateUOM.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Order", "btnInsert_Click", Ex.Message);
        }

        ddlItemCode.Focus();
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtPoDate.Text == "")
            {
                flag = false;
            }
            else if (ddlSupplier.SelectedIndex == 0)
            {
                flag = false;
            }
            //else if (dgMainPO.Enabled)
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
            CommonClasses.SendError("Supplier PO", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Supplier PO", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,ltrim(rtrim(I_CODENO)) as I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!='-2147483648' ORDER BY ltrim(rtrim(I_CODENO))");
            dt = CommonClasses.Execute("select I_CODE,ltrim(rtrim(I_CODENO)) as I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ltrim(rtrim(I_CODENO))");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region LoadPoType
    private void LoadPoType()
    {
        try
        {
            dt = CommonClasses.Execute("select PO_T_CODE,PO_T_SHORT_NAME AS PO_T_SHORT_NAME from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY PO_T_SHORT_NAME");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_SHORT_NAME";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Select PO Type", "0"));
            ddlPOType.SelectedIndex = -1;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,ltrim(rtrim(I_NAME)) as I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!='-2147483648' ORDER BY ltrim(rtrim(I_NAME))");
            dt = CommonClasses.Execute("select I_CODE,ltrim(rtrim(I_NAME)) as I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ltrim(rtrim(I_NAME))");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    #region LoadTax
    private void LoadTax()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlSalesTax.DataSource = dt;
            ddlSalesTax.DataTextField = "ST_TAX_NAME";
            ddlSalesTax.DataValueField = "ST_CODE";
            ddlSalesTax.DataBind();
            ddlSalesTax.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            if (dt.Rows.Count > 0)
            {
                ddlSalesTax.SelectedValue = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadTax", Ex.Message);
        }

    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2'    AND    P_ACTIVE_IND=1 order by P_NAME");
            /*Change As Sugested A :- Remove P_Type 20-09-2018*/
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND P_ACTIVE_IND=1 order by P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        //try
        //{
        //    dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1'      AND    P_ACTIVE_IND=1 order by P_NAME");
        //    ddlCustomer.DataSource = dt;
        //    ddlCustomer.DataTextField = "P_NAME";
        //    ddlCustomer.DataValueField = "P_CODE";
        //    ddlCustomer.DataBind();
        //    ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        //}
        //catch (Exception Ex)
        //{
        //    CommonClasses.SendError("Supplier Purchase Order", "LoadCustomer", Ex.Message);
        //}

    }
    #endregion

    //#region LoadPOUnit
    //private void LoadPOUnit()
    //{
    //    try
    //    {

    //            // ddlItemName.SelectedValue = ddlItemCode.SelectedValue;

    //           // DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
    //            DataTable dt1 = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=1");

    //            if (dt1.Rows.Count > 0)
    //            {
    //                ddlPOUnit.DataSource = dt1;
    //                ddlPOUnit.DataTextField = "I_UOM_NAME";
    //                ddlPOUnit.DataValueField = "I_UOM_CODE";
    //                ddlPOUnit.DataBind();
    //                ddlPOUnit.Items.Insert(0, new ListItem("Select Po Unit", "0"));
    //                ddlPOUnit.SelectedIndex = -1;
    //            }


    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Customer Order Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

    //    }
    //}
    //#endregion

    #region LoadRateUOM
    private void LoadRateUOM()
    {
        try
        {

            // ddlItemName.SelectedValue = ddlItemCode.SelectedValue;

            // DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
            DataTable dt1 = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=1");

            if (dt1.Rows.Count > 0)
            {
                ddlRateUOM.DataSource = dt1;
                ddlRateUOM.DataTextField = "I_UOM_NAME";
                ddlRateUOM.DataValueField = "I_UOM_CODE";
                ddlRateUOM.DataBind();
                ddlRateUOM.Items.Insert(0, new ListItem("Select Po Unit", "0"));
                ddlRateUOM.SelectedIndex = -1;
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                LoadGST();
                DataTable dtGST = CommonClasses.Execute("select I_CODE,I_E_CODE,E_TARIFF_NO from ITEM_MASTER,EXCISE_TARIFF_MASTER where ITEM_MASTER.ES_DELETE=0 and EXCISE_TARIFF_MASTER.ES_DELETE=0 and I_E_CODE=E_CODE and I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtGST.Rows.Count > 0)
                {
                    ddlGSTCODE.SelectedValue = Convert.ToString(dtGST.Rows[0]["I_E_CODE"]);
                }
                //   DataTable dt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='"+ddlItemCode.SelectedValue+"'");
                //   txtRate.Text =string.Format("{0:0.00}", dt.Rows[0]["I_INV_RATE"]);
                DataTable dt = CommonClasses.Execute("select isnull(SPOD_RATE,0) as SPOD_RATE from ITEM_MASTER ,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SPOD_I_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE= (SELECT MAX(SPOM_CODE) FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.000}", dt.Rows[0]["SPOD_RATE"]);
                }
                else
                {
                    DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");
                    txtRate.Text = string.Format("{0:0.000}", dtt.Rows[0]["I_INV_RATE"]);
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region ddlPOType_SelectedIndexChanged
    protected void ddlPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPOType.SelectedIndex != 0)
        {
            //CarryForward();
        }
        // For Heimatec
        //if (ddlPOType.SelectedValue == "-2147483647")
        //{
        //    pnlCurrancy.Visible = true;
        //}
        //else
        //{
        //    pnlCurrancy.Visible = false;
        //}
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedIndex != 0)
        {
            BlankGridView();
            CarryForward();
            DataTable dtPaymentTerm = CommonClasses.Execute("select * from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' and P_CODE='" + ddlSupplier.SelectedValue + "'");
            txtPaymentTerm.Text = dtPaymentTerm.Rows[0]["P_PAYMENT_TERM"].ToString();
        }
        else
        {
            BlankGridView();

        }
    }
    #endregion

    #region LoadUnit
    private void LoadUnit()
    {
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE ,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ");
            ddlStockUOM.DataSource = dt;
            ddlStockUOM.DataTextField = "I_UOM_NAME";
            ddlStockUOM.DataValueField = "I_UOM_CODE";
            ddlStockUOM.DataBind();
            ddlStockUOM.Items.Insert(0, new ListItem("Unit", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadUnit", Ex.Message);
        }

    }
    #endregion

    #region LoadGST
    private void LoadGST()
    {
        try
        {
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where ES_DELETE=0 and E_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ");
            ddlGSTCODE.DataSource = dt;
            ddlGSTCODE.DataTextField = "E_TARIFF_NO";
            ddlGSTCODE.DataValueField = "E_CODE";
            ddlGSTCODE.DataBind();
            ddlGSTCODE.Items.Insert(0, new ListItem("HSN", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadGST", Ex.Message);
        }

    }
    #endregion

    #region LoadUOM
    private void LoadUOM()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlStockUOM.DataSource = dt1;
        ddlStockUOM.DataTextField = "I_UOM_NAME";
        ddlStockUOM.DataValueField = "I_UOM_CODE";
        ddlStockUOM.DataBind();

    }
    #endregion

    #region LoadCurr
    private void LoadCurr()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER WHERE ES_DELETE=0 AND CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "CURR_NAME";
            ddlCurrency.DataValueField = "CURR_CODE";
            ddlCurrency.DataBind();
            //ddlCurrency.Items.Insert(0, new ListItem("Select currency", "0"));
            ddlCurrency.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
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
                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                //DataTable dt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemName.SelectedValue + "'");
                //txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["I_INV_RATE"]);

                LoadGST();
                DataTable dtGST = CommonClasses.Execute("select I_CODE,I_E_CODE,E_TARIFF_NO from ITEM_MASTER,EXCISE_TARIFF_MASTER where ITEM_MASTER.ES_DELETE=0 and EXCISE_TARIFF_MASTER.ES_DELETE=0 and I_E_CODE=E_CODE and I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtGST.Rows.Count > 0)
                {
                    ddlGSTCODE.SelectedValue = Convert.ToString(dtGST.Rows[0]["I_E_CODE"]);
                }

                DataTable dt = CommonClasses.Execute("select isnull(SPOD_RATE,0) as SPOD_RATE from ITEM_MASTER ,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SPOD_I_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE= (SELECT MAX(SPOM_CODE) FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.000}", dt.Rows[0]["SPOD_RATE"]);
                }
                else
                {
                    DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");
                    txtRate.Text = string.Format("{0:0.000}", dtt.Rows[0]["I_INV_RATE"]);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadRateUOM();
            LoadPoType();
            LoadSupplier();
            LoadICode();
            LoadIName();
            LoadTax();
            LoadCurr();
            LoadProCode();
            dtPODetail.Clear();
            txtPoDate.Attributes.Add("readonly", "readonly");
            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
            txtValid.Attributes.Add("readonly", "readonly");

            dt = CommonClasses.Execute("Select SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_CUST_CODE,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_VALID_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_PONO,SPOM_PROJ_NAME FROM SUPP_PO_MASTER WHERE ES_DELETE=0 AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SPOM_CODE"]);
                ddlPOType.SelectedValue = dt.Rows[0]["SPOM_TYPE"].ToString();

                #region comment For Heimatec
                //if (ddlPOType.SelectedValue == "-2147483647")
                //{
                //pnlCurrancy.Visible = true;
                //ddlCurrency.SelectedValue = dt.Rows[0]["SPOM_CURR_CODE"].ToString();
                //ddlCurrency_SelectedIndexChanged(null, null);
                //}
                //else
                //{
                //    //pnlCurrancy.Visible = false;
                //} 
                #endregion

                pnlCurrancy.Visible = true;
                ddlCurrency.SelectedValue = dt.Rows[0]["SPOM_CURR_CODE"].ToString();
                ddlCurrency_SelectedIndexChanged(null, null);
                ddlCurrency.Enabled = false;

                ddlCustomer.SelectedValue = dt.Rows[0]["SPOM_CUST_CODE"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[0]["SPOM_P_CODE"].ToString();
                txtPoNo.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                txtPoNo.Text = dt.Rows[0]["SPOM_PONO"].ToString();

                txtPoDate.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_DATE"]).ToString("dd MMM yyyy");
                txtSupplierRefDate.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_SUP_REF_DATE"]).ToString("dd MMM yyyy");

                txtValid.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_VALID_DATE"]).ToString("dd MMM yyyy");
                txtSupplierRef.Text = dt.Rows[0]["SPOM_SUP_REF"].ToString();
                ddlProjectCode.SelectedValue = dt.Rows[0]["SPOM_PROJECT"].ToString();
                txtProjName.Text = dt.Rows[0]["SPOM_PROJ_NAME"].ToString();

                txtFinalTotalAmount.Text = string.Format("{0:0.00}", dt.Rows[0]["SPOM_AMOUNT"]);


                txtTranspoter.Text = dt.Rows[0]["SPOM_TRANSPOTER"].ToString();

                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SPOM_GUARNTY"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SPOM_DELIVERED_TO"].ToString();
                txtNote.Text = dt.Rows[0]["SPOM_NOTES"].ToString();
                txtPaymentTerm.Text = dt.Rows[0]["SPOM_PAY_TERM1"].ToString();

                txtDeliverySchedule.Text = dt.Rows[0]["SPOM_DEL_SHCEDULE"].ToString();
                txtCIF.Text = dt.Rows[0]["SPOM_CIF_NO"].ToString();
                txtPacking.Text = dt.Rows[0]["SPOM_PACKING"].ToString();

                //dtPODetail = CommonClasses.Execute("select SPOD_I_CODE as ItemCode,I_CODENO as ShortName,I_DOC_NAME as Docname,I_DOC_PATH,SPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, cast(SPOD_ORDER_QTY as numeric(10,3)) as OrderQty,cast(SPOD_RATE as numeric(20,2)) as Rate,SPOD_RATE_UOM as RateUOM1,I_UOM_NAME as RateUOM,SPOD_CONV_RATIO as ConversionRatio,cast(SPOD_TOTAL_AMT as numeric(20,2)) as TotalAmount,cast(SPOD_DISC_PER as float) as DiscPerc,cast(SPOD_DISC_AMT as numeric(20,2)) as DiscAmount,isnull(SPOD_EXC_Y_N,0) as ExcInclusive,SPOD_T_CODE as SalesTax1,ST_TAX_NAME as SalesTax ,SPOD_ACTIVE_IND as ActiveInd ,SPOD_SPECIFICATION as Specification,SPOD_ITEM_NAME,SPOD_EXC_PER as E_BASIC, SPOD_EDU_CESS_PER as E_EDU_CESS,SPOD_H_EDU_CESS as E_H_EDU,ISNULL(SPOD_INW_QTY,0) AS SPOD_INW_QTY FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,SALES_TAX_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SUPP_PO_DETAILS.SPOD_UOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE and SPOD_T_CODE=SALES_TAX_MASTER.ST_CODE and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                dtPODetail = CommonClasses.Execute("SELECT SUPP_PO_DETAILS.SPOD_I_CODE AS ItemCode, ITEM_MASTER.I_CODENO AS ShortName, ITEM_MASTER.I_DOC_NAME,SPOD_DOC_NAME AS Docname, ITEM_MASTER.I_DOC_PATH,SUPP_PO_DETAILS.SPOD_I_CODE AS ItemName1, ITEM_MASTER.I_CODENO AS ItemCode1, ITEM_MASTER.I_NAME AS ItemName,SUPP_PO_DETAILS.SPOD_UOM_CODE AS StockUOM1, ITEM_UNIT_MASTER.I_UOM_NAME AS StockUOM,CAST(SUPP_PO_DETAILS.SPOD_ORDER_QTY AS numeric(10, 3)) AS OrderQty, CAST(SUPP_PO_DETAILS.SPOD_RATE AS numeric(20, 3)) AS Rate,SUPP_PO_DETAILS.SPOD_RATE_UOM AS RateUOM1, ITEM_UNIT_MASTER.I_UOM_NAME AS RateUOM, SUPP_PO_DETAILS.SPOD_CONV_RATIO AS ConversionRatio,CAST(SUPP_PO_DETAILS.SPOD_TOTAL_AMT AS numeric(20, 2)) AS TotalAmount, CAST(SUPP_PO_DETAILS.SPOD_DISC_PER AS float) AS DiscPerc,CAST(SUPP_PO_DETAILS.SPOD_DISC_AMT AS numeric(20, 2)) AS DiscAmount,ISNULL(SUPP_PO_DETAILS.SPOD_EXC_Y_N, 0) AS ExcInclusive,SUPP_PO_DETAILS.SPOD_T_CODE AS SalesTax1, ' ' AS SalesTax, SUPP_PO_DETAILS.SPOD_ACTIVE_IND AS ActiveInd,SUPP_PO_DETAILS.SPOD_SPECIFICATION AS Specification, SUPP_PO_DETAILS.SPOD_ITEM_NAME, SUPP_PO_DETAILS.SPOD_EXC_PER AS E_BASIC,SUPP_PO_DETAILS.SPOD_EDU_CESS_PER AS E_EDU_CESS, SUPP_PO_DETAILS.SPOD_H_EDU_CESS AS E_H_EDU, ISNULL(SUPP_PO_DETAILS.SPOD_INW_QTY,0) AS SPOD_INW_QTY,ISNULL(SPOD_CENTRAL_TAX_AMT,0) as ExisDuty,isnull(SPOD_STATE_TAX_AMT,0) as EduCess,isnull(SPOD_INTEGRATED_TAX_AMT,0) as SHECess, EXCISE_TARIFF_MASTER.E_TARIFF_NO,EXCISE_TARIFF_MASTER.E_CODE as E_TARIFF_NO1 FROM SUPP_PO_MASTER INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE INNER JOIN ITEM_UNIT_MASTER ON SUPP_PO_DETAILS.SPOD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN ITEM_MASTER ON SUPP_PO_DETAILS.SPOD_I_CODE = ITEM_MASTER.I_CODE  INNER JOIN EXCISE_TARIFF_MASTER ON SUPP_PO_DETAILS.SPOD_E_TARRIF_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE (SUPP_PO_MASTER.SPOM_CM_COMP_ID ='" + Convert.ToInt32(Session["CompanyId"]) + "') AND (SUPP_PO_DETAILS.SPOD_SPOM_CODE ='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "')");
                if (dtPODetail.Rows.Count != 0)
                {
                    dgSupplierPurchaseOrder.DataSource = dtPODetail;
                    dgSupplierPurchaseOrder.DataBind();
                    ViewState["dt2"] = dtPODetail;
                    GetTotal();
                }
                DataTable dtDoc = CommonClasses.Execute("select SPOA_SPOM_CODE,SPOA_DOC_NAME,SPOA_DOC_PATH from SUPP_PO_ATTACHMENT where SPOA_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (dtDoc.Rows.Count > 0)
                {
                    dgDocView.DataSource = dtDoc;
                    dgDocView.DataBind();
                    ViewState["dt3"] = dtDoc;
                }
            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;
                txtConversionRetio.Enabled = false;
                txtPoDate.Enabled = false;
                txtDescAmount.Enabled = false;
                txtDescPerc.Enabled = false;

                txtOrderQty.Enabled = false;
                txtPoDate.Enabled = false;
                txtPoNo.Enabled = false;
                txtValid.Enabled = false;
                ddlProjectCode.Enabled = false;
                imgUpload.Enabled = false;
                txtRate.Enabled = false;
                txtGuranteeWaranty.Enabled = false;
                txtSpecification.Enabled = false;
                ddlStockUOM.Enabled = false;
                txtSupplierRef.Enabled = false;
                txtSupplierRefDate.Enabled = false;
                txtTotalAmount.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlPOType.Enabled = false;
                ddlRateUOM.Enabled = false;
                ddlSalesTax.Enabled = false;
                ddlSupplier.Enabled = false;
                chkActiveInd.Enabled = false;
                chkExcInclusive.Enabled = false;

                txtDeliverySchedule.Enabled = false;
                txtTranspoter.Enabled = false;
                txtPaymentTerm.Enabled = false;
                txtDeliveryTo.Enabled = false;
                txtFreightTermsg.Enabled = false;
                txtNote.Enabled = false;
                dgSupplierPurchaseOrder.Enabled = false;
                ddlCurrency.Enabled = false;
                //txtCurrencyRate.Enabled = false;
                txtMissItemName.Enabled = false;
                ddlGSTCODE.Enabled = false;

            }
            else if (str == "MOD" || str == "AMEND")
            {
                ddlSupplier.Enabled = false;

                txtConversionRetio.Enabled = false;
                CommonClasses.SetModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier Purchase Order Transaction", "ViewRec", Ex.Message);
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

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlStockUOM.SelectedIndex = 0;
            txtOrderQty.Text = "";
            txtRate.Text = "";
            ddlRateUOM.SelectedIndex = 0;

            txtTotalAmount.Text = "";
            txtDescPerc.Text = "";
            txtDescAmount.Text = "";
            txtConversionRetio.Text = "";
            chkExcInclusive.Checked = false;

            ddlSalesTax.SelectedIndex = 0;

            txtSpecification.Text = "";
            // chkActiveInd.Checked = false;
            txtMissItemName.Text = "0.00";
            txtExcisePer.Text = "0.00";
            //txtAmount.Text = "0.00";
            //txtCustItemCode.Text = "";
            //txtCustItemName.Text = "";
            //ddlTaxCategory.SelectedIndex = 0;
            //ddlCurrancy.SelectedIndex = 0;
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double CGST_AMT = 0, SGST_AMT = 0, IGST_AMT = 0;
        if (dgSupplierPurchaseOrder.Rows.Count > 0)
        {
            for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount"))).Text;
                string QED_Disc = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);
                double Discount = Convert.ToDouble(QED_Disc);
                decTotal = decTotal + Amount - Discount;

                string CGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblExisDuty"))).Text;
                string SGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblEduCess"))).Text;
                string IGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSHECess"))).Text;

                CGST_AMT = CGST_AMT + Convert.ToDouble(CGST);
                SGST_AMT = SGST_AMT + Convert.ToDouble(SGST);
                IGST_AMT = IGST_AMT + Convert.ToDouble(IGST);
            }
        }
        double Totaltax = CGST_AMT + SGST_AMT + IGST_AMT;
        txtFinalTotalAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal + Totaltax), 2);
    }
    #endregion

    #region txtOrderQty_TextChanged
    protected void txtOrderQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "" || txtOrderQty.Text == Convert.ToDouble(0).ToString())
            {
                txtOrderQty.Text = "0";
            }
            if (txtMissItemName.Text.Trim() == "")
            {
                txtMissItemName.Text = "0";
            }
            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtMissItemName.Text) && Convert.ToDouble(txtOrderQty.Text) != 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should not less than Inward Qty " + txtMissItemName.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            Calculate();
            //txtRate.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Transaction", "txtOrderQty_OnTextChanged", Ex.Message);
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
            //if ( no > 15)
            //{
            //     no = 15;
            //}
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

    #region Calculate
    public void Calculate()
    {
        string totalStr = "";

        if (txtOrderQty.Text == "")
        {
            txtOrderQty.Text = "0.00";
        }
        if (txtRate.Text == "")
        {
            txtRate.Text = "0.00";
        }
        if (txtTotalAmount.Text == "")
        {
            txtTotalAmount.Text = "0.00";
        }

        if (txtDescPerc.Text == "")
        {
            txtDescPerc.Text = "0.00";
        }
        if (txtDescAmount.Text == "")
        {
            txtDescAmount.Text = "0.00";
        }
        if (txtConversionRetio.Text == "")
        {
            txtConversionRetio.Text = "0.00";
        }
        //--------------------------------------
        //----------------------------------------------------------------------------------------
        totalStr = DecimalMasking(txtOrderQty.Text);
        txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        totalStr = DecimalMasking(txtRate.Text);
        //txtRate.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        //txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2));
        double Amount = 0; string OrderQty = ""; string OrderRate = "";
        OrderQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
        OrderRate = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtRate.Text), 3));
        Amount = Convert.ToDouble(Math.Round((Convert.ToDouble(OrderQty) * Convert.ToDouble(OrderRate)), 0, MidpointRounding.AwayFromZero));
        txtTotalAmount.Text = Amount.ToString();
        if (ddlStockUOM.SelectedItem.ToString() != ddlRateUOM.SelectedItem.ToString())
        {
            totalStr = DecimalMasking(txtConversionRetio.Text);
            txtConversionRetio.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            double TempTotal = Convert.ToDouble(txtOrderQty.Text) * (Convert.ToDouble(txtConversionRetio.Text));
            txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round(TempTotal) * Convert.ToDouble(txtRate.Text.ToString()), 2);

        }
        totalStr = DecimalMasking(txtDescPerc.Text);
        txtDescPerc.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtDescAmount.Text = string.Format("{0:0.00}", Math.Round((((Convert.ToDouble(txtTotalAmount.Text.ToString())) / 100) * Convert.ToDouble(txtDescPerc.Text)), 2));

    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calculate();
            //txtDescPerc.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Transaction", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDescPerc_TextChanged
    protected void txtDescPerc_TextChanged(object sender, EventArgs e)
    {
        Calculate();
    }
    #endregion

    #region dgSupplierPurchaseOrder_RowCommand
    protected void dgSupplierPurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgSupplierPurchaseOrder.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                // int rowindex = row.RowIndex;
                //dgSupplierPurchaseOrder.DeleteRow(Index);

                string ICode = ((Label)(row.FindControl("lblItemCode"))).Text;

                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ICode + "'");

                if (dtCheckExistItem.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cannot delete the item bcause the item used in Inward";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtOrderQty.Focus();
                    return;
                }

                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
                dgSupplierPurchaseOrder.DataBind();
                GetTotal();
                if (((DataTable)ViewState["dt2"]).Rows.Count == 0)
                {
                    BlankGridView();
                }
            }
            if (e.CommandName == "Modify")
            {
                //LoadSupplier();
                LoadICode();
                LoadIName();
                LoadTax();
                // LoadPOUnit();
                LoadRateUOM();

                //********
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_NAME1"))).Text;
                ddlItemName_SelectedIndexChanged(null, null);
                ddlStockUOM.SelectedValue = ((Label)(row.FindControl("lblUOM1"))).Text;
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblRate"))).Text;
                ddlRateUOM.SelectedValue = ((Label)(row.FindControl("lblRateUOM1"))).Text;

                //ChkRound.Checked = ((Label)(row.FindControl("lblRound"))).Text;
                txtTotalAmount.Text = ((Label)(row.FindControl("lblTotalAmount"))).Text;
                txtDescPerc.Text = ((Label)(row.FindControl("lblDiscPerc"))).Text;
                txtDescAmount.Text = ((Label)(row.FindControl("lblDiscAmount"))).Text;
                txtConversionRetio.Text = ((Label)(row.FindControl("lblConversionRatio"))).Text;

                ddlSalesTax.SelectedValue = ((Label)(row.FindControl("lblSalesTax1"))).Text;
                ViewState["fileName"] = ((LinkButton)(row.FindControl("lnkView"))).Text;
                txtSpecification.Text = ((Label)(row.FindControl("lblSpecification"))).Text;
                txtMissItemName.Text = ((Label)(row.FindControl("lblSPOD_INW_QTY"))).Text;
                if ((((Label)(row.FindControl("lblExcInclusive"))).Text).ToString() == "True")
                {
                    chkExcInclusive.Checked = true;
                }
                else
                {
                    chkExcInclusive.Checked = false;
                }
                txtExcisePer.Text = ((Label)(row.FindControl("lblE_BASIC"))).Text;

                if ((((Label)(row.FindControl("lblActiveInd"))).Text) == "1")
                {
                    chkActiveInd.Checked = true;
                }
                else
                {
                    chkActiveInd.Checked = false;
                }
                //$$$
                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtCheckExistItem.Rows.Count > 0)
                {
                    ddlItemCode.Enabled = false;
                    ddlItemName.Enabled = false;
                }
                else
                {
                    ddlItemCode.Enabled = true;
                    ddlItemName.Enabled = true;
                }
            }
            if (e.CommandName == "ViewPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    code = ((Label)(row.FindControl("lblItemCode"))).Text;
                    filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                    directory = "../../UpLoadPath/SupPO/" + code + "/" + filePath;
                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                    // Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");
                }
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order Transaction", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierPurchaseOrder_RowDeleting
    protected void dgSupplierPurchaseOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
            ddlProjectCode.DataSource = dt;
            ddlProjectCode.DataTextField = "PROCM_NAME";
            ddlProjectCode.DataValueField = "PROCM_CODE";
            ddlProjectCode.DataBind();

            //ddlProjectCode.Items.Insert(0, new ListItem("Select Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadProCode", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        bool result1 = false;
        if (hf.Value == "0")
        {
            hf.Value = "1";
            try
            {
                if (txtSupplierRefDate.Text == "")
                {
                    txtSupplierRefDate.Text = System.DateTime.Now.ToString();
                }
                if (txtPoDate.Text == "")
                {

                }
                #region INSERT
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    int Po_Doc_no = 0;
                    DataTable dt = new DataTable();
                    string strPONo = "";
                    dt = CommonClasses.Execute("Select isnull(max(SPOM_PO_NO),0) as SPOM_PO_NO FROM SUPP_PO_MASTER WHERE SPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and SPOM_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0 AND SPOM_POTYPE=0 AND SPOM_TYPE  ='" + ddlPOType.SelectedValue + "'");
                    DataTable dtPrefix = CommonClasses.Execute("SELECT * FROM PO_TYPE_MASTER WHERE ES_DELETE =0 AND PO_T_COMP_ID='" + (string)Session["CompanyId"] + "' AND PO_T_CODE  ='" + ddlPOType.SelectedValue + "'");
                    if (dt.Rows.Count > 0)
                    {
                        //PCPCL0000
                        if (true)
                        {
                        }
                        Po_Doc_no = Convert.ToInt32(dt.Rows[0]["SPOM_PO_NO"]);
                        Po_Doc_no = Po_Doc_no + 1;
                        string strYear = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy").Substring(7, 4) + "-" + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy").ToString().Substring(9, 2);
                        strPONo = "SPC-" + strYear + "-" + dtPrefix.Rows[0]["PO_T_FIRST_LETTER"].ToString() + "-" + CommonClasses.GenBillNo(Po_Doc_no);
                    }
                    SqlTransaction trans = null;

                    if (CommonClasses.Execute1("INSERT INTO SUPP_PO_MASTER (SPOM_CM_CODE,SPOM_AMOUNT,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_VALID_DATE,SPOM_USER_CODE,SPOM_POTYPE,SPOM_PROJ_NAME,SPOM_PONO,SPOM_CUST_CODE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtSupplierRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "','" + txtFreightTermsg.Text + "','" + txtGuranteeWaranty.Text + "','" + txtNote.Text.Trim().Replace("'", "\''") + "','" + txtDeliveryTo.Text + "','" + ddlCurrency.SelectedValue + "','" + txtCIF.Text + "','" + txtPacking.Text + "','" + ddlProjectCode.SelectedValue + "','" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["UserCode"].ToString()) + "',0,'" + txtProjName.Text.ToUpper().Trim() + "','" + strPONo + "','" + ddlCustomer.SelectedValue + "')"))
                    //if (CommonClasses.Execute1("INSERT INTO SUPP_PO_MASTER (SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "', 2 ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text + "','" + txtSupplierRefDate.Text + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
                        for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                        {
                            //CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_CURR_CODE) values ('" + Code + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCurrCode")).Text + "')");
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_E_TARRIF_CODE,SPOD_CENTRAL_TAX_AMT,SPOD_STATE_TAX_AMT,SPOD_INTEGRATED_TAX_AMT,SPOD_DOC_NAME) values ('" + Code + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_TARIFF_NO1")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ",'" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text) + "')");
                            CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");

                            #region UploadImage
                            if (ViewState["fileName"].ToString() != "")
                            {
                                string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");
                                ObjSearchDir = new DirectoryInfo(sDirPath);
                                string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                                DirectoryInfo dir = new DirectoryInfo(sDirPath1);
                                dir.Refresh();

                                if (!ObjSearchDir.Exists)
                                {
                                    ObjSearchDir.Create();
                                }
                                string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
                                string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                                string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                                string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                                DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                                FileInfo[] fi = di.GetFiles();
                                System.IO.File.Move(fullFilePath, copyToPath);
                                //DataTable dtUpdateDocName = CommonClasses.Execute("update INWARD_DETAIL set IWD_DOC_NAME='" + (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text) + "' where ");
                            }
                            #endregion UploadImage
                        }
                        for (int i = 0; i < dgDocView.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_ATTACHMENT (SPOA_SPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Code + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/SupplierPO/" + Code + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "')");
                        }
                        CommonClasses.WriteLog("Supplier Purchase Order", "Save", "Supplier Purchase Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;


                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ddlPOType.Focus();
                    }
                }
                #endregion INSERT

                #region MODIFY
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    if (CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' ,SPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim() + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "'    ,SPOM_PROJ_NAME='" + txtProjName.Text.ToUpper().Trim() + "' ,SPOM_CUST_CODE='" + ddlCustomer.SelectedValue + "',SPOM_DOC_NAME='" + ViewState["fileName"].ToString() + "'  where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        result1 = CommonClasses.Execute1("DELETE FROM SUPP_PO_ATTACHMENT WHERE SPOA_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                            {
                                //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,SPOD_TOTAL_AMT,SPOD_I_SIZE,SPOD_INW_QTY,SPOD_EXC_Y_N,SPOD_TAX_Y_N) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_I_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_UOM_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_ORDER_QTY")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_RATE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_DISC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_DISC_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_EXC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_T_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_TOTAL_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_I_SIZE")).Text + "',0,'" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_EXC_Y_N")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_TAX_Y_N")).Text + "')");
                                CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_E_TARRIF_CODE,SPOD_CENTRAL_TAX_AMT,SPOD_STATE_TAX_AMT,SPOD_INTEGRATED_TAX_AMT,SPOD_DOC_NAME) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_TARIFF_NO1")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ",'" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text) + "')");
                                CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }
                            for (int i = 0; i < dgDocView.Rows.Count; i++)
                            {
                                CommonClasses.Execute1("INSERT INTO SUPP_PO_ATTACHMENT (SPOA_SPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/SupplierPO/" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfileName"))).Text + "')");
                            }
                            CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                            CommonClasses.WriteLog("Supplier Purchase Order", "Update", "Supplier Purcahse Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                    }
                    else
                    {
                        //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        ddlPOType.Focus();
                    }
                }
                #endregion MODIFY

                #region AMEND
                else if (Request.QueryString[0].Equals("AMEND"))
                {
                    int AMEND_COUNT = 0;
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    dt = CommonClasses.Execute("select isnull(SPOM_AM_COUNT,0) as SPOM_AM_COUNT from SUPP_PO_MASTER WHERE SPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    if (dt.Rows.Count > 0)
                    {
                        AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["SPOM_AM_COUNT"]);
                        AMEND_COUNT = AMEND_COUNT + 1;
                    }
                    if (AMEND_COUNT == 0)
                    {
                        AMEND_COUNT = AMEND_COUNT + 1;
                    }
                    CommonClasses.Execute1("update  SUPP_PO_MASTER set SPOM_IS_SHORT_CLOSE='0' , SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',SPOM_AM_COUNT='" + AMEND_COUNT + "',SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim() + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "' WHERE SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    //if (CommonClasses.Execute1("INSERT INTO SUPP_PO_AM_MASTER select * from SUPP_PO_MASTER where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                    ////{

                    if (CommonClasses.Execute1("INSERT INTO SUPP_PO_AM_MASTER select SPOM_CODE,SPOM_CM_CODE,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_LM_CODE,SPOM_QUT_NO,SPOM_CONTACT_PERSON,SPOM_CONTACT_DETAIL,SPOM_PT_CODE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_DISC_PER,SPOM_DISC_AMT,SPOM_AMOUNT,SPOM_TRANSPORT_AMT,SPOM_TRANSPORT_DESC,SPOM_OCTROI_PER,SPOM_OCTROI_DESC,SPOM_DELIVERY_AT,SPOM_LOADING_PER,SPOM_LOADING_DESC,SPOM_FREIGHT_AMT,SPOM_FREIGHT_DESC,SPOM_TERM_CODE,SPOM_INW_QTY,SPOM_DET_TERMS,SPOM_SCHED_S_DATE,SPOM_SCHED_E_DATE,SPOM_PAY_TERM,SPOM_REMARK,SPOM_DELIVERY_MODE,SPOM_VALID_DATE,MODIFY,ES_DELETE,SPOM_TEN_ID,SPOM_POST,SPOM_P_AMEND_CODE,SPOM_DEL_SHCEDULE,SPOM_PAY_TERM1,SPOM_TERMS_COND,SPOM_IS_AMEND,SPOM_WITH_CASH,SPOM_PO_UNIT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_TRANSPOTER,SPOM_AUTHR_FLAG,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_IS_SHORT_CLOSE,SPOM_AM_COUNT,SPOM_AM_DATE,SPOM_CANCEL_PO,SPOM_PROJECT, SPOM_USER_CODE,SPOM_POTYPE,SPOM_PONO,SPOM_PROJ_NAME from SUPP_PO_MASTER where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        string MatserCode = CommonClasses.GetMaxId("Select Max(AM_CODE) from SUPP_PO_AM_MASTER");
                        DataTable dtDetail = CommonClasses.Execute("select * from SUPP_PO_DETAILS where SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        for (int j = 0; j < dtDetail.Rows.Count; j++)
                        {
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_AMD_DETAILS (SPOD_AMD_CODE,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE,SPOD_AMD_UOM_CODE,SPOD_AMD_ORDER_QTY,SPOD_AMD_RATE,SPOD_AMD_RATE_UOM,SPOD_AMD_CONV_RATIO,SPOD_AMD_DISC_PER,SPOD_AMD_DISC_AMT,SPOD_AMD_EXC_Y_N,SPOD_AMD_T_CODE,SPOD_AMD_TOTAL_AMT,SPOD_AMD_INW_QTY,SPOD_AMD_ACTIVE_IND,SPOD_AMD_SPECIFICATION,SPOD_AMD_EXC_PER,SPOD_AMD_EDU_CESS_PER,	SPOD_AMD_H_EDU_CESS,AMD_AM_CODE,SPOD_AMD_ITEM_NAME)  values('" + dtDetail.Rows[j]["SPOD_CODE"] + "','" + dtDetail.Rows[j]["SPOD_SPOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_I_CODE"] + "','" + dtDetail.Rows[j]["SPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_ORDER_QTY"] + "','" + dtDetail.Rows[j]["SPOD_RATE"] + "','" + dtDetail.Rows[j]["SPOD_RATE_UOM"] + "','" + dtDetail.Rows[j]["SPOD_CONV_RATIO"] + "','" + dtDetail.Rows[j]["SPOD_DISC_PER"] + "','" + dtDetail.Rows[j]["SPOD_DISC_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_Y_N"] + "','" + dtDetail.Rows[j]["SPOD_T_CODE"] + "','" + dtDetail.Rows[j]["SPOD_TOTAL_AMT"] + "','" + dtDetail.Rows[j]["SPOD_INW_QTY"] + "','" + dtDetail.Rows[j]["SPOD_ACTIVE_IND"] + "','" + dtDetail.Rows[j]["SPOD_SPECIFICATION"] + "','" + dtDetail.Rows[j]["SPOD_EXC_PER"] + "','" + dtDetail.Rows[j]["SPOD_EDU_CESS_PER"] + "','" + dtDetail.Rows[j]["SPOD_H_EDU_CESS"] + "','" + MatserCode + "','" + dtDetail.Rows[j]["SPOD_ITEM_NAME"] + "')");
                        }

                        #region ModifyOriginalPO
                        if (CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_P_CODE='" + ddlSupplier.SelectedValue + "',SPOM_PO_NO='" + txtPoNo.Text + "',SPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "',SPOM_AM_COUNT='" + AMEND_COUNT + "',SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' ,SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "'  ,SPOM_CUST_CODE='" + ddlCustomer.SelectedValue + "'  where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                        {
                            result = CommonClasses.Execute1("update  SUPP_PO_AM_MASTER set SPOM_AM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE SPOM_AM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and SPOM_AM_AM_COUNT='" + AMEND_COUNT + "'");
                            result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                            if (result)
                            {
                                for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                                {
                                    CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_E_TARRIF_CODE,SPOD_CENTRAL_TAX_AMT,SPOD_STATE_TAX_AMT,SPOD_INTEGRATED_TAX_AMT) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_INW_QTY")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_TARIFF_NO1")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ")");
                                    CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }

                                CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                                CommonClasses.WriteLog("Supplier Purchase Order", "Amend", "Supplier Purcahse Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                                ((DataTable)ViewState["dt2"]).Rows.Clear();
                                result = true;
                            }
                            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                        }

                        #endregion
                    }
                    else
                    {
                        //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "PO Not Amend";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlPOType.Focus();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Supplier Purchase Order", "SaveRec", ex.Message);
            }
        }
        return result;
    }
    #endregion

    #region ddlRateUOM_SelectedIndexChanged
    protected void ddlRateUOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStockUOM.SelectedItem.Text == ddlRateUOM.SelectedItem.Text)
        {
            txtConversionRetio.Enabled = false;
            txtConversionRetio.Text = "0";
            Calculate();
        }
        else
        {
            txtConversionRetio.Enabled = true;
        }
    }
    #endregion

    private void BlankGridView()
    {
        if (((DataTable)ViewState["dt2"]) != null)
        {
            ((DataTable)ViewState["dt2"]).Clear();
        }

        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode1");

            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");

            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("Round");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("NetTotal");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("ConversionRatio");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcInclusive");
            ((DataTable)ViewState["dt2"]).Columns.Add("Excdatepass");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcGapRate");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");

            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("round1");
            ((DataTable)ViewState["dt2"]).Columns.Add("PurVatCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("SerTCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_ITEM_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("ActiveInd");

            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_INW_QTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_TARIFF_NO");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_TARIFF_NO1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExisDuty");
            //((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            // ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");

        }
        ((DataTable)ViewState["dt2"]).Rows.Add(((DataTable)ViewState["dt2"]).NewRow());

        dgSupplierPurchaseOrder.Visible = true;
        dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
        dgSupplierPurchaseOrder.DataBind();
        dgSupplierPurchaseOrder.Enabled = false;

        ((DataTable)ViewState["dt3"]).Clear();
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SPOM_CODE");
            // ((DataTable)ViewState["dt3"]).Columns.Add("Download");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_NAME");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_PATH");
        }
        ((DataTable)ViewState["dt3"]).Rows.Add(((DataTable)ViewState["dt3"]).NewRow());

        dgDocView.Visible = true;
        dgDocView.DataSource = ((DataTable)ViewState["dt3"]);

        dgDocView.DataBind();
        // dgDocView.Enabled = false;
    }

    protected void btnAddDoc_Click(object sender, EventArgs e)
    {
        BindGridRow();
    }

    private void BindGridRow()
    {
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SPOM_CODE");
            // ((DataTable)ViewState["dt3"]).Columns.Add("Download");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_NAME");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_PATH");
        }
        ((DataTable)ViewState["dt3"]).Rows.Add(((DataTable)ViewState["dt3"]).NewRow());

        dgDocView.Visible = true;
        dgDocView.DataSource = ((DataTable)ViewState["dt3"]);
        dgDocView.DataBind();
    }

    protected void txtConversionRetio_TextChanged(object sender, EventArgs e)
    {
        Double TempTotal;
        if (ddlItemCode.SelectedIndex != 0)
        {
            if (txtTotalAmount.Text != "" || txtTotalAmount.Text != "0")
            {
                if (ddlStockUOM.SelectedItem.Text != ddlRateUOM.SelectedItem.Text)
                {


                    if (txtConversionRetio.Text != "" || txtConversionRetio.Text != "0")
                    {
                        Calculate();
                        //TempTotal = Convert.ToDouble(txtOrderQty.Text) * (Convert.ToDouble(txtConversionRetio.Text));
                        ////txtTotalAmount.Text = (TempTotal * (Convert.ToDouble(txtRate.Text))).ToString();
                        //txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round(TempTotal) * Convert.ToDouble(txtRate.Text.ToString()), 2);

                    }
                    else
                    {

                    }
                }
                else
                {
                    txtConversionRetio.Text = "";
                    txtConversionRetio.Enabled = false;

                    ddlRateUOM.SelectedIndex = 0;
                }
            }
            else
            {
                //ShowMessage("#Avisos", "Enter Qty and Rate", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Qty and Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                txtConversionRetio.Text = "";
                txtConversionRetio.Enabled = false;
                ddlRateUOM.SelectedIndex = 0;
            }
        }
        else
        {
            //ShowMessage("#Avisos", "Select on Item Code", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Please First Select on Item Code";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            txtConversionRetio.Text = "";
            txtConversionRetio.Enabled = false;
            ddlRateUOM.SelectedIndex = 0;
        }

    }

    protected void ddlStockUOM_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCurrrate = CommonClasses.Execute("SELECT CURR_CODE,CAST(CURR_RATE as numeric(20,2)) AS CURR_RATE FROM CURRANCY_MASTER WHERE ES_DELETE=0 and CURR_CODE='" + ddlCurrency.SelectedValue + "'");
            if (dtCurrrate.Rows.Count > 0)
            {
                //txtCurrencyRate.Text = dtCurrrate.Rows[0]["CURR_RATE"].ToString();
            }
            else
            {
                // txtCurrencyRate.Text = "0.00";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Po", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }

    }

    #region dgDocView_RowCommand
    protected void dgDocView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
        GridViewRow row = dgDocView.Rows[Convert.ToInt32(ViewState["Index"].ToString())];
        string filePath = "";
        FileInfo File;
        string code = "";
        string directory = "";

        if (e.CommandName == "Delete")
        {
            // int rowindex = row.RowIndex;
            //dgSupplierPurchaseOrder.DeleteRow(Index);
            ((DataTable)ViewState["dt3"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

            dgDocView.DataSource = ((DataTable)ViewState["dt3"]);
            dgDocView.DataBind();
            if (((DataTable)ViewState["dt3"]).Rows.Count == 0)
            {
                BlankGridView();
            }
        }
        if (e.CommandName == "View")
        {

            if (filePath != "")
            {
                File = new FileInfo(filePath);
            }
            else
            {
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    // CommonClasses.Execute("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
                    code = CommonClasses.GetMaxId("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
                }
                else
                {
                    code = ((Label)(row.FindControl("lblSPOA_SPOM_CODE"))).Text;
                    if (code == "")
                    {
                        code = CommonClasses.GetMaxId("Select SPOM_CODE from SUPP_PO_MASTER where SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                    }
                }
                filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");
            }
        }
        if (e.CommandName == "Download")
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
            }
            else
            {
                code = ((Label)(row.FindControl("lblSPOA_SPOM_CODE"))).Text;
                if (code == "")
                {
                    code = CommonClasses.GetMaxId("Select SPOM_CODE from SUPP_PO_MASTER where SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                }
            }
            filePath = ((Label)(row.FindControl("lblfilename"))).Text;
            directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
            Response.TransmitFile(Server.MapPath(directory));
            Response.End();
        }

    }
    #endregion

    #region FileLoactionCreate

    void FileLoactionCreate()
    {
        DataTable dt = new DataTable();
        if (Request.QueryString[0].Equals("MODIFY"))
        {
            ViewState["File_Code"] = Convert.ToInt32(ViewState["mlCode"].ToString());
        }
        else
        {
            dt = CommonClasses.Execute("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
            if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
            {
                DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('SUPP_PO_MASTER')+1");
                if (dt.Rows[0][0].ToString() == "-2147483647")
                {
                    ViewState["File_Code"] = -2147483648;
                }
                else
                {
                    ViewState["File_Code"] = int.Parse(dt1.Rows[0][0].ToString());
                }
            }
            else
            {
                ViewState["File_Code"] = int.Parse(dt.Rows[0][0].ToString());
            }
        }

        string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["File_Code"].ToString() + "");

        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
    }

    #endregion

    protected void dgDocView_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            ViewState["fileName"] = null;
            FileLoactionCreate();

            GridViewRow row = dgDocView.Rows[e.RowIndex];

            FileUpload flUp = (FileUpload)row.FindControl("imgUpload");
            string directory = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["File_Code"].ToString() + "");

            string fileName = Path.GetFileName(flUp.PostedFile.FileName);

            flUp.SaveAs(Path.Combine(directory, fileName));

            ((Label)(dgDocView.Rows[e.RowIndex].FindControl("lblfilename"))).Text = fileName.ToString();
            ViewState["fileName"] = ViewState["File_Code"].ToString() + "/" + fileName.ToString();
        }
        catch
        {

        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    LinkButton lnkbtn = sender as LinkButton;
        //    GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        //    string filePath = dgDocView.DataKeys[gvrow.RowIndex].Value.ToString();
        //    FileInfo File = new FileInfo(filePath);
        //    string filename = File.Name;
        //    if (filePath != "")
        //    {
        //        //Response.ContentType = "image/jpg";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
        //        Response.TransmitFile(Server.MapPath(filePath));
        //        Response.End();
        //    }
        //}
        //catch
        //{

        //}
    }

    #region dgDocView_RowDeleting
    protected void dgDocView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region chkExcInclusive_CheckedChanged
    protected void chkExcInclusive_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExcInclusive.Checked == true)
        {
            txtExcisePer.Text = "0.00";
            txtExcisePer.Enabled = false;
        }
        else
        {
            txtExcisePer.Enabled = true;
        }
    }
    #endregion

}
