using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class AdminCMSPageRepository : IAdminCMSPageRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<CmsPage> _cmsPages;
        public AdminCMSPageRepository(CiPlatformContext db,
            IRepository<CmsPage> cmsPages)
        {
            _db = db;
            _cmsPages = cmsPages;
        }
        public IEnumerable<AdminCmsPageViewModel> getCmsPages(string searchText, int pageIndex)
        {
            List<CmsPage> cmsPages = (from item in _db.CmsPages
                                      where item.Title.Contains(searchText)
                                      select item).ToList();

            List<AdminCmsPageViewModel> pagesFin = new List<AdminCmsPageViewModel>();

            foreach (var page in cmsPages)
            {
                var pageViewModel = new AdminCmsPageViewModel()
                {
                    CmsPageId = page.CmsPageId,
                    Title = page.Title,
                    Description = page.Description,
                    Slug = page.Slug,
                    Status = page.Status,
                    TotalPages = cmsPages.Count(),
                };
                pagesFin.Add(pageViewModel);
            }

            var pagesize = 9;
            int PageIndex = pageIndex;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                pagesFin = pagesFin.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            return pagesFin;
        }
    }
}
