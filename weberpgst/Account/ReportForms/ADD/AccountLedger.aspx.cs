using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data.Sql;
public partial class Account_ADD_AccountLedger : System.Web.UI.Page
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

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string StrCond = Request.QueryString[6].ToString();

            string group = Request.QueryString[4].ToString();
            GenerateReport(Title, From, To, StrCond, group);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string StrCond, string group)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            
            DataTable dtAccountsLedger = new DataTable();

            string finyear = Session["CompanyFinancialYr"].ToString();
            string sep = "-";

            string[] splitContent = finyear.Split(sep.ToCharArray());
            int yearid = (Convert.ToInt32(splitContent[0])-1);
            string year = yearid.ToString();


            ds = null;
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@Cond", StrCond);
            par[1] = new SqlParameter("@yrid", year);
            
            string Query = "";

            ds = DL_DBAccess.SelectDataDataset("AccountledgerwithOopening", par, "dtAccountsLedger");
            dtAccountsLedger = ds.Tables[0];
            

            

            #region Crystal_Report
            if (dtAccountsLedger.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                string head = "";
                rptname.Load(Server.MapPath("~/Account/Reports/AccountLedgerReport.rpt"));
                rptname.FileName = Server.MapPath("~/Account/Reports/AccountLedgerReport.rpt");
                head = "Cash Book Receipt Datewise";
                rptname.Refresh();
                rptname.SetDataSource(dtAccountsLedger);
                rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                rptname.SetParameterValue("txtTitle", head);
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Account/VIEW/ViewAccountLedger.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
}


// Query = "SELECT BPM_INV_NO,BPM_INV_DATE,BPM_BASIC_AMT,BPM_EXCIES_AMT,BPM_ECESS_AMT,BPM_HECESS_AMT,BPM_TAX_AMT,BPM_DISCOUNT_AMT,BPM_DISC_AMT,BPM_FREIGHT,BPM_PACKING_AMT,BPM_OCTRO_AMT,BPM_G_AMT,P_NAME FROM BILL_PASSING_MASTER, PARTY_MASTER WHERE BPM_P_CODE=P_CODE AND BILL_PASSING_MASTER.ES_DELETE=0";
//Query = " select P_CODE,P_NAME,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE ,case when ACCNT_DOC_TYPE='BILLPASSING' OR ACCNT_DOC_TYPE='CRDENTRY' OR ACCNT_DOC_TYPE='RECIEPTENTRY' THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY,case when ACCNT_DOC_TYPE='TAXINV' OR ACCNT_DOC_TYPE='DEBITENTRY' OR ACCNT_DOC_TYPE='PAYMENTENTRY'  OR ACCNT_DOC_TYPE='ADJPAYMENTENTRY' THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY1 into #Temp from ACCOUNTS_LEDGER inner join PARTY_MASTER ON ACCNT_P_CODE=P_CODE WHERE PARTY_MASTER.ES_DELETE=0 select P_CODE,P_NAME,0 as ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE,ACCNT_DOC_QTY,ACCNT_DOC_QTY1,ISNULL(ACCNT_DOC_QTY,0)-ISNULL(ACCNT_DOC_QTY1,0) as Total from #Temp Drop table #Temp";


