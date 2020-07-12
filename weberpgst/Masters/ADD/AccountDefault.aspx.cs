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

public partial class Masters_AccountDefault : System.Web.UI.Page
{
    string right = "";
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
                #region Hiding Menus As Per Rights

                #region AccountMaster
                string AccountMaster = "";
                DataTable dtAccountMaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 66 + "");
                AccountMaster = dtAccountMaster.Rows.Count == 0 ? "0000000" : dtAccountMaster.Rows[0][0].ToString();
                if (AccountMaster == "0000000" || AccountMaster == "0111111")
                {
                    LiAccountMaster.Visible = false;
                }
                #endregion AccountMaster

                #endregion
            }
        }
    }
    #endregion Page_Load

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #region btnTallySales_click
    protected void btnTallySales_click(object sender, EventArgs e)
    {
        checkRights(66);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            //Response.Redirect("../../Utility/VIEW/ViewTallyTransferSales.aspx", false);
            // Hiten 
            // Direct Shows Tally Transfer Screen
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/TallyTransSales .aspx?c_name=" + type, false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region btnTallyPurchase_click
    protected void btnTallyPurchase_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Utility/ADD/TallyTransferPurchase.aspx?c_name=INSERT", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/AccountDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Account Default", "btnOk_Click", Ex.Message);
        }
    }


}
