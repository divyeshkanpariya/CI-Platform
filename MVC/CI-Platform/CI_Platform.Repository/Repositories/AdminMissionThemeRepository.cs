using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class AdminMissionThemeRepository : IAdminMissionThemeRepository
    {

        public readonly CiPlatformContext _db;
        public readonly IRepository<MissionTheme> _Themes;

        public AdminMissionThemeRepository(CiPlatformContext db,
            IRepository<MissionTheme> themes)
        {
            _db = db;
            _Themes = themes;
        }

        public IEnumerable<AdminMissionThemeViewModel> GetThemes(string SearchText, int PageIndex)
        {
            List<AdminMissionThemeViewModel> Themes = new List<AdminMissionThemeViewModel>();
            IEnumerable<MissionTheme> ThemeDb = (from theme in _db.MissionThemes
                                          where theme.Title.Contains(SearchText) && theme.DeletedAt == null
                                          select theme).ToList();

            foreach (MissionTheme theme in ThemeDb)
            {
                AdminMissionThemeViewModel newmodel = new AdminMissionThemeViewModel
                {
                    ThemeId = theme.MissionThemeId,
                    Title = theme.Title,
                    Status = theme.Status,
                    ThemeCount = ThemeDb.Count(),
                };
                Themes.Add(newmodel);
            }
            var pagesize = 9;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                Themes = Themes.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            return Themes;
        }
    }
}
