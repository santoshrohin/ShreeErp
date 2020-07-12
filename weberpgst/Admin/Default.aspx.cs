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

public partial class Admin_Default : System.Web.UI.Page
{
    string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='1'");
        //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }

    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE="+sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    protected void btnCompany_click(object sender, EventArgs e)
    {
        checkRights(1);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {            
            Response.Redirect("~/Admin/Add/CompanyInfo.aspx", false);
        }
        else
        {            
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.TargetControlID = "comp1";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
           
            return;
        }
    }
    protected void btnSysConfig_click(object sender, EventArgs e)
    {
        //checkRights(1);
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        //{
        Response.Redirect("~/Admin/Add/SystemConfigration.aspx", false);
        //}
        // else
        //{
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            //ModalPopupMsg.TargetControlID = "comp1";
            //popUpPanel5.Visible = true;
            //ModalPopupMsg.Show();

            return;
        //}
    }


    protected void btnUserMaster_click(object sender, EventArgs e)
    {
        checkRights(2);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {            
            Response.Redirect("~/Admin/View/ViewUsers.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp2";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }
    protected void btnTallyMaster_click(object sender, EventArgs e)
    {
        checkRights(32);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewTallyMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp2";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }
    protected void btnUnlockRecords_click(object sender, EventArgs e)
    {
        checkRights(4);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {           
            Response.Redirect("~/Admin/Add/RemoveModifyLock.aspx", false);
        }
        else
        {           
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.TargetControlID = "comp3";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
        }
    }

   

    protected void btnCountryMaster_click(object sender, EventArgs e)
    {
        checkRights(5);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCountryMaster.aspx", false);
        }
        else
        {
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.TargetControlID = "comp4";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
        }
    }
    protected void btnStateMaster_click(object sender, EventArgs e)
    {
        checkRights(6);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");

            ModalPopupMsg.TargetControlID = "comp5";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
        }
    }
    protected void btnCityMaster_click(object sender, EventArgs e)
    {
        checkRights(7);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCityMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.TargetControlID = "comp6";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
        }
    }
    protected void btnCurrencyMaster_click(object sender, EventArgs e)
    {
        checkRights(8);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewCurrancyMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp7";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }
    protected void btnUserRights_click(object sender, EventArgs e)
    {
        checkRights(3);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/Add/UserRights.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp8";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }

    protected void btnUserMasterReport_click(object sender, EventArgs e)
    {
        checkRights(3);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/MasterRegister/VIEW/ViewUserMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp9";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }

    protected void btnLogMaster_click(object sender, EventArgs e)
    {
        checkRights(36);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewLogMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp11";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }

    protected void btnISOMaster_click(object sender, EventArgs e)
    {
        checkRights(80);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/VIEW/IsoNoMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.TargetControlID = "comp12";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }

    protected void btnDatabaseBackup_click(object sender, EventArgs e)
    {
        checkRights(65);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewDatabaseBackup.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.TargetControlID = "comp12";
            popUpPanel5.Visible = true;
            ModalPopupMsg.Show();
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception Ex)
        {


            CommonClasses.SendError("Admin Default", "btnOk_Click", Ex.Message);
        }
    }

    protected void btnUnitMaster_click(object sender, EventArgs e)
    {
        checkRights(10);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTransportMaster_click(object sender, EventArgs e)
    {
        checkRights(132);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/VIEW/ViewTransportMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnVehicleMaster_click
    protected void btnVehicleMaster_click(object sender, EventArgs e)
    {
        checkRights(133);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/VIEW/ViewVehicleMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnVehicleMaster_click

    protected void btnAreaMaster_click(object sender, EventArgs e)
    {
        checkRights(13);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewAreaMaster.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


}
