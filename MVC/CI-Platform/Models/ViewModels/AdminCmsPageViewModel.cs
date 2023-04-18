using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminCmsPageViewModel : CmsPage
    {
        //public long CmsPageId { get; set; }

        //public string? Title { get; set; }

        //public string? Description { get; set; }

        //public string Slug { get; set; } = null!;

        //public string Status { get; set; } = null!;

        public int TotalPages { get; set; }
    }
}
