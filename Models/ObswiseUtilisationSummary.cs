using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ObswiseUtilisationSummary
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UserRole { get; set; }
        public string Id { get; set; }
        public string EmployementCategory { get; set; }
        
        public string RoleName { get; set; }


        public string  branchName { get; set; }

        public int ExpectedWork { get; set; }
        public int PTIEMPCount { get; set; }
        public int PTIexceptedWork { get; set; }
        public int PTIactuallywork { get; set; }

        public string PercentagePTIexceptedWork { get; set; }

        public int ISETEMPCount { get; set; }
        public int ISETexceptedWork { get; set; }
        public int ISETMactuallywork { get; set; }
        public string PercentageISETexceptedWork { get; set; }

        public int ISIMEMPCount { get; set; }
        public int ISIMexceptedWork { get; set; }
        public int ISIMactuallywork { get; set; }
        public string PercentageISIMexceptedWork { get; set; }


        public int ISIMSPEMPCount { get; set; }
        public int ISIMSPexceptedWork { get; set; }
        public int ISIMSPactuallywork { get; set; }
        public string PercentageISIMSPexceptedWork { get; set; }

        public int ISGBEMPCount { get; set; }
        public int ISGBexceptedWork { get; set; }
        public int ISGBactuallywork { get; set; }
        public string PercentageISGBexceptedWork { get; set; }

        public int ISGREMPCount { get; set; }
        public int ISGRexceptedWork { get; set; }
        public int ISGRactuallywork { get; set; }
        public string PercentageISGRexceptedWork { get; set; }

        public int PCGEMPCount { get; set; }
        public int PCGexceptedWork { get; set; }
        public int PCGactuallywork { get; set; }
        public string PercentagePCGexceptedWork { get; set; }

        public int PCEMPCount { get; set; }
        public int PCexceptedWork { get; set; }
        public int PCactuallywork { get; set; }
        public string PercentagePCexceptedWork { get; set; }

        public int DVSEMPCount { get; set; }
        public int DVSexceptedWork { get; set; }
        public int DVSactuallywork { get; set; }
        public string PercentageDVSexceptedWork { get; set; }

        public int EMPcount { get; set; }
        public int EMPexceptedWork { get; set; }
        public int EMPactuallywork { get; set; }
        public string PercentageEMPexceptedWork { get; set; }


        public int GrandExpectedWork { get; set; }
        public int GrandPTIEMPCount { get; set; }
        public int GrandPTIexceptedWork { get; set; }
        public int GrandPTIactuallywork { get; set; }

        public float GrandPercentagePTIexceptedWork { get; set; }

        public int GrandISETEMPCount { get; set; }
        public int GrandISETexceptedWork { get; set; }
        public int GrandISETMactuallywork { get; set; }
        public float GrandPercentageISETexceptedWork { get; set; }


        public int GrandISETSPEMPCount { get; set; }
        public int GrandISETSPexceptedWork { get; set; }
        public int GrandISETMSPactuallywork { get; set; }
        public float GrandPercentageISETSPexceptedWork { get; set; }


        public int GrandISIMEMPCount { get; set; }
        public int GrandISIMexceptedWork { get; set; }
        public int GrandISIMactuallywork { get; set; }
        public float GrandPercentageISIMexceptedWork { get; set; }

        public int GrandISGBEMPCount { get; set; }
        public int GrandISGBexceptedWork { get; set; }
        public int GrandISGBactuallywork { get; set; }
        public float GrandPercentageISGBexceptedWork { get; set; }

        public int GrandISGREMPCount { get; set; }
        public int GrandISGRexceptedWork { get; set; }
        public int GrandISGRactuallywork { get; set; }
        public float GrandPercentageISGRexceptedWork { get; set; }

        public int GrandPCGEMPCount { get; set; }
        public int GrandPCGexceptedWork { get; set; }
        public int GrandPCGactuallywork { get; set; }
        public float GrandPercentagePCGexceptedWork { get; set; }

        public int GrandPCEMPCount { get; set; }
        public int GrandPCexceptedWork { get; set; }
        public int GrandPCactuallywork { get; set; }
        public float GrandPercentagePCexceptedWork { get; set; }

        public int GrandDVSEMPCount { get; set; }
        public int GrandDVSexceptedWork { get; set; }
        public int GrandDVSactuallywork { get; set; }
        public float GrandPercentageDVSexceptedWork { get; set; }

        public int GrandEMPcount { get; set; }
        public int GrandEMPexceptedWork { get; set; }
        public int GrandEMPactuallywork { get; set; }
        public float GrandPercentageEMPexceptedWork { get; set; }

        //added by shrutika salve 19102023
        public int GrandTotalPT_ISIMEMPCount { get; set; }
        public int GrandTotalPT_ISIMexceptedWork { get; set; }
        public int GrandTotalPT_ISIMactuallywork { get; set; }
        public float GrandPercentageTotalPT_ISIMexceptedWork { get; set; }


        //added by shrutika salve 19102023
        public int TotalPT_ISIMEMPCount { get; set; }
        public int TotalPT_ISIMexceptedWork { get; set; }
        public int TotalPT_ISIMactuallywork { get; set; }
        public string PercentageTotalPT_ISIMexceptedWork { get; set; }




        public List<ObswiseUtilisationSummary> lstComplaintDashBoard1 { get; set; }
        public ObswiseUtilisationSummary GrandTotal { get; set; }


        public DataTable dtUserAttendance = new DataTable();

    }
}