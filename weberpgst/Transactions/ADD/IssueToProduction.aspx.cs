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
using System.Data.SqlClient;

public partial class Transactions_ADD_IssueToProduction : System.Web.UI.Page
{

    #region Variable

    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    DataTable dtInw = new DataTable();
    DataTable dtPODetail = new DataTable();



    static int mlCode = 0;
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    static string right = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    int ItemCode;
    int ItemName;



    #endregion

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {

                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='51'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                        LoadMaterialReq();
                        LoadICode();
                        LoadIName();
                        LoadDept();
                        //BlankGridView();
                        //ddlMaterialReq.Enabled = false;
                        ViewState["mlCode"] = mlCode;
                        ViewState["BindTable"] = BindTable;
                        ViewState["TemTaable"] = TemTaable;
                        ViewState["dtInfo"] = dtInfo;
                        ViewState["dt2"] = dt2;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        try
                        {
                            ((DataTable)ViewState["BindTable"]).Clear();

                            if (Request.QueryString[0].Equals("VIEW"))
                            {
                                ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                                ViewRec("VIEW");
                            }
                            else if (Request.QueryString[0].Equals("MODIFY"))
                            {
                                ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                                ViewRec("MOD");

                            }

                            if (Request.QueryString[0].Equals("INSERT"))
                            {
                                str = "";
                                //LoadMaterialReq();
                                //LoadICode();
                                // LoadIName();
                                ddlIssueType.Enabled = false;
                                ddlMaterialReq.Visible = false;
                                ItemCodeAll();
                                BlankGridView();
                                //txtRequiredQty.Enabled = false;
                                ddlMaterialReq.Enabled = false;
                                txtIssueDate.Attributes.Add("readonly", "readonly");
                                ((DataTable)ViewState["dt2"]).Rows.Clear();
                                ((DataTable)ViewState["dt2"]).Columns.Clear();

                                txtIssueDate.Text = CommonClasses.GetCurrentTime().Date.ToString("dd MMM yyyy");
                            }
                            txtIssueDate.Focus();
                        }
                        catch (Exception ex)
                        {
                            CommonClasses.SendError("Issue To Production", "Page_Load", ex.Message.ToString());
                        }
                  
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "Page_Load", ex.Message.ToString());
        }

    }

    #endregion

    #region LoadMaterialReq
    private void LoadMaterialReq()
    {
        try
        {
            dt = CommonClasses.Execute("select MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO from MATERIAL_REQUISITION_MASTER where ES_DELETE=0 ORDER BY MR_BATCH_NO DESC");
            ddlMaterialReq.DataSource = dt;
            ddlMaterialReq.DataTextField = "MR_BATCH_NO";
            ddlMaterialReq.DataValueField = "MR_CODE";
            ddlMaterialReq.DataBind();
            ddlMaterialReq.Items.Insert(0, new ListItem("Select Material Req.No", "0"));
            ddlMaterialReq.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue to production  ", "LoadMaterialReq", Ex.Message);
        }

    }
    #endregion


    protected void txtIssueDate_TextChanged(object sender, EventArgs e)
    {

        if (CommonClasses.ValidRights(int.Parse(right.Substring(6, 1)), this, "For Back Date"))
        {
           
        }
        else
        {
            if (Convert.ToDateTime(txtIssueDate.Text)>=DateTime.Now)
            {
                            
            }
            else
            {

                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtIssueDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return; 
            }
        }
    }
    #region ddlIssueType_SelectedIndexChanged
    protected void ddlIssueType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ItemCodeAll();

            if (ddlIssueType.SelectedIndex == 1)
            {
                ddlMaterialReq.Enabled = true;

            }
            else
            {
                ddlMaterialReq.SelectedIndex = 0;
                ddlMaterialReq.Enabled = false;
                //txtCurrStock.Text = "";
                dgIssueTo.DataSource = null;
                dgIssueTo.DataBind();
                BlankGridView();

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlIssueType_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlMaterialReq_SelectedIndexChanged

    protected void ddlMaterialReq_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadICode();
            LoadIName();
            DataTable dt = new DataTable();
            if (ddlMaterialReq.SelectedIndex != 0)
            {
                //dt2.Rows.Clear();
                //dt2.Columns.Clear();
                //dt = CommonClasses.Execute("select I_CODENO,BD_I_CODE,cast(BD_VQTY*CPOD_ORD_QTY as numeric(10,3)) as BD_VQTY,I_NAME from BOM_DETAIL,BOM_MASTER,ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where BM_CODE=BD_BM_CODE and BOM_MASTER.ES_DELETE=0 AND I_CODE=BD_I_CODE and BM_I_CODE='" + ddlItemName.SelectedValue + "' and CPOD_CPOM_CODE=CPOM_CODE and CPOM_CODE='" + ddlOrderNo.SelectedValue + "' and CPOD_I_CODE=BM_I_CODE");
                dt = CommonClasses.Execute("select cast(sum(isnull(MRD_ADD_IN,0)-isnull(MRD_ISSUE_QTY,0)) as numeric(10,3)) as IMD_REQ_QTY,I_CODENO as ItemCodeNo,I_NAME as ItemName,MRD_I_CODE as ItemCode,ITEM_UNIT_MASTER.I_UOM_CODE as UOM_CODE,I_UOM_NAME as StockUOM,cast((I_CURRENT_BAL) as numeric(10,3)) as CurrStock,0.000 as IssueQty,'' as Remark,I_MIN_LEVEL from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER where MR_CODE=MRD_MR_CODE and ITEM_MASTER.I_CODE=MRD_I_CODE and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "' group by I_CODENO,I_NAME,MRD_I_CODE,ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_CURRENT_BAL,I_MIN_LEVEL");
            }

            if (dt.Rows.Count > 0)
            {
                dgIssueTo.DataSource = dt;
                dgIssueTo.DataBind();
                ViewState["dt2"] = dt;
                dgIssueTo.Enabled = true;
            }
            else
            {
                BlankGridView();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlMaterialReq_SelectedIndexChanged", ex.Message);
        }
    }


    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            //if ( no > 15)
            //{
            //     no = 15;
            //}
            // int n = no - 1;
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {

                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {

            }


            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion

    #region ddlMaterialReq_SelectedIndexChanged

    protected void txtIssueQty_TextChanged(object sender, EventArgs e)
    {
        if (txtIssueQty.Text == "")
        {
            txtIssueQty.Text = "0.000";
        }
        if (txtRequiredQty.Text == "")
        {
            txtRequiredQty.Text = "0.000";
        }

        string totalStr = DecimalMasking(txtIssueQty.Text);
        txtIssueQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        txtIssueQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtIssueQty.Text));
        txtRequiredQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtIssueQty.Text));
        txtAmount.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtIssueQty.Text) * Convert.ToDouble(txtrate.Text), 3));
    }


    #endregion

    #region Grid Events


    #region dgIssueTo_RowCommand
    protected void dgIssueTo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgIssueTo.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (ddlIssueType.SelectedIndex == 2)
            {
                if (e.CommandName == "Delete")
                {
                    // int rowindex = row.RowIndex;
                    //dgSupplierPurchaseOrder.DeleteRow(Index);
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                    dgIssueTo.DataSource = ((DataTable)ViewState["dt2"]);
                    dgIssueTo.DataBind();
                    if (dgIssueTo.Rows.Count == 0)
                    {
                        BlankGridView();
                    }
                }
            }
            if (e.CommandName == "Modify")
            {
                //LoadSupplier();
                if (ddlIssueType.SelectedIndex == 1)
                {
                    LoadICode();
                    LoadIName();

                }
                else
                {
                    ItemCodeAll();
                }


                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                //ddlItemCode_SelectedIndexChanged(null, null);
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                // ddlItemName_SelectedIndexChanged(null, null);


                LoadUOM();
                LoadCurrStock();
                ddlUOM.SelectedValue = ((Label)(row.FindControl("lblUOM_CODE"))).Text;
                //LoadCurrStock();
                txtCurrStock.Text = ((Label)(row.FindControl("lblCurrStock"))).Text;
                //----
                txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text));
                //----
                if (ddlMaterialReq.Enabled)
                {
                    txtRequiredQty.Enabled = true;
                }
                else
                {
                    txtRequiredQty.Enabled = false;
                }

                txtRequiredQty.Text = ((Label)(row.FindControl("lblQtyRequirment"))).Text;
                //----
                txtRequiredQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtRequiredQty.Text));
                //----
                txtIssueQty.Text = ((TextBox)(row.FindControl("txtIssueQty"))).Text;
                //----
                txtIssueQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtIssueQty.Text));
                //----
                txtrate.Text = ((Label)(row.FindControl("lblIMD_RATE"))).Text;
                txtAmount.Text = ((Label)(row.FindControl("lblIMD_AMOUNT"))).Text;


                txtRemark.Text = ((TextBox)(row.FindControl("txtRemark"))).Text;

                //ddlItemCode.Enabled = false;
                //ddlItemName.Enabled = false;
                //txtRequiredQty.Enabled = false;

            }



        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgIssueTo_RowDeleting

    protected void dgIssueTo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #endregion


    #endregion

    #region btnInsert_Click

    protected void btnInsert_Click(object sender, EventArgs e)
    {


        try
        {

            if (txtIssueDate.Text == "")
            {
                ShowMessage("#Avisos", "The Field Issue Date Is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Issue Date";
                txtIssueDate.Focus();
                return;
            }
            if (txtIssueBy.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "The Field Issue By Is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Issue Date";
                txtIssueBy.Focus();
                return;
            }


            if (ddlIssueType.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Issue Type", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Issue Type";

                ddlIssueType.Focus();
                return;
            }
            if (ddlIssueType.SelectedIndex == 1)
            {
                if (ddlMaterialReq.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Material Req.No", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Please Select Material Req";

                    ddlMaterialReq.Focus();
                    return;
                }
            }

            if (ddlItemCode.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Finished Component Code", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Item Code";

                ddlItemCode.Focus();
                return;
            }

            if (ddlItemName.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Finished Component Name", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }

            if (txtRequiredQty.Text == "" || txtRequiredQty.Text == "0.000")
            {
                //ShowMessage("#Avisos", "Select Finished Component Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Required Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRequiredQty.Focus();
                return;


            }

            if (txtIssueQty.Text == "" || txtIssueQty.Text == "0.000")
            {
                //ShowMessage("#Avisos", "Select Finished Component Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "the Field Issue Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtIssueQty.Focus();
                return;


            }
            //double CurrStock =Convert.ToDouble( ddlCurrStock.SelectedItem);
            //if(Convert.ToDouble( txtIssueQty.Text)>=CurrStock)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = " Issue Qty not greter than Curr stock Qty";
            //    return;
            //}
            if (txtRequiredQty.Text == "" || txtRequiredQty.Text == "0")
            {
                txtRequiredQty.Text = "0.000";
            }

            if (txtRequiredQty.Text != "")
            {
                if (txtRequiredQty.Text != "0.000")
                {


                    if (Convert.ToDouble(txtIssueQty.Text) > Convert.ToDouble(txtRequiredQty.Text))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = " Issue qty should be less than or equal to required qty";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtIssueQty.Focus();
                        return;
                    }
                    //----
                    if (Convert.ToDouble(txtIssueQty.Text) > Convert.ToDouble(txtCurrStock.Text))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = " Issue qty should be less than or equal to current stock";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtIssueQty.Focus();
                        return;
                    }
                    //----
                }
            }
            if (txtCurrStock.Text == "")
            {
                txtCurrStock.Text = "0";
            }
            if (txtCurrStock.Text != "")
            {
                if (txtCurrStock.Text != "0")
                {
                    if (txtCurrStock.Text != "0.000")
                    {

                        if (Convert.ToDouble(txtCurrStock.Text) < Convert.ToDouble(txtIssueQty.Text))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = " Issue Qty not greter than Current Stock";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtIssueQty.Focus();
                            return;
                        }
                    }
                }
            }




            if (dgIssueTo.Rows.Count > 0)
            {
                for (int i = 0; i < dgIssueTo.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgIssueTo.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            // ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Item Already Exists ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }

                    }

                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                //----
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCodeNo"); //change Binding Value Name From ItemCode1 to ItemCodeNo
                //----
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName"); //change Binding Value Name From StockUOM1 to UOM_CODE
                //----
                ((DataTable)ViewState["dt2"]).Columns.Add("UOM_CODE");
                //----
                ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
                ((DataTable)ViewState["dt2"]).Columns.Add("CurrStock");
                ((DataTable)ViewState["dt2"]).Columns.Add("IMD_REQ_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IssueQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Remark");
                ((DataTable)ViewState["dt2"]).Columns.Add("IMD_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IMD_AMOUNT");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemCode.SelectedValue;
            //----
            dr["ItemCodeNo"] = ddlItemCode.SelectedItem;
            //----
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["UOM_CODE"] = ddlUOM.SelectedValue;
            dr["StockUOM"] = ddlUOM.SelectedItem;
            dr["IMD_REQ_QTY"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtRequiredQty.Text)), 3));
            if (txtCurrStock.Text != "")
            {
                // dr["CurrStock1"] = ddlCurrStock.SelectedValue;
                dr["CurrStock"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtCurrStock.Text)), 3));
            }
            else
            {
            }
            // dr["CurrStock"] = ddlCurrStock.SelectedItem;
            dr["IssueQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtIssueQty.Text)), 3));
            dr["Remark"] = txtRemark.Text;
            dr["IMD_RATE"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtrate.Text)), 3));
            dr["IMD_AMOUNT"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtAmount.Text)), 3));
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            dgIssueTo.Visible = true;
            dgIssueTo.DataSource = ((DataTable)ViewState["dt2"]);
            dgIssueTo.DataBind();
            dgIssueTo.Enabled = true;


            #endregion



            clearDetail();



        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "btnInsert_Click", Ex.Message);
        }



    }


    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MR_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!=-2147483646 ORDER BY I_NAME");
            //dt = CommonClasses.Execute(" select distinct(I_CODE) as I_CODE, I_CODENO  from ITEM_MASTER,MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL where MRD_I_CODE=I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  and MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "'");
            dt = CommonClasses.Execute(" select distinct(I_CODE) as I_CODE, I_CODENO  from ITEM_MASTER,MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL where MRD_I_CODE=I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  and MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "'");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadDept
    private void LoadDept()
    {
        try
        {
            dt = CommonClasses.Execute(" select *  from  Department");
            txtReqPerson.DataSource = dt;
            txtReqPerson.DataTextField = "Dept_Name";
            txtReqPerson.DataValueField = "Dept_Name";
            txtReqPerson.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute(" select distinct(I_CODE) as I_CODE, I_NAME  from ITEM_MASTER,MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL where MRD_I_CODE=I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  and MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "'");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    #region ItemCodeAll

    private void ItemCodeAll()
    {
        if (ddlIssueType.SelectedIndex != 1)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(" select distinct(I_CODE),I_CODENO  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;

            DataTable dt1 = new DataTable();
            dt1 = CommonClasses.Execute(" select distinct(I_CODE),I_NAME  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_NAME ");
            ddlItemName.DataSource = dt1;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }

    }

    #endregion

    #region LoadUOM
    private void LoadUOM()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlUOM.DataSource = dt1;
        ddlUOM.DataTextField = "I_UOM_NAME";
        ddlUOM.DataValueField = "I_UOM_CODE";

        ddlUOM.DataBind();
        ddlUOM.Items.Insert(0, new ListItem("Select Unit", "0"));
        ddlUOM.SelectedIndex = 1;
        //ddlUOM.SelectedValue = ddlItemCode.SelectedValue;


    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlUOM.SelectedIndex = 0;
            //ddlCurrStock.SelectedIndex = 0;
            txtCurrStock.Text = "";
            txtRequiredQty.Text = "";

            txtAmount.Text = "0";
            txtIssueQty.Text = "";
            txtRemark.Text = "";

            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                LoadUOM();
                LoadCurrStock();
            }
            else
            {
                ddlItemName.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemCode_SelectedIndexChanged", Ex.Message);

        }
    }

    #endregion

    #region ddlItemName_SelectedIndexChanged

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                LoadUOM();
                LoadCurrStock();
            }
            else
            {
                ddlItemCode.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region LoadCurrStock
    private void LoadCurrStock()
    {
        try
        {
            //if (ddlMaterialReq.SelectedIndex != 0)
            //{
            dt = CommonClasses.Execute("select I_CODE,ISNULL((I_CURRENT_BAL),0) AS I_CURRENT_BAL,ISNULL(I_INV_RATE,0) AS I_INV_RATE from ITEM_MASTER where  ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and I_CODE='" + ddlItemCode.SelectedValue + "' ");

            if (dt.Rows[0]["I_CURRENT_BAL"].ToString() == "")
            {
                txtCurrStock.Text = "0.000";
            }
            else
            {
                txtCurrStock.Text = dt.Rows[0]["I_CURRENT_BAL"].ToString();
                txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text));
            }
            txtrate.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["I_INV_RATE"].ToString()));
            txtIssueQty.Text = "0.000";
            txtRequiredQty.Text = "0.000";
            // }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadCurrStock", Ex.Message);
        }

    }
    #endregion

    #region BlankGridView

    private void BlankGridView()
    {
        DataTable dtfill = new DataTable();
        if (dgIssueTo.Rows.Count == 0)
        {
            if (dtfill.Columns.Count == 0)
            {

                dtfill.Columns.Add("ItemCode");
                dtfill.Columns.Add("ItemCodeNo"); //change Binding Value Name From ItemCode1 to ItemCodeNo
                dtfill.Columns.Add("ItemName");
                dtfill.Columns.Add("UOM_CODE"); //change Binding Value Name From StockUOM1 to UOM_CODE
                dtfill.Columns.Add("StockUOM");
                dtfill.Columns.Add("CurrStock");
                dtfill.Columns.Add("IMD_REQ_QTY");
                dtfill.Columns.Add("IssueQty");
                dtfill.Columns.Add("Remark");
                dtfill.Columns.Add("IMD_RATE");
                dtfill.Columns.Add("IMD_AMOUNT");

            }
            dtfill.Rows.Add(dtfill.NewRow());


            dgIssueTo.Visible = true;
            dgIssueTo.Enabled = false;
            dgIssueTo.DataSource = dtfill;
            dgIssueTo.DataBind();
        }

    }


    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtRequiredQty.Enabled = false;
            txtIssueDate.Attributes.Add("readonly", "readonly");
            LoadICode();
            ddlItemCode_SelectedIndexChanged(null, null);
            LoadIName();
            LoadUOM();
            txtIssueDate.Enabled = false;
            LoadMaterialReq();
            dtPODetail.Clear();


            //dt = CommonClasses.Execute("Select SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + mlCode + "");
            dt = CommonClasses.Execute("Select IM_CODE,IM_NO,IM_DATE,IM_TYPE,IM_MATERIAL_REQ,IM_ISSUEBY,IM_REQBY from ISSUE_MASTER where ES_DELETE=0 and IM_COMP_ID='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["IM_CODE"]);
                ddlIssueType.SelectedValue = dt.Rows[0]["IM_TYPE"].ToString();
                ddlMaterialReq.SelectedValue = dt.Rows[0]["IM_MATERIAL_REQ"].ToString();
                txtIssueNo.Text = dt.Rows[0]["IM_NO"].ToString();
                txtIssueBy.Text = dt.Rows[0]["IM_ISSUEBY"].ToString();
                txtReqPerson.Text = dt.Rows[0]["IM_REQBY"].ToString();

                txtIssueDate.Text = Convert.ToDateTime(dt.Rows[0]["IM_DATE"]).ToString("dd MMM yyyy");

                if (ddlIssueType.SelectedIndex == 1)
                {
                    //dtPODetail = CommonClasses.Execute("select SPOD_I_CODE as ItemCode,SPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, SPOD_ORDER_QTY as OrderQty,SPOD_RATE as Rate,SPOD_RATE_UOM as RateUOM1,I_UOM_NAME as RateUOM,SPOD_CONV_RATIO as ConversionRatio,SPOD_TOTAL_AMT as TotalAmount,cast(SPOD_DISC_PER as float) as DiscPerc,SPOD_DISC_AMT as DiscAmount,isnull(SPOD_EXC_Y_N,0) as ExcInclusive,SPOD_T_CODE as SalesTax1,ST_TAX_NAME as SalesTax ,SPOD_ACTIVE_IND as ActiveInd ,SPOD_SPECIFICATION as Specification FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,SALES_TAX_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SUPP_PO_DETAILS.SPOD_UOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE and SPOD_T_CODE=SALES_TAX_MASTER.ST_CODE and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOD_SPOM_CODE='" + mlCode + "'");
                    dtPODetail = CommonClasses.Execute("select IMD_I_CODE as ItemCode, I_CODENO as ItemCodeNo,I_NAME as ItemName,IMD_UOM as UOM_CODE,I_UOM_NAME as StockUOM,cast(IMD_REQ_QTY as numeric(10,3)) as IMD_REQ_QTY,cast(IMD_CURR_STOCK as numeric(10,3)) as CurrStock, cast(IMD_ISSUE_QTY as numeric(10,3)) as IssueQty,IMD_REMARK as Remark FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE ISSUE_MASTER_DETAIL.IM_CODE=ISSUE_MASTER.IM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and IMD_I_CODE=ITEM_MASTER.I_CODE and IMD_COMP_ID= '" + Convert.ToInt32(Session["CompanyCode"]) + "' and ISSUE_MASTER_DETAIL.IM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");
                }
                else
                {
                    ddlMaterialReq.Enabled = false;
                    dtPODetail = CommonClasses.Execute("select IMD_I_CODE as ItemCode, I_CODENO as ItemCodeNo,I_NAME as ItemName,IMD_UOM as UOM_CODE,I_UOM_NAME as StockUOM,cast(I_CURRENT_BAL as numeric(10,3)) as IMD_REQ_QTY,cast(IMD_CURR_STOCK as numeric(10,3)) as CurrStock, cast(IMD_ISSUE_QTY as numeric(10,3)) as IssueQty,IMD_REMARK as Remark,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE ISSUE_MASTER_DETAIL.IM_CODE=ISSUE_MASTER.IM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ISSUE_MASTER_DETAIL.IMD_UOM and IMD_I_CODE=ITEM_MASTER.I_CODE and IMD_COMP_ID= '" + Convert.ToInt32(Session["CompanyCode"]) + "' and ISSUE_MASTER_DETAIL.IM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");

                }


                if (dtPODetail.Rows.Count != 0)
                {
                    dgIssueTo.DataSource = dtPODetail;
                    dgIssueTo.DataBind();
                    ViewState["dt2"] = dtPODetail;



                }
                if (ddlIssueType.SelectedIndex == 1)
                {
                    LoadICode();
                    LoadIName();

                }
                else
                {
                    ItemCodeAll();
                }
                LoadUOM();
                ddlIssueType.Enabled = false;
                ddlMaterialReq.Enabled = false;

            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;

                txtIssueDate.Enabled = false;
                ddlIssueType.Enabled = false;
                ddlMaterialReq.Enabled = false;

                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlUOM.Enabled = false;

                txtCurrStock.Enabled = false;
                txtIssueQty.Enabled = false;

                dgIssueTo.Enabled = false;
                txtRemark.Enabled = false;
                txtRequiredQty.Enabled = false;
                txtReqPerson.Enabled = false;
                txtIssueBy.Enabled = false;

            }
            else if (str == "MOD")
            {

                CommonClasses.SetModifyLock("Issue To Production", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region btnCancel_Click

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (mlCode != 0 && mlCode != null)
        //{
        //    CommonClasses.RemoveModifyLock("ISSUE_MASTER", "MODIFY", "IM_CODE", mlCode);
        //}

        //  ((DataTable)ViewState["dt2"]).Rows.Clear();
        //Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false)
        try
        {

            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();

                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "btnCancel_Click", ex.Message.ToString());
        }

    }

    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlIssueType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtIssueDate.Text == "")
            {
                flag = false;
            }

            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec(); 
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Issue To Production", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("ISSUE_MASTER", "MODIFY", "IM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        //if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Save"))
        //{

        if (dgIssueTo.Rows.Count != 0)
        {
            if (dgIssueTo.Enabled == true)
            {



                if (txtIssueDate.Text == "")
                {
                    // ShowMessage("#Avisos", "Select Po Type", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Issue Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                    txtIssueDate.Focus();
                    return;
                }

                if (ddlIssueType.SelectedIndex == 0)
                {
                    //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Issue Type";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    ddlIssueType.Focus();
                    return;
                }
                if (ddlIssueType.SelectedIndex == 1)
                {
                    if (ddlMaterialReq.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Material Req";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        ddlMaterialReq.Focus();
                        return;
                    }
                }

                SaveRec();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Fill Table";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }


        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Fill Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

        }
        //}
        //else
        //{
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "You have no rights to Save";
        //    return;
        //}


    }

    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        string msg = "";
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {

                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(IM_NO),0) as IM_NO FROM ISSUE_MASTER WHERE IM_COMP_ID = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["IM_NO"]);
                    Po_Doc_no = Po_Doc_no + 1;
                }
                //if (CommonClasses.Execute1("INSERT INTO ISSUE_MASTER (SPOM_CM_CODE,SPOM_AMOUNT,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text + "','" + Convert.ToDateTime(txtSupplierRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "','" + txtFreightTermsg.Text + "','" + txtGuranteeWaranty.Text + "','" + txtNote.Text + "','" + txtDeliveryTo.Text + "')"))
                if (CommonClasses.Execute1("INSERT INTO ISSUE_MASTER (IM_COMP_ID,IM_NO,IM_DATE,IM_TYPE,IM_MATERIAL_REQ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Po_Doc_no + "','" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlIssueType.SelectedValue + "','" + ddlMaterialReq.SelectedValue + "','" + txtIssueBy.Text + "','" + txtReqPerson.Text + "','" + Convert.ToInt32(Session["UserCode"].ToString()) + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(IM_CODE) from ISSUE_MASTER");
                    for (int i = 0; i < dgIssueTo.Rows.Count; i++)
                    {
                        //Inserting Into Issue To Production Detail
                        double PTS_QTY = 0;
                        if (Convert.ToString(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text) != null || Convert.ToString(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text) != "")
                        {
                            PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text)), 3);
                        }

                        result = CommonClasses.Execute1("INSERT INTO ISSUE_MASTER_DETAIL(IMD_COMP_ID,IM_CODE,IMD_I_CODE,IMD_UOM,IMD_CURR_STOCK,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK,IMD_RATE,IMD_AMOUNT) values ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Code + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblUOM_CODE")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblCurrStock")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblQtyRequirment")).Text + "','" + PTS_QTY + "','" + ((TextBox)dgIssueTo.Rows[i].FindControl("txtRemark")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblIMD_RATE")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblIMD_AMOUNT")).Text + "')");

                        if (result == true)
                        {
                            //-----
                            // Inserting Into Stock Ledger                                                   

                            // Inserting Into Stock Ledger
                            if (PTS_QTY > 0.0)
                            {
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Po_Doc_no + "','ISSPROD','" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "','-" + PTS_QTY + "')");
                                }
                                // relasing Stock Form Item Master

                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + PTS_QTY + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "' where I_CODE='" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }
                                // effect of materail requisition issue qty............
                                if (result == true)
                                {
                                    //result = CommonClasses.Execute1("UPDATE MATERIAL_REQUISITION_DETAIL SET MRD_ISSUE_QTY=MRD_ISSUE_QTY+" + ((Label)dgIssueTo.Rows[i].FindControl("lblIssueQty")).Text + " where MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "' and MRD_I_CODE='" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "'");
                                    //float PTS_QTY = float.Parse(((Label)dgIssueTo.Rows[i].FindControl("lblIssueQty")).Text);
                                    int I_CODE = Convert.ToInt32(((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text);
                                    int MR_CODE = Convert.ToInt32(ddlMaterialReq.SelectedValue);
                                    SqlParameter[] Params1 =               
                                        {                                
                                            new SqlParameter("@I_CODE",I_CODE),
                                            new SqlParameter("@PTS_QTY",PTS_QTY),
                                            new SqlParameter("@MR_CODE",MR_CODE)				              
                                        };
                                    result = DL_DBAccess.Insertion_Updation_Delete("WIP_WIPE_OFF", Params1);

                                }
                            }

                        }

                        //-----
                    }


                    CommonClasses.WriteLog("Issue To Production", "Save", "Issue To Production", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtIssueDate.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                if (CommonClasses.Execute1("UPDATE ISSUE_MASTER SET IM_TYPE='" + ddlIssueType.SelectedValue + "',IM_MATERIAL_REQ='" + ddlMaterialReq.SelectedValue + "',IM_DATE='" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "' ,IM_ISSUEBY='" + txtIssueBy.Text.Trim() + "' ,IM_REQBY='" + txtReqPerson.Text.Trim() + "' ,IM_UM_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "'  where IM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and IM_COMP_ID='" + Convert.ToInt32(Session["CompanyCode"]) + "'"))
                {


                    //---- Getting Old Details
                    DataTable DtOldDetails = CommonClasses.Execute("select IMD_I_CODE,IMD_ISSUE_QTY from ISSUE_MASTER_DETAIL WHERE IM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                    //---- Reseting Item Master Stock
                    for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                    {
                        CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + DtOldDetails.Rows[n]["IMD_ISSUE_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["IMD_I_CODE"] + "'");
                        //if (ddlMaterialReq.SelectedIndex != 0)
                        //{
                        //    CommonClasses.Execute1("UPDATE MATERIAL_REQUISITION_DETAIL SET MRD_ISSUE_QTY=MRD_ISSUE_QTY-" + DtOldDetails.Rows[n]["IMD_ISSUE_QTY"] + " where MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "' and MRD_I_CODE='" + ((Label)dgIssueTo.Rows[n].FindControl("lblItemCode")).Text + "'");
                        //}
                    }

                    result = CommonClasses.Execute1("DELETE FROM ISSUE_MASTER_DETAIL WHERE IM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and STL_DOC_TYPE='ISSPROD'");


                    if (result)
                    {

                        for (int i = 0; i < dgIssueTo.Rows.Count; i++)
                        {
                            double PTS_QTY = 0;
                            if (Convert.ToString(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text) != null || Convert.ToString(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text) != "")
                            {
                                PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgIssueTo.Rows[i].FindControl("txtIssueQty")).Text)), 3);
                            }
                            result = CommonClasses.Execute1("INSERT INTO ISSUE_MASTER_DETAIL (IMD_COMP_ID,IM_CODE,IMD_I_CODE,IMD_UOM,IMD_CURR_STOCK,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK,IMD_RATE,IMD_AMOUNT) values ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblUOM_CODE")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblCurrStock")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblQtyRequirment")).Text + "','" + PTS_QTY + "','" + ((TextBox)dgIssueTo.Rows[i].FindControl("txtRemark")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblIMD_RATE")).Text + "','" + ((Label)dgIssueTo.Rows[i].FindControl("lblIMD_AMOUNT")).Text + "')");
                            //-----
                            // Inserting Into Stock Ledger

                            if (result == true)
                            {

                                // Inserting Into Stock Ledger
                                if (PTS_QTY > 0)
                                {
                                    if (result == true)
                                    {
                                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtIssueNo.Text + "','ISSPROD','" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "','-" + PTS_QTY + "')");
                                    }
                                    // relasing Stock Form Item Master

                                    if (result == true)
                                    {
                                        result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + PTS_QTY + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "' where I_CODE='" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "'");
                                    }
                                    // Inserting Into Material requisition based Based On type 
                                }
                                //if (result == true)
                                //{
                                //    //result = CommonClasses.Execute1("UPDATE MATERIAL_REQUISITION_DETAIL SET MRD_ISSUE_QTY=MRD_ISSUE_QTY+" + ((Label)dgIssueTo.Rows[i].FindControl("lblIssueQty")).Text + " where MRD_MR_CODE='" + ddlMaterialReq.SelectedValue + "' and MRD_I_CODE='" + ((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text + "'");
                                //    float PTS_QTY = float.Parse(((Label)dgIssueTo.Rows[i].FindControl("lblIssueQty")).Text);
                                //    int I_CODE = Convert.ToInt32(((Label)dgIssueTo.Rows[i].FindControl("lblItemCode")).Text);
                                //    int MR_CODE = Convert.ToInt32(ddlMaterialReq.SelectedValue);
                                //    //Upadte Material Requsition Qty
                                //    SqlParameter[] Params1 =               
                                //    {                                
                                //        new SqlParameter("@I_CODE",I_CODE),
                                //        new SqlParameter("@PTS_QTY",PTS_QTY),
                                //        new SqlParameter("@MR_CODE",MR_CODE)				              
                                //    };
                                //    result = DL_DBAccess.Insertion_Updation_Delete("WIP_WIPE_OFF", Params1);

                                //}

                            }

                            //-----

                        }

                        CommonClasses.RemoveModifyLock("ISSUE_MASTER", "MODIFY", "IM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Issue To Production", "Update", "Issue To Production", txtIssueNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);
                }
                else
                {
                    //ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtIssueDate.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void txtRequiredQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRequiredQty.Text);
        txtRequiredQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtIssueQty1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox thisCheckBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;

            double PTS_QTY = 0, Cur_Bal = 0, Req_Qty = 0;
            if (Convert.ToString(((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text) != null || Convert.ToString(((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text) != "")
            {
                PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text)), 3);
            }
            if (Convert.ToString(((Label)dgIssueTo.Rows[index].FindControl("lblCurrStock")).Text) != null || Convert.ToString(((Label)dgIssueTo.Rows[index].FindControl("lblCurrStock")).Text) != "")
            {
                Cur_Bal = Math.Round((Convert.ToDouble(((Label)dgIssueTo.Rows[index].FindControl("lblCurrStock")).Text)), 3);
            }
            //if (Convert.ToString(((Label)dgIssueTo.Rows[index].FindControl("lblQtyRequirment")).Text) != null || Convert.ToString(((Label)dgIssueTo.Rows[index].FindControl("lblQtyRequirment")).Text) != "")
            //{
            //    Req_Qty = Math.Round((Convert.ToDouble(((Label)dgIssueTo.Rows[index].FindControl("lblQtyRequirment")).Text)), 3);
            //}
            //if (PTS_QTY > Req_Qty && PTS_QTY < Cur_Bal)
            //{
            //    ((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text = Req_Qty.ToString();
            //    //((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Focus();
            //    ((TextBox)dgIssueTo.Rows[index].FindControl("txtRemark")).Focus();
            //    return;
            //}
            if (PTS_QTY < Cur_Bal)
            {
                ((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text = Cur_Bal.ToString();
                //((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Focus();
                ((TextBox)dgIssueTo.Rows[index].FindControl("txtRemark")).Focus();
                return;
            }
            ((TextBox)dgIssueTo.Rows[index].FindControl("txtRemark")).Focus();
            //----
            //if (PTS_QTY <= Cur_Bal)
            //{                
            //    ((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Text = Req_Qty.ToString();
            //    ((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Focus();
            //    return;
            //}


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "txtIssueQty_TextChanged", ex.Message.ToString());
        }
    }
}
