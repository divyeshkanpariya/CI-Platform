using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class PrivacyPolicyViewModel
    {
        public long PrivacyPolicyId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public string Status { get; set; }
    }
}
