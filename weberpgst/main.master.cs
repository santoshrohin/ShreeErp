using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["Username"]) || string.IsNullOrEmpty((string)Session["UserCode"]) || string.IsNullOrEmpty((string)Session["UserActivityCode"]))
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                lblUserName.Text = Session["Username"].ToString();
                string CompanyName = Session["CompanyName"].ToString();
                lblfinacial.Text = "FY Year " + Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy").Substring(7, 4) + " - " + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy").ToString().Substring(7, 4) + " / " + CompanyName;
                DataTable dt = CommonClasses.Execute("select UM_NAME from USER_MASTER where UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "'");
                if (dt.Rows.Count > 0)
                {
                    lblUserName.Text = dt.Rows[0]["UM_NAME"].ToString();
                }

                #region Hiding Module Menus

                string masters = "";
                DataTable dtmaster = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 72 + "");
                masters = dtmaster.Rows.Count == 0 ? "0000000" : dtmaster.Rows[0][0].ToString();
                if (masters == "0000000" || masters == "0111111")
                {
                    Masters.Visible = false;
                }

                string Purchase1 = "";
                DataTable dtPurchase = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 73 + "");
                Purchase1 = dtPurchase.Rows.Count == 0 ? "0000000" : dtPurchase.Rows[0][0].ToString();
                if (Purchase1 == "0000000" || Purchase1 == "0111111")
                {
                    Purchase.Visible = false;
                }

                string Production1 = "";
                DataTable dtProduction = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 74 + "");
                Production1 = dtProduction.Rows.Count == 0 ? "0000000" : dtProduction.Rows[0][0].ToString();
                if (Production1 == "0000000" || Production1 == "0111111")
                {
                    Production.Visible = false;
                }

                string Sales = "";
                DataTable dtSalses = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 75 + "");
                Sales = dtSalses.Rows.Count == 0 ? "0000000" : dtSalses.Rows[0][0].ToString();
                if (Sales == "0000000" || Sales == "0111111")
                {
                    Sale.Visible = false;
                }

                string RND = "";
                DataTable dtRnd = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 99 + "");
                RND = dtRnd.Rows.Count == 0 ? "0000000" : dtRnd.Rows[0][0].ToString();
                if (RND == "0000000" || RND == "0111111")
                {
                    RNDQC.Visible = false;
                }

                string Utility1 = "";
                DataTable dtUtility = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 76 + "");
                Utility1 = dtUtility.Rows.Count == 0 ? "0000000" : dtUtility.Rows[0][0].ToString();
                if (Utility1 == "0000000" || Utility1 == "0111111")
                {
                    Utility.Visible = false;
                }

                #region Account
                string Account = "";
                DataTable dtAccount = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 129 + "");
                Account = dtAccount.Rows.Count == 0 ? "0000000" : dtAccount.Rows[0][0].ToString();
                if (Account == "0000000" || Account == "0111111")
                {
                    LiAccount.Visible = false;
                }
                #endregion Account

                string Excise1 = "";
                DataTable dtExcise = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 106 + "");
                Excise1 = dtExcise.Rows.Count == 0 ? "0000000" : dtExcise.Rows[0][0].ToString();
                if (Excise1 == "0000000" || Excise1 == "0111111")
                {
                    Excise.Visible = false;
                }

                string Admin = "";
                DataTable dtAdmin = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + 77 + "");
                Admin = dtAdmin.Rows.Count == 0 ? "0000000" : dtAdmin.Rows[0][0].ToString();
                if (Admin == "0000000" || Admin == "0111111")
                {
                    Adminstrator.Visible = false;
                }
                #endregion
            }
        }
    }
    protected void lnk_logout(object sender, EventArgs e)
    {
        //bool UAUpdate = CommonClasses.Execute1("update SMP_PETRO_USER_ACTIVITY  set SMP_UA_LO_DATE=getdate() where  SMP_UA_CODE='" + Session["UserActivityCode"].ToString() + "'");
        //if (UAUpdate)
        //{
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/Default.aspx", true);
        //}
    }
    //protected void Production_Click(object sender, EventArgs e)
    //{
    //   // Masters.Attributes.Remove("class");
    //    Production.Attributes.Add("class", "active");
    //    //Dashboard_Operations.Attributes.Remove("class");
    //    //Dashboard_Inventory.Attributes.Remove("class");
    //    //Dashboard_Management.Attributes.Remove("class");
    //    Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);

    //}
}
