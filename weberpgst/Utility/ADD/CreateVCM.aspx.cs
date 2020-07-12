using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Utility_ADD_CreateVCM : System.Web.UI.Page
{

    DataTable dtFilter = new DataTable();
    static DataTable DtInvDet = new DataTable();
    DataRow dr;
    static int mlCode = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCombos();
            LoadInvoices();
            LoadFilter();
        }
    }

    private void LoadInvoices()
    {
        try
        {
            DataTable FromInv = new DataTable();
            FromInv = CommonClasses.Execute("SELECT DISTINCT INM_CODE,INM_NO FROM INVOICE_MASTER WHERE   ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='TAXINV' ORDER BY INM_NO");
            ddlFromInvNo.DataSource = FromInv;
            ddlFromInvNo.DataTextField = "INM_NO";
            ddlFromInvNo.DataValueField = "INM_CODE";
            ddlFromInvNo.DataBind();
            ddlFromInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));

            ddlToInvNo.DataSource = FromInv;
            ddlToInvNo.DataTextField = "INM_NO";
            ddlToInvNo.DataValueField = "INM_CODE";
            ddlToInvNo.DataBind();
            ddlToInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));




        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Create VCM", "LoadInvoices", Ex.Message.ToString());
        }
    }


    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomer.DataSource = custdet;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally transfer Sales", "LoadCombos", Ex.Message);
        }

    }
    #endregion


    #region btnLoad_Click
    protected void btnLoad_Click(object sender, EventArgs e)
    {

        try
        {

            if (ddlInvType.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice Type";
                PanelMsg.Visible = true;
            }
            if (ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice From & To";
                PanelMsg.Visible = true;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Customer";
                PanelMsg.Visible = true;
            }

            string Query = "SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE FROM INVOICE_MASTER WHERE INM_NO BETWEEN '" + ddlFromInvNo.SelectedItem.ToString() + "' AND '" + ddlToInvNo.SelectedItem.ToString() + "' AND INM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "'";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count == 0)
            {
                LoadFilter();
            }
            else
            {
                dgInvDetails.DataSource = dt;

                dgInvDetails.DataBind();
                //DtInvDet = dt;
                dgInvDetails.Enabled = true;
            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion



    protected void btnExport_Click(object sender, EventArgs e)
    {


        DirectoryInfo DI = new DirectoryInfo(Server.MapPath(@"~/UpLoadPath/"));
        FileInfo[] Delfiles = DI.GetFiles("*.csv");
        int d = 0;
        foreach (FileInfo fi in Delfiles)
        {
            System.IO.File.Delete(DI + "/" + Delfiles[d]);
            d++;
        }


        string InvCodes = "";
        try
        {
            if (DtInvDet.Columns.Count == 0)
            {
                DtInvDet.Columns.Add("INM_CODE");
                DtInvDet.Columns.Add("INM_NO");
                DtInvDet.Columns.Add("INM_DATE");
                //DtInvDet.Columns.Add("P_CODE");
                //DtInvDet.Columns.Add("P_NAME");
                //DtInvDet.Columns.Add("INM_NET_AMT");
                //DtInvDet.Columns.Add("INM_IS_TALLY_TRANS");
            }

            for (int j = 0; j < dgInvDetails.Rows.Count; j++)
            {
                CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[j].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked)
                {
                    dr = DtInvDet.NewRow();
                    dr["INM_CODE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text;
                    dr["INM_NO"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_NO"))).Text;
                    dr["INM_DATE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_DATE"))).Text;
                    //dr["P_CODE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblP_CODE"))).Text;
                    //dr["P_NAME"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblP_NAME"))).Text;
                    //dr["INM_NET_AMT"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblINM_NET_AMT"))).Text;
                    //dr["INM_IS_TALLY_TRANS"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblINM_IS_TALLY_TRANS"))).Text;
                    DtInvDet.Rows.Add(dr);
                }

                if (j == dgInvDetails.Rows.Count - 1)
                {
                    InvCodes = InvCodes + ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text;
                }
                else
                {
                    InvCodes = InvCodes + ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text + ",";
                }
            }

            if (ddlInvType.SelectedIndex == 1)
            {
                DataTable dtExport = CommonClasses.Execute("SELECT CPOM_PONO as PoNumber,'10' as [Item No],IND_INQTY as [Quantity],INM_TNO as [Vendor Challan No],INM_DATE as [Challan Date],INM_ACCESSIBLE_AMT as [Assesable Value],INM_BE_AMT as [BED Value],'0' as [SED Value],'0' as [AED Value],'0' as [ECS value],'0' as [S&H E cess Value],'0' as [S&H E cess Rate %],'0' as [ECS Rate %],INM_BEXCISE as [BED %],'0' as [AED %],'0' as [SED %],'0' as [Sales Tax],INM_G_AMT as [Invoice amount],INM_S_TAX_AMT as [VAT amount],'INR' as [Currency],'' as [Form 31-Series No],'' as [Form 31 No],'' as [Invoice Number],'' as [Invoice Date],INM_VEH_NO as [Vehicle Number],'' as Packages,'' as PackUnit FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,ITEM_MASTER WHERE INM_CODE=IND_INM_CODE AND IND_CPOM_CODE=CPOM_CODE AND IND_I_CODE=I_CODE  AND INM_CODE in (" + InvCodes + ")  ORDER BY INM_TNO ");
                if (dtExport.Rows.Count > 0)
                {
                    //for (int i = 0; i < dtExport.Columns.Count; i++)
                    //{
                    using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                    {
                        writer.WriteLine(dtExport.Columns[0].ColumnName.ToString()
                            + ',' + dtExport.Columns[1].ColumnName.ToString()
                            + ',' + dtExport.Columns[2].ColumnName.ToString()
                            + ',' + dtExport.Columns[3].ColumnName.ToString()
                            + ',' + dtExport.Columns[4].ColumnName.ToString()
                            + ',' + dtExport.Columns[5].ColumnName.ToString()
                            + ',' + dtExport.Columns[6].ColumnName.ToString()
                            + ',' + dtExport.Columns[7].ColumnName.ToString()
                            + ',' + dtExport.Columns[8].ColumnName.ToString()
                            + ',' + dtExport.Columns[9].ColumnName.ToString()
                            + ',' + dtExport.Columns[10].ColumnName.ToString()
                            + ',' + dtExport.Columns[11].ColumnName.ToString()
                            + ',' + dtExport.Columns[12].ColumnName.ToString()
                            + ',' + dtExport.Columns[13].ColumnName.ToString()
                            + ',' + dtExport.Columns[14].ColumnName.ToString()
                            + ',' + dtExport.Columns[15].ColumnName.ToString()
                            + ',' + dtExport.Columns[16].ColumnName.ToString()
                            + ',' + dtExport.Columns[17].ColumnName.ToString()
                            + ',' + dtExport.Columns[18].ColumnName.ToString()
                            + ',' + dtExport.Columns[19].ColumnName.ToString()
                            + ',' + dtExport.Columns[20].ColumnName.ToString()
                            + ',' + dtExport.Columns[21].ColumnName.ToString()
                            + ',' + dtExport.Columns[22].ColumnName.ToString()
                            + ',' + dtExport.Columns[23].ColumnName.ToString()
                            + ',' + dtExport.Columns[24].ColumnName.ToString()
                            + ',' + dtExport.Columns[25].ColumnName.ToString()
                            + ',' + dtExport.Columns[26].ColumnName.ToString()
                            + ',');
                    }
                    //}
                    for (int i = 0; i < dtExport.Rows.Count; i++)
                    {
                        using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                        {
                            writer.WriteLine(dtExport.Rows[i]["PoNumber"].ToString()
                                + ',' + dtExport.Rows[i]["Item No"].ToString()
                                + ',' + dtExport.Rows[i]["Quantity"].ToString()
                                + ',' + dtExport.Rows[i]["Vendor Challan No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["Challan Date"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Assesable Value"].ToString()
                                + ',' + dtExport.Rows[i]["BED Value"].ToString()
                                + ',' + dtExport.Rows[i]["SED Value"].ToString()
                                + ',' + dtExport.Rows[i]["AED Value"].ToString()
                                + ',' + dtExport.Rows[i]["ECS value"].ToString()
                                + ',' + dtExport.Rows[i]["S&H E cess Value"].ToString()
                                + ',' + dtExport.Rows[i]["S&H E cess Rate %"].ToString()
                                + ',' + dtExport.Rows[i]["ECS Rate %"].ToString()
                                + ',' + dtExport.Rows[i]["BED %"].ToString()
                                + ',' + dtExport.Rows[i]["AED %"].ToString()
                                + ',' + dtExport.Rows[i]["SED %"].ToString()
                                + ',' + dtExport.Rows[i]["Sales Tax"].ToString()
                                + ',' + dtExport.Rows[i]["Invoice amount"].ToString()
                                + ',' + dtExport.Rows[i]["VAT amount"].ToString()
                                + ',' + dtExport.Rows[i]["Currency"].ToString()
                                + ',' + dtExport.Rows[i]["Form 31-Series No"].ToString()
                                + ',' + dtExport.Rows[i]["Form 31 No"].ToString()
                                + ',' + dtExport.Rows[i]["Invoice Number"].ToString()
                                + ',' + dtExport.Rows[i]["Invoice Date"].ToString()
                                + ',' + dtExport.Rows[i]["Vehicle Number"].ToString()
                                + ',' + dtExport.Rows[i]["Packages"].ToString()
                                + ',' + dtExport.Rows[i]["PackUnit"].ToString()
                                + ',');
                        }
                    }

                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(Server.MapPath(@"~/UpLoadPath/ASN.csv"), System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "ASN.csv");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();

                }
            }
            else if (ddlInvType.SelectedIndex == 2)
            {
                DataTable dtExport = CommonClasses.Execute("SELECT CPOM_PONO as [PO/SA Number],IND_SR_NO as [Item Sr No],I_CODENO as [Part No],IND_INQTY as [ASN Quantity],INM_TNO as [Invoice No],INM_DATE as [Invoice Date (dd.mm.yyyy)],INM_G_AMT as [Invoice Amount],INM_BE_AMT as [Excise Amount],'NA' as [LR No],INM_LR_DATE as [LR Date (dd.mm.yyyy)],INM_VEH_NO as [Veh No],IND_RATE as [Material Base Price] FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,ITEM_MASTER WHERE INM_CODE=IND_INM_CODE AND IND_CPOM_CODE=CPOM_CODE AND IND_I_CODE=I_CODE  AND INM_CODE in (" + InvCodes + ")    ORDER BY INM_TNO");
                if (dtExport.Rows.Count > 0)
                {
                    //for (int i = 0; i < dtExport.Columns.Count; i++)
                    //{
                    using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                    {
                        writer.WriteLine(dtExport.Columns[0].ColumnName.ToString()
                            + ',' + dtExport.Columns[1].ColumnName.ToString()
                            + ',' + dtExport.Columns[2].ColumnName.ToString()
                            + ',' + dtExport.Columns[3].ColumnName.ToString()
                            + ',' + dtExport.Columns[4].ColumnName.ToString()
                            + ',' + dtExport.Columns[5].ColumnName.ToString()
                            + ',' + dtExport.Columns[6].ColumnName.ToString()
                            + ',' + dtExport.Columns[7].ColumnName.ToString()
                            + ',' + dtExport.Columns[8].ColumnName.ToString()
                            + ',' + dtExport.Columns[9].ColumnName.ToString()
                            + ',' + dtExport.Columns[10].ColumnName.ToString()
                            + ',' + dtExport.Columns[11].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[12].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[13].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[14].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[15].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[16].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[17].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[18].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[19].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[20].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[21].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[22].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[23].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[24].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[25].ColumnName.ToString()
                            //+ ',' + dtExport.Columns[26].ColumnName.ToString()
                            + ',');
                    }
                    //}
                    for (int i = 0; i < dtExport.Rows.Count; i++)
                    {
                        using (var writer = new StreamWriter(Server.MapPath(@"~/UpLoadPath/ASN.csv"), true))
                        {
                            writer.WriteLine(dtExport.Rows[i]["PO/SA Number"].ToString()
                                + ',' + "10"
                                + ',' + dtExport.Rows[i]["Part No"].ToString()
                                + ',' + dtExport.Rows[i]["ASN Quantity"].ToString()
                                + ',' + dtExport.Rows[i]["Invoice No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["Invoice Date (dd.mm.yyyy)"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Invoice Amount"].ToString()
                                + ',' + dtExport.Rows[i]["Excise Amount"].ToString()
                                + ',' + dtExport.Rows[i]["LR No"].ToString()
                                + ',' + Convert.ToDateTime(dtExport.Rows[i]["LR Date (dd.mm.yyyy)"].ToString()).ToString("dd.MM.yyyy")
                                + ',' + dtExport.Rows[i]["Veh No"].ToString()
                                + ',' + dtExport.Rows[i]["Material Base Price"].ToString()

                                + ',');
                        }
                    }

                    System.IO.FileStream fs = null;
                    fs = System.IO.File.Open(Server.MapPath(@"~/UpLoadPath/ASN.csv"), System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "ASN.csv");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                    Response.End();

                }
            }

        }
        catch (Exception)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
    }

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("INM_NET_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("INM_IS_TALLY_TRANS", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvDetails.DataSource = dtFilter;
                dgInvDetails.DataBind();

                dgInvDetails.Enabled = false;
            }
        }
        else
        {
            dgInvDetails.Enabled = true;
        }
    }
    #endregion

}
