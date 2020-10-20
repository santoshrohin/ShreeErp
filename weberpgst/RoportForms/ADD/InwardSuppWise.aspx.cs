using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;


public partial class RoportForms_ADD_InwardSuppWise : System.Web.UI.Page
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
            //bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            //bool ChkAllUser = Convert.ToBoolean(Request.QueryString[4].ToString());

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //int p_name = Convert.ToInt32(Request.QueryString[6].ToString());
            //int i_name = Convert.ToInt32(Request.QueryString[7].ToString());
            //string u_name = Request.QueryString[9].ToString();

            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();
            //i_name = i_name.Replace("'", "''");
            string Type = Request.QueryString[6].ToString();
            string POType = Request.QueryString[7].ToString();
            string POTypeMaster = Request.QueryString[8].ToString();
            string ShowType = Request.QueryString[9].ToString();
            #region MyRegion

            #endregion

            GenerateReport(Title, From, To, group, way, Cond, Type.Trim().ToString(), POType, POTypeMaster, ShowType);
        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string condition, string Type, string POType, string POTypeMaster, string ShowType)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "SELECT distinct IWM_NO,I_NAME,IWM_DATE, LM_NAME,UOM_NAME,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWD_REV_QTY,IWD_RATE,SIT_NAME FROM UNIT_MASTER,LEDGER_MASTER,ITEM_MASTER ,INWARD_MASTER,INWARD_DETAIL,SITE_MASTER where IWM_CODE=IWD_IWM_CODE and I_CODE=IWD_I_CODE AND UOM_CODE=IWD_UOM_CODE and IWM_P_CODE=LM_CODE AND IWM_SITE_CODE=SIT_CODE AND INWARD_MASTER.ES_DELETE=0";
            //old Query = "SELECT distinct IWM_NO,I_CODENO,I_NAME,IWM_DATE, P_NAME as LM_NAME,I_UOM_NAME as UOM_NAME,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWD_REV_QTY,IWD_RATE,SPOM_PO_NO FROM ITEM_UNIT_MASTER,PARTY_MASTER,ITEM_MASTER ,INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER where  " + condition + "   IWM_CODE=IWD_IWM_CODE and I_CODE=IWD_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=IWD_UOM_CODE and IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND INWARD_MASTER.ES_DELETE=0 and SPOM_CODE=IWD_CPOM_CODE and SUPP_PO_MASTER.ES_DELETE=0";
            //Query = "SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,INWARD_DETAIL.IWD_RATE, SUPP_PO_MASTER.SPOM_PO_NO, PO_TYPE_MASTER.PO_T_SHORT_NAME FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE WHERE " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) and (PO_TYPE_MASTER.ES_DELETE=0)";

            if (POType == "SUPP")
            {
                if (POTypeMaster != "0")
                {
                    condition = condition + " PO_T_CODE='" + POTypeMaster + "' AND ";
                }
                Query = "SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, SPOM_PO_NO, PO_TYPE_MASTER.PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE INNER JOIN PROJECT_CODE_MASTER ON SUPP_PO_MASTER.SPOM_PROJECT = PROJECT_CODE_MASTER.PROCM_CODE WHERE " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) and (PO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0)";

            }
            else if (POType == "CUST")
            {
                Query = "SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, CPOM_PONO AS SPOM_PO_NO, SO_T_SHORT_NAME AS PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN CUSTPO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE INNER JOIN SO_TYPE_MASTER ON SO_TYPE_MASTER.SO_T_CODE = CUSTPO_MASTER.CPOM_TYPE INNER JOIN PROJECT_CODE_MASTER ON CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE WHERE     " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) and (SO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0)";

            }
            else if (POType == "ALL")
            {
                string condition1 = "";
                if (POTypeMaster != "0")
                {
                    condition = condition + " PO_T_CODE='" + POTypeMaster + "' AND ";
                }
                Query = "SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, convert(varchar,SPOM_PO_NO) AS SPOM_PO_NO, PO_TYPE_MASTER.PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE INNER JOIN PROJECT_CODE_MASTER ON SUPP_PO_MASTER.SPOM_PROJECT = PROJECT_CODE_MASTER.PROCM_CODE WHERE " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) and (PO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0)  AND IWM_TYPE  IN ( 'IWIM','OUTCUSTINV')  UNION SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, CPOM_PONO AS SPOM_PO_NO, SO_T_SHORT_NAME AS PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN CUSTPO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE INNER JOIN SO_TYPE_MASTER ON SO_TYPE_MASTER.SO_T_CODE = CUSTPO_MASTER.CPOM_TYPE INNER JOIN PROJECT_CODE_MASTER ON CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE WHERE     " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) and (SO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0) AND IWM_TYPE = 'IWIFP'  UNION  SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, '0' AS SPOM_PO_NO, '0' AS PO_T_SHORT_NAME, '0'  as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE  WHERE   " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ")  AND (INWARD_MASTER.ES_DELETE = 0)  AND IWM_TYPE = 'Without PO inward' ";
            }
            else
            {
                Query = "SELECT DISTINCT INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, INWARD_DETAIL.IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, 0 AS SPOM_PO_NO, 0 AS PO_T_SHORT_NAME, 0  as PRO_CODE,IWD_REMARK  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE  WHERE   " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ")  AND (INWARD_MASTER.ES_DELETE = 0) ";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (ShowType.ToUpper() == "S")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    #region Datewise
                    if (group == "Datewise")
                    {

                        if (way == "Summary")
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "DateWise " + Type + " Summary Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Summary Report");

                        }
                        else
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "DateWise " + Type + " Detail Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Detail Report");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                        CrystalReportViewer1.ReportSource = rptname;

                    }
                    #endregion

                    #region ItemWise
                    if (group == "ItemWise")
                    {

                        if (way == "Summary")
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "Itemwise " + Type + " Summery Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Summary Report");

                        }
                        else
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "Itemwise " + Type + " Detail Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Detail Report");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                        CrystalReportViewer1.ReportSource = rptname;

                    }

                    #endregion

                    #region SupplierWise
                    if (group == "CustWise")
                    {

                        if (way == "Summary")
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "Supplierwise " + Type + " Summery Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Summary Report");
                        }
                        else
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt"));
                            rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt");
                            rptname.Refresh();
                            rptname.SetDataSource(dt);
                            rptname.SetParameterValue("txtType", "Supplierwise " + Type + " Detail Report");
                            rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                            //rptname.SetParameterValue("txtToDate", To);
                            rptname.SetParameterValue("txtReportType", "Detail Report");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                        CrystalReportViewer1.ReportSource = rptname;

                    }
                    #endregion
                }
                else
                {

                    DataTable dtResult = new DataTable();

                    dtResult = dt;
                    DataTable dtExport = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtExport.Columns.Add("Gin No");
                        dtExport.Columns.Add("Gin Date");
                        dtExport.Columns.Add("Po No");
                        dtExport.Columns.Add("Po Type");
                        dtExport.Columns.Add("Project Code");
                        dtExport.Columns.Add("Supplier Name");
                        dtExport.Columns.Add("Item Code");
                        dtExport.Columns.Add("Item Name");
                        dtExport.Columns.Add("Ch.No");
                        dtExport.Columns.Add("Ch.Date");
                        dtExport.Columns.Add("Unit");
                        dtExport.Columns.Add("Rec.Qty");
                        dtExport.Columns.Add("Rate");
                        dtExport.Columns.Add("Amount");
                        dtExport.Columns.Add("Remark");


                        for (int i = 0; i < dtResult.Rows.Count; i++)
                        {
                            dtExport.Rows.Add(dtResult.Rows[i]["IWM_NO"].ToString(),
                                Convert.ToDateTime(dtResult.Rows[i]["IWM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                dtResult.Rows[i]["SPOM_PO_NO"].ToString(), dtResult.Rows[i]["PO_T_SHORT_NAME"].ToString(), dtResult.Rows[i]["PRO_CODE"].ToString(),
  dtResult.Rows[i]["LM_NAME"].ToString(), dtResult.Rows[i]["I_CODENO"].ToString(), dtResult.Rows[i]["I_NAME"].ToString(),

                                              dtResult.Rows[i]["IWM_CHALLAN_NO"].ToString(),
                                                Convert.ToDateTime(dtResult.Rows[i]["IWM_CHAL_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                              dtResult.Rows[i]["UOM_NAME"].ToString(),
                                              Math.Round((Convert.ToDouble(dtResult.Rows[i]["IWD_REV_QTY"].ToString())),3),
                                              dtResult.Rows[i]["IWD_RATE"].ToString(),
                                              Math.Round(Math.Round(Convert.ToDouble(dtResult.Rows[i]["IWD_REV_QTY"].ToString()),3)*Convert.ToDouble(dtResult.Rows[i]["IWD_RATE"].ToString()),2),
                                              dtResult.Rows[i]["IWD_REMARK"].ToString()

                                             );
                        }

                    }


                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=MaterialInwardRegister.xls");

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

                                if (i == 4)
                                {
                                    if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                                    {
                                        HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                                    }
                                    else
                                    {
                                        HttpContext.Current.Response.Write("<Td>");
                                    }
                                }
                                else
                                {
                                    HttpContext.Current.Response.Write("<Td>");
                                }
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
                }

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Inward Supp Wise", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/RoportForms/VIEW/ViewInwardSuppWise.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supplierwise", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

}
