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
    public class AdminBannerRepository : IAdminBannerRepository
    {
        public readonly CiPlatformContext _db;
        public readonly IRepository<Banner> _Banners;

        public AdminBannerRepository(CiPlatformContext db,
            IRepository<Banner> Banners)
        {
            _db = db;
            _Banners = Banners;
        }

        public IEnumerable<AdminBannerViewModel> GetBannerDetails(string SearchText, int PageIndex)
        {
            List<AdminBannerViewModel> Banners = new List<AdminBannerViewModel>();
            IEnumerable<Banner> BannerDb = (from banner in _db.Banners
                                                 where banner.Title.Contains(SearchText) && banner.DeletedAt == null
                                                 select banner).ToList();

            foreach (Banner banner in BannerDb)
            {
                AdminBannerViewModel newmodel = new AdminBannerViewModel
                {
                    Id = banner.BannerId,
                    Title = banner.Title,
                    Text = banner.Text,
                    SortOrder = banner.SortOrder,
                    Image = banner.Image,
                    BannerCount = BannerDb.Count(),
                };
                Banners.Add(newmodel);
            }
            var pagesize = 9;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                Banners = Banners.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            return Banners;
        }

        public AdminBannerViewModel GetBanner(long BannerId)
        {
            Banner curr = _Banners.GetFirstOrDefault(u =>u.BannerId == BannerId);
            AdminBannerViewModel model = new AdminBannerViewModel();
            
            if (curr != null)
            {
                model.Id = curr.BannerId;
                model.Title = curr.Title;
                model.Text = curr.Text;
                model.SortOrder = curr.SortOrder;
                model.Image = curr.Image;
            }
            

            return model;

        }
        public string SaveBannerDetails(AdminAddEditBannerViewModel ViewModel,string Webroot)
        {
            if(ViewModel.Id == null || ViewModel.Id == "0")
            {
                Banner newBanner = new Banner();
                newBanner.Title = ViewModel.Title.Trim();
                newBanner.Text = ViewModel.Text.Trim().Replace("  "," ");
                newBanner.SortOrder = Convert.ToInt32(ViewModel.SortOrder);

                var file = ViewModel.Image;
                string folder = "Uploads/Banners/";
                string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                string Filename = file.FileName.Split(".")[0] + "-" + Guid.NewGuid().ToString().Substring(0, 8);
                folder += Filename + "." + ext;
                string serverFolder = Path.Combine(Webroot, folder);
                file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                string path = "/" + folder;
                newBanner.Image = path;
                _Banners.AddNew(newBanner);
                _Banners.Save();
                return "Added";
            }
            else
            {
                if(_Banners.ExistUser(u => u.BannerId == Convert.ToInt64(ViewModel.Id)))
                {
                    Banner currBanner = _Banners.GetFirstOrDefault(u => u.BannerId == Convert.ToInt64(ViewModel.Id));
                    if(currBanner != null)
                    {
                        string path = Path.Combine(Webroot, currBanner.Image.Substring(1));
                        if (File.Exists(path))
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error" + ex.Message);
                            }
                        }
                        currBanner.Text = ViewModel.Text.Trim().Replace("  ", " ");
                        currBanner.Title =ViewModel.Title.Trim();
                        currBanner.SortOrder = Convert.ToInt32(ViewModel.SortOrder);
                        currBanner.UpdatedAt = DateTime.Now;

                        var file = ViewModel.Image;
                        string folder = "Uploads/Banners/";
                        string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                        string Filename = file.FileName.Split(".")[0] + "-" + Guid.NewGuid().ToString().Substring(0, 8);
                        folder += Filename + "." + ext;
                        string serverFolder = Path.Combine(Webroot, folder);
                        file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                        string newpath = "/" + folder;

                        currBanner.Image = newpath;
                        _Banners.Update(currBanner);
                        _Banners.Save();
                        return "Updated";
                    }
                }
            }

            return "";
        }
    }
}
