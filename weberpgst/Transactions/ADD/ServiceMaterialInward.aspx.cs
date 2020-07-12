using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Transactions_ADD_ServiceMaterialInward : System.Web.UI.Page
{
    #region General Declaration
    ServiceInward_BL BL_ServiceInwardMaster = null;
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

                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='10'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_ServiceInwardMaster = new ServiceInward_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());

                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        BL_ServiceInwardMaster = new ServiceInward_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtGRNDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                        BlankGrid();
                        LoadCombos();

                        dtFilter.Rows.Clear();

                        txtGRNDate.Attributes.Add("readonly", "readonly");
                        txtChallanDate.Attributes.Add("readonly", "readonly");
                        txtInvoiceDate.Attributes.Add("readonly", "readonly");

                    }
                    ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Service Inward", "PageLoad", ex.Message);
                }
            }

        }
    }

    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgServiceInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("S_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("S_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_CPOM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SRPOM_PO_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_REV_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_RATE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_CH_QTY", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("SID_REMARK", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SID_BATCH_NO", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgServiceInwardMaster.DataSource = dtFilter;
            dgServiceInwardMaster.DataBind();
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
                //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1  AND SPOM_POTYPE=0 AND SPOM_IS_SHORT_CLOSE=0  and SPOM_CANCEL_PO=0  AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0) AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME ");
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SRPOM_P_CODE=P_CODE AND SRPOM_CODE=SRPOD_SPOM_CODE AND SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_POST=1  AND SRPOM_POTYPE=0 AND SRPOM_IS_SHORT_CLOSE=0  and SRPOM_CANCEL_PO=0  AND ((SRPOD_INW_QTY) < (SRPOD_ORDER_QTY) OR SRPOD_ORDER_QTY=0) AND SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SRPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            else
            {
                //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND SPOM_POTYPE=0   AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME ");
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SRPOM_P_CODE=P_CODE AND SRPOM_CODE=SRPOD_SPOM_CODE AND SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_POST=1 AND SRPOM_POTYPE=0   AND  SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SRPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME");
            }
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
            //DataTable dtItem = new DataTable();
            //dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS WHERE ES_DELETE=0 and i_code=SPOD_I_CODE and (SPOD_INW_QTY) < (SPOD_ORDER_QTY)  and I_CM_COMP_ID=1  ORDER BY I_NAME");
            //ddlServiceName.DataSource = dtItem;
            //ddlServiceName.DataTextField = "I_NAME";
            //ddlServiceName.DataValueField = "I_CODE";
            //ddlServiceName.DataBind();
            dt = CommonClasses.FillCombo("SERVICE_TYPE_MASTER,SERVICE_PO_DETAILS", "S_NAME", "S_CODE", "ES_DELETE=0 and S_CODE=SRPOD_I_CODE and (ISNULL(SRPOD_ORDER_QTY,0)-ISNULL(SRPOD_INW_QTY,0))>0 and S_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY S_NAME", ddlServiceName);
            ddlServiceName.Items.Insert(0, new ListItem("Please Select Service ", "0"));



            DataTable dt1 = CommonClasses.FillCombo("SERVICE_TYPE_MASTER,SERVICE_PO_DETAILS", "S_CODENO", "S_CODE", "ES_DELETE=0 and S_CODE=SRPOD_I_CODE and (ISNULL(SRPOD_ORDER_QTY,0)-ISNULL(SRPOD_INW_QTY,0))>0 and S_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY S_CODENO", ddlServiceCode);
            ddlServiceCode.Items.Insert(0, new ListItem("Please Select Service ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "LoadCombos", Ex.Message);
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
            CommonClasses.SendError("Service Inward", "LoadCombos", Ex.Message);
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
            CommonClasses.SendError("Service Inward", "LoadCombos", Ex.Message);
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
                    if (dgServiceInwardMaster.Rows.Count > 0 && dgServiceInwardMaster.Enabled)
                    {
                        SaveRec();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlServiceName.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlServiceName.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Service Inward", "btnSubmit_Click", Ex.Message);
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
            dt = CommonClasses.Execute("SELECT SIM_INV_NO,SIM_INV_DATE,SIM_TRANSPORATOR_NAME,SIM_CODE,SIM_INWARD_TYPE,SIM_NO,SIM_TYPE,SIM_DATE,SIM_P_CODE,SIM_CHALLAN_NO,SIM_CHAL_DATE,SIM_EGP_NO,SIM_LR_NO,SIM_OCT_NO,SIM_VEH_NO,SIM_CM_CODE FROM SERVICE_INWARD_MASTER where ES_DELETE=0 and SIM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and SIM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {


                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SIM_CODE"]);
                // SIM_CM_CODE = Convert.ToInt32(dt.Rows[0]["SIM_CM_CODE"]);
                //SIM_INWARD_TYPE = Convert.ToInt32(dt.Rows[0]["SIM_INWARD_TYPE"]);
                txtGRNno.Text = Convert.ToInt32(dt.Rows[0]["SIM_NO"]).ToString();
                //SIM_TYPE = (dt.Rows[0]["SIM_TYPE"]).ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["SIM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = Convert.ToInt32(dt.Rows[0]["SIM_P_CODE"]).ToString();
                // ddlSupplier.SelectedValue(null, null);
                txtChallanNo.Text = (dt.Rows[0]["SIM_CHALLAN_NO"]).ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["SIM_CHAL_DATE"]).ToString("dd MMM yyyy");
                txtEgpNo.Text = (dt.Rows[0]["SIM_EGP_NO"]).ToString();
                txtLrNo.Text = (dt.Rows[0]["SIM_LR_NO"]).ToString();
                txtOctNo.Text = (dt.Rows[0]["SIM_OCT_NO"]).ToString();
                txtVehno.Text = (dt.Rows[0]["SIM_VEH_NO"]).ToString();

                txtInvoiceNo.Text = (dt.Rows[0]["SIM_INV_NO"]).ToString();

                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["SIM_INV_DATE"]).ToString("dd MMM yyyy");
                if (txtInvoiceDate.Text == "01 Jan 1900")
                {
                    txtInvoiceDate.Text = "";
                }
                else
                {

                }


                txtTransporterName.Text = (dt.Rows[0]["SIM_TRANSPORATOR_NAME"]).ToString();
                //ddlSiteName.SelectedValue = Convert.ToInt32(dt.Rows[0]["SIM_SITE_CODE"]).ToString();
                //ddlUom.SelectedValue = Convert.ToInt32(dt.Rows[0]["SIM_UOM_CODE"]).ToString();
                ddlServiceName.Items.Clear();
                int id = Convert.ToInt32(ddlSupplier.SelectedValue);
                //ddlSiteName.Enabled = false;
                CommonClasses.FillCombo("SERVICE_TYPE_MASTER IM,PARTY_MASTER PM,SERVICE_PO_MASTER SM,SERVICE_PO_DETAILS SD ", "S_NAME", "S_CODE", "SM.SRPOM_P_CODE=" + id + " and SM.SRPOM_CODE=SD.SRPOD_SPOM_CODE and SD.SRPOD_I_CODE=IM.S_CODE and IM.ES_DELETE=0 and S_CM_COMP_ID=" + (string)Session["CompanyId"], ddlServiceName);
                CommonClasses.FillCombo("SERVICE_TYPE_MASTER IM,PARTY_MASTER PM,SERVICE_PO_MASTER SM,SERVICE_PO_DETAILS SD ", "S_CODENO", "S_CODE", "SM.SRPOM_P_CODE=" + id + " and SM.SRPOM_CODE=SD.SRPOD_SPOM_CODE and SD.SRPOD_I_CODE=IM.S_CODE and IM.ES_DELETE=0 and S_CM_COMP_ID=" + (string)Session["CompanyId"], ddlServiceCode);
                ddlServiceName.Items.Insert(0, new ListItem("Please Select Service Name ", "0"));
                ddlServiceCode.Items.Insert(0, new ListItem("Please Select Service Code", "0"));

                ddlPoNumber.Items.Clear();
                // CommonClasses.FillCombo("SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM", "SPOM_PO_NO", "SPOM_CODE", " SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SM.SPOM_P_CODE=" + id + " and IM.ES_DELETE=0 and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]), ddlPoNumber);

                dt = CommonClasses.Execute("select  DISTINCT SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM ,COMPANY_MASTER where SPOM_CM_CODE=CM_CODE AND SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SM.SPOM_P_CODE=" + ddlSupplier.SelectedValue + " and IM.ES_DELETE=0 and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + " AND SPOM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"].ToString()) + "'");
                ddlPoNumber.DataSource = dt;
                ddlPoNumber.DataTextField = "SPOM_PO_NO";
                ddlPoNumber.DataValueField = "SPOM_CODE";
                ddlPoNumber.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("Please Select PO ", "0"));
                //ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));

                dtInwardDetail = CommonClasses.Execute("SELECT SID_BATCH_NO,SID_SIM_CODE,SID_I_CODE,S_CODENO,S_NAME,Convert(varchar,SRPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SRPOM_PO_NO,cast(SID_RATE  as numeric(20,3)) as SID_RATE,SID_UOM_CODE,SID_CPOM_CODE,cast(SID_CH_QTY as numeric(10,3)) as SID_CH_QTY,cast(SID_REV_QTY as numeric(10,3)) as SID_REV_QTY,SID_REMARK FROM SERVICE_INWARD_DETAIL,SERVICE_INWARD_MASTER,SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,COMPANY_MASTER WHERE SIM_CODE=SID_SIM_CODE and SID_I_CODE=SERVICE_TYPE_MASTER.S_CODE AND SRPOM_CODE=SID_CPOM_CODE and SIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  SRPOM_CM_CODE=CM_CODE and SID_SIM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgServiceInwardMaster.DataSource = dtInwardDetail;
                    dgServiceInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }

            if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;

                ddlServiceName.Enabled = false;
                ddlServiceCode.Enabled = false;
                ddlPoNumber.Enabled = false;
                txtChallanQty.Enabled = false;
                txtRecdQty.Enabled = false;
                txtRemark.Enabled = false;
                dgServiceInwardMaster.Enabled = false;
                txtBatchNo.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtInvoiceDate.Enabled = false;
                txtTransporterName.Enabled = false;

            }

            if (str == "MOD")
            {
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("SERVICE_INWARD_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "ViewRec", Ex.Message);
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
                ddlSupplier.SelectedValue = BL_ServiceInwardMaster.SIM_P_CODE.ToString();
                txtGRNno.Text = BL_ServiceInwardMaster.SIM_NO.ToString();
                txtChallanNo.Text = BL_ServiceInwardMaster.SIM_CHALLAN_NO.ToString();
                txtEgpNo.Text = BL_ServiceInwardMaster.SIM_EGP_NO.ToString();
                txtLrNo.Text = BL_ServiceInwardMaster.SIM_LR_NO.ToString();
                txtOctNo.Text = BL_ServiceInwardMaster.SIM_OCT_NO.ToString();
                txtVehno.Text = BL_ServiceInwardMaster.SIM_VEH_NO.ToString();
                txtGRNDate.Text = BL_ServiceInwardMaster.SIM_DATE.ToString("dd MMM YYYY");
                txtChallanDate.Text = BL_ServiceInwardMaster.SIM_CHAL_DATE.ToString("dd MMM YYYY");

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

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Inward", "GetValues", ex.Message);
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


            BL_ServiceInwardMaster.SIM_P_CODE = Convert.ToInt32(ddlSupplier.SelectedValue);
            //BL_ServiceInwardMaster.SIM_SITE_CODE = Convert.ToInt32(ddlSiteName.SelectedValue);
            BL_ServiceInwardMaster.SIM_NO = Convert.ToInt32(txtGRNno.Text);
            BL_ServiceInwardMaster.SIM_CHALLAN_NO = txtChallanNo.Text.ToString();
            BL_ServiceInwardMaster.SIM_TYPE = "SIWM";

            BL_ServiceInwardMaster.SIM_CHAL_DATE = Convert.ToDateTime(txtChallanDate.Text);
            BL_ServiceInwardMaster.SIM_DATE = Convert.ToDateTime(txtGRNDate.Text);
            BL_ServiceInwardMaster.SIM_EGP_NO = txtEgpNo.Text.ToString();
            BL_ServiceInwardMaster.SIM_LR_NO = txtLrNo.Text.ToString();
            BL_ServiceInwardMaster.SIM_OCT_NO = txtOctNo.Text.ToString();
            BL_ServiceInwardMaster.SIM_VEH_NO = txtVehno.Text.ToString();
            BL_ServiceInwardMaster.SIM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            BL_ServiceInwardMaster.SIM_INV_NO = txtInvoiceNo.Text;

            if (txtInvoiceDate.Text == "")
            {
                BL_ServiceInwardMaster.SIM_INV_DATE = Convert.ToDateTime("1/1/1900");
            }
            else
            {
                BL_ServiceInwardMaster.SIM_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text);
            }
            BL_ServiceInwardMaster.SIM_TRANSPORATOR_NAME = txtTransporterName.Text;
            BL_ServiceInwardMaster.SID_BATCH_NO = txtBatchNo.Text;


            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Inward", "Setvalues", ex.Message);
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
                BL_ServiceInwardMaster = new ServiceInward_BL();
                txtGRNno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_ServiceInwardMaster.Save(dgServiceInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SIM_CODE) from SERVICE_INWARD_MASTER");
                        //CommonClasses.WriteLog("Material Inward", "Save", "Material Inward", BL_ServiceInwardMaster.SIM_EGP_NO, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]),(Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        CommonClasses.Execute("update SERVICE_INWARD_DETAIL set SID_INSP_FLG=1 where SID_SIM_CODE='" + Code + "'");
                        CommonClasses.WriteLog("Service Inward ", "Insert", "Material Inward", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewServiceInward.aspx", false);
                    }
                    else
                    {
                        if (BL_ServiceInwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ServiceInwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_ServiceInwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_ServiceInwardMaster = new ServiceInward_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_ServiceInwardMaster.Update(dgServiceInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("SERVICE_INWARD_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        // CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward",  Convert.ToInt32(ViewState["mlCode"].ToString()) , Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewServiceInward.aspx", false);
                    }
                    else
                    {
                        if (BL_ServiceInwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ServiceInwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_ServiceInwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
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
            CommonClasses.SendError("Service Inward", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(SIM_NO) as SIM_NO from SERVICE_INWARD_MASTER where SIM_CM_CODE='" + Session["CompanyCode"] + "' AND SIM_TYPE='SIWM' and ES_DELETE=0");
        if (dt.Rows[0]["SIM_NO"] == null || dt.Rows[0]["SIM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["SIM_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion
    protected void dgServiceInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #region dgServiceInwardMaster_RowCommand
    protected void dgServiceInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgServiceInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];


            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgServiceInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgServiceInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgServiceInwardMaster.DataBind();
                if (dgServiceInwardMaster.Rows.Count == 0)
                    BlankGrid();
                // clearDetail();
            }
            if (e.CommandName == "Select")
            {
                LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                lnkDelete.Enabled = false;

                //dgInwardMaster.Columns[1].Visible = false;

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                string s = ((Label)(row.FindControl("lblSID_I_CODE"))).Text;
                ddlServiceCode.SelectedValue = ((Label)(row.FindControl("lblSID_I_CODE"))).Text;
                ddlServiceName.SelectedValue = ((Label)(row.FindControl("lblSID_I_CODE"))).Text;
                ddlServiceCode_SelectedIndexChanged(null, null);
                ddlPoNumber.SelectedValue = ((Label)(row.FindControl("lblSID_CPOM_CODE"))).Text;
                txtRecdQty.Text = ((Label)(row.FindControl("lblSID_REV_QTY"))).Text;
                if (txtRecdQty.Text == "")
                {
                    txtRecdQty.Text = "0";
                }
                txtRate.Text = ((Label)(row.FindControl("lblSID_RATE"))).Text;
                //txtRate.Text = ((Label)(row.FindControl("lblSPOD_RATE"))).Text;
                txtChallanQty.Text = ((Label)(row.FindControl("lblSID_CH_QTY"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblSID_REMARK"))).Text;
                dt = CommonClasses.Execute("select (ISNULL(SRPOD_ORDER_QTY,0)-ISNULL(SRPOD_INW_QTY,0)) as PENDQTY,SRPOD_RATE,SRPOD_UOM_CODE from SERVICE_PO_MASTER,SERVICE_PO_DETAILS where SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SRPOD_I_CODE='" + ddlServiceName.SelectedValue + "' and SRPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]) + Convert.ToDouble(txtRecdQty.Text);
                }
                else
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]);
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                //ddlUom.SelectedValue = ((Label)(row.FindControl("lblUOM_CODE"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblSID_BATCH_NO"))).Text;

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "dgServiceInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlServiceName.SelectedValue = "0";
            ddlServiceCode.SelectedValue = "0";
            ddlSupplier.Items.Insert(0, new ListItem("Please Select PO ", "0"));
            ddlPoNumber.SelectedValue = "0";
            txtRate.Text = "0.00";
            txtChallanQty.Text = "0.00";
            txtRecdQty.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Service Inward ", "clearDetail", Ex.Message);
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
                ddlServiceCode.Focus();
                return;
            }

            if (txtChallanNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Challan/Invoice No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlServiceCode.Focus();
                return;

            }

            if (Convert.ToInt32(ddlServiceName.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Service Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlServiceCode.Focus();
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
                DataTable dtCheckOpen = CommonClasses.Execute("SELECT SRPOD_ORDER_QTY FROM SERVICE_PO_DETAILS WHERE SRPOD_SPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND SRPOD_I_CODE='" + ddlServiceCode.SelectedValue + "' and SRPOD_ORDER_QTY>0");
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

            if (dgServiceInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgServiceInwardMaster.Rows.Count; i++)
                {
                    string SERVICE_CODE = ((Label)(dgServiceInwardMaster.Rows[i].FindControl("lblSID_I_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (SERVICE_CODE == ddlServiceName.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (SERVICE_CODE == ddlServiceName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
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
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_I_CODE");

                ((DataTable)ViewState["dt2"]).Columns.Add("S_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("S_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_CPOM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SRPOM_PO_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_CH_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_REV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("SID_REMARK");

                ((DataTable)ViewState["dt2"]).Columns.Add("SID_BATCH_NO");


            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["SID_I_CODE"] = ddlServiceName.SelectedValue;
            dr["S_CODENO"] = ddlServiceCode.SelectedItem;
            dr["S_NAME"] = ddlServiceName.SelectedItem;
            dr["SID_CPOM_CODE"] = ddlPoNumber.SelectedValue;
            dr["SRPOM_PO_NO"] = ddlPoNumber.SelectedItem.Text;
            dr["SID_RATE"] = string.Format("{0:0.000}", (Convert.ToDouble(txtRate.Text)));
            dr["SID_CH_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtChallanQty.Text)));
            dr["SID_REV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtRecdQty.Text)));
            dr["SID_REMARK"] = txtRemark.Text;

            dr["SID_BATCH_NO"] = txtBatchNo.Text;


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
                    ddlServiceName.SelectedIndex = 0;
                    ddlServiceCode.SelectedIndex = 0;
                    txtPendingQty.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtChallanQty.Text = "";
                txtRecdQty.Text = "";
                txtRemark.Text = "";

                ddlServiceName.SelectedIndex = 0;
                ddlServiceCode.SelectedIndex = 0;
                txtPendingQty.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgServiceInwardMaster.Enabled = true;
            dgServiceInwardMaster.Visible = true;
            dgServiceInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgServiceInwardMaster.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion


            //clearDetail();
        }


        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "btnInsert_Click", Ex.Message);
        }



    }
    #endregion

    #region ddlServiceName_SelectedIndexChanged
    protected void ddlServiceCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlServiceCode.SelectedIndex != 0)
        {
            ddlServiceName.SelectedValue = ddlServiceCode.SelectedValue;
            //DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlServiceCode.SelectedValue + "'");
            ////DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlServiceCode.SelectedItem + "");

            //if (true)
            //{

            //}

            //if (dt1.Rows.Count > 0)
            //{
            //    ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            //    // lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
            //}
            //else
            //{
            //    ddlUom.Text = "";
            //}
        }
        txtChallanQty.Text = "";
        txtRecdQty.Text = "";
        pendingQty();
    }
    #endregion

    #region ddlServiceName_SelectedIndexChanged
    protected void ddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlServiceCode.SelectedValue = ddlServiceName.SelectedValue;
        if (ddlServiceName.SelectedIndex != 0)
        {


            ddlServiceCode.SelectedValue = ddlServiceName.SelectedValue;
            //DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlServiceCode.SelectedValue + "'");
            ////DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

            //if (dt1.Rows.Count > 0)
            //{
            //    ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            //    // lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
            //}
            //else
            //{
            //    ddlUom.Text = "";
            //}
            txtChallanQty.Text = "";
            txtRecdQty.Text = "";
            pendingQty();
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
            dt = CommonClasses.Execute("select (ISNULL(SRPOD_ORDER_QTY,0)-ISNULL(SRPOD_INW_QTY,0)) as PENDQTY,  CASE when isnull(SRPOD_ORDER_QTY,0)=0 then SRPOD_RATE else SRPOD_RATE - ROUND((SRPOD_DISC_AMT/SRPOD_ORDER_QTY),2) end   AS SRPOD_RATE,SRPOD_UOM_CODE,SRPOM_POST from SERVICE_PO_MASTER,SERVICE_PO_DETAILS where SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SRPOD_I_CODE='" + ddlServiceName.SelectedValue + "' and SRPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SRPOM_POST"].ToString() == "False")
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
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
                txtRate.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["SRPOD_RATE"].ToString()));
                //ddlUom.SelectedValue = dt.Rows[0]["SRPOD_UOM_CODE"].ToString();
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            int id = Convert.ToInt32(ddlSupplier.SelectedValue);
            ddlServiceName.Items.Clear();
            ddlServiceCode.Items.Clear();
            DataTable dtService = new DataTable();
            //dtService = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND ((SPOD_INW_QTY) < (SPOD_ORDER_QTY) OR SPOD_ORDER_QTY=0)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SPOM_POTYPE=0   AND  SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
            dtService = CommonClasses.Execute("SELECT DISTINCT S_NAME,S_CODE,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_DETAILS,SERVICE_PO_MASTER WHERE SERVICE_PO_MASTER.ES_DELETE=0 and S_CODE=SRPOD_I_CODE and SRPOM_CANCEL_PO=0 and SRPOM_POST=1  AND SRPOM_IS_SHORT_CLOSE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND ((SRPOD_INW_QTY) < (SRPOD_ORDER_QTY) OR SRPOD_ORDER_QTY=0)  and S_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SRPOM_POTYPE=0   AND  SRPOM_POST=1 AND SRPOM_P_CODE=" + id + " ORDER BY S_NAME");
            ddlServiceName.DataSource = dtService;
            ddlServiceName.DataTextField = "S_NAME";
            ddlServiceName.DataValueField = "S_CODE";
            ddlServiceName.DataBind();
            ddlServiceCode.DataSource = dtService;
            ddlServiceCode.DataTextField = "S_CODENO";
            ddlServiceCode.DataValueField = "S_CODE";
            ddlServiceCode.DataBind();
            //CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_NAME", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlServiceName);
            //CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_CODENO", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemCode);

            ddlServiceName.Items.Insert(0, new ListItem("Please Select Service ", "0"));
            ddlServiceCode.Items.Insert(0, new ListItem("Please Select Service", "0"));
            BlankGrid();
            // pendingQty();

            //if (dt.Rows.Count > 0)
            //{

            //}


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Inward", "ddlSupplier_SelectedIndexChanged", Ex.Message);
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
            ddlPoNumber.Items.Clear();
            // CommonClasses.FillCombo("SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM", "SPOM_PO_NO", "SPOM_CODE", " SM.SPOM_CODE=SD.SPOD_SPOM_CODE  and  SM.SPOM_P_CODE=" + ddlSupplier.SelectedValue + " AND SD.SPOD_I_CODE=IM.I_CODE AND SD.SPOD_I_CODE=" + ddlServiceName.SelectedValue + " AND SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND  SM.ES_DELETE=0 AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY)  and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]), ddlPoNumber);
            //dtPO = CommonClasses.Execute("SELECT SPOM_CODE,SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER WHERE  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE AND SPOD_I_CODE='" + ddlServiceName.SelectedValue + "' AND SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY) ");
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtPO = CommonClasses.Execute("select DISTINCT SRPOM_CODE,Convert(varchar,SRPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SRPOM_PO_NO FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS,PROJECT_CODE_MASTER,SERVICE_TYPE_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SRPOM_PROJECT AND  SRPOM_CODE=SRPOD_SPOM_CODE  AND SRPOM_POST=1 AND SRPOM_IS_SHORT_CLOSE=0 and SRPOM_CANCEL_PO=0 and  SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SRPOD_I_CODE=S_CODE AND SRPOD_I_CODE='" + ddlServiceName.SelectedValue + "' AND SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SRPOM_CM_CODE=CM_CODE AND  SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   order by SRPOM_CODE");

            }
            else
            {
                dtPO = CommonClasses.Execute("select DISTINCT SRPOM_CODE,Convert(varchar,SRPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'')   as  SRPOM_PO_NO FROM SERVICE_PO_MASTER,SERVICE_PO_DETAILS,PROJECT_CODE_MASTER,SERVICE_TYPE_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SRPOM_PROJECT AND  SRPOM_CODE=SRPOD_SPOM_CODE  AND SRPOM_POST=1 AND SRPOM_IS_SHORT_CLOSE=0 and SRPOM_CANCEL_PO=0 and  SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SRPOD_I_CODE=S_CODE AND SRPOD_I_CODE='" + ddlServiceName.SelectedValue + "' AND SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SRPOM_CM_CODE=CM_CODE AND  SERVICE_PO_MASTER.ES_DELETE=0 and SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ((SRPOD_INW_QTY) < (SRPOD_ORDER_QTY) OR SRPOD_ORDER_QTY=0) order by SRPOM_CODE");

            }
            ddlPoNumber.DataSource = dtPO;
            ddlPoNumber.DataTextField = "SRPOM_PO_NO";
            ddlPoNumber.DataValueField = "SRPOM_CODE";
            ddlPoNumber.DataBind();

            ddlPoNumber.Items.Insert(0, new ListItem("Select PO ", "0"));
            //if (dt.Rows.Count > 0)
            //{
            //dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY, SPOD_RATE - ROUND((SPOD_DISC_AMT/SPOD_ORDER_QTY),2) AS SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlServiceName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");

            dt = CommonClasses.Execute("select (ISNULL(SRPOD_ORDER_QTY,0)-ISNULL(SRPOD_INW_QTY,0)) as PENDQTY, SRPOD_RATE,SRPOD_UOM_CODE,SRPOM_POST from SERVICE_PO_MASTER,SERVICE_PO_DETAILS where SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND SRPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SRPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SRPOD_I_CODE='" + ddlServiceName.SelectedValue + "' and SRPOM_CODE='" + ddlPoNumber.SelectedValue + "'");

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SRPOM_POST"].ToString() == "False")
                {
                    txtChallanQty.Enabled = false;
                    txtRecdQty.Enabled = false;
                    // txtPendingQty.Text = "0";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Only Post PO Can InWard";
                    // ddlPoNumber.Enabled = false;
                }

                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
                txtRate.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["SRPOD_RATE"].ToString()));
                // dt = CommonClasses.Execute("select DISTINCT I_UOM_NAME from UNIT_MASTER UM,SUPP_PO_DETAILS SD  where" + dt.Rows[0]["SPOD_UOM_CODE"] + "=UOM_CODE");
                //ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();

            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Service Material Inward", "PendingQty", Ex.Message);
        }
        #endregion
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
                Response.Redirect("~/Transactions/VIEW/ViewServiceInward.aspx", false);
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
            CommonClasses.SendError("Service Inward", "btnCancel_Click", Ex.Message);
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

            CommonClasses.SendError("Service Inward", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("SERVICE_INWARD_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }


            Response.Redirect("~/Transactions/VIEW/ViewServiceInward.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Inward", "CancelRecord", ex.Message.ToString());
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
            //else if (dgInwardMaster.Enabled && dgInwardMaster.Rows.Count > 0)
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
            CommonClasses.SendError("Service Inward", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);

        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtRecdQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecdQty.Text);

        txtRecdQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
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
