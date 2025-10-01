using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuvVision.BusinessEntities;

namespace TuvVision.BusinessServices.Interface
{
    public interface INewsDetails
    {
        int AddNewsDetails_New(News_VM _vminfo);
        IEnumerable<News_VM> GetAllNewsDetails();
        News_VM GetNewsDataById(int? Id);
        bool DeleteNewsById(int? Id);
    }
}
