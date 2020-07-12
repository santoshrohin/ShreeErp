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

public partial class Masters_ProductionDefault : System.Web.UI.Page
{
    #region Variable
    string right = "";
    #endregion

    #region Event

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

        //LinkButton ProductionLink = this.Page.Master.FindControl("Production") as li;


        //ProductionLink.Attributes.Add("class", "active");

        //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("List").FindControl("Production");

        //home.Attributes["class"] = "active";
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            if (!IsPostBack)
            {
                #region Hiding Menus As Per Rights
                string bom = "";
                DataTable dtbom = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 44 + "");
                bom = dtbom.Rows.Count == 0 ? "0000000" : dtbom.Rows[0][0].ToString();
                if (bom == "0000000" || bom == "0111111")
                {
                    //BOM.Visible = false;
                }


                string matreq = "";
                DataTable dtmatreq = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 50 + "");
                matreq = dtmatreq.Rows.Count == 0 ? "0000000" : dtmatreq.Rows[0][0].ToString();
                if (matreq == "0000000" || matreq == "0111111")
                {
                    MatReq.Visible = false;
                }

                string fillofsheet = "";
                DataTable dtfill = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 101 + "");
                fillofsheet = dtfill.Rows.Count == 0 ? "0000000" : dtfill.Rows[0][0].ToString();
                if (fillofsheet == "0000000" || fillofsheet == "0111111")
                {
                    FillOffSheet.Visible = false;
                }

                //string issueprod = "";
                //DataTable dtissueprod = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 51 + "");
                //issueprod = dtissueprod.Rows.Count == 0 ? "0000000" : dtissueprod.Rows[0][0].ToString();
                //if (issueprod == "0000000" || issueprod == "0111111")
                //{
                //    IssueProd.Visible = false;
                //}

                string issuefillofsheet = "";
                DataTable dtissuefill = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 105 + "");
                issuefillofsheet = dtissuefill.Rows.Count == 0 ? "0000000" : dtissuefill.Rows[0][0].ToString();
                if (issuefillofsheet == "0000000" || issuefillofsheet == "0111111")
                {
                    IssueFillOffSheet.Visible = false;
                }

                //string prodstore = "";
                //DataTable dtprodstore = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 52 + "");
                //prodstore = dtprodstore.Rows.Count == 0 ? "0000000" : dtprodstore.Rows[0][0].ToString();
                //if (prodstore == "0000000" || prodstore == "0111111")
                //{
                //    ProdStore.Visible = false;
                //}

                string stockadj = "";
                DataTable dtstockadj = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 60 + "");
                stockadj = dtstockadj.Rows.Count == 0 ? "0000000" : dtstockadj.Rows[0][0].ToString();
                if (stockadj == "0000000" || stockadj == "0111111")
                {
                    StockAdj.Visible = false;
                }

                //string bomreg = "";
                //DataTable dtbomreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 45 + "");
                //bomreg = dtbomreg.Rows.Count == 0 ? "0000000" : dtbomreg.Rows[0][0].ToString();
                //if (bomreg == "0000000" || bomreg == "0111111")
                //{
                //    BOMReg.Visible = false;
                //}

                string matreqreg = "";
                DataTable dtmatreqreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 55 + "");
                matreqreg = dtmatreqreg.Rows.Count == 0 ? "0000000" : dtmatreqreg.Rows[0][0].ToString();
                if (matreqreg == "0000000" || matreqreg == "0111111")
                {
                    MatReqReg.Visible = false;
                }

                //string issueprodreg = "";
                //DataTable dtissueprodreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 56 + "");
                //issueprodreg = dtissueprodreg.Rows.Count == 0 ? "0000000" : dtissueprodreg.Rows[0][0].ToString();
                //if (issueprodreg == "0000000" || issueprodreg == "0111111")
                //{
                //    IssueProdReg.Visible = false;
                //}

                //string prodstorereg = "";
                //DataTable dtprodstorereg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 57 + "");
                //prodstorereg = dtprodstorereg.Rows.Count == 0 ? "0000000" : dtprodstorereg.Rows[0][0].ToString();
                //if (prodstorereg == "0000000" || prodstorereg == "0111111")
                //{
                //    ProdReg.Visible = false;
                //}

                string matreqmis = "";
                DataTable dtmatreqmis = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 62 + "");
                matreqmis = dtmatreqmis.Rows.Count == 0 ? "0000000" : dtmatreqmis.Rows[0][0].ToString();
                if (matreqmis == "0000000" || matreqmis == "0111111")
                {
                    MatreqMisReg.Visible = false;
                }

                string stockadjreg = "";
                DataTable dtstockadjreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 61 + "");
                stockadjreg = dtstockadjreg.Rows.Count == 0 ? "0000000" : dtstockadjreg.Rows[0][0].ToString();
                if (stockadjreg == "0000000" || stockadjreg == "0111111")
                {
                    StockAsjreg.Visible = false;
                }

                //string custrej = "";
                //DataTable dtcustrej = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 21 + "");
                //custrej = dtcustrej.Rows.Count == 0 ? "0000000" : dtcustrej.Rows[0][0].ToString();
                //if (custrej == "0000000" || custrej == "0111111")
                //{
                //    CustRej.Visible = false;
                //}

                //string custrejreg = "";
                //DataTable dtcustrejreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 53 + "");
                //custrejreg = dtcustrejreg.Rows.Count == 0 ? "0000000" : dtcustrejreg.Rows[0][0].ToString();
                //if (custrejreg == "0000000" || custrejreg == "0111111")
                //{
                //    CustRejReg.Visible = false;
                //}

                string matinwdreg = "";
                DataTable dtmatinwdreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 47 + "");
                matinwdreg = dtmatinwdreg.Rows.Count == 0 ? "0000000" : dtmatinwdreg.Rows[0][0].ToString();
                if (matinwdreg == "0000000" || matinwdreg == "0111111")
                {
                    InwdReg.Visible = false;
                }

                string inspreg = "";
                DataTable dtinspreg = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 48 + "");
                inspreg = dtinspreg.Rows.Count == 0 ? "0000000" : dtinspreg.Rows[0][0].ToString();
                if (inspreg == "0000000" || inspreg == "0111111")
                {
                    Inspreg.Visible = false;
                }
                #endregion


                //string turningRegister = "";
                //DataTable dtturningRegister = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 114 + "");
                //turningRegister = dtturningRegister.Rows.Count == 0 ? "0000000" : dtturningRegister.Rows[0][0].ToString();
                //if (turningRegister == "0000000" || turningRegister == "0111111")
                //{
                //    TurningRegister.Visible = false;
                //}

                //string turIWD = "";
                //DataTable dtturIWD = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 115 + "");
                //turIWD = dtturIWD.Rows.Count == 0 ? "0000000" : dtturIWD.Rows[0][0].ToString();
                //if (turIWD == "0000000" || turIWD == "0111111")
                //{
                //    TurIWD.Visible = false;
                //}


            }

        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production Default", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region Master
    protected void btnBOMMaster_click(object sender, EventArgs e)
    {
        checkRights(44);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx", false);
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


    protected void btnFillOffSheet_click(object sender, EventArgs e)
    {
        checkRights(101);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewFillOffSheet.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    //btnSubContractorInward_click
    protected void btnSubContractorInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewSubContractorIssue.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    //btnServiceInward_click
    protected void btnServiceInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewServiceInward.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    //btnCashInward_click
    protected void btnCashInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }

    }

    protected void btnForProcessInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }

    }
    protected void btnIssueFillOffSheet_click(object sender, EventArgs e)
    {
        checkRights(105);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssueFillOffSheet.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
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

    protected void btnIssueToProduction_click(object sender, EventArgs e)
    {
        checkRights(51);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnStockAdjustment_click(object sender, EventArgs e)
    {
        checkRights(60);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnCustomerRejection_click(object sender, EventArgs e)
    {
        checkRights(21);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    //btnDCRetrun_click

    protected void btnDCRetrun_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewDCReturn.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnTrayDCRetrun_click(object sender, EventArgs e)
    {
        checkRights(71);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTrayDCReturn.aspx", false);
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

    #region btnWithoutPOInward_click
    protected void btnWithoutPOInward_click(object sender, EventArgs e)
    {
        checkRights(39);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewMaterialInwardExcludeSPO.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    #endregion btnWithoutPOInward_click


    protected void btnTurningIWD_click(object sender, EventArgs e)
    {
        checkRights(115);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewTurningInward.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnInwardSuppWise_click(object sender, EventArgs e)
    {
        checkRights(47);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewInwardSuppWise.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnInspectionRegisterReport_click(object sender, EventArgs e)
    {
        checkRights(48);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewInspectionRegisterReport.aspx", false);
        }
        else
        {
            // Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnDispatchToSub_click(object sender, EventArgs e)
    {
        checkRights(21);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    #endregion

    #region Reports
    protected void btnBillOfMaterialRegister_Click(object sender, EventArgs e)
    {
        checkRights(45);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBillOfMaterialRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }


    protected void btnDispatchToSubReport_click(object sender, EventArgs e)
    {
        checkRights(21);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewDispatchtoSubcontractor.aspx", false);
        }
        else
        {
            Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnMaterialRequisitionRegister_Click(object sender, EventArgs e)
    {
        checkRights(55);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMaterialRequisitionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnIssueToProductionRegister_Click(object sender, EventArgs e)
    {
        checkRights(56);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewIssueToProductionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnProdcutionToStoreRegister_Click(object sender, EventArgs e)
    {
        checkRights(57);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewProdcutionToStoreRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnMaterialRequisitionMISReport_Click(object sender, EventArgs e)
    {
        checkRights(62);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewMaterialRequisition_MIS.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnStockAdjustmentRegister_Click(object sender, EventArgs e)
    {
        checkRights(61);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockAdjustmentRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnCustomerRejectionRegister_click(object sender, EventArgs e)
    {
        checkRights(53);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRejectionRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnSubContStockRegister_click(object sender, EventArgs e)
    {
        checkRights(53);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubContStockRegister.aspx", false);
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


    protected void btnVendorStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubContStockRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    protected void btnTrayStockLedger_click(object sender, EventArgs e)
    {
        checkRights(91);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/TrayStockReport.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }
    protected void btnDCRetrunRegister_Click(object sender, EventArgs e)
    {
        checkRights(78);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/DCReturnRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }



    protected void btnTurning_click(object sender, EventArgs e)
    {
        checkRights(114);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTurningRegisterReport.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    #endregion

    protected void btnCustStockReg_click(object sender, EventArgs e)
    {
        checkRights(122);
        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
        {
            Response.Redirect("~/RoportForms/VIEW/CustomerStockRegister.aspx", false);
        }
        else
        {
            //Response.Write("<script> alert('You Have No Rights To View.');</script>");
            ModalPopupMsg.Show();
            return;
        }
    }

    #endregion

    #region Methods

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #endregion
}
