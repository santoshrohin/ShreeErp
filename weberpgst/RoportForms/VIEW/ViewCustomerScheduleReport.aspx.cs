using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewCustomerScheduleReport : System.Web.UI.Page
{

    static string right = "";
    string From = "";
    string To = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='116'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            txtMonth.Text = System.DateTime.Now.Date.ToString("MMM yyyy");
            txtMonth.Attributes.Add("readonly", "readonly");
            FillCombos();
            LoadCustomer();
            chkAllCust.Checked = true;
            ddlCustomer.Enabled = false;
            chkAllComp.Checked = true;
            ddlItemCode.Enabled = false;
        }
    }
    #endregion Page_Load


    #region FillCombos
    void FillCombos()
    {
        try
        {
            DataTable dtItem = new DataTable();
            if (chkAllCust.Checked == true)
            {
                dtItem = CommonClasses.Execute("SELECT I_NAME +' '+ I_CODENO as I_CODENO,I_CODE FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL,ITEM_MASTER where CUSTOMER_SCHEDULE.CS_CODE=CUSTOMER_SCHEDULE_DETAIL.CS_CODE AND CSD_I_CODE=I_CODE AND CUSTOMER_SCHEDULE.ES_DELETE=0   AND DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "'AND DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' ");

            }
            else
            {
                dtItem = CommonClasses.Execute("SELECT I_NAME +' '+ I_CODENO as I_CODENO,I_CODE FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL,ITEM_MASTER where CUSTOMER_SCHEDULE.CS_CODE=CUSTOMER_SCHEDULE_DETAIL.CS_CODE AND CSD_I_CODE=I_CODE AND CUSTOMER_SCHEDULE.ES_DELETE=0 AND CS_P_CODE='" + ddlCustomer.SelectedValue + "'   AND DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "'AND DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' ");

            }

            ddlItemCode.DataSource = dtItem;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception)
        {

        }
    }
    #endregion FillCombos

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                //ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region chkAllCust_CheckedChanged
    protected void chkAllCust_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCust.Checked == true)
        {
            FillCombos();
            ddlCustomer.SelectedIndex = 0;
            ddlCustomer.Enabled = false;

            //ddlItemName.SelectedIndex = 0;
            //ddlItemName.Enabled = false;
        }
        else
        {
            FillCombos();
            ddlCustomer.SelectedIndex = 0;
            ddlCustomer.Enabled = true;
            ddlCustomer.Focus();
            //ddlItemName.SelectedIndex = 0;
            //ddlItemName.Enabled = true;
            //ddlItemName.Focus();
        }
    }
    #endregion chkAllCust_CheckedChanged

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
            //ddlItemName.SelectedIndex = 0;
            //ddlItemName.Enabled = false;
        }
        else
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
            //ddlItemName.SelectedIndex = 0;
            //ddlItemName.Enabled = true;
            //ddlItemName.Focus();
        }
    }
    #endregion

    protected void chkAllItemName_CheckedChanged(object sender, EventArgs e)
    {
    }



    #region MyRegion
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
    #endregion btnCancel_Click


    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
        {
            try
            {
                From = txtMonth.Text.Trim();

                string str1 = "";
                if (chkAllCust.Checked == false)
                {
                    if (ddlCustomer.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer Name ";
                        ddlCustomer.Focus();
                        return;
                    }
                }

                if (chkAllComp.Checked == false)
                {
                    if (ddlItemCode.SelectedValue == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item Name ";
                        ddlItemCode.Focus();
                        return;
                    }


                }
                if (ddlCustomer.SelectedValue != "0")
                {
                    str1 = str1 + " CS_P_CODE =" + ddlCustomer.SelectedValue.ToString() + " AND ";
                }
                if (ddlItemCode.SelectedValue != "0")
                {
                    str1 = str1 + " CSD_I_CODE =" + ddlItemCode.SelectedValue.ToString() + " AND ";
                }
                //str1 = "Detail";
                Response.Redirect("~/RoportForms/ADD/CustomerSchedule.aspx?Title=" + Title + "&Date=" + From + "&cust=" + ddlCustomer.SelectedValue + "&Item=" + ddlItemCode.SelectedValue + "&type=" + rbtType.SelectedValue + "", false);
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("ItemSaleMonthWise", "btnShow_Click", Ex.Message);
            }
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedValue != "0")
            {
                FillCombos();
            }
        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError(" Customer Order Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,CUSTOMER_SCHEDULE CS where CS.CS_P_CODE=P_CODE AND DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "'AND DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' and CS.ES_DELETE=0 and CS.CS_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and P_TYPE='1' and P_ACTIVE_IND=1 ORDER BY P_NAME ASC");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadCustomer", Ex.Message);
        }
    }
    #endregion
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
}
