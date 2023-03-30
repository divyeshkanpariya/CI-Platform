using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

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
        private readonly IRepository<User> _Users;
        private readonly IRepository<MissionInvite> _MissionInviteList;
        private readonly IRepository<Comment> _CommentList;
        private readonly IRepository<MissionApplication> _MissionApplicationList;
        private readonly IRepository<MissionDocument> _MissionDocumentList;


        private readonly IMissionCardRepository _MissionCard;


        public VolunteeringMissionRepository(CiPlatformContext db, 
            IRepository<City> cityList, 
            IRepository<Country> countryList,
            IRepository<MissionTheme> ThemeList, 
            IRepository<Skill> SkillList, 
            IRepository<Mission> MissionList, 
            IRepository<MissionMedium> MissionMedia, 
            IRepository<MissionMedium> missionMedia, 
            IMissionCardRepository MissionCard, 
            IRepository<MissionRating> missionRating, 
            IRepository<MissionSeat> MissionSeats, 
            IRepository<GoalMission> Goals, 
            IRepository<MissionSkill> MissionSkills, 
            IRepository<FavoriteMission> FavouriteMissions,
            IRepository<User> Users,
            IRepository<MissionInvite> MissionInvites,
            IRepository<Comment> Comments,
            IRepository<MissionApplication> MissionApplications,
            IRepository<MissionDocument> MissionDocuments
            )
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
            _Users = Users;
            _MissionInviteList = MissionInvites;
            _CommentList = Comments;
            _MissionApplicationList = MissionApplications;
            _MissionDocumentList = MissionDocuments;
        }
        public MissionListingViewModel GetAllMissionData(long missionId, long UserId)
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
            viewModel.Users = _Users.GetAll();
            viewModel.Comments = _CommentList.GetAll().Where(u=>u.MissionId == missionId);

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
                mission.ratingCount = _MissionRatingList.GetAll().Where(u => u.MissionId == mission.MissionId).Count();
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
                    mediaArr.Add("/images/Default.jpg");
                    mission.MissionMediaPaths = mediaArr;
                }
