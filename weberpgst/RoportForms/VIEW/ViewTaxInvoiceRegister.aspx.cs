using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;

public partial class RoportForms_VIEW_ViewTaxInvoiceRegister : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //Menu highlight code
        //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        //home.Attributes["class"] = "active";
        //HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        //home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='25'");
            right = dtRights.Rows.Count.Equals(0) ? "0000000" : dtRights.Rows[0][0].ToString();

            //This code for disable calendar dates which is not in financial year
            //txtFromDate_CalenderExtender.StartDate = Convert.ToDateTime(Session["OpeningDate"]);
            //txtFromDate_CalenderExtender.EndDate = Convert.ToDateTime(Session["ClosingDate"]);
            //CalendarExtender1.StartDate = Convert.ToDateTime(Session["OpeningDate"]);
            //CalendarExtender1.EndDate = Convert.ToDateTime(Session["ClosingDate"]);

            LoadCombos();
            ddlCustomerName.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            ddlComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            chkAllComp.Checked = true;

            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            //txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy");
            //txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy");
        }
    }
    #endregion

    private void LoadItem()
    {
        string str = "";
        if (chkDateAll.Checked != true)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + "INM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
            }
        }

        DataTable dtItemDet = new DataTable();
        dtItemDet = CommonClasses.Execute("select distinct I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,INVOICE_MASTER,INVOICE_DETAIL WHERE " + str + " IND_INM_CODE=INM_CODE and I_CODE=IND_I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 ORDER BY I_NAME");

        ddlComponent.DataSource = dtItemDet;
        ddlComponent.DataTextField = "I_CODENO";
        ddlComponent.DataValueField = "I_CODE";
        ddlComponent.DataBind();
        ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

        ddlFinishedComponent.DataSource = dtItemDet;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "INM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }

            if (chkAllItem.Checked != true)
            {
                if (ddlFinishedComponent.SelectedIndex != 9)
                {
                    str = str + "I_CODE ='" + ddlFinishedComponent.SelectedValue + "' and ";
                }
            }
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("SELECT distinct PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM ITEM_MASTER INNER JOIN INVOICE_DETAIL ON ITEM_MASTER.I_CODE = INVOICE_DETAIL.IND_I_CODE INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE where " + str + " ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " order by P_NAME");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception)
        {
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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadCombos();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region chkType_CheckedChanged
    protected void chkType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkType.Checked == true)
        {
            ddltype.SelectedIndex = 0;
            ddltype.Enabled = false;
        }
        else
        {
            ddltype.SelectedIndex = 0;
            ddltype.Enabled = true;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            LoadItem();
            LoadCombos();
        }
        else
        {
            LoadItem();
            LoadCombos();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadItem();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            LoadCombos();
        }
        else
        {
            LoadItem();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            LoadItem();
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = false;
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            LoadItem();
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = true;
            ddlComponent.Focus();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region ddlComponent_SelectedIndexChanged
    protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlComponent.SelectedIndex != 0)
            {
                ddlFinishedComponent.SelectedValue = ddlComponent.SelectedValue;
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }

                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlCustomerName.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                string str1 = "";
                string str = "";

                #region Detail
                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                string strCondition = "";
                if (chkDateAll.Checked == true)
                {
                    strCondition = " AND INM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'   ";
                    From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                }
                else
                {
                    strCondition = " AND INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   ";
                }
                if (chkAllItem.Checked != true)
                {
                    strCondition = strCondition + " AND I_CODE= '" + ddlFinishedComponent.SelectedValue + "'";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + " AND INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + " AND INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                if (ddltype.SelectedValue == "0")
                {
                    strCondition = strCondition + " AND  INM_TYPE IN ( 'TAXINV', 'OutJWINM' ) ";
                }
                else if (ddltype.SelectedValue == "1")
                {
                    strCondition = strCondition + " AND INM_TYPE= 'TAXINV' AND INM_IS_SUPPLIMENT=1 ";
                }
                else if (ddltype.SelectedValue == "2")
                {
                    strCondition = strCondition + " AND INM_TYPE= 'TAXINV' AND INM_IS_SUPPLIMENT=0 ";
                }
                else if (ddltype.SelectedValue == "3")
                {
                    strCondition = strCondition + " AND INM_TYPE= 'OutJWINM' ";
                }
                else if (ddltype.SelectedValue == "4")
                {
                    strCondition = strCondition + " AND INM_TYPE= 'TAXINV' AND IND_AMORTAMT>0 ";
                }

                Response.Redirect("~/RoportForms/ADD/TaxInvoiceRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCondition + "&Type=SHOW", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlCustomerName.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                string str1 = "";
                string str = "";

                #region Detail

                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                string strCondition = "";
                if (chkDateAll.Checked == true)
                {
                    strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'   ";
                }
                else
                {
                    strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   ";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + "   AND  INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                if (chkAllItem.Checked != true)
                {
                    strCondition = strCondition + "   AND  IND_I_CODE= '" + ddlFinishedComponent.SelectedValue + "'";
                }
                if (ddltype.SelectedValue == "0")
                {
                    strCondition = strCondition + "   AND  INM_TYPE IN ( 'TAXINV', 'OutJWINM' )  ";
                }
                else if (ddltype.SelectedValue == "1")
                {
                    strCondition = strCondition + "   AND  INM_TYPE= 'TAXINV'  AND INM_IS_SUPPLIMENT=1 ";
                }
                else if (ddltype.SelectedValue == "2")
                {
                    strCondition = strCondition + "   AND  INM_TYPE= 'TAXINV'  AND INM_IS_SUPPLIMENT=0 ";
                }
                else if (ddltype.SelectedValue == "3")
                {
                    strCondition = strCondition + "   AND  INM_TYPE= 'OutJWINM'   ";
                }
                else if (ddltype.SelectedValue == "4")
                {
                    strCondition = strCondition + "   AND  INM_TYPE= 'TAXINV'  AND IND_AMORTAMT>0  ";
                }
                Response.Redirect("~/RoportForms/ADD/TaxInvoiceRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCondition + "&Type=EXPORT", false);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click1(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlCustomerName.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                string str1 = "";
                string str = "";

                #region Detail

                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "HSNWise";
                    }
                }
                #endregion

                string strCondition = "";
                if (chkDateAll.Checked == true)
                {
                    strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'   ";
                }
                else
                {
                    strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   ";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + "   AND  INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                if (chkAllItem.Checked != true)
                {
                    strCondition = strCondition + "   AND  IND_I_CODE= '" + ddlFinishedComponent.SelectedValue + "'";
                }



                strCondition = strCondition + "   AND  INM_TYPE= 'TAXINV'  AND INM_IS_SUPPLIMENT=1 ";


                Response.Redirect("~/RoportForms/ADD/TaxInvoiceRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCondition + "&Type=EXPORT1", false);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion


    #region btnExport_Click
    protected void btnExport_Click2(object sender, EventArgs e)
    {
        try
        {

            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;


            string strCondition = "";
            if (chkDateAll.Checked == true)
            {
                strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'   ";
            }
            else
            {
                strCondition = " AND  INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   ";
            }

            #region EXPORT

            DataTable dtResult = new DataTable();
            dtResult = CommonClasses.Execute(" SELECT DISTINCT P_NAME,P_LBT_NO,INM_TNO,INM_NO,INM_DATE,INM_ACCESSIBLE_AMT,MAX(E_TARIFF_NO) AS E_TARIFF_NO,MAX(E_H_EDU) AS E_H_EDU,SUM(IND_INQTY) as IND_INQTY,SUM(IND_AMT) as IND_AMT,INM_NET_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_G_AMT ,SM_NAME FROM INVOICE_MASTER,INVOICE_DETAIL,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,STATE_MASTER  WHERE INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV' AND INM_P_CODE=P_CODE AND IND_I_CODE=I_CODE AND I_E_CODE=E_CODE   " + strCondition + "    AND P_SM_CODE=SM_CODE GROUP BY P_NAME,P_LBT_NO,INM_TNO,INM_NO,INM_DATE,INM_ACCESSIBLE_AMT,INM_NET_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_G_AMT ,SM_NAME  ORDER BY INM_NO ");
            DataTable dtExport = new DataTable();
            if (dtResult.Rows.Count > 0)
            {
                dtExport.Columns.Add("Sr No");
                dtExport.Columns.Add("Customer Name");
                dtExport.Columns.Add("GSTIN No.");
                dtExport.Columns.Add("Invoice No.");
                dtExport.Columns.Add("Invoice Date");
                dtExport.Columns.Add("Shipping Bill No");
                dtExport.Columns.Add("Shipping Bill Date ");
                dtExport.Columns.Add("HSN / SAC No.");
                dtExport.Columns.Add("Qty");
                dtExport.Columns.Add("Gross Value");
                dtExport.Columns.Add("Taxable value");
                dtExport.Columns.Add("GST Rate");
                dtExport.Columns.Add("IGST");
                dtExport.Columns.Add("Central Tax");
                dtExport.Columns.Add("State Tax");
                dtExport.Columns.Add("State Name");
                dtExport.Columns.Add("Type Of Supply");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(i + 1,
                                      dtResult.Rows[i]["P_NAME"].ToString(),
                                      dtResult.Rows[i]["P_LBT_NO"].ToString(),
                                      dtResult.Rows[i]["INM_TNO"].ToString(),
                                      Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                      "", "",
                                      dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                                      dtResult.Rows[i]["IND_INQTY"].ToString(),
                                      dtResult.Rows[i]["INM_G_AMT"].ToString(),
                                      dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
                                      dtResult.Rows[i]["E_H_EDU"].ToString() + "%",
                                      dtResult.Rows[i]["INM_H_EDUC_AMT"].ToString(),
                                      dtResult.Rows[i]["INM_BE_AMT"].ToString(),
                                      dtResult.Rows[i]["INM_EDUC_AMT"].ToString(),
                                      dtResult.Rows[i]["SM_NAME"].ToString(),
                                      "INTRA"
                                      );
                }
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=TaxInvoice.xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
            "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
            "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = dtExport.Columns.Count;
            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }

            HttpContext.Current.Response.Write("</TR>");
            for (int k = 0; k < dtExport.Rows.Count; k++)
            {//write in new row

                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    if (i == dtExport.Columns.Count - 1)
                    {
                        HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    else
                    {
                        //if (i == 5)
                        //{
                        //    if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                        //    {
                        //        HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                        //    }
                        //    else
                        //    {
                        //        HttpContext.Current.Response.Write("<Td>");
                        //    }
                        //}
                        //else
                        //{
                        HttpContext.Current.Response.Write("<Td>");
                        //}
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            #endregion


        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
        if (ddlComponent.SelectedIndex != 0)
        {
            ddlFinishedComponent.SelectedValue = ddlComponent.SelectedValue;
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadItem();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadItem();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Focus();
                return;
            }
        }
    }
}
