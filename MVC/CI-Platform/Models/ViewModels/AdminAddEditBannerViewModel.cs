using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminAddEditBannerViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string SortOrder { get; set; }

        public IFormFile Image { get; set; }
    }
}
