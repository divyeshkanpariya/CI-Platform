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
    
    public class PrivacyPolicyRepository : IPrivacyPolicyRepository
    {
        private readonly IRepository<CmsPage> _cmsPage;

        public PrivacyPolicyRepository (IRepository<CmsPage> cmsPage)
        {
            _cmsPage = cmsPage;
        }
        public IEnumerable<PrivacyPolicyViewModel> GetPolicies()
        {
            var cms = _cmsPage.GetAll();
            List<PrivacyPolicyViewModel> finModel = new List<PrivacyPolicyViewModel>();
            foreach(var page in cms)
            {
                PrivacyPolicyViewModel newmodel = new PrivacyPolicyViewModel()
                {
                    PrivacyPolicyId = page.CmsPageId,
                    Title = page.Title,
                    Description = page.Description,
                    Slug = page.Slug,
                    Status = page.Status,
                };
                finModel.Add(newmodel);
            }
            return finModel;
        }
    }
}
