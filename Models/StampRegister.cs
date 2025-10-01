using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class StampRegister
    {
        public int    Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Surveyor Name")]
        public string SurveyorName { get; set; }

        public string SurveyorId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Location")]
        public string Location { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Stamp Number")]
        public string StampNumber { get; set; }
        public DateTime JoiningDate { get; set; }

        public string JoiningDate1 { get; set; }
        public string Remarks { get; set; }
        public string RubberStampA { get; set; }
        public string RubberStampB { get; set; }
        public string HardStampC { get; set; }
        public string HardStampD { get; set; }
        public string HardStampE { get; set; }
        public string IssuingAuthority { get; set; }
        public string TotalHardStamps { get; set; }
        public string TotalRubberStamps { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string chkImageId { get; set; }

        public List<StampImageList> StampImageList { get; set; }

        public List<LAdditionalStamp> LAdditionalStamp { get; set; }

        public string AdditionalStamp { get; set; }

        public List<StampImageListUpdate> StampImageListUpdate { get; set; }

        public string HiddenAdditionalStamp { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class StampImageList
    {
        public int ImageId { get; set; }
        public string attachment { get; set; }
        public string ImageName { get; set; }

        public string Quantity { get; set; }

        public string Type { get; set; }
    }

    public class LAdditionalStamp
    {
        public int lstAdditionalStamp { get; set; }
    }

    public class StampImageListUpdate
    {
        public string ImageUpdate { get; set; }
    }



    }