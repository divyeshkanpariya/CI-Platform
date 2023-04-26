using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class AdminMissionPageRepository : IAdminMissionPageRepository
    {

        private readonly CiPlatformContext _db;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<MissionTheme> _MissionThemes;
        private readonly IRepository<MissionSkill> _MissionSkills;
        private readonly IRepository<MissionSeat> _MissionSeats;
        private readonly IRepository<GoalMission> _MissionGoals;
        private readonly IRepository<MissionMedium> _MissionMediums;
        private readonly IRepository<MissionDocument> _MissionDocuments;

        public AdminMissionPageRepository(CiPlatformContext db,
            IRepository<Mission> missionList,
            IRepository<MissionTheme> missionThemes,
            IRepository<MissionSkill> missionSkills,
            IRepository<MissionSeat> missionSeats,
            IRepository<GoalMission> missionGoals, 
            IRepository<MissionDocument> missionDocuments,
            IRepository<MissionMedium> missionMediums)
        {
            _db = db;
            _Missions = missionList;
            _MissionThemes = missionThemes;
            _MissionSkills = missionSkills;
            _MissionSeats = missionSeats;
            _MissionGoals = missionGoals;
            _MissionDocuments = missionDocuments;
            _MissionMediums = missionMediums;
        }

        public void DeleteMission(long missionId)
        {
            if(_Missions.ExistUser(ms => ms.MissionId == missionId))
            {
                Mission mission = _Missions.GetFirstOrDefault(ms => ms.MissionId == missionId);
                if(mission != null)
                {
                    mission.DeletedAt = DateTime.Now;
                    _Missions.Update(mission);
                    _Missions.Save();
                }
            }
        }

        public IEnumerable<AdminMissionTableViewModel> GetAdminMissions(string searchText, int pageIndex)
        {
            List<AdminMissionTableViewModel> MissionList = new List<AdminMissionTableViewModel>();
            
            List<Mission> Missions = (from mis in _db.Missions
                                      where mis.Title.Contains(searchText) && mis.DeletedAt == null
                                      select mis).ToList();
            if(Missions.Count > 0)
            {
                foreach (Mission mis in Missions)
                {
                    AdminMissionTableViewModel newmodel = new AdminMissionTableViewModel
                    {
                        Title = mis.Title,
                        MissionType = mis.MissionType,
                        MissionId = mis.MissionId,
                        StartDate = mis.StartDate,
                        EndDate = mis.EndDate,
                        TotalMissions = Missions.Count(),
                    };
                    MissionList.Add(newmodel);
                }
            }
            var pagesize = 8;
            int PageIndex = pageIndex;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                MissionList = MissionList.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            return MissionList;
        }

        public AdminAddEditMissionViewModel  GetMissionDetails(long MissionId,string WebroootPath)
        {
            AdminAddEditMissionViewModel model = new AdminAddEditMissionViewModel();
            if(_Missions.ExistUser(ms => ms.MissionId == MissionId))
            {
                Mission mission = _Missions.GetFirstOrDefault(m => m.MissionId == MissionId);
                model.MissionId = MissionId;
                model.CityId = mission.CityId.ToString();
                model.CountryId = mission.CountryId.ToString();
                model.ThemeId = mission.ThemeId.ToString();
                model.Title = mission.Title;
                model.ShortDescription = mission.ShortDescription;
                model.Description = mission.Description;
                model.EndDate = mission.EndDate;
                model.StartDate = mission.StartDate;
                model.MissionType = mission.MissionType;
                model.OrganizationName = mission.OrganizationName;
                model.OrganizationDetail = mission.OrganizationDetail;
                model.Availability = mission.Availability;
                model.Status = mission.Status;
                
                
                if (_MissionSkills.ExistUser(ms => ms.MissionId == MissionId))
                {
                    
                    IEnumerable<MissionSkill> mskills = _MissionSkills.GetRecordsWhere(ms => ms.MissionId == MissionId);
                    string[] skills = new string[mskills.Count()];
                    var x =0;
                    foreach (MissionSkill skill in mskills)
                    {
                        skills[x++] = skill.SkillId.ToString();
                    }
                    model.Skills = skills;
                }
                
                if (mission.MissionType == "Go")
                {
                    if(_MissionGoals.ExistUser(ms => ms.MissionId == MissionId))
                    {
                        GoalMission gm = _MissionGoals.GetFirstOrDefault(gm => gm.MissionId == MissionId);
                        model.GoalObjectiveText = gm.GoalObjectiveText;
                        model.GoalValue = gm.GoalValue;

                    }
                }
                else if(mission.MissionType == "Time")
                {
                    if (_MissionSeats.ExistUser(ms => ms.MissionId == MissionId))
                    {
                        
                        MissionSeat gm = _MissionSeats.GetFirstOrDefault(gm => gm.MissionId == MissionId);
                        model.TotalSeats = gm.TotalSeats;
                        model.RegistrationDeadline = mission.RegistrationDeadline;
                    }
                }

                

            }
            return model;
            
        }

        public List<List<string>> GetMedias(long MissionId)
        {
            List<List<string>> mediaList = new List<List<string>>();

            List<string> DefaultImage = new List<string>();
            List<string> Images = new List<string>();
            List<string> Documents = new List<string>();
            List<string> DocumentsName = new List<string>();
            if (_MissionMediums.ExistUser(ms => ms.MissionId == MissionId && ms.Default == "1"))
            {
                string file = _MissionMediums.GetFirstOrDefault(ms => ms.MissionId == MissionId && ms.Default == "1" && ms.MediaType == "image").MediaPath;
                DefaultImage.Add(file);
            }
            if (_MissionMediums.ExistUser(ms => ms.MissionId == MissionId && ms.Default == "0"))
            {
                IEnumerable<MissionMedium> files = _MissionMediums.GetRecordsWhere(ms => ms.MissionId == MissionId && ms.Default == "0" && ms.MediaType == "image");
                foreach (MissionMedium file in files)
                {
                    Images.Add(file.MediaPath);
                }
            }
            if (_MissionDocuments.ExistUser(ms => ms.MissionId == MissionId))
            {
                IEnumerable<MissionDocument> Docs = _MissionDocuments.GetRecordsWhere(ms => ms.MissionId == MissionId);
                foreach (MissionDocument file in Docs)
                {
                    Documents.Add(file.DocumentPath);
                    DocumentsName.Add(file.DocumentName);
                }
            }
            mediaList.Add(DefaultImage);
            mediaList.Add(Images);
            mediaList.Add(Documents);
            mediaList.Add(DocumentsName);

            return mediaList;
        }

        public List<string> GetMissionLoc(long MissionId)
        {
            List<string> missionLoc = new List<string>();
            if(MissionId != null && MissionId != 0)
            {
                if(_Missions.ExistUser(ms => ms.MissionId == MissionId))
                {
                    Mission mission = _Missions.GetFirstOrDefault(ms => ms.MissionId == MissionId);
                    missionLoc.Add(mission.CountryId.ToString());
                    missionLoc.Add(mission.CityId.ToString());
                    missionLoc.Add(mission.ThemeId.ToString());
                    missionLoc.Add(mission.MissionType);
                    missionLoc.Add(mission.Availability);
                }
                
            }
            return missionLoc;
        }
        public IEnumerable<MissionTheme> getMissionThemes()
        {
            return _MissionThemes.GetAll().Where(mt => mt.Status == "1");
        }


        public void SaveMissionDetails(AdminAddEditMissionViewModel viewModel,string WebRootPath)
        {
            if (viewModel == null) return;
            else
            {
                Mission newMission = new Mission();
                if (viewModel.MissionId != 0 && viewModel.MissionId != null) newMission = _Missions.GetFirstOrDefault(m => m.MissionId == viewModel.MissionId);
                
                newMission.Title = viewModel.Title;
                newMission.Description = viewModel.Description;
                newMission.ShortDescription = viewModel.ShortDescription;
                newMission.CityId = Convert.ToInt64(viewModel.CityId);
                newMission.CountryId = Convert.ToInt64(viewModel.CountryId);
                newMission.ThemeId = Convert.ToInt64(viewModel.ThemeId);

                newMission.StartDate = viewModel.StartDate;
                newMission.EndDate = viewModel.EndDate;
                newMission.MissionType = viewModel.MissionType;
                newMission.Availability = viewModel.Availability;
                newMission.Status = viewModel.Status;
                newMission.OrganizationName = viewModel.OrganizationName;
                
                newMission.OrganizationDetail = viewModel.OrganizationDetail;
                
                if (viewModel.MissionId == 0 || viewModel.MissionId == null) _Missions.AddNew(newMission);
                
                else _Missions.Update(newMission);
                
                _Missions.Save();

                if (viewModel.MissionId == 0 || viewModel.MissionId == null)
                {
                    if(_Missions.ExistUser(m => m.Title == viewModel.Title && m.CityId == Convert.ToInt64(viewModel.CityId) && viewModel.StartDate == m.StartDate && m.MissionType == viewModel.MissionType))
                    {
                        long MissionId = _Missions.GetFirstOrDefault(m => m.Title == viewModel.Title && m.CityId == Convert.ToInt64(viewModel.CityId) && viewModel.StartDate == m.StartDate && m.MissionType == viewModel.MissionType).MissionId;
                        if (viewModel.MissionType == "Go")
                        {
                            GoalMission newGoal = new GoalMission()
                            {
                                GoalObjectiveText = viewModel.GoalObjectiveText,
                                GoalValue = viewModel.GoalValue,
                                MissionId = MissionId,
                                GoalArchived = 0
                            };
                            _MissionGoals.AddNew(newGoal);
                            _MissionGoals.Save();
                        }

                        else if (viewModel.MissionType == "Time")
                        {
                            if (viewModel.TotalSeats == 0 || viewModel.TotalSeats == null)
                            {
                                MissionSeat newSeats = new MissionSeat()
                                {
                                    MissionId = MissionId,
                                    Islimited = 0,
                                    SeatsFilled = 0,
                                };
                                _MissionSeats.AddNew(newSeats);
                                _MissionSeats.Save();
                            }
                            else
                            {
                                MissionSeat newSeats = new MissionSeat()
                                {
                                    MissionId = MissionId,
                                    TotalSeats = viewModel.TotalSeats,
                                    Islimited = 1,
                                    SeatsFilled = 0,
                                };
                                _MissionSeats.AddNew(newSeats);
                                _MissionSeats.Save();
                            }

                        }

                        // ---------------------------------- Add Skills -----------------------

                        if(viewModel.Skills != null)
                        {
                            foreach (string skill in viewModel.Skills)
                            {
                                MissionSkill newMissionSkill = new MissionSkill()
                                {
                                    SkillId = Convert.ToInt32(skill),
                                    MissionId = MissionId
                                };
                                _MissionSkills.AddNew(newMissionSkill);
                            }
                            _MissionSkills.Save();
                        }

                        // ---------------------------------- Add Media -----------------------

                        if (viewModel.DefaultImage != null)
                        {
                            var file = viewModel.DefaultImage;
                            string folder = "Uploads/Mission/Photos/";
                            string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                            string Filename = Convert.ToString(MissionId) + "-Default-" + Guid.NewGuid().ToString().Substring(0, 20);
                            folder += Filename + "." + ext;
                            string serverFolder = Path.Combine(WebRootPath, folder);
                            file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                            string path = "/" + folder;
                            UploadMedia(MissionId, "image",Filename, path,true);
                        }

                        if(viewModel.Images != null)
                        {
                            foreach(var image in viewModel.Images)
                            {
                                string folder = "Uploads/Mission/Photos/";
                                string ext = image.ContentType.ToLower().Substring(image.ContentType.LastIndexOf("/") + 1);
                                string Filename = Convert.ToString(MissionId) + "-" + Guid.NewGuid().ToString().Substring(0, 20);
                                folder += Filename + "." + ext;
                                string serverFolder = Path.Combine(WebRootPath, folder);
                                image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                                string path = "/" + folder;
                                
                                UploadMedia(MissionId, "image",Filename, path,false);
                            }
                        }

                        if(viewModel.VideoURLs != null)
                        {
                            string[] Urls = viewModel.VideoURLs.Split('\r');
                            int x = 0;
                            foreach (string Url in Urls)
                            {
                                string finUrl;
                                if (x++ != 0)
                                {
                                    finUrl = Url.Remove(0, 1);
                                }
                                else
                                {
                                    finUrl = Url;
                                }
                                if (finUrl != "")
                                {
                                    UploadMedia(MissionId, "Video", "Youtube "+x.ToString(), Url, false);

                                }
                            }
                        }

                        if(viewModel.Documents != null)
                        {

                            foreach(var Document in viewModel.Documents)
                            {
                                string folder = "Uploads/Mission/Documents/";
                                string ext = Document.FileName.ToLower().Substring(Document.FileName.LastIndexOf(".") + 1);
                                string Filename = Convert.ToString(MissionId) + "-" + Guid.NewGuid().ToString().Substring(0, 20);
                                folder += Filename + "." + ext;
                                string serverFolder = Path.Combine(WebRootPath, folder);
                                Document.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                                string path = "/" + folder;
                                UploadDocument(MissionId, Document.ContentType, Filename, path);
                            }
                            
                        }
                    }
                }
                else
                {
                    long MissionId = viewModel.MissionId;

                    // ---------------- Mission Type Goal  and Time Changes

                    if (viewModel.MissionType == "Go")
                    {
                        if (_MissionSeats.ExistUser(u => u.MissionId == MissionId && u.DeletedAt == null))
                        {
                            MissionSeat TimeMission = _MissionSeats.GetFirstOrDefault(m => m.MissionId == MissionId);
                            TimeMission.DeletedAt = DateTime.Now;
                            TimeMission.UpdatedAt = DateTime.Now;
                            _MissionSeats.Update(TimeMission);
                            _MissionSeats.Save();
                        }
                        if (_MissionGoals.ExistUser(u => u.MissionId == MissionId))
                        {
                            GoalMission GoalMiss = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                            GoalMiss.GoalValue = viewModel.GoalValue;
                            GoalMiss.GoalObjectiveText = viewModel.GoalObjectiveText;
                            if (GoalMiss.DeletedAt != null) GoalMiss.DeletedAt = null;
                            GoalMiss.UpdatedAt = DateTime.Now;
                            _MissionGoals.Update(GoalMiss);
                            _MissionGoals.Save();
                        }
                        else
                        {
                            GoalMission newGoal = new GoalMission()
                            {
                                GoalObjectiveText = viewModel.GoalObjectiveText,
                                GoalValue = viewModel.GoalValue,
                                MissionId = MissionId,
                                GoalArchived = 0
                            };
                            _MissionGoals.AddNew(newGoal);
                            _MissionGoals.Save();
                        }
                        newMission.RegistrationDeadline = null;
                    }
                    else if (viewModel.MissionType == "Time")
                    {
                        newMission.RegistrationDeadline = viewModel.RegistrationDeadline;
                        if (_MissionGoals.ExistUser(u => u.MissionId == MissionId && u.DeletedAt == null))
                        {
                            GoalMission GoalMiss = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                            GoalMiss.DeletedAt = DateTime.Now;
                            GoalMiss.UpdatedAt = DateTime.Now;
                            _MissionGoals.Update(GoalMiss);
                            _MissionGoals.Save();
                        }
                        if (_MissionSeats.ExistUser(u => u.MissionId == MissionId))
                        {
                            MissionSeat TimeMission = _MissionSeats.GetFirstOrDefault(m => m.MissionId == MissionId);
                            if (viewModel.TotalSeats == 0 || viewModel.TotalSeats == null)
                            {
                                TimeMission.Islimited = 1;
                                TimeMission.TotalSeats = null;
                            }
                            else
                            {
                                TimeMission.Islimited = 0;
                                TimeMission.TotalSeats = viewModel.TotalSeats;
                            }
                            TimeMission.UpdatedAt = DateTime.Now;
                            if (TimeMission.DeletedAt != null) TimeMission.DeletedAt = null;

                        }
                        else
                        {
                            if (viewModel.TotalSeats == 0 || viewModel.TotalSeats == null)
                            {
                                MissionSeat newSeats = new MissionSeat()
                                {
                                    MissionId = MissionId,
                                    Islimited = 0,
                                    SeatsFilled = 0,
                                };
                                _MissionSeats.AddNew(newSeats);
                                _MissionSeats.Save();
                            }
                            else
                            {
                                MissionSeat newSeats = new MissionSeat()
                                {
                                    MissionId = MissionId,
                                    TotalSeats = viewModel.TotalSeats,
                                    Islimited = 1,
                                    SeatsFilled = 0,
                                };
                                _MissionSeats.AddNew(newSeats);
                                _MissionSeats.Save();
                            }
                        }


                    }

                    IEnumerable<MissionSkill> missionSK = _MissionSkills.GetRecordsWhere(m => m.MissionId == MissionId);
                    string[] newSkills = viewModel.Skills;

                    
                    if (viewModel.Skills != null)

                    {
                        foreach (MissionSkill skill in missionSK)
                        {
                            if (newSkills.Contains(skill.SkillId.ToString())){
                                newSkills = newSkills.Where( u => u != skill.SkillId.ToString()).ToArray();
                            }
                            else
                            {
                                _MissionSkills.DeleteField(skill);
                                _MissionSkills.Save();
                            }
                        }
                        foreach (string skill in newSkills)
                        {
                            MissionSkill newMissionSkill = new MissionSkill()
                            {
                                SkillId = Convert.ToInt32(skill),
                                MissionId = MissionId
                            };
                            _MissionSkills.AddNew(newMissionSkill);
                        }
                        _MissionSkills.Save();
                    }
                    else
                    {
                        foreach (MissionSkill skill in missionSK)
                        {
                            _MissionSkills.DeleteField(skill);
                            _MissionSkills.Save();
                        }
                    }

                    DeleteMedia(WebRootPath, MissionId);

                    if (viewModel.DefaultImage != null)
                    {
                        var file = viewModel.DefaultImage;
                        string folder = "Uploads/Mission/Photos/";
                        string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                        string Filename = Convert.ToString(MissionId) + "-Default-" + Guid.NewGuid().ToString().Substring(0, 20);
                        folder += Filename + "." + ext;
                        string serverFolder = Path.Combine(WebRootPath, folder);
                        file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                        string path = "/" + folder;
                        UploadMedia(MissionId, "image", Filename, path, true);
                    }

                    if (viewModel.Images != null)
                    {
                        foreach (var image in viewModel.Images)
                        {
                            string folder = "Uploads/Mission/Photos/";
                            string ext = image.ContentType.ToLower().Substring(image.ContentType.LastIndexOf("/") + 1);
                            string Filename = Convert.ToString(MissionId) + "-" + Guid.NewGuid().ToString().Substring(0, 20);
                            folder += Filename + "." + ext;
                            string serverFolder = Path.Combine(WebRootPath, folder);
                            image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                            string path = "/" + folder;

                            UploadMedia(MissionId, "image", Filename, path, false);
                        }
                    }

                    if (viewModel.VideoURLs != null)
                    {
                        string[] Urls = viewModel.VideoURLs.Split('\r');
                        int x = 0;
                        foreach (string Url in Urls)
                        {
                            string finUrl;
                            if (x++ != 0)
                            {
                                finUrl = Url.Remove(0, 1);
                            }
                            else
                            {
                                finUrl = Url;
                            }
                            if (finUrl != "")
                            {
                                UploadMedia(MissionId, "Video", "Youtube " + x.ToString(), Url, false);

                            }
                        }
                    }

                    if (viewModel.Documents != null)
                    {

                        foreach (var Document in viewModel.Documents)
                        {
                            string folder = "Uploads/Mission/Documents/";
                            string ext = Document.FileName.ToLower().Substring(Document.FileName.LastIndexOf(".") + 1);
                            string Filename = Convert.ToString(MissionId) + "-" + Guid.NewGuid().ToString().Substring(0, 20);
                            folder += Filename + "." + ext;
                            string serverFolder = Path.Combine(WebRootPath, folder);
                            Document.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                            string path = "/" + folder;
                            UploadDocument(MissionId, Document.ContentType, Filename, path);
                        }

                    }
                }
            }
        }

        public void UploadMedia(long MissionId,string FileType,string FileName,string path,bool IsDefault)
        {
            MissionMedium newMedia = new MissionMedium();
            newMedia.MissionId = MissionId;
            newMedia.MediaName = FileName;
            newMedia.MediaPath = path;
            newMedia.MediaType = FileType;
            if (IsDefault) newMedia.Default = "1";
            else newMedia.Default = "0";

            _MissionMediums.AddNew(newMedia);
            _MissionMediums.Save();
        }

        public void UploadDocument(long MissionId,string FileType, string FileName, string path)
        {
            MissionDocument newDoc = new MissionDocument();
            newDoc.MissionId = MissionId;
            newDoc.DocumentType = FileType;
            newDoc.DocumentName = FileName;
            newDoc.DocumentPath = path;

            _MissionDocuments.AddNew(newDoc);
            _MissionDocuments.Save();
        }
        public void DeleteMedia(string Root, long MissionId)
        {
            // --------------------- Delete Images

            IEnumerable<MissionMedium> images = _MissionMediums.GetRecordsWhere(m => m.MissionId == MissionId);
            if(images.Count() > 0)
            {
                foreach (MissionMedium image in images)
                {
                    if(image.MediaType == "image")
                    {
                        string path = Path.Combine(Root, image.MediaPath.Substring(1));
                        
                        if (File.Exists(path))
                        {
                            try
                            {
                                File.Delete(path);
                            }catch (Exception ex)
                            {
                                Console.WriteLine("Error" + ex.Message);
                            }
                            
                        }
                    }
                    _MissionMediums.DeleteField(image);
                    _MissionMediums.Save();

                }
            }

            // --------------------- Delete Documents

            IEnumerable<MissionDocument> Documents = _MissionDocuments.GetRecordsWhere(m => m.MissionId == MissionId);
            if(Documents.Count() > 0)
            {
                foreach(MissionDocument document in Documents)
                {
                    string path = Path.Combine(Root, document.DocumentPath.Substring(1));
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error" + ex.Message);
                        }

                    }
                    _MissionDocuments.DeleteField(document);
                    _MissionDocuments.Save();
                }
            }
        }
        
    }
}
