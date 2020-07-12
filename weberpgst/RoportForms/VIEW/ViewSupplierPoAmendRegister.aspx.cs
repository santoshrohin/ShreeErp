using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class RoportForms_VIEW_ViewSupplierPoAmendRegister : System.Web.UI.Page
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
            ddlCustomerName.Enabled = false;
           // ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            //chkAllItem.Checked = true;

        }

    }
    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if(chkDateAll.Checked==false)
            {
                if(txtFromDate.Text=="" || txtToDate.Text=="")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    return;
                }
            }

            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex==0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Item ";
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

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                    if (Convert.ToDateTime(From) < Convert.ToDateTime(Date1) || Convert.ToDateTime(From) > Convert.ToDateTime(Date2) || Convert.ToDateTime(To) < Convert.ToDateTime(Date1) || Convert.ToDateTime(To) > Convert.ToDateTime(Date2))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        return;
                    }
                    //else if (Date1 > Date2)
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                    //    return;

                    //}
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
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                }
                //if (rbtGroup.SelectedIndex == 1)
                //{
                //    str = "ItemWise";

                //    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }
                //    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                //    {
                //        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                //    }

                //}
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "CustWise";


                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);
                    }

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

                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }

                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "ItemWise";

                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                }
                if (rbtGroup.SelectedIndex == 2)
                {
                    str = "CustWise";

                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked == true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllCustomer.Checked != true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked == true && chkAllItem.Checked != true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllCustomer.Checked != true && chkAllItem.Checked == true)
                    {
                        Response.Redirect("../ADD/SupplierPOAmendRegister.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&All_CUST=" + chkAllCustomer.Checked.ToString() + "&ALL_ITEM=" + chkAllItem.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&p_name=" + ddlCustomerName.SelectedValue.ToString() + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&datewise=" + str + "&detail=" + str1 + "", false);

                    }
                }
            }
            #endregion

           
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
            CommonClasses.SendError("Supplier Order Amendment Register", "ShowMessage", Ex.Message);
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

            txtFromDate.Attributes.Add("readonly","readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
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
        }
        else
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
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
        dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
        ddlFinishedComponent.DataSource = dt;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item ", "0"));


        dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion
}
