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
        private readonly CiPlatformContext _db;

        public MissionCardRepository(CiPlatformContext db)
        {
            _db = db;
        }
        public IEnumerable<MissionCardViewModel> FillData(IEnumerable<Mission> missions)
        {
            List<MissionCardViewModel> MissionCard = new List<MissionCardViewModel>();

            foreach (Mission mission in missions)
            {
                MissionCardViewModel view = new MissionCardViewModel();
                view.MissionId = mission.MissionId;
                view.CountryId = mission.CountryId;
                view.CityId = mission.CityId;
                view.ThemeId = mission.ThemeId;
                view.StartDate = mission.StartDate;
                view.EndDate = mission.EndDate;
                view.Title = mission.Title;
                view.Description = mission.Description;
                view.ShortDescription = mission.ShortDescription;
                view.MissionType = mission.MissionType;
                view.OrganizationName = mission.OrganizationName;
                
                MissionCard.Add(view);
            }
            IEnumerable<MissionCardViewModel> fin = MissionCard;
            return fin;
        }
        
    }
}
