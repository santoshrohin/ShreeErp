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
/// Summary description for VehicleMaster_BL
/// </summary>
public class VehicleMaster_BL
{
    #region Data Members
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _VM_CODE;
    private int _VM_CM_COMP_ID;
    private string _VM_NAME;
    private int _VM_T_CODE;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region Constructor
    public VehicleMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public VehicleMaster_BL(int Id)
    {
        _VM_CODE = Id;
    } 
    #endregion

    #region Public Properties
    public int VM_CODE
    {
        get { return _VM_CODE; }
        set { _VM_CODE = value; }
    }
    public int VM_CM_COMP_ID
    {
        get { return _VM_CM_COMP_ID; }
        set { _VM_CM_COMP_ID = value; }
    }
    public string VM_NAME
    {
        get { return _VM_NAME; }
        set { _VM_NAME = value; }
    }
    public int VM_T_CODE
    {
        get { return _VM_T_CODE; }
        set { _VM_T_CODE = value; }
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
				new SqlParameter("@VM_CODE",SqlDbType.Int),
				new SqlParameter("@VM_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@VM_NAME",SqlDbType.VarChar),
				new SqlParameter("@VM_T_CODE",SqlDbType.Int),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
			};
            if (VM_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = VM_CODE;
            }
            if (VM_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = VM_CM_COMP_ID;
            }
            if (VM_NAME != null)
            {
                Params[2].Value = VM_NAME.Trim();
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (VM_T_CODE != null)
            {
                Params[3].Value = VM_T_CODE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[4].Value = Type;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }
            dt = DL_DBAccess.SelectData("VEHICLE_MASTER_SELECT", Params);

            return dt;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "GetInfo", ex.Message);
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
            VM_CODE = Convert.ToInt32(dt.Rows[0]["VM_CODE"]);
            VM_NAME = dt.Rows[0]["VM_NAME"].ToString();
            VM_T_CODE = Convert.ToInt32(dt.Rows[0]["VM_T_CODE"]);
        
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
				    new SqlParameter("@VM_CM_COMP_ID",VM_CM_COMP_ID),
				    new SqlParameter("@VM_NAME",VM_NAME),
                    new SqlParameter("@VM_T_CODE",VM_T_CODE)
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("VEHICLE_MASTER_INSERT", Params);
            }
            else
            {
                Msg = "Record Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "Save", Ex.Message);
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
				    new SqlParameter("@VM_CODE",VM_CODE),
				    new SqlParameter("@VM_CM_COMP_ID",VM_CM_COMP_ID),
				    new SqlParameter("@VM_NAME",VM_NAME),
                    new SqlParameter("@VM_T_CODE",VM_T_CODE)
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_STATE_MASTER_UPDATE", Params);
            }
            else
            {
                Msg = "Record Already Exist...";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vehicle Master", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@VM_CODE", VM_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_STATE_MASTER_DELETE", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vehicle Master", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion

}
