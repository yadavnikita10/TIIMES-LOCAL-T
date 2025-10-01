using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TuvVision.BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.BusinessServices.Interface;

namespace TuvVision.BusinessServices.Implementation
{
    public class GalleryServices : IGalleryServices
    {
        //private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        
        public int AddGalleryDetails(Gallery_VM _vmgal)
        {
            int i = 0;
            if (_vmgal.PK_GalleryId == 0)
            {
                try
                {
                    SqlCommand addnewgallery = new SqlCommand("SP_Food_GalleryMaster", con);
                    addnewgallery.CommandType = CommandType.StoredProcedure;
                    addnewgallery.Parameters.AddWithValue("@SP_Type", 1);
                    addnewgallery.Parameters.AddWithValue("@Title", _vmgal.Title);
                    addnewgallery.Parameters.AddWithValue("@Year", _vmgal.Year);
                    addnewgallery.Parameters.AddWithValue("@EventDate", _vmgal.EventDate);
                    addnewgallery.Parameters.AddWithValue("@Gallery", _vmgal.Gallery);
                    addnewgallery.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    addnewgallery.Parameters.AddWithValue("@CreatedBy", _vmgal.CreatedBy);
                    con.Open();
                    i = addnewgallery.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else if (_vmgal.PK_GalleryId > 0)
            {
                try
                {
                    SqlCommand updategallery = new SqlCommand("SP_Food_GalleryMaster", con);
                    updategallery.CommandType = CommandType.StoredProcedure;
                    updategallery.Parameters.AddWithValue("@SP_Type", 5);
                    updategallery.Parameters.AddWithValue("@PK_GalleryId", _vmgal.PK_GalleryId);
                    updategallery.Parameters.AddWithValue("@Title", _vmgal.Title);
                    updategallery.Parameters.AddWithValue("@Year", _vmgal.Year);
                    updategallery.Parameters.AddWithValue("@EventDate", _vmgal.EventDate);
                    updategallery.Parameters.AddWithValue("@Gallery", _vmgal.Gallery);
                    updategallery.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    updategallery.Parameters.AddWithValue("@CreatedBy", _vmgal.CreatedBy);
                    con.Open();
                    i = updategallery.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return i;
        }
        public IEnumerable<Gallery_VM> GetAllGalleryList()
        {
            List<Gallery_VM> GalleryList = new List<Gallery_VM>();
            try
            {
                SqlCommand getGalleryList = new SqlCommand("SP_Food_GalleryMaster", con);
                getGalleryList.CommandType = CommandType.StoredProcedure;
                getGalleryList.Parameters.AddWithValue("@SP_Type", 2);
                con.Open();
                SqlDataReader dr = getGalleryList.ExecuteReader();
                while (dr.Read())
                {
                    Gallery_VM _vmgal = new Gallery_VM();
                    _vmgal.PK_GalleryId = Convert.ToInt32(dr["PK_GalleryId"]);
                    _vmgal.Title = dr["Title"].ToString();
                    _vmgal.Year = Convert.ToInt32(dr["Year"]);
                    _vmgal.EventDate = Convert.ToDateTime(dr["EventDate"]);
                    _vmgal.Gallery = dr["Gallery"].ToString();
                    _vmgal.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    _vmgal.CreatedBy = dr["CreatedBy"].ToString();
                    GalleryList.Add(_vmgal);
                }
                return GalleryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public Gallery_VM GetGalleryById(int? id)
        {
            Gallery_VM _vmgetbyid = new Gallery_VM();
            try
            {
                SqlCommand getgalbyid = new SqlCommand("SP_Food_GalleryMaster", con);
                getgalbyid.CommandType = CommandType.StoredProcedure;
                getgalbyid.Parameters.AddWithValue("@SP_Type", 3);
                getgalbyid.Parameters.AddWithValue("@PK_GalleryId", id);
                con.Open();
                SqlDataReader dr = getgalbyid.ExecuteReader();
                while (dr.Read())
                {
                    _vmgetbyid.PK_GalleryId = Convert.ToInt32(dr["PK_GalleryId"]);
                    _vmgetbyid.Year = Convert.ToInt32(dr["Year"]);
                    _vmgetbyid.Title = dr["Title"].ToString();
                    _vmgetbyid.EventDate = Convert.ToDateTime(dr["EventDate"]);
                    _vmgetbyid.Gallery = dr["Gallery"].ToString();
                    _vmgetbyid.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    _vmgetbyid.CreatedBy = dr["CreatedBy"].ToString();
                }
                return _vmgetbyid;
           
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public IEnumerable<Gallery_VM> GetAllYearList()
        {
            List<Gallery_VM> YearList = new List<Gallery_VM>();
            try
            {
                SqlCommand getYearList = new SqlCommand("SP_Food_GalleryMaster", con);
                getYearList.CommandType = CommandType.StoredProcedure;
                getYearList.Parameters.AddWithValue("@SP_Type", 7);
                con.Open();
                SqlDataReader dr = getYearList.ExecuteReader();
                while (dr.Read())
                {
                    Gallery_VM _vmgal = new Gallery_VM();
                    _vmgal.Year = Convert.ToInt32(dr["Year"]);
                    YearList.Add(_vmgal);
                }
                return YearList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public IEnumerable<Gallery_VM> GetAllTitleList(int? Year)
        {
            List<Gallery_VM> TitleList = new List<Gallery_VM>();
            try
            {
                SqlCommand getTitleList = new SqlCommand("SP_Food_GalleryMaster", con);
                getTitleList.CommandType = CommandType.StoredProcedure;
                getTitleList.Parameters.AddWithValue("@SP_Type", 4);
                getTitleList.Parameters.AddWithValue("@Year", Year);
                con.Open();
                SqlDataReader dr = getTitleList.ExecuteReader();
                while (dr.Read())
                {
                    Gallery_VM _vmgal = new Gallery_VM();
                    _vmgal.Year = Convert.ToInt32(dr["Year"]);
                    _vmgal.Title = dr["Title"].ToString();
                    _vmgal.EventDate = Convert.ToDateTime(dr["EventDate"]);
                    TitleList.Add(_vmgal);
                }                
                return TitleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public List<string> GetAllGallery(string ttl)
        {
            Gallery_VM _vmlistgall = new Gallery_VM();
            List<Gallery_VM> GalleryList = new List<Gallery_VM>();
            try
            {
                SqlCommand getallGallery = new SqlCommand("SP_Food_GalleryMaster", con);
                getallGallery.CommandType = CommandType.StoredProcedure;
                getallGallery.Parameters.AddWithValue("@SP_Type", 6);
                getallGallery.Parameters.AddWithValue("@Title", ttl);
                con.Open();
                SqlDataReader dr = getallGallery.ExecuteReader();
                while (dr.Read())
                {
                    Gallery_VM _vmgetgal = new Gallery_VM();
                    _vmgetgal.Gallery = dr["Gallery"].ToString();
                    GalleryList.Add(_vmgetgal);
                }
                var data = GalleryList.Select(a=>a.Gallery).ToList();


                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        public IEnumerable<Gallery_VM> GetAllList()
        {
            var alllist = this.GetAllGalleryList();
            List<Gallery_VM> AllTitleList = new List<Gallery_VM>();
            try
            {
                SqlCommand getTitleList = new SqlCommand("SP_Food_GalleryMaster", con);
                getTitleList.CommandType = CommandType.StoredProcedure;
                getTitleList.Parameters.AddWithValue("@SP_Type", 8);
                getTitleList.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                con.Open();
                SqlDataReader dr = getTitleList.ExecuteReader();
                while (dr.Read())
                {
                    Gallery_VM _vmgal = new Gallery_VM();
                    _vmgal.Year = Convert.ToInt32(dr["Year"]);
                    _vmgal.Title = dr["Title"].ToString();
                    _vmgal.EventDate = Convert.ToDateTime(dr["EventDate"]);
                    _vmgal.GallaryImages = alllist.Where(a=>a.Title == _vmgal.Title && a.Gallery != null && a.Gallery != "" && a.Year == _vmgal.Year && a.EventDate == _vmgal.EventDate).Select(a=>a.Gallery).ToList();
                    if (_vmgal.GallaryImages.FirstOrDefault() != null)
                    {
                        _vmgal.Gallery = _vmgal.GallaryImages.FirstOrDefault();
                    }
                    AllTitleList.Add(_vmgal);
                }
                return AllTitleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public bool DeleteImage(int? id)
        {
            int i = 0;
            try
            {
                SqlCommand deleteimage = new SqlCommand("SP_Food_GalleryMaster", con);
                deleteimage.CommandType = CommandType.StoredProcedure;
                deleteimage.Parameters.AddWithValue("@SP_Type", 9);
                deleteimage.Parameters.AddWithValue("@PK_GalleryId", id);
                con.Open();
                i = deleteimage.ExecuteNonQuery();
                if (i != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

        }
    }
}