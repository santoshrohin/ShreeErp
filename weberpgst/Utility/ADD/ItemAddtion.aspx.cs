using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;

public partial class Utility_ADD_ItemAddtion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\";";
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

            string[] SheetName1 = GetExcelSheetNames(filename);
            string SheetName = SheetName1[0].ToString();// "ContractualMonthAttend-3";

            string Con = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                        @"Data Source=" + filename + ";" +
                        @"Extended Properties=Excel 12.0"; //+Convert.ToChar(34).ToString() +
            //@"Excel 8.0;" + "Imex=2;" + "HDR=Yes;" + Convert.ToChar(34).ToString();
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

            if (dt.Columns[1].ColumnName != "Item Category")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 2nd(B) Column Name Name Sould be 'Item Category' instead of " + dt.Columns[1].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            if (dt.Columns[2].ColumnName != "Item Code")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 3rd(C) Column Name Name Sould be 'Item Code' instead of " + dt.Columns[2].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            if (dt.Columns[3].ColumnName != "Item Name")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 4th(D) Column Name Name Sould be 'Item Name' instead of " + dt.Columns[3].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[4].ColumnName != "Tariff Code")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 5th(E) Column Name Name Sould be 'Tariff Code' instead of " + dt.Columns[4].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[5].ColumnName != "Costing Head")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 6th(F) Column Name Name Sould be 'Costing Head' instead of " + dt.Columns[5].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[6].ColumnName != "Ac Head Purchase")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 7th(G) Column Name Name Sould be 'Ac Head Purchase' instead of " + dt.Columns[6].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }


            if (dt.Columns[7].ColumnName != "Ac Head Sale")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 8th(H) Column Name Name Sould be 'Ac Head Sale' instead of " + dt.Columns[7].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[8].ColumnName != "UOM")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 9th(I) Column Name Name Sould be 'Ac Head Purchase' instead of " + dt.Columns[8].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[9].ColumnName != "Weight OUM")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 10th(J) Column Name Name Sould be 'Weight OUM' instead of " + dt.Columns[9].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dt.Columns[10].ColumnName != "IS DEVELOPMENT (Y/N)")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The 11th(K) Column Name Name Sould be 'IS DEVELOPMENT (Y/N)' instead of " + dt.Columns[10].ColumnName + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }




            if (dt.Rows.Count > 0)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    DataTable dtIcodeCheck = CommonClasses.Execute("select * from ITEM_MASTER where ES_DELETE=0 and I_CODENO='" + dt.Rows[k]["Item Code"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtIcodeCheck.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Duplicate Item Code found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }


                    DataTable dtInameCheck = CommonClasses.Execute("select * from ITEM_MASTER where ES_DELETE=0 and I_NAME='" + dt.Rows[k]["Item Name"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtInameCheck.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Duplicate Item Name found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    DataTable dtCat = CommonClasses.Execute("SELECT * FROM ITEM_CATEGORY_MASTER WHERE I_CAT_NAME ='" + dt.Rows[k]["Item Category"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtCat.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Item Category Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    DataTable TariffCode = CommonClasses.Execute("select * FROM EXCISE_TARIFF_MASTER WHERE E_TARIFF_NO='" + dt.Rows[k]["Tariff Code"].ToString().TrimEnd().TrimStart() + "'");
                    if (TariffCode.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Tariff Code Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    DataTable dtAcPur = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE TALLY_NAME ='" + dt.Rows[k]["Ac Head Purchase"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtAcPur.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Purchase Ac Head Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    DataTable dtAcSale = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE TALLY_NAME ='" + dt.Rows[k]["Ac Head Sale"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtAcSale.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Sale Ac Head Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    DataTable dtUOM = CommonClasses.Execute("SELECT * FROM ITEM_UNIT_MASTER WHERE I_UOM_NAME='" + dt.Rows[k]["UOM"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtUOM.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Item UOM Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    DataTable dtWUOM = CommonClasses.Execute("SELECT * FROM ITEM_UNIT_MASTER WHERE I_UOM_NAME='" + dt.Rows[k]["Weight OUM"].ToString().TrimEnd().TrimStart() + "'");
                    if (dtWUOM.Rows.Count == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Item weight UOM Not found at Sr No " + (k + 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "No rows Found";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            string I_Cat = "";
            string I_code = "";
            string I_Name = "";
            string I_TARIFF_CODE = "";
            string COST_HEAD = "";
            string AC_HED_PER = "";
            string AC_HED_SALE = "";
            string UOM = "";
            string W_UOM = "";
            string ISDEV = "";
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataTable dtCat = CommonClasses.Execute("SELECT * FROM ITEM_CATEGORY_MASTER WHERE I_CAT_NAME ='" + dt.Rows[j]["Item Category"].ToString().TrimEnd().TrimStart() + "'");
                I_Cat = dtCat.Rows[0]["I_CAT_CODE"].ToString();

                I_code = dt.Rows[j]["Item Code"].ToString().TrimEnd().TrimStart();
                I_Name = dt.Rows[j]["Item Name"].ToString().TrimEnd().TrimStart();

                DataTable TariffCode = CommonClasses.Execute("select * FROM EXCISE_TARIFF_MASTER WHERE E_TARIFF_NO='" + dt.Rows[j]["Tariff Code"].ToString().TrimEnd().TrimStart() + "'");

                I_TARIFF_CODE = TariffCode.Rows[0]["E_CODE"].ToString();
                COST_HEAD = dt.Rows[j]["Costing Head"].ToString().TrimEnd().TrimStart();

                DataTable dtAcPur = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE TALLY_NAME ='" + dt.Rows[j]["Ac Head Purchase"].ToString().TrimEnd().TrimStart() + "'");
                AC_HED_PER = dtAcPur.Rows[0]["TALLY_CODE"].ToString();

                DataTable dtAcSale = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE TALLY_NAME ='" + dt.Rows[j]["Ac Head Sale"].ToString().TrimEnd().TrimStart() + "'");
                AC_HED_SALE = dtAcSale.Rows[0]["TALLY_CODE"].ToString();

                DataTable dtUOM = CommonClasses.Execute("SELECT * FROM ITEM_UNIT_MASTER WHERE I_UOM_NAME='" + dt.Rows[j]["UOM"].ToString().TrimEnd().TrimStart() + "'");
                UOM = dtUOM.Rows[0]["I_UOM_CODE"].ToString();

                DataTable dtWUOM = CommonClasses.Execute("SELECT * FROM ITEM_UNIT_MASTER WHERE I_UOM_NAME='" + dt.Rows[j]["Weight OUM"].ToString().TrimEnd().TrimStart() + "'");
                W_UOM = dtWUOM.Rows[0]["I_UOM_CODE"].ToString();

                if (dt.Rows[j]["IS DEVELOPMENT (Y/N)"].ToString().TrimEnd().TrimStart().ToUpper() == "Y")
                {
                    ISDEV = "True";
                }
                else
                {
                    ISDEV = "False";
                }
                CommonClasses.Execute("INSERT INTO ITEM_MASTER (I_CM_COMP_ID,I_CAT_CODE,I_SCAT_CODE,I_CODENO,I_DRAW_NO,I_NAME,I_E_CODE,I_ACCOUNT_SALES,I_ACCOUNT_PURCHASE,I_UOM_CODE,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL,I_OP_BAL,I_OP_BAL_RATE,I_STORE_LOC,I_INV_RATE,I_RECEIPT_DATE,I_ISSUE_DATE,I_CURRENT_BAL,I_ACTIVE_IND,I_UWEIGHT,I_COSTING_HEAD,I_INV_CAT,I_OPEN_RATE,I_DENSITY,I_PIGMENT,I_SOLIDS,I_VOLATILE,I_WEIGHT_UOM,I_DEVELOMENT,I_TARGET_WEIGHT) VALUES ('" + (string)Session["CompanyId"] + "','" + I_Cat + "','0','" + I_code + "','" + I_code + "','" + I_Name + "','" + I_TARIFF_CODE + "','" + AC_HED_SALE + "','" + AC_HED_PER + "','" + UOM + "','0','0','0','0','0','','0','31/MAR/2017','31/MAR/2017','0','TRUE','0','" + COST_HEAD + "','1','0','0','0','0','0','" + W_UOM + "','" + ISDEV + "','0')");
            }
            PanelMsg.Visible = true;
            lblmsg.Text = "Item Master Added Successfully...";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }

        #region Import_CSV
        //if (FileUpload1.HasFile)
            // {
        //    string csvPath = Server.MapPath("~/UpLoadPath/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
        //    FileUpload1.SaveAs(csvPath);
        //    DataTable dt = new DataTable();
        //    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
        //    new DataColumn("Name", typeof(string)),
        //    new DataColumn("Country",typeof(string)) });


        //    string csvData = File.ReadAllText(csvPath);
        //    foreach (string row in csvData.Split('\n'))
        //    {
        //        if (!string.IsNullOrEmpty(row))
        //        {
        //            dt.Rows.Add();
        //            int i = 0;
        //            foreach (string cell in row.Split(','))
        //            {
        //                dt.Rows[dt.Rows.Count - 1][i] = cell;
        //                i++;
        //            }
        //        }
        //    }
        //}
        #endregion Import CSV Comment
    }
}
