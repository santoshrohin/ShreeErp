using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;

public partial class RoportForms_ADD_OutstandingReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];

            //bool All_CUST = Convert.ToBoolean(Request.QueryString[1].ToString());

           
            string Condition = Request.QueryString[1].ToString();
            //string ToDate = Request.QueryString[2].ToString();
            //int p_name = Convert.ToInt32(Request.QueryString[3].ToString());
            //string rbtGroup = Request.QueryString[4].ToString();
             string ddlType = Request.QueryString[2].ToString();
            //string rbtReportType = Request.QueryString[6].ToString();
            //string cond = Request.QueryString[7].ToString();
            //string CondInv = Request.QueryString[8].ToString();
            //string CondiCredit = Request.QueryString[9].ToString();
            //string CondiDebit = Request.QueryString[10].ToString();
            //string CondiAdvance = Request.QueryString[11].ToString();
             GenerateReport(Title, Condition, ddlType);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    /*
    private void GenerateReport(string Title, bool All_CUST, string ToDate, int p_name, string rbtGroup, string ddlType, string rbtReportType, string cond, string CondInv, string CondiCredit, string CondiDebit, string CondiAdvance)
    {
        try
        {
            string selectstmt="";
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            string lastfinYear = Session["CompanyClosingDate"].ToString();

            SqlParameter[] par = new SqlParameter[7];
            par[0] = new SqlParameter("@billpassingcond", cond);
            par[1] = new SqlParameter("@Invcond", CondInv);
            par[2] = new SqlParameter("@CondiCreditcond", CondiCredit);
            par[3] = new SqlParameter("@CondiDebitcond", CondiDebit);
            string finyear = Session["CompanyFinancialYr"].ToString();
            string currentFinancialYear = ConfigurationManager.AppSettings["CurrentYearid"];
            string storeprocName = "";
            string sep = "-";

            string[] splitContent = finyear.Split(sep.ToCharArray());
            int yearid = Convert.ToInt32(splitContent[0]);
            if (currentFinancialYear==finyear)
            {
                storeprocName = "Outstanding";
            }
            else
            {
                storeprocName = "OutstandingAccordingToFinYear";
 
            }
            
            DataTable dtOutstanding = new DataTable();
            
            if (rbtReportType.ToString() == "0")
            {
                if (ddlType.ToString() == "0")
                {
                    selectstmt = "select P_CODE ,P_NAME,P_ADD1 ,P_CONTACT ,P_PHONE,BPM_INV_NO ,case  when  BPM_DATE is NULL then INM_DATE else BPM_DATE end as BPM_DATE ,BPM_PAYABLE_AMT,INM_NO ,INM_DATE ,INM_RECV_AMT ,DUE_DATE ,DUE_DAYS ,flag  from @temp order by BPM_DATE delete from @temp";
                    par[4] = new SqlParameter("@Selectstatement", selectstmt);
                    par[5] = new SqlParameter("@yearid", yearid);
                    par[6] = new SqlParameter("@CondiAdv", CondiAdvance);
                    ds = DL_DBAccess.SelectDataDataset(storeprocName, par, "dtOutstanding");

                }
                else if (ddlType.ToString() == "1")
                {
                    selectstmt = "select P_CODE ,P_NAME,P_ADD1 ,P_CONTACT ,P_PHONE,BPM_INV_NO ,case  when  BPM_DATE is NULL then INM_DATE else BPM_DATE end as BPM_DATE ,BPM_PAYABLE_AMT,INM_NO ,INM_DATE ,INM_RECV_AMT ,DUE_DATE ,DUE_DAYS ,flag  from @temp  where flag=1  order by BPM_DATE delete from @temp";
                    par[4] = new SqlParameter("@Selectstatement", selectstmt);
                    par[5] = new SqlParameter("@yearid", yearid);
                    ds = DL_DBAccess.SelectDataDataset(storeprocName, par, "dtOutstanding");

                }
                else
                {
                    selectstmt = "select P_CODE ,P_NAME,P_ADD1 ,P_CONTACT ,P_PHONE,BPM_INV_NO ,case  when  BPM_DATE is NULL then INM_DATE else BPM_DATE end as BPM_DATE ,BPM_PAYABLE_AMT,INM_NO ,INM_DATE ,INM_RECV_AMT ,DUE_DATE ,DUE_DAYS ,flag  from @temp where flag=0 order by BPM_DATE  delete from @temp";
                    par[4] = new SqlParameter("@Selectstatement", selectstmt);
                    par[5] = new SqlParameter("@yearid", yearid);
                    ds = DL_DBAccess.SelectDataDataset(storeprocName, par, "dtOutstanding");

                }
            }
            else
            {
                if (ddlType.ToString() == "0")
                    dtOutstanding = CommonClasses.Execute("declare @temps table(Id int NOT NULL identity(1,1),P_CODE int,P_NAME varchar(500),BPM_PAYABLE_AMT float,INM_RECV_AMT float,flag bit) insert into @temps select P_CODE,P_NAME,SUM(isnull(BPM_G_AMT,0))-SUM(isnull(BPM_PAID_AMT,0)),0,0  from BILL_PASSING_MASTER,PARTY_MASTER where " + cond + " BPM_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and isnull(BPM_G_AMT,0)-ISNULL(BPM_PAID_AMT,0)>0 group by P_CODE,P_NAME insert into @temps select P_CODE,P_NAME,0,SUM(isnull(INM_G_AMT,0))-SUM(isnull(INM_RECIEVED_AMT,0)),1 from INVOICE_MASTER,PARTY_MASTER where " + CondInv + " INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and  PARTY_MASTER.ES_DELETE=0 and isnull(INM_G_AMT,0)-ISNULL(INM_RECIEVED_AMT,0)>0 group by P_CODE,P_NAME select * from @temps delete from @temps");
                else if (ddlType.ToString() == "1")
                    dtOutstanding = CommonClasses.Execute("declare @temps table(Id int NOT NULL identity(1,1),P_CODE int,P_NAME varchar(500),BPM_PAYABLE_AMT float,INM_RECV_AMT float,flag bit) insert into @temps select P_CODE,P_NAME,SUM(isnull(BPM_G_AMT,0))-SUM(isnull(BPM_PAID_AMT,0)),0,0  from BILL_PASSING_MASTER,PARTY_MASTER where " + cond + " BPM_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and isnull(BPM_G_AMT,0)-ISNULL(BPM_PAID_AMT,0)>0 group by P_CODE,P_NAME insert into @temps select P_CODE,P_NAME,0,SUM(isnull(INM_G_AMT,0))-SUM(isnull(INM_RECIEVED_AMT,0)),1 from INVOICE_MASTER,PARTY_MASTER where " + CondInv + " INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and  PARTY_MASTER.ES_DELETE=0 and isnull(INM_G_AMT,0)-ISNULL(INM_RECIEVED_AMT,0)>0 group by P_CODE,P_NAME select * from @temps where flag=1 delete from @temps");
                else
                    dtOutstanding = CommonClasses.Execute("declare @temps table(Id int NOT NULL identity(1,1),P_CODE int,P_NAME varchar(500),BPM_PAYABLE_AMT float,INM_RECV_AMT float,flag bit) insert into @temps select P_CODE,P_NAME,SUM(isnull(BPM_G_AMT,0))-SUM(isnull(BPM_PAID_AMT,0)),0,0  from BILL_PASSING_MASTER,PARTY_MASTER where " + cond + " BPM_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and isnull(BPM_G_AMT,0)-ISNULL(BPM_PAID_AMT,0)>0 group by P_CODE,P_NAME insert into @temps select P_CODE,P_NAME,0,SUM(isnull(INM_G_AMT,0))-SUM(isnull(INM_RECIEVED_AMT,0)),1 from INVOICE_MASTER,PARTY_MASTER where " + CondInv + " INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and  PARTY_MASTER.ES_DELETE=0 and isnull(INM_G_AMT,0)-ISNULL(INM_RECIEVED_AMT,0)>0 group by P_CODE,P_NAME select * from @temps where flag=0 delete from @temps");
                //dtOutstanding = CommonClasses.Execute("declare @temps table(Id int NOT NULL identity(1,1),P_CODE int,P_NAME varchar(500),BPM_PAYABLE_AMT float,INM_RECV_AMT float) insert into @temps select P_CODE,P_NAME,SUM(BPM_G_AMT)-SUM(BPM_PAID_AMT),0  from BILL_PASSING_MASTER,PARTY_MASTER where BPM_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and BPM_G_AMT-ISNULL(BPM_PAID_AMT,0)>0 group by P_CODE,P_NAME insert into @temps select P_CODE,P_NAME,0,SUM(isnull(INM_G_AMT,0))-SUM(isnull(INM_RECIEVED_AMT,0)) from INVOICE_MASTER,PARTY_MASTER where INM_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and INM_G_AMT-ISNULL(INM_RECIEVED_AMT,0)>0 group by P_CODE,P_NAME select * from @temps delete from @temps ");
            }

            dtOutstanding = ds.Tables[0];
            #region Count
            //if (ds.r)
            //{
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            if (rbtReportType.ToString() == "0")
            {
                rptname.Load(Server.MapPath("~/Account/Reports/OutstandingReport.rpt"));
                rptname.FileName = Server.MapPath("~/Account/Reports/OutstandingReport.rpt");
            }
            else
            {
                rptname.Load(Server.MapPath("~/Account/Reports/OutstandingSummary.rpt"));
                rptname.FileName = Server.MapPath("~/Account/Reports/OutstandingSummary.rpt");
            }

            rptname.Refresh();
            rptname.SetDataSource(dtOutstanding);

            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtDate", "TillDate " + ToDate);
            CrystalReportViewer1.ReportSource = rptname;

            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = " No Record Found! ";
            //}
            #endregion

        }
        catch (Exception Ex)
        {

        }
    }
      */
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Condition, string ddlType)
    {
        try
        {
            string selectstmt = "";
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            ds = null;

            string lastfinYear = Session["CompanyClosingDate"].ToString();

            //SqlParameter[] par = new SqlParameter[7];
            //par[0] = new SqlParameter("@billpassingcond", cond);
            //par[1] = new SqlParameter("@Invcond", CondInv);
            //par[2] = new SqlParameter("@CondiCreditcond", CondiCredit);
            //par[3] = new SqlParameter("@CondiDebitcond", CondiDebit);
            //string finyear = Session["CompanyFinancialYr"].ToString();
            //string currentFinancialYear = ConfigurationManager.AppSettings["CurrentYearid"];
            //string storeprocName = "";
            //string sep = "-";

            //string[] splitContent = finyear.Split(sep.ToCharArray());
            //int yearid = Convert.ToInt32(splitContent[0]);
            //if (currentFinancialYear == finyear)
            //{
            //    storeprocName = "Outstanding";
            //}
            //else
            //{
            //    storeprocName = "OutstandingAccordingToFinYear";

            //}

            DataTable dtOutstanding = new DataTable();
            if (ddlType=="1")
            {
                dtOutstanding = CommonClasses.Execute("SELECT  * FROM  TALLY_OUTSTANDING  where " + Condition + " ORDER BY TALLY_OUT_PARTYNAME ");


            }
            else
            {
                dtOutstanding = CommonClasses.Execute("SELECT  * FROM  TALLY_OUTSTANDING where " + Condition + "    ORDER BY TALLY_OUT_PARTYNAME ");

            }

           
            
            #region Count
            
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Account/Reports/outstanding.rpt"));
            rptname.FileName = Server.MapPath("~/Account/Reports/outstanding.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dtOutstanding);

            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString()); 
            CrystalReportViewer1.ReportSource = rptname;
 
            #endregion

        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/View/ViewOutstandingReport.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion btnCancel_Click
}