//Query = " select ACCNT_CODE,P_CODE,P_NAME,CAST('' AS VARCHAR(max)) as ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE , case when ACCNT_DOC_TYPE='BILLPASSING' OR ACCNT_DOC_TYPE='CRDENTRY' OR ACCNT_DOC_TYPE='RECIEPTENTRY' THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY, case when ACCNT_DOC_TYPE='TAXINV' OR ACCNT_DOC_TYPE='DEBITENTRY' OR ACCNT_DOC_TYPE='PAYMENTENTRY' OR ACCNT_DOC_TYPE='ADJPAYMENTENTRY' THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY1, case ACCNT_DOC_TYPE when 'ADJPAYMENTENTRY' then ACCNT_DOC_QTY else 0  end  AS  Total  into #Temp from ACCOUNTS_LEDGER inner join PARTY_MASTER ON ACCNT_P_CODE=P_CODE WHERE PARTY_MASTER.ES_DELETE=0  declare @temp table(id int identity(1,1),fpkey int,Ttype varchar(50),pkey int) insert into @temp select ACCNT_I_CODE,ACCNT_DOC_TYPE,ACCNT_CODE from #Temp declare @cnt int=0,@i int=1,@typ varchar(500),@mkey int,@dinvoice int,@pkey int,@invoiceno varchar(max),@amt varchar(max),@chequeno varchar(500),@paymdtype int select @cnt=count(*) from @temp while(@cnt>=@i) begin select @typ=Ttype,@mkey=fpkey,@pkey=pkey from @temp where id=@i if(@typ = 'PAYMENTENTRY') begin  declare @count int=0,@j int=1,@paymdinvoicecode int,@pmdtype int,@pmdamt varchar(max), @string varchar(max)='', @delimeter varchar(max)=' - ', @delimeter1 varchar(max)=' / ',@PAYMADJAMT VARCHAR(MAX),@PAYMADJREMARK VARCHAR(MAX),@STRINGADJ VARCHAR(MAX)=''  select @invoiceno=PAYM_NO,@chequeno=PAYM_CHEQUE_NO from PAYMENT_MASTER where PAYM_CODE=@mkey  select idd = Row_number() OVER (ORDER BY PAYMD_INVOICE_CODE),PAYMD_INVOICE_CODE,PAYMD_TYPE,PAYMD_AMOUNT,PAYMD_ADJ_AMOUNT,PAYMD_REMARK into #paymentdetails  from PAYMENT_DETAIL where PAYMD_PAYM_CODE=@mkey  select @count=count(*) from #paymentdetails  while(@count>=@j) begin select @paymdinvoicecode=PAYMD_INVOICE_CODE,@paymdtype=PAYMD_TYPE,@pmdamt=PAYMD_AMOUNT,@PAYMADJAMT=PAYMD_ADJ_AMOUNT,@PAYMADJREMARK=PAYMD_REMARK from #paymentdetails where idd=@j if(@paymdtype =1) begin select @string= @string + cast(BPM_INV_NO as varchar(5000)) from BILL_PASSING_MASTER where BPM_CODE=@paymdinvoicecode IF(@PAYMADJAMT != '0') BEGIN select @STRINGADJ= @STRINGADJ + cast(BPM_INV_NO as varchar(5000)) from BILL_PASSING_MASTER where BPM_CODE=@paymdinvoicecode set @STRINGADJ=@PAYMADJREMARK+'  '+ @STRINGADJ update #Temp set ACCNT_DOC_NO=@STRINGADJ where ACCNT_I_CODE=@paymdinvoicecode and ACCNT_DOC_TYPE='ADJPAYMENTENTRY' AND ACCNT_DOC_QTY1=@PAYMADJAMT	END set @string=@string + @delimeter +@pmdamt +@delimeter1	 end       if(@paymdtype =7) begin select @string= @string + cast(REF_NAME as varchar(5000)) from NEWREFERENCE_MASTER where REF_CODE=@paymdinvoicecode set @string=@string + @delimeter +@pmdamt +@delimeter1 end set @STRINGADJ='' set @j=@j+1 end update #Temp set ACCNT_DOC_NO=@chequeno+' '+ @string where ACCNT_I_CODE=@mkey and ACCNT_CODE=@pkey drop table #paymentdetails End IF(@typ='BILLPASSING') BEGIN DECLARE @BILLPASSING VARCHAR(MAX)='' SELECT @BILLPASSING='Bill No. ' + BPM_INV_NO from BILL_PASSING_MASTER where BPM_CODE=@mkey update #Temp set ACCNT_DOC_NO=@BILLPASSING where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='BILLPASSING' END IF(@typ='TAXINV') BEGIN DECLARE @TAXINV VARCHAR(MAX)='' ,@ACCNT_DOC_CODE int select @ACCNT_DOC_CODE=ACCNT_DOC_NO from ACCOUNTS_LEDGER where ACCNT_CODE=@pkey SELECT @TAXINV='Bill No. ' + cast(INM_NO as varchar(max)) from INVOICE_MASTER where INM_CODE=@ACCNT_DOC_CODE update #Temp set ACCNT_DOC_NO=@TAXINV where ACCNT_CODE=@pkey and ACCNT_DOC_TYPE='TAXINV' END  IF(@typ='DEBITENTRY')  BEGIN DECLARE @DEBITTNOTE VARCHAR(MAX)='',@DEBITTNOTEPK INT SELECT @DEBITTNOTEPK=ACCNT_DOC_NO FROM ACCOUNTS_LEDGER WHERE ACCNT_CODE=@pkey SELECT @DEBITTNOTE='DN.No. ' + DNM_SERIAL_NO +' ' + DNM_REMARKS from DEBIT_NOTE_MASTER where DNM_CODE=@DEBITTNOTEPK update #Temp set ACCNT_DOC_NO=@DEBITTNOTE where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='DEBITENTRY' END IF(@typ='CRDENTRY') BEGIN DECLARE @CREDITTNOTE VARCHAR(MAX)='',@CREDITNOTEPK INT SELECT @CREDITNOTEPK=ACCNT_DOC_NO FROM ACCOUNTS_LEDGER WHERE ACCNT_CODE=@pkey SELECT @CREDITTNOTE='CN.No. ' + CNM_SERIAL_NO +' ' + DNM_REMARKS from credit_NOTE_MASTER where CNM_CODE=@CREDITNOTEPK update #Temp set ACCNT_DOC_NO=@CREDITTNOTE where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='CRDENTRY' END set @i=@i+1  end select P_CODE,P_NAME,case WHEN ACCNT_DOC_NO LIKE '%/'  THEN LEFT(ACCNT_DOC_NO, LEN(ACCNT_DOC_NO)-1)    ELSE ACCNT_DOC_NO end as ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE,ACCNT_DOC_QTY,ACCNT_DOC_QTY1, Total from #Temp Drop table #Temp";


