using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALTrainingSchedule
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        static string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"].ToString();
        public DataSet GetTopicList(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '1');
                cmd.Parameters.AddWithValue("@TrainingName", prefix);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }


        public DataSet GetTrainerName(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');
                cmd.Parameters.AddWithValue("@TrainerName", prefix);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetTraineeName(string prefix, string obsType, string EmpCat, string TypeOfEmp, string Branch, string TrainType, string UserRole)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@TraineeName", prefix);
                cmd.Parameters.AddWithValue("@obsType", obsType);
                cmd.Parameters.AddWithValue("@EmpCat", EmpCat);
                cmd.Parameters.AddWithValue("@TypeOfEmp", TypeOfEmp);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@TrainType", TrainType);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetBranchName(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@BranchName", prefix);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetTrainerIdByName(string n)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@TrainerName", n);
                SqlDataAdapter da = new SqlDataAdapter(cmd); 
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetTrainingIdByName(string n)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@TrainingName", n);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public DataTable GetTraineeIdByName(string n)
        {
            DataTable DSTraineeId = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
                cmd.Parameters.AddWithValue("@TraineeName",n);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSTraineeId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSTraineeId.Dispose();
            }
            return DSTraineeId;
        }

        public DataSet GetTraineeIdByName1(string n)
        {
            DataSet DSTraineeId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
                cmd.Parameters.AddWithValue("@TraineeName", n);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSTraineeId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSTraineeId.Dispose();
            }
            return DSTraineeId;
        }

        public DataTable GetBranchIdByName(string n)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@BranchName", n);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }


        //public string Insert(TrainingScheduleModel TSM,string TrainerId)

