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

public partial class Masters_PurchaseDefault : System.Web.UI.Page
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
                string potypemaster = "";
                DataTable dtpotypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 33 + "");
                potypemaster = dtpotypemaster.Rows.Count == 0 ? "0000000" : dtpotypemaster.Rows[0][0].ToString();
                if (potypemaster == "0000000" || potypemaster == "0111111")
                {
                    POTypeMaster.Visible = false;
                }


                string suptypemaster = "";
                DataTable dtsuptypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 17 + "");
                suptypemaster = dtsuptypemaster.Rows.Count == 0 ? "0000000" : dtsuptypemaster.Rows[0][0].ToString();
                if (suptypemaster == "0000000" || suptypemaster == "0111111")
                {
                    SuppTypeMaster.Visible = false;
                }

                string supplier = "";
                DataTable dtsupplier = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 26 + "");
                supplier = dtsupplier.Rows.Count == 0 ? "0000000" : dtsupplier.Rows[0][0].ToString();
                if (supplier == "0000000" || supplier == "0111111")
                {
                    SuppMaster.Visible = false;
                }

                string purord = "";
                DataTable dtpurord = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 42 + "");
                purord = dtpurord.Rows.Count == 0 ? "0000000" : dtpurord.Rows[0][0].ToString();
                if (purord == "0000000" || purord == "0111111")
                {
                    PurOrd.Visible = false;
                }

                string inwrd = "";
                DataTable dtinwrd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 39 + "");
                inwrd = dtinwrd.Rows.Count == 0 ? "0000000" : dtinwrd.Rows[0][0].ToString();
                if (inwrd == "0000000" || inwrd == "0111111")
                {
                    Inwrd.Visible = false;
                }

                string inspection = "";
                DataTable dtinspection = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 40 + "");
                inspection = dtinspection.Rows.Count == 0 ? "0000000" : dtinspection.Rows[0][0].ToString();
                if (inspection == "0000000" || inspection == "0111111")
                {
                    Inspection.Visible = false;
                }

                string billpass = "";
                DataTable dtbillpass = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 41 + "");
                billpass = dtbillpass.Rows.Count == 0 ? "0000000" : dtbillpass.Rows[0][0].ToString();
                if (billpass == "0000000" || billpass == "0111111")
                {
                    BillPass.Visible = false;
                }

                string purreq = "";
                DataTable dtpurreq = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 54 + "");
                purreq = dtpurreq.Rows.Count == 0 ? "0000000" : dtpurreq.Rows[0][0].ToString();
                if (purreq == "0000000" || purreq == "0111111")
                {
                    PurReq.Visible = false;
                }

                string purrej = "";
                DataTable dtpurrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 68 + "");
                purrej = dtpurrej.Rows.Count == 0 ? "0000000" : dtpurrej.Rows[0][0].ToString();
                if (purrej == "0000000" || purrej == "0111111")
                {
                    //PurRej.Visible = false;
                }

                string amc = "";
                DataTable dtamc = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 69 + "");
                amc = dtamc.Rows.Count == 0 ? "0000000" : dtamc.Rows[0][0].ToString();
                if (amc == "0000000" || amc == "0111111")
                {
                    AMC.Visible = false;
                }

                string wo = "";
                DataTable dtwo = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 70 + "");
                wo = dtwo.Rows.Count == 0 ? "0000000" : dtwo.Rows[0][0].ToString();
                if (wo == "0000000" || wo == "0111111")
                {
                    WRK.Visible = false;
                }

                string PO_Transfer = "";
                DataTable dtPOTransfer = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 106 + "");
                wo = dtPOTransfer.Rows.Count == 0 ? "0000000" : dtPOTransfer.Rows[0][0].ToString();
                if (PO_Transfer == "0000000" || PO_Transfer == "0111111")
                {
                    POTransfer.Visible = false;
                }

                string purordreg = "";
                DataTable dtpurordreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 46 + "");
                purordreg = dtpurordreg.Rows.Count == 0 ? "0000000" : dtpurordreg.Rows[0][0].ToString();
                if (purordreg == "0000000" || purordreg == "0111111")
                {
                    POReg.Visible = false;
                }

                string matinwdreg = "";
                DataTable dtmatinwdreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 47 + "");
                matinwdreg = dtmatinwdreg.Rows.Count == 0 ? "0000000" : dtmatinwdreg.Rows[0][0].ToString();
                if (matinwdreg == "0000000" || matinwdreg == "0111111")
                {
                    //InwdReg.Visible = false;
                }

                string inspreg = "";
                DataTable dtinspreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 48 + "");
                inspreg = dtinspreg.Rows.Count == 0 ? "0000000" : dtinspreg.Rows[0][0].ToString();
                if (inspreg == "0000000" || inspreg == "0111111")
                {
                    //Inspreg.Visible = false;
                }

                string billpassreg = "";
                DataTable dtbillpassreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 49 + "");
                billpassreg = dtbillpassreg.Rows.Count == 0 ? "0000000" : dtbillpassreg.Rows[0][0].ToString();
                if (billpassreg == "0000000" || billpassreg == "0111111")
                {
                    BillPassReg.Visible = false;
                }

                //string purreqreg = "";
                //DataTable dtpurreqreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 58 + "");
                //purreqreg = dtpurreqreg.Rows.Count == 0 ? "0000000" : dtpurreqreg.Rows[0][0].ToString();
                //if (purreqreg == "0000000" || purreqreg == "0111111")
                //{
                //    PurReqReg.Visible = false;
                //}

                //string purrejreg = "";
                //DataTable dtpurrejreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 79 + "");
                //purrejreg = dtpurrejreg.Rows.Count == 0 ? "0000000" : dtpurrejreg.Rows[0][0].ToString();
                //if (purrejreg == "0000000" || purrejreg == "0111111")
                //{
                //    PurRejReg.Visible = false;
                //}


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

    #region Master
    protected void btnSupplierMaster_click(object sender, EventArgs e)
    {
        checkRights(26);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSupplierMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
  
    protected void btnSupplierTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(17);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSupplierTypeMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnItemMaster_click(object sender, EventArgs e)
    {
        checkRights(11);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnProcess_click(object sender, EventArgs e)
    {
        checkRights(88);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "purchase";
            Response.Redirect("~/Admin/VIEW/ViewProcessMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPoTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(33);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewPoTypeMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnProCode_click(object sender, EventArgs e)
    {
        checkRights(112);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    #region Transaction
    protected void btnSupplierPO_click(object sender, EventArgs e)
    {
        checkRights(42);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnServicePO_click(object sender, EventArgs e)
    {
        checkRights(120);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewServicePurchaseOrder.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    

    //btnSubContractorPO_click
    protected void btnSubContractorPO_click(object sender, EventArgs e)
    {
        checkRights(42);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnMaterialInspection_click(object sender, EventArgs e)
    {
        checkRights(40);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInspection.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnMaterialInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnAMC_click(object sender, EventArgs e)
    {
        checkRights(69);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnWO_click(object sender, EventArgs e)
    {
        checkRights(70);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewAMCWorkOrder.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

  

    protected void btnBillPassing_click(object sender, EventArgs e)
    {
        checkRights(41);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
        }
        else
        {
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnCustomerSchedule_click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewCustomerSchedule.aspx", false);
        checkRights(41);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerSchedule.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnPurReq_click(object sender, EventArgs e)
    {
        checkRights(54);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
         {
        Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);
         }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnScustSchedule_click(object sender, EventArgs e)
    {
        Response.Redirect("~/RoportForms/VIEW/ViewCustomerScheduleReport.aspx", false);
        
        //checkRights(54);
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        //{
        //    Response.Redirect("~/RoportForms/VIEW/ViewCustomerScheduleReport.aspx", false);
        //}
        //else
        //{
        //    //Response.Write("<script> alert('You Have No Rights To View.');</script>");
        //    ModalPopupMsg.Show();
        //    return;
        //}
    }

    protected void btnPurRej_click(object sender, EventArgs e)
    {
       checkRights(68);
       if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
             Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
        }
       else
       {
           Response.Write("<script> alert('You Have No Rights To View.');</script>");
           ModalPopupMsg.Show();
           return;
       }
    }

    protected void POTransfer_click(object sender, EventArgs e)
    {
        checkRights(106);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPOTransfer.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    #endregion

    #region Reports


    protected void btnServicePoRegister_click(object sender, EventArgs e)
    {
        checkRights(121);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewServicePoRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnSupplierPoRegister_click(object sender, EventArgs e)
    {
        checkRights(46);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSupplierPoAmendRegister_click(object sender, EventArgs e)
    {
        checkRights(89);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoAmendRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnBillPassingRegister_click(object sender, EventArgs e)
    {
        checkRights(49);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBillPassingRegister.aspx", false);
        }
        else
        {
           // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPurchaseRequisitionRegister_Click(object sender, EventArgs e)
    {
        checkRights(58);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewPurchaseRequisitionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnPurchaseRejectionRegister_Click(object sender, EventArgs e)
    {
        checkRights(79);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/PurchaseRejectionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Default", "btnOk_Click", Ex.Message);
        }
    }

  
    
}
