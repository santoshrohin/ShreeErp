using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_GSTCreditReport : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    string From = "";
    string To = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='123'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                ddlCustomerName.Enabled = false;
                chkAllComp.Checked = true;
                LoadCustomer();
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../../Default.aspx'; </script>");
            }
        }

    }

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable dt = new DataTable();

            if (chkDateAll.Checked == true)
            {
                dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND IWD_IWM_CODE=IWM_CODE AND PARTY_MASTER.ES_DELETE='0' AND IWD_MODVAT_FLG=0 AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by P_NAME", ddlCustomerName);
            }
            else
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND IWD_IWM_CODE=IWM_CODE AND IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND PARTY_MASTER.ES_DELETE='0' AND IWD_MODVAT_FLG=0 AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by P_NAME", ddlCustomerName);
                }
            }
            ddlCustomerName.Items.Insert(0, new ListItem("Select Party Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Contractor Stock Ledger", "LoadCustomer", Ex.Message);
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
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;

            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
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

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string i_code = "";
            string str1 = "";
            string strCondition = "";
            string ChkWithAmt = "";

            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    strCondition = strCondition + "IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                From = txtFromDate.Text;
                To = txtToDate.Text;

                strCondition = strCondition + "IWM_DATE between '" + From + "' AND '" + To + "' AND ";
            }

            if (chkAllComp.Checked != true)
            {
                if (ddlCustomerName.SelectedIndex != 0)
                {
                    strCondition = strCondition + " P_CODE= '" + ddlCustomerName.SelectedValue + "'  AND";
                    //i_code = ddlItemCode.SelectedValue.ToString();
                }
                else
                {
                    Response.Write("<script> alert('Please select Party name')</script>");
                    return;
                }
            }

            Response.Redirect("~/RoportForms/ADD/GSTCreditReport.aspx?Title=" + Title + "&Condition=" + strCondition + "&FromDate=" + From + "&ToDate=" + To + "&Party=" + ddlCustomerName.SelectedValue + "", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SubContractor Stock Ledger", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
}
