
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;

public partial class Transactions_ADD_ServicePurchaseOrder : System.Web.UI.Page
{
    #region Variable
    DirectoryInfo ObjSearchDir;
    string fileName1 = "";
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
                    //LoadPoType();
                    //LoadSupplier();
                    //LoadICode();
                    //LoadIName();
                    //LoadTax();

                    //LoadRateUOM();
                    //chkActiveInd.Checked = true;
                    //BlankGridView();
                    //txtConversionRetio.Enabled = false;
                    ViewState["fileName"] = fileName1;
                    //ViewState["dt2"] = dt2;
                    //((DataTable)ViewState["dt2"]).Rows.Clear();
                    ViewState["mlCode"] = mlCode;
                    ViewState["Index"] = Index;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    LoadType();
                    LoadProCode();
                    try
                    {
                        //BindTable.Clear();
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
                            LoadSCode();
                            LoadSName();
                            LoadTax();
                            LoadUnit();
                            LoadRateUOM();
                            LoadCurr();
                            //CreateDataTable();
                            chkActiveInd.Checked = true;
                            BlankGridView();
                            txtConversionRetio.Enabled = false;
                            //CarryForward();
                            txtPoDate.Attributes.Add("readonly", "readonly");
                            txtPoDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtServiceRefDate.Attributes.Add("readonly", "readonly");
                            txtServiceRefDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");

                            txtValid.Attributes.Add("readonly", "readonly");
                            txtValid.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                            //dt2.Rows.Clear();
                            //dt2.Columns.Clear();
                        }
                        ddlPOType.Focus();
                        imgUpload.Attributes["onchange"] = "UploadFile(this)";
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Service Order", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
        }

    }
    #endregion Page_Load

    protected void Upload(object sender, EventArgs e)
    {
        string Code = ddlServiceName.SelectedValue;
        if (Code == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please select Item Name first";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlServiceCode.Focus();
            return;
        }
        string sDirPath = Server.MapPath(@"~/UpLoadPath/SupPO/" + Code + "");

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/SupPO/" + Code + "/" + Path.GetFileName(imgUpload.FileName)));
        fileName1 = Path.GetFileName(imgUpload.FileName);
        ViewState["fileName"] = fileName1;
        CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DOC_NAME='" + fileName1 + "',I_DOC_PATH='" + "~/UpLoadPath/SupPO/" + Code + "" + '/' + "" + fileName1 + "' where I_CODE='" + Code + "'");
    }

    #region CarryForward
    public void CarryForward()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TOP 1 * from SERVICE_PO_MASTER where ES_DELETE=0 and SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' order by SRPOM_CODE DESC");
            if (dt.Rows.Count > 0)
            {
                txtPaymentTerm.Text = dt.Rows[0]["SRPOM_PAY_TERM1"].ToString();
                txtDeliverySchedule.Text = dt.Rows[0]["SRPOM_DEL_SHCEDULE"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SRPOM_DELIVERED_TO"].ToString();
                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SRPOM_GUARNTY"].ToString();
                txtPacking.Text = dt.Rows[0]["SRPOM_PACKING"].ToString();
                txtNote.Text = dt.Rows[0]["SRPOM_NOTES"].ToString();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Order", "CarryForward", ex.Message.ToString());
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

        if (dgServicePurchaseOrder.Rows.Count != 0)
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
                //lblmsg.Text = "Please Select Service";
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
            if (dgServicePurchaseOrder.Enabled && dgServicePurchaseOrder.Rows.Count > 0)
            {

                SaveRec();

            }
            else
            {
                ShowMessage("#Avisos", "Insert Service Detail", CommonClasses.MSG_Warning);
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
            //    CommonClasses.RemoveModifyLock("SERVICE_PO_MASTER", "MODIFY", "SRPOM_CODE", mlCode);
            //}

            //dt2.Rows.Clear();
            //Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);

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
            CommonClasses.SendError("Service Purchase Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
    private void CreateDataTable()
    {
        #region datatable structure
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceCode1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceName");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");

            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
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
                //ShowMessage("#Avisos", "Select Service", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Service";
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

            if (ddlServiceCode.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Service Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlServiceCode.Focus();
                return;
            }

            if (ddlServiceName.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Service Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlServiceName.Focus();
                return;
            }
            if (txtOrderQty.Text == "" || txtOrderQty.Text == "0.000")
            {
                //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Order Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtMissItemName.Text))
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
                lblmsg.Text = "Please Enter  Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRate.Focus();
                return;
            }
            //if (ddlRateUOM.SelectedIndex == 0)
            //{
            //    //ShowMessage("#Avisos", "Select Rate UOM", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Select Rate Unit";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlRateUOM.Focus();
            //    return;
            //}
            if (chkActiveInd.Checked == true)
            {
                Active = 1;
            }
            else
            {
                Active = 0;
            }


            if (ddlGSTTax.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select  Sales Tax Category", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select GST Tax";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlGSTTax.Focus();
                return;
            }
            //if (txtConversionRetio.Text == "" || txtConversionRetio.Text == "0.0")
            //{

            //    if (ddlStockUOM.SelectedItem.Text != ddlRateUOM.SelectedItem.Text)
            //    {
            //        //ShowMessage("#Avisos", "Enter Conversion Ratio", CommonClasses.MSG_Warning);
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "Please Enter Conversion Ratio";
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //        return;

            //    }
            //    else
            //    {

            //    }
            //}


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
                lblmsg.Text = "Percentage not greter than 100";
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


            //dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE  from SALES_TAX_MASTER where  ST_CODE=" + ddlGSTTax.SelectedValue + " and ES_DELETE=0 and  ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "");
            //Double Tax = Convert.ToDouble(dt.Rows[0]["ST_SALES_TAX"].ToString());

            Double Subamount = (Convert.ToDouble(txtTotalAmount.Text.ToString()) - DescAmount);

            double E_BASIC = 0;
            double E_EDU_CESS = 0;
            double E_H_EDU = 0;
            double ExisDuty = 0;
            double EduCess = 0;
            double SHECess = 0;
            //if (!chkExcInclusive.Checked)
            //{
            dt = CommonClasses.Execute("select E_BASIC, E_EDU_CESS,E_H_EDU,E_CODE from SERVICE_TYPE_MASTER,EXCISE_TARIFF_MASTER where S_E_CODE=E_CODE and SERVICE_TYPE_MASTER.ES_DELETE=0 and EXCISE_TARIFF_MASTER.ES_DELETE=0 and S_CODE='" + ddlServiceCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString());
                E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString());
                E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString());
                ddlGSTTax.SelectedValue = dt.Rows[0]["E_CODE"].ToString();
            }

            DataTable dtGSTcal = CommonClasses.Execute("select P_CODE,P_NAME,P_SM_CODE,CM_STATE from PARTY_MASTER,COMPANY_MASTER where P_TYPE=2 and CM_ID=P_CM_COMP_ID and PARTY_MASTER.ES_DELETE=0 and P_CODE='" + ddlSupplier.SelectedValue + "'");
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

            //}
            //E_BASIC = Convert.ToDouble(txtExcisePer.Text);
            //double ExisDuty = Subamount * Convert.ToDouble(txtExcduty.Text.ToString()) / Convert.ToDouble( 100);
            //double EduCess = ExisDuty * Convert.ToDouble(txtEduCess.Text.ToString()) / Convert.ToDouble(100);
            //double SHECess = ExisDuty * Convert.ToDouble(txtSHECess.Text.ToString()) / Convert.ToDouble(100);
            //Double SalesTax = (((Convert.ToDouble(txtTotalAmount.Text.ToString())) + Subamount + ExisDuty + EduCess + SHECess) * Tax) / (Convert.ToDouble(100));


            // NetTotal = Subamount + ExisDuty + EduCess + SHECess + SalesTax;

            if (dgServicePurchaseOrder.Rows.Count > 0)
            {
                for (int i = 0; i < dgServicePurchaseOrder.Rows.Count; i++)
                {
                    string ServiceCode = ((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblServiceCode1"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ServiceCode == ddlServiceCode.SelectedValue.ToString())
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
                        if (ServiceCode == ddlServiceCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
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
            dr["ServiceCode"] = ddlServiceCode.SelectedItem;
            dr["ServiceCode1"] = ddlServiceCode.SelectedValue;
            dr["ServiceName"] = ddlServiceName.SelectedItem;

            dr["OrderQty"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 2));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));

            //dr["Round"] = ChkRound.Checked;
            dr["TotalAmount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtOrderQty.Text)), 2));
            // dr["NetTotal"] = string.Format("{0:0.00}", Math.Round(NetTotal), 2);

            dr["DiscPerc"] = txtDescPerc.Text;
            dr["DiscAmount"] = string.Format("{0:0.00}", Math.Round(DescAmount), 2);
            //if (txtConversionRetio.Text == "")
            //{
            //    dr["ConversionRatio"] = 0.0;
            //}
            //else
            //{
            //    dr["ConversionRatio"] = Convert.ToDouble(txtConversionRetio.Text);
            //}

            //if (chkExcInclusive.Checked == true)
            //{
            //    dr["ExcInclusive"] = "1";
            //}
            //else
            //{
            //    dr["ExcInclusive"] = "0";
            //}
            //dr["ExcInclusive"] = chkExcInclusive.Checked;
            dr["SalesTax1"] = ddlGSTTax.SelectedValue;
            dr["SalesTax"] = ddlGSTTax.SelectedItem;


            //dr["round1"] = chkRound1.Checked;

            dr["Specification"] = txtSpecification.Text.Trim().Replace("'", "\''");
            //dr["SRPOD_ITEM_NAME"] = txtMissItemName.Text.Trim();
            //if (chkActiveInd.Checked == true)
            //{
            //    dr["ActiveInd"] = "1";
            //}
            //else
            //{
            //    dr["ActiveInd"] = "0";
            //}
            //dr["ActiveInd"] = Active;
            dr["E_BASIC"] = E_BASIC;
            dr["E_EDU_CESS"] = E_EDU_CESS;
            dr["E_H_EDU"] = E_H_EDU;
            //if (txtMissItemName.Text.Trim() == "")
            //{
            //    txtMissItemName.Text = "0";
            //}
            //dr["SRPOD_INW_QTY"] = txtMissItemName.Text;
            dr["ExisDuty"] = ExisDuty;
            dr["EduCess"] = EduCess;
            dr["SHECess"] = SHECess;
            fileName1 = ViewState["fileName"].ToString();
            dr["DocName"] = fileName1;

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
            dgServicePurchaseOrder.Visible = true;
            dgServicePurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
            //dgServicePurchaseOrder.DataSource = (DataTable)ViewState["NewTable"]; 
            dgServicePurchaseOrder.DataBind();
            dgServicePurchaseOrder.Enabled = true;


            #endregion

            ddlServiceCode.Enabled = true;
            ddlServiceName.Enabled = true;

            GetTotal();
            clearDetail();
            ddlRateUOM.SelectedIndex = 0;


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO Order", "btnInsert_Click", Ex.Message);
        }
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
            CommonClasses.SendError("Service PO", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Service PO", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("SERVICE_PO_MASTER", "MODIFY", "SRPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region LoadSCode
    private void LoadSCode()
    {
        try
        {
            dt = CommonClasses.Execute("select S_CODE,S_CODENO from SERVICE_TYPE_MASTER where ES_DELETE=0 and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " and S_ACTIVE_IND=1 ORDER BY S_CODENO");
            ddlServiceCode.DataSource = dt;
            ddlServiceCode.DataTextField = "S_CODENO";
            ddlServiceCode.DataValueField = "S_CODE";
            ddlServiceCode.DataBind();
            ddlServiceCode.Items.Insert(0, new ListItem("Select Service Code", "0"));
            ddlServiceCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service PO", "LoadSCode", Ex.Message);
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
            CommonClasses.SendError("Service Purchase Order ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadSName
    private void LoadSName()
    {
        try
        {
            dt = CommonClasses.Execute("select S_CODE,S_NAME from SERVICE_TYPE_MASTER where ES_DELETE=0 and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " and S_ACTIVE_IND=1 ORDER BY S_NAME");
            ddlServiceName.DataSource = dt;
            ddlServiceName.DataTextField = "S_NAME";
            ddlServiceName.DataValueField = "S_CODE";
            ddlServiceName.DataBind();
            ddlServiceName.Items.Insert(0, new ListItem("Select Service Name", "0"));
            ddlServiceName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order ", "LoadSName", Ex.Message);
        }

    }
    #endregion

    #region LoadTax
    private void LoadTax()
    {
        try
        {
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where ES_DELETE=0 and E_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and E_TALLY_GST_EXCISE=1  ORDER BY E_TARIFF_NO");
            ddlGSTTax.DataSource = dt;
            ddlGSTTax.DataTextField = "E_TARIFF_NO";
            ddlGSTTax.DataValueField = "E_CODE";
            ddlGSTTax.DataBind();
            ddlGSTTax.Items.Insert(0, new ListItem("Select SAC Code", "0"));
            ddlGSTTax.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order ", "LoadTax", Ex.Message);
        }

    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2'   AND (P_STM_CODE=-2147483648 OR P_STM_CODE=-2147483646)  AND    P_ACTIVE_IND=1 order by P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order", "LoadSupplier", Ex.Message);
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
            CommonClasses.SendError("Service Purchase Order -Transaction", "ddlServiceCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region ddlServiceCode_SelectedIndexChanged
    protected void ddlServiceCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServiceCode.SelectedIndex != 0)
            {
                ddlServiceName.SelectedValue = ddlServiceCode.SelectedValue;

                //    LoadUOM();
                //    ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                //    //   DataTable dt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='"+ddlItemCode.SelectedValue+"'");
                //    //   txtRate.Text =string.Format("{0:0.00}", dt.Rows[0]["I_INV_RATE"]);
                //    DataTable dt = CommonClasses.Execute("select isnull(SRPOD_RATE,0) as SRPOD_RATE from ITEM_MASTER ,SERVICE_PO_DETAILS,SERVICE_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SRPOD_I_CODE AND SRPOM_CODE=SRPOD_SRPOM_CODE AND SRPOM_CODE= (SELECT MAX(SRPOM_CODE) FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SRPOM_CODE AND SRPOD_I_CODE='" + ddlServiceCode.SelectedValue + "' AND SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                //    if (dt.Rows.Count > 0)
                //    {
                //        txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOD_RATE"]);
                //    }
                //    else
                //    {
                //        DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlServiceCode.SelectedValue + "'");
                //        txtRate.Text = string.Format("{0:0.00}", dtt.Rows[0]["I_INV_RATE"]);
                //    }

                DataTable dt = CommonClasses.Execute("select E_BASIC, E_EDU_CESS,E_H_EDU,E_CODE from SERVICE_TYPE_MASTER,EXCISE_TARIFF_MASTER where S_E_CODE=E_CODE and SERVICE_TYPE_MASTER.ES_DELETE=0 and EXCISE_TARIFF_MASTER.ES_DELETE=0 and S_CODE='" + ddlServiceCode.SelectedValue + "'");
                if (dt.Rows.Count > 0)
                {
                    ddlGSTTax.SelectedValue = dt.Rows[0]["E_CODE"].ToString();
                }
                else
                {
                    ddlGSTTax.SelectedIndex = 0;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order -Transaction", "ddlServiceCode_SelectedIndexChanged", Ex.Message);

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
        BlankGridView();
        clearDetail();
        txtTotalAmount.Text = "";
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
            CommonClasses.SendError("Service PO Transaction", "LoadUnit", Ex.Message);
        }

    }
    #endregion

    #region LoadUOM
    private void LoadUOM()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlServiceCode.SelectedValue + "'");
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
            CommonClasses.SendError("Export PO", "LoadSName", Ex.Message);
        }
    }
    #endregion

    #region ddlServiceName_SelectedIndexChanged
    protected void ddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServiceName.SelectedIndex != 0)
            {
                ddlServiceCode.SelectedValue = ddlServiceName.SelectedValue;
                //    LoadUOM();
                //    ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                //    //DataTable dt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemName.SelectedValue + "'");
                //    //txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["I_INV_RATE"]);
                //    DataTable dt = CommonClasses.Execute("select isnull(SRPOD_RATE,0) as SRPOD_RATE from ITEM_MASTER ,SERVICE_PO_DETAILS,SERVICE_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SRPOD_I_CODE AND SRPOM_CODE=SRPOD_SRPOM_CODE AND SRPOM_CODE= (SELECT MAX(SRPOM_CODE) FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SRPOM_CODE AND SRPOD_I_CODE='" + ddlServiceCode.SelectedValue + "' AND SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                //    if (dt.Rows.Count > 0)
                //    {
                //        txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOD_RATE"]);
                //    }
                //    else
                //    {
                //        DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlServiceCode.SelectedValue + "'");
                //        txtRate.Text = string.Format("{0:0.00}", dtt.Rows[0]["I_INV_RATE"]);
                //    }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order -Transaction", "ddlServiceName_SelectedIndexChanged", Ex.Message);
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
            LoadSCode();
            LoadSName();
            LoadTax();
            LoadCurr();
            LoadProCode();
            dtPODetail.Clear();
            txtPoDate.Attributes.Add("readonly", "readonly");
            txtServiceRefDate.Attributes.Add("readonly", "readonly");
            txtValid.Attributes.Add("readonly", "readonly");

            dt = CommonClasses.Execute("Select SRPOM_CODE,SRPOM_TYPE,SRPOM_DATE,SRPOM_PO_NO,SRPOM_P_CODE,SRPOM_AMOUNT,SRPOM_SUP_REF,SRPOM_SUP_REF_DATE,SRPOM_VALID_DATE,SRPOM_DEL_SHCEDULE,SRPOM_TRANSPOTER,SRPOM_PAY_TERM1,SOM_FREIGHT_TERM,SRPOM_GUARNTY,SRPOM_NOTES,SRPOM_DELIVERED_TO,SRPOM_CURR_CODE,SRPOM_CIF_NO,SRPOM_PACKING,SRPOM_PROJECT,SRPOM_PONO,SRPOM_PROJ_NAME,SRPOM_CGST_AMT , SRPOM_SGST_AMT ,SRPOM_IGST_AMT ,SRPOM_NET_AMT    from SERVICE_PO_MASTER where ES_DELETE=0 and SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SRPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SRPOM_CODE"]);
                ddlPOType.SelectedValue = dt.Rows[0]["SRPOM_TYPE"].ToString();
                if (ddlPOType.SelectedValue == "-2147483647")
                {
                    pnlCurrancy.Visible = true;
                    ddlCurrency.SelectedValue = dt.Rows[0]["SRPOM_CURR_CODE"].ToString();
                    ddlCurrency_SelectedIndexChanged(null, null);
                }
                else
                {
                    pnlCurrancy.Visible = false;
                }
                ddlSupplier.SelectedValue = dt.Rows[0]["SRPOM_P_CODE"].ToString();
                txtPoNo.Text = dt.Rows[0]["SRPOM_PO_NO"].ToString();
                txtPoNo.Text = dt.Rows[0]["SRPOM_PONO"].ToString();

                txtPoDate.Text = Convert.ToDateTime(dt.Rows[0]["SRPOM_DATE"]).ToString("dd MMM yyyy");
                txtServiceRefDate.Text = Convert.ToDateTime(dt.Rows[0]["SRPOM_SUP_REF_DATE"]).ToString("dd MMM yyyy");

                txtValid.Text = Convert.ToDateTime(dt.Rows[0]["SRPOM_VALID_DATE"]).ToString("dd MMM yyyy");
                txtServiceRef.Text = dt.Rows[0]["SRPOM_SUP_REF"].ToString();
                ddlProjectCode.SelectedValue = dt.Rows[0]["SRPOM_PROJECT"].ToString();
                txtProjName.Text = dt.Rows[0]["SRPOM_PROJ_NAME"].ToString();

                txtFinalTotalAmount.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOM_AMOUNT"]);
                txtCGSTTax.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOM_CGST_AMT"]);
                txtSGSTTax.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOM_SGST_AMT"]);
                txtIGSTTax.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOM_IGST_AMT"]);
                txtNetTotal.Text = string.Format("{0:0.00}", dt.Rows[0]["SRPOM_NET_AMT"]);

                txtTranspoter.Text = dt.Rows[0]["SRPOM_TRANSPOTER"].ToString();

                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SRPOM_GUARNTY"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SRPOM_DELIVERED_TO"].ToString();
                txtNote.Text = dt.Rows[0]["SRPOM_NOTES"].ToString();
                txtPaymentTerm.Text = dt.Rows[0]["SRPOM_PAY_TERM1"].ToString();

                txtDeliverySchedule.Text = dt.Rows[0]["SRPOM_DEL_SHCEDULE"].ToString();
                txtCIF.Text = dt.Rows[0]["SRPOM_CIF_NO"].ToString();
                txtPacking.Text = dt.Rows[0]["SRPOM_PACKING"].ToString();


                // dtPODetail = CommonClasses.Execute("select SRPOD_I_CODE,SRPOD_UOM_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,cast(SRPOD_DISC_PER as float) as SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_EXC_PER,SRPOD_T_CODE,SRPOD_TOTAL_AMT,SRPOD_I_SIZE,I_NAME,UOM_NAME,T_NAME,isnull(SRPOD_EXC_Y_N,0) as SRPOD_EXC_Y_N,isnull(SRPOD_TAX_Y_N,0) as SRPOD_TAX_Y_N FROM SERVICE_PO_MASTER,SUPP_PO_DETAIL,ITEM_MASTER,UNIT_MASTER,TAX_MASTER WHERE SRPOM_CODE=SRPOD_SRPOM_CODE and SRPOD_I_CODE=ITEM_MASTER.I_CODE and SRPOD_UOM_CODE=UNIT_MASTER.UOM_CODE and SRPOD_T_CODE=TAX_MASTER.T_CODE and SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SRPOD_SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                //dtPODetail = CommonClasses.Execute("select SRPOD_I_CODE as ItemCode,I_CODENO as ShortName,I_DOC_NAME as Docname,I_DOC_PATH,SRPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SRPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, cast(SRPOD_ORDER_QTY as numeric(10,3)) as OrderQty,cast(SRPOD_RATE as numeric(20,2)) as Rate,SRPOD_RATE_UOM as RateUOM1,I_UOM_NAME as RateUOM,SRPOD_CONV_RATIO as ConversionRatio,cast(SRPOD_TOTAL_AMT as numeric(20,2)) as TotalAmount,cast(SRPOD_DISC_PER as float) as DiscPerc,cast(SRPOD_DISC_AMT as numeric(20,2)) as DiscAmount,isnull(SRPOD_EXC_Y_N,0) as ExcInclusive,SRPOD_T_CODE as SalesTax1,ST_TAX_NAME as SalesTax ,SRPOD_ACTIVE_IND as ActiveInd ,SRPOD_SPECIFICATION as Specification,SRPOD_ITEM_NAME,SRPOD_EXC_PER as E_BASIC, SRPOD_EDU_CESS_PER as E_EDU_CESS,SRPOD_H_EDU_CESS as E_H_EDU,ISNULL(SRPOD_INW_QTY,0) AS SRPOD_INW_QTY FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,SALES_TAX_MASTER WHERE SRPOM_CODE=SRPOD_SRPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SERVICE_PO_DETAILS.SRPOD_UOM_CODE and SRPOD_I_CODE=ITEM_MASTER.I_CODE and SRPOD_T_CODE=SALES_TAX_MASTER.ST_CODE and SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SRPOD_SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                dtPODetail = CommonClasses.Execute("SELECT SERVICE_PO_DETAILS.SRPOD_I_CODE AS ServiceCode1, SERVICE_TYPE_MASTER.S_CODENO AS ServiceCode, CAST(SERVICE_PO_DETAILS.SRPOD_ORDER_QTY AS numeric(10, 3)) AS OrderQty, CAST(SERVICE_PO_DETAILS.SRPOD_RATE AS numeric(20, 2)) AS Rate, CAST(SERVICE_PO_DETAILS.SRPOD_TOTAL_AMT AS numeric(20, 2)) AS TotalAmount, CAST(SERVICE_PO_DETAILS.SRPOD_DISC_PER AS float) AS DiscPerc, CAST(SERVICE_PO_DETAILS.SRPOD_DISC_AMT AS numeric(20, 2)) AS DiscAmount, SRPOD_T_CODE AS SalesTax1,SERVICE_PO_DETAILS.SRPOD_SPECIFICATION AS Specification, SERVICE_PO_DETAILS.SRPOD_ITEM_NAME AS ServiceName, SERVICE_PO_DETAILS.SRPOD_EXC_PER AS E_BASIC, SERVICE_PO_DETAILS.SRPOD_EDU_CESS_PER AS E_EDU_CESS, SERVICE_PO_DETAILS.SRPOD_H_EDU_CESS AS E_H_EDU, EXCISE_TARIFF_MASTER.E_TARIFF_NO as SalesTax,ISNULL(SRPOD_CENTRAL_TAX_AMT,0) as ExisDuty,isnull(SRPOD_STATE_TAX_AMT,0) as EduCess,isnull(SRPOD_INTEGRATED_TAX_AMT,0) as SHECess, SERVICE_PO_ATTACHMENT.SPOA_DOC_NAME as DocName FROM SERVICE_PO_MASTER INNER JOIN SERVICE_PO_DETAILS ON SERVICE_PO_MASTER.SRPOM_CODE = SERVICE_PO_DETAILS.SRPOD_SPOM_CODE INNER JOIN SERVICE_TYPE_MASTER ON SERVICE_PO_DETAILS.SRPOD_I_CODE = SERVICE_TYPE_MASTER.S_CODE INNER JOIN EXCISE_TARIFF_MASTER ON SERVICE_TYPE_MASTER.S_E_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN SERVICE_PO_ATTACHMENT ON SERVICE_PO_MASTER.SRPOM_CODE = SERVICE_PO_ATTACHMENT.SPOA_SRPOM_CODE WHERE (SERVICE_PO_MASTER.SRPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "') AND (SERVICE_PO_DETAILS.SRPOD_SPOM_CODE = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "')");
                if (dtPODetail.Rows.Count > 0)
                {
                    dgServicePurchaseOrder.DataSource = dtPODetail;
                    dgServicePurchaseOrder.DataBind();
                    ViewState["dt2"] = dtPODetail;

                    GetTotal();


                }
                DataTable dtDoc = CommonClasses.Execute("select SPOA_SRPOM_CODE,SPOA_DOC_NAME,SPOA_DOC_PATH from SERVICE_PO_ATTACHMENT where SPOA_SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
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
                txtServiceRef.Enabled = false;
                txtServiceRefDate.Enabled = false;
                txtTotalAmount.Enabled = false;

                ddlServiceCode.Enabled = false;
                ddlServiceName.Enabled = false;
                ddlPOType.Enabled = false;

                ddlRateUOM.Enabled = false;
                ddlGSTTax.Enabled = false;
                ddlSupplier.Enabled = false;

                chkActiveInd.Enabled = false;

                chkExcInclusive.Enabled = false;

                txtDeliverySchedule.Enabled = false;
                txtTranspoter.Enabled = false;
                txtPaymentTerm.Enabled = false;
                txtDeliveryTo.Enabled = false;
                txtFreightTermsg.Enabled = false;
                txtNote.Enabled = false;
                dgServicePurchaseOrder.Enabled = false;
                ddlCurrency.Enabled = false;
                txtCurrencyRate.Enabled = false;
                txtMissItemName.Enabled = false;

            }
            else if (str == "MOD" || str == "AMEND")
            {
                ddlSupplier.Enabled = false;

                txtConversionRetio.Enabled = false;
                CommonClasses.SetModifyLock("SERVICE_PO_MASTER", "MODIFY", "SRPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Service Purchase Order Transaction", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Service Purchase Order ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlServiceName.SelectedIndex = 0;
            ddlServiceCode.SelectedIndex = 0;
            //ddlStockUOM.SelectedIndex = 0;
            txtOrderQty.Text = "";
            txtRate.Text = "";
            //ddlRateUOM.SelectedIndex = 0;

            txtTotalAmount.Text = "";
            txtDescPerc.Text = "";
            txtDescAmount.Text = "";
            //txtConversionRetio.Text = "";
            //chkExcInclusive.Checked = false;

            ddlGSTTax.SelectedIndex = 0;
            ViewState["ItemUpdateIndex"] = -1;
            txtSpecification.Text = "";
            // chkActiveInd.Checked = false;
            //txtMissItemName.Text = "0.00";
            //txtExcisePer.Text = "0.00";
            //txtAmount.Text = "0.00";
            //txtCustItemCode.Text = "";
            //txtCustItemName.Text = "";
            //ddlTaxCategory.SelectedIndex = 0;
            //ddlCurrancy.SelectedIndex = 0;
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Purchase Order", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double CGSTTax = 0, SGSTTax = 0, IGSTTax = 0, NetTotal = 0;
        if (dgServicePurchaseOrder.Rows.Count > 0)
        {
            for (int i = 0; i < dgServicePurchaseOrder.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount"))).Text;
                string QED_Disc = ((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);
                double Discount = Convert.ToDouble(QED_Disc);
                decTotal = decTotal + Amount - Discount;
                CGSTTax += Convert.ToDouble(((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblExisDuty"))).Text);
                SGSTTax += Convert.ToDouble(((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblEduCess"))).Text);
                IGSTTax += Convert.ToDouble(((Label)(dgServicePurchaseOrder.Rows[i].FindControl("lblSHECess"))).Text);
                NetTotal = decTotal + CGSTTax + SGSTTax + IGSTTax;
            }
        }
        txtFinalTotalAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2), 2);
        txtCGSTTax.Text = string.Format("{0:0.00}", Math.Round(CGSTTax, 2), 2);
        txtSGSTTax.Text = string.Format("{0:0.00}", Math.Round(SGSTTax, 2), 2);
        txtIGSTTax.Text = string.Format("{0:0.00}", Math.Round(IGSTTax), 2);
        txtNetTotal.Text = string.Format("{0:0.00}", Math.Round(NetTotal, 2), 2);
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
        txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2));

        //if (ddlStockUOM.SelectedItem.ToString() != ddlRateUOM.SelectedItem.ToString())
        //{
        //    totalStr = DecimalMasking(txtConversionRetio.Text);
        //    txtConversionRetio.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        //    double TempTotal = Convert.ToDouble(txtOrderQty.Text) * (Convert.ToDouble(txtConversionRetio.Text));
        //    txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round(TempTotal) * Convert.ToDouble(txtRate.Text.ToString()), 2);

        //}
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
           // txtDescPerc.Focus();
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

    #region dgServicePurchaseOrder_RowCommand
    protected void dgServicePurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgServicePurchaseOrder.Rows[Convert.ToInt32(ViewState["Index"].ToString())];


            if (e.CommandName == "Delete")
            {
                // int rowindex = row.RowIndex;
                //dgServicePurchaseOrder.DeleteRow(Index);

                string ICode = ((Label)(row.FindControl("lblServiceCode"))).Text;

                //DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ICode + "'");

                //if (dtCheckExistItem.Rows.Count > 0)
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "You cannot delete the Service because it is used in Inward";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    txtOrderQty.Focus();
                //    return;
                //}




                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgServicePurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
                dgServicePurchaseOrder.DataBind();
                GetTotal();
                if (((DataTable)ViewState["dt2"]).Rows.Count == 0)
                {
                    BlankGridView();
                }

            }
            if (e.CommandName == "Modify")
            {
                //LoadService();
                LoadSCode();
                LoadSName();
                LoadTax();
                // LoadPOUnit();
                LoadRateUOM();

                //********
                str = "Modify";
                //LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                //lnkDelete.Enabled = false;

                //((TemplateField)dgServicePurchaseOrder.Columns[0]).EditItemTemplate = null;

                foreach (GridViewRow gvr in dgServicePurchaseOrder.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }

                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlServiceCode.SelectedValue = ((Label)(row.FindControl("lblServiceCode1"))).Text;
                ddlServiceCode_SelectedIndexChanged(null, null);
                ddlServiceName.SelectedValue = ((Label)(row.FindControl("lblServiceCode1"))).Text;
                ddlServiceName_SelectedIndexChanged(null, null);
                //ddlStockUOM.SelectedValue = ((Label)(row.FindControl("lblUOM1"))).Text;
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblRate"))).Text;
                //ddlRateUOM.SelectedValue = ((Label)(row.FindControl("lblRateUOM1"))).Text;

                //ChkRound.Checked = ((Label)(row.FindControl("lblRound"))).Text;
                txtTotalAmount.Text = ((Label)(row.FindControl("lblTotalAmount"))).Text;
                txtDescPerc.Text = ((Label)(row.FindControl("lblDiscPerc"))).Text;
                txtDescAmount.Text = ((Label)(row.FindControl("lblDiscAmount"))).Text;
                //txtConversionRetio.Text = ((Label)(row.FindControl("lblConversionRatio"))).Text;
                ddlGSTTax.SelectedValue = ((Label)(row.FindControl("lblSalesTax1"))).Text;
                txtSpecification.Text = ((Label)(row.FindControl("lblSpecification"))).Text;
                //txtMissItemName.Text = ((Label)(row.FindControl("lblSRPOD_INW_QTY"))).Text;
                //if ((((Label)(row.FindControl("lblExcInclusive"))).Text).ToString() == "True")
                //{
                //    chkExcInclusive.Checked = true;
                //}
                //else
                //{
                //    chkExcInclusive.Checked = false;
                //}
                //txtExcisePer.Text = ((Label)(row.FindControl("lblE_BASIC"))).Text;

                //if ((((Label)(row.FindControl("lblActiveInd"))).Text) == "1")
                //{
                //    chkActiveInd.Checked = true;
                //}
                //else
                //{
                //    chkActiveInd.Checked = false;
                //}
                //$$$
                //DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ddlServiceCode.SelectedValue + "'");
                //if (dtCheckExistItem.Rows.Count > 0)
                //{
                //    ddlServiceCode.Enabled = false;
                //    ddlServiceName.Enabled = false;
                //}
                //else
                //{
                //    ddlServiceCode.Enabled = true;
                //    ddlServiceName.Enabled = true;
                //}
                GetTotal();
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
            CommonClasses.SendError("Service Purchase Order Transaction", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgServicePurchaseOrder_RowDeleting
    protected void dgServicePurchaseOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            ddlProjectCode.Items.Insert(0, new ListItem("Project Code", "0"));
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
        try
        {
            if (txtServiceRefDate.Text == "")
            {
                txtServiceRefDate.Text = System.DateTime.Now.ToString();
            }
            if (txtPoDate.Text == "")
            {

            }
            if (Request.QueryString[0].Equals("INSERT"))
            {

                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                string strPONo = "";
                dt = CommonClasses.Execute("Select isnull(max(SRPOM_PO_NO),0) as SRPOM_PO_NO FROM SERVICE_PO_MASTER WHERE SRPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and SRPOM_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0 AND SRPOM_POTYPE=0");
                if (dt.Rows.Count > 0)
                {
                    //PCPCL0000
                    if (true)
                    {

                    }
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["SRPOM_PO_NO"]);

                    Po_Doc_no = Po_Doc_no + 1;
                    strPONo = "SUNS" + CommonClasses.GenBillNo(Po_Doc_no);
                }
                SqlTransaction trans = null;

                if (CommonClasses.Execute1("INSERT INTO SERVICE_PO_MASTER (SRPOM_CM_CODE,SRPOM_AMOUNT,SRPOM_CM_COMP_ID,SRPOM_TYPE,SRPOM_PO_NO,SRPOM_DATE,SRPOM_P_CODE,SRPOM_SUP_REF,SRPOM_SUP_REF_DATE,SRPOM_DEL_SHCEDULE,SRPOM_TRANSPOTER,SRPOM_PAY_TERM1,SOM_FREIGHT_TERM,SRPOM_GUARNTY,SRPOM_NOTES,SRPOM_DELIVERED_TO,SRPOM_CURR_CODE,SRPOM_CIF_NO,SRPOM_PACKING,SRPOM_PROJECT,SRPOM_VALID_DATE,SRPOM_USER_CODE,SRPOM_POTYPE,SRPOM_PROJ_NAME,SRPOM_PONO,SRPOM_CGST_AMT, SRPOM_SGST_AMT,SRPOM_IGST_AMT,SRPOM_NET_AMT)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtServiceRef.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtServiceRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text.Trim().Replace("'", "\''") + "','" + txtTranspoter.Text.Trim().Replace("'", "\''") + "','" + txtPaymentTerm.Text.Trim().Replace("'", "\''") + "','" + txtFreightTermsg.Text.Trim().Replace("'", "\''") + "','" + txtGuranteeWaranty.Text.Trim().Replace("'", "\''") + "','" + txtNote.Text.Trim().Replace("'", "\''") + "','" + txtDeliveryTo.Text.Trim().Replace("'", "\''") + "','" + ddlCurrency.SelectedValue + "','" + txtCIF.Text.Trim().Replace("'", "\''") + "','" + txtPacking.Text.Trim().Replace("'", "\''") + "','" + ddlProjectCode.SelectedValue + "','" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["UserCode"].ToString()) + "',0,'" + txtProjName.Text.ToUpper().Trim().Replace("'", "\''") + "','" + strPONo + "'," + txtSGSTTax.Text + "," + txtSGSTTax.Text + "," + txtIGSTTax.Text + "," + txtNetTotal.Text + ")"))
                //if (CommonClasses.Execute1("INSERT INTO SERVICE_PO_MASTER (SRPOM_CM_COMP_ID,SRPOM_TYPE,SRPOM_PO_NO,SRPOM_DATE,SRPOM_P_CODE,SRPOM_SUP_REF,SRPOM_SUP_REF_DATE,SRPOM_DEL_SHCEDULE,SRPOM_TRANSPOTER,SRPOM_PAY_TERM1)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "', 2 ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtServiceRef.Text + "','" + txtServiceRefDate.Text + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SRPOM_CODE) from SERVICE_PO_MASTER");
                    for (int i = 0; i < dgServicePurchaseOrder.Rows.Count; i++)
                    {
                        //CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_CURR_CODE) values ('" + Code + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCurrCode")).Text + "')");
                        CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SPOM_CODE,SRPOD_I_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_TOTAL_AMT,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_T_CODE,SRPOD_SPECIFICATION,SRPOD_ITEM_NAME,SRPOD_EXC_PER,SRPOD_EDU_CESS_PER,SRPOD_H_EDU_CESS,SRPOD_CENTRAL_TAX_AMT,SRPOD_STATE_TAX_AMT,SRPOD_INTEGRATED_TAX_AMT) values ('" + Code + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceCode1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceName")).Text + "'," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ")");
                        //CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                    }
                    for (int i = 0; i < dgDocView.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO SERVICE_PO_ATTACHMENT (SPOA_SRPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Code + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/ServicePO/" + Code + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "')");
                    }

                    CommonClasses.WriteLog("Service Purchase Order", "Save", "Service Purchase Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ddlPOType.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                if (CommonClasses.Execute1("UPDATE SERVICE_PO_MASTER SET SRPOM_TYPE='" + ddlPOType.SelectedValue + "',SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' ,SRPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SRPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SRPOM_SUP_REF ='" + txtServiceRef.Text.Trim() + "', SRPOM_SUP_REF_DATE='" + txtServiceRefDate.Text + "',SRPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SRPOM_TRANSPOTER='" + txtTranspoter.Text.Trim().Replace("'", "\''") + "',SRPOM_PAY_TERM1='" + txtPaymentTerm.Text.Trim().Replace("'", "\''") + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text.Trim().Replace("'", "\''") + "',SRPOM_GUARNTY='" + txtGuranteeWaranty.Text.Trim().Replace("'", "\''") + "',SRPOM_NOTES='" + txtNote.Text.Trim().Replace("'", "\''") + "',SRPOM_DELIVERED_TO='" + txtDeliveryTo.Text.Trim().Replace("'", "\''") + "',SRPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SRPOM_CIF_NO='" + txtCIF.Text.Trim().Replace("'", "\''") + "',SRPOM_PACKING='" + txtPacking.Text.Trim().Replace("'", "\''") + "' , SRPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SRPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SRPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "'    ,SRPOM_PROJ_NAME='" + txtProjName.Text.ToUpper().Trim().Replace("'", "\''") + "',SRPOM_CGST_AMT=" + txtCGSTTax.Text + ", SRPOM_SGST_AMT=" + txtSGSTTax.Text + ",SRPOM_IGST_AMT=" + txtIGSTTax.Text + ",SRPOM_NET_AMT=" + txtNetTotal.Text + "  where SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM SERVICE_PO_DETAILS WHERE SRPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result1 = CommonClasses.Execute1("DELETE FROM SERVICE_PO_ATTACHMENT WHERE SPOA_SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgServicePurchaseOrder.Rows.Count; i++)
                        {
                            //CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SRPOM_CODE,SRPOD_I_CODE,SRPOD_UOM_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_EXC_PER,SRPOD_T_CODE,SRPOD_TOTAL_AMT,SRPOD_I_SIZE,SRPOD_INW_QTY,SRPOD_EXC_Y_N,SRPOD_TAX_Y_N) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_I_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_UOM_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_ORDER_QTY")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_RATE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_DISC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_DISC_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_EXC_PER")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_T_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_TOTAL_AMT")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_I_SIZE")).Text + "',0,'" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_EXC_Y_N")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblSRPOD_TAX_Y_N")).Text + "')");
                            //CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SRPOM_CODE,SRPOD_I_CODE,SRPOD_UOM_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_RATE_UOM,SRPOD_CONV_RATIO,SRPOD_TOTAL_AMT,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_EXC_Y_N,SRPOD_T_CODE,SRPOD_ACTIVE_IND,SRPOD_SPECIFICATION,SRPOD_ITEM_NAME,SRPOD_INW_QTY,SRPOD_EXC_PER,SRPOD_EDU_CESS_PER,SRPOD_H_EDU_CESS) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSRPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + ")");
                            CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SPOM_CODE,SRPOD_I_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_TOTAL_AMT,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_T_CODE,SRPOD_SPECIFICATION,SRPOD_ITEM_NAME,SRPOD_EXC_PER,SRPOD_EDU_CESS_PER,SRPOD_H_EDU_CESS,SRPOD_CENTRAL_TAX_AMT,SRPOD_STATE_TAX_AMT,SRPOD_INTEGRATED_TAX_AMT) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceCode1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceName")).Text + "'," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ")");
                            // CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                        }
                        for (int i = 0; i < dgDocView.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO SERVICE_PO_ATTACHMENT (SPOA_SRPOM_CODE, SPOA_DOC_NAME, SPOA_DOC_PATH)VALUES('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)(dgDocView.Rows[i].FindControl("lblfilename"))).Text + "','" + "~/UpLoadPath/ServicePO/" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "" + '/' + "" + ((Label)(dgDocView.Rows[i].FindControl("lblfileName"))).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("SERVICE_PO_MASTER", "MODIFY", "SRPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Service Purchase Order", "Update", "Service Purcahse Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);
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
            else if (Request.QueryString[0].Equals("AMEND"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(SRPOM_AM_COUNT,0) as SRPOM_AM_COUNT from SERVICE_PO_MASTER WHERE SRPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["SRPOM_AM_COUNT"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                CommonClasses.Execute1("update  SERVICE_PO_MASTER set SRPOM_IS_SHORT_CLOSE='0' , SRPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',SRPOM_AM_COUNT='" + AMEND_COUNT + "',SRPOM_SUP_REF ='" + txtServiceRef.Text.Trim() + "', SRPOM_SUP_REF_DATE='" + txtServiceRefDate.Text + "',SRPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SRPOM_TRANSPOTER='" + txtTranspoter.Text + "',SRPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SRPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SRPOM_NOTES='" + txtNote.Text + "',SRPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SRPOM_CIF_NO='" + txtCIF.Text + "',SRPOM_PACKING='" + txtPacking.Text + "' , SRPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SRPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SRPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "' WHERE SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (CommonClasses.Execute1("INSERT INTO SERVICE_PO_AM_MASTER select * from SERVICE_PO_MASTER where SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                {
                    string MatserCode = CommonClasses.GetMaxId("Select Max(AM_CODE) from SERVICE_PO_AM_MASTER");
                    DataTable dtDetail = CommonClasses.Execute("select * from SERVICE_PO_DETAILS where SRPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        CommonClasses.Execute1("INSERT INTO SERVICE_PO_AMD_DETAILS (SRPOD_AMD_CODE,SRPOD_AMD_SPOM_AM_CODE,SRPOD_AMD_I_CODE,SRPOD_AMD_UOM_CODE,SRPOD_AMD_ORDER_QTY,SRPOD_AMD_RATE,SRPOD_AMD_RATE_UOM,SRPOD_AMD_CONV_RATIO,SRPOD_AMD_DISC_PER,SRPOD_AMD_DISC_AMT,SRPOD_AMD_EXC_Y_N,SRPOD_AMD_T_CODE,SRPOD_AMD_TOTAL_AMT,SRPOD_AMD_INW_QTY,SRPOD_AMD_ACTIVE_IND,SRPOD_AMD_SPECIFICATION,SRPOD_AMD_EXC_PER,SRPOD_AMD_EDU_CESS_PER,	SRPOD_AMD_H_EDU_CESS,AMD_AM_CODE,SRPOD_AMD_ITEM_NAME)  values('" + dtDetail.Rows[j]["SRPOD_CODE"] + "','" + dtDetail.Rows[j]["SRPOD_SPOM_CODE"] + "','" + dtDetail.Rows[j]["SRPOD_I_CODE"] + "','" + dtDetail.Rows[j]["SRPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["SRPOD_ORDER_QTY"] + "','" + dtDetail.Rows[j]["SRPOD_RATE"] + "','" + dtDetail.Rows[j]["SRPOD_RATE_UOM"] + "','" + dtDetail.Rows[j]["SRPOD_CONV_RATIO"] + "','" + dtDetail.Rows[j]["SRPOD_DISC_PER"] + "','" + dtDetail.Rows[j]["SRPOD_DISC_AMT"] + "','" + dtDetail.Rows[j]["SRPOD_EXC_Y_N"] + "','" + dtDetail.Rows[j]["SRPOD_T_CODE"] + "','" + dtDetail.Rows[j]["SRPOD_TOTAL_AMT"] + "','" + dtDetail.Rows[j]["SRPOD_INW_QTY"] + "','" + dtDetail.Rows[j]["SRPOD_ACTIVE_IND"] + "','" + dtDetail.Rows[j]["SRPOD_SPECIFICATION"] + "','" + dtDetail.Rows[j]["SRPOD_EXC_PER"] + "','" + dtDetail.Rows[j]["SRPOD_EDU_CESS_PER"] + "','" + dtDetail.Rows[j]["SRPOD_H_EDU_CESS"] + "','" + MatserCode + "','" + dtDetail.Rows[j]["SRPOD_ITEM_NAME"] + "')");
                    }

                    #region ModifyOriginalPO
                    if (CommonClasses.Execute1("UPDATE SERVICE_PO_MASTER SET SRPOM_TYPE='" + ddlPOType.SelectedValue + "',SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "',SRPOM_PONO='" + txtPoNo.Text + "',SRPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SRPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SRPOM_SUP_REF_DATE='" + txtServiceRefDate.Text + "',SRPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SRPOM_TRANSPOTER='" + txtTranspoter.Text + "',SRPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SRPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SRPOM_NOTES='" + txtNote.Text + "',SRPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SRPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SRPOM_CIF_NO='" + txtCIF.Text + "',SRPOM_PACKING='" + txtPacking.Text + "',SRPOM_AM_COUNT='" + AMEND_COUNT + "',SRPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' ,SRPOM_PROJECT='" + ddlProjectCode.SelectedValue + "'  where SRPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        result = CommonClasses.Execute1("update  SERVICE_PO_AM_MASTER set SRPOM_AM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE SRPOM_AM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and SRPOM_AM_AM_COUNT='" + AMEND_COUNT + "'");
                        result = CommonClasses.Execute1("DELETE FROM SERVICE_PO_DETAILS WHERE SRPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgServicePurchaseOrder.Rows.Count; i++)
                            {
                                //CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SPOM_CODE,SRPOD_I_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_RATE_UOM,SRPOD_CONV_RATIO,SRPOD_TOTAL_AMT,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_EXC_Y_N,SRPOD_T_CODE,SRPOD_ACTIVE_IND,SRPOD_SPECIFICATION,SRPOD_ITEM_NAME,SRPOD_INW_QTY,SRPOD_EXC_PER,SRPOD_EDU_CESS_PER,SRPOD_H_EDU_CESS) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceCode1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSpecification")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSRPOD_ITEM_NAME")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSRPOD_INW_QTY")).Text + "'," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + ")");
                                CommonClasses.Execute1("INSERT INTO SERVICE_PO_DETAILS (SRPOD_SPOM_CODE,SRPOD_I_CODE,SRPOD_ORDER_QTY,SRPOD_RATE,SRPOD_TOTAL_AMT,SRPOD_DISC_PER,SRPOD_DISC_AMT,SRPOD_T_CODE,SRPOD_SPECIFICATION,SRPOD_ITEM_NAME,SRPOD_EXC_PER,SRPOD_EDU_CESS_PER,SRPOD_H_EDU_CESS,SRPOD_CENTRAL_TAX_AMT,SRPOD_STATE_TAX_AMT,SRPOD_INTEGRATED_TAX_AMT) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceCode1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblServiceName")).Text + "'," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblExisDuty")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblEduCess")).Text + "," + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblSHECess")).Text + ")");
                                //CommonClasses.Execute("update ITEM_MASTER set I_INV_RATE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblRate")).Text + "' WHERE I_CODE='" + ((Label)dgServicePurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }

                            CommonClasses.RemoveModifyLock("SERVICE_PO_MASTER", "MODIFY", "SRPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                            CommonClasses.WriteLog("Service Purchase Order", "Amend", "Service Purcahse Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);
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
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Purchase Order", "SaveRec", ex.Message);
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
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceCode1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ServiceName");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");

            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExisDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");

        }
        ((DataTable)ViewState["dt2"]).Rows.Add(((DataTable)ViewState["dt2"]).NewRow());

        dgServicePurchaseOrder.Visible = true;
        dgServicePurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
        dgServicePurchaseOrder.DataBind();
        dgServicePurchaseOrder.Enabled = false;


        ((DataTable)ViewState["dt3"]).Clear();
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SRPOM_CODE");
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
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SRPOM_CODE");
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
        if (ddlServiceCode.SelectedIndex != 0)
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
            CommonClasses.SendError("Service Po", "ddlCurrency_SelectedIndexChanged", Ex.Message);
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
            //dgServicePurchaseOrder.DeleteRow(Index);
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
                    // CommonClasses.Execute("select isnull(max(SRPOM_CODE+1),0)as Code from SERVICE_PO_MASTER");
                    code = CommonClasses.GetMaxId("select isnull(max(SRPOM_CODE+1),0)as Code from SERVICE_PO_MASTER");
                }
                else
                {
                    code = ((Label)(row.FindControl("lblSPOA_SRPOM_CODE"))).Text;
                    if (code == "")
                    {
                        code = CommonClasses.GetMaxId("Select SRPOM_CODE from SERVICE_PO_MASTER where SRPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                    }
                }
                filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                directory = "../../UpLoadPath/ServicePO/" + code + "/" + filePath;
                Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");
            }
        }
        if (e.CommandName == "Download")
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                code = CommonClasses.GetMaxId("Select Max(SRPOM_CODE) from SERVICE_PO_MASTER");
            }
            else
            {
                code = ((Label)(row.FindControl("lblSPOA_SRPOM_CODE"))).Text;
                if (code == "")
                {
                    code = CommonClasses.GetMaxId("Select SRPOM_CODE from SERVICE_PO_MASTER where SRPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                }
            }
            filePath = ((Label)(row.FindControl("lblfilename"))).Text;
            directory = "../../UpLoadPath/ServicePO/" + code + "/" + filePath;
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
            dt = CommonClasses.Execute("select isnull(max(SRPOM_CODE+1),0)as Code from SERVICE_PO_MASTER");
            if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
            {
                DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('SERVICE_PO_MASTER')+1");
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

        string sDirPath = Server.MapPath(@"~/UpLoadPath/ServicePO/" + ViewState["File_Code"].ToString() + "");

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
            string directory = Server.MapPath(@"~/UpLoadPath/ServicePO/" + ViewState["File_Code"].ToString() + "");

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
