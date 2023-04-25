using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionThemeRepository
    {
        public IEnumerable<AdminMissionThemeViewModel> GetThemes(string SearchText, int PageIndex);

        public string SaveTheme(long ThemeId, string Name, string Status);

        public void DeleteTheme(long ThemeId);
    }
}
