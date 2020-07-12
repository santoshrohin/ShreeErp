using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

public partial class RoportForms_ADD_DispToSubContPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    string rrreportType = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        invoice_code = Request.QueryString[0];
        reportType = Request.QueryString[1];
        rrreportType = Request.QueryString[2];
        GenerateReport(invoice_code, rrreportType);
    }


    #region GenerateReport
    private void GenerateReport(string code, string Cond)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            // DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_TAX_NAME,ST_SALES_TAX FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX");        
            //DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "'  group by I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO ,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME ,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE ,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX ,E_TARIFF_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,ST_TAX_NAME,INM_TAX_TCS_AMT");
            DataTable dtfinal = new DataTable();
            if (reportType == "Single")
            {
                //dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE");

                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME, IND_CON_QTY AS  IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_CENTRAL_TAX_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_STATE_TAX_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_INTEGRATED_TAX_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,E_BASIC, E_EDU_CESS, E_H_EDU ,(SELECT I_UOM_NAME FROM ITEM_UNIT_MASTER where I_UOM_CODE=SPOD_RATE_UOM ) AS CONVUNIT  FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE  AND INM_CM_CODE=" + (string)Session["CompanyCode"] + " and IND_I_CODE = I_CODE AND INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE and   SPOD_I_CODE=IND_I_CODE  AND INVOICE_MASTER.ES_DELETE=0  ");
            }
            else
            {
                //dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='OutSUBINM' AND INVOICE_MASTER.ES_DELETE=0 AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "'");
                //dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 and   SPOD_I_CODE=IND_I_CODE ");
                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME, IND_CON_QTY AS  IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_CENTRAL_TAX_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_STATE_TAX_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_INTEGRATED_TAX_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,E_BASIC, E_EDU_CESS, E_H_EDU,(SELECT I_UOM_NAME FROM ITEM_UNIT_MASTER where I_UOM_CODE=SPOD_RATE_UOM ) AS CONVUNIT FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE  AND INM_CM_CODE=" + (string)Session["CompanyCode"] + "  and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 and   SPOD_I_CODE=IND_I_CODE  AND INM_INVOICE_TYPE=1 UNION SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT, 0 as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,0 AS SPOD_EXC_AMT,0 AS SPOD_EXC_E_AMT,0 AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,E_BASIC, E_EDU_CESS, E_H_EDU,''AS CONVUNIT FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER,BOM_MASTER,BOM_DETAIL WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE  AND INM_CM_CODE=" + (string)Session["CompanyCode"] + "  AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_INVOICE_TYPE=0 and BD_BM_CODE=BM_CODE and BD_I_CODE=IND_I_CODE AND SPOD_I_CODE=BM_I_CODE");
            }

            if (dtfinal.Rows.Count > 0)
            {

                DataTable dtTemp = new DataTable();
                DataSet ds57A4 = new DataSet();
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");

                if (rrreportType == "1")
                {

                    rptname.Load(Server.MapPath("~/Reports/rptDipToSubCont.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDipToSubCont.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    //rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    //rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    //rptname.SetParameterValue("txtGSTNo", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    //rptname.SetParameterValue("txtUser", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;

                }
                else
                {
                    #region Cond_2_Plain Print

                    //dtTemp.Columns.Add("INM_CODE", typeof(int));
                    //dtTemp.Columns.Add("INM_NO", typeof(int));
                    //dtTemp.Columns.Add("INM_NAME", typeof(string));
                    //dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                    DataTable dt57A4 = new DataTable();
                    dt57A4 = dtfinal;

                    dtTemp.Columns.Add("INM_CODE", typeof(int));
                    dtTemp.Columns.Add("INM_NO", typeof(int));
                    dtTemp.Columns.Add("INM_NAME", typeof(string));
                    dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                    int x = Convert.ToInt32(invoice_code);
                    int y = Convert.ToInt32(reportType);


                    for (; x <= y; x++)
                    {
                        DataTable dtInm = CommonClasses.Execute(" SELECT * FROM INVOICE_MASTER where INM_TYPE='OutSUBINM' AND ES_DELETE=0 AND INM_NO ='" + x.ToString() + "'");
                        code = dtInm.Rows[0]["INM_CODE"].ToString();

                        DataRow dr = dtTemp.NewRow();
                        dr["INM_CODE"] = code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Original For Job work";
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
                        dr["INM_NAME"] = "Acknowledgement";
                        dr["INM_SEQNO"] = 3;
                        dtTemp.Rows.Add(dr);

                        //dr = dtTemp.NewRow();
                        //dr["INM_CODE"] = code;
                        //dr["INM_NO"] = 1;
                        //dr["INM_NAME"] = "Quaradruplicate";
                        //dr["INM_SEQNO"] = 4;
                        //dtTemp.Rows.Add(dr);
                    }
                    ds57A4.Tables.Add(dt57A4);
                    ds57A4.Tables[0].TableName = "dtDispToSubCont";
                    DataTable dta = new DataTable();
                    dta = (DataTable)dtTemp;
                    ds57A4.Tables.Add(dta);
                    ds57A4.Tables[1].TableName = "NoOfCopy57A4";

                    // Create a DataRow, add Name and Age data, and add to the DataTable
                    rptname.Load(Server.MapPath("~/Reports/JobWorkChallan.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/JobWorkChallan.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(ds57A4);
                    rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    rptname.SetParameterValue("txtGSTNo", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    rptname.SetParameterValue("txtUser", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;


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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
    }
}