//string iTrainingId,string ITrainerId,string ITraineeId,string IBranchId
        public int Insert(TrainingScheduleModel TSM, int iTrainingId, string ITrainerId, string ITraineeId, int ConvertBranchId)
        {
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            int ReturnScheduleId = 0;
            string Result = string.Empty;
            con.Open();
            try
            {
                if (TSM.PK_TrainingScheduleId != 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "12");
                    cmd.Parameters.AddWithValue("@TrainingId", iTrainingId);
                    cmd.Parameters.AddWithValue("@TraineeId", ITraineeId);
                    cmd.Parameters.AddWithValue("@BranchId", TSM.BranchId);
                    cmd.Parameters.AddWithValue("@TrainerId", ITrainerId);
                    //cmd.Parameters.AddWithValue("@TrainingStartDate", DateTime.ParseExact(TSM.TrainingStartDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@TrainingEndDate", DateTime.ParseExact(TSM.TrainingEndDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@TrainingStartTime", TSM.TrainingStartTime);
                    //cmd.Parameters.AddWithValue("@TrainingEndTime", TSM.TrainingEndTime);
                    
                    cmd.Parameters.AddWithValue("@TrainerName", TSM.TrainerName);
                    cmd.Parameters.AddWithValue("@TraineeName", TSM.TraineeName);
                    cmd.Parameters.AddWithValue("@BranchName", TSM.Branch);
                    cmd.Parameters.AddWithValue("@ExternalTrainer", TSM.ExternalTrainer);
                    cmd.Parameters.AddWithValue("@TotalHours", TSM.TotalHours);
                    cmd.Parameters.AddWithValue("@Venue", TSM.Venue);
                    cmd.Parameters.AddWithValue("@Scope", TSM.Scope);
                    cmd.Parameters.AddWithValue("@Remarks", TSM.Remarks);
                    cmd.Parameters.AddWithValue("@Status", TSM.Status);
                    cmd.Parameters.AddWithValue("@chkAllEmployee", TSM.chkAllEmployee);
                    cmd.Parameters.AddWithValue("@chkBoxBranch", TSM.checkBoxBranch);
                    cmd.Parameters.AddWithValue("@EmployementCategory", TSM.EmployementCategory);
                    cmd.Parameters.AddWithValue("@TrainingName", TSM.TrainingName);
                    cmd.Parameters.AddWithValue("@TrainType", TSM.TrainType);
                    cmd.Parameters.Add("@ReturnScheduleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@PK_TrainingScheduleId", TSM.PK_TrainingScheduleId);
                    cmd.Parameters.AddWithValue("@QuizeTimeInHours", TSM.QuizeTimeInHours);
                    cmd.Parameters.AddWithValue("@EvaluationMethod", TSM.EvaluationMethod);
                    cmd.Parameters.AddWithValue("@Category", TSM.Category);
                    cmd.Parameters.AddWithValue("@obsType", TSM.ProjectType);
                    //cmd.Parameters.AddWithValue("@QuizeEndDate", DateTime.ParseExact(TSM.QuizeEndDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@QuizeEndTime", TSM.QuizeEndTime1);
                    cmd.Parameters.AddWithValue("@UserRole", TSM.UserRole);
                    cmd.Parameters.AddWithValue("@Link", TSM.Link);

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    // cmd.Parameters.AddWithValue("@ModifiedDateTime", N.ServiceCode);
                    Result = cmd.ExecuteNonQuery().ToString();

                    ReturnScheduleId = Convert.ToInt32(cmd.Parameters["@ReturnScheduleId"].Value.ToString());
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '9');
                    cmd.Parameters.AddWithValue("@TrainingId", iTrainingId);
                    cmd.Parameters.AddWithValue("@TraineeId", ITraineeId);
                    cmd.Parameters.AddWithValue("@BranchId", TSM.Branch);
                    cmd.Parameters.AddWithValue("@TrainerId", ITrainerId);
                    //if (TSM.TrainingStartDate != null)
                    //    cmd.Parameters.AddWithValue("@TrainingStartDate", DateTime.ParseExact(TSM.TrainingStartDate, "dd/MM/yyyy", theCultureInfo));
                    //if (TSM.TrainingEndDate != null)
                    //    cmd.Parameters.AddWithValue("@TrainingEndDate", DateTime.ParseExact(TSM.TrainingEndDate, "dd/MM/yyyy", theCultureInfo));


                    //cmd.Parameters.AddWithValue("@TrainingStartTime", TSM.TrainingStartTime);
                    //cmd.Parameters.AddWithValue("@TrainingEndTime", TSM.TrainingEndTime);


                    cmd.Parameters.AddWithValue("@TrainerName", TSM.TrainerName);
                    cmd.Parameters.AddWithValue("@TraineeName", TSM.TraineeName);
                    cmd.Parameters.AddWithValue("@BranchName", TSM.BranchName);
                    cmd.Parameters.AddWithValue("@ExternalTrainer", TSM.ExternalTrainer);
                    cmd.Parameters.AddWithValue("@TotalHours", TSM.TotalHours);
                    if (TSM.TrainType.ToUpper() == "ELEARN")
                        cmd.Parameters.AddWithValue("@Venue", "ELearning");
                    else
                        cmd.Parameters.AddWithValue("@Venue", TSM.Venue);

                    cmd.Parameters.AddWithValue("@Scope", TSM.Scope);
                    cmd.Parameters.AddWithValue("@Remarks", TSM.Remarks);
                    cmd.Parameters.AddWithValue("@Status", TSM.Status);
                    cmd.Parameters.AddWithValue("@chkAllEmployee", TSM.chkAllEmployee);
                    cmd.Parameters.AddWithValue("@QuizeTimeInHours", TSM.QuizeTimeInHours);

                    cmd.Parameters.AddWithValue("@TrainingName", TSM.TrainingName);
                    cmd.Parameters.AddWithValue("@TrainType", TSM.TrainType);
                    cmd.Parameters.AddWithValue("@EvaluationMethod", TSM.EvaluationMethod);
                    cmd.Parameters.AddWithValue("@Category", TSM.Category);
                    cmd.Parameters.AddWithValue("@TypeOfEmp", TSM.TypeOfEmployee);
                    cmd.Parameters.AddWithValue("@ObsType", TSM.ProjectType);
                    cmd.Parameters.AddWithValue("@ETrainType", TSM.ETrainType);
                    cmd.Parameters.AddWithValue("@EmployementCategory", TSM.EmployementCategory);
                    
                    //cmd.Parameters.AddWithValue("@TrainType", TSM.TrainType);
                    //cmd.Parameters.AddWithValue("@EvaluationMethod", TSM.EvaluationMethod);
                    //cmd.Parameters.AddWithValue("@Category", TSM.Category);
                    //cmd.Parameters.AddWithValue("@obsType", TSM.ProjectType);
                    //cmd.Parameters.AddWithValue("@QuizeEndDate", DateTime.ParseExact(TSM.QuizeEndDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@QuizeEndTime", TSM.QuizeEndTime1);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@UserRole", TSM.UserRole);
                    cmd.Parameters.AddWithValue("@Link", TSM.Link);
                    cmd.Parameters.Add("@ReturnScheduleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    ReturnScheduleId = Convert.ToInt32(cmd.Parameters["@ReturnScheduleId"].Value.ToString());
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return ReturnScheduleId;
        }

        public string InsertTraineeIdForRecord(TrainingScheduleModel TSM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9N");
                cmd.Parameters.AddWithValue("@TraineeIDForReport", TSM.TraineeIdForRecord);
                cmd.Parameters.AddWithValue("@ReturnScheduleId", TSM.RTrainingScheduleId);






                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                Result = cmd.ExecuteNonQuery().ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public DataSet GetDataById(int id)
        {
            string User = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@PK_TrainingScheduleId", id);
                cmd.Parameters.AddWithValue("@TraineeId", User);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public DataTable GetFeedbackData(int ffid, string UserID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");
                cmd.Parameters.AddWithValue("@TrainingRecordId", ffid);
                cmd.Parameters.AddWithValue("@RTraineeId", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public DataTable GetTraineeData(string UserIds , string Trainer)//Get All DropDownlist 
        {
            DataTable GetAllTrainee = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 100);
                CMDGetDdlLst.Parameters.AddWithValue("@TraineeId", UserIds);
                CMDGetDdlLst.Parameters.AddWithValue("@TrainerId", Trainer);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(GetAllTrainee);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetAllTrainee.Dispose();
            }
            return GetAllTrainee;
        }

        public DataTable GetTrainingRecordDataById(int id)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@RPK_TrainingScheduleId", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetDataById1(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "20");
                cmd.Parameters.AddWithValue("@PK_TrainingScheduleId", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public int Update(TrainingScheduleModel TSM, int iTrainingId, string ITrainerId, string ITraineeId, int ConvertBranchId)
        {
            string Result = string.Empty;
            int ReturnScheduleId = 0;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@TrainingId", iTrainingId);
                cmd.Parameters.AddWithValue("@TraineeId", ITraineeId);
                cmd.Parameters.AddWithValue("@BranchId", ConvertBranchId);
                cmd.Parameters.AddWithValue("@TrainerId", ITrainerId);
                cmd.Parameters.AddWithValue("@TrainingStartDate", DateTime.ParseExact(TSM.TrainingStartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@TrainingEndDate", DateTime.ParseExact(TSM.TrainingEndDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@TrainingStartTime", TSM.TrainingStartTime);
                cmd.Parameters.AddWithValue("@TrainingEndTime", TSM.TrainingEndTime);

                cmd.Parameters.AddWithValue("@TrainerName", TSM.TrainerName);
                cmd.Parameters.AddWithValue("@TraineeName", TSM.TraineeName);
                cmd.Parameters.AddWithValue("@BranchName", TSM.BranchName);
                cmd.Parameters.AddWithValue("@ExternalTrainer", TSM.ExternalTrainer);
                cmd.Parameters.AddWithValue("@TotalHours", TSM.TotalHours);
                cmd.Parameters.AddWithValue("@Venue", TSM.Venue);
                cmd.Parameters.AddWithValue("@Scope", TSM.Scope);
                cmd.Parameters.AddWithValue("@Remarks", TSM.Remarks);
                cmd.Parameters.AddWithValue("@Status", TSM.Status);
                cmd.Parameters.AddWithValue("@chkAllEmployee", TSM.chkAllEmployee);
                cmd.Parameters.AddWithValue("@chkBoxBranch", TSM.checkBoxBranch);
                cmd.Parameters.Add("@ReturnScheduleId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@PK_TrainingScheduleId", TSM.PK_TrainingScheduleId);
                cmd.Parameters.AddWithValue("@QuizeTimeInHours", TSM.QuizeTimeInHours);

                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                // cmd.Parameters.AddWithValue("@ModifiedDateTime", N.ServiceCode);
                Result = cmd.ExecuteNonQuery().ToString();

                ReturnScheduleId = Convert.ToInt32(cmd.Parameters["@ReturnScheduleId"].Value.ToString());

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return ReturnScheduleId;
        }

        public string AddTrainingRecord(TrainingScheduleModel TSM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '1');
                cmd.Parameters.AddWithValue("@Attachment", TSM.Attachment);                
                //cmd.Parameters.AddWithValue("@TrainingRecordId", TSM.TrainingRecordId);            
                //foreach (var item in TSM.TrainingRecordList)
                //{
                    cmd.Parameters.AddWithValue("@RIsPresent", TSM.RIsPresent);
                    cmd.Parameters.AddWithValue("@RAccessorId", TSM.AccessorId);
                cmd.Parameters.AddWithValue("@RAccessorName", TSM.AccessorName);
                cmd.Parameters.AddWithValue("@RTraineeId", TSM.RTraineeId);
                 cmd.Parameters.AddWithValue("@RPK_TrainingScheduleId", TSM.RTrainingScheduleId);
                // cmd.Parameters.AddWithValue("@RTraineeId", s);
                //cmd.Parameters.AddWithValue("@RPK_TrainingScheduleId", TSM.RTrainingScheduleId);
                //}
                // cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                // cmd.Parameters.AddWithValue("@ModifiedDateTime", N.ServiceCode);
                Result = cmd.ExecuteNonQuery().ToString();
             }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }

        #region Added by Ankush
        public string AddTrainingAttendance(TrainingScheduleModel TSM)
        {
            string Result = string.Empty;
            con.Open();
            if (TSM.PK_ATID == 0)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "1");
                    cmd.Parameters.AddWithValue("@TrainingRecordId", TSM.RTrainingScheduleId);
                    cmd.Parameters.AddWithValue("@RTraineeId", TSM.RTraineeId);
                    cmd.Parameters.AddWithValue("@RIsPresent", TSM.RIsPresent == true ? "1" : TSM.RIsPresent == false ? "0" : "2");
                    cmd.Parameters.AddWithValue("@TraineeName", TSM.TraineeName);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@Modifiedby", TSM.RTraineeId);
                    cmd.Parameters.AddWithValue("@EmpType", TSM.EmpType);
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "2");
                    cmd.Parameters.AddWithValue("@RTraineeId", TSM.PK_ATID);
                    cmd.Parameters.AddWithValue("@TrainingRecordId", TSM.RTrainingScheduleId);
                    cmd.Parameters.AddWithValue("@RIsPresent", TSM.RIsPresent == true ? "1" : TSM.RIsPresent == false ? "0" : "2");
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@Modifiedby", TSM.RTraineeId);
                    cmd.Parameters.AddWithValue("@EmpType", TSM.EmpType);
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }            
            return Result;
        }
        public int AddTrainingFeedback(TrainingScheduleModel objTF)
        {
            int s = 0;
            if (objTF.FFID == 0)
            {
                try
                {
                    SqlCommand CMDAddFeedback = new SqlCommand("SP_TrAttendance", con);
                    CMDAddFeedback.CommandType = CommandType.StoredProcedure;
                    CMDAddFeedback.Parameters.AddWithValue("@SP_Type", "4");
                    CMDAddFeedback.Parameters.AddWithValue("@Aim", objTF.C1);
                    CMDAddFeedback.Parameters.AddWithValue("@Duration", objTF.C2);
                    CMDAddFeedback.Parameters.AddWithValue("@Expectation", objTF.C3);
                    CMDAddFeedback.Parameters.AddWithValue("@PContent", objTF.C4);
                    CMDAddFeedback.Parameters.AddWithValue("@Result", objTF.C5);
                    CMDAddFeedback.Parameters.AddWithValue("@Command", objTF.D1);
                    CMDAddFeedback.Parameters.AddWithValue("@Presentation", objTF.D2);
                    CMDAddFeedback.Parameters.AddWithValue("@Question", objTF.D3);
                    CMDAddFeedback.Parameters.AddWithValue("@Gesture", objTF.D4);
                    CMDAddFeedback.Parameters.AddWithValue("@Arrangement", objTF.E1);
                    CMDAddFeedback.Parameters.AddWithValue("@Summary", objTF.F1);
                    CMDAddFeedback.Parameters.AddWithValue("@Rating", objTF.G1);
                    CMDAddFeedback.Parameters.AddWithValue("@TrainingRecordId", objTF.PK_TrainingScheduleId);
                    CMDAddFeedback.Parameters.AddWithValue("@TraineeName", objTF.tfid);

                    con.Open();
                    s = CMDAddFeedback.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand CMDAddFeedback = new SqlCommand("SP_TrAttendance", con);
                    CMDAddFeedback.CommandType = CommandType.StoredProcedure;
                    CMDAddFeedback.Parameters.AddWithValue("@SP_Type", "8");
                    CMDAddFeedback.Parameters.AddWithValue("@Aim", objTF.C1);
                    CMDAddFeedback.Parameters.AddWithValue("@Duration", objTF.C2);
                    CMDAddFeedback.Parameters.AddWithValue("@Expectation", objTF.C3);
                    CMDAddFeedback.Parameters.AddWithValue("@PContent", objTF.C4);
                    CMDAddFeedback.Parameters.AddWithValue("@Result", objTF.C5);
                    CMDAddFeedback.Parameters.AddWithValue("@Command", objTF.D1);
                    CMDAddFeedback.Parameters.AddWithValue("@Presentation", objTF.D2);
                    CMDAddFeedback.Parameters.AddWithValue("@Question", objTF.D3);
                    CMDAddFeedback.Parameters.AddWithValue("@Gesture", objTF.D4);
                    CMDAddFeedback.Parameters.AddWithValue("@Arrangement", objTF.E1);
                    CMDAddFeedback.Parameters.AddWithValue("@Summary", objTF.F1);
                    CMDAddFeedback.Parameters.AddWithValue("@Rating", objTF.G1);
                    CMDAddFeedback.Parameters.AddWithValue("@TrainingRecordId", objTF.PK_TrainingScheduleId);
                    CMDAddFeedback.Parameters.AddWithValue("@TraineeName", objTF.tfid);
                    CMDAddFeedback.Parameters.AddWithValue("@FeedbackFile", objTF.FeedbackFile);

                    con.Open();
                    s = CMDAddFeedback.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }           
            return s;
        }
        public int AddFeedbackFile(int TrainingId, string TraineeName, string FileName)
        {
            int k = 0;
            try
            {
                SqlCommand CMDAddFeedback = new SqlCommand("SP_TrAttendance", con);
                CMDAddFeedback.CommandType = CommandType.StoredProcedure;
                CMDAddFeedback.Parameters.AddWithValue("@SP_Type", "6");
                CMDAddFeedback.Parameters.AddWithValue("@FeedbackFile", FileName);
                CMDAddFeedback.Parameters.AddWithValue("@TrainingRecordId", TrainingId);
                CMDAddFeedback.Parameters.AddWithValue("@RTraineeId", TraineeName);

                con.Open();
                k = CMDAddFeedback.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return k;
        }
        public DataSet GetPDFData(TrainingScheduleModel objTFP)
        {
            DataSet DtGetPDFData = new DataSet();
            try
            {
                SqlCommand CMDGetDetails = new SqlCommand("SP_TrAttendance", con);
                CMDGetDetails.CommandType = CommandType.StoredProcedure;
                CMDGetDetails.Parameters.AddWithValue("@SP_Type", "5");
                CMDGetDetails.Parameters.AddWithValue("@RTraineeId", objTFP.tfid);
                CMDGetDetails.Parameters.AddWithValue("@TrainingRecordId", objTFP.PK_TrainingScheduleId);
                con.Open();
                SqlDataAdapter dr = new SqlDataAdapter(CMDGetDetails);
                dr.Fill(DtGetPDFData);
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                    DtGetPDFData.Dispose();
                }
            }
            return DtGetPDFData;
        }
        public DataTable ValidateRecord(int tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrAttendance", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 3);
                CMDGetList.Parameters.AddWithValue("@TrainingRecordId", Convert.ToInt32(tid));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }
        #endregion


        public DataSet GetAccessorName()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet Excel(string id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '9');
                cmd.Parameters.AddWithValue("@TrainingRecordId", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public DataSet ExportFeedBackList(string id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@TrainingRecordId", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet ExportFeedBackList1()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "106");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@RPK_TrainingScheduleId", id);
                Result = cmd.ExecuteNonQuery().ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        public DataSet GetDetails()
        {
            string User = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "101");
                cmd.Parameters.AddWithValue("@TraineeId", User);                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet GetElearningDetails()
        {
            string User = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "102");
                cmd.Parameters.AddWithValue("@TraineeId", User);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataTable GetProposedDate(string tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", "302");
                CMDGetList.Parameters.AddWithValue("@TrainingName", Convert.ToString(tid));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }
        
        public DataTable GetEmpList()//Get All Emp List 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", "301");                
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }

        public DataTable GetBranchEmpList()//Get All Emp List 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", "303");
                CMDGetList.Parameters.AddWithValue("@BranchID", Convert.ToString(System.Web.HttpContext.Current.Session["UserBranchId"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }

        public DataSet GetDdlLst()
        {
            DataSet DTEMDashBoard = new DataSet();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 304);               
                
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }


        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_TrainingID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                ////DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);

                    ////, item.FileContent
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_TrainingUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListTrainingUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        public string InsertTrainingScheduleFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_TrainingID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                ////DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, Convert.ToString(JOB_ID)+'_'+ item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);

                    ////, item.FileContent
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_TrainingScheduleUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListTrainingScheduleUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }


        public DataTable GetAllTraineeIdByName(string n, string obsType, string EmpCat, string TypeOfEmp, string Branch, string TrainType, string UserRole)
        {
            DataTable DSTraineeId = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "305");
                cmd.Parameters.AddWithValue("@obsType", obsType);
                cmd.Parameters.AddWithValue("@EmpCat", EmpCat);
                cmd.Parameters.AddWithValue("@TypeOfEmp", TypeOfEmp);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@TrainType", TrainType);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSTraineeId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSTraineeId.Dispose();
            }
            return DSTraineeId;
        }


        public DataTable GetTrainingDeatils(string trainingID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_TrainingSchedule", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 306);
                CMDEditUsers.Parameters.AddWithValue("@PK_TrainingScheduleId", trainingID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }


        public DataTable GetFeedBackList(int tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrAttendance", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 10);
                CMDGetList.Parameters.AddWithValue("@TrainingRecordId", Convert.ToInt32(tid));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }

        public DataTable GetTrainingStartTimeEndTime(string trainingID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_TrainingSchedule", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 103);
                CMDEditUsers.Parameters.AddWithValue("@TrainingId", trainingID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }


        public int InsertTrainingStartTimeEndTime(TrainingScheduleModel TSM)
        {
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            int ReturnScheduleId = 0;
            string Result = string.Empty;
            con.Open();
            try
            {
                if (TSM.PK_TrainingScheduleId != 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_TrainingSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "104");




                    cmd.Parameters.AddWithValue("@TrainingStartDate", DateTime.ParseExact(TSM.TrainingStartDate, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@TrainingStartTime", TSM.TrainingStartTime);
                    cmd.Parameters.AddWithValue("@TrainingEndTime", TSM.TrainingEndTime);
                    cmd.Parameters.AddWithValue("@TotalHours", TSM.TotalHours);
                    cmd.Parameters.AddWithValue("@PK_TrainingScheduleId", TSM.PK_TrainingScheduleId);
                   
                    

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();

                    //ReturnScheduleId = Convert.ToInt32(cmd.Parameters["@ReturnScheduleId"].Value.ToString());
                }
                

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return ReturnScheduleId;
        }

        public DataTable GetTrainingScheduleFile(string trainingID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_TrainingScheduleUploadedFile", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUsers.Parameters.AddWithValue("@FK_TrainingID", trainingID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_TrainingScheduleUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable ExportQuizeResultExcel(String id)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_Quiz", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 16);
                CMDEditUploadedFile.Parameters.AddWithValue("@TrainingID", id);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable ExportQuizeQ(String id)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_Quiz", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 18);
                CMDEditUploadedFile.Parameters.AddWithValue("@TrainingID", id);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        //public DataSet ExportQuizeResultExcel(string id)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_Quiz", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SP_Type", "16");
        //        cmd.Parameters.AddWithValue("@TrainingID", id);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //    return ds;
        //}



        //public DataSet dtTrainingRecord()
        //{
        //    DataSet DTEMDashBoard = new DataSet();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_TrAttendance", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 12);

        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEMDashBoard.Dispose();
        //    }
        //    return DTEMDashBoard;
        //}

        public DataTable dtTrainingRecord()
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_TrAttendance", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 13);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable dtTrainingRecordData()
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_TrAttendance", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 12);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable dtTrainingRecordDataByDate(TrainingScheduleModel obj)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_TrAttendance", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 13);
                //CMDEditUploadedFile.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(obj.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //CMDEditUploadedFile.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(obj.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //CMDEditUploadedFile.Parameters.AddWithValue("@FromDate", obj.FromDate);
                //CMDEditUploadedFile.Parameters.AddWithValue("@ToDate", obj.ToDate);

                CMDEditUploadedFile.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(obj.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDEditUploadedFile.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(obj.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataSet TrainingRecordExport()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TrAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "14");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        #region Training Record By Id
        public DataTable GetTrainingRecordById(int tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_TrAttendance", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", "15");
                CMDGetList.Parameters.AddWithValue("@TrainingRecordId", Convert.ToInt32(tid));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetList);
                SDAGetDdlLst.Fill(GetTraineeData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                GetTraineeData.Dispose();
            }
            return GetTraineeData;
        }

        public List<TrainingRecordById> TrainingRecordExportToExcel(string PK_TrainingScheduleId)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<TrainingRecordById> lstEnquiryDashB = new List<TrainingRecordById>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_TrAttendance", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "15");
                CMDGetEnquriy.Parameters.AddWithValue("@TrainingRecordId", Convert.ToString(PK_TrainingScheduleId));
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new TrainingRecordById
                           {
                               Count = DTEMDashBoard.Rows.Count,

                               TraineeName = Convert.ToString(dr["TraineeName"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               Email = Convert.ToString(dr["Email"]),
                               MobileNo = Convert.ToString(dr["MobileNo"]),
                               IsPresent = Convert.ToString(dr["IsPresent"]),
                               QuizAttended = Convert.ToString(dr["QuizAttended"]),
                               Score = Convert.ToString(dr["Score"]),
                               Result = Convert.ToString(dr["Result"]),
                               FeedBack = Convert.ToString(dr["FeedBack"]),
                               Attempt = Convert.ToString(dr["Attempt"]),



                           }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }


        #endregion

    }
}