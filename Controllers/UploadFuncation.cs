using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TuvVision.Controllers
{
    public class UploadFuncation
    {
        public string FileUploadDynamicName(string path, HttpPostedFileBase fu)
        {
            try
            {
                if (fu != null)
                {
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                    string sTime = Convert.ToString(DateTime.Now.Hour) + "" + Convert.ToString(DateTime.Now.Minute) + "" + Convert.ToString(DateTime.Now.Second);
                    string ReportName = fileNameWithoutExtension + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + sTime + System.IO.Path.GetExtension(fu.FileName);
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ReportName;
                    string ImageDirectoryFP = path.Replace("/", "\\");
                    string ImageDirectory = "~/" + path;
                    string fileNameWithExtension = System.IO.Path.GetExtension(ReportName);
                    string ImagePath = "~" + path + ReportName;
                    string ImageName = ReportName;
                    fu.SaveAs(filePath);

                    return ImagePath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}