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

public partial class Transactions_ADD_StockAdjustment : System.Web.UI.Page
{
    static int mlCode = 0;
    DataTable dt = new DataTable();
    DataTable dtDetail = new DataTable();
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static string right = "";
    public static string str = "";
    static DataTable dt2 = new DataTable();
    static string ItemUpdateIndex = "-1";
    public static int Index = 0;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='60'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    //LoadMaterialReq();
                    //LoadICode();
                    //LoadIName();
                    //BlankGridView();
                    //ddlMaterialReq.Enabled = false;

                    ViewState["BindTable"] = BindTable;
                    ViewState["dt2"] = dt2;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["Index"] = Index;
                    try
                    {
                        ((DataTable)ViewState["BindTable"]).Clear();

                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }

                        if (Request.QueryString[0].Equals("INSERT"))
                        {
                            txtStockAdjustmentDate.Attributes.Add("readonly", "readonly");
                            str = "";
                            LoadICode();
                            LoadIName();
                            BlankGridView();
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            ((DataTable)ViewState["dt2"]).Columns.Clear();
                            //txtStockAdjustmentDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtStockAdjustmentDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Stock Adjustment", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Stock Adjustment", "Page_Load", ex.Message.ToString());
        }
    }

    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_NAME  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_NAME ASC");
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
    #region LoadCurrStock
    private void LoadCurrStock()
    {
        try
        {
            //if (ddlMaterialReq.SelectedIndex != 0)
            //{
            dt = CommonClasses.Execute("select I_CODE,ISNULL(ROUND(( SELECT SUM(STL_DOC_QTY) FROM STOCK_LEDGER where STL_I_CODE=I_CODE),2),0) AS I_CURRENT_BAL from ITEM_MASTER where  ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODE='" + ddlItemCode.SelectedValue + "' ");


            if (dt.Rows[0]["I_CURRENT_BAL"].ToString() == "")
            {
                txtCurrStock.Text = "0.000";
            }
            else
            {
                txtCurrStock.Text = dt.Rows[0]["I_CURRENT_BAL"].ToString();
                txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text));
            }
            // }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadCurrStock", Ex.Message);
        }

    }
    #endregion

    private void LoadICode()
    {
        try
        {
            //dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER,MATERIAL_REQUISITION_MASTER where MR_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!=-2147483646 ORDER BY I_NAME");
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_CODENO  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_CODENO");
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

    private void BlankGridView()
    {
        DataTable dtFilter = new DataTable();
        if (dgStockAdjustment.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("SAD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SAD_ADJUSTMENT_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SAD_REMARK", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgStockAdjustment.DataSource = dtFilter;
                dgStockAdjustment.DataBind();
                dgStockAdjustment.Enabled = false;
            }
        }
    }

    private void ViewRec(string str)
    {
        try
        {
            txtStockAdjustmentDate.Attributes.Add("readonly", "readonly");
            LoadICode();
            ddlItemCode_SelectedIndexChanged(null, null);
            LoadIName();
            dtDetail.Clear();
            //dt = CommonClasses.Execute("Select SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + mlCode + "");
            dt = CommonClasses.Execute("SELECT SAM_DATE,SAM_DOC_NO FROM STOCK_ADJUSTMENT_MASTER where ES_DELETE=0 and SAM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and SAM_CODE=" + mlCode + "");

            if (dt.Rows.Count > 0)
            {
                //mlCode = Convert.ToInt32(dt.Rows[0]["SAM_CODE"]);
                // ddlIssueType.SelectedValue = dt.Rows[0]["IM_TYPE"].ToString();
                //  ddlMaterialReq.SelectedValue = dt.Rows[0]["IM_MATERIAL_REQ"].ToString();
                txtIssueNo.Text = dt.Rows[0]["SAM_DOC_NO"].ToString();

                txtStockAdjustmentDate.Text = Convert.ToDateTime(dt.Rows[0]["SAM_DATE"]).ToString("dd MMM yyyy");

                dtDetail = CommonClasses.Execute("select SAD_I_CODE,I_CODENO+(I_NAME) as ItemName,cast(SAD_ADJUSTMENT_QTY as numeric(10,3)) as SAD_ADJUSTMENT_QTY,SAD_REMARK FROM STOCK_ADJUSTMENT_DETAIL,ITEM_MASTER where SAD_I_CODE=I_CODE and SAD_SAM_CODE='" + mlCode + "' ");

                if (dtDetail.Rows.Count > 0)
                {
                    dgStockAdjustment.DataSource = dtDetail;
                    dgStockAdjustment.DataBind();
                    ViewState["dt2"] = dtDetail;
                }

            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;

                txtStockAdjustmentDate.Enabled = false;

                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;


                txtCurrStock.Enabled = false;
                txtStockAdjustmentQty.Enabled = false;

                dgStockAdjustment.Enabled = false;
                txtRemark.Enabled = false;
            }
            else if (str == "MOD")
            {
                CommonClasses.SetModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", Convert.ToInt32(mlCode));
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ViewRec", Ex.Message);
        }
    }


    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;

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

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;

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

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtStockAdjustmentDate.Text == "")
            {
                //ShowMessage("#Avisos", "Please Select Stock Adjustment Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Stock Adjustment Date Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Focus();
                return;
            }

            if (ddlItemCode.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

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
            if (txtStockAdjustmentQty.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter Stock Adjustment Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Stock Adjustment Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Focus();
                return;
            }
            if (txtCurrStock.Text.Trim() == "")
            {
                txtCurrStock.Text = "0";
            }
            if ((Convert.ToDouble(txtCurrStock.Text) + Convert.ToDouble(txtStockAdjustmentQty.Text)) < 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " Adjustment Qty  should not greater than Current Stock";
                txtStockAdjustmentQty.Focus();
                return;
            }
            //if (txtCurrStock.Text != "")
            //{
            //    if (txtCurrStock.Text != "0")
            //    {
            //        if (txtCurrStock.Text != "0.000")
            //        {

            //            if (Convert.ToDouble(txtCurrStock.Text) < Convert.ToDouble(txtStockAdjustmentQty.Text))
            //            {
            //                PanelMsg.Visible = true;
            //                lblmsg.Text = " Adjustment Qty not greater than Current Stock";
            //                txtStockAdjustmentQty.Focus();
            //                return;
            //            }
            //        }
            //    }
            //}




            if (dgStockAdjustment.Rows.Count > 0)
            {
                for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgStockAdjustment.Rows[i].FindControl("lblItemCode"))).Text;
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
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }

                    }

                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_I_CODE");
                //----
                //((DataTable)ViewState["dt2"]).Columns.Add("ItemCodeNo"); //change Binding Value Name From ItemCode1 to ItemCodeNo
                //----
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName"); //change Binding Value Name From StockUOM1 to UOM_CODE
                //----
                //((DataTable)ViewState["dt2"]).Columns.Add("UOM_CODE");
                //----
                //((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
                //((DataTable)ViewState["dt2"]).Columns.Add("CurrStock");
                //((DataTable)ViewState["dt2"]).Columns.Add("IMD_REQ_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_ADJUSTMENT_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_REMARK");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["SAD_I_CODE"] = ddlItemCode.SelectedValue;
            //----

            //----
            dr["ItemName"] = ddlItemCode.SelectedItem + "  (" + ddlItemName.SelectedItem + ")";


            //if (txtCurrStock.Text != "")
            //{
            //    // dr["CurrStock1"] = ddlCurrStock.SelectedValue;
            //    dr["CurrStock"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtCurrStock.Text)), 3));
            //}
            //else
            //{
            //}
            // dr["CurrStock"] = ddlCurrStock.SelectedItem;
            dr["SAD_ADJUSTMENT_QTY"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtStockAdjustmentQty.Text)), 3));
            dr["SAD_REMARK"] = txtRemark.Text;

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
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                dgStockAdjustment.Enabled = true;
                dgStockAdjustment.Visible = true;
                dgStockAdjustment.DataSource = ((DataTable)ViewState["dt2"]);
                dgStockAdjustment.DataBind();
            }
            #endregion
            clearDetail();
        }
        catch (Exception ex)
        {

        }

    }

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;


            txtCurrStock.Text = "";



            txtStockAdjustmentQty.Text = "";
            txtRemark.Text = "";

            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "clearDetail", Ex.Message);
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

        //((DataTable)ViewState["dt2"]).Rows.Clear();
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
            if (txtStockAdjustmentDate.Text == "")
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions//VIEW/ViewStockAdjustment.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "CancelRecord", Ex.Message);
        }
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgStockAdjustment.Enabled == true)
        {
            if (txtStockAdjustmentDate.Text == "")
            {
                // ShowMessage("#Avisos", "Select Po Type", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Stock Adjustment Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                txtStockAdjustmentDate.Focus();
                return;
            }


            if (dgStockAdjustment.Rows.Count > 0)
            {

                SaveRec();
            }


        }
        else
        {
            //ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found In Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        // dt = CommonClasses.Execute("select max(cast((isnull(PS_GIN_NO,0)) as numeric(10,0))) as PS_GIN_NO from PRODUCTION_TO_STORE_MASTER where PS_CM_COMP_CODE='" + Session["CompanyCode"] + "' ");
        dt = CommonClasses.Execute("Select isnull(max(SAM_DOC_NO),0) as SAM_DOC_NO FROM STOCK_ADJUSTMENT_MASTER WHERE SAM_CM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
        if (dt.Rows[0]["SAM_DOC_NO"] == null || dt.Rows[0]["SAM_DOC_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["SAM_DOC_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {

                int Doc_no = Convert.ToInt32(Numbering());
                DataTable dt = new DataTable();
                //dt = CommonClasses.Execute("Select isnull(max(SAM_DOC_NO),0) as SAM_DOC_NO FROM STOCK_ADJUSTMENT_MASTER WHERE SAM_CM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                //if (dt.Rows.Count > 0)
                //{
                //    Doc_no = Convert.ToInt32(dt.Rows[0]["SAM_DOC_NO"]);
                //    Doc_no = Doc_no + 1;
                //}

                //if (CommonClasses.Execute1("INSERT INTO ISSUE_MASTER (SPOM_CM_CODE,SPOM_AMOUNT,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text + "','" + Convert.ToDateTime(txtSupplierRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text + "','" + txtTranspoter.Text + "','" + txtPaymentTerm.Text + "','" + txtFreightTermsg.Text + "','" + txtGuranteeWaranty.Text + "','" + txtNote.Text + "','" + txtDeliveryTo.Text + "')"))
                if (CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_MASTER(SAM_DATE,SAM_DOC_NO,SAM_CM_COMP_CODE) VALUES ('" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + Doc_no + "','" + Convert.ToInt32(Session["CompanyCode"]) + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SAM_CODE) from STOCK_ADJUSTMENT_MASTER");
                    for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                    {
                        //Inserting Into Issue To Production Detail
                        result = CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_DETAIL(SAD_SAM_CODE,SAD_I_CODE,SAD_ADJUSTMENT_QTY,SAD_REMARK)VALUES('" + Code + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblRemark")).Text + "')");

                        if (result == true)
                        {
                            //-----
                            // Inserting Into Stock Ledger


                            if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text != "0.000")
                            {
                                // Inserting Into Stock Ledger

                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Doc_no + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "')");
                                }
                                // relasing Stock Form Item Master

                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }

                            }
                        }
                        //-----
                    }

                    CommonClasses.WriteLog("Stock Adjustment", "Save", "Stock Adjustment", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);

                }
                else
                {

                    //ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockAdjustmentDate.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                if (CommonClasses.Execute1("UPDATE STOCK_ADJUSTMENT_MASTER SET SAM_DATE ='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where SAM_CODE='" + mlCode + "' and SAM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'"))
                {


                    //---- Getting Old Details
                    DataTable DtOldDetails = CommonClasses.Execute("select SAD_I_CODE,SAD_ADJUSTMENT_QTY from STOCK_ADJUSTMENT_DETAIL WHERE SAD_SAM_CODE='" + mlCode + "'");

                    //---- Reseting Item Master Stock
                    for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                    {
                        if (Convert.ToInt32(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"]) > 0)
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)-" + DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                        else
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                    }

                    result = CommonClasses.Execute1("DELETE FROM STOCK_ADJUSTMENT_DETAIL WHERE SAD_SAM_CODE='" + mlCode + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + mlCode + "' and STL_DOC_TYPE='STCADJ'");


                    if (result)
                    {

                        for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_DETAIL (SAD_SAM_CODE,SAD_I_CODE,SAD_ADJUSTMENT_QTY,SAD_REMARK) values ('" + mlCode + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblRemark")).Text + "')");
                            //-----
                            // Inserting Into Stock Ledger

                            if (result == true)
                            {
                                if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text != "0.000")
                                {
                                    // Inserting Into Stock Ledger

                                    if (result == true)
                                    {
                                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + mlCode + "','" + txtIssueNo.Text + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "')");
                                    }
                                    // relasing Stock Form Item Master

                                    if (result == true)
                                    {
                                        result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=ISNULL(I_CURRENT_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ", I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                    }

                                }
                            }
                            //-----

                        }

                        CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
                        CommonClasses.WriteLog("STOCK_ADJUSTMENT_MASTER", "Update", "STOCK_ADJUSTMENT_MASTER", txtIssueNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);
                }
                else
                {
                    //ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtStockAdjustmentDate.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("STOCK_ADJUSTMENT_MASTER", "SaveRec", ex.Message);
        }
        return result;
    }

    protected void dgStockAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #region dgStockAdjustment_RowCommand
    protected void dgStockAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgStockAdjustment.Rows[Convert.ToInt32(ViewState["Index"].ToString())];


            if (e.CommandName == "Delete")
            {
                // int rowindex = row.RowIndex;
                //dgSupplierPurchaseOrder.DeleteRow(Convert.ToInt32(ViewState["Index"].ToString()));
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgStockAdjustment.DataSource = ((DataTable)ViewState["dt2"]);
                dgStockAdjustment.DataBind();
                if (dgStockAdjustment.Rows.Count == 0)
                    BlankGridView();
            }

            if (e.CommandName == "Modify")
            {
                //LoadSupplier();

                LoadICode();
                LoadIName();
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                //ddlItemCode_SelectedIndexChanged(null, null);
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                // ddlItemName_SelectedIndexChanged(null, null);


                ddlItemCode_SelectedIndexChanged(null, null);


                //txtCurrStock.Text = ((Label)(row.FindControl("lblCurrStock"))).Text;
                //----

                //----



                //----


                txtStockAdjustmentQty.Text = ((Label)(row.FindControl("lblSAD_ADJUSTMENT_QTY"))).Text;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    if (Convert.ToDouble(txtStockAdjustmentQty.Text) > 0)
                    {
                        txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text) - Convert.ToDouble(txtStockAdjustmentQty.Text));
                    }
                    else
                    {
                        txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text) + Math.Abs(Convert.ToDouble(txtStockAdjustmentQty.Text)));
                    }
                }
                //----
                txtStockAdjustmentQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtStockAdjustmentQty.Text));
                //----

                txtRemark.Text = ((Label)(row.FindControl("lblRemark"))).Text;

            }



        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment", "dgStockAdjustment_RowCommand", Ex.Message);
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
            if ( no > 15)
            {
                 no = 15;
            }
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
    protected void txtStockAdjustmentQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtStockAdjustmentQty.Text);

        txtStockAdjustmentQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

    }


    protected void txtStockAdjustmentDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtStockAdjustmentDate.Text) >= DateTime.Now)
            {
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }

    }
}
