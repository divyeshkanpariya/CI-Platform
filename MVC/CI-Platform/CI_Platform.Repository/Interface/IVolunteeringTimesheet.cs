using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IVolunteeringTimesheet
    {
        public List<List<string>> Missions(long UserId);
        public void SaveVolDetails(string Type, long UserId, long MissionId, DateTime Date, int Hours, int Minutes, int Action, string Message);
    }
}
