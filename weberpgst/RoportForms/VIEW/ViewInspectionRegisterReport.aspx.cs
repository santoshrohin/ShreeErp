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

public partial class RoportForms_VIEW_ViewInspectionRegisterReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='22'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlItemName.Enabled = false;
            ddlItemCode.Enabled = false;
            // ddlUser.Enabled = false;
            ddlSupplierName.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            //chkAll.Checked = true;
            chkAllItemName.Checked = true;
            chkAllSupplierName.Checked = true;
            rbtGroup.SelectedIndex = 0;
            rbtType.SelectedIndex = 0;
            rbtReject.SelectedIndex = 0;

            // chkAllUser.Checked = true;
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (chkDateAll.Checked == false)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + "INSM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
            }
            else
            {

            }
        }

        CommonClasses.FillCombo("ITEM_MASTER,INSPECTION_S_MASTER,INSPECTION_S_DETAIL", "I_NAME", "I_CODE", "ITEM_MASTER.I_CODE = INSPECTION_S_DETAIL.INSD_I_CODE and INSPECTION_S_DETAIL.INSD_INSM_CODE = INSPECTION_S_MASTER.INSM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 ORDER BY I_NAME", ddlItemName);
        ddlItemName.Items.Insert(0, new ListItem("--Select Item--", "0"));

        CommonClasses.FillCombo("ITEM_MASTER,INSPECTION_S_MASTER,INSPECTION_S_DETAIL", "I_CODENO", "I_CODE", "ITEM_MASTER.I_CODE = INSPECTION_S_DETAIL.INSD_I_CODE and INSPECTION_S_DETAIL.INSD_INSM_CODE = INSPECTION_S_MASTER.INSM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 ORDER BY I_CODENO", ddlItemCode);
        ddlItemCode.Items.Insert(0,new ListItem("--Select Item Code--","0"));
    }
    #endregion

    private void LoadSupplier()
    {
        string str = "";
        if (chkDateAll.Checked == false)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + "INSM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
            }
            else
            {

            }
        }
        if (chkAllItemName.Checked == false)
        {
            if (ddlItemName.SelectedValue != "0")
            {
                str = str + "I_CODE=" + ddlItemName.SelectedValue + " and ";
            }
        }

        CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,ITEM_MASTER,INSPECTION_S_MASTER,INSPECTION_S_DETAIL", "P_NAME", "P_CODE", "" + str + "INSPECTION_S_MASTER.INSM_IWM_CODE = INWARD_MASTER.IWM_CODE and ITEM_MASTER.I_CODE = INSPECTION_S_DETAIL.INSD_I_CODE and INSPECTION_S_DETAIL.INSD_INSM_CODE = INSPECTION_S_MASTER.INSM_CODE and INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and P_TYPE=2 ORDER BY P_NAME", ddlSupplierName);
        ddlSupplierName.Items.Insert(0, new ListItem("--Select Supplier--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";

                    return;
                }
            }

            if (chkAllItemName.Checked == false)
            {
                if (ddlItemName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Item ";
                    ddlItemName.Focus();
                    return;
                }
            }
            if (chkAllSupplierName.Checked == false)
            {
                if (ddlSupplierName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Supplier ";
                    ddlSupplierName.Focus();
                    return;
                }
            }
            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";
            string str3 = "";
            string Condition = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date Must Be Equal or Smaller Than To Date";
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
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    Condition = Condition + "INSM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Date";
                    txtFromDate.Focus();
                    return;
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
                Condition = Condition + "INSM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ";
            }

            if (chkAllItemName.Checked == false)
            {
                if (ddlItemName.SelectedIndex != 0)
                {
                    Condition = Condition + "ITEM_MASTER.I_CODE='" + ddlItemName.SelectedValue + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Item Name";
                    ddlItemName.Focus();
                    return;
                }
            }
            if (chkAllSupplierName.Checked == false)
            {
                if (ddlSupplierName.SelectedIndex != 0)
                {
                    Condition = Condition + "PARTY_MASTER.P_CODE='" + ddlSupplierName.SelectedValue + "' and ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Supplier Name";
                    ddlSupplierName.Focus();
                    return;
                }
            }
            if (ddlPDINo.SelectedValue == "0")
            {
                Condition = Condition + "INSM_PDR_CHECK=0 and ";
            }
            if (ddlPDINo.SelectedValue == "1")
            {
                Condition = Condition + "INSM_PDR_CHECK=1 and ";
            }
            if (ddlPDINo.SelectedValue == "2")
            {
                //Condition = Condition + " ";
            }

            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Inspected";
                Condition = Condition + "INWARD_DETAIL.IWD_INSP_FLG=1 and ";
            }
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "pending";
                Condition = Condition + "INWARD_DETAIL.IWD_INSP_FLG=0 and ";
            }

            if (rbtReject.SelectedIndex == 0)
            {
                str2 = "all";
               // Condition = Condition + " ";
            }
            if (rbtReject.SelectedIndex == 1)
            {
                str2 = "rejected";
                Condition = Condition + " INSPECTION_S_MASTER.INSM_REJ_QTY>0 and ";
            }

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

            Response.Redirect("~/RoportForms/ADD/InspectionRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&inspected=" + str1 + "&showStatus=" + str2 + "&Cond=" + Condition + "", false);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Inspection Register Report", "btnShow_Click", Ex.Message);
        }
    }

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
            CommonClasses.SendError("Inspection Register Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    //protected void chkReject_CheckedChanged(object sender, EventArgs e)
    //{

    //}

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
            LoadCombos();
        }
    }

    protected void chkAllItemName_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItemName.Checked == true)
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
            ddlItemCode.Enabled = false;
            LoadSupplier();
        }
        else
        {
            LoadCombos();
            ddlItemCode.SelectedIndex = 0;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();

        }
    }

    protected void chkAllSupplierName_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllSupplierName.Checked == true)
        {
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = false;
        }
        else
        {
            LoadSupplier();
            ddlSupplierName.SelectedIndex = 0;
            ddlSupplierName.Enabled = true;
            ddlSupplierName.Focus();

        }
    }

    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtType.SelectedIndex == 1)
        {
            rbtReject.Items[1].Enabled = false;

        }
        else
        {
            rbtReject.Items[1].Enabled = true;
        }
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (chkAllSupplierName.Checked==false)
        {
            LoadSupplier();  
        }
       
    }

    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
        if (chkAllSupplierName.Checked == false)
        {
            LoadSupplier();
        }
    }

}
