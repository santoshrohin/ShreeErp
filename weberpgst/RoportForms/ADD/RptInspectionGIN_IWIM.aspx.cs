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
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_RptInspectionGIN_IWIM : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Type = "";
    string PrintType = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Code = Request.QueryString[1];

            Type = Request.QueryString[2].ToString();
            PrintType = Request.QueryString[3].ToString();

            GenerateReport(Title, Code, PrintType, Type);
        }
        catch (Exception Ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = Ex.Message;
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Code, string PrintType1, string Type1)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWD_IWM_CODE=" + Code + " join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
            if (Type1 == "SIWM")
            {
                if (PrintType1 == "Mult")
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "' and IWM.ES_DELETE=0 join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "'   and LOG_MASTER.LG_SOURCE= 'Material Inward'  and IWD_INSP_FLG=1 order by IWM_NO --and LG_EVENT='Insert'  ";
                    Query = " select distinct SRPOM_CODE AS SPOM_CODE,P_NAME as LM_NAME ,SID_BATCH_NO AS IWD_BATCH_NO,SIM_INV_NO AS IWM_INV_NO,SIM_INV_DATE AS IWM_INV_DATE,SIM_TRANSPORATOR_NAME AS IWM_TRANSPORATOR_NAME,S_CODENO AS I_CODENO, SIM_LR_NO   AS   IWM_LR_NO,SRPOM_DATE AS SPOM_DATE,P_ADD1 AS LM_ADD1,SIM_NO AS IWM_NO,SIM_DATE AS IWM_DATE,SIM_CHALLAN_NO AS IWM_CHALLAN_NO,SIM_CHAL_DATE AS IWM_CHAL_DATE,SRPOM_PO_NO AS SPOM_PO_NO,S_NAME AS I_NAME,SID_CH_QTY AS IWD_CH_QTY,SID_REV_QTY AS IWD_REV_QTY,SID_REV_QTY AS IWD_CON_OK_QTY,SID_CON_REJ_QTY AS IWD_CON_REJ_QTY,SID_RATE AS IWD_RATE,SID_REMARK AS IWD_REMARK,'NA' AS UOM_NAME,' ' AS IWD_INSP_NO,SID_INSP_FLG AS IWD_INSP_FLG,LG_U_NAME AS LG_U_NAME,LG_U_CODE AS LG_U_CODE,UM_NAME AS UM_NAME  FROM SERVICE_INWARD_MASTER AS IWM INNER JOIN SERVICE_INWARD_DETAIL AS IND ON IWM.SIM_CODE = IND.SID_SIM_CODE AND IWM.ES_DELETE = 0 INNER JOIN SERVICE_TYPE_MASTER ON IND.SID_I_CODE = SERVICE_TYPE_MASTER.S_CODE INNER JOIN PARTY_MASTER ON IWM.SIM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SERVICE_PO_MASTER ON SERVICE_PO_MASTER.SRPOM_CODE = IND.SID_CPOM_CODE INNER JOIN   LOG_MASTER ON LOG_MASTER.LG_DOC_CODE = IWM.SIM_CODE INNER JOIN  USER_MASTER ON LOG_MASTER.LG_U_CODE = USER_MASTER.UM_CODE WHERE (  INM.SIM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "') AND (LOG_MASTER.LG_SOURCE = 'Service Inward ')";
                }
                else
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWD_IWM_CODE=" + Code + " join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    Query = "  select distinct  SRPOM_CODE AS SPOM_CODE,P_NAME as LM_NAME ,SID_BATCH_NO AS IWD_BATCH_NO,SIM_INV_NO AS IWM_INV_NO,SIM_INV_DATE AS IWM_INV_DATE,SIM_TRANSPORATOR_NAME AS IWM_TRANSPORATOR_NAME,S_CODENO AS I_CODENO, SIM_LR_NO   AS   IWM_LR_NO,SRPOM_DATE AS SPOM_DATE,P_ADD1 AS LM_ADD1,SIM_NO AS IWM_NO,SIM_DATE AS IWM_DATE,SIM_CHALLAN_NO AS IWM_CHALLAN_NO,SIM_CHAL_DATE AS IWM_CHAL_DATE,SRPOM_PO_NO AS SPOM_PO_NO,S_NAME AS I_NAME,SID_CH_QTY AS IWD_CH_QTY,SID_REV_QTY AS IWD_REV_QTY,SID_REV_QTY AS IWD_CON_OK_QTY,SID_CON_REJ_QTY AS IWD_CON_REJ_QTY,SID_RATE AS IWD_RATE,SID_REMARK AS IWD_REMARK,'NA' AS UOM_NAME,' ' AS IWD_INSP_NO,SID_INSP_FLG AS IWD_INSP_FLG,LG_U_NAME AS LG_U_NAME,LG_U_CODE AS LG_U_CODE,UM_NAME AS UM_NAME   FROM SERVICE_INWARD_MASTER AS IWM INNER JOIN SERVICE_INWARD_DETAIL AS IND ON IWM.SIM_CODE = IND.SID_SIM_CODE AND IWM.ES_DELETE = 0 INNER JOIN SERVICE_TYPE_MASTER ON IND.SID_I_CODE = SERVICE_TYPE_MASTER.S_CODE INNER JOIN PARTY_MASTER ON IWM.SIM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SERVICE_PO_MASTER ON SERVICE_PO_MASTER.SRPOM_CODE = IND.SID_CPOM_CODE INNER JOIN   LOG_MASTER ON LOG_MASTER.LG_DOC_CODE = IWM.SIM_CODE INNER JOIN  USER_MASTER ON LOG_MASTER.LG_U_CODE = USER_MASTER.UM_CODE WHERE (IND.SID_SIM_CODE = '" + Code + "') AND (LOG_MASTER.LG_SOURCE = 'Service Inward ')";
                }

            }
            else
            {


                if (PrintType1 == "Mult")
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "' and IWM.ES_DELETE=0 join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "'   and LOG_MASTER.LG_SOURCE= 'Material Inward'  and IWD_INSP_FLG=1 order by IWM_NO --and LG_EVENT='Insert'  ";
                }
                else
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWD_IWM_CODE=" + Code + " join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWD_IWM_CODE=" + Code + "   and LOG_MASTER.LG_SOURCE= 'Material Inward'  and IWD_INSP_FLG=1 --and LG_EVENT='Insert'--order by IWM_NO";
                }
            }
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {
                rptname.Load(Server.MapPath("~/Reports/RptInspectionGIN_IWIM.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/RptInspectionGIN_IWIM.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txttitle", Session["CompanyName"].ToString());
                rptname.SetParameterValue("CompAdd", Session["CompanyAdd"].ToString());

                if (Type1.Trim() == "IWCP")
                {
                    rptname.SetParameterValue("Type", "Goods Inward Note   (Cash)");
                }
                if (Type1.Trim() == "IWIFP")
                {
                    rptname.SetParameterValue("Type", "Goods Inward Note  (For Process Inward)");
                }
                if (Type1.Trim() == "IWIM")
                {
                    rptname.SetParameterValue("Type", "Goods Inward Note  (Raw Material)");
                }
                if (Type1.Trim() == "OUTCUSTINV")
                {
                    rptname.SetParameterValue("Type", "Goods Inward Note  (Sub Contractor)");
                }
                if (Type1.Trim() == "SIWM")
                {
                    rptname.SetParameterValue("Type", "Service Inward Note ");
                }
                CrystalReportViewer1.ReportSource = rptname;



            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region Cancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
    }
    #endregion
}
