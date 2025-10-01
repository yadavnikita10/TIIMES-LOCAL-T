using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class StandardStatementController : Controller
    {
        // GET: StandardStatement
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        [HttpGet]
        public ActionResult UploadStatement()
        {
            DALCallMaster objDAM = new DALCallMaster();
            ViewData["Drpproduct"] = objDAM.GetDrpList();
            List<string> Selected = new List<string>();
            string[] splitedProduct_Name;

            return View();
        }

        [HttpPost]
        public ActionResult Preview(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var allowedExtensions = new[] { ".xls", ".xlsx" };
                if (allowedExtensions.Contains(fileExtension.ToLower()) &&
               (file.ContentType == "application/vnd.ms-excel" ||
                file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                {

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;
                        var colCount = worksheet.Dimension.Columns;

                        var excelData = new List<ExcelDataModel>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            excelData.Add(new ExcelDataModel
                            {
                                Statementwithconclu = worksheet.Cells[row, 1].Text
                            });
                        }
                        return Json(excelData);
                    }
                }
                else
                {
                    return Json(new { message = "Only Excel files (.xls, .xlsx) are allowed." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }


        [HttpPost]
        public ActionResult UploadData(List<ExcelDataModel> excelData)
        {
            string Message = string.Empty;
            if (excelData != null && excelData.Any())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Statementconclusion", typeof(string));
                DataSet dsTable = new DataSet();
                dsTable.Tables.Add(dt);
                foreach (var item in excelData)
                {
                    dt.Rows.Add(item.Id, item.Statementwithconclu);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("Sp_InsertUpdateStandardStatement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xmlData", dsTable.GetXml());
                cmd.Parameters.AddWithValue("@Sp_Type", '1');
                cmd.ExecuteNonQuery();
                con.Close();
                Message = "Save";
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllProductStatement(int productId)
        {
            DataTable DTGetProductStatement = new DataTable();
            try
            {
                con.Open();
                SqlCommand CMDGetExtenstion = new SqlCommand("Sp_GetProductStatementData", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", '2');
                CMDGetExtenstion.Parameters.AddWithValue("@Id", productId);
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetProductStatement);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetProductStatement.Dispose();
                con.Close();
            }
            string jsonData = JsonConvert.SerializeObject(DTGetProductStatement);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetProductSummery()
        {
            DataTable DTGetProductSummery = new DataTable();
            try
            {
                con.Open();
                SqlCommand CMDGetExtenstion = new SqlCommand("Sp_GetProductStatementData", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", '1');
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetProductSummery);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetProductSummery.Dispose();
                con.Close();
            }
            string jsonData = JsonConvert.SerializeObject(DTGetProductSummery);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProductItemDelete(int id)
        {
            string Res = "False";
            try
            {
                con.Open();
                SqlCommand CMDGetExtenstion = new SqlCommand("Sp_GetProductStatementData", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", '3');
                CMDGetExtenstion.Parameters.AddWithValue("@Id", id);
                object result = CMDGetExtenstion.ExecuteNonQuery();
                if (result != null)
                {
                    Res = "True";
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProductItemEdit(List<ExcelDataModel> objExcelModel)
        {
            string Res = "False";
            try
            {
                //con.Open();
                //SqlCommand CMDGetExtenstion = new SqlCommand("Sp_GetProductStatementData", con);
                //CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                //CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", '4');
                //CMDGetExtenstion.Parameters.AddWithValue("@Id", objExcelModel[0].Id);
                //CMDGetExtenstion.Parameters.AddWithValue("@Statementwithconclusion", objExcelModel[0].Statementwithconclu);
                //object result = CMDGetExtenstion.ExecuteNonQuery();
                //if (result != null)
                //{
                //    Res = "True";
                //}
                //con.Close();

                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Statementconclusion", typeof(string));
                DataSet dsTable = new DataSet();
                dsTable.Tables.Add(dt);
                foreach (var item in objExcelModel)
                {
                    dt.Rows.Add(item.Id, item.Statementwithconclu);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("Sp_InsertUpdateStandardStatement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xmlData", dsTable.GetXml());
                cmd.Parameters.AddWithValue("@Sp_Type", '2');
                object res=cmd.ExecuteNonQuery();
                if(res!=null)
                {
                    Res = "True";
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Res, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult DeleteSummaryProductItem(int productId)
        {
            string Res = "False";
            try
            {
                con.Open();
                SqlCommand CMDGetExtenstion = new SqlCommand("Sp_GetProductStatementData", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", '5');
                CMDGetExtenstion.Parameters.AddWithValue("@Id", productId);
                object result = CMDGetExtenstion.ExecuteNonQuery();
                if (result != null)
                {
                    Res = "True";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Json(Res, JsonRequestBehavior.AllowGet);
        }

        public class ExcelDataModel
        {
            public int Id { get; set; }
            public string Product { get; set; }
            public string Statementwithconclu { get; set; }

        }
    }
}