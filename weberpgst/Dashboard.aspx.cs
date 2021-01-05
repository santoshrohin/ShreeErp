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

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Last8DaysSales();
        Last8DaysPurchase();
        Last8DaysRawPurchase();
        ShceduleCOmplianceALL();
        
        ShceduleCOmpliance025();
        ShceduleCOmpliance050();
        ShceduleCOmpliance075();
        ShceduleCOmpliance100();
        ShceduleCOmplianceG100();
    }
    protected void Last8DaysSales()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select ROW_NUMBER() OVER (ORDER BY INM_DATE) AS RowNum,Cast(INM_DATE as date) as INM_DATE,FORMAT(SUM(INM_G_AMT),'C', 'en-IN') AS GrandAmt from INVOICE_MASTER where INM_TYPE='TAXINV' and ES_DELETE=0 and  INM_DATE between DATEADD(DAY, -10, getdate())  and GETDATE() group by INM_DATE");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        RepterDetails.DataSource = ds;
        RepterDetails.DataBind();
    }
    protected void Last8DaysPurchase()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select ROW_NUMBER() OVER (ORDER BY IWM_CHAL_DATE) AS RowNum,IWM_CHAL_DATE,FORMAT(ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2),'C', 'en-IN') AS GrandAmt from INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE and INWARD_MASTER.ES_DELETE=0 and IWM_CHAL_DATE between DATEADD(DAY, -10, getdate())  and GETDATE() group by IWM_CHAL_DATE");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        Repeaterpurchase.DataSource = ds;
        Repeaterpurchase.DataBind();
    }
    protected void Last8DaysRawPurchase()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select ROW_NUMBER() OVER (ORDER BY IWM_CHAL_DATE) AS RowNum,IWM_CHAL_DATE,FORMAT(ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2),'C', 'en-IN') AS  GrandAmt from INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER where IWM_CODE=IWD_IWM_CODE and INWARD_MASTER.ES_DELETE=0 and SPOM_CODE=IWD_CPOM_CODE and SPOM_TYPE=-2147483637 and IWM_CHAL_DATE between DATEADD(DAY, -10, getdate())  and GETDATE() group by IWM_CHAL_DATE");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        Repeaterpurchaseraw.DataSource = ds;
        Repeaterpurchaseraw.DataBind();
    }
    #region ShceduleCOmplianceDt
    public  DataTable ShceduleCOmplianceDt(string ctype)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@complianceType", ctype);

            dt = DL_DBAccess.SelectData("SPShceduleCOmpliance", par);
        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion
    protected void ShceduleCOmplianceALL()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("All");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        RepeaterShceduleCOmpliance.DataSource = ds;
        RepeaterShceduleCOmpliance.DataBind();
 
    }
    protected void ShceduleCOmpliance025()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("25");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        repeater025.DataSource = ds;
        repeater025.DataBind();

    }
    protected void ShceduleCOmpliance050()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("50");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
               repeater050.DataSource = ds;
        repeater050.DataBind();

    }
    protected void ShceduleCOmpliance075()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("75");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
            repeater075.DataSource = ds;
          repeater075.DataBind();

    }
    protected void ShceduleCOmpliance100()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("100");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        repeater100.DataSource = ds;
        repeater100.DataBind();

    }
    protected void ShceduleCOmplianceG100()
    {
        DataTable dt = new DataTable();
        dt = ShceduleCOmplianceDt("G100");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        repeaterG100.DataSource = ds;
        repeaterG100.DataBind();

    }
}
