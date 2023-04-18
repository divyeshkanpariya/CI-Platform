using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminCMSPageRepository
    {
        public IEnumerable<AdminCmsPageViewModel> getCmsPages(string searchText, int pageIndex);

        public AdminAddCmsViewModel getCmsDetails(long CmsId);

        public void SaveCmsPage(AdminAddCmsViewModel model);

        public void DeleteCmsPage(long CmsId);

    }
}
