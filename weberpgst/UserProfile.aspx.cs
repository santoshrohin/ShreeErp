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
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Timers;


public partial class UserProfile : System.Web.UI.Page
{
    static DataTable dtUserDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            lblmesg.Visible = false;

            dtUserDetail = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_PASSWORD,UM_LEVEL FROM USER_MASTER WHERE UM_CODE=" + Session["UserCode"] + "");
            if (dtUserDetail.Rows.Count > 0)
            {
                if (dtUserDetail.Rows[0]["UM_LEVEL"].ToString() == "Administrator")
                {
                    txtusername.Text = dtUserDetail.Rows[0]["UM_USERNAME"].ToString();
                    UserName.Visible = true;
                   // LoadUser();
                    txtNewPassward.Text = "";
                    txtConfirmPass.Text = "";
                }
                else
                {
                    UserName.Visible = false;
                    txtusername.Text = dtUserDetail.Rows[0]["UM_USERNAME"].ToString();
                    txtNewPassward.Text = "";
                    txtConfirmPass.Text = "";
                }

            }

        }

    }

    public void LoadUser()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select UM_CODE,UM_NAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID='" + (string)Session["CompanyId"] + "'");
                ddlUser.DataSource = dt;
                ddlUser.DataTextField = "UM_NAME";
                ddlUser.DataValueField = "UM_CODE";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new ListItem("Select", "0"));

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Profile", "LoadCountry", Ex.Message);

        }
    }

    protected void btnChangePass_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtUserDetail.Rows[0]["UM_LEVEL"].ToString() == "Administrator")
            {
                if (txtNewPassward.Text == txtConfirmPass.Text)
                {
                    string pass = CommonClasses.Encrypt(txtConfirmPass.Text);
                    bool flag = CommonClasses.Execute1("UPDATE USER_MASTER SET UM_PASSWORD='" + pass + "' WHERE UM_CODE='" + ddlUser.SelectedValue + "' ");
                    if (flag == true)
                    {
                        txtConfirmPass.Text = "";
                        txtNewPassward.Text = "";
                        txtusername.Text = "";

                        lblmesg.Visible = true;
                        lblmesg.Text = "Password Reset Successfull.";
                    }
                }
                else
                {
                    lblmesg.Visible = true;
                    lblmesg.Text = "Password Mismatch. Please Enter Correct Password";
                    txtNewPassward.Focus();
                }
            }
            else
            {
                if (txtNewPassward.Text == txtConfirmPass.Text)
                {
                    string pass = CommonClasses.Encrypt(txtConfirmPass.Text);
                    bool flag = CommonClasses.Execute1("UPDATE USER_MASTER SET UM_PASSWORD='" + pass + "' WHERE UM_CODE='" + Session["UserCode"] + "'");
                    if (flag == true)
                    {
                        txtConfirmPass.Text = "";
                        txtNewPassward.Text = "";
                        txtusername.Text = "";

                        lblmesg.Visible = true;
                        lblmesg.Text = "Password Reset Successfull.";
                    }
                }
                else
                {
                    lblmesg.Visible = true;
                    lblmesg.Text = "Password Mismatch. Please Enter Correct Password";
                    txtNewPassward.Focus();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnRegirct_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {

        }
    }   
}
