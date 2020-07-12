using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Transactions_ADD_BillPassing : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    BillPassing_BL billPassing_BL = null;
    static int mlCode = 0;
    static string right = "";
    public static string str = "";
    public static string Type = "";
    static DataTable dt2 = new DataTable();
    DataTable dtBillPassing = new DataTable();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                ViewState["mlCode"] = mlCode;
                ViewState["right"] = right;
                ViewState["Type"] = Type;
                ViewState["dt2"] = dt2;
                ViewState["SuppType"] = 0;
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='41'");
                ViewState["right"] = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        billPassing_BL = new BillPassing_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        billPassing_BL = new BillPassing_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("MOD");
                        // EnabaleTextBoxes(MainPanel);
                        ddlSupplierName.Enabled = false;
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        //txtBillDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        //txtInvoiceDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtBillDate.Text = CommonClasses.GetCurrentTime().ToString("dd-MMM-yyyy");
                        txtInvoiceDate.Text = CommonClasses.GetCurrentTime().ToString("dd-MMM-yyyy");
                        LoadCombos();
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        LoadFilter();
                    }
                    txtBillDate.Attributes.Add("readonly", "readonly");
                    txtInvoiceDate.Attributes.Add("readonly", "readonly");

                    ddlSupplierName.Focus();
                    //rbLstIsExise.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Bill Passing", "PageLoad", ex.Message);
                }
            }
        }

    }
    #endregion

    #region Event

    #region GirdEvent
    protected void dgBillPassing_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBillPassing.PageIndex = e.NewPageIndex;
            LoadBill();
        }
        catch (Exception)
        {
        }

    }

    protected void dgBillPassing_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    #region ButtonEvent

    #region rbLstIsExise_SelectedIndexChanged
    protected void rbLstIsExise_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSupplierName.SelectedIndex != 0)
            {
                LoadBill();
            }
            else if (ddlSupplierName.SelectedIndex == 0)
            {
                dtFilter.Clear();
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_RATE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_TOTAL_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_DUTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_HCESS", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("EX_FREIGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_PACKING_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_OTHER_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_INSURANCE_AMT", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier PO Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                //CancelRecord();
                Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            if (CommonClasses.ValidRights(int.Parse(ViewState["right"].ToString().Substring(0, 1)), this, "For Save"))
            {
                int flag = 0;
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        flag++;
                    }
                }
                if (flag == 0)
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please, Select Item";
                    return;
                }
                if (Convert.ToDouble(txtGrandTotal.Text) <= 0)
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Warning);
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please, Select Item";
                    return;
                }
                if (txtInvoceNo.Text.Trim() == "")
                {
                    ShowMessage("#Avisos", "Please Enter Invoice No", CommonClasses.MSG_Warning);
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please, Select Item";
                    return;
                }
                if (SaveRec())
                {

                }
                ddlSupplierName.Focus();
            }
            else
            {
                ShowMessage("#Avisos", "You have no rights to Save", CommonClasses.MSG_Info);

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "You have no rights to Save";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "btnSubmit_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Cancel Record
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "CancelRecord", ex.Message);
        }
    }
    #endregion

    #region btnOK_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec();
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "btnOK_Click", ex.Message);
        }
    }

    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region Check Validation
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtBillDate.Text == "")
            {
                flag = false;
            }
            else if (ddlSupplierName.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtInvoceNo.Text == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "CheckValid", ex.Message);
        }
        return flag;
    }
    #endregion

    #endregion

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;
            Calculate(index);
            //txtAccesableValue.Focus();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlSupplierName_SelectedIndexChanged
    protected void ddlSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //lblmsg.Visible = false;
            //PanelMsg.Visible = false;
            if (ddlSupplierName.SelectedIndex != 0)
            {
                DataTable dtSuppType = CommonClasses.Execute("SELECT IWM_TYPE  FROM INWARD_MASTER where IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND ES_DELETE=0");
                if (dtSuppType.Rows.Count > 0)
                {

                    if (dtSuppType.Rows[0]["IWM_TYPE"].ToString() == "Without PO inward")
                    {
                        ViewState["SuppType"] = 1;
                    }
                }
                LoadBill();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier PO Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion


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

    public void grandtotal()
    {
       // txtTaxableAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtloadingAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
        //txtExciseAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtexcper.Text) / 100);
        //txtEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txteducessper.Text) / 100);
        //txtSHEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtsheper.Text) / 100);

       // txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtExciseAmount.Text) + Convert.ToDouble(txtEduCessAmt.Text) + Convert.ToDouble(txtSHEduCessAmt.Text) + Convert.ToDouble(txtRoundOff.Text));
        //txtExamt.Text = string.Format("{0:0.0000}", Convert.ToDouble(txtExciseAmount.Text));
        //txtEDU.Text = string.Format("{0:0.0000}", Convert.ToDouble(txtEduCessAmt.Text));
        //txtEDUC.Text = string.Format("{0:0.0000}", Convert.ToDouble(txtSHEduCessAmt.Text));
    }

    #region User Defined Method

    #region Enabale
    public static void EnabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = true;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = true;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = true;

                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = true;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        EnabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region Diabale
    public static void DiabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = false;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = false;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = false;

                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = false;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        DiabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {

            dt = CommonClasses.Execute("select  ISNULL(BPM_EX_YN,0) AS BPM_EX_YN,  CASE WHEN BPM_TYPE='OUTCUSTINV' then 'OUTCUSTINV' WHEN BPM_TYPE='WPO' then 'Without PO inward' WHEN BPM_TYPE='CUSTOMER-REJECTION' then 'CUSTOMER-REJECTION' else 'IWIM' END IWM_TYPE ,BPM_CODE,BPM_NO,CONVERT(varchar,BPM_DATE,106) as BPM_DATE, ISNULL(BPM_EX_TYPE,0)  AS BPM_EX_TYPE,BPM_P_CODE,P_NAME,BPM_IWM_CODE,BPM_INV_NO,CONVERT(varchar,BPM_INV_DATE,106) as BPM_INV_DATE,BPM_BILL_PASS_BY,BPM_BASIC_AMT, BPM_DISCOUNT_AMT,	BPM_PACKING_AMT,BPM_ACCESS_AMT ,BPM_EXCIES_AMT ,BPM_ECESS_AMT,BPM_HECESS_AMT ,BPM_EXCPER ,BPM_EXCEDCESS_PER ,BPM_EXCHIEDU_PER ,BPM_TAXABLE_AMT ,BPM_TAX_AMT ,BPM_TAX_PER ,BPM_TAX_CODE ,BPM_OTHER_AMT ,BPM_ADD_DUTY ,BPM_FREIGHT ,BPM_INSURRANCE ,BPM_TRANSPORT ,isnull(BPM_OCTRO_AMT,0) as BPM_OCTRO_AMT,isnull(BPM_ROUND_OFF,0) as BPM_ROUND_OFF ,BPM_G_AMT,BPM_IS_SERVICEIN   from BILL_PASSING_MASTER,PARTY_MASTER WHERE BPM_P_CODE=P_CODE and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["BPM_CODE"]);

                if (dt.Rows[0]["IWM_TYPE"].ToString() == "CUSTOMER-REJECTION")
                {
                    chkUseCustRej.Checked = true;
                    ViewState["Type"] = "CUSTOMER-REJECTION";
                }
                ChkUseService.Checked = Convert.ToBoolean(dt.Rows[0]["BPM_IS_SERVICEIN"]);
                // chkUseCustRej.Checked = Convert.ToBoolean(dt.Rows[0]["BPM_EX_YN"]);
                ChkUseService.Enabled = false;
                LoadCombos();
                txtBillNo.Text = Convert.ToInt32(dt.Rows[0]["BPM_NO"]).ToString();
                txtBillDate.Text = Convert.ToDateTime(dt.Rows[0]["BPM_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["BPM_INV_DATE"]).ToString("dd MMM yyyy");
                ddlSupplierName.SelectedValue = Convert.ToInt32(dt.Rows[0]["BPM_P_CODE"]).ToString();
                txtInvoceNo.Text = (dt.Rows[0]["BPM_INV_NO"]).ToString();
                rbLstIsExise.SelectedValue = dt.Rows[0]["BPM_EX_TYPE"].ToString();
                ViewState["SuppType"] = dt.Rows[0]["BPM_EX_TYPE"].ToString();

                txtBasicAmount.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_BASIC_AMT"]));
                txtDiscountAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_DISCOUNT_AMT"]));
                //txtNetAmt.Text = string.Format("{0:0.0000}",Convert.ToDouble(dt.Rows[0]["BPM_NET_AMT"]));
                txtPackingAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_PACKING_AMT"]));
                txtAccesableValue.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_ACCESS_AMT"]));

                txtExciseAmount.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_EXCIES_AMT"]));
                txtEduCessAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_ECESS_AMT"]));
                txtSHEduCessAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_HECESS_AMT"]));

                txtTaxableAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_TAXABLE_AMT"]));

                txtTaxPer.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_TAX_PER"]));
                txtsalestaxamt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_TAX_AMT"]));
                txtOtherCharges.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_OTHER_AMT"]));
                txtloadingAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_ADD_DUTY"]));

                txtFreightAmt1.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_FREIGHT"]));
                txtInsuranceAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_INSURRANCE"]));
                txtTransportAmt.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_TRANSPORT"]));

                txtOctroiAmt2.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_OCTRO_AMT"]));
                txtRoundOff.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_ROUND_OFF"]));
                txtGrandTotal.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_G_AMT"]));

                txtexcper.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_EXCPER"]));
                txteducessper.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_EXCEDCESS_PER"]));
                txtsheper.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["BPM_EXCHIEDU_PER"]));

                ddlSupplierName.Enabled = false;
                //dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*IWD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_T_CODE,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY  FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,SALES_TAX_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND SPOD_T_CODE = ST_CODE AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  and BPD_IWM_CODE=IWM_CODE and BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                // dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*IWD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE   AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  and BPD_IWM_CODE=IWM_CODE and BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                if (chkUseCustRej.Checked)
                {
                    dtBillPassing = CommonClasses.Execute("select distinct 'CUSTOMER-REJECTION' as IWM_TYPE,CR_CODE as IWM_CODE,CD_PO_CODE as IWD_CPOM_CODE,CPOM_PONO as SPOM_PO_NO,CR_GIN_NO as IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CR_CHALLAN_NO as IWM_CHALLAN_NO,convert(varchar,CR_CHALLAN_DATE,106) as IWM_CHAL_DATE,CD_I_CODE as IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,cast(CD_CHALLAN_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(CD_RECEIVED_QTY as numeric(10,3)) as IWD_REV_QTY,cast(CD_CHALLAN_QTY as numeric(10,3)) as IWD_CON_OK_QTY,0 As IWD_CON_REJ_QTY,0 as IWD_CON_SCRAP_QTY,CONVERT(decimal(10,2),CD_RATE) AS SPOD_RATE,CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))AS SPOD_TOTAL_AMT,  '0.00' as SPOD_DISC_AMT,CASE WHEN CM_STATE=P_SM_CODE then E_BASIC else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_BASIC*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 )else 0 END as EX_EX_DUTY,CASE WHEN CM_STATE=P_SM_CODE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_EDU_CESS*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE)) /100) ELSE 0 END as EX_EX_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),E_H_EDU*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 ) ELSE 0 END   AS EX_EX_HCESS,0 as SPOD_T_CODE,'' as ST_TAX_NAME,'' AS ST_SALES_TAX,'0.00' AS T_AMT,CPOM_CODE as SPOM_CODE,CR_CODE as IWM_CODE,'0.00' AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,'1' as SPOD_EXC_Y_N,0 as EX_FREIGHT,0 as EX_PACKING_AMT,0 as EX_OTHER_AMT,0 as EX_INSURANCE_AMT   from  CUSTPO_MASTER,CUSTPO_DETAIL,CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,EXCISE_TARIFF_MASTER ,PARTY_MASTER,COMPANY_MASTER where CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CPOM_CODE =CPOD_CPOM_CODE AND CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND  CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=CPOM_CODE  and BPD_SPOM_CODE=CD_PO_CODE AND CD_MODVAT_FLG = 1 AND  CUSTREJECTION_MASTER.ES_DELETE = 0 AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE AND  BPD_IWM_CODE=CR_CODE and BPM_CODE=BPD_BPM_CODE and CD_I_CODE=BPD_I_CODE  and BILL_PASSING_MASTER.ES_DELETE=0 and   BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + ViewState["mlCode"].ToString() + "'   ORDER BY CUSTREJECTION_MASTER.CR_GIN_NO");
                }
                else if (!ChkUseService.Checked)
                {
                    if (ViewState["SuppType"].ToString() == "0")
                    {
                        //dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,3)) as SPOD_RATE,CONVERT (decimal(20,4),((IWD_CH_QTY * ISNULL( IWM_CURR_RATE , 0)) * ROUND(IWD_RATE,3)))  AS SPOD_TOTAL_AMT,CAST(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT, CASE WHEN P_SM_CODE=CM_STATE then SPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,CASE WHEN P_SM_CODE=CM_STATE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER, CASE WHEN P_SM_CODE<>CM_STATE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS,BPM_FREIGHT as EX_FREIGHT,BPM_PACKING_AMT as EX_PACKING_AMT,BPM_OTHER_AMT as EX_OTHER_AMT,BPM_INSURRANCE as EX_INSURANCE_AMT  FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,COMPANY_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND BPD_I_CODE=I_CODE  AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND      BPD_IWM_CODE=IWM_CODE and BPM_CM_CODE=CM_CODE AND P_CODE=BPM_P_CODE AND  BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and  BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                        dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE, IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, CONVERT (decimal(20, 4), CASE when round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4)=0 then IWD_REV_QTY ELSE ( INWARD_DETAIL.IWD_REV_QTY *round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4)) END) AS    IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,3)) as SPOD_RATE,  CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY * ISNULL((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END), 0) * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT,CAST(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT, CASE WHEN P_LBT_IND=1 THEN CASE WHEN P_SM_CODE=CM_STATE then SPOD_EXC_PER ELSE 0 END ELSE 0 END  AS SPOD_EXC_PER,CASE WHEN P_LBT_IND=1 THEN CASE WHEN P_SM_CODE=CM_STATE then SPOD_EDU_CESS_PER else 0 END ELSE 0 END  AS SPOD_EDU_CESS_PER,CASE WHEN P_LBT_IND=1 THEN CASE WHEN P_SM_CODE<>CM_STATE then SPOD_H_EDU_CESS else 0 END   ELSE 0 END  AS SPOD_H_EDU_CESS  ,SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS,BPM_FREIGHT as EX_FREIGHT,BPM_PACKING_AMT as EX_PACKING_AMT,BPM_OTHER_AMT as EX_OTHER_AMT,BPM_INSURRANCE as EX_INSURANCE_AMT  FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,COMPANY_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND BPD_I_CODE=I_CODE  AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND      BPD_IWM_CODE=IWM_CODE and BPM_CM_CODE=CM_CODE AND P_CODE=BPM_P_CODE AND  BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and  BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                    }
                    else
                    {
                        dtBillPassing = CommonClasses.Execute(" select distinct IWM_TYPE AS IWM_TYPE, IWM_CODE,IWD_CPOM_CODE,0 AS SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,3)) as SPOD_RATE,CONVERT (decimal(20,4),((IWD_CH_QTY *   ROUND(IWD_RATE,3))))  AS SPOD_TOTAL_AMT,0 as SPOD_DISC_AMT,  CASE WHEN P_SM_CODE=CM_STATE then E_BASIC else 0 END  AS SPOD_EXC_PER,CASE WHEN P_SM_CODE=CM_STATE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER, CASE WHEN P_SM_CODE<>CM_STATE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,0 AS SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,0 AS SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS,BPM_FREIGHT as EX_FREIGHT,BPM_PACKING_AMT as EX_PACKING_AMT,BPM_OTHER_AMT as EX_OTHER_AMT,BPM_INSURRANCE as EX_INSURANCE_AMT  FROM   INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,COMPANY_MASTER,EXCISE_TARIFF_MASTER where  IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE  AND BPD_I_CODE=I_CODE    and BPD_SPOM_CODE=IWD_CPOM_CODE AND  IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND I_E_CODE=E_CODE AND      BPD_IWM_CODE=IWM_CODE and BPM_CM_CODE=CM_CODE AND P_CODE=BPM_P_CODE AND  BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and  BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "'  and BPD_BPM_CODE= '" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                    }
                }
                else
                {

                    dtBillPassing = CommonClasses.Execute("select distinct SIM_TYPE AS IWM_TYPE,  SIM_CODE AS IWM_CODE,SID_CPOM_CODE AS IWD_CPOM_CODE,SRPOM_PO_NO AS SPOM_PO_NO,SIM_NO AS IWM_NO,convert(varchar,SIM_DATE,106) as IWM_DATE,SIM_CHALLAN_NO AS IWM_CHALLAN_NO,convert(varchar,SIM_CHAL_DATE,106) as IWM_CHAL_DATE,SID_I_CODE AS IWD_I_CODE,S_NAME AS I_NAME,cast(SID_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(SID_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(SID_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,SID_CON_REJ_QTY AS IWD_CON_REJ_QTY,SID_CON_SCRAP_QTY AS IWD_CON_SCRAP_QTY,cast(SID_RATE as numeric(20,3)) as SPOD_RATE,cast(SID_CH_QTY*SID_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SRPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT, CASE WHEN P_SM_CODE=CM_STATE then SRPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,CASE WHEN P_SM_CODE=CM_STATE then SRPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER, CASE WHEN P_SM_CODE<>CM_STATE then SRPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,SRPOD_T_CODE AS SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SRPOD_EXC_Y_N AS SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS,BPM_FREIGHT as EX_FREIGHT,BPM_PACKING_AMT as EX_PACKING_AMT,BPM_OTHER_AMT as EX_OTHER_AMT,BPM_INSURRANCE as EX_INSURANCE_AMT  FROM   SERVICE_PO_MASTER ,SERVICE_PO_DETAILS,SERVICE_INWARD_MASTER,SERVICE_INWARD_DETAIL,SERVICE_TYPE_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,COMPANY_MASTER where SRPOD_SPOM_CODE = SRPOM_CODE AND SIM_CODE = SID_SIM_CODE AND SID_I_CODE = S_CODE AND BPD_SPOM_CODE=SRPOM_CODE  and BPD_SPOM_CODE=SID_CPOM_CODE AND SRPOD_I_CODE = S_CODE   AND SID_BILL_PASS_FLG = 1 AND SID_INSP_FLG = 1 AND  SERVICE_INWARD_MASTER.ES_DELETE = 0 AND      BPD_IWM_CODE=SIM_CODE and BPM_CM_CODE=CM_CODE AND P_CODE=BPM_P_CODE AND  BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and  BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY SERVICE_INWARD_MASTER.SIM_NO");

                }
                if (dtBillPassing.Rows.Count != 0)
                {
                    dgBillPassing.DataSource = dtBillPassing;
                    dgBillPassing.DataBind();
                    ViewState["dt2"] = dtBillPassing;
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        chkRow.Checked = true;
                    }
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));

            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
            if (Convert.ToDouble(txtsheper.Text) == 0)
            {
                txtsheper.Enabled = false;
                txtSHEduCessAmt.Enabled = false;
                txtexcper.Enabled = true;
                txtExciseAmount.Enabled = true;
                txteducessper.Enabled = true;
                txtEduCessAmt.Enabled = true;
            }
            if (Convert.ToDouble(txtexcper.Text) == 0)
            {
                txtsheper.Enabled = true;
                txtSHEduCessAmt.Enabled = true;
                txtexcper.Enabled = false;
                txtExciseAmount.Enabled = false;
                txteducessper.Enabled = false;
                txtEduCessAmt.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {

                txtBillNo.Text = billPassing_BL.BPM_NO.ToString();
                txtBillDate.Text = billPassing_BL.BPM_DATE.ToString("dd MMM yyyy");
                txtInvoiceDate.Text = billPassing_BL.BPM_INV_DATE.ToString("dd MMM yyyy");
                ddlSupplierName.SelectedValue = billPassing_BL.BPM_P_CODE.ToString();
                txtInvoceNo.Text = billPassing_BL.BPM_INV_NO.ToString();
                //ddlBillPass.SelectedItem.Text = billPassing_BL.BPM_BILL_PASS_BY.ToString();

                txtBasicAmount.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_BASIC_AMT);
                txtDiscountAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_DISCOUNT_AMT);
                txtPackingAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_PACKING_AMT);
                txtAccesableValue.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_ACCESS_AMT);
                //txtNetAmt.Text = string.Format("{0:0.0000}",billPassing_BL.BPM_NET_AMT);
                txtExciseAmount.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_EXCIES_AMT);
                txtEduCessAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_ECESS_AMT);
                txtSHEduCessAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_HECESS_AMT);
                txtTaxableAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_TAXABLE_AMT);
                txtsalestaxamt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_TAX_AMT);
                txtOtherCharges.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_OTHER_AMT);
                txtloadingAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_ADD_DUTY);
                txtFreightAmt1.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_FREIGHT);

                txtInsuranceAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_INSURRANCE);
                txtTransportAmt.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_TRANSPORT);
                //txtFreightAmt1.Text = Convert.ToDouble(dt.Rows[0]["BPM_FREIGHT"]).ToString();
                //txtOctroiAmt1.Text = string.Format("{0:0.0000}",billPassing_BL.BPM_OCTRO_PER);
                txtOctroiAmt2.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_OCTRO_AMT);
                txtRoundOff.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_ROUND_OFF);

                txtGrandTotal.Text = string.Format("{0:0.0000}", billPassing_BL.BPM_G_AMT);


                rbLstIsExise.SelectedValue = billPassing_BL.BPM_EX_TYPE.ToString();
            }

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            billPassing_BL.BPM_NO = Convert.ToInt32(txtBillNo.Text);
            billPassing_BL.BPM_DATE = Convert.ToDateTime(txtBillDate.Text);
            billPassing_BL.BPM_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text);
            billPassing_BL.BPM_P_CODE = Convert.ToInt32(ddlSupplierName.SelectedValue);
            billPassing_BL.BPM_INV_NO = txtInvoceNo.Text.ToString();
            billPassing_BL.BPM_BILL_PASS_BY = (string)Session["Username"];

            billPassing_BL.BPM_BASIC_AMT = Math.Round(Convert.ToDouble(txtBasicAmount.Text), 2);
            billPassing_BL.BPM_DISCOUNT_AMT = Math.Round(Convert.ToDouble(txtDiscountAmt.Text), 2);
            billPassing_BL.BPM_PACKING_AMT = Math.Round(Convert.ToDouble(txtPackingAmt.Text), 2);
            billPassing_BL.BPM_ACCESS_AMT = Math.Round(Convert.ToDouble(txtAccesableValue.Text), 2);
            // billPassing_BL.BPM_NET_AMT = float.Parse(txtNetAmt.Text);
            billPassing_BL.BPM_EXCIES_AMT = Math.Round(Convert.ToDouble(txtExciseAmount.Text), 2);
            billPassing_BL.BPM_ECESS_AMT = Math.Round(Convert.ToDouble(txtEduCessAmt.Text), 2);
            billPassing_BL.BPM_HECESS_AMT = Math.Round(Convert.ToDouble(txtSHEduCessAmt.Text), 2);
            //billPassing_BL.BPM_EXCPER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_EXC_PER"))).Text), 2);
            //billPassing_BL.BPM_EXCEDCESS_PER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_EDU_CESS_PER"))).Text), 2);
            //billPassing_BL.BPM_EXCHIEDU_PER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_H_EDU_CESS"))).Text), 2);


            billPassing_BL.BPM_EXCPER = Math.Round(Convert.ToDouble(txtexcper.Text), 2);
            billPassing_BL.BPM_EXCEDCESS_PER = Math.Round(Convert.ToDouble(txteducessper.Text), 2);
            billPassing_BL.BPM_EXCHIEDU_PER = Math.Round(Convert.ToDouble(txtsheper.Text), 2);

            billPassing_BL.BPM_TAXABLE_AMT = Math.Round(Convert.ToDouble((txtTaxableAmt.Text)), 2);
            //billPassing_BL.BPM_TAX_AMT = float.Parse(txtsalestaxamt.Text);
            billPassing_BL.BPM_TAX_AMT = Math.Round(Convert.ToDouble(txtsalestaxamt.Text), 2);
            billPassing_BL.BPM_TAX_PER = Math.Round(Convert.ToDouble(txtTaxPer.Text), 2);
            billPassing_BL.BPM_TAX_CODE = Convert.ToInt32(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_T_CODE"))).Text);
            billPassing_BL.BPM_OTHER_AMT = Math.Round(Convert.ToDouble(txtOtherCharges.Text), 2);
            billPassing_BL.BPM_ADD_DUTY = Math.Round(Convert.ToDouble(txtloadingAmt.Text), 2);
            billPassing_BL.BPM_FREIGHT = Math.Round(Convert.ToDouble(txtFreightAmt1.Text), 2);
            billPassing_BL.BPM_INSURRANCE = Math.Round(Convert.ToDouble(txtInsuranceAmt.Text), 2);
            billPassing_BL.BPM_TRANSPORT = Math.Round(Convert.ToDouble(txtTransportAmt.Text), 2);
            billPassing_BL.BPM_OCTRO_AMT = Math.Round(Convert.ToDouble(txtOctroiAmt2.Text), 2);
            billPassing_BL.BPM_ROUND_OFF = Math.Round(Convert.ToDouble(txtRoundOff.Text), 2);
            billPassing_BL.BPM_G_AMT = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
            billPassing_BL.BPM_CM_CODE = Convert.ToInt32(Session["CompanyCode"].ToString());

            //excise
            // billPassing_BL.BPM_EX_TYPE = rbLstIsExise.SelectedValue.ToString();
            billPassing_BL.BPM_EX_TYPE = ViewState["SuppType"].ToString();
            billPassing_BL.BPM_TYPE = ViewState["Type"].ToString();

            if (ChkUseService.Checked)
            {
                billPassing_BL.BPM_IS_SERVICEIN = 1;
            }
            else
            {
                billPassing_BL.BPM_IS_SERVICEIN = 0;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM BILL_PASSING_MASTER WHERE ES_DELETE=0 AND BPM_INV_NO='" + txtInvoceNo.Text.Trim() + "'  AND BPM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND BPM_CM_CODE=" + Session["CompanyCode"] + "");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No allready Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                billPassing_BL = new BillPassing_BL();
                txtBillNo.Text = Numbering();
                if (Setvalues())
                {
                    if (billPassing_BL.Save(dgBillPassing, "INSERT"))
                    {

                        string Code = CommonClasses.GetMaxId("Select Max(BPM_CODE) from BILL_PASSING_MASTER");
                        CommonClasses.WriteLog("Bill Passing", "Save", "Bill Passing", billPassing_BL.BPM_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
                    }
                    else
                    {
                        if (billPassing_BL.Msg != "")
                        {
                            ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            billPassing_BL.Msg = "";
                        }
                        ddlSupplierName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM BILL_PASSING_MASTER WHERE ES_DELETE=0 AND BPM_INV_NO='" + txtInvoceNo.Text + "'  AND BPM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND BPM_CM_CODE=" + Session["CompanyCode"] + " AND BPM_CODE !='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                billPassing_BL = new BillPassing_BL(Convert.ToInt32(ViewState["mlCode"]));

                if (Setvalues())
                {
                    if (billPassing_BL.Save(dgBillPassing, "UPDATE"))
                    {

                        CommonClasses.RemoveModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Bill Passing", "Update", "Bill Passing", billPassing_BL.BPM_NO.ToString(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
                    }
                    else
                    {
                        if (billPassing_BL.Msg != "")
                        {
                            ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            billPassing_BL.Msg = "";
                        }
                        ddlSupplierName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(BPM_NO) as BPM_NO from BILL_PASSING_MASTER  WHERE ES_DELETE=0 AND BPM_TYPE='" + ViewState["Type"].ToString() + "' ");
        if (dt.Rows[0]["BPM_NO"] == null || dt.Rows[0]["BPM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["BPM_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region ClearFunction
    public static void ClearTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Text = String.Empty;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    double BasicAmt = 0;
    double DiscountAmt = 0;
    double acceablevalue = 0;
    double ExcPer = 0;
    double ExcEduCessPer = 0;
    double ExcHighPer = 0;
    double ExcAmt = 0;
    double ExcEduCessAmt = 0;
    double ExcHighAmt = 0;
    double Taxper = 0;
    double TaxAmt = 0;
    double TaxableAmt = 0, OtherAmt = 0, AddAmt = 0, FrightAmt = 0, InsurranceAmt = 0, TransportAmt = 0, OctriPer = 0;
    double OctriAmt = 0, RoundOff = 0, GrandAmt = 0, packagingAmt = 0;

    #region Calucalte
    void Calculate(int index)
    {
        try
        {
            string totalStr = "";
            if (index != -1)
            {

                bool flag = false; int count = 0;
                string str1 = "";
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);


                        ExcAmt = ExcAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEX_EX_DUTY"))).Text);
                        ExcEduCessAmt = ExcEduCessAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEX_EX_CESS"))).Text);
                        ExcHighAmt = ExcHighAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEX_EX_HCESS"))).Text);

                        string type = ((Label)(dgBillPassing.Rows[i].FindControl("lblIWM_TYPE"))).Text;
                        if (type == "OUTCUSTINV")
                        {
                            ViewState["Type"] = "OUTCUSTINV";
                        }
                        else if (type == "SIWM")
                        {
                            ViewState["Type"] = "SIWM";
                        }
                        else if (type == "Without PO inward")
                        {
                            ViewState["Type"] = "WPO";
                            ViewState["SuppType"] = 1;
                        }
                        else if (type.Trim() == "CUSTOMER-REJECTION")
                        {
                            ViewState["Type"] = "CUSTOMER-REJECTION";
                        }
                        else
                        {
                            ViewState["Type"] = "IWIM";
                        }


                        if (count == 0 && flag == false)
                        {
                            count = 1;
                            flag = true;
                        }
                    }
                    if (flag == true && count == 1)
                    {
                        count = 0;
                        str1 = (((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_Y_N"))).Text);
                        if (str1 == "True")
                        {
                            ExcPer = Convert.ToDouble(txtexcper.Text);
                            ExcEduCessPer = Convert.ToDouble(txteducessper.Text);
                            ExcHighPer = Convert.ToDouble(txtsheper.Text);
                            txtexcper.ReadOnly = false;
                            txteducessper.ReadOnly = false;
                            txtsheper.ReadOnly = false;
                            ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                            ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                            ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                        }
                        else
                        {
                            txtexcper.ReadOnly = true;
                            txteducessper.ReadOnly = true;
                            txtsheper.ReadOnly = true;
                            ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                            ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                            ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                        }
                        //Taxper = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblT_SALESTAX"))).Text);
                    }
                }
                if (BasicAmt > 0)
                {

                    txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(BasicAmt, 4));
                    acceablevalue = BasicAmt - Convert.ToDouble(txtDiscountAmt.Text) + Convert.ToDouble(txtPackingAmt.Text);
                    // 
                    txtAccesableValue.Text = string.Format("{0:0.00}", acceablevalue);


                    OtherAmt = Convert.ToDouble(txtOtherCharges.Text);


                    AddAmt = Convert.ToDouble(txtloadingAmt.Text);


                    FrightAmt = Convert.ToDouble(txtFreightAmt1.Text);


                    packagingAmt = Convert.ToDouble(txtPackingAmt.Text);

                    TransportAmt = Convert.ToDouble(txtTransportAmt.Text);

                    InsurranceAmt = Convert.ToDouble(txtInsuranceAmt.Text);


                    OctriAmt = Convert.ToDouble(txtOctroiAmt2.Text);

                    TaxableAmt = acceablevalue + AddAmt + FrightAmt + InsurranceAmt + TransportAmt + OctriAmt + OtherAmt;
                    txtTaxableAmt.Text = string.Format("{0:0.00}", TaxableAmt);


                    if (rbLstIsExise.SelectedValue == "1")
                    {

                        txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                        txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                        txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);
                        txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                        txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                        txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    }
                    else
                    {

                        txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                        txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                        txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);

                        ExcAmt = Math.Round((TaxableAmt * ExcPer / 100), 2);
                        ExcEduCessAmt = Math.Round((TaxableAmt * ExcEduCessPer / 100), 2);
                        ExcHighAmt = Math.Round((TaxableAmt * ExcHighPer / 100), 2);
                        txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                        txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                        txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    }
                    if (ExcHighPer == 0)
                    {
                        txtsheper.Enabled = false;
                        txtSHEduCessAmt.Enabled = false;
                        txtexcper.Enabled = true;
                        txtExciseAmount.Enabled = true;
                        txteducessper.Enabled = true;
                        txtEduCessAmt.Enabled = true;
                    }

                    if (ExcPer == 0)
                    {
                        txtsheper.Enabled = true;
                        txtSHEduCessAmt.Enabled = true;

                        txtexcper.Enabled = false;
                        txtExciseAmount.Enabled = false;

                        txteducessper.Enabled = false;
                        txtEduCessAmt.Enabled = false;
                    }


                    if (txtsheper.Text.Trim() == "0")
                    {
                        txtsheper.Text = "0.00";
                    }
                    if (txtexcper.Text.Trim() == "0")
                    {
                        txtexcper.Text = "0.00";
                    }
                    if (txteducessper.Text.Trim() == "0")
                    {
                        txteducessper.Text = "0.00";
                    }
                    //txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    //txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                    //txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                    //if (Convert.ToDouble(txtsheper.Text) != 0)
                    //{
                    //    ExcHighAmt = (TaxableAmt * Convert.ToDouble(txtsheper.Text) / 100);
                    //    txtSHEduCessAmt.Text = string.Format("{0:0.0000}", ExcHighAmt);
                    //}
                    //else
                    //{
                    //    ExcAmt = (TaxableAmt * Convert.ToDouble(txtexcper.Text) / 100);
                    //    txtExciseAmount.Text = string.Format("{0:0.0000}", ExcAmt);

                    //    ExcEduCessAmt = (TaxableAmt * Convert.ToDouble(txteducessper.Text) / 100);
                    //    txtEduCessAmt.Text = string.Format("{0:0.0000}", ExcEduCessAmt);
                    //}
                    RoundOff = Convert.ToDouble(txtRoundOff.Text);

                    GrandAmt = Math.Round(TaxableAmt + ExcAmt + ExcEduCessAmt + ExcHighAmt + RoundOff, 2);
                    txtGrandTotal.Text = string.Format("{0:0.00}", GrandAmt);
                }
                else
                {
                    txtBasicAmount.Text = "0.00";
                    txtDiscountAmt.Text = "0.00";
                    txtPackingAmt.Text = "0.00";
                    txtAccesableValue.Text = "0.00";
                    txtExciseAmount.Text = "0.00";
                    txtEduCessAmt.Text = "0.00";
                    txtSHEduCessAmt.Text = "0.00";
                    txtTaxableAmt.Text = "0.00";
                    txtsalestaxamt.Text = "0.00";
                    txtTaxPer.Text = "0.00";
                    txtOtherCharges.Text = "0.00";
                    txtloadingAmt.Text = "0.00";
                    txtFreightAmt1.Text = "0.00";
                    txtInsuranceAmt.Text = "0.00";
                    txtOctroiAmt2.Text = "0.00";
                    txtRoundOff.Text = "0.00";
                    txtGrandTotal.Text = "0.00";
                    txtexcper.Text = "0.00";
                    txteducessper.Text = "0.00";
                    txtsheper.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "Calculate", ex.Message.ToString());
        }
    }
    #endregion

    #region txtPackingAmt_TextChanged
    protected void txtPackingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                string totalStr = DecimalMasking(txtPackingAmt.Text);
                txtPackingAmt.Text = string.Format("{0:0.0000}", Math.Round(Convert.ToDouble(totalStr), 4));
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtPackingAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt_TextChanged
    protected void txtDiscountAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtOtherCharges_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtloadingAmt_TextChanged
    protected void txtloadingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtloadingAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtFreightAmt1_TextChanged
    protected void txtFreightAmt1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtInsuranceAmt_TextChanged
    protected void txtInsuranceAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtInsuranceAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtTransportAmt_TextChanged
    protected void txtTransportAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtTransportAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtOctroiAmt2_TextChanged
    protected void txtOctroiAmt2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtOctroiAmt2_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtRoundOff_TextChanged
    protected void txtRoundOff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtRoundOff_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt1_TextChanged
    protected void txtDiscountAmt1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt1_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt2_TextChanged
    protected void txtDiscountAmt2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt2_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    //#region txtOctroiAmt1_TextChanged
    //protected void txtOctroiAmt1_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Calculate(-1);

    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Bill Passing", "txtOctroiAmt1_TextChanged", Ex.Message.ToString());
    //    }
    //}
    //#endregion


    #region LoadBill
    private void LoadBill()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                if (ddlSupplierName.SelectedIndex != 0)
                {
                    if (rbLstIsExise.SelectedIndex == 1)
                    {
                        dt.Rows.Clear();
                        // dt = CommonClasses.Execute("SELECT distinct  IWM_TYPE AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO, CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT (decimal(20,4),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20,4),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT (decimal(20,4),ROUND(IWD_RATE,2)) AS IWD_RATE , CONVERT (decimal(20,4),(IWD_REV_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT (decimal(20,4),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,SPOD_EXC_PER,0 AS EX_EX_DUTY,SPOD_EDU_CESS_PER,0 AS EX_EX_CESS, SPOD_H_EDU_CESS ,0 AS EX_EX_HCESS,SPOD_T_CODE,ST_ALIAS AS T_SHORT,  ST_SALES_TAX AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT (decimal(20,4),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CON_OK_QTY,2) AS SPOD_TOTAL_AMT,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N   FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,SALES_TAX_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND SPOD_T_CODE = ST_CODE AND IWD_BILL_PASS_FLG = 0 AND IWD_INSP_FLG = 1   AND INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY INWARD_MASTER.IWM_NO");
                        if (!ChkUseService.Checked)
                        {
                            dt = CommonClasses.Execute("SELECT distinct  IWM_TYPE AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO, CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT (decimal(20,4),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20,4),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT (decimal(20,4),ROUND(IWD_RATE,4)) AS IWD_RATE , CONVERT (decimal(20,4),(( IWD_CH_QTY * ISNULL( IWM_CURR_RATE , 0)) * ROUND(IWD_RATE,4)))  AS SPOD_TOTAL_AMT ,CONVERT (decimal(20,4),SPOD_DISC_AMT) AS SPOD_DISC_AMT , CASE WHEN P_SM_CODE=CM_STATE then SPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,0 AS EX_EX_DUTY,CASE WHEN P_SM_CODE=CM_STATE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER,0 AS EX_EX_CESS, CASE WHEN P_SM_CODE<>CM_STATE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,0 AS EX_EX_HCESS,SPOD_T_CODE,'' AS T_SHORT,  '' AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT (decimal(20,4),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE, ROUND(IWD_CH_QTY,4) AS IWD_CH_QTY,ROUND(IWD_RATE,4) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,4)*IWD_CH_QTY,4) AS SPOD_TOTAL_AMT,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N    ,P_SM_CODE,CM_STATE,EX_FREIGHT,EX_PACKING_AMT,EX_OTHER_AMT ,EX_INSURANCE_AMT FROM EXICSE_ENTRY, EXCISE_DETAIL,SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER,COMPANY_MASTER where EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = IWM_CODE AND SPOD_SPOM_CODE = SPOM_CODE AND ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE  AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND IWD_BILL_PASS_FLG = 0 AND IWD_INSP_FLG = 1 AND INWARD_MASTER.ES_DELETE = 0 AND P_CODE = SPOM_P_CODE AND IWM_CM_CODE = CM_CODE  AND IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "' and EXICSE_ENTRY.ES_DELETE=0 ORDER BY INWARD_MASTER.IWM_NO");
                        }
                        else
                        {
                            dt = CommonClasses.Execute("SELECT distinct  SIM_TYPE AS IWM_TYPE  ,SRPOM_PONO AS SPOM_PO_NO,SIM_NO AS IWM_NO,CONVERT(VARCHAR,SIM_DATE,106)  AS IWM_DATE,SIM_CHALLAN_NO AS IWM_CHALLAN_NO, CONVERT(VARCHAR,SIM_CHAL_DATE,106)  AS IWM_CHAL_DATE,SID_I_CODE AS IWD_I_CODE,S_CODENO AS I_CODENO,S_NAME AS I_NAME,'' AS I_UOM_NAME,CONVERT (decimal(20,4),SID_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20,4),SID_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT (decimal(20,4),ROUND(SID_RATE,4)) AS IWD_RATE , CONVERT (decimal(20,4),(SID_CH_QTY * ROUND(SID_RATE,4)))AS SPOD_TOTAL_AMT,CONVERT (decimal(20,4),SRPOD_DISC_AMT) AS SPOD_DISC_AMT , CASE WHEN P_SM_CODE=CM_STATE then SRPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,0 AS EX_EX_DUTY,CASE WHEN P_SM_CODE=CM_STATE then SRPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER,0 AS EX_EX_CESS, CASE WHEN P_SM_CODE<>CM_STATE then SRPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,0 AS EX_EX_HCESS,SRPOD_T_CODE AS SPOD_T_CODE,'' AS T_SHORT,  '' AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SRPOM_CODE AS SPOM_CODE,SIM_CODE AS IWM_CODE,CONVERT (decimal(20,4),SRPOD_GP_RATE) AS SPOD_GP_RATE,SID_CPOM_CODE AS IWD_CPOM_CODE, ROUND(SID_CH_QTY,4) AS IWD_CH_QTY,ROUND(SID_RATE,4) AS SPOD_RATE,ROUND(ROUND(SID_RATE,4)*SID_CH_QTY,4) AS SPOD_TOTAL_AMT,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SRPOD_EXC_Y_N AS SPOD_EXC_Y_N    ,P_SM_CODE,CM_STATE,EX_FREIGHT,EX_PACKING_AMT,EX_OTHER_AMT FROM  EXICSE_ENTRY, EXCISE_DETAIL, SERVICE_PO_MASTER ,SERVICE_PO_DETAILS,SERVICE_INWARD_MASTER,SERVICE_INWARD_DETAIL,SERVICE_TYPE_MASTER,PARTY_MASTER,COMPANY_MASTER where EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = SIM_CODE AND  SRPOD_SPOM_CODE = SRPOM_CODE AND SIM_CODE = SID_SIM_CODE AND SID_I_CODE = S_CODE AND   SRPOM_CODE = SID_CPOM_CODE AND SRPOD_I_CODE = S_CODE AND SID_BILL_PASS_FLG = 0 AND SID_INSP_FLG = 1   AND SERVICE_INWARD_MASTER.ES_DELETE = 0 AND P_CODE=SRPOM_P_CODE AND SIM_CM_CODE=CM_CODE  AND    SIM_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY SERVICE_INWARD_MASTER.SIM_NO");
                        }
                        txtBasicAmount.Enabled = true; txtDiscountAmt.Enabled = true; txtPackingAmt.Enabled = true; txtAccesableValue.Enabled = true;
                        txtExciseAmount.Enabled = true; txtEduCessAmt.Enabled = true; txtSHEduCessAmt.Enabled = true; txtTaxableAmt.Enabled = true;
                        txtsalestaxamt.Enabled = true; txtTaxPer.Enabled = true; txtOtherCharges.Enabled = true; txtloadingAmt.Enabled = true;
                        txtFreightAmt1.Enabled = true; txtInsuranceAmt.Enabled = true;
                        txtOctroiAmt2.Enabled = true;
                        txtTransportAmt.Enabled = true;
                        txtGrandTotal.Enabled = true;
                        txtexcper.Enabled = true;
                        txteducessper.Enabled = true;
                        txtsheper.Enabled = true;
                    }
                    else
                    {
                        dt.Rows.Clear();


                        if (chkUseCustRej.Checked)
                        {
                            // dt = CommonClasses.Execute("SELECT CD_PO_CODE as IWD_CPOM_CODE,'CUSTOMER-REJECTION' AS IWM_TYPE , CPOM_PONO as SPOM_PO_NO,CR_GIN_NO as IWM_NO,CONVERT(VARCHAR,CR_GIN_DATE,106) AS IWM_DATE,CR_CHALLAN_NO as IWM_CHALLAN_NO,CONVERT(VARCHAR,CR_CHALLAN_DATE,106) AS  IWM_CHAL_DATE,CD_I_CODE as IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CH_QTY,CONVERT(decimal(10,3),CD_RECEIVED_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),CD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))AS NET_AMT,'0.00' AS SPOD_DISC_AMT , CASE WHEN CM_STATE=P_SM_CODE then E_BASIC else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_BASIC*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 )else 0 END as EXC_AMT,CASE WHEN CM_STATE=P_SM_CODE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_EDU_CESS*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE)) /100) ELSE 0 END as EDU_AMT ,CASE WHEN CM_STATE!=P_SM_CODE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),E_H_EDU*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 ) ELSE 0 END   AS HEDU_AMT,CPOD_ST_CODE as SPOD_T_CODE,' ' as ST_TAX_NAME, '' AS ST_SALES_TAX,'0.00' AS T_AMT,CPOM_CODE as SPOM_CODE,CR_CODE as IWM_CODE,'0.00' AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,'1' as SPOD_EXC_Y_N From CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL, CUSTPO_MASTER, CUSTPO_DETAIL, ITEM_MASTER, ITEM_UNIT_MASTER, EXCISE_TARIFF_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND CPOM_CODE =CPOD_CPOM_CODE AND CPOM_CODE = CD_PO_CODE AND CD_I_CODE = I_CODE AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE    AND CD_MODVAT_FLG=0 AND CUSTREJECTION_MASTER.ES_DELETE = 0 AND CR_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyID"]) + "' AND CR_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY  CPOM_PONO,IWM_NO");
                            dt = CommonClasses.Execute(" SELECT CD_PO_CODE as IWD_CPOM_CODE,'CUSTOMER-REJECTION' AS IWM_TYPE , CPOM_PONO as SPOM_PO_NO,CR_GIN_NO as IWM_NO,CONVERT(VARCHAR,CR_GIN_DATE,106) AS IWM_DATE,CR_CHALLAN_NO as IWM_CHALLAN_NO,CONVERT(VARCHAR,CR_CHALLAN_DATE,106) AS  IWM_CHAL_DATE,CD_I_CODE as IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CH_QTY,CONVERT(decimal(10,3),CD_RECEIVED_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),CD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CD_RATE))AS SPOD_TOTAL_AMT,'0.00' AS SPOD_DISC_AMT , CASE WHEN CM_STATE=P_SM_CODE then E_BASIC else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_BASIC*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CD_RATE))/100 )else 0 END as EX_EX_DUTY,CASE WHEN CM_STATE=P_SM_CODE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_EDU_CESS*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CD_RATE)) /100) ELSE 0 END as EX_EX_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),E_H_EDU*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CD_RATE))/100 ) ELSE 0 END   AS EX_EX_HCESS,CPOD_ST_CODE as SPOD_T_CODE,' ' as T_SHORT, '' AS ST_SALES_TAX,'' AS T_SALESTAX,'' AS SALESTAX_AMT,'0.00' AS T_AMT,CPOM_CODE as SPOM_CODE,CR_CODE as IWM_CODE,'0.00' AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,'1' as SPOD_EXC_Y_N ,CONVERT(decimal(10,2),CD_RATE)  AS SPOD_RATE,'' As SPOD_TOTAL_AMT, ' 'AS	ST_TAX_NAME, '' As	EX_FREIGHT	,'' AS EX_PACKING_AMT	,'' AS EX_OTHER_AMT,'' as	EX_INSURANCE_AMT	From CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL, CUSTPO_MASTER, CUSTPO_DETAIL, ITEM_MASTER, ITEM_UNIT_MASTER, EXCISE_TARIFF_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND CPOM_CODE =CPOD_CPOM_CODE AND CPOM_CODE = CD_PO_CODE AND CD_I_CODE = I_CODE AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE    AND CD_MODVAT_FLG=0 AND CUSTREJECTION_MASTER.ES_DELETE = 0 AND   CR_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyID"]) + "' AND CR_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY  CPOM_PONO,IWM_NO");

                        }
                        else if (ChkUseService.Checked)
                        {
                            dt = CommonClasses.Execute("SELECT  distinct SIM_TYPE  AS IWM_TYPE  ,SRPOM_PONO AS SPOM_PO_NO,SIM_NO as IWM_NO,CONVERT(VARCHAR,SIM_DATE,106)  AS IWM_DATE,SIM_CHALLAN_NO as IWM_CHALLAN_NO,CONVERT(VARCHAR,SIM_CHAL_DATE,106)  AS IWM_CHAL_DATE,SID_I_CODE AS IWD_I_CODE,S_CODENO AS I_CODENO,S_NAME AS I_NAME,'' AS I_UOM_NAME,CONVERT (decimal(20,4),SID_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20,4),SID_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT (decimal(20,4),SID_RATE) AS IWD_RATE , CONVERT (decimal(20,4),(SID_CH_QTY * ROUND(SID_RATE,3)))AS SPOD_TOTAL_AMT,CONVERT (decimal(20,4),SRPOD_DISC_AMT) AS SPOD_DISC_AMT ,EXD_EXC_PER AS  SPOD_EXC_PER,EX_EX_DUTY,EXD_EDU_PER AS SPOD_EDU_CESS_PER,EX_EX_CESS, EXD_HSEDU_PER AS SPOD_H_EDU_CESS ,EX_EX_HCESS,SRPOD_T_CODE AS SPOD_T_CODE,'' AS T_SHORT, '' AS ST_SALES_TAX ,''AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SRPOM_CODE AS SPOM_CODE,SIM_CODE AS IWM_CODE,CONVERT (decimal(20,4),SRPOD_GP_RATE) AS SPOD_GP_RATE,SID_CPOM_CODE AS IWD_CPOM_CODE,ROUND(SID_CH_QTY,4) AS IWD_CH_QTY,ROUND(SID_RATE,4) AS SPOD_RATE,ROUND(ROUND(SID_RATE,4)*SID_CH_QTY,4) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME,'' as ST_SALES_TAX,SRPOD_EXC_Y_N AS SPOD_EXC_Y_N,EX_FREIGHT ,EX_PACKING_AMT,EX_OTHER_AMT,EX_INSURANCE_AMT  From EXICSE_ENTRY, EXCISE_DETAIL, SERVICE_INWARD_MASTER, SERVICE_INWARD_DETAIL, SERVICE_PO_MASTER, SERVICE_PO_DETAILS,SERVICE_TYPE_MASTER WHERE EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = SIM_CODE AND SIM_CODE = SID_SIM_CODE AND SRPOM_CODE = SID_CPOM_CODE AND SRPOM_CODE = SRPOD_SPOM_CODE  AND SID_I_CODE = S_CODE    AND SID_BILL_PASS_FLG=0 AND SID_INSP_FLG = 1 AND (SIM_TYPE='SIWM') AND SIM_INWARD_TYPE IN(0,1) AND SERVICE_INWARD_MASTER.ES_DELETE = 0 AND SID_MODVAT_FLG = 1 AND EXICSE_ENTRY.ES_DELETE = 0  AND EXD_I_CODE= SID_I_CODE   AND SIM_P_CODE='" + ddlSupplierName.SelectedValue + "'  AND SRPOD_I_CODE = SID_I_CODE AND EX_INV_NO =  '" + txtInvoceNo.Text.Trim() + "' GROUP BY SIM_TYPE,SRPOM_PONO,SIM_NO,SIM_DATE,SIM_CHALLAN_NO,SIM_CHAL_DATE,SID_I_CODE,S_CODENO,S_NAME,SID_REV_QTY,SID_CON_OK_QTY,SRPOD_RATE,SID_RATE,SRPOD_DISC_AMT,SRPOD_EXC_PER,EX_EX_DUTY,SRPOD_EDU_CESS_PER,EX_EX_CESS,SRPOD_H_EDU_CESS , EX_EX_HCESS, SRPOD_T_CODE, SRPOM_CODE, SIM_CODE, SRPOD_GP_RATE,SID_CPOM_CODE,SID_CH_QTY,SRPOD_RATE,SRPOD_TOTAL_AMT,SRPOD_EXC_Y_N,SRPOM_PONO, EXD_EXC_PER,EXD_EDU_PER,EXD_HSEDU_PER,EX_FREIGHT,EX_PACKING_AMT,EX_OTHER_AMT,EX_INSURANCE_AMT order by IWM_NO");
                        }
                        else
                        {
                            //dt = CommonClasses.Execute(" SELECT DISTINCT INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PONO AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), CASE when round(IWD_TUR_WEIGHT,4)=0 then IWD_REV_QTY ELSE ( INWARD_DETAIL.IWD_REV_QTY *round(IWD_TUR_WEIGHT,4)) END) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CON_OK_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY * ISNULL(IWD_TUR_WEIGHT, 0) * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT, CONVERT (decimal(20, 4), SUPP_PO_DETAILS.SPOD_DISC_AMT) AS SPOD_DISC_AMT,   CASE WHEN CM_STATE=P_SM_CODE then SPOD_EXC_PER else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EXC_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4), 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END AS EX_EX_DUTY , CASE WHEN CM_STATE = P_SM_CODE THEN SPOD_EDU_CESS_PER ELSE 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EDU_CESS_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4) , 0)) * round(IWD_RATE,4))) /100) ELSE 0 END AS EX_EX_CESS ,CASE WHEN CM_STATE != P_SM_CODE THEN SPOD_H_EDU_CESS ELSE 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20,4), SPOD_H_EDU_CESS * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4) , 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END AS EX_EX_HCESS, SUPP_PO_DETAILS.SPOD_T_CODE, '' AS T_SHORT, '' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, CONVERT (decimal(20, 4), SUPP_PO_MASTER.SPOD_GP_RATE) AS SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4) AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME, '' AS ST_SALES_TAX, SUPP_PO_DETAILS.SPOD_EXC_Y_N,   0 AS EX_FREIGHT,0 AS EX_PACKING_AMT,0 AS EX_OTHER_AMT,0 AS EX_INSURANCE_AMT   FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE AND INWARD_DETAIL.IWD_I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE=COMPANY_MASTER.CM_CODE  INNER JOIN   PARTY_MASTER ON PARTY_MASTER.P_CODE=INWARD_MASTER.IWM_P_CODE  WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND (INWARD_MASTER.IWM_TYPE = 'IWIM' OR INWARD_MASTER.IWM_TYPE = 'OUTCUSTINV') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND IWD_BILL_PASS_FLG=0  AND (INWARD_MASTER.ES_DELETE = 0)   AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "') GROUP BY INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PO_NO, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, SUPP_PO_DETAILS.SPOD_RATE, INWARD_DETAIL.IWD_RATE, SUPP_PO_DETAILS.SPOD_DISC_AMT, SUPP_PO_DETAILS.SPOD_EXC_PER, SUPP_PO_DETAILS.SPOD_EDU_CESS_PER, SUPP_PO_DETAILS.SPOD_H_EDU_CESS, SUPP_PO_DETAILS.SPOD_T_CODE, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, SUPP_PO_MASTER.SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, SUPP_PO_DETAILS.SPOD_RATE, SUPP_PO_DETAILS.SPOD_TOTAL_AMT, SUPP_PO_DETAILS.SPOD_EXC_Y_N, SUPP_PO_MASTER.SPOM_PONO, INWARD_MASTER.IWM_CURR_RATE, COMPANY_MASTER.CM_STATE,PARTY_MASTER.P_SM_CODE,SPOD_EXC_Y_N  ,IWD_TUR_WEIGHT UNION SELECT DISTINCT INWARD_MASTER.IWM_TYPE, '' AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR,       INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4),                       INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY  * ROUND(INWARD_DETAIL.IWD_RATE, 4))                       AS SPOD_TOTAL_AMT, 0 AS SPOD_DISC_AMT, CASE WHEN CM_STATE = P_SM_CODE THEN E_BASIC ELSE 0 END AS SPOD_EXC_PER, CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4),  E_BASIC * CONVERT (decimal(20, 4), ((round(IWD_CH_QTY, 4)   * round(IWD_RATE, 4))) / 100)) ELSE 0 END AS EX_EX_DUTY,                       CASE WHEN CM_STATE = P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS SPOD_EDU_CESS_PER, CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4),                       E_EDU_CESS * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4)  * round(IWD_RATE, 4))) / 100) ELSE 0 END AS EX_EX_CESS,                       CASE WHEN CM_STATE != P_SM_CODE THEN E_H_EDU ELSE 0 END AS SPOD_H_EDU_CESS, CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20, 4),                      E_H_EDU * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4) *   round(IWD_RATE, 4))) / 100) ELSE 0 END AS EX_EX_HCESS,0 AS SPOD_T_CODE, '' AS T_SHORT,                       '' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, 0 AS SPOM_CODE, INWARD_MASTER.IWM_CODE, 0 AS SPOD_GP_RATE ,INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4)                       AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT,                       '' AS ST_TAX_NAME, '' AS ST_SALES_TAX,0 as SPOD_EXC_Y_N, 0 AS EX_FREIGHT, 0 AS EX_PACKING_AMT, 0 AS EX_OTHER_AMT, 0 AS EX_INSURANCE_AMT   FROM         INWARD_MASTER INNER JOIN                      INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN                      ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE = COMPANY_MASTER.CM_CODE INNER JOIN                      PARTY_MASTER ON PARTY_MASTER.P_CODE = INWARD_MASTER.IWM_P_CODE INNER JOIN                      EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE   WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND ( INWARD_MASTER.IWM_TYPE = 'Without PO inward') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_DETAIL.IWD_INSP_FLG = 1)  AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "')   GROUP BY INWARD_MASTER.IWM_TYPE, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE,       INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY,                       INWARD_DETAIL.IWD_RATE, INWARD_MASTER.IWM_CODE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, INWARD_MASTER.IWM_CURR_RATE,                       COMPANY_MASTER.CM_STATE, PARTY_MASTER.P_SM_CODE,	E_SPECIAL	,E_EDU_CESS	,E_H_EDU,E_BASIC  ORDER BY INWARD_MASTER.IWM_NO");
                            //change by amit                           
                            //  dt = CommonClasses.Execute(" SELECT DISTINCT INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PONO AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), CASE when round(IWD_TUR_WEIGHT,4)=0 then IWD_REV_QTY ELSE ( INWARD_DETAIL.IWD_REV_QTY *round(IWD_TUR_WEIGHT,4)) END) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CON_OK_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY * ISNULL(IWD_TUR_WEIGHT, 0) * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT, CONVERT (decimal(20, 4), SUPP_PO_DETAILS.SPOD_DISC_AMT) AS SPOD_DISC_AMT,  CASE WHEN P_LBT_IND=1 then CASE WHEN CM_STATE=P_SM_CODE then SPOD_EXC_PER else 0 END  ELSE  0 END  as SPOD_EXC_PER,   CASE WHEN P_LBT_IND=1  THEN CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EXC_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4), 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END  ELSE  0 END AS EX_EX_DUTY ,   CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE = P_SM_CODE THEN SPOD_EDU_CESS_PER ELSE 0 END ELSE  0 END AS SPOD_EDU_CESS_PER,  CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EDU_CESS_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4) , 0)) * round(IWD_RATE,4))) /100) ELSE 0 END  ELSE  0 END AS EX_EX_CESS ,   CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE != P_SM_CODE THEN SPOD_H_EDU_CESS ELSE 0 END  ELSE  0 END  AS SPOD_H_EDU_CESS ,  CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20,4), SPOD_H_EDU_CESS * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round(IWD_TUR_WEIGHT,4) , 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END  ELSE  0 END  AS EX_EX_HCESS , SUPP_PO_DETAILS.SPOD_T_CODE, '' AS T_SHORT, '' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, CONVERT (decimal(20, 4), SUPP_PO_MASTER.SPOD_GP_RATE) AS SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4) AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME, '' AS ST_SALES_TAX, SUPP_PO_DETAILS.SPOD_EXC_Y_N,   0 AS EX_FREIGHT,0 AS EX_PACKING_AMT,0 AS EX_OTHER_AMT,0 AS EX_INSURANCE_AMT   FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE AND INWARD_DETAIL.IWD_I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE=COMPANY_MASTER.CM_CODE  INNER JOIN   PARTY_MASTER ON PARTY_MASTER.P_CODE=INWARD_MASTER.IWM_P_CODE  WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND (INWARD_MASTER.IWM_TYPE = 'IWIM' OR INWARD_MASTER.IWM_TYPE = 'OUTCUSTINV') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND IWD_BILL_PASS_FLG=0  AND (INWARD_MASTER.ES_DELETE = 0)   AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "') GROUP BY INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PO_NO, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, SUPP_PO_DETAILS.SPOD_RATE, INWARD_DETAIL.IWD_RATE, SUPP_PO_DETAILS.SPOD_DISC_AMT, SUPP_PO_DETAILS.SPOD_EXC_PER, SUPP_PO_DETAILS.SPOD_EDU_CESS_PER, SUPP_PO_DETAILS.SPOD_H_EDU_CESS, SUPP_PO_DETAILS.SPOD_T_CODE, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, SUPP_PO_MASTER.SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, SUPP_PO_DETAILS.SPOD_RATE, SUPP_PO_DETAILS.SPOD_TOTAL_AMT, SUPP_PO_DETAILS.SPOD_EXC_Y_N, SUPP_PO_MASTER.SPOM_PONO, INWARD_MASTER.IWM_CURR_RATE, COMPANY_MASTER.CM_STATE,PARTY_MASTER.P_SM_CODE,SPOD_EXC_Y_N  ,IWD_TUR_WEIGHT,P_LBT_IND UNION SELECT DISTINCT INWARD_MASTER.IWM_TYPE, '' AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR,INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY  * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT, 0 AS SPOD_DISC_AMT,   CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN E_BASIC ELSE 0 END    ELSE 0 END AS SPOD_EXC_PER, CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4),  E_BASIC * CONVERT (decimal(20, 4), ((round(IWD_CH_QTY, 4)   * round(IWD_RATE, 4))) / 100)) ELSE 0 END  ELSE 0 END  AS EX_EX_DUTY,   CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN E_EDU_CESS ELSE 0 END  ELSE 0 END  AS SPOD_EDU_CESS_PER,CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4), E_EDU_CESS * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4)  * round(IWD_RATE, 4))) / 100) ELSE 0 END  ELSE 0 END  AS EX_EX_CESS, CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE != P_SM_CODE THEN E_H_EDU ELSE 0 END   ELSE 0 END  AS SPOD_H_EDU_CESS, CASE WHEN P_LBT_IND=1 then CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20, 4), E_H_EDU * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4) *   round(IWD_RATE, 4))) / 100) ELSE 0 END  ELSE 0 END  AS EX_EX_HCESS   ,0 AS SPOD_T_CODE, '' AS T_SHORT,'' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, 0 AS SPOM_CODE, INWARD_MASTER.IWM_CODE, 0 AS SPOD_GP_RATE ,INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4)                       AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT,                       '' AS ST_TAX_NAME, '' AS ST_SALES_TAX,0 as SPOD_EXC_Y_N, 0 AS EX_FREIGHT, 0 AS EX_PACKING_AMT, 0 AS EX_OTHER_AMT, 0 AS EX_INSURANCE_AMT   FROM         INWARD_MASTER INNER JOIN                      INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN                      ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE = COMPANY_MASTER.CM_CODE INNER JOIN                      PARTY_MASTER ON PARTY_MASTER.P_CODE = INWARD_MASTER.IWM_P_CODE INNER JOIN                      EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE   WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND ( INWARD_MASTER.IWM_TYPE = 'Without PO inward') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_DETAIL.IWD_INSP_FLG = 1)  AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "')   GROUP BY INWARD_MASTER.IWM_TYPE, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE,INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY,INWARD_DETAIL.IWD_RATE, INWARD_MASTER.IWM_CODE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, INWARD_MASTER.IWM_CURR_RATE,COMPANY_MASTER.CM_STATE, PARTY_MASTER.P_SM_CODE,	E_SPECIAL	,E_EDU_CESS	,E_H_EDU,E_BASIC,P_LBT_IND  ORDER BY INWARD_MASTER.IWM_NO");

                            //added by amit
                            dt = CommonClasses.Execute(" SELECT DISTINCT INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PONO AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), CASE when round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4)=0 then IWD_REV_QTY ELSE ( INWARD_DETAIL.IWD_REV_QTY *round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4)) END) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CON_OK_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY * ISNULL((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END), 0) * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT, CONVERT (decimal(20, 4), SUPP_PO_DETAILS.SPOD_DISC_AMT) AS SPOD_DISC_AMT,  CASE WHEN P_LBT_IND=1 then CASE WHEN CM_STATE=P_SM_CODE then SPOD_EXC_PER else 0 END  ELSE  0 END  as SPOD_EXC_PER,   CASE WHEN P_LBT_IND=1  THEN CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EXC_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4), 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END  ELSE  0 END AS EX_EX_DUTY ,   CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE = P_SM_CODE THEN SPOD_EDU_CESS_PER ELSE 0 END ELSE  0 END AS SPOD_EDU_CESS_PER,  CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20,4),SPOD_EDU_CESS_PER * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4) , 0)) * round(IWD_RATE,4))) /100) ELSE 0 END  ELSE  0 END AS EX_EX_CESS ,   CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE != P_SM_CODE THEN SPOD_H_EDU_CESS ELSE 0 END  ELSE  0 END  AS SPOD_H_EDU_CESS ,  CASE WHEN P_LBT_IND=1  THEN  CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20,4), SPOD_H_EDU_CESS * CONVERT (decimal(20,4),((round(IWD_CH_QTY,4) * ISNULL( round((CASE WHEN SPOD_UOM_CODE=SPOD_RATE_UOM then 1 else IWD_TUR_WEIGHT END),4) , 0)) * round(IWD_RATE,4)))/100 ) ELSE 0 END  ELSE  0 END  AS EX_EX_HCESS , SUPP_PO_DETAILS.SPOD_T_CODE, '' AS T_SHORT, '' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, CONVERT (decimal(20, 4), SUPP_PO_MASTER.SPOD_GP_RATE) AS SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4) AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME, '' AS ST_SALES_TAX, SUPP_PO_DETAILS.SPOD_EXC_Y_N,   0 AS EX_FREIGHT,0 AS EX_PACKING_AMT,0 AS EX_OTHER_AMT,0 AS EX_INSURANCE_AMT   FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE AND INWARD_DETAIL.IWD_I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE=COMPANY_MASTER.CM_CODE  INNER JOIN   PARTY_MASTER ON PARTY_MASTER.P_CODE=INWARD_MASTER.IWM_P_CODE  WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND (INWARD_MASTER.IWM_TYPE = 'IWIM' OR INWARD_MASTER.IWM_TYPE = 'OUTCUSTINV') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND IWD_BILL_PASS_FLG=0  AND (INWARD_MASTER.ES_DELETE = 0)   AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "') GROUP BY INWARD_MASTER.IWM_TYPE, SUPP_PO_MASTER.SPOM_PO_NO, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, SUPP_PO_DETAILS.SPOD_RATE, INWARD_DETAIL.IWD_RATE, SUPP_PO_DETAILS.SPOD_DISC_AMT, SUPP_PO_DETAILS.SPOD_EXC_PER, SUPP_PO_DETAILS.SPOD_EDU_CESS_PER, SUPP_PO_DETAILS.SPOD_H_EDU_CESS, SUPP_PO_DETAILS.SPOD_T_CODE, SUPP_PO_MASTER.SPOM_CODE, INWARD_MASTER.IWM_CODE, SUPP_PO_MASTER.SPOD_GP_RATE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, SUPP_PO_DETAILS.SPOD_RATE, SUPP_PO_DETAILS.SPOD_TOTAL_AMT, SUPP_PO_DETAILS.SPOD_EXC_Y_N, SUPP_PO_MASTER.SPOM_PONO, INWARD_MASTER.IWM_CURR_RATE, COMPANY_MASTER.CM_STATE,PARTY_MASTER.P_SM_CODE,SPOD_EXC_Y_N  ,IWD_TUR_WEIGHT,P_LBT_IND,SPOD_UOM_CODE,SPOD_RATE_UOM  UNION SELECT DISTINCT INWARD_MASTER.IWM_TYPE, '' AS SPOM_PO_NO, INWARD_MASTER.IWM_NO, CONVERT(VARCHAR, INWARD_MASTER.IWM_DATE, 106) AS IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, CONVERT(VARCHAR,INWARD_MASTER.IWM_CHAL_DATE, 106) AS IWM_CHAL_DATE, INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_REV_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_REV_QTY) AS IWD_CON_OK_QTY, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_RATE) AS IWD_RATE, CONVERT (decimal(20, 4), INWARD_DETAIL.IWD_CH_QTY  * ROUND(INWARD_DETAIL.IWD_RATE, 4)) AS SPOD_TOTAL_AMT, 0 AS SPOD_DISC_AMT,   CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN E_BASIC ELSE 0 END    ELSE 0 END AS SPOD_EXC_PER, CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4),  E_BASIC * CONVERT (decimal(20, 4), ((round(IWD_CH_QTY, 4)   * round(IWD_RATE, 4))) / 100)) ELSE 0 END  ELSE 0 END  AS EX_EX_DUTY,   CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN E_EDU_CESS ELSE 0 END  ELSE 0 END  AS SPOD_EDU_CESS_PER,CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE = P_SM_CODE THEN CONVERT (decimal(20, 4), E_EDU_CESS * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4)  * round(IWD_RATE, 4))) / 100) ELSE 0 END  ELSE 0 END  AS EX_EX_CESS, CASE WHEN P_LBT_IND=1 then  CASE WHEN CM_STATE != P_SM_CODE THEN E_H_EDU ELSE 0 END   ELSE 0 END  AS SPOD_H_EDU_CESS, CASE WHEN P_LBT_IND=1 then CASE WHEN CM_STATE != P_SM_CODE THEN CONVERT (decimal(20, 4), E_H_EDU * CONVERT (decimal(20, 4), (round(IWD_CH_QTY, 4) *   round(IWD_RATE, 4))) / 100) ELSE 0 END  ELSE 0 END  AS EX_EX_HCESS   ,0 AS SPOD_T_CODE, '' AS T_SHORT,'' AS ST_SALES_TAX, '' AS T_SALESTAX, '0.00' AS SALESTAX_AMT, 0 AS SPOM_CODE, INWARD_MASTER.IWM_CODE, 0 AS SPOD_GP_RATE ,INWARD_DETAIL.IWD_CPOM_CODE, ROUND(INWARD_DETAIL.IWD_CH_QTY, 4)                       AS IWD_CH_QTY, ROUND(INWARD_DETAIL.IWD_RATE, 4) AS SPOD_RATE, ROUND(ROUND(INWARD_DETAIL.IWD_RATE, 4) * INWARD_DETAIL.IWD_CH_QTY, 4) AS SPOD_TOTAL_AMT,                       '' AS ST_TAX_NAME, '' AS ST_SALES_TAX,0 as SPOD_EXC_Y_N, 0 AS EX_FREIGHT, 0 AS EX_PACKING_AMT, 0 AS EX_OTHER_AMT, 0 AS EX_INSURANCE_AMT   FROM         INWARD_MASTER INNER JOIN                      INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN                      ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON INWARD_DETAIL.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      COMPANY_MASTER ON INWARD_MASTER.IWM_CM_CODE = COMPANY_MASTER.CM_CODE INNER JOIN                      PARTY_MASTER ON PARTY_MASTER.P_CODE = INWARD_MASTER.IWM_P_CODE INNER JOIN                      EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE   WHERE     (INWARD_DETAIL.IWD_INSP_FLG = 1) AND ( INWARD_MASTER.IWM_TYPE = 'Without PO inward') AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_DETAIL.IWD_INSP_FLG = 1)  AND (INWARD_MASTER.IWM_INWARD_TYPE IN (0, 1)) AND (INWARD_DETAIL.IWD_BILL_PASS_FLG = 0) AND (INWARD_MASTER.ES_DELETE = 0) AND (INWARD_MASTER.IWM_P_CODE = '" + ddlSupplierName.SelectedValue + "')   GROUP BY INWARD_MASTER.IWM_TYPE, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE,INWARD_DETAIL.IWD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY,INWARD_DETAIL.IWD_RATE, INWARD_MASTER.IWM_CODE, INWARD_DETAIL.IWD_CPOM_CODE, INWARD_DETAIL.IWD_CH_QTY, INWARD_MASTER.IWM_CURR_RATE,COMPANY_MASTER.CM_STATE, PARTY_MASTER.P_SM_CODE,	E_SPECIAL	,E_EDU_CESS	,E_H_EDU,E_BASIC,P_LBT_IND  ORDER BY INWARD_MASTER.IWM_NO");

                        }
                        txtBasicAmount.Enabled = false;

                        txtAccesableValue.Enabled = false;
                        txtExciseAmount.Enabled = false;
                        txtEduCessAmt.Enabled = false;
                        txtSHEduCessAmt.Enabled = false;
                        txtTaxableAmt.Enabled = false;
                        //txtsalestaxamt.Enabled = false;
                        //txtTaxPer.Enabled = false;
                        //txtOtherCharges.Enabled = false;
                        //txtloadingAmt.Enabled = false;
                        //txtFreightAmt1.Enabled = false;
                        //txtInsuranceAmt.Enabled = false;
                        //txtOctroiAmt2.Enabled = false;
                        //txtTransportAmt.Enabled = false;
                        txtGrandTotal.Enabled = false;
                        txtexcper.Enabled = false;
                        txteducessper.Enabled = false;
                        txtsheper.Enabled = false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dgBillPassing.Enabled = true;
                        dgBillPassing.DataSource = dt;
                        dgBillPassing.DataBind();
                    }
                    else
                    {
                        dgBillPassing.DataSource = null;
                        dgBillPassing.DataBind();
                        LoadFilter();
                        //PanelMsg.Visible = true;
                        //lblmsg.Text = "This Supplier Bill Not Present";
                        LoadFilter();
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Taluka Master", "LoadDistrict", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillSupplier
        DataTable dt = new DataTable();
        try
        {
            if (ChkUseService.Checked)
            {
                dt = CommonClasses.FillCombo("SERVICE_INWARD_MASTER,SERVICE_INWARD_DETAIL,PARTY_MASTER", "P_NAME", "P_CODE", "SIM_CODE=SID_SIM_CODE AND P_CODE=SIM_P_CODE AND SIM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND SID_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0' AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by P_NAME", ddlSupplierName);
                ddlSupplierName.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
            }
            if (chkUseCustRej.Checked)
            {
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dt = CommonClasses.FillCombo("CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER", "P_NAME", "P_CODE", "CR_CODE=CD_CR_CODE AND P_CODE=CR_P_CODE AND CR_CM_COMP_ID= " + Convert.ToInt32(Session["CompanyId"]) + " ORDER by P_NAME", ddlSupplierName);
                }
                else
                {
                    dt = CommonClasses.FillCombo("CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER", "P_NAME", "P_CODE", "CR_CODE=CD_CR_CODE AND P_CODE=CR_P_CODE AND CD_MODVAT_FLG=0 AND CR_CM_COMP_ID= " + Convert.ToInt32(Session["CompanyId"]) + " ORDER by P_NAME", ddlSupplierName);
                }
            }
            else
            {
                dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + "   AND IWD_IWM_CODE=IWM_CODE AND IWD_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0'  AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME", ddlSupplierName);

            }
            ddlSupplierName.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "FillSupplier", Ex.Message);
        }
        #endregion


    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgBillPassing.Rows.Count == 0)
        {

            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_RATE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_TOTAL_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_DUTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_HCESS", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("EX_FREIGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_PACKING_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_OTHER_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_INSURANCE_AMT", typeof(String)));


                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
            }
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from SUPP_PO_MASTERS where SPOM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Record used by another user";
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing-ADD", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Bill Passing - ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    protected void ChkUseService_CheckedChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

    protected void chkUseCustRej_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            LoadCombos();
        }
        catch (Exception)
        {

        }
    }

    #endregion

    #region txtexcper_TextChanged
    protected void txtexcper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtExciseAmount.Text = string.Format("{0:0.0000}", (Convert.ToDouble(txtAccesableValue.Text)*Convert.ToDouble(txtexcper.Text))/100);
    }
    #endregion txtexcper_TextChanged

    #region txteducessper_TextChanged
    protected void txteducessper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtEduCessAmt.Text = string.Format("{0:0.0000}", (Convert.ToDouble(txtExciseAmount.Text) * Convert.ToDouble(txteducessper.Text)) / 100);
    }
    #endregion txteducessper_TextChanged

    #region txtsheper_TextChanged
    protected void txtsheper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtSHEduCessAmt.Text = string.Format("{0:0.0000}", (Convert.ToDouble(txtExciseAmount.Text) * Convert.ToDouble(txtsheper.Text)) / 100);
    }
    #endregion txtsheper_TextChanged

    protected void txtExciseAmount_TextChanged(object sender, EventArgs e)
    {
        if (dgBillPassing.Enabled == true)
        {
            Calculate(1);
        }
    }
}

