using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuvVision.BusinessEntities;

namespace TuvVision.BusinessServices.Interface
{
    public interface IGalleryServices
    {
        int AddGalleryDetails(Gallery_VM _vmgal);
        IEnumerable<Gallery_VM> GetAllGalleryList();
        Gallery_VM GetGalleryById(int? id);
        IEnumerable<Gallery_VM> GetAllYearList();
        IEnumerable<Gallery_VM> GetAllTitleList(int? Year);
        List<string> GetAllGallery(string ttl);
        IEnumerable<Gallery_VM> GetAllList();
        bool DeleteImage(int? id);
    }
}
