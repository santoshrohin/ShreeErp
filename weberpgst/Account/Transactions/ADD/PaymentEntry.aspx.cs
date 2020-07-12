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

public partial class Account_Transactions_ADD_PaymentEntry : System.Web.UI.Page
{

    #region General Declaration
    PaymentEntry_BL BL_PaymentEntry = null;
    double PedQty = 0;


    static int mlCode = 0;
    static string right = "";
    static string redirect = "~/Account/Transactions/VIEW/ViewPaymentEntry.aspx";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    #endregion

    public string Msg = "";

    #region Events

    #region Page_Load
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
                ViewState["Index"] = Index;
                ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                ViewState["dt2"] = dt2;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                str = "";
                ViewState["str"] = str;

                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_PaymentEntry = new PaymentEntry_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());

                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        BL_PaymentEntry = new PaymentEntry_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtPAYMDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        txtChequeDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                        BlankGrid();
                        LoadCombos();
                        //LoadCurr();
                        dtFilter.Rows.Clear();

                        //txtPAYMDate.Attributes.Add("readonly", "readonly");
                        //txtChequeDate.Attributes.Add("readonly", "readonly");

                    }
                    ddlCustomer.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Payment Entry", "PageLoad", ex.Message);
                }
            }

        }
    }

    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_REF_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("REF_DESC", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_INVOICE_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_INVOICE_CODE_TEMP", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INVOICE_NO", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_AMOUNT", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_ADJ_AMOUNT", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_REMARK", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PAYMD_TYPE", typeof(string)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Clear();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillCustomer
        try
        {
            DataTable dtParty = new DataTable();
            //  dt = CommonClasses.FillCombo("PARTY_MASTER,BILL_PASSING_MASTER", "P_NAME", "P_CODE", "BILL_PASSING_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and PAYM_P_CODE=P_CODE  ORDER BY P_NAME", ddlCustomer);
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE=0   AND PARTY_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_ACTIVE_IND=1  UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 ORDER BY P_NAME  ");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE=0  AND PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_ACTIVE_IND=1  UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1  ORDER BY P_NAME ");
            }
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "LoadCombos", Ex.Message);
        }
        #endregion

        #region ACCOUNTLEDGER
        try
        {

            dt = CommonClasses.FillCombo("ACCOUNT_LEDGER", "L_NAME", "L_CODE", "ACCOUNT_LEDGER.ES_DELETE=0  and L_CM_ID=" + (string)Session["CompanyId"] + "  ORDER BY L_NAME", ddlLedger);
            ddlLedger.Items.Insert(0, new ListItem("Please Select Ledger ", "0"));




        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "LoadCombos", Ex.Message);
        }
        #endregion





    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {
                if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtPAYMDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtPAYMDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtChequeDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtChequeDate.Text))))
                {
                    if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                    {
                        SaveRec();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Invoice ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlInvoiceNo.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlInvoiceNo.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Payment Entry", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion


    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtChequeDate.Attributes.Add("readonly", "readonly");
            txtPAYMDate.Attributes.Add("readonly", "readonly");


            LoadCombos();
            dt = CommonClasses.Execute(" SELECT PAYM_CODE,PAYM_NO,PAYM_DATE,PAYM_P_CODE,PAYM_CHEQUE_NO,PAYM_CHEQUE_DATE,PAYM_AMOUNT,PAYM_LEDGER_CODE,PAYM_REMARK,PAYM_CM_CODE FROM PAYMENT_MASTER WHERE PAYMENT_MASTER.ES_DELETE = 0 AND PAYM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and PAYM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["PAYM_CODE"]);
                txtPAYMno.Text = Convert.ToInt32(dt.Rows[0]["PAYM_NO"]).ToString();
                txtPAYMDate.Text = Convert.ToDateTime(dt.Rows[0]["PAYM_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = Convert.ToInt32(dt.Rows[0]["PAYM_P_CODE"]).ToString();
                txtChequeNo.Text = (dt.Rows[0]["PAYM_CHEQUE_NO"]).ToString();
                txtChequeDate.Text = Convert.ToDateTime(dt.Rows[0]["PAYM_CHEQUE_DATE"]).ToString("dd MMM yyyy");

                txtAmount.Text = (dt.Rows[0]["PAYM_AMOUNT"]).ToString();
                ddlLedger.SelectedValue = dt.Rows[0]["PAYM_LEDGER_CODE"].ToString();

                ddlInvoiceNo.Items.Clear();

                int id = Convert.ToInt32(ddlCustomer.SelectedValue);
                ddlInvoiceNo.Items.Clear();
                ddlRefernce.Items.Clear();
                DataTable dtItem = new DataTable();
                dtItem = CommonClasses.Execute("declare @temp table(BPM_CODE int IDENTITY (1,1),INVOICE_CODE int,BPM_NO nvarchar(500),INM_TYPE int)insert into @temp SELECT BPM_CODE,CONCAT(BPM_NO,'-','BP') as BPM_NO ,1 AS BTYPE FROM BILL_PASSING_MASTER WHERE BILL_PASSING_MASTER.ES_DELETE=0 and BPM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ((ISNULL(BPM_PAID_AMT,0)) < (BPM_G_AMT))  and BPM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT DNM_CODE,CONCAT(DNM_SERIAL_NO,'-','DN'),0 AS BTYPE  FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 and DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "' AND ((ISNULL(DNM_PAID_AMT,0)) < (DNM_NET_AMOUNT))  and DNM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT CNM_CODE,CONCAT(CNM_SERIAL_NO,'-','CN'),2 AS BTYPE FROM CREDIT_NOTE_MASTER WHERE CREDIT_NOTE_MASTER.ES_DELETE=0 and CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'  AND ((ISNULL(CNM_RECIEVED_AMT,0)) < (CNM_NET_AMOUNT)) and CNM_CM_CODE='" + (string)Session["CompanyCode"] + "' select * from @temp ");
                ddlInvoiceNo.DataSource = dtItem;
                ddlInvoiceNo.DataTextField = "BPM_NO";
                ddlInvoiceNo.DataValueField = "BPM_CODE";
                ddlInvoiceNo.DataBind();
                ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));



                CommonClasses.FillCombo("REFERENCE_MASTER", "REF_DESC", "REF_CODE", " ES_DELETE=0 and REF_CM_ID=" + (string)Session["CompanyId"], ddlRefernce);

                ddlRefernce.Items.Insert(0, new ListItem("Reference Type ", "0"));


                dtInwardDetail = CommonClasses.Execute("select identity(int,1,1) as RN, PAYMENT_DETAIL.PAYMD_TYPE,PAYMD_REF_CODE,REF_DESC,PAYMD_INVOICE_CODE,PAYMD_INVOICE_CODE_TEMP,PAYMD_INVOICE_NO as INVOICE_NO,PAYMD_AMOUNT,PAYMD_ADJ_AMOUNT,PAYMD_REMARK into #Tempp from PAYMENT_MASTER,PAYMENT_DETAIL,REFERENCE_MASTER where PAYM_CODE=PAYMD_PAYM_CODE and REF_CODE=PAYMD_REF_CODE   and PAYM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and PAYMD_PAYM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' declare @temp table(id int identity(1,1),newrefkey int,Ttype int) declare @cnt int=0,@i int=1,@newreferncecode int=0,@newreferncename varchar(5000) insert into @temp select PAYMD_INVOICE_CODE,PAYMD_TYPE from #Tempp where PAYMD_TYPE=7  select @cnt=count(*) from @temp where Ttype=7   while(@cnt>=@i) begin   select @newreferncecode=newrefkey from @Temp where id=@i select @newreferncename=REF_NAME from NEWREFERENCE_MASTER where REF_CODE=@newreferncecode   update #Tempp set INVOICE_NO=@newreferncename where PAYMD_INVOICE_CODE=@newreferncecode and PAYMD_TYPE=7   set @i=@i+1  end   select * from #Tempp   drop table #Tempp");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }

            if (str == "VIEW")
            {
                ddlCustomer.Enabled = false;

                ddlInvoiceNo.Enabled = false;
                ddlRefernce.Enabled = false;

                txtRecievedAmount.Enabled = false;
                txtAdjsAmt.Enabled = false;
                txtRemark.Enabled = false;
                dgInwardMaster.Enabled = false;

            }

            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("PAYMENT_MASTER", "MODIFY", "PAYM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "ViewRec", Ex.Message);
        }
    }
    #endregion

    //#region LoadCurr
    //private void LoadCurr()
    //{
    //    try
    //    {
    //        DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER WHERE ES_DELETE = 0 AND CURR_CM_COMP_ID = " + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
    //        ddlLedger.DataSource = dt;
    //        ddlLedger.DataTextField = "CURR_NAME";
    //        ddlLedger.DataValueField = "CURR_CODE";
    //        ddlLedger.DataBind();
    //        ddlLedger.Items.Insert(0, new ListItem("Select Ledger", "0"));
    //        ddlLedger.SelectedIndex = -1;
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
    //    }
    //}
    //#endregion
    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlCustomer.SelectedValue = BL_PaymentEntry.PAYM_P_CODE.ToString();
                txtPAYMno.Text = BL_PaymentEntry.PAYM_NO.ToString();
                txtChequeNo.Text = BL_PaymentEntry.PAYM_CHEQUE_NO.ToString();

                txtPAYMDate.Text = BL_PaymentEntry.PAYM_DATE.ToString("dd MMM YYYY");
                txtChequeDate.Text = BL_PaymentEntry.PAYM_CHEQUE_DATE.ToString("dd MMM YYYY");
                ddlLedger.SelectedValue = BL_PaymentEntry.PAYM_LEDGER_CODE.ToString();
                txtAmount.Text = BL_PaymentEntry.PAYM_AMOUNT.ToString();
            }
            else if (str == "VIEW")
            {
                ddlCustomer.Enabled = false;
                txtPAYMno.Enabled = false;
                txtChequeNo.Enabled = false;

                txtChequeDate.Enabled = false;
                txtPAYMDate.Enabled = false;
                BtnInsert.Visible = false;
                btnSubmit.Visible = false;
                ddlLedger.Enabled = false;
                txtAmount.Enabled = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Payment Entry", "GetValues", ex.Message);
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
            BL_PaymentEntry.PAYM_P_CODE = Convert.ToInt32(ddlCustomer.SelectedValue);
            //BL_PaymentEntry.PAYM_SITE_CODE = Convert.ToInt32(ddlSiteName.SelectedValue);
            BL_PaymentEntry.PAYM_NO = Convert.ToInt32(txtPAYMno.Text);
            BL_PaymentEntry.PAYM_CHEQUE_NO = txtChequeNo.Text.ToString();


            BL_PaymentEntry.PAYM_CHEQUE_DATE = Convert.ToDateTime(txtChequeDate.Text);
            BL_PaymentEntry.PAYM_DATE = Convert.ToDateTime(txtPAYMDate.Text);
            BL_PaymentEntry.PAYM_LEDGER_CODE = Convert.ToInt32(ddlLedger.SelectedValue);
            BL_PaymentEntry.PAYM_AMOUNT = float.Parse(txtAmount.Text);
            BL_PaymentEntry.PAYM_REMARK = txtRemark.Text;
            BL_PaymentEntry.PAYM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);



            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Payment Entry", "Setvalues", ex.Message);
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
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_PaymentEntry = new PaymentEntry_BL();
                txtPAYMno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_PaymentEntry.Save(dgInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PAYM_CODE) from PAYMENT_MASTER");
                        CommonClasses.WriteLog("Payment Entry ", "Insert", "Payment Entry", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(redirect, false);
                    }
                    else
                    {
                        if (BL_PaymentEntry.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_PaymentEntry.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_PaymentEntry.Msg = "";
                        }
                        ddlCustomer.Focus();
                    }
                }
            }
            #endregion

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_PaymentEntry = new PaymentEntry_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_PaymentEntry.Update(dgInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("PAYMENT_MASTER", "MODIFY", "PAYM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        // CommonClasses.WriteLog("Payment Entry ", "Update", "Payment Entry",  Convert.ToInt32(ViewState["mlCode"].ToString()) , Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        CommonClasses.WriteLog("Payment Entry ", "Update", "Payment Entry", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(redirect, false);
                    }
                    else
                    {
                        if (BL_PaymentEntry.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_PaymentEntry.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_PaymentEntry.Msg = "";
                        }
                        ddlCustomer.Focus();
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Payment Entry", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Payment Entry", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(PAYM_NO) as PAYM_NO from PAYMENT_MASTER where PAYM_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0");
        if (dt.Rows[0]["PAYM_NO"] == null || dt.Rows[0]["PAYM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["PAYM_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion
    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];


            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgInwardMaster.DataBind();
                if (dgInwardMaster.Rows.Count == 0)
                    BlankGrid();
                // clearDetail();
                double Total_Amount = 0;

                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {

                    if (((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_TYPE")).Text == "0")
                    {
                        Total_Amount = Total_Amount - Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);
                    }


                    else
                    {
                        Total_Amount = Total_Amount + Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);
                    }
                }
                txtAmount.Text = Total_Amount.ToString();
            }
            if (e.CommandName == "Select")
            {

                if (((Label)(row.FindControl("lblPAYMD_REF_CODE"))).Text == "-2147483647")
                {
                    ddlInvoiceNo.Items.Clear();
                    txtRemaningamt.Text = "";
                    txtRecievedAmount.Text = "";
                    txtAdjsAmt.Text = "";
                    DataTable dtItem = new DataTable();
                    dtItem = CommonClasses.Execute("select * from NEWREFERENCE_MASTER where ES_DELETE=0   and REF_CM_COMP_ID='" + (string)Session["CompanyID"] + "'");
                    ddlInvoiceNo.DataSource = dtItem;
                    ddlInvoiceNo.DataTextField = "REF_NAME";
                    ddlInvoiceNo.DataValueField = "REF_CODE";
                    ddlInvoiceNo.DataBind();
                    ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Reference ", "0"));
                }
                else
                {
                    FillInvoiceNo();
                }

                LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                lnkDelete.Enabled = false;

                //dgInwardMaster.Columns[1].Visible = false;

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                string s = ((Label)(row.FindControl("lblPAYMD_INVOICE_CODE"))).Text;
                ddlRefernce.SelectedValue = ((Label)(row.FindControl("lblPAYMD_REF_CODE"))).Text;
                ddlInvoiceNo.SelectedValue = ((Label)(row.FindControl("lblPAYMD_INVOICE_CODE_TEMP"))).Text;
                ddlRefernce_SelectedIndexChanged(null, null);

                txtAdjsAmt.Text = ((Label)(row.FindControl("lblPAYMD_ADJ_AMOUNT"))).Text;
                if (txtAdjsAmt.Text == "")
                {
                    txtAdjsAmt.Text = "0";
                }

                //txtRate.Text = ((Label)(row.FindControl("lblSPOD_RATE"))).Text;
                txtRecievedAmount.Text = ((Label)(row.FindControl("lblPAYMD_AMOUNT"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblPAYMD_REMARK"))).Text;

                txtRemaningamt.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                RemainingamtModify();

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlInvoiceNo.SelectedValue = "0";
            ddlRefernce.SelectedValue = "0";
            ddlCustomer.Items.Insert(0, new ListItem("Please Select PO ", "0"));
            txtRecievedAmount.Text = "0.00";
            txtAdjsAmt.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Payment Entry ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {

        try
        {
            PanelMsg.Visible = false;

            if (Convert.ToInt32(ddlCustomer.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }

            if (txtChequeNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Cheque No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChequeNo.Focus();
                return;

            }
            /* Logic Change :- as per Swati Madam*/
            //if (txtAmount.Text == "" || Convert.ToDecimal(txtAmount.Text) <= 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Enter Cheque Amount";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtAmount.Focus();
            //    return;
            //}
            if (Convert.ToInt32(ddlLedger.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Ledger Head";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlLedger.Focus();
                return;
            }
            if (Convert.ToInt32(ddlRefernce.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Reference No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRefernce.Focus();
                return;
            }
            if (Convert.ToInt32(ddlInvoiceNo.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Invoice No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlInvoiceNo.Focus();
                return;
            }

            if (txtRecievedAmount.Text == "" || Convert.ToDecimal(txtRecievedAmount.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recieved Amount";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecievedAmount.Focus();
                return;
            }
            if (txtAdjsAmt.Text == "")
            {
                txtAdjsAmt.Text = "0.0";
            }
            if (Convert.ToDouble(txtAdjsAmt.Text) > 0 || Convert.ToDouble(txtAdjsAmt.Text) < 0)
            {
                if (txtRemark.Text=="")
                {
                   lblmsg.Text = "Enter Remark";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRemark.Focus();
                return; 
                }
            }
            if (txtRecievedAmount.Text == "")
            {
                txtRecievedAmount.Text = "0.0";
            }
            double chq = Convert.ToDouble(txtRecievedAmount.Text);
            if (txtRemaningamt.Text == "")
            {
                txtRemaningamt.Text = "0.0";
            }
            double pdq = Convert.ToDouble(txtRemaningamt.Text);



            PanelMsg.Visible = false;
            
            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblINVOICE_NO"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlInvoiceNo.SelectedItem.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Invoice No";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlInvoiceNo.SelectedItem.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Invoice No";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }

                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_REF_CODE");

                ((DataTable)ViewState["dt2"]).Columns.Add("REF_DESC");
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_INVOICE_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_INVOICE_CODE_TEMP");
                ((DataTable)ViewState["dt2"]).Columns.Add("INVOICE_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_AMOUNT");
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_ADJ_AMOUNT");
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("PAYMD_TYPE");

            }
            #endregion
            double adjamt = 0;
            if (txtAdjsAmt.Text != "")
            {
                adjamt = Convert.ToDouble(txtAdjsAmt.Text);
            }
            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["PAYMD_REF_CODE"] = ddlRefernce.SelectedValue;
            dr["REF_DESC"] = ddlRefernce.SelectedItem;

            string strInvoicType = ddlInvoiceNo.SelectedItem.Text;
            DataTable dtInvoiceCode = new DataTable();
            string[] strArr = null;
            if (ddlRefernce.SelectedValue == "-2147483648")
            {
                strArr = strInvoicType.Split('-');






                if (strArr[1] == "CN")
                {
                    dtInvoiceCode = CommonClasses.Execute("select CNM_CODE  AS PK from CREDIT_NOTE_MASTER where CNM_SERIAL_NO='" + strArr[0] + "'");
                }


                if (strArr[1] == "BP")
                {
                    dtInvoiceCode = CommonClasses.Execute("select BPM_CODE AS PK from BILL_PASSING_MASTER where BPM_P_CODE='"+ ddlCustomer.SelectedValue+"' and ES_DELETE=0 AND   BPM_INV_NO='" + strArr[0] + "'");
                }

                if (strArr[1] == "INV")
                {
                    dtInvoiceCode = CommonClasses.Execute("select INM_CODE AS PK from INVOICE_MASTER where INM_NO='" + strArr[0] + "'");
                }
                if (strArr[1] == "DN")
                {
                    dtInvoiceCode = CommonClasses.Execute("select DNM_CODE AS PK from DEBIT_NOTE_MASTER where DNM_SERIAL_NO='" + strArr[0] + "'");
                }
            }

            if (dtInvoiceCode.Rows.Count > 0)
            {
                dr["PAYMD_INVOICE_CODE"] = dtInvoiceCode.Rows[0]["PK"].ToString();
            }
            if (ddlRefernce.SelectedValue == "-2147483647")
            {
                dr["PAYMD_INVOICE_CODE"] = ddlInvoiceNo.SelectedValue;
            }

            dr["PAYMD_INVOICE_CODE_TEMP"] = ddlInvoiceNo.SelectedValue;







            //dr["PAYMD_INVOICE_CODE"] = ddlInvoiceNo.SelectedValue;
            dr["INVOICE_NO"] = ddlInvoiceNo.SelectedItem.ToString();
            dr["PAYMD_AMOUNT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRecievedAmount.Text)));
            dr["PAYMD_ADJ_AMOUNT"] = string.Format("{0:0.00}", (adjamt));

            dr["PAYMD_REMARK"] = txtRemark.Text;
            if (ddlRefernce.SelectedValue == "-2147483648")
            {
                if (strArr[1] == "DN")
                {
                    dr["PAYMD_TYPE"] = 0;
                }

                if (strArr[1] == "BP")
                {
                    dr["PAYMD_TYPE"] = 1;
                }

                if (strArr[1] == "CN")
                {
                    dr["PAYMD_TYPE"] = 2;
                }
                if (strArr[1] == "INV")
                {
                    dr["PAYMD_TYPE"] = 3;

                }
            }
            if (ddlRefernce.SelectedValue == "-2147483647")
            {
                dr["PAYMD_TYPE"] = 7;
            }

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtRecievedAmount.Text = "";
                    txtAdjsAmt.Text = "";
                    txtRemark.Text = "";
                    ddlInvoiceNo.SelectedIndex = 0;
                    ddlRefernce.SelectedIndex = 0;
                    txtRemaningamt.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtRecievedAmount.Text = "";
                txtAdjsAmt.Text = "";
                txtRemark.Text = "";

                ddlInvoiceNo.SelectedIndex = 0;
                ddlRefernce.SelectedIndex = 0;
                txtRemaningamt.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion

            double Total_Amount = 0;

            for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
            {

                if (((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_TYPE")).Text == "0")
                    {
                        Total_Amount = Total_Amount - Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);
                    }

                
                else
                {
                    Total_Amount = Total_Amount + Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);
                }
            }
            txtAmount.Text = Total_Amount.ToString();
            //clearDetail();
            ddlRefernce.SelectedValue = "-2147483648";
            ddlInvoiceNo.Focus();
        }


        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "btnInsert_Click", Ex.Message);
        }


        ddlRefernce.Focus();
    }
    #endregion

    #region ddlInvoiceNo_SelectedIndexChanged
    protected void ddlRefernce_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRefernce.SelectedIndex != 0)
        {
            if (ddlRefernce.SelectedValue == "-2147483647")
            {
                ddlInvoiceNo.Items.Clear();
                txtRemaningamt.Text = "";
                txtRecievedAmount.Text = "";
                txtAdjsAmt.Text = "";
                DataTable dtItem = new DataTable();
                dtItem = CommonClasses.Execute("select * from NEWREFERENCE_MASTER where ES_DELETE=0   and REF_CM_COMP_ID='" + (string)Session["CompanyID"] + "'");
                ddlInvoiceNo.DataSource = dtItem;
                ddlInvoiceNo.DataTextField = "REF_NAME";
                ddlInvoiceNo.DataValueField = "REF_CODE";
                ddlInvoiceNo.DataBind();
                ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Reference ", "0"));
            }
            else
            {
                FillInvoiceNo();
            }
        }

    }
    #endregion
    #region Remainingamt
    public void Remainingamt()
    {
        string strInvoicType = ddlInvoiceNo.SelectedItem.Text;

        string[] strArr = null;
        strArr = strInvoicType.Split('-');
        DataTable dt1 = new DataTable();
        if (ddlInvoiceNo.SelectedIndex != 0)
        {


            if (ddlRefernce.SelectedValue == "-2147483648")
            {

                if (strArr[1] == "BP")
                {
                    dt1 = CommonClasses.Execute("select BPM_G_AMT-ISNULL(BPM_PAID_AMT,0) as BPM_G_AMT,'' as plusminus  from BILL_PASSING_MASTER where BPM_P_CODE="+ ddlCustomer.SelectedValue +" and  BPM_INV_NO='" + strArr[0] + "'");
                }
                if (strArr[1] == "DN")
                {
                    dt1 = CommonClasses.Execute("select DNM_NET_AMOUNT-ISNULL(DNM_PAID_AMT,0) as BPM_G_AMT ,plusminus from DEBIT_NOTE_MASTER where DNM_CUST_CODE=" + ddlCustomer.SelectedValue + " and   DNM_SERIAL_NO='" + strArr[0] + "'");
                }

                if (strArr[1] == "CN")
                {
                    dt1 = CommonClasses.Execute("select CNM_NET_AMOUNT-ISNULL(CNM_RECIEVED_AMT,0) as BPM_G_AMT,plusminus  from CREDIT_NOTE_MASTER where  CNM_CUST_CODE=" + ddlCustomer.SelectedValue + " and    CNM_SERIAL_NO='" + strArr[0] + "'");
                }


                if (strArr[1] == "INV")
                {
                    dt1 = CommonClasses.Execute("select INM_G_AMT-ISNULL(INM_RECIEVED_AMT,0) as BPM_G_AMT ,'' as plusminus from INVOICE_MASTER where ES_DELETE=0 AND INM_NO='" + strArr[0] + "'");
                }
                //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlRefernce.SelectedItem + "");

                if (dt1.Rows.Count > 0)
                {
                    //txtRemaningamt.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["BPM_G_AMT"].ToString();
                    //txtRecievedAmount.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["BPM_G_AMT"].ToString();

                    txtRemaningamt.Text = dt1.Rows[0]["BPM_G_AMT"].ToString();
                    txtRecievedAmount.Text = dt1.Rows[0]["BPM_G_AMT"].ToString();
                }
                else
                {

                }
                if (ddlInvoiceNo.SelectedIndex != 0)
                {

                }
                //txtRecievedAmount.Text = "";
                txtAdjsAmt.Text = "";
            }
        }
        else
        {
        }
    }
    #endregion
    #region ddlInvoiceNo_SelectedIndexChanged
    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Remainingamt();
    }
    #endregion

    #region RemainingamtModify
    public void RemainingamtModify()
    {
        string strInvoicType = ddlInvoiceNo.SelectedItem.Text;

        string[] strArr = null;
        strArr = strInvoicType.Split('-');
        DataTable dt1 = new DataTable();
        if (ddlInvoiceNo.SelectedIndex != 0)
        {


            if (ddlRefernce.SelectedValue == "-2147483648")
            {

                if (strArr[1] == "BP")
                {
                    dt1 = CommonClasses.Execute("select BPM_G_AMT-ISNULL(BPM_PAID_AMT,0) as BPM_G_AMT,'' as plusminus  from BILL_PASSING_MASTER where BPM_NO='" + strArr[0] + "'");
                }
                if (strArr[1] == "DN")
                {
                    dt1 = CommonClasses.Execute("select DNM_NET_AMOUNT-ISNULL(DNM_PAID_AMT,0) as BPM_G_AMT ,plusminus from DEBIT_NOTE_MASTER where DNM_SERIAL_NO='" + strArr[0] + "'");
                }

                if (strArr[1] == "CN")
                {
                    dt1 = CommonClasses.Execute("select CNM_NET_AMOUNT-ISNULL(CNM_RECIEVED_AMT,0) as BPM_G_AMT,plusminus  from CREDIT_NOTE_MASTER where CNM_SERIAL_NO='" + strArr[0] + "'");
                }

                if (strArr[1] == "INV")
                {
                    dt1 = CommonClasses.Execute("select INM_G_AMT-ISNULL(INM_RECIEVED_AMT,0) as BPM_G_AMT ,'' as plusminus from INVOICE_MASTER where ES_DELETE=0 AND INM_NO='" + strArr[0] + "'");
                }
                //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlRefernce.SelectedItem + "");

                if (dt1.Rows.Count > 0)
                {
                    //txtRemaningamt.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["BPM_G_AMT"].ToString();
                    //txtRecievedAmount.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["BPM_G_AMT"].ToString();

                    txtRemaningamt.Text = dt1.Rows[0]["BPM_G_AMT"].ToString();
                    //txtRecievedAmount.Text = dt1.Rows[0]["BPM_G_AMT"].ToString();
                }
                else
                {

                }
                if (ddlInvoiceNo.SelectedIndex != 0)
                {

                }
               
            }
        }
        else
        {
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            if (ddlCustomer.SelectedValue.ToString() == "0")
            {
                //LoadCombos();
            }
            else
            {
                ddlRefernce.Items.Clear();

                int id = Convert.ToInt32(ddlCustomer.SelectedValue);


                DataTable dt1 = CommonClasses.FillCombo("REFERENCE_MASTER", "REF_DESC", "REF_CODE", "ES_DELETE=0  and REF_CM_ID=" + (string)Session["CompanyId"] + "  ORDER BY REF_DESC", ddlRefernce);
                ddlRefernce.Items.Insert(0, new ListItem("Please Select Reference ", "0"));

                FillInvoiceNo();
                BlankGrid();

                ddlRefernce.SelectedValue = "-2147483648";
                ddlInvoiceNo.Focus();


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region FillInvoiceNo
    public void FillInvoiceNo()
    {
        ddlInvoiceNo.Items.Clear();

        DataTable dtItem = new DataTable();
        dtItem = CommonClasses.Execute("declare @temp table(BPM_CODE int IDENTITY (1,1),INVOICE_CODE int,BPM_NO nvarchar(500),INM_TYPE int)insert into @temp SELECT BPM_CODE,CONCAT(BPM_INV_NO,'-','BP') as BPM_NO ,1 AS BTYPE FROM BILL_PASSING_MASTER WHERE BILL_PASSING_MASTER.ES_DELETE=0 and BPM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ((ISNULL(BPM_PAID_AMT,0)) < (BPM_G_AMT))  and BPM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT DNM_CODE,CONCAT(DNM_SERIAL_NO,'-','DN'),0 AS BTYPE  FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 and DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "' AND ((ISNULL(DNM_PAID_AMT,0)) < (DNM_NET_AMOUNT))  and DNM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT CNM_CODE,CONCAT(CNM_SERIAL_NO,'-','CN'),2 AS BTYPE FROM CREDIT_NOTE_MASTER WHERE CREDIT_NOTE_MASTER.ES_DELETE=0 and CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'  AND ((ISNULL(CNM_RECIEVED_AMT,0)) < (CNM_NET_AMOUNT)) and CNM_CM_CODE='" + (string)Session["CompanyCode"] + "' UNION SELECT INM_CODE,CONCAT(INM_NO,'-','INV') as INM_NO,3 AS INM_TYPE FROM INVOICE_MASTER WHERE   INVOICE_MASTER.ES_DELETE=0 AND ((ISNULL(INM_RECIEVED_AMT,0)) < (INM_G_AMT))  and INM_P_CODE='" + ddlCustomer.SelectedValue + "'  and INM_CM_CODE='" + (string)Session["CompanyCode"] + "'   select * from @temp ");
        ddlInvoiceNo.DataSource = dtItem;
        ddlInvoiceNo.DataTextField = "BPM_NO";
        ddlInvoiceNo.DataValueField = "BPM_CODE";
        ddlInvoiceNo.DataBind();

        ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));


    }
    #endregion




    #region CtlDisable
    public void CtlDisable()
    {
        ddlCustomer.Enabled = false;
        txtPAYMno.Enabled = false;
        txtChequeNo.Enabled = false;

        txtChequeDate.Enabled = false;
        txtPAYMDate.Enabled = false;
        BtnInsert.Visible = false;
        btnSubmit.Visible = false;
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect(redirect, false);
            }
            else
            {
                if (CheckValid())
                {
                    ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payment Entry", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
            //SaveRec();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Payment Entry", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("PAYMENT_MASTER", "MODIFY", "PAYM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect(redirect, false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Payment Entry", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.Text == "")
            {
                flag = false;
            }
            else if (txtChequeNo.Text == "")
            {
                flag = false;
            }
            else if (txtChequeDate.Text == "")
            {
                flag = false;
            }
            else if (txtPAYMDate.Text == "")
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
            CommonClasses.SendError("Payment Entry", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    protected void txtRecievedAmount_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecievedAmount.Text);

        txtRecievedAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtAdjsAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtAdjsAmt.Text);

        txtAdjsAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));
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




}
//#region FillCustomer
//        try
//        {
//            DataTable dtParty = new DataTable();
//            //  dt = CommonClasses.FillCombo("PARTY_MASTER,BILL_PASSING_MASTER", "P_NAME", "P_CODE", "BILL_PASSING_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and PAYM_P_CODE=P_CODE  ORDER BY P_NAME", ddlCustomer);
//            if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
//            {
//                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE=0   AND PARTY_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_ACTIVE_IND=1  UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0  AND DNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0   AND CNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 ORDER BY P_NAME  ");
//            }
//            else
//            {
//                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE=0  AND PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_ACTIVE_IND=1  UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0  AND DNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0   AND CNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1  ORDER BY P_NAME ");
//            }
//            ddlCustomer.DataSource = dt;
//            ddlCustomer.DataTextField = "P_NAME";
//            ddlCustomer.DataValueField = "P_CODE";
//            ddlCustomer.DataBind();
//            ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
//        }
//        catch (Exception Ex)
//        {
//            CommonClasses.SendError("Payment Entry", "LoadCombos", Ex.Message);
//        }
//        #endregion