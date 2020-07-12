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
public partial class RoportForms_ADD_LabourChargeInvoicePrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        invoice_code = Request.QueryString[0];
        reportType = Request.QueryString[1];
        string ptype = Request.QueryString[2];
        GenerateReport(invoice_code, ptype);
    }

    #region GenerateReport
    private void GenerateReport(string code, string type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            //        DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_TAX_NAME,ST_SALES_TAX FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX");        

            //DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "'  group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO ,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME ,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE ,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX ,E_TARIFF_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,ST_TAX_NAME,INM_TAX_TCS_AMT");
            DataTable dtfinal = new DataTable();
            if (reportType == "Single")
            {
                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CPOM_PO_DATE as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,(case when P_SM_CODE=CM_STATE then EXCISE_TARIFF_MASTER.E_BASIC else 0 end) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,(case when P_SM_CODE=CM_STATE then EXCISE_TARIFF_MASTER.E_EDU_CESS else 0 end) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,(case when P_SM_CODE<>CM_STATE then EXCISE_TARIFF_MASTER.E_H_EDU else 0 end) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,isnull(INM_ADDRESS_SELECTED,0) as INM_ADDRESS_SELECTED FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' AND  INM_TYPE='OutJWINM' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INM_ADDRESS_SELECTED=" + Request.QueryString[3].ToString() + "");
            }
            else
            {
                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME, I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CPOM_PO_DATE as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,(case when P_SM_CODE=CM_STATE then EXCISE_TARIFF_MASTER.E_BASIC else 0 end) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,(case when P_SM_CODE=CM_STATE then EXCISE_TARIFF_MASTER.E_EDU_CESS else 0 end) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,(case when P_SM_CODE<>CM_STATE then EXCISE_TARIFF_MASTER.E_H_EDU else 0 end) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_HSN_CODE AS IND_E_TARIFF_NO,isnull(INM_ADDRESS_SELECTED,0) as INM_ADDRESS_SELECTED  FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,COMPANY_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER WHERE INM_CM_CODE = COMPANY_MASTER.CM_CODE and INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_SCAT_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND  INM_TYPE='OutJWINM'  AND P_SM_CODE=SM_CODE  and INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and INM_ADDRESS_SELECTED=" + Request.QueryString[3].ToString() + "");
            }
            DataTable dtCompGSTNo = CommonClasses.Execute("SELECT CM_GST_NO,SM_NAME,CM_NAME,CM_ADDRESS1,CM_ADDRESS2 FROM COMPANY_MASTER,STATE_MASTER where SM_CODE=CM_STATE AND  CM_ID=" + Session["CompanyId"].ToString() + "");

            if (dtfinal.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (type == "1")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");


                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
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
                    //string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,INVOICE_MASTER where INM_CODE='" + code + "' and ISO_SCREEN_NO=81 and INM_DATE<=ISO_WEF_DATE and INM_DATE>=ISO_WEF_DATE");
                    string CINNo = DL_DBAccess.GetColumn("select CM_CIN_NO from COMPANY_MASTER where CM_ID='" + Session["CompanyId"] + "'");
                    rptname.SetParameterValue("txtCINNo", CINNo);
                }

                else
                {
                    DataTable dtTaxInvoiceGST = new DataTable();
                    dtTaxInvoiceGST = dtfinal;

                    DataTable dtTemp = new DataTable();
                    DataSet dsTaxInvoiceGST = new DataSet();
                    dtTemp.Columns.Add("INM_CODE", typeof(int));
                    dtTemp.Columns.Add("INM_NO", typeof(int));
                    dtTemp.Columns.Add("INM_NAME", typeof(string));
                    dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                    DataRow dr = dtTemp.NewRow();
                    dr["INM_CODE"] = code;
                    dr["INM_NO"] = 1;
                    dr["INM_NAME"] = "Original For Recipient";
                    dr["INM_SEQNO"] = 1;
                    dtTemp.Rows.Add(dr);

                    dr = dtTemp.NewRow();
                    dr["INM_CODE"] = code;
                    dr["INM_NO"] = 1;
                    dr["INM_NAME"] = "Duplicate For Transporter";
                    dr["INM_SEQNO"] = 2;
                    dtTemp.Rows.Add(dr);

                    dr = dtTemp.NewRow();
                    dr["INM_CODE"] = code;
                    dr["INM_NO"] = 1;
                    dr["INM_NAME"] = "Triplicate for Supplier";
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


                    rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGSTLabour.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGSTLabour.rpt");

                    rptname.SetDataSource(dsTaxInvoiceGST);
                    rptname.SetParameterValue("txtCompName", dtCompGSTNo.Rows[0]["CM_NAME"].ToString());
                    if (dtfinal.Rows[0]["INM_ADDRESS_SELECTED"].ToString() == "1")
                    {
                        rptname.SetParameterValue("txtCompAdd", dtCompGSTNo.Rows[0]["CM_ADDRESS2"].ToString());
                    }
                    else
                    {
                        rptname.SetParameterValue("txtCompAdd", dtCompGSTNo.Rows[0]["CM_ADDRESS1"].ToString()); 
                    }
                    rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0]["CM_GST_NO"].ToString());
                    rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                }
                CrystalReportViewer1.ReportSource = rptname;

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
        Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
    }
    #endregion
}
