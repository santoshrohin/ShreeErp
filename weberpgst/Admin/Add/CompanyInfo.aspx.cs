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
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class Admin_Add_CompanyInfo : System.Web.UI.Page
{
    CompanyMaster_BL CompanyMaster_BL = null;
    static int mlCode = 0;
    static string right = "";
    DateTime d = new DateTime();

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
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='1'");
                        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                        {


                            LoadCountry();
                            CompanyMaster_BL = new CompanyMaster_BL();
                            mlCode = Convert.ToInt32(Session["CompanyId"]);
                            ViewRec("MOD");

                        }
                        else
                        {
                            Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");

                        }

                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Company Master", "PageLoad", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "PageLoad", ex.Message);
        }
    }
    #endregion Page_Load

    #region ViewRec
    private void ViewRec(string str)
    {

        try
        {
            CompanyMaster_BL = new CompanyMaster_BL(mlCode);
            DataTable dt = new DataTable();
            CompanyMaster_BL.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Company Master", "ViewRec", Ex.Message);
        }



        //try
        //{
        //    CompanyMaster_BL = new CompanyMaster_BL(mlCode);
        //    DataTable dt = new DataTable();
        //    CompanyMaster_BL.GetInfo();
        //    //DataTable DtCompInfo = CommonClasses.Execute("SELECT CM_CODE,CM_ID, CM_NAME,CM_ADDRESS1, CM_ADDRESS2, CM_ADDRESS3, CM_CITY, CM_STATE, CM_COUNTRY, CM_OWNER, CM_PHONENO1, CM_PHONENO2, CM_PHONENO3, CM_FAXNO, CM_EMAILID, CM_WEBSITE, CM_SURCHARGE_NO, CM_VAT_TIN_NO, CM_CST_NO, CM_SERVICE_TAX_NO, CM_PAN_NO, CM_COMMODITY_NO, CM_ACTIVE_IND, CM_OPENING_DATE, CM_CLOSING_DATE FROM COMPANY_MASTER  WHERE CM_DELETE_FLAG='false' and  CM_ID = " + mlCode + "");
        //    if (DtCompInfo.Rows.Count > 0)
        //    {
        //        mlCode = Convert.ToInt32(DtCompInfo.Rows[0]["CM_CODE"]); ;
        //        txtCompanyName.Text = DtCompInfo.Rows[0]["CM_NAME"].ToString();
        //        txtAddressLine1.Text = DtCompInfo.Rows[0]["CM_ADDRESS1"].ToString();
        //        txtAddressLine2.Text = DtCompInfo.Rows[0]["CM_ADDRESS2"].ToString();
        //        //CompanyMaster_BL.CM_ADDRESS3;
        //        txtAuthSign.Text = DtCompInfo.Rows[0]["CM_OWNER"].ToString();
        //        txtPhoneNo1.Text = DtCompInfo.Rows[0]["CM_PHONENO1"].ToString();
        //        txtPhoneNumber2.Text = DtCompInfo.Rows[0]["CM_PHONENO2"].ToString();
        //        txtPhoneNumber3.Text = DtCompInfo.Rows[0]["CM_PHONENO3"].ToString();
        //        txtFaxNo.Text = DtCompInfo.Rows[0]["CM_FAXNO"].ToString();
        //        txtEmailId.Text = DtCompInfo.Rows[0]["CM_EMAILID"].ToString();
        //        txtWebsite.Text = DtCompInfo.Rows[0]["CM_WEBSITE"].ToString();
        //        // CompanyMaster_BL.CM_SURCHARGE_NO;
        //        txtVatTinNo.Text = DtCompInfo.Rows[0]["CM_VAT_TIN_NO"].ToString();
        //        txtCstNo.Text = DtCompInfo.Rows[0]["CM_CST_NO"].ToString();
        //        txtServiceTax.Text = DtCompInfo.Rows[0]["CM_SERVICE_TAX_NO"].ToString();
        //        txtPanNo.Text = DtCompInfo.Rows[0]["CM_PAN_NO"].ToString();
        //        //  CompanyMaster_BL.CM_COMMODITY_NO;
        //        if (DtCompInfo.Rows[0]["CM_ACTIVE_IND"].ToString() == "True")
        //        {
        //            cbActiveIndex.Checked = true;

        //        }
        //        else
        //        {
        //            cbActiveIndex.Checked = false;
        //        }
        //        txtOpeningDate.Text = Convert.ToDateTime(DtCompInfo.Rows[0]["CM_OPENING_DATE"]).ToString("dd/MMM/yyyy");
        //        txtClosingDate.Text = Convert.ToDateTime(DtCompInfo.Rows[0]["CM_CLOSING_DATE"]).ToString("dd/MMM/yyyy");


        //        int Country = Convert.ToInt32(DtCompInfo.Rows[0]["CM_COUNTRY"]);
        //        int state =Convert.ToInt32( DtCompInfo.Rows[0]["CM_STATE"]);
        //        int City = Convert.ToInt32(DtCompInfo.Rows[0]["CM_CITY"]);
        //        LoadCountry();
        //        ddlCountry.SelectedValue = Country.ToString();

        //        LoadState();
        //        ddlState.SelectedValue = state.ToString();

        //        LoadCity();
        //        ddlCity.SelectedValue = City.ToString();

        //        if (str == "VIEW")
        //        {
        //            txtCompanyName.Enabled = false;
        //            txtAddressLine1.Enabled = false;
        //            txtAddressLine2.Enabled = false;
        //            //CompanyMaster_BL.CM_ADDRESS3;
        //            //ddlCity.Text.Enabled = false;
        //            //ddlState.Text.Enabled = false;
        //            ddlCountry.Enabled = false;
        //            txtAuthSign.Enabled = false;
        //            txtPhoneNo1.Enabled = false;
        //            txtPhoneNumber2.Enabled = false;
        //            txtPhoneNumber3.Enabled = false;
        //            txtFaxNo.Enabled = false;
        //            txtEmailId.Enabled = false;
        //            txtWebsite.Enabled = false;

        //            txtVatTinNo.Enabled = false;
        //            txtCstNo.Enabled = false;
        //            txtServiceTax.Enabled = false;
        //            txtPanNo.Enabled = false;

        //            cbActiveIndex.Enabled = false;
        //            txtOpeningDate.Enabled = false;
        //            txtClosingDate.Enabled = false;
        //            btnSubmit.Visible = false;

        //        }

        //    }

        //}
        //catch (Exception ex)
        //{
        //    CommonClasses.SendError("Company Master", "ViewRec", ex.Message);
        //}
    }
    #endregion ViewRec

    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();

        try
        {
            CompanyMaster_BL = new CompanyMaster_BL();
            dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + Session["CompanyId"] + "");
            ddlCountry.DataSource = dt;
            ddlCountry.DataTextField = "COUNTRY_NAME";
            ddlCountry.DataValueField = "COUNTRY_CODE";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("------Country-------", "0"));

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadCountry", ex.Message);
        }
        finally
        {
            dt.Dispose();
        }

    }
    #endregion LoadCountry

    #region Load State
    private void LoadState()
    {

        DataTable dt = new DataTable();
        try
        {
            try
            {
                CompanyMaster_BL = new CompanyMaster_BL();
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + " and SM_COUNTRY_CODE=" + ddlCountry.SelectedValue + "");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("------State-------", "0"));

                ddlState.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Company Master", "LoadState", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadState", ex.Message);
        }

    }
    #endregion Load State

    #region LoadCity
    private void LoadCity()
    {
        DataTable dt = new DataTable();
        try
        {
            CompanyMaster_BL = new CompanyMaster_BL();
            dt = CommonClasses.Execute("select CITY_CODE,CITY_NAME from CITY_MASTER where ES_DELETE=0 and CITY_CM_COMP_ID=" + Session["CompanyId"] + " and CITY_COUNTRY_CODE=" + ddlCountry.SelectedValue + " and CITY_SM_CODE=" + ddlState.SelectedValue + "");
            ddlCity.DataSource = dt;
            ddlCity.DataTextField = "CITY_NAME";
            ddlCity.DataValueField = "CITY_CODE";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("------City-------", "0"));
            ddlCity.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadCity", ex.Message);
        }
        finally
        {
            dt.Dispose();
        }


    }
    #endregion LoadCity

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
        //{
        SaveRec();
        //}
    }
    #endregion btnSubmit_Click

    protected void btnOk_Click(object sender, EventArgs e)
    {
        // SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            //if (cbActiveIndex.Checked == false)
            //{
            //    ShowMessage("#Avisos", "Please Check Active Checkbox", CommonClasses.MSG_Warning);
            //    return false;
            //}

            //else
            //{
            //    DataTable dtComp = CommonClasses.Execute("Update COMPANY_MASTER Set CM_NAME = '" + txtCompanyName.Text + "',CM_ADDRESS1 = '" + txtAddressLine1.Text + "',	CM_ADDRESS2 = '" + txtAddressLine2.Text + "',	CM_ADDRESS3 = '',	CM_CITY = '" + Convert.ToInt32(ddlCity.SelectedValue) + "',	CM_STATE = '" + Convert.ToInt32(ddlState.SelectedValue) + "',	CM_COUNTRY = '" + Convert.ToInt32(ddlCountry.SelectedValue) + "',	CM_OWNER = '"+txtAuthSign.Text+"',	CM_PHONENO1 = '" + txtPhoneNo1.Text + "',	CM_PHONENO2 = '" + txtPhoneNumber2.Text + "',CM_PHONENO3 = '" + txtPhoneNumber3.Text + "',	CM_FAXNO = '" + txtFaxNo.Text + "',	CM_EMAILID = '" + txtEmailId.Text + "',	CM_WEBSITE = '" + txtWebsite.Text + "',	CM_SURCHARGE_NO = '',	CM_VAT_TIN_NO = '" + txtVatTinNo.Text + "',	CM_CST_NO = '" + txtCstNo.Text + "',	CM_SERVICE_TAX_NO = '" + txtServiceTax.Text + "',	CM_PAN_NO = '" + txtPanNo.Text + "',CM_COMMODITY_NO = '',	CM_ACTIVE_IND = '" + cbActiveIndex.Checked + "',	CM_OPENING_DATE = '" + Convert.ToDateTime(txtOpeningDate.Text).ToString("yyyy/MM/dd") + "',	CM_CLOSING_DATE = '" + Convert.ToDateTime(txtClosingDate.Text).ToString("yyyy/MM/dd") + "'	Where CM_ID = " + Convert.ToInt32(Session["CompanyId"]) + "");

            //    if(dtComp.Rows.Count==0)
            //    {
            //        CommonClasses.RemoveModifyLock("COMPANY_MASTER", "CM_MODIFY_FLAG", "CM_ID", mlCode);
            //        CommonClasses.WriteLog("Company Information", "Update", "Company Information", txtCompanyName.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
            //        result = true;
            //       // ShowMessage("#Avisos", "Record Updated Successfully", CommonClasses.MSG_Ok);
            //        Response.Redirect("~/Admin/Default.aspx", false);
            //    }
            //    else
            //    {
            //        ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
            //    }

            //}


            //lblmsg.Visible = false;
            CompanyMaster_BL = new CompanyMaster_BL(mlCode);
            if (Setvalues())
            {
                if (cbActiveIndex.Checked == false)
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please Check Active Checkbox";
                    ShowMessage("#Avisos", "Please Check Active Checkbox", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return false;
                }


                if (CompanyMaster_BL.Update())
                {
                    CommonClasses.RemoveModifyLock("COMPANY_MASTER", "CM_MODIFY_FLAG", "CM_ID", mlCode);
                    CommonClasses.WriteLog("Company Information", "Update", "Company Information", CompanyMaster_BL.CM_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    //ShowMessage("#Avisos", CompanyMaster_BL.Msg.ToString(), CommonClasses.MSG_Ok);
                    Response.Redirect("~/Admin/Default.aspx", false);
                }
                else
                {
                    if (CompanyMaster_BL.Msg != "")
                    {
                        ShowMessage("#Avisos", CompanyMaster_BL.Msg, CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //lblmsg.Visible = false;
                        //lblmsg.Text = CompanyMaster_BL.Msg;
                        CompanyMaster_BL.Msg = "";
                    }

                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        d = (Convert.ToDateTime("01/Jan/1930"));
        bool res = false;
        try
        {

            //CompanyMaster_BL.CM_CODE;
            CompanyMaster_BL.CM_ID = Convert.ToInt32(Session["CompanyId"]);
            CompanyMaster_BL.CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            CompanyMaster_BL.CM_NAME = Regex.Replace(txtCompanyName.Text.ToUpper(), "'", "`");
            CompanyMaster_BL.CM_ADDRESS1 = Regex.Replace(txtAddressLine1.Text, "'", "`");
            CompanyMaster_BL.CM_ADDRESS2 = Regex.Replace(txtAddressLine2.Text, "'", "`");
            CompanyMaster_BL.CM_ADDRESS3 = "";
            CompanyMaster_BL.CM_CITY = Convert.ToInt32(ddlCity.SelectedValue.ToString());
            CompanyMaster_BL.CM_STATE = Convert.ToInt32(ddlState.SelectedValue.ToString());
            CompanyMaster_BL.CM_COUNTRY = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
            CompanyMaster_BL.CM_OWNER = Regex.Replace(txtAuthSign.Text, "'", "`");
            CompanyMaster_BL.CM_PHONENO1 = Regex.Replace(txtPhoneNo1.Text, "'", "`");
            CompanyMaster_BL.CM_PHONENO2 = Regex.Replace(txtPhoneNumber2.Text, "'", "`");
            CompanyMaster_BL.CM_PHONENO3 = Regex.Replace(txtPhoneNumber3.Text, "'", "`");
            CompanyMaster_BL.CM_FAXNO = Regex.Replace(txtFaxNo.Text, "'", "`");
            CompanyMaster_BL.CM_EMAILID = Regex.Replace(txtEmailId.Text, "'", "`");
            CompanyMaster_BL.CM_WEBSITE = Regex.Replace(txtWebsite.Text, "'", "`");
            CompanyMaster_BL.CM_SURCHARGE_NO = "";
            CompanyMaster_BL.CM_VAT_TIN_NO = Regex.Replace(txtVatTinNo.Text, "'", "`");
            CompanyMaster_BL.CM_CST_NO = Regex.Replace(txtCstNo.Text, "'", "`");
            CompanyMaster_BL.CM_SERVICE_TAX_NO = Regex.Replace(txtServiceTax.Text, "'", "`");
            CompanyMaster_BL.CM_PAN_NO = Regex.Replace(txtPanNo.Text, "'", "`");
            CompanyMaster_BL.CM_COMMODITY_NO = "";
            CompanyMaster_BL.CM_ACTIVE_IND = cbActiveIndex.Checked;

            CompanyMaster_BL.CM_OPENING_DATE = Convert.ToDateTime(txtOpeningDate.Text);
            CompanyMaster_BL.CM_CLOSING_DATE = Convert.ToDateTime(txtClosingDate.Text);
            CompanyMaster_BL.CM_REGD_NO = txtReghNo.Text;
            CompanyMaster_BL.CM_ECC_NO = txtEccNo.Text;

            CompanyMaster_BL.CM_CIN_NO = txtCinNo.Text;

            if (txtVatWef.Text == "")
            {
                CompanyMaster_BL.CM_VAT_WEF = d;
            }
            else
            {
                CompanyMaster_BL.CM_VAT_WEF = Convert.ToDateTime(txtVatWef.Text);
            }
            if (txtCstWef.Text == "")
            {
                CompanyMaster_BL.CM_CST_WEF = d;
            }
            else
            {
                CompanyMaster_BL.CM_CST_WEF = Convert.ToDateTime(txtCstWef.Text);
            }

            CompanyMaster_BL.CM_ISO_NUMBER = txtISONumber.Text;
            CompanyMaster_BL.CM_EXP_LICEN_NO = txtExpLicenNo.Text;
            CompanyMaster_BL.CM_EXP_PERMISSOIN_NO = txtExpPermisiionNo.Text;

            CompanyMaster_BL.CM_EXCISE_RANGE = txtExciseRange.Text;
            CompanyMaster_BL.CM_EXCISE_DIVISION = txtExciseDevision.Text;
            CompanyMaster_BL.CM_COMMISONERATE = txtCommisionerate.Text;
            CompanyMaster_BL.CM_EXC_SUPRE_DETAILS = txtExcSupreDetail.Text;

            CompanyMaster_BL.CM_BANK_NAME = txtBankersName.Text;
            CompanyMaster_BL.CM_BANK_ADDRESS = txtBranchAddress.Text;
            CompanyMaster_BL.CM_BANK_ACC_NO = txtAccountNo.Text;
            CompanyMaster_BL.CM_ACC_TYPE = txtTypeofAccount.Text;
            CompanyMaster_BL.CM_B_SWIFT_CODE = txtSwiftCode.Text;
            CompanyMaster_BL.CM_IFSC_CODE = txtIFSCCode.Text;
            CompanyMaster_BL.CM_COMM_CUSTOM = txtCommCustom.Text;
            CompanyMaster_BL.CM_AUT_SPEC_SIGN = txtSpecimenSign.Text;
            CompanyMaster_BL.CM_GST_NO = txtGSTNo.Text;

            res = true;

            if (CompanyMaster_BL.CM_CLOSING_DATE <= CompanyMaster_BL.CM_OPENING_DATE)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Correct Cloasing Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                // ShowMessage("#Avisos", "Please Enter Correct Cloasing Name", CommonClasses.MSG_Warning);
                res = false;
                return res;

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "SetValues", ex.Message);
        }
        return res;
    }
    #endregion Setvalues

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtCompanyName.Text = CompanyMaster_BL.CM_NAME;
            txtAddressLine1.Text = CompanyMaster_BL.CM_ADDRESS1;
            txtAddressLine2.Text = CompanyMaster_BL.CM_ADDRESS2;
            //CompanyMaster_BL.CM_ADDRESS3;
            txtAuthSign.Text = CompanyMaster_BL.CM_OWNER;
            txtPhoneNo1.Text = CompanyMaster_BL.CM_PHONENO1;
            txtPhoneNumber2.Text = CompanyMaster_BL.CM_PHONENO2;
            txtPhoneNumber3.Text = CompanyMaster_BL.CM_PHONENO3;
            txtFaxNo.Text = CompanyMaster_BL.CM_FAXNO;
            txtEmailId.Text = CompanyMaster_BL.CM_EMAILID;
            txtWebsite.Text = CompanyMaster_BL.CM_WEBSITE;
            // CompanyMaster_BL.CM_SURCHARGE_NO;
            txtVatTinNo.Text = CompanyMaster_BL.CM_VAT_TIN_NO;
            txtCstNo.Text = CompanyMaster_BL.CM_CST_NO;
            txtServiceTax.Text = CompanyMaster_BL.CM_SERVICE_TAX_NO;
            txtPanNo.Text = CompanyMaster_BL.CM_PAN_NO;
            //  CompanyMaster_BL.CM_COMMODITY_NO;
            cbActiveIndex.Checked = CompanyMaster_BL.CM_ACTIVE_IND;
            txtOpeningDate.Text = CompanyMaster_BL.CM_OPENING_DATE.ToString("dd/MMM/yyyy");
            txtClosingDate.Text = CompanyMaster_BL.CM_CLOSING_DATE.ToString("dd/MMM/yyyy");

            txtReghNo.Text = CompanyMaster_BL.CM_REGD_NO;
            txtEccNo.Text = CompanyMaster_BL.CM_ECC_NO;
            if (CompanyMaster_BL.CM_VAT_WEF.ToString() == "01-01-1930 00:00:00")
            {
                txtVatWef.Text = "";
            }
            else
            {
                txtVatWef.Text = CompanyMaster_BL.CM_VAT_WEF.ToString("dd/MMM/yyyy");
            }
            if (CompanyMaster_BL.CM_CST_WEF.ToString() == Convert.ToDateTime("01-01-1930 00:00:00").ToString())
            {
                txtCstWef.Text = "";
            }
            else
            {
                txtCstWef.Text = CompanyMaster_BL.CM_CST_WEF.ToString("dd/MMM/yyyy");
            }


            txtISONumber.Text = CompanyMaster_BL.CM_ISO_NUMBER;

            txtExpLicenNo.Text = CompanyMaster_BL.CM_EXP_LICEN_NO;
            txtExpPermisiionNo.Text = CompanyMaster_BL.CM_EXP_PERMISSOIN_NO;


            txtExciseRange.Text = CompanyMaster_BL.CM_EXCISE_RANGE;
            txtExciseDevision.Text = CompanyMaster_BL.CM_EXCISE_DIVISION;
            txtCommisionerate.Text = CompanyMaster_BL.CM_COMMISONERATE;
            txtExcSupreDetail.Text = CompanyMaster_BL.CM_EXC_SUPRE_DETAILS;

            txtBankersName.Text = CompanyMaster_BL.CM_BANK_NAME;
            txtBranchAddress.Text = CompanyMaster_BL.CM_BANK_ADDRESS;
            txtAccountNo.Text = CompanyMaster_BL.CM_BANK_ACC_NO;
            txtTypeofAccount.Text = CompanyMaster_BL.CM_ACC_TYPE;
            txtSwiftCode.Text = CompanyMaster_BL.CM_B_SWIFT_CODE;
            txtIFSCCode.Text = CompanyMaster_BL.CM_IFSC_CODE;
            txtCinNo.Text = CompanyMaster_BL.CM_CIN_NO;
            txtCommCustom.Text = CompanyMaster_BL.CM_COMM_CUSTOM;
            txtSpecimenSign.Text = CompanyMaster_BL.CM_AUT_SPEC_SIGN;
            txtGSTNo.Text = CompanyMaster_BL.CM_GST_NO;

            int Country = CompanyMaster_BL.CM_COUNTRY;
            int state = CompanyMaster_BL.CM_STATE;
            int City = CompanyMaster_BL.CM_CITY;
            LoadCountry();
            ddlCountry.SelectedValue = Country.ToString();

            LoadState();
            ddlState.SelectedValue = state.ToString();

            LoadCity();
            ddlCity.SelectedValue = City.ToString();

            if (str == "VIEW")
            {
                txtCompanyName.Enabled = false;
                txtAddressLine1.Enabled = false;
                txtAddressLine2.Enabled = false;
                //CompanyMaster_BL.CM_ADDRESS3;
                //ddlCity.Text.Enabled = false;
                //ddlState.Text.Enabled = false;
                ddlCountry.Enabled = false;
                txtAuthSign.Enabled = false;
                txtPhoneNo1.Enabled = false;
                txtPhoneNumber2.Enabled = false;
                txtPhoneNumber3.Enabled = false;
                txtFaxNo.Enabled = false;
                txtEmailId.Enabled = false;
                txtWebsite.Enabled = false;

                txtVatTinNo.Enabled = false;
                txtCstNo.Enabled = false;
                txtServiceTax.Enabled = false;
                txtPanNo.Enabled = false;

                txtBankersName.Enabled = false;
                txtBranchAddress.Enabled = false;
                txtAccountNo.Enabled = false;
                txtTypeofAccount.Enabled = false;
                txtSwiftCode.Enabled = false;
                txtIFSCCode.Enabled = false;
                txtGSTNo.Enabled = false;

                cbActiveIndex.Enabled = false;
                txtOpeningDate.Enabled = false;
                txtClosingDate.Enabled = false;
                txtCommCustom.Enabled = false;
                txtSpecimenSign.Enabled = false;
                btnSubmit.Visible = false;

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion GetValues

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Request.QueryString[0].Equals("VIEW"))
            //{
            //    CancelRecord();
            //}
            //else
            //{
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
            //}
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }


    }
    #endregion btnCancel_Click

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCompanyName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtAddressLine1.Text.Trim() == "")
            {
                flag = false;
            }

            else if (txtOpeningDate.Text.Trim() == "")
            {
                flag = false;
            }

            else if (txtClosingDate.Text.Trim() == "")
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
            CommonClasses.SendError("Currency Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    private void CancelRecord()
    {

        try
        {
            CommonClasses.RemoveModifyLock("CM_COMPANY_MASTER", "CM_MODIFY_FLAG", "CM_ID", mlCode);
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "btnCancel_Click", ex.Message);
        }
    }

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region ddlCountry_SelectedIndexChanged
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadState();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "ddlCountry_SelectedIndexChanged", ex.Message);
        }

    }
    #endregion ddlCountry_SelectedIndexChanged

    #region ddlState_SelectedIndexChanged
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCity();
    }
    #endregion ddlState_SelectedIndexChanged

    #region Imgclose_Click
    protected void Imgclose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            CommonClasses.RemoveModifyLock("CM_COMPANY_MASTER", "CM_MODIFY_FLAG", "CM_ID", mlCode);
            Response.Redirect("~/Masters/ADD/BranchSelection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "Imgclose_Click", ex.Message);
        }
    }
    #endregion Imgclose_Click

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
            CommonClasses.SendError("Department Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void txtOpeningDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOpeningDate.Text != "")
            {
                if (Convert.ToDateTime(txtOpeningDate.Text).Month <= 3)
                {
                    txtClosingDate.Text = "31/Mar/" + Convert.ToDateTime(txtOpeningDate.Text).Year;
                }
                else
                {
                    txtClosingDate.Text = "31/Mar/" + (Convert.ToDateTime(txtOpeningDate.Text).Year + 1);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "txtOpeningDate_TextChanged", ex.Message);
        }
    }

    protected void txtVatTinNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVatTinNo.Text != "")
            {
                char[] ch = txtVatTinNo.Text.ToCharArray();
                ch[ch.Length - 1] = 'C';
                txtCstNo.Text = new string(ch);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "txtVatTinNo_TextChanged", ex.Message);
        }
    }
}
