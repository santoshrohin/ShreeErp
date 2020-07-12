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

public partial class Admin_Add_TransportMaster : System.Web.UI.Page
{
    #region General Declaration
    TransportMaster_BL BL_TransportMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

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
               DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
              right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_TransportMaster = new TransportMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_TransportMaster = new TransportMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtTransportName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Transport Master", "PageLoad", ex.Message);
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
           //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
           // {
            if (txtTransportName .Text == "")
            {
                ShowMessage("#Avisos", "The Field 'Country Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                return;
            }
                SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Transport Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();

       
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtTransportName.Text.Trim() == "")
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
                CommonClasses.RemoveModifyLock("TRANSAPORT_MASTER", "MODIFY", "T_CODE", mlCode);
            }
            Response.Redirect("~/Admin/View/ViewTransportMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    
    #region Methods


    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_TransportMaster = new TransportMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_TransportMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("TRANSAPORT_MASTER", "MODIFY", "T_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "ViewRec", Ex.Message);
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
                txtTransportName.Text = BL_TransportMaster.T_NAME;
                txtTransportName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtTransportName.Text = BL_TransportMaster.T_NAME;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "GetValues", ex.Message);
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
           
            BL_TransportMaster.T_NAME = txtTransportName.Text.ToUpper();
            BL_TransportMaster.T_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "Setvalues", ex.Message);
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
            string StrReplaceSctorName = txtTransportName.Text;


            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_TransportMaster = new TransportMaster_BL();
                if (Setvalues())
                {
                    if (BL_TransportMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(T_CODE) from TRANSAPORT_MASTER");
                        CommonClasses.WriteLog("Transport Master", "Save", "Transport Master", BL_TransportMaster.T_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewTransportMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_TransportMaster.Msg != "")
                        {
                            //ShowMessage("#Avisos", BL_TransportMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            lblmsg.Text = "Record Already Exists";
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
    
                            BL_TransportMaster.Msg = "";
                        }
                        txtTransportName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_TransportMaster = new TransportMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_TransportMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("TRANSAPORT_MASTER", "MODIFY", "T_CODE", mlCode);
                        CommonClasses.WriteLog("Transport Master", "Update", "Transport Master", BL_TransportMaster.T_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewTransportMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_TransportMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_TransportMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                            BL_TransportMaster.Msg = "";
                        }
                        txtTransportName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Transport Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    #endregion

}
