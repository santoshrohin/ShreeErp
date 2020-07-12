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


public partial class Masters_ADD_MasterDefault : System.Web.UI.Page
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
                //string country = "";
                //DataTable dtcountry = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 5 + "");
                //country = dtcountry.Rows.Count == 0 ? "0000000" : dtcountry.Rows[0][0].ToString();
                //if (country == "0000000" || country == "0111111")
                //{
                //    Country.Visible = false;
                //}


                //string state = "";
                //DataTable dtstate = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 6 + "");
                //state = dtstate.Rows.Count == 0 ? "0000000" : dtstate.Rows[0][0].ToString();
                //if (state == "0000000" || state == "0111111")
                //{
                //    State.Visible = false;
                //}

                //string city = "";
                //DataTable dtcity = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 7 + "");
                //city = dtcity.Rows.Count == 0 ? "0000000" : dtcity.Rows[0][0].ToString();
                //if (city == "0000000" || city == "0111111")
                //{
                //    City.Visible = false;
                //}

                //string currency = "";
                //DataTable dtcurrency = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 8 + "");
                //currency = dtcurrency.Rows.Count == 0 ? "0000000" : dtcurrency.Rows[0][0].ToString();
                //if (currency == "0000000" || currency == "0111111")
                //{
                //    Currency.Visible = false;
                //}

                string tally = "";
                DataTable dttally = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 32 + "");
                tally = dttally.Rows.Count == 0 ? "0000000" : dttally.Rows[0][0].ToString();
                if (tally == "0000000" || tally == "0111111")
                {
                    Tally.Visible = false;
                }

                string subCategory = "";
                DataTable dtSubCategory = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 128 + "");
                subCategory = dtSubCategory.Rows.Count == 0 ? "0000000" : dtSubCategory.Rows[0][0].ToString();
                if (subCategory == "0000000" || subCategory == "0111111")
                {
                    SubCategory.Visible = false;
                }

                string itemcat = "";
                DataTable dtitemcat = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 9 + "");
                itemcat = dtitemcat.Rows.Count == 0 ? "0000000" : dtitemcat.Rows[0][0].ToString();
                if (itemcat == "0000000" || itemcat == "0111111")
                {
                    ItemCategory.Visible = false;
                }

                string itemsubcat = "";
                DataTable dtitemsubcat = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 34 + "");
                itemsubcat = dtitemsubcat.Rows.Count == 0 ? "0000000" : dtitemsubcat.Rows[0][0].ToString();
                if (itemsubcat == "0000000" || itemsubcat == "0111111")
                {
                    ItemSubCat.Visible = false;
                }

                //string excise = "";
                //DataTable dtexcise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 15 + "");
                //excise = dtexcise.Rows.Count == 0 ? "0000000" : dtexcise.Rows[0][0].ToString();
                //if (excise == "0000000" || excise == "0111111")
                //{
                //    Excise.Visible = false;
                //}

                //string unit = "";
                //DataTable dtunit = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 10 + "");
                //unit = dtunit.Rows.Count == 0 ? "0000000" : dtunit.Rows[0][0].ToString();
                //if (unit == "0000000" || unit == "0111111")
                //{
                //    Unit.Visible = false;
                //}

                string item = "";
                DataTable dtitem = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 11 + "");
                item = dtitem.Rows.Count == 0 ? "0000000" : dtitem.Rows[0][0].ToString();
                if (item == "0000000" || item == "0111111")
                {
                    Item.Visible = false;
                }

                //string area = "";
                //DataTable dtarea = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 13 + "");
                //area = dtarea.Rows.Count == 0 ? "0000000" : dtarea.Rows[0][0].ToString();
                //if (area == "0000000" || area == "0111111")
                //{
                //    Area.Visible = false;
                //}

                //string saletax = "";
                //DataTable dtsaletax = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 16 + "");
                //saletax = dtsaletax.Rows.Count == 0 ? "0000000" : dtsaletax.Rows[0][0].ToString();
                //if (saletax == "0000000" || saletax == "0111111")
                //{
                //    SalesTax.Visible = false;
                //}

                //ServiceTypeMaster

                string servicetypemaster = "";
                DataTable dtservicetypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 119 + "");
                servicetypemaster = dtservicetypemaster.Rows.Count == 0 ? "0000000" : dtservicetypemaster.Rows[0][0].ToString();
                if (servicetypemaster == "0000000" || servicetypemaster == "0111111")
                {
                    ServiceTypeMaster.Visible = false;
                }

                string gst = "";
                DataTable dtgst = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 118 + "");
                gst = dtgst.Rows.Count == 0 ? "0000000" : dtgst.Rows[0][0].ToString();
                if (gst == "0000000" || gst == "0111111")
                {
                    GST.Visible = false;
                }
                #endregion
            }
        }
    }

    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
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
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnSubCategoryMaster_click
    protected void btnSubCategoryMaster_click(object sender, EventArgs e)
    {
        checkRights(128);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Admin/View/ViewSubCategoryMaster.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnSubCategoryMaster_click

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
            ModalPopupMsg.Show();
            return;
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

            ModalPopupMsg.Show();
            return;
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
            ModalPopupMsg.Show();
            return;
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
            ModalPopupMsg.Show();
            return;
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }

    protected void btnItemCategory_click(object sender, EventArgs e)
    {
        checkRights(9);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);

        }
        else
        {
            //popUpPanel5.Visible = true;
            //ModalPopupMsg.TargetControlID = "l1";
            ModalPopupMsg.Show();
            return;
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
        }
    }
    //btnItemSubCategoryMaster_click
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
    protected void btnItemSubCategoryMaster_click(object sender, EventArgs e)
    {
        checkRights(34);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSubCategoryMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

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

    protected void btnGSTMaster_click(object sender, EventArgs e)
    {
        checkRights(118);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {

            Response.Redirect("~/Masters/VIEW/ViewGST_HSNSAC_Master.aspx?Title=true", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnExciseTariffDetails_click(object sender, EventArgs e)
    {
        checkRights(15);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewExciseTariffDetails.aspx?Title=false", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //btnBOMMaster_click
    protected void btnBOMMaster_click(object sender, EventArgs e)
    {
        checkRights(44);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnBOMRegister_click(object sender, EventArgs e)
    {
        checkRights(45);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/RoportForms/VIEW/ViewBillOfMaterialRegister.aspx?c_name=" + type + "", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMasterReport_click(object sender, EventArgs e)
    {
        checkRights(117);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/RoportForms/ADD/AllMasterReport.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }



    protected void btnServiceTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(119);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {

            Response.Redirect("~/Masters/VIEW/ViewServiceMaster.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnItemMaster_click(object sender, EventArgs e)
    {
        checkRights(11);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "sale";
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    #region btnUnitMaster_click
    //protected void btnUnitMaster_click(object sender, EventArgs e)
    //{
    //    checkRights(10);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        string type = "purchase";
    //        Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx?c_name=" + type + "", false);
    //    }
    //    else
    //    {
    //        // Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}
    #endregion btnUnitMaster_click

    protected void btnSalesTaxMaster_click(object sender, EventArgs e)
    {
        checkRights(16);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Default", "btnOk_Click", Ex.Message);
        }
    }






}
