using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IShareStoryRepository
    {
        public IEnumerable<Mission> GetMissions(long UserId);
    }
}
