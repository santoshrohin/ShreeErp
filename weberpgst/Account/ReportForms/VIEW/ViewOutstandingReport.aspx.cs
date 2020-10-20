using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class RoportForms_VIEW_ViewOutstandingReport : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='22'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            loadparty();
            ddlPname.Enabled = false;
            ddlPartyName.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");

            txtToDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy");
            txtToDate.Attributes.Add("readonly", "readonly");
            chkALLP.Checked = true;
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

    #region loadparty
    private void loadparty()
    {
        /*Load All Party fro Party_master*/
        DataTable dtparty = CommonClasses.Execute("SELECT  DISTINCT TALLY_OUT_PARTYNAME FROM  TALLY_OUTSTANDING   ORDER BY TALLY_OUT_PARTYNAME ");
        ddlPname.DataSource = dtparty;
        ddlPname.DataTextField = "TALLY_OUT_PARTYNAME";
        ddlPname.DataValueField = "TALLY_OUT_PARTYNAME";
        ddlPname.DataBind();
        ddlPname.Items.Insert(0, new ListItem("--Select Party--", "0"));
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

    #region chkALLP_CheckedChanged
    protected void chkALLP_CheckedChanged(object sender, EventArgs e)
    {
        if (chkALLP.Checked == true)
        {
            ddlPname.SelectedIndex = 0;
            ddlPname.Enabled = false;
        }
        else
        {
            ddlPname.SelectedIndex = 0;
            ddlPname.Enabled = true;
            ddlPname.Focus();
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


            string To = "";

            To = txtToDate.Text;
            string str = "";
            string CondInv = "";
            string CondiBill = "";
            string CondiCredit = "";
            string CondiDebit = "";
            string CondiAdvance = "";
            #region ChkDateRegion

            if (To != "")
            {

                DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                if (Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                    return;
                }

            }

            else
            {

                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                To = To2.ToString("dd/MMM/yyyy");
            }
            #endregion
            //chkDateAll.Checked == true && 
            #region Conditions

            if (chkAllParty.Checked == false)
            {
                CondiBill = CondiBill + " BPM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                CondInv = CondInv + " INM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                CondiCredit = CondiCredit + " CNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                CondiDebit = CondiDebit + " DNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                CondiAdvance = CondiAdvance + " PAYM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";


            }


            if (ddlType.SelectedValue == "0" && chkAllParty.Checked == true)
            {
                CondiBill = CondiBill + "BPM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                CondInv = CondInv + "INM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                CondiCredit = CondiCredit + "CNM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                CondiDebit = CondiDebit + "DNM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                CondiAdvance = CondiAdvance + " PAYM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
            }
            else if (ddlType.SelectedValue == "1" && chkAllParty.Checked == false)
            {
                if (ddlPartyName.SelectedIndex == 0 && chkAllParty.Checked == false)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Party Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPartyName.Focus();
                    return;
                }
                if (chkAllParty.Checked == false)
                {
                    CondiBill = CondiBill + "BPM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'and BPM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondInv = CondInv + "INM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and INM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";

                    CondiCredit = CondiCredit + "CNM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and CNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondiDebit = CondiDebit + "DNM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and DNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondiAdvance = CondiAdvance + "PAYM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and PAYM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                }
                else
                {
                    CondiBill = CondiBill + "BPM_DATE <= and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                    CondInv = CondInv + "INM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";

                    CondiCredit = CondiCredit + "CNM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                    CondiDebit = CondiDebit + "DNM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                    CondiAdvance = CondiAdvance + "PAYM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";

                }
            }
            else if (ddlType.SelectedValue == "2" && chkAllParty.Checked == false)
            {
                if (ddlPartyName.SelectedIndex == 0 && chkAllParty.Checked == false)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Party Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPartyName.Focus();
                    return;
                }

                if (chkAllParty.Checked == false)
                {
                    CondiBill = CondiBill + "BPM_DATE <= and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and BPM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondInv = CondInv + "INM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and INM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondiCredit = CondiCredit + "CNM_DATE <= and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and CNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondiDebit = CondiDebit + "DNM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and DNM_CUST_CODE='" + ddlPartyName.SelectedValue + "' and ";
                    CondiAdvance = CondiAdvance + "PAYM_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and PAYM_P_CODE='" + ddlPartyName.SelectedValue + "' and ";


                }
                else
                {
                    CondiBill = CondiBill + "BPM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                    CondInv = CondInv + "INM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                    CondiAdvance = CondiAdvance + "PAYM_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
                }//CondInv = CondInv + "INM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ";
            }


            if (ddlBillType.SelectedValue == "1")
            {
                CondiBill = CondiBill + "DATEDIFF(day,DATEADD(DAY,convert(int,P_CREDITDAYS),BPM_DATE),GETDATE())>0 and ";
                CondInv = CondInv + "DATEDIFF(day,DATEADD(DAY,convert(int,P_CREDITDAYS),INM_DATE),GETDATE())>0 and ";
                //CondiAdvance = CondiAdvance + "DATEDIFF(day,DATEADD(DAY,convert(int,P_CREDITDAYS),PAYM_DATE),GETDATE())>0 and ";
                //CondiCredit = CondiCredit + " DATEDIFF(day,DATEADD(DAY,convert(int,P_CREDITDAYS),CNM_DATE),GETDATE())>0  and ";
                //CondiDebit = CondiDebit + " DATEDIFF(day,DATEADD(DAY,convert(int,P_CREDITDAYS),DNM_DATE),GETDATE())>0  and ";


            }

            #endregion Conditions
            string strCondition = "";
            if (chkALLP.Checked == false)
            {
                if (ddlPname.SelectedValue == "0")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Party Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPname.Focus();
                    return;
                }
                else
                {
                    strCondition = strCondition + " TALLY_OUT_PARTYNAME='" + ddlPname.SelectedValue + "' AND ";
                }
            }
            strCondition = strCondition + "   TALLY_OUT_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyyy") + "'   ";

            str = rbtGroup.SelectedValue;
            Response.Redirect("~/Account/ReportForms/ADD/OutstandingReport.aspx?Title=" + Title + "&Condition=" + strCondition + "&Type=" + ddlBillType.SelectedValue + " ", false);


            //try
            //{


            //    DataTable dtResult = new DataTable();
            //    if (chkALLP.Checked == true)
            //    {
            //        dtResult = CommonClasses.Execute("SELECT  * FROM  TALLY_OUTSTANDING   ORDER BY TALLY_OUT_PARTYNAME ");


            //    }
            //    else
            //    {
            //        dtResult = CommonClasses.Execute("SELECT  * FROM  TALLY_OUTSTANDING where TALLY_OUT_PARTYNAME='" + ddlPname.SelectedValue + "'   ORDER BY TALLY_OUT_PARTYNAME ");

            //    }
            //    DataTable dtExport = new DataTable();
            //    if (dtResult.Rows.Count > 0)
            //    {
            //        dtExport.Columns.Add("Sr No");
            //        dtExport.Columns.Add("Ref No");
            //        dtExport.Columns.Add("Date");
            //        dtExport.Columns.Add("Party Name");
            //        dtExport.Columns.Add("Amount");
            //        dtExport.Columns.Add("Due Days");
            //        for (int i = 0; i < dtResult.Rows.Count; i++)
            //        {
            //            dtExport.Rows.Add(i + 1,
            //                              dtResult.Rows[i]["TALLY_OUT_REFNO"].ToString(),
            //                             Convert.ToDateTime(dtResult.Rows[i]["TALLY_OUT_DATE"].ToString()).ToString("dd/MM/yyyy"),
            //                              dtResult.Rows[i]["TALLY_OUT_PARTYNAME"].ToString(),
            //                              dtResult.Rows[i]["TALLY_OUT_AMT"].ToString(),
            //                              dtResult.Rows[i]["TALLY_OUT_DUEDAYS"].ToString()
            //                             );
            //        }
            //    }

            //    HttpContext.Current.Response.Clear();
            //    HttpContext.Current.Response.ClearContent();
            //    HttpContext.Current.Response.ClearHeaders();
            //    HttpContext.Current.Response.Buffer = true;
            //    HttpContext.Current.Response.ContentType = "application/ms-excel";
            //    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

            //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=OutstandingLedger.xls");

            //    HttpContext.Current.Response.Charset = "utf-8";
            //    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //    //sets font
            //    HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            //    HttpContext.Current.Response.Write("<BR><BR><BR>");
            //    HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
            //    "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
            //    "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //    //am getting my grid's column headers
            //    int columnscount = dtExport.Columns.Count;
            //    for (int j = 0; j < columnscount; j++)
            //    {      //write in new column
            //        HttpContext.Current.Response.Write("<Td>");
            //        //Get column headers  and make it as bold in excel columns
            //        HttpContext.Current.Response.Write("<B>");
            //        HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
            //        HttpContext.Current.Response.Write("</B>");
            //        HttpContext.Current.Response.Write("</Td>");
            //    }

            //    HttpContext.Current.Response.Write("</TR>");
            //    for (int k = 0; k < dtExport.Rows.Count; k++)
            //    {//write in new row

            //        HttpContext.Current.Response.Write("<TR>");
            //        for (int i = 0; i < dtExport.Columns.Count; i++)
            //        {
            //            if (i == dtExport.Columns.Count - 1)
            //            {
            //                HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
            //                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
            //                HttpContext.Current.Response.Write("</Td>");
            //            }
            //            else
            //            {

            //                HttpContext.Current.Response.Write("<Td>");

            //                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
            //                HttpContext.Current.Response.Write("</Td>");
            //            }
            //        }
            //        HttpContext.Current.Response.Write("</TR>");
            //    }



            //    HttpContext.Current.Response.Write("</Table>");
            //    HttpContext.Current.Response.Write("</font>");
            //    HttpContext.Current.Response.Flush();
            //    HttpContext.Current.Response.End();
            //}
            //catch (Exception)
            //{

            //}
        }
        catch (Exception Ex)
        {
            //CommonClasses.SendError("Outstanding Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "0")
        {
            LoadCombos();
        }
        else if (ddlType.SelectedValue == "1")
        {
            DataTable dtCustomer = new DataTable();
            dtCustomer = CommonClasses.Execute("SELECT distinct PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM ITEM_MASTER INNER JOIN INVOICE_DETAIL ON ITEM_MASTER.I_CODE = INVOICE_DETAIL.IND_I_CODE INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE where ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and P_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' order by P_NAME");
            ddlPartyName.DataSource = dtCustomer;
            ddlPartyName.DataTextField = "P_NAME";
            ddlPartyName.DataValueField = "P_CODE";
            ddlPartyName.DataBind();
            ddlPartyName.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        else
        {
            CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWD_IWM_CODE=IWM_CODE AND IWD_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0' AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME ", ddlPartyName);
            ddlPartyName.Items.Insert(0, new ListItem("--Select Supplier--", "0"));
        }
    }
    #endregion ddlType_SelectedIndexChanged
}