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

public partial class RoportForms_ADD_ProjcodeWiseMIS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Popup
    
 
    

    #region btnConfirm_Click
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            CommonClasses.WriteLogActivity("Dashboard", "Confirm", "Dashboard", Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Dashboard", "btnConfirm_Click", Ex.Message);
        }
    }
    #endregion

    protected void dgActivity_Task_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.Equals("Modify"))
        //{
        //    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='14'");
        //    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();

        //    if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
        //    {
        //        if (!ModifyLog(e.CommandArgument.ToString()))
        //        {
        //            string type = "MODIFY";
        //            string ce_code = e.CommandArgument.ToString();
        //            DataTable dt = CommonClasses.Execute("SELECT ACTIVITY_TYPE_MASTER.ACT_TYPE_IS_AUTOPOST,ACTIVITY_DETAIL.ACT_TYPE FROM ACTIVITY_DETAIL INNER JOIN ACTIVITY_TYPE_MASTER ON ACTIVITY_DETAIL.ACT_TYPE = ACTIVITY_TYPE_MASTER.ACT_TYPE_CODE WHERE (ACTIVITY_DETAIL.ACT_CODE =" + ce_code + ") AND (ACTIVITY_DETAIL.ES_DELETE = 0) AND (ACTIVITY_TYPE_MASTER.ES_DELETE = 0)");
        //            if (dt.Rows.Count > 0)
        //            {
        //                int Autopost = Convert.ToInt32(dt.Rows[0]["ACT_TYPE_IS_AUTOPOST"]);
        //                if (Autopost == 1)
        //                {
        //                    // Response.Write("<script>alert('You can't modify the Autopost Entry')</script>");
        //                    //return;
        //                    PnlMsgDailyAct.Visible = true;
        //                    lblDailyActMsg.Text = "You can't modify the Autopost Entry";
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //                    LoadActivity();
        //                    popUpPanel5.Visible = true;
        //                    ModalCancleConfirmation.Show();
        //                    return;
        //                }
        //                else
        //                {
        //                    Response.Redirect("~/Transactions/ADD/ActivityForm.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Response.Write("<script>alert('Record used by another person')</script>");
        //            // return;
        //            PnlMsgDailyAct.Visible = true;
        //            lblDailyActMsg.Text = "record used by another person";
        //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //            LoadActivity();
        //            popUpPanel5.Visible = true;
        //            ModalCancleConfirmation.Show();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        PnlMsgDailyAct.Visible = true;
        //        lblDailyActMsg.Text = "You have no rights to modify";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //        LoadActivity();
        //        popUpPanel5.Visible = true;
        //        ModalCancleConfirmation.Show();
        //        return;
        //    }
        //}
    }
    #endregion
}