// Working Query Backup 14 April 2019
//Query = " select ACCNT_CODE,P_CODE,P_NAME,CAST('' AS VARCHAR(max)) as ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE , case when ACCNT_DOC_TYPE='BILLPASSING' OR ACCNT_DOC_TYPE='CRDENTRY' OR ACCNT_DOC_TYPE='RECIEPTENTRY' OR ACCNT_DOC_TYPE='ADJRECIEPTENTRY' THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY,case when ACCNT_DOC_TYPE='TAXINV' OR ACCNT_DOC_TYPE='DEBITENTRY' OR ACCNT_DOC_TYPE='PAYMENTENTRY' OR ACCNT_DOC_TYPE='ADJPAYMENTENTRY'  THEN ACCNT_DOC_QTY ELSE 0 END AS ACCNT_DOC_QTY1,case ACCNT_DOC_TYPE when 'ADJPAYMENTENTRY' then ACCNT_DOC_QTY  else 0  end  AS  Total ,case ACCNT_DOC_TYPE when 'ADJRECIEPTENTRY' then ACCNT_DOC_QTY  else 0  end  AS  TotalRecAdj,P_TYPE into #Temp from ACCOUNTS_LEDGER inner join PARTY_MASTER ON ACCNT_P_CODE=P_CODE WHERE " + StrCond + " PARTY_MASTER.ES_DELETE=0 declare @temp table(id int identity(1,1),fpkey int,Ttype varchar(50),pkey int) insert into @temp select ACCNT_I_CODE,ACCNT_DOC_TYPE,ACCNT_CODE from #Temp declare @cnt int=0,@i int=1,@typ varchar(500),@mkey int,@dinvoice int,@pkey int,@rcpinvoiceno varchar(max),@invoiceno varchar(max),@amt varchar(max),@chequeno varchar(500),@rcpchequeno varchar(500),@paymdtype int select @cnt=count(*) from @temp while(@cnt>=@i) begin select @typ=Ttype,@mkey=fpkey,@pkey=pkey from @temp where id=@i if(@typ = 'PAYMENTENTRY') begin  declare @count int=0,@j int=1,@paymdinvoicecode int,@pmdtype int,@pmdamt varchar(max), @string varchar(max)='', @delimeter varchar(max)=' - ', @delimeter1 varchar(max)=' / ',@PAYMADJAMT VARCHAR(MAX),@PAYMADJREMARK VARCHAR(MAX),@STRINGADJ VARCHAR(MAX)='' select @invoiceno=PAYM_NO,@chequeno=PAYM_CHEQUE_NO from PAYMENT_MASTER where PAYM_CODE=@mkey  select idd = Row_number() OVER (ORDER BY PAYMD_INVOICE_CODE),PAYMD_INVOICE_CODE,PAYMD_TYPE,PAYMD_AMOUNT,PAYMD_ADJ_AMOUNT,PAYMD_REMARK into #paymentdetails  from PAYMENT_DETAIL where PAYMD_PAYM_CODE=@mkey  select @count=count(*) from #paymentdetails  while(@count>=@j) begin select @paymdinvoicecode=PAYMD_INVOICE_CODE,@paymdtype=PAYMD_TYPE,@pmdamt=PAYMD_AMOUNT,@PAYMADJAMT=PAYMD_ADJ_AMOUNT,@PAYMADJREMARK=PAYMD_REMARK from #paymentdetails where idd=@j if(@paymdtype =1) begin select @string= @string + cast(BPM_INV_NO as varchar(5000)) from BILL_PASSING_MASTER where BPM_CODE=@paymdinvoicecode IF(@PAYMADJAMT != '0') BEGIN select @STRINGADJ= @STRINGADJ + cast(BPM_INV_NO as varchar(5000)) from BILL_PASSING_MASTER where BPM_CODE=@paymdinvoicecode set @STRINGADJ=@PAYMADJREMARK+'  '+ @STRINGADJ update #Temp set ACCNT_DOC_NO=@STRINGADJ where ACCNT_I_CODE=@paymdinvoicecode and ACCNT_DOC_TYPE='ADJPAYMENTENTRY' AND ACCNT_DOC_QTY1=@PAYMADJAMT	END set @string=@string + @delimeter +@pmdamt +@delimeter1	 end   if(@paymdtype =0)begin select @string= @string + cast(DNM_SERIAL_NO as varchar(5000))+'- DN' from DEBIT_NOTE_MASTER where DNM_CODE=@paymdinvoicecode set @string=@string + @delimeter +@pmdamt +@delimeter1 end if(@paymdtype =2)begin select @string= @string + cast(CNM_SERIAL_NO as varchar(5000))+'- CN' from CREDIT_NOTE_MASTER where CNM_CODE=@paymdinvoicecode set @string=@string + @delimeter +@pmdamt +@delimeter1 end  if(@paymdtype =7) begin select @string= @string + cast(REF_NAME as varchar(5000)) from NEWREFERENCE_MASTER where REF_CODE=@paymdinvoicecode set @string=@string + @delimeter +@pmdamt +@delimeter1 end set @STRINGADJ='' set @j=@j+1 end update #Temp set ACCNT_DOC_NO=@chequeno+' '+ @string where ACCNT_I_CODE=@mkey and ACCNT_CODE=@pkey drop table #paymentdetails End if(@typ = 'RECIEPTENTRY')  begin  declare @countrec int=0,@k int=1,@rcpdinvoicecode int,@rcdtype int,@rcdamt varchar(max), @recstring varchar(max)='', @recdelimeter varchar(max)=' - ', @recdelimeter1 varchar(max)=' / ',@RCPADJAMT VARCHAR(MAX),@RCPDADJREMARK VARCHAR(MAX),@RCPSTRINGADJ VARCHAR(MAX)=''  select @invoiceno=RCP_NO,@rcpchequeno=RCP_CHEQUE_NO from RECIEPT_MASTER where RCP_CODE=@mkey  select ridd = Row_number() OVER (ORDER BY RCPD_INVOICE_CODE),RCPD_INVOICE_CODE,RCPD_TYPE,RCPD_AMOUNT,RCPD_ADJ_AMOUNT,RCPD_REMARK into #rcpttdetails  from RECIEPT_DETAIL where RCPD_RCP_CODE=@mkey select @countrec=count (*) from #rcpttdetails while(@countrec>=@k) begin select @rcpdinvoicecode=RCPD_INVOICE_CODE,@rcdtype=RCPD_TYPE,@rcdamt=RCPD_AMOUNT,@RCPADJAMT=RCPD_ADJ_AMOUNT,@RCPDADJREMARK=RCPD_REMARK from #rcpttdetails where ridd=@k if(@rcdtype =1) begin select @recstring= @recstring + cast(INM_NO as varchar(5000)) from INVOICE_MASTER where INM_CODE=@rcpdinvoicecode  IF(@RCPADJAMT != '0') BEGIN select @RCPSTRINGADJ= @RCPSTRINGADJ + cast(INM_NO as varchar(5000)) from INVOICE_MASTER where INM_CODE=@rcpdinvoicecode   set @RCPSTRINGADJ=@RCPDADJREMARK+'  '+ @RCPSTRINGADJ update #Temp set ACCNT_DOC_NO=@RCPSTRINGADJ where ACCNT_I_CODE=@rcpdinvoicecode and ACCNT_DOC_TYPE='ADJRECIEPTENTRY' AND ACCNT_DOC_QTY=@RCPADJAMT	END set @recstring=@recstring + @recdelimeter +@rcdamt +@recdelimeter1	end if(@rcdtype =2)begin select @recstring= @recstring + cast(DNM_SERIAL_NO as varchar(5000))+'- DN' from DEBIT_NOTE_MASTER where DNM_CODE=@rcpdinvoicecode Set @recstring=@recstring + @delimeter +@rcdamt +@delimeter1 end if(@rcdtype =0)begin select @recstring= @recstring + cast(CNM_SERIAL_NO as varchar(5000))+'- CN' from CREDIT_NOTE_MASTER where CNM_CODE=@rcpdinvoicecode set @recstring=@recstring + @delimeter +@rcdamt +@delimeter1 end  if(@rcdtype =7) begin select @recstring= @recstring + cast(REF_NAME as varchar(5000)) from NEWREFERENCE_MASTER where REF_CODE=@rcpdinvoicecode select @RCPSTRINGADJ= @RCPSTRINGADJ + cast(REF_NAME as varchar(5000)) from NEWREFERENCE_MASTER where REF_CODE=@rcpdinvoicecode update #Temp set ACCNT_DOC_NO=@RCPSTRINGADJ where ACCNT_I_CODE=@rcpdinvoicecode and ACCNT_DOC_TYPE='ADJRECIEPTENTRY' AND ACCNT_DOC_QTY=@RCPADJAMT set @recstring=@recstring + @recdelimeter +@rcdamt +@recdelimeter1 end set @RCPSTRINGADJ='' set @k=@k+1 end update #Temp set ACCNT_DOC_NO=@rcpchequeno+' '+ @recstring where ACCNT_I_CODE=@mkey and ACCNT_CODE=@pkey drop table #rcpttdetails End IF(@typ='BILLPASSING') BEGIN DECLARE @BILLPASSING VARCHAR(MAX)='' SELECT @BILLPASSING='Bill No. ' + BPM_INV_NO from BILL_PASSING_MASTER where BPM_CODE=@mkey update #Temp set ACCNT_DOC_NO=@BILLPASSING where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='BILLPASSING' END IF(@typ='TAXINV') BEGIN DECLARE @TAXINV VARCHAR(MAX)='' ,@ACCNT_DOC_CODE int select @ACCNT_DOC_CODE=ACCNT_DOC_NO from ACCOUNTS_LEDGER where ACCNT_CODE=@pkey SELECT @TAXINV='Bill No. ' + cast(INM_NO as varchar(max)) from INVOICE_MASTER where INM_CODE=@ACCNT_DOC_CODE update #Temp set ACCNT_DOC_NO=@TAXINV where ACCNT_CODE=@pkey and ACCNT_DOC_TYPE='TAXINV' END  IF(@typ='DEBITENTRY')  BEGIN DECLARE @DEBITTNOTE VARCHAR(MAX)='',@DEBITTNOTEPK INT SELECT @DEBITTNOTEPK=ACCNT_DOC_NO FROM ACCOUNTS_LEDGER WHERE ACCNT_CODE=@pkey SELECT @DEBITTNOTE='DN.No. ' + DNM_SERIAL_NO +' ' + DNM_REMARKS from DEBIT_NOTE_MASTER where DNM_CODE=@DEBITTNOTEPK update #Temp set ACCNT_DOC_NO=@DEBITTNOTE where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='DEBITENTRY' END IF(@typ='CRDENTRY') BEGIN DECLARE @CREDITTNOTE VARCHAR(MAX)='',@CREDITNOTEPK INT,@CREDTAMT VARCHAR(MAX)='' SELECT @CREDITNOTEPK=ACCNT_DOC_NO,@CREDTAMT=ACCNT_DOC_QTY FROM ACCOUNTS_LEDGER WHERE ACCNT_CODE=@pkey SELECT @CREDITTNOTE='CN.No. ' + CNM_SERIAL_NO +' ' + DNM_REMARKS from credit_NOTE_MASTER where CNM_CODE=@CREDITNOTEPK update #Temp set ACCNT_DOC_NO=@CREDITTNOTE where ACCNT_I_CODE=@mkey and ACCNT_DOC_TYPE='CRDENTRY' AND ACCNT_DOC_QTY=@CREDTAMT END  set @i=@i+1  end select P_CODE,P_NAME,case WHEN ACCNT_DOC_NO LIKE '%/'  THEN LEFT(ACCNT_DOC_NO, LEN(ACCNT_DOC_NO)-1)    ELSE ACCNT_DOC_NO end as ACCNT_DOC_NO,ACCNT_DOC_NUMBER,ACCNT_DOC_DATE,ACCNT_DOC_TYPE,ACCNT_I_CODE,ABS(ACCNT_DOC_QTY) as ACCNT_DOC_QTY,ACCNT_DOC_QTY1, Total,ABS(TotalRecAdj) as TotalRecAdj,P_TYPE from #Temp order by ACCNT_DOC_DATE Drop table #Temp";
