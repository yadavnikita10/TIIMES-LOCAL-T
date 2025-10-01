using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.ComponentModel.DataAnnotations;

namespace TuvVision.DataAccessLayer
{
    public class DALQuiz
    {
        SqlConnection Con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public int AddQuiz(QuizModel vmQuiz)
        {
            int s = 0;
            Con.Open();
            if (vmQuiz.QuizID == 0)
            {
                try
                {
                    SqlCommand CMD = new SqlCommand("SP_Quiz", Con);
                    CMD.CommandType = CommandType.StoredProcedure;
                    CMD.Parameters.AddWithValue("@SP_Type", "1");
                    CMD.Parameters.AddWithValue("@TrainingId", vmQuiz.TopicName);
                    CMD.Parameters.AddWithValue("@Question", vmQuiz.Question);
                    CMD.Parameters.AddWithValue("@Option1", vmQuiz.Option1);
                    CMD.Parameters.AddWithValue("@Option2", vmQuiz.Option2);
                    CMD.Parameters.AddWithValue("@Option3", vmQuiz.Option3);
                    CMD.Parameters.AddWithValue("@Option4", vmQuiz.Option4);
                    CMD.Parameters.AddWithValue("@Answer", vmQuiz.Answer);

                    s = CMD.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (Con.State != ConnectionState.Closed)
                    {
                        Con.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand CMD = new SqlCommand("SP_Quiz", Con);
                    CMD.CommandType = CommandType.StoredProcedure;
                    CMD.Parameters.AddWithValue("@SP_Type", "2");
                    CMD.Parameters.AddWithValue("@PK_ID", vmQuiz.QuizID);
                    CMD.Parameters.AddWithValue("@TrainingId", vmQuiz.TopicName);
                    CMD.Parameters.AddWithValue("@Question", vmQuiz.Question);
                    CMD.Parameters.AddWithValue("@Option1", vmQuiz.Option1);
                    CMD.Parameters.AddWithValue("@Option2", vmQuiz.Option2);
                    CMD.Parameters.AddWithValue("@Option3", vmQuiz.Option3);
                    CMD.Parameters.AddWithValue("@Option4", vmQuiz.Option4);
                    CMD.Parameters.AddWithValue("@Answer", vmQuiz.Answer);

                    s = CMD.ExecuteNonQuery();
                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (Con.State != ConnectionState.Closed)
                    {
                        Con.Close();
                    }
                }
            }
            return s;
        }
        //public QuizModel GetQuestionByID(int id)
        //{
        //    QuizModel _vmQuiz = new QuizModel();
        //    Con.Open();
        //    try
        //    {
        //        SqlCommand CMDVLinkByID = new SqlCommand("SP_Quiz", Con);
        //        CMDVLinkByID.CommandType = CommandType.StoredProcedure;
        //        CMDVLinkByID.Parameters.AddWithValue("@SP_Type", "3");
        //        CMDVLinkByID.Parameters.AddWithValue("@PK_ID",Convert.ToInt32(id));
        //        SqlDataReader dr = CMDVLinkByID.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            _vmQuiz.QuizID = Convert.ToInt32(dr["@PK_ID"]);
        //            _vmQuiz.TrainingID = Convert.ToInt32(dr["@TrainingID"]);
        //            _vmQuiz.Question = Convert.ToString(dr["@Question"]);
        //            _vmQuiz.Option1 = Convert.ToString(dr["@Option1"]);
        //            _vmQuiz.Option2 = Convert.ToString(dr["@Option2"]);
        //            _vmQuiz.Option3 = Convert.ToString(dr["@Option3"]);
        //            _vmQuiz.Option4 = Convert.ToString(dr["@Option4"]);
        //            _vmQuiz.Answer = Convert.ToString(dr["@Answer"]);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string Error = Ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (Con.State != ConnectionState.Closed)
        //        {
        //            Con.Close();
        //        }
        //    }
        //    return _vmQuiz;
        //}
        public DataTable GetQuestionByID(int aid)
        {
            DataTable DSQuestionId = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3");
                cmd.Parameters.AddWithValue("@PK_ID", Convert.ToInt32(aid));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSQuestionId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSQuestionId.Dispose();
            }
            return DSQuestionId;
        }

