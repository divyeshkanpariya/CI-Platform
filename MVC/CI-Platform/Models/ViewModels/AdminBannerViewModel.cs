using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminBannerViewModel
    {
        public long Id { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int? SortOrder { get; set; }

        public int BannerCount { get; set; }
    }
}
