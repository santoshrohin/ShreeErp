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

public partial class Transactions_ADD_CustomerPO : System.Web.UI.Page
{
    #region Variable

    DirectoryInfo ObjSearchDir;
    string fileName = "";
    string path = "";
    static int File_Code = 0;
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    static int mlCode = 0;
    static int InqCode = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
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
                        LoadCustomer();
                        LoadICode();
                        LoadIName();
                        LoadTax();
                        LoadCurr();
                        LoadType();
                        LoadIUnit();
                        LoadEnquiry();
                        LoadProCode();
                        ViewState["fileName"] = fileName;
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        ViewState["str"] = str;
                        ViewState["ItemUpdateIndex"] = "-1";
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("AMEND"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("AMEND");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            LoadFilter();
                            txtPODate.Attributes.Add("readonly", "readonly");
                            txtCustPoDate.Attributes.Add("readonly", "readonly");
                            //txtPODate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            //txtCustPoDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                            txtPODate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtCustPoDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                            txtModDate.Attributes.Add("readonly", "readonly");
                            //txtModDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtModDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            dgMainPO.Enabled = false;
                            ddlEnquiryNo.Enabled = true;
                            //OrderNo();
                        }
                        else if (Request.QueryString[0].Equals("ConvertToOrder"))
                        {
                            InqCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["InqCode"] = InqCode;
                            txtPODate.Attributes.Add("readonly", "readonly");
                            txtCustPoDate.Attributes.Add("readonly", "readonly");
                            //txtPODate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            //txtCustPoDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtPODate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtCustPoDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                            SetEnquiryDetail();
                            ddlEnquiryNo.Enabled = false;
                        }
                        ddlPOType.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
                    }
                }
                //imgUpload.Attributes["onchange"] = "UploadFile(this)";
                // fileName = Path.GetFileName(imgUpload.FileName);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
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
            sDirPath = Server.MapPath(@"~/UpLoadPath/CustPO/" + ViewState["mlCode"].ToString() + "/");
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
                imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/CustPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            }
            LnkDoc.Visible = true;
            LnkDoc.Text = ViewState["fileName"].ToString();
        }
    }
    #endregion

    #region Upload_Comment
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
    //    string sDirPath = Server.MapPath(@"~/UpLoadPath/CustPO/" + Code + "");

    //    ObjSearchDir = new DirectoryInfo(sDirPath);

    //    if (!ObjSearchDir.Exists)
    //    {
    //        ObjSearchDir.Create();
    //    }
    //    imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/CustPO/" + Code + "/" + Path.GetFileName(imgUpload.FileName)));
    //    fileName = Path.GetFileName(imgUpload.FileName);
    //    ViewState["fileName"] = fileName;
    //    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DOC_NAME='" + fileName + "',I_DOC_PATH='" + "~/UpLoadPath/CustPO/" + Code + "" + '/' + "" + fileName + "' where I_CODE='" + Code + "'");
    //}
    #endregion Upload

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

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Select Sale Order Type", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select PO Type";
                ddlPOType.Focus();
                return;
            }

            if (ddlProjectCode.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Select Project code", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select PO Type";
                ddlProjectCode.Focus();
                return;
            }

            if (txtPONumber.Text == "")
            {
                ShowMessage("#Avisos", "The Field 'PO Number' Is Requireed", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                // PanelMsg.Visible = true;
                //lblmsg.Text = "Enter PO Number";
                txtPONumber.Focus();
                return;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Customer Name", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }

            if (Convert.ToDateTime(txtPODate.Text) < Convert.ToDateTime(txtCustPoDate.Text))
            {
                ShowMessage("#Avisos", "PO Date Should Not Greater than Entry Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                txtPODate.Focus();
                return;
            }
            if (dgMainPO.Enabled && dgMainPO.Rows.Count > 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Exist In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Record Not Exist In Grid View";
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "btnSubmit_Click", Ex.Message);
        }
    }

    #endregion btnSubmit_Click

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
            else if (txtPONumber.Text == "")
            {
                flag = false;
            }
            else if (ddlCustomer.SelectedIndex == 0)
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
            CommonClasses.SendError("Customer Order", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Customer Order", "btnOk_Click", Ex.Message);
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
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && ViewState["mlCode"] != null)
            {
                CommonClasses.RemoveModifyLock("CUSTPO_MASTER", "MODIFY", "CPOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "CancelRecord", Ex.Message);
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
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Customer Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtPODate.Attributes.Add("readonly", "readonly");
            //BL_Customer = new CustomerPO_BL(ml_code);
            //BL_Customer.CPOM_CM_COMP_ID = Convert.ToInt32(Session["CompanyCode"]);
            LoadCustomer();
            LoadICode();
            LoadIName();
            LoadTax();
            LoadCurr();
            LoadType();
            LoadIUnit();
            LoadEnquiry();
            dtInwardDetail.Clear();

            dt = CommonClasses.Execute("SELECT CPOM_CODE,CPOM_P_CODE,CPOM_PONO,CPOM_DOC_NO,CPOM_TYPE,CPOM_DATE,CPOM_PAY_TERM,cast(CPOM_BASIC_AMT as numeric(20,3)) as CPOM_BASIC_AMT,CPOM_CM_COMP_ID,MODIFY,CPOM_FINAL_DEST,CPOM_PRE_CARR_BY,CPOM_PORT_LOAD,CPOM_PORT_DIS,CPOM_PLACE_DEL,CPOM_BUYER_NAME,CPOM_BUYER_ADD,CPOM_CURR_CODE,CPOM_WORK_ODR_NO,CPOM_PO_DATE,CPOM_INQ_CODE,CPOM_PROJECT_CODE,CPOM_PROJECT_NAME ,ISNULL(CPOM_FREIGHT_CHARGES,0) AS CPOM_FREIGHT_CHARGES FROM CUSTPO_MASTER where ES_DELETE=0 and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and CPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["CPOM_CODE"]); ;

                txtPONumber.Text = dt.Rows[0]["CPOM_PONO"].ToString();
                txtPODocNo.Text = dt.Rows[0]["CPOM_DOC_NO"].ToString();
                txtOrderNo.Text = dt.Rows[0]["CPOM_WORK_ODR_NO"].ToString();
                txtPODate.Text = Convert.ToDateTime(dt.Rows[0]["CPOM_DATE"]).ToString("dd MMM yyyy");
                txtCustPoDate.Text = Convert.ToDateTime(dt.Rows[0]["CPOM_PO_DATE"]).ToString("dd MMM yyyy");
                ddlPOType.SelectedValue = Convert.ToInt32(dt.Rows[0]["CPOM_TYPE"]).ToString();
                ddlCustomer.SelectedValue = dt.Rows[0]["CPOM_P_CODE"].ToString();
                ddlEnquiryNo.SelectedValue = dt.Rows[0]["CPOM_INQ_CODE"].ToString();
                txtBasicAmount.Text = Convert.ToDouble(dt.Rows[0]["CPOM_BASIC_AMT"]).ToString();
                txtBasicAmount.Text = string.Format("{0:0.000}", Convert.ToDouble(txtBasicAmount.Text));
                txtPayTerm.Text = dt.Rows[0]["CPOM_PAY_TERM"].ToString();

                txtFinalD.Text = dt.Rows[0]["CPOM_FINAL_DEST"].ToString();
                txtPreCarr.Text = dt.Rows[0]["CPOM_PRE_CARR_BY"].ToString();
                txtPortLoad.Text = dt.Rows[0]["CPOM_PORT_LOAD"].ToString();
                txtPortDis.Text = dt.Rows[0]["CPOM_PORT_DIS"].ToString();
                txtPlace.Text = dt.Rows[0]["CPOM_PLACE_DEL"].ToString();
                txtBuyerName.Text = dt.Rows[0]["CPOM_BUYER_NAME"].ToString();
                txtBuyerAdd.Text = dt.Rows[0]["CPOM_BUYER_ADD"].ToString();
                txtProjName.Text = dt.Rows[0]["CPOM_PROJECT_NAME"].ToString();
                ddlProjectCode.SelectedValue = dt.Rows[0]["CPOM_PROJECT_CODE"].ToString();
                txtFreightCharges.Text = dt.Rows[0]["CPOM_FREIGHT_CHARGES"].ToString();
                if (ddlPOType.SelectedIndex == 2)
                {
                    ddlCurrancy.SelectedValue = dt.Rows[0]["CPOM_CURR_CODE"].ToString();
                }
                //dtInwardDetail = CommonClasses.Execute("select CPOD_I_CODE as ItemCode,I_CODENO as ShortName,cast((CPOD_ORD_QTY*CPOD_RATE) as numeric(20,2)) as Amount,CPOD_AMT,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as UnitCode,I_UOM_NAME as Unit,cast(CPOD_ORD_QTY as numeric(20,2))  as OrderQty,cast(CPOD_RATE as  numeric(10,2)) as Rate,CPOD_DESC as Description FROM CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER WHERE CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_MASTER.ES_DELETE=0  and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' order by CPOD_I_CODE");
                dtInwardDetail = CommonClasses.Execute("select CPOD_I_CODE as ItemCode,I_CODENO as ShortName,I_DOC_NAME ,CPOD_DOC_NAME as Docname,I_DOC_PATH,I_NAME as ItemName,I_UOM_NAME as Unit,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,cast(CPOD_ORD_QTY as numeric(10,3))  as OrderQty,cast(CPOD_RATE as  numeric(10,2)) as Rate,cast(CPOD_AMT as numeric(20,2)) as Amount,CPOD_CUST_I_CODE as CustItemCode,CPOD_CUST_I_NAME as CustItemName,'' as TaxCategory,'-2147483648' as TaxCatCode,CPOD_STATUS as StatusInd,(CASE CPOD_STATUS WHEN 0 THEN 'Active' WHEN 2 THEN 'Short Open' WHEN 3 THEN 'Terminate' END) AS Status,cast(CPOD_DISPACH as numeric(10,3))  as CPOD_DISPACH , ISNULL(CPOD_MODNO,0) AS ModNo,ISNULL(CPOD_MODDATE,GetDate()) AS ModDate,ISNULL(CPOD_AMORTRATE,0) AS AmortRate,ISNULL(CPOD_DIEAMORTRATE,0) AS CPOD_DIEAMORTRATE,ISNULL(CPOD_DISC_PER,0) AS CPOD_DISC_PER,ISNULL(CPOD_DISC_AMT,0) AS CPOD_DISC_AMT FROM CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER where CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_DETAIL.CPOD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ORDER BY CPOD_I_CODE");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgMainPO.DataSource = dtInwardDetail;
                    dgMainPO.DataBind();
                    dgMainPO.Enabled = true;
                    ViewState["dt2"] = dtInwardDetail;
                    GetTotal();
                }
                else
                {
                    dtInwardDetail.Rows.Add(dtInwardDetail.NewRow());
                    dgMainPO.DataSource = dtInwardDetail;
                    dgMainPO.DataBind();
                    dgMainPO.Enabled = false;
                }

            }
            if (ddlPOType.SelectedIndex != 2)
            {

                ddlCurrancy.Visible = false;
                pnlExport.Visible = false;
            }
            if (str == "VIEW")
            {
                ddlPOType.Enabled = false;
                txtPONumber.Enabled = false;
                txtOrderNo.Enabled = false;
                txtPODate.Enabled = false;
                txtCustPoDate.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtRate.Enabled = false;
                ddlUnit.Enabled = false;
                txtAmount.Enabled = false;
                txtCustItemCode.Enabled = false;
                txtCustItemName.Enabled = false;
                ddlTaxCategory.Enabled = false;
                txtOrderQty.Enabled = false;
                txtPayTerm.Enabled = false;
                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                dgMainPO.Enabled = false;
                txtBuyerName.Enabled = false;
                txtBuyerAdd.Enabled = false;
                txtPortLoad.Enabled = false;
                txtPortDis.Enabled = false;
                txtPreCarr.Enabled = false;
                txtPayTerm.Enabled = false;
                ddlCurrancy.Enabled = false;
                txtPlace.Enabled = false;
                ddlEnquiryNo.Enabled = false;
                ddlProjectCode.Enabled = false;
                txtProjName.Enabled = false;
            }
            if (str == "MOD" || str == "AMEND")
            {
                ddlCustomer.Enabled = false;
                ddlPOType.Enabled = false;
                txtOrderNo.Enabled = false;
                ddlEnquiryNo.Enabled = false;
                CommonClasses.SetModifyLock("CUSTPO_MASTER", "MODIFY", "CPOM_CODE", Convert.ToInt32(mlCode));

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1 order by P_NAME");
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_ACTIVE_IND=1 order by P_NAME");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadEnquiry
    private void LoadEnquiry()
    {
        try
        {
            //if (Request.QueryString[0].Equals("INSERT") || Request.QueryString[0].Equals("ConvertToOrder"))
            //{
            //    dt = CommonClasses.Execute("select distinct(INQ_CODE) ,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO from ENQUIRY_MASTER,PARTY_MASTER where ENQUIRY_MASTER.ES_DELETE=0 and INQ_CUST_NAME=P_CODE and INQ_CM_COMP_ID=" + (string)Session["CompanyId"] + " and INQ_QT_FLAG=0 order by INQ_CODE desc");
            //}
            //else
            //{
            //    dt = CommonClasses.Execute("select distinct(INQ_CODE) ,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO from ENQUIRY_MASTER,PARTY_MASTER where ENQUIRY_MASTER.ES_DELETE=0 and INQ_CUST_NAME=P_CODE and INQ_CM_COMP_ID=" + (string)Session["CompanyId"] + "  order by INQ_CODE desc");
            //}
            //ddlEnquiryNo.DataSource = dt;
            //ddlEnquiryNo.DataTextField = "INQ_NO";
            //ddlEnquiryNo.DataValueField = "INQ_CODE";
            //ddlEnquiryNo.DataBind();
            ddlEnquiryNo.Items.Insert(0, new ListItem("Select Enquiry", "0"));
            ddlEnquiryNo.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadEnquiry", Ex.Message);
        }

    }
    #endregion

    #region LoadType
    private void LoadType()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(SO_T_CODE) ,SO_T_DESC from SO_TYPE_MASTER where ES_DELETE=0 and SO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by SO_T_DESC");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "SO_T_DESC";
            ddlPOType.DataValueField = "SO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Select Sale Order Type", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE='-2147483646' ORDER BY I_CODENO");
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO --and I_CAT_CODE IN ('-2147483648','-2147483630') ");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            // dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and I_CAT_CODE='-2147483646' ORDER BY I_NAME");
            dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY I_NAME --and I_CAT_CODE IN ('-2147483648','-2147483630')");
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

    #region LoadCurr
    private void LoadCurr()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(CURR_NAME),CURR_CODE from CURRANCY_MASTER where ES_DELETE=0 and CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrancy.DataSource = dt;
            ddlCurrancy.DataTextField = "CURR_NAME";
            ddlCurrancy.DataValueField = "CURR_CODE";
            ddlCurrancy.DataBind();
            ddlCurrancy.Items.Insert(0, new ListItem("Select Currency", "0"));
            ddlCurrancy.SelectedIndex = -1;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadTax
    private void LoadTax()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  AND ST_TAX_NAME='Not Applicable' ORDER BY ST_TAX_NAME");
            ddlTaxCategory.DataSource = dt;
            ddlTaxCategory.DataTextField = "ST_TAX_NAME";
            ddlTaxCategory.DataValueField = "ST_CODE";
            ddlTaxCategory.DataBind();
            ddlTaxCategory.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            //ddlTaxCategory.SelectedIndex = 0;
            if (ddlTaxCategory.SelectedIndex == 0)
            {
                DataTable dtTax = CommonClasses.Execute("SELECT * from SALES_TAX_MASTER WHERE ES_DELETE=0 and ST_CM_COMP_ID= '" + (string)Session["CompanyId"] + "' AND ST_TAX_NAME='Not Applicable'");
                if (dtTax.Rows.Count > 0)
                {
                    ddlTaxCategory.SelectedValue = dtTax.Rows[0]["ST_CODE"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadTax", Ex.Message);
        }
    }
    #endregion

    #region LoadIUnit
    private void LoadIUnit()
    {
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY I_UOM_NAME");
            ddlUnit.DataSource = dt;
            ddlUnit.DataTextField = "I_UOM_NAME";
            ddlUnit.DataValueField = "I_UOM_CODE";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("Select Item Unit", "0"));
            ddlUnit.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadIUnit", Ex.Message);
        }

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT") || Request.QueryString[0].Equals("ConvertToOrder"))
            {
                // if (!CommonClasses.CheckExistSave("CUSTPO_MASTER", "CPOM_WORK_ODR_NO", txtOrderNo.Text))
                OrderNo();
                string strPONo = "";
                DataTable dtworkorder = CommonClasses.Execute("SELECT * FROM CUSTPO_MASTER WHERE CPOM_WORK_ODR_NO='" + txtOrderNo.Text + "' and CPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                if (dtworkorder.Rows.Count == 0)
                {
                    int Po_Doc_no = 0;
                    string Po_No_WithPrefx = "";
                    DataTable dt = new DataTable();
                    DataTable dtSoType = CommonClasses.Execute("SELECT * FROM SO_TYPE_MASTER WHERE  ES_DELETE = '0' AND SO_T_COMP_ID = '" + (string)Session["CompanyId"] + "'  AND SO_T_CODE = '" + ddlPOType.SelectedValue + "'");
                    dt = CommonClasses.Execute("SELECT ISNULL(MAX(CPOM_DOC_NO),0) AS CPOM_PONO FROM CUSTPO_MASTER WHERE CPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0 AND CPOM_TYPE = '" + ddlPOType.SelectedValue + "'");
                    if (dt.Rows.Count > 0)
                    {
                        Po_Doc_no = Convert.ToInt32(dt.Rows[0]["CPOM_PONO"]);
                        Po_Doc_no = Po_Doc_no + 1;
                        txtOrderNo.Text = Po_Doc_no.ToString();
                        string strYear = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy").Substring(9, 2) + "-" + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy").ToString().Substring(9, 2);
                        Po_No_WithPrefx = dtSoType.Rows[0]["SO_T_FIRST_LETTER"].ToString() + strYear + "/" + CommonClasses.GenBillNo(Po_Doc_no);
                    }
                    DataTable dtAbb = new DataTable();
                    if (chkIsVerbal.Checked == false)
                    {
                        dtAbb = CommonClasses.Execute("Select * FROM CUSTPO_MASTER WHERE CPOM_PONO= UPPER('" + txtPONumber.Text.Trim() + "') and ES_DELETE='False' and CPOM_CM_COMP_ID='" + Session["CompanyId"] + "'");
                        if (dtAbb.Rows.Count > 0)
                        {
                            ShowMessage("#Avisos", "PO No Already Exists", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtPONumber.Focus();
                            return false;
                        }
                    }
                    if (CommonClasses.Execute1("INSERT INTO CUSTPO_MASTER (CPOM_CM_COMP_ID,CPOM_P_CODE,CPOM_PONO,CPOM_DOC_NO,CPOM_TYPE,CPOM_DATE,CPOM_BASIC_AMT,CPOM_PAY_TERM,CPOM_FINAL_DEST,CPOM_PRE_CARR_BY,CPOM_PORT_LOAD,CPOM_PORT_DIS,CPOM_PLACE_DEL,CPOM_BUYER_NAME,CPOM_BUYER_ADD,CPOM_CURR_CODE,CPOM_WORK_ODR_NO,CPOM_PO_DATE,CPOM_INQ_CODE,CPOM_IS_VERBAL,CPOM_PROJECT_CODE,CPOM_PROJECT_NAME , CPOM_NO , CPOM_FREIGHT_CHARGES) VALUES ('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlCustomer.SelectedValue + "','" + txtPONumber.Text + "','" + Po_Doc_no + "','" + ddlPOType.SelectedValue + "','" + Convert.ToDateTime(txtPODate.Text).ToString("dd/MMM/yyyy") + "','" + txtBasicAmount.Text + "','" + txtPayTerm.Text + "','" + txtFinalD.Text + "','" + txtPreCarr.Text + "','" + txtPortLoad.Text + "','" + txtPortDis.Text + "','" + txtPlace.Text + "','" + txtBuyerName.Text + "','" + txtBuyerAdd.Text + "','" + ddlCurrancy.SelectedValue + "','" + txtOrderNo.Text + "','" + Convert.ToDateTime(txtCustPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlEnquiryNo.SelectedValue + "','" + chkIsVerbal.Checked + "'," + ddlProjectCode.SelectedValue + ",'" + txtProjName.Text + "','" + Po_No_WithPrefx + "','" + txtFreightCharges.Text.ToString().Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CPOM_CODE) from CUSTPO_MASTER");
                        for (int i = 0; i < dgMainPO.Rows.Count; i++)
                        {
                            string TaxCode = "";
                            if (((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text == "Not Applicable")
                            {
                            }
                            CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_DISPACH,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_UOM_CODE,CPOD_MODNO,CPOD_MODDATE,CPOD_AMORTRATE,CPOD_DIEAMORTRATE,CPOD_DISC_PER,CPOD_DISC_AMT,CPOD_DOC_NAME) values ('" + Code + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCPOD_DISPACH")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblI_UOM_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblModNo")).Text + "','" + Convert.ToDateTime(((Label)dgMainPO.Rows[i].FindControl("lblModDate")).Text).ToString("dd MMM yyyy") + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmortRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblDieAmortRate")).Text + "' ,'" + ((Label)dgMainPO.Rows[i].FindControl("lblDiscountPer")).Text + "' ,'" + ((Label)dgMainPO.Rows[i].FindControl("lblDiscountAmt")).Text + "','" + (((LinkButton)dgMainPO.Rows[i].FindControl("lnkView")).Text) + "')");

                            #region Upload_Doc
                            if (ViewState["fileName"].ToString() != "")
                            {
                                string sDirPath = Server.MapPath(@"~/UpLoadPath/CustPO/" + Code + "");

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

                                string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                                string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + (((LinkButton)dgMainPO.Rows[i].FindControl("lnkView")).Text));
                                // Get the destination path
                                string copyToPath = Server.MapPath(@"~/UpLoadPath/CustPO/" + Code + "/" + (((LinkButton)dgMainPO.Rows[i].FindControl("lnkView")).Text));
                                DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                                System.IO.File.Move(fullFilePath, copyToPath);
                            }
                            #endregion Upload_Doc
                        }
                        CommonClasses.Execute("UPDATE ENQUIRY_MASTER set INQ_QT_FLAG = '1' where INQ_CODE=" + ddlEnquiryNo.SelectedValue + "");
                        CommonClasses.WriteLog("Customer Order", "Save", "Customer Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;



                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                        txtPONumber.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Order No is already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtOrderNo.Focus();
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dtAbb = new DataTable();
                if (chkIsVerbal.Checked == false)
                {
                    dtAbb = CommonClasses.Execute("Select * FROM CUSTPO_MASTER WHERE CPOM_CODE != '" + Convert.ToInt32(ViewState["mlCode"]) + "' and CPOM_PONO= UPPER('" + txtPONumber.Text.Trim() + "') and ES_DELETE='False' and CPOM_CM_COMP_ID='" + Session["CompanyId"] + "'");
                    if (dtAbb.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "PO No Already Exists", CommonClasses.MSG_Warning);

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtPONumber.Focus();
                        return false;
                    }
                }
                if (CommonClasses.Execute1("UPDATE CUSTPO_MASTER SET CPOM_P_CODE = '" + ddlCustomer.SelectedValue + "', CPOM_PONO = '" + txtPONumber.Text + "',CPOM_DOC_NO='" + txtPODocNo.Text + "',CPOM_DATE='" + Convert.ToDateTime(txtPODate.Text).ToString("dd/MMM/yyyy") + "',CPOM_TYPE='" + ddlPOType.SelectedValue + "',CPOM_BASIC_AMT='" + txtBasicAmount.Text + "',CPOM_PAY_TERM='" + txtPayTerm.Text + "',CPOM_FINAL_DEST='" + txtFinalD.Text + "',CPOM_PRE_CARR_BY='" + txtPreCarr.Text + "',CPOM_PORT_LOAD='" + txtPortLoad.Text + "',CPOM_PORT_DIS='" + txtPortDis.Text + "',CPOM_PLACE_DEL='" + txtPlace.Text + "',CPOM_BUYER_NAME='" + txtBuyerName.Text + "',CPOM_BUYER_ADD='" + txtBuyerAdd.Text + "',CPOM_CURR_CODE='" + ddlCurrancy.SelectedValue + "',CPOM_WORK_ODR_NO='" + txtOrderNo.Text + "',CPOM_PO_DATE='" + Convert.ToDateTime(txtCustPoDate.Text).ToString("dd/MMM/yyyy") + "',CPOM_INQ_CODE='" + ddlEnquiryNo.SelectedValue + "',CPOM_IS_VERBAL='" + chkIsVerbal.Checked + "',CPOM_PROJECT_CODE=" + ddlProjectCode.SelectedValue + ",CPOM_PROJECT_NAME='" + txtProjName.Text + "' , CPOM_FREIGHT_CHARGES = '" + txtFreightCharges.Text.ToString().Trim().Replace("'", "\''") + "' WHERE CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM CUSTPO_DETAIL WHERE CPOD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgMainPO.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_DISPACH,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_UOM_CODE,CPOD_MODNO,CPOD_MODDATE,CPOD_AMORTRATE,CPOD_DIEAMORTRATE ,CPOD_DISC_PER, CPOD_DISC_AMT,CPOD_DOC_NAME) values ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCPOD_DISPACH")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblI_UOM_CODE")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblModNo")).Text + "','" + Convert.ToDateTime(((Label)dgMainPO.Rows[i].FindControl("lblModDate")).Text).ToString("dd MMM yyyy") + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmortRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblDieAmortRate")).Text + "' ,'" + ((Label)dgMainPO.Rows[i].FindControl("lblDiscountPer")).Text + "' ,'" + ((Label)dgMainPO.Rows[i].FindControl("lblDiscountAmt")).Text + "','" + (((LinkButton)dgMainPO.Rows[i].FindControl("lnkView")).Text) + "')");
                        }
                        CommonClasses.RemoveModifyLock("CUSTPO_MASTER", "MODIFY", "CPOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Customer Order", "Update", "Customer Order", txtPONumber.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtPONumber.Focus();
                }
            }
            #endregion MODIFY

            else if (Request.QueryString[0].Equals("AMEND"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(CPOM_AM_COUNT,0) as CPOM_AM_COUNT from CUSTPO_MASTER WHERE CPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["CPOM_AM_COUNT"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                CommonClasses.Execute1("update  CUSTPO_MASTER set CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',CPOM_AM_COUNT='" + AMEND_COUNT + "' WHERE CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (CommonClasses.Execute1("INSERT INTO CUSTPO_AM_MASTER select * from CUSTPO_MASTER where CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' "))
                {
                    string MatserCode = CommonClasses.GetMaxId("Select Max(CPOM_AM_CODE) from CPOM_AM_MASTER");
                    DataTable dtDetail = CommonClasses.Execute("select * from CUSTPO_DETAIL where CPOD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        CommonClasses.Execute1("INSERT INTO CUSTPO_AMD_DETAIL values('" + dtDetail.Rows[j]["CPOD_CPOM_CODE"] + "','" + dtDetail.Rows[j]["CPOD_I_CODE"] + "','" + dtDetail.Rows[j]["CPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["CPOD_ORD_QTY"] + "','" + dtDetail.Rows[j]["CPOD_RATE"] + "','" + dtDetail.Rows[j]["CPOD_AMT"] + "','" + dtDetail.Rows[j]["CPOD_STATUS"] + "','" + dtDetail.Rows[j]["CPOD_DISPACH"] + "','" + dtDetail.Rows[j]["CPOD_DESC"] + "','" + dtDetail.Rows[j]["CPOD_CUST_I_CODE"] + "','" + dtDetail.Rows[j]["CPOD_CUST_I_NAME"] + "','" + dtDetail.Rows[j]["CPOD_ST_CODE"] + "','" + dtDetail.Rows[j]["CPOD_CURR_CODE"] + "','" + MatserCode + "','" + ((Label)dgMainPO.Rows[j].FindControl("lblModNo")).Text + "','" + Convert.ToDateTime(((Label)dgMainPO.Rows[j].FindControl("lblModDate")).Text).ToString("dd MMM yyyy") + "','" + ((Label)dgMainPO.Rows[j].FindControl("lblAmortRate")).Text + "')");
                    }

                    #region ModifyOriginalPO
                    if (CommonClasses.Execute1("UPDATE CUSTPO_MASTER SET CPOM_P_CODE='" + ddlCustomer.SelectedValue + "',CPOM_PONO='" + txtPONumber.Text + "',CPOM_DOC_NO='" + txtPODocNo.Text + "',CPOM_DATE='" + Convert.ToDateTime(txtPODate.Text).ToString("dd/MMM/yyyy") + "',CPOM_TYPE='" + ddlPOType.SelectedValue + "',CPOM_BASIC_AMT='" + txtBasicAmount.Text + "',CPOM_PAY_TERM='" + txtPayTerm.Text + "',CPOM_FINAL_DEST='" + txtFinalD.Text + "',CPOM_PRE_CARR_BY='" + txtPreCarr.Text + "',CPOM_PORT_LOAD='" + txtPortLoad.Text + "',CPOM_PORT_DIS='" + txtPortDis.Text + "',CPOM_PLACE_DEL='" + txtPlace.Text + "',CPOM_BUYER_NAME='" + txtBuyerName.Text + "',CPOM_BUYER_ADD='" + txtBuyerAdd.Text + "',CPOM_CURR_CODE='" + ddlCurrancy.SelectedValue + "',CPOM_WORK_ODR_NO='" + txtOrderNo.Text + "',CPOM_PO_DATE='" + Convert.ToDateTime(txtCustPoDate.Text).ToString("dd/MMM/yyyy") + "',CPOM_AM_COUNT='" + AMEND_COUNT + "',CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',CPOM_INQ_CODE='" + ddlEnquiryNo.SelectedValue + "',CPOM_IS_VERBAL='" + chkIsVerbal.Checked + "' where CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                    {
                        result = CommonClasses.Execute1("update  CUSTPO_AM_MASTER set CPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE CPOM_AM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and CPOM_AM_COUNT='" + AMEND_COUNT + "'");
                        result = CommonClasses.Execute1("DELETE FROM CUSTPO_DETAIL WHERE CPOD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgMainPO.Rows.Count; i++)
                            {
                                CommonClasses.Execute1("INSERT INTO CUSTPO_DETAIL (CPOD_CPOM_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_DISPACH,CPOD_RATE,CPOD_AMT,CPOD_STATUS,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOD_ST_CODE,CPOD_UOM_CODE) values ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCPOD_DISPACH")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblAmount")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblStatusInd")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblCustItemName")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblTaxCatCode")).Text + "','" + ((Label)dgMainPO.Rows[i].FindControl("lblI_UOM_CODE")).Text + "')");
                            }
                            for (int j = 0; j < dtDetail.Rows.Count; j++)
                            {
                                CommonClasses.Execute1("update CUSTPO_DETAIL set CPOD_WO_QTY='" + dtDetail.Rows[j]["CPOD_WO_QTY"] + "' where CPOD_CPOM_CODE='" + dtDetail.Rows[j]["CPOD_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtDetail.Rows[j]["CPOD_I_CODE"] + "' ");
                            }
                            //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");

                            CommonClasses.RemoveModifyLock("CUSTPO_MASTER", "MODIFY", "CPOM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                            CommonClasses.WriteLog("Customer Order", "Update", "Customer Order", txtPONumber.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
                    }

                    #endregion
                }
                else
                {
                    //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                    ShowMessage("#Avisos", "PO Not Amend", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    ddlPOType.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Oreder", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            #region Comment
            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");


            if (ddlPOType.SelectedIndex == 0)
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "Select Sale Order Type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                //ShowMessage("#Avisos", "Select Sale Order Type", CommonClasses.MSG_Warning);


                ddlPOType.Focus();
                return;
            }


            if (ddlCustomer.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                ddlCustomer.Focus();
                return;
            }

            if (txtPONumber.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select PO Number";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlProjectCode.Focus();
                return;
            }
            if (ddlProjectCode.SelectedValue == "0")
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "Select Project COde";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlProjectCode.Focus();
                return;
            }
            if (ddlItemCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlItemName.Focus();
                return;
            }
            if (ddlPOType.SelectedIndex == 2)
            {
                if (ddlCurrancy.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Currancy Name", CommonClasses.MSG_Warning);

                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Select Currancy Name";
                    ddlCustomer.Focus();
                    return;
                }
            }
            if (txtOrderQty.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Enter Order Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtOrderQty.Focus();
                return;
            }
            if (txtRate.Text == "" || txtRate.Text == "0.00")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Enter Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtRate.Focus();
                return;
            }
            //if (ddlTaxCategory.SelectedIndex == 0)
            //{
            //    ShowMessage("#Avisos", "Select Tax Category", CommonClasses.MSG_Warning);

            //    //ddlTaxCategory.Focus();
            //    //return;
            //    //PanelMsg.Visible = true;
            //    //lblmsg.Text = "Select Tax Category";
            //    ddlCustomer.Focus();
            //    return;
            //} 
            #endregion

            ddlItemCode.Enabled = true;
            ddlItemName.Enabled = true;
            DataTable dt1 = CommonClasses.Execute("select cpom_code from CUSTPO_MASTER inner join CUSTPO_DETAIL ON CPOM_CODE=CPOD_CPOM_CODE where ES_DELETE=0 AND CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOM_IS_VERBAL=1 and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "'");
            if (dt1.Rows.Count > 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "These Item Verbal PO Is Already Pending";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlItemCode.Focus();
                return;
            }
            double Qty = 0.0;
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("Select isnull(max(CPOD_WO_QTY),0) as CPOD_WO_QTY FROM CUSTPO_DETAIL WHERE CPOD_CPOM_CODE = '" + Convert.ToInt32(ViewState["mlCode"]) + "' and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                Qty = Convert.ToDouble(dt.Rows[0]["CPOD_WO_QTY"]);
            }
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0";
            }
            if (Convert.ToDouble(txtOrderQty.Text) != 0)
            {

                if (Convert.ToDouble(txtOrderQty.Text) < Qty)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Order Quantity Should Not Be Less Than Work Order Qty: " + Qty.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtOrderQty.Focus();
                    return;
                }
            }
            if (dgMainPO.Enabled)
            {
                for (int i = 0; i < dgMainPO.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgMainPO.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            ddlItemCode.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            ddlItemCode.Focus();
                            return;
                        }
                    }

                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("ShortName");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("CPOD_DISPACH");
                ((DataTable)ViewState["dt2"]).Columns.Add("Rate");

                ((DataTable)ViewState["dt2"]).Columns.Add("CurrName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Amount");
                ((DataTable)ViewState["dt2"]).Columns.Add("CustItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("CustItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("TaxCategory");
                ((DataTable)ViewState["dt2"]).Columns.Add("TaxCatCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("StatusInd");
                ((DataTable)ViewState["dt2"]).Columns.Add("Status");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_CODE");

                ((DataTable)ViewState["dt2"]).Columns.Add("ModNo");
                ((DataTable)ViewState["dt2"]).Columns.Add("ModDate");
                ((DataTable)ViewState["dt2"]).Columns.Add("AmortRate");
                ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
                ((DataTable)ViewState["dt2"]).Columns.Add("CPOD_DIEAMORTRATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("CPOD_DISC_PER");
                ((DataTable)ViewState["dt2"]).Columns.Add("CPOD_DISC_AMT");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["ShortName"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["Unit"] = ddlUnit.SelectedItem;
            //  dr["UnitCode"] = lblUnit.Text;
            dr["OrderQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 3));
            if (ViewState["str"].ToString() == "Modify" && ViewState["ItemUpdateIndex"].ToString() != "-1")
            {
                if (Convert.ToDouble(txtOrderQty.Text) != 0)
                {
                    if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(Session["Dispatch_QTY"]))
                    {
                        //ShowMessage("#Avisos", "Order Qty Is Not Less Than Dispatch Qty : " + Session["Dispatch_QTY"], CommonClasses.MSG_Warning);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Order Qty Is Not Less Than Dispatch Qty : " + Session["Dispatch_QTY"];
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                    else
                    {
                        dr["CPOD_DISPACH"] = Session["Dispatch_QTY"];
                    }
                }
                else
                {
                    dr["CPOD_DISPACH"] = Session["Dispatch_QTY"];
                }
            }
            else
            {
                dr["CPOD_DISPACH"] = "0";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0";
            }
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));

            dr["CustItemCode"] = txtCustItemCode.Text.Trim().Replace("'", "''");
            dr["CustItemName"] = txtCustItemName.Text.Trim().Replace("'", "''");
            dr["TaxCategory"] = ddlTaxCategory.SelectedItem;
            dr["TaxCatCode"] = "-2147483648";//ddlTaxCategory.SelectedValue;
            dr["StatusInd"] = rbtStatus.SelectedIndex;
            dr["Status"] = rbtStatus.SelectedItem;
            dr["I_UOM_CODE"] = ddlUnit.SelectedValue;
            dr["ModNo"] = txtModNo.Text.Trim();
            if (txtModDate.Text.Trim() == "")
            {
                dr["ModDate"] = System.DateTime.Now.ToString("dd MMM yyyy");
            }
            else
            {
                dr["ModDate"] = txtModDate.Text;
            }
            if (txtAmortRate.Text == "")
            {
                txtAmortRate.Text = "0";
            }
            dr["AmortRate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtAmortRate.Text)), 2));

            //File_Code = Convert.ToInt32(ddlItemCode.SelectedValue);
            //Server.MapPath(@"~/UpLoadPath/CustPO/" + File_Code + "");

            dr["DocName"] = ViewState["fileName"].ToString();
            if (txtDieAmorRate.Text == "")
            {
                txtDieAmorRate.Text = "0";
            }
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0";
            }
            if (txtDiscAmt.Text == "")
            {
                txtDiscAmt.Text = "0";
            }
            dr["CPOD_DIEAMORTRATE"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtDieAmorRate.Text)), 2));
            dr["CPOD_DISC_PER"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtDiscPer.Text)), 2));
            dr["CPOD_DISC_AMT"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtDiscAmt.Text)), 2));
            dr["Amount"] = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtOrderQty.Text)) - Convert.ToDouble(txtDiscAmt.Text)), 2));
            #endregion



            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"]));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"]));
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            dgMainPO.Visible = true;
            dgMainPO.DataSource = ((DataTable)ViewState["dt2"]);
            dgMainPO.DataBind();
            dgMainPO.Enabled = true;
            #endregion

            GetTotal();
            clearDetail();
            ddlItemCode.Focus();
            DataTable dtTax = CommonClasses.Execute("SELECT * from SALES_TAX_MASTER WHERE ES_DELETE=0 and ST_CM_COMP_ID= '" + (string)Session["CompanyId"] + "' AND ST_TAX_NAME='Not Applicable'");
            if (dtTax.Rows.Count > 0)
            {
                ddlTaxCategory.SelectedValue = dtTax.Rows[0]["ST_CODE"].ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "btnInsert_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Order ", "ShowMessage", Ex.Message);
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
            ddlUnit.SelectedIndex = 0;
            //txtOrderQty.Text = "0.000";
            //txtRate.Text = "0.00";
            txtAmount.Text = "0.00";
            txtCustItemCode.Text = "";
            txtCustItemName.Text = "";
            ddlTaxCategory.SelectedIndex = 0;
            ddlCurrancy.SelectedIndex = 0;
            rbtStatus.SelectedIndex = 0;
            ddlCurrancy.SelectedIndex = 0;
            //txtDieAmorRate.Text = "0.00";
            //txtDiscPer.Text = "0.00";
            //txtDiscAmt.Text = "0.00";
            ViewState["str"] = "0.00";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Order ", "clearDetail", Ex.Message);
        }
        ddlItemCode.Focus();
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
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtitem = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODENO=" + ddlItemCode.SelectedItem + "");

                if (dt1.Rows.Count > 0)
                {
                    //txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtCustItemCode.Text = ddlItemCode.SelectedItem.ToString();
                    txtCustItemName.Text = ddlItemName.SelectedItem.ToString();

                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                DataTable dtAdd = CommonClasses.Execute("select top 1 CPOD_CUST_I_CODE,CPOD_CUST_I_NAME from CUSTPO_MASTER,CUSTPO_MASTER where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE order by CPOM_CODE desc");
                if (dtAdd.Rows.Count > 0)
                {

                    txtCustItemCode.Text = dtAdd.Rows[0]["CPOD_CUST_I_CODE"].ToString();
                    txtCustItemName.Text = dtAdd.Rows[0]["CPOD_CUST_I_NAME"].ToString();
                }
                txtOrderQty.Focus();

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtitemcode = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_NAME=" + ddlItemName.Text + "");

                if (dt1.Rows.Count > 0)
                {
                    //txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtCustItemCode.Text = ddlItemCode.SelectedItem.ToString();
                    txtCustItemName.Text = ddlItemName.SelectedItem.ToString();
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                //DataTable DtRate = new DataTable();

                DataTable dtAdd = CommonClasses.Execute("select top 1 CPOD_CUST_I_CODE,CPOD_CUST_I_NAME  from CUSTPO_MASTER,CUSTPO_MASTER where CPOD_I_CODE=" + ddlItemName.SelectedValue + " and CUSTPO_MASTER.ES_DELETE=0 and CPOM_CODE=CPOD_CPOM_CODE order by CPOM_CODE desc");
                if (dtAdd.Rows.Count > 0)
                {

                    txtCustItemCode.Text = dtAdd.Rows[0]["CPOD_CUST_I_CODE"].ToString();
                    txtCustItemName.Text = dtAdd.Rows[0]["CPOD_CUST_I_NAME"].ToString();
                }
                txtOrderQty.Focus();

            }
            //else
            //{
            //    ddlItemCode.SelectedValue = "0";

            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Order Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
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
            if (no > 10)
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

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0.00";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtOrderQty.Text);
            double Amount = 0; string OrderQty = ""; string OrderRate = "";
            OrderQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
            OrderRate = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtRate.Text), 3));
            Amount = Convert.ToDouble(Math.Round((Convert.ToDouble(OrderQty) * Convert.ToDouble(OrderRate)), 0, MidpointRounding.AwayFromZero));
            txtAmount.Text = Amount.ToString();
            txtDiscPer_TextChanged(null, null);
            ddlTaxCategory.Focus();
            //txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
            //txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            //txtAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2), 2);
            //txtAmount.Text = amount.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtOrderQty_OnTextChanged
    protected void txtOrderQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0";
            }
            string totalStr = DecimalMasking(txtOrderQty.Text);
            double Amount = 0; string OrderQty = ""; string OrderRate = "";
            OrderQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
            OrderRate = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtRate.Text), 3));
            Amount = Convert.ToDouble(Math.Round((Convert.ToDouble(OrderQty) * Convert.ToDouble(OrderRate)), 0, MidpointRounding.AwayFromZero));
            txtAmount.Text = Amount.ToString();
            txtDiscPer_TextChanged(null, null);
            //txtRate.Focus();
            #region Old_Code
            //txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            //txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtRate.Text), 2));
            //txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inventory Customer Order Transaction", "txtOrderQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region CheckNo
    public void CheckNo(TextBox t)
    {
        if (t.Text != "")
        {
            double num;
            bool b = Double.TryParse(t.Text, out num);
            if (b != true)
            {
                ShowMessage("#Avisos", "Enter Valid Number!!!", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                t.Text = "0.00";
                t.Focus();
            }
        }

    }
    #endregion

    #region dgMainPO_Deleting
    protected void dgMainPO_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainPO_RowCommand
    protected void dgMainPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["Index"] = Index;
            GridViewRow row = dgMainPO.Rows[Convert.ToInt32(ViewState["Index"])];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
                {
                    string itemCode = ((Label)(row.FindControl("lblItemCode"))).Text;

                    if (CommonClasses.CheckUsedInTran("WORK_ORDER_DETAIL,WORK_ORDER_MASTER", "WOD_I_CODE", "AND WO_CODE=WOD_CODE AND WORK_ORDER_MASTER.ES_DELETE=0 and WO_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ", itemCode))
                    {
                        //PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Work Order";

                        //ShowMessage("#Avisos", "You can't delete this record, it is used in Invoice", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }

                    if (CommonClasses.CheckUsedInTran("INVOICE_DETAIL,INVOICE_MASTER", "IND_I_CODE", "AND INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 and IND_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ", itemCode))
                    {
                        //PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Invoice";

                        //ShowMessage("#Avisos", "You can't delete this record, it is used in Invoice", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_I_CODE", "AND IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 and IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ", itemCode))
                    {
                        //PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Inward";

                        //ShowMessage("#Avisos", "You can't delete this record, it is used in Invoice", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
                dgMainPO.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);

                dgMainPO.DataSource = ((DataTable)ViewState["dt2"]);
                dgMainPO.DataBind();
                if (dgMainPO.Rows.Count == 0)
                {
                    dgMainPO.Enabled = false;
                    GetTotal();
                    LoadFilter();
                }
                else
                {
                    GetTotal();
                }
            }
            if (e.CommandName == "Select")
            {
                string itemCode = ((Label)(row.FindControl("lblItemCode"))).Text;
                if (CommonClasses.CheckUsedInTran("INVOICE_DETAIL,INVOICE_MASTER", "IND_I_CODE", "AND INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 and IND_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ", itemCode))
                {
                    ddlItemCode.Enabled = false;
                    ddlItemName.Enabled = false;
                }
                else if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_I_CODE", "AND IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 and IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' ", itemCode))
                {
                    ddlItemCode.Enabled = false;
                    ddlItemName.Enabled = false;
                }
                else
                {
                    ddlItemCode.Enabled = true;
                    ddlItemName.Enabled = true;
                }

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblI_UOM_CODE"))).Text;
                //txtTaxName.Text = ((Label)(row.FindControl("lblTaxName"))).Text;
                //txtTaxPer.Text = ((Label)(row.FindControl("lblTaxPer"))).Text;
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                Session["Dispatch_QTY"] = ((Label)(row.FindControl("lblCPOD_DISPACH"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblRate"))).Text;
                double amount = Convert.ToDouble(((Label)(row.FindControl("lblRate"))).Text) * Convert.ToDouble(((Label)(row.FindControl("lblOrderQty"))).Text);
                txtAmount.Text = Convert.ToString(amount);
                txtCustItemCode.Text = ((Label)(row.FindControl("lblCustItemCode"))).Text;
                txtCustItemName.Text = ((Label)(row.FindControl("lblCustItemName"))).Text;
                //ddlTaxCategory.SelectedValue = ((Label)(row.FindControl("lblTaxCatCode"))).Text;
                rbtStatus.SelectedIndex = Convert.ToInt32(((Label)(row.FindControl("lblStatusInd"))).Text);

                txtModNo.Text = ((Label)(row.FindControl("lblModNo"))).Text;
                txtModDate.Text = ((Label)(row.FindControl("lblModDate"))).Text;
                txtAmortRate.Text = ((Label)(row.FindControl("lblAmortRate"))).Text;
                txtDieAmorRate.Text = ((Label)(row.FindControl("lblDieAmortRate"))).Text;
                txtDiscPer.Text = ((Label)(row.FindControl("lblDiscountPer"))).Text;
                txtDiscAmt.Text = ((Label)(row.FindControl("lblDiscountAmt"))).Text;
                // txtUnit.Text = CommonClasses.Execute("select UOM_NAME from UNIT_MASTER,ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE and I_UOM_CODE_S=UOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "'").Rows[0]["UOM_NAME"].ToString();
                ViewState["fileName"] = ((LinkButton)(row.FindControl("lnkView"))).Text;
                lnkView.Visible = true;
                lnkView.Text = ((LinkButton)(row.FindControl("lnkView"))).Text;
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
                    directory = "../../UpLoadPath/CustPO/" + code + "/" + filePath;
                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                    // Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedIndex != 0)
            {
                DataTable dtAdd = CommonClasses.Execute("select top 1 * from CUSTPO_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and ES_DELETE=0 and CPOM_TYPE='" + ddlPOType.SelectedValue + "' order by CPOM_CODE desc");
                if (dtAdd.Rows.Count > 0)
                {
                    txtBuyerName.Text = dtAdd.Rows[0]["CPOM_BUYER_NAME"].ToString();

                    txtBuyerAdd.Text = dtAdd.Rows[0]["CPOM_BUYER_ADD"].ToString();
                    txtPayTerm.Text = dtAdd.Rows[0]["CPOM_PAY_TERM"].ToString();
                    txtPortLoad.Text = dtAdd.Rows[0]["CPOM_PORT_LOAD"].ToString();
                    txtFinalD.Text = dtAdd.Rows[0]["CPOM_FINAL_DEST"].ToString();
                    txtPortDis.Text = dtAdd.Rows[0]["CPOM_PORT_DIS"].ToString();
                    txtPreCarr.Text = dtAdd.Rows[0]["CPOM_PRE_CARR_BY"].ToString();
                    txtPlace.Text = dtAdd.Rows[0]["CPOM_PLACE_DEL"].ToString();
                    if (ddlPOType.SelectedIndex == 2)
                    {
                        ddlCurrancy.SelectedValue = dtAdd.Rows[0]["CPOM_CURR_CODE"].ToString();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError(" Customer Order Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlPOType_SelectedIndexChanged
    protected void ddlPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPOType.SelectedIndex == 2)
        {

            pnlExport.Visible = true;
        }
        else
        {
            pnlExport.Visible = false;

        }
        if (ddlPOType.SelectedValue == "-2147483643")
        {
            BindBillPassingSupplier();

        }
    }
    #endregion
    public void BindBillPassingSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER,BILL_PASSING_MASTER where P_CODE=BPM_P_CODE and  PARTY_MASTER.ES_DELETE=0 and BILL_PASSING_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and P_ACTIVE_IND=1 order by P_NAME");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }

    }
    #region txtBasicAmount_TextChanged
    protected void txtBasicAmount_TextChanged(object sender, EventArgs e)
    {
        GetTotal();
    }
    #endregion

    #region txtRound_TextChanged
    protected void txtRound_TextChanged(object sender, EventArgs e)
    {
        GetTotal();
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double disTotal = 0;
        if (dgMainPO.Enabled)
        {
            for (int i = 0; i < dgMainPO.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgMainPO.Rows[i].FindControl("lblAmount"))).Text;

                double Amount = Convert.ToDouble(QED_AMT);
                decTotal = decTotal + Amount;

                disTotal = disTotal + Convert.ToDouble(((Label)(dgMainPO.Rows[i].FindControl("lblDiscountAmt"))).Text);
            }
        }
        if (ddlPOType.SelectedIndex == 2 && ddlCurrancy.SelectedIndex > 0)
        {
            lblBasicAmt.Text = "Basic Amount (In " + ddlCurrancy.SelectedItem + ")";
        }
        else
        {
            lblBasicAmt.Text = "Basic Amount";
        }
        txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));
        txtDiscount.Text = string.Format("{0:0.00}", Math.Round(disTotal, 2));




    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgMainPO.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ShortName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("OrderQty", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_DISPACH", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("Amount", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CustItemCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CustItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TaxCategory", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TaxCatCode", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("StatusInd", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Status", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("ModNo", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ModDate", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("DocName", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_DIEAMORTRATE", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_DISC_PER", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_DISC_AMT", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_FREIGHT_CHARGES", typeof(string)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgMainPO.DataSource = dtFilter;
                dgMainPO.DataBind();
            }
        }
    }
    #endregion

    #region OrderNo
    public void OrderNo()
    {
        int OrderNo = 0;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("Select isnull(max(cast(CPOM_WORK_ODR_NO as numeric(10,0)) ),0) as CPOM_WORK_ODR_NO FROM CUSTPO_MASTER WHERE CPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
        if (dt.Rows.Count > 0)
        {
            OrderNo = Convert.ToInt32(dt.Rows[0]["CPOM_WORK_ODR_NO"]);
            OrderNo = OrderNo + 1;
            txtOrderNo.Text = OrderNo.ToString();
        }
    }
    #endregion

    #region ddlEnquiryNo_SelectedIndexChanged
    protected void ddlEnquiryNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEnquiryNo.SelectedIndex != 0)
            {
                //   SetEnquiryDetail();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "ddlEnquiryNo_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region SetEnquiryDetail
    public void SetEnquiryDetail()
    {
        try
        {
            if (Request.QueryString[0].Equals("ConvertToOrder"))
            {
                DataTable enqDetail = CommonClasses.Execute("select * from ENQUIRY_MASTER where INQ_CODE='" + Convert.ToInt32(ViewState["InqCode"]) + "' and ES_DELETE=0");
                if (enqDetail.Rows.Count > 0)
                {
                    ddlEnquiryNo.SelectedValue = enqDetail.Rows[0]["INQ_CODE"].ToString();
                    DataTable partydt = CommonClasses.Execute("select * from PARTY_MASTER where upper(P_NAME) like upper('%" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "%') and ES_DELETE=0 and P_TYPE=1");
                    if (partydt.Rows.Count > 0)
                    {
                        ddlCustomer.SelectedValue = partydt.Rows[0]["P_CODE"].ToString();
                    }
                    else
                    {
                        if (CommonClasses.Execute1("insert into PARTY_MASTER values('" + Session["CompanyId"] + "','1','" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "','','','','','','','','','','','','','','','','','','','','','','" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "',0,0,1,'','',0,'-2147483648','-2147483648','-2147483648','','','','','','','','-2147483648','-2147483648','-2147483648')"))
                        {
                            LoadCustomer();
                            string Code = CommonClasses.GetMaxId("Select Max(P_CODE) from PARTY_MASTER where ES_DELETE=0");
                            ddlCustomer.SelectedValue = Code;
                        }
                    }
                }
            }
            if (Request.QueryString[0].Equals("INSERT") && ddlEnquiryNo.SelectedIndex != 0)
            {
                DataTable enqDetail = CommonClasses.Execute("select * from ENQUIRY_MASTER where INQ_CODE='" + ddlEnquiryNo.SelectedValue + "' and ES_DELETE=0");
                if (enqDetail.Rows.Count > 0)
                {
                    ddlCustomer.SelectedValue = enqDetail.Rows[0]["INQ_CUST_NAME"].ToString();
                    //ddlEnquiryNo.SelectedValue = enqDetail.Rows[0]["INQ_CODE"].ToString();
                    //DataTable partydt = CommonClasses.Execute("select * from PARTY_MASTER where upper(P_NAME) like upper('%" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "%') and ES_DELETE=0 and P_TYPE=1");
                    //if (partydt.Rows.Count > 0)
                    //{
                    //    ddlCustomer.SelectedValue = partydt.Rows[0]["P_CODE"].ToString();
                    //}
                    //else
                    //{
                    //    if (CommonClasses.Execute1("insert into PARTY_MASTER values('" + Session["CompanyId"] + "','1','" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "','','','','','','','','','','','','','','','','','','','','','','" + enqDetail.Rows[0]["INQ_CUST_NAME"].ToString() + "',0,0,1,'','',0,'-2147483648','-2147483648','-2147483648','','','','','','','','-2147483648','-2147483648','-2147483648')"))
                    //    {
                    //        LoadCustomer();
                    //        string Code = CommonClasses.GetMaxId("Select Max(P_CODE) from PARTY_MASTER where ES_DELETE=0");
                    //        ddlCustomer.SelectedValue = Code;
                    //    }
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "SetEnquiryDetail", ex.Message);
        }
    }
    #endregion

    #region chkIsVerbal_CheckedChanged
    protected void chkIsVerbal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkIsVerbal.Checked)
            {
                txtPONumber.Text = "VERBAL";
                txtPONumber.Enabled = false;
            }
            else
            {
                txtPONumber.Text = "";
                txtPONumber.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscPer_TextChanged
    protected void txtDiscPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0";
            }
            if (txtDiscAmt.Text == "")
            {
                txtDiscAmt.Text = "0";
            }
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0";
            }
            string totalStr = DecimalMasking(txtRate.Text);
            double DiscAmt = 0;
            //txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtOrderQty.Text), 3));
            //txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            //txtDiscPer.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtDiscPer.Text), 2));
            DiscAmt = Math.Round((Convert.ToDouble(txtAmount.Text.ToString()) * Convert.ToDouble(txtDiscPer.Text)) / 100, 0);
            txtDiscAmt.Text = DiscAmt.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Transaction", "txtDiscPer_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion txtDiscPer_TextChanged

    #region lnkView_Click
    protected void lnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                directory = "../../UpLoadPath/CustPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer PO", "lnkView_Click", ex.Message);
        }
    }
    #endregion lnkView_Click
}
