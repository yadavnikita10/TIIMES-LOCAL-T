using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class InspectorPerformance
    {

        public List<InspectorPerformance> listinginspector { get; set; }

        public List<InspectorPerformance> lstComplaintDashBoard1 { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InspectorName { get; set; }
        public string TotalCalls { get; set; }
        public string CallClosedCount { get; set; }
        public string CallClosedCountPercentage { get; set; }
        public string CallOpenCount { get; set; }
        public string CallOpenCountPercentage { get; set; }
        public string IVRCount { get; set; }
        public string IVR_Creation_OntimeCount { get; set; }
        public string IVRGeneratedPercentage { get; set; }
        public string IVRcreationDelayCount { get; set; }
        public string IVRGenratedelayPercentage { get; set; }
        public string IVRReqInTUVFormat { get; set; }
        public string IVRReqInTUVFormatPercentage { get; set; }
        public string IVRcreationOnTimeCount { get; set; }
        public string IVRReqCutSpeFormat { get; set; }
        public string IVRReqCutSpeFormatPercentage { get; set; }
        public string IVRGeneratedOnTimeV { get; set; }
        public string IVRGeneratedDelayV { get; set; }
        public string ActualIVRGeneratedOnTiimesV { get; set; }

        public string IVRDownloaded { get; set; }
        public string IVRDownloadedper { get; set; }
        public string ActualIVRGeneratedOnTiimesVper { get; set; }
        public string IVRcreationOnTimeCountper { get; set; }


        //public double PercentageOfIVRGeneratedOnTime { get; set; }

        //public double PercentageOfIVRNotGeneratedOnTime { get; set; }

        public string PercentageOfIVRGeneratedOnTime { get; set; }

        public string PercentageOfIVRNotGeneratedOnTime { get; set; }
        public string ActualIVRNotGeneratedOnTiimesV { get; set; }

        public string SupDocAttCount { get; set; }
        public string SupDocAttCountper { get; set; }


        public int GrandTotalTotalCalls { get; set; }
        public int GrandCallClosedCount { get; set; }
        public float GrandCallClosedCountPercentage { get; set; }
        public int GrandCallOpenCount { get; set; }
        public float GrandCallOpenCountPercentage { get; set; }
        public int GrandIVRCount { get; set; }
        public int GrandIVRReqInTUVFormat { get; set; }
        public float GrandIVRReqInTUVFormatPercentage { get; set; }
        public int GrnadIVRDownloaded { get; set; }
        public float GrnadIVRDownloadedPercentage { get; set; }
        public int GrnadActualIVRGeneratedOnTiimesV { get; set; }
        public float GrnadActualIVRGeneratedOnTiimesVPercentage { get; set; }
        public int GrnadIVRcreationOnTimeCount { get; set; }
        public float GrnadIVRcreationOnTimeCountPercentange { get; set; }
        public int GrnadSupDocAttCount { get; set; }
        public float GrnadSupDocAttCountPercentage { get; set; }
        public float GrandTotalPerformance { get; set; }

        public InspectorPerformance GrandTotal { get; set; }


        public string Performance { get; set; }

        //added by shrutika salve 27092023

        public string IVRRevCount { get; set; }
        public string IVRRevCountper { get; set; }

        public int GrnadIVRRevCount { get; set; }
        public float GrnadIVRRevCountPercentage { get; set; }
        public string Branch_Name { get; set; }
        public string BranchName { get; set; }

        //addded by shrutika salve 

        public string IRNRevCount { get; set; }
        public string IRNRevCountper { get; set; }


        public string IRNCount { get; set; }
        public string IRN_Creation_OntimeCount { get; set; }
        public string IRNGeneratedPercentage { get; set; }
        public string IRNcreationDelayCount { get; set; }
        public string IRNGenratedelayPercentage { get; set; }

        public int GrandIRNRevCount { get; set; }
        public float GrandIRNRevCountper { get; set; }

        public int GrandIRNCount { get; set; }
        public int GrnadIRNcreationOnTimeCount { get; set; }
        public float GrnadIRNcreationOnTimeCountPercentange { get; set; }

        public int GrnadIRNcreationdelaysCount { get; set; }
        public float GrnadIrncreationdelayedCountPercentange { get; set; }
        public float GrandTotalIRNPerformance { get; set; }
        public string PerformanceIRN { get; set; }



        public string TotalNCR { get; set; }
        public string OpenNCR { get; set; }
        public string OpenNCRPercentange { get; set; }
        public string CloseNCR { get; set; }
        public string CloseNCRPercentange { get; set; }
        public string PerformanceNCR { get; set; }



        public int GrandTotalNCR { get; set; }
        public int GrandOpenNCR { get; set; }
        public float GrandOpenNCRPercentange { get; set; }
        public int GrandCloseNCR { get; set; }
        public float GrandCloseNCRPercentange { get; set; }
        public float GrandPerformanceNCR { get; set; }

        //addded by shrutika salve 27102023
        public string OnsiteMonitoringCount { get; set; }
        public string onsiteTotalAvg { get; set; }
        public string OffsiteMonitoringCount { get; set; }
        public string OffsiteTotalAvg { get; set; }
        public string MentoringCount { get; set; }
        public string MentoringTotalAvg { get; set; }
        public string MonitoringOfMonitorCount { get; set; }
        public string MonitoringOfMonitorTotalAvg { get; set; }


        public string AVGperformance { get; set; }
        public string AVGperformance2 { get; set; }

        public int GrandOnsiteMonitoringCount { get; set; }
        public float GrandonsiteTotalAvg { get; set; }
        public int GrandOffsiteMonitoringCount { get; set; }
        public float GrandOffsiteTotalAvg { get; set; }
        public int GrandMentoringCount { get; set; }
        public float GrandMentoringTotalAvg { get; set; }
        public int GrandMonitoringOfMonitorCount { get; set; }
        public float GrandMonitoringOfMonitorTotalAvg { get; set; }
        public float GrandAVGperformance { get; set; }
        public float GrandAVGperformance2 { get; set; }

        public string fromD { get; set; }
        public string ToD { get; set; }



        public string Username { get; set; }
        public string SelectedPeriodDays { get; set; }
        public string TSFilledDays { get; set; }
        public string Tsperformance { get; set; }


        public int GrandSelectedPeriodDays { get; set; }
        public int GrandTSFilledDays { get; set; }
        public float GrandTsperformance { get; set; }
        public string UserRole { get; set; }
        public string Id { get; set; }
        public string EmployementCategory { get; set; }


        //added by shrutika salve 30102023
        public string IVRinspectorperformance { get; set; }
        public string Irninspectorperformance { get; set; }
        public string NCRinspectorperformance { get; set; }
        public string MonitoringAveragePerformance { get; set; }
        public string TSFilledinpectorPerformance { get; set; }
        public string FinalPerformance { get; set; }


        public float GrandIVRinspectorperformance { get; set; }
        public float GrandIrninspectorperformance { get; set; }
        public float GrandNCRinspectorperformance { get; set; }
        public float GrandMonitoringAveragePerformance { get; set; }
        public float GrandTSFilledinpectorPerformance { get; set; }
        public float GrandFinalPerformance { get; set; }

        public string KPI { get; set; }
        public float GrandKPIPerformance { get; set; }


        //added by shrutika salve 06112023
        public string Arranged { get; set; }
        public string Attended { get; set; }
        public string Attendedper { get; set; }
        public string QuizisMandatoryfor { get; set; }
        public string QuizisMandatoryforper { get; set; }
        public string Quizpassed { get; set; }
        public string QuizpassedPer { get; set; }
        public string Feedbacksubmited { get; set; }
        public string Feedbacksubmitedper { get; set; }
        public string Avg { get; set; }
        public string avgper { get; set; }


        public int GrandArranged { get; set; }
        public int GrandAttended { get; set; }
        public float GrandAttendedper { get; set; }
        public int GrandQuizisMandatoryfor { get; set; }
        public float GrandQuizisMandatoryforper { get; set; }
        public int GrandQuizpassed { get; set; }
        public float GrandQuizpassedPer { get; set; }
        public int GrandFeedbacksubmited { get; set; }
        public float GrandFeedbacksubmitedper { get; set; }
        public int GrandAvg { get; set; }
        public float Grandavgper { get; set; }

        public string TrainingCount { get; set; }
        public float GrandTrainingCount { get; set; }


        public string Totalmonitoring { get; set; }
        public string MonitoringObservationNoted { get; set; }
        public string MonitoringObservationNotedPer { get; set; }
        public string finalAvg { get; set; }


        public int GrandTotalmonitoring { get; set; }
        public int GrandMonitoringObservationNoted { get; set; }
        public float GrandMonitoringObservationNotedPer { get; set; }
        public float GrandfinalAvg { get; set; }

        //added by shrutika salve 06122023
        public string ShowCount { get; set; }
        public string ReadCount { get; set; }
        public string Lessionlearntperformance { get; set; }

        public int GrandTotalShowCount { get; set; }
        public int GrandTotalReadCount { get; set; }
        public float GrandLessionlearntperformance { get; set; }

        //added by shrutika salve 11122023
        public string profileperformance { get; set; }

        public float GrandprofileCount { get; set; }
        public float GrandLessonsLearntCount { get; set; }



    }

    public class customerFeedbackregister
    {

        public string RequestCreatedBy { get; set; }
        public string Branch_Name { get; set; }
        public string RequestDate { get; set; }
        public string CompanyName { get; set; }
        public string JobNo { get; set; }
        public string ResponseRequiredDate { get; set; }
        public string NameOfOrg { get; set; }
        public string NameOfRespondant { get; set; }
        public string DesignationOfRespondant { get; set; }
        public string AddressOfOrg { get; set; }
        public string FeedbackRemark { get; set; }
        public string Emailid { get; set; }
        public string QNo1 { get; set; }
        public string QNo2 { get; set; }
        public string QNo3 { get; set; }
        public string QNo4 { get; set; }
        public string QNo5 { get; set; }
        public string QNo6 { get; set; }
        public string QNo7 { get; set; }
        public string QNo8 { get; set; }
        public string QNo9 { get; set; }
        public string QNo10 { get; set; }
        public string QNo11 { get; set; }
        public string QNo12 { get; set; }
        public string QNo13 { get; set; }
        public string QNo14 { get; set; }
        public string QNo15 { get; set; }
        public string QNo16 { get; set; }
        public string QNo17 { get; set; }
        public string QNo18 { get; set; }
        public string QNo19 { get; set; }
        public string QNo20 { get; set; }
        public List<customerFeedbackregister> lstCustomerFeedback { get; set; }
        public string ResponseFilledDate { get; set; }



        


    }

    public class InspectorWorkload
    {
        public List<InspectorWorkload> lst1 { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string InspectorName { get; set; }
        public string BranchName { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string DaysCount { get; set; }
        public string NoofDays { get; set; }
        public string Avgdays { get; set; }


        public int GrandTotalCall { get; set; }
        public float GrandAvg { get; set; }

        public int GrandDaysCount { get; set; }
        public int GrandNoofDays { get; set; }
        public float GrandAvgdays { get; set; }

        public InspectorWorkload GrandTotal { get; set; }

    }
}