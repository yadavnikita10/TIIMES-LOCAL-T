using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;

public class CommonControl
    {

        public CommonControl()
        {

        }
        public static string FileUpload(string path, HttpPostedFileBase fu)
        {

            if (fu != null)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + fu.FileName;
                string ImageDirectoryFP = path.Replace("/", "\\");
                string ImageDirectory = "~/" + path;
                string ImagePath = "~/" + path + fu.FileName;
                string fileNameWithExtension = System.IO.Path.GetExtension(fu.FileName);
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                string ImageName = fu.FileName;

                int iteration = 1;

                while (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath)))
                {
                    ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    iteration += 1;
                }

                if (iteration == 1)
                {
                    fu.SaveAs(filePath);
                }
                else
                {
                    fu.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                }
                return ImageName;
            }
            else
            {
                return null;
            }

        }

        public static string FileUploadCompress(string path, HttpPostedFileBase fu, int ImgId,string callID)
        {

            if (fu != null)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + fu.FileName;

                string ImageDirectoryFP = path.Replace("/", "\\");
                string ImageDirectory = "~/" + path;
                string ImagePath = "~/" + path + fu.FileName;
                string fileNameWithExtension = System.IO.Path.GetExtension(fu.FileName);
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                string ImageName = callID + "_" + fu.FileName;


                if (ImgId != 0)
                {
                    while (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath)))
                    {

                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath));
                    }
                }


                string CompressfilePath = AppDomain.CurrentDomain.BaseDirectory + "CompressFiles\\" + ImageName;

                fu.SaveAs(filePath);
                GenerateThumbnails(0.5, filePath, CompressfilePath);

                return ImageName;
            }
            else
            {
                return null;
            }

        }

    public static string FileUploadResize(string path, HttpPostedFileBase fu, int ImgId, string callID)
    {
        if (fu != null)
        {
            string ImageDirectoryFP = path.Replace("/", "\\");
            string ImageDirectory = "~/" + path;
            string ImagePath = "~/" + path + fu.FileName;
            string fileNameWithExtension = System.IO.Path.GetExtension(fu.FileName);
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
            string ImageName = callID + "_" + fu.FileName;

            if (ImgId != 0)
            {
                while (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath)))
                {

                    System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath));
                }

            }

            string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + fu.FileName;
            fu.SaveAs(filePath);

            Image originalImage = Image.FromFile(filePath);

            Size newSize = new Size(500, 300);

            Image resizedImage = ResizeImage(originalImage, newSize);

            // Save the resized image with JPEG compression and quality settings
            string CompressfilePath = AppDomain.CurrentDomain.BaseDirectory + "CompressFiles\\" + ImageName;
            SaveImageWithCompression(resizedImage, CompressfilePath, 99); // 99 is the quality level (0-100)

            // Dispose the original and resized images after use
            originalImage.Dispose();
            resizedImage.Dispose();

            return ImageName;
        }
        else
        {
            return null;
        }

    }

    public static Image ResizeImage(Image image, Size newSize)
    {
        // Create a new Bitmap with the desired size
        Bitmap resizedImage = new Bitmap(newSize.Width, newSize.Height);

        // Create a Graphics object from the resized image
        using (Graphics g = Graphics.FromImage(resizedImage))
        {
            // Set the interpolation mode to high quality bicubic
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Draw the original image onto the resized image with the new size
            g.DrawImage(image, new Rectangle(Point.Empty, newSize));
        }

        // Return the resized image
        return resizedImage;
    }

    // SaveImageWithCompression method to save the image with compression and quality settings
    public static void SaveImageWithCompression(Image image, string filename, long quality)
    {
        // Create an EncoderParameters object to set JPEG compression quality
        EncoderParameters encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        // Get the JPEG codec
        ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);



        // Save the image using the JPEG codec and compression quality
        image.Save(filename, jpegCodec, encoderParameters);
    }

    // GetEncoderInfo method to get the codec information for the specified image format
    public static ImageCodecInfo GetEncoderInfo(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }

        // If no matching codec found, return null
        return null;
    }

    public static string FileUploadCompress_old(string path, HttpPostedFileBase fu, int ImgId)
        {

            if (fu != null)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + fu.FileName;

                string ImageDirectoryFP = path.Replace("/", "\\");
                string ImageDirectory = "~/" + path;
                string ImagePath = "~/" + path + fu.FileName;
                string fileNameWithExtension = System.IO.Path.GetExtension(fu.FileName);
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                string ImageName = fu.FileName;

                int iteration = 1;

                while (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath)))
                {
                    ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                    iteration += 1;
                }

                // string CompressfilePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\CompressFiles\\" + ImageName;
                string CompressfilePath = AppDomain.CurrentDomain.BaseDirectory + "CompressFiles\\" + ImageName;

                if (iteration == 1)
                {
                    fu.SaveAs(filePath); //
                }
                else
                {
                    fu.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                }
                GenerateThumbnails(0.5, AppDomain.CurrentDomain.BaseDirectory + filePath, CompressfilePath);

                return ImageName;
            }
            else
            {
                return null;
            }

        }

        private static void GenerateThumbnails(double scaleFactor, string sourcePath, string targetPath)
        {
            try
            {

                // Get a bitmap.
                Bitmap bmp1 = new Bitmap(sourcePath);

                //Or you do can use buil-in method
                //ImageCodecInfo jgpEncoder GetEncoderInfo("image/gif");//"image/jpeg",...
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);


            //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 15L);
            myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(targetPath, jgpEncoder, myEncoderParameters);
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }


        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static string FileUploadDynamicName(string path, HttpPostedFileBase fu)
        {

            if (fu != null)
            {
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                string sTime = Convert.ToString(DateTime.Now.Hour) + "" + Convert.ToString(DateTime.Now.Minute) + "" + Convert.ToString(DateTime.Now.Second);
                string ReportName = fileNameWithoutExtension + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + sTime + ".pdf";
                string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ReportName;
                string ImageDirectoryFP = path.Replace("/", "\\");
                string ImageDirectory = "~/" + path;
                string ImagePath = "~/" + path + fu.FileName;
                string fileNameWithExtension = System.IO.Path.GetExtension(ReportName);

                string ImageName = ReportName;
                fu.SaveAs(filePath);
                //int iteration = 1;

                //while (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(ImagePath)))
                //{
                //    ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                //    filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                //    ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                //    iteration += 1;
                //}

                //if (iteration == 1)
                //{
                //    fu.SaveAs(filePath);
                //}
                //else
                //{
                //    fu.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                //}
                return ImageName;
            }
            else
            {
                return null;
            }

        }
        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        internal static string FileUpload(string v, string signature)
        {
            throw new NotImplementedException();
        }
		
		public static string Wrap(string singleLineString, int columns)
    {
        if (singleLineString == null)
            throw new ArgumentNullException("singleLineString");
        if (columns < 1)
            throw new ArgumentException("'columns' must be greater than 0.");

        var rows = Math.Ceiling((double)singleLineString.Length / columns);
        if (rows < 2) return singleLineString;

        var sb = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            if (i > 0) sb.Append(Environment.NewLine);
            var index = i * columns;
            var length = Math.Min(columns, singleLineString.Length - index);
            var line = singleLineString.Substring(index, length);
            sb.Append(line);
        }

        return sb.ToString();
    }

    public void SaveSign(List<FileDetails> lstFileDtls, int ID)
    {
        foreach (var item in lstFileDtls)
        {
            string savePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SIGN/") + item.FileName + item.Extension;
            System.IO.File.WriteAllBytes(savePath, item.FileContent);
        }
    }


    public void SaveFileToPhysicalLocation1(List<FileDetails> lstFileDtls, string ID)
    {
        foreach (var item in lstFileDtls)
        {


            string a = DateTime.Now.Month.ToString();
            int intC = Convert.ToInt32(a);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);




            string CurrentYear = DateTime.Now.Year.ToString();
            string pathYear = "~/Content/" + CurrentYear;
            string pathMonth = "~/Content/" + CurrentMonth;
            string FinalPath = "~/Content/" + CurrentYear + '/' + CurrentMonth;
            string FinalPath1 = "Content\\" + CurrentYear + '\\' + CurrentMonth + '\\';

            if (!Directory.Exists(pathYear))
            {
                //Directory.CreateDirectory(CurrentYear);
                // Directory.CreateDirectory(Server.MapPath("~/Content/" + CurrentYear));
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/" + CurrentYear));


                if (!Directory.Exists(FinalPath))
                {
                    //Create Final Path
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(FinalPath));


                    //Save File
                    //string savePath = (FinalPath1 + ID + '_' + item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), FinalPath1 + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
                else
                {
                    //string savePath = (FinalPath1 + ID + '_'+ item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, FinalPath1 + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
            }
            else
            {
                if (!Directory.Exists(FinalPath))
                {
                    Directory.CreateDirectory(pathYear);
                }
                else
                {
                    string savePath = (FinalPath + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }

            }

            //string path = "~/Content/"+ CurrentYear;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}


            //string savePath = (@"D:\Content\JobDocument\" + item.FileName);
            //System.IO.File.WriteAllBytes(savePath, item.FileContent);
        }
    }


    public void SaveFileToPhysicalLocation(List<FileDetails> lstFileDtls, int ID)
    {
        foreach (var item in lstFileDtls)
        {


            string a = DateTime.Now.Month.ToString();
            int intC = Convert.ToInt32(a);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);




            string CurrentYear = DateTime.Now.Year.ToString();
            string pathYear = "~/Content/" + CurrentYear;
            string pathMonth = "~/Content/" + CurrentMonth;
            string FinalPath = "~/Content/" + CurrentYear + '/' + CurrentMonth;
            string FinalPath1 = "Content\\" + CurrentYear + '\\' + CurrentMonth + '\\';

            if (!Directory.Exists(pathYear))
            {
                //Directory.CreateDirectory(CurrentYear);
                // Directory.CreateDirectory(Server.MapPath("~/Content/" + CurrentYear));
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/" + CurrentYear));


                if (!Directory.Exists(FinalPath))
                {
                    //Create Final Path
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(FinalPath));


                    //Save File
                    //string savePath = (FinalPath1 + ID + '_' + item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), FinalPath1 + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
                else
                {
                    //string savePath = (FinalPath1 + ID + '_'+ item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, FinalPath1 + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
            }
            else
            {
                if (!Directory.Exists(FinalPath))
                {
                    Directory.CreateDirectory(pathYear);
                }
                else
                {
                    string savePath = (FinalPath + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }

            }

            //string path = "~/Content/"+ CurrentYear;
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}


            //string savePath = (@"D:\Content\JobDocument\" + item.FileName);
            //System.IO.File.WriteAllBytes(savePath, item.FileContent);
        }
    }

    public string GetUniqueID()
    {

        {
            Random objRandom = new Random();
            string UniqueID = string.Empty;
            UniqueID = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                + DateTime.Now.Millisecond.ToString() + DateTime.Now.Ticks.ToString() + objRandom.Next(DateTime.Now.Millisecond).ToString();
            return UniqueID;
        }

    }


    public async Task<string> SignPdfWithDigitalSignature(string filePath, string SignLoc, string signannotation, string pfxId, string pfxPwd, string apiKey, string XYAxis, string ReportNo)
    {
        //string apiUrl = "http://10.10.10.46:8084/ws/v1/signpdf"; 
        string apiUrl = "http://10.10.10.46:8084/ws/v1/signpdflocs"; 
        
        string timestamp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");
        string checksum = GenerateChecksum(apiKey, timestamp);  // Implement checksum logic

        

        using (var client = new HttpClient())
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Add required fields
                //formData.Add(new StringContent(pfxId), "pfxid");
                //formData.Add(new StringContent(pfxPwd), "pfxpwd");
                formData.Add(new StringContent("tuv01"), "pfxid");
                formData.Add(new StringContent("Tc%9pxL77vc7yorqHgk="), "pfxpwd");

                formData.Add(new StringContent(timestamp), "timestamp");
                formData.Add(new StringContent(checksum), "checksum");

                // Set the signloc with anchor and offset
                //string signloc = "{\"anchor\":\"TUV India representative:\",\"offset\":\"[-2:-80]\"}";


                //string signloc = "{\"anchor\":\"TUV India representative:\",\"offset\":\"[-2:-80]\"}";
                //string signloc = "{\"anchor\":\"" + SignLoc + "\",\"offset\":\"[-2:-80]\"}";
                string signloc = "{\"anchor\":\"" + SignLoc + "\",\"offset\":\"" + XYAxis + "\"}";
                formData.Add(new StringContent(signloc), "signloc");

                // Add the signannotation parameter
                
                formData.Add(new StringContent(signannotation), "signannotation");

                // Add the descriptor parameter to send unique value

                //string descriptor = ReportNo;
                //formData.Add(new StringContent(descriptor), "descriptor");

                // Attach the generated PDF file
                byte[] fileBytes = File.ReadAllBytes(filePath);
                formData.Add(new ByteArrayContent(fileBytes), "uploadfile", Path.GetFileName(filePath));

                // Send request to the API
                HttpResponseMessage response = await client.PostAsync(apiUrl, formData);
                if (response.IsSuccessStatusCode)
                {
                    // Get signed PDF
                    byte[] signedPdf = await response.Content.ReadAsByteArrayAsync();
                    //string signedFilePath = Path.Combine(Path.GetDirectoryName(filePath), "Signed_" + Path.GetFileName(filePath));
                    string signedFilePath = Path.Combine(Path.GetDirectoryName(filePath),  Path.GetFileName(filePath));
                    File.WriteAllBytes(signedFilePath, signedPdf);
                    return signedFilePath;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Error signing file: {errorMessage}";
                }
            }
        }
    }

    public async Task<string> SignPdfWithDigitalSignature1(string filePath, string SignLoc, string signannotation, string pfxId, string pfxPwd, string apiKey, string XYAxis)
    {
        //string apiUrl = "http://10.10.10.46:8084/ws/v1/signpdf"; // Replace <hostname> and <port>
        string apiUrl = "http://10.10.10.46:8084/ws/v1/signpdflocs"; // Replace <hostname> and <port>

        string timestamp = DateTime.Now.ToString("ddMMyyyyHH:mm:ss");
        string checksum = GenerateChecksum(apiKey, timestamp);  // Implement checksum logic



        using (var client = new HttpClient())
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Add required fields
                //formData.Add(new StringContent(pfxId), "pfxid");
                //formData.Add(new StringContent(pfxPwd), "pfxpwd");
                formData.Add(new StringContent("tuv01"), "pfxid");
                formData.Add(new StringContent("Tc%9pxL77vc7yorqHgk="), "pfxpwd");

                formData.Add(new StringContent(timestamp), "timestamp");
                formData.Add(new StringContent(checksum), "checksum");

                // Set the signloc with anchor and offset
                //string signloc = "{\"anchor\":\"TUV India representative:\",\"offset\":\"[-2:-80]\"}";


                //string signloc = "{\"anchor\":\"TUV India representative:\",\"offset\":\"[-2:-80]\"}";
                //string signloc = "{\"anchor\":\"" + SignLoc + "\",\"offset\":\"[-2:-80]\"}";
                string signloc = "{\"anchor\":\"" + SignLoc + "\",\"offset\":\"" + XYAxis + "\"}";
                formData.Add(new StringContent(signloc), "signloc");

                // Add the signannotation parameter

                formData.Add(new StringContent(signannotation), "signannotation");

                // Attach the generated PDF file
                byte[] fileBytes = File.ReadAllBytes(filePath);
                formData.Add(new ByteArrayContent(fileBytes), "uploadfile", Path.GetFileName(filePath));

                // Send request to the API
                HttpResponseMessage response = await client.PostAsync(apiUrl, formData);
                if (response.IsSuccessStatusCode)
                {
                    // Get signed PDF
                    byte[] signedPdf = await response.Content.ReadAsByteArrayAsync();
                    //string signedFilePath = Path.Combine(Path.GetDirectoryName(filePath), "Signed_" + Path.GetFileName(filePath));
                    string signedFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
                    File.WriteAllBytes(signedFilePath, signedPdf);
                    return signedFilePath;
                    
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Error signing file: {errorMessage}";
                }
            }
        }
    }

    public static string GenerateChecksum(string apiKey, string timestamp)
    {
        using (MD5 md5 = MD5.Create())
        {
            string data = apiKey + timestamp;
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString().Substring(0, 16).ToUpper();
        }
    }

    public void SaveExpediting(List<FileDetails> lstFileDtls, int ID)
    {
        foreach (var item in lstFileDtls)
        {


            string a = DateTime.Now.Month.ToString();
            int intC = Convert.ToInt32(a);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);




            string CurrentYear = DateTime.Now.Year.ToString();
            string pathYear = "~/Content/ExpeditingAttachement/" + CurrentYear;
            string pathMonth = "~/Content/ExpeditingAttachement/" + CurrentMonth;
            string FinalPath = "~/Content/ExpeditingAttachement/" + CurrentYear + '/' + CurrentMonth;
            string FinalPath1 = "Content\\ExpeditingAttachement\\" + CurrentYear + '\\' + CurrentMonth + '\\';

            if (!Directory.Exists(pathYear))
            {
                //Directory.CreateDirectory(CurrentYear);
                // Directory.CreateDirectory(Server.MapPath("~/Content/" + CurrentYear));
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/ExpeditingAttachement" + CurrentYear));


                if (!Directory.Exists(FinalPath))
                {
                    //Create Final Path
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(FinalPath));


                    //Save File
                    //string savePath = (FinalPath1 + ID + '_' + item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), FinalPath1 + item.FileName + item.Extension);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
                else
                {
                    //string savePath = (FinalPath1 + ID + '_'+ item.FileName);
                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, FinalPath1 + ID + item.FileName + item.Extension);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }
            }
            else
            {
                if (!Directory.Exists(FinalPath))
                {
                    Directory.CreateDirectory(pathYear);
                }
                else
                {
                    string savePath = (FinalPath + ID + '_' + item.FileName);
                    System.IO.File.WriteAllBytes(savePath, item.FileContent);
                }

            }


        }

    }

    //Get IP Address
    public string GetUserIPAddress(HttpRequestBase request)
    {
        string ipAddress = string.Empty;

        // Check if the request has an HTTP_X_FORWARDED_FOR header (for proxies)
        if (request.Headers["HTTP_X_FORWARDED_FOR"] != null)
        {
            ipAddress = request.Headers["HTTP_X_FORWARDED_FOR"];
        }
        else
        {
            // Use ServerVariables to fetch the REMOTE_ADDR or UserHostAddress
            ipAddress = request.ServerVariables["REMOTE_ADDR"];
        }

        // Fallback to UserHostAddress if other methods fail
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = request.UserHostAddress;
        }

        return ipAddress;
    }


}

