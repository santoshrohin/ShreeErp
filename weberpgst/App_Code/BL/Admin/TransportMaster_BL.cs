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
/// Summary description for TransportMaster_BL
/// </summary>
public class TransportMaster_BL
{

    #region Data Members
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _T_CODE;
    private int _T_CM_COMP_ID;
    private string _T_NAME;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region Constructor
    public TransportMaster_BL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public TransportMaster_BL(int Id)
    {
        _T_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int T_CODE
    {
        get { return _T_CODE; }
        set { _T_CODE = value; }
    }
    public int T_CM_COMP_ID
    {
        get { return _T_CM_COMP_ID; }
        set { _T_CM_COMP_ID = value; }
    }
    public string T_NAME
    {
        get { return _T_NAME; }
        set { _T_NAME = value; }
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

    #region GetRecords
    public DataTable GetRecords(string Type)
    {
        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@T_CODE",SqlDbType.Int),
				new SqlParameter("@T_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@T_NAME",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar)
			};

            if (T_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = T_CODE;
            }

            if (T_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = T_CM_COMP_ID;
            }

            if (T_NAME != null)
            {
                Params[2].Value = T_NAME.Trim();
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }


            if (ES_DELETE != null)
            {
                Params[3].Value = ES_DELETE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[4].Value = MODIFY;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[5].Value = Type;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }
            dt = DL_DBAccess.SelectData("TRANSPORT_MASTER_Select", Params);

            return dt;
        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("Transport Master", "GetInfo", ex.Message);
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
            T_CODE = Convert.ToInt32(dt.Rows[0]["T_CODE"]);
            T_NAME = dt.Rows[0]["T_NAME"].ToString().Trim();
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
				    new SqlParameter("@T_CM_COMP_ID",T_CM_COMP_ID),
				    new SqlParameter("@T_NAME",T_NAME),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("TRANSPORT_MASTER_Insert", Params);
            }
            else
            {
                Msg = "Record Already Exist..";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "Save", Ex.Message);
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
            if (ChkExstUpdateName())
            {
                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@T_CODE",T_CODE),
				    new SqlParameter("@T_CM_COMP_ID",T_CM_COMP_ID),
				    new SqlParameter("@T_NAME",T_NAME)
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("TRANSAPORT_MASTER_Update", Params);
            }
            else
            {
                Msg = "Country Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@T_CODE", T_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("TRANSAPORT_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion
    
}
