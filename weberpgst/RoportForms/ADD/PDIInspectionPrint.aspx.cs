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

public partial class RoportForms_ADD_PDIInspectionPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Code = "", Type = "", IWM_CODE = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        Type = Request.QueryString[0];
        Code = Request.QueryString[1];
        IWM_CODE = Request.QueryString[2];
        GenerateReport(Type, Code);
    }
    #endregion Page_Init

    #region GenerateReport
    private void GenerateReport(string Type, string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtfinal = new DataTable();
        if (Type == "0")//
        {
            //Material Inspection Print
            //dtfinal = CommonClasses.Execute("select distinct INSPDI_CODE,INSD_RM_CODE as Reason,INSM_REMARK,INSM_REMARK,IWM_NO as INM_NO,IWM_CHALLAN_NO,convert(varchar,IWM_DATE,106) as INM_DATE,IWD_CH_QTY,INSPDI_INSM_CODE,P_NAME,I_NAME,I_CODENO,INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,INSM_CON_RECEIVED_QTY,INSM_CON_OK_QTY,INSM_CON_REJ_QTY,INSM_CON_SCRAP_QTY from INSPECTION_PDI_DETAIL,ITEM_MASTER,PARTY_MASTER,INWARD_MASTER,INSPECTION_S_MASTER,INWARD_DETAIL where P_CODE=IWM_P_CODE and I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and IWM_CODE=IWD_IWM_CODE and INSM_CODE=INSPDI_INSM_CODE and ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND INSPECTION_S_MASTER.ES_DELETE=0 and I_CODE='" + code + "' order by I_CODENO");
            //dtfinal = CommonClasses.Execute("select distinct INSPDI_CODE,INSD_RM_CODE as Reason,INSM_REMARK,INSM_REMARK,IWM_NO as INM_NO,IWM_CHALLAN_NO,convert(varchar,IWM_DATE,106) as INM_DATE,IWD_CH_QTY,INSPDI_INSM_CODE,P_NAME,I_NAME,I_CODENO,INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,INSM_CON_RECEIVED_QTY,INSM_CON_OK_QTY,INSM_CON_REJ_QTY,INSM_CON_SCRAP_QTY from INSPECTION_PDI_DETAIL,ITEM_MASTER,PARTY_MASTER,INWARD_MASTER,INSPECTION_S_MASTER,INWARD_DETAIL,INSPECTION_S_DETAIL where P_CODE=IWM_P_CODE and I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and IWM_CODE=IWD_IWM_CODE and INSM_CODE=INSPDI_INSM_CODE and ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND INSPECTION_S_MASTER.ES_DELETE=0 and INSM_CODE='" + code + "' and INSD_INSM_CODE=INSM_CODE order by I_CODENO");
            dtfinal = CommonClasses.Execute("select distinct INSPDI_CODE,isnull((SELECT INSPECTION_S_DETAIL.INSD_RM_CODE FROM INSPECTION_S_DETAIL RIGHT OUTER JOIN INSPECTION_S_MASTER ON INSPECTION_S_DETAIL.INSD_INSM_CODE = INSPECTION_S_MASTER.INSM_CODE WHERE (INSPECTION_S_MASTER.INSM_CODE = INSPECTION.INSM_CODE)),'') as Reason, INSM_REMARK,IWM_NO as INM_NO,IWM_CHALLAN_NO,convert(varchar,IWM_DATE,106) as INM_DATE,IWD_CH_QTY,INSPDI_INSM_CODE,P_NAME,I_NAME,I_CODENO,INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY,INSM_CON_RECEIVED_QTY,INSM_CON_OK_QTY,INSM_CON_REJ_QTY,INSM_CON_SCRAP_QTY from INSPECTION_PDI_DETAIL,ITEM_MASTER,PARTY_MASTER,INWARD_MASTER,INSPECTION_S_MASTER as INSPECTION,INWARD_DETAIL where P_CODE=IWM_P_CODE and I_CODE=INSM_I_CODE and INSM_IWM_CODE=IWM_CODE and IWM_CODE=IWD_IWM_CODE and INSM_CODE=INSPDI_INSM_CODE and ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND INSPECTION.ES_DELETE=0 and I_CODE='" + code + "' and IWM_CODE=" + IWM_CODE + "  order by I_CODENO");
        }
        else
        {
            //PDI Detail Print
            dtfinal = CommonClasses.Execute("select distinct INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,INSPDI_INSM_CODE,P_NAME,I_CODE,I_NAME,I_CODENO,INSPDI_CODE,INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSPDI_REMARK,IND_INQTY as INSM_RECEIVED_QTY,IND_CON_QTY as INSM_CON_RECEIVED_QTY,IND_QTY_PACK as INSM_OK_QTY,INSPDI_NOTE from INSPECTION_PDI_DETAIL,INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,PARTY_MASTER where INVOICE_MASTER.ES_DELETE=0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INM_CODE=IND_INM_CODE and INSPDI_INSM_CODE=INM_CODE and INM_P_CODE=P_CODE and I_CODE=INSPDI_I_CODE and INM_CM_CODE='" + Session["CompanyCode"] + "' and INSPDI_INSM_CODE=" + code + " AND INSPDI_I_CODE=IND_I_CODE");
        }

        if (dtfinal.Rows.Count > 0)
        {
        }
        try
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            if (Type == "0")
            {
                //Material Inspection Print
                rptname.Load(Server.MapPath("~/Reports/rptPDIInspectionDetails.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptPDIInspectionDetails.rpt");
            }
            else
            {
                //PDI Detail Print
                rptname.Load(Server.MapPath("~/Reports/rptPDIDetails.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptPDIDetails.rpt");
            }

            rptname.Refresh();
            rptname.SetDataSource(dtfinal);

            rptname.SetParameterValue("txtCName", Session["CompanyName"].ToString());

            CrystalReportViewer1.ReportSource = rptname;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PDI Print", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString[0] == "0")
        {
            Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
        }
        else
        {
            Response.Redirect("~/Transactions/View/ViewPDIDetails.aspx", false);
        }

    }
    #endregion
}