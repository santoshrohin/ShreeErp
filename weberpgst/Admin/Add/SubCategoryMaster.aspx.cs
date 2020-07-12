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

public partial class Admin_Add_SubCategoryMaster : System.Web.UI.Page
{
    # region Variables
    //UserMaster_BL BL_UserMaster = null;
    static int mlCode = 0;
    static string right = "";
    # endregion

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
                        LoadCategory();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("SubCategory Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
    }
    #endregion Page_Load

    #region LoadCategory
    private void LoadCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("SELECT I_CAT_CODE,I_CAT_NAME FROM ITEM_CATEGORY_MASTER WHERE I_CAT_CM_COMP_ID ='" + (string)Session["CompanyId"] + "' AND ES_DELETE=0 ORDER BY I_CAT_NAME");
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "I_CAT_NAME";
            ddlCategory.DataValueField = "I_CAT_CODE";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
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
    #endregion LoadCategory

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            // BL_UserMaster = new UserMaster_BL(mlCode);
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM ITEM_SUBCATEGORY_MASTER WHERE SCAT_CODE=" + mlCode + " AND SCAT_CM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                txtSubCategoryName.Text = dt.Rows[0]["SCAT_DESC"].ToString();
                ddlCategory.SelectedValue = Convert.ToInt32(dt.Rows[0]["SCAT_CAT_CODE"]).ToString();
                
                if (str == "VIEW")
                {
                    txtSubCategoryName.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SCAT_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SubCategory Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtSubCategoryName.Text == "")
        {
            ShowMessage("#Avisos", "The Field 'SubCategory Name' is Required", CommonClasses.MSG_Warning);
            return;
            //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

        }
        SaveRec();
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select SCAT_CODE,SCAT_DESC FROM ITEM_SUBCATEGORY_MASTER WHERE LOWER(SCAT_DESC)= LOWER('" + txtSubCategoryName.Text.Trim().Replace("'","''") + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO ITEM_SUBCATEGORY_MASTER (SCAT_CM_COMP_ID,SCAT_DESC,SCAT_CAT_CODE) VALUES ('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtSubCategoryName.Text.Trim().Replace("'", "''") + "','" + ddlCategory.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("SELECT MAX(SCAT_CODE) FROM ITEM_SUBCATEGORY_MASTER");
                        CommonClasses.WriteLog("SubCategory Master", "Save", "SubCategory Master", txtSubCategoryName.Text.Trim().Replace("'", "''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewSubCategoryMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtSubCategoryName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "SubCategory Name Already Exists", CommonClasses.MSG_Warning);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM ITEM_SUBCATEGORY_MASTER WHERE ES_DELETE=0 AND SCAT_CODE!= '" + mlCode + "' AND LOWER(SCAT_DESC) = LOWER('" + txtSubCategoryName.Text.Trim().Replace("'", "''") + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE ITEM_SUBCATEGORY_MASTER SET SCAT_DESC ='" + txtSubCategoryName.Text.Trim().Replace("'", "''") + "', SCAT_CAT_CODE ='" + ddlCategory.SelectedValue + "' WHERE SCAT_CODE ='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SCAT_CODE", mlCode);
                        CommonClasses.WriteLog("SubCategory Master", "Update", "SubCategory Master", txtSubCategoryName.Text.Trim().Replace("'", "''"), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewSubCategoryMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        txtSubCategoryName.Focus();
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "SubCategory Name Already Exists", CommonClasses.MSG_Warning);
                    txtSubCategoryName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SubCategory Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
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
        catch (Exception ex)
        {
            CommonClasses.SendError("SubCategory Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion btnCancel_Click

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SCAT_CODE", mlCode);
            }
            Response.Redirect("~/Admin/VIEW/ViewSubCategoryMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SubCategory Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion CancelRecord

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtSubCategoryName.Text.Trim() == "")
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
            CommonClasses.SendError("SubCategory Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion CheckValid

    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click
}
