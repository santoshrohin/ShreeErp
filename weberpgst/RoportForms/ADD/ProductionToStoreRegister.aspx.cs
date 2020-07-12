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

public partial class RoportForms_ADD_ProductionToStoreRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string type = "";

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
            //bool chkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool chkAllItem = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool chkAllProd = Convert.ToBoolean(Request.QueryString[3].ToString());

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //string i_code = Request.QueryString[6].ToString();
            //string prod_no = Request.QueryString[7].ToString();
            string group = Request.QueryString[3].ToString();
            type = Request.QueryString[4].ToString();
            string Condition = Request.QueryString[5].ToString();
            string Value = Request.QueryString[6].ToString();

            GenerateReport(Title, From, To, group, Condition, Value);
            #region Datewise

            //if (group == "Datewise")
            //{

            //    if (chkAllDate == true && chkAllItem == true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "All", "All", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem == true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "ONE", "All", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem != true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "All", "ONE", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem == true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "All", "All", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem != true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem != true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem == true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "ONE", "All", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem != true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "All", "ONE", "ONE", From, To, i_code, prod_no, group);
            //    }
            //}

            //if (group == "Itemwise")
            //{

            //    if (chkAllDate == true && chkAllItem == true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "All", "All", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem == true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "ONE", "All", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem != true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "All", "ONE", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem == true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "All", "All", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem != true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem != true && chkAllProd == true)
            //    {
            //        GenerateReport(Title, "ONE", "ONE", "All", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate != true && chkAllItem == true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "ONE", "All", "ONE", From, To, i_code, prod_no, group);
            //    }
            //    if (chkAllDate == true && chkAllItem != true && chkAllProd != true)
            //    {
            //        GenerateReport(Title, "All", "ONE", "ONE", From, To, i_code, prod_no, group);
            //    }
            //}
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "Page_Init", Ex.Message);

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string vcond, string value)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (type == "0")
            {
                Query = "select PS_CODE,PS_GIN_NO,Convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE,ISNULL(PSD_SCRAP,0  ) AS PSD_SCRAP,ISNULL(PSD_RATE,0) AS PSD_RATE from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE " + vcond + "   ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PS_TYPE=2 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }
            else if (type == "1")
            {
                Query = "select PS_CODE,PS_GIN_NO,Convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE,ISNULL(PSD_SCRAP,0  ) AS PSD_SCRAP,ISNULL(PSD_RATE,0) AS PSD_RATE  from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE " + vcond + "    ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PS_TYPE=1 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }
            else
            {
                Query = "select PS_CODE,PS_GIN_NO,Convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE,ISNULL(PSD_SCRAP,0  ) AS PSD_SCRAP,ISNULL(PSD_RATE,0) AS PSD_RATE  from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE  " + vcond + "   ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }

            #region Datewise
            //if (group == "Datewise")
            //{
            //    if (date1 == "All" && item == "All" && prod == "All")
            //    {
            //        Query = Query;
            //    }
            //    if (date1 != "All" && item == "All" && prod == "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
            //    }
            //    if (date1 == "All" && item != "All" && prod == "All")
            //    {
            //        Query = Query + " and I_CODE =" + I_code + "";
            //    }
            //    if (date1 == "All" && item == "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item != "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and I_CODE ='" + I_code + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 == "All" && item != "All" && prod != "All")
            //    {
            //        Query = Query + " and I_CODE ='" + I_code + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item == "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item != "All" && prod == "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and I_CODE ='" + I_code + "'";
            //    }
            //}
            //#endregion

            //#region Itemwise
            //if (group == "Itemwise")
            //{
            //    if (date1 == "All" && item == "All" && prod == "All")
            //    {
            //        Query = Query;
            //    }
            //    if (date1 != "All" && item == "All" && prod == "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
            //    }
            //    if (date1 == "All" && item != "All" && prod == "All")
            //    {
            //        Query = Query + " and I_CODE =" + I_code + "";
            //    }
            //    if (date1 == "All" && item == "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item != "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and I_CODE ='" + I_code + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 == "All" && item != "All" && prod != "All")
            //    {
            //        Query = Query + " and I_CODE ='" + I_code + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item == "All" && prod != "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and PS_GIN_NO='" + pro_no + "'";
            //    }
            //    if (date1 != "All" && item != "All" && prod == "All")
            //    {
            //        Query = Query + " and PS_GIN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and I_CODE ='" + I_code + "'";
            //    }
            //}
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (group == "Datewise")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptProdToStoreDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptProdToStoreDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);

                    rptname.SetParameterValue("txtvalue", value);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "Itemwise")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptProdToStoreItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptProdToStoreItemWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtvalue", value);
                    //if (From != "" && To != "")
                    //{

                    //    rptname.SetParameterValue("txtperiod", " From " + From.ToString() + " To " + To.ToString() + "");

                    //}
                    //else
                    //{
                    //    DataTable dtDate = CommonClasses.Execute("SELECT CM_OPENING_DATE,CM_CLOSING_DATE from COMPANY_MASTER where CM_ID='" + Session["CompanyId"] + "'");
                    //    DateTime opendate = Convert.ToDateTime(dtDate.Rows[0]["CM_OPENING_DATE"]);
                    //    DateTime closedate = Convert.ToDateTime(dtDate.Rows[0]["CM_CLOSING_DATE"]);

                    //    rptname.SetParameterValue("txtperiod", " From " + opendate.ToString("dd/MMM/yyyy") + " To " + closedate.ToString("dd/MMM/yyyy") + "");

                    //}
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
            CommonClasses.SendError("Production To Store Register", "GenerateReport", Ex.Message);

        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewProdcutionToStoreRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Production To Store Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
