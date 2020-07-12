using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class RoportForms_VIEW_ViewLogMaster : System.Web.UI.Page
{
   
    static string right = "";
    DataTable dt = new DataTable();

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            LoadCombos();
            ddlModule.Enabled = false;
            ddlUser.Enabled = false;
           // ddlActivityType.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;

            chkDateAll.Checked = true;
            chkAllModule.Checked = true;
            chkAllUser.Checked = true;
            //chkAllActivityType.Checked = true;

        }

    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {

        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
            //{

            if (chkDateAll.Checked == false)
            {
                if(txtFromDate.Text=="" || txtToDate.Text=="")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field Date is required";
                    return;
                }
            }
            if(chkAllUser.Checked==false)
            {
                if(ddlUser.SelectedIndex==0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select User Name";
                    return;
                }
            }
            if (chkAllModule.Checked == false)
            {
                if (ddlModule.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Module";
                    return;
                }
            }

            string From = "";
            string To = "";
            //DateTime From;
            //DateTime To;
            From = txtFromDate.Text;
            To = txtToDate.Text;
            //From = Convert.ToDateTime(txtFromDate.Text);
            //To = Convert.ToDateTime(txtToDate.Text);
                string str1 = "";
                string str = "";

                if (chkDateAll.Checked == false)
                {
                    if (From != "" && To != "")
                    {
                        
                        DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                        DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                        //if (Convert.ToDateTime(From) < Convert.ToDateTime(Date1) || Convert.ToDateTime(From) > Convert.ToDateTime(Date2) || Convert.ToDateTime(To) < Convert.ToDateTime(Date1) || Convert.ToDateTime(To) > Convert.ToDateTime(Date2))
                        //{
                        //    PanelMsg.Visible = true;
                        //    lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        //    return;
                        //}
                        if (Date1 > Date2)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
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

                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "DateWise";

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }


                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                }
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "ModuleWise";

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }


                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                }
                if (rbtType.SelectedIndex == 2)
                {
                    str1 = "UserWise";

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked == true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }


                    if (chkDateAll.Checked == true && chkAllUser.Checked != true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                    if (chkDateAll.Checked != true && chkAllUser.Checked != true && chkAllModule.Checked == true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }

                    if (chkDateAll.Checked != true && chkAllUser.Checked == true && chkAllModule.Checked != true)
                    {
                        Response.Redirect("../ADD/LogMaster.aspx?Title=" + Title + "&ALL_DATE=" + chkDateAll.Checked.ToString() + "&ALL_USER=" + chkAllUser.Checked.ToString() + "&ALL_MODULE=" + chkAllModule.Checked.ToString() + " &FromDate=" + From + "&ToDate=" + To + "&U_name=" + ddlUser.SelectedValue.ToString() + " &M_name=" + ddlModule.SelectedValue.ToString() + " &datewise=" + str1 + "", false);

                    }
                }
                


            //}
            //else
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "You Have No Rights To View";
            //}



        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        dt = CommonClasses.Execute("select distinct(LG_SOURCE) as LG_SOURCE  from LOG_MASTER where LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY LG_SOURCE");
        ddlModule.DataSource = dt;
        ddlModule.DataTextField = "LG_SOURCE";
        //ddlModule.DataValueField = "LG_CODE";
        ddlModule.DataBind();
        ddlModule.Items.Insert(0, new ListItem("Select Module ", "0"));


        dt = CommonClasses.Execute("select UM_CODE,UM_USERNAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID=" + (string)Session["CompanyId"] + "");
        ddlUser.DataSource = dt;
        ddlUser.DataTextField = "UM_USERNAME";
        ddlUser.DataValueField = "UM_CODE";
        ddlUser.DataBind();
        ddlUser.Items.Insert(0, new ListItem("Select User", "0"));


        //dt = CommonClasses.Execute("select distinct(LG_EVENT) as LG_EVENT from LOG_MASTER where  LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "");
        //ddlActivityType.DataSource = dt;
        //ddlActivityType.DataTextField = "LG_EVENT";
        ////ddlActivityType.DataValueField = "LG_CODE";
        //ddlActivityType.DataBind();
        //ddlActivityType.Items.Insert(0, new ListItem("Select Activity Type", "0"));


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
        }
    }
    #endregion 

    #region chkAllModule_CheckedChanged
    protected void chkAllModule_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllModule.Checked == true)
        {
            ddlModule.SelectedIndex = 0;
            ddlModule.Enabled = false;
        }
        else
        {
            ddlModule.SelectedIndex = 0;
            ddlModule.Enabled = true;
            ddlModule.Focus();
        }
    }
    
    #endregion

    #region chkAllUser_CheckedChanged
    protected void chkAllUser_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllUser.Checked == true)
        {
            ddlUser.SelectedIndex = 0;
            ddlUser.Enabled = false;
        }
        else
        {
            ddlUser.SelectedIndex = 0;
            ddlUser.Enabled = true;
            ddlUser.Focus();
        }

    }
    #endregion 

    //protected void chkAllActivityType_CheckedChanged(object sender, EventArgs e)
    //{

    //    if (chkAllActivityType.Checked == true)
    //    {
    //        ddlActivityType.SelectedIndex = 0;
    //        ddlActivityType.Enabled = false;
    //    }
    //    else
    //    {
    //        ddlActivityType.SelectedIndex = 0;
    //        ddlActivityType.Enabled = true;
    //        ddlActivityType.Focus();
    //    }
    //}

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        try
        {
            //if (CheckValid())
            //{
            //    ModalPopupPrintSelection.TargetControlID = "btnCancel";
            //    ModalPopupPrintSelection.Show();
            //    popUpPanel5.Visible = true;
            //}
            //else
            //{
            //    CancelRecord();

            //}
            Response.Redirect("~/Admin/Default.aspx", false);

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Log Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
       // ShowRecord();
        CancelRecord();
    }
    #endregion 

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion 

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
          
                flag = true;
           

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Log Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion 






}
