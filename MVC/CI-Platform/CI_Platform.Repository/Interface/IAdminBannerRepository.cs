﻿using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminBannerRepository
    {
        public IEnumerable<AdminBannerViewModel> GetBannerDetails(string SearchText, int PageIndex);

        public AdminBannerViewModel GetBanner(long BannerId);

        public string SaveBannerDetails(AdminAddEditBannerViewModel ViewModel, string Webroot);

        public void DeleteBanner(long BannerId, string WebRootPath);

        public IEnumerable<Banner> getAllBanners();
    }
}
