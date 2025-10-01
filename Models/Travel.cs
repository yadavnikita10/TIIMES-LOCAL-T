using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Travel
    {
        public List<Travel> lstTravelExpense { get; set; }

        public List<Travel> lstUserRole { get; set; }
        public List<Travel> lstTravel { get; set; }
        public Travel Grand { get; set; }

        public List<Travel> GrandTotallist { get; set; }

        public string Name { get; set; }
        public int code { get; set; }
        public string Fromd { get; set; }
        public string Tod { get; set; }



        public string RoleName { get; set; }
        public string EmpCategoray { get; set; }


        public string EmployeementCategoray { get; set; }
        public string UserRole { get; set; }


        public string BranchName { get; set; }




        public int ptitotal { get; set; }

        public int ptiApproved { get; set; }
        public int ISIM_SP_total { get; set; }
        public int ISIM_SP_Approved { get; set; }
        public int ISIM_total { get; set; }
        public int ISIM_Approved { get; set; }
        public int ISGB_total { get; set; }
        public int ISGB_Approved { get; set; }
        public int DVS_total { get; set; }
        public int DVS_Approved { get; set; }
        public int ISET_total { get; set; }
        public int ISET_Approved { get; set; }
        public int PCG_total { get; set; }
        public int PCG_Approved { get; set; }
        public int ISGR_total { get; set; }
        public int ISGR_Approved { get; set; }
        public int Totalamount { get; set; }
        public int Approvedamount { get; set; }
        public int GrandTotal { get; set; }


        public int PC_total { get; set; }
        public int PC_Approved { get; set; }

        public int GrandGrandApproved { get; set; }


        public int SumOfptiISSIm { get; set; }
        public int SumOfptiISSImApproved { get; set; }


        public int grandptitotal { get; set; }
        public int grandptiApproved { get; set; }
        public int grandISIM_SP_total { get; set; }
        public int grandISIM_SP_Approved { get; set; }
        public int grandISIM_total { get; set; }
        public int grandISIM_Approved { get; set; }
        public int grandISGB_total { get; set; }
        public int grandISGB_Approved { get; set; }
        public int grandDVS_total { get; set; }
        public int grandDVS_Approved { get; set; }
        public int grandISET_total { get; set; }
        public int grandISET_Approved { get; set; }
        public int grandPCG_total { get; set; }
        public int grandPCG_Approved { get; set; }
        public int grandISGR_total { get; set; }
        public int grandISGR_Approved { get; set; }
        public int grandPC_total { get; set; }
        public int grandPC_Approved { get; set; }

        public int grandtotal1 { get; set; }
        public int grandapproved { get; set; }
        public int grandISETREnewable_Total { get; set; }
        public int grandISETREnewable_Approved { get; set; }

        public int GrandGrandTotal { get; set; }

        public int GrandGrandTotal_Approved { get; set; }



        public int ISETREnewable_Total { get; set; }
        public int ISETREnewable_Approved { get; set; }

    }
}