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
        sales();
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
    public DataTable ShceduleCOmplianceDt(string ctype)
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

    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();

    protected void sales()
    {
        DL_DBAccess = new DatabaseAccessLayer();

        SqlParameter[] par1 = new SqlParameter[1];
        par1[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        DataTable dtLoadStock = DL_DBAccess.SelectData("LoadSales", par1);

        DataTable dtfirst = new DataTable();
        dtfirst = dtLoadStock;
        string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP1"].ToString();
        SqlConnection conn1 = null;
        SqlCommand cmd1 = null;
        SqlDataAdapter adapter1 = null;
        DataTable _dt1 = new DataTable();

        DatabaseAccessLayer DL_DBAccess1 = new DatabaseAccessLayer();
        DL_DBAccess1 = new DatabaseAccessLayer();
        SqlParameter[] par2 = new SqlParameter[1];
        par2[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        conn1 = new SqlConnection(strConnString);
        conn1.Open();
        cmd1 = new SqlCommand("LoadSales", conn1);
        cmd1.CommandType = CommandType.StoredProcedure;
        if (par2 != null)
        {
            for (int index = 0; index < par2.Length; index++)
            {
                cmd1.Parameters.Add(par2[index]);
            }
        }
        adapter1 = new SqlDataAdapter(cmd1);
        adapter1.Fill(_dt1);


        dtfirst.Merge(_dt1);


        string strConnString2 = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP2"].ToString();
        SqlConnection conn2 = null;
        SqlCommand cmd2 = null;
        SqlDataAdapter adapter2 = null;
        DataTable _dt2 = new DataTable();

        DatabaseAccessLayer DL_DBAccess2 = new DatabaseAccessLayer();
        DL_DBAccess2 = new DatabaseAccessLayer();
        SqlParameter[] par3 = new SqlParameter[1];
        par3[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        conn2 = new SqlConnection(strConnString2);
        conn2.Open();
        cmd2 = new SqlCommand("LoadSales", conn2);
        cmd2.CommandType = CommandType.StoredProcedure;
        if (par3 != null)
        {
            for (int index = 0; index < par3.Length; index++)
            {
                cmd2.Parameters.Add(par3[index]);
            }
        }
        adapter2 = new SqlDataAdapter(cmd2);
        adapter2.Fill(_dt2);
        dtfirst.Merge(_dt2);

        DataTable dtresult = new DataTable();
        DataView dv1 = dtfirst.DefaultView;
        dv1.RowFilter = "sr ='AccessibleAmt'";
        dtresult = dv1.ToTable();

        DataTable dtresult1 = new DataTable();
        DataView dv11 = dtfirst.DefaultView;
        dv11.RowFilter = "sr ='TotalAmt'";
        dtresult1 = dv11.ToTable();


        SPCLM.Text = dtresult.Rows[0]["BasicAmtLM"].ToString();
        QualitatLM.Text = dtresult.Rows[1]["BasicAmtLM"].ToString();
        CalidadLM.Text = dtresult.Rows[2]["BasicAmtLM"].ToString();
        totalLM.Text = (Convert.ToDouble(SPCLM.Text) + Convert.ToDouble(QualitatLM.Text) + Convert.ToDouble(CalidadLM.Text)).ToString();

        SPCTaxLM.Text = dtresult1.Rows[0]["BasicAmtLM"].ToString();
        QualitatTaxLM.Text = dtresult1.Rows[1]["BasicAmtLM"].ToString();
        CalidadTaxLM.Text = dtresult1.Rows[2]["BasicAmtLM"].ToString();
        totalTaxSales.Text = (Convert.ToDouble(SPCTaxLM.Text) + Convert.ToDouble(QualitatTaxLM.Text) + Convert.ToDouble(CalidadTaxLM.Text)).ToString();


        SPCCM.Text = dtresult.Rows[0]["BasicAmtCM"].ToString();
        QualitatCM.Text = dtresult.Rows[1]["BasicAmtCM"].ToString();
        CalidadCM.Text = dtresult.Rows[2]["BasicAmtCM"].ToString();
        totalCM.Text = (Convert.ToDouble(SPCCM.Text) + Convert.ToDouble(QualitatCM.Text) + Convert.ToDouble(CalidadCM.Text)).ToString();

        SPCTaxCM.Text = dtresult1.Rows[0]["BasicAmtCM"].ToString();
        QualitatTaxCM.Text = dtresult1.Rows[1]["BasicAmtCM"].ToString();
        CalidadTaxCM.Text = dtresult1.Rows[2]["BasicAmtCM"].ToString();
        TotalTaxCM.Text = (Convert.ToDouble(SPCTaxCM.Text) + Convert.ToDouble(QualitatTaxCM.Text) + Convert.ToDouble(CalidadTaxCM.Text)).ToString();


        basicSalesDiff.Text = (Convert.ToDouble(totalCM.Text) - Convert.ToDouble(totalLM.Text)).ToString();
        SPCSalesDiff.Text = (Convert.ToDouble(SPCCM.Text) - Convert.ToDouble(SPCLM.Text)).ToString();
        qualitatSalesDiff.Text = (Convert.ToDouble(QualitatCM.Text) - Convert.ToDouble(QualitatLM.Text)).ToString();
        CalidadSalesDiff.Text = (Convert.ToDouble(CalidadCM.Text) - Convert.ToDouble(CalidadLM.Text)).ToString();


        TotalSalesDiffTax.Text = (Convert.ToDouble(TotalTaxCM.Text) - Convert.ToDouble(totalTaxSales.Text)).ToString();
        SPCSalesDiffTax.Text = (Convert.ToDouble(SPCTaxCM.Text) - Convert.ToDouble(SPCTaxLM.Text)).ToString();
        QualitatSalesDiffTax.Text = (Convert.ToDouble(QualitatTaxCM.Text) - Convert.ToDouble(QualitatTaxLM.Text)).ToString();
        CalidatSalesDiffTax.Text = (Convert.ToDouble(CalidadTaxCM.Text) - Convert.ToDouble(CalidadTaxLM.Text)).ToString();

    }
}
