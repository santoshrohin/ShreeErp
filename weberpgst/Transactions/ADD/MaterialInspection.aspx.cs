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

public partial class Transactions_ADD_MaterialInspection : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int InwardNo = 0;
    static string ItemUpdateIndex = "-1";
    MaterialInspection_BL inspect_BL = new MaterialInspection_BL();
    static DataTable inspectiondt = new DataTable();
    static DataTable dt = new DataTable();
    static string type = "";
    DataTable dtFilter = new DataTable();
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static string str = "";
    public int icode;
    static string fileName2 = "";
    DirectoryInfo ObjSearchDir;
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
                    ViewState["InwardNo"] = null;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["fileName2"] = fileName2;
                    ViewState["mlCode"] = mlCode;
                    ((DataTable)ViewState["dt2"]).Clear();

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        InwardNo = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["InwardNo"] = InwardNo;
                        ViewRec("VIEW");
                        type = "VIEW";
                        lblType.Text = "Inspected List";
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        InwardNo = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["InwardNo"] = InwardNo;

                        ViewRec("MOD");
                        LoadFilter();//new Suja
                        type = "MOD";
                        lblType.Text = "Inspected List";
                    }
                    else if (Request.QueryString[0].Equals("ADD"))
                    {
                        txtInspDate.Attributes.Add("readonly", "readonly");
                        InwardNo = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["InwardNo"] = InwardNo;
                        ViewRec("ADD");
                        type = "ADD";
                        LoadFilter();//new Suja
                        lblType.Text = "Pending List";
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-ADD", "Page_Load", Ex.Message);
        }
        if (IsPostBack && FileUpload1.PostedFile != null)
        {
            if (FileUpload1.PostedFile.FileName.Length > 0)
            {
                fileName2 = FileUpload1.PostedFile.FileName;
                ViewState["fileName2"] = fileName2;
                Upload2(null, null);
            }
        }
    }
    #endregion

    #region Upload2
    protected void Upload2(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("ADD"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/Inspection/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("ADD"))
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Inspection/" + ViewState["fileName2"].ToString()));
            }
            else
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
            }
            lnkTModel.Visible = true;
            lnkTModel.Text = ViewState["fileName2"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtInspDate.Attributes.Add("readonly", "readonly");
            if (str == "VIEW")
            {
                inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"]) + "' ");
                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = true;
                    dgInspection.Columns[1].Visible = false;
                    dgInspection.Columns[2].Visible = false;
                    dgInspection.Columns[3].Visible = false;
                    dgInspection.Columns[4].Visible = false;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }


                //btnSubmit.Visible = false;
                // dgInspection.Columns[0].Visible = true;
                //dgInspection.Columns[1].Visible = false;
                //dgInspection.Columns[2].Visible = false;
                //dgInspection.Columns[3].Visible = false;

            }
            else if (str == "ADD")
            {
                //txtInspDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                txtInspDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy"); 
                inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"]) + "' ");
                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = false;
                    dgInspection.Columns[1].Visible = false;
                    dgInspection.Columns[2].Visible = true;
                    dgInspection.Columns[3].Visible = false;
                    dgInspection.Columns[4].Visible = false;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }
            else if (str == "MOD")
            {
                inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"]) + "' ");
                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = true;
                    dgInspection.Columns[1].Visible = true;
                    dgInspection.Columns[2].Visible = false;
                    dgInspection.Columns[3].Visible = true;
                    dgInspection.Columns[4].Visible = true;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }

                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region getInfo
    private bool getInfo(string str)
    {
        bool flag = false;
        try
        {
            //DataTable dt = new DataTable();

            if (str == "VIEW" || str == "MOD")
            {
                dt = CommonClasses.Execute("select I_CODENO,INSM_CODE,INSM_REMARK,ISNULL(INSM_FILE,'') AS INSM_FILE,convert(varchar,INSM_DATE,106) as INSM_DATE,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,ISNULL(INSM_TC_CHECK,0) AS INSM_TC_CHECK,INSM_TC_NO,INSM_PDR_NO,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE=0 AND INSM_IWM_CODE=IWM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 AND IWD_I_CODE=INSM_I_CODE  and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWD_I_CODE='" + icode + "' and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"]) + "' ");
                if (dt.Rows.Count > 0)
                {
                    ViewState["mlCode"] = dt.Rows[0]["INSM_CODE"].ToString();
                    lblGRNCODE.Text = dt.Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = dt.Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = dt.Rows[0]["I_NAME"].ToString();
                    txtInspDate.Text = Convert.ToDateTime(dt.Rows[0]["INSM_DATE"].ToString()).ToString("dd MMM yyyy");
                    txtrecQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                    txtUnitName.Text = dt.Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = dt.Rows[0]["IWD_CON_OK_QTY"].ToString();
                    txtRejQty.Text = dt.Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = dt.Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRemark.Text = dt.Rows[0]["INSM_REMARK"].ToString();
                    txtPDRNo.Text = dt.Rows[0]["INSM_PDR_NO"].ToString();
                    chkPDR.Checked = Convert.ToBoolean(dt.Rows[0]["INSM_PDR_CHECK"].ToString());
                    txtRate.Text = dt.Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = dt.Rows[0]["IWM_P_CODE"].ToString();
                    txtItemCode.Text = dt.Rows[0]["I_CODENO"].ToString();
                    txtTcNo.Text = dt.Rows[0]["INSM_TC_NO"].ToString();
                    chkTcNo.Checked = Convert.ToBoolean(dt.Rows[0]["INSM_TC_CHECK"].ToString());
                    lnkTModel.Text = dt.Rows[0]["INSM_FILE"].ToString();
                    if (chkPDR.Checked == true)
                    {
                        txtPDRNo.Enabled = true;
                    }
                    if (Convert.ToDouble(txtRejQty.Text) > 0)
                    {
                        DataTable dtReason = CommonClasses.Execute("select INSD_RM_CODE as Reason from INSPECTION_S_DETAIL where INSD_INSM_CODE='" + dt.Rows[0]["INSM_CODE"].ToString() + "' and INSD_I_CODE='" + icode + "'");
                        if (dtReason.Rows.Count > 0)
                        {
                            txtReason.Text = dtReason.Rows[0]["Reason"].ToString();
                        }
                    }
                }

                DataTable dtPDIDetail = CommonClasses.Execute("select INSPDI_PARAMETERS,INSPDI_SPECIFTION,INSM_FILE AS DocName,INSPDI_INSPECTION,INSPDI_OBSR1,INSPDI_OBSR2,INSPDI_OBSR3,INSPDI_OBSR4,INSPDI_OBSR5,INSPDI_DSPOSITION,INSPDI_REMARK,INSPDI_I_CODE from INSPECTION_PDI_DETAIL INNER JOIN INSPECTION_S_MASTER ON INSPDI_INSM_CODE=INSM_CODE where INSPECTION_PDI_DETAIL.ES_DELETE=0 AND INSPECTION_S_MASTER.ES_DELETE=0 AND INSPDI_INSM_CODE='" + dt.Rows[0]["INSM_CODE"] + "'");

                if (dgPDIDEtail.Rows.Count != 0)
                {
                    dgPDIDEtail.DataSource = dtPDIDetail;
                    dgPDIDEtail.DataBind();
                    ViewState["dt2"] = dtPDIDetail;
                    dgPDIDEtail.Enabled = true;
                }
                else
                {
                    LoadFilter();
                }
            }

            else if (str == "ADD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE  from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE  AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"]) + "' ");
                if (dt.Rows.Count > 0)
                {
                    lblGRNCODE.Text = dt.Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = dt.Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = dt.Rows[0]["I_NAME"].ToString();
                    txtItemCode.Text = dt.Rows[0]["I_CODENO"].ToString();

                    txtrecQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();

                    txtUnitName.Text = dt.Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                    txtRejQty.Text = dt.Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = dt.Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRate.Text = dt.Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = dt.Rows[0]["IWM_P_CODE"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "getInfo", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {

            inspect_BL.INSM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            inspect_BL.INSM_NO = Convert.ToInt32(txtInspNo.Text);
            inspect_BL.INSM_DATE = Convert.ToDateTime(txtInspDate.Text);

            //inspect_BL.INSM_IWM_CODE = Convert.ToInt32(mlCode);
            //inspect_BL.INSM_SPOM_CODE = Convert.ToInt32(txtPONumber.Text);
            //inspect_BL.INSM_I_CODE = Convert.ToInt32(txtItemName.Text);
            //inspect_BL.INSM_UOM_CODE = Convert.ToInt32(txtUnitName.Text);
            inspect_BL.INSM_RECEIVED_QTY = float.Parse(txtrecQty.Text == "" ? "0" : txtrecQty.Text);
            inspect_BL.INSM_OK_QTY = float.Parse(txtOkQty.Text == "" ? "0" : txtOkQty.Text);
            inspect_BL.INSM_REJ_QTY = float.Parse(txtRejQty.Text == "" ? "0" : txtRejQty.Text);
            inspect_BL.INSM_SCRAP_QTY = float.Parse(txtScrapQty.Text == "" ? "0" : txtScrapQty.Text);
            inspect_BL.INSM_REMARK = txtRemark.Text;
            inspect_BL.INSM_RATE = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
            inspect_BL.INSM_PDR_CHECK = chkPDR.Checked;
            inspect_BL.INSM_PDR_NO = txtPDRNo.Text.Trim();
            inspect_BL.INSM_TC_CHECK = chkTcNo.Checked;
            inspect_BL.INSM_TC_NO = txtTcNo.Text.Trim();
            inspect_BL.INSM_FILE = lnkTModel.Text;
            //New Suja
            //inspect_BL.INSPDI_PARAMETERS = txtparameter.Text;
            //inspect_BL.INSPDI_SPECIFTION = txtSpecification.Text;
            //inspect_BL.INSPDI_INSPECTION = txtInspection.Text;
            //inspect_BL.INSPDI_OBSR1 = txtObservation1.Text;
            //inspect_BL.INSPDI_OBSR2 = txtObservation2.Text;
            //inspect_BL.INSPDI_OBSR3 = txtObservation3.Text;
            //inspect_BL.INSPDI_OBSR4 = txtObservation4.Text;
            //inspect_BL.INSPDI_OBSR5 = txtObservation5.Text;
            //inspect_BL.INSPDI_DSPOSITION = txtDisposition.Text;
            //inspect_BL.INSPDI_REMARK = txtRemarkPDI.Text;

            DataTable dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + lblGRNCODE.Text + "'");
            string INWARD_NO = dtParty.Rows[0]["IWM_NO"].ToString();
            //if (dtParty.Rows.Count > 0)
            //{
            //    if (dtParty.Rows[0]["P_STM_CODE"].ToString() == "-2147483648")
            //    {
            //        inspect_BL.INSM_TYPE = "IWIM";
            //    }
            //    else
            //    {
            //        inspect_BL.INSM_TYPE = "IWIAP";
            //    }
            //}
            if (dtParty.Rows.Count > 0)
            {
                if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                {
                    inspect_BL.INSM_TYPE = "IWIAP";
                }
                else
                {
                    inspect_BL.INSM_TYPE = "IWIM";
                }
            }
            if (inspect_BL.INSM_REJ_QTY > 0)
            {
                inspect_BL.INSD_RM_CODE = txtReason.Text;
            }
            if (dt.Rows.Count > 0)
            {
                if (type == "MOD")
                {
                    inspect_BL.INSM_CODE = Convert.ToInt32(dt.Rows[0]["INSM_CODE"].ToString());
                }
                inspect_BL.INSM_IWM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_IWM_CODE"].ToString());
                inspect_BL.INSM_SPOM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_CPOM_CODE"].ToString());
                inspect_BL.INSM_I_CODE = Convert.ToInt32(dt.Rows[0]["IWD_I_CODE"].ToString());
                inspect_BL.INSM_UOM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_UOM_CODE"].ToString());
                res = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "Setvalues", Ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (dgPDIDEtail.Rows.Count > 0 && dgPDIDEtail.Enabled == true)
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Insert atleast one entry in grid";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return result;
            }


            if (type == "ADD")
            {
                if (Setvalues())
                {
                    string msg = "";
                    if (inspect_BL.Save(out mlCode, dgPDIDEtail))
                    {
                        ViewState["mlCode"] = mlCode;
                        #region FileUpload for 3D Model
                        if (inspect_BL.INSM_FILE != "")
                        {
                            string sDirPath13 = Server.MapPath(@"~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath13);

                            string sDirPath12 = Server.MapPath(@"~/UpLoadPath/Inspection/");
                            DirectoryInfo dir1 = new DirectoryInfo(sDirPath13);

                            dir1.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\Inspection ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/Inspection/" + ViewState["fileName2"]);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"]);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion

                        CommonClasses.WriteLog("Material Inspection", "Save", "Material Inspection", "", Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        //type = "";
                        ViewState["mlCode"] = "0";
                        result = true;
                        ClearTextBoxes(panelDetail);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Saved Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        panelDetail.Visible = false;
                        panelInspection.Visible = true;
                        ViewRec("ADD");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            else if (type == "MOD")
            {
                //BL_SuppPo = new SupplierPOMaster_BL(mlcode);
                if (Setvalues())
                {
                    string msg = "";
                    if (inspect_BL.Update(out mlCode, dgPDIDEtail))
                    {
                        ViewState["mlCode"] = mlCode;
                        CommonClasses.WriteLog("Material Inspection", "Update", "Material Inspection", "", Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        //type = "";
                        ViewState["mlCode"] = "0";
                        result = true;
                        ClearTextBoxes(panelDetail);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Saved Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        panelDetail.Visible = false;
                        panelInspection.Visible = true;
                        ViewRec("MOD");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "SaveRec", Ex.Message);
        }
        return result;
    }
    #endregion

    #region ClearFunction
    private void ClearTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Text = String.Empty;

                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;

                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;

                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection -ADD", "ClearTextBoxes", Ex.Message);
        }
    }
    #endregion

    #region ViewInspection
    private void ViewInspection(string str, string item_code)
    {
        try
        {
            if (str == "VIEW")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);

                getInfo("VIEW");

                txtInspDate.Enabled = false;
                txtRejQty.Enabled = false;
                txtScrapQty.Enabled = false;
                txtRemark.Enabled = false;
                txtReason.Enabled = false;
                txtPDRNo.Enabled = false;
                chkPDR.Enabled = false;
            }
            else if (str == "MOD")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);
                getInfo("MOD");

            }
            else if (str == "ADD")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                chkPDR.Checked = false;  // not compulsory
                txtPDRNo.Enabled = true; // not compulsory
                chkTcNo.Checked = false;
                txtTcNo.Enabled = true;
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);
                getInfo("ADD");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region Numbering
    int Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(INSM_NO) as INSM_NO from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
        if (dt.Rows[0]["INSM_NO"] == null || dt.Rows[0]["INSM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["INSM_NO"]) + 1;
        }
        return GenGINNO;
    }
    #endregion

    #region Event

    #region GridEvent

    #region dgMainPO_Deleting
    protected void dgMainPO_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainPO_SelectedIndexChanged
    protected void dgMainPO_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region dgMainPO_RowCommand
    protected void dgMainPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    #endregion

    #region dgInspection_RowCommand
    protected void dgInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection("VIEW", cpom_code);
            }
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection(type, cpom_code);
            }
            #endregion Modify

            #region Delete
            else if (e.CommandName.Equals("Delete"))
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                string item_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                string insp_no = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_INSP_NO"))).Text;
                string p_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_P_CODE"))).Text;
                DataTable dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + cpom_code + "'");
                string INWARD_NO = dtParty.Rows[0]["IWM_NO"].ToString();
                string Type = "";
                if (dtParty.Rows.Count > 0)
                {
                    if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                    {
                        Type = "IWIAP";
                    }
                    else
                    {
                        Type = "IWIM";
                    }
                }
                if (type == "MOD")
                {
                    if (CommonClasses.CheckUsedInTran("INWARD_DETAIL,INWARD_MASTER", "IWD_IWM_CODE", "AND IWD_I_CODE='" + item_code + "' and IWD_BILL_PASS_FLG='1' and  IWD_IWM_CODE=IWM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Bill Passing";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                        //ShowMessage("#Avisos", "You cant delete this record it has used in Item Master", CommonClasses.MSG_Warning);
                    }
                    else if (CommonClasses.Execute1("update INWARD_DETAIL set IWD_CON_OK_QTY=0,IWD_CON_REJ_QTY=0,IWD_CON_SCRAP_QTY=0,IWD_INSP_NO='',IWD_INSP_FLG=0 where IWD_IWM_CODE='" + cpom_code + "' and IWD_I_CODE='" + item_code + "' "))
                    {
                        if (CommonClasses.Execute1("update INSPECTION_S_MASTER set ES_DELETE=1 where INSM_NO='" + insp_no + "'"))
                        {
                            DataTable dtinsecption = new DataTable();
                            if (Type == "IWIAP")
                            {
                                dtinsecption = CommonClasses.Execute("SELECT INSM_OK_QTY+INSM_REJ_QTY AS INSM_OK_QTY FROM INSPECTION_S_MASTER where    INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "'  ");

                            }
                            else
                            {
                                dtinsecption = CommonClasses.Execute("SELECT INSM_OK_QTY AS INSM_OK_QTY FROM INSPECTION_S_MASTER where INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "'  ");

                            }
                            for (int k = 0; k < dtinsecption.Rows.Count; k++)
                            {
                                CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtinsecption.Rows[k]["INSM_OK_QTY"] + " where  I_CODE='" + item_code + "'");
                            }
                            CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_I_CODE='" + item_code + "'   AND STL_DOC_NO='" + cpom_code + "'     AND STL_DOC_NUMBER='" + INWARD_NO + "' AND STL_DOC_TYPE='" + Type + "'");
                            if (Type == "IWIAP")
                            {
                                CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_I_CODE='" + item_code + "' AND STL_DOC_NO='" + cpom_code + "'  AND STL_DOC_NUMBER='" + INWARD_NO + "' AND STL_DOC_TYPE='SubContractorRejection'");
                            }
                            //
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            ViewRec(type);
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Deleted..";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "These Is Not Inspected Record";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
            #endregion Delete

            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                int Type = 0;
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;//INSPDI_INSM_CODE
                string IWM_CODE = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                Response.Redirect("~/RoportForms/ADD/PDIInspectionPrint.aspx?Type=" + Type + "&inv_code=" + cpom_code + "&IWM_CODE=" + IWM_CODE, false);
            }
            #endregion Print


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection-ADD", "dgInspection_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInspection_RowDeleting
    protected void dgInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgInspection_PageIndexChanging
    protected void dgInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInspection.PageIndex = e.NewPageIndex;

        }
        catch (Exception)
        {
        }
    }
    #endregion

    #endregion

    #region ButtonEvent

    #region btnSave_Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //    if (double.Parse(txtOkQty.Text) ==0)
            //    {
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "Ok Quantity is not 0";
            //        return;
            //    }
            double RecQty = double.Parse(txtOkQty.Text) + double.Parse(txtRejQty.Text) + double.Parse(txtScrapQty.Text);
            if (double.Parse(txtrecQty.Text) != RecQty)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Check Rejected & Ok Quantity";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if ((double.Parse(txtRejQty.Text) > 0.000 && txtReason.Text == "") || (double.Parse(txtScrapQty.Text) > 0.000 && txtReason.Text == ""))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter The Reason";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReason.Focus();
                return;
            }
            //if (chkPDR.Checked == true && txtPDRNo.Text.Trim() == "")
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter PDR No.";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtPDRNo.Focus();
            //    return;
            //}

            //if (chkTcNo.Checked == true && txtTcNo.Text.Trim() == "")
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter TC No.";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtTcNo.Focus();
            //    return;
            //}
            //if (chkPDR.Checked == true && dgPDIDEtail.Enabled==false)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Enter PDI Details";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    // txtPDRNo.Focus();
            //    return;
            //}

            else
            {
                if (type == "ADD")
                {
                    txtInspNo.Text = Numbering().ToString();
                    if (txtInspNo.Text != "")
                    {
                        SaveRec();
                    }
                }
                else if (type == "MOD")
                {
                    SaveRec();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnSave_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
        ////panelDetail.Visible = false;
        ////panelInspection.Visible = true;

        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
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

    #region btnInspCancel_Click
    protected void btnInspCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
    }
    #endregion

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
            if (no > 15)
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

    #region txtRejQty_TextChanged
    protected void txtRejQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtRejQty.Text);

            txtRejQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            if (Convert.ToDouble(txtRejQty.Text) > Convert.ToDouble(txtrecQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Rejected Quantity Not Greater Than Received Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else
            {
                if (txtScrapQty.Text == "")
                {
                    txtScrapQty.Text = "0";
                }
                if (txtRejQty.Text == "")
                {
                    txtRejQty.Text = "0";
                }
                txtOkQty.Text = (Convert.ToDouble(txtrecQty.Text.ToString()) - (Convert.ToDouble(txtScrapQty.Text.ToString()) + Convert.ToDouble(txtRejQty.Text.ToString()))).ToString();

                if (Convert.ToDouble(txtOkQty.Text) < 0)
                {
                    txtOkQty.Text = "0.000";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Sum of (Ok Qty,  Reject Qty,  Scrap Qty) Should not be Greater than  Receive Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
                txtOkQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtOkQty.Text));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspetion", "txtRejQty_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtScrapQty_TextChanged
    protected void txtScrapQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtScrapQty.Text);

            txtScrapQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            if (Convert.ToDouble(txtScrapQty.Text) > Convert.ToDouble(txtrecQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Scrap Quantity Not Greater Than Received Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else
            {
                if (txtScrapQty.Text == "")
                {
                    txtScrapQty.Text = "0";
                }
                if (txtRejQty.Text == "")
                {
                    txtRejQty.Text = "0";
                }
                txtOkQty.Text = (Convert.ToDouble(txtrecQty.Text.ToString()) - (Convert.ToDouble(txtScrapQty.Text.ToString()) + Convert.ToDouble(txtRejQty.Text.ToString()))).ToString();

                if (Convert.ToDouble(txtOkQty.Text) < 0)
                {
                    txtOkQty.Text = "0.000";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Sum of (Ok Qty,  Reject Qty,  Scrap Qty) Should not be Greater than  Receive Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
                txtOkQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtOkQty.Text));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspetion", "txtScrapQty_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

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

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("INSPECTION_S_MASTER", "MODIFY", "INSM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }


            Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            if (panelDetail.Visible == true)
            {


                if (Convert.ToDouble(txtOkQty.Text) == 0.000)
                {
                    flag = false;
                }
                Double RecQty = Convert.ToDouble(txtOkQty.Text) + Convert.ToDouble(txtRejQty.Text) + Convert.ToDouble(txtScrapQty.Text);
                if (Convert.ToDouble(txtrecQty.Text) != RecQty)
                {

                    flag = false;
                }
                else if (Convert.ToDouble(txtRejQty.Text) > 0 && txtReason.Text == "")
                {
                    flag = false;
                }
                else if (txtRemark.Text == "")
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                flag = false;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "CheckValid", Ex.Message);
        }

        return flag;

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
            //if (txtparameter.Text == "" || txtInspection.Text == "" || txtSpecification.Text == "" || txtObservation1.Text == "" || txtObservation2.Text == "" || txtObservation3.Text == "" || txtObservation4.Text == "" || txtObservation5.Text == "" || txtDisposition.Text == "")
            if (txtparameter.Text == "" || txtInspection.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter PDI details";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

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
                    string Par10 = ((Label)(dgPDIDEtail.Rows[i].FindControl("lblINSPDI_I_CODE"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (Par == txtparameter.Text.Trim() && Par1 == txtSpecification.Text.Trim() && Par2 == txtInspection.Text.Trim() && Par3 == txtObservation1.Text.Trim() && Par4 == txtObservation2.Text.Trim() && Par5 == txtObservation3.Text.Trim() && Par6 == txtObservation4.Text.Trim() && Par7 == txtObservation5.Text.Trim() && Par8 == txtDisposition.Text.Trim() && Par9 == txtRemarkPDI.Text.Trim())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist with Same PDI In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            ClearControles();
                            return;
                        }
                    }
                    else
                    {
                        if (Par == txtparameter.Text.Trim() && Par1 == txtSpecification.Text.Trim() && Par2 == txtInspection.Text.Trim() && Par3 == txtObservation1.Text.Trim() && Par4 == txtObservation2.Text.Trim() && Par5 == txtObservation3.Text.Trim() && Par6 == txtObservation4.Text.Trim() && Par7 == txtObservation5.Text.Trim() && Par8 == txtDisposition.Text.Trim() && Par9 == txtRemarkPDI.Text.Trim() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist with Same PDI In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
                ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
                ((DataTable)ViewState["dt2"]).Columns.Add("INSPDI_I_CODE");

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
            dr["DocName"] = ViewState["fileName2"].ToString();
            dr["INSPDI_I_CODE"] = 0;
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
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgPDIDEtail.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            #region Modify
            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgPDIDEtail.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgPDIDEtail.DataSource = ((DataTable)ViewState["dt2"]);
                dgPDIDEtail.DataBind();
                if (dgPDIDEtail.Rows.Count == 0)
                    LoadFilter();
            }
            #endregion Modify

            #region Modify
            if (e.CommandName == "Modify")
            {
                //dgInwardMaster.Columns[1].Visible = false;

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

                foreach (GridViewRow gvr in dgPDIDEtail.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
            #endregion Modify

            #region ViewPDF
            else if (e.CommandName == "ViewPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/Inspection/" + code + "/" + filePath;
                    }
                    myframe.Attributes["src"] = directory;
                    ModalPopupExtenderDovView.Show();
                    PanelDoc.Visible = true;
                }
            }
            #endregion ViewPDF
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
        if (dgPDIDEtail.Rows.Count == 0)
        {
            dtFilter.Clear();
            if (dtFilter.Columns.Count == 0)
            {
                // dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_CODE", typeof(String)));
                // dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_PARAMETERS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_SPECIFTION", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_INSPECTION", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR1", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR2", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR3", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR4", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_OBSR5", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_DSPOSITION", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_REMARK", typeof(String)));//INSPDI_INSM_CODE
                dtFilter.Columns.Add(new System.Data.DataColumn("DocName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INSPDI_I_CODE", typeof(String)));//INSPDI_INSM_CODE


                dtFilter.Rows.Add(dtFilter.NewRow());
                dgPDIDEtail.DataSource = dtFilter;
                dgPDIDEtail.DataBind();
                dgPDIDEtail.Enabled = false;
            }
        }
    }
    #endregion

    protected void chkPDR_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPDR.Checked == true)
        {
            txtPDRNo.Enabled = true;
        }
        else
        {
            txtPDRNo.Enabled = false;
        }
    }


    protected void chkTcNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTcNo.Checked == true)
        {
            txtTcNo.Enabled = true;
        }
        else
        {
            txtTcNo.Enabled = false;
        }
    }

    #region lnkuploadTModel_Click
    protected void lnkuploadTModel_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("ADD"))
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/Inspection/" + filePath;
            }
            else
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }

            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("upplier Master Entry", "lnkupload_Click", ex.Message);
        }
    }
    #endregion
}
