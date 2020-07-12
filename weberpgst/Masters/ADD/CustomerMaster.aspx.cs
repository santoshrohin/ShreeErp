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

public partial class Masters_ADD_CustomerMaster : System.Web.UI.Page
{
    #region Declartion

    static int mlCode = 0;
    static string right = "";
    int CValue;
    int SValue;
    #endregion

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
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='11'");
                //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    //LoadSector();
                    LoadCountry();
                    LoadState();
                    LoadArea();
                    //LoadExise();
                    LoadCity();
                    LoadCustomerType();
                    //txtLBTNo.Enabled = false;

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        //BL_DepartmentMaster = new DepartmentMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        //BL_DepartmentMaster = new DepartmentMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    //LoadState();
                    txtPartyName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Customer Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (chlLBTApplicable.Checked == true)
            {
                if (txtLBTNo.Text == "" || txtLBTNo.Text == "0")
                {
                    ShowMessage("#Avisos", "Enter GST NO.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        SValue = Convert.ToInt32(ddlState.SelectedValue);
        LoadCity();
    }
    #endregion

    #region Methods


    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                //dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + "");
                dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + Session["CompanyId"] + "");

                ddlCountry.DataSource = dt;
                ddlCountry.DataTextField = "COUNTRY_NAME";
                ddlCountry.DataValueField = "COUNTRY_CODE";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("----Select Country----", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadCountry", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "LoadCountry", ex.Message);
        }
    }
    #endregion Load Country

