using CI_Platform.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CI_Platform.Models.ViewModels
{
    public class ShareYourStoryViewModel
    {
        [Required]
        public string Mission { get; set; }
        [Required]
        public string StoryTitle { get; set; }
        [Required]

        public DateOnly Date { get; set; }
        [Required]
        public string StoryDescription { get; set;}

        public string StoryVideoUrl { get; set;}
        
        public IFormFileCollection Photos { get; set;}

    }
}
