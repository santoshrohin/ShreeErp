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

public partial class Masters_UtilityDefault : System.Web.UI.Page
{
    string right = "";
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
                string tallysales = "";
                DataTable dttallysales = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 66 + "");
                tallysales = dttallysales.Rows.Count == 0 ? "0000000" : dttallysales.Rows[0][0].ToString();
                if (tallysales == "0000000" || tallysales == "0111111")
                {
                    TallySales.Visible = false;
                }


                string tallypurchase = "";
                DataTable dttallypurchase = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 67 + "");
                tallypurchase = dttallypurchase.Rows.Count == 0 ? "0000000" : dttallypurchase.Rows[0][0].ToString();
                if (tallypurchase == "0000000" || tallypurchase == "0111111")
                {
                    TallyPurchase.Visible = false;
                }


                string itemMaster = "";
                DataTable dtItemMaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 113 + "");
                itemMaster = dtItemMaster.Rows.Count == 0 ? "0000000" : dtItemMaster.Rows[0][0].ToString();
                if (itemMaster == "0000000" || itemMaster == "0111111")
                {
                    ItemMaster.Visible = false;
                }

                
                #endregion
            }

        }
    }
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



    #region Einv_click
    protected void Einv_click(object sender, EventArgs e)
    {
        checkRights(66);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            //Response.Redirect("../../Utility/VIEW/ViewTallyTransferSales.aspx", false);
            // Hiten 
            // Direct Shows Tally Transfer Screen
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/EInvoice.aspx?c_name=" + type, false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion


    #region SuppliInvoice_click
    protected void SuppliInvoice_click(object sender, EventArgs e)
    {
        checkRights(66);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            //Response.Redirect("../../Utility/VIEW/ViewTallyTransferSales.aspx", false);
            // Hiten 
            // Direct Shows Tally Transfer Screen
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/SuppliementoryInvoice.aspx?c_name=" + type, false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region tallyImport_click
    protected void tallyImport_click(object sender, EventArgs e)
    {
       
            string type = "INSERT";
            Response.Redirect("~/Utility/ADD/TallyImport.aspx", false);
       
    }
    #endregion

    #region btnTallyPurchase_click
    protected void btnTallyPurchase_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            //Response.Redirect("../../Utility/VIEW/ViewTallyTransferPurchase.aspx", false);
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

    #region btnVCM_click
    protected void btnVCM_click(object sender, EventArgs e)
    {
        checkRights(67);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Utility/ADD/CreateVCM.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion


    #region btnECE_click
    protected void btnECE_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("../../Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
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
            Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Utility Default", "btnOk_Click", Ex.Message);
        }
    }

    protected void btnItemAdd_click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Utility/ADD/ItemAddtion.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Utility Default", "btnItemAdd_click", Ex.Message);
        }
    }

protected void tempBal_click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Utility/ADD/ClearUnusedStock.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Utility Default", "btnItemAdd_click", Ex.Message);
        }
    }

}
