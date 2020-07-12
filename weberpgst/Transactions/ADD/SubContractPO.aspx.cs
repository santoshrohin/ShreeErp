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

public partial class Transactions_ADD_SubContractPO : System.Web.UI.Page
{
    #region Variable
    DirectoryInfo ObjSearchDir;
    string fileName1 = "";
    // string path = "";
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    string fileName = "";
    DataTable dtPODetail = new DataTable();
    int Active = 0;
    static int mlCode = 0;
    DataRow dr;
    static int File_Code = 0;
    string path = "";
    public static DataTable dt3 = new DataTable();

    public static DataTable dt2 = new DataTable();
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
                    ViewState["Qtn"] = "";
                    ViewState["fileName"] = fileName;
                    ViewState["fileName1"] = fileName1;
                    ViewState["mlCode"] = mlCode;
                    ViewState["Index"] = Index;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;

                    if (dt2 == null)
                    {
                        dt2 = new DataTable();
                    }
                    if (dt3 == null)
                    {
                        dt3 = new DataTable();
                    }
                    ViewState["dt2"] = dt2;
                    ViewState["dt3"] = dt3;
                    LoadType();
                    LoadProCode();
                    LoadITariff();
                    try
                    {
                        //BindTable.Clear();

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
                            LoadRateUOM();
                            LoadCurr();
                            LoadProcess();
                            //CreateDataTable();
                            chkActiveInd.Checked = true;
                            BlankGridView();
                            txtConversionRetio.Enabled = false;
                            //CarryForward();
                            txtPoDate.Attributes.Add("readonly", "readonly");
                             
                            //txtPoDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtPoDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
                            txtSupplierRefDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtdispach.Text = "0.00";
                            txtInward.Text = "0.00";
                            txtValid.Attributes.Add("readonly", "readonly");
                            txtValid.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                            //dt2.Rows.Clear();
                            //dt2.Columns.Clear();
                        }
                        ddlPOType.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Supplier Order", "Page_Load", ex.Message.ToString());
                    }
                }
                // imgUpload.Attributes["onchange"] = "UploadFile(this)";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
        }
        #region Doc_Upload
        if (IsPostBack && imgUpload.PostedFile != null)
        {
            if (imgUpload.PostedFile.FileName.Length > 0)
            {
                fileName = imgUpload.PostedFile.FileName;
                ViewState["fileName"] = fileName;
                Upload(null, null);
            }
        }
        #endregion Doc_Upload

        #region AttachQtn
        if (IsPostBack && qtnUpload.PostedFile != null)
        {
            if (qtnUpload.PostedFile.FileName.Length > 0)
            {
                ViewState["Qtn"] = qtnUpload.PostedFile.FileName;
                UploadQtn(null, null);
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
            sDirPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName"].ToString()));
        }
        else
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/SubConPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
        //lnkView.Visible = true;
        //lnkView.Text = ViewState["fileName"].ToString();
    }
    #endregion

    #region UploadQtn
    protected void UploadQtn(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (qtnUpload.PostedFile.ContentLength > 0)
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                qtnUpload.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["Qtn"].ToString()));
            }
            else
            {
                qtnUpload.SaveAs(Server.MapPath("~/UpLoadPath/SubConPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["Qtn"].ToString()));
            }

            lnkView.Text = ViewState["Qtn"].ToString();
        }
    }
    #endregion Upload

    #region lnkView_Click
    protected void lnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            // int height = Convert.ToInt32(FileUpload1.Height);

            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkView.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkView.Text;
                directory = "../../UpLoadPath/SubConPO/" + code + "/" + filePath;

            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }
    #endregion

    #region Upload_Old_Code_Comment
    //protected void Upload(object sender, EventArgs e)
    //{
    //    string Code = ddlItemName.SelectedValue;
    //    if (Code == "0")
    //    {
    //        PanelMsg.Visible = true;
    //        lblmsg.Text = "Please select Item Name first";
    //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    //        ddlItemCode.Focus();
    //        return;
    //    }
    //    string sDirPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + Code + "");

    //    ObjSearchDir = new DirectoryInfo(sDirPath);

    //    if (!ObjSearchDir.Exists)
    //    {
    //        ObjSearchDir.Create();
    //    }
    //    imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/SubConPO/" + Code + "/" + Path.GetFileName(imgUpload.FileName)));
    //    fileName1 = Path.GetFileName(imgUpload.FileName);
    //    ViewState["fileName1"] = fileName1;
    //    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DOC_NAME='" + fileName1 + "',I_DOC_PATH='" + "~/UpLoadPath/SubConPO/" + Code + "" + '/' + "" + fileName1 + "' where I_CODE='" + Code + "'");
    //    // lblMessage.Visible = true;
    //}
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

    #region LoadProcess
    private void LoadProcess()
    {
        try
        {
            dt = CommonClasses.Execute("select PROCESS_CODE,PROCESS_NAME from PROCESS_MASTER where ES_DELETE=0 and PROCESS_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCESS_NAME");
            ddlProcess.DataSource = dt;
            ddlProcess.DataTextField = "PROCESS_NAME";
            ddlProcess.DataValueField = "PROCESS_CODE";
            ddlProcess.DataBind();
            // ddlProcess.Items.Insert(0, new ListItem("Process Type", "0"));
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
            if (ddlSupplier.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Supplier";
                ddlSupplier.Focus();
                return;
            }
            if (txtPoDate.Text == "")
            {
                ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtPoDate.Focus();
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
                // tab_2_2.Visible = true;
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
            CommonClasses.SendError("Sub Contractor Purchase Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CreateDataTable
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
            ((DataTable)ViewState["dt2"]).Columns.Add("ProcessCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ProcessName");
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

            ((DataTable)ViewState["dt2"]).Columns.Add("CostWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("FinishWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("TurningWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_E_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_HE_AMT");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_TARIFF_NO");


        }
        #endregion
        ViewState["NewTable"] = ((DataTable)ViewState["dt2"]);
    }
    #endregion CreateDataTable

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        Double Per = 100;
        // string fileName1 = "";
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
            //if (txtOrderQty.Text == "" || txtOrderQty.Text == "0.000")
            //{
            //    //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter  Order Qty";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtOrderQty.Focus();
            //    return;
            //}

            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtdispach.Text) && Convert.ToDouble(txtOrderQty.Text) != 0)
            {
                //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should Be grater then Dispatch Qty  " + txtdispach.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }

            if (txtRate.Text == "" || txtRate.Text == "0.00")
            {
                //ShowMessage("#Avisos", "Please Enter Rate", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Rate";
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
            if (txtCostWeight.Text.Trim() == "")
            {
                txtCostWeight.Text = "0";
            }
            if (txtfinishweight.Text.Trim() == "")
            {
                txtfinishweight.Text = "0";
            }
            if (txtTruningWeight.Text.Trim() == "")
            {
                txtTruningWeight.Text = "0";
            }
            if (txtdispach.Text.Trim() == "")
            {
                txtdispach.Text = "0";
            }
            if (txtInward.Text.Trim() == "")
            {
                txtInward.Text = "0";
            }
            if (txtDescPerc.Text == "")
            {
                txtDescPerc.Text = "0";
            }
            else if (Convert.ToDouble(txtDescPerc.Text) > Per)
            {
                //ShowMessage("#Avisos", "Percentage not greter than 100", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Percentage not greter than 100";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDescPerc.Focus();
                return;
            }
            if (txtExcisePer.Text.Trim() == "")
            {
                txtExcisePer.Text = "0.00";
            }
            //if (chkExcInclusive.Checked == false && Convert.ToDouble(txtExcisePer.Text) <= 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter Excise % ";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtExcisePer.Focus();
            //    return;
            //}

            //  dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE  from SALES_TAX_MASTER where  ST_CODE=" + ddlSalesTax.SelectedValue + " and ES_DELETE=0 and  ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "");
            Double Tax = 0;
            Double Subamount = (Convert.ToDouble(txtTotalAmount.Text.ToString()) - DescAmount);

            double AccesableValue = Convert.ToDouble(txtTotalAmount.Text) - Convert.ToDouble(txtDescAmount.Text);


            double E_BASIC = 0;
            double E_EDU_CESS = 0;
            double E_H_EDU = 0;

            double SPOD_EXC_AMT = 0;
            double SPOD_EXC_E_AMT = 0;
            double SPOD_EXC_HE_AMT = 0;

            //if (!chkExcInclusive.Checked)
            //{
            dt = CommonClasses.Execute("select E_BASIC, E_EDU_CESS,E_H_EDU from EXCISE_TARIFF_MASTER,ITEM_MASTER where I_SCAT_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString());
                E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString());
                E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString());
                //E_BASIC = 9;
                //E_EDU_CESS = 9;
                //E_H_EDU = 18;

                DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
                DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlSupplier.SelectedValue + "'");

                if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
                {
                    SPOD_EXC_AMT = Math.Round(AccesableValue * E_BASIC / 100);
                    SPOD_EXC_E_AMT = Math.Round(AccesableValue * E_EDU_CESS / 100);
                }
                else
                {
                    SPOD_EXC_HE_AMT = Math.Round(AccesableValue * E_H_EDU / 100);
                }
            }
            //}
            //E_BASIC = Convert.ToDouble(txtExcisePer.Text);
            //double ExisDuty = Subamount * Convert.ToDouble(txtExcduty.Text.ToString()) / Convert.ToDouble( 100);
            // double EduCess = ExisDuty * Convert.ToDouble(txtEduCess.Text.ToString()) / Convert.ToDouble(100);
            // double SHECess = ExisDuty * Convert.ToDouble(txtSHECess.Text.ToString()) / Convert.ToDouble(100);
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

            //((DataTable)ViewState["dt2"]) = (DataTable)ViewState["NewTable"]; 
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                for (int i = ((DataTable)ViewState["dt2"]).Rows.Count - 1; i >= 0; i--)
                {
                    if (((DataTable)ViewState["dt2"]).Rows[i][1] == DBNull.Value)
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
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));

            dr["RateUOM1"] = ddlRateUOM.SelectedValue;
            dr["RateUOM"] = ddlRateUOM.SelectedItem;

            dr["ProcessCode"] = ddlProcess.SelectedValue;
            dr["ProcessName"] = ddlProcess.SelectedItem;
            //dr["Round"] = ChkRound.Checked;
            dr["TotalAmount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtTotalAmount.Text)), 2));
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

            dr["CostWeight"] = txtCostWeight.Text;
            dr["FinishWeight"] = txtfinishweight.Text.Trim();
            dr["TurningWeight"] = txtTruningWeight.Text.Trim();
            dr["SPOD_INW_QTY"] = txtInward.Text.Trim();
            dr["SPOD_DISPACH"] = txtdispach.Text.Trim();
            // dr["DocName"] = imgUpload.PostedFile.FileName;
            dr["DocName"] = ViewState["fileName"].ToString();

            dr["SPOD_E_CODE"] = "-2147483547";
            dr["SPOD_E_TARIFF_NO"] = "998932";

            dr["SPOD_EXC_AMT"] = SPOD_EXC_AMT;
            dr["SPOD_EXC_E_AMT"] = SPOD_EXC_E_AMT;
            dr["SPOD_EXC_HE_AMT"] = SPOD_EXC_HE_AMT;

            //string Code = ddlItemName.SelectedValue;
            //File_Code = Convert.ToInt32(Code);
            //string sDirPath = Server.MapPath(@"~/UpLoadPath/" + File_Code + "");

            //ObjSearchDir = new DirectoryInfo(sDirPath);

            //if (!ObjSearchDir.Exists)
            //{
            //    ObjSearchDir.Create();
            //}
            //if (imgUpload.HasFile)
            //{
            //    //FileUpload flUp = new FileUpload();
            //    File_Code = Convert.ToInt32(ddlItemCode.SelectedValue);
            //    string directory = Server.MapPath(@"~/UpLoadPath/" + File_Code + "");
            //    //string fExtension = Path.GetExtension(imgUpload.PostedFile.FileName);
            //    fileName1 = Path.GetFileName(imgUpload.FileName);
            //    fileName1 = fileName1.Replace("'", "''");
            //    imgUpload.SaveAs(Path.Combine(directory, fileName1));
            //    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DOC_NAME='" + fileName1 + "',I_DOC_PATH='" + "~/UpLoadPath/" + Code + "" + '/' + "" + fileName1 + "' where I_CODE='" + Code + "'");
            //}

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
            CommonClasses.SendError("Sub Contractor Purchase Order", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Sub Contractor Purchase Order", "btnOk_Click", Ex.Message);
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
            Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "   ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order", "LoadICode", Ex.Message);
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
            ddlPOType.Items.Insert(0, new ListItem("Select Po Type", "0"));
            ddlPOType.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "   ORDER BY I_NAME");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order ", "LoadIName", Ex.Message);
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
            ddlSalesTax.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order ", "LoadTax", Ex.Message);
        }

    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1   order by P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order", "LoadCustomer", Ex.Message);
        }

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
            CommonClasses.SendError("Sub Contractor Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
                SetHSN();
                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
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
            CommonClasses.SendError("Sub Contractor Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
        if (ddlPOType.SelectedValue == "-2147483647")
        {
            pnlCurrancy.Visible = true;
        }
        else
        {
            pnlCurrancy.Visible = false;

        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedIndex != 0)
        {
            CarryForward();
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
            DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER where ES_DELETE=0 and CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "CURR_NAME";
            ddlCurrency.DataValueField = "CURR_CODE";
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("Select currency", "0"));
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

                SetHSN();
                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                //DataTable dt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemName.SelectedValue + "'");
                //txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["I_INV_RATE"]);
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
            CommonClasses.SendError("Sub Contractor Purchase Order -Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
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
            LoadProcess();
            dtPODetail.Clear();
            txtPoDate.Attributes.Add("readonly", "readonly");
            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
            txtValid.Attributes.Add("readonly", "readonly");

            dt = CommonClasses.Execute("Select ISNULL(SPOM_DOC_NAME,'') AS SPOM_DOC_NAME,SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_VALID_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_PONO,SPOM_PROJ_NAME from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SPOM_CODE"]);
                ddlPOType.SelectedValue = dt.Rows[0]["SPOM_TYPE"].ToString();
                if (ddlPOType.SelectedValue == "-2147483647")
                {
                    pnlCurrancy.Visible = true;
                    ddlCurrency.SelectedValue = dt.Rows[0]["SPOM_CURR_CODE"].ToString();
                    ddlCurrency_SelectedIndexChanged(null, null);
                }
                else
                {
                    pnlCurrancy.Visible = false;
                }
                ddlSupplier.SelectedValue = dt.Rows[0]["SPOM_P_CODE"].ToString();
                lnkView.Text = dt.Rows[0]["SPOM_DOC_NAME"].ToString();
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
                // dtPODetail = CommonClasses.Execute("select SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,cast(SPOD_DISC_PER as float) as SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,SPOD_TOTAL_AMT,SPOD_I_SIZE,I_NAME,UOM_NAME,T_NAME,isnull(SPOD_EXC_Y_N,0) as SPOD_EXC_Y_N,isnull(SPOD_TAX_Y_N,0) as SPOD_TAX_Y_N FROM SUPP_PO_MASTER,SUPP_PO_DETAIL,ITEM_MASTER,UNIT_MASTER,TAX_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE and SPOD_UOM_CODE=UNIT_MASTER.UOM_CODE and SPOD_T_CODE=TAX_MASTER.T_CODE and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOD_SPOM_CODE='" + mlCode + "'");
                //dtPODetail = CommonClasses.Execute("select SPOD_I_CODE as ItemCode,SPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, cast(SPOD_ORDER_QTY as numeric(10,3)) as OrderQty,cast(SPOD_RATE as numeric(20,2)) as Rate,SPOD_RATE_UOM as RateUOM1,I_UOM_NAME as RateUOM,SPOD_CONV_RATIO as ConversionRatio,cast(SPOD_TOTAL_AMT as numeric(20,2)) as TotalAmount,cast(SPOD_DISC_PER as float) as DiscPerc,cast(SPOD_DISC_AMT as numeric(20,2)) as DiscAmount,isnull(SPOD_EXC_Y_N,0) as ExcInclusive,SPOD_T_CODE as SalesTax1,ST_TAX_NAME as SalesTax ,SPOD_ACTIVE_IND as ActiveInd ,SPOD_SPECIFICATION as Specification,SPOD_ITEM_NAME,SPOD_EXC_PER as E_BASIC, SPOD_EDU_CESS_PER as E_EDU_CESS,SPOD_H_EDU_CESS as E_H_EDU  ,SPOD_PROCESS_CODE AS ProcessCode,PROCESS_NAME AS ProcessName,ISNULL(SPOD_COSTWEIGHT,0) AS CostWeight,ISNULL(SPOD_FINISHWEIGHT,0) AS FinishWeight,ISNULL(SPOD_TURNINGWEIGHT,0) AS TurningWeight,ISNULL(SPOD_INW_QTY,0) AS SPOD_INW_QTY,ISNULL(SPOD_DISPACH,0) AS SPOD_DISPACH,I_DOC_NAME as DocName  ,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT, ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,ISNULL(SPOD_E_CODE,0) AS SPOD_E_CODE,ISNULL(SPOD_E_TARIFF_NO,0) AS SPOD_E_TARIFF_NO   FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,SALES_TAX_MASTER,PROCESS_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SUPP_PO_DETAILS.SPOD_UOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE and SPOD_T_CODE=SALES_TAX_MASTER.ST_CODE and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'  AND SPOD_PROCESS_CODE=PROCESS_CODE");

                dtPODetail = CommonClasses.Execute("select SPOD_I_CODE as ItemCode,SPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, cast(SPOD_ORDER_QTY as numeric(10,3)) as OrderQty,cast(SPOD_RATE as numeric(20,3)) as Rate,SPOD_RATE_UOM as RateUOM1,ISNULL((SELECT I_UOM_NAME  FROM ITEM_UNIT_MASTER WHERE I_UOM_CODE=SPOD_RATE_UOM),'') as RateUOM,SPOD_CONV_RATIO as ConversionRatio,cast(SPOD_TOTAL_AMT as numeric(20,2)) as TotalAmount,cast(SPOD_DISC_PER as float) as DiscPerc,cast(SPOD_DISC_AMT as numeric(20,2)) as DiscAmount,isnull(SPOD_EXC_Y_N,0) as ExcInclusive,0 as  SalesTax1,''  as SalesTax ,SPOD_ACTIVE_IND as ActiveInd ,SPOD_SPECIFICATION as Specification,SPOD_ITEM_NAME,SPOD_EXC_PER as E_BASIC, SPOD_EDU_CESS_PER as E_EDU_CESS,SPOD_H_EDU_CESS as E_H_EDU  ,SPOD_PROCESS_CODE AS ProcessCode,PROCESS_NAME AS ProcessName,ISNULL(SPOD_COSTWEIGHT,0) AS CostWeight,ISNULL(SPOD_FINISHWEIGHT,0) AS FinishWeight,ISNULL(SPOD_TURNINGWEIGHT,0) AS TurningWeight,ISNULL(SPOD_INW_QTY,0) AS SPOD_INW_QTY,ISNULL(SPOD_DISPACH,0) AS SPOD_DISPACH,SPOD_DOC_NAME as DocName  ,ISNULL(SPOD_CENTRAL_TAX_AMT,0) AS SPOD_EXC_AMT, ISNULL(SPOD_STATE_TAX_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_INTEGRATED_TAX_AMT,0) AS SPOD_EXC_HE_AMT,0 as  SPOD_E_TARIFF_NO,ISNULL(SPOD_E_TARRIF_CODE,0) AS SPOD_E_CODE FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,PROCESS_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SUPP_PO_DETAILS.SPOD_UOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE  and SPOM_CM_COMP_ID='1' and SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'  AND SPOD_PROCESS_CODE=PROCESS_CODE");
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
                //dgSupplierPurchaseOrder.Enabled = false;
                dgSupplierPurchaseOrder.Columns[0].Visible = false;
                dgSupplierPurchaseOrder.Columns[1].Visible = false;
                ddlCurrency.Enabled = false;
                txtCurrencyRate.Enabled = false;
                txtMissItemName.Enabled = false;
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
            CommonClasses.SendError(" Sub Contractor Purchase Order Transaction", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Sub Contractor Purchase Order ", "ShowMessage", Ex.Message);
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
            ViewState["fileName"] = "";
            ddlSalesTax.SelectedIndex = 0;
            txtExcisePer.Text = "0.00";

            txtInward.Text = "0.00";
            txtdispach.Text = "0.00";
            txtSpecification.Text = "";
            // chkActiveInd.Checked = false;
            txtMissItemName.Text = "";
            txtTruningWeight.Text = "0.00";
            txtfinishweight.Text = "0.00";
            txtCostWeight.Text = "0.00";
            //txtAmount.Text = "0.00";
            //txtCustItemCode.Text = "";
            //txtCustItemName.Text = "";
            //ddlTaxCategory.SelectedIndex = 0;
            //ddlCurrancy.SelectedIndex = 0;
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order", "clearDetail", Ex.Message);
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

                string SGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT"))).Text;
                string CGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT"))).Text;
                string IGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT"))).Text;

                CGST_AMT = CGST_AMT + Convert.ToDouble(CGST);
                SGST_AMT = SGST_AMT + Convert.ToDouble(SGST);
                IGST_AMT = IGST_AMT + Convert.ToDouble(IGST);
            }
        }
        txtFinalTotalAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal + CGST_AMT + SGST_AMT + IGST_AMT), 2);





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
            //if (txtRate.Text == "")
            //{
            //    txtRate.Text = "0";
            //}

            //txtTotalAmount.Text = (Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())).ToString();

            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtdispach.Text) && Convert.ToDouble(txtOrderQty.Text) != 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should not less than Dispatch Qty " + txtdispach.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            Calculate();
            txtRate.Focus();
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
            //    no = 6;
            //}
            //int n = no - 1;
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
        double Amount = 0; string OrderQty = ""; string OrderRate = "";
        OrderQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
        OrderRate = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtRate.Text), 3));
        Amount = Convert.ToDouble(Math.Round((Convert.ToDouble(OrderQty) * Convert.ToDouble(OrderRate)), 0, MidpointRounding.AwayFromZero));
        txtTotalAmount.Text = Amount.ToString();
        //txtRate.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        //txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2));

        if (ddlStockUOM.SelectedValue.ToString() != ddlRateUOM.SelectedValue.ToString())
        {
            totalStr = DecimalMasking(txtConversionRetio.Text);
            txtConversionRetio.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));

            double TempTotal = Convert.ToDouble(txtOrderQty.Text) * (Convert.ToDouble(txtConversionRetio.Text));
            txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round(TempTotal) * Convert.ToDouble(txtRate.Text.ToString()), 3);

        }
        totalStr = DecimalMasking(txtDescPerc.Text);
        txtDescPerc.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));
        txtDescAmount.Text = string.Format("{0:0.00}", Math.Round((((Convert.ToDouble(txtTotalAmount.Text.ToString())) / 100) * Convert.ToDouble(txtDescPerc.Text)), 3));

    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            //if (txtOrderQty.Text == "")
            //{
            //    txtOrderQty.Text = "0";
            //}
            //if (txtRate.Text == "")
            //{
            //    txtRate.Text = "0";
            //}
            //txtTotalAmount.Text = (Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())).ToString();
            ////      txtAmount.Text = amount.ToString();
            Calculate();
            txtDescPerc.Focus();
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
        //txtDescAmount.Text = (Convert.ToDouble(txtDescPerc.Text.ToString()) * Convert.ToDouble(txtTotalAmount.Text.ToString())/100).ToString();
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
                //string POCode = ViewState["mlCode"].ToString();

                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT IND_CON_QTY,IND_CON_QTY,* FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OUTSUBINM' AND IND_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IND_I_CODE='" + ICode + "'");

                if (dtCheckExistItem.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cannot delete the item because the item used in Dispatch";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtOrderQty.Focus();
                    return;
                }

                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
                dgSupplierPurchaseOrder.DataBind();
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
                LoadUnit();
                LoadRateUOM();
                LoadTax();
                // LoadPOUnit();

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
                if (ddlStockUOM.SelectedValue != ddlRateUOM.SelectedValue)
                {
                    txtConversionRetio.Enabled = true;
                }
                //ChkRound.Checked = ((Label)(row.FindControl("lblRound"))).Text;
                txtTotalAmount.Text = ((Label)(row.FindControl("lblTotalAmount"))).Text;
                txtDescPerc.Text = ((Label)(row.FindControl("lblDiscPerc"))).Text;
                txtDescAmount.Text = ((Label)(row.FindControl("lblDiscAmount"))).Text;
                txtConversionRetio.Text = ((Label)(row.FindControl("lblConversionRatio"))).Text;

                txtExcisePer.Text = ((Label)(row.FindControl("lblE_BASIC"))).Text;

                ddlSalesTax.SelectedValue = ((Label)(row.FindControl("lblSalesTax1"))).Text;

                ddlProcess.SelectedValue = ((Label)(row.FindControl("lblProcessCode"))).Text;
                ViewState["fileName"] = ((LinkButton)(row.FindControl("lnkView"))).Text;
                txtSpecification.Text = ((Label)(row.FindControl("lblSpecification"))).Text;
                txtMissItemName.Text = ((Label)(row.FindControl("lblSPOD_ITEM_NAME"))).Text;
                if ((((Label)(row.FindControl("lblExcInclusive"))).Text).ToString() == "True")
                {
                    chkExcInclusive.Checked = true;
                }
                else
                {
                    chkExcInclusive.Checked = false;
                }

                if ((((Label)(row.FindControl("lblActiveInd"))).Text) == "1")
                {
                    chkActiveInd.Checked = true;
                }
                else
                {
                    chkActiveInd.Checked = false;
                }

                txtCostWeight.Text = ((Label)(row.FindControl("lblCostWeight"))).Text;
                txtfinishweight.Text = ((Label)(row.FindControl("lblFinishWeight"))).Text;
                txtTruningWeight.Text = ((Label)(row.FindControl("lblTurningWeight"))).Text;

                txtInward.Text = ((Label)(row.FindControl("lblSPOD_INW_QTY"))).Text;
                txtdispach.Text = ((Label)(row.FindControl("lblSPOD_DISPACH"))).Text;

                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT IND_CON_QTY,IND_CON_QTY,* FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OUTSUBINM' AND IND_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IND_I_CODE='" + ddlItemCode.SelectedValue + "'");
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
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                    directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                }
                else
                {
                    code = ViewState["mlCode"].ToString();
                    filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                    directory = "../../UpLoadPath/SubConPO/" + code + "/" + filePath;
                }

                IframeViewPDF.Attributes["src"] = directory;
                ModalPopDocument.Show();
                PopDocument.Visible = true;
                // Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order Transaction", "dgMainPO_RowCommand", Ex.Message);
        }
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
            ddlProjectCode.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadProCode", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierPurchaseOrder_RowDeleting
    protected void dgSupplierPurchaseOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        bool result1 = false;
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
                string strPONo = "";
                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(SPOM_PO_NO),0) as SPOM_PO_NO FROM SUPP_PO_MASTER WHERE SPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and SPOM_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0  AND SPOM_POTYPE=1");
                if (dt.Rows.Count > 0)
                {
                    //PCPCL0000
                    if (true)
                    {

                    }
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["SPOM_PO_NO"]);
                    Po_Doc_no = Po_Doc_no + 1;
                    string strYear = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy").Substring(9, 2) + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy").ToString().Substring(9, 2);
                    ///strPONo = "SC-PCPL" + CommonClasses.GenBillNo(Po_Doc_no);
                    strPONo = "SC" + strYear + CommonClasses.GenBillNo(Po_Doc_no);
                }
                SqlTransaction trans = null;

                if (CommonClasses.Execute1("INSERT INTO SUPP_PO_MASTER (SPOM_CM_CODE,SPOM_AMOUNT,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_VALID_DATE,SPOM_USER_CODE,SPOM_POTYPE, SPOM_PONO,SPOM_PROJ_NAME,SPOM_DOC_NAME)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text + "','" + Convert.ToDateTime(txtSupplierRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "','" + txtFreightTermsg.Text + "','" + txtGuranteeWaranty.Text + "','" + txtNote.Text + "','" + txtDeliveryTo.Text + "','" + ddlCurrency.SelectedValue + "','" + txtCIF.Text + "','" + txtPacking.Text + "','" + ddlProjectCode.SelectedValue + "','" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["UserCode"].ToString()) + "',1,'" + strPONo + "','" + txtProjName.Text.ToUpper().Trim() + "','" + lnkView.Text + "')"))
                //if (CommonClasses.Execute1("INSERT INTO SUPP_PO_MASTER (SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "', 2 ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text + "','" + txtSupplierRefDate.Text + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
                    for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                    {
                        //CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_CURR_CODE) values ('" + Code + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCurrCode")).Text + "')");
                        //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH) values ('" + Code + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0)");

                        CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_TARRIF_CODE,SPOD_STATE_TAX_AMT,SPOD_CENTRAL_TAX_AMT,SPOD_INTEGRATED_TAX_AMT,SPOD_DOC_NAME) values ('" + Code + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + ",'" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text) + "')");
                        CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");

                        #region Document_Upload
                        if (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + Code + "");
                            ObjSearchDir = new DirectoryInfo(sDirPath);
                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);
                            dir.Refresh();
                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + Code + "/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion Document_Upload
                    }
                    for (int i = 0; i < dgDocView.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO SUPP_PO_ATTACHMENT (SPOA_SPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Code + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/SupplierPO/" + Code + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "')");
                    }
                    CommonClasses.WriteLog("Sub Contractor Purchase Order", "Save", "Sub Contractor Purchase Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;

                    #region UploadImage
                    if (lnkView.Text.Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + Code + "");
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
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + lnkView.Text.Trim());
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SubConPO/" + Code + "/" + lnkView.Text.Trim());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        FileInfo[] fi = di.GetFiles();
                        System.IO.File.Move(fullFilePath, copyToPath);
                        //DataTable dtUpdateDocName = CommonClasses.Execute("update INWARD_DETAIL set IWD_DOC_NAME='" + (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text) + "' where ");
                    }
                    #endregion UploadImage

                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ddlPOType.Focus();
                }
            }
            #endregion

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_P_CODE='" + ddlSupplier.SelectedValue + "',SPOM_PO_NO='" + txtPoNo.Text + "',SPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim() + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_DOC_NAME='" + lnkView.Text + "'  ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "' where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result1 = CommonClasses.Execute1("DELETE FROM SUPP_PO_ATTACHMENT WHERE SPOA_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                        {
                            //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_T_CODE,SPOD_TOTAL_AMT,SPOD_I_SIZE,SPOD_INW_QTY,SPOD_EXC_Y_N,SPOD_TAX_Y_N) values ('" + mlCode + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_I_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_UOM_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_ORDER_QTY")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_RATE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_DISC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_DISC_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_EXC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_T_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_TOTAL_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_I_SIZE")).Text + "',0,'" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_EXC_Y_N")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSPOD_TAX_Y_N")).Text + "')");
                            //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0)");
                            // CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT) values              ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_TARIFF_NO")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + ")");
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_TARRIF_CODE,SPOD_STATE_TAX_AMT,SPOD_CENTRAL_TAX_AMT,SPOD_INTEGRATED_TAX_AMT,SPOD_DOC_NAME) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + ",'" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text) + "')");
                            CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                        }
                        for (int i = 0; i < dgDocView.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_ATTACHMENT (SPOA_SPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/SupplierPO/" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfileName"))).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Sub Contractor Purchase Order", "Update", "Sub Contractor Purchase Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPOType.Focus();
                }
            }
            #endregion

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
                CommonClasses.Execute1("update  SUPP_PO_MASTER set SPOM_IS_SHORT_CLOSE='0', SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',SPOM_AM_COUNT='" + AMEND_COUNT + "' ,SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim() + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "' WHERE SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                //if (CommonClasses.Execute1("INSERT INTO SUPP_PO_AM_MASTER select * from SUPP_PO_MASTER where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                //{

                if (CommonClasses.Execute1("INSERT INTO SUPP_PO_AM_MASTER select SPOM_CODE,SPOM_CM_CODE,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_LM_CODE,SPOM_QUT_NO,SPOM_CONTACT_PERSON,SPOM_CONTACT_DETAIL,SPOM_PT_CODE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_DISC_PER,SPOM_DISC_AMT,SPOM_AMOUNT,SPOM_TRANSPORT_AMT,SPOM_TRANSPORT_DESC,SPOM_OCTROI_PER,SPOM_OCTROI_DESC,SPOM_DELIVERY_AT,SPOM_LOADING_PER,SPOM_LOADING_DESC,SPOM_FREIGHT_AMT,SPOM_FREIGHT_DESC,SPOM_TERM_CODE,SPOM_INW_QTY,SPOM_DET_TERMS,SPOM_SCHED_S_DATE,SPOM_SCHED_E_DATE,SPOM_PAY_TERM,SPOM_REMARK,SPOM_DELIVERY_MODE,SPOM_VALID_DATE,MODIFY,ES_DELETE,SPOM_TEN_ID,SPOM_POST,SPOM_P_AMEND_CODE,SPOM_DEL_SHCEDULE,SPOM_PAY_TERM1,SPOM_TERMS_COND,SPOM_IS_AMEND,SPOM_WITH_CASH,SPOM_PO_UNIT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_TRANSPOTER,SPOM_AUTHR_FLAG,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_IS_SHORT_CLOSE,SPOM_AM_COUNT,SPOM_AM_DATE,SPOM_CANCEL_PO,SPOM_PROJECT, SPOM_USER_CODE,SPOM_POTYPE,SPOM_PONO,SPOM_PROJ_NAME from SUPP_PO_MASTER where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                {
                    string MatserCode = CommonClasses.GetMaxId("Select Max(AM_CODE) from SUPP_PO_AM_MASTER");
                    DataTable dtDetail = CommonClasses.Execute("select * from SUPP_PO_DETAILS where SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        // CommonClasses.Execute1("INSERT INTO SUPP_PO_AMD_DETAILS (SPOD_AMD_CODE,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE,SPOD_AMD_UOM_CODE,SPOD_AMD_ORDER_QTY,SPOD_AMD_RATE,SPOD_AMD_RATE_UOM,SPOD_AMD_CONV_RATIO,SPOD_AMD_DISC_PER,SPOD_AMD_DISC_AMT,SPOD_AMD_EXC_Y_N,SPOD_AMD_T_CODE,SPOD_AMD_TOTAL_AMT,SPOD_AMD_ACTIVE_IND,SPOD_AMD_SPECIFICATION,SPOD_AMD_ITEM_NAME,SPOD_AMD_INW_QTY,SPOD_AMD_EXC_PER,SPOD_AMD_EDU_CESS_PER,SPOD_AMD_H_EDU_CESS ,AMD_AM_CODE,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH)  values('" + dtDetail.Rows[j]["SPOD_CODE"] + "','" + dtDetail.Rows[j]["SPOD_SPOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_I_CODE"] + "','" + dtDetail.Rows[j]["SPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_ORDER_QTY"] + "','" + dtDetail.Rows[j]["SPOD_RATE"] + "','" + dtDetail.Rows[j]["SPOD_RATE_UOM"] + "','" + dtDetail.Rows[j]["SPOD_CONV_RATIO"] + "','" + dtDetail.Rows[j]["SPOD_DISC_PER"] + "','" + dtDetail.Rows[j]["SPOD_DISC_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_Y_N"] + "','" + dtDetail.Rows[j]["SPOD_T_CODE"] + "','" + dtDetail.Rows[j]["SPOD_TOTAL_AMT"] + "','" + dtDetail.Rows[j]["SPOD_ACTIVE_IND"] + "','" + dtDetail.Rows[j]["SPOD_SPECIFICATION"] + "','" + dtDetail.Rows[j]["SPOD_ITEM_NAME"] + "','" + dtDetail.Rows[j]["SPOD_INW_QTY"] + "','" + dtDetail.Rows[j]["SPOD_EXC_PER"] + "','" + dtDetail.Rows[j]["SPOD_EDU_CESS_PER"] + "','" + dtDetail.Rows[j]["SPOD_H_EDU_CESS"] + "','" + MatserCode + "','" + dtDetail.Rows[j]["SPOD_PROCESS_CODE"] + "', '" + dtDetail.Rows[j]["SPOD_COSTWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_FINISHWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_TURNINGWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_DISPACH"] + "' )");
                        CommonClasses.Execute1("INSERT INTO SUPP_PO_AMD_DETAILS (SPOD_AMD_CODE,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE,SPOD_AMD_UOM_CODE,SPOD_AMD_ORDER_QTY,SPOD_AMD_RATE,SPOD_AMD_RATE_UOM,SPOD_AMD_CONV_RATIO,SPOD_AMD_DISC_PER,SPOD_AMD_DISC_AMT,SPOD_AMD_EXC_Y_N,SPOD_AMD_T_CODE,SPOD_AMD_TOTAL_AMT,SPOD_AMD_ACTIVE_IND,SPOD_AMD_SPECIFICATION,SPOD_AMD_ITEM_NAME,SPOD_AMD_INW_QTY,SPOD_AMD_EXC_PER,SPOD_AMD_EDU_CESS_PER,SPOD_AMD_H_EDU_CESS ,AMD_AM_CODE,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_TARRIF_CODE,SPOD_CENTRAL_TAX_AMT,SPOD_STATE_TAX_AMT,SPOD_INTEGRATED_TAX_AMT)  values('" + dtDetail.Rows[j]["SPOD_CODE"] + "','" + dtDetail.Rows[j]["SPOD_SPOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_I_CODE"] + "','" + dtDetail.Rows[j]["SPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_ORDER_QTY"] + "','" + dtDetail.Rows[j]["SPOD_RATE"] + "','" + dtDetail.Rows[j]["SPOD_RATE_UOM"] + "','" + dtDetail.Rows[j]["SPOD_CONV_RATIO"] + "','" + dtDetail.Rows[j]["SPOD_DISC_PER"] + "','" + dtDetail.Rows[j]["SPOD_DISC_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_Y_N"] + "','" + dtDetail.Rows[j]["SPOD_T_CODE"] + "','" + dtDetail.Rows[j]["SPOD_TOTAL_AMT"] + "','" + dtDetail.Rows[j]["SPOD_ACTIVE_IND"] + "','" + dtDetail.Rows[j]["SPOD_SPECIFICATION"] + "','" + dtDetail.Rows[j]["SPOD_ITEM_NAME"] + "','" + dtDetail.Rows[j]["SPOD_INW_QTY"] + "','" + dtDetail.Rows[j]["SPOD_EXC_PER"] + "','" + dtDetail.Rows[j]["SPOD_EDU_CESS_PER"] + "','" + dtDetail.Rows[j]["SPOD_H_EDU_CESS"] + "','" + MatserCode + "','" + dtDetail.Rows[j]["SPOD_PROCESS_CODE"] + "', '" + dtDetail.Rows[j]["SPOD_COSTWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_FINISHWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_TURNINGWEIGHT"] + "','" + dtDetail.Rows[j]["SPOD_DISPACH"] + "' ," + dtDetail.Rows[j]["SPOD_E_TARRIF_CODE"] + ", " + dtDetail.Rows[j]["SPOD_CENTRAL_TAX_AMT"] + "," + dtDetail.Rows[j]["SPOD_STATE_TAX_AMT"] + "," + dtDetail.Rows[j]["SPOD_INTEGRATED_TAX_AMT"] + " )");

                    }

                    #region ModifyOriginalPO
                    if (CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_P_CODE='" + ddlSupplier.SelectedValue + "',SPOM_PO_NO='" + txtPoNo.Text + "',SPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "',SPOM_AM_COUNT='" + AMEND_COUNT + "',SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' ,SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "'  where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        result = CommonClasses.Execute1("update  SUPP_PO_AM_MASTER set SPOM_AM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE SPOM_AM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and SPOM_AM_AM_COUNT='" + AMEND_COUNT + "'");
                        result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                            {
                                //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + ")");
                                //  CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_INW_QTY")).Text + " '," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_DISPACH")).Text + ")");

                                //CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_INW_QTY")).Text + " '," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_DISPACH")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_TARIFF_NO")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + ")");
                                CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS   (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_PROCESS_CODE,SPOD_COSTWEIGHT,SPOD_FINISHWEIGHT,SPOD_TURNINGWEIGHT,SPOD_DISPACH,SPOD_E_TARRIF_CODE,SPOD_STATE_TAX_AMT,SPOD_CENTRAL_TAX_AMT,SPOD_INTEGRATED_TAX_AMT   ) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblProcessCode")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblCostWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblFinishWeight")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTurningWeight")).Text + ",0," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + ")");
                                CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }

                            CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                            CommonClasses.WriteLog("Sub Contractor Purchase Order", "Amend", "Sub Contractor Purchase Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
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
            CommonClasses.SendError("Sub Contractor Purchase Order", "SaveRec", ex.Message);
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
        ((DataTable)ViewState["dt2"]).Clear();
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
            ((DataTable)ViewState["dt2"]).Columns.Add("ProcessCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ProcessName");
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

            ((DataTable)ViewState["dt2"]).Columns.Add("CostWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("FinishWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("TurningWeight");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_INW_QTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_DISPACH");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_E_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_HE_AMT");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_TARIFF_NO");

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
                txtCurrencyRate.Text = dtCurrrate.Rows[0]["CURR_RATE"].ToString();
            }
            else
            {
                txtCurrencyRate.Text = "0.00";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Purchase Order", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }

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
                    // code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
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
            File_Code = Convert.ToInt32(ViewState["mlCode"].ToString());
        }
        else
        {
            dt = CommonClasses.Execute("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
            if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
            {
                DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('SUPP_PO_MASTER')+1");
                if (dt.Rows[0][0].ToString() == "-2147483647")
                {
                    File_Code = -2147483648;
                }
                else
                {
                    File_Code = int.Parse(dt1.Rows[0][0].ToString());
                }
            }
            else
            {
                File_Code = int.Parse(dt.Rows[0][0].ToString());
            }
        }

        string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + File_Code + "");

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
            string directory = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + File_Code + "");

            string fileName = Path.GetFileName(flUp.PostedFile.FileName);

            flUp.SaveAs(Path.Combine(directory, fileName));

            ((Label)(dgDocView.Rows[e.RowIndex].FindControl("lblfilename"))).Text = fileName.ToString();
            ViewState["fileName"] = File_Code + "/" + fileName.ToString();
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

    void SetHSN()
    {
        DataTable dt1 = CommonClasses.Execute("Select  I_SCAT_CODE AS I_E_CODE from ITEM_MASTER where ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlTariff.SelectedValue = dt1.Rows[0]["I_E_CODE"].ToString();
    }

    #region LoadITariff
    private void LoadITariff()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT E_CODE,E_TARIFF_NO FROM EXCISE_TARIFF_MASTER WHERE ES_DELETE=0 AND E_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY E_TARIFF_NO");
            ddlTariff.DataSource = dt;
            ddlTariff.DataTextField = "E_TARIFF_NO";
            ddlTariff.DataValueField = "E_CODE";
            ddlTariff.DataBind();
            ddlTariff.Items.Insert(0, new ListItem("HSN No", "0"));
            ddlTariff.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("LoadIUnit", "LoadIUnit", Ex.Message);
        }

    }
    #endregion
}
