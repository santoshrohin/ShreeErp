using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_GINPendingForBillPassing : System.Web.UI.Page
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
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='124'");
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
                dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND IWD_IWM_CODE=IWM_CODE AND PARTY_MASTER.ES_DELETE='0' AND IWD_INSP_FLG=1 AND IWM_TYPE IN ('Without PO inward','IWIM','OUTCUSTINV') AND IWD_BILL_PASS_FLG=0 AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by P_NAME", ddlCustomerName);
            }
            else
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND IWD_IWM_CODE=IWM_CODE AND IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND PARTY_MASTER.ES_DELETE='0' AND IWD_INSP_FLG=1 AND IWM_TYPE IN ('Without PO inward','IWIM','OUTCUSTINV')  and IWD_BILL_PASS_FLG=0 AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by P_NAME", ddlCustomerName);
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
        LoadCustomer();
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
                    //strCondition = strCondition + "IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                From = txtFromDate.Text;
                To = txtToDate.Text;

                //  strCondition = strCondition + "IWM_DATE between '" + From + "' AND '" + To + "' AND ";
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
                    Response.Write("<script> alert('Please select Party name'); </script>");
                    return;
                }
            }
            string type = "SHOW";
            Response.Redirect("~/RoportForms/ADD/GINPendingForBillPassing.aspx?Title=" + Title + "&Condition=" + strCondition + "&FromDate=" + From + "&ToDate=" + To + "&Party=" + ddlCustomerName.SelectedValue + "&type=" + type + "", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SubContractor Stock Ledger", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport1_Click
    protected void btnExport1_Click(object sender, EventArgs e)
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
                }
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                From = txtFromDate.Text;
                To = txtToDate.Text;

                //  strCondition = strCondition + "IWM_DATE between '" + From + "' AND '" + To + "' AND ";
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
                    Response.Write("<script> alert('Please select Party name'); </script>");
                    return;
                }
            }
            string type = "Export";
            Response.Redirect("~/RoportForms/ADD/GINPendingForBillPassing.aspx?Title=" + Title + "&Condition=" + strCondition + "&FromDate=" + From + "&ToDate=" + To + "&Party=" + ddlCustomerName.SelectedValue + "&type=" + type + "", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SubContractor Stock Ledger", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
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
                    //strCondition = strCondition + "IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                From = txtFromDate.Text;
                To = txtToDate.Text;

                //  strCondition = strCondition + "IWM_DATE between '" + From + "' AND '" + To + "' AND ";
            }


            #region Export

            DataTable dtResult = new DataTable();
            dtResult = CommonClasses.Execute("SELECT BPM_NO,BPM_DATE,BPM_INV_NO,BPM_INV_DATE,BPM_TAXABLE_AMT,BPM_BASIC_AMT,	BPM_EXCIES_AMT	,BPM_ECESS_AMT,	BPM_HECESS_AMT,BPM_G_AMT,P_NAME,P_GST_NO,P_LBT_NO, MAX(I_CAT_NAME) AS I_CAT_NAME,MAX(EXCISE_TARIFF_MASTER.E_TARIFF_NO) AS E_COMMODITY,MAX(SERVICE.E_TARIFF_NO) AS E_SERVICE, SUM(BPD_RECD_QTY) AS BPD_RECD_QTY,MAX(I_UOM_NAME)  AS I_UOM_NAME  FROM BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER,EXCISE_TARIFF_MASTER,EXCISE_TARIFF_MASTER AS SERVICE  where BPM_CODE=BPD_BPM_CODE AND BILL_PASSING_MASTER.ES_DELETE=0 AND BPM_P_CODE=P_CODE AND BPD_I_CODE=I_CODE AND ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE  AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND I_E_CODE=EXCISE_TARIFF_MASTER.E_CODE  AND I_SCAT_CODE=SERVICE.E_CODE  AND  BPM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString() + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString() + "'   GROUP BY BPM_NO,BPM_DATE,BPM_INV_NO,BPM_INV_DATE,BPM_TAXABLE_AMT,BPM_BASIC_AMT,	BPM_EXCIES_AMT	,BPM_ECESS_AMT,	BPM_HECESS_AMT,BPM_G_AMT,P_NAME,P_GST_NO,P_LBT_NO");

            DataTable dtExport = new DataTable();
            if (dtResult.Rows.Count > 0)
            {
                dtExport.Columns.Add("Sr No");
                dtExport.Columns.Add("Invoice No.");
                dtExport.Columns.Add("Invoice Date");
                dtExport.Columns.Add("Type of Inward Supllier");
                dtExport.Columns.Add("Name of the Supllier");
                dtExport.Columns.Add("GSTIN No. of the Supllier");
                dtExport.Columns.Add("Date of Material Received");
                dtExport.Columns.Add("Net Value of Tax Invoice");
                dtExport.Columns.Add("CGST (Central Tax)");
                dtExport.Columns.Add("SGST (State Tax)");
                dtExport.Columns.Add("IGST (Integrated Tax)");
                dtExport.Columns.Add("Gross Total of Tax Invoice");
                dtExport.Columns.Add("Description of Goods / Services");
                dtExport.Columns.Add("HSN Code (Goods)");
                dtExport.Columns.Add("SAC Code (Service)");
                dtExport.Columns.Add("Qty Received");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(i + 1,
                                      dtResult.Rows[i]["BPM_INV_NO"].ToString(),
                                     Convert.ToDateTime(dtResult.Rows[i]["BPM_INV_DATE"].ToString()).ToString("dd/MMM/yyyy"),
                                     "",
                                      dtResult.Rows[i]["P_NAME"].ToString(),
                                      dtResult.Rows[i]["P_LBT_NO"].ToString(),
                                      "",
                                      dtResult.Rows[i]["BPM_TAXABLE_AMT"].ToString(),
                                      dtResult.Rows[i]["BPM_EXCIES_AMT"].ToString(),
                                      dtResult.Rows[i]["BPM_ECESS_AMT"].ToString(),
                                       dtResult.Rows[i]["BPM_HECESS_AMT"].ToString(),
                                       dtResult.Rows[i]["BPM_G_AMT"].ToString(),
                                        dtResult.Rows[i]["I_CAT_NAME"].ToString(),
                                         dtResult.Rows[i]["E_COMMODITY"].ToString(),
                                          dtResult.Rows[i]["E_SERVICE"].ToString(),
                                           dtResult.Rows[i]["BPD_RECD_QTY"].ToString() + " " + dtResult.Rows[i]["I_UOM_NAME"].ToString()
                                     );
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=BillPassing.xls");

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
                        // }
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
            //Response.Redirect("~/RoportForms/ADD/GINPendingForBillPassing.aspx?Title=" + Title + "&Condition=" + strCondition + "&FromDate=" + From + "&ToDate=" + To + "&Party=" + ddlCustomerName.SelectedValue + "", false);

        }
        catch (Exception Ex)
        {
            // CommonClasses.SendError("SubContractor Stock Ledger", "btnShow_Click", Ex.Message);
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
