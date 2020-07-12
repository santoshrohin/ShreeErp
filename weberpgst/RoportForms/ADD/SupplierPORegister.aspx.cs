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
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;


public partial class RoportForms_ADD_SupplierPORegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {


    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            //  bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            // bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            // bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            // bool ChkAllUser = Convert.ToBoolean(Request.QueryString[4].ToString());

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            // int p_name = Convert.ToInt32(Request.QueryString[6].ToString());
            // int i_name = Convert.ToInt32(Request.QueryString[7].ToString());
            //string u_name = Request.QueryString[9].ToString();

            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, way, Cond);

            //i_name = i_name.Replace("'", "''");


            #region Detail
            if (way == "All")
            {
                if (group == "Datewise")
                {


                }
                if (group == "ItemWise")
                {


                }
                if (group == "CustWise")
                {


                }
            }
            #endregion

            #region Summary
            if (way == "Pending")
            {
                if (group == "Datewise")
                {
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                }
                if (group == "ItemWise")
                {
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                }
                if (group == "CustWise")
                {
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    //{
                    //    GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    //{
                    //    GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    //}
                    //if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    //{
                    //    GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    //}
                }

            }
            #endregion
        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string condition)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,PARTY_MASTER.P_NAME,SUPP_PO_DETAILS.SPOD_ORDER_QTY,SUPP_PO_DETAILS.SPOD_RATE, ISNULL(SUPP_PO_DETAILS.SPOD_INW_QTY, 0.00) AS SPOM_INW_QTY, SUPP_PO_MASTER.SPOM_PO_NO,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, SUPP_PO_MASTER.SPOM_DATE, PO_TYPE_MASTER.PO_T_SHORT_NAME,PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE FROM PARTY_MASTER INNER JOIN SUPP_PO_MASTER ON PARTY_MASTER.P_CODE = SUPP_PO_MASTER.SPOM_P_CODE INNER JOIN ITEM_MASTER INNER JOIN SUPP_PO_DETAILS ON ITEM_MASTER.I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN ITEM_UNIT_MASTER ON SUPP_PO_DETAILS.SPOD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE INNER JOIN PROJECT_CODE_MASTER ON SUPP_PO_MASTER.SPOM_PROJECT = PROJECT_CODE_MASTER.PROCM_CODE WHERE " + condition + " (SUPP_PO_MASTER.SPOM_CM_CODE = " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (SUPP_PO_MASTER.ES_DELETE = 0) and (PO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0) ";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPODateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPODateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Pending")
                    {
                        rptname.SetParameterValue("txtType", group + " PO Pending Report");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", group + " PO All Report ");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPOItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPOItemWise.rpt");


                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    
                    if (way == "Pending")
                    {
                        rptname.SetParameterValue("txtType", group + "  PO Pending Report");


                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", group + " PO All Report");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPOSuppWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPOSuppWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", " SupplierWIse PO Pending Report");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "SupplierWIse PO All Report");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;

                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoRegister.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }
    }
}
