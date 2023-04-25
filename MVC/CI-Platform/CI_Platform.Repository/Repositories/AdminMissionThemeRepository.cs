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

        public string SaveTheme(long ThemeId, string Name, string Status)
        {
            if (ThemeId == 0 || ThemeId == null)
            {
                if (_Themes.ExistUser(u => u.Title.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.DeletedAt == null))
                {
                    return "Theme Already Exist";
                }else if(_Themes.ExistUser(u => u.Title.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.DeletedAt != null))
                {
                    MissionTheme missionTheme = _Themes.GetFirstOrDefault(u => u.Title.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.DeletedAt != null);
                    if (missionTheme != null)
                    {
                        missionTheme.DeletedAt = null;
                        missionTheme.UpdatedAt = DateTime.Now;
                        _Themes.Update(missionTheme);
                        _Themes.Save();
                        return "Added";
                    }
                }
                MissionTheme newSkill = new MissionTheme
                {
                    Title = Name,
                    Status = Status,
                };
                _Themes.AddNew(newSkill);
                _Themes.Save();
                return "Added";
            }
            else
            {
                if (_Themes.ExistUser(sk => sk.MissionThemeId == ThemeId))
                {
                    MissionTheme theme = _Themes.GetFirstOrDefault(sk => sk.MissionThemeId == ThemeId);
                    if (theme.Status == Status && theme.Title == Name) return "Theme not changed";

                    else if (_Themes.ExistUser(u => u.Title.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.MissionThemeId != ThemeId && u.DeletedAt == null))
                    {
                         return "Theme Already Exist"; 
                    }

                    if (theme != null)
                    {
                        theme.Status = Status;
                        theme.Title = Name.Trim();
                        theme.UpdatedAt = DateTime.Now;
                        theme.DeletedAt = null;
                        _Themes.Update(theme);
                        _Themes.Save();
                        return "Updated";
                    }
                }
                return "";

            }
        }
        public void DeleteTheme(long ThemeId)
        {
            if (_Themes.ExistUser(sk => sk.MissionThemeId == ThemeId))
            {
                MissionTheme theme = _Themes.GetFirstOrDefault(sk => sk.MissionThemeId == ThemeId);
                if (theme != null)
                {
                    theme.UpdatedAt = DateTime.Now;
                    theme.DeletedAt = DateTime.Now;
                    _Themes.Update(theme);
                    _Themes.Save();
                }
            }
        }
    }
}
