using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewInvoiceReport : System.Web.UI.Page
{
    #region Variables
    string Title = "";
    string Cond = "";
    string invoice_code = "";
    string reportType = "";
    string chkPrint1 = "";
    string supp = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT MAX(INM_NO) AS  INM_NO FROM INVOICE_MASTER where INM_TYPE='TAXINV' and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND    INVOICE_MASTER.ES_DELETE=0");
            txtToNo.Text = dt.Rows[0]["INM_NO"].ToString();
        }
    }
    #endregion Page_Load

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }
    #endregion btnCancel_Click

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        Title = Request.QueryString[0];
        Cond = Request.QueryString[1];
        invoice_code = Request.QueryString[3];
        reportType = Request.QueryString[4];
        supp = Request.QueryString[5];
        chkPrint1 = Convert.ToInt32(Request.QueryString[2]).ToString();
    }
    #endregion #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        //string inv_code = e.CommandArgument.ToString();
        string inv_code = "0";
        string type = "Mult";
        string FromNo = txtFrom.Text;
        string toNo = txtToNo.Text;
        //Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?inv_code=" + FromNo + "&type=" + toNo, false);
        Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + Cond + "&chkPrint1=" + chkPrint1 + "&code=" + FromNo + "&type=" + type + "&toNo=" + toNo + "&Supp=" + supp + "", false);
    }
    #endregion btnShow_Click

}
