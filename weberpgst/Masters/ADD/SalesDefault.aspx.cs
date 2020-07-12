using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Masters_ADD_SalesDefault : System.Web.UI.Page
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
                string sotypemaster = "";
                DataTable dtsotypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 63 + "");
                sotypemaster = dtsotypemaster.Rows.Count == 0 ? "0000000" : dtsotypemaster.Rows[0][0].ToString();
                if (sotypemaster == "0000000" || sotypemaster == "0111111")
                {
                    SoType.Visible = false;
                }


                string custtypemaster = "";
                DataTable dtcusttypemaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 64 + "");
                custtypemaster = dtcusttypemaster.Rows.Count == 0 ? "0000000" : dtcusttypemaster.Rows[0][0].ToString();
                if (custtypemaster == "0000000" || custtypemaster == "0111111")
                {
                    CustType.Visible = false;
                }

                string customer = "";
                DataTable dtcustomer = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 14 + "");
                customer = dtcustomer.Rows.Count == 0 ? "0000000" : dtcustomer.Rows[0][0].ToString();
                if (customer == "0000000" || customer == "0111111")
                {
                    Customer.Visible = false;
                }

                string CustomerEnq = "";
                DataTable dtCustEnq = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 95 + "");
                CustomerEnq = dtCustEnq.Rows.Count == 0 ? "0000000" : dtCustEnq.Rows[0][0].ToString();
                if (CustomerEnq == "0000000" || CustomerEnq == "0111111")
                {
                    CustomerEnquiry.Visible = false;
                }

                string saleorder = "";
                DataTable dtsaleorder = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 18 + "");
                saleorder = dtsaleorder.Rows.Count == 0 ? "0000000" : dtsaleorder.Rows[0][0].ToString();
                if (saleorder == "0000000" || saleorder == "0111111")
                {
                    Saleorder.Visible = false;
                }

                string taxinvoice = "";
                DataTable dttaxinvoice = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 19 + "");
                taxinvoice = dttaxinvoice.Rows.Count == 0 ? "0000000" : dttaxinvoice.Rows[0][0].ToString();
                if (taxinvoice == "0000000" || taxinvoice == "0111111")
                {
                    TaxInvoice.Visible = false;
                }

                string expinv = "";
                DataTable dtexpinv = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 20 + "");
                expinv = dtexpinv.Rows.Count == 0 ? "0000000" : dtexpinv.Rows[0][0].ToString();
                if (expinv == "0000000" || expinv == "0111111")
                {
                    ExportInvoice.Visible = false;
                }

                string WorkOrder = "";
                DataTable dtworkorder = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 94 + "");
                WorkOrder = dtworkorder.Rows.Count == 0 ? "0000000" : dtworkorder.Rows[0][0].ToString();
                if (WorkOrder == "0000000" || WorkOrder == "0111111")
                {
                    workorder.Visible = false;
                }


                string itemSale = "";
                DataTable dtItemSale = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 116 + "");
                itemSale = dtItemSale.Rows.Count == 0 ? "0000000" : dtItemSale.Rows[0][0].ToString();
                if (itemSale == "0000000" || itemSale == "0111111")
                {
                    ItemSale.Visible = false;
                }



                //string custrej = "";
                //DataTable dtcustrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 21 + "");
                //custrej = dtcustrej.Rows.Count == 0 ? "0000000" : dtcustrej.Rows[0][0].ToString();
                //if (custrej == "0000000" || custrej == "0111111")
                //{
                //    CustRej.Visible = false;
                //}

                string masterreg = "";
                DataTable dtmasterreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 22 + "");
                masterreg = dtmasterreg.Rows.Count == 0 ? "0000000" : dtmasterreg.Rows[0][0].ToString();
                if (masterreg == "0000000" || masterreg == "0111111")
                {
                    MasterRpt.Visible = false;
                }

                string saleordreg = "";
                DataTable dtsaleordreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 23 + "");
                saleordreg = dtsaleordreg.Rows.Count == 0 ? "0000000" : dtsaleordreg.Rows[0][0].ToString();
                if (saleordreg == "0000000" || saleordreg == "0111111")
                {
                    SaleOrdReg.Visible = false;
                }

                string taxinvreg = "";
                DataTable dtQtn = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 24 + "");
                taxinvreg = dtQtn.Rows.Count == 0 ? "0000000" : dtQtn.Rows[0][0].ToString();
                if (taxinvreg == "00000000" || taxinvreg == "01111111")
                {
                    TaxInvReg.Visible = false;
                }

                string expinvreg = "";
                DataTable dtexpinvreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 35 + "");
                expinvreg = dtexpinvreg.Rows.Count == 0 ? "0000000" : dtexpinvreg.Rows[0][0].ToString();
                if (expinvreg == "0000000" || expinvreg == "0111111")
                {
                    ExpInvReg.Visible = false;
                }

                //string custrejreg = "";
                //DataTable dtcustrejreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 53 + "");
                //custrejreg = dtcustrejreg.Rows.Count == 0 ? "0000000" : dtcustrejreg.Rows[0][0].ToString();
                //if (custrejreg == "0000000" || custrejreg == "0111111")
                //{
                //    CustRejReg.Visible = false;
                //}

                string stocklegderReg = "";
                DataTable dtstocklegderReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 59 + "");
                stocklegderReg = dtstocklegderReg.Rows.Count == 0 ? "0000000" : dtstocklegderReg.Rows[0][0].ToString();
                if (stocklegderReg == "0000000" || stocklegderReg == "0111111")
                {
                    StockReg.Visible = false;
                }

                string stocklegder = "";
                DataTable dtstocklegder = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 91 + "");
                stocklegder = dtstocklegder.Rows.Count == 0 ? "0000000" : dtstocklegder.Rows[0][0].ToString();
                if (stocklegder == "0000000" || stocklegder == "0111111")
                {
                    StockReport.Visible = false;
                }

                string salesoredependingStatus = "";
                DataTable dtsalesoredependingStatus = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 91 + "");
                salesoredependingStatus = dtsalesoredependingStatus.Rows.Count == 0 ? "0000000" : dtsalesoredependingStatus.Rows[0][0].ToString();
                if (salesoredependingStatus == "0000000" || salesoredependingStatus == "0111111")
                {
                    //StockReport.Visible = false;
                    SalesOrderPendingStatus.Visible = false;
                }

                string StockLedgerType = "";
                DataTable dtStoctLedgerType = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 108 + "");
                StockLedgerType = dtStoctLedgerType.Rows.Count == 0 ? "0000000" : dtStoctLedgerType.Rows[0][0].ToString();
                if (StockLedgerType == "0000000" || StockLedgerType == "0111111")
                {
                    StoctLedgerType.Visible = false;
                }

                string delchareg = "";
                DataTable dtbofreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 78 + "");
                delchareg = dtbofreg.Rows.Count == 0 ? "0000000" : dtbofreg.Rows[0][0].ToString();
                if (delchareg == "0000000" || delchareg == "0111111")
                {
                    DelChaReg.Visible = false;
                }

                string underDrwaback = "";
                DataTable dtUnderDrwaback = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 92 + "");
                underDrwaback = dtUnderDrwaback.Rows.Count == 0 ? "0000000" : dtUnderDrwaback.Rows[0][0].ToString();
                if (underDrwaback == "0000000" || underDrwaback == "0111111")
                {
                    UnderDrwaback.Visible = false;
                }

                string selfSealingCertificate = "";
                DataTable dtSelfSealingCertificate = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 93 + "");
                selfSealingCertificate = dtSelfSealingCertificate.Rows.Count == 0 ? "0000000" : dtSelfSealingCertificate.Rows[0][0].ToString();
                if (selfSealingCertificate == "0000000" || selfSealingCertificate == "0111111")
                {
                    SelfSealingCertificate.Visible = false;
                }

                string EnqReg = "";
                DataTable dtEnquiryReg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 96 + "");
                EnqReg = dtEnquiryReg.Rows.Count == 0 ? "0000000" : dtEnquiryReg.Rows[0][0].ToString();
                if (EnqReg == "0000000" || EnqReg == "0111111")
                {
                    EnquiryReg.Visible = false;
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

    #region Masters
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

    protected void btnSOTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(63);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewSOTypeMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustomerTypeMaster_click(object sender, EventArgs e)
    {
        checkRights(64);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/View/ViewCustomerTypeMaster.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
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
    protected void btnRawMaterial_click(object sender, EventArgs e)
    {
        checkRights(11);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            string type = "sale";
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + type + "", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //protected void btnSectorMaster_click(object sender, EventArgs e)
    //{
    //    checkRights(12);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/Masters/VIEW/ViewSectorMaster.aspx", false);
    //    }
    //    else
    //    {
    //        //Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}
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
    protected void btnCustomerMaster_click(object sender, EventArgs e)
    {
        checkRights(14);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewCustomerMaster.aspx", false);
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
            Response.Redirect("~/Masters/VIEW/ViewExciseTariffDetails.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //protected void btnSalesTaxMaster_click(object sender, EventArgs e)
    //{
    //    checkRights(16);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/Masters/VIEW/ViewSalesTaxMaster.aspx", false);
    //    }
    //    else
    //    {
    //        // Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}
    //protected void btnSupplierTypeMaster_click(object sender, EventArgs e)
    //{
    //    checkRights(17);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/Masters/VIEW/ViewSupplierTypeMaster.aspx", false);
    //    }
    //    else
    //    {
    //        //Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}
    #endregion

    #region Transaction

    protected void btnCustomerEnquiry_click(object sender, EventArgs e)
    {
        checkRights(95);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerEnquiry.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustomerPO_click(object sender, EventArgs e)
    {
        checkRights(18);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnProductionToStore_click(object sender, EventArgs e)
    {
        checkRights(52);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMaterialRequisition_click(object sender, EventArgs e)
    {
        checkRights(50);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnIssueToProduction_click(object sender, EventArgs e)
    {
        checkRights(51);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTaxInvoice_click(object sender, EventArgs e)
    {
        checkRights(19);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnLabourChargeInvoiceInvoice_click(object sender, EventArgs e)
    {
        checkRights(19);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnExportInvoice_click(object sender, EventArgs e)
    {
        checkRights(20);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewExportInvoice.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPurchaseRejInvoice_click(object sender, EventArgs e)
    {
        checkRights(20);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejeInvoice.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    //protected void btnCustomerRejection_click(object sender, EventArgs e)
    //{
    //    checkRights(21);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
    //    }
    //    else
    //    {
    //        //Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}

    protected void btnDeliveryChallan_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewDeliveryChallan.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTrayDeliveryChallan_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/TrayChallan.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnWorkOrder_click(object sender, EventArgs e)
    {
        checkRights(94);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSaleWorkOrder.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnPDIDetails_click(object sender, EventArgs e)
    {
        checkRights(125);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewPDIDetails.aspx", false);
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
    protected void btnMasterReport_click(object sender, EventArgs e)
    {
        checkRights(22);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnCustomerOrderRegister_click(object sender, EventArgs e)
    {
        checkRights(23);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerOrderRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustomerOrderReport_click(object sender, EventArgs e)
    {
        checkRights(90);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSalesOrderReport.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //btnSalesSummaryRegister_click

    protected void btnSalesSummaryRegister_click(object sender, EventArgs e)
    {
        checkRights(25);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSalesSummary.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTaxInvoiceRegister_click(object sender, EventArgs e)
    {
        checkRights(25);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTaxInvoiceRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //protected void btnCustomerRejectionRegister_click(object sender, EventArgs e)
    //{
    //    checkRights(53);
    //    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
    //    {
    //        Response.Redirect("~/RoportForms/VIEW/ViewCustomerRejectionRegister.aspx", false);
    //    }
    //    else
    //    {
    //        //Response.Write("<script> alert('You Have No Rights To View.');</script>");
    //        ModalPopupMsg.Show();
    //        return;
    //    }
    //}

    protected void btnExportInvoiceRegister_click(object sender, EventArgs e)
    {
        checkRights(35);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewExportInvoiceRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStockLedgerRegister_click(object sender, EventArgs e)
    {
        checkRights(59);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedgerRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedger.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStoctLedgerType_click(object sender, EventArgs e)
    {
        checkRights(108);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedgerTypewise.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnDeliveryChallanRegister(object sender, EventArgs e)
    {
        checkRights(78);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DeliveryChallanRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTrayDeliveryChallanRegister(object sender, EventArgs e)
    {
        checkRights(78);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DeliveryChallanRegisterTray.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnUnderDrwaback_click(object sender, EventArgs e)
    {
        checkRights(92);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewUnderDrawbackReport.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSelfSealingCertificate(object sender, EventArgs e)
    {
        checkRights(93);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSelfSealingCertificateReport.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnEnquiryRegister_click(object sender, EventArgs e)
    {
        checkRights(96);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerEnquiryRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSalesOrderPendingStatus_click(object sender, EventArgs e)
    {
        //checkRights(109);
        checkRights(90);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSalesPendingOrderStatus.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnItemSale_click(object sender, EventArgs e)
    {
        checkRights(116);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewItemSaleMonthWise.aspx", false);
        }
        else
        {
            ModalPopupMsg.Show();
            return;
        }
    }


    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Default", "btnOk_Click", Ex.Message);
        }
    }
}