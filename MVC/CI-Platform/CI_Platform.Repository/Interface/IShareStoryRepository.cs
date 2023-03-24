﻿using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IShareStoryRepository
    {
        public List<List<string>> GetMissions(long UserId);

        public void UploadStory(ShareYourStoryViewModel ShareStoryModel);
    }
}
