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

public partial class RoportForms_VIEW_ViewIssueToProductionRegister : System.Web.UI.Page
{
    static string right = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='23'");
            //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            //{
            LoadCombos();
            LoadDept();
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;

            chkAllItem.Checked = true;
            dateCheck();
            chkAlldept.Checked = true;
            ddlDepartment.Enabled = false;

            //}
            //else
            //{
            //    Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            //}
        }
    }
    #endregion

    #region LoadDept
    private void LoadDept()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" SELECT * FROM DEPARTMENT ");
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataTextField = "Dept_Name";
        ddlDepartment.DataValueField = "Dept_Name";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("Select Department Name", "0"));
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {


            DataTable dtItemDet = new DataTable();
            if (rbtType.SelectedIndex == 0)
            {
                dtItemDet = CommonClasses.Execute("Select  DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where IM_TYPE=2 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");

            }
            if (rbtType.SelectedIndex == 1)
            {
                dtItemDet = CommonClasses.Execute("Select DISTINCT IMD_I_CODE,I_CODE,I_NAME,I_CODENO from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where IM_TYPE=1 and ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");

            }
            //else
            //{
            //    dtItemDet = CommonClasses.Execute("Select ISSUE_MASTER.IM_CODE,IM_TYPE,IMD_I_CODE,I_CODE,I_NAME from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER where  ISSUE_MASTER.ES_DELETE=0 and ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE and I_CODE=IMD_I_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I_NAME");

            //}


            //dtItemDet = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE!='-2147483646' ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));


            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            //{
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    return;

                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
                    return;

                }
            }
            string From = "";
            string To = "";
            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }

            string str1 = "";
            string str = "";

            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Direct";
                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                    //if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}

                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    //if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}

                }

            }
            #endregion

            #region AsperReq
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "AsperReq";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";

                    //if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    //if (chkDateAll.Checked == true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked != true && chkAllItem.Checked == true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                    //if (chkDateAll.Checked == true && chkAllItem.Checked != true)
                    //{
                    //    Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&Direct=" + str1 + "", false);

                    //}
                }

            }
            #endregion

            string StrCond = "";
            if (chkDateAll.Checked != true)
            {
                StrCond = StrCond + "  IM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text) + "'  AND '" + Convert.ToDateTime(txtToDate.Text) + "'  AND ";
            }
            if (chkAllItem.Checked != true)
            {
                StrCond = StrCond + "  IMD_I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
            }
            if (chkAlldept.Checked != true)
            {
                StrCond = StrCond + "  IM_REQBY = '" + ddlDepartment.SelectedValue + "'  AND  ";
            }
            Response.Redirect("../../RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "&Cond=" + StrCond + "", false);

            //#region All
            //if (rbtType.SelectedIndex == 2)
            //{
            //    str1 = "All";
            //    if (rbtGroup.SelectedIndex == 0)
            //    {
            //        str = "Datewise";
            //        if (chkDateAll.Checked == true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked == true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }

            //    }
            //    if (rbtGroup.SelectedIndex == 1)
            //    {
            //        str = "ItemWise";

            //        if (chkDateAll.Checked == true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked != true && chkAllItem.Checked == true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }
            //        if (chkDateAll.Checked == true && chkAllItem.Checked != true)
            //        {
            //            Response.Redirect("~/RoportForms/ADD/IssueToProductionRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&All=" + str1 + "", false);

            //        }

            //    }

            //}
            //#endregion





            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "You have no rights to Print";
            //}
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        //if(Convert.ToDateTime(txtFromDate.Text))
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;
        To = txtToDate.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    return;

                }
            }

        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
        }
    }

    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
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
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

        }
        dateCheck();
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion

    #region chkAlldept_CheckedChanged
    protected void chkAlldept_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAlldept.Checked == true)
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDepartment.Enabled = false;

        }
        else
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDepartment.Enabled = true;

        }
    }
    #endregion
}