    #region Load State
    private void LoadState()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + " and SM_COUNTRY_CODE= " + CValue + "");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("----Select State----", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadSate", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "LoadState", ex.Message);
        }
    }
    #endregion Load State

    #region LoadCity
    private void LoadCity()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select CITY_CODE,CITY_NAME from CITY_MASTER where ES_DELETE=0 and CITY_CM_COMP_ID=" + Session["CompanyId"] + " and CITY_SM_CODE= " + SValue + "");
                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "CITY_NAME";
                ddlCity.DataValueField = "CITY_CODE";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("----Select City----", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadSate", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "LoadState", ex.Message);
        }
    }
    #endregion Load City

    #region LoadCustomerType
    private void LoadCustomerType()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
            dt = CommonClasses.Execute("select CTM_CODE,CTM_TYPE_CODE from CUSTOMER_TYPE_MASTER where CTM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0");


            ddlCustomerType.DataSource = dt;
            ddlCustomerType.DataTextField = "CTM_TYPE_CODE";
            ddlCustomerType.DataValueField = "CTM_CODE";
            ddlCustomerType.DataBind();
            ddlCustomerType.Items.Insert(0, new ListItem("Select..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "LoadCustomerType", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    //#region LoadExise
    //private void LoadExise()
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        //BL_EmployeeMaster = new EmployeeMaster_BL();
    //        //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
    //        dt = CommonClasses.Execute("select E_CODE,E_EX_TYPE from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY E_EX_TYPE");


    //        ddlExiseType.DataSource = dt;
    //        ddlExiseType.DataTextField = "E_EX_TYPE";
    //        ddlExiseType.DataValueField = "E_CODE";
    //        ddlExiseType.DataBind();
    //        ddlExiseType.Items.Insert(0, new ListItem("----Select Excise----", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //        CommonClasses.SendError("Customer Master", "LoadDesignation", ex.Message);
    //    }
    //    finally
    //    {
    //        //dt.Dispose();
    //    }
    //}
    //#endregion

    //#region LoadSector
    //private void LoadSector()
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        BL_EmployeeMaster = new EmployeeMaster_BL();
    //        dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");

    //        ddlSector.DataSource = dt;
    //        ddlSector.DataTextField = "SCT_DESC";
    //        ddlSector.DataValueField = "SCT_CODE";
    //        ddlSector.DataBind();
    //        ddlSector.Items.Insert(0, new ListItem("----Select Sector----", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //        CommonClasses.SendError("Customer Master", "LoadDesignation", ex.Message);
    //    }
    //    finally
    //    {
    //        dt.Dispose();
    //    }
    //}
    //#endregion

    #region LoadArea
    private void LoadArea()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
            dt = CommonClasses.Execute("select A_CODE,A_DESC from AREA_MASTER where A_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0");


            ddlArea.DataSource = dt;
            ddlArea.DataTextField = "A_DESC";
            ddlArea.DataValueField = "A_CODE";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("----Select Area----", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "LoadDesignation", ex.Message);
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
            DataTable dt = new DataTable();
            //dt = CommonClasses.Execute("SELECT P_CODE,P_CM_COMP_ID,P_TYPE,P_NAME,P_CONTACT,P_PARTY_CODE,P_ADD1,P_PIN_CODE,P_PHONE,P_MOB,P_SCT_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_ACTIVE_IND,P_CUST_TYPE,P_TALLY,P_SM_CODE,P_CITY_CODE,P_COORDINATOR,P_COORDINATOR_EMAIL,P_DELIVERY_ADD,P_NOTE FROM PARTY_MASTER WHERE P_CODE=" + mlCode + " AND P_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            dt = CommonClasses.Execute(" SELECT P_CODE,P_CM_COMP_ID,P_TYPE,P_NAME,P_CONTACT,P_PARTY_CODE,P_VEND_CODE,P_ADD1,P_CITY,P_DISTRICT,P_PIN_CODE,P_PHONE,P_MOB,P_SCT_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_SER_TAX_NO,P_ECC_NO,P_CATEGORY,P_EXC_DIV,P_EXC_RANGE,P_EXC_COLLECTORATE,P_TALLY,ES_DELETE,MODIFY,isnull(P_ACTIVE_IND,0) as P_ACTIVE_IND,isnull(P_INHOUSE_IND,0) as P_INHOUSE_IND,isnull(P_LBT_NO,0) as P_LBT_NO,P_LBT_IND,P_CUST_TYPE,P_SM_CODE,P_CITY_CODE,P_COORDINATOR,P_COORDINATOR_EMAIL,P_DELIVERY_ADD,P_NOTE,P_CREDITDAYS,P_TDS,P_E_CODE,P_A_CODE,P_COUNTRY_CODE,P_STM_CODE,P_CUST_TYPE,P_ABBREVATION FROM PARTY_MASTER WHERE P_CODE=" + mlCode + " AND P_CM_COMP_ID = " + (string)Session["CompanyId"] + " and P_TYPE=1 and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["P_CODE"]); ;
                txtPartyCode.Text = dt.Rows[0]["P_PARTY_CODE"].ToString();
                ddlCustomerType.SelectedValue = dt.Rows[0]["P_CUST_TYPE"].ToString();
                //ddlSector.SelectedValue = dt.Rows[0]["P_SCT_CODE"].ToString();
                //txtCustomerType.Text = dt.Rows[0]["P_CUST_TYPE"].ToString();
                txtPartyName.Text = dt.Rows[0]["P_NAME"].ToString();
                txtContactPerson.Text = dt.Rows[0]["P_CONTACT"].ToString();
                txtAddress.Text = dt.Rows[0]["P_ADD1"].ToString();
                txtPhoneNo.Text = dt.Rows[0]["P_PHONE"].ToString();
                txtEmailId.Text = dt.Rows[0]["P_EMAIL"].ToString();
                txtTallyName.Text = dt.Rows[0]["P_TALLY"].ToString();
                txtPhoneNo.Text = dt.Rows[0]["P_PHONE"].ToString();
                txtVATTINNo.Text = dt.Rows[0]["P_VAT"].ToString();
                txtCSTTINNo.Text = dt.Rows[0]["P_CST"].ToString();
                txtFaxNo.Text = dt.Rows[0]["P_FAX"].ToString();
                txtMobileNo.Text = dt.Rows[0]["P_MOB"].ToString();
                txtPinCode.Text = dt.Rows[0]["P_PIN_CODE"].ToString();
                txtPANNO.Text = dt.Rows[0]["P_PAN"].ToString();
                txtCreditdays.Text = dt.Rows[0]["P_CREDITDAYS"].ToString();

                txtVendorCode.Text = dt.Rows[0]["P_VEND_CODE"].ToString();
                txtECCNo.Text = dt.Rows[0]["P_ECC_NO"].ToString();
                txtExiseDevision.Text = dt.Rows[0]["P_EXC_DIV"].ToString();
                txtExiseRange.Text = dt.Rows[0]["P_EXC_RANGE"].ToString();
                txtExiseCollectorate.Text = dt.Rows[0]["P_EXC_COLLECTORATE"].ToString();
                txtTDSTAX.Text = dt.Rows[0]["P_TDS"].ToString();
                txtLBTNo.Text = dt.Rows[0]["P_LBT_NO"].ToString();
                txtSerTaxRegNo.Text = dt.Rows[0]["P_SER_TAX_NO"].ToString();
                ddlCategory.Text = dt.Rows[0]["P_CATEGORY"].ToString();
                ddlArea.Text = dt.Rows[0]["P_A_CODE"].ToString();
                ddlExiseType.Text = dt.Rows[0]["P_E_CODE"].ToString();
                txtAbbrevation.Text = dt.Rows[0]["P_ABBREVATION"].ToString();


                ddlCountry.SelectedValue = dt.Rows[0]["P_COUNTRY_CODE"].ToString();
                ddlCountry_SelectedIndexChanged(null, null);
                ddlState.SelectedValue = dt.Rows[0]["P_SM_CODE"].ToString();
                ddlState_SelectedIndexChanged(null, null);
                ddlCity.SelectedValue = dt.Rows[0]["P_CITY_CODE"].ToString();



                if (Convert.ToBoolean(dt.Rows[0]["P_ACTIVE_IND"].ToString()) == true)
                {
                    ChkActiveInd.Checked = true;
                    //txtLBTNo.Enabled = true;
                }
                else
                {
                    ChkActiveInd.Checked = false;
                }

                //if (Convert.ToBoolean(dt.Rows[0]["P_INHOUSE_IND"].ToString()) == true)
                //{
                //    ChkInHoursParty.Checked = true;
                //}
                //else
                //{
                //    ChkInHoursParty.Checked = false;
                //}

                if (Convert.ToBoolean(dt.Rows[0]["P_LBT_IND"].ToString()) == true)
                {
                    chlLBTApplicable.Checked = true;
                }
                else
                {
                    chlLBTApplicable.Checked = false;
                }



                if (str == "VIEW")
                {
                    txtAddress.Enabled = false;
                    txtContactPerson.Enabled = false;
                    txtCreditdays.Enabled = false;
                    txtCSTTINNo.Enabled = false;
                    txtECCNo.Enabled = false;
                    txtEmailId.Enabled = false;
                    txtExiseCollectorate.Enabled = false;
                    txtExiseDevision.Enabled = false;
                    txtExiseRange.Enabled = false;
                    txtFaxNo.Enabled = false;
                    txtLBTNo.Enabled = false;
                    txtMobileNo.Enabled = false;
                    txtPANNO.Enabled = false;
                    txtPartyCode.Enabled = false;
                    txtPartyName.Enabled = false;
                    txtPhoneNo.Enabled = false;
                    txtPinCode.Enabled = false;
                    txtSerTaxRegNo.Enabled = false;
                    //txtCustomerType.Enabled = false;
                    txtTallyName.Enabled = false;
                    txtTDSTAX.Enabled = false;
                    ddlCustomerType.Enabled = false;
                    txtVATTINNo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    ddlArea.Enabled = false;
                    ddlCategory.Enabled = false;
                    ddlCity.Enabled = false;
                    ddlCountry.Enabled = false;
                    //txtCustomerType.Enabled = false;
                    ddlExiseType.Enabled = false;
                    //ddlSector.Enabled = false;
                    ddlState.Enabled = false;


                    btnSubmit.Visible = false;

                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        string StrPartyName = txtPartyName.Text;
        //string StrReplaceCustinfo = txtCustomerInfo.Text;
        string StrReplaceCustAddt = txtAddress.Text;
        string StrReplacePerson = txtContactPerson.Text;
        //string StrReplaceCoordinator = txtCoordinator.Text;
        //string StrReplaceDelAdd = txtDeliveryAdd.Text;
        string StrReplacetallyname = txtTallyName.Text;
        StrPartyName = StrPartyName.Replace("'", "''");
        //StrReplaceCustinfo = StrReplaceCustinfo.Replace("'", "''");
        StrReplaceCustAddt = StrReplaceCustAddt.Replace("'", "''");
        StrReplacePerson = StrReplacePerson.Replace("'", "''");
        //StrReplaceCoordinator = StrReplaceCoordinator.Replace("'", "''");
        //StrReplaceDelAdd = StrReplaceDelAdd.Replace("'", "''");
        StrReplacetallyname = StrReplacetallyname.Replace("'", "''");

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                string Custmer_No = "";
                dt = CommonClasses.Execute("SELECT P_CODE,P_NAME,P_PARTY_CODE FROM PARTY_MASTER WHERE P_NAME= lower('" + txtPartyName.Text.Trim() + "') and P_TYPE=1 and ES_DELETE='False'");
                //result = CommonClasses.CheckExistSave("PARTY_MASTER", "P_ABBREVATION", txtAbbrevation.Text.Trim().ToUpper());
                //if(result)
                if (dt.Rows.Count == 0)
                {
                    if (txtAbbrevation.Text.Trim() != "")
                    {
                        DataTable dtAbb = new DataTable();
                        dtAbb = CommonClasses.Execute("Select * FROM PARTY_MASTER WHERE P_ABBREVATION= UPPER('" + txtAbbrevation.Text.Trim() + "') and P_TYPE=1 and ES_DELETE='False' and P_CM_COMP_ID='" + Session["CompanyId"] + "'");
                        if (dtAbb.Rows.Count > 0)
                        {
                            ShowMessage("#Avisos", "Abbrevation Already Exists", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtAbbrevation.Focus();
                        }
                    }
                    else
                    {
                        int CUST_NO = 0;
                        dt = CommonClasses.Execute("SELECT ISNULL(MAX(P_PARTY_CODE),0) AS P_PARTY_CODE FROM PARTY_MASTER WHERE P_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                        if (dt.Rows.Count > 0)
                        {
                            CUST_NO = Convert.ToInt32(dt.Rows[0]["P_PARTY_CODE"]);
                            string strYear = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy").Substring(9, 2) + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy").ToString().Substring(9, 2);
                            CUST_NO = CUST_NO + 1;
                            Custmer_No = "HIPL" + "/" + strYear + CommonClasses.GenBillNo(CUST_NO);
                        }
                        //if (CommonClasses.Execute1("INSERT INTO PARTY_MASTER (P_CM_COMP_ID,P_TYPE,P_NAME,P_CONTACT,P_PARTY_CODE,P_ADD1,P_PHONE,P_MOB,P_SCT_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_ACTIVE_IND,P_TALLY,P_CUST_TYPE,P_SM_CODE,P_CITY_CODE,P_PIN_CODE,P_COORDINATOR,P_COORDINATOR_EMAIL,P_DELIVERY_ADD,P_NOTE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "',1,'" + StrReplaceName + "','" + StrReplacePerson + "'," + Convert.ToInt32(CUST_NO) + ",'" + StrReplaceCustAddt + "','" + txtPhoneNo.Text + "','" + txtMobNo.Text + "','" + ddlSector.SelectedValue.ToString() + "','" + txtFAXNo.Text + "','" + txtEmailID.Text + "','" + txtPANNo.Text + "','" + txtCSTTINNo.Text + "','" + txtVATTINNo.Text + "','1','" + StrReplacetallyname + "','0','" + ddlState.SelectedValue + "','" + ddlCity.SelectedValue + "','" + txtPinCode.Text + "','" + StrReplaceCoordinator + "','" + txtCoordinateEmail.Text + "','" + StrReplaceDelAdd + "','" + StrReplaceCustinfo + "')"))
                        // if (CommonClasses.Execute1("INSERT INTO PARTY_MASTER (P_TYPE,P_CM_COMP_ID,P_SCT_CODE,P_NAME,P_CONTACT,P_VEND_CODE,P_ADD1,P_COUNTRY_CODE,P_CITY_CODE,P_SM_CODE,P_PIN_CODE,P_PHONE,P_MOB,P_CATEGORY,P_A_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_SER_TAX_NO,P_ECC_NO,P_E_CODE,P_EXC_DIV,P_EXC_COLLECTORATE,P_TALLY,P_ACTIVE_IND,P_INHOUSE_IND,P_TDS,P_LBT_IND,P_LBT_NO )VALUES(1,'" + Convert.ToInt32(Session["CompanyId"]) + "','"+ddlSector.SelectedValue.ToString()+"','"+StrPartyName+"','"+StrReplacePerson+"','"+txtVendorCode.Text+"','"+StrReplaceCustAddt+"','"+ddlCountry.SelectedValue.ToString()+"','"+ddlCity.SelectedValue.ToString()+"','"+ddlState.SelectedValue.ToString()+"','"+txtPinCode.Text+"','"+txtPhoneNo.Text+"','"+txtMobileNo.Text+"',"+ Convert.ToInt16(ddlCategory.SelectedValue.ToString())+",'"+ddlArea.SelectedValue.ToString()+"','"+txtFaxNo.Text+"','"+txtEmailId.Text+"','"+txtPANNO.Text+"','"+txtCSTTINNo.Text+"','"+txtSerTaxRegNo.Text+"','"+txtECCNo.Text+"','"+ddlExiseType.SelectedValue.ToString()+"','"+txtExiseDevision.Text+"','"+txtExiseRange.Text+"','"+txtExiseCollectorate.Text+"','"+StrReplacetallyname+"','"+ChkActiveInd.Checked+"','"+ChkInHoursParty.Checked+"','"+txtTDSTAX.Text+"','"+chlLBTApplicable.Checked+"','"+txtLBTNo.Text+"') "))
                        if (CommonClasses.Execute1("INSERT INTO PARTY_MASTER (P_EXC_RANGE,P_PARTY_CODE,P_TYPE,P_CM_COMP_ID,P_NAME,P_CONTACT,P_VEND_CODE,P_ADD1,P_COUNTRY_CODE,P_CITY_CODE,P_SM_CODE,P_PIN_CODE,P_PHONE,P_MOB,P_CATEGORY,P_A_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_SER_TAX_NO,P_ECC_NO,P_E_CODE,P_EXC_DIV,P_EXC_COLLECTORATE,P_TALLY,P_ACTIVE_IND,P_TDS,P_LBT_IND,P_LBT_NO,P_CREDITDAYS,P_CUST_TYPE,P_ABBREVATION , P_PARTY_NO)VALUES('" + txtExiseRange.Text + "'," + CUST_NO + ",1," + Convert.ToInt32(Session["CompanyId"]) + ",'" + StrPartyName + "','" + StrReplacePerson + "','" + txtVendorCode.Text + "','" + StrReplaceCustAddt + "','" + ddlCountry.SelectedValue.ToString() + "','" + ddlCity.SelectedValue.ToString() + "','" + ddlState.SelectedValue.ToString() + "','" + txtPinCode.Text + "','" + txtPhoneNo.Text + "','" + txtMobileNo.Text + "','" + ddlCategory.SelectedValue.ToString() + "','" + ddlArea.SelectedValue.ToString() + "','" + txtFaxNo.Text + "','" + txtEmailId.Text + "','" + txtPANNO.Text + "','" + txtCSTTINNo.Text + "','" + txtVATTINNo.Text + "','" + txtSerTaxRegNo.Text + "','" + txtECCNo.Text + "','" + ddlExiseType.SelectedValue.ToString() + "','" + txtExiseDevision.Text + "','" + txtExiseCollectorate.Text + "','" + StrReplacetallyname + "','" + ChkActiveInd.Checked + "','" + Convert.ToDouble(txtTDSTAX.Text) + "','" + chlLBTApplicable.Checked + "','" + txtLBTNo.Text + "','" + txtCreditdays.Text.ToString() + "','" + ddlCustomerType.SelectedValue + "','" + txtAbbrevation.Text.Trim().ToUpper() + "', '" + Custmer_No.ToString() + "' )"))
                        {
                            string Code = CommonClasses.GetMaxId("Select Max(P_CODE) from PARTY_MASTER");
                            CommonClasses.WriteLog("Customer Master", "Save", "Customer Master", txtPartyName.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            result = true;
                            Response.Redirect("~/Masters/VIEW/ViewCustomerMaster.aspx", false);
                        }
                        else
                        {
                            ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtPartyName.Focus();
                        }
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PARTY_MASTER WHERE ES_DELETE='FALSE' AND P_CODE != '" + mlCode + "' and P_TYPE=1 AND lower(P_NAME) = lower('" + txtPartyName.Text + "')");
                //result = CommonClasses.CheckExistUpdate("PARTY_MASTER", "P_NAME", txtCustomerName.Text, "P_CODE", mlCode);
                if (dt.Rows.Count == 0)
                {
                    //if (CommonClasses.Execute1("UPDATE PARTY_MASTER SET P_NAME='" + StrReplaceName + "',P_CONTACT='" + StrReplacePerson + "',P_PARTY_CODE='" + Convert.ToInt32(txtCustomerID.Text) + "',P_ADD1='" + StrReplaceCustAddt + "',P_PHONE='" + txtPhoneNo.Text + "',P_MOB='" + txtMobNo.Text + "',P_SCT_CODE='" + ddlSector.SelectedValue + "',P_FAX='" + txtFAXNo.Text + "',P_EMAIL='" + txtEmailID.Text + "',P_PAN='" + txtPANNo.Text + "',P_CST='" + txtCSTTINNo.Text + "',P_VAT='" + txtVATTINNo.Text + "',P_ACTIVE_IND=1,P_TALLY='" + StrReplacetallyname + "',P_CUST_TYPE='0',P_SM_CODE='" + ddlState.SelectedValue + "',P_CITY_CODE='" + ddlCity.SelectedValue + "',P_PIN_CODE='" + txtPinCode.Text + "',P_COORDINATOR='" + StrReplaceCoordinator + "',P_COORDINATOR_EMAIL='" + txtCoordinateEmail.Text + "',P_DELIVERY_ADD='" + StrReplaceDelAdd + "',P_NOTE='" + StrReplaceCustinfo + "' WHERE P_CODE='" + mlCode + "'"))
                    // if (CommonClasses.Execute1("INSERT INTO PARTY_MASTER (P_PARTY_CODE,P_TYPE,P_CM_COMP_ID,P_SCT_CODE,P_NAME,P_CONTACT,P_VEND_CODE,P_ADD1,P_COUNTRY_CODE,P_CITY_CODE,P_SM_CODE,P_PIN_CODE,P_PHONE,P_MOB,P_CATEGORY,P_A_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_SER_TAX_NO,P_ECC_NO,P_E_CODE,P_EXC_DIV,P_EXC_COLLECTORATE,P_TALLY,P_ACTIVE_IND,P_INHOUSE_IND,P_TDS,P_LBT_IND,P_LBT_NO )
                    //VALUES(" + CUST_NO + ",1," + Convert.ToInt32(Session["CompanyId"]) + "," + ddlSector.SelectedValue.ToString() + ",'" + StrPartyName + "','" + StrReplacePerson + "','" + txtVendorCode.Text + "','" + StrReplaceCustAddt + "','" + ddlCountry.SelectedValue.ToString() + "','" + ddlCity.SelectedValue.ToString() + "','" + ddlState.SelectedValue.ToString() + "','" + txtPinCode.Text + "','" + txtPhoneNo.Text + "','" + txtMobileNo.Text + "','" + ddlCategory.SelectedValue.ToString() + "','" + ddlArea.SelectedValue.ToString() + "','" + txtFaxNo.Text + "','" + txtEmailId.Text + "','" + txtPANNO.Text + "','" + txtCSTTINNo.Text + "','" + txtSerTaxRegNo.Text + "','" + txtECCNo.Text + "','" + ddlExiseType.SelectedValue.ToString() + "','" + txtExiseDevision.Text + "','" + txtExiseRange.Text + "','" + txtExiseCollectorate.Text + "','" + StrReplacetallyname + "','" + ChkActiveInd.Checked + "','" + ChkInHoursParty.Checked + "','" + txtTDSTAX.Text + "','" + chlLBTApplicable.Checked + "','" + txtLBTNo.Text + "')"))
                    if (txtAbbrevation.Text.Trim() != "")
                    {
                        DataTable dtAbb = new DataTable();
                        dtAbb = CommonClasses.Execute("Select * FROM PARTY_MASTER WHERE P_CODE != '" + mlCode + "' and P_ABBREVATION= UPPER('" + txtAbbrevation.Text.Trim() + "') and P_TYPE=1 and ES_DELETE='False' and P_CM_COMP_ID='" + Session["CompanyId"] + "'");
                        if (dtAbb.Rows.Count > 0)
                        {
                            ShowMessage("#Avisos", "Abbrevation Already Exists", CommonClasses.MSG_Warning);

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtAbbrevation.Focus();
                        }
                    }
                    else
                    {
                        if (CommonClasses.Execute1("UPDATE PARTY_MASTER SET P_TYPE=1, P_NAME= '" + StrPartyName + "', P_CONTACT='" + StrReplacePerson + "',P_VEND_CODE='" + txtVendorCode.Text + "',P_ADD1='" + StrReplaceCustAddt + "',P_COUNTRY_CODE= '" + ddlCountry.SelectedValue.ToString() + "',P_CITY_CODE= '" + ddlCity.SelectedValue.ToString() + "',P_SM_CODE='" + ddlState.SelectedValue.ToString() + "',P_PIN_CODE='" + txtPinCode.Text + "',P_PHONE='" + txtPhoneNo.Text + "',P_MOB='" + txtMobileNo.Text + "',P_CATEGORY='" + ddlCategory.SelectedValue.ToString() + "',P_A_CODE='" + ddlArea.SelectedValue.ToString() + "',P_FAX='" + txtFaxNo.Text + "',P_EMAIL='" + txtEmailId.Text + "',P_PAN='" + txtPANNO.Text + "',P_CST='" + txtCSTTINNo.Text + "',P_VAT= '" + txtVATTINNo.Text + "',P_SER_TAX_NO='" + txtSerTaxRegNo.Text + "',P_ECC_NO= '" + txtECCNo.Text + "',P_E_CODE='" + ddlExiseType.SelectedValue.ToString() + "',P_EXC_DIV='" + txtExiseDevision.Text + "',P_EXC_COLLECTORATE='" + txtExiseCollectorate.Text + "',P_TALLY='" + StrReplacetallyname + "',P_ACTIVE_IND='" + ChkActiveInd.Checked + "',P_TDS='" + txtTDSTAX.Text + "',P_LBT_IND='" + chlLBTApplicable.Checked + "',P_LBT_NO='" + txtLBTNo.Text + "',P_CUST_TYPE='" + ddlCustomerType.SelectedValue + "',P_ABBREVATION='" + txtAbbrevation.Text.Trim().ToUpper() + "',P_CREDITDAYS=" + txtCreditdays.Text +" WHERE P_CODE='" + mlCode + "' "))
                        {
                            CommonClasses.RemoveModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", mlCode);
                            CommonClasses.WriteLog("Customer Master", "Update", "Customer Master", txtPartyName.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            result = true;
                            Response.Redirect("~/Masters/VIEW/ViewCustomerMaster.aspx", false);
                        }
                        else
                        {
                            ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            txtPartyName.Focus();
                        }
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Customer Master", "ShowMessage", Ex.Message);
            return false;
        }
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

            CommonClasses.SendError("Customer Master", "btnOk_Click", Ex.Message);
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewCustomerMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtPartyName.Text == "")
            {
                flag = false;
            }
            //else if (ddlArea.SelectedIndex == 0)
            //{
            //    flag = false;
            //}
            //else if (ddlSector.SelectedIndex ==0)
            //{
            //    flag = false;
            //}           
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #endregion

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        CValue = Convert.ToInt32(ddlCountry.SelectedValue);
        LoadState();
    }

    protected void chlLBTApplicable_CheckedChanged(object sender, EventArgs e)
    {
        if (chlLBTApplicable.Checked == true)
        {
            //txtLBTNo.Enabled = true;
        }
        else
        {
            //txtLBTNo.Enabled = false;
            txtLBTNo.Text = "";

        }
    }

    #region txtVatTinNo_TextChanged
    protected void txtVatTinNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVATTINNo.Text != "")
            {
                char[] ch = txtVATTINNo.Text.ToCharArray();
                ch[ch.Length - 1] = 'C';
                txtCSTTINNo.Text = new string(ch);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "txtVatTinNo_TextChanged", ex.Message);
        }
    }
    #endregion txtVatTinNo_TextChanged

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

    #region txtTDSTAX_TextChanged
    protected void txtTDSTAX_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtTDSTAX.Text);
            double tdsper = Math.Round(Convert.ToDouble(totalStr), 2);
            txtTDSTAX.Text = string.Format("{0:0.00}", tdsper);
            if (tdsper <= 100)
            {

            }
            else
            {
                txtTDSTAX.Text = "0.00";
                //ShowMessage("#Avisos", "TDS PER IS Less Than 100.", CommonClasses.MSG_Warning);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "txtTDSTAX_TextChanged", ex.Message);
        }
    }
    #endregion
}