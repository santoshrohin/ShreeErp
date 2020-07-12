using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class RoportForms_VIEW_ViewAccountLedger : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='22'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlPartyName.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            chkAllParty.Checked = true;
        }
    }
    #endregion Page_Load

    #region LoadCombos
    private void LoadCombos()
    {
        /*Load All Party fro Party_master*/
        CommonClasses.FillCombo("PARTY_MASTER", "P_NAME", "P_CODE", "PARTY_MASTER.ES_DELETE='0' AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME ", ddlPartyName);
        ddlPartyName.Items.Insert(0, new ListItem("--Select Party--", "0"));
    }
    #endregion LoadCombos

    #region chkAllParty_CheckedChanged
    protected void chkAllParty_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllParty.Checked == true)
        {
            ddlPartyName.SelectedIndex = 0;
            ddlPartyName.Enabled = false;
        }
        else
        {
            ddlPartyName.SelectedIndex = 0;
            ddlPartyName.Enabled = true;
            ddlPartyName.Focus();
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
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
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
            CommonClasses.SendError("Outstanding Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validation
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    return;
                }
            }
            if (chkAllParty.Checked == false)
            {
                if (ddlPartyName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Party ";
                    ddlPartyName.Focus();
                    return;
                }
            }
            #endregion Validation

            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str = "";

            #region ChkDateRegion
            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        //PanelMsg.Visible = true;
                        //lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        //return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }
            #endregion

            string StrCondition = "";
            if (chkAllParty.Checked != true && chkDateAll.Checked != true)
                StrCondition = StrCondition + "P_CODE='" + ddlPartyName.SelectedValue + "' and ACCNT_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND ";
            else if (chkDateAll.Checked == false && chkAllParty.Checked == true)
                StrCondition = StrCondition + "ACCNT_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND ";
            else if (chkDateAll.Checked == true && chkAllParty.Checked == false)
                StrCondition = StrCondition + "P_CODE='" + ddlPartyName.SelectedValue + "' and ACCNT_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND ";
            Response.Redirect("~/Account/ReportForms/ADD/AccountLedger.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + Convert.ToDateTime(From.ToString()).ToString("dd/MMM/yyyy") + "&ToDate=" + Convert.ToDateTime(To.ToString()).ToString("dd/MMM/yyyy") + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&StrCondition=" + StrCondition + "", false);

            #region WiseRegion_Comment
            //if (rbtGroup.SelectedIndex == 0)
            //{
            //    str = "Datewise";

            //    if (chkDateAll.Checked == true && chkAllParty.Checked == true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked != true && chkAllParty.Checked != true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked != true && chkAllParty.Checked == true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked == true && chkAllParty.Checked != true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //}
            //else if (rbtGroup.SelectedIndex == 1)
            //{
            //    str = "CustWise";

            //    if (chkDateAll.Checked == true && chkAllParty.Checked == true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked != true && chkAllParty.Checked != true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked != true && chkAllParty.Checked == true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //    if (chkDateAll.Checked == true && chkAllParty.Checked != true)
            //    {
            //        Response.Redirect("../ADD/BillPassingRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllParty.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlPartyName.SelectedValue.ToString() + "&datewise=" + str + "", false);
            //    }
            //}
            #endregion

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Outstanding Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion
}