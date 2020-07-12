using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Masters_ADD_ServiceTypeMaster : System.Web.UI.Page
{
    static int mlCode = 0;

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
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='23'");
                //right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();

                try
                {

                    LoadTariffHeading();
                    LoadTaxHeadSales();
                    LoadTaxHeadPurchase();

                    mlCode = 0;
                    ViewState["mlCode"] = null;
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
                    txtServiceCode.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Item Master", "PageLoad", ex.Message);
                }
            }

        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtServiceCode.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Service Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtServiceCode.Focus();
                return;
            }
            if (txtServiceName.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Service Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtServiceCode.Focus();
                return;
            }
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            //{
            SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Item Master", "btnSubmit_Click", Ex.Message);
        }
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;



        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {

                if (checkExist())
                {
                    result = CommonClasses.Execute1("INSERT INTO [dbo].[SERVICE_TYPE_MASTER] ([S_CM_COMP_ID] ,[S_CODENO] ,[S_NAME] ,[S_E_CODE] ,[S_COSTING_HEAD] ,[S_ACCOUNT_SALES] ,[S_ACCOUNT_PURCHASE] ,[S_ACTIVE_IND] ,[ES_DELETE] ,[MODIFY]) VALUES (" + Session["CompanyId"].ToString() + " ,'" + txtServiceCode.Text.Trim().Replace("'", "\''") + "' ,'" + txtServiceName.Text.Trim().Replace("'", "\''") + "' ," + ddlSACCode.SelectedValue + " ,'" + txtCostingHead.Text.Trim().Replace("'", "\''") + "' ," + ddlTallyAccS.SelectedValue + " ," + ddltallyAccP.SelectedValue + ",'" + ChkActiveInd.Checked + "',0 ,0) ");
                    if (result)
                    {
                        Response.Redirect("~/Masters/VIEW/ViewServiceMaster.aspx", false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Already Exist for Service Code or Service Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (checkExist())
                {
                    result = CommonClasses.Execute1("UPDATE [dbo].[SERVICE_TYPE_MASTER] SET [S_CM_COMP_ID] = " + Session["CompanyId"].ToString() + " ,[S_CODENO] = '" + txtServiceCode.Text.Trim().Replace("'", "\''") + "' ,[S_NAME] = '" + txtServiceName.Text.Trim().Replace("'", "\''") + "' ,[S_E_CODE] = " + ddlSACCode.SelectedValue + " ,[S_COSTING_HEAD] = '" + txtCostingHead.Text.Trim().Replace("'", "\''") + "' ,[S_ACCOUNT_SALES] = " + ddlTallyAccS.SelectedValue + " ,[S_ACCOUNT_PURCHASE] = " + ddltallyAccP.SelectedValue + " ,[S_ACTIVE_IND] = '" + ChkActiveInd.Checked + "'  WHERE S_CODE=" + ViewState["mlCode"] + "");
                    if (result)
                    {
                        Response.Redirect("~/Masters/VIEW/ViewServiceMaster.aspx", false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Already Exist for Service Code or Service Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Service Type Master", "SaveRec", ex.Message);
        }
        return result;
    }

    private bool checkExist()
    {
        bool result = false;
        string Query = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            Query = "SELECT * FROM SERVICE_TYPE_MASTER where S_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' and (S_CODENO='" + txtServiceCode.Text.Trim() + "' or S_NAME='" + txtServiceName.Text.Trim() + "') and ES_DELETE=0";
        }
        else if (Request.QueryString[0].Equals("MODIFY"))
        {
            Query = "SELECT * FROM SERVICE_TYPE_MASTER where S_CODE!=" + ViewState["mlCode"] + " and S_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' and (S_CODENO='" + txtServiceCode.Text.Trim() + "' or S_NAME='" + txtServiceName.Text.Trim() + "') and ES_DELETE=0";
        }
        DataTable dtExist = CommonClasses.Execute(Query);
        if (dtExist.Rows.Count > 0)
        {
            result = false;
        }
        else
        {
            result = true;
        }
        return result;
    }
    #endregion
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
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
            if (txtServiceCode.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtServiceName.Text.Trim() == "")
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
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewServiceMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "btnCancel_Click", Ex.Message);
        }
    }
    private void LoadTaxHeadSales()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            //dt = CommonClasses.Execute("select SCT_DESC,SCT_CODE from SECTOR_MASTER where SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCT_DESC");
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  order by TALLY_NAME");


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
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 order by TALLY_NAME");


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

    private void ViewRec(string p)
    {
        DataTable dt = CommonClasses.Execute("SELECT S_CODE, S_CM_COMP_ID, S_CODENO, S_NAME, S_E_CODE, S_COSTING_HEAD, S_ACCOUNT_SALES, S_ACCOUNT_PURCHASE, S_ACTIVE_IND, ES_DELETE, MODIFY FROM SERVICE_TYPE_MASTER WHERE (ES_DELETE = 0) and S_CODE=" + ViewState["mlCode"]);
        if (dt.Rows.Count > 0)
        {
            txtServiceCode.Text = dt.Rows[0]["S_CODENO"].ToString();
            txtServiceName.Text = dt.Rows[0]["S_NAME"].ToString();
            ddlSACCode.SelectedValue = dt.Rows[0]["S_E_CODE"].ToString();
            txtCostingHead.Text = dt.Rows[0]["S_COSTING_HEAD"].ToString();
            ddltallyAccP.SelectedValue = dt.Rows[0]["S_ACCOUNT_PURCHASE"].ToString();
            ddlTallyAccS.SelectedValue = dt.Rows[0]["S_ACCOUNT_SALES"].ToString();
            ChkActiveInd.Checked = Convert.ToBoolean(dt.Rows[0]["S_ACTIVE_IND"].ToString());
        }
        if (p == "VIEW")
        {
            txtServiceCode.Enabled = false;
            txtServiceName.Enabled = false;
            ddlSACCode.Enabled = false;
            txtCostingHead.Enabled = false;
            ddltallyAccP.Enabled = false;
            ddlTallyAccS.Enabled = false;
            ChkActiveInd.Enabled = false;
            btnSubmit.Visible = false;
        }
        if (p == "MODIFY")
        {
            //CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", mlCode);
        }
    }
    #region LoadTariffHeading
    private void LoadTariffHeading()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and E_TALLY_GST_EXCISE='1' and ES_DELETE=0 ORDER BY E_CODE");

            ddlSACCode.DataSource = dt;
            ddlSACCode.DataTextField = "E_TARIFF_NO";
            ddlSACCode.DataValueField = "E_CODE";
            ddlSACCode.DataBind();
            ddlSACCode.Items.Insert(0, new ListItem("Select", "0"));
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
}