        public DataSet GetTrainingByID(int aid)
        {
            DataSet DSQuestionId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "13");
                cmd.Parameters.AddWithValue("@TrainingId", Convert.ToInt32(aid));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSQuestionId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSQuestionId.Dispose();
            }
            return DSQuestionId;
        }

        public DataSet ChkAtt(int aid)
        {
            DataSet DSQuestionId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17N");
                cmd.Parameters.AddWithValue("@TrainingID", Convert.ToInt32(aid));
                cmd.Parameters.AddWithValue("@UserId", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSQuestionId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSQuestionId.Dispose();
            }
            return DSQuestionId;
        }

        public IEnumerable<TopicList> GetTopicList()
        {
            List<TopicList> TopicList = new List<TopicList>();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_Quiz", Con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", "5");
                Con.Open();
                SqlDataReader dr = CMDGetList.ExecuteReader();
                while (dr.Read())
                {
                    TopicList _vmroll = new TopicList();
                    _vmroll.TopicID = Convert.ToInt32(dr["ID"]);
                    _vmroll.Name = Convert.ToString(dr["TrainingName"]);

                    TopicList.Add(_vmroll);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return TopicList;
        }
        public bool DeleteRoll(int id)
        {
            int i = 0;
            SqlCommand CMDDeleteRoll = new SqlCommand("SP_Quiz", Con);
            CMDDeleteRoll.CommandType = CommandType.StoredProcedure;
            CMDDeleteRoll.Parameters.AddWithValue("@SP_Type", 4);
            CMDDeleteRoll.Parameters.AddWithValue("@PK_ID", id);
            Con.Open();
            i = CMDDeleteRoll.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<QuizModel> GetQuestionList(int tid)
        {
            List<QuizModel> QuestionList = new List<QuizModel>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "6N");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", tid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizModel _vmqz = new QuizModel();
                    _vmqz.QuizID = Convert.ToInt32(dr["PK_ID"]);
                    _vmqz.Question = Convert.ToString(dr["Question"]);
                    _vmqz.TrainingID = Convert.ToInt32(dr["FK_TrainingID"]);

                    QuestionList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return QuestionList;
        }
        public IEnumerable<QuizQuestion> GetQuizQuestion(int kid)
        {
            List<QuizQuestion> QuestionList = new List<QuizQuestion>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "6");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", kid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizQuestion _vmqz = new QuizQuestion();
                    _vmqz.TQuizID = Convert.ToInt32(dr["PK_ID"]);
                    _vmqz.TQuestion = Convert.ToString(dr["Question"]);
                    _vmqz.TrainingID = Convert.ToInt32(dr["FK_TrainingID"]);
                    _vmqz.TOption1 = Convert.ToString(dr["Option1"]);
                    _vmqz.TOption2 = Convert.ToString(dr["Option2"]);
                    _vmqz.TOption3 = Convert.ToString(dr["Option3"]);
                    _vmqz.TOption4 = Convert.ToString(dr["Option4"]);
                    QuestionList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return QuestionList;
        }
        public string AddQuizAnswers(QuizModel QZ)
        {
            string Result = string.Empty;
            Con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "8");
                cmd.Parameters.AddWithValue("@PK_ID", QZ.QuizID);
                cmd.Parameters.AddWithValue("@TrainingId", QZ.TrainingID);
                cmd.Parameters.AddWithValue("@Answer", QZ.Answer);
                cmd.Parameters.AddWithValue("@UserId", QZ.UserID);
                Result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return Result;
        }
        public string CalculateResult(QuizModel QZ)
        {
            string Result = string.Empty;
            Con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@TrainingId", QZ.TrainingID);
                cmd.Parameters.AddWithValue("@UserId", QZ.UserID);
                Result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return Result;
        }
        public IEnumerable<QuizModel> GetResultList(int tid)
        {
            List<QuizModel> ResultList = new List<QuizModel>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "10");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", tid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizModel _vmqz = new QuizModel();
                    _vmqz.UserID = Convert.ToString(dr["TraineeName"]);
                    _vmqz.TrainingID = Convert.ToInt32(dr["FK_TrainingID"]);
                    _vmqz.Percent = Convert.ToDecimal(dr["TotalPercent"]);
                    _vmqz.Mobile = Convert.ToString(dr["MobileNo"]);
                    _vmqz.Email = Convert.ToString(dr["Tuv_Email_Id"]);
                    _vmqz.Branch_Name = Convert.ToString(dr["Branch_Name"]);
                    _vmqz.Attempt = Convert.ToString(dr["Attempt"]);
                    ResultList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return ResultList;
        }

        public IEnumerable<QuizModel> GetQList(int tid)
        {
            List<QuizModel> ResultList = new List<QuizModel>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "18");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", tid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizModel _vmqz = new QuizModel();
                    _vmqz.Question = Convert.ToString(dr["Question"]);
                    
                    ResultList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return ResultList;
        }

        public IEnumerable<QuizModel> GetAList(int tid)
        {
            List<QuizModel> ResultList = new List<QuizModel>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "19");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", tid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizModel _vmqz = new QuizModel();
                    _vmqz.Answer = Convert.ToString(dr["Answer"]);

                    ResultList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return ResultList;
        }


        public IEnumerable<QuizModel> GetFeedbackList(int rid)
        {
            List<QuizModel> FeedbackList = new List<QuizModel>();
            try
            {
                SqlCommand CMDGetQnsList = new SqlCommand("SP_Quiz", Con);
                CMDGetQnsList.CommandType = CommandType.StoredProcedure;
                CMDGetQnsList.Parameters.AddWithValue("@SP_Type", "11");
                CMDGetQnsList.Parameters.AddWithValue("@TrainingID", rid);
                Con.Open();
                SqlDataReader dr = CMDGetQnsList.ExecuteReader();
                while (dr.Read())
                {
                    QuizModel _vmqz = new QuizModel();
                    _vmqz.UserID = Convert.ToString(dr["TraineeName"]);
                    _vmqz.Question = Convert.ToString(dr["Rating"]);
                    _vmqz.Answer = Convert.ToString(dr["FeedbackFile"]);
                    _vmqz.TrainingID = Convert.ToInt32(dr["FK_Training_ID"]);
                    FeedbackList.Add(_vmqz);
                }
            }
            catch (Exception Ex)
            {
                string Error = Ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return FeedbackList;
        }
        public DataSet GetData(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@TrainingID", id);                
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
        public DataSet QuizVerification(int aid)
        {
            string User = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet DSTraineeId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@TrainingId", aid);
                cmd.Parameters.AddWithValue("@UserId", User);
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

        public DataTable ExportSetQuizeList(string tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_Quiz", Con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 16);
                CMDGetList.Parameters.AddWithValue("@TrainingID", Convert.ToInt32(tid));
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

        public DataTable ExportResultList(string tid)//Get All DropDownlist 
        {
            DataTable GetTraineeData = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_Quiz", Con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 14);
                CMDGetList.Parameters.AddWithValue("@TrainingID", Convert.ToInt32(tid));
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

        public DataSet GetHeaderQuetion(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "15");
                cmd.Parameters.AddWithValue("@TrainingID", id);
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

        public string InsertEndDate(QuizModel QZ)
        {
            string Result = string.Empty;
            Con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "20");
                cmd.Parameters.AddWithValue("@PK_trainingScheduleId", QZ.PK_TrainingScheduleId);
                if (QZ.QuizeEndDate == null)
                {
                    cmd.Parameters.AddWithValue("@QuizeEndDate", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QuizeEndDate", DateTime.ParseExact(QZ.QuizeEndDate, "dd/MM/yyyy", theCultureInfo));
                }
                if (QZ.QuizStartDate == null)
                {
                    cmd.Parameters.AddWithValue("@QuizeStartDate", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@QuizeStartDate", DateTime.ParseExact(QZ.QuizStartDate, "dd/MM/yyyy", theCultureInfo));
                }



                cmd.Parameters.AddWithValue("@QuizeEndTime", QZ.QuizeEndTime1);


                if (QZ.ReattendQuizStartDate == null)
                {
                    cmd.Parameters.AddWithValue("@ReattendQuizStartDate", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReattendQuizStartDate", DateTime.ParseExact(QZ.ReattendQuizStartDate, "dd/MM/yyyy", theCultureInfo));

                }

                if (QZ.ReattendQuizEndDate == null)
                {
                    cmd.Parameters.AddWithValue("@ReattendQuizEndDate", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ReattendQuizEndDate", DateTime.ParseExact(QZ.ReattendQuizEndDate, "dd/MM/yyyy", theCultureInfo));

                }
                cmd.Parameters.AddWithValue("@QTimeInMinutes", QZ.QTimeInMinutes);

                Result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                }
            }
            return Result;
        }

        public DataSet GetQuizeEndDate(int? aid)
        {
            DataSet DSQuestionId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "21");
                cmd.Parameters.AddWithValue("@PK_trainingScheduleId", Convert.ToInt32(aid));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSQuestionId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSQuestionId.Dispose();
            }
            return DSQuestionId;
        }

        public DataSet ReattemptDatechk(int aid)
        {
            DataSet DSQuestionId = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Quiz", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "22");
                cmd.Parameters.AddWithValue("@TrainingId", Convert.ToInt32(aid));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DSQuestionId);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSQuestionId.Dispose();
            }
            return DSQuestionId;
        }



    }
}