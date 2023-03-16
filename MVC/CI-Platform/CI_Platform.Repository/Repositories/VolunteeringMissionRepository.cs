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
    public class VolunteeringMissionRepository : IVolunteeringMissionRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<City> _CityList;
        private readonly IRepository<Country> _CountryList;
        private readonly IRepository<MissionTheme> _ThemeList;
        private readonly IRepository<Skill> _SkillList;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<MissionMedium> _MissionMedia;
        private readonly IRepository<MissionRating> _MissionRatingList;
        private readonly IRepository<MissionSeat> _MissionSeats;
        private readonly IRepository<GoalMission> _Goals;
        private readonly IRepository<MissionSkill> _MissionSkills;
        private readonly IRepository<FavoriteMission> _FavoriteMissions;


        private readonly IMissionCardRepository _MissionCard;


        public VolunteeringMissionRepository(CiPlatformContext db, IRepository<City> cityList, IRepository<Country> countryList, IRepository<MissionTheme> ThemeList, IRepository<Skill> SkillList, IRepository<Mission> MissionList, IRepository<MissionMedium> MissionMedia, IRepository<MissionMedium> missionMedia, IMissionCardRepository MissionCard, IRepository<MissionRating> missionRating, IRepository<MissionSeat> MissionSeats, IRepository<GoalMission> Goals, IRepository<MissionSkill> MissionSkills, IRepository<FavoriteMission> FavouriteMissions)
        {
            _db = db;
            _CityList = cityList;
            _CountryList = countryList;
            _ThemeList = ThemeList;
            _SkillList = SkillList;
            _Missions = MissionList;
            _MissionMedia = missionMedia;
            _MissionCard = MissionCard;
            _MissionRatingList = missionRating;
            _MissionSeats = MissionSeats;
            _Goals = Goals;
            _MissionSkills = MissionSkills;
            _FavoriteMissions = FavouriteMissions;
        }
        public MissionListingViewModel GetAllMissionData(long missionId)
        {
            MissionListingViewModel viewModel = new MissionListingViewModel();

            var SelectedMissions = _Missions.GetAll().Where(u=>u.MissionId == missionId);
            viewModel.MissionCards = _MissionCard.FillData(SelectedMissions);

            //MissionCardViewModel cardmodel = new MissionCardViewModel();
            //IEnumerable<Mission> missions = _Missions.GetAll();

            //IEnumerable<MissionCardViewModel> miss = _MissionCard.FillData(missions);

            viewModel.Cities = _CityList.GetAll();
            viewModel.Countries = _CountryList.GetAll();
            viewModel.MissionThemes = _ThemeList.GetAll();
            viewModel.Skills = _SkillList.GetAll();
            viewModel.Missions = _Missions.GetAll();
            viewModel.MissionMedia = _MissionMedia.GetAll();

            foreach (var mission in viewModel.MissionCards)
            {
                mission.City = _CityList.GetFirstOrDefault(u => u.CityId == mission.CityId).Name;

                mission.Country = _CountryList.GetFirstOrDefault(u => u.CountryId == mission.CountryId).Name;

                mission.Theme = _ThemeList.GetFirstOrDefault(u => u.MissionThemeId == mission.ThemeId).Title;

                /* Path */
                if (_MissionMedia.ExistUser(u => u.MissionId == mission.MissionId))
                {

                    if (_MissionMedia.GetAll().Any(u => u.MissionId == mission.MissionId && u.Default == 1))
                    {
                        mission.Path = _MissionMedia.GetFirstOrDefault(u => u.MissionId == mission.MissionId && u.Default == 1).MediaPath;
                    }
                    else
                    {
                        mission.Path = _MissionMedia.GetFirstOrDefault(u => u.MissionId == mission.MissionId).MediaPath;
                    }

                }
                else
                {
                    mission.Path = "https://localhost:7172/images/Grow-Trees-On-the-path-to-environment-sustainability.png";
                }

                /* Rating */



                if (_MissionRatingList.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    var x = _MissionRatingList.GetAll().Where(u => u.MissionId == mission.MissionId).Average(r => r.Rating);
                    mission.Ratings = Convert.ToInt16(x);
                }
                else
                {
                    mission.Ratings = 0;
                }

                /* Ongoing Activity or not */

                if (mission.StartDate != null && mission.EndDate != null)
                {
                    if (DateTime.Compare((DateTime)mission.StartDate, DateTime.Now) < 0 && DateTime.Compare((DateTime)mission.EndDate, DateTime.Now) >= 0)
                    {
                        mission.IsOngoingActivity = true;
                    }
                    else
                    {
                        mission.IsOngoingActivity = false;
                    }
                }
                /* Goal */

                if (mission.MissionType == "Go")
                {

                    if (_Goals.GetAll().Any(u => u.MissionId == mission.MissionId))
                    {
                        var current = _Goals.GetFirstOrDefault(u => u.MissionId == mission.MissionId);
                        mission.GoalObjectiveText = current.GoalObjectiveText;
                        mission.GoalValue = current.GoalValue;
                        mission.GoalArchived = current.GoalArchived;
                        mission.prArchivement = (current.GoalArchived * 100) / current.GoalValue;
                    }
                }

                /* Seats */


                if (_MissionSeats.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    mission.IsSeatDataFound = true;
                    var current = _MissionSeats.GetFirstOrDefault(u => u.MissionId == mission.MissionId);

                    var islimited = current.Islimited;
                    if (islimited != null)
                    {
                        if (islimited == 1)
                        {
                            mission.IsLimitedSeats = islimited;
                            mission.TotalSeats = current.TotalSeats;
                            mission.SeatsLeft = current.TotalSeats - current.SeatsFilled;
                            mission.SeatsFilled = current.SeatsFilled;

                        }
                        else if (islimited == 0)
                        {
                            mission.IsLimitedSeats = islimited;
                            mission.SeatsFilled = current.SeatsFilled;
                        }
                    }
                }

                /* Mission Skills */

                if (_MissionSkills.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    var Skills = _MissionSkills.GetAll().Where(u => u.MissionId == mission.MissionId);
                    List<string> skillArr = new List<string>();
                    foreach (var skill in Skills)
                    {
                        int skillId = skill.SkillId;
                        skillArr.Add(_SkillList.GetFirstOrDefault(u => u.SkillId == skillId).SkillName);
                    }
                    mission.MissionSkills = skillArr;
                }

                /* Is Favourite Mission */

                if(_FavoriteMissions.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    mission.IsFavourite = true;
                }

                /* Mission Media */

                if(_MissionMedia.ExistUser(u => u.MissionId == mission.MissionId)){
                    var media = _MissionMedia.GetAll().Where(u => u.MissionId == mission.MissionId);
                    List<string> mediaArr = new List<string>();
                    foreach(var item in media)
                    {
                        mediaArr.Add(item.MediaPath);
                    }
                    mission.MissionMediaPaths = mediaArr;
                }
                else
                {
                    List<string> mediaArr = new List<string>();
                    mediaArr.Add("https://localhost:7172/images/Grow-Trees-On-the-path-to-environment-sustainability.png");
                    mission.MissionMediaPaths = mediaArr;
                }
            }

            

            return viewModel;


        
        }
    }
}
