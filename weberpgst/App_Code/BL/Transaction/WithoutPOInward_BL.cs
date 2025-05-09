﻿using System;
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
public class WithoutPOInward_BL
{
    #region Constructor
    public WithoutPOInward_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public WithoutPOInward_BL(int Id)
    {
        IWM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Variables

    private int _IWM_CODE;
    private int _IWM_CM_CODE;
    private int _IWM_INWARD_TYPE;
    private int _IWM_NO;
    private string _IWM_TYPE;
    private DateTime _IWM_DATE;
    private int _IWM_P_CODE;
    private string _IWM_CHALLAN_NO;
    private DateTime _IWM_CHAL_DATE;
    private string _IWM_EGP_NO;
    private string _IWM_LR_NO;
    private string _IWM_OCT_NO;
    private string _IWM_VEH_NO;
    private int _IWM_SITE_CODE;
    private int _IWM_UOM_CODE;

    private string _IWM_INV_NO;
    private DateTime _IWM_INV_DATE;
    private string _IWM_TRANSPORATOR_NAME;
    private string _IWD_BATCH_NO;

    private int _IWM_CURR_CODE;
    private double _IWM_CURR_RATE;

    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int IWM_CODE
    {
        get { return _IWM_CODE; }
        set { _IWM_CODE = value; }
    }
    public int IWM_CM_CODE
    {
        get { return _IWM_CM_CODE; }
        set { _IWM_CM_CODE = value; }
    }
    public int IWM_INWARD_TYPE
    {
        get { return _IWM_INWARD_TYPE; }
        set { _IWM_INWARD_TYPE = value; }
    }

    public int IWM_NO
    {
        get { return _IWM_NO; }
        set { _IWM_NO = value; }
    }

    public string IWM_TYPE
    {
        get { return _IWM_TYPE; }
        set { _IWM_TYPE = value; }
    }
    public DateTime IWM_DATE
    {
        get { return _IWM_DATE; }
        set { _IWM_DATE = value; }
    }
    public int IWM_P_CODE
    {
        get { return _IWM_P_CODE; }
        set { _IWM_P_CODE = value; }
    }
    public string IWM_CHALLAN_NO
    {
        get { return _IWM_CHALLAN_NO; }
        set { _IWM_CHALLAN_NO = value; }
    }
    public DateTime IWM_CHAL_DATE
    {
        get { return _IWM_CHAL_DATE; }
        set { _IWM_CHAL_DATE = value; }
    }
    public string IWM_EGP_NO
    {
        get { return _IWM_EGP_NO; }
        set { _IWM_EGP_NO = value; }
    }
    public string IWM_LR_NO
    {
        get { return _IWM_LR_NO; }
        set { _IWM_LR_NO = value; }
    }
    public string IWM_OCT_NO
    {
        get { return _IWM_OCT_NO; }
        set { _IWM_OCT_NO = value; }
    }
    public string IWM_VEH_NO
    {
        get { return _IWM_VEH_NO; }
        set { _IWM_VEH_NO = value; }
    }
    public int IWM_SITE_CODE
    {
        get { return _IWM_SITE_CODE; }
        set { _IWM_SITE_CODE = value; }
    }

    public int IWM_UOM_CODE
    {
        get { return _IWM_UOM_CODE; }
        set { _IWM_UOM_CODE = value; }
    }

    public string IWM_INV_NO
    {
        get { return _IWM_INV_NO; }
        set { _IWM_INV_NO = value; }
    }

    public DateTime IWM_INV_DATE
    {
        get { return _IWM_INV_DATE; }
        set { _IWM_INV_DATE = value; }
    }
    public string IWM_TRANSPORATOR_NAME
    {
        get { return _IWM_TRANSPORATOR_NAME; }
        set { _IWM_TRANSPORATOR_NAME = value; }
    }
    public string IWD_BATCH_NO
    {
        get { return _IWD_BATCH_NO; }
        set { _IWD_BATCH_NO = value; }
    }

    #region IWM_CURR_RATE
    public int IWM_CURR_CODE
    {
        get { return _IWM_CURR_CODE; }
        set { _IWM_CURR_CODE = value; }
    }
    #endregion IWM_CURR_CODE

    #region IWM_CURR_RATE
    public double IWM_CURR_RATE
    {
        get { return _IWM_CURR_RATE; }
        set { _IWM_CURR_RATE = value; }
    }
    #endregion IWM_CURR_RATE

    int PK_CODE;

    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {

            dt = CommonClasses.Execute("select IWM_CODE,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE ,P_NAME from INWARD_MASTER,PARTY_MASTER where INWARD_MASTER.IWM_P_CODE=PARTY_MASTER.P_CODE AND INWARD_MASTER.ES_DELETE=0 and IWM_CM_CODE=" + IWM_CM_CODE + "");
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
            SqlParameter[] par = new SqlParameter[19];
            par[0] = new SqlParameter("@PROCESS", "Insert");
            par[1] = new SqlParameter("@IWM_CODE", DBNull.Value);
            par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
            par[3] = new SqlParameter("@IWM_INWARD_TYPE", IWM_INWARD_TYPE);
            par[4] = new SqlParameter("@IWM_NO", IWM_NO);
            par[5] = new SqlParameter("@IWM_TYPE", IWM_TYPE);
            par[6] = new SqlParameter("@IWM_DATE", IWM_DATE);
            par[7] = new SqlParameter("@IWM_P_CODE", IWM_P_CODE);
            par[8] = new SqlParameter("@IWM_CHALLAN_NO", IWM_CHALLAN_NO);
            par[9] = new SqlParameter("@IWM_CHAL_DATE", IWM_CHAL_DATE);
            par[10] = new SqlParameter("@IWM_EGP_NO", IWM_EGP_NO);
            par[11] = new SqlParameter("@IWM_LR_NO", IWM_LR_NO);
            par[12] = new SqlParameter("@IWM_OCT_NO", IWM_OCT_NO);
            par[13] = new SqlParameter("@IWM_VEH_NO", IWM_VEH_NO);
            //par[14] = new SqlParameter("@IWM_SITE_CODE", IWM_SITE_CODE);
            par[14] = new SqlParameter("@IWM_INV_NO", IWM_INV_NO);
            par[15] = new SqlParameter("@IWM_INV_DATE", IWM_INV_DATE);
            par[16] = new SqlParameter("@IWM_TRANSPORATOR_NAME", IWM_TRANSPORATOR_NAME);
            par[17] = new SqlParameter("@IWM_CURR_CODE", IWM_CURR_CODE);
            par[18] = new SqlParameter("@IWM_CURR_RATE", IWM_CURR_RATE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_MASTER", par, out message, out PK_CODE);
            if (result == true)
            {
                bool shortclose = false;
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int IWD_IWM_CODE = PK_CODE;
                    int IWD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                    float IWD_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CH_QTY")).Text);
                    double IWD_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    float IWD_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    int IWD_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                    double IWD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_RATE")).Text);
                    double IWD_DISCOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_DISCOUNT")).Text);
                    double IWD_GRATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_GRATE")).Text);
                    string IWD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblIWD_REMARK")).Text;
                    int IWD_UOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);
                    string IWD_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblIWD_BATCH_NO")).Text;
                    string IWD_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_PROCESS_CODE")).Text;
                    string Doc_Name = "";
                    int ASS_I_CODE = IWD_I_CODE;
                    string IWD_TUR_QTY = "0";
                    string IWD_TUR_WEIGHT = "0";
                    //if (IWM_TYPE == "IWIM")
                    //{
                        Doc_Name = ((LinkButton)XGrid.Rows[i].FindControl("lnkView")).Text;
                    //}

                    //if (IWM_TYPE == "OUTCUSTINV")
                    //{
                    //    IWD_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_QTY")).Text;
                    //    IWD_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_WEIGHT")).Text;
                    //}

                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[16];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@IWD_IWM_CODE", IWD_IWM_CODE);
                    par1[2] = new SqlParameter("@IWD_I_CODE", IWD_I_CODE);
                    par1[3] = new SqlParameter("@IWD_CH_QTY", IWD_CH_QTY);
                    par1[4] = new SqlParameter("@IWD_REV_QTY", IWD_REV_QTY);
                    par1[5] = new SqlParameter("@IWD_SQTY", IWD_SQTY);
                    par1[6] = new SqlParameter("@IWD_CPOM_CODE", IWD_CPOM_CODE);
                    par1[7] = new SqlParameter("@IWD_RATE", IWD_RATE);
                    par1[8] = new SqlParameter("@IWD_REMARK", IWD_REMARK);
                    par1[9] = new SqlParameter("@IWD_UOM_CODE", IWD_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@IWD_BATCH_NO", IWD_BATCH_NO);
                    par1[12] = new SqlParameter("@IWD_PROCESS_CODE", IWD_PROCESS_CODE);

                    par1[13] = new SqlParameter("@IWD_TUR_QTY", IWD_TUR_QTY);
                    par1[14] = new SqlParameter("@IWD_TUR_WEIGHT", IWD_TUR_WEIGHT);
                    par1[15] = new SqlParameter("@Doc_Name", Doc_Name);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_DETAIL", par1, out message);

                    double IWD_REV_QTYy = IWD_REV_QTY;

                    CommonClasses.Execute("Update INWARD_DETAIL set IWD_INSP_FLG=1,IWD_CON_OK_QTY='" + IWD_REV_QTY + "',IWD_GRATE='" + IWD_GRATE + "',IWD_DISCOUNT='" + IWD_DISCOUNT + "' where IWD_IWM_CODE='" + IWD_IWM_CODE + "' AND IWD_I_CODE='" + IWD_I_CODE + "'");


                    //Insert Into Stock Ledger Table 
                    if (result == true)
                    {
                        SqlParameter[] par2 = new SqlParameter[6];
                        par2[0] = new SqlParameter("@STL_I_CODE", IWD_I_CODE);
                        par2[1] = new SqlParameter("@STL_DOC_NO", IWD_IWM_CODE);
                        par2[2] = new SqlParameter("@STL_DOC_NUMBER", IWM_NO);
                        par2[3] = new SqlParameter("@STL_DOC_TYPE", IWM_TYPE);
                        par2[4] = new SqlParameter("@STL_DOC_DATE", IWM_DATE);
                        par2[5] = new SqlParameter("@STL_DOC_QTY", IWD_REV_QTY);
                        //par2[6] = new SqlParameter("@STL_SIT_CODE", IWM_SITE_CODE);
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    }

                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)+'" + IWD_REV_QTY + "' where I_CODE='" + IWD_I_CODE + "'");

                    
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
            DataTable dtInwardDetail = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_RATE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE and IWD_IWM_CODE='" + IWM_CODE + "' and INWARD_MASTER.ES_DELETE='0'");
            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
            {
                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[k]["IWD_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["IWD_I_CODE"] + "'");

                PK_CODE = IWM_CODE;


            }

            //updating Master Record
            SqlParameter[] par = new SqlParameter[19];
            par[0] = new SqlParameter("@PROCESS", "Update");
            par[1] = new SqlParameter("@IWM_CODE", IWM_CODE);
            par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
            par[3] = new SqlParameter("@IWM_INWARD_TYPE", IWM_INWARD_TYPE);
            par[4] = new SqlParameter("@IWM_NO", IWM_NO);
            par[5] = new SqlParameter("@IWM_TYPE", IWM_TYPE);
            par[6] = new SqlParameter("@IWM_DATE", IWM_DATE);
            par[7] = new SqlParameter("@IWM_P_CODE", IWM_P_CODE);
            par[8] = new SqlParameter("@IWM_CHALLAN_NO", IWM_CHALLAN_NO);
            par[9] = new SqlParameter("@IWM_CHAL_DATE", IWM_CHAL_DATE);
            par[10] = new SqlParameter("@IWM_EGP_NO", IWM_EGP_NO);
            par[11] = new SqlParameter("@IWM_LR_NO", IWM_LR_NO);
            par[12] = new SqlParameter("@IWM_OCT_NO", IWM_OCT_NO);
            par[13] = new SqlParameter("@IWM_VEH_NO", IWM_VEH_NO);
            //par[14] = new SqlParameter("@IWM_SITE_CODE", IWM_SITE_CODE);
            par[14] = new SqlParameter("@IWM_INV_NO", IWM_INV_NO);
            par[15] = new SqlParameter("@IWM_INV_DATE", IWM_INV_DATE);
            par[16] = new SqlParameter("@IWM_TRANSPORATOR_NAME", IWM_TRANSPORATOR_NAME);
            par[17] = new SqlParameter("@IWM_CURR_CODE", IWM_CURR_CODE);
            par[18] = new SqlParameter("@IWM_CURR_RATE", IWM_CURR_RATE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_MASTER", par, out message, out PK_CODE);
            //result = DL_DBAccess.Insertion_Updation_Delete_Modify("SP_UPDATE_INWARD_MASTER", par, out PK_CODE);


            //Deleteing Inward Detail part
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM INWARD_DETAIL WHERE IWD_IWM_CODE='" + PK_CODE + "'");
                CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='" + IWM_TYPE + "'");
                //CommonClasses.Execute("DELETE FROM GIN_STOCK_LEDGER where GL_DOC_ID='" + PK_CODE + "' and GL_DOC_TYPE='" + IWM_TYPE + "'");

            }

            if (result == true)
            {

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int IWD_IWM_CODE = PK_CODE;
                    int IWD_I_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_I_CODE")).Text);
                    float IWD_CH_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_CH_QTY")).Text);
                    double IWD_REV_QTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    float IWD_SQTY = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_REV_QTY")).Text);
                    int IWD_CPOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblIWD_CPOM_CODE")).Text);
                    double IWD_RATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_RATE")).Text);
                    double IWD_DISCOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_DISCOUNT")).Text);
                    double IWD_GRATE = float.Parse(((Label)XGrid.Rows[i].FindControl("lblIWD_GRATE")).Text);
                    string IWD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblIWD_REMARK")).Text;
                    int IWD_UOM_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblUOM_CODE")).Text);
                    string IWD_BATCH_NO = ((Label)XGrid.Rows[i].FindControl("lblIWD_BATCH_NO")).Text;
                    string IWD_PROCESS_CODE = ((Label)XGrid.Rows[i].FindControl("lblIWD_PROCESS_CODE")).Text;
                    string Doc_Name = "";
                    string IWD_TUR_QTY = "0";
                    string IWD_TUR_WEIGHT = "0";
                    int ASS_I_CODE = IWD_I_CODE;
                    //if (IWM_TYPE == "IWIM")
                    //{
                    Doc_Name = ((LinkButton)XGrid.Rows[i].FindControl("lnkView")).Text;
                    //}

                    //if (IWM_TYPE == "OUTCUSTINV")
                    //{
                    //    IWD_TUR_QTY = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_QTY")).Text;
                    //    IWD_TUR_WEIGHT = ((Label)XGrid.Rows[i].FindControl("lblIWD_TUR_WEIGHT")).Text;
                    //}
                    //DataTable dtitemMaster = CommonClasses.Execute("select I_CURRENT_BAL,I_CODE from ITEM_MASTER where I_CODE='" + IWD_I_CODE + "' and ITEM_MASTER.ES_DELETE='0'");
                    //for (int p = 0; p < dtInwardDetail.Rows.Count; p++)
                    //{
                    //    if (dtInwardDetail.Rows[i]["IWD_I_CODE"].ToString() == IWD_I_CODE.ToString())
                    //    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtInwardDetail.Rows[p]["IWD_REV_QTY"] + " where  I_CODE='" + IWD_I_CODE + "'");
                    //}


                    //Inserting new Inward Detail Part

                    SqlParameter[] par1 = new SqlParameter[16];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@IWD_IWM_CODE", IWD_IWM_CODE);
                    par1[2] = new SqlParameter("@IWD_I_CODE", IWD_I_CODE);
                    par1[3] = new SqlParameter("@IWD_CH_QTY", IWD_CH_QTY);
                    par1[4] = new SqlParameter("@IWD_REV_QTY", IWD_REV_QTY);
                    par1[5] = new SqlParameter("@IWD_SQTY", IWD_SQTY);
                    par1[6] = new SqlParameter("@IWD_CPOM_CODE", IWD_CPOM_CODE);
                    par1[7] = new SqlParameter("@IWD_RATE", IWD_RATE);
                    par1[8] = new SqlParameter("@IWD_REMARK", IWD_REMARK);
                    par1[9] = new SqlParameter("@IWD_UOM_CODE", IWD_UOM_CODE);
                    par1[10] = new SqlParameter("@PK_CODE", DBNull.Value);
                    par1[11] = new SqlParameter("@IWD_BATCH_NO", IWD_BATCH_NO);
                    par1[12] = new SqlParameter("@IWD_PROCESS_CODE", IWD_PROCESS_CODE);

                    par1[13] = new SqlParameter("@IWD_TUR_QTY", IWD_TUR_QTY);
                    par1[14] = new SqlParameter("@IWD_TUR_WEIGHT", IWD_TUR_WEIGHT);
                    par1[15] = new SqlParameter("@Doc_Name", Doc_Name);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_INWARD_DETAIL", par1, out message);

                    //CommonClasses.Execute("Update INWARD_DETAIL set IWD_INSP_FLG=1,IWD_CON_OK_QTY='" + IWD_REV_QTY + "' where IWD_IWM_CODE='" + IWD_IWM_CODE + "' AND IWD_I_CODE='" + IWD_I_CODE + "'");
                    CommonClasses.Execute("Update INWARD_DETAIL set IWD_INSP_FLG=1,IWD_CON_OK_QTY='" + IWD_REV_QTY + "',IWD_GRATE='" + IWD_GRATE + "',IWD_DISCOUNT='" + IWD_DISCOUNT + "' where IWD_IWM_CODE='" + IWD_IWM_CODE + "' AND IWD_I_CODE='" + IWD_I_CODE + "'");


                    //inserting new stock entry
                    if (result == true)
                    {
                        SqlParameter[] par2 = new SqlParameter[6];
                        par2[0] = new SqlParameter("@STL_I_CODE", IWD_I_CODE);
                        par2[1] = new SqlParameter("@STL_DOC_NO", IWD_IWM_CODE);
                        par2[2] = new SqlParameter("@STL_DOC_NUMBER", IWM_NO);
                        par2[3] = new SqlParameter("@STL_DOC_TYPE", IWM_TYPE);
                        par2[4] = new SqlParameter("@STL_DOC_DATE", IWM_DATE);
                        par2[5] = new SqlParameter("@STL_DOC_QTY", IWD_REV_QTY);
                        //par2[6] = new SqlParameter("@STL_SIT_CODE", IWM_SITE_CODE);
                        result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_STOCKLEDGER", par2);
                    }
                    CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)+'" + IWD_REV_QTY + "' where I_CODE='" + IWD_I_CODE + "'");

                    //if (result == true)
                    //{
                    //    SqlParameter[] par3 = new SqlParameter[11];
                    //    par3[0] = new SqlParameter("@GL_CH_NO", IWM_CHALLAN_NO);
                    //    par3[1] = new SqlParameter("@GL_DATE", IWM_DATE);
                    //    par3[2] = new SqlParameter("@GL_GIN_TYPE", IWM_TYPE);
                    //    par3[3] = new SqlParameter("@GL_P_CODE", IWM_P_CODE);
                    //    par3[4] = new SqlParameter("@GL_I_CODE", IWD_I_CODE);
                    //    par3[5] = new SqlParameter("@GL_CQTY", IWD_REV_QTY);
                    //    par3[6] = new SqlParameter("@GL_CON_QTY", IWD_REV_QTY);
                    //    par3[7] = new SqlParameter("@GL_DOC_ID", IWD_IWM_CODE);
                    //    par3[8] = new SqlParameter("@GL_DOC_NO", IWM_NO);
                    //    par3[9] = new SqlParameter("@GL_DOC_DATE", IWM_DATE);
                    //    par3[10] = new SqlParameter("@GL_DOC_TYPE", IWM_TYPE);
                    //    // par3[11] = new SqlParameter("@GL_SIT_CODE", IWM_SITE_CODE);
                    //    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_GIN_STOCKLEDGER", par3);
                    //}
                    // 
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
            par[0] = new SqlParameter("@PK_CODE", IWM_CODE);
            par[1] = new SqlParameter("@PK_Field", "IWM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "INWARD_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //Delete from stock table
                DataTable stockqty = CommonClasses.Execute("select IWD_IWM_CODE,IWD_I_CODE,IWD_REV_QTY,IWD_CPOM_CODE from INWARD_DETAIL,INWARD_MASTER where IWD_IWM_CODE=IWM_CODE AND IWD_IWM_CODE='" + IWM_CODE + "'");
                DataTable dtitemMaster1 = new DataTable();
                for (int s = 0; s < stockqty.Rows.Count; s++)
                {
                    dtitemMaster1 = CommonClasses.Execute("select I_CURRENT_BAL,I_CODE from ITEM_MASTER where I_CODE='" + stockqty.Rows[s]["IWD_I_CODE"] + "' and ITEM_MASTER.ES_DELETE='0'");
                    for (int j = 0; j < dtitemMaster1.Rows.Count; j++)
                    {
                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtitemMaster1.Rows[j]["I_CURRENT_BAL"] + ",I_RECEIPT_DATE='" + IWM_DATE + "' where  I_CODE='" + dtitemMaster1.Rows[j]["I_CODE"] + "'");
                    }
                }
                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + IWM_CODE + "' and  STL_DOC_TYPE='IWIM' ");
                }
                for (int k = 0; k < stockqty.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + stockqty.Rows[k]["IWD_REV_QTY"] + " where  SPOD_I_CODE='" + stockqty.Rows[k]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + stockqty.Rows[k]["IWD_CPOM_CODE"] + "'");
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
    public bool CheckExistSaveNo(string genpono, string IN_MIWM_CM_CODE)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("Select IWM_NO from INWARD_MASTER where ES_DELETE<> '1' and IWM_NO='" + genpono + "' and IWM_CM_CODE=" + IN_MIWM_CM_CODE + "");
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
