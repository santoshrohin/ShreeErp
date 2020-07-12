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

public partial class Transactions_ADD_DebitNote : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    static int mlCode = 0;
    static int RowCount = 0;
    DataRow dr;
    int PartyType = 0;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    double disamt = 0;
    DataTable dtFilter = new DataTable();

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
                    try
                    {
                        txtTOS.Text = DateTime.Now.ToString("hh:mm:ss");
                        dt2.Clear();
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Clear();
                        ViewState["mlCode"] = "";
                        ViewState["RowCount"] = "";
                        ViewState["Index"] = "";

                        ViewState["mlCode"] = mlCode;
                        ViewState["RowCount"] = RowCount;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;

                        LoadCustomer();
                        LoadICode();
                        LoadIName();
                        LoadUnit();
                        LoadFilter();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            str = "VIEW";
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            str = "MOD";
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            str = "INSERT";
                            LoadFilter();

                            dgMainDC.Enabled = false;
                           // txtDebitDate.Text = CommonClasses.GetCurrentTime().Date.ToString("dd MMM yyyy");
                            txtDebitDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("DebitNote", "Page_Load", ex.Message.ToString());
                    }
                    txtDebitDate.Attributes.Add("readonly", "readonly");
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("DebitNote", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgMainDC.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UnitCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Qty", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Amt", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("Disc_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Disc_Amt", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Amort_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Amort_Amt", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Qty_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Packing", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("CGST_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SGST_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IGST_Per", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CGST_Amt", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SGST_Amt", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IGST_Amt", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("Remark", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgMainDC.DataSource = dtFilter;
                dgMainDC.DataBind();
                dgMainDC.Enabled = false;
            }
        }
    }
    #endregion LoadFilter

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            //dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND P_TYPE='1' AND P_ACTIVE_IND=1   ORDER BY P_NAME ASC-- and CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC");
            // Remove join with CUSTPO_MASTER (load All Customers) And Check All Party types(Customer/Supplier)
            dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND P_ACTIVE_IND=1   ORDER BY P_NAME ASC ");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadCustomer", Ex.Message);
        }

        try
        {
            //dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND P_TYPE='1' AND P_ACTIVE_IND=1   ORDER BY P_NAME ASC-- and CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC");
            // Remove join with CUSTPO_MASTER (load All Customers) And Check All Party types(Customer/Supplier)
            dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND P_ACTIVE_IND=1   ORDER BY P_NAME ASC ");
            ddlConsignee.DataSource = dt;
            ddlConsignee.DataTextField = "P_NAME";
            ddlConsignee.DataValueField = "P_CODE";
            ddlConsignee.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadUnit
    private void LoadUnit()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT I_UOM_CODE ,I_UOM_NAME FROM ITEM_UNIT_MASTER WHERE ITEM_UNIT_MASTER.ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ");
            ddlUnit.DataSource = dt;
            ddlUnit.DataTextField = "I_UOM_NAME";
            ddlUnit.DataValueField = "I_UOM_CODE";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("Unit", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadUnit", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Validation())
            //{
            if (dgMainDC.Enabled == false)
            {
                ShowMessage("#Avisos", "Please insert Item details.", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (dgMainDC.Rows.Count > 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            }
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion btnSubmit_Click

    #region SaveRec
    bool SaveRec()
    {
        DataTable dtplusminus = new DataTable();
        string plusminus = "";
        bool Result = false;
        try
        {

            dtplusminus = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_LBT_NO,P_NAME,ISNULL( P_LBT_IND,0) AS P_LBT_IND,P_TYPE  FROM PARTY_MASTER where ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "' AND P_ACTIVE_IND=1   AND P_CODE= '" + ddlCustomer.SelectedValue + "' ");

            if (dtplusminus.Rows.Count > 0)
            {
                PartyType = Convert.ToInt32(dtplusminus.Rows[0]["P_TYPE"]);


            }
            if (PartyType == 2)
            {
                plusminus = "-";
            }
            if (PartyType == 1)
            {

            }

            #region Apostrophe_Replace
            string address = txtAddress.Text;
            address = address.Replace("'", "\''");
            #endregion Apostrophe_Replace

            // HSN_CODE_FROM Select from  EXCISE_TARIFF_MASTER
            #region HSN_CODE_FROM_EXCISE_MASTER
            string HSNCode = "";
            DataTable dtHSNCode = CommonClasses.Execute("SELECT * FROM EXCISE_TARIFF_MASTER WHERE EXCISE_TARIFF_MASTER.ES_DELETE=0  AND E_TARIFF_NO='" + txtHSNNumber.Text + "'");
            if (dtHSNCode.Rows.Count > 0)
            {
                HSNCode = dtHSNCode.Rows[0]["E_CODE"].ToString();
            }
            else
            {
                HSNCode = CommonClasses.Execute("SELECT E_CODE FROM EXCISE_TARIFF_MASTER WHERE EXCISE_TARIFF_MASTER.ES_DELETE=0  AND E_TARIFF_NO='N/A'").ToString();
            }
            #endregion HSN_CODE_FROM_EXCISE_MASTER

            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Credit_No = 0;
                DataTable dt = new DataTable();
                string DebitNote_No = "";
                // Insert Serial Number in Credit Master
                dt = CommonClasses.Execute("SELECT ISNULL(MAX(CAST(DNM_SERIAL_NO AS INT)),0) AS DNM_SERIAL_NO FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Credit_No = Convert.ToInt32(dt.Rows[0]["DNM_SERIAL_NO"]);
                    Credit_No = Credit_No + 1;
                    DebitNote_No = CommonClasses.GenBillNo(Credit_No);
                    DebitNote_No = "G" + DebitNote_No;
                }
                // Insert Credit Master
                if (CommonClasses.Execute1("INSERT INTO [dbo].[DEBIT_NOTE_MASTER]  	([DNM_CM_CODE],[DNM_CUST_CODE] ,[DNM_CM_ID],	[DNM_DATE] ,	[DNM_SERIAL_NO] ,	[DNM_GSTIN_NO] ,	[DNM_EWAY_BILL_NO] ,	[DNM_CUST_ADDRESS] ,	[DNM_CUST_STATE_NAME] ,	[DNM_CUST_STATE_CODE] ,	[DNM_GRAND_TOTAL] ,	[DNM_CENTRAL_TAX_AMT] ,	[DNM_STATE_UNION_TAX_AMT] ,	[DNM_INTEGRATED_TAX_AMT] ,	[DNM_CESS_TAX_AMT] ,	[MODIFY] ,	[ES_DELETE],	DNM_NET_AMOUNT,DNM_PACK_PER,DNM_PACK_AMT,DNM_AMR_TOTAL,DNM_FRIEGHT_AMT,DNM_INSUR_AMT,DNM_OTHER_AMT,DNM_CONSIGNE_CODE,DNM_TRANSPORTER,DNM_VEHICLE_NO,DNM_LR_NO,DNM_TIME_SUPPLY,DNM_CR_DAYS,DNM_REMARKS,plusminus)	VALUES ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlCustomer.SelectedValue + "','" + Convert.ToInt32(Session["CompanyId"]) + "',	'" + Convert.ToDateTime(txtDebitDate.Text).ToString("dd/MMM/yyyy") + "','" + Credit_No + "',	'" + txtGSTINNo.Text + "',	'" + txtEWayBillNo.Text + "',	'" + address.ToString() + "',	'" + ddlState.SelectedItem + "',	'" + ddlState.SelectedValue + "',	'" + txtBasicAmount.Text + "',	'" + txtCGSTAmt.Text + "',	'" + txtSGSTAmt.Text + "',	'" + txtIGSTAmt.Text + "',	0,	0,	0,'" + txtNetAmount.Text + "','" + txtPackingPer.Text + "','" + txtPackingAmt.Text + "','" + txtAmortTotalAmt.Text + "','" + txtFrieghtAmt.Text + "','" + txtInsuranceAmt.Text + "','" + txtOtherAmt.Text + "','" + ddlConsignee.SelectedValue + "','" + txtTransporter.Text + "','" + txtVehicleNo.Text + "','" + txtLrNo.Text + "','" + txtTOS.Text + "','" + txtCrDays.Text + "','" + txtRemarkm.Text + "','" + plusminus + "') "))
                {
                    string Code = CommonClasses.GetMaxId("SELECT MAX(DNM_CODE) FROM DEBIT_NOTE_MASTER");

                    //Entry In Account Ledger

                    Result = CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + ddlCustomer.SelectedValue + "','" + Code + "','" + Credit_No + "','DEBITENTRY','" + Convert.ToDateTime(txtDebitDate.Text).ToString("dd/MMM/yyyy") + "'," + txtNetAmount.Text + ",'" + ddlCustomer.SelectedValue + "')");



                    // // Insert Credit Detail
                    for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                    {
                        Result = CommonClasses.Execute1("INSERT INTO [dbo].[DEBIT_NOTE_DETAILS] ([DND_DNM_CODE] ,	[DND_ITEM_CODE] ,	[DND_DESCRIPTION] ,	[DND_HSN_SAC] ,	[DND_UOM] ,	[DND_QTY] ,	[DND_RATE] ,	[DND_AMOUNT] ,	[DND_CENTRAL_TAX] ,	[DND_STATE_UNION_TAX] ,	[DND_INTEGRATED_TAX] ,	[DND_CESS_TAX] ,	[DND_CENTRAL_TAX_AMT] ,	[DND_STATE_UNION_TAX_AMT] ,	[DND_INTEGRATED_TAX_AMT] ,	[DND_CESS_TAX_AMT],DND_DISC_PER,DND_DISC_AMT,DND_AMR_RATE,DND_AMR_AMT,DND_QTY_PACK,DND_PACKING) 	VALUES ('" + Code + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["ItemCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Remark"] + "','" + HSNCode.ToString() + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["UnitCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Qty"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Rate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Per"] + "',	0,'" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Amt"] + "',0,'" + ((DataTable)ViewState["dt2"]).Rows[i]["Disc_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Disc_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amort_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amort_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Qty_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Packing"] + "')");








                    }
                    // Insert Log Master
                    CommonClasses.WriteLog("DebitNote", "Save", "DebitNote", Convert.ToString(Credit_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    Result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewDebitNote.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtDebitSerialNumber.Focus();
                }
            }
            #endregion Insert

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                // Update DEBIT_NOTE_MASTER
                if (CommonClasses.Execute1("UPDATE DEBIT_NOTE_MASTER SET [DNM_CM_CODE]='" + Convert.ToInt32(Session["CompanyCode"]) + "',	[DNM_CUST_CODE] ='" + ddlCustomer.SelectedValue + "',	[DNM_CM_ID]='" + Convert.ToInt32(Session["CompanyId"]) + "',	[DNM_DATE] ='" + Convert.ToDateTime(txtDebitDate.Text).ToString("dd/MMM/yyyy") + "',	[DNM_SERIAL_NO] ='" + txtDebitSerialNumber.Text + "',	[DNM_GSTIN_NO] ='" + txtGSTINNo.Text + "',	[DNM_EWAY_BILL_NO] ='" + txtEWayBillNo.Text + "',	[DNM_CUST_ADDRESS] ='" + address.ToString() + "',	[DNM_CUST_STATE_NAME] ='" + ddlState.SelectedItem + "',	[DNM_CUST_STATE_CODE] ='" + ddlState.SelectedValue + "',	[DNM_GRAND_TOTAL] ='" + txtBasicAmount.Text + "',	[DNM_CENTRAL_TAX_AMT] ='" + txtCGSTAmt.Text + "',	[DNM_STATE_UNION_TAX_AMT] ='" + txtSGSTAmt.Text + "',	[DNM_INTEGRATED_TAX_AMT] ='" + txtIGSTAmt.Text + "',	[DNM_CESS_TAX_AMT] =0,	[MODIFY] =0,	[ES_DELETE]=0,	DNM_NET_AMOUNT='" + txtNetAmount.Text + "' ,	DNM_PACK_PER='" + txtPackingPer.Text + "',	DNM_PACK_AMT='" + txtPackingAmt.Text + "',	DNM_AMR_TOTAL='" + txtAmortTotalAmt.Text + "',	DNM_FRIEGHT_AMT='" + txtFrieghtAmt.Text + "',	DNM_INSUR_AMT='" + txtInsuranceAmt.Text + "',	DNM_OTHER_AMT='" + txtOtherAmt.Text + "',	DNM_CONSIGNE_CODE='" + ddlConsignee.SelectedValue + "',	DNM_TRANSPORTER='" + txtTransporter.Text + "',	DNM_VEHICLE_NO='" + txtVehicleNo.Text + "',	DNM_LR_NO='" + txtLrNo.Text + "',	DNM_TIME_SUPPLY='" + txtTOS.Text + "',	DNM_CR_DAYS='" + txtCrDays.Text + "',	DNM_REMARKS='" + txtRemarkm.Text + "',	plusminus='" + plusminus + "' WHERE DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {

                    //Entry In Account Ledger

                    CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and ACCNT_DOC_TYPE='DEBITENTRY'");

                    CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + ddlCustomer.SelectedValue + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtDebitSerialNumber.Text + "','DEBITENTRY','" + Convert.ToDateTime(txtDebitDate.Text).ToString("dd/MMM/yyyy") + "','" + txtNetAmount.Text + "','" + ddlCustomer.SelectedValue + "')");






                    // Delete DEBIT_NOTE_DETAILS
                    Result = CommonClasses.Execute1("DELETE FROM DEBIT_NOTE_DETAILS WHERE DND_DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                    for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                    {
                        // Insert DEBIT_NOTE_DETAILS
                        //Result = CommonClasses.Execute1("INSERT INTO [dbo].[DEBIT_NOTE_DETAILS] ([DND_DNM_CODE] ,	[DND_ITEM_CODE] ,	[DND_DESCRIPTION] ,	[DND_HSN_SAC] ,	[DND_UOM] ,	[DND_QTY] ,	[DND_RATE] ,	[DND_AMOUNT] ,	[DND_CENTRAL_TAX] ,	[DND_STATE_UNION_TAX] ,	[DND_INTEGRATED_TAX] ,	[DND_CESS_TAX] ,	[DND_CENTRAL_TAX_AMT] ,	[DND_STATE_UNION_TAX_AMT] ,	[DND_INTEGRATED_TAX_AMT] ,	[DND_CESS_TAX_AMT]) 	VALUES ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["ItemCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Remark"] + "','" + HSNCode.ToString() + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["UnitCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Qty"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Rate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Per"] + "',	0,'" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Amt"] + "',	0)");
                        Result = CommonClasses.Execute1("INSERT INTO [dbo].[DEBIT_NOTE_DETAILS] ([DND_DNM_CODE] ,	[DND_ITEM_CODE] ,	[DND_DESCRIPTION] ,	[DND_HSN_SAC] ,	[DND_UOM] ,	[DND_QTY] ,	[DND_RATE] ,	[DND_AMOUNT] ,	[DND_CENTRAL_TAX] ,	[DND_STATE_UNION_TAX] ,	[DND_INTEGRATED_TAX] ,	[DND_CESS_TAX] ,	[DND_CENTRAL_TAX_AMT] ,	[DND_STATE_UNION_TAX_AMT] ,	[DND_INTEGRATED_TAX_AMT] ,	[DND_CESS_TAX_AMT],DND_DISC_PER,DND_DISC_AMT,DND_AMR_RATE,DND_AMR_AMT,DND_QTY_PACK,DND_PACKING) 	VALUES ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["ItemCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Remark"] + "','" + HSNCode.ToString() + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["UnitCode"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Qty"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Rate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Per"] + "',	0,'" + ((DataTable)ViewState["dt2"]).Rows[i]["CGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["SGST_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IGST_Amt"] + "',0,'" + ((DataTable)ViewState["dt2"]).Rows[i]["Disc_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Disc_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amort_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Amort_Amt"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Qty_Per"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["Packing"] + "')");


                    }
                    CommonClasses.RemoveModifyLock("DEBIT_NOTE_MASTER", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                    CommonClasses.WriteLog("DebitNote", "Update", "Tax Invoice", Convert.ToString(Convert.ToInt32(ViewState["mlCode"].ToString())), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    //Result = true;
                    Response.Redirect("~/Transactions/VIEW/ViewDebitNote.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    //   txtPONo.Focus();
                }
            }
            #endregion Modify

            ViewState["RowCount"] = 0;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return Result;
    }
    #endregion SaveRec

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Cancel();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("DebitNote", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                if (str == "VIEW")
                {
                    Cancel();
                }
                else
                {
                    if (CheckValid())
                    {
                        popUpPanel5.Visible = true;
                        ModalPopupPrintSelection.Show();
                    }
                    else
                    {
                        Cancel();
                    }
                }
            }

            catch (Exception ex)
            {
                CommonClasses.SendError("DebitNote", "CancelRecord", ex.Message.ToString());
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("DebitNote", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Cancel
    public void Cancel()
    {
        if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
        {
            CommonClasses.RemoveModifyLock("DEBIT_MASTER_NOTE", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
        }
        ((DataTable)ViewState["dt2"]).Rows.Clear();
        Response.Redirect("~/Transactions/VIEW/ViewDebitNote.aspx", false);
    }
    #endregion Cancel

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
            else if (txtDebitDate.Text == "")
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
            CommonClasses.SendError("DebitNote", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dtCreditDetail = new DataTable();
            DataTable dtCreditMaster = new DataTable();
            dtCreditMaster = CommonClasses.Execute("SELECT [DNM_CM_CODE],[DNM_CUST_CODE] ,[DNM_CM_ID],	[DNM_DATE] ,	[DNM_SERIAL_NO] ,	[DNM_GSTIN_NO] ,	[DNM_EWAY_BILL_NO] ,	[DNM_CUST_ADDRESS] ,	[DNM_CUST_STATE_NAME] ,	[DNM_CUST_STATE_CODE] ,	ISNULL([DNM_GRAND_TOTAL],0) AS DNM_GRAND_TOTAL ,	ISNULL([DNM_CENTRAL_TAX_AMT],0) AS DNM_CENTRAL_TAX_AMT ,	ISNULL([DNM_STATE_UNION_TAX_AMT] ,0) AS DNM_STATE_UNION_TAX_AMT,	ISNULL([DNM_INTEGRATED_TAX_AMT],0) AS DNM_INTEGRATED_TAX_AMT ,ISNULL([DNM_CESS_TAX_AMT],0) AS DNM_CESS_TAX_AMT ,DEBIT_NOTE_MASTER.[MODIFY] ,	DEBIT_NOTE_MASTER.[ES_DELETE],ISNULL(DNM_NET_AMOUNT,0) AS DNM_NET_AMOUNT ,STATE_MASTER.SM_NAME ,ISNULL(STATE_MASTER.SM_STATE_CODE,'') AS SM_STATE_CODE,DNM_PACK_PER,DNM_PACK_AMT,DNM_AMR_TOTAL,DNM_FRIEGHT_AMT,DNM_INSUR_AMT,DNM_OTHER_AMT,DNM_CONSIGNE_CODE,DNM_TRANSPORTER,DNM_VEHICLE_NO,DNM_LR_NO,DNM_TIME_SUPPLY,DNM_CR_DAYS,DNM_REMARKS FROM DEBIT_NOTE_MASTER INNER JOIN STATE_MASTER ON DEBIT_NOTE_MASTER.DNM_CUST_STATE_CODE=STATE_MASTER.SM_CODE  WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 AND STATE_MASTER.ES_DELETE=0 AND  DNM_CM_CODE='" + (string)Session["CompanyCode"] + "' AND DNM_CM_ID='" + (string)Session["CompanyId"] + "' AND DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
            if (dtCreditMaster.Rows.Count > 0)
            {
                txtDebitSerialNumber.Text = dtCreditMaster.Rows[0]["DNM_SERIAL_NO"].ToString();
                txtDebitDate.Text = Convert.ToDateTime(dtCreditMaster.Rows[0]["DNM_DATE"]).ToString("dd MMM yyyy");
                txtGSTINNo.Text = dtCreditMaster.Rows[0]["DNM_GSTIN_NO"].ToString();
                txtEWayBillNo.Text = dtCreditMaster.Rows[0]["DNM_EWAY_BILL_NO"].ToString();
                txtEWayBillNo.Text = dtCreditMaster.Rows[0]["DNM_EWAY_BILL_NO"].ToString();
                txtStateCode.Text = dtCreditMaster.Rows[0]["SM_STATE_CODE"].ToString();
                ddlCustomer.SelectedValue = dtCreditMaster.Rows[0]["DNM_CUST_CODE"].ToString();
                ddlCustomer_SelectedIndexChanged(null, null);
                ddlState.SelectedValue = dtCreditMaster.Rows[0]["DNM_CUST_STATE_CODE"].ToString();
                ddlState_SelectedIndexChanged(null, null);
                txtAddress.Text = dtCreditMaster.Rows[0]["DNM_CUST_ADDRESS"].ToString();

                txtPackingPer.Text = dtCreditMaster.Rows[0]["DNM_PACK_PER"].ToString();
                txtPackingAmt.Text = dtCreditMaster.Rows[0]["DNM_PACK_AMT"].ToString();
                txtAmortTotalAmt.Text = dtCreditMaster.Rows[0]["DNM_AMR_TOTAL"].ToString();
                txtFrieghtAmt.Text = dtCreditMaster.Rows[0]["DNM_FRIEGHT_AMT"].ToString();
                txtInsuranceAmt.Text = dtCreditMaster.Rows[0]["DNM_INSUR_AMT"].ToString();
                ddlConsignee.SelectedValue = dtCreditMaster.Rows[0]["DNM_CONSIGNE_CODE"].ToString();
                txtTransporter.Text = dtCreditMaster.Rows[0]["DNM_TRANSPORTER"].ToString();
                txtVehicleNo.Text = dtCreditMaster.Rows[0]["DNM_VEHICLE_NO"].ToString();
                txtLrNo.Text = dtCreditMaster.Rows[0]["DNM_LR_NO"].ToString();
                txtTOS.Text = dtCreditMaster.Rows[0]["DNM_TIME_SUPPLY"].ToString();
                txtCrDays.Text = dtCreditMaster.Rows[0]["DNM_CR_DAYS"].ToString();
                txtRemarkm.Text = dtCreditMaster.Rows[0]["DNM_REMARKS"].ToString();
                txtOtherAmt.Text = dtCreditMaster.Rows[0]["DNM_OTHER_AMT"].ToString();

                dtCreditDetail = CommonClasses.Execute("SELECT DISTINCT DEBIT_NOTE_DETAILS.DND_ITEM_CODE AS ItemCode,ITEM_MASTER.I_CODENO AS IND_I_CODENO ,I_NAME AS ItemName,ITEM_UNIT_MASTER.I_UOM_CODE AS UnitCode,ITEM_UNIT_MASTER.I_UOM_NAME AS Unit,DND_QTY AS Qty,DND_RATE AS Rate,DND_AMOUNT AS Amt,DND_CENTRAL_TAX AS CGST_Per, DND_STATE_UNION_TAX AS SGST_Per,DND_INTEGRATED_TAX AS IGST_Per,DND_CENTRAL_TAX_AMT AS CGST_Amt,DND_STATE_UNION_TAX_AMT AS SGST_Amt,DND_INTEGRATED_TAX_AMT AS IGST_Amt, DND_DESCRIPTION AS Remark,DND_DISC_PER as Disc_Per,DND_DISC_AMT as Disc_Amt,DND_AMR_RATE as Amort_Per,DND_AMR_AMT as Amort_Amt,DND_QTY_PACK as Qty_Per,DND_PACKING as Packing FROM DEBIT_NOTE_MASTER ,DEBIT_NOTE_DETAILS ,ITEM_MASTER ,ITEM_UNIT_MASTER WHERE DNM_CODE=DND_DNM_CODE AND DEBIT_NOTE_DETAILS.DND_ITEM_CODE=I_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND DEBIT_NOTE_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND ITEM_UNIT_MASTER.ES_DELETE=0 AND DNM_CM_ID='" + (string)Session["CompanyId"] + "' AND DNM_CM_CODE='" + (string)Session["CompanyCode"] + "' AND DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");
                //txtCGSTPer.Text = dtCreditDetail.Rows[0]["CGST_Per"].ToString();
                txtCGSTPer.Text = "0";
                txtCGSTAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtCreditDetail.Rows[0]["CGST_Amt"].ToString()));
                //txtSGSTPer.Text = dtCreditDetail.Rows[0]["SGST_Per"].ToString();
                txtSGSTPer.Text = "0";
                txtSGSTAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtCreditDetail.Rows[0]["SGST_Amt"].ToString()));
                //txtIGSTPer.Text = dtCreditDetail.Rows[0]["IGST_Per"].ToString();
                txtIGSTPer.Text = "0";
                txtIGSTAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtCreditDetail.Rows[0]["IGST_Amt"].ToString()));
                LoadICode();
                LoadIName();
                //LoadPO();
                if (dtCreditDetail.Rows.Count != 0)
                {
                    dgMainDC.DataSource = dtCreditDetail;
                    dgMainDC.DataBind();
                    ViewState["dt2"] = dtCreditDetail;
                    dgMainDC.Enabled = true;
                    //txtTotalAmt.Text = string.Format("{0:0.00}", sum1);
                    Calcall();
                }
                if (str == "VIEW")
                {
                    btnSubmit.Enabled = false;
                    ddlCustomer.Enabled = false;
                    ddlState.Enabled = false;
                    ddlItemCode.Enabled = false;
                    ddlItemName.Enabled = false;
                    txtAmount.Enabled = false;
                    txtAddress.Enabled = false;
                    txtStateCode.Enabled = false;
                    txtQuantity.Enabled = false;
                    txtRate.Enabled = false;
                    txtRemark.Enabled = false;
                    txtEWayBillNo.Enabled = false;
                    btnInsert.Enabled = false;
                    dgMainDC.Enabled = false;
                }
                if (str == "MOD")
                {
                    ddlCustomer.Enabled = false;
                    CommonClasses.SetModifyLock("DEBIT_NOTE_MASTER", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Visible = true;
                lblmsg.Text = "Data Not Found...";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
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
            if (ddlItemCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            if (txtDebitDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDebitDate.Focus();
                return;
            }
            if (ddlState.SelectedIndex == 0 || ddlState.SelectedIndex == -1)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select State Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }
            if (txtAmount.Text == "0.00" || txtAmount.Text == "0.000")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Insert Quantity and Rate.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDebitDate.Focus();
                return;
            }
            #endregion Validation

            #region Record_Exist
            if (dgMainDC.Enabled)
            {
                for (int i = 0; i < dgMainDC.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgMainDC.Rows[i].FindControl("lblItemCode"))).Text;
                    //string PO_CODE = ((Label)(dgMainDC.Rows[i].FindControl("lblPO_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "This Item details already inserted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "This Item details already inserted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                }
            }
            #endregion Record_Exist

            #region Add Data table coloums
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("UnitCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                //((DataTable)ViewState["dt2"]).Columns.Add("PO_CODE");
                //((DataTable)ViewState["dt2"]).Columns.Add("PO_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("Qty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
                ((DataTable)ViewState["dt2"]).Columns.Add("Amt");

                ((DataTable)ViewState["dt2"]).Columns.Add("Disc_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("Disc_Amt");

                ((DataTable)ViewState["dt2"]).Columns.Add("Amort_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("Amort_Amt");

                ((DataTable)ViewState["dt2"]).Columns.Add("Qty_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("Packing");

                ((DataTable)ViewState["dt2"]).Columns.Add("CGST_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("SGST_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("IGST_Per");
                ((DataTable)ViewState["dt2"]).Columns.Add("CGST_Amt");
                ((DataTable)ViewState["dt2"]).Columns.Add("SGST_Amt");
                ((DataTable)ViewState["dt2"]).Columns.Add("IGST_Amt");
                ((DataTable)ViewState["dt2"]).Columns.Add("Remark");
            }
            #endregion

            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["UnitCode"] = ddlUnit.SelectedValue;
            dr["Unit"] = ddlUnit.SelectedItem;
            //dr["PO_CODE"] = ddlPONo.SelectedValue;
            //dr["PO_NO"] = ddlPONo.SelectedItem;
            dr["Qty"] = txtQuantity.Text;
            dr["Rate"] = txtRate.Text;
            dr["Amt"] = txtAmount.Text;
            dr["Disc_Per"] = txtDiscPer.Text;
            dr["Disc_Amt"] = txtDiscAmt.Text;
            dr["Amort_Per"] = txtAmort.Text;
            dr["Amort_Amt"] = txtAmortAmt.Text;
            dr["Qty_Per"] = txtQtyPack.Text;
            dr["Packing"] = txtPacking.Text;
            double CGST_Per = 0;
            double SGST_Per = 0;
            double IGST_Per = 0;
            double CGST_Amt = 0;
            double SGST_Amt = 0;
            double IGST_Amt = 0;
            DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU,E_SPECIAL FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + ddlItemCode.SelectedValue + "");
            if (dtExcisePer.Rows.Count > 0)
            {
                CGST_Per = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                SGST_Per = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                IGST_Per = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
            }
            txtCGSTPer.Text = string.Format("{0:0.00}", Convert.ToDouble(CGST_Per));
            txtSGSTPer.Text = string.Format("{0:0.00}", Convert.ToDouble(SGST_Per));
            txtIGSTPer.Text = string.Format("{0:0.00}", Convert.ToDouble(IGST_Per));

            DataTable dtCompState = new DataTable();
            DataTable dtCustState = new DataTable();
            dtCompState = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_CODE='" + Session["CompanyCode"] + "' AND ISNULL(CM_DELETE_FLAG,0)='0' AND CM_ID= '" + Session["CompanyId"] + "'");
            //dtCustState = CommonClasses.Execute("SELECT P_SM_CODE,* FROM CUSTPO_MASTER INNER JOIN PARTY_MASTER ON CUSTPO_MASTER.CPOM_P_CODE=PARTY_MASTER.P_CODE WHERE CUSTPO_MASTER.CPOM_CODE='" + ddlPONo.SelectedValue + "' AND CUSTPO_MASTER.CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CUSTPO_MASTER.ES_DELETE=0");
            dtCustState = CommonClasses.Execute("SELECT P_SM_CODE,* FROM  PARTY_MASTER  WHERE  PARTY_MASTER.P_CODE='" + ddlCustomer.SelectedValue + "' AND PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "' AND PARTY_MASTER.ES_DELETE=0 ");

            if (dtCompState.Rows[0]["CM_STATE"].ToString() == dtCustState.Rows[0]["P_SM_CODE"].ToString())
            {
                CGST_Amt = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtQuantity.Text)) * CGST_Per) / 100), 2);
                SGST_Amt = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtQuantity.Text)) * SGST_Per) / 100), 2);
                //EHEduCess = Math.Round(EBasic * EHEduCessPer / 100, 2);
                dr["CGST_Amt"] = CGST_Amt;
                dr["SGST_Amt"] = SGST_Amt;
                dr["IGST_Amt"] = 0;
                dr["CGST_Per"] = txtCGSTPer.Text;
                dr["SGST_Per"] = txtSGSTPer.Text;
                dr["IGST_Per"] = 0;
                txtIGSTPer.Text = "0.00";
            }
            else
            {
                IGST_Amt = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtQuantity.Text)) * IGST_Per) / 100), 2);
                dr["CGST_Amt"] = 0;
                dr["SGST_Amt"] = 0;
                dr["IGST_Amt"] = IGST_Amt;
                dr["CGST_Per"] = 0;
                dr["SGST_Per"] = 0;
                dr["IGST_Per"] = txtIGSTPer.Text;
                txtCGSTPer.Text = "0.00";
                txtSGSTPer.Text = "0.00";
            }
            // Apostrophe Replace
            string StrRemark = txtRemark.Text;
            StrRemark = StrRemark.Replace("'", "");
            dr["Remark"] = StrRemark.ToString();

            #region insert Data or Modify Grid Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    dgMainDC.Enabled = true;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                dgMainDC.Enabled = true;
            }
            #endregion

            //((DataTable)ViewState["dt2"]).Rows.Add(dr);
            //dgMainDC.Enabled = true;

            dgMainDC.Visible = true;
            dgMainDC.Enabled = true;

            dgMainDC.DataSource = ((DataTable)ViewState["dt2"]);
            dgMainDC.DataBind();

            Calcall();
            ClearControles();

            #region Summary_Percentage
            if (dgMainDC.Enabled)
            {

                for (int i = 0; i < dgMainDC.Rows.Count; i++)
                {
                    // for Percentage show bellow Grid
                    string Central_Tax = ((Label)(dgMainDC.Rows[i].FindControl("lblCentralTaxPer"))).Text;
                    string State_Tax = ((Label)(dgMainDC.Rows[i].FindControl("lblStatePer"))).Text;
                    string Integrated_Tax = ((Label)(dgMainDC.Rows[i].FindControl("lblIntegratedTaxPer"))).Text;

                    double Central_Tax_per = 0; double State_Tax_per = 0; double Integrated_Tax_per = 0;
                    Central_Tax_per = Convert.ToDouble(Central_Tax.ToString());
                    State_Tax_per = Convert.ToDouble(State_Tax.ToString());
                    Integrated_Tax_per = Convert.ToDouble(Integrated_Tax.ToString());
                    if (dgMainDC.Rows.Count > 1)
                    {
                        double CentralTax = 0; double StateTax = 0; double IntegratedTax = 0;
                        CentralTax = Convert.ToDouble((Central_Tax_per * dgMainDC.Rows.Count) / dgMainDC.Rows.Count);
                        StateTax = Convert.ToDouble((State_Tax_per * dgMainDC.Rows.Count) / dgMainDC.Rows.Count);
                        IntegratedTax = Convert.ToDouble((Integrated_Tax_per * dgMainDC.Rows.Count) / dgMainDC.Rows.Count);


                        if ((CentralTax - Convert.ToDouble(txtCGSTPer.Text)) != 0)
                        {
                            txtCGSTPer.Text = "0";
                        }
                        if ((StateTax - Convert.ToDouble(txtSGSTPer.Text)) != 0)
                        {
                            txtSGSTPer.Text = "0";
                        }
                        if ((IntegratedTax - Convert.ToDouble(txtIGSTPer.Text)) != 0)
                        {
                            txtIGSTPer.Text = "0";
                        }
                    }
                }

            }
            #endregion Summary_Percentage

            ViewState["RowCount"] = 1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion btnInsert_Click

    #region Calcall
    protected void Calcall()
    {
        try
        {
            double TotAmount = 0;
            double AmrtAmt = 0;
            double TotCGSTAmount = 0;
            double TotSGSTAmount = 0;
            double TotIGSTAmount = 0;
            if (dgMainDC.Enabled)
            {
                for (int i = 0; i < dgMainDC.Rows.Count; i++)
                {
                    string Grid_Amount = ((Label)(dgMainDC.Rows[i].FindControl("lblAMT"))).Text;
                    //string Amort = ((Label)(dgMainDC.Rows[i].FindControl("lblAmortAmount"))).Text;
                    string Grid_CGST_Amount = ((Label)(dgMainDC.Rows[i].FindControl("lblCGSTAmt"))).Text;
                    string Grid_SGST_Amount = ((Label)(dgMainDC.Rows[i].FindControl("lblSGSTAmt"))).Text;
                    string Grid_IGST_Amount = ((Label)(dgMainDC.Rows[i].FindControl("lblIGSTAmt"))).Text;
                    string Amrtamt = ((Label)(dgMainDC.Rows[i].FindControl("lblAAmount"))).Text;
                    double Amount = Convert.ToDouble(Grid_Amount);
                    TotAmount = TotAmount + Amount;
                    TotCGSTAmount = TotCGSTAmount + Convert.ToDouble(Grid_CGST_Amount);
                    TotSGSTAmount = TotSGSTAmount + Convert.ToDouble(Grid_SGST_Amount);
                    TotIGSTAmount = TotIGSTAmount + Convert.ToDouble(Grid_IGST_Amount);
                    if (Amrtamt == "")
                    {
                        Amrtamt = "0.00";
                    }
                    AmrtAmt = Convert.ToDouble(Amrtamt) + AmrtAmt;
                }
            }
            txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(TotAmount, 2));
            double CGST = 0; double SGST = 0; double IGST = 0;
            CGST = Convert.ToDouble(Math.Round(TotCGSTAmount, 0, MidpointRounding.AwayFromZero));
            SGST = Convert.ToDouble(Math.Round(TotSGSTAmount, 0, MidpointRounding.AwayFromZero));
            IGST = Convert.ToDouble(Math.Round(TotIGSTAmount, 0, MidpointRounding.AwayFromZero));
            txtCGSTAmt.Text = CGST.ToString();
            txtSGSTAmt.Text = SGST.ToString();
            txtIGSTAmt.Text = IGST.ToString();
            txtAmortTotalAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(AmrtAmt), 2));

            #region Check_Empty_textbox
            if (txtBasicAmount.Text == "")
            {
                txtBasicAmount.Text = "0.00";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0.00";
            }
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0.00";
            }
            if (txtCGSTPer.Text == "")
            {
                txtCGSTPer.Text = "0.00";
            }
            if (txtCGSTAmt.Text == "")
            {
                txtCGSTAmt.Text = "0.00";
            }
            if (txtSGSTPer.Text == "")
            {
                txtSGSTPer.Text = "0.00";
            }
            if (txtSGSTAmt.Text == "")
            {
                txtSGSTAmt.Text = "0.00";
            }
            if (txtIGSTPer.Text == "")
            {
                txtIGSTPer.Text = "0.00";
            }
            if (txtIGSTAmt.Text == "")
            {
                txtIGSTAmt.Text = "0.00";
            }
            if (txtAmortTotalAmt.Text == "")
            {
                txtAmortTotalAmt.Text = "0.00";
            }
            if (txtFrieghtAmt.Text == "")
            {
                txtFrieghtAmt.Text = "0.00";
            }
            if (txtInsuranceAmt.Text == "")
            {
                txtInsuranceAmt.Text = "0.00";
            }
            if (txtOtherAmt.Text == "")
            {
                txtOtherAmt.Text = "0.00";
            }
            if (txtPackingAmt.Text == "")
            {
                txtPackingAmt.Text = "0.00";
            }

            #endregion Check_Empty_textbox

            #region Gst_Calc_Comment
            //double CGSTAmt = (Convert.ToDouble(txtBasicAmount.Text) * (Convert.ToDouble(txtCGSTPer.Text) / 100));
            //txtCGSTAmt.Text = Math.Round(CGSTAmt, 0).ToString();
            //txtCGSTAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtCGSTAmt.Text), 0));
            //double SGSTAmt = (Convert.ToDouble(txtBasicAmount.Text) * (Convert.ToDouble(txtSGSTPer.Text) / 100));
            //txtSGSTAmt.Text = Math.Round(SGSTAmt, 0).ToString();
            //txtSGSTAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSGSTAmt.Text), 0));
            //double IGSTAmt = (Convert.ToDouble(txtBasicAmount.Text) * (Convert.ToDouble(txtIGSTPer.Text) / 100));
            //txtIGSTAmt.Text = Math.Round(IGSTAmt, 0).ToString();
            //txtIGSTAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtIGSTAmt.Text), 0));
            #endregion Gst_Calc



            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
            Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
            + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));

        }
        catch (Exception Ex)
        {
            throw Ex;

        }
    }
    #endregion Calcall

    #region ClearControles
    protected void ClearControles()
    {
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        //ddlPONo.SelectedIndex = 0;
        ddlUnit.SelectedIndex = 0;
        txtQuantity.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtRemark.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        str = "";
        ViewState["RowCount"] = 0;
        ViewState["ItemUpdateIndex"] = "-1";
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                // Bind Unit 
                BindUnit();

                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL FROM ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtPO = new DataTable();
                //if (Request.QueryString[0].Equals("MODIFY"))
                //{
                //    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0   and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "  AND CPOD_STATUS=0   and CPOM_IS_VERBAL=0");
                //}
                //else
                //{
                //    //dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0 ) AND CPOD_STATUS=0   and CPOM_IS_VERBAL=0");
                //    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ES_DELETE=0  AND CPOD_STATUS=0   and CPOM_IS_VERBAL=0");
                //}
                //ddlPONo.DataSource = dtPO;
                //ddlPONo.DataTextField = "CPOM_PONO";
                //ddlPONo.DataValueField = "CPOM_CODE";
                //ddlPONo.DataBind();

                //ddlPONo_SelectedIndexChanged(null, null);

                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                DataTable dtHSN = new DataTable();
                //dtHSN = CommonClasses.Execute("SELECT DISTINCT I_E_CODE,E_TARIFF_NO,E_COMMODITY FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER ,EXCISE_TARIFF_MASTER WHERE CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0   AND I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND I_CODE='" + ddlItemCode.SelectedValue + "'");
                dtHSN = CommonClasses.Execute("SELECT DISTINCT I_E_CODE,E_TARIFF_NO,E_COMMODITY FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtHSN.Rows.Count > 0)
                {
                    txtHSNNumber.Text = dtHSN.Rows[0]["E_TARIFF_NO"].ToString();
                }
                else
                {
                    txtHSNNumber.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region BindUnit
    private void BindUnit()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlUnit.DataSource = dt1;
        ddlUnit.DataTextField = "I_UOM_NAME";
        ddlUnit.DataValueField = "I_UOM_CODE";
        ddlUnit.DataBind();
    }
    #endregion BindUnit

    #region ddlUnit_SelectedIndexChanged
    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion ddlUnit_SelectedIndexChanged

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                // Bind Unit 
                BindUnit();
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtPO = CommonClasses.Execute("select distinct(CPOM_CODE),CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and ES_DELETE=0 and CPOM_TYPE=-2147483648");
                //DataTable dtPO = new DataTable();
                //if (Request.QueryString[0].Equals("MODIFY"))
                //{
                //    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CPOD_STATUS=0    AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " --and CPOM_TYPE=-2147483648");
                //}
                //else
                //{
                //    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 AND CPOD_STATUS=0   and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0 ) --and CPOM_TYPE=-2147483648");
                //}
                //ddlPONo.DataSource = dtPO;
                //ddlPONo.DataTextField = "CPOM_PONO";
                //ddlPONo.DataValueField = "CPOM_CODE";
                //ddlPONo.DataBind();
                ////ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));
                //ddlPONo_SelectedIndexChanged(null, null);
                //ddlPONo.SelectedIndexChanged(null, null);

                DataTable DtRate = new DataTable();
                DataTable dtHSN = new DataTable();
                dtHSN = CommonClasses.Execute("SELECT DISTINCT I_E_CODE,E_TARIFF_NO,E_COMMODITY FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE  I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtHSN.Rows.Count > 0)
                {
                    txtHSNNumber.Text = dtHSN.Rows[0]["E_TARIFF_NO"].ToString();
                }
                else
                {
                    txtHSNNumber.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" DebitNote", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlPONo_SelectedIndexChanged
    //protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlItemCode.SelectedIndex != 0)
    //        {
    //            DataTable dt1 = CommonClasses.Execute("select CPOD_RATE,ISNULL(CPOD_MODNO,0) AS ModNo,ISNULL(CPOD_MODDATE,GetDate()) AS ModDate,ISNULL(CPOD_AMORTRATE,0)  AS CPOD_AMORTRATE  FROM CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
    //            DataTable dtQty = CommonClasses.Execute("SELECT (CPOD_ORD_QTY-CPOD_DISPACH) as Qty , ISNULL(CPOD_ORD_QTY,0) AS CPOD_ORD_QTY,ISNULL(CPOD_DISC_PER,0) AS CPOD_DISC_PER,	ISNULL(CPOD_DISC_AMT,0) AS CPOD_DISC_AMT  FROM CUSTPO_DETAIL where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + "  ");
    //            if (dt1.Rows.Count > 0)
    //            {
    //                txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_RATE"]);
    //                txtQuantity.Text = string.Format("{0:0.00}", dtQty.Rows[0]["CPOD_ORD_QTY"]);
    //                txtAmount.Text = (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtQuantity.Text)).ToString();
    //            }
    //            else
    //            {
    //                txtRate.Text = "0.00";
    //                txtQuantity.Text = "0.00";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("DebitNote", "LoadPO", Ex.Message);
    //    }
    //}
    #endregion ddlPONo_SelectedIndexChanged

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            #region Clear_Grid_Controls
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            dgMainDC.Visible = true;
            dgMainDC.Enabled = true;
            dgMainDC.DataSource = ((DataTable)ViewState["dt2"]);
            dgMainDC.DataBind();
            LoadFilter();
            txtRate.Text = "";
            txtAmount.Text = "";
            txtQuantity.Text = "";
            txtHSNNumber.Text = "";
            txtCGSTPer.Text = "";
            txtSGSTPer.Text = "";
            txtIGSTPer.Text = "";
            txtCGSTAmt.Text = "";
            txtSGSTAmt.Text = "";
            txtIGSTAmt.Text = "";
            txtBasicAmount.Text = "";
            txtNetAmount.Text = "";

            //txtRate.Text = "0.00";
            //txtAmount.Text = "0.00";
            //txtQuantity.Text = "0.00";
            //txtHSNNumber.Text = "";
            //txtCGSTPer.Text = "0.00";
            //txtSGSTPer.Text = "0.00";
            //txtIGSTPer.Text = "0.00";
            //txtCGSTAmt.Text = "0.00";
            //txtSGSTAmt.Text = "0.00";
            //txtIGSTAmt.Text = "0.00";
            //txtBasicAmount.Text = "0.00";
            //txtNetAmount.Text = "0.00";
            #endregion Clear_Grid_Controls

            LoadICode();
            LoadIName();
            LoadState();
            LoadCustomerAddress();
            dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_LBT_NO,P_NAME,ISNULL( P_LBT_IND,0) AS P_LBT_IND  FROM PARTY_MASTER where ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "' AND P_TYPE='1' AND P_ACTIVE_IND=1   AND P_CODE= '" + ddlCustomer.SelectedValue + "' ");
            if (dt.Rows.Count > 0)
            {
                // Check LBT Indicator Of LBT Applicable 
                if (dt.Rows[0]["P_LBT_IND"].ToString() == "True")
                {
                    txtGSTINNo.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    txtGSTINNo.Text = dt.Rows[0]["P_LBT_NO"].ToString();
                }
                else
                {
                    txtGSTINNo.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDFDF");
                    txtGSTINNo.Text = "";
                    //txtGstNo.Attributes.Add("onFocus", "DoFocus(this);"); 
                    //txtGstNo.Attributes.Add("readonly", "readonly");
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE CUSTPO_MASTER.ES_DELETE=0 AND CPOD_I_CODE=I_CODE AND  CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CPOM_IS_VERBAL=0  ORDER BY I_CODENO ASC");
            // Load All Items 
            dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER WHERE I_ACTIVE_IND='1' AND ES_DELETE='0'  ORDER BY I_CODENO ASC");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT distinct I_CODE,I_NAME FROM ITEM_MASTER  WHERE I_ACTIVE_IND='1' AND ES_DELETE='0' ORDER BY I_NAME ASC");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadState
    private void LoadState()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT DISTINCT SM_CODE,SM_NAME FROM PARTY_MASTER, STATE_MASTER  WHERE PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "'  and P_ACTIVE_IND=1  AND P_SM_CODE=SM_CODE AND STATE_MASTER.ES_DELETE='0' ");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "SM_NAME";
            ddlState.DataValueField = "SM_CODE";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("Select State", "0"));
            DataTable dt1 = CommonClasses.Execute("SELECT SM_CODE,SM_NAME FROM PARTY_MASTER, STATE_MASTER  WHERE PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "'  and P_ACTIVE_IND=1 AND P_CODE= '" + ddlCustomer.SelectedValue + "'  AND P_SM_CODE=SM_CODE AND STATE_MASTER.ES_DELETE='0' ");
            if (dt1.Rows.Count > 0)
            {
                ddlState.SelectedValue = dt1.Rows[0]["SM_CODE"].ToString();
            }
            ddlConsignee.SelectedValue = ddlCustomer.SelectedValue;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadState", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomerAddress
    private void LoadCustomerAddress()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_ADD1,P_NAME from PARTY_MASTER where  ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "'  AND P_ACTIVE_IND=1   AND P_CODE= '" + ddlCustomer.SelectedValue + "' ");
            if (dt.Rows.Count > 0)
            {
                txtAddress.Text = dt.Rows[0]["P_ADD1"].ToString();
            }
            else
            {
                txtAddress.Text = "";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadCustomerAddress", Ex.Message);
        }
    }
    #endregion LoadCustomerAddress

    #region dgMainDC_Deleting
    protected void dgMainDC_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainDC_RowCommand
    protected void dgMainDC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgMainDC.Rows[Convert.ToInt32(ViewState["Index"])];
            if (e.CommandName == "Delete")
            {
                ViewState["RowCount"] = 0;
                int rowindex = row.RowIndex;
                dgMainDC.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgMainDC.DataSource = ((DataTable)ViewState["dt2"]);
                dgMainDC.DataBind();

                if (dgMainDC.Rows.Count == 0)
                {
                    dgMainDC.Enabled = false;
                    Calcall();
                    LoadFilter();
                }
                else
                {
                    Calcall();
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ViewState["RowCount"] = 0;
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                //ddlItemCode_SelectedIndexChanged(null, null);
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUnitCode"))).Text;
                //ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;
                txtQuantity.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblQuantity"))).Text), 3));
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRate"))).Text), 2));
                txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAmt"))).Text), 2));
                txtRemark.Text = (((Label)(row.FindControl("lblRemark"))).Text);
                txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblDisc_Per"))).Text), 2));
                txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblDiscAmt"))).Text), 2));
                txtAmort.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblARate"))).Text), 2));
                txtAmortAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAAmount"))).Text), 2));
                txtQtyPack.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblQtyper"))).Text), 2));
                txtPacking.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblPacking"))).Text), 2));
                //LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                //lnkDelete.Enabled = false;
                // All delete Enable False within Gridview
                foreach (GridViewRow gvr in dgMainDC.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "dgMainDC_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region txtQuantity_OnTextChanged
    protected void txtQuantity_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlPONo.SelectedIndex == -1)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Select PO Number";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlItemCode.Focus();
            //    return;
            //}
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtQuantity.Text);
                txtQuantity.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "txtQuantity_OnTextChanged", Ex.Message);
        }
    }
    #endregion txtQuantity_OnTextChanged

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtQuantity.Text);

                txtQuantity.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
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
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            //      txtAmount.Text = amount.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtRate_TextChanged

    #region ddlState_SelectedIndexChanged
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //dt = CommonClasses.Execute("SELECT DISTINCT SM_CODE,SM_NAME FROM PARTY_MASTER,CUSTPO_MASTER, STATE_MASTER  WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_CM_COMP_ID= '" + (string)Session["CompanyId"] + "' AND P_TYPE='1' and P_ACTIVE_IND=1  AND P_SM_CODE=SM_CODE AND STATE_MASTER.ES_DELETE='0' AND P_SM_CODE= '" + ddlState.SelectedValue + "' ");
            //ddlState.DataSource = dt;
            //ddlState.DataTextField = "SM_NAME";
            //ddlState.DataValueField = "SM_CODE";
            //ddlState.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DebitNote", "LoadState", Ex.Message);
        }
    }
    #endregion ddlState_SelectedIndexChanged

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

    #region Validation
    public bool Validation()
    {
        bool flag = false;
        if (ddlCustomer.SelectedIndex == 0)
        {
            //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Customer Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlCustomer.Focus();
            return flag;

        }
        if (ddlItemCode.SelectedIndex == 0)
        {
            //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Item Code";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlItemCode.Focus();
            return flag;
        }
        if (ddlItemName.SelectedIndex == 0)
        {
            //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Item Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlItemName.Focus();
            return flag;
        }
        if (txtDebitDate.Text == "")
        {
            //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Date";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtDebitDate.Focus();
            return flag;
        }
        else
        {
            return flag;
        }
    }
    #endregion Validation

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
            CommonClasses.SendError("DebitNote", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #region txtDiscPer_TextChanged
    protected void txtDiscPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            #region Comment
            //string totalStr = DecimalMasking(txtRate.Text);
            //double DiscAmt = 0;
            //txtQuantity.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtQuantity.Text), 3));
            //txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            //txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            //txtDiscPer.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtDiscPer.Text), 2));
            //DiscAmt = Math.Round((Convert.ToDouble(txtAmount.Text.ToString()) * Convert.ToDouble(txtDiscPer.Text)) / 100, 0);
            //txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(DiscAmt), 2));  
            #endregion Comment
            string totalStr = DecimalMasking(txtRate.Text);
            string DiscPercentage = DecimalMasking(txtRate.Text);
            double DiscAmt = 0;
            Convert.ToDecimal(txtQuantity.Text = string.IsNullOrEmpty(txtQuantity.Text) ? "0" : string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtQuantity.Text), 3)));
            Convert.ToDecimal(txtAmount.Text = string.IsNullOrEmpty(txtAmount.Text) ? "0" : string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtAmount.Text), 3)));
            txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            txtDiscPer.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(DiscPercentage), 2));
            DiscAmt = Math.Round((Convert.ToDouble(txtAmount.Text.ToString()) * Convert.ToDouble(DiscPercentage)) / 100, 0);
            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(DiscAmt), 2));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtDiscPer_TextChanged

    #region txtAmort_TextChanged
    protected void txtAmort_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtAmort.Text);
            double DiscAmt = 0;
            txtQuantity.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtQuantity.Text), 3));
            txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtAmort.Text.ToString())), 2);

            DiscAmt = Math.Round((Convert.ToDouble(txtQuantity.Text.ToString()) * Convert.ToDouble(txtAmort.Text)), 0);

            txtAmortAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(DiscAmt), 2));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtAmort_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtAmort_TextChanged

    #region txtPackingPer_TextChanged
    protected void txtPackingPer_TextChanged(object sender, EventArgs e)
    {
        try
        {

            double DiscAmt = 0;

            DiscAmt = Math.Round((Convert.ToDouble(txtBasicAmount.Text.ToString()) * Convert.ToDouble(txtPackingPer.Text)) / 100, 0);
            txtPackingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(DiscAmt), 2));
            //txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(DiscAmt) + Convert.ToDouble(txtNetAmount.Text), 2));

            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
                Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
                + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtPackingPer_TextChanged

    #region txtFrieghtAmt_TextChanged
    protected void txtFrieghtAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {




            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
                Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
                + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtFrieghtAmt_TextChanged

    //#region txtInsuranceAmt_TextChanged
    //protected void txtInsuranceAmt_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {


    //        txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
    //            Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
    //            + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
    //    }
    //}
    //#endregion txtInsuranceAmt_TextChanged


    #region txtOtherAmt_TextChanged
    protected void txtOtherAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
                Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
                + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtOtherAmt_TextChanged
    #region txtInsuranceAmt_TextChanged
    protected void txtInsuranceAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {


            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicAmount.Text) + Convert.ToDouble(txtCGSTAmt.Text) + Convert.ToDouble(txtSGSTAmt.Text) +
                Convert.ToDouble(txtIGSTAmt.Text) + Convert.ToDouble(txtAmortTotalAmt.Text) + Convert.ToDouble(txtFrieghtAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text)
                + Convert.ToDouble(txtOtherAmt.Text) + Convert.ToDouble(txtPackingAmt.Text), 0));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtInsuranceAmt_TextChanged
}