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
/// Summary description for RecieptEntry_BL
/// </summary>
public class RecieptEntry_BL
{
    #region Constructor
    public RecieptEntry_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public RecieptEntry_BL(int Id)
    {
        RCP_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Variables

    private int _RCP_CODE;
    private int _RCP_NO;
    private DateTime _RCP_DATE;
    private int _RCP_P_CODE;

    private string _RCP_CHEQUE_NO;
    private DateTime _RCP_CHEQUE_DATE;
    private float _RCP_AMOUNT;
    private int _RCP_LEDGER_CODE;
    private string _RCP_REMARK;
    private int _RCP_CM_CODE;




    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int RCP_CODE
    {
        get { return _RCP_CODE; }
        set { _RCP_CODE = value; }
    }
    public int RCP_NO
    {
        get { return _RCP_NO; }
        set { _RCP_NO = value; }
    }

    public DateTime RCP_DATE
    {
        get { return _RCP_DATE; }
        set { _RCP_DATE = value; }
    }

    public int RCP_P_CODE
    {
        get { return _RCP_P_CODE; }
        set { _RCP_P_CODE = value; }
    }
    public string RCP_CHEQUE_NO
    {
        get { return _RCP_CHEQUE_NO; }
        set { _RCP_CHEQUE_NO = value; }
    }
    public DateTime RCP_CHEQUE_DATE
    {
        get { return _RCP_CHEQUE_DATE; }
        set { _RCP_CHEQUE_DATE = value; }
    }

    public float RCP_AMOUNT
    {
        get { return _RCP_AMOUNT; }
        set { _RCP_AMOUNT = value; }
    }

    #region RCP_LEDGER_RATE
    public int RCP_LEDGER_CODE
    {
        get { return _RCP_LEDGER_CODE; }
        set { _RCP_LEDGER_CODE = value; }
    }
    #endregion RCP_LEDGER_CODE

    #region RCP_REMARK
    public string RCP_REMARK
    {
        get { return _RCP_REMARK; }
        set { _RCP_REMARK = value; }
    }
    #endregion _RCP_REMARK
    public int RCP_CM_CODE
    {
        get { return _RCP_CM_CODE; }
        set { _RCP_CM_CODE = value; }
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

            dt = CommonClasses.Execute("select RCP_CODE,RCP_NO,RCP_DATE,RCP_CHEQUE_NO,convert(varchar,RCP_CHEQUE_DATE,106) as RCP_CHEQUE_DATE ,P_NAME from RECIEPT_MASTER,PARTY_MASTER where RECIEPT_MASTER.RCP_P_CODE=PARTY_MASTER.P_CODE AND RECIEPT_MASTER.ES_DELETE=0 and RCP_CM_CODE=" + RCP_CM_CODE + "");
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
            SqlParameter[] par = new SqlParameter[11];
            par[0] = new SqlParameter("@PROCESS", "Insert");
            par[1] = new SqlParameter("@RCP_CODE", DBNull.Value);
            par[2] = new SqlParameter("@RCP_NO", RCP_NO);
            par[3] = new SqlParameter("@RCP_DATE", RCP_DATE);
            par[4] = new SqlParameter("@RCP_P_CODE", RCP_P_CODE);
            par[5] = new SqlParameter("@RCP_CHEQUE_NO", RCP_CHEQUE_NO);
            par[6] = new SqlParameter("@RCP_CHEQUE_DATE", RCP_CHEQUE_DATE);
            par[7] = new SqlParameter("@RCP_AMOUNT", RCP_AMOUNT);
            par[8] = new SqlParameter("@RCP_LEDGER_CODE", RCP_LEDGER_CODE);
            par[9] = new SqlParameter("@RCP_REMARK", RCP_REMARK);
            par[10] = new SqlParameter("@RCP_CM_CODE", RCP_CM_CODE);





            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_RECIEPT_MASTER", par, out message, out PK_CODE);


            if (result == true)
            {

                CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PK_CODE + "','" + PK_CODE + "','" + RCP_NO + "','RECIEPTENTRY','" + Convert.ToDateTime(RCP_DATE).ToString("dd/MMM/yyyy") + "','" + RCP_AMOUNT + "','" + RCP_P_CODE + "')");

                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int RCPD_RCP_CODE = PK_CODE;
                    int RCPD_INVOICE_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_INVOICE_CODE")).Text);
                    int RCPD_INVOICE_CODE_TEMP = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_INVOICE_CODE_TEMP")).Text);
                    int RCPD_REF_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_REF_CODE")).Text);

                    string rcpdamt = (((Label)XGrid.Rows[i].FindControl("lblRCPD_AMOUNT")).Text).Replace("-", "");


                    float RCPD_AMOUNT = float.Parse(rcpdamt);
                    float RCPD_ADJ_AMOUNT =float.Parse(((Label)XGrid.Rows[i].FindControl("lblRCPD_ADJ_AMOUNT")).Text);

                     
                    string RCPD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblRCPD_REMARK")).Text;
                    int RCPD_TYPE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_TYPE")).Text);

                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[9];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@RCPD_RCP_CODE", RCPD_RCP_CODE);
                    par1[2] = new SqlParameter("@RCPD_REF_CODE", RCPD_REF_CODE);
                    par1[3] = new SqlParameter("@RCPD_INVOICE_CODE", RCPD_INVOICE_CODE);
                    par1[4] = new SqlParameter("@RCPD_INVOICE_CODE_TEMP", RCPD_INVOICE_CODE_TEMP);
                    par1[5] = new SqlParameter("@RCPD_AMOUNT", RCPD_AMOUNT);
                    par1[6] = new SqlParameter("@RCPD_ADJ_AMOUNT", RCPD_ADJ_AMOUNT);
                    par1[7] = new SqlParameter("@RCPD_REMARK", RCPD_REMARK);
                    par1[8] = new SqlParameter("@RCPD_TYPE", RCPD_TYPE);


                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_RECIEPT_DETAIL", par1, out message);


                    if (result == true)
                    {
                        if (RCPD_ADJ_AMOUNT !=0)
                        {


                            CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + RCPD_INVOICE_CODE + "','" + RCPD_RCP_CODE + "','" + RCP_NO + "','ADJRECIEPTENTRY','" + Convert.ToDateTime(RCP_DATE).ToString("dd/MMM/yyyy") + "','" + RCPD_ADJ_AMOUNT + "','" + RCP_P_CODE + "')");
                        
                        }

                        if (RCPD_TYPE == 1)
                        {
                            CommonClasses.Execute("Update INVOICE_MASTER set INM_RECIEVED_AMT=ISNULL(INM_RECIEVED_AMT,0)+ " + RCPD_AMOUNT + "  where INM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }
                        if (RCPD_TYPE == 0)
                        {
                            CommonClasses.Execute("Update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)+ " + RCPD_AMOUNT + "  where CNM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }
                        if (RCPD_TYPE == 2)
                        {
                            CommonClasses.Execute("Update DEBIT_NOTE_MASTER set DNM_RECIEVED_AMT=ISNULL(DNM_RECIEVED_AMT,0)+ " + RCPD_AMOUNT + "  where CNM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }

                        if (RCPD_TYPE == 3)
                        {
                            CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)+ " + RCPD_AMOUNT + "  where BPM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }

                    }

                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "Save", Ex.Message);

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
            DataTable dtInwardDetail = CommonClasses.Execute("select RCPD_RCP_CODE,RCPD_INVOICE_CODE,RCPD_AMOUNT ,RCPD_TYPE  from RECIEPT_DETAIL,RECIEPT_MASTER where RCPD_RCP_CODE=RCP_CODE and RCPD_RCP_CODE='" + RCP_CODE + "' and RECIEPT_MASTER.ES_DELETE='0'");
            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
            {
                if (dtInwardDetail.Rows[k]["RCPD_TYPE"].ToString() == "1")
                {
                    CommonClasses.Execute("Update INVOICE_MASTER set INM_RECIEVED_AMT=ISNULL(INM_RECIEVED_AMT,0)-" + dtInwardDetail.Rows[k]["RCPD_AMOUNT"] + " where INM_CODE='" + dtInwardDetail.Rows[k]["RCPD_INVOICE_CODE"] + "'");

                }
                else
                {
                    CommonClasses.Execute("Update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)-" + dtInwardDetail.Rows[k]["RCPD_AMOUNT"] + " where CNM_CODE='" + dtInwardDetail.Rows[k]["RCPD_INVOICE_CODE"] + "'");

                }
                // CommonClasses.Execute("Update ITEM_MASTER set I_LEDGERENT_BAL=I_LEDGERENT_BAL-" + dtInwardDetail.Rows[k]["RCPD_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["RCPD_I_CODE"] + "'");

                PK_CODE = RCP_CODE;

            }

            //updating Master Record
            SqlParameter[] par = new SqlParameter[11];
            par[0] = new SqlParameter("@PROCESS", "Update");
            par[1] = new SqlParameter("@RCP_CODE", RCP_CODE);
            par[2] = new SqlParameter("@RCP_NO", RCP_NO);
            par[3] = new SqlParameter("@RCP_DATE", RCP_DATE);
            par[4] = new SqlParameter("@RCP_P_CODE", RCP_P_CODE);
            par[5] = new SqlParameter("@RCP_CHEQUE_NO", RCP_CHEQUE_NO);
            par[6] = new SqlParameter("@RCP_CHEQUE_DATE", RCP_CHEQUE_DATE);
            par[7] = new SqlParameter("@RCP_AMOUNT", RCP_AMOUNT);
            par[8] = new SqlParameter("@RCP_LEDGER_CODE", RCP_LEDGER_CODE);
            par[9] = new SqlParameter("@RCP_REMARK", RCP_REMARK);
            par[10] = new SqlParameter("@RCP_CM_CODE", RCP_CM_CODE);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_RECIEPT_MASTER", par, out message, out PK_CODE);
            //result = DL_DBAccess.Insertion_Updation_Delete_Modify("SP_UPDATE_RECIEPT_MASTER", par, out PK_CODE);


            //Deleteing Inward Detail part
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM RECIEPT_DETAIL WHERE RCPD_RCP_CODE='" + PK_CODE + "'");
                result = CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + PK_CODE + "' and ACCNT_DOC_TYPE='RECIEPTENTRY'");
                //CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='" + RCP_TYPE + "'");
                //CommonClasses.Execute("DELETE FROM GIN_STOCK_LEDGER where GL_DOC_ID='" + PK_CODE + "' and GL_DOC_TYPE='" + RCP_TYPE + "'");

            }

            if (result == true)
            {
                CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PK_CODE + "','" + PK_CODE + "','" + RCP_NO + "','RECIEPTENTRY','" + Convert.ToDateTime(RCP_DATE).ToString("dd/MMM/yyyy") + "','" + RCP_AMOUNT + "','" + RCP_P_CODE + "')");
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int RCPD_RCP_CODE = PK_CODE;
                    int RCPD_INVOICE_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_INVOICE_CODE")).Text);
                    int RCPD_INVOICE_CODE_TEMP = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_INVOICE_CODE_TEMP")).Text);
                    int RCPD_REF_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_REF_CODE")).Text);
                    float RCPD_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblRCPD_AMOUNT")).Text);

                    float RCPD_ADJ_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblRCPD_ADJ_AMOUNT")).Text);
                    string RCPD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblRCPD_REMARK")).Text;

                    int RCPD_TYPE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblRCPD_TYPE")).Text);

                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[9];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@RCPD_RCP_CODE", RCPD_RCP_CODE);
                    par1[2] = new SqlParameter("@RCPD_REF_CODE", RCPD_REF_CODE);
                    par1[3] = new SqlParameter("@RCPD_INVOICE_CODE", RCPD_INVOICE_CODE);
                    par1[4] = new SqlParameter("@RCPD_INVOICE_CODE_TEMP", RCPD_INVOICE_CODE_TEMP);
                    par1[5] = new SqlParameter("@RCPD_AMOUNT", RCPD_AMOUNT);
                    par1[6] = new SqlParameter("@RCPD_ADJ_AMOUNT", RCPD_ADJ_AMOUNT);
                    par1[7] = new SqlParameter("@RCPD_REMARK", RCPD_REMARK);
                    par1[8] = new SqlParameter("@RCPD_TYPE", RCPD_TYPE);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_RECIEPT_DETAIL", par1, out message);



                    if (RCPD_ADJ_AMOUNT != 0)
                    {


                        CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + RCPD_INVOICE_CODE + "','" + RCPD_RCP_CODE + "','" + RCP_NO + "','ADJRECIEPTENTRY','" + Convert.ToDateTime(RCP_DATE).ToString("dd/MMM/yyyy") + "','" + RCPD_ADJ_AMOUNT + "','" + RCP_P_CODE + "')");

                    }






                        if (RCPD_TYPE == 1)
                        {
                            CommonClasses.Execute("Update INVOICE_MASTER set INM_RECIEVED_AMT=ISNULL(INM_RECIEVED_AMT,0)+ " + RCPD_AMOUNT + "  where INM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }
                        if (RCPD_TYPE == 0)
                        {
                            CommonClasses.Execute("Update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)+ " + RCPD_AMOUNT + "  where CNM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }

                        if (RCPD_TYPE == 2)
                        {
                            CommonClasses.Execute("Update DEBIT_NOTE_MASTER set DNM_PAID_AMT=ISNULL(DNM_PAID_AMT,0)+ " + RCPD_AMOUNT + "  where CNM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }

                        if (RCPD_TYPE == 3)
                        {
                            CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)+ " + RCPD_AMOUNT + "  where BPM_CODE='" + RCPD_INVOICE_CODE + "'");

                        }
                    
                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "Update", Ex.Message);

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
            par[0] = new SqlParameter("@PK_CODE", RCP_CODE);
            par[1] = new SqlParameter("@PK_Field", "RCP_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "RECIEPT_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //Delete from stock table
                DataTable stockqty = CommonClasses.Execute("select RCPD_RCP_CODE,RCPD_AMOUNT from RECIEPT_DETAIL,RECIEPT_MASTER where RCPD_RCP_CODE=RCP_CODE AND RCPD_RCP_CODE='" + RCP_CODE + "'");

                for (int k = 0; k < stockqty.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update INVOICE_MASTER set INM_RECIEVED_AMT=ISNULL(INM_RECIEVED_AMT,0)-" + stockqty.Rows[k]["RCPD_AMOUNT"] + " where INM_CODE='" + stockqty.Rows[k]["RCPD_INVOICE_CODE"] + "'");
                }

            }

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion

    #region CheckExistSaveNo
    public bool CheckExistSaveNo(string genpono, string IN_MRCP_CM_CODE)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("Select RCP_NO from RECIEPT_MASTER where ES_DELETE<> '1' and RCP_NO='" + genpono + "' and RCP_CM_CODE=" + IN_MRCP_CM_CODE + "");
            if (dt.Rows.Count > 0)
            {
                res = true;
                //MessageBox.Show("GIN  Number Already Exists");
                // throw new Exception("Inward Number Already Exists");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "CheckExistSaveNo", Ex.Message);

        }
        return res;
    }
    #endregion

}
