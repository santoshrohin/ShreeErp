using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

public partial class Masters_ADD_AdminDashboard : System.Web.UI.Page
{
    protected void Last8DaysSales()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("declare @sumvalue table (RowNum int identity(1,1),INM_DATE varchar(500),GrandAmt nvarchar(1000),Total nvarchar(1000),GrandAmtwithoutformat float,Totalsaletilldate nvarchar(4000),TotalBasicsaletilldate nvarchar(4000))insert into @sumvalue select CONVERT(varchar, Cast(INM_DATE as date), 103) as INM_DATE ,FORMAT(SUM(INM_G_AMT),'C', 'en-IN') AS GrandAmt,0,SUM(INM_G_AMT),(select FORMAT(SUM(INM_G_AMT),'C', 'en-IN') from INVOICE_MASTER where INM_CM_CODE in (select top 1 CM_CODE from COMPANY_MASTER  order by CM_CODE desc)),(select FORMAT(isnull(round(sum(IND_AMT),2),0),'C', 'en-IN')   from INVOICE_MASTER,INVOICE_DETAIL where   INM_CODE=IND_INM_CODE    AND  INM_TYPE in ('TAXINV','OutJWINM')   and INVOICE_MASTER.ES_DELETE=0 and INM_CM_CODE in (select top 1 CM_CODE from COMPANY_MASTER  order by CM_CODE desc)) from INVOICE_MASTER where INM_TYPE='TAXINV' and ES_DELETE=0 and  INM_DATE between DATEADD(DAY, -9, getdate())  and GETDATE() group by INM_DATE order by INM_DATE desc declare @Totalvalue float select @Totalvalue=sum(GrandAmtwithoutformat) from @sumvalue update @sumvalue set total= FORMAT(@Totalvalue,'C', 'en-IN') select * from @sumvalue");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        if (dt.Rows.Count > 0)
        {
            lbltotal8dayssale.Text = dt.Rows[0]["Total"].ToString();
            lbltilldateSale.Text = dt.Rows[0]["Totalsaletilldate"].ToString();
            lblBasictilldateSale.Text = dt.Rows[0]["TotalBasicsaletilldate"].ToString();
        }
        RepterDetails.DataSource = ds;
        RepterDetails.DataBind();
    }
    protected void Last8DaysPurchase()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("declare @sumvalue table (RowNum int identity(1,1),INM_DATE varchar(500),GrandAmt nvarchar(1000),Total nvarchar(1000),GrandAmtwithoutformat float) insert into @sumvalue select CONVERT(varchar, Cast(IWM_CHAL_DATE as date), 103) as IWM_CHAL_DATE,FORMAT(ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2),'C', 'en-IN') AS GrandAmt,0,ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2) from INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE and INWARD_MASTER.ES_DELETE=0 and IWM_CHAL_DATE between DATEADD(DAY, -9, getdate())  and GETDATE() group by IWM_CHAL_DATE order by IWM_CHAL_DATE desc declare @Totalvalue float select @Totalvalue=sum(GrandAmtwithoutformat) from @sumvalue update @sumvalue set total= FORMAT(@Totalvalue,'C', 'en-IN') select * from @sumvalue");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        if (dt.Rows.Count > 0)
        {
            lbltotal8daysPurchase.Text = dt.Rows[0]["Total"].ToString();
        }
        Repeaterpurchase.DataSource = ds;
        Repeaterpurchase.DataBind();
    }
    protected void Last8DaysRawPurchase()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("declare @sumvalue table (RowNum int identity(1,1),INM_DATE varchar(500),GrandAmt nvarchar(1000),Total nvarchar(1000),GrandAmtwithoutformat float)insert into @sumvalue select CONVERT(varchar, Cast(IWM_CHAL_DATE as date), 103) as IWM_CHAL_DATE,FORMAT(ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2),'C', 'en-IN') AS  GrandAmt,0,ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2) from INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER where IWM_CODE=IWD_IWM_CODE and INWARD_MASTER.ES_DELETE=0 and SPOM_CODE=IWD_CPOM_CODE and SPOM_TYPE=-2147483637 and IWM_CHAL_DATE between DATEADD(DAY, -9, getdate())  and GETDATE() group by IWM_CHAL_DATE order by IWM_CHAL_DATE desc declare @Totalvalue float select @Totalvalue=sum(GrandAmtwithoutformat) from @sumvalue update @sumvalue set total= FORMAT(@Totalvalue,'C', 'en-IN') select * from @sumvalue");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        if (dt.Rows.Count > 0)
        {
            lbltotal8daysRawPurchase.Text = dt.Rows[0]["Total"].ToString();
        }
        Repeaterpurchaseraw.DataSource = ds;
        Repeaterpurchaseraw.DataBind();
    }
    protected void Last8DaysBOUGHTOUT()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("declare @sumvalue table (RowNum int identity(1,1),INM_DATE varchar(500),GrandAmt nvarchar(1000),Total nvarchar(1000),GrandAmtwithoutformat float)insert into @sumvalue select CONVERT(varchar, Cast(IWM_CHAL_DATE as date), 103) as IWM_CHAL_DATE,FORMAT(ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2),'C', 'en-IN') AS  GrandAmt,0,ROUND(SUM(IWD_CH_QTY*ROUND(IWD_RATE,2)),2) from INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER where IWM_CODE=IWD_IWM_CODE and INWARD_MASTER.ES_DELETE=0 and SPOM_CODE=IWD_CPOM_CODE and SPOM_TYPE=-2147483636 and IWM_CHAL_DATE between DATEADD(DAY, -9, getdate())  and GETDATE() group by IWM_CHAL_DATE order by IWM_CHAL_DATE desc declare @Totalvalue float select @Totalvalue=sum(GrandAmtwithoutformat) from @sumvalue update @sumvalue set total= FORMAT(@Totalvalue,'C', 'en-IN') select * from @sumvalue");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        if (dt.Rows.Count > 0)
        {
            lbltotal8daysBOUGHTOUTPurchase.Text = dt.Rows[0]["Total"].ToString();
        }
        RepeaterBOUGHTOUTraw.DataSource = ds;
        RepeaterBOUGHTOUTraw.DataBind();
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
    
    
    #region SelectedItemShceduleCOmplianceDt
    public DataTable SelectedItemShceduleCOmplianceDt(string ctype)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@complianceType", ctype);

            dt = DL_DBAccess.SelectData("uSPShceduleCOmpliance_SelectedItem", par);
        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion
    #region PartyWiseShceduleCOmplianceDt
    public DataTable PartyWiseShceduleCOmplianceDt(string ctype)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@complianceType", ctype);

            dt = DL_DBAccess.SelectData("uSPShceduleCOmpliancePartyWise", par);
        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion
    #region purchaseratio
    public DataTable purchaseratio(int ctype)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@CompanyId", ctype);

            dt = DL_DBAccess.SelectData("RawRatio", par);

        }
        catch (Exception Ex)
        {
        }
        return dt;
    }
    #endregion
    #region uspYesterdayandtoadyssale
    public DataTable uspYesterdayandtoadyssale(int ctype)
    {
        DataTable dt = new DataTable();
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@companyid", 1);

            dt = DL_DBAccess.SelectData("uspYesterdayandtoadyssale", par);

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
        dt = PartyWiseShceduleCOmplianceDt("All");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        RepeaterShceduleCOmpliance.DataSource = ds;
        RepeaterShceduleCOmpliance.DataBind();

    }

    protected void uspYesterdayandtoadyssale()
    {
        DataTable dt = new DataTable();
        dt = uspYesterdayandtoadyssale(1);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        repeateryesterdaydispatch.DataSource = ds;
        repeateryesterdaydispatch.DataBind();

    }

    protected void purchasetypewiseratio()
    {
        DataTable dt = new DataTable();
        dt = purchaseratio(1);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);

        if (dt.Rows.Count > 0)
        {
            lbltotalSaleamout.Text = dt.Rows[0]["totalsale"].ToString();
            lbltotalSaleamout.Text = convertorupee(lbltotalSaleamout.Text);
        }

        repeaterratio.DataSource = ds;
        repeaterratio.DataBind();

    }
    protected void SelectedItemShceduleCOmplianceALL()
    {
        DataTable dt = new DataTable();
        dt = SelectedItemShceduleCOmplianceDt("All");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        RepeaterShceduleCOmplianceSelectItem.DataSource = ds;
        RepeaterShceduleCOmplianceSelectItem.DataBind();

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


    protected void sales()
    {
        DL_DBAccess = new DatabaseAccessLayer();

        SqlParameter[] par1 = new SqlParameter[1];
        par1[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        DataTable dtLoadStock = DL_DBAccess.SelectData("LoadSales", par1);

        DataTable dtfirst = new DataTable();
        dtfirst = dtLoadStock;






        DataTable dtresult = new DataTable();
        DataView dv1 = dtfirst.DefaultView;
        dv1.RowFilter = "sr ='AccessibleAmt'";
        dtresult = dv1.ToTable();

        DataTable dtresult1 = new DataTable();
        DataView dv11 = dtfirst.DefaultView;
        dv11.RowFilter = "sr ='TotalAmt'";
        dtresult1 = dv11.ToTable();


        SPCLM.Text = dtresult.Rows[0]["BasicAmtLM"].ToString();
        totalLM.Text = (Convert.ToDouble(SPCLM.Text)).ToString();

        SPCTaxLM.Text = dtresult1.Rows[0]["BasicAmtLM"].ToString();
        totalTaxSales.Text = (Convert.ToDouble(SPCTaxLM.Text)).ToString();


        SPCCM.Text = dtresult.Rows[0]["BasicAmtCM"].ToString();
        totalCM.Text = (Convert.ToDouble(SPCCM.Text)).ToString();

        SPCTaxCM.Text = dtresult1.Rows[0]["BasicAmtCM"].ToString();

        TotalTaxCM.Text = (Convert.ToDouble(SPCTaxCM.Text)).ToString();


        basicSalesDiff.Text = (Convert.ToDouble(totalLM.Text) - Convert.ToDouble(totalCM.Text)).ToString();
        SPCSalesDiff.Text = (Convert.ToDouble(SPCLM.Text) - Convert.ToDouble(SPCCM.Text)).ToString();



        TotalSalesDiffTax.Text = (Convert.ToDouble(totalTaxSales.Text) - Convert.ToDouble(TotalTaxCM.Text)).ToString();
        SPCSalesDiffTax.Text = (Convert.ToDouble(SPCTaxLM.Text) - Convert.ToDouble(SPCTaxCM.Text)).ToString();

        SPCLM.Text = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", SPCLM.Text);


        SPCLM.Text = convertorupee(SPCLM.Text);
        SPCTaxLM.Text = convertorupee(SPCTaxLM.Text);
        SPCCM.Text = convertorupee(SPCCM.Text);
        SPCTaxCM.Text = convertorupee(SPCTaxCM.Text);
        SPCSalesDiff.Text = convertorupee(SPCSalesDiff.Text);
        SPCSalesDiffTax.Text = convertorupee(SPCSalesDiffTax.Text);

        lblFinYear.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).Year.ToString() + " - " + Convert.ToDateTime(Session["CompanyClosingDate"]).Year.ToString();
        lblFinYear1.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).Year.ToString() + " - " + Convert.ToDateTime(Session["CompanyClosingDate"]).Year.ToString();

    }
    #region general
    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
    DataTable dtFilter = new DataTable();
    DataTable dtStoreFilter = new DataTable();
    public static string right = "";
    #endregion
    public string convertorupee(string amt)
    {
        decimal parsed = decimal.Parse(amt, CultureInfo.InvariantCulture);
        CultureInfo hindi = new CultureInfo("hi-IN");
        string text = string.Format(hindi, "{0:c}", parsed);
        return text;
    }
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //DataTable dtRights = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE='139'");
                //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                //ViewState["right"] = right;

                if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    //if (Session["DASHBOARD"].ToString() == "1")
                    //{
                    ////    LoadActivity();
                    //}

                    //if (CommonClasses.ValidRights(int.Parse(((string)ViewState["right"]).Substring(5, 1)), this, "For Print"))
                    //{

                    string previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");
                    string CMonth = DateTime.Now.ToString("MMMM");
                    lblLMName.Text = previousMonth;
                    lblLMName1.Text = previousMonth;
                    lblTMName.Text = CMonth;
                    lblTMName1.Text = CMonth;
                    Last8DaysSales();
                    Last8DaysPurchase();
                    Last8DaysRawPurchase();
                    Last8DaysBOUGHTOUT();
                    ShceduleCOmplianceALL();
                    uspYesterdayandtoadyssale();
                    purchasetypewiseratio();
                    SelectedItemShceduleCOmplianceALL();
                    ShceduleCOmpliance025();
                    ShceduleCOmpliance050();
                    ShceduleCOmpliance075();
                    ShceduleCOmpliance100();
                    ShceduleCOmplianceG100();
                    sales();
                    // }
                    //else
                    //{
                    //    StockValuation.Visible = false;
                    //}
                }
            }
        }
        catch (Exception ex)
        {


        }

    }
    #endregion Page_Load

    

    
}
