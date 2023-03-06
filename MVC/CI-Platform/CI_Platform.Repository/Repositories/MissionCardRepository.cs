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
    public class MissionCardRepository : IMissionCardRepository
    {
        public IMissionCardRepository FillData(IEnumerable<Mission> missions)
        {
            //MissionCardViewModel data = new MissionCardViewModel();
            IEnumerable<MissionCardViewModel> fin = missions.Cast<MissionCardViewModel>(); 
            foreach (MissionCardViewModel item in missions)
            {
                item.Missions = missions.ToList();
            }
            return fin;
            
        }
    }
}
