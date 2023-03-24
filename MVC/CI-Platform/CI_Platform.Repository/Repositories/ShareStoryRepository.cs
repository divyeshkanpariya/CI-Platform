using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class ShareStoryRepository : IShareStoryRepository
    {
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<MissionApplication> _MissionApplications;
        private readonly IRepository<Story> _Storys;


        public ShareStoryRepository(IRepository<Mission> Missions,
            IRepository<MissionApplication> MissionApplications,
            IRepository<Story> storys)
        {
            _Missions = Missions;
            _MissionApplications = MissionApplications;
            _Storys = storys;
        }

        public List<List<string>> GetMissions(long UserId)
        {
            List<long> missionIds = new List<long>();

            IEnumerable<MissionApplication> missionApplications = _MissionApplications.GetAll().Where(u=>UserId == UserId);

            foreach (MissionApplication application in missionApplications)
            {
                missionIds.Add(application.MissionId);
            }
            IEnumerable<Mission> missions = _Missions.GetAll().Where(u => missionIds.Contains(u.MissionId));

            List<List<string>> selM = new List<List<string>>();
            foreach (var user in missions)
            {

                List<string> newU = new List<string>()
                        {

                            Convert.ToString(user.MissionId),
                            user.Title
                        };
                selM.Add(newU);
            }
            return selM;
        }

        public void UploadStory(ShareYourStoryViewModel ShareStoryModel)
        {
            
        }
    }
}
