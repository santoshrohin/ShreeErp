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

public partial class RoportForms_ADD_StockAdjustmentRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            // bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            // bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool ChkAllitem = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //string p_name = Request.QueryString[6].ToString();
            //string i_name = Request.QueryString[5].ToString();
            string group = Request.QueryString[3].ToString();
             string way = Request.QueryString[4].ToString();
            string cond = Request.QueryString[5].ToString();
            // i_name = i_name.Replace("'", "''");




            //if (group == "Datewise")
            //{

            //    if (ChkAllDate == true  && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "All", "All", From, To,  i_name, group);
            //    }
            //    if (ChkAllDate != true  && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", From, To,  i_name, group);
            //    }
            //    if (ChkAllDate != true  && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "ONE", "All", From, To,  i_name, group);
            //    }
            //    if (ChkAllDate == true  && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "All", "ONE", From, To,  i_name, group);
            //    }
            //}
            //if (group == "ItemWise")
            //{
            //    if (ChkAllDate == true && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "All", "All", From, To, i_name, group);
            //    }
            //    if (ChkAllDate != true && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group);
            //    }
            //    if (ChkAllDate != true && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "ONE", "All", From, To, i_name, group);
            //    }
            //    if (ChkAllDate == true && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "All", "ONE", From, To, i_name, group);
            //    }
            //}
            //if (group == "CustWise")
            //{

            //    if (ChkAllDate == true && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "All", "All", From, To, i_name, group);
            //    }
            //    if (ChkAllDate != true && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", From, To, i_name, group);
            //    }
            //    if (ChkAllDate != true && ChkAllitem == true)
            //    {
            //        GenerateReport(Title, "ONE", "All", From, To, i_name, group);
            //    }
            //    if (ChkAllDate == true && ChkAllitem != true)
            //    {
            //        GenerateReport(Title, "All", "ONE", From, To, i_name, group);
            //    }
            //}
            GenerateReport(Title, From, To, group, cond);
        }


        catch (Exception Ex)
        {

        }
    }

    #endregion


    private void GenerateReport(string Title, string From, string To, string group, string cond)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "SELECT CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT FROM PARTY_MASTER INNER JOIN CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN LOG_MASTER ON CUSTPO_MASTER.CPOM_CODE = LOG_MASTER.LG_DOC_CODE ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_MASTER ON CUSTPO_DETAIL.CPOD_I_CODE = ITEM_MASTER.I_CODE where CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";

            Query = "SELECT SAM_DATE,SAM_DOC_NO,isnull(cast((case when SAD_ADJUSTMENT_QTY > 0 THEN SAD_ADJUSTMENT_QTY end) as numeric(10,3)),0) as STOCK_IN,isnull(ABS(cast((case when SAD_ADJUSTMENT_QTY < 0 THEN SAD_ADJUSTMENT_QTY end) as numeric(10,3))),0) as STOCK_OUT,SAD_REMARK,SAD_I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,STOCK_ADJUSTMENT_DETAIL,STOCK_ADJUSTMENT_MASTER WHERE  " + cond + "  SAM_CODE=SAD_SAM_CODE AND SAD_I_CODE=I_CODE";

            #region MyRegion
            /*
            #region Datewise
            if (group == "Datewise")
            {

                if (date1 == "All" && item == "All")
                {
                    Query = Query;

                }
                if (date1 == "All" && item != "All")
                {
                    Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                }
                if (date1 != "All" && item != "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                }
                if (date1 != "All" && item == "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                }
            }
            #endregion

            #region ItemWise
            if (group == "ItemWise")
            {
                if (date1 == "All" && item == "All")
                {
                    Query = Query;

                }


                if (date1 == "All" && item != "All")
                {
                    Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                }
                if (date1 != "All" && item != "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                }
                if (date1 != "All" && item == "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                }

            }
            #endregion

            #region CustWise
            if (group == "CustWise")
            {

                if (date1 == "All" && item == "All")
                {
                    Query = Query;

                }


                if (date1 == "All" && item != "All")
                {
                    Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                }
                if (date1 != "All" && item != "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                }
                if (date1 != "All" && item == "All")
                {
                    Query = Query + " and SAM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                }
            }
            #endregion

            */

            #endregion


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            // Query = Query + " GROUP BY INM_LR_DATE,I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,SAM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE";
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockAdjustmentDatewiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockAdjustmentDatewiseReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockAdjustmentItemwiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockAdjustmentItemwiseReg.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

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
            CommonClasses.SendError("Stock Adjustment Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockAdjustmentRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


}
