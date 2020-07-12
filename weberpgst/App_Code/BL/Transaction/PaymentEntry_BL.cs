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
/// Summary description for PaymentEntry_BL
/// </summary>
public class PaymentEntry_BL
{
    #region Constructor
    public PaymentEntry_BL()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

     #region Parameterise Constructor
    public PaymentEntry_BL(int Id)
    {
        PAYM_CODE = Id;
    }
    #endregion

    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;

    static DataTable dt2 = new DataTable();
    #endregion

    #region Variables

    private int _PAYM_CODE;
    private int _PAYM_NO;
    private DateTime _PAYM_DATE;
    private int _PAYM_P_CODE;
    
    private string _PAYM_CHEQUE_NO;
    private DateTime _PAYM_CHEQUE_DATE;
    private float _PAYM_AMOUNT;
    private int _PAYM_LEDGER_CODE;
    private string _PAYM_REMARK;
    private int _PAYM_CM_CODE;
    

    

    public string message = "";

    public string Msg = "";
    #endregion

    #region Public Properties
    public int PAYM_CODE
    {
        get { return _PAYM_CODE; }
        set { _PAYM_CODE = value; }
    }
    public int PAYM_NO
    {
        get { return _PAYM_NO; }
        set { _PAYM_NO = value; }
    }

    public DateTime PAYM_DATE
    {
        get { return _PAYM_DATE; }
        set { _PAYM_DATE = value; }
    }

    public int PAYM_P_CODE
    {
        get { return _PAYM_P_CODE; }
        set { _PAYM_P_CODE = value; }
    }
    public string PAYM_CHEQUE_NO
    {
        get { return _PAYM_CHEQUE_NO; }
        set { _PAYM_CHEQUE_NO = value; }
    }
    public DateTime PAYM_CHEQUE_DATE
    {
        get { return _PAYM_CHEQUE_DATE; }
        set { _PAYM_CHEQUE_DATE = value; }
    }

    public float PAYM_AMOUNT
    {
        get { return _PAYM_AMOUNT; }
        set { _PAYM_AMOUNT = value; }
    }

    #region PAYM_LEDGER_RATE
    public int PAYM_LEDGER_CODE
    {
        get { return _PAYM_LEDGER_CODE; }
        set { _PAYM_LEDGER_CODE = value; }
    }
    #endregion PAYM_LEDGER_CODE

    #region PAYM_REMARK
    public string PAYM_REMARK
    {
        get { return _PAYM_REMARK; }
        set { _PAYM_REMARK = value; }
    }
    #endregion _PAYM_REMARK
    public int PAYM_CM_CODE
    {
        get { return _PAYM_CM_CODE; }
        set { _PAYM_CM_CODE = value; }
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

            dt = CommonClasses.Execute("select PAYM_CODE,PAYM_NO,PAYM_DATE,PAYM_CHEQUE_NO,convert(varchar,PAYM_CHEQUE_DATE,106) as PAYM_CHEQUE_DATE ,P_NAME from PAYMENT_MASTER,PARTY_MASTER where PAYMENT_MASTER.PAYM_P_CODE=PARTY_MASTER.P_CODE AND PAYMENT_MASTER.ES_DELETE=0 and PAYM_CM_CODE=" + PAYM_CM_CODE + "");
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
            par[1] = new SqlParameter("@PAYM_CODE", DBNull.Value);
            par[2] = new SqlParameter("@PAYM_NO", PAYM_NO);
            par[3] = new SqlParameter("@PAYM_DATE", PAYM_DATE);
            par[4] = new SqlParameter("@PAYM_P_CODE", PAYM_P_CODE);
            par[5] = new SqlParameter("@PAYM_CHEQUE_NO", PAYM_CHEQUE_NO);
            par[6] = new SqlParameter("@PAYM_CHEQUE_DATE", PAYM_CHEQUE_DATE);
            par[7] = new SqlParameter("@PAYM_AMOUNT", PAYM_AMOUNT);
            par[8] = new SqlParameter("@PAYM_LEDGER_CODE", PAYM_LEDGER_CODE);
            par[9] = new SqlParameter("@PAYM_REMARK", PAYM_REMARK);
            par[10] = new SqlParameter("@PAYM_CM_CODE", PAYM_CM_CODE);
            
         
            
            

            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PAYMENT_MASTER", par, out message, out PK_CODE);
            if (result == true)
            {
                CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PK_CODE + "','" + PK_CODE + "','" + PAYM_NO + "','PAYMENTENTRY','" + Convert.ToDateTime(PAYM_DATE).ToString("dd/MMM/yyyy") + "','" + PAYM_AMOUNT + "','" + PAYM_P_CODE + "')");
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int PAYMD_PAYM_CODE = PK_CODE;
                    int PAYMD_INVOICE_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_INVOICE_CODE")).Text);
                    int PAYMD_INVOICE_CODE_TEMP = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_INVOICE_CODE_TEMP")).Text);
                    int PAYMD_REF_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_REF_CODE")).Text);
                    float PAYMD_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);
                    
