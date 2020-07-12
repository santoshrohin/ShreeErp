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


public partial class RoportForms_ADD_SupplierPOAmendRegister : System.Web.UI.Page
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
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            // bool ChkAllUser = Convert.ToBoolean(Request.QueryString[4].ToString());

            string From = Request.QueryString[4].ToString();
            string To = Request.QueryString[5].ToString();
            int p_name = Convert.ToInt32(Request.QueryString[6].ToString());
            int i_name = Convert.ToInt32(Request.QueryString[7].ToString());
            //string u_name = Request.QueryString[9].ToString();

            string group = Request.QueryString[8].ToString();
            string way = Request.QueryString[9].ToString();

            //i_name = i_name.Replace("'", "''");


            #region Detail
            if (way == "All")
            {
                if (group == "Datewise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }
                if (group == "ItemWise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }
                if (group == "CustWise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }
            }
            #endregion

            #region Summary
            if (way == "Pending")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }
                if (group == "CustWise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == false && ChkAllitem == false)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                }

            }
            #endregion


        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string cust, string item, string From, string To, int p_name, int i_name, string group, string way)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            // Query = "SELECT CPOM_CODE,CPOM_PONO,SPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,LEDGER_MASTER.LM_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT FROM LEDGER_MASTER INNER JOIN SUPP_PO_MASTER INNER JOIN SUPP_PO_DETAIL ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAIL.SPOD_SPOM_CODE INNER JOIN LOG_MASTER ON CUSTPO_MASTER.CPOM_CODE = LOG_MASTER.LG_DOC_CODE ON LEDGER_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_MASTER ON CUSTPO_DETAIL.CPOD_I_CODE = ITEM_MASTER.I_CODE where CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order Master'";
            //Query = "SELECT I_NAME,LM_NAME,SPOD_ORDER_QTY, SPOD_RATE,SPOD_INW_QTY,SPOM_PO_NO,UOM_NAME,SPOM_DATE FROM ITEM_MASTER,LEDGER_MASTER,SUPP_PO_DETAIL,SUPP_PO_MASTER,UNIT_MASTER WHERE SPOD_I_CODE=I_CODE and SPOD_UOM_CODE=UOM_CODE and SPOM_LM_CODE=LM_CODE and SPOD_SPOM_CODE=SPOM_CODE and SUPP_PO_MASTER.ES_DELETE=0";
            Query = "SELECT I_CODE,I_NAME, I_CODENO,P_NAME,SPOD_AMD_ORDER_QTY, SPOD_AMD_RATE,isnull( SPOD_AMD_INW_QTY,0.00)as SPOM_AM_INW_QTY ,SPOM_AM_PO_NO,I_UOM_NAME as UOM_NAME,SPOM_AM_DATE,SPOM_AM_AM_COUNT,SPOM_AM_AM_DATE,AM_CODE FROM ITEM_MASTER,PARTY_MASTER,SUPP_PO_AMD_DETAILS,SUPP_PO_AM_MASTER,ITEM_UNIT_MASTER WHERE SPOD_AMD_I_CODE=I_CODE and SPOD_AMD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and SPOM_AM_P_CODE=P_CODE and AM_CODE=AMD_AM_CODE and SUPP_PO_AM_MASTER.ES_DELETE=0 ";
            #region All
            if (way == "All")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LEDGER_MASTER.LM_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }
                #endregion

                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LEDGER_MASTER.LM_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }
                #endregion

                #region CustWise
                if (group == "CustWise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_AM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }
                #endregion
            }
            #endregion

            #region Pending
            if (way == "Pending")
            {
                Query = Query + "and (SPOD_ORDER_QTY - isnull( SPOD_INW_QTY,0.000)) > 0";
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LEDGER_MASTER.LM_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";


                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";


                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";


                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }

                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    Query = Query + "and (SPOD_ORDER_QTY - isnull( SPOD_INW_QTY,0.000)) > 0";
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LEDGER_MASTER.LM_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }
                #endregion
                #region CustWise
                if (group == "CustWise")
                {
                    Query = Query + "and (SPOD_ORDER_QTY - isnull( SPOD_INW_QTY,0.000)) > 0";
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LEDGER_MASTER.LM_CODE='" + p_name + "' and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PATY_MASTER.P_CODE='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";

                    }
                    if (date1 == "All" && cust == "ONE" && item == "ONE")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and ITEM_MASTER.I_CODE='" + i_name + "'  ";
                    }
                    if (date1 == "ONE" && cust == "All" && item == "ONE")
                    {
                        Query = Query + "and ITEM_MASTER.I_CODE='" + i_name + "' and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                    if (date1 == "ONE" && cust == "ONE" && item == "All")
                    {
                        Query = Query + "and PARTY_MASTER.P_CODE='" + p_name + "'and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                    }
                }
                #endregion

            }
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPOAmendDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPOAmendDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Pending")
                    {
                        rptname.SetParameterValue("txtType", group + " PO Pending Report");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", group + " PO All Report ");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                //if (group == "ItemWise")
                //{
                //    rptname.Load(Server.MapPath("~/Reports/rptSupplierPOItemWise.rpt"));
                //    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPOItemWise.rpt");


                //    rptname.Refresh();
                //    rptname.SetDataSource(dt);
                    
                //    if (way == "Pending")
                //    {
                //        rptname.SetParameterValue("txtType", group + "  PO Pending Report");


                //    }
                //    else
                //    {
                //        rptname.SetParameterValue("txtType", group + " PO All Report");

                //    }
                //    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                //    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                //    CrystalReportViewer1.ReportSource = rptname;

                //}
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPOAmendSuppWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPOAmendSuppWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", " SupplierWIse PO Pending Report");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "SupplierWIse PO All Report");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoAmendRegister.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Amendment Register", "btnCancel_Click", Ex.Message);
        }
    }
}
