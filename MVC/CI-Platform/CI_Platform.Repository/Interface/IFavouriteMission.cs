using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IFavouriteMission
    {
        public void AddToFavourite(long MisisionId, long UserId);
    }
}
