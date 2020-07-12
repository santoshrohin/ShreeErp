using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.IO;


public partial class Transactions_ADD_MaterialInward : System.Web.UI.Page
{
    #region General Declaration
    Inward_BL BL_InwardMaster = null;
    double PedQty = 0;
    static int mlCode = 0;
    static string right = "";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();

    string fileName = "";
    DirectoryInfo ObjSearchDir;
    #endregion

    public string Msg = "";

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ProductionMV");
        home1.Attributes["class"] = "active";

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
                ViewState["fileName"] = fileName;
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='10'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_InwardMaster = new Inward_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());

                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        BL_InwardMaster = new Inward_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        //txtGRNDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        //txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtGRNDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        txtChallanDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        BlankGrid();
                        LoadCombos();
                        LoadCurr();
                        dtFilter.Rows.Clear();

                        txtGRNDate.Attributes.Add("readonly", "readonly");
                        txtChallanDate.Attributes.Add("readonly", "readonly");
                        txtInvoiceDate.Attributes.Add("readonly", "readonly");

                    }
                    ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Material Inward", "PageLoad", ex.Message);
                }
            }
            if (IsPostBack && imgUpload.PostedFile != null)
            {
                if (imgUpload.PostedFile.FileName.Length > 0)
                {
                    fileName = imgUpload.PostedFile.FileName;
                    ViewState["fileName"] = fileName;
                    Upload(null, null);
                }
            }
        }
    }

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ProductionMV");
        home1.Attributes["class"] = "active";
        string sDirPath = Server.MapPath(@"~/UpLoadPath/InwardTemp/");
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/InwardTemp/" + ViewState["fileName"].ToString()));
        }
        else
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/InwardDoc/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
    }
    #endregion

    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_RATE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_UOM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REMARK", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_BATCH_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("DocName", typeof(string)));
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
        #region FillSupplier
        try
        {
            DataTable dtParty = new DataTable();
            //  dt = CommonClasses.FillCombo("PARTY_MASTER,SUPP_PO_MASTER", "P_NAME", "P_CODE", "SUPP_PO_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and SPOM_P_CODE=P_CODE  ORDER BY P_NAME", ddlSupplier);
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1  AND SPOM_POTYPE=0 AND SPOM_IS_SHORT_CLOSE=0  and SPOM_CANCEL_PO=0  AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0) AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0  and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND SPOM_POTYPE=0   AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0  and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
            //DataTable dtItem = new DataTable();
            //dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS WHERE ES_DELETE=0 and i_code=SPOD_I_CODE and (SPOD_INW_QTY) < (SPOD_ORDER_QTY)  and I_CM_COMP_ID=1  ORDER BY I_NAME");
            //ddlItemName.DataSource = dtItem;
            //ddlItemName.DataTextField = "I_NAME";
            //ddlItemName.DataValueField = "I_CODE";
            //ddlItemName.DataBind();
            dt = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS", "I_NAME", "I_CODE", "ES_DELETE=0 and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME", ddlItemName);
            ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));



            DataTable dt1 = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS", "I_CODENO", "I_CODE", "ES_DELETE=0 and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO", ddlItemCode);
            ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillUOM
        try
        {
            dt = CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_UOM_NAME", ddlUom);
            ddlUom.Items.Insert(0, new ListItem(" ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region PO
        try
        {
            dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
            ddlPoNumber.DataSource = dt;
            ddlPoNumber.DataTextField = "SPOM_PO_NO";
            ddlPoNumber.DataValueField = "SPOM_CODE";
            ddlPoNumber.DataBind();
            ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));

            //dt = CommonClasses.FillCombo("SUPP_PO_MASTER", "SPOM_PO_NO", "SPOM_CODE", "ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlPoNumber);
            //ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
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
                if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtGRNDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtGRNDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtChallanDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtChallanDate.Text))))
                {
                    if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                    {
                        // For Null File name 
                        if (ViewState["fileName"].ToString() == "" || ViewState["fileName"].ToString() == null)
                            ViewState["fileName"] = DBNull.Value;
                        SaveRec();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Material Inward", "btnSubmit_Click", Ex.Message);
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
            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGRNDate.Attributes.Add("readonly", "readonly");
            txtInvoiceDate.Attributes.Add("readonly", "readonly");

            LoadCombos();
            dt = CommonClasses.Execute(" SELECT IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,IWM_CODE,IWM_INWARD_TYPE,IWM_NO,IWM_TYPE,IWM_DATE,IWM_P_CODE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWM_EGP_NO,IWM_LR_NO,IWM_OCT_NO,IWM_VEH_NO,IWM_CM_CODE , IWM_CURR_CODE,ISNULL(IWM_CURR_RATE,0) AS IWM_CURR_RATE ,LTRIM(RTRIM( CURR_NAME)) AS CURR_NAME FROM INWARD_MASTER INNER JOIN CURRANCY_MASTER  ON INWARD_MASTER.IWM_CURR_CODE = CURRANCY_MASTER.CURR_CODE  WHERE INWARD_MASTER.ES_DELETE = 0 AND CURRANCY_MASTER.ES_DELETE=0  AND IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and IWM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["IWM_CODE"]);
                // IWM_CM_CODE = Convert.ToInt32(dt.Rows[0]["IWM_CM_CODE"]);
                //IWM_INWARD_TYPE = Convert.ToInt32(dt.Rows[0]["IWM_INWARD_TYPE"]);
                Currency(dt.Rows[0]["CURR_NAME"].ToString());
                //ddlCurrency.SelectedValue = dt.Rows[0]["IWM_CURR_CODE"].ToString();
                txtGRNno.Text = Convert.ToInt32(dt.Rows[0]["IWM_NO"]).ToString();
                //IWM_TYPE = (dt.Rows[0]["IWM_TYPE"]).ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_P_CODE"]).ToString();
                // ddlSupplier.SelectedValue(null, null);
                txtChallanNo.Text = (dt.Rows[0]["IWM_CHALLAN_NO"]).ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_CHAL_DATE"]).ToString("dd MMM yyyy");
                txtEgpNo.Text = (dt.Rows[0]["IWM_EGP_NO"]).ToString();
                txtLrNo.Text = (dt.Rows[0]["IWM_LR_NO"]).ToString();
                txtOctNo.Text = (dt.Rows[0]["IWM_OCT_NO"]).ToString();
                txtVehno.Text = (dt.Rows[0]["IWM_VEH_NO"]).ToString();
                txtInvoiceNo.Text = (dt.Rows[0]["IWM_INV_NO"]).ToString();
                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_INV_DATE"]).ToString("dd MMM yyyy");
                //ddlCurrency.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_CURR_CODE"]).ToString();
                //ddlCurrency_SelectedIndexChanged(null, null);
                txtCurrencyRate.Text = (dt.Rows[0]["IWM_CURR_RATE"]).ToString();
                if (txtInvoiceDate.Text == "01 Jan 1900")
                {
                    txtInvoiceDate.Text = "";
                }
                else
                {
                }
                txtTransporterName.Text = (dt.Rows[0]["IWM_TRANSPORATOR_NAME"]).ToString();
                //ddlSiteName.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_SITE_CODE"]).ToString();
                //ddlUom.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_UOM_CODE"]).ToString();
                ddlItemName.Items.Clear();
                int id = Convert.ToInt32(ddlSupplier.SelectedValue);
                //ddlSiteName.Enabled = false;
                CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_NAME", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemName);
                CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_CODENO", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemCode);
                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));

                ddlPoNumber.Items.Clear();
                // CommonClasses.FillCombo("SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM", "SPOM_PO_NO", "SPOM_CODE", " SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SM.SPOM_P_CODE=" + id + " and IM.ES_DELETE=0 and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]), ddlPoNumber);

                dt = CommonClasses.Execute("select  DISTINCT SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM ,COMPANY_MASTER where SPOM_CM_CODE=CM_CODE AND SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SM.SPOM_P_CODE=" + ddlSupplier.SelectedValue + " and IM.ES_DELETE=0 and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + " ");
                ddlPoNumber.DataSource = dt;
                ddlPoNumber.DataTextField = "SPOM_PO_NO";
                ddlPoNumber.DataValueField = "SPOM_CODE";
                ddlPoNumber.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("Please Select PO ", "0"));
                //ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));

                dtInwardDetail = CommonClasses.Execute("SELECT IWD_DOC_NAME as DocName, IWD_BATCH_NO,IWD_IWM_CODE,IWD_I_CODE,I_CODENO,I_NAME,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO,cast(IWD_RATE  as numeric(20,3)) as IWD_RATE,IWD_UOM_CODE,IWD_CPOM_CODE,cast(IWD_CH_QTY as numeric(10,3)) as IWD_CH_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME,IWD_REMARK FROM INWARD_DETAIL,INWARD_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_MASTER,COMPANY_MASTER WHERE IWM_CODE=IWD_IWM_CODE and IWD_I_CODE=ITEM_MASTER.I_CODE AND SPOM_CODE=IWD_CPOM_CODE AND IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  SPOM_CM_CODE=CM_CODE and IWD_IWM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    GetTotal();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }

            if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;

                ddlItemName.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlPoNumber.Enabled = false;
                txtChallanQty.Enabled = false;
                txtRecdQty.Enabled = false;
                txtRemark.Enabled = false;
                dgInwardMaster.Enabled = true;
                dgInwardMaster.Columns[0].Visible = false;
                dgInwardMaster.Columns[1].Visible = false;
                txtBatchNo.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtInvoiceDate.Enabled = false;
                txtTransporterName.Enabled = false;
            }

            if (str == "MOD")
            {
                txtChallanNo.Enabled = false;
                txtChallanDate.Enabled = false;
                txtEgpNo.Enabled = false;
                txtLrNo.Enabled = false;
                txtVehno.Enabled = false;
                txtTransporterName.Enabled = false;
                txtGRNDate.Enabled = false;
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadCurr
    private void LoadCurr()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER WHERE ES_DELETE = 0 AND CURR_CM_COMP_ID = " + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
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

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlSupplier.SelectedValue = BL_InwardMaster.IWM_P_CODE.ToString();
                txtGRNno.Text = BL_InwardMaster.IWM_NO.ToString();
                txtChallanNo.Text = BL_InwardMaster.IWM_CHALLAN_NO.ToString();
                txtEgpNo.Text = BL_InwardMaster.IWM_EGP_NO.ToString();
                txtLrNo.Text = BL_InwardMaster.IWM_LR_NO.ToString();
                txtOctNo.Text = BL_InwardMaster.IWM_OCT_NO.ToString();
                txtVehno.Text = BL_InwardMaster.IWM_VEH_NO.ToString();
                txtGRNDate.Text = BL_InwardMaster.IWM_DATE.ToString("dd MMM YYYY");
                txtChallanDate.Text = BL_InwardMaster.IWM_CHAL_DATE.ToString("dd MMM YYYY");
                ddlCurrency.SelectedValue = BL_InwardMaster.IWM_CURR_CODE.ToString();
                txtCurrencyRate.Text = BL_InwardMaster.IWM_CURR_RATE.ToString();
            }
            else if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;
                txtGRNno.Enabled = false;
                txtChallanNo.Enabled = false;
                txtEgpNo.Enabled = false;
                txtLrNo.Enabled = false;
                txtOctNo.Enabled = false;
                txtVehno.Enabled = false;
                txtChallanDate.Enabled = false;
                txtGRNDate.Enabled = false;
                BtnInsert.Visible = false;
                btnSubmit.Visible = false;
                ddlCurrency.Enabled = false;
                txtCurrencyRate.Enabled = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "GetValues", ex.Message);
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
            BL_InwardMaster.IWM_P_CODE = Convert.ToInt32(ddlSupplier.SelectedValue);
            //BL_InwardMaster.IWM_SITE_CODE = Convert.ToInt32(ddlSiteName.SelectedValue);
            BL_InwardMaster.IWM_NO = Convert.ToInt32(txtGRNno.Text);
            BL_InwardMaster.IWM_CHALLAN_NO = txtChallanNo.Text.ToString();
            BL_InwardMaster.IWM_TYPE = "IWIM";

            BL_InwardMaster.IWM_CHAL_DATE = Convert.ToDateTime(txtChallanDate.Text);
            BL_InwardMaster.IWM_DATE = Convert.ToDateTime(txtGRNDate.Text);
            BL_InwardMaster.IWM_EGP_NO = txtEgpNo.Text.ToString();
            BL_InwardMaster.IWM_LR_NO = txtLrNo.Text.ToString();
            BL_InwardMaster.IWM_OCT_NO = txtOctNo.Text.ToString();
            BL_InwardMaster.IWM_VEH_NO = txtVehno.Text.ToString();
            BL_InwardMaster.IWM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            BL_InwardMaster.IWM_INV_NO = txtInvoiceNo.Text;

            if (txtInvoiceDate.Text == "")
            {
                BL_InwardMaster.IWM_INV_DATE = Convert.ToDateTime("1/1/1900");
            }
            else
            {
                BL_InwardMaster.IWM_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text);
            }
            BL_InwardMaster.IWM_TRANSPORATOR_NAME = txtTransporterName.Text;
            BL_InwardMaster.IWD_BATCH_NO = txtBatchNo.Text;
            BL_InwardMaster.IWM_CURR_CODE = Convert.ToInt32(ddlCurrency.SelectedValue.ToString());
            BL_InwardMaster.IWM_CURR_RATE = Convert.ToDouble(txtCurrencyRate.Text.ToString());
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "Setvalues", ex.Message);
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
                BL_InwardMaster = new Inward_BL();
                txtGRNno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_InwardMaster.Save(dgInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(IWM_CODE) from INWARD_MASTER");
                        //CommonClasses.WriteLog("Material Inward", "Save", "Material Inward", BL_InwardMaster.IWM_EGP_NO, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]),(Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        /*UploadImage in InwardDoc Folder and Save_File name in DB*/
                        #region UploadImage
                        for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                        {
                            if (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text != "")
                            {
                                string sDirPath = Server.MapPath(@"~/UpLoadPath/InwardDoc/" + Code + "");
                                ObjSearchDir = new DirectoryInfo(sDirPath);
                                string sDirPath1 = Server.MapPath(@"~/UpLoadPath/InwardTemp");
                                DirectoryInfo dir = new DirectoryInfo(sDirPath1);
                                dir.Refresh();

                                if (!ObjSearchDir.Exists)
                                {
                                    ObjSearchDir.Create();
                                }
                                string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
                                string fullFilePath1 = currentApplicationPath + "UpLoadPath\\InwardTemp";
                                string fullFilePath = Server.MapPath(@"~/UpLoadPath/InwardTemp/" + (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text));
                                string copyToPath = Server.MapPath(@"~/UpLoadPath/InwardDoc/" + Code + "/" + (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text));
                                DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                                FileInfo[] fi = di.GetFiles();
                                System.IO.File.Move(fullFilePath, copyToPath);
                                //DataTable dtUpdateDocName = CommonClasses.Execute("update INWARD_DETAIL set IWD_DOC_NAME='" + (((LinkButton)dgInwardMaster.Rows[i].FindControl("lnkView")).Text) + "' where ");
                            }
                        }
                        #endregion UploadImage
                        CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_TUR_WEIGHT=1 where IWD_IWM_CODE='" + Code + "'");
                        CommonClasses.WriteLog("Material Inward ", "Insert", "Material Inward", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
                    }
                    else
                    {
                        if (BL_InwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_InwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_InwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
            #endregion

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_InwardMaster = new Inward_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_InwardMaster.Update(dgInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_TUR_WEIGHT=1 where IWD_IWM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        // CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward",  Convert.ToInt32(ViewState["mlCode"].ToString()) , Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
                    }
                    else
                    {
                        if (BL_InwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_InwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_InwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Material Inward", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(IWM_NO) as IWM_NO from INWARD_MASTER where IWM_CM_CODE='" + Session["CompanyCode"] + "' AND IWM_TYPE='IWIM' and ES_DELETE=0");
        if (dt.Rows[0]["IWM_NO"] == null || dt.Rows[0]["IWM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["IWM_NO"]) + 1;
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

            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            #region Delete
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
            }
            #endregion Delete

            #region Modify
            if (e.CommandName == "Select")
            {
                LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                lnkDelete.Enabled = false;

                //dgInwardMaster.Columns[1].Visible = false;

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                string s = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlPoNumber.SelectedValue = ((Label)(row.FindControl("lblIWD_CPOM_CODE"))).Text;
                txtRecdQty.Text = ((Label)(row.FindControl("lblIWD_REV_QTY"))).Text;
                if (txtRecdQty.Text == "")
                {
                    txtRecdQty.Text = "0";
                }
                /*Can't Set Value To Fileupload Control*/
                string DocName = ((LinkButton)(row.FindControl("lnkView"))).Text;
                ViewState["fileName"] = DocName;
                // imgUpload.FileName = Convert.ToString(DocName);// (((LinkButton)dgInwardMaster.Rows[0].FindControl("lnkView")).Text);
                txtRate.Text = ((Label)(row.FindControl("lblIWD_RATE"))).Text;
                //txtRate.Text = ((Label)(row.FindControl("lblSPOD_RATE"))).Text;
                txtChallanQty.Text = ((Label)(row.FindControl("lblIWD_CH_QTY"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblIWD_REMARK"))).Text;
                dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]) + Convert.ToDouble(txtRecdQty.Text);
                }
                else
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]);
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                ddlUom.SelectedValue = ((Label)(row.FindControl("lblUOM_CODE"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblIWD_BATCH_NO"))).Text;
            }
            #endregion Modify

            #region ViewPDF
            if (e.CommandName == "ViewPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    IframeViewPDF.Attributes["src"] = "";
                    directory = "";
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/InwardTemp/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/InwardDoc/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            #endregion ViewPDF

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedValue = "0";
            ddlItemCode.SelectedValue = "0";
            ddlSupplier.Items.Insert(0, new ListItem("Please Select PO ", "0"));
            ddlPoNumber.SelectedValue = "0";
            txtRate.Text = "0.00";
            txtChallanQty.Text = "0.00";
            txtRecdQty.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Material Inward ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;

            if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Supplier Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }

            if (txtChallanNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Challan/Invoice No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;

            }

            if (Convert.ToInt32(ddlItemName.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (Convert.ToInt32(ddlPoNumber.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select PO";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlPoNumber.Focus();
                return;
            }

            if (txtChallanQty.Text == "" || Convert.ToDecimal(txtChallanQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
                return;
            }
            double chq = Convert.ToDouble(txtChallanQty.Text);

            double pdq = Convert.ToDouble(txtPendingQty.Text);
            //@@@@@@@@
            if (pdq < chq)
            {
                DataTable dtCheckOpen = CommonClasses.Execute("SELECT SPOD_ORDER_QTY FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOD_ORDER_QTY>0");
                if (dtCheckOpen.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Visible = true;
                    lblmsg.Text = "Challan quantity should always be less or equal to pending quantity";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtChallanQty.Text = "0";
                    txtChallanQty.Focus();
                    return;
                }
            }


            if (txtRecdQty.Text == "" || Convert.ToDecimal(txtRecdQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recd. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecdQty.Focus();
                return;
            }
            double req = Convert.ToDouble(txtRecdQty.Text);

            if (chq < req)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recd. Qty less or equal to Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecdQty.Text = "";
                txtRecdQty.Focus();
                return;
            }

            //if ( Convert.ToInt32(ddlPoNumber.SelectedValue) >= 0)
            //{
            //    ShowMessage("#Avisos", "Select Po Number", CommonClasses.MSG_Warning);

            //    ddlPoNumber.Focus();
            //    return;
            //}
            PanelMsg.Visible = false;

            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIWD_I_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }

                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_I_CODE");

                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_CPOM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SPOM_PO_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_CH_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_REV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_UOM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_BATCH_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("DocName");

            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IWD_I_CODE"] = ddlItemName.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["I_NAME"] = ddlItemName.SelectedItem;
            dr["IWD_CPOM_CODE"] = ddlPoNumber.SelectedValue;
            dr["SPOM_PO_NO"] = ddlPoNumber.SelectedItem.Text;
            dr["IWD_RATE"] = string.Format("{0:0.0000}", (Convert.ToDouble(txtRate.Text)));
            dr["IWD_CH_QTY"] = string.Format("{0:0.0000}", (Convert.ToDouble(txtChallanQty.Text)));
            dr["IWD_REV_QTY"] = string.Format("{0:0.0000}", (Convert.ToDouble(txtRecdQty.Text)));
            dr["IWD_REMARK"] = txtRemark.Text;
            dr["I_UOM_NAME"] = ddlUom.SelectedItem;
            dr["IWD_UOM_CODE"] = ddlUom.SelectedValue;
            dr["IWD_BATCH_NO"] = txtBatchNo.Text;
            fileName = ViewState["fileName"].ToString();
            dr["DocName"] = fileName;

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtChallanQty.Text = "";
                    txtRecdQty.Text = "";
                    txtRemark.Text = "";
                    ddlItemName.SelectedIndex = 0;
                    ddlItemCode.SelectedIndex = 0;
                    txtPendingQty.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtChallanQty.Text = "";
                txtRecdQty.Text = "";
                txtRemark.Text = "";

                ddlItemName.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                txtPendingQty.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            GetTotal();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            ViewState["fileName"] = "";
            #endregion

            //clearDetail();
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.SelectedIndex != 0)
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME ,I_E_CODE ,ISNULL((SELECT E_H_EDU  FROM EXCISE_TARIFF_MASTER where E_CODE=I_E_CODE AND ES_DELETE=0),0) AS E_H_EDU from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
            //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                // lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                txtIGST.Text = dt1.Rows[0]["E_H_EDU"].ToString();
            }
            else
            {
                txtIGST.Text = "0";
                ddlUom.Text = "";
            }


            LoadPO();

        }
        else
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            LoadPO();
        }
        txtChallanQty.Text = "";
        txtRecdQty.Text = "";
        pendingQty();
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (ddlItemName.SelectedIndex != 0)
        {

            ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_E_CODE ,ISNULL((SELECT E_H_EDU  FROM EXCISE_TARIFF_MASTER where E_CODE=I_E_CODE AND ES_DELETE=0),0) AS E_H_EDU  from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
            //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                // lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                txtIGST.Text = dt1.Rows[0]["E_H_EDU"].ToString();
            }
            else
            {
                txtIGST.Text = "0";
                ddlUom.Text = "";
            }

            LoadPO();

            txtChallanQty.Text = "";
            txtRecdQty.Text = "";
            pendingQty();
        }
        else
        {
            ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
            LoadPO();
        }
    }
    #endregion

    #region ddlPoNumber_SelectedIndexChanged
    protected void ddlPoNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtChallanQty.Text = "";
        txtRecdQty.Text = "";
        try
        {


            DataTable dtItem = new DataTable();
            if (ddlPoNumber.SelectedValue.ToString() == "0")
            {
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SPOM_POTYPE=0   AND  SPOM_POST=1 AND SPOM_P_CODE=" + ddlSupplier.SelectedValue + "   ORDER BY I_NAME");
            }
            else
            {
                if (ddlItemCode.SelectedValue.ToString() == "0")
                {
                    dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SPOM_POTYPE=0   AND  SPOM_POST=1 AND SPOM_P_CODE=" + ddlSupplier.SelectedValue + "  AND SPOM_CODE='" + ddlPoNumber.SelectedValue + "' ORDER BY I_NAME");
                }
            }
            if (dtItem.Rows.Count > 0)
            {


                ddlItemName.DataSource = dtItem;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemCode.DataSource = dtItem;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();

                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));
            }
            pendingQty();

            /*
            
             */
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            if (ddlSupplier.SelectedValue.ToString() == "0")
            {
                LoadCombos();
            }
            else
            {


                int id = Convert.ToInt32(ddlSupplier.SelectedValue);
                ddlItemName.Items.Clear();
                ddlItemCode.Items.Clear();
                DataTable dtItem = new DataTable();
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SPOM_POTYPE=0   AND  SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
                ddlItemName.DataSource = dtItem;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemCode.DataSource = dtItem;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();

                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));
                BlankGrid();


                ddlPoNumber.Items.Clear();

                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE   AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   order by SPOM_CODE  ");

                }
                else
                {
                    dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE   AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0) order by SPOM_CODE  ");

                }
                ddlPoNumber.DataSource = dtPO;
                ddlPoNumber.DataTextField = "SPOM_PO_NO";
                ddlPoNumber.DataValueField = "SPOM_CODE";
                ddlPoNumber.DataBind();

                ddlPoNumber.Items.Insert(0, new ListItem("Select PO ", "0"));

                // Cuurency Bind From Currency Master and Purchase Master
                DataTable dtCurrncy = CommonClasses.Execute("SELECT * FROM CURRANCY_MASTER INNER JOIN SUPP_PO_MASTER ON CURRANCY_MASTER.CURR_CODE = SUPP_PO_MASTER.SPOM_CURR_CODE   WHERE CURRANCY_MASTER.ES_DELETE = 0 AND SUPP_PO_MASTER.ES_DELETE = 0  AND SPOM_P_CODE = '" + ddlSupplier.SelectedValue + "' AND CURR_CM_COMP_ID ='" + (string)Session["CompanyId"] + "'  ORDER BY SPOM_DATE DESC");
                ddlCurrency.DataSource = dtCurrncy;
                ddlCurrency.DataTextField = "CURR_NAME";
                ddlCurrency.DataValueField = "CURR_CODE";
                ddlCurrency.DataBind();
                Currency(dtCurrncy.Rows[0]["CURR_NAME"].ToString());
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ddlSupplier_SelectedIndexChanged", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region pendingQty
    private void pendingQty()
    {
        #region PendingQty
        try
        {
            dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,  CASE when isnull(SPOD_ORDER_QTY,0)=0 then SPOD_RATE else SPOD_RATE - ROUND((SPOD_DISC_AMT/SPOD_ORDER_QTY),2) end   AS SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
                {
                    txtChallanQty.Enabled = false;
                    txtRecdQty.Enabled = false;
                    // txtPendingQty.Text = "0";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Only Post PO Can InWard";
                    // ddlPoNumber.Enabled = false;
                }
                else
                {

                    PanelMsg.Visible = false;
                    txtChallanQty.Enabled = true;
                    txtRecdQty.Enabled = true;
                }
                txtPendingQty.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
                txtRate.Text = string.Format("{0:0.0000}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
                ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "PendingQty", Ex.Message);
        }
        #endregion
    }
    #endregion


    #region LoadPO
    private void LoadPO()
    {
        ddlPoNumber.Items.Clear();

        string Cond = "";
        if (ddlItemCode.SelectedValue.ToString() != "0")
        {
            Cond = "  AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' ";
        }
        if (Request.QueryString[0].Equals("MODIFY"))
        {
            dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE   AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   " + Cond + "  order by SPOM_CODE  ");

        }
        else
        {
            dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE   AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   " + Cond + "   AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0) order by SPOM_CODE  ");

        }
        ddlPoNumber.DataSource = dtPO;
        ddlPoNumber.DataTextField = "SPOM_PO_NO";
        ddlPoNumber.DataValueField = "SPOM_CODE";
        ddlPoNumber.DataBind();

        ddlPoNumber.Items.Insert(0, new ListItem("Select PO ", "0"));
    }
    #endregion

    #region CtlDisable
    public void CtlDisable()
    {
        ddlSupplier.Enabled = false;
        txtGRNno.Enabled = false;
        txtChallanNo.Enabled = false;
        txtEgpNo.Enabled = false;
        txtLrNo.Enabled = false;
        txtOctNo.Enabled = false;
        txtVehno.Enabled = false;
        txtChallanDate.Enabled = false;
        txtGRNDate.Enabled = false;
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
                Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Material Inward", "btnCancel_Click", Ex.Message);
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

            CommonClasses.SendError("Material Inward", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region lnkPO_click
    protected void lnkPO_click(object sender, EventArgs e)
    {
        if (ddlPoNumber.SelectedValue != "0")
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('YourURL','_newtab');", true);
            Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=AMEND&cpom_code=" + ddlPoNumber.SelectedValue, false);
            //Server.Transfer("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=AMEND&cpom_code=" + ddlPoNumber.SelectedValue, false);

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
                CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSupplier.Text == "")
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
            else if (txtGRNDate.Text == "")
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
            CommonClasses.SendError("Material Inward", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);

        txtChallanQty.Text = string.Format("{0:0.0000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtRecdQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecdQty.Text);

        txtRecdQty.Text = string.Format("{0:0.0000}", Math.Round(Convert.ToDouble(totalStr), 3));
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

    #region ddlCurrency_SelectedIndexChanged
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Currency(ddlCurrency.SelectedItem.ToString());
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion ddlCurrency_SelectedIndexChanged

    #region Currency
    public void Currency(string str)
    {
        try
        {
            DataTable dtCurrncy = CommonClasses.Execute("SELECT * FROM CURRANCY_MASTER WHERE CURR_NAME ='" + str.ToString() + "' AND CURRANCY_MASTER.ES_DELETE = 0 ");
            if (dtCurrncy.Rows[0]["CURR_SHORT_NAME"].ToString() == "Rs")
            {
                txtCurrencyRate.Text = string.Format("{0:0.0000}", Math.Round(Convert.ToDouble(dtCurrncy.Rows[0]["CURR_RATE"]), 4));
                txtCurrencyRate.Enabled = false;
                ddlCurrency.DataSource = dtCurrncy;
                ddlCurrency.DataValueField = "CURR_CODE";
                ddlCurrency.DataTextField = "CURR_NAME";
                ddlCurrency.DataBind();
            }
            else
            {
                ddlCurrency.DataSource = dtCurrncy;
                ddlCurrency.DataValueField = "CURR_CODE";
                ddlCurrency.DataTextField = "CURR_NAME";
                ddlCurrency.DataBind();
                txtCurrencyRate.Text = string.Format("{0:0.0000}", Math.Round(Convert.ToDouble(dtCurrncy.Rows[0]["CURR_RATE"]), 4));
                txtCurrencyRate.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion Currency
    protected void txtChallanNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0 || ddlSupplier.SelectedValue == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Supplier Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlSupplier.Focus();
            return;
        }

        dt = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE ES_DELETE=0  AND IWM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND IWM_CHALLAN_NO='" + txtChallanNo.Text.Trim().Replace("'", "''") + "' AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "'");
        if (dt.Rows.Count > 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Challan/Invoice No.  Already Exist For this party";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtChallanNo.Text = "";
            txtChallanNo.Focus();
            return;
        }
    }

    public void GetTotal()
    {
        double amt = 0;
        for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
        {
            double IWD_REV_QTY = Convert.ToDouble(((Label)(dgInwardMaster.Rows[i].FindControl("lblIWD_REV_QTY"))).Text);
            double IWD_RATE = Convert.ToDouble(((Label)(dgInwardMaster.Rows[i].FindControl("lblIWD_RATE"))).Text);

            amt = amt + Math.Round(IWD_REV_QTY * IWD_RATE);
        }
        txtamt.Text = amt.ToString();
    }
}