/*                Recent Volunteer */
                if (_MissionApplicationList.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    var usersList = (from users in _db.Users
                                     join app in _db.MissionApplications on users.UserId equals app.UserId
                                     where app.MissionId == mission.MissionId && users.UserId != UserId
                                     select new
                                     {
                                         users.UserId,
                                         users.FirstName,
                                         users.LastName,
                                         users.Email,
                                         users.Avatar,
                                         users.Password,
                                         users.PhoneNumber,
                                     }).ToList();
                    List<List<string>> recentUsers = new List<List<string>>();
                    foreach (var user in usersList)
                    {
                        string avatar = "/images/default-user-icon.jpg";
                        if (user.Avatar!=null)
                        {
                            avatar = user.Avatar;
                        }
                        List<string> newU = new List<string>()
                        {
                            avatar,
                            user.FirstName,
                            user.LastName
                        };
                        recentUsers.Add(newU);
                    }
                    mission.RecentVolunteers = recentUsers;

                }
                /*                Mission Documents */
                if (_MissionDocumentList.ExistUser(u => u.MissionId == mission.MissionId))
                {
                    mission.MissionDocumentss = _MissionDocumentList.GetAll().Where(u => u.MissionId == mission.MissionId).ToList();
                }
                /* Mission Approval Status */
                if (_MissionApplicationList.ExistUser(u => u.MissionId == mission.MissionId && u.UserId == UserId))
                {
                    string status = _MissionApplicationList.GetFirstOrDefault(u => u.MissionId == mission.MissionId && u.UserId == UserId).ApprovalStatus;
                    mission.ApprovalStatus = status;
                }
                else
                {
                    mission.ApprovalStatus = "NotApplied";
                }

            }

            

            return viewModel;


        
        }

        public MissionListingViewModel GetRelatedMissions(long missionId,long UserId)
        {
            MissionListingViewModel viewModel = new MissionListingViewModel();
            long cityId = _Missions.GetFirstOrDefault(u=>u.MissionId == missionId).CityId;
            long countryId = _Missions.GetFirstOrDefault(u => u.MissionId == missionId).CountryId;
            long themeId = _Missions.GetFirstOrDefault(u => u.MissionId == missionId).ThemeId;


            //IEnumerable<Mission> AllMissions = _Missions.GetAll();
             
            List<Mission> Relatedmissions = new List<Mission>();
            
            IEnumerable<Mission> relatedcity = _Missions.GetAll().Where(u => u.CityId == cityId && u.MissionId != missionId);
            
            Relatedmissions.AddRange(relatedcity);
            if (Relatedmissions.Count() < 3)
            {
                IEnumerable<Mission> relatedCountry = _Missions.GetAll().Where(u => u.CityId != cityId && u.CountryId == countryId);
                Relatedmissions.AddRange(relatedCountry);
            }
            if (Relatedmissions.Count() < 3)
            {
                IEnumerable<Mission> relatedTheme = _Missions.GetAll().Where(u => u.CityId != cityId && u.CountryId != countryId && u.ThemeId == themeId);
                Relatedmissions.AddRange(relatedTheme);
            }
            
            IEnumerable<Mission> AllMissions = Relatedmissions.Take(3);



            viewModel.MissionCards = _MissionCard.FillData(AllMissions);

            viewModel.MissionCount = AllMissions.Count();

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
                mission.ratingCount = _MissionRatingList.GetAll().Where(u => u.MissionId == mission.MissionId).Count();

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
                long uid = Convert.ToInt64(UserId);
                if (_FavoriteMissions.ExistUser(u => u.MissionId == mission.MissionId && u.UserId == uid))
                {
                    mission.IsFavourite = true;
                }

            }
            return viewModel;
        }


        public void RateMission(int Rating, long MissionId,long UserId)
        {
            
            if (_MissionRatingList.ExistUser(u=>u.MissionId == MissionId && u.UserId == UserId))
            {
                var rate = _MissionRatingList.GetFirstOrDefault(u => u.MissionId == MissionId && u.UserId == UserId);
                rate.Rating = Math.Abs(Convert.ToInt16(Rating));
                rate.MissionId = MissionId;
                rate.UpdatedAt = DateTime.Now;
                _MissionRatingList.Update(rate);
            }
            else
            {
                MissionRating rate = new MissionRating
                {
                    Rating = Math.Abs(Convert.ToInt16(Rating)),
                    MissionId = MissionId,
                    UserId = UserId,
                };
                _MissionRatingList.AddNew(rate);
            }
            _MissionRatingList.Save();
        }

        public void SendInvitation(long EmailTo, long Emailfrom,long MissionId,string bodyText)
        {
           
            var email = _Users.GetFirstOrDefault(u=>u.UserId == EmailTo).Email;
            
          
            var fromMail = new MailAddress("divpatel5706@gmail.com");
            var frompwd = "nomqsmrwgidwesns";
            var toEmail = new MailAddress(email);

            string subject = "Invitation !!!!";
            string body = bodyText;



            var smtp = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, frompwd)
            };

            MailMessage message = new MailMessage(fromMail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);

            
        }
        public void addInvitation(long from_id,long to_id, long missionId)
        {
            
            MissionInvite newInvite = new MissionInvite
            {
                FromUserId = from_id,
                ToUserId = to_id,
                MissionId = missionId
            };
            _MissionInviteList.AddNew(newInvite);
            _MissionInviteList.Save();
        }
        public void PostComment(long MissionId, string CommentText, long UserId)
        {
            Comment comment = new Comment
            {
                UserId = UserId,
                MissionId= MissionId,
                CommentText = CommentText,
                ApprovalStatus = "PENDING"
            };
            _CommentList.AddNew(comment);
            _CommentList.Save();
        }

        public void ApplyMission(long MissionId, long UserId)
        {
            if(_MissionApplicationList.ExistUser(u=>u.UserId == UserId && u.MissionId == MissionId) != true)
            {
                MissionApplication newApplication = new MissionApplication
                {
                    MissionId = MissionId,
                    UserId = UserId,
                    ApprovalStatus = "PENDING",
                    AppliedAt = DateTime.Now
                };
                _MissionApplicationList.AddNew(newApplication);
                _MissionApplicationList.Save();
            }
            
        }

        
    }
    
    }
