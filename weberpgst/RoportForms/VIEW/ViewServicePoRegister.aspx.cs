using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class RoportForms_VIEW_ViewServicePoRegister : System.Web.UI.Page
{
    static string right = "";
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='22'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            LoadType();
            LoadProCode();


            ddlCustomerName.Enabled = false;
            ddlServiceName.Enabled = false;
            ddlServiceCode.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            chkProCodeAll.Checked = true;
            chkAllPOType.Checked = true;
            ddlPOType.Enabled = false;
            ddlProCode.Enabled = false;

            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");

        }

    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    return;
                }
            }

            if (chkAllItem.Checked == false)
            {
                if (ddlServiceName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Service Name ";
                    return;
                }
            }
            if (chkAllCustomer.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Customer ";
                    return;
                }
            }

            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str1 = "";
            string str = "";
            string strCond = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                    if (Convert.ToDateTime(From) < Convert.ToDateTime(Date1) || Convert.ToDateTime(From) > Convert.ToDateTime(Date2) || Convert.ToDateTime(To) < Convert.ToDateTime(Date1) || Convert.ToDateTime(To) > Convert.ToDateTime(Date2))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
                        return;
                    }
                }

            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }
            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "All";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";
                }
                if (rbtGroup.SelectedIndex == 2)
                {
                    str = "CustWise";
                }
            }
            #endregion

            #region Summary
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "Pending";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";
                }
                if (rbtGroup.SelectedIndex == 2)
                {
                    str = "CustWise";
                }
            }
            #endregion

            if (chkDateAll.Checked != true)
            {
                strCond = strCond + " SRPOM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            else
            {
                strCond = strCond + " SRPOM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            if (chkAllItem.Checked != true)
            {
                strCond = strCond + " S_CODE = '" + ddlServiceName.SelectedValue + "'  AND  ";
            }
            if (chkAllCustomer.Checked != true)
            {
                //strCond = strCond + " IWM_P_CODE = '" + ddlCustomerName.SelectedValue + "'  AND  ";
                strCond = strCond + " P_CODE = '" + ddlCustomerName.SelectedValue + "'  AND  ";
            }
            if (chkAllPOType.Checked != true)
            {
                if (ddlPOType.SelectedValue != "0")
                {
                    strCond = strCond + " PO_T_CODE = '" + ddlPOType.SelectedValue + "'  AND  ";
                }
            }
            if (chkProCodeAll.Checked != true)
            {
                if (ddlProCode.SelectedValue != "0")
                {
                    strCond = strCond + " PROCM_CODE = '" + ddlProCode.SelectedValue + "'  AND  ";  //Master name PROJECT_CODE_MASTER
                }
            }


            // Response.Redirect("../ADD/SupplierPORegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlServiceName.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
            Response.Redirect("../ADD/ServicePORegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "", false);

        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception)
        {

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
            CommonClasses.SendError("Supplier Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;

            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlServiceName.SelectedIndex = 0;
            ddlServiceName.Enabled = false;
            ddlServiceCode.SelectedIndex = 0;
            ddlServiceCode.Enabled = false;
        }
        else
        {
            ddlServiceName.SelectedIndex = 0;
            ddlServiceName.Enabled = true;
            ddlServiceName.Focus();
            ddlServiceCode.SelectedIndex = 0;
            ddlServiceCode.Enabled = true;
        }
    }
    #endregion

    #region chkAllPOType_CheckedChanged
    protected void chkAllPOType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllPOType.Checked == true)
        {
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = false;
        }
        else
        {
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = true;
            ddlPOType.Focus();
        }
    }
    #endregion

    #region chkProCodeAll_CheckedChanged
    protected void chkProCodeAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkProCodeAll.Checked == true)
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = false;
        }
        else
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = true;
            ddlProCode.Focus();
        }
    }
    #endregion

    #region LoadType
    private void LoadType()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("select distinct(PO_T_CODE) ,PO_T_DESC from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by PO_T_DESC");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_DESC";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Purchase Order", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
            ddlProCode.DataSource = dt;
            ddlProCode.DataTextField = "PROCM_NAME";
            ddlProCode.DataValueField = "PROCM_CODE";
            ddlProCode.DataBind();
            ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadItem();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion



    #region LoadCombos
    private void LoadCombos()
    {

        //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
        dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY S_NAME");
        ddlServiceName.DataSource = dt;
        ddlServiceName.DataTextField = "S_NAME";
        ddlServiceName.DataValueField = "S_CODE";
        ddlServiceName.DataBind();
        ddlServiceName.Items.Insert(0, new ListItem("Select Service Name", "0"));

        //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO");
        dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY S_NAME");
        ddlServiceCode.DataSource = dt;
        ddlServiceCode.DataTextField = "S_CODENO";
        ddlServiceCode.DataValueField = "S_CODE";
        ddlServiceCode.DataBind();
        ddlServiceCode.Items.Insert(0, new ListItem("Select Service Code ", "0"));

        //dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SUPP_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0  AND  SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SERVICE_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0  AND  SRPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion

    #region LoadParty
    private void LoadParty()
    {
        dt = CommonClasses.Execute(" select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SUPP_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0  AND  SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion

    #region LoadItem
    private void LoadItem()
    {
        //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
        dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY S_NAME");
        ddlServiceName.DataSource = dt;
        ddlServiceName.DataTextField = "S_NAME";
        ddlServiceName.DataValueField = "S_CODE";
        ddlServiceName.DataBind();
        ddlServiceName.Items.Insert(0, new ListItem("Select Service Name", "0"));

        //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY I_CODENO");
        dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY S_NAME");
        ddlServiceCode.DataSource = dt;
        ddlServiceCode.DataTextField = "S_CODENO";
        ddlServiceCode.DataValueField = "S_CODE";
        ddlServiceCode.DataBind();
        ddlServiceCode.Items.Insert(0, new ListItem("Select Service Code", "0"));
    }

    #endregion



    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedValue != "0")
            {
                //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'  ORDER BY I_NAME");
                dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SRPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY S_NAME");
                ddlServiceName.DataSource = dt;
                ddlServiceName.DataTextField = "S_NAME";
                ddlServiceName.DataValueField = "S_CODE";
                ddlServiceName.DataBind();
                ddlServiceName.Items.Insert(0, new ListItem("Select Service Name", "0"));

                //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY I_CODENO");
                dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SRPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY S_NAME");
                ddlServiceCode.DataSource = dt;
                ddlServiceCode.DataTextField = "S_CODENO";
                ddlServiceCode.DataValueField = "S_CODE";
                ddlServiceCode.DataBind();
                ddlServiceCode.Items.Insert(0, new ListItem("Select Service Code ", "0"));

                //dt = CommonClasses.Execute("SELECT DISTINCT(PO_T_CODE) ,PO_T_DESC FROM PO_TYPE_MASTER,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0  AND PO_TYPE_MASTER.ES_DELETE=0 AND SPOM_TYPE=PO_T_CODE  and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY PO_T_DESC");
                //ddlServiceCode.DataSource = dt;
                //ddlServiceCode.DataTextField = "PO_T_DESC";
                //ddlServiceCode.DataValueField = "PO_T_CODE";
                //ddlServiceCode.DataBind();
                //ddlServiceCode.Items.Insert(0, new ListItem("Select Purchase Order", "0"));

                dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
                ddlProCode.DataSource = dt;
                ddlProCode.DataTextField = "PROCM_NAME";
                ddlProCode.DataValueField = "PROCM_CODE";
                ddlProCode.DataBind();
                ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
            }
            else
            {
                //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
                dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY S_NAME");
                ddlServiceName.DataSource = dt;
                ddlServiceName.DataTextField = "S_NAME";
                ddlServiceName.DataValueField = "S_CODE";
                ddlServiceName.DataBind();
                ddlServiceName.Items.Insert(0, new ListItem("Select Service Name", "0"));

                //dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY I_CODENO");
                dt = CommonClasses.Execute("SELECT DISTINCT S_CODE,S_NAME,S_CODENO FROM SERVICE_TYPE_MASTER,SERVICE_PO_MASTER,SERVICE_PO_DETAILS WHERE SERVICE_TYPE_MASTER.ES_DELETE=0 AND   SERVICE_PO_MASTER.ES_DELETE=0 AND SRPOM_CODE=SRPOD_SPOM_CODE AND  SRPOD_I_CODE=S_CODE  and S_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY S_NAME");
                ddlServiceCode.DataSource = dt;
                ddlServiceCode.DataTextField = "S_CODENO";
                ddlServiceCode.DataValueField = "S_CODE";
                ddlServiceCode.DataBind();
                ddlServiceCode.Items.Insert(0, new ListItem("Select Service Code", "0"));

                LoadType();
                LoadProCode();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlServiceCode_SelectedIndexChanged
    protected void ddlServiceCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlServiceName.SelectedValue = ddlServiceCode.SelectedValue;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlServiceCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlServiceName_SelectedIndexChanged
    protected void ddlServiceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlServiceCode.SelectedValue = ddlServiceName.SelectedValue;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlServiceName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    private void POtype(string ICode)
    {
        dt = CommonClasses.Execute("SELECT DISTINCT(PO_T_CODE) ,PO_T_DESC FROM PO_TYPE_MASTER,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0  AND PO_TYPE_MASTER.ES_DELETE=0 AND SPOM_TYPE=PO_T_CODE  and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY PO_T_DESC");
        ddlServiceCode.DataSource = dt;
        ddlServiceCode.DataTextField = "PO_T_DESC";
        ddlServiceCode.DataValueField = "PO_T_CODE";
        ddlServiceCode.DataBind();
        ddlServiceCode.Items.Insert(0, new ListItem("Select Purchase Order", "0"));

        dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
        ddlProCode.DataSource = dt;
        ddlProCode.DataTextField = "PROCM_NAME";
        ddlProCode.DataValueField = "PROCM_CODE";
        ddlProCode.DataBind();
        ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
    }
}
