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
        public string? Mission { get; set; }
        [Required]
        [MaxLength(255)]
        public string? StoryTitle { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MinLength(2)]
        public string StoryDescription { get; set;}

        public string? StoryVideoUrl { get; set;}
        [MinLength(1)]
        public IFormFileCollection? Photos { get; set;}

    }
}