                    float PAYMD_ADJ_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPAYMD_ADJ_AMOUNT")).Text);
                    string PAYMD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblPAYMD_REMARK")).Text;

                    int PAYMD_TYPE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_TYPE")).Text);
                    string PAYMD_INVOICE_NO = ((Label)XGrid.Rows[i].FindControl("lblINVOICE_NO")).Text;
                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[10];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@PAYMD_PAYM_CODE", PAYMD_PAYM_CODE);
                    par1[2] = new SqlParameter("@PAYMD_REF_CODE", PAYMD_REF_CODE);
                    par1[3] = new SqlParameter("@PAYMD_INVOICE_CODE", PAYMD_INVOICE_CODE);
                    par1[4] = new SqlParameter("@PAYMD_INVOICE_CODE_TEMP", PAYMD_INVOICE_CODE_TEMP);
                    par1[5] = new SqlParameter("@PAYMD_AMOUNT", PAYMD_AMOUNT);
                    par1[6] = new SqlParameter("@PAYMD_ADJ_AMOUNT", PAYMD_ADJ_AMOUNT);
                    par1[7] = new SqlParameter("@PAYMD_REMARK", PAYMD_REMARK);
                    par1[8] = new SqlParameter("@PAYMD_TYPE", PAYMD_TYPE);
                    par1[9] = new SqlParameter("@PAYMD_INVOICE_NO", PAYMD_INVOICE_NO);

                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PAYMENT_DETAIL", par1, out message);


                    if (result == true)
                    {
                        if (PAYMD_ADJ_AMOUNT != 0)
                        {

                            CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PAYMD_INVOICE_CODE + "','" + PAYMD_PAYM_CODE + "','" + PAYM_NO + "','ADJPAYMENTENTRY','" + Convert.ToDateTime(PAYM_DATE).ToString("dd/MMM/yyyy") + "','" + PAYMD_ADJ_AMOUNT + "','" + PAYM_P_CODE + "')");
                        }


