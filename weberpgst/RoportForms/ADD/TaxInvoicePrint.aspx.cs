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
using System.IO;
using System.Net;
using System.Net.Mail;
public partial class RoportForms_ADD_TaxInvoicePrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    string type = "";
    string title = "";
    string Cond = "";
    string chkPrint1 = "";
    string rbtType = "";
    string toNo = "";
    string path = "";
    string supp = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        string Title = Request.QueryString[0];
        Cond = Request.QueryString[1];
        invoice_code = Request.QueryString[3];
        reportType = Request.QueryString[4];
        //if (Cond == "1" )
        //{
        chkPrint1 = Convert.ToInt32(Request.QueryString[2]).ToString();
        //}
        if (reportType == "Mult")
        {
            toNo = Request.QueryString[5];
            supp = Request.QueryString[6];
        }
        GenerateReport(invoice_code);
    }
    #endregion Page_Init

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtTaxInvoice = new DataTable();
        DataTable dtTemp = new DataTable();
        DataSet dsTaxInvoiceGST = new DataSet();
        ReportDocument rptname = null;
        rptname = new ReportDocument();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            //DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_TAX_NAME,ST_SALES_TAX FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX");        
            //DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "'  group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO ,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME ,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE ,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX ,E_TARIFF_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,ST_TAX_NAME,INM_TAX_TCS_AMT");
            string Condition = "";
            if (reportType == "Mult")
            {
                if (supp.ToUpper() == "TRUE")
                {
                    Condition = " AND INM_SUPPLEMENTORY=1 AND INM_IS_SUPPLIMENT=1 ";
                }
                else
                {
                    Condition = " AND INM_SUPPLEMENTORY=0 AND INM_IS_SUPPLIMENT=0 ";
                }
            }
            if (reportType == "Single")
            {
                //dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CPOM_PO_DATE as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='TAXINV'");
                // dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,   P_CST,P_PAN AS P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,'' as ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,ISNULL( IND_HSN_CODE,'') AS [HSN_NO] ,ISNULL( INM_HSN_CODE ,'') AS [EWAYBILL_NO] , IND_PACK_DESC AS  INM_ELECTRREFNUM,INM_ADDRESS,INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE ,STATE_MASTER.SM_NAME,ISNULL(STATE_MASTER.SM_STATE_CODE ,'') AS SM_STATE_CODE ,STATE_MASTER1.SM_NAME AS SM_NAME_CUST,ISNULL(STATE_MASTER1.SM_STATE_CODE ,'') AS SM_STATE_CODE_CUST FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER ,STATE_MASTER ,STATE_MASTER  AS STATE_MASTER1  WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE and INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='TAXINV' AND STATE_MASTER.SM_CODE=INVOICE_MASTER.INM_STATE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER1.SM_CODE=P_SM_CODE AND STATE_MASTER1.ES_DELETE=0");
                //after supplimentory colum added 
                dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,   P_CST,P_PAN AS P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,I_CODENO as ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_PO_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,ISNULL( IND_HSN_CODE,'') AS [HSN_NO] ,ISNULL( INM_HSN_CODE ,'') AS [EWAYBILL_NO] , IND_PACK_DESC AS  INM_ELECTRREFNUM,INM_ADDRESS,INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE ,STATE_MASTER.SM_NAME,ISNULL(STATE_MASTER.SM_STATE_CODE ,'') AS SM_STATE_CODE ,STATE_MASTER1.SM_NAME AS SM_NAME_CUST,ISNULL(STATE_MASTER1.SM_STATE_CODE ,'') AS SM_STATE_CODE_CUST,INM_LC_NO,	INM_LC_DATE,INM_IS_SUPPLIMENT, INM_SUPPLEMENTORY,  case when (INM_IS_SUPPLIMENT=1 and INM_SUPPLEMENTORY=1) THEN (SELECT IND.IND_RATE FROM INVOICE_MASTER IM,INVOICE_DETAIL IND WHERE IM.INM_CODE =IND.IND_INM_CODE AND IM.INM_TYPE='TAXINV' AND IM.INM_IS_SUPPLIMENT=0    AND IM.INM_SUPPLEMENTORY=0 AND IM.ES_DELETE=0  AND IM.INM_NO=INVOICE_MASTER.INM_LC_NO AND IND.IND_I_CODE=I_CODE AND   IM.INM_P_CODE=P_CODE) ELSE 0 END AS OLD_RATE,P_EMAIL FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER ,STATE_MASTER ,STATE_MASTER  AS STATE_MASTER1  WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE and INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='TAXINV'                                                                 AND STATE_MASTER.SM_CODE=INVOICE_MASTER.INM_STATE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER1.SM_CODE=P_SM_CODE AND STATE_MASTER1.ES_DELETE=0");
            }
            else
            {
                //dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME, I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CPOM_PO_DATE as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS   ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND INM_TYPE='TAXINV' and INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "'");
                dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,   P_CST,P_PAN AS P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,I_CODENO as ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_PO_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,ISNULL( IND_HSN_CODE,'') AS [HSN_NO] ,ISNULL( INM_HSN_CODE,'')  AS [EWAYBILL_NO] , IND_PACK_DESC AS  INM_ELECTRREFNUM,INM_ADDRESS,INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE ,STATE_MASTER.SM_NAME ,ISNULL(STATE_MASTER.SM_STATE_CODE ,'') AS SM_STATE_CODE ,STATE_MASTER1.SM_NAME AS SM_NAME_CUST,ISNULL(STATE_MASTER1.SM_STATE_CODE ,'') AS SM_STATE_CODE_CUST,INM_LC_NO,	INM_LC_DATE,INM_IS_SUPPLIMENT, INM_SUPPLEMENTORY,  case when (INM_IS_SUPPLIMENT=1 and INM_SUPPLEMENTORY=1) THEN (SELECT IND.IND_RATE FROM INVOICE_MASTER IM,INVOICE_DETAIL IND WHERE IM.INM_CODE =IND.IND_INM_CODE AND IM.INM_TYPE='TAXINV' AND IM.INM_IS_SUPPLIMENT=0    AND IM.INM_SUPPLEMENTORY=0 AND IM.ES_DELETE=0  AND IM.INM_NO=INVOICE_MASTER.INM_LC_NO AND IND.IND_I_CODE=I_CODE AND   IM.INM_P_CODE=P_CODE) ELSE 0 END AS OLD_RATE,P_EMAIL  FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER ,STATE_MASTER ,STATE_MASTER  AS STATE_MASTER1 WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV' " + Condition + " and INM_NO BETWEEN '" + invoice_code + "' AND '" + toNo + "' AND STATE_MASTER.SM_CODE=INVOICE_MASTER.INM_STATE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER1.SM_CODE=P_SM_CODE AND STATE_MASTER1.ES_DELETE=0 AND INM_CM_CODE='" + Session["CompanyCode"] + "'");
            }
            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");
            if (dtComp.Rows.Count > 0)
            {
                if (dtTaxInvoice.Rows.Count > 0)
                {
                    if (Cond == "1")
                    {
                        #region Cond_1_Printed_Material
                        dtTemp.Columns.Add("INM_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                        if (chkPrint1 == "1")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);
                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoice);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoice";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                            rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                            rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                            rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                            rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                            rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                            rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                            rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                            rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                            rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,INVOICE_MASTER where INM_CODE='" + code + "' and ISO_SCREEN_NO=81 and INM_DATE<=ISO_WEF_DATE and INM_DATE>=ISO_WEF_DATE");
                            rptname.SetParameterValue("txtCINNo", IsoNo);

                            //rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                            //rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        if (chkPrint1 == "2")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;

                            dtTemp.Rows.Add(dr);
                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoice);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoice";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                            rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                            rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                            rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                            rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                            rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                            rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                            rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                            rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                            rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,INVOICE_MASTER where INM_CODE='" + code + "' and ISO_SCREEN_NO=81 and INM_DATE<=ISO_WEF_DATE and INM_DATE>=ISO_WEF_DATE");
                            rptname.SetParameterValue("txtCINNo", IsoNo);
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        if (chkPrint1 == "3")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoice);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoice";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                            rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                            rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                            rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                            rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                            rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                            rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                            rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                            rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                            rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,INVOICE_MASTER where INM_CODE='" + code + "' and ISO_SCREEN_NO=81 and INM_DATE<=ISO_WEF_DATE and INM_DATE>=ISO_WEF_DATE");
                            rptname.SetParameterValue("txtCINNo", IsoNo);
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        if (chkPrint1 == "4")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Quaradruplicate";
                            dr["INM_SEQNO"] = 4;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoice);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoice";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                            rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                            rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                            rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                            rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                            rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                            rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                            rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                            rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                            rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                            rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,INVOICE_MASTER where INM_CODE='" + code + "' and ISO_SCREEN_NO=81 and INM_DATE<=ISO_WEF_DATE and INM_DATE>=ISO_WEF_DATE");
                            rptname.SetParameterValue("txtCINNo", IsoNo);
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        #endregion Printed_Material
                    }

                    #region Cond_2_Plain Print
                    if (Cond == "2")
                    {
                        DataTable dtTaxInvoiceGST = new DataTable();
                        dtTaxInvoiceGST = dtTaxInvoice;

                        dtTemp.Columns.Add("INM_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                        if (chkPrint1 == "1")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);
                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";


                        }
                        if (chkPrint1 == "2")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";


                        }
                        if (chkPrint1 == "3")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";


                        }
                        if (chkPrint1 == "4")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplication";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Quaradruplicate";
                            dr["INM_SEQNO"] = 4;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";


                        }

                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceHiematecGST1.rpt"));  //rptTaxInvoiceGST  //rptTaxInvoice  rptTaxInvoiceGST
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceHiematecGST1.rpt"); //rptTaxInvoiceHiematecGST for Hiematec

                        rptname.Refresh();
                        //rptname.SetDataSource(dtfinal);
                        rptname.SetDataSource(dsTaxInvoiceGST);
                        rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                        rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                        rptname.SetParameterValue("txtCompGSTIN", dtComp.Rows[0]["CM_GST_NO"].ToString());
                        rptname.SetParameterValue("txtCompGSTSTATECODE", dtComp.Rows[0]["CM_GST_NO"].ToString().Substring(0, 2));

                        rptname.SetParameterValue("txtCompCIN", dtComp.Rows[0]["CM_CIN_NO"].ToString());
                        rptname.SetParameterValue("txtCompTelPhone", dtComp.Rows[0]["CM_PHONENO1"].ToString());
                        rptname.SetParameterValue("txtCompMobile", dtComp.Rows[0]["CM_PHONENO2"].ToString());
                        rptname.SetParameterValue("txtCompEmail", dtComp.Rows[0]["CM_EMAILID"].ToString());
                        rptname.SetParameterValue("txtCompWebsite", dtComp.Rows[0]["CM_WEBSITE"].ToString());
                        rptname.SetParameterValue("txtCompPAN", dtComp.Rows[0]["CM_PAN_NO"].ToString());

                        //CrystalReportViewer1.ReportSource = rptname;


                        CrystalReportViewer1.ReportSource = rptname;
                        DateTime now = DateTime.Now;
                        string invoiceName = Server.MapPath("~/UpLoadPath/Invoice/" + dsTaxInvoiceGST.Tables[0].Rows[0]["INM_NO"]+"-"+now.Year+now.Month+now.Day+now.Hour+now.Minute+now.Second+now.Millisecond + "invoice.pdf");
                        string invoicenumber = dtComp.Rows[0]["CM_NAME"].ToString() + " Invoice " + dsTaxInvoiceGST.Tables[0].Rows[0]["INM_NO"] +" Date:"+now.Day+"/"+now.Month+"/"+now.Year;
                        rptname.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, invoiceName);
                        SendEmail(dsTaxInvoiceGST.Tables[0].Rows[0]["P_EMAIL"].ToString(), invoiceName, dsTaxInvoiceGST.Tables[0].Rows[0]["P_NAME"].ToString(), dtComp.Rows[0]["CM_NAME"].ToString(), invoicenumber);
                        //view pdf on web server
                        //path = Server.MapPath("~/UpLoadPath/report.pdf");

                        //for local host
                        //path = ("../../UpLoadPath/report.pdf");
                        //iframe1.Attributes["src"] = path;
                    }
                    #endregion Cond_2_Plain Print

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
    }
    #endregion

    public void SendEmail(string tomail,string attachment,string pname,string cmname,string subject)
    {
        string FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
        string ToEmail = tomail;//ConfigurationManager.AppSettings["ToEmail"].ToString();
        string Subject = subject;// ConfigurationManager.AppSettings["Subject"].ToString();
                
                string password = ConfigurationManager.AppSettings["networkCredential"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                using (MailMessage mail = new MailMessage(FromEmail, ToEmail))
                {
                    mail.Subject = Subject;
                    string htmlString = @"<html>
                      <body>
                      <p>Hi " +pname+ @",</p>
                      <p>We are contacting you in regards to the new invoice raised that has been created on your account.</p>
<p>Please see the attached invoice and get back to us in case if you have queries.</p>
                      <p>Sincerely,<br><br>" + cmname + @"</br></p>
                      </body>
                      </html>
                     ";
                    mail.Body = htmlString;
                    mail.Attachments.Add(new Attachment(attachment));

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(FromEmail, password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Convert.ToInt32(port);
                    smtp.Send(mail);
                }
    }
}
