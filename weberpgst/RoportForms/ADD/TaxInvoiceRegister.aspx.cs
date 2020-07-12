using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_TaxInvoiceRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        //home.Attributes["class"] = "active";
        //HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        //home1.Attributes["class"] = "active";

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];

            //bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //string p_name = Request.QueryString[6].ToString();
            //string i_name = Request.QueryString[7].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Condition = Request.QueryString[5];
            string Type = Request.QueryString[6];
            // i_name = i_name.Replace("'", "''");

            GenerateReport(Title, From, To, group, way, Condition, Type);

        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string strCondition, string Type)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "SELECT CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT FROM PARTY_MASTER INNER JOIN CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN LOG_MASTER ON CUSTPO_MASTER.CPOM_CODE = LOG_MASTER.LG_DOC_CODE ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_MASTER ON CUSTPO_DETAIL.CPOD_I_CODE = ITEM_MASTER.I_CODE where CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";

            //Query = "SELECT DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT FROM CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER WHERE INVOICE_MASTER.ES_DELETE=0 and  INM_CM_CODE='"+Session["CompanyCode"]+"' AND INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and IND_I_CODE=CPOD_I_CODE ";

            Query = "SELECT  DISTINCT(I_CODE),I_CODENO,CONVERT(VARCHAR,INM_LC_DATE,106) AS  INM_LC_DATE,INM_LC_NO ,I_CODENO,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY, CASE WHEN 'EXPORT1'='" + Type + "' then  cast(isnull(IND_INQTY,0) as numeric(20,3)) ELSE CASE when INM_IS_SUPPLIMENT =1 then 0 else  cast(isnull(IND_INQTY,0) as numeric(20,3)) END END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO, ISNULL(IND_AMORTRATE,0)  AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0)  AS IND_AMORTAMT, PARTY_MASTER.P_LBT_NO, EXCISE_TARIFF_MASTER.E_TARIFF_NO,ISNULL(E_BASIC_CentralT,0) AS    E_BASIC_CentralT,ISNULL(E_EDU_CESS_State,0) AS E_EDU_CESS_State, ISNULL(E_H_EDU_Integrated,0) AS  E_H_EDU_Integrated,ISNULL(IND_HSN_CODE,0) AS  IND_HSN_CODE,ISNULL(IND_EX_AMT,0) AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) AS   IND_SH_CESS_AMT,SM_NAME  FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER ,ITEM_UNIT_MASTER,EXCISE_TARIFF_MASTER ,STATE_MASTER  WHERE I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE and EXCISE_TARIFF_MASTER.ES_DELETE=0  AND P_SM_CODE=SM_CODE   and INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE  AND I_CODE=IND_I_CODE   AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE  " + strCondition + "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Query = Query + "  GROUP BY INM_LR_DATE,I_CODENO,I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,INM_VEH_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_IS_SUPPLIMENT,INM_TNO,I_UOM_NAME,INM_TYPE,IND_AMORTRATE,IND_AMORTAMT,PARTY_MASTER.P_LBT_NO, EXCISE_TARIFF_MASTER.E_TARIFF_NO ,E_BASIC_CentralT,E_EDU_CESS_State,E_H_EDU_Integrated,IND_HSN_CODE,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT,SM_NAME,INM_LC_DATE,INM_LC_NO     ORDER BY INM_TNO";

            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (Type == "SHOW")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    string report = "";
                    if (group == "Datewise")
                    {
                        report = "TaxInvRegDatewise";
                    }
                    if (group == "ItemWise")
                    {
                        report = "TaxInvRegItemwise";
                    }
                    if (group == "CustWise")
                    {
                        report = "TaxInvRegCustwise";
                    }
                    if (group == "HSNWise")
                    {
                        report = "TaxInvRegItemwise";
                    }
                    rptname.Load(Server.MapPath("~/Reports/" + report + ".rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/" + report + ".rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", Convert.ToDateTime(From).ToString("dd-MMM-yyyy") + " - " + Convert.ToDateTime(To).ToString("dd-MMM-yyyy"));

                    CrystalReportViewer1.ReportSource = rptname; 

                }                   
                else
                {


                    DataTable dtResult = new DataTable();

                    if (way == "Summary")
                    {
                        dt.Rows.Clear();
                        dt = CommonClasses.Execute("SELECT  DISTINCT CONVERT(VARCHAR,INM_LC_DATE,106) AS  INM_LC_DATE,INM_LC_NO  ,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,CPOM_PONO,CPOM_DATE, CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE, CASE WHEN 'EXPORT1'='EXPORT' then  cast(isnull(SUM(IND_INQTY),0) as numeric(20,3)) ELSE CASE when INM_IS_SUPPLIMENT =1 then 0 else  cast(isnull(SUM(IND_INQTY),0) as numeric(20,3)) END END as IND_INQTY, cast(isnull(SUM(IND_AMT),0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO,  PARTY_MASTER.P_LBT_NO  FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER ,ITEM_UNIT_MASTER,EXCISE_TARIFF_MASTER ,STATE_MASTER  WHERE I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE and EXCISE_TARIFF_MASTER.ES_DELETE=0  AND P_SM_CODE=SM_CODE   and INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE  AND I_CODE=IND_I_CODE   AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE    " + strCondition + "      AND  INM_TYPE IN ( 'TAXINV', 'OutJWINM' )    GROUP BY INM_LR_DATE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,CPOM_PONO,CPOM_DATE,INM_VEH_NO,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_IS_SUPPLIMENT,INM_TNO,INM_TYPE ,PARTY_MASTER.P_LBT_NO, EXCISE_TARIFF_MASTER.E_TARIFF_NO ,SM_NAME,INM_LC_DATE,INM_LC_NO     ORDER BY INM_TNO");
                    }
                    dtResult = dt;
                    DataTable dtExport = new DataTable();
                    if (dt.Rows.Count > 0)
                    {


                        if (way == "Summary")
                        {
                            dtExport.Columns.Add("Inv Date");
                            dtExport.Columns.Add("Inv No");
                            dtExport.Columns.Add("Party Name");
                            //dtExport.Columns.Add("Type");
                            

                       
                            dtExport.Columns.Add("Basic Amt");
                            dtExport.Columns.Add("CGST");
                            dtExport.Columns.Add("SGST");
                            dtExport.Columns.Add("IGST");
                            dtExport.Columns.Add("Total Amt");

                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                dtExport.Rows.Add(Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                                  dtResult.Rows[i]["INM_TNO"].ToString(),
                                    
                                                  dtResult.Rows[i]["P_NAME"].ToString(),
                                                  
                                                  dtResult.Rows[i]["IND_AMT"].ToString(),
                                                  dtResult.Rows[i]["INM_BE_AMT"].ToString(),
                                                  dtResult.Rows[i]["INM_EDUC_AMT"].ToString(),
                                                  dtResult.Rows[i]["INM_H_EDUC_AMT"].ToString(),
                                                   dtResult.Rows[i]["INM_G_AMT"].ToString()

                                                 );
                            }
                        }
                        else
                        {


                            //if (Type == "EXPORT")
                            //{
                            dtExport.Columns.Add("Inv Date");
                            dtExport.Columns.Add("Inv No");
                            dtExport.Columns.Add("Party Name");
                            //dtExport.Columns.Add("Type");
                            dtExport.Columns.Add("Item Name");
                            dtExport.Columns.Add("Item Code");

                            dtExport.Columns.Add("Qty");
                            dtExport.Columns.Add("Rate");
                            dtExport.Columns.Add("Basic Amt");
                            dtExport.Columns.Add("CGST");
                            dtExport.Columns.Add("SGST");
                            dtExport.Columns.Add("IGST");
                            dtExport.Columns.Add("TCS AMOUNT");
                            dtExport.Columns.Add("TOTAL"); 

                            if (Type == "EXPORT1")
                            {
                                dtExport.Columns.Add("Old Inv No");
                                dtExport.Columns.Add("Old Inv Date");
                                dtExport.Columns.Add("Net");
                            }
                            if (Type == "EXPORT")
                            {

                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    dtExport.Rows.Add(Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                                      dtResult.Rows[i]["INM_TNO"].ToString(),
                                        // dtResult.Rows[i]["INM_INVOICE_TYPE"].ToString(),
                                                      dtResult.Rows[i]["P_NAME"].ToString(),
                                                      dtResult.Rows[i]["I_NAME"].ToString(),
                                                      dtResult.Rows[i]["I_CODENO"].ToString(),
                                                      dtResult.Rows[i]["IND_INQTY"].ToString(),
                                                      dtResult.Rows[i]["IND_RATE"].ToString(),
                                                      dtResult.Rows[i]["IND_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_EX_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
                                                      dtResult.Rows[i]["INM_TAX_TCS_AMT"].ToString(),
                                                      dtResult.Rows[i]["INM_G_AMT"].ToString()

                                                     );
                                }
                            }
                            else
                            {
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    dtExport.Rows.Add(Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                                      dtResult.Rows[i]["INM_TNO"].ToString(),
                                        // dtResult.Rows[i]["INM_INVOICE_TYPE"].ToString(),
                                                      dtResult.Rows[i]["P_NAME"].ToString(),
                                                      dtResult.Rows[i]["I_NAME"].ToString(),
                                                      dtResult.Rows[i]["I_CODENO"].ToString(),
                                                      dtResult.Rows[i]["IND_INQTY"].ToString(),
                                                      dtResult.Rows[i]["IND_RATE"].ToString(),
                                                      dtResult.Rows[i]["IND_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_EX_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
                                                      dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
                                                       dtResult.Rows[i]["INM_TAX_TCS_AMT"].ToString(),
                                                      dtResult.Rows[i]["INM_G_AMT"].ToString(),
                                                        dtResult.Rows[i]["INM_LC_NO"].ToString(),
                                                        dtResult.Rows[i]["INM_LC_DATE"].ToString(),
                                                         dtResult.Rows[i]["INM_G_AMT"].ToString()
                                                     );
                                }
                            }
                        }
                    }

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=TaxInvoiceSummary.xls");

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

                            if (way == "Summary")
                            {
                                    HttpContext.Current.Response.Write("<Td>");
                                    
                                    HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                    HttpContext.Current.Response.Write("</Td>");

                            }
                            else
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
        catch (Exception Ex)
        {
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
            CommonClasses.SendError("Tax Invoice Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTaxInvoiceRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
