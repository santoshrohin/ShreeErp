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

public partial class RoportForms_VIEW_ViewCustomerRegister : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='29'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            //{
                LoadCombos();
                ddlCustomerName.Enabled = false;
                chkAll.Checked = true;
                //ddlSector.Enabled = false;
                //chkAllSector.Checked = true;
            //}
            //else
            //{
            //    Response.Write("<script> alert('You Have No Rights To View.');window.location='../SalesDefault.aspx'; </script>");

            //}
        }

    }
    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY P_NAME");

            ddlCustomerName.DataSource = dt;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer Name", "0"));

            //DataTable dt1 = CommonClasses.Execute("select SCT_CODE,SCT_DESC FROM SECTOR_MASTER WHERE SCT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY SCT_DESC");

            //ddlSector.DataSource = dt1;
            //ddlSector.DataTextField = "SCT_DESC";
            //ddlSector.DataValueField = "SCT_CODE";
            //ddlSector.DataBind();
            //ddlSector.Items.Insert(0, new ListItem("Select Sector", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "LoadCombos", Ex.Message);
        }

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAll_CheckedChanged
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAll.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    //#region chkAllSector_CheckedChanged
    //protected void chkAllSector_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkAllSector.Checked == true)
    //    {
    //        ddlSector.SelectedIndex = 0;
    //        ddlSector.Enabled = false;
    //    }
    //    else
    //    {
    //        ddlSector.SelectedIndex = 0;
    //        ddlSector.Enabled = true;
    //        ddlSector.Focus();
    //    }
    //}
    //#endregion

    //#region ddlSector_SelectedIndexChanged
    //protected void ddlSector_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chkAllSector.Checked != true && chkAll.Checked != true)
    //        {
    //            DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 and P_SCT_CODE='" + ddlSector.SelectedValue.ToString() + "' ORDER BY P_NAME");

    //            ddlCustomerName.DataSource = dt;
    //            ddlCustomerName.DataTextField = "P_NAME";
    //            ddlCustomerName.DataValueField = "P_CODE";
    //            ddlCustomerName.DataBind();
    //            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer Name", "0"));
                
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Customer Master Register", "ddlSector_SelectedIndexChanged", Ex.Message);

    //    }
    //}
    //#endregion

    //#region ddlCustomerName_SelectedIndexChanged
    //protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chkAllSector.Checked != true && chkAll.Checked != true)
    //        {
    //            if (ddlCustomerName.SelectedIndex != 0)
    //            {
    //                DataTable dt = CommonClasses.Execute("select P_CODE,P_NAME,P_SCT_CODE FROM PARTY_MASTER WHERE P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 and P_CODE='" + ddlCustomerName.SelectedValue.ToString() + "' ORDER BY P_NAME");

    //                //ddlSector.SelectedValue = dt.Rows[0]["P_SCT_CODE"].ToString();
    //            }
    //            else
    //            {
    //                ShowMessage("#Avisos", "Please Select Customer Name", CommonClasses.MSG_Warning);
    //                return;
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Customer Master Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
    //    }
    //}
    //#endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {

                if (chkAll.Checked == true && ddlCustomerName.SelectedIndex == 0)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "", false);
                }
                if (chkAll.Checked == false && ddlCustomerName.SelectedIndex>0)
                {
                    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() +"", false);

                }
                if (chkAll.Checked == false && ddlCustomerName.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Customer ", CommonClasses.MSG_Warning);
                    return;
                }

                //if (chkAll.Checked == true && chkAllSector.Checked == true)
                //{
                //    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "&SectorAll_code=" + chkAllSector.Checked.ToString() + "", false);
                //}
                //if (chkAll.Checked == false && chkAllSector.Checked == true)
                //{
                //    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "&SectorAll_code=" + chkAllSector.Checked.ToString() + "", false);

                //}
                //if (chkAll.Checked == true && chkAllSector.Checked == false)
                //{
                //    Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlSector.SelectedValue.ToString() + "&SectorAll_code=" + chkAllSector.Checked.ToString() + "", false);

                //}
                //if (chkAll.Checked == false && chkAllSector.Checked == false)
                //{
                //    if (ddlCustomerName.SelectedIndex != 0 && ddlSector.SelectedIndex != 0)
                //    {

                //        Response.Redirect("~/RoportForms/ADD/CustomerRegister.aspx?Title=" + Title + "&All_code=" + chkAll.Checked.ToString() + "&val_code=" + ddlCustomerName.SelectedValue.ToString() + "&SectorAll_code=" + chkAllSector.Checked.ToString() + "", false);
                //    }
                //    else
                //    {
                //        ShowMessage("#Avisos", "Please Select Customer ", CommonClasses.MSG_Warning);
                //        return;
                //    }

                //}
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../VIEW/ViewMasterReport.aspx'; </script>");

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}
