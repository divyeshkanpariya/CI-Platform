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
                                      where item.Title!.Contains(searchText) && item.DeletedAt == null
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
            pagesFin = pagesFin.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            
            return pagesFin;
        }

        public AdminAddCmsViewModel getCmsDetails(long CmsId)
        {
            CmsPage page = _cmsPages.GetFirstOrDefault(cms => cms.CmsPageId == CmsId);
            AdminAddCmsViewModel viewModel = new AdminAddCmsViewModel()
            {
                Id = CmsId.ToString(),
                Title = page.Title,
                Description = page.Description!.Replace("'\n'",""),
                Slug = page.Slug,
                Status= page.Status,
            };
            return viewModel;
        }

        public void SaveCmsPage(AdminAddCmsViewModel model)
        {
            if(model.Id != "0")
            {
                CmsPage curr = _cmsPages.GetFirstOrDefault(cms => cms.CmsPageId == Convert.ToInt64(model.Id));
                if(curr != null)
                {
                    curr.Title = model.Title;
                    curr.Description = model.Description;
                    curr.Slug = model.Slug.Trim().Replace(" ","");
                    curr.Status = model.Status;
                    curr.UpdatedAt = DateTime.Now;
                    _cmsPages.Update(curr);
                    _cmsPages.Save();
                }
                
            }
            else
            {
                CmsPage newpage = new CmsPage()
                {
                    Title = model.Title,
                    Description =model.Description,
                    Slug=model.Slug.Trim().Replace(" ", ""),
                    Status = model.Status,
                };
                _cmsPages.AddNew(newpage);
                _cmsPages.Save();
            }
        }
        public void DeleteCmsPage(long CmsId)
        {
            if(_cmsPages.ExistUser(cms => cms.CmsPageId == CmsId))
            {
                CmsPage page =  _cmsPages.GetFirstOrDefault(cms =>cms.CmsPageId == CmsId);
                page.DeletedAt = DateTime.Now;
                _cmsPages.Update(page);
                _cmsPages.Save();
            }
        }

        public bool IsSlugExist(string slug)
        {
            return _cmsPages.ExistUser(c => c.Slug.Trim().ToLower() == slug.Trim().ToLower() && c.DeletedAt == null);
        }
    }
}
