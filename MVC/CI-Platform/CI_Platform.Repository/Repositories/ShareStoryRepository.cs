using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
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

        public ShareStoryRepository(IRepository<Mission> Missions,
            IRepository<MissionApplication> MissionApplications)
        {
            _Missions = Missions;
            _MissionApplications = MissionApplications;
        }

        public IEnumerable<Mission> GetMissions(long UserId)
        {
            List<long> missionIds = new List<long>();

            IEnumerable<MissionApplication> missionApplications = _MissionApplications.GetAll().Where(u=>UserId == UserId);

            foreach (MissionApplication application in missionApplications)
            {
                missionIds.Add(application.MissionId);
            }
            IEnumerable<Mission> missions = _Missions.GetAll().Where(u => missionIds.Contains(u.MissionId));
            return missions;
        }
    }
}
