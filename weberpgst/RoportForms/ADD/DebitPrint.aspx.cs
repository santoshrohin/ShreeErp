﻿using System;
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

public partial class RoportForms_ADD_DebitPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Credit_Code = "";
    string reportType = "";
    string toNo = "";

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        Credit_Code = Request.QueryString[0];
        string Title = Request.QueryString[1];
        //Cond = Request.QueryString[1];
        GenerateReport(Credit_Code);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewDebitNote.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtDebitNote = new DataTable();
        DataTable dtTemp = new DataTable();
        DataSet dsDebitNote = new DataSet();
        ReportDocument rptname = null;
        rptname = new ReportDocument();
        try
        {
            //dtDebitNote = CommonClasses.Execute("SELECT DISTINCT(I_CODE),DNM_SERIAL_NO,ISNULL(DNM_GSTIN_NO,'') as DNM_GSTIN_NO,ISNULL(DNM_EWAY_BILL_NO,'') as DNM_EWAY_BILL_NO,CONVERT(VARCHAR,DNM_DATE,103) AS DNM_DATE ,DNM_CUST_ADDRESS,DNM_CUST_STATE_NAME,DNM_CUST_STATE_CODE, CAST(ISNULL(DNM_GRAND_TOTAL,0) AS NUMERIC(20,2)) AS DNM_GRAND_TOTAL,CAST(ISNULL(DNM_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_CENTRAL_TAX_AMT, CAST(ISNULL(DNM_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_STATE_UNION_TAX_AMT,CAST(ISNULL(DNM_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_INTEGRATED_TAX_AMT, CAST(ISNULL(DNM_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_CESS_TAX_AMT,CAST(ISNULL(DNM_NET_AMOUNT,0) AS NUMERIC(20,2)) AS DNM_NET_AMOUNT, CAST(ISNULL(DND_QTY,0) AS NUMERIC (20,2)) AS DND_QTY, CAST(ISNULL(DND_RATE,0) AS NUMERIC(20,2)) AS DND_RATE,CAST(ISNULL(DND_AMOUNT,0) AS NUMERIC(20,2)) AS DND_AMOUNT,	CAST(ISNULL(DND_CENTRAL_TAX,0) AS NUMERIC(20,2)) AS DND_CENTRAL_TAX, CAST(ISNULL(DND_STATE_UNION_TAX,0) AS NUMERIC(20,2)) AS DND_STATE_UNION_TAX, CAST(ISNULL(DND_INTEGRATED_TAX,0) AS NUMERIC(20,2)) AS DND_INTEGRATED_TAX,CAST(ISNULL(DND_CESS_TAX,0) AS NUMERIC(20,2)) AS DND_CESS_TAX,CAST(ISNULL(DND_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_CENTRAL_TAX_AMT, CAST(ISNULL(DND_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_STATE_UNION_TAX_AMT,	CAST(ISNULL(DND_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_INTEGRATED_TAX_AMT, 	CAST(ISNULL(DND_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_CESS_TAX_AMT, P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,ISNULL(P_LBT_NO,'') AS P_GST_NO ,P_PIN_CODE,P_PARTY_CODE,E_TARIFF_NO ,SM_NAME,SM_STATE_CODE ,I_CODENO,I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME FROM DEBIT_NOTE_MASTER,DEBIT_NOTE_DETAILS,PARTY_MASTER ,ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL,EXCISE_TARIFF_MASTER,ITEM_UNIT_MASTER,STATE_MASTER WHERE  DEBIT_NOTE_MASTER.DNM_CODE=DEBIT_NOTE_DETAILS.DND_DNM_CODE  AND DEBIT_NOTE_MASTER.DNM_CUST_CODE=PARTY_MASTER.P_CODE AND ITEM_MASTER.I_CODE=DEBIT_NOTE_DETAILS.DND_ITEM_CODE AND CUSTPO_MASTER.CPOM_CODE=CUSTPO_DETAIL.CPOD_CPOM_CODE AND DEBIT_NOTE_MASTER.DNM_CUST_CODE=CUSTPO_MASTER.CPOM_P_CODE AND DEBIT_NOTE_DETAILS.DND_ITEM_CODE = CUSTPO_DETAIL.CPOD_I_CODE AND ITEM_MASTER.I_E_CODE=EXCISE_TARIFF_MASTER.E_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER.SM_CODE=P_SM_CODE AND  DND_DNM_CODE='" + Credit_Code + "' ");
            dtDebitNote = CommonClasses.Execute("SELECT DISTINCT(I_CODE),DNM_SERIAL_NO,ISNULL(DNM_GSTIN_NO,'') as DNM_GSTIN_NO,ISNULL(DNM_EWAY_BILL_NO,'') as DNM_EWAY_BILL_NO,CONVERT(VARCHAR,DNM_DATE,103) AS DNM_DATE ,DNM_CUST_ADDRESS,DNM_CUST_STATE_NAME,DNM_CUST_STATE_CODE, CAST(ISNULL(DNM_GRAND_TOTAL,0) AS NUMERIC(20,2)) AS DNM_GRAND_TOTAL,CAST(ISNULL(DNM_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_CENTRAL_TAX_AMT, CAST(ISNULL(DNM_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_STATE_UNION_TAX_AMT,CAST(ISNULL(DNM_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_INTEGRATED_TAX_AMT, CAST(ISNULL(DNM_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS DNM_CESS_TAX_AMT,CAST(ISNULL(DNM_NET_AMOUNT,0) AS NUMERIC(20,2)) AS DNM_NET_AMOUNT, CAST(ISNULL(DND_QTY,0) AS NUMERIC (20,2)) AS DND_QTY, CAST(ISNULL(DND_RATE,0) AS NUMERIC(20,2)) AS DND_RATE,CAST(ISNULL(DND_AMOUNT,0) AS NUMERIC(20,2)) AS DND_AMOUNT,	CAST(ISNULL(DND_CENTRAL_TAX,0) AS NUMERIC(20,2)) AS DND_CENTRAL_TAX, CAST(ISNULL(DND_STATE_UNION_TAX,0) AS NUMERIC(20,2)) AS DND_STATE_UNION_TAX, CAST(ISNULL(DND_INTEGRATED_TAX,0) AS NUMERIC(20,2)) AS DND_INTEGRATED_TAX,CAST(ISNULL(DND_CESS_TAX,0) AS NUMERIC(20,2)) AS DND_CESS_TAX,CAST(ISNULL(DND_CENTRAL_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_CENTRAL_TAX_AMT, CAST(ISNULL(DND_STATE_UNION_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_STATE_UNION_TAX_AMT,	CAST(ISNULL(DND_INTEGRATED_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_INTEGRATED_TAX_AMT, 	CAST(ISNULL(DND_CESS_TAX_AMT,0) AS NUMERIC(20,2)) AS DND_CESS_TAX_AMT, P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,ISNULL(P_LBT_NO,'') AS P_GST_NO ,P_PIN_CODE,P_PARTY_CODE,E_TARIFF_NO ,SM_NAME,SM_STATE_CODE ,I_CODENO,I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME FROM DEBIT_NOTE_MASTER,DEBIT_NOTE_DETAILS,PARTY_MASTER ,ITEM_MASTER,EXCISE_TARIFF_MASTER,ITEM_UNIT_MASTER,STATE_MASTER WHERE  DEBIT_NOTE_MASTER.DNM_CODE=DEBIT_NOTE_DETAILS.DND_DNM_CODE  AND DEBIT_NOTE_MASTER.DNM_CUST_CODE=PARTY_MASTER.P_CODE AND ITEM_MASTER.I_CODE=DEBIT_NOTE_DETAILS.DND_ITEM_CODE AND ITEM_MASTER.I_E_CODE=EXCISE_TARIFF_MASTER.E_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND STATE_MASTER.ES_DELETE=0 AND STATE_MASTER.SM_CODE=P_SM_CODE AND  DND_DNM_CODE='" + Credit_Code + "' ");
            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");
            if (dtComp.Rows.Count > 0)
            {
                if (dtDebitNote.Rows.Count > 0)
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDebitNote.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDebitNote.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtDebitNote);

                    rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    rptname.SetParameterValue("txtCompGSTIN", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Debit Note Print", "GenerateReport", Ex.Message);
        }
    }
    #endregion

}