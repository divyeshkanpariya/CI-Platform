using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class FavouriteMissionRepository : IFavouriteMission
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<FavoriteMission> _FavoriteMissions;
        public FavouriteMissionRepository(CiPlatformContext db,IRepository<FavoriteMission> FavMissions)
        {
            _db = db;
            _FavoriteMissions = FavMissions;
        }

        public void AddToFavourite(long MissionId, long UserId)
        {
            var isfav = _FavoriteMissions.GetAll().Any(u => u.MissionId == MissionId && u.UserId == UserId);
            if (isfav == false)
            {
                var addfav = new FavoriteMission
                {
                    MissionId = MissionId,
                    UserId = UserId,
                };
                _FavoriteMissions.AddNew(addfav);
                _FavoriteMissions.Save();
            }
            else
            {
                var field = _FavoriteMissions.GetFirstOrDefault(u => u.UserId == UserId && u.MissionId == MissionId);
                _FavoriteMissions.DeleteField(field);
                _FavoriteMissions.Save();
            }
        }
    }
}
