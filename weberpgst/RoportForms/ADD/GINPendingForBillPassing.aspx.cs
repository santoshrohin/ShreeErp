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

public partial class RoportForms_ADD_GINPendingForBillPassing : System.Web.UI.Page
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
            string Condition = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string Party = Request.QueryString[4].ToString();
            string Type = Request.QueryString[5].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            if (Condition != "")
            {
                // Condition = "WHERE " + Condition;
                //string sub = Condition.Substring(0, Condition.Length-3);
                //Condition = sub;
            }

            GenerateReport(Title, Condition, From, To, Party, Type);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title, string Condition, string From, string To, string Party, string Type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();

        double srpRcvQty = 0;
        dt.Clear();
        // Check IWD_BILL_PASS_FLG flag for Bill passing is done or not
        dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME,P_NAME,IWM_NO,IWD_SQTY,IWD_RATE,convert(varchar,IWM_DATE,106) as IWM_DATE,SPOM_PONO,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE from ITEM_MASTER,SUPP_PO_MASTER,PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where " + Condition + "  IWM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND PARTY_MASTER.ES_DELETE=0 AND I_ACTIVE_IND=1 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE and I_CODE=IWD_I_CODE and INWARD_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND P_ACTIVE_IND=1  AND IWM_TYPE IN ( 'IWIM','OUTCUSTINV') AND IWD_BILL_PASS_FLG = 0 AND  IWD_INSP_FLG=1 AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=IWD_CPOM_CODE AND P_TYPE=2  UNION  SELECT I_CODE,I_CODENO,I_NAME,P_NAME,CR_GIN_NO ,CD_RECEIVED_QTY ,CD_RATE,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CPOM_PONO,CR_CHALLAN_NO,convert(varchar,CR_CHALLAN_NO,106) as IWM_CHAL_DATE FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER,ITEM_MASTER,CUSTPO_MASTER,PARTY_MASTER  where " + Condition + " CR_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND P_CODE=CR_P_CODE AND CD_I_CODE=I_CODE AND CD_PO_CODE=CPOM_CODE  AND CD_MODVAT_FLG=0  UNION select I_CODE,I_CODENO,I_NAME,P_NAME,IWM_NO,IWD_SQTY,IWD_RATE,convert(varchar,IWM_DATE,106) as IWM_DATE,'0' AS SPOM_PONO,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE from ITEM_MASTER,PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where  " + Condition + "   IWM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND  PARTY_MASTER.ES_DELETE=0 AND I_ACTIVE_IND=1 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE and I_CODE=IWD_I_CODE and INWARD_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND P_ACTIVE_IND=1  AND IWM_TYPE IN ('Without PO inward') AND IWD_BILL_PASS_FLG = 0 AND  IWD_INSP_FLG=1 AND P_TYPE=2");

        //Query done without Item if required
        //dt = CommonClasses.Execute("select distinct P_NAME,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,SPOM_PONO from SUPP_PO_MASTER,PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where  PARTY_MASTER.ES_DELETE=0 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE and INWARD_MASTER.ES_DELETE=0 AND P_ACTIVE_IND=1 AND IWD_MODVAT_FLG=0 AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=IWD_CPOM_CODE");
        if (dt.Rows.Count > 0)
        {
            if (Type.Trim().ToUpper() == "SHOW")
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptPendingGINForCredit.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptPendingGINForCredit.rpt");
                rptname.Refresh();

                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                rptname.SetParameterValue("Title", "Pending GIN For Bill Passing");
                rptname.SetParameterValue("Date", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {

                DataTable dtResult = new DataTable();
                dtResult = dt;


                DataTable dtExport = new DataTable();
                if (dtResult.Rows.Count > 0)
                {
                    dtExport.Columns.Add("Sr No");
                    dtExport.Columns.Add("INWARD NO.");
                    dtExport.Columns.Add("INWARD DATE");
                    dtExport.Columns.Add("PARTY NAME");
                    dtExport.Columns.Add("PO NO");
                    dtExport.Columns.Add("PART NO");
                    dtExport.Columns.Add("DESCRIPTION");
                    dtExport.Columns.Add("CHALLAN NO");
                    dtExport.Columns.Add("CHALLAN DATE");
                    dtExport.Columns.Add("AMOUNT");

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtExport.Rows.Add(i + 1,
                                          dtResult.Rows[i]["IWM_NO"].ToString(),
                                          Convert.ToDateTime(dtResult.Rows[i]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy"),
                                          dtResult.Rows[i]["P_NAME"].ToString(),
                                          dtResult.Rows[i]["SPOM_PONO"].ToString(),
                                          dtResult.Rows[i]["I_CODENO"].ToString(),
                                          dtResult.Rows[i]["I_NAME"].ToString(),
                                          dtResult.Rows[i]["IWM_CHALLAN_NO"].ToString(),
                                          dtResult.Rows[i]["IWM_CHAL_DATE"].ToString(),
                                          Math.Round(Convert.ToDouble(dtResult.Rows[i]["IWD_SQTY"].ToString()) * Convert.ToDouble(dtResult.Rows[i]["IWD_RATE"].ToString()))
                                         );
                    }
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=PendingForBillPassing.xls");

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
                            HttpContext.Current.Response.Write("<Td>");
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
            lblmsg.Text = "Record Not Found";
            return;
        }
    }

    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/GINPendingForBillPassing.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
