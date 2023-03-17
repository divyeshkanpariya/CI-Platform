using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IVolunteeringMissionRepository
    {
        public MissionListingViewModel GetAllMissionData(long missionId);

        public void SendInvitation(long EmailTo, long Emailfrom,long MissionId,string NameOfSender,string Url);

        public void RateMission(int Rating, long MissionId,long UserId);
    }
}
