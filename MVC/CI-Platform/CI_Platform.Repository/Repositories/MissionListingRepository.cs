﻿using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CI_Platform.Repository.Repositories
{
    public class MissionListingRepository : IMissionListingRepository
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
        private readonly IRepository<MissionApplication> _MissionApplicationList;
        

        private readonly IMissionCardRepository _MissionCard;


        public MissionListingRepository(CiPlatformContext db,IRepository<City> cityList, IRepository<Country> countryList, IRepository<MissionTheme> ThemeList, IRepository<Skill> SkillList, IRepository<Mission> MissionList, IRepository<MissionMedium> MissionMedia, IRepository<MissionMedium> missionMedia,IMissionCardRepository MissionCard,IRepository<MissionRating> missionRating,IRepository<MissionSeat> MissionSeats,IRepository<GoalMission> Goals,IRepository<MissionSkill> MissionSkills,IRepository<FavoriteMission> FavouriteMissions, IRepository<MissionApplication> MissionApplications)
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
            _MissionSkills= MissionSkills;
            _FavoriteMissions = FavouriteMissions;
            _MissionApplicationList = MissionApplications;
        }
        public long[] SplitsLong(string ids)
        {
            string[] CharArr = ids.Split(",");
            long[] IntArr = new long[CharArr.Length];
            for (int i = 0; i < CharArr.Length; i++)
            {
                IntArr[i] = Convert.ToInt64(CharArr[i]);
            }
            return IntArr;
        }
        public int[] SplitsInt(string ids)
        {
            string[] CharArr = ids.Split(",");
            int[] IntArr = new int[CharArr.Length];
            for (int i = 0; i < CharArr.Length; i++)
            {
                IntArr[i] = Convert.ToInt32(CharArr[i]);
            }
            return IntArr;
        }

        public int ValOrMaxInt(int? value)
        {
            if (value == null) return int.MaxValue;
            else return (int)value;
        }

        public int ValOrZero(int? value)
        {
            if (value == null) return 0;
            else return (int)value;
        }

        

        public MissionListingViewModel GetAllData(string Countryids, string Cityids, string Themeids, string Skillids, string Sortby, string SearchText,string UserId,string PageIndex)
        {
            

            MissionListingViewModel viewModel = new MissionListingViewModel();
            var pagesize = 6;
            if (PageIndex != null)
            {
                if (PageIndex == "")
                {
                    PageIndex = "1";
                }
            }
            IEnumerable<Mission> AllMissions = _Missions.GetRecordsWhere(m =>m.DeletedAt == null).ToList();
            viewModel.MissionCount = AllMissions.Count();
            AllMissions = AllMissions.Skip((Convert.ToInt16(PageIndex) - 1) * pagesize).Take(pagesize).ToList();


            if (Countryids != null && Countryids != "")
            {
                List<long> MissionFromSkills = (from m in _db.Missions
                                                join sk in _db.MissionSkills on m.MissionId equals sk.MissionId
                                                where SplitsInt(Skillids).Contains(sk.SkillId)
                                                select m.MissionId).ToList();
                
                var Mss = (from m in _db.Missions              
                            where SplitsLong(Countryids).Contains(m.CountryId) &&
                                m.DeletedAt == null &&
                                SplitsLong(Cityids).Contains(m.CityId) &&
                                SplitsLong(Themeids).Contains(m.ThemeId) &&
                                MissionFromSkills.Contains(m.MissionId) &&
                                (m.Title.Contains(SearchText)|| m.OrganizationName!.Contains(SearchText))
                            select m);

                List<Mission> newMissions = new List<Mission>();
                switch (Convert.ToInt32(Sortby))
                {
                    case 1:
                        newMissions = (from m in Mss
                                       orderby m.CreatedAt
                                       select m).ToList();                                    
                        break;

                    case 2:
                        newMissions = (from m in Mss
                                       orderby m.CreatedAt descending
                                       select m).ToList();
                        break;
                    case 3:
                        newMissions = (from m in Mss
                                       join ms in _db.MissionSeats on m.MissionId equals ms.MissionId
                                       orderby ms.TotalSeats - ms.SeatsFilled
                                       select m).ToList();
                        break;
                    case 4:
                        newMissions = (from m in Mss
                                       join ms in _db.MissionSeats on m.MissionId equals ms.MissionId 
                                       orderby ms.TotalSeats - ms.SeatsFilled descending
                                       select m).ToList();
                        break;
                    case 5:
                        newMissions = (from m in Mss
                                       join fv in _db.FavoriteMissions
                                       on m.MissionId equals fv.MissionId
                                       where fv.UserId == Convert.ToInt64(UserId)
                                       select m).ToList();
                        break;
                    case 6:
                        newMissions = (from m in Mss
                                       where m.StartDate > DateTime.Now
                                       orderby m.RegistrationDeadline 
                                       select m).ToList();
                        break;
                    case 7:
                        newMissions = (from m in Mss
                                       join mth in (from mt in _db.Missions group mt by mt.ThemeId into grp select new { theme_id = grp.Key, count = grp.Count()})
                                       on m.ThemeId equals mth.theme_id
                                       orderby mth.count descending
                                       select m).ToList();
                        break;
                    case 8:
                        newMissions = (from m in Mss
                                       join mrt in from ratings in _db.MissionRatings group ratings by ratings.MissionId into grp select new { mission_id = grp.Key, avg = grp.Average(r=>r.Rating)}
                                       on m.MissionId equals mrt.mission_id into newt
                                       from subt in newt.DefaultIfEmpty()
                                       orderby subt.avg descending
                                       select m).ToList();
                        break;
                    case 9:
                        newMissions = (from m in Mss
                                       join mfv in from f in _db.FavoriteMissions group f by f.MissionId into grp select new { mission_id = grp.Key, count = grp.Count()}
                                       on m.MissionId equals mfv.mission_id into newt
                                       from subt in newt.DefaultIfEmpty()
                                       orderby subt.count descending
                                       select m).ToList();
                        break;
                    case 10:
                        newMissions = (from m in Mss
                                       orderby Guid.NewGuid()
                                       select m).ToList();
                        break;

                    
                }
                viewModel.MissionCount = newMissions.Count;
                newMissions = newMissions.Skip((Convert.ToInt16(PageIndex) - 1) * pagesize).Take(pagesize).ToList();

                //SqlConnection connection = new SqlConnection("Data Source=PCI117\\SQL2017;DataBase=CI-Platform;User ID=sa;Password=Tatva@123;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");

                //SqlCommand command = new SqlCommand("my_stored_procedure", connection);
                //command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.Add("@my_array", SqlDbType.VarChar).Value = Countryids;
                //command.Parameters.Add("@city_ids", SqlDbType.VarChar).Value = Cityids;
                //command.Parameters.Add("@theme_ids", SqlDbType.VarChar).Value = Themeids;
                //command.Parameters.Add("@skill_ids", SqlDbType.VarChar).Value = Skillids;   
                //command.Parameters.Add("@sortby", SqlDbType.VarChar).Value = Sortby;
                //command.Parameters.Add("@searchtext", SqlDbType.VarChar).Value = SearchText;
                //command.Parameters.Add("@user_id", SqlDbType.VarChar).Value = UserId;


                //SqlDataAdapter adapter = new SqlDataAdapter(command);

                //DataTable dataTable = new DataTable();
                //adapter.Fill(dataTable);

                //List<Mission> newMissions = new List<Mission>();

                //foreach (DataRow row in dataTable.Rows)
                //{
                //    // Access data from the row using the column name or index.
                //    Mission newM = new Mission();
                //    newM.MissionId = Convert.ToInt64(row["mission_id"]);
                //    newM.ThemeId = Convert.ToInt64(row["theme_id"]);
                //    newM.CityId = Convert.ToInt64(row["city_id"]);
                //    newM.CountryId = Convert.ToInt64(row["Country_id"]);
                //    newM.Title = Convert.ToString(row["title"])!;
                //    if (row["end_date"] != DBNull.Value)
                //    {
                //        newM.EndDate = Convert.ToDateTime(row["end_date"]);

                //    }

                //    newM.ShortDescription = Convert.ToString(row["short_description"]);
                //    if(row["start_date"] != DBNull.Value){
                //        newM.StartDate = Convert.ToDateTime(row["start_date"]);

                //    }
                //    if(row["end_date"] != DBNull.Value)
                //    {
                //        newM.EndDate = Convert.ToDateTime(row["end_date"]);

                //    }
                //    if (row["registration_deadline"] != DBNull.Value)
                //    {
                //        newM.RegistrationDeadline = Convert.ToDateTime(row["registration_deadline"]);

                //    }
                //    newM.MissionType = Convert.ToString(row["mission_type"])!;
                //    newM.Status = Convert.ToString(row["status"])!;
                //    newM.OrganizationName = Convert.ToString(row["organization_name"]);
                //    if (row["organization_detail"] != DBNull.Value)
                //    {
                //        newM.OrganizationDetail = Convert.ToString(row["organization_detail"]);

                //    }
                //    if (row["availability"] != DBNull.Value)
                //    {
                //        newM.Availability = Convert.ToString(row["availability"]);

                //    }
                //    newM.CreatedAt = Convert.ToDateTime(row["created_at"]);



                //    newMissions.Add(newM);
                //}
                //connection.Close();

                AllMissions = newMissions;
            }
            

            viewModel.MissionCards = _MissionCard.FillData(AllMissions);
            
            viewModel.Cities = _CityList.GetAll();
            viewModel.Countries = _CountryList.GetAll();
            viewModel.MissionThemes = _ThemeList.GetAll();
            viewModel.Skills = _SkillList.GetAll();
            viewModel.Missions = _Missions.GetAll();
            viewModel.MissionMedia = _MissionMedia.GetAll();

            foreach(var mission in viewModel.MissionCards)
            {
                mission.City = _CityList.GetFirstOrDefault(u => u.CityId == mission.CityId).Name;

                mission.Country = _CountryList.GetFirstOrDefault(u => u.CountryId == mission.CountryId).Name;

                mission.Theme = _ThemeList.GetFirstOrDefault(u => u.MissionThemeId == mission.ThemeId).Title!;

                /* Path */
                if (_MissionMedia.ExistUser(u => u.MissionId == mission.MissionId))
                {

                    if(_MissionMedia.GetAll().Any(u => u.MissionId == mission.MissionId && u.Default == "1")){
                        mission.Path = _MissionMedia.GetFirstOrDefault(u => u.MissionId == mission.MissionId && u.Default == "1").MediaPath;
                    }else
                    {
                        mission.Path = _MissionMedia.GetFirstOrDefault(u => u.MissionId == mission.MissionId).MediaPath;
                    }
                    
                }
                else
                {
                    mission.Path = "/images/Default.jpg";
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

                if(mission.StartDate != null && mission.EndDate != null){
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

                if(mission.MissionType == "Go")
                {
                    
                    if (_Goals.GetAll().Any(u => u.MissionId == mission.MissionId))
                    {
                        var current = _Goals.GetFirstOrDefault(u => u.MissionId == mission.MissionId);
                        mission.GoalObjectiveText = current.GoalObjectiveText;
                        mission.GoalValue = current.GoalValue;
                        mission.GoalArchived= current.GoalArchived;
                        mission.prArchivement = (current.GoalArchived * 100) / current.GoalValue;
                    }
                }

                /* Seats */
                
                
                if (_MissionSeats.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    mission.IsSeatDataFound = true;
                    var current = _MissionSeats.GetFirstOrDefault(u => u.MissionId == mission.MissionId);
                    
                    var islimited = current.Islimited;
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

                /* Mission Skills */

                if(_MissionSkills.ExistUser(u=>u.MissionId == mission.MissionId))
                {
                    var Skills = _MissionSkills.GetAll().Where(u => u.MissionId == mission.MissionId);
                    List<string> skillArr = new List<string>();
                    foreach (var skill in Skills)
                    {
                        int skillId = skill.SkillId;
                        skillArr.Add(_SkillList.GetFirstOrDefault(u=>u.SkillId == skillId).SkillName!);
                    }
                    mission.MissionSkills= skillArr;
                }

                /* Is Favourite Mission */
                long uid = Convert.ToInt64(UserId);
                if (_FavoriteMissions.ExistUser(u => u.MissionId == mission.MissionId && u.UserId ==uid))
                {
                    mission.IsFavourite = true;
                }

                /* Approval Status */
                if (_MissionApplicationList.ExistUser(u => u.MissionId == mission.MissionId && u.UserId == uid))
                {
                    string status = _MissionApplicationList.GetFirstOrDefault(u => u.MissionId == mission.MissionId && u.UserId == uid).ApprovalStatus;
                    mission.ApprovalStatus = status;
                }
                else
                {
                    mission.ApprovalStatus = "NotApplied";
                }

            }
          
            
            return viewModel;

            
        }
        



        public IEnumerable<City> getCityByCountry(long CountriesId)
        {
            var cities = _CityList.GetAll().Where(u=>u.CountryId== CountriesId);
            return cities;
        }
    }
}
