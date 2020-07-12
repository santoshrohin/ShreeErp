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
/// Summary description for Inward_BL
/// </summary>
public class ServiceInward_BL
{
    #region Constructor
    public ServiceInward_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public ServiceInward_BL(int Id)
    {
        SIM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Variables

    private int _SIM_CODE;
    private int _SIM_CM_CODE;
    private int _SIM_INWARD_TYPE;
    private int _SIM_NO;
    private string _SIM_TYPE;
    private DateTime _SIM_DATE;
    private int _SIM_P_CODE;
    private string _SIM_CHALLAN_NO;
    private DateTime _SIM_CHAL_DATE;
    private string _SIM_EGP_NO;
    private string _SIM_LR_NO;
    private string _SIM_OCT_NO;
    private string _SIM_VEH_NO;
    private int _SIM_SITE_CODE;
    private int _SIM_UOM_CODE;

    private string _SIM_INV_NO;
    private DateTime _SIM_INV_DATE;
    private string _SIM_TRANSPORATOR_NAME;
    private string _SID_BATCH_NO;



    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int SIM_CODE
    {
        get { return _SIM_CODE; }
        set { _SIM_CODE = value; }
    }
    public int SIM_CM_CODE
    {
        get { return _SIM_CM_CODE; }
        set { _SIM_CM_CODE = value; }
    }
    public int SIM_INWARD_TYPE
    {
        get { return _SIM_INWARD_TYPE; }
        set { _SIM_INWARD_TYPE = value; }
    }

    public int SIM_NO
    {
        get { return _SIM_NO; }
        set { _SIM_NO = value; }
    }

    public string SIM_TYPE
    {
        get { return _SIM_TYPE; }
        set { _SIM_TYPE = value; }
    }
    public DateTime SIM_DATE
    {
        get { return _SIM_DATE; }
        set { _SIM_DATE = value; }
    }
    public int SIM_P_CODE
    {
        get { return _SIM_P_CODE; }
        set { _SIM_P_CODE = value; }
    }
    public string SIM_CHALLAN_NO
    {
        get { return _SIM_CHALLAN_NO; }
        set { _SIM_CHALLAN_NO = value; }
    }
    public DateTime SIM_CHAL_DATE
    {
        get { return _SIM_CHAL_DATE; }
        set { _SIM_CHAL_DATE = value; }
    }
    public string SIM_EGP_NO
    {
        get { return _SIM_EGP_NO; }
        set { _SIM_EGP_NO = value; }
    }
    public string SIM_LR_NO
    {
        get { return _SIM_LR_NO; }
        set { _SIM_LR_NO = value; }
    }
    public string SIM_OCT_NO
    {
        get { return _SIM_OCT_NO; }
        set { _SIM_OCT_NO = value; }
    }
    public string SIM_VEH_NO
    {
        get { return _SIM_VEH_NO; }
        set { _SIM_VEH_NO = value; }
    }
    public int SIM_SITE_CODE
    {
        get { return _SIM_SITE_CODE; }
        set { _SIM_SITE_CODE = value; }
    }

    public int SIM_UOM_CODE
    {
        get { return _SIM_UOM_CODE; }
        set { _SIM_UOM_CODE = value; }
    }

    public string SIM_INV_NO
    {
        get { return _SIM_INV_NO; }
        set { _SIM_INV_NO = value; }
    }

    public DateTime SIM_INV_DATE
    {
        get { return _SIM_INV_DATE; }
        set { _SIM_INV_DATE = value; }
    }
    public string SIM_TRANSPORATOR_NAME
    {
        get { return _SIM_TRANSPORATOR_NAME; }
        set { _SIM_TRANSPORATOR_NAME = value; }
    }
    public string SID_BATCH_NO
    {
        get { return _SID_BATCH_NO; }
        set { _SID_BATCH_NO = value; }
    }




    int PK_CODE;

    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {

            dt = CommonClasses.Execute("select SIM_CODE,SIM_NO,SIM_DATE,SIM_CHALLAN_NO,convert(varchar,SIM_CHAL_DATE,106) as SIM_CHAL_DATE ,P_NAME from SERVICE_INWARD_MASTER,PARTY_MASTER where SERVICE_INWARD_MASTER.SIM_P_CODE=PARTY_MASTER.P_CODE AND SERVICE_INWARD_MASTER.ES_DELETE=0 and SIM_CM_CODE=" + SIM_CM_CODE + "");
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Save
    public bool Save(GridView XGrid)
    {

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
            SqlParameter[] par = new SqlParameter[17];
            par[0] = new SqlParameter("@PROCESS", "Insert");
            par[1] = new SqlParameter("@SIM_CODE", DBNull.Value);
            par[2] = new SqlParameter("@SIM_CM_CODE", SIM_CM_CODE);
            par[3] = new SqlParameter("@SIM_INWARD_TYPE", SIM_INWARD_TYPE);
            par[4] = new SqlParameter("@SIM_NO", SIM_NO);
            par[5] = new SqlParameter("@SIM_TYPE", SIM_TYPE);
            par[6] = new SqlParameter("@SIM_DATE", SIM_DATE);
            par[7] = new SqlParameter("@SIM_P_CODE", SIM_P_CODE);
            par[8] = new SqlParameter("@SIM_CHALLAN_NO", SIM_CHALLAN_NO);
            par[9] = new SqlParameter("@SIM_CHAL_DATE", SIM_CHAL_DATE);
            par[10] = new SqlParameter("@SIM_EGP_NO", SIM_EGP_NO);
            par[11] = new SqlParameter("@SIM_LR_NO", SIM_LR_NO);
            par[12] = new SqlParameter("@SIM_OCT_NO", SIM_OCT_NO);
            par[13] = new SqlParameter("@SIM_VEH_NO", SIM_VEH_NO);
            //par[14] = new SqlParameter("@SIM_SITE_CODE", SIM_SITE_CODE);
            par[14] = new SqlParameter("@SIM_INV_NO", SIM_INV_NO);
            par[15] = new SqlParameter("@SIM_INV_DATE", SIM_INV_DATE);
            par[16] = new SqlParameter("@SIM_TRANSPORATOR_NAME", SIM_TRANSPORATOR_NAME);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_SERVICE_INWARD_MASTER", par, out message, out PK_CODE);
            if (result == true)
            {
                bool shortclose = false;
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {
                   
                    int SID_SIM_CODE = PK_CODE;
                    int SID_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblSID_I_CODE")).Text);
                    float SID_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_CH_QTY")).Text);
                    double SID_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_REV_QTY")).Text);
                    float SID_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_REV_QTY")).Text);
                    int SID_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblSID_CPOM_CODE")).Text);
                    float SID_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_RATE")).Text);
                    string SID_REMARK = ((Label)XGrid.Rows[i].FindControl("lblSID_REMARK")).Text;
                    int SID_UOM_CODE = 0;// Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);
                    string SID_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblSID_BATCH_NO")).Text;
                    string SID_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblSID_PROCESS_CODE")).Text;

                    int ASS_I_CODE = SID_I_CODE;
                    string SID_TUR_QTY = "0";
                    string SID_TUR_WEIGHT = "0";

                    if (SIM_TYPE == "OUTCUSTINV")
                    {
                        SID_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblSID_TUR_QTY")).Text;
                        SID_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblSID_TUR_WEIGHT")).Text;
                    }

                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[15];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@SID_SIM_CODE", SID_SIM_CODE);
                    par1[2] = new SqlParameter("@SID_I_CODE", SID_I_CODE);
                    par1[3] = new SqlParameter("@SID_CH_QTY", SID_CH_QTY);
                    par1[4] = new SqlParameter("@SID_REV_QTY", SID_REV_QTY);
                    par1[5] = new SqlParameter("@SID_SQTY", SID_SQTY);
                    par1[6] = new SqlParameter("@SID_CPOM_CODE", SID_CPOM_CODE);
                    par1[7] = new SqlParameter("@SID_RATE", SID_RATE);
                    par1[8] = new SqlParameter("@SID_REMARK", SID_REMARK);
                    par1[9] = new SqlParameter("@SID_UOM_CODE", SID_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@SID_BATCH_NO", SID_BATCH_NO);
                    par1[12] = new SqlParameter("@SID_PROCESS_CODE", SID_PROCESS_CODE);

                    par1[13] = new SqlParameter("@SID_TUR_QTY", SID_TUR_QTY);
                    par1[14] = new SqlParameter("@SID_TUR_WEIGHT", SID_TUR_WEIGHT);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_SERVICE_INWARD_DETAIL", par1, out message);

                    double SID_REV_QTYy = SID_REV_QTY;
                    if (SIM_P_CODE.ToString() == "-2147483271")
                    {
                        CommonClasses.Execute("Update SERVICE_INWARD_DETAIL set SID_INSP_FLG=1 where SID_SIM_CODE='" + SID_SIM_CODE + "'");
                    }
                    //if (SIM_TYPE != "IWIM" && SIM_TYPE != "IWCP")
                    //{
                    //    DataTable rowmateril = new DataTable();
                    //    DataTable dtchallan = new DataTable();
                    //    if (SIM_INWARD_TYPE == 0)
                    //    {
                    //        dtchallan = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where SPOD_SPOM_CODE='" + SID_CPOM_CODE + "' AND BM_I_CODE='" + SID_I_CODE + "' AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + SIM_P_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0  ORDER BY  Convert (int,CL_CH_NO)");
                    //        rowmateril = CommonClasses.Execute(" SELECT (ISNULL(BD_SCRAPQTY,0)+ISNULL(BD_VQTY,0)) AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + SID_I_CODE + "' AND BOM_MASTER.ES_DELETE=0");


                    //        //SID_REV_QTY = Math.Round(SID_REV_QTY * Convert.ToDouble(rowmateril.Rows[0]["BD_QTY"].ToString()));
                    //        //SID_I_CODE = Convert.ToInt32(rowmateril.Rows[0]["CL_I_CODE"].ToString());
                    //    }
                    //    else
                    //    {
                    //        dtchallan = CommonClasses.Execute("SELECT  SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ORDER BY  Convert (int,CL_CH_NO)");

                    //    }
                    //    if (dtchallan.Rows.Count > 0)
                    //    {
                    //        if (SIM_INWARD_TYPE == 0)
                    //        {
                    //            double qty = SID_REV_QTY;
                    //            for (int s = 0; s < rowmateril.Rows.Count; s++)
                    //            {
                    //                SID_REV_QTY = Math.Round(SID_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                    //                SID_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                    //                double Bal_Qty = SID_REV_QTY;
                    //                for (int z = 0; z < dtchallan.Rows.Count; z++)
                    //                {
                    //                    if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == SID_I_CODE.ToString())
                    //                    {
                    //                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                    //                        {
                    //                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                            // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                            Bal_Qty = 0;
                    //                            break;
                    //                        }
                    //                        else
                    //                        {
                    //                            Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                            SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                        }
                    //                    }
                    //                }
                    //                SID_REV_QTY = qty;
                    //            }
                    //        }
                    //        else
                    //        {

                    //            double Bal_Qty = SID_REV_QTY;
                    //            for (int z = 0; z < dtchallan.Rows.Count; z++)
                    //            {
                    //                if (Bal_Qty > 0)
                    //                {
                    //                    if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                    //                    {
                    //                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                        // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                        Bal_Qty = 0;
                    //                        break;
                    //                    }
                    //                    else
                    //                    {
                    //                        Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                        //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                        SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                    }
                    //                }
                    //            }

                    //        }
                    //    }
                    //}
                    //Insert Into Stock Ledger Table 
                    //if (result == true)
                    //{
                    //    SqlParameter[] par2 = new SqlParameter[6];
                    //    par2[0] = new SqlParameter("@STL_I_CODE", SID_I_CODE);
                    //    par2[1] = new SqlParameter("@STL_DOC_NO", SID_SIM_CODE);
                    //    par2[2] = new SqlParameter("@STL_DOC_NUMBER", SIM_NO);
                    //    par2[3] = new SqlParameter("@STL_DOC_TYPE", SIM_TYPE);
                    //    par2[4] = new SqlParameter("@STL_DOC_DATE", SIM_DATE);
                    //    par2[5] = new SqlParameter("@STL_DOC_QTY", SID_REV_QTY);
                    //    //par2[6] = new SqlParameter("@STL_SIT_CODE", SIM_SITE_CODE);
                    //    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    //}

                    //if (result == true)
                    //{
                    //    SqlParameter[] par3 = new SqlParameter[11];
                    //    par3[0] = new SqlParameter("@GL_CH_NO", SIM_CHALLAN_NO);
                    //    par3[1] = new SqlParameter("@GL_DATE", SIM_DATE);
                    //    par3[2] = new SqlParameter("@GL_GIN_TYPE", SIM_TYPE);
                    //    par3[3] = new SqlParameter("@GL_P_CODE", SIM_P_CODE);
                    //    par3[4] = new SqlParameter("@GL_I_CODE", SID_I_CODE);
                    //    par3[5] = new SqlParameter("@GL_CQTY", SID_REV_QTY);
                    //    par3[6] = new SqlParameter("@GL_CON_QTY", SID_REV_QTY);
                    //    par3[7] = new SqlParameter("@GL_DOC_ID", SID_SIM_CODE);
                    //    par3[8] = new SqlParameter("@GL_DOC_NO", SIM_NO);
                    //    par3[9] = new SqlParameter("@GL_DOC_DATE", SIM_DATE);
                    //    par3[10] = new SqlParameter("@GL_DOC_TYPE", SIM_TYPE);
                    //    //par3[11] = new SqlParameter("@GL_SIT_CODE", SIM_SITE_CODE);
                    //    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_GIN_STOCKLEDGER", par3);
                    //}
                    //DataTable dtShortclose = new DataTable();

                    if (result == true)
                    {
                        //Update Supplier PO Qty
                        CommonClasses.Execute("Update SERVICE_PO_DETAILS set SRPOD_INW_QTY=SRPOD_INW_QTY+" + SID_REV_QTYy + " where SRPOD_I_CODE='" + SID_I_CODE + "' and SRPOD_SPOM_CODE='" + SID_CPOM_CODE + "'");
                        //CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + SID_REV_QTY + ",I_RECEIPT_DATE='" + SIM_DATE + "' where I_CODE='" + SID_I_CODE + "'");
                        // bool Res = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_IS_SHORT_CLOSE=1 WHERE SPOM_CODE='" + cpom_code + "'");
                        //dtShortclose = CommonClasses.Execute("SELECT * FROM ITEM_MASTER,ITEM_CATEGORY_MASTER WHERE ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE and ITEM_CATEGORY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND ISNULL(ITEM_CATEGORY_MASTER.I_CAT_SHORTCLOSE,0)=1 AND S_CODE='" + SID_I_CODE + "' ");

                    }
                    //if (dtShortclose.Rows.Count > 0)
                    //{
                    //    bool Res = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_IS_SHORT_CLOSE=1 WHERE SPOM_CODE='" + SID_CPOM_CODE + "'");
                    //}
                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Save", Ex.Message);

        }
        return result;
    }
    #endregion
    
    #region Update
    public bool Update(GridView XGrid)
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            DataTable dtInwardDetail = CommonClasses.Execute("select SID_SIM_CODE,SID_I_CODE,SID_RATE,SID_REV_QTY,SID_CPOM_CODE from SERVICE_INWARD_DETAIL,SERVICE_INWARD_MASTER where SID_SIM_CODE=SIM_CODE and SID_SIM_CODE='" + SIM_CODE + "' and SERVICE_INWARD_MASTER.ES_DELETE='0'");
            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
            {
                CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtInwardDetail.Rows[k]["SID_REV_QTY"] + " where  SPOD_I_CODE='" + dtInwardDetail.Rows[k]["SID_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtInwardDetail.Rows[k]["SID_CPOM_CODE"] + "'");
                // CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[k]["SID_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["SID_I_CODE"] + "'");

                PK_CODE = SIM_CODE;
                if (SIM_TYPE != "IWIM" && SIM_TYPE != "IWCP")
                {
                    DataTable stockLedger = new DataTable();
                    if (SIM_INWARD_TYPE == 0)
                    {
                        stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE IN (SELECT BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BOM_MASTER.ES_DELETE=0 AND BM_CODE=BD_BM_CODE AND BM_I_CODE='" + dtInwardDetail.Rows[k]["SID_I_CODE"] + "') ");
                        for (int z = 0; z < stockLedger.Rows.Count; z++)
                        {
                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                            CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "'  ");
                        }
                    }
                    else
                    {
                        stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND  CL_I_CODE='" + dtInwardDetail.Rows[k]["SID_I_CODE"] + "' ");
                        for (int z = 0; z < stockLedger.Rows.Count; z++)
                        {
                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='OutSUBINM'");
                        }
                        CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + PK_CODE + "' and CL_DOC_TYPE='IWIAP' AND CL_I_CODE='" + dtInwardDetail.Rows[k]["SID_I_CODE"] + "'  ");
                    }
                }
            }

            //updating Master Record
            SqlParameter[] par = new SqlParameter[17];
            par[0] = new SqlParameter("@PROCESS", "Update");
            par[1] = new SqlParameter("@SIM_CODE", SIM_CODE);
            par[2] = new SqlParameter("@SIM_CM_CODE", SIM_CM_CODE);
            par[3] = new SqlParameter("@SIM_INWARD_TYPE", SIM_INWARD_TYPE);
            par[4] = new SqlParameter("@SIM_NO", SIM_NO);
            par[5] = new SqlParameter("@SIM_TYPE", SIM_TYPE);
            par[6] = new SqlParameter("@SIM_DATE", SIM_DATE);
            par[7] = new SqlParameter("@SIM_P_CODE", SIM_P_CODE);
            par[8] = new SqlParameter("@SIM_CHALLAN_NO", SIM_CHALLAN_NO);
            par[9] = new SqlParameter("@SIM_CHAL_DATE", SIM_CHAL_DATE);
            par[10] = new SqlParameter("@SIM_EGP_NO", SIM_EGP_NO);
            par[11] = new SqlParameter("@SIM_LR_NO", SIM_LR_NO);
            par[12] = new SqlParameter("@SIM_OCT_NO", SIM_OCT_NO);
            par[13] = new SqlParameter("@SIM_VEH_NO", SIM_VEH_NO);
            //par[14] = new SqlParameter("@SIM_SITE_CODE", SIM_SITE_CODE);
            par[14] = new SqlParameter("@SIM_INV_NO", SIM_INV_NO);
            par[15] = new SqlParameter("@SIM_INV_DATE", SIM_INV_DATE);
            par[16] = new SqlParameter("@SIM_TRANSPORATOR_NAME", SIM_TRANSPORATOR_NAME);


            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_SERVICE_INWARD_MASTER", par, out message, out PK_CODE);
            //result = DL_DBAccess.Insertion_Updation_Delete_Modify("SP_UPDATE_SERVICE_INWARD_MASTER", par, out PK_CODE);


            //Deleteing Inward Detail part
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM SERVICE_INWARD_DETAIL WHERE SID_SIM_CODE='" + PK_CODE + "'");
                //CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='" + SIM_TYPE + "'");
                //CommonClasses.Execute("DELETE FROM GIN_STOCK_LEDGER where GL_DOC_ID='" + PK_CODE + "' and GL_DOC_TYPE='" + SIM_TYPE + "'");

            }
           
            if (result == true)
            {

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int SID_SIM_CODE = PK_CODE;
                    int SID_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblSID_I_CODE")).Text);
                    float SID_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_CH_QTY")).Text);
                    double SID_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_REV_QTY")).Text);
                    float SID_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_REV_QTY")).Text);
                    int SID_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblSID_CPOM_CODE")).Text);
                    float SID_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblSID_RATE")).Text);
                    string SID_REMARK = ((Label)XGrid.Rows[i].FindControl("lblSID_REMARK")).Text;
                    int SID_UOM_CODE = 0;// Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);

                    string SID_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblSID_BATCH_NO")).Text;

                    string SID_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblSID_PROCESS_CODE")).Text;

                    string SID_TUR_QTY = "0";
                    string SID_TUR_WEIGHT = "0";
                    int ASS_I_CODE = SID_I_CODE;

                    if (SIM_TYPE == "OUTCUSTINV")
                    {
                        SID_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblSID_TUR_QTY")).Text;
                        SID_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblSID_TUR_WEIGHT")).Text;
                    }
                    //DataTable dtitemMaster = CommonClasses.Execute("select I_CURRENT_BAL,I_CODE from ITEM_MASTER where I_CODE='" + SID_I_CODE + "' and ITEM_MASTER.ES_DELETE='0'");
                    //for (int p = 0; p < dtInwardDetail.Rows.Count; p++)
                    //{
                    //    if (dtInwardDetail.Rows[i]["SID_I_CODE"].ToString() == SID_I_CODE.ToString())
                    //    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[p]["SID_REV_QTY"] + " where  I_CODE='" + SID_I_CODE + "'");
                    //}


                    //Inserting new Inward Detail Part
              
                    SqlParameter[] par1 = new SqlParameter[15];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@SID_SIM_CODE", SID_SIM_CODE);
                    par1[2] = new SqlParameter("@SID_I_CODE", SID_I_CODE);
                    par1[3] = new SqlParameter("@SID_CH_QTY", SID_CH_QTY);
                    par1[4] = new SqlParameter("@SID_REV_QTY", SID_REV_QTY);
                    par1[5] = new SqlParameter("@SID_SQTY", SID_SQTY);
                    par1[6] = new SqlParameter("@SID_CPOM_CODE", SID_CPOM_CODE);
                    par1[7] = new SqlParameter("@SID_RATE", SID_RATE);
                    par1[8] = new SqlParameter("@SID_REMARK", SID_REMARK);
                    par1[9] = new SqlParameter("@SID_UOM_CODE", SID_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@SID_BATCH_NO", SID_BATCH_NO);
                    par1[12] = new SqlParameter("@SID_PROCESS_CODE", SID_PROCESS_CODE);

                    par1[13] = new SqlParameter("@SID_TUR_QTY", SID_TUR_QTY);
                    par1[14] = new SqlParameter("@SID_TUR_WEIGHT", SID_TUR_WEIGHT);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_SERVICE_INWARD_DETAIL", par1, out message);

                    double SID_REV_QTYy = SID_REV_QTY;
                    if (SIM_TYPE != "IWIM" && SIM_TYPE != "IWCP")
                    {

                        DataTable rowmateril = new DataTable();
                        DataTable dtchallan = new DataTable();
                        if (SIM_INWARD_TYPE == 0)
                        {
                            dtchallan = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM SUPP_PO_DETAILS,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where SPOD_SPOM_CODE='" + SID_CPOM_CODE + "' AND BM_I_CODE='" + SID_I_CODE + "' AND SPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + SIM_P_CODE + "'  AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ORDER BY  Convert (int,CL_CH_NO)");
                            rowmateril = CommonClasses.Execute(" SELECT (ISNULL(BD_SCRAPQTY,0)+ISNULL(BD_VQTY,0)) AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + SID_I_CODE + "' AND BOM_MASTER.ES_DELETE=0");


                            //SID_REV_QTY = Math.Round(SID_REV_QTY * Convert.ToDouble(rowmateril.Rows[0]["BD_QTY"].ToString()));
                            //SID_I_CODE = Convert.ToInt32(rowmateril.Rows[0]["CL_I_CODE"].ToString());
                        }
                        else
                        {
                            dtchallan = CommonClasses.Execute("SELECT  SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "'   AND CL_DOC_TYPE='OutSUBINM'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0  ORDER BY  Convert (int,CL_CH_NO)");

                        }



                        #region MyRegion
                        /*
                        if (dtchallan.Rows.Count > 0)                       
                        {
                            double qty = SID_REV_QTY;
                            if (SIM_INWARD_TYPE == 0)
                            {

                                for (int s = 0; s < rowmateril.Rows.Count; s++)
                                {
                                    SID_REV_QTY = Math.Round(SID_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                    SID_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                    double Bal_Qty = SID_REV_QTY;
                                    for (int z = 0; z < dtchallan.Rows.Count; z++)
                                    {
                                        if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == SID_I_CODE.ToString())
                                        {
                                            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                            {
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + SID_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                Bal_Qty = 0;
                                                break;
                                            }
                                            else
                                            {
                                                Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + SID_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            }
                                        }
                                    }
                                    SID_REV_QTY = qty;
                                }
                            }
                            else
                            {
                                double Bal_Qty = SID_REV_QTY;
                                for (int z = 0; z < dtchallan.Rows.Count; z++)
                                {
                                    if (Bal_Qty > 0)
                                    {
                                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                        {
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + SID_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            Bal_Qty = 0;
                                            break;
                                        }
                                        else
                                        {
                                            Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + SID_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                            SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                        }
                                    }
                                    SID_REV_QTY = qty;
                                }
                            }
                        } 
                       
                        */
                        #endregion
                    //    if (dtchallan.Rows.Count > 0)
                    //    {
                    //        if (SIM_INWARD_TYPE == 0)
                    //        {
                    //            double qty = SID_REV_QTY;
                    //            for (int s = 0; s < rowmateril.Rows.Count; s++)
                    //            {
                    //                SID_REV_QTY = Math.Round(SID_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                    //                SID_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                    //                double Bal_Qty = SID_REV_QTY;
                    //                for (int z = 0; z < dtchallan.Rows.Count; z++)
                    //                {
                    //                    if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == SID_I_CODE.ToString())
                    //                    {
                    //                        if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                    //                        {
                    //                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                            // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                            Bal_Qty = 0;
                    //                            break;
                    //                        }
                    //                        else
                    //                        {
                    //                            Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                            CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                            //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                            CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                            SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                        }
                    //                    }
                    //                }
                    //                SID_REV_QTY = qty;
                    //            }
                    //        }
                    //        else
                    //        {

                                
                    //            for (int z = 0; z < dtchallan.Rows.Count; z++)
                    //            {
                    //                double Bal_Qty = SID_REV_QTY;
                    //                if (Bal_Qty > 0)
                    //                {
                    //                    if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                    //                    {
                    //                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                        // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                        Bal_Qty = 0;
                    //                        break;
                    //                    }
                    //                    else
                    //                    {
                    //                        Bal_Qty = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + SIM_P_CODE + "','" + ASS_I_CODE + "','" + SID_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + SID_SIM_CODE + "','" + SIM_NO + "','" + Convert.ToDateTime(SIM_DATE).ToString("dd/MMM/yyyy") + "','IWIAP',0)");
                    //                        //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + SIM_P_CODE + "' AND CL_I_CODE='" + SID_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                    //                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                    //                        SID_REV_QTY = SID_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                    //                    }
                    //                }
                    //            }

                    //        }
                    //    }
                    }

                    //inserting new stock entry
                    //if (result == true)
                    //{
                    //    SqlParameter[] par2 = new SqlParameter[6];
                    //    par2[0] = new SqlParameter("@STL_I_CODE", SID_I_CODE);
                    //    par2[1] = new SqlParameter("@STL_DOC_NO", SID_SIM_CODE);
                    //    par2[2] = new SqlParameter("@STL_DOC_NUMBER", SIM_NO);
                    //    par2[3] = new SqlParameter("@STL_DOC_TYPE", SIM_TYPE);
                    //    par2[4] = new SqlParameter("@STL_DOC_DATE", SIM_DATE);
                    //    par2[5] = new SqlParameter("@STL_DOC_QTY", SID_REV_QTY);
                    //    //par2[6] = new SqlParameter("@STL_SIT_CODE", SIM_SITE_CODE);
                    //    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    //}
                    //if (result == true)
                    //{
                    //    SqlParameter[] par3 = new SqlParameter[11];
                    //    par3[0] = new SqlParameter("@GL_CH_NO", SIM_CHALLAN_NO);
                    //    par3[1] = new SqlParameter("@GL_DATE", SIM_DATE);
                    //    par3[2] = new SqlParameter("@GL_GIN_TYPE", SIM_TYPE);
                    //    par3[3] = new SqlParameter("@GL_P_CODE", SIM_P_CODE);
                    //    par3[4] = new SqlParameter("@GL_I_CODE", SID_I_CODE);
                    //    par3[5] = new SqlParameter("@GL_CQTY", SID_REV_QTY);
                    //    par3[6] = new SqlParameter("@GL_CON_QTY", SID_REV_QTY);
                    //    par3[7] = new SqlParameter("@GL_DOC_ID", SID_SIM_CODE);
                    //    par3[8] = new SqlParameter("@GL_DOC_NO", SIM_NO);
                    //    par3[9] = new SqlParameter("@GL_DOC_DATE", SIM_DATE);
                    //    par3[10] = new SqlParameter("@GL_DOC_TYPE", SIM_TYPE);
                    //    // par3[11] = new SqlParameter("@GL_SIT_CODE", SIM_SITE_CODE);
                    //    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_GIN_STOCKLEDGER", par3);
                    //}
                    //Updating new Inward Qty In Supplier PO Detail
                    if (result == true)
                    {
                        DataTable dtSpodUpdate = CommonClasses.Execute("Update SERVICE_PO_DETAILS set SRPOD_INW_QTY=SRPOD_INW_QTY+" + SID_REV_QTYy + " where SRPOD_I_CODE='" + SID_I_CODE + "' and SRPOD_SPOM_CODE='" + SID_CPOM_CODE + "'");
                        //CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + SID_REV_QTY + ",I_RECEIPT_DATE='" + SIM_DATE + "' where I_CODE='" + SID_I_CODE + "'");
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Update", Ex.Message);

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
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@PK_CODE", SIM_CODE);
            par[1] = new SqlParameter("@PK_Field", "SIM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "SERVICE_INWARD_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //Delete from stock table
                DataTable stockqty = CommonClasses.Execute("select SID_SIM_CODE,SID_I_CODE,SID_REV_QTY,SID_CPOM_CODE from SERVICE_INWARD_DETAIL,SERVICE_INWARD_MASTER where SID_SIM_CODE=SIM_CODE AND SID_SIM_CODE='" + SIM_CODE + "'");
                DataTable dtitemMaster1 = new DataTable();
                for (int s = 0; s < stockqty.Rows.Count; s++)
                {
                    dtitemMaster1 = CommonClasses.Execute("select I_CURRENT_BAL,I_CODE from ITEM_MASTER where I_CODE='" + stockqty.Rows[s]["SID_I_CODE"] + "' and ITEM_MASTER.ES_DELETE='0'");
                    for (int j = 0; j < dtitemMaster1.Rows.Count; j++)
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtitemMaster1.Rows[j]["I_CURRENT_BAL"] + ",I_RECEIPT_DATE='" + SIM_DATE + "' where  I_CODE='" + dtitemMaster1.Rows[j]["I_CODE"] + "'");
                    }
                }
                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + SIM_CODE + "' and  STL_DOC_TYPE='SIWM' ");
                }
                for (int k = 0; k < stockqty.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + stockqty.Rows[k]["SID_REV_QTY"] + " where  SPOD_I_CODE='" + stockqty.Rows[k]["SID_I_CODE"] + "' and SPOD_SPOM_CODE='" + stockqty.Rows[k]["SID_CPOM_CODE"] + "'");
                }

            }

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion
    
    #region CheckExistSaveNo
    public bool CheckExistSaveNo(string genpono, string IN_MSIM_CM_CODE)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("Select SIM_NO from SERVICE_INWARD_MASTER where ES_DELETE<> '1' and SIM_NO='" + genpono + "' and SIM_CM_CODE=" + IN_MSIM_CM_CODE + "");
            if (dt.Rows.Count > 0)
            {
                res = true;
                //MessageBox.Show("GIN  Number Already Exists");
                // throw new Exception("Inward Number Already Exists");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "CheckExistSaveNo", Ex.Message);

        }
        return res;
    }
    #endregion

}
