using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class InspectorPerformanceController : Controller
    {
        // GET: InspectorPerformance

        Dalinspectorperformance Dalobj = new Dalinspectorperformance();
        InspectorPerformance obj1 = new InspectorPerformance();
        InspectorWorkload obj2 = new InspectorWorkload();

        public ActionResult Index()
        {
            return View();
        }
        // [HttpPost]

        public ActionResult inspectorPerformance()
        {
            DataTable InspectorIndividual = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDate"] != null && Session["Todate"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FromDate"].ToString());
                obj1.ToDate = Convert.ToString(Session["Todate"].ToString());
                InspectorIndividual = Dalobj.GetData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (InspectorIndividual.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in InspectorIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["FullName"]),
                            Branch_Name = Convert.ToString(dr["BranchName"]),
                            TotalCalls = Convert.ToString(dr["TotalCalls"]),
                            CallClosedCount = Convert.ToString(dr["CallClosedCount"]),
                            CallClosedCountPercentage = Convert.ToString(dr["CallClosedCountPercentage"]),
                            CallOpenCount = Convert.ToString(dr["CallOpenCount"]),
                            CallOpenCountPercentage = Convert.ToString(dr["CallOpenCountPercentage"]),
                            IVRCount = Convert.ToString(dr["IVRCount"]),
                            IVRReqInTUVFormat = Convert.ToString(dr["IVRReqInTUVFormat"]),
                            IVRReqInTUVFormatPercentage = Convert.ToString(dr["IVRReqInTUVFormatPer"]),
                            IVRDownloaded = Convert.ToString(dr["IVRDownloaded"]),
                            IVRDownloadedper = Convert.ToString(dr["IVRDownloadedper"]),
                            ActualIVRGeneratedOnTiimesV = Convert.ToString(dr["ActualIVRGeneratedOnTiimesV"]),
                            ActualIVRGeneratedOnTiimesVper = Convert.ToString(dr["ActualIVRGeneratedOnTiimesVper"]),
                            IVRcreationOnTimeCount = Convert.ToString(dr["IVRcreationOnTimeCount"]),
                            IVRcreationOnTimeCountper = Convert.ToString(dr["IVRcreationOnTimeCountper"]),
                            SupDocAttCount = Convert.ToString(dr["SupDocAttCount"]),
                            SupDocAttCountper = Convert.ToString(dr["SupDocAttPerc"]),
                            
                            IVRRevCount = Convert.ToString(dr["IVRRevCount"]),
                            IVRRevCountper = Convert.ToString(dr["IVRRevCountper"]),
                            Performance = Convert.ToString(dr["Performance"]),

                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;


                    InspectorPerformance grandTotal = new InspectorPerformance();

                    //grandTotal.InspectorName = "Grand Total";
                    //grandTotal.Branch_Name = " ";
                    //grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    //grandTotal.GrandCallClosedCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.CallClosedCount));
                    //grandTotal.GrandCallClosedCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.CallClosedCountPercentage)).Select(item => float.Parse(item.CallClosedCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    //grandTotal.GrandCallOpenCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.CallOpenCount));
                    //grandTotal.GrandCallOpenCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.CallOpenCountPercentage)).Select(item => float.Parse(item.CallOpenCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    //grandTotal.GrandIVRCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRCount));
                    //grandTotal.GrandIVRReqInTUVFormat = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRReqInTUVFormat));
                    //grandTotal.GrandIVRReqInTUVFormatPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRReqInTUVFormatPercentage)).Select(item => float.Parse(item.IVRReqInTUVFormatPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    //grandTotal.GrnadIVRDownloaded = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRDownloaded));
                    //grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRDownloadedper)).Select(item => float.Parse(item.IVRDownloadedper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    //grandTotal.GrnadActualIVRGeneratedOnTiimesV = lstComplaintDashBoard1.Sum(item => int.Parse(item.ActualIVRGeneratedOnTiimesV));
                    //grandTotal.GrnadActualIVRGeneratedOnTiimesVPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.ActualIVRGeneratedOnTiimesVper)).Select(item => float.Parse(item.ActualIVRGeneratedOnTiimesVper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    //grandTotal.GrnadIVRcreationOnTimeCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRcreationOnTimeCount));
                    //grandTotal.GrnadIVRcreationOnTimeCountPercentange = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRcreationOnTimeCountper)).Select(item => float.Parse(item.IVRcreationOnTimeCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    //grandTotal.GrnadSupDocAttCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.SupDocAttCount));
                    //grandTotal.GrnadSupDocAttCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.SupDocAttCountper)).Select(item => float.Parse(item.SupDocAttCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    //grandTotal.GrnadIVRRevCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRRevCount));
                    //grandTotal.GrnadIVRRevCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRRevCountper)).Select(item => float.Parse(item.IVRRevCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 


                    //grandTotal.GrandTotalPerformance = lstComplaintDashBoard1.Sum(item => int.Parse(item.Performance));
                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandCallClosedCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.CallClosedCount));
                    grandTotal.GrandCallClosedCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.CallClosedCountPercentage)).Select(item => float.Parse(item.CallClosedCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandCallOpenCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.CallOpenCount));
                    grandTotal.GrandCallOpenCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.CallOpenCountPercentage)).Select(item => float.Parse(item.CallOpenCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandIVRCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRCount));
                    grandTotal.GrandIVRReqInTUVFormat = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRReqInTUVFormat));
                    //grandTotal.GrandIVRReqInTUVFormatPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRReqInTUVFormatPercentage)).Select(item => float.Parse(item.IVRReqInTUVFormatPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average grandTotal.GrandIVRReqInTUVFormatPercentage = lstComplaintDashBoard1
                    grandTotal.GrandIVRReqInTUVFormatPercentage = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.IVRReqInTUVFormatPercentage))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.IVRReqInTUVFormatPercentage.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {
                            // Handle the parsing error. You can log it or provide a default value.
                            // Here, we provide a default value of 0 for failed parsing.
                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();






                    grandTotal.GrnadIVRDownloaded = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRDownloaded));
                    //grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRDownloadedper)).Select(item => float.Parse(item.IVRDownloadedper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.IVRDownloadedper))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.IVRDownloadedper.TrimEnd('%'));
        }
        catch (FormatException)
        {
            // Handle the parsing error. You can log it or provide a default value.
            // Here, we provide a default value of 0 for failed parsing.
            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrnadActualIVRGeneratedOnTiimesV = lstComplaintDashBoard1.Sum(item => int.Parse(item.ActualIVRGeneratedOnTiimesV));
                    //grandTotal.GrnadActualIVRGeneratedOnTiimesVPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.ActualIVRGeneratedOnTiimesVper)).Select(item => float.Parse(item.ActualIVRGeneratedOnTiimesVper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    grandTotal.GrnadActualIVRGeneratedOnTiimesVPercentage = lstComplaintDashBoard1
   .Where(item => !string.IsNullOrWhiteSpace(item.ActualIVRGeneratedOnTiimesVper))
   .Select(item =>
   {
       try
       {
           return float.Parse(item.ActualIVRGeneratedOnTiimesVper.TrimEnd('%'));
       }
       catch (FormatException)
       {
           // Handle the parsing error. You can log it or provide a default value.
           // Here, we provide a default value of 0 for failed parsing.
           return 0;
       }
   })
   .DefaultIfEmpty(0)
   .Average();
                    grandTotal.GrnadIVRcreationOnTimeCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRcreationOnTimeCount));
                    //grandTotal.GrnadIVRcreationOnTimeCountPercentange = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRcreationOnTimeCountper)).Select(item => float.Parse(item.IVRcreationOnTimeCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    grandTotal.GrnadIVRcreationOnTimeCountPercentange = lstComplaintDashBoard1
   .Where(item => !string.IsNullOrWhiteSpace(item.IVRcreationOnTimeCountper))
   .Select(item =>
   {
       try
       {
           return float.Parse(item.IVRcreationOnTimeCountper.TrimEnd('%'));
       }
       catch (FormatException)
       {
           // Handle the parsing error. You can log it or provide a default value.
           // Here, we provide a default value of 0 for failed parsing.
           return 0;
       }
   })
   .DefaultIfEmpty(0)
   .Average();
                    grandTotal.GrnadSupDocAttCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.SupDocAttCount));

                    //grandTotal.GrnadSupDocAttCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.SupDocAttCountper)).Select(item => float.Parse(item.SupDocAttCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 

                    grandTotal.GrnadSupDocAttCountPercentage = lstComplaintDashBoard1
   .Where(item => !string.IsNullOrWhiteSpace(item.SupDocAttCountper))
   .Select(item =>
   {
       try
       {
           return float.Parse(item.SupDocAttCountper.TrimEnd('%'));
       }
       catch (FormatException)
       {
           // Handle the parsing error. You can log it or provide a default value.
           // Here, we provide a default value of 0 for failed parsing.
           return 0;
       }
   })
   .DefaultIfEmpty(0)
   .Average();
                    //added by shrutika salve 27092023

                    grandTotal.GrnadIVRRevCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IVRRevCount));

                    //grandTotal.GrnadIVRRevCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.IVRRevCountper)).Select(item => float.Parse(item.IVRRevCountper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average 
                    grandTotal.GrnadIVRRevCountPercentage = lstComplaintDashBoard1
  .Where(item => !string.IsNullOrWhiteSpace(item.IVRRevCountper))
  .Select(item =>
  {
      try
      {
          return float.Parse(item.IVRRevCountper.TrimEnd('%'));
      }
      catch (FormatException)
      {
          // Handle the parsing error. You can log it or provide a default value.
          // Here, we provide a default value of 0 for failed parsing.
          return 0;
      }
  })
  .DefaultIfEmpty(0)
  .Average();



                    //grandTotal.GrandTotalPerformance = lstComplaintDashBoard1.Sum(item => int.Parse(item.Performance));

                    grandTotal.GrandTotalPerformance = lstComplaintDashBoard1
  .Where(item => !string.IsNullOrWhiteSpace(item.Performance))
  .Select(item =>
  {
      try
      {
          return float.Parse(item.Performance.TrimEnd('%'));
      }
      catch (FormatException)
      {
          // Handle the parsing error. You can log it or provide a default value.
          // Here, we provide a default value of 0 for failed parsing.
          return 0;
      }
  })
  .DefaultIfEmpty(0)
  .Average();


                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult inspectorPerformance(InspectorPerformance CM)
        {

            Session["FromDate"] = CM.FromDate;
            Session["Todate"] = CM.ToDate;


            return RedirectToAction("inspectorPerformance");
            return View();

        }

        //added by shrutika salve
        public ActionResult IRNinspectorPerformance()
        {

            DataTable InspectorIRNIndividual = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDate1"] != null && Session["Todate1"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FromDate1"].ToString());
                obj1.ToDate = Convert.ToString(Session["Todate1"].ToString());
                InspectorIRNIndividual = Dalobj.GetIRNData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (InspectorIRNIndividual.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in InspectorIRNIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["username"]),
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            IRNCount = Convert.ToString(dr["IRNCount"]),
                            IRNRevCount = Convert.ToString(dr["IRNRevCount"]),
                            IRNRevCountper = Convert.ToString(dr["IRNRevCountPer"]),
                            IRN_Creation_OntimeCount = Convert.ToString(dr["IRNcreationOnTimeCount"]),
                            IRNGeneratedPercentage = Convert.ToString(dr["IRNcreationOnTimeCountPer"]),
                            IRNcreationDelayCount = Convert.ToString(dr["IRNcreationdelayCount"]),
                            IRNGenratedelayPercentage = Convert.ToString(dr["IRNcreationOnTimeCountPer"]),

                            PerformanceIRN = Convert.ToString(dr["IRNinspectorPerformance"]),

                        });
                    }

                    ViewData["ListDataIRN"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandIRNCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IRNCount));

                    grandTotal.GrandIRNRevCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IRNRevCount));

                    grandTotal.GrandIRNRevCountper = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.IRNRevCountper))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.IRNRevCountper.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrnadIRNcreationOnTimeCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IRN_Creation_OntimeCount));

                    //grandTotal.GrnadIRNcreationOnTimeCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IRN_Creation_OntimeCount));

                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.IRN_Creation_OntimeCount))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.IRN_Creation_OntimeCount.TrimEnd('%'));
        }
        catch (FormatException)
        {

            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.IRN_Creation_OntimeCount))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.IRN_Creation_OntimeCount.TrimEnd('%'));
        }
        catch (FormatException)
        {

            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrnadIRNcreationOnTimeCountPercentange = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.IRNGeneratedPercentage))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.IRNGeneratedPercentage.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrnadIRNcreationdelaysCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.IRNcreationDelayCount));


                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.IRNcreationDelayCount))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.IRNcreationDelayCount.TrimEnd('%'));
        }
        catch (FormatException)
        {

            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrnadIrncreationdelayedCountPercentange = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.IRNGenratedelayPercentage))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.IRNGenratedelayPercentage.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();


                    grandTotal.GrandTotalIRNPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.PerformanceIRN))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.PerformanceIRN.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();




                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDate1"]);
                    viewModel.ToDate = Convert.ToString(Session["Todate1"]);

                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult IRNinspectorPerformance(InspectorPerformance CM)
        {

            Session["FromDate1"] = CM.FromDate;
            Session["Todate1"] = CM.ToDate;


            return RedirectToAction("IRNinspectorPerformance");
            return View();

        }

        public ActionResult NCRPerformance()
        {

            DataTable InspectorNCRIndividual = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDate2"] != null && Session["Todate2"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FromDate2"].ToString());
                obj1.ToDate = Convert.ToString(Session["Todate2"].ToString());
                InspectorNCRIndividual = Dalobj.GetNCRData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (InspectorNCRIndividual.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in InspectorNCRIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["inspectorName"]),
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            TotalNCR = Convert.ToString(dr["TotalNCR"]),
                            OpenNCR = Convert.ToString(dr["NCROpen"]),
                            OpenNCRPercentange = Convert.ToString(dr["NCROpenper"]),
                            CloseNCR = Convert.ToString(dr["NCRClose"]),
                            CloseNCRPercentange = Convert.ToString(dr["NCRCloseper"]),

                            PerformanceNCR = Convert.ToString(dr["PerformancePercentage"]),

                        });
                    }

                    ViewData["ListDataNCR"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandTotalNCR = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalNCR));

                    grandTotal.GrandOpenNCR = lstComplaintDashBoard1.Sum(item => int.Parse(item.OpenNCR));

                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.OpenNCR))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.OpenNCR.TrimEnd('%'));
        }
        catch (FormatException)
        {

            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrandOpenNCRPercentange = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.OpenNCRPercentange))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.OpenNCRPercentange.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandCloseNCR = lstComplaintDashBoard1.Sum(item => int.Parse(item.CloseNCR));

                    grandTotal.GrnadIVRDownloadedPercentage = lstComplaintDashBoard1
    .Where(item => !string.IsNullOrWhiteSpace(item.CloseNCR))
    .Select(item =>
    {
        try
        {
            return float.Parse(item.CloseNCR.TrimEnd('%'));
        }
        catch (FormatException)
        {

            return 0;
        }
    })
    .DefaultIfEmpty(0)
    .Average();

                    grandTotal.GrandCloseNCRPercentange = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.CloseNCRPercentange))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.CloseNCRPercentange.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();


                    grandTotal.GrandPerformanceNCR = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.PerformanceNCR))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.PerformanceNCR.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();





                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDate2"]);
                    viewModel.ToDate = Convert.ToString(Session["Todate2"]);

                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult NCRPerformance(InspectorPerformance CM)
        {

            Session["FromDate2"] = CM.FromDate;
            Session["Todate2"] = CM.ToDate;


            return RedirectToAction("NCRPerformance");
            return View();

        }

        //added by shrutika salve
        public ActionResult MonitoringdeatilsinspectorPerformance()
        {

            DataTable MonitoringIndividual = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDate3"] != null && Session["Todate3"] != null)
            {

                obj1.fromD = Convert.ToString(Session["FromDate3"].ToString());
                obj1.ToD = Convert.ToString(Session["Todate3"].ToString());
                MonitoringIndividual = Dalobj.GetMonitoringData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (MonitoringIndividual.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in MonitoringIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["MonitorName"]),
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            OnsiteMonitoringCount = Convert.ToString(dr["OnsiteMonitoringCount"]),
                            onsiteTotalAvg = Convert.ToString(dr["onsiteTotalAvg"]),
                            OffsiteMonitoringCount = Convert.ToString(dr["OffsiteMonitoringCount"]),
                            OffsiteTotalAvg = Convert.ToString(dr["OffsiteTotalAvg"]),
                            MentoringCount = Convert.ToString(dr["MentoringCount"]),
                            MentoringTotalAvg = Convert.ToString(dr["MentoringTotalAvg"]),
                            MonitoringOfMonitorCount = Convert.ToString(dr["MonitoringOfMonitorCount"]),
                            MonitoringOfMonitorTotalAvg = Convert.ToString(dr["MonitoringOfMonitorTotalAvg"]),

                            AVGperformance2 = Convert.ToString(dr["TotalAvg1"]),
                            AVGperformance = Convert.ToString(dr["TotalAvg"]),
                            Totalmonitoring = Convert.ToString(dr["Totalmonitoring"]),
                            MonitoringObservationNoted = Convert.ToString(dr["MonitoringObservationNoted"]),
                            MonitoringObservationNotedPer = Convert.ToString(dr["monitoringPercentage"]),
                            finalAvg = Convert.ToString(dr["finalResult"]),

                        });
                    }

                    ViewData["ListMonitoring"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandOnsiteMonitoringCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.OnsiteMonitoringCount));

                    grandTotal.GrandonsiteTotalAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.onsiteTotalAvg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.onsiteTotalAvg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();
                    grandTotal.GrandOffsiteMonitoringCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.OffsiteMonitoringCount));

                    //grandTotal.GrandOffsiteTotalAvg = lstComplaintDashBoard1.Sum(item => int.Parse(item.OffsiteTotalAvg));
                    grandTotal.GrandOffsiteTotalAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.OffsiteTotalAvg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.OffsiteTotalAvg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();
                    grandTotal.GrandMentoringCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.MentoringCount));

                    //grandTotal.GrandMentoringTotalAvg = lstComplaintDashBoard1.Sum(item => int.Parse(item.MentoringTotalAvg));
                    grandTotal.GrandMentoringTotalAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.MentoringTotalAvg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.MentoringTotalAvg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();
                    grandTotal.GrandMonitoringOfMonitorCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.MonitoringOfMonitorCount));

                    //grandTotal.GrandMonitoringOfMonitorTotalAvg = lstComplaintDashBoard1.Sum(item => int.Parse(item.MonitoringOfMonitorTotalAvg));
                    grandTotal.GrandMonitoringOfMonitorTotalAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.MonitoringOfMonitorTotalAvg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.MonitoringOfMonitorTotalAvg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();




                    grandTotal.GrandAVGperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.AVGperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.AVGperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandAVGperformance2 = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.AVGperformance2))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.AVGperformance2.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandTotalmonitoring = lstComplaintDashBoard1.Sum(item => int.Parse(item.Totalmonitoring));
                    grandTotal.GrandMonitoringObservationNoted = lstComplaintDashBoard1.Sum(item => int.Parse(item.MonitoringObservationNoted));

                    grandTotal.GrandMonitoringObservationNotedPer = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.MonitoringObservationNotedPer))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.MonitoringObservationNotedPer.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();
                    grandTotal.GrandfinalAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.finalAvg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.finalAvg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();




                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDate3"]);
                    viewModel.ToDate = Convert.ToString(Session["Todate3"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult MonitoringdeatilsinspectorPerformance(InspectorPerformance CM)
        {



            Session["FromDate3"] = CM.FromDate;
            Session["Todate3"] = CM.ToDate;


            return RedirectToAction("MonitoringdeatilsinspectorPerformance");
            return View();

        }


        [HttpGet]
        public ActionResult TsFilledoperation()
        {

            DataSet dsAuditorName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<NameCode> lstAuditorNamee = new List<NameCode>();
            dsAuditorName = Dalobj.BindAuditorName();

            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(dsAuditorName.Tables[0].Columns["RoleName"].ToString()),
                                       Code = n.Field<int>(dsAuditorName.Tables[0].Columns["UserRoleID"].ToString())

                                   }).ToList();
            }

            IEnumerable<SelectListItem> AuditorName;
            AuditorName = new SelectList(lstAuditorNamee, "Code", "Name");
            ViewBag.ProjectTypeItems = AuditorName;
            ViewData["AuditorName"] = AuditorName;
            ViewData["AuditorName"] = lstAuditorNamee;



            DataSet dsName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<NameCode1> lstamee = new List<NameCode1>();
            dsName = Dalobj.Employeecategory();

            if (dsName.Tables[0].Rows.Count > 0)
            {
                lstamee = (from n in dsName.Tables[0].AsEnumerable()
                           select new NameCode1()
                           {
                               Name = n.Field<string>(dsName.Tables[0].Columns["Name"].ToString()),
                               //Code = n.Field<string> Convert.ToInt32(dsName.Tables[0].Columns["id"].ToString())
                               Code = n.Field<string>(dsName.Tables[0].Columns["id"].ToString())

                           }).ToList();
            }

            IEnumerable<SelectListItem> EmployeeCategory;
            EmployeeCategory = new SelectList(lstamee, "Code", "Name");
            ViewBag.EmployeeCategory = EmployeeCategory;
            ViewData["EmployeeCategory"] = EmployeeCategory;
            ViewData["EmployeeCategory"] = lstamee;



            var Data2 = Dalobj.EmployementCategory();
            ViewBag.EmployementCategory = new SelectList(Data2, "Id", "EmployementCategory");


            DataTable TsFilled = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDate4"] != null && Session["Todate4"] != null || Session["UserRole"] != null || Session["Employee"] != null)
            {

                obj1.fromD = Convert.ToString(Session["FromDate4"].ToString());
                obj1.ToD = Convert.ToString(Session["Todate4"].ToString());
                obj1.UserRole = Convert.ToString(Session["UserRole"]);
                obj1.Id = Convert.ToString(Session["Employee"]);
                TsFilled = Dalobj.GetTsfilledData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (TsFilled.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in TsFilled.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            Username = Convert.ToString(dr["Username"]),
                            BranchName = Convert.ToString(dr["branchName"]),
                            SelectedPeriodDays = Convert.ToString(dr["SelectedPeriodDays"]),
                            TSFilledDays = Convert.ToString(dr["TSFilledDays"]),
                            Tsperformance = Convert.ToString(dr["TSFilledPercentage"]),


                        });
                    }

                    ViewData["TsData"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandSelectedPeriodDays = lstComplaintDashBoard1.Sum(item => int.Parse(item.SelectedPeriodDays));

                    grandTotal.GrandTSFilledDays = lstComplaintDashBoard1.Sum(item => int.Parse(item.TSFilledDays));

                    grandTotal.GrandTsperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Tsperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Tsperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();




                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDate4"]);
                    viewModel.ToDate = Convert.ToString(Session["Todate4"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);


        }


        [HttpPost]
        public ActionResult TsFilledoperation(InspectorPerformance CM, FormCollection fc)
        {
            string ProList = string.Join(",", fc["BrAuditee1"]);
            CM.UserRole = ProList;

            string itemList = string.Join(",", fc["BrAuditee"]);
            CM.Id = itemList;

            Session["FromDate4"] = CM.FromDate;
            Session["Todate4"] = CM.ToDate;
            Session["UserRole"] = CM.UserRole;
            Session["Employee"] = CM.Id;


            return RedirectToAction("TsFilledoperation");
            return View();

        }

        [HttpGet]
        public ActionResult Finalinspectionperformance()
        {
            DataTable Finaldata = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FinalFromDate"] != null && Session["FinalTodate"] != null)
            {

                obj1.fromD = Convert.ToString(Session["FinalFromDate"].ToString());
                obj1.ToD = Convert.ToString(Session["FinalTodate"].ToString());
                Finaldata = Dalobj.GetFinalinspectorData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (Finaldata.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in Finaldata.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["FullName"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            IVRinspectorperformance = Convert.ToString(dr["IVRPerformance"]),
                            Irninspectorperformance = Convert.ToString(dr["IRNinspectorPerformance"]),
                            NCRinspectorperformance = Convert.ToString(dr["NCRPerformancePercentage"]),
                            MonitoringAveragePerformance = Convert.ToString(dr["TotalAvg"]),
                            TSFilledinpectorPerformance = Convert.ToString(dr["TSFilledPercentage"]),
                            FinalPerformance = Convert.ToString(dr["FinalPerformance"]),
                            TrainingCount = Convert.ToString(dr["PercentageTrainingAVG"]),
                            profileperformance = Convert.ToString(dr["profilePerformancePercentage"]),
                            Lessionlearntperformance = Convert.ToString(dr["ReadPercentage"]),
                            KPI = Convert.ToString(dr["KPI"]),


                        });
                    }

                    ViewData["FinalMonitoring"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandIVRinspectorperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.IVRinspectorperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.IVRinspectorperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandIrninspectorperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Irninspectorperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Irninspectorperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandNCRinspectorperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.NCRinspectorperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.NCRinspectorperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandMonitoringAveragePerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.MonitoringAveragePerformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.MonitoringAveragePerformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandFinalPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.FinalPerformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.FinalPerformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandKPIPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.KPI))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.KPI.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandTsperformance = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.TSFilledinpectorPerformance))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.TSFilledinpectorPerformance.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();

                    grandTotal.GrandTrainingCount = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.TrainingCount))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.TrainingCount.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandprofileCount = lstComplaintDashBoard1
                 .Where(item => !string.IsNullOrWhiteSpace(item.profileperformance))
                 .Select(item =>
                 {
                     try
                     {
                         return float.Parse(item.profileperformance.TrimEnd('%'));
                     }
                     catch (FormatException)
                     {

                         return 0;
                     }
                 })
                 .DefaultIfEmpty(0)
                 .Average();

                    grandTotal.GrandLessonsLearntCount = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.Lessionlearntperformance))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.Lessionlearntperformance.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();



                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FinalFromDate"]);
                    viewModel.ToDate = Convert.ToString(Session["FinalTodate"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult Finalinspectionperformance(InspectorPerformance CM)
        {
            Session["FinalFromDate"] = CM.FromDate;
            Session["FinalTodate"] = CM.ToDate;

            return RedirectToAction("Finalinspectionperformance");

        }

        [HttpGet]
        public ActionResult TraningAttandanceperformance()
        {
            DataTable Traningdata = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDateTraning"] != null && Session["TodateTraning"] != null)
            {

                obj1.fromD = Convert.ToString(Session["FromDateTraning"].ToString());
                obj1.ToD = Convert.ToString(Session["TodateTraning"].ToString());
                Traningdata = Dalobj.GetTraningData(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (Traningdata.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in Traningdata.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            InspectorName = Convert.ToString(dr["UserName"]),
                            BranchName = Convert.ToString(dr["branch_name"]),
                            Arranged = Convert.ToString(dr["arranged"]),
                            Attended = Convert.ToString(dr["TrainingAttended"]),
                            Attendedper = Convert.ToString(dr["PercentageTrainingAttended"]),
                            QuizisMandatoryfor = Convert.ToString(dr["quizmandatory"]),

                            Quizpassed = Convert.ToString(dr["Pass"]),
                            QuizpassedPer = Convert.ToString(dr["PercentagePass"]),

                            Feedbacksubmited = Convert.ToString(dr["Feedback"]),
                            Feedbacksubmitedper = Convert.ToString(dr["PercentageFeedback"]),
                            Avg = Convert.ToString(dr["PercentageFinalAVG"]),



                        });
                    }

                    ViewData["Traning"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandArranged = lstComplaintDashBoard1.Sum(item => int.Parse(item.Arranged));
                    grandTotal.GrandAttended = lstComplaintDashBoard1.Sum(item => int.Parse(item.Attended));
                    grandTotal.GrandAttendedper = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Attendedper))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Attendedper.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();


                    grandTotal.GrandQuizisMandatoryfor = lstComplaintDashBoard1.Sum(item => int.Parse(item.QuizisMandatoryfor));
                    grandTotal.GrandQuizisMandatoryforper = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.QuizisMandatoryforper))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.QuizisMandatoryforper.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();
                    grandTotal.GrandQuizpassed = lstComplaintDashBoard1.Sum(item => int.Parse(item.Quizpassed));


                    grandTotal.GrandFeedbacksubmited = lstComplaintDashBoard1.Sum(item => int.Parse(item.Feedbacksubmited));
                    grandTotal.GrandFeedbacksubmitedper = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Feedbacksubmitedper))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Feedbacksubmitedper.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.Grandavgper = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Avg))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Avg.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();


                    ViewData["GrandTotal"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDateTraning"]);
                    viewModel.ToDate = Convert.ToString(Session["TodateTraning"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);

        }
        [HttpPost]
        public ActionResult TraningAttandanceperformance(InspectorPerformance CM)
        {
            Session["FromDateTraning"] = CM.FromDate;
            Session["TodateTraning"] = CM.ToDate;


            return RedirectToAction("TraningAttandanceperformance");

        }

        [HttpGet]
        public ActionResult LessionLearntPerformance()
        {

            DataTable LessionLearntData = new DataTable();
            List<InspectorPerformance> lstComplaintDashBoard = new List<InspectorPerformance>();


            if (Session["FromDateLessionLearnt"] != null && Session["TodateLessionLearnt"] != null)
            {

                obj1.fromD = Convert.ToString(Session["FromDateLessionLearnt"].ToString());
                obj1.ToD = Convert.ToString(Session["TodateLessionLearnt"].ToString());
                LessionLearntData = Dalobj.GetLessionLearnt(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (LessionLearntData.Rows.Count > 0)
                {
                    List<InspectorPerformance> lstComplaintDashBoard1 = new List<InspectorPerformance>();

                    foreach (DataRow dr in LessionLearntData.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorPerformance
                        {
                            Username = Convert.ToString(dr["UserName"]),
                            BranchName = Convert.ToString(dr["branch_name"]),
                            ShowCount = Convert.ToString(dr["show"]),
                            ReadCount = Convert.ToString(dr["Readcount"]),
                            Lessionlearntperformance = Convert.ToString(dr["ReadPercentage"])



                        });
                    }

                    ViewData["Lessionlearnt"] = lstComplaintDashBoard1;
                    InspectorPerformance grandTotal = new InspectorPerformance();

                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandTotalShowCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.ShowCount));
                    grandTotal.GrandTotalReadCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.ReadCount));
                    grandTotal.GrandLessionlearntperformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Lessionlearntperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Lessionlearntperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    ViewData["GrandTotalLessionlearnt"] = grandTotal;

                    InspectorPerformance viewModel = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDateLessionLearnt"]);
                    viewModel.ToDate = Convert.ToString(Session["TodateLessionLearnt"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorPerformance obj1 = new InspectorPerformance
                    {
                        lstComplaintDashBoard1 = new List<InspectorPerformance>(),
                        GrandTotal = new InspectorPerformance()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);

            return View();
        }


        [HttpPost]
        public ActionResult LessionLearntPerformance(InspectorPerformance IP)
        {
            Session["FromDateLessionLearnt"] = IP.FromDate;
            Session["TodateLessionLearnt"] = IP.ToDate;
            return RedirectToAction("LessionLearntPerformance");
        }

        [HttpGet]
        //added by shrutika salve 07122023
        public ActionResult profileUpdatePerformance()
        {
            DataTable Userdetails = new DataTable();
            List<ProfileUser> lstComplaintDashBoard = new List<ProfileUser>();
            Userdetails = Dalobj.Userdetails();
            try
            {
                if (Userdetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in Userdetails.Rows)
                    {
                        lstComplaintDashBoard.Add(new ProfileUser
                        {
                            UserName = Convert.ToString(dr["UserName"]),
                            MainBranch = Convert.ToString(dr["mainBranch"]),
                            profileDetails = Convert.ToString(dr["PersonalDetails"]),
                            Attachments = Convert.ToString(dr["DocumentUpload"]),
                            EyeTest = Convert.ToString(dr["EyeTest"]),
                            ProffesionalQualificationDetails = Convert.ToString(dr["ProfCertsFill"]),
                            EducationDetails = Convert.ToString(dr["QualificationDetailFill"]),
                            PerformancePercentage = Convert.ToString(dr["PerformancePercentage"])


                        });
                    }

                    ViewData["Userdetails"] = lstComplaintDashBoard;


                }

            }
            catch (Exception E)
            {
                string error = E.Message.ToString();
            }




            return View();
        }


        [HttpGet]
        public ActionResult InspectorWorkLoad()
        {
            DataTable WorkLoad = new DataTable();
            List<InspectorWorkload> lstComplaintDashBoard = new List<InspectorWorkload>();
            if (Session["FromdateWorkload"] != null && Session["ToateWorkload"] != null)
            {
                obj2.Fromdate = Session["FromdateWorkload"].ToString();
                obj2.Todate = Session["ToateWorkload"].ToString();
                WorkLoad = Dalobj.InspectorWorkload(obj2); ;
            }
            else
            {

            }

            string showdata = string.Empty;
            try
            {
                if (WorkLoad.Rows.Count > 0)
                {
                    foreach (DataRow dr in WorkLoad.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new InspectorWorkload
                            {

                                InspectorName = Convert.ToString(dr["Inspector"]),
                                //Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                BranchName = Convert.ToString(dr["Branch"]),
                                DaysCount = Convert.ToString(dr["DayCnt"]),
                                NoofDays = Convert.ToString(dr["NoOFDays"]),
                                Avgdays = Convert.ToString(dr["AverageDays"]),

                            });


                    }

                    ViewData["inspectorwiseData"] = lstComplaintDashBoard;


                    InspectorWorkload grandTotal = new InspectorWorkload();
                    grandTotal.BranchName = "Grand Total";
                    grandTotal.InspectorName = "Grand Total";
                    grandTotal.GrandDaysCount = lstComplaintDashBoard.Sum(item => int.Parse(item.DaysCount));
                    grandTotal.GrandNoofDays = lstComplaintDashBoard.Sum(item => int.Parse(item.NoofDays));

                    grandTotal.GrandAvgdays = lstComplaintDashBoard
                    .Where(item => !string.IsNullOrWhiteSpace(item.Avgdays))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Avgdays.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    ViewData["GrandTotal"] = grandTotal;

                    InspectorWorkload viewModel = new InspectorWorkload
                    {
                        lst1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    viewModel.Fromdate = Convert.ToString(Session["From"]);
                    viewModel.Todate = Convert.ToString(Session["To"]);


                    return View(viewModel);


                }




                else
                {
                    InspectorWorkload obj2 = new InspectorWorkload
                    {
                        lst1 = new List<InspectorWorkload>(),
                        GrandTotal = new InspectorWorkload()
                    };



                    return View(obj2);


                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["Workdata"] = lstComplaintDashBoard;
            obj2.lst1 = lstComplaintDashBoard;

            return View(obj2);
        }

        [HttpPost]
        public ActionResult InspectorWorkLoad(InspectorWorkload IW)
        {
            Session["FromdateWorkload"] = IW.Fromdate;
            Session["ToateWorkload"] = IW.Todate;
            return RedirectToAction("InspectorWorkLoad");

        }



        public ActionResult BranchWorkLoad()
        {

            DataTable BranchSummary = new DataTable();
            List<InspectorWorkload> lstComplaintDashBoard = new List<InspectorWorkload>();


            if (Session["From"] != null && Session["To"] != null)
            {

                obj2.Fromdate = Convert.ToString(Session["From"].ToString());
                obj2.Todate = Convert.ToString(Session["To"].ToString());
                BranchSummary = Dalobj.InspectorWorkloadBranchWise(obj2);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (BranchSummary.Rows.Count > 0)
                {
                    List<InspectorWorkload> lstComplaintDashBoard1 = new List<InspectorWorkload>();

                    foreach (DataRow dr in BranchSummary.Rows)
                    {
                        lstComplaintDashBoard1.Add(new InspectorWorkload
                        {
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            NoofDays = Convert.ToString(dr["NoOfCall"]),
                            Avgdays = Convert.ToString(dr["AverageDays"]),

                        });
                    }

                    ViewData["BranchData"] = lstComplaintDashBoard1;


                    InspectorWorkload grandTotal = new InspectorWorkload();
                    grandTotal.BranchName = "Grand Total";
                    grandTotal.GrandTotalCall = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoofDays));

                    grandTotal.GrandAvg = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Avgdays))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Avgdays.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    ViewData["GrandTotalBranch"] = grandTotal;

                    InspectorWorkload viewModel = new InspectorWorkload
                    {
                        lst1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    viewModel.Fromdate = Convert.ToString(Session["From"]);
                    viewModel.Todate = Convert.ToString(Session["To"]);


                    return View(viewModel);
                }
                else
                {

                    InspectorWorkload obj2 = new InspectorWorkload
                    {
                        lst1 = new List<InspectorWorkload>(),
                        GrandTotal = new InspectorWorkload()
                    };



                    return View(obj2);
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View();
        }



        [HttpPost]
        public ActionResult BranchWorkLoad(InspectorWorkload CM)
        {

            Session["From"] = CM.Fromdate;
            Session["To"] = CM.Todate;

            return RedirectToAction("BranchWorkLoad");
        }



    }
}
    