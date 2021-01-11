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
using System.IO;
using System.Data.OleDb;




public partial class Utility_ADD_SuppliementoryInvoice : System.Web.UI.Page
{
    private static readonly string[] _validExtensions = { ".xls", ".xlsx" };
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility1MV");
        //home.Attributes["class"] = "active";
        //HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility");
        //home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='159'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Add"))
            {
                loadCustomer();
            }
            else
            {
                loadCustomer();
            }
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
        }
    }

    private String[] GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            // Connection String. Change the excel file to the file you
            // will search.
            String connString = "";
            try
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }
            catch (Exception ex)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }


            // Get the data table containg the schema guid.
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }

            String[] excelSheets = new String[dt.Rows.Count];
            int i = 0;

            // Add the sheet name to the string array.
            foreach (DataRow row in dt.Rows)
            {
                excelSheets[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            // Loop through all of the sheets if you want too...
            for (int j = 0; j < excelSheets.Length; j++)
            {
                // Query each excel sheet.
            }

            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (ddlCustomerName.SelectedValue == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Customer Name ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        if (ddlFinishedComponent.SelectedValue == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Item Code/ Name ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }

        if (FileUpload1.HasFile)
        {
            PanelMsg.Visible = false;
            DirectoryInfo DI = new DirectoryInfo(Server.MapPath(@"~/UpLoadPath/ItemMaster/"));
            FileInfo[] Delfiles = DI.GetFiles("*.xlsx");
            int i = 0;
            foreach (FileInfo fi in Delfiles)
            {
                System.IO.File.Delete(DI + "/" + Delfiles[i]);
                i++;
            }
            string filename1 = Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(Server.MapPath(@"~/UpLoadPath/ItemMaster/" + filename1));
            string filename = Server.MapPath(@"~/UpLoadPath/ItemMaster/" + filename1);
            //string SheetName = "Sheet1";
            string ext = Path.GetExtension(filename1).ToLower();
            if (_validExtensions.Contains(ext.ToLower()))
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Excel File";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            string[] SheetName1 = GetExcelSheetNames(filename);
            string SheetName = SheetName1[0].ToString();// "ContractualMonthAttend-3";

            string Con = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                        @"Data Source=" + filename + ";" +
                        @"Extended Properties=Excel 12.0";
            OleDbConnection oleConn = new OleDbConnection(Con);
            oleConn.Open();
            OleDbCommand oleCmdSelect = new OleDbCommand();
            oleCmdSelect = new OleDbCommand(
                    @"SELECT * FROM ["
                    + SheetName +
                    "]", oleConn);
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            oleAdapter.SelectCommand = oleCmdSelect;
            DataTable dt = new DataTable("Table1");
            oleAdapter.FillSchema(dt, SchemaType.Source);
            oleAdapter.Fill(dt);
            oleCmdSelect.Dispose();
            oleCmdSelect = null;
            oleAdapter.Dispose();
            oleAdapter = null;
            oleConn.Dispose();
            oleConn = null;
            if (dt.Rows.Count > 0)
            {
                #region CheckPOvalidation
                DataTable dtPo = new DataTable();
                //validate for PONO ,QTY and Rate Blank Value
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k]["PONO"].ToString().Trim() == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Enter PO NO at  Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (dt.Rows[k]["INVQTY"].ToString().Trim() == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Enter Qty at  Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (dt.Rows[k]["DIFFRATE"].ToString().Trim() == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Enter Diff. Rate  at  Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "PONO");
                for (int k = 0; k < distinctValues.Rows.Count; k++)
                {
                    dtPo = CommonClasses.Execute("SELECT   *  FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOD_STATUS=0 AND CPOD_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0  AND CPOM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND CPOD_I_CODE='" + ddlFinishedComponent.SelectedValue + "' AND CPOM_PONO='" + distinctValues.Rows[k]["PONO"].ToString() + "'");

                    if (dtPo.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "PO Number is not valid " + dt.Rows[k]["PONO"].ToString();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                #endregion
                //to Compare   customer and Company state for GST calculation 
                DataTable dtCust = new DataTable();
                dtCust = CommonClasses.Execute(" SELECT * FROM PARTY_MASTER  where  P_SM_CODE= (SELECT CM_STATE  FROM COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"].ToString() + "') AND P_CODE='" + ddlCustomerName.SelectedValue + "'");

                //local variable declared for store  CGST,SGST and IGST Per for Selected Item
                double CGST = 0, SGST = 0, IGST = 0;

                //get GST per of selected Item
                DataTable DtGst = new DataTable();
                DtGst = CommonClasses.Execute("SELECT EXCISE_TARIFF_MASTER.* FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_E_CODE=E_CODE AND I_CODE='" + ddlFinishedComponent.SelectedValue + "'");
                if (DtGst.Rows.Count > 0)
                {
                    if (dtCust.Rows.Count > 0)
                    {
                        CGST = Convert.ToDouble(DtGst.Rows[0]["E_BASIC"].ToString());
                        SGST = Convert.ToDouble(DtGst.Rows[0]["E_EDU_CESS"].ToString());
                    }
                    else
                    {
                        IGST = Convert.ToDouble(DtGst.Rows[0]["E_H_EDU"].ToString());
                    }
                }
                string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
                SqlTransaction trans;

                SqlConnection connection = new SqlConnection(strConnString);
                connection.Open();
                trans = connection.BeginTransaction();

                try
                {
                    //Local varible for Store Invoice qty, Diff rate ,CSGT amount, IGST amount,SGST amount and Grand Total
                    double inv_qty = 0, diff_Rate = 0, AMT = 0, CGST_AMT = 0, SGST_AMT = 0, IGST_AMT = 0, TCS_PER = 0, TCS_AMT = 0, GRAND_TOTAL = 0;

                    if (dtCust.Rows.Count > 0)
                    {
                        TCS_PER = Convert.ToDouble(dtCust.Rows[0]["P_TCS_PER"].ToString());

                    }

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {

                        dtPo = CommonClasses.Execute("SELECT * FROM CUSTPO_MASTER where CPOM_PONO='" + dt.Rows[0]["PONO"].ToString() + "' AND CUSTPO_MASTER.ES_DELETE=0  ");

                        inv_qty = Convert.ToDouble(dt.Rows[d]["INVQTY"].ToString());
                        diff_Rate = Convert.ToDouble(dt.Rows[d]["DIFFRATE"].ToString());
                        AMT = Math.Round(inv_qty * diff_Rate, 2);
                        CGST_AMT = Math.Round((AMT * CGST) / 100, 2);
                        SGST_AMT = Math.Round((AMT * SGST) / 100, 2);
                        IGST_AMT = Math.Round((AMT * IGST) / 100, 2);

                        TCS_AMT = Math.Round(((AMT + CGST_AMT + SGST_AMT + IGST_AMT) * TCS_PER) / 100, 2);
                        GRAND_TOTAL = AMT + CGST_AMT + SGST_AMT + IGST_AMT + TCS_AMT;

                        int Po_Doc_no = 0;
                        //code for common series
                        //SqlCommand cmd1 = new SqlCommand("Select isnull(max(INM_NO),0) as IM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = '" + (string)Session["CompanyCode"] + "' and ES_DELETE=0   AND INM_TYPE='TAXINV'", connection, trans);
                        // Old Code where supp invoice series start from 1..
                        SqlCommand cmd1 = new SqlCommand("Select isnull(max(INM_NO),0) as IM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = '" + (string)Session["CompanyCode"] + "' and ES_DELETE=0  AND  INM_IS_SUPPLIMENT=1 AND INM_SUPPLEMENTORY=1 AND INM_TYPE='TAXINV'", connection, trans);
                        cmd1.Transaction = trans;
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        while (dr1.Read())
                        {
                            Po_Doc_no = Convert.ToInt32(dr1[0].ToString().Trim());
                            Po_Doc_no = Po_Doc_no + 1;
                        }
                        cmd1.Dispose();
                        dr1.Dispose();

                        String strInvoiceNo = CommonClasses.GenBillNo(Po_Doc_no);
                        strInvoiceNo = "RD" + strInvoiceNo;


                        SqlCommand command = new SqlCommand(" INSERT INTO INVOICE_MASTER ( INM_CM_CODE, INM_NO, INM_DATE, INM_INVOICE_TYPE, INM_TYPE, INM_P_CODE, INM_SUPPLEMENTORY, INM_NET_AMT, INM_S_TAX, INM_S_TAX_AMT, INM_BEXCISE, INM_BE_AMT, INM_EDUC_CESS, INM_EDUC_AMT, INM_H_EDUC_CESS, INM_H_EDUC_AMT, INM_DISC, INM_DISC_AMT, INM_PACK_AMT, INM_T_CODE, INM_T_AMT, INM_G_AMT, INM_G_AMORT_AMT, INM_TAX_TCS, INM_TAX_TCS_AMT, INM_FREIGHT, INM_VEH_NO, INM_TRANSPORT, INM_ISSUE_DATE, INM_REMOVAL_DATE, INM_STO_LOC, INM_REMARK, INM_REWORK_FLAG, INM_TALLYTNF, INM_TEMP_TALLYTNF, INM_TALLYTNF1, INM_TEMP_TALLYTNF1, INM_TRANS_AMT, INM_OTHER_AMT, INM_LR_NO, INM_LR_DATE, INM_INSURANCE, INM_ACCESSIBLE_AMT, INM_TAXABLE_AMT, INM_ROUNDING_AMT, INM_OCTRI_AMT, INM_IS_SUPPLIMENT, INM_ISSU_TIME, INM_REMOVEL_TIME, INM_TNO, INM_TRAY_CODE, INM_TRAY_QTY, INM_ELECTRREFNUM, INM_TERMSNCONDITIONS, INM_AUTHORIZEDNAME,INM_BUYER_NAME,INM_BUYTER_ADD,INM_PREPARE_BY,INM_LC_NO,INM_LC_DATE,INM_STATE ,INM_TCS_PERCENTAGE,INM_TCS_PERCENTAGE_AMOUNT)VALUES ( '" + (string)Session["CompanyCode"] + "','" + Po_Doc_no + "','" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "',0,'TAXINV','" + ddlCustomerName.SelectedValue + "',1,'" + AMT + "',0,0,'" + CGST + "','" + CGST_AMT + "','" + SGST + "','" + SGST_AMT + "','" + IGST + "','" + IGST_AMT + "',0,0,0,-2147483648,0,'" + GRAND_TOTAL + "',0,0,0,0,0,0,'" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "','" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "',0,0,0,0,0,0,0,0,0,0,'" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "',0,'" + AMT + "','" + GRAND_TOTAL + "',0,0,1 , (CONVERT(VARCHAR(5), GETDATE(), 108)  )  , (CONVERT(VARCHAR(5), GETDATE(), 108)  )  ,'" + strInvoiceNo + "',0,0,0,0,0,0,'" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "','" + Session["UserCode"].ToString() + "','" + dt.Rows[d]["INVNO"].ToString() + "','" + CommonClasses.GetCurrentTime().ToString("dd/MMM/yyyy") + "',(select P_SM_CODE from PARTY_MASTER where P_CODE='" + ddlCustomerName.SelectedValue + "'),'" + TCS_PER + "','" + TCS_AMT + "')", connection, trans);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();

                        string Code = "";
                        SqlCommand cmd0 = new SqlCommand(" Select Max(INM_CODE) from INVOICE_MASTER", connection, trans);
                        cmd0.Transaction = trans;
                        SqlDataReader dr0 = cmd0.ExecuteReader();
                        while (dr0.Read())
                        {
                            Code = dr0[0].ToString().Trim();
                        }
                        cmd0.Dispose();
                        dr0.Dispose();

                        SqlCommand command2 = new SqlCommand(" INSERT INTO INVOICE_DETAIL (IND_INM_CODE, IND_I_CODE, IND_CPOM_CODE, IND_INQTY, IND_RATE,IND_CON_QTY, IND_AMORT_RATE, IND_NO_PACK, IND_QTY_PACK, IND_EX_AMT, IND_E_CESS_AMT, IND_SH_CESS_AMT, IND_ACT_WEIGHT, IND_PAK_QTY, IND_AMT, IND_REFUNDABLE_QTY, IND_AMORTRATE, IND_AMORTAMT, IND_HSN_CODE)VALUES ('" + Code + "','" + ddlFinishedComponent.SelectedValue + "','" + dtPo.Rows[0]["CPOM_CODE"].ToString() + "','" + inv_qty + "','" + diff_Rate + "',0,0,0,0,'" + CGST_AMT + "','" + SGST_AMT + "','" + IGST_AMT + "',0,0,'" + AMT + "',0,0,0,'" + DtGst.Rows[0]["E_TARIFF_NO"].ToString() + "')", connection, trans);
                        command2.Transaction = trans;
                        command2.ExecuteNonQuery();
                    }
                    trans.Commit();
                    //CommonClasses.WriteLog("Tax Invoice", "Save", "Tax Invoice", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invoice Added Successfully...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
                catch (Exception ex) //error occurred
                {
                    trans.Rollback();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "No rows Found";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please select File";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }



    public void loadCustomer()
    {
        DataTable dtcust = new DataTable();
        dtcust = CommonClasses.Execute("SELECT distinct PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM   INVOICE_DETAIL  INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE where     INM_TYPE='TAXINV' AND  INVOICE_MASTER.ES_DELETE=0 and P_CM_COMP_ID='" + Session["CompanyId"].ToString() + "'  AND P_TYPE=1 order by P_NAME");
        ddlCustomerName.DataSource = dtcust;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));
    }

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Customer Name ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            DataTable dtItem = new DataTable();
            dtItem = CommonClasses.Execute(" SELECT DISTINCT I_CODE,I_CODENO +' - '+I_NAME AS I_CODENO  FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOD_STATUS=0 AND CPOD_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0  AND CPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'");
            ddlFinishedComponent.DataSource = dtItem;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice Upload", "ddlCustomerName_SelectedIndexChanged", ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlCustomerName.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Customer Name ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (ddlFinishedComponent.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Code/ Name ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }


            DataTable dtResult = new DataTable();
            dtResult = CommonClasses.Execute("SELECT CPOM_PONO,INM_NO,  INM_DATE,IND_I_CODE,IND_INQTY,IND_RATE FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE AND CPOD_STATUS=0  AND  CPOM_CODE=IND_CPOM_CODE AND  INM_CODE=IND_INM_CODE AND INM_TYPE='TAXINV' AND INM_IS_SUPPLIMENT=0 AND CPOD_I_CODE=IND_I_CODE  AND INM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND IND_I_CODE='" + ddlFinishedComponent.SelectedValue + "'  AND INVOICE_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0    AND INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ORDER BY INM_NO");
            DataTable dtExport = new DataTable();
            if (dtResult.Rows.Count > 0)
            {
                dtExport.Columns.Add("Sr No");
                dtExport.Columns.Add("PONO");
                dtExport.Columns.Add("INVNO");
                //dtExport.Columns.Add("Type");
                dtExport.Columns.Add("INVDATE");
                dtExport.Columns.Add("INVQTY");
                dtExport.Columns.Add("INVRATE");
                dtExport.Columns.Add("DIFFRATE");

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(i + 1, dtResult.Rows[i]["CPOM_PONO"].ToString(), dtResult.Rows[i]["INM_NO"].ToString(), Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                      dtResult.Rows[i]["IND_INQTY"].ToString(),
                          dtResult.Rows[i]["IND_RATE"].ToString(),
                        0
                                     );

                }


                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=TaxInvoiceSummary.xls");

                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                //sets font
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                //am getting my grid's column headers
                int columnscount = dtExport.Columns.Count;
                for (int j = 0; j < columnscount; j++)
                {      //write in new column
                    HttpContext.Current.Response.Write("<Td>");
                    //Get column headers  and make it as bold in excel columns
                    HttpContext.Current.Response.Write("<B>");
                    HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
                for (int k = 0; k < dtExport.Rows.Count; k++)
                {//write in new row

                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < dtExport.Columns.Count; i++)
                    {
                        if (i == dtExport.Columns.Count - 1)
                        {
                            HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                            HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                            HttpContext.Current.Response.Write("</Td>");
                        }
                        else
                        {

                            if (i == 4)
                            {

                                HttpContext.Current.Response.Write("<Td>");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                            }
                            HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                            HttpContext.Current.Response.Write("</Td>");
                        }
                    }
                    HttpContext.Current.Response.Write("</TR>");
                }
                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "No rows Found";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

        }
        catch (Exception ex)
        {
            //ss CommonClasses.SendError("Tax Invoice Upload", "btnExport_Click", ex.Message);
        }
    }
}
