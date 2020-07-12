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
/// Summary description for MaterialInspection_BL
/// </summary>
public class MaterialInspection_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables

    private int _INSM_CODE;
    private int _INSM_CM_CODE;
    private DateTime _INSM_DATE;
    private int _INSM_NO;
    private int _INSM_IWM_CODE;
    private int _INSM_SPOM_CODE;
    private int _INSM_I_CODE;
    private int _INSM_UOM_CODE;
    private double _INSM_RECEIVED_QTY;
    private double _INSM_OK_QTY;
    private double _INSM_REJ_QTY;
    private double _INSM_SCRAP_QTY;
    private string _INSM_REMARK;
    private Boolean _INSM_PDR_CHECK;
    private string _INSM_PDR_NO;
    private string _INSM_TYPE;
    private double _INSM_RATE;

    private int _INSD_CODE;
    private int _INSD_INSM_CODE;
    private int _INSD_I_CODE;
    private string _INSD_RM_CODE;
    private double _INSD_REJ_QTY;

    private Boolean _INSM_TC_CHECK;
    private string _INSM_FILE;

    public Boolean INSM_TC_CHECK
    {
        get { return _INSM_TC_CHECK; }
        set { _INSM_TC_CHECK = value; }
    }
    private string _INSM_TC_NO;

    public string INSM_TC_NO
    {
        get { return _INSM_TC_NO; }
        set { _INSM_TC_NO = value; }
    }

    private int _INSPDI_INSM_CODE;

    public int INSPDI_INSM_CODE
    {
        get { return _INSPDI_INSM_CODE; }
        set { _INSPDI_INSM_CODE = value; }
    }
    private string _INSPDI_PARAMETERS;

    public string INSPDI_PARAMETERS
    {
        get { return _INSPDI_PARAMETERS; }
        set { _INSPDI_PARAMETERS = value; }
    }
    private string _INSPDI_SPECIFTION;

    public string INSPDI_SPECIFTION
    {
        get { return _INSPDI_SPECIFTION; }
        set { _INSPDI_SPECIFTION = value; }
    }
    private string _INSPDI_INSPECTION;

    public string INSPDI_INSPECTION
    {
        get { return _INSPDI_INSPECTION; }
        set { _INSPDI_INSPECTION = value; }
    }
    private string _INSPDI_OBSR1;

    public string INSPDI_OBSR1
    {
        get { return _INSPDI_OBSR1; }
        set { _INSPDI_OBSR1 = value; }
    }
    private string _INSPDI_OBSR2;

    public string INSPDI_OBSR2
    {
        get { return _INSPDI_OBSR2; }
        set { _INSPDI_OBSR2 = value; }
    }
    private string _INSPDI_OBSR3;

    public string INSPDI_OBSR3
    {
        get { return _INSPDI_OBSR3; }
        set { _INSPDI_OBSR3 = value; }
    }
    private string _INSPDI_OBSR4;

    public string INSPDI_OBSR4
    {
        get { return _INSPDI_OBSR4; }
        set { _INSPDI_OBSR4 = value; }
    }
    private string _INSPDI_OBSR5;

    public string INSPDI_OBSR5
    {
        get { return _INSPDI_OBSR5; }
        set { _INSPDI_OBSR5 = value; }
    }
    private string _INSPDI_DSPOSITION;

    public string INSPDI_DSPOSITION
    {
        get { return _INSPDI_DSPOSITION; }
        set { _INSPDI_DSPOSITION = value; }
    }
    private string _INSPDI_REMARK;

    public string INSPDI_REMARK
    {
        get { return _INSPDI_REMARK; }
        set { _INSPDI_REMARK = value; }
    }
    private string _INSPDI_I_CODE;

    public string INSPDI_I_CODE
    {
        get { return _INSPDI_I_CODE; }
        set { _INSPDI_I_CODE = value; }
    }

    public string INSM_FILE
    {
        get { return _INSM_FILE; }
        set { _INSM_FILE = value; }
    }

    string message;
    int PK_CODE;
    #endregion

    #region "Constructor"
    public MaterialInspection_BL()
    {

    }
    #endregion

    #region Parameterise Constructor
    public MaterialInspection_BL(int Id)
    {
        _INSM_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int INSM_CODE
    {
        get { return _INSM_CODE; }
        set { _INSM_CODE = value; }
    }
    public int INSM_CM_CODE
    {
        get { return _INSM_CM_CODE; }
        set { _INSM_CM_CODE = value; }
    }
    public DateTime INSM_DATE
    {
        get { return _INSM_DATE; }
        set { _INSM_DATE = value; }
    }

    public int INSM_NO
    {
        get { return _INSM_NO; }
        set { _INSM_NO = value; }
    }

    public int INSM_IWM_CODE
    {
        get { return _INSM_IWM_CODE; }
        set { _INSM_IWM_CODE = value; }
    }
    public int INSM_SPOM_CODE
    {
        get { return _INSM_SPOM_CODE; }
        set { _INSM_SPOM_CODE = value; }
    }
    public int INSM_I_CODE
    {
        get { return _INSM_I_CODE; }
        set { _INSM_I_CODE = value; }
    }
    public int INSM_UOM_CODE
    {
        get { return _INSM_UOM_CODE; }
        set { _INSM_UOM_CODE = value; }
    }
    public double INSM_RECEIVED_QTY
    {
        get { return _INSM_RECEIVED_QTY; }
        set { _INSM_RECEIVED_QTY = value; }
    }
    public double INSM_OK_QTY
    {
        get { return _INSM_OK_QTY; }
        set { _INSM_OK_QTY = value; }
    }
    public double INSM_REJ_QTY
    {
        get { return _INSM_REJ_QTY; }
        set { _INSM_REJ_QTY = value; }
    }
    public double INSM_SCRAP_QTY
    {
        get { return _INSM_SCRAP_QTY; }
        set { _INSM_SCRAP_QTY = value; }
    }
    public string INSM_REMARK
    {
        get { return _INSM_REMARK; }
        set { _INSM_REMARK = value; }
    }
    public Boolean INSM_PDR_CHECK
    {
        get { return _INSM_PDR_CHECK; }
        set { _INSM_PDR_CHECK = value; }
    }

    public string INSM_PDR_NO
    {
        get { return _INSM_PDR_NO; }
        set { _INSM_PDR_NO = value; }
    }
    public string INSM_TYPE
    {
        get { return _INSM_TYPE; }
        set { _INSM_TYPE = value; }
    }
    public double INSM_RATE
    {
        get { return _INSM_RATE; }
        set { _INSM_RATE = value; }
    }


    public int INSD_CODE
    {
        get { return _INSD_CODE; }
        set { _INSD_CODE = value; }
    }

    public int INSD_INSM_CODE
    {
        get { return _INSD_INSM_CODE; }
        set { _INSD_INSM_CODE = value; }
    }
    public int INSD_I_CODE
    {
        get { return _INSD_I_CODE; }
        set { _INSD_I_CODE = value; }
    }

    public string INSD_RM_CODE
    {
        get { return _INSD_RM_CODE; }
        set { _INSD_RM_CODE = value; }
    }
    public double INSD_REJ_QTY
    {
        get { return _INSD_REJ_QTY; }
        set { _INSD_REJ_QTY = value; }
    }
    #endregion

    #region Public Methods

    #region Save
    public bool Save(out int MAX_CODE, GridView XGrid)
    {
        bool result = false;
        MAX_CODE = 0;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[17];

            par[0] = new SqlParameter("@INSM_CM_CODE", INSM_CM_CODE);
            par[1] = new SqlParameter("@INSM_DATE", INSM_DATE);
            par[2] = new SqlParameter("@INSM_NO", INSM_NO);
            par[3] = new SqlParameter("@INSM_IWM_CODE", INSM_IWM_CODE);
            par[4] = new SqlParameter("@INSM_SPOM_CODE", INSM_SPOM_CODE);
            par[5] = new SqlParameter("@INSM_I_CODE", INSM_I_CODE);
            par[6] = new SqlParameter("@INSM_UOM_CODE", INSM_UOM_CODE);
            par[7] = new SqlParameter("@INSM_RECEIVED_QTY", INSM_RECEIVED_QTY);
            par[8] = new SqlParameter("@INSM_OK_QTY", INSM_OK_QTY);
            par[9] = new SqlParameter("@INSM_REJ_QTY", INSM_REJ_QTY);
            par[10] = new SqlParameter("@INSM_SCRAP_QTY", INSM_SCRAP_QTY);
            par[11] = new SqlParameter("@INSM_REMARK", INSM_REMARK);
            par[12] = new SqlParameter("@INSM_PDR_CHECK", INSM_PDR_CHECK);
            par[13] = new SqlParameter("@INSM_PDR_NO", INSM_PDR_NO);
            par[14] = new SqlParameter("@INSM_TC_CHECK", INSM_TC_CHECK);
            par[15] = new SqlParameter("@INSM_TC_NO", INSM_TC_NO);
            par[16] = new SqlParameter("@INSM_FILE", INSM_FILE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_MaterialInspection", par);

            string Party = CommonClasses.GetColumn("SELECT ISNULL(P_INHOUSE_IND,0) AS P_INHOUSE_IND  FROM PARTY_MASTER where P_CODE IN ( SELECT DISTINCT IWM_P_COE FROM  INWARD_MASTER where  IWM_CODE ='" + INSM_IWM_CODE + "')");

            INSD_INSM_CODE = Convert.ToInt32(CommonClasses.GetMaxId("Select MAX(INSM_CODE) from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + INSM_CM_CODE + "'"));
            MAX_CODE = INSD_INSM_CODE;
            if (INSM_REJ_QTY > 0)
            {
                if (result == true)
                {
                    INSD_I_CODE = INSM_I_CODE;
                    INSD_REJ_QTY = INSM_REJ_QTY;
                    SqlParameter[] par1 = new SqlParameter[4];
                    par1[0] = new SqlParameter("@INSD_INSM_CODE", INSD_INSM_CODE);
                    par1[1] = new SqlParameter("@INSD_I_CODE", INSD_I_CODE);
                    par1[2] = new SqlParameter("@INSD_RM_CODE", INSD_RM_CODE);
                    par1[3] = new SqlParameter("@INSD_REJ_QTY", INSD_REJ_QTY);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionDetail", par1);
                }
            }

            INSPDI_INSM_CODE = Convert.ToInt32(CommonClasses.GetMaxId("Select MAX(INSM_CODE) from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + INSM_CM_CODE + "'"));


            for (int i = 0; i < XGrid.Rows.Count; i++)
            {
                int INSM_CODE = INSPDI_INSM_CODE;
                string INSPDI_PARAMETERS = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_PARAMETERS")).Text;
                string INSPDI_SPECIFTION = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_SPECIFTION")).Text;
                string INSPDI_INSPECTION = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_INSPECTION")).Text;
                string INSPDI_OBSR1 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR1")).Text;
                string INSPDI_OBSR2 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR2")).Text;
                string INSPDI_OBSR3 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR3")).Text;
                string INSPDI_OBSR4 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR4")).Text;
                string INSPDI_OBSR5 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR5")).Text;
                string INSPDI_DSPOSITION = ((Label)XGrid.Rows[i].FindControl("lblIINSPDI_DSPOSITION")).Text;
                string INSPDI_REMARK = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_REMARK")).Text;

                SqlParameter[] par2 = new SqlParameter[11];
                par2[0] = new SqlParameter("@INSPDI_INSM_CODE", INSM_CODE);
                par2[1] = new SqlParameter("@INSPDI_PARAMETERS", INSPDI_PARAMETERS);
                par2[2] = new SqlParameter("@INSPDI_SPECIFTION", INSPDI_SPECIFTION);
                par2[3] = new SqlParameter("@INSPDI_INSPECTION", INSPDI_INSPECTION);
                par2[4] = new SqlParameter("@INSPDI_OBSR1", INSPDI_OBSR1);
                par2[5] = new SqlParameter("@INSPDI_OBSR2", INSPDI_OBSR2);
                par2[6] = new SqlParameter("@INSPDI_OBSR3", INSPDI_OBSR3);
                par2[7] = new SqlParameter("@INSPDI_OBSR4", INSPDI_OBSR4);
                par2[8] = new SqlParameter("@INSPDI_OBSR5", INSPDI_OBSR5);
                par2[9] = new SqlParameter("@INSPDI_DSPOSITION", INSPDI_DSPOSITION);
                par2[10] = new SqlParameter("@INSPDI_REMARK", INSPDI_REMARK);

                result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionPDIDetail", par2);
            }
            if (result)
            {
                DataTable dtInward = new DataTable();
                dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");

                CommonClasses.Execute("update INWARD_MASTER set IWM_INSP_NO=ISNULL(IWM_INSP_NO,'')+'," + INSM_NO + "' where IWM_CODE='" + INSM_IWM_CODE + "' ");
                CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY='" + INSM_OK_QTY + "',IWD_CON_REJ_QTY='" + INSM_REJ_QTY + "',IWD_CON_SCRAP_QTY='" + INSM_SCRAP_QTY + "',IWD_INSP_NO='" + INSM_NO + "',IWD_INSP_FLG=1 where IWD_IWM_CODE='" + INSM_IWM_CODE + "' and IWD_I_CODE='" + INSM_I_CODE + "'");


                //for Stock add
                if (INSM_TYPE == "IWIAP")
                {
                    if (Party.ToUpper() == "FALSE" || Party == "0")
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY + INSM_REJ_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'   where I_CODE='" + INSM_I_CODE + "'");
                        //for StockLedger Effiect
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "')");
                        if (INSM_REJ_QTY > 0)
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'SubContractorRejection' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_REJ_QTY) + "')");
                        }
                    }
                }
                else
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "',I_INV_RATE='" + INSM_RATE + "'   where I_CODE='" + INSM_I_CODE + "'");
                    //for StockLedger Effiect
                    CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "')");
                }

            }

        }
        catch (Exception Ex)
        {

        }
        return result;

    }

    #endregion

    #region Update
    public bool Update(out int MAX_CODE, GridView XGrid)
    {
        bool result = false;
        MAX_CODE = 0;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            //DataTable dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY FROM INSPECTION_S_MASTER where  INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            DataTable dtinsecption = new DataTable();
            if (INSM_TYPE == "IWIAP")
            {
                dtinsecption = CommonClasses.Execute("SELECT (INSM_OK_QTY+INSM_REJ_QTY) AS INSM_OK_QTY FROM INSPECTION_S_MASTER where  INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            }
            else
            {
                dtinsecption = CommonClasses.Execute("SELECT (INSM_OK_QTY) AS INSM_OK_QTY FROM INSPECTION_S_MASTER where  INSPECTION_S_MASTER.ES_DELETE='0' AND INSM_CODE='" + INSM_CODE + "' ");
            }
            string Party = CommonClasses.GetColumn("SELECT ISNULL(P_INHOUSE_IND,0) AS P_INHOUSE_IND  FROM PARTY_MASTER where P_CODE IN ( SELECT DISTINCT IWM_P_COE FROM  INWARD_MASTER where  IWM_CODE ='" + INSM_IWM_CODE + "')");

            if (Party.ToUpper() == "FALSE" || Party == "0")
            {
                for (int k = 0; k < dtinsecption.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtinsecption.Rows[k]["INSM_OK_QTY"] + " where  I_CODE='" + INSM_I_CODE + "'");
                }
            }
            SqlParameter[] par = new SqlParameter[18];

            par[0] = new SqlParameter("@INSM_CODE", INSM_CODE);
            par[1] = new SqlParameter("@INSM_CM_CODE", INSM_CM_CODE);
            par[2] = new SqlParameter("@INSM_DATE", INSM_DATE);
            par[3] = new SqlParameter("@INSM_NO", INSM_NO);
            par[4] = new SqlParameter("@INSM_IWM_CODE", INSM_IWM_CODE);
            par[5] = new SqlParameter("@INSM_SPOM_CODE", INSM_SPOM_CODE);
            par[6] = new SqlParameter("@INSM_I_CODE", INSM_I_CODE);
            par[7] = new SqlParameter("@INSM_UOM_CODE", INSM_UOM_CODE);
            par[8] = new SqlParameter("@INSM_RECEIVED_QTY", INSM_RECEIVED_QTY);
            par[9] = new SqlParameter("@INSM_OK_QTY", INSM_OK_QTY);
            par[10] = new SqlParameter("@INSM_REJ_QTY", INSM_REJ_QTY);
            par[11] = new SqlParameter("@INSM_SCRAP_QTY", INSM_SCRAP_QTY);
            par[12] = new SqlParameter("@INSM_REMARK", INSM_REMARK);
            par[13] = new SqlParameter("@INSM_PDR_CHECK", INSM_PDR_CHECK);
            par[14] = new SqlParameter("@INSM_PDR_NO", INSM_PDR_NO);
            par[15] = new SqlParameter("@INSM_TC_CHECK", INSM_TC_CHECK);
            par[16] = new SqlParameter("@INSM_TC_NO", INSM_TC_NO);
            par[17] = new SqlParameter("@INSM_FILE", INSM_FILE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_Update_MaterialInspection", par);
            MAX_CODE = INSM_CODE;
            //for Stock less

            DataTable dtInward = new DataTable();
            dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");

            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM INSPECTION_S_DETAIL WHERE INSD_INSM_CODE='" + INSM_CODE + "'");
                CommonClasses.Execute("DELETE FROM INSPECTION_PDI_DETAIL WHERE INSPDI_INSM_CODE='" + INSM_CODE + "'");
                if (Party.ToUpper() == "FALSE" || Party == "0")
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' AND STL_I_CODE='" + INSM_I_CODE + "'  AND STL_DOC_TYPE='" + INSM_TYPE + "'");
                    if (INSM_TYPE == "IWIAP")
                    {
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + dtInward.Rows[0]["IWM_CODE"].ToString() + "'  AND STL_I_CODE='" + INSM_I_CODE + "'  AND STL_DOC_TYPE='SubContractorRejection'");
                    }
                }
            }

            if (INSM_REJ_QTY > 0)
            {
                if (result == true)
                {
                    INSD_I_CODE = INSM_I_CODE;
                    INSD_REJ_QTY = INSM_REJ_QTY;
                    INSD_INSM_CODE = INSM_CODE;
                    SqlParameter[] par1 = new SqlParameter[4];
                    par1[0] = new SqlParameter("@INSD_INSM_CODE", INSD_INSM_CODE);
                    par1[1] = new SqlParameter("@INSD_I_CODE", INSD_I_CODE);
                    par1[2] = new SqlParameter("@INSD_RM_CODE", INSD_RM_CODE);
                    par1[3] = new SqlParameter("@INSD_REJ_QTY", INSD_REJ_QTY);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionDetail", par1);
                }
            }
            for (int i = 0; i < XGrid.Rows.Count; i++)
            {
                int INSPD_INSM_CODE = INSM_CODE;
                string INSPDI_PARAMETERS = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_PARAMETERS")).Text;
                string INSPDI_SPECIFTION = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_SPECIFTION")).Text;
                string INSPDI_INSPECTION = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_INSPECTION")).Text;
                string INSPDI_OBSR1 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR1")).Text;
                string INSPDI_OBSR2 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR2")).Text;
                string INSPDI_OBSR3 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR3")).Text;
                string INSPDI_OBSR4 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR4")).Text;
                string INSPDI_OBSR5 = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_OBSR5")).Text;
                string INSPDI_DSPOSITION = ((Label)XGrid.Rows[i].FindControl("lblIINSPDI_DSPOSITION")).Text;
                string INSPDI_REMARK = ((Label)XGrid.Rows[i].FindControl("lblINSPDI_REMARK")).Text;
                int INSPDI_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblINSPDI_I_CODE")).Text);

                SqlParameter[] par2 = new SqlParameter[12];
                par2[0] = new SqlParameter("@INSPDI_INSM_CODE", INSPD_INSM_CODE);
                par2[1] = new SqlParameter("@INSPDI_PARAMETERS", INSPDI_PARAMETERS);
                par2[2] = new SqlParameter("@INSPDI_SPECIFTION", INSPDI_SPECIFTION);
                par2[3] = new SqlParameter("@INSPDI_INSPECTION", INSPDI_INSPECTION);
                par2[4] = new SqlParameter("@INSPDI_OBSR1", INSPDI_OBSR1);
                par2[5] = new SqlParameter("@INSPDI_OBSR2", INSPDI_OBSR2);
                par2[6] = new SqlParameter("@INSPDI_OBSR3", INSPDI_OBSR3);
                par2[7] = new SqlParameter("@INSPDI_OBSR4", INSPDI_OBSR4);
                par2[8] = new SqlParameter("@INSPDI_OBSR5", INSPDI_OBSR5);
                par2[9] = new SqlParameter("@INSPDI_DSPOSITION", INSPDI_DSPOSITION);
                par2[10] = new SqlParameter("@INSPDI_REMARK", INSPDI_REMARK);
                par2[11] = new SqlParameter("@INSPDI_I_CODE", INSPDI_I_CODE);

                result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_InspectionPDIDetail", par2);
            }
            if (result)
            {
                CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY='" + INSM_OK_QTY + "',IWD_CON_REJ_QTY='" + INSM_REJ_QTY + "',IWD_CON_SCRAP_QTY='" + INSM_SCRAP_QTY + "',IWD_INSP_NO='" + INSM_NO + "',IWD_INSP_FLG=1 where IWD_IWM_CODE='" + INSM_IWM_CODE + "' and IWD_I_CODE='" + INSM_I_CODE + "'");
                //for Stock add
                if (Party.ToUpper() == "FALSE" || Party == "0")
                {
                    if (INSM_TYPE == "IWIAP")
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY + INSM_REJ_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "'     where I_CODE='" + INSM_I_CODE + "'");
                        //for StockLedger Effiect

                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "')");

                        if (INSM_REJ_QTY > 0)
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'SubContractorRejection' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_REJ_QTY) + "')");
                        }
                    }
                    else
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + (INSM_OK_QTY) + ",I_RECEIPT_DATE='" + INSM_DATE.ToString("dd/MMM/yyyy") + "',I_INV_RATE='" + INSM_RATE + "'     where I_CODE='" + INSM_I_CODE + "'");

                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + INSM_I_CODE + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + INSM_TYPE + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + (INSM_OK_QTY) + "')");

                    }
                }
            }
        }
        catch (Exception Ex)
        {
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
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", INSM_CODE);
            par[1] = new SqlParameter("@PK_Field", "INSM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "INSPECTION_S_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result)
            {

                DataTable dtInward = new DataTable();
                dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + INSM_IWM_CODE + "' ");
                string Party = CommonClasses.GetColumn("SELECT ISNULL(P_INHOUSE_IND,0) AS P_INHOUSE_IND  FROM PARTY_MASTER where P_CODE IN ( SELECT DISTINCT IWM_P_COE FROM  INWARD_MASTER where  IWM_CODE ='" + INSM_IWM_CODE + "')");

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("select IWD_Code,IWD_CON_OK_QTY+IWD_CON_REJ_QTY AS QTY from INWARD_DETAIL,INSPECTION_S_MASTER where INSM_I_CODE=IWD_I_CODE and INSM_NO=IWD_INSP_NO and INWARD_DETAIL.ES_DELETE=0 and INSM_IWM_CODE='" + INSM_CODE + "' ");
                if (dt.Rows.Count > 0)
                {
                    CommonClasses.Execute("update INWARD_DETAIL set IWD_CON_OK_QTY=0,IWD_CON_REJ_QTY='0',IWD_CON_SCRAP_QTY='0',IWD_INSP_NO='',IWD_INSP_FLG=0 where IWD_Code='" + Convert.ToInt32(dt.Rows[0]["IWD_Code"].ToString()) + "' ");
                    result = true;
                }

                if (Party.ToUpper() == "FALSE" || Party == "0")
                {
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + Convert.ToDouble(dt.Rows[0]["QTY"].ToString()) + "  where I_CODE='" + INSM_I_CODE + "'");

                }
                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM INSPECTION_S_DETAIL WHERE INSD_INSM_CODE='" + INSM_CODE + "'");
                    if (Party.ToUpper() == "FALSE" || Party == "0")
                    {
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + INSM_CODE + "'   AND STL_I_CODE='" + INSM_I_CODE + "' AND STL_DOC_TYPE='" + INSM_TYPE + "'");
                    }
                }

            }
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        { }
    }
    #endregion

    #endregion
}
