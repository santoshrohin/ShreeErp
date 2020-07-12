using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ReferenceMaster_BL
/// </summary>
public class ReferenceMaster_BL
{
	#region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _REF_CODE;
    private int _REF_CM_COMP_ID;
    private string _REF_NAME;
    private string _REF_DESC;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsNEWREFERENCE_MASTER objclsNEWREFERENCE_MASTER;
    #endregion

    #region "Constructor"
    public ReferenceMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public ReferenceMaster_BL(int Id)
    {
        _REF_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int REF_CODE
    {
        get { return _REF_CODE; }
        set { _REF_CODE = value; }
    }
    public int REF_CM_COMP_ID
    {
        get { return _REF_CM_COMP_ID; }
        set { _REF_CM_COMP_ID = value; }
    }
    public string REF_NAME
    {
        get { return _REF_NAME; }
        set { _REF_NAME = value; }
    }
    public string REF_DESC
    {
        get { return _REF_DESC; }
        set { _REF_DESC = value; }
    }
    public bool ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }
    public bool MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }
    #endregion

    #region Public Methods
    
    #region GetRecords
    public DataTable GetRecords(string Type)
    {
        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@REF_CODE",SqlDbType.Int),
				new SqlParameter("@REF_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@REF_NAME",SqlDbType.VarChar),
				new SqlParameter("@REF_DESC",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
		
			};


            if (REF_CODE == 0 )
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = REF_CODE;
            }

            if (REF_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = REF_CM_COMP_ID;
            }

            if (REF_NAME != null)
            {
                Params[2].Value = REF_NAME;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }

            if (REF_DESC != null)
            {
                Params[3].Value = REF_DESC;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (ES_DELETE != null)
            {
                Params[4].Value = ES_DELETE;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[5].Value = MODIFY;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[6].Value = Type;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }
            dt = DL_DBAccess.SelectData("SP_NEWREFERENCE_MASTER_Select", Params);

            return dt;

        }
        catch (Exception ex)
        {
            
            //throw new Exception(ex.Message);
            CommonClasses.SendError("REFERENCE Master Class", "GetInfo", ex.Message);
            return dt;
        }
        
    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
        dt = GetRecords("GETINFO");

        if (dt.Rows.Count > 0)
        {
            REF_CODE = Convert.ToInt32(dt.Rows[0]["REF_CODE"]);
            REF_NAME = dt.Rows[0]["REF_NAME"].ToString();
            REF_DESC = dt.Rows[0]["REF_DESC"].ToString();
        }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DataTable dt = new DataTable();
        dt = GetRecords("FILLGRID");

        XGrid.DataSource = dt;
        XGrid.DataBind();
    }
    #endregion

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {

        bool res = false;
        DataTable dt = new DataTable();
        dt = GetRecords("CHECKSAVE");

        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DataTable dt = new DataTable();
        dt = GetRecords("CHECKUPDATE");
        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion

    #region Save
    public bool Save()
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (CheckExistSaveName())
            {

                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@REF_CM_COMP_ID",REF_CM_COMP_ID),
				    new SqlParameter("@REF_NAME",REF_NAME),
				    new SqlParameter("@REF_DESC",REF_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_NEWREFERENCE_MASTER_Insert", Params);
            }
            else
            {
                Msg = "REFERENCE Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REFERENCE Master Class", "Save", Ex.Message);
        }
        return result;
    }
    #endregion

    #region Update
    public bool Update()
    {
        bool result = false;
        try
        {
            //if (ChkExstUpdateName())
            //{
            DL_DBAccess = new DatabaseAccessLayer();
            if (CommonClasses.Execute("SELECT REF_CODE,REF_NAME,REF_DESC FROM NEWREFERENCE_MASTER where REF_NAME='" + REF_NAME + "'  and REF_CODE!=" + REF_CODE + "").Rows.Count == 0)
            {
                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@REF_CODE",REF_CODE),
				    new SqlParameter("@REF_CM_COMP_ID",REF_CM_COMP_ID),
				    new SqlParameter("@REF_NAME",REF_NAME),
				    new SqlParameter("@REF_DESC",REF_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_NEWREFERENCE_MASTER_Update", Params);
            }
            else
            {
                Msg = "REFERENCE Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REFERENCE Master Class", "Update", Ex.Message);
        }
        return result;
    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] Params = { new SqlParameter("@REF_CODE", REF_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_NEWREFERENCE_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REFERENCE Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


    #endregion
}
