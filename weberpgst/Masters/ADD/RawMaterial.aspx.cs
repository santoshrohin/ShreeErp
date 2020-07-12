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
using System.Collections.Generic;
using System.IO;
using System.Web.Services;

public partial class Masters_ADD_RawMaterial : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int File_Code = 0;
    static string right = "";
    RawMaterial_BL BL_RawMaterial = null;

    public static string imgEmpImage;
    public static string ImageUrl;
    DataTable dt = new DataTable();
    DataRow dr;
    DirectoryInfo ObjSearchDir;
    string c_type = "";
    #endregion

    [WebMethod]
    public void CheckUser(string useroremail)
    {
        DataTable dtUserCheck = new DataTable();
        dtUserCheck = CommonClasses.Execute("select I_CODE, I_CODENO,I_NAME from ITEM_MASTER where I_CODENO like '%" + useroremail + "%' AND ES_DELETE=0");
        if (dtUserCheck.Rows.Count > 0)
        {
            dgRawMaterial.DataSource = dtUserCheck;
            dgRawMaterial.DataBind();
            ModalPopupExtender1.Show();
            textPanel1.Visible = true;
        }
    }

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                c_type = Request.QueryString[1].ToString();
                ViewState["c_type"] = c_type;
            }
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='11'");
                right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadStockUOM();
                    LoadItemCategory();
                    LoadItemSubCategory();
                    LoadTariffHeading();
                    LoadSAC();
                    LoadTaxHeadSales();
                    LoadTaxHeadPurchase();
                    LoadItemSubCategory();
                    ddlItemCategory.Enabled = true;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_RawMaterial = new RawMaterial_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        ViewState["c_type"] = Request.QueryString[2].ToString();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_RawMaterial = new RawMaterial_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                        ViewState["c_type"] = Request.QueryString[2].ToString();
                    }
                    ddlItemCategory.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Item Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion Page_Load

    private void LoadTaxHeadSales()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY TALLY_NAME");


            ddlTallyAccS.DataSource = dt;
            ddlTallyAccS.DataTextField = "TALLY_NAME";
            ddlTallyAccS.DataValueField = "TALLY_CODE";
            ddlTallyAccS.DataBind();
            ddlTallyAccS.Items.Insert(0, new ListItem("----Select Tax----", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Load Tax Head", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    private void LoadTaxHeadPurchase()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY TALLY_NAME");


            ddltallyAccP.DataSource = dt;
            ddltallyAccP.DataTextField = "TALLY_NAME";
            ddltallyAccP.DataValueField = "TALLY_CODE";
            ddltallyAccP.DataBind();
            ddltallyAccP.Items.Insert(0, new ListItem("----Select Tax----", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Load Tax Head", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }

    //#region LoadAccountName
    //private void LoadAccountName()
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        //BL_EmployeeMaster = new EmployeeMaster_BL();
    //        dt = CommonClasses.Execute("select ST_CODE,ST_SALES_ACC_HEAD,ST_TAX_ACC_HEAD from SALES_TAX_MASTER where ST_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY ST_TAX_NAME");

    //        ddltallyAccP.DataSource = dt;
    //        ddltallyAccP.DataTextField = "ST_TAX_ACC_HEAD";
    //        ddltallyAccP.DataValueField = "ST_CODE";
    //        ddltallyAccP.DataBind();
    //        ddltallyAccP.Items.Insert(0, new ListItem("Select..", "0"));


    //        ddlTallyAccS.DataSource = dt;
    //        ddlTallyAccS.DataTextField = "ST_SALES_ACC_HEAD";
    //        ddlTallyAccS.DataValueField = "ST_CODE";
    //        ddlTallyAccS.DataBind();
    //        ddlTallyAccS.Items.Insert(0, new ListItem("Select..", "0"));


    //    }
    //    catch (Exception ex)
    //    {
    //        CommonClasses.SendError("Item Master", "LoadAccountName", ex.Message);
    //    }
    //    finally
    //    {
    //        //dt.Dispose();
    //    }
    //}
    //#endregion

    #region LoadStockUOM
    private void LoadStockUOM()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where I_UOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_UOM_NAME");

            ddlStockUOM.DataSource = dt;
            ddlStockUOM.DataTextField = "I_UOM_NAME";
            ddlStockUOM.DataValueField = "I_UOM_CODE";
            ddlStockUOM.DataBind();
            ddlStockUOM.Items.Insert(0, new ListItem("Select..", "0"));
            ddlWeightUOM.DataSource = dt;
            ddlWeightUOM.DataTextField = "I_UOM_NAME";
            ddlWeightUOM.DataValueField = "I_UOM_CODE";
            ddlWeightUOM.DataBind();
            ddlWeightUOM.Items.Insert(0, new ListItem("Select..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadStockUOM", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region LoadTariffHeading
    private void LoadTariffHeading()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY E_TARIFF_NO");

            ddlTariffHeading.DataSource = dt;
            ddlTariffHeading.DataTextField = "E_TARIFF_NO";
            ddlTariffHeading.DataValueField = "E_CODE";
            ddlTariffHeading.DataBind();
            ddlTariffHeading.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadTariffHeading", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region LoadSAC
    private void LoadSAC()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND E_EX_TYPE=1 AND E_TALLY_GST_EXCISE=1  ORDER BY E_TARIFF_NO");
            //dt = CommonClasses.Execute("SELECT * FROM dbo.ITEM_SUBCATEGORY_MASTER WHERE ES_DELETE = '0'");
            ddlSAC.DataSource = dt;
            ddlSAC.DataTextField = "E_TARIFF_NO";
            ddlSAC.DataValueField = "E_CODE";
            ddlSAC.DataBind();
            ddlSAC.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadTariffHeading", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region LoadItemCategory
    private void LoadItemCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select I_CAT_CODE,I_CAT_NAME from ITEM_CATEGORY_MASTER where I_CAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_CAT_NAME");

            ddlItemCategory.DataSource = dt;
            ddlItemCategory.DataTextField = "I_CAT_NAME";
            ddlItemCategory.DataValueField = "I_CAT_CODE";
            ddlItemCategory.DataBind();
            ddlItemCategory.Items.Insert(0, new ListItem("Select Category", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadStockUOM", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region ddlItemCategory_SelectedIndexChanged
    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadItemSubCategory();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ddlItemCategory_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region LoadItemSubCategory
    private void LoadItemSubCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            if (ddlItemCategory.SelectedIndex == 0)
            {
                dt = CommonClasses.Execute("select SCAT_CODE,SCAT_DESC from ITEM_SUBCATEGORY_MASTER where SCAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCAT_DESC");
                // ddlSubCategory.Enabled = false;
            }
            else
            {
                dt = CommonClasses.Execute("select DISTINCT SCAT_CODE,SCAT_DESC from ITEM_SUBCATEGORY_MASTER where SCAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and SCAT_CAT_CODE='" + ddlItemCategory.SelectedValue + "' and ES_DELETE=0 ORDER BY SCAT_DESC");
                // ddlSubCategory.Enabled = true;
            }
            ddlSubCategory.DataSource = dt;
            ddlSubCategory.DataTextField = "SCAT_DESC";
            ddlSubCategory.DataValueField = "SCAT_CODE";
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, new ListItem("Select Sub Category", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadItemSubCategory", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            #region OldCode

            //DataTable dt = new DataTable();

            //dt = CommonClasses.Execute("Select I_CODE,I_CM_COMP_ID,I_CAT_CODE,I_CODENO,I_DRAW_NO,I_NAME,I_MATERIAL,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_UOM_CODE,I_INV_CAT,I_UWEIGHT,I_ACTIVE_IND,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL,I_OP_BAL,I_OPEN_RATE,I_STORE_LOC,I_INV_RATE,I_CURRENT_BAL,I_RECEIPT_DATE,I_ISSUE_DATE FROM ITEM_MASTER WHERE I_CODE=" + mlCode + " AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            //if (dt.Rows.Count > 0)
            //{
            //    mlCode = Convert.ToInt32(dt.Rows[0]["I_CODE"]);
            //    ddlItemCategory.SelectedValue = dt.Rows[0]["I_CAT_CODE"].ToString();
            //    txtItemCode.Text = dt.Rows[0]["I_CODENO"].ToString();
            //    txtDrawingNumnber.Text = dt.Rows[0]["I_DRAW_NO"].ToString();
            //    txtItemName.Text = dt.Rows[0]["I_NAME"].ToString();
            //    txtMaterialDetails.Text = dt.Rows[0]["I_MATERIAL"].ToString();
            //    txtLenght.Text = dt.Rows[0]["I_LENGTH"].ToString();
            //    txtHeight.Text = dt.Rows[0]["I_HEIGHT"].ToString();
            //    txtWidth.Text = dt.Rows[0]["I_WIDTH"].ToString();
            //    txtRows.Text = dt.Rows[0]["I_ROWS"].ToString();
            //    txtSroried.Text = dt.Rows[0]["I_STORIED"].ToString();
            //    ddlStockUOM.SelectedValue = dt.Rows[0]["I_UOM_CODE"].ToString();
            //    ddlInventoryCategory.SelectedIndex = Convert.ToInt32(dt.Rows[0]["I_INV_CAT"].ToString());
            //    txtUniWeight.Text = dt.Rows[0]["I_UWEIGHT"].ToString();
            //    txtMaximumLevel.Text = dt.Rows[0]["I_MAX_LEVEL"].ToString();
            //    txtMinimumLevel.Text = dt.Rows[0]["I_MIN_LEVEL"].ToString();
            //    txtReOrderLevel.Text = dt.Rows[0]["I_REORDER_LEVEL"].ToString();
            //    txtOpeningBalance.Text = dt.Rows[0]["I_OP_BAL"].ToString();
            //    txtOpeningBalanceRate.Text = dt.Rows[0]["I_OPEN_RATE"].ToString();
            //    txtStoreLocation.Text = dt.Rows[0]["I_STORE_LOC"].ToString();
            //    txtInventoryRate.Text = dt.Rows[0]["I_INV_RATE"].ToString();
            //    txtCurrentBal.Text = dt.Rows[0]["I_CURRENT_BAL"].ToString();
            //    txtLastRecdDate.Text = dt.Rows[0]["I_RECEIPT_DATE"].ToString();
            //    txtLastIssueDate.Text = dt.Rows[0]["I_ISSUE_DATE"].ToString();
            //    //imgEmpImage = dt.Rows[0]["I_DOC_NAME"].ToString();


            //    //LblFileName.Text = "Previous Attached File " + dt.Rows[0]["I_DOC_NAME"].ToString();
            //    //LblFileName.Visible = true;
            //    //btnDownload.Visible = true;
            //    if (Convert.ToBoolean(dt.Rows[0]["I_ACTIVE_IND"].ToString()) == true)
            //    {
            //        ChkActiveInd.Checked = true;
            //    }
            //    else
            //    {
            //        ChkActiveInd.Checked = false;
            //    }

            //    if (str == "VIEW")
            //    {
            //        txtItemCode.Enabled = false;
            //        txtDrawingNumnber.Enabled = false;
            //        txtItemName.Enabled = false;
            //        txtMaterialDetails.Enabled = false;
            //        txtLenght.Enabled = false;
            //        txtHeight.Enabled = false;
            //        txtWidth.Enabled = false;
            //        txtRows.Enabled = false;
            //        txtSroried.Enabled = false;
            //        ddlStockUOM.Enabled = false;
            //        ddlInventoryCategory.Enabled = false;
            //        ChkActiveInd.Enabled = false;
            //        txtMaximumLevel.Enabled = false;
            //        txtMinimumLevel.Enabled = false;
            //        txtReOrderLevel.Enabled = false;
            //        txtOpeningBalance.Enabled = false;
            //        txtOpeningBalanceRate.Enabled = false;
            //        txtInventoryRate.Enabled = false;
            //        txtCurrentBal.Enabled = false;
            //        txtLastIssueDate.Enabled = false;
            //        txtLastRecdDate.Enabled = false;
            //        txtStoreLocation.Enabled = false;
            //        txtUniWeight.Enabled = false;
            //        btnSubmit.Visible = false;

            //    }
            //    else if (str == "MOD")
            //    {
            //        CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
            //    }
            //} 
            #endregion
            try
            {
                BL_RawMaterial = new RawMaterial_BL(mlCode);
                DataTable dt = new DataTable();
                BL_RawMaterial.I_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);

                BL_RawMaterial.GetInfo();
                GetValues(str);
                if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Item Master", "ViewRec", Ex.Message);
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Item Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            mlCode = BL_RawMaterial.I_CODE;
            ddlItemCategory.SelectedValue = BL_RawMaterial.I_CAT_CODE.ToString();

            ddlSubCategory.Enabled = true;
            ddlSAC.SelectedValue = BL_RawMaterial.I_SCAT_CODE.ToString();
            ddlSubCategory.SelectedValue = BL_RawMaterial.I_SUBCAT_CODE.ToString();
            txtItemCode.Text = BL_RawMaterial.I_CODENO;
            txtDrawingNumnber.Text = BL_RawMaterial.I_DRAW_NO;
            txtItemName.Text = BL_RawMaterial.I_NAME;
            //txtMaterialDetails.Text = BL_RawMaterial.I_MATERIAL;
            txtSpecifications.Text = BL_RawMaterial.I_SPECIFICATION;
            //txtSize.Text = BL_RawMaterial.I_SIZE;
            ddlTariffHeading.SelectedValue = BL_RawMaterial.I_E_CODE.ToString();
            ddltallyAccP.SelectedValue = BL_RawMaterial.I_ACCOUNT_PURCHASE.ToString();
            ddlTallyAccS.SelectedValue = BL_RawMaterial.I_ACCOUNT_SALES.ToString();
            txtCoastingHead.Text = BL_RawMaterial.I_COSTING_HEAD;
            ddlStockUOM.SelectedValue = BL_RawMaterial.I_UOM_CODE.ToString();
            ddlInventoryCategory.SelectedIndex = Convert.ToInt32(BL_RawMaterial.I_INV_CAT);
            txtUniWeight.Text = BL_RawMaterial.I_UWEIGHT.ToString();
            txtMaximumLevel.Text = BL_RawMaterial.I_MAX_LEVEL.ToString();
            txtMinimumLevel.Text = BL_RawMaterial.I_MIN_LEVEL.ToString();
            txtReOrderLevel.Text = BL_RawMaterial.I_REORDER_LEVEL.ToString();
            txtOpeningBalance.Text = BL_RawMaterial.I_OP_BAL.ToString();
            txtOpeningBalanceRate.Text = BL_RawMaterial.I_OP_BAL_RATE.ToString();
            txtStoreLocation.Text = BL_RawMaterial.I_STORE_LOC.ToString();
            txtInventoryRate.Text = BL_RawMaterial.I_INV_RATE.ToString();
            txtCurrentBal.Text = BL_RawMaterial.I_CURRENT_BAL.ToString();
            txtLastRecdDate.Text = Convert.ToDateTime(BL_RawMaterial.I_RECEIPT_DATE).ToString("dd MMM yyyy");
            txtLastIssueDate.Text = Convert.ToDateTime(BL_RawMaterial.I_ISSUE_DATE).ToString("dd MMM yyyy"); ;
            txtDensity.Text = BL_RawMaterial.I_DENSITY.ToString();
            txtPigment.Text = BL_RawMaterial.I_PIGMENT.ToString();
            txtSolids.Text = BL_RawMaterial.I_SOLIDS.ToString();
            txtVolatile.Text = BL_RawMaterial.I_VOLATILE.ToString();
            ddlWeightUOM.SelectedValue = BL_RawMaterial.I_WEIGHT_UOM.ToString();
            ChkActiveInd.Checked = BL_RawMaterial.I_ACTIVE_IND;
            chkDevelopment.Checked = BL_RawMaterial.I_DEVELOMENT;
            txtTargetWeight.Text = BL_RawMaterial.I_TARGET_WEIGHT.ToString();
            //imgEmpImage = dt.Rows[0]["I_DOC_NAME"].ToString();


            //LblFileName.Text = "Previous Attached File " + dt.Rows[0]["I_DOC_NAME"].ToString();
            //LblFileName.Visible = true;
            //btnDownload.Visible = true;
            if (Convert.ToBoolean(BL_RawMaterial.I_ACTIVE_IND) == true)
            {
                ChkActiveInd.Checked = true;
            }
            else
            {
                ChkActiveInd.Checked = false;
            }

            if (str == "VIEW")
            {
                ddlSubCategory.Enabled = false;
                ddlItemCategory.Enabled = false;
                txtItemCode.Enabled = false;
                txtDrawingNumnber.Enabled = false;
                txtItemName.Enabled = false;
                //txtMaterialDetails.Enabled = false;
                // txtSize.Enabled= false;
                ddltallyAccP.Enabled = false;
                ddlTallyAccS.Enabled = false;
                txtCoastingHead.Enabled = false;
                ddlTariffHeading.Enabled = false;
                ddlStockUOM.Enabled = false;
                ddlInventoryCategory.Enabled = false;
                ChkActiveInd.Enabled = false;
                txtMaximumLevel.Enabled = false;
                txtMinimumLevel.Enabled = false;
                txtReOrderLevel.Enabled = false;
                txtOpeningBalance.Enabled = false;
                txtOpeningBalanceRate.Enabled = false;
                txtInventoryRate.Enabled = false;
                txtCurrentBal.Enabled = false;
                txtLastIssueDate.Enabled = false;
                txtLastRecdDate.Enabled = false;
                txtStoreLocation.Enabled = false;
                txtUniWeight.Enabled = false;
                btnSubmit.Visible = false;
                txtDensity.Enabled = false;
                txtPigment.Enabled = false;
                txtSolids.Enabled = false;
                txtVolatile.Enabled = false;
                txtSpecifications.Enabled = false;
                ddlSAC.Enabled = false;
                ddlWeightUOM.Enabled = false;
                chkDevelopment.Enabled = false;
                txtTargetWeight.Enabled = false;
            }
            else if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "GetValues", ex.Message);
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
            string StrReplaceCodeno = txtItemCode.Text.Trim();
            string StrReplaceName = txtItemName.Text.Trim();
            //string StrReplaceMaterial = txtMaterialDetails.Text;
            string StrReplaceSpecify = txtSpecifications.Text.Trim();
            string StrReplaceCostHeading = txtCoastingHead.Text.Trim();
            //string StrReplaceSize= txtSize.Text;


            StrReplaceCodeno = StrReplaceCodeno.Replace("'", "''");
            StrReplaceName = StrReplaceName.Replace("'", "''");
            //StrReplaceMaterial = StrReplaceMaterial.Replace("'", "''");
            StrReplaceSpecify = StrReplaceSpecify.Replace("'", "''");
            StrReplaceCostHeading = StrReplaceCostHeading.Replace("'", "''");
            // StrReplaceSize = StrReplaceSize.Replace("'", "''");

            //I_CODE = Convert.ToInt32(dt.Rows[0]["I_CODE"]);
            BL_RawMaterial.I_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_RawMaterial.I_CAT_CODE = Convert.ToInt32(ddlItemCategory.SelectedValue);
            BL_RawMaterial.I_SCAT_CODE = Convert.ToInt32(ddlSAC.SelectedValue);
            BL_RawMaterial.I_CODENO = StrReplaceCodeno;
            BL_RawMaterial.I_DRAW_NO = txtDrawingNumnber.Text.Trim().Replace("'", "\'");
            BL_RawMaterial.I_NAME = StrReplaceName;
            BL_RawMaterial.I_SUBCAT_CODE = Convert.ToInt32(ddlSubCategory.SelectedValue);
            BL_RawMaterial.I_SPECIFICATION = StrReplaceSpecify;
            BL_RawMaterial.I_E_CODE = Convert.ToInt32(ddlTariffHeading.SelectedValue);
            BL_RawMaterial.I_ACCOUNT_SALES = Convert.ToInt32(ddlTallyAccS.SelectedValue);
            BL_RawMaterial.I_ACCOUNT_PURCHASE = Convert.ToInt32(ddltallyAccP.SelectedValue);
            BL_RawMaterial.I_UOM_CODE = Convert.ToInt32(ddlStockUOM.SelectedValue);
            BL_RawMaterial.I_INV_CAT = Convert.ToInt32(ddlInventoryCategory.SelectedIndex).ToString();
            BL_RawMaterial.I_MAX_LEVEL = Convert.ToDouble(txtMaximumLevel.Text == "" ? "0" : txtMaximumLevel.Text);
            BL_RawMaterial.I_MIN_LEVEL = Convert.ToDouble(txtMinimumLevel.Text == "" ? "0" : txtMinimumLevel.Text);
            BL_RawMaterial.I_REORDER_LEVEL = Convert.ToDouble(txtReOrderLevel.Text == "" ? "0" : txtReOrderLevel.Text);
            BL_RawMaterial.I_OP_BAL = Convert.ToDouble(txtOpeningBalance.Text == "" ? "0" : txtOpeningBalance.Text);
            BL_RawMaterial.I_OP_BAL_RATE = Convert.ToDouble(txtOpeningBalanceRate.Text == "" ? "0" : txtOpeningBalanceRate.Text);
            BL_RawMaterial.I_STORE_LOC = txtStoreLocation.Text.Trim().Replace("'", "\'");
            BL_RawMaterial.I_INV_RATE = Convert.ToDouble(txtInventoryRate.Text == "" ? "0" : txtInventoryRate.Text);
            string receiptDate = DateTime.Now.ToString();
            BL_RawMaterial.I_RECEIPT_DATE = Convert.ToDateTime(txtLastRecdDate.Text == "" ? Convert.ToDateTime(receiptDate).ToString("dd/MMM/yyyy") : txtLastRecdDate.Text);
            BL_RawMaterial.I_ISSUE_DATE = Convert.ToDateTime(txtLastIssueDate.Text == "" ? Convert.ToDateTime(receiptDate).ToString("dd/MMM/yyyy") : txtLastIssueDate.Text);
            BL_RawMaterial.I_CURRENT_BAL = Convert.ToDouble(txtCurrentBal.Text == "" ? "0" : txtCurrentBal.Text);
            BL_RawMaterial.I_ACTIVE_IND = Convert.ToBoolean(ChkActiveInd.Checked);
            BL_RawMaterial.I_UWEIGHT = Convert.ToDouble(txtUniWeight.Text == "" ? "0" : txtUniWeight.Text);
            BL_RawMaterial.I_COSTING_HEAD = StrReplaceCostHeading;
            //BL_RawMaterial.I_SIZE = StrReplaceSize;
            BL_RawMaterial.I_INV_CAT = Convert.ToInt32(ddlInventoryCategory.SelectedValue).ToString();
            BL_RawMaterial.I_OP_BAL_RATE = Convert.ToDouble(txtOpeningBalanceRate.Text == "" ? "0" : txtOpeningBalanceRate.Text);
            BL_RawMaterial.I_DENSITY = Math.Round(Convert.ToDouble(txtDensity.Text == "" ? "0" : txtDensity.Text.ToString()), 2);
            BL_RawMaterial.I_PIGMENT = Math.Round(Convert.ToDouble(txtPigment.Text == "" ? "0" : txtPigment.Text.ToString()), 2);
            BL_RawMaterial.I_SOLIDS = Math.Round(Convert.ToDouble(txtSolids.Text == "" ? "0" : txtSolids.Text.ToString()), 2);
            BL_RawMaterial.I_VOLATILE = Math.Round(Convert.ToDouble(txtVolatile.Text == "" ? "0" : txtVolatile.Text.ToString()), 2);
            BL_RawMaterial.I_WEIGHT_UOM = Convert.ToInt32(ddlWeightUOM.SelectedValue);
            BL_RawMaterial.I_DEVELOMENT = Convert.ToBoolean(chkDevelopment.Checked);
            BL_RawMaterial.I_TARGET_WEIGHT = Convert.ToDouble(txtTargetWeight.Text == "" ? "0" : txtTargetWeight.Text);

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Setvalues", ex.Message);
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
            string fileName = "";

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_RawMaterial = new RawMaterial_BL();
                if (Setvalues())
                {
                    if (BL_RawMaterial.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(I_CODE) from ITEM_MASTER");
                        CommonClasses.WriteLog("item Master", "Save", "item Master", BL_RawMaterial.I_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + ViewState["c_type"] + "", false);
                    }
                    else
                    {
                        if (BL_RawMaterial.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RawMaterial.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            //lblmsg.Text = BL_RawMaterial.Msg;
                            //PanelMsg.Visible = true;
                            //BL_RawMaterial.Msg = "";
                        }
                        ddlItemCategory.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_RawMaterial = new RawMaterial_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_RawMaterial.Update())
                    {
                        CommonClasses.RemoveModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
                        CommonClasses.WriteLog("item Master", "Update", "item Master", BL_RawMaterial.I_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + ViewState["c_type"] + "", false);
                    }
                    else
                    {
                        if (BL_RawMaterial.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RawMaterial.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_RawMaterial.Msg = "";
                        }
                        ddlItemCategory.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
                SaveRec();
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Item Master", "btnSubmit_Click", Ex.Message);
        }
    }

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
            CommonClasses.SendError("Item Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtItemCode.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtItemName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlItemCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlSubCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlStockUOM.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlInventoryCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddltallyAccP.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlTallyAccS.SelectedIndex <= 0)
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
            CommonClasses.SendError("Item Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + ViewState["c_type"] + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "btnCancel_Click", Ex.Message);
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

    protected void txtMinimumLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtMinimumLevel.Text);

        txtMinimumLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

    }
    protected void txtReOrderLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtReOrderLevel.Text);

        txtReOrderLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    protected void txtInventoryRate_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtInventoryRate.Text);

        txtInventoryRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    protected void txtMaximumLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtMaximumLevel.Text);

        txtMaximumLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    protected void txtUniWeight_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtUniWeight.Text);

        txtUniWeight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    protected void txtTargetWeight_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtTargetWeight.Text);

        txtTargetWeight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    protected void txtItemCode_TextChanged(object sender, EventArgs e)
    {
        if (txtItemCode.Text.Trim() != "")
        {
            CheckUser(txtItemCode.Text.Trim());

        }
        txtDrawingNumnber.Text = txtItemCode.Text;
    }
}