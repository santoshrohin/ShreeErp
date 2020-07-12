using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
/// <summary>
/// Summary description for ProductionToStore_BL
/// </summary>
public class ProductionToStore_BL
{
    #region Counstructor
    public ProductionToStore_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public ProductionToStore_BL(int Id)
    {
        PS_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    private int _PS_CODE;
    private int _PS_MR_CODE;
    private int _PS_CM_COMP_CODE;
    private int _PS_GIN_NO;
    private DateTime _PS_GIN_DATE;
    private string _PS_PERSON_NAME;
    private int _PS_TYPE;
    private int _PS_BATCH_NO;
    private int _PS_P_CODE;


    public string message = "";
    public int PK_CODE;
    public string Msg = "";
    #endregion

    #region Public Properties
    public int PS_CODE
    {
        get { return _PS_CODE; }
        set { _PS_CODE = value; }
    }
    public int PS_CM_COMP_CODE
    {
        get { return _PS_CM_COMP_CODE; }
        set { _PS_CM_COMP_CODE = value; }
    }
    public int PS_GIN_NO
    {
        get { return _PS_GIN_NO; }
        set { _PS_GIN_NO = value; }
    }
    public DateTime PS_GIN_DATE
    {
        get { return _PS_GIN_DATE; }
        set { _PS_GIN_DATE = value; }
    }
    public string PS_PERSON_NAME
    {
        get { return _PS_PERSON_NAME; }
        set { _PS_PERSON_NAME = value; }
    }
    public int PS_TYPE
    {
        get { return _PS_TYPE; }
        set { _PS_TYPE = value; }
    }
    public int PS_MR_CODE
    {
        get { return _PS_MR_CODE; }
        set { _PS_MR_CODE = value; }
    }
    public int PS_BATCH_NO
    {
        get { return _PS_BATCH_NO; }
        set { _PS_BATCH_NO = value; }
    }
    public int PS_P_CODE
    {
        get { return _PS_P_CODE; }
        set { _PS_P_CODE = value; }
    }
    #endregion

    #region Save
    public bool Save(GridView XGrid, string Process)
    {
        string Proc;
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
        }
        catch (Exception ex)
        { }
        try
        {
            //Inserting Inward Master Part
            if (Process == "INSERT")
            {
                Proc = "Insert";
            }
            else
            {
                Proc = "Update";
            }

            SqlParameter[] Params = 
			{ 
                
                new SqlParameter("@PROCESS", Proc),
				new SqlParameter("@PS_CODE",PS_CODE),
                new SqlParameter("@PS_MR_CODE",PS_MR_CODE),
				new SqlParameter("@PS_CM_COMP_CODE",PS_CM_COMP_CODE),
				new SqlParameter("@PS_GIN_NO",PS_GIN_NO),
				new SqlParameter("@PS_GIN_DATE",PS_GIN_DATE),
				new SqlParameter("@PS_PERSON_NAME",PS_PERSON_NAME),
				new SqlParameter("@PS_TYPE",PS_TYPE),
			    new SqlParameter("@PS_BATCH_NO",PS_BATCH_NO),
				new SqlParameter("@PS_P_CODE",PS_P_CODE)
				
			};

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PRODUCTION_TO_STORE_MASTER", Params, out message, out PK_CODE);
            int PSD_PS_CODE = PK_CODE;
            if (Process == "INSERT")
            {

            }
            else
            {
                DataTable dtProductionDetail = CommonClasses.Execute("select PSD_PS_CODE,PSD_I_CODE,PSD_QTY from PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER where PSD_PS_CODE=PS_CODE and PSD_PS_CODE='" + PK_CODE + "'");
                float Quantity = 0;
                float QuantityTot = 0;
                for (int k = 0; k < dtProductionDetail.Rows.Count; k++)
                {
                    //CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtProductionDetail.Rows[k]["IWD_REV_QTY"] + " where  SPOD_I_CODE='" + dtProductionDetail.Rows[k]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtProductionDetail.Rows[k]["IWD_CPOM_CODE"] + "'");
                    Quantity = float.Parse(dtProductionDetail.Rows[k]["PSD_QTY"].ToString());
                    //  QuantityTot = QuantityTot + Quantity;
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + Quantity + " where  I_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");

                    DataTable dtTemp = CommonClasses.Execute("SELECT *  FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' AND STL_DOC_NUMBER='" + PS_GIN_NO + "'  AND STL_I_CODE=-2147483533 and  STL_DOC_TYPE LIKE 'Scrap Against%' AND STL_SIT_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");
                    if (dtTemp.Rows.Count > 0)
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtTemp.Rows[0]["STL_DOC_QTY"].ToString() + " where  I_CODE=-2147483533");
                    }

                    CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' AND STL_DOC_NUMBER='" + PS_GIN_NO + "'  AND STL_I_CODE=-2147483533 and  STL_DOC_TYPE LIKE 'Scrap Against%' AND STL_SIT_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");



                    if (PS_TYPE == 2)
                    {
                        DataTable dt = new DataTable();
                        dt = CommonClasses.Execute(" SELECT BD_I_CODE,(BD_VQTY +BD_SCRAPQTY)AS PROD, '" + Quantity + "' AS STOCK  INTO #TEMP FROM BOM_MASTER,BOM_DETAIL where BM_CODE=BD_BM_CODE  AND BM_I_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "' SELECT BD_I_CODE,PROD	*STOCK AS MAX_PROD  FROM #TEMP ORDER BY (PROD	*STOCK)     DROP TABLE #TEMP");
                        if (dt.Rows.Count > 0)
                        {
                            for (int p = 0; p < dt.Rows.Count; p++)
                            {
                                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='PRODTOSTORECONSUME' AND STL_I_CODE='" + dt.Rows[p]["BD_I_CODE"].ToString() + "'");

                                result = CommonClasses.Execute1("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + dt.Rows[p]["MAX_PROD"].ToString() + "  where I_CODE='" + dt.Rows[p]["BD_I_CODE"].ToString() + "'");
                            }
                        }
                    }
                }


                result = CommonClasses.Execute1("delete from PRODUCTION_TO_STORE_DETAIL where PSD_PS_CODE='" + PK_CODE + "'");
                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='PRODTOSTORE'");

            }
            if (result == true)
            {
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {
                    string PSD_I_CODE = "";
                    string PSD_UOM_CODE = "";
                    string PSD_REMARK = "";
                    string BT_CODE = "";
                    float PSD_QTY = 0;
                    float PSD_QTY_TOTAL = 0;

                    PSD_I_CODE = ((Label)XGrid.Rows[i].FindControl("lblPSD_I_CODE")).Text;
                    PSD_UOM_CODE = ((Label)XGrid.Rows[i].FindControl("lblUOMCODE")).Text;
                    PSD_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPSD_QTY")).Text);
                    PSD_QTY_TOTAL = PSD_QTY_TOTAL + PSD_QTY;
                    PSD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblPSD_REMARK")).Text;
                    BT_CODE = ((Label)XGrid.Rows[i].FindControl("lblBT_CODE")).Text;
                    //Inserting Production Detail Part
                    SqlParameter[] Params1 =               
                    	  { 
                               
				                new SqlParameter("@PSD_PS_CODE",PSD_PS_CODE),
				                new SqlParameter("@PSD_I_CODE",PSD_I_CODE), 
				                new SqlParameter("@PSD_QTY",PSD_QTY),
				                new SqlParameter("@PSD_REMARK",PSD_REMARK),
                                new SqlParameter("@PK_CODE", DBNull.Value)
			                };

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PRODUCTION_TO_STORE_DETAIL", Params1, out message);
                    result = CommonClasses.Execute1("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + PSD_QTY_TOTAL + ",I_RECEIPT_DATE='" + Convert.ToDateTime(PS_GIN_DATE).ToString("dd/MMM/yyyy") + "' where I_CODE='" + PSD_I_CODE + "'");

                    if (PS_TYPE == 2)
                    {
                        DataTable dt = new DataTable();
                        dt = CommonClasses.Execute(" SELECT BD_I_CODE,(BD_VQTY +BD_SCRAPQTY)AS PROD, '" + PSD_QTY + "' AS STOCK  INTO #TEMP FROM BOM_MASTER,BOM_DETAIL where BM_CODE=BD_BM_CODE  AND BM_I_CODE='" + PSD_I_CODE + "' SELECT BD_I_CODE,PROD	*STOCK AS MAX_PROD  FROM #TEMP ORDER BY (PROD	*STOCK)     DROP TABLE #TEMP");
                        if (dt.Rows.Count > 0)
                        {
                            for (int p = 0; p < dt.Rows.Count; p++)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + dt.Rows[p]["BD_I_CODE"].ToString() + "','" + PK_CODE + "','" + PS_GIN_NO + "','PRODTOSTORECONSUME','" + Convert.ToDateTime(PS_GIN_DATE).ToString("dd MMM yyyy") + "','-" + dt.Rows[p]["MAX_PROD"].ToString() + "')");
                                result = CommonClasses.Execute1("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dt.Rows[p]["MAX_PROD"].ToString() + ",I_RECEIPT_DATE='" + Convert.ToDateTime(PS_GIN_DATE).ToString("dd/MMM/yyyy") + "' where I_CODE='" + dt.Rows[p]["BD_I_CODE"].ToString() + "'");
                            }
                        }
                    }

                    #region Insert Into Stock Ledger
                    if (result == true)
                    {
                        SqlParameter[] par2 = new SqlParameter[6];
                        par2[0] = new SqlParameter("@STL_I_CODE", PSD_I_CODE);
                        par2[1] = new SqlParameter("@STL_DOC_NO", PSD_PS_CODE);
                        par2[2] = new SqlParameter("@STL_DOC_NUMBER", PS_GIN_NO);
                        par2[3] = new SqlParameter("@STL_DOC_TYPE", "PRODTOSTORE");
                        par2[4] = new SqlParameter("@STL_DOC_DATE", PS_GIN_DATE);
                        par2[5] = new SqlParameter("@STL_DOC_QTY", PSD_QTY_TOTAL);
                        //par2[6] = new SqlParameter("@STL_SIT_CODE", IWM_SITE_CODE);
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    }
                    #endregion

                    DataTable dtScrap = new DataTable();
                    //dtScrap = CommonClasses.Execute("insert into STOCK_LEDGER(STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_DOC_QTY)  SELECT I_CODE,'Scrap Against ' +I_CODENO AS I_CODENO ,'" + PSD_QTY + "' AS PROD, CASE WHEN I_DENSITY=0 then ISNULL((SELECT round(SUM(BD_SCRAPQTY),4)  FROM BOM_MASTER ,BOM_DETAIL where BM_CODE=BD_BM_CODE AND BOM_MASTER.ES_DELETE=0 AND BM_I_CODE=I_CODE),0) ELSE I_DENSITY  END AS SCRAP  into #temp    FROM ITEM_MASTER  where  I_CODE='" + PSD_I_CODE + "'  SELECT  I_CODE,'" + PSD_PS_CODE + "' AS PSD_PS_CODE,'" + PS_GIN_NO + "' AS PS_GIN_NO,I_CODENO,'" + PS_GIN_DATE + "' AS PS_GIN_DATE,round((PROD*SCRAP),4),I_CODE FROM #temp drop table #temp");
                    dtScrap = CommonClasses.Execute(" SELECT I_INV_RATE,I_CODE,'Scrap Against ' +I_CODENO AS I_CODENO ,'" + PSD_QTY + "' AS PROD, CASE WHEN I_DENSITY=0 then ISNULL((SELECT round(SUM(BD_SCRAPQTY),4)  FROM BOM_MASTER ,BOM_DETAIL where BM_CODE=BD_BM_CODE AND BOM_MASTER.ES_DELETE=0 AND BM_I_CODE=I_CODE),0) ELSE I_DENSITY  END AS SCRAP  into #temp    FROM ITEM_MASTER  where  I_CODE='" + PSD_I_CODE + "'  SELECT  I_INV_RATE,I_CODE AS STL_I_CODE,'" + PSD_PS_CODE + "' AS STL_DOC_NO,'" + PS_GIN_NO + "' AS STL_DOC_NUMBER,I_CODENO AS STL_DOC_TYPE,'" + PS_GIN_DATE.ToString("dd/MMM/yyyy") + "' AS STL_DOC_DATE,round((PROD*SCRAP),4) AS STL_DOC_QTY,I_CODE AS STL_SIT_CODE FROM #temp drop table #temp");
                    if (dtScrap.Rows.Count > 0)
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + dtScrap.Rows[0]["STL_DOC_QTY"].ToString() + " where  I_CODE=-2147483533");
                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_SIT_CODE) VALUES ('-2147483533','" + PK_CODE + "','" + PS_GIN_NO + "','" + dtScrap.Rows[0]["STL_DOC_TYPE"].ToString() + "','" + Convert.ToDateTime(PS_GIN_DATE).ToString("dd MMM yyyy") + "','" + dtScrap.Rows[0]["STL_DOC_QTY"].ToString() + "','" + dtScrap.Rows[0]["STL_SIT_CODE"].ToString() + "')");

                        CommonClasses.Execute("Update PRODUCTION_TO_STORE_DETAIL set PSD_SCRAP=" + dtScrap.Rows[0]["STL_DOC_QTY"].ToString() + ",PSD_RATE= (SELECT I_INV_RATE FROM ITEM_MASTER where I_CODE=-2147483533) where  PSD_PS_CODE='" + PK_CODE + "' AND PSD_I_CODE='" + PSD_I_CODE + "'");

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Prod TO Store ", "Save/Update", Ex.Message);

        }
        return result;
    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            //Update Master Table Flag
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", PS_CODE);
            par[1] = new SqlParameter("@PK_Field", "PS_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "PRODUCTION_TO_STORE_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);
            if (result)
            {
                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PS_CODE + "' and  STL_DOC_TYPE='PRODTOSTORE'");
                DataTable dtProductionDetail = CommonClasses.Execute("select PS_TYPE, PSD_PS_CODE,PSD_I_CODE,PSD_QTY from PRODUCTION_TO_STORE_DETAIL,PRODUCTION_TO_STORE_MASTER where PSD_PS_CODE=PS_CODE and PSD_PS_CODE='" + PS_CODE + "'");

                float Quantity = 0;
                float QtyTotal = 0;
                for (int k = 0; k < dtProductionDetail.Rows.Count; k++)
                {
                    Quantity = float.Parse(dtProductionDetail.Rows[k]["PSD_QTY"].ToString());
                    //QtyTotal = QtyTotal + Quantity;
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + Quantity + " where  I_CODE='" + dtProductionDetail.Rows[0]["PSD_I_CODE"] + "'");

                    DataTable dtTemp = CommonClasses.Execute("SELECT *  FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PS_CODE + "'  AND STL_I_CODE=-2147483533 and  STL_DOC_TYPE LIKE 'Scrap Against%' AND STL_SIT_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");
                    if (dtTemp.Rows.Count > 0)
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtTemp.Rows[0]["STL_DOC_QTY"].ToString() + " where  I_CODE=-2147483533");
                    }

                    CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PS_CODE + "'   AND STL_I_CODE=-2147483533 and  STL_DOC_TYPE LIKE 'Scrap Against%' AND STL_SIT_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "'");



                    if (dtProductionDetail.Rows[0]["PS_TYPE"].ToString() == "2")
                    {
                        DataTable dt = new DataTable();
                        dt = CommonClasses.Execute(" SELECT BD_I_CODE,(BD_VQTY +BD_SCRAPQTY)AS PROD, '" + Quantity + "' AS STOCK  INTO #TEMP FROM BOM_MASTER,BOM_DETAIL where BM_CODE=BD_BM_CODE  AND BM_I_CODE='" + dtProductionDetail.Rows[k]["PSD_I_CODE"] + "' SELECT BD_I_CODE,PROD	*STOCK AS MAX_PROD  FROM #TEMP ORDER BY (PROD	*STOCK)     DROP TABLE #TEMP");
                        if (dt.Rows.Count > 0)
                        {
                            for (int p = 0; p < dt.Rows.Count; p++)
                            {
                                result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PS_CODE + "' and  STL_DOC_TYPE='PRODTOSTORECONSUME' AND STL_I_CODE='" + dt.Rows[p]["BD_I_CODE"].ToString() + "'");
                                result = CommonClasses.Execute1("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + dt.Rows[p]["MAX_PROD"].ToString() + "  where I_CODE='" + dt.Rows[p]["BD_I_CODE"].ToString() + "'");

                            }

                        }
                    }

                }
            }
            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion
}
