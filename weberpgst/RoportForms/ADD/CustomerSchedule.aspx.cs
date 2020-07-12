using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_CustomerSchedule : System.Web.UI.Page
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
            string Month = Request.QueryString[1].ToString();
            string Cust = Request.QueryString[2].ToString();
            string item = Request.QueryString[3].ToString();
            string type = Request.QueryString[4].ToString();

            GenerateReport(Title, Month, Cust, item, type);


            //GenerateReport(Title, "All");

        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Month, string To, string item, string type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        string str1 = "", str = "";
        if (To != "0")
        {
            str1 = str1 + " P_CODE =" + To + " AND ";
            str = str + " P_CODE =" + To + " AND ";
        }
        if (item != "0")
        {
            str1 = str1 + " I_CODE =" + item + " AND ";
            str = str + " I_CODE =" + item + " AND ";
        }
        if (type == "2")
        {
            Query = "SELECT * into #temp from ( SELECT  1 AS aabc,CS_MONTH,CUSTOMER_SCHEDULE.CS_CODE ,CS_P_CODE,CS_NO,CSD_I_CODE,CSD_QTY,I_CODENO,I_NAME,P_NAME ,0 AS INM_NO,'0' AS INM_TNO,0 as IND_INQTY , GETDATE() AS INM_DATE    FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL,ITEM_MASTER,PARTY_MASTER where CUSTOMER_SCHEDULE.CS_CODE=CUSTOMER_SCHEDULE_DETAIL.CSD_CS_CODE AND CSD_I_CODE=I_CODE AND CS_P_CODE=P_CODE  AND  " + str1 + " DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(Month).Month + "' AND DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(Month).Year + "' UNION SELECT  2 AS aabc, INM_DATE AS CS_MONTH, INM_CODE AS CS_CODE, INM_P_CODE as CS_P_CODE,0 AS CS_NO , IND_I_CODE AS CSD_I_CODE,0 aS CSD_QTY, I_CODENO,  I_NAME,  P_NAME,INM_NO,INM_TNO,IND_INQTY ,   INM_DATE  FROM INVOICE_MASTER,INVOICE_DETAIL,PARTY_MASTER,ITEM_MASTER where INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND  INM_P_CODE=P_CODE AND IND_I_CODE=I_CODE  AND  " + str + "  DATEPART(MM,INM_DATE)='" + Convert.ToDateTime(Month).Month + "' AND DATEPART(YYYY,INM_DATE)='" + Convert.ToDateTime(Month).Year + "') xyz  SELECT * FROM #temp ORDER BY CS_P_CODE,CSD_I_CODE,aabc DROP TABLE #temp";
        }
        else
        {
            Query = " SELECT CS_MONTH,CUSTOMER_SCHEDULE.CS_CODE ,CS_P_CODE,CS_NO,CS_TOTAL_SCHEDULE,CSD_I_CODE,CSD_QTY,I_CODENO,I_NAME,P_NAME,   ISNULL((SELECT SUM(IND_INQTY) FROM INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE AND INM_TYPE='TAXINV' AND IND_I_CODE=CSD_I_CODE AND INM_P_CODE=CS_P_CODE AND   DATEPART(MM,INM_DATE)='" + Convert.ToDateTime(Month).Month + "' AND DATEPART(YYYY,INM_DATE)='" + Convert.ToDateTime(Month).Year + "'  AND INM_IS_SUPPLIMENT=0) ,0) AS INV_QTY ,  ISNULL((SELECT TOP(1) CPOD_RATE FROM CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=CSD_I_CODE AND CPOM_P_CODE=CS_P_CODE  ) ,0) AS CPOD_RATE    FROM CUSTOMER_SCHEDULE,CUSTOMER_SCHEDULE_DETAIL,ITEM_MASTER,PARTY_MASTER where  " + str1 + " CUSTOMER_SCHEDULE.CS_CODE=CUSTOMER_SCHEDULE_DETAIL.CSD_CS_CODE AND CSD_I_CODE=I_CODE AND CS_P_CODE=P_CODE AND  DATEPART(MM,CS_MONTH)='" + Convert.ToDateTime(Month).Month + "' AND DATEPART(YYYY,CS_MONTH)='" + Convert.ToDateTime(Month).Year + "'";
        }


        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {


            ReportDocument rptname = null;
            rptname = new ReportDocument();
            // rptCustSchedule2
            if (type == "0")
            {
                rptname.Load(Server.MapPath("~/Reports/rptCustSchedule.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustSchedule.rpt");
            }
            if (type == "1")
            {
                rptname.Load(Server.MapPath("~/Reports/rptCustSchedule1.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustSchedule1.rpt");
            }
            if (type == "2")
            {
                rptname.Load(Server.MapPath("~/Reports/rptCustSchedule2.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustSchedule2.rpt");
            }
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

            rptname.SetParameterValue("txtPeriod", "SCHEDULE VS SALE for the Month  of " + Convert.ToDateTime(Month).ToString("MMM/yyyy"));


            CrystalReportViewer1.ReportSource = rptname;

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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerScheduleReport.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ItemSaleMonthWise", "btnCancel_Click", Ex.Message);
        }

    }
}
