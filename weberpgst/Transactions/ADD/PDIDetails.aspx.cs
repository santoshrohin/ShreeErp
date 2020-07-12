using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Transactions_ADD_PDIDetails : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int InwardNo = 0;
    static string ItemUpdateIndex = "-1";
    static DataTable inspectiondt = new DataTable();
    static DataTable dt = new DataTable();
    static string type = "";
    DataTable dtFilter = new DataTable();
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static string str = "";
    public int icode;
    DatabaseAccessLayer DL_DBAccess = null;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            //home.Attributes["class"] = "active";
            //HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
            //home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    type = Request.QueryString[0].ToString();
                    dt2.Clear();
                    ViewState["dt2"] = dt2;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ((DataTable)ViewState["dt2"]).Clear();
                    ViewState["mlCode"] = "";
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        InwardNo = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        type = "VIEW";
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        InwardNo = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                        type = "MOD";
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        ViewRec("ADD");
                        type = "ADD";
                        LoadFilter();//new Suja
                        LoadInvoiceNo();
                    }
                    // LoadInvoiceNo();
                    // LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-ADD", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            if (str == "MOD" || str == "VIEW")
            {
                DataTable DtTblDetail = new DataTable();
                DtTblDetail = CommonClasses.Execute("select INSPDI_CODE,isnull(INSPDI_TYPE,0) as INSPDI_TYPE,INSPDI_INSM_CODE,INSPDI_I_CODE from INSPECTION_PDI_DETAIL where INSPDI_INSM_CODE=" + Request.QueryString[1] + "");
                if (DtTblDetail.Rows.Count > 0)
                {
                    LoadInvoiceNo();
                    ddlInvoiceNo.SelectedValue = Convert.ToInt32(DtTblDetail.Rows[0]["INSPDI_INSM_CODE"]).ToString();
                    ddlInvoiceNo_SelectedIndexChanged(null, null);
                    ddlItemName.SelectedValue = Convert.ToInt32(DtTblDetail.Rows[0]["INSPDI_I_CODE"]).ToString();
                    if (rbtType.SelectedValue == "0")
                    {
                        rbtType.SelectedValue = Convert.ToInt32(DtTblDetail.Rows[0]["INSPDI_TYPE"]).ToString();
                    }
                    else
                    {
                        rbtType.SelectedValue = Convert.ToInt32(DtTblDetail.Rows[0]["INSPDI_TYPE"]).ToString();
                    }
                    // ddlItemName.Enabled = false;
                    ddlInvoiceNo.Enabled = false;
                    rbtType.Enabled = false;
                }

                DataTable Dtdetail = new DataTable();
                Dtdetail = CommonClasses.Execute("select INSPDI_CODE,I_CODENO+'-'+I_NAME as INSPDI_I_CODE,I_CODE as INSPDI_I_CODEVAL,INSPDI_INSM_CODE,INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSPDI_NOTE from INSPECTION_PDI_DETAIL,ITEM_MASTER where INSPDI_I_CODE<0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_I_CODE=I_CODE and INSPDI_INSM_CODE=" + Request.QueryString[1] + "");
                if (Dtdetail.Rows.Count > 0)
                {
                    dgPDIDEtail.Enabled = true;
                    dgPDIDEtail.DataSource = Dtdetail;
                    dgPDIDEtail.DataBind();
                    ViewState["dt2"] = Dtdetail;
                }
                else
                {
                    LoadFilter();
                }
            }
            if (str == "VIEW")
            {
                rbtType.Enabled = false;
                ddlInvoiceNo.Enabled = false;
                ddlItemName.Enabled = false;
                txtparameter.Enabled = false;
                txtSpecification.Enabled = false;
                txtInspection.Enabled = false;
                txtObservation1.Enabled = false;
                txtObservation2.Enabled = false;
                txtObservation3.Enabled = false;
                txtObservation4.Enabled = false;
                txtObservation5.Enabled = false;
                txtRemarkPDI.Enabled = false;
                txtDisposition.Enabled = false;
                dgPDIDEtail.Enabled = false;
                btnInsert.Enabled = false;
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
                txtNote.Enabled = false;
            }
            //else if (str == "ADD")
            //{
            //    btnSubmit.Visible = false;
            //    btnCancel.Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
            // Change By Mahesh :- (PCPL Logic is Insert Only One Item Insert) Source Code Commented For GST ERP

            #region Validation
            if (ddlInvoiceNo.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Invoice No.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            //if (type == "MOD")
            //{

            //}
            //else
            //{
            if (ddlItemName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            // }

            if (txtparameter.Text.Trim() == "" || txtInspection.Text.Trim() == "" || txtSpecification.Text.Trim() == "" || txtObservation1.Text.Trim() == "" || txtObservation2.Text.Trim() == "" || txtObservation3.Text.Trim() == "" || txtObservation4.Text.Trim() == "" || txtObservation5.Text.Trim() == "" || txtDisposition.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter PDI details";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            #endregion Validation

            if (((DataTable)ViewState["dt2"]).Columns.Count != 0)
            {
                for (int i = 0; i < dgPDIDEtail.Rows.Count; i++)
                {
                    string Par = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_PARAMETERS"))).Text;
                    string Par1 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_SPECIFTION"))).Text;
                    string Par2 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSPECTION"))).Text;
                    string Par3 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR1"))).Text;
                    string Par4 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR2"))).Text;
                    string Par5 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR3"))).Text;
                    string Par6 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR4"))).Text;
                    string Par7 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR5"))).Text;
                    string Par8 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblIINSPDI_DSPOSITION"))).Text;
                    string Par9 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_REMARK"))).Text;
                    string Par10 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODE"))).Text;//suja-I Name and I CODENO
                    int Par11 = Convert.ToInt32(((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSM_CODE"))).Text);
                    int Par12 = Convert.ToInt32(((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODEVAL"))).Text);//suja-I Code 
                    string Par13 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_NOTE"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (Par == txtparameter.Text.Trim() && Par1 == txtSpecification.Text.Trim() && Par2 == txtInspection.Text.Trim() && Par3 == txtObservation1.Text.Trim() && Par4 == txtObservation2.Text.Trim() && Par5 == txtObservation3.Text.Trim() && Par6 == txtObservation4.Text.Trim() && Par7 == txtObservation5.Text.Trim() && Par8 == txtDisposition.Text.Trim() && Par9 == txtRemarkPDI.Text.Trim() && Convert.ToInt32(Par12).ToString() == ddlItemName.SelectedValue)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist with Same PDI In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            ClearControles();
                            return;
                        }
                    }
                    else
                    {
                        if (Par == txtparameter.Text.Trim() && Par1 == txtSpecification.Text.Trim() && Par2 == txtInspection.Text.Trim() && Par3 == txtObservation1.Text.Trim() && Par4 == txtObservation2.Text.Trim() && Par5 == txtObservation3.Text.Trim() && Par6 == txtObservation4.Text && Par7 == txtObservation5.Text.Trim() && Par8 == txtDisposition.Text.Trim() && Par9 == txtRemarkPDI.Text.Trim() && Convert.ToInt32(Par12).ToString() == ddlItemName.SelectedValue && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist with Same PDI In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            ClearControles();
                            return;
                        }
                    }
                }
            }

            #region Add Data table coloums
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_PARAMETERS");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_SPECIFTION");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_INSPECTION");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_OBSR1");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_OBSR2");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_OBSR3");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_OBSR4");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_OBSR5");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_DSPOSITION");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_I_CODEVAL");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_INSM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_NOTE");
            }
            #endregion

            #region add control value to Dt

            dr = ((DataTable)ViewState["dt2"]).NewRow();

            dr["INSPDI_PARAMETERS"] = txtparameter.Text;
            dr["INSPDI_SPECIFTION"] = txtSpecification.Text;
            dr["INSPDI_INSPECTION"] = txtInspection.Text;
            dr["INSPDI_OBSR1"] = txtObservation1.Text;
            dr["INSPDI_OBSR2"] = txtObservation2.Text;
            dr["INSPDI_OBSR3"] = txtObservation3.Text;
            dr["INSPDI_OBSR4"] = txtObservation4.Text;
            dr["INSPDI_OBSR5"] = txtObservation5.Text;
            dr["INSPDI_DSPOSITION"] = txtDisposition.Text;
            dr["INSPDI_REMARK"] = txtRemarkPDI.Text;
            dr["INSPDI_I_CODE"] = ddlItemName.SelectedItem.Text;
            dr["INSPDI_I_CODEVAL"] = ddlItemName.SelectedValue;
            dr["INSPDI_INSM_CODE"] = ddlInvoiceNo.SelectedValue;
            dr["INSPDI_NOTE"] = txtNote.Text;

            #endregion control

            #region insert Data or Modify Grid Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    dgPDIDEtail.Enabled = true;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                dgPDIDEtail.Enabled = true;
            }
            #endregion

            //#region check Data table,insert or Modify Data
            //if (str == "Modify")
            //{
            //    if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            //    {
            //        ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
            //        ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
            //        dgPDIDEtail.Enabled = true;
            //    }
            //}
            //else
            //{
            //    ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            //    dgPDIDEtail.Enabled = true;
            //    //ddlInvoiceNo.Enabled = false;
            //    // rbtType.Enabled = false;
            //}
            //#endregion

            #region Binding data to Grid
            dgPDIDEtail.Visible = true;
            dgPDIDEtail.Enabled = true;

            dgPDIDEtail.DataSource = ((DataTable)ViewState["dt2"]);
            dgPDIDEtail.DataBind();

            #endregion

            #region Tax
            if (((DataTable)ViewState["dt2"]).Rows.Count == 1)
            {
            }
            #endregion

            #region Clear Controles

            ClearControles();

            #endregion

            ViewState["RowCount"] = 1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ClearControles
    void ClearControles()
    {
        // ddlInvoiceNo.SelectedValue = "0";
        // ddlItemName.SelectedValue = "0";
        txtparameter.Text = "";
        txtInspection.Text = "";
        txtSpecification.Text = "";
        txtObservation1.Text = "";
        txtObservation2.Text = "";
        txtObservation3.Text = "";
        txtObservation4.Text = "";
        txtObservation5.Text = "";
        txtDisposition.Text = "";
        txtRemarkPDI.Text = "";
        txtNote.Text = "";

        str = "";

        ViewState["RowCount"] = 0;
        ViewState["ItemUpdateIndex"] = "-1";
    }
    #endregion

    #region dgPDIDEtail_RowDeleting
    protected void dgPDIDEtail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgPDIDEtail_PageIndexChanging
    protected void dgPDIDEtail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgPDIDEtail.PageIndex = e.NewPageIndex;
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgPDIDEtail_RowCommand
    protected void dgPDIDEtail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgPDIDEtail.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgPDIDEtail.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgPDIDEtail.DataSource = ((DataTable)ViewState["dt2"]);
                dgPDIDEtail.DataBind();
                if (dgPDIDEtail.Rows.Count == 0)
                {
                    LoadFilter();
                    dt2.Clear();
                    ViewState["dt2"] = dt2;
                }
            }
            if (e.CommandName == "Modify")
            {


                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();

                // string s = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                txtparameter.Text = ((Label)(row.FindControl("lblINSPDI_PARAMETERS"))).Text;
                txtSpecification.Text = ((Label)(row.FindControl("lblINSPDI_SPECIFTION"))).Text;
                txtInspection.Text = ((Label)(row.FindControl("lblINSPDI_INSPECTION"))).Text;
                txtObservation1.Text = ((Label)(row.FindControl("lblINSPDI_OBSR1"))).Text;
                txtObservation2.Text = ((Label)(row.FindControl("lblINSPDI_OBSR2"))).Text;
                txtObservation3.Text = ((Label)(row.FindControl("lblINSPDI_OBSR3"))).Text;
                txtObservation4.Text = ((Label)(row.FindControl("lblINSPDI_OBSR4"))).Text;
                txtObservation5.Text = ((Label)(row.FindControl("lblINSPDI_OBSR5"))).Text;
                txtDisposition.Text = ((Label)(row.FindControl("lblIINSPDI_DSPOSITION"))).Text;
                txtRemarkPDI.Text = ((Label)(row.FindControl("lblINSPDI_REMARK"))).Text;
                LoadInvoiceNo();
                ddlInvoiceNo.SelectedValue = ((Label)(row.FindControl("lblINSPDI_INSM_CODE"))).Text;
                ddlInvoiceNo_SelectedIndexChanged(null, null);
                //ddlItemName.SelectedItem.Text = ((Label)(row.FindControl("lblINSPDI_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblINSPDI_I_CODEVAL"))).Text;
                txtNote.Text = ((Label)(row.FindControl("lblINSPDI_NOTE"))).Text;
                foreach (GridViewRow gvr in dgPDIDEtail.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection-ADD", "dgInspection_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region LoadFilter//new suja
    public void LoadFilter()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_PARAMETERS", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_SPECIFTION", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSPECTION", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR1", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR2", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR3", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR4", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR5", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_DSPOSITION", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_REMARK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_I_CODEVAL", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSM_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_NOTE", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgPDIDEtail.DataSource = dtFilter;
            dgPDIDEtail.DataBind();
            dgPDIDEtail.Enabled = false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/View/ViewPDIDetails.aspx", false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnSave_Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validation
            if (ddlInvoiceNo.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Invoice No.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (type == "MOD")
            {

            }
            else
            {
                if (ddlItemName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Item Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
            }
            if (dgPDIDEtail.Enabled == false)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter PDI details";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            //if (txtparameter.Text == "" || txtInspection.Text == "" || txtSpecification.Text == "" || txtObservation1.Text == "" || txtObservation2.Text == "" || txtObservation3.Text == "" || txtObservation4.Text == "" || txtObservation5.Text == "" || txtDisposition.Text == "")
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please enter PDI details";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //    return;
            //}
            #endregion Validation

            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PDI Details", "btnSave_Click", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                for (int i = 0; i < dgPDIDEtail.Rows.Count; i++)
                {
                    string INSPDI_CODE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODEVAL")).Text;
                    string INSPDI_INSM_CODE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSM_CODE")).Text;
                    string INSPDI_PARAMETERS = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_PARAMETERS")).Text;
                    string INSPDI_SPECIFTION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_SPECIFTION")).Text;
                    string INSPDI_INSPECTION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSPECTION")).Text;
                    string INSPDI_OBSR1 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR1")).Text;
                    string INSPDI_OBSR2 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR2")).Text;
                    string INSPDI_OBSR3 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR3")).Text;
                    string INSPDI_OBSR4 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR4")).Text;
                    string INSPDI_OBSR5 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR5")).Text;
                    string INSPDI_DSPOSITION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblIINSPDI_DSPOSITION")).Text;
                    string INSPDI_REMARK = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_REMARK")).Text;
                    string INSPDI_I_CODE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODE")).Text;
                    string INSPDI_I_CODEVAL = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODEVAL")).Text;
                    string INSPDI_NOTE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_NOTE")).Text;
                    //string INSPDI_TYPE=(())
                    if (CommonClasses.Execute1("INSERT INTO INSPECTION_PDI_DETAIL (INSPDI_INSM_CODE, INSPDI_PARAMETERS, INSPDI_SPECIFTION, INSPDI_INSPECTION, INSPDI_OBSR1, INSPDI_OBSR2, INSPDI_OBSR3, INSPDI_OBSR4, INSPDI_OBSR5, INSPDI_DSPOSITION, INSPDI_REMARK, INSPDI_I_CODE,INSPDI_TYPE,INSPDI_NOTE)VALUES ('" + INSPDI_INSM_CODE + "','" + INSPDI_PARAMETERS + "','" + INSPDI_SPECIFTION + "','" + INSPDI_INSPECTION + "','" + INSPDI_OBSR1 + "','" + INSPDI_OBSR2 + "','" + INSPDI_OBSR3 + "','" + INSPDI_OBSR4 + "','" + INSPDI_OBSR5 + "','" + INSPDI_DSPOSITION + "','" + INSPDI_REMARK + "','" + INSPDI_I_CODEVAL + "','" + rbtType.SelectedValue + "','" + INSPDI_NOTE + "')")) ;
                    {
                        CommonClasses.WriteLog("PDI DETAIL", "Save", "PDI DETAIL", Convert.ToString(INSPDI_PARAMETERS), Convert.ToInt32(INSPDI_INSM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewPDIDetails.aspx", false);
                    }
                }
            }
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("delete from INSPECTION_PDI_DETAIL where INSPDI_INSM_CODE='" + Request.QueryString[1] + "'"))
                {
                    for (int i = 0; i < dgPDIDEtail.Rows.Count; i++)
                    {
                        string INSPDI_INSM_CODE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSM_CODE")).Text;
                        string INSPDI_PARAMETERS = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_PARAMETERS")).Text;
                        string INSPDI_SPECIFTION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_SPECIFTION")).Text;
                        string INSPDI_INSPECTION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_INSPECTION")).Text;
                        string INSPDI_OBSR1 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR1")).Text;
                        string INSPDI_OBSR2 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR2")).Text;
                        string INSPDI_OBSR3 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR3")).Text;
                        string INSPDI_OBSR4 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR4")).Text;
                        string INSPDI_OBSR5 = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_OBSR5")).Text;
                        string INSPDI_DSPOSITION = ((Label)dgPDIDEtail.Rows[i].FindControl("lblIINSPDI_DSPOSITION")).Text;
                        string INSPDI_REMARK = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_REMARK")).Text;
                        string INSPDI_I_CODE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODE")).Text;
                        string INSPDI_I_CODEVAL = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODEVAL")).Text;
                        string INSPDI_NOTE = ((Label)dgPDIDEtail.Rows[i].FindControl("lblINSPDI_NOTE")).Text;
                        {
                            if (CommonClasses.Execute1("INSERT INTO INSPECTION_PDI_DETAIL (INSPDI_INSM_CODE, INSPDI_PARAMETERS, INSPDI_SPECIFTION, INSPDI_INSPECTION, INSPDI_OBSR1, INSPDI_OBSR2, INSPDI_OBSR3, INSPDI_OBSR4, INSPDI_OBSR5, INSPDI_DSPOSITION, INSPDI_REMARK, INSPDI_I_CODE,INSPDI_TYPE,INSPDI_NOTE)VALUES ('" + INSPDI_INSM_CODE + "','" + INSPDI_PARAMETERS + "','" + INSPDI_SPECIFTION + "','" + INSPDI_INSPECTION + "','" + INSPDI_OBSR1 + "','" + INSPDI_OBSR2 + "','" + INSPDI_OBSR3 + "','" + INSPDI_OBSR4 + "','" + INSPDI_OBSR5 + "','" + INSPDI_DSPOSITION + "','" + INSPDI_REMARK + "','" + INSPDI_I_CODEVAL + "','" + rbtType.SelectedValue + "','" + INSPDI_NOTE + "')")) ;
                            {
                                CommonClasses.WriteLog("PDI DETAIL", "Update", "PDI DETAIL", Convert.ToString(Convert.ToInt32(ViewState["mlCode"].ToString())), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                                ((DataTable)ViewState["dt2"]).Rows.Clear();
                                result = true;
                            }
                        }
                    }
                }
                Response.Redirect("~/Transactions/VIEW/ViewPDIDetails.aspx", false);
            }
            else
            {

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PDI Details", "SaveRec", Ex.Message);
        }
        return result;
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PDI Details", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("INSPECTION_S_MASTER", "MODIFY", "INSM_CODE", mlCode);
            }
            Response.Redirect("~/Transactions/View/ViewPDIDetails.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
            //SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (type == "MOD")
        {
            LoadItem();
        }
        else
        {
            LoadItem();
            dt2.Clear();
            ViewState["dt2"] = dt2;
            LoadFilter();
        }
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
       // DataTable dtPar = CommonClasses.Execute("select INSPDI_CODE,INSPDI_INSM_CODE,INSPDI_PARAMETERS,INSPDI_SPECIFTION from INVOICE_MASTER,INSPECTION_PDI_DETAIL where INM_CODE=INSPDI_INSM_CODE and INM_CM_CODE='" + Session["Company_CODE"] + "' and INVOICE_MASTER.ES_DELETE=0 and INSPECTION_PDI_DETAIL.ES_DELETE=0");
        txtparameter.Text = "APPERANCE";
        txtSpecification.Text = "Electronic parts should be soldered at their respective positions. Through holes to be filled properly. There should be no extra solder scrap, No joint soldering, No dry solder, No Cu track peel off, No bare Cu, No bend of terminal.";
    }

    #region LoadInvoiceNo
    private void LoadInvoiceNo()
    {
        try
        {
            DataTable dt = new DataTable();
            if (type == "ADD")
            {
                if (rbtType.SelectedValue == "0")
                {
                    dt = CommonClasses.Execute("SELECT distinct INM_CODE,INM_NO FROM INVOICE_MASTER where INM_CODE NOT IN (SELECT INSPDI_INSM_CODE FROM INSPECTION_PDI_DETAIL where INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0) and INM_TYPE='TAXINV' and INVOICE_MASTER.ES_DELETE= 0 order by INM_NO desc");
                }
                else
                {
                    dt = CommonClasses.Execute("SELECT distinct INM_CODE,INM_NO FROM INVOICE_MASTER where INM_CODE NOT IN (SELECT INSPDI_INSM_CODE FROM INSPECTION_PDI_DETAIL where INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0) and INM_TYPE='OutJWINM' and INVOICE_MASTER.ES_DELETE= 0 order by INM_NO desc");
                }
            }

            if (type == "MOD" || type == "MODIFY" || type == "VIEW")
            {
                DataTable dtType = CommonClasses.Execute("select INSPDI_INSM_CODE,INSPDI_TYPE from INSPECTION_PDI_DETAIL where INSPDI_INSM_CODE=" + Request.QueryString[1] + "");
                if (!Convert.ToBoolean(dtType.Rows[0]["INSPDI_TYPE"]))
                {
                    dt = CommonClasses.Execute("SELECT distinct INM_CODE,INM_NO FROM INVOICE_MASTER where INM_CODE IN (SELECT INSPDI_INSM_CODE FROM INSPECTION_PDI_DETAIL where INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_INSM_CODE=" + Request.QueryString[1] + ") and INM_TYPE='TAXINV' and INVOICE_MASTER.ES_DELETE= 0 order by INM_NO desc");
                }
                else
                {
                    dt = CommonClasses.Execute("SELECT distinct INM_CODE,INM_NO FROM INVOICE_MASTER where INM_CODE IN (SELECT INSPDI_INSM_CODE FROM INSPECTION_PDI_DETAIL where INSPDI_I_CODE <0 and INSPECTION_PDI_DETAIL.ES_DELETE=0 and INSPDI_INSM_CODE=" + Request.QueryString[1] + ") and INM_TYPE='OutJWINM' and INVOICE_MASTER.ES_DELETE= 0 order by INM_NO desc");
                }
            }
            ddlInvoiceNo.DataSource = dt;
            ddlInvoiceNo.DataTextField = "INM_NO";
            ddlInvoiceNo.DataValueField = "INM_CODE";
            ddlInvoiceNo.DataBind();
            ddlInvoiceNo.Items.Insert(0, new ListItem("Select Invoice No.", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("PDI Details", "LoadInvoiceNo", Ex.Message);
        }
    }
    #endregion

    #region LoadItem
    private void LoadItem()
    {
        try
        {
            DataTable dt = new DataTable();
            if (type == "ADD")
            {
                if (rbtType.SelectedValue == "0")
                {
                    dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER where INM_CODE=IND_INM_CODE and IND_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and INM_TYPE='TAXINV' and INM_CODE='" + ddlInvoiceNo.SelectedValue + "' ORDER BY I_NAME");
                }
                if (rbtType.SelectedValue == "1")
                {
                    dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER where INM_CODE=IND_INM_CODE and IND_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and INM_TYPE='OutJWINM' and INM_CODE='" + ddlInvoiceNo.SelectedValue + "' ORDER BY I_NAME");
                }
            }
            if (type == "MOD" || type == "MODIFY" || type == "VIEW")
            {
                DataTable dtType = CommonClasses.Execute("select INSPDI_INSM_CODE,INSPDI_TYPE from INSPECTION_PDI_DETAIL where INSPDI_INSM_CODE=" + Request.QueryString[1] + "");
                if (!Convert.ToBoolean(dtType.Rows[0]["INSPDI_TYPE"]))
                {
                    dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER where INM_CODE=IND_INM_CODE and IND_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and INM_TYPE='TAXINV' and INM_CODE='" + ddlInvoiceNo.SelectedValue + "' ORDER BY I_NAME");
                }
                else
                {
                    dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER where INM_CODE=IND_INM_CODE and IND_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and INM_TYPE='OutJWINM' and INM_CODE='" + ddlInvoiceNo.SelectedValue + "' ORDER BY I_NAME");
                }
            }

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "LoadItem", Ex.Message);
        }
    }
    #endregion

    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtType.SelectedValue == "0")
        {
            LoadInvoiceNo();
            //((DataTable)ViewState["dt2"]).Columns.Clear();
            // ViewState["dt2"] = null;
            dt2.Clear();
            ViewState["dt2"] = dt2;
            LoadFilter();
        }
        if (rbtType.SelectedValue == "1")
        {
            LoadInvoiceNo();
            // ViewState["dt2"] = null;
            dt2.Clear();
            ViewState["dt2"] = dt2;
            LoadFilter();
        }
    }
}