                        if (PAYMD_TYPE==1)
                        {
                            CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)+ " + PAYMD_AMOUNT + "  where BPM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }
                        if (PAYMD_TYPE == 0)
                        {
                            CommonClasses.Execute("Update DEBIT_NOTE_MASTER set DNM_PAID_AMT=ISNULL(DNM_PAID_AMT,0)+ " + PAYMD_AMOUNT + "  where DNM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }

                        if (PAYMD_TYPE == 2)
                        {
                            CommonClasses.Execute("Update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)+ " + PAYMD_AMOUNT + "  where CNM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }
                        

                    }
                   
                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payemt Entry", "Save", Ex.Message);

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
            DataTable dtInwardDetail = CommonClasses.Execute("select PAYMD_PAYM_CODE,PAYMD_INVOICE_CODE,PAYMD_AMOUNT,PAYMD_TYPE from PAYMENT_DETAIL,PAYMENT_MASTER where PAYMD_PAYM_CODE=PAYM_CODE and PAYMD_PAYM_CODE='" + PAYM_CODE + "' and PAYMENT_MASTER.ES_DELETE='0'");   
            for (int k = 0; k < dtInwardDetail.Rows.Count; k++)
            {
                if (dtInwardDetail.Rows[k]["PAYMD_TYPE"].ToString()=="1")
                {
                    CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)-" + dtInwardDetail.Rows[k]["PAYMD_AMOUNT"] + " where BPM_CODE='" + dtInwardDetail.Rows[k]["PAYMD_INVOICE_CODE"] + "'");
                }
                else
                {
                    CommonClasses.Execute("Update DEBIT_NOTE_MASTER set DNM_PAID_AMT=ISNULL(DNM_PAID_AMT,0)-" + dtInwardDetail.Rows[k]["PAYMD_AMOUNT"] + " where DNM_CODE='" + dtInwardDetail.Rows[k]["PAYMD_INVOICE_CODE"] + "'");
                }
                // CommonClasses.Execute("Update ITEM_MASTER set I_LEDGERENT_BAL=I_LEDGERENT_BAL-" + dtInwardDetail.Rows[k]["PAYMD_REV_QTY"] + " where  I_CODE='" + dtInwardDetail.Rows[k]["PAYMD_I_CODE"] + "'");

                PK_CODE = PAYM_CODE;
                
            }

            //updating Master Record
            SqlParameter[] par = new SqlParameter[11];
            par[0] = new SqlParameter("@PROCESS", "Update");
            par[1] = new SqlParameter("@PAYM_CODE", PAYM_CODE);
            par[2] = new SqlParameter("@PAYM_NO", PAYM_NO);
            par[3] = new SqlParameter("@PAYM_DATE", PAYM_DATE);
            par[4] = new SqlParameter("@PAYM_P_CODE", PAYM_P_CODE);
            par[5] = new SqlParameter("@PAYM_CHEQUE_NO", PAYM_CHEQUE_NO);
            par[6] = new SqlParameter("@PAYM_CHEQUE_DATE", PAYM_CHEQUE_DATE);
            par[7] = new SqlParameter("@PAYM_AMOUNT", PAYM_AMOUNT);
            par[8] = new SqlParameter("@PAYM_LEDGER_CODE", PAYM_LEDGER_CODE);
            par[9] = new SqlParameter("@PAYM_REMARK", PAYM_REMARK);
            par[10] = new SqlParameter("@PAYM_CM_CODE", PAYM_CM_CODE);
           
            result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PAYMENT_MASTER", par, out message, out PK_CODE);
            //result = DL_DBAccess.Insertion_Updation_Delete_Modify("SP_UPDATE_PAYMENT_MASTER", par, out PK_CODE);
            

            //Deleteing Inward Detail part
            if (result == true)
            {
                CommonClasses.Execute("DELETE FROM PAYMENT_DETAIL WHERE PAYMD_PAYM_CODE='" + PK_CODE + "'");
                //CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + PK_CODE + "' and  STL_DOC_TYPE='" + PAYM_TYPE + "'");
                //CommonClasses.Execute("DELETE FROM GIN_STOCK_LEDGER where GL_DOC_ID='" + PK_CODE + "' and GL_DOC_TYPE='" + PAYM_TYPE + "'");
                CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + PK_CODE + "' and ACCNT_DOC_TYPE='PAYMENTENTRY'");
                CommonClasses.Execute1("DELETE FROM ACCOUNTS_LEDGER WHERE ACCNT_DOC_NO='" + PK_CODE + "' and ACCNT_DOC_TYPE='ADJPAYMENTENTRY'");
            }

            if (result == true)
            {
                CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PK_CODE + "','" + PK_CODE + "','" + PAYM_NO + "','PAYMENTENTRY','" + Convert.ToDateTime(PAYM_DATE).ToString("dd/MMM/yyyy") + "','" + PAYM_AMOUNT + "','" + PAYM_P_CODE + "')");
                for (int i = 0; i < XGrid.Rows.Count; i++)
                {

                    int PAYMD_PAYM_CODE = PK_CODE;
                    int PAYMD_INVOICE_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_INVOICE_CODE")).Text);
                    int PAYMD_INVOICE_CODE_TEMP = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_INVOICE_CODE_TEMP")).Text);
                    int PAYMD_REF_CODE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_REF_CODE")).Text);
                    float PAYMD_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPAYMD_AMOUNT")).Text);

                    float PAYMD_ADJ_AMOUNT = float.Parse(((Label)XGrid.Rows[i].FindControl("lblPAYMD_ADJ_AMOUNT")).Text);
                    string PAYMD_REMARK = ((Label)XGrid.Rows[i].FindControl("lblPAYMD_REMARK")).Text;
                    int PAYMD_TYPE = Convert.ToInt32(((Label)XGrid.Rows[i].FindControl("lblPAYMD_TYPE")).Text);
                    string PAYMD_INVOICE_NO = ((Label)XGrid.Rows[i].FindControl("lblINVOICE_NO")).Text;
                    //Inserting Inward Detail Part
                    SqlParameter[] par1 = new SqlParameter[10];
                    par1[0] = new SqlParameter("@PROCESS", "Insert");
                    par1[1] = new SqlParameter("@PAYMD_PAYM_CODE", PAYMD_PAYM_CODE);
                    par1[2] = new SqlParameter("@PAYMD_REF_CODE", PAYMD_REF_CODE);
                    par1[3] = new SqlParameter("@PAYMD_INVOICE_CODE", PAYMD_INVOICE_CODE);
                    par1[4] = new SqlParameter("@PAYMD_INVOICE_CODE_TEMP", PAYMD_INVOICE_CODE_TEMP);
                    par1[5] = new SqlParameter("@PAYMD_AMOUNT", PAYMD_AMOUNT);
                    par1[6] = new SqlParameter("@PAYMD_ADJ_AMOUNT", PAYMD_ADJ_AMOUNT);
                    par1[7] = new SqlParameter("@PAYMD_REMARK", PAYMD_REMARK);
                    par1[8] = new SqlParameter("@PAYMD_TYPE", PAYMD_TYPE);
                    par1[9] = new SqlParameter("@PAYMD_INVOICE_NO", PAYMD_INVOICE_NO);



                    result = DL_DBAccess.Insertion_Updation_Delete("SP_INSERT_PAYMENT_DETAIL", par1, out message);


                    if (result == true)
                    {


                        if (PAYMD_ADJ_AMOUNT != 0)
                        {

                            CommonClasses.Execute1("INSERT INTO ACCOUNTS_LEDGER (ACCNT_I_CODE,ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_TYPE,ACCNT_DOC_DATE,ACCNT_DOC_QTY,ACCNT_P_CODE) VALUES ('" + PAYMD_INVOICE_CODE + "','" + PAYMD_PAYM_CODE + "','" + PAYM_NO + "','ADJPAYMENTENTRY','" + Convert.ToDateTime(PAYM_DATE).ToString("dd/MMM/yyyy") + "','" + PAYMD_ADJ_AMOUNT + "','" + PAYM_P_CODE + "')");
                        }



                        if (PAYMD_TYPE == 1)
                        {
                            CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)+ " + PAYMD_AMOUNT + "  where BPM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }
                        if (PAYMD_TYPE == 0)
                        {
                            CommonClasses.Execute("Update DEBIT_NOTE_MASTER set DNM_PAID_AMT=ISNULL(DNM_PAID_AMT,0)+ " + PAYMD_AMOUNT + "  where DNM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }
                        if (PAYMD_TYPE == 2)
                        {
                            CommonClasses.Execute("Update CREDIT_NOTE_MASTER set CNM_RECIEVED_AMT=ISNULL(CNM_RECIEVED_AMT,0)+ " + PAYMD_AMOUNT + "  where CNM_CODE='" + PAYMD_INVOICE_CODE + "'");
                        }

                    }

                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payemt Entry", "Update", Ex.Message);

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
            par[0] = new SqlParameter("@PK_CODE", PAYM_CODE);
            par[1] = new SqlParameter("@PK_Field", "PAYM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "PAYMENT_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            if (result == true)
            {
                //Delete from stock table
                DataTable stockqty = CommonClasses.Execute("select PAYMD_PAYM_CODE,PAYMD_AMOUNT from PAYMENT_DETAIL,PAYMENT_MASTER where PAYMD_PAYM_CODE=PAYM_CODE AND PAYMD_PAYM_CODE='" + PAYM_CODE + "'");
                
                for (int k = 0; k < stockqty.Rows.Count; k++)
                {
                    CommonClasses.Execute("Update BILL_PASSING_MASTER set BPM_PAID_AMT=ISNULL(BPM_PAID_AMT,0)-" + stockqty.Rows[k]["PAYMD_AMOUNT"] + " where BPM_CODE='" + stockqty.Rows[k]["PAYMD_INVOICE_CODE"] + "'");
                }

            }

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payemt Entry", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion

    #region CheckExistSaveNo
    public bool CheckExistSaveNo(string genpono, string IN_MPAYM_CM_CODE)
    {
        bool res = false;
        try
        {
            DataTable dt = CommonClasses.Execute("Select PAYM_NO from PAYMENT_MASTER where ES_DELETE<> '1' and PAYM_NO='" + genpono + "' and PAYM_CM_CODE=" + IN_MPAYM_CM_CODE + "");
            if (dt.Rows.Count > 0)
            {
                res = true;
                //MessageBox.Show("GIN  Number Already Exists");
                // throw new Exception("Inward Number Already Exists");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Payemt Entry", "CheckExistSaveNo", Ex.Message);

        }
        return res;
    }
    #endregion

}
