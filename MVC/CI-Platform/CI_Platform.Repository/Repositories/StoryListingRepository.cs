using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class StoryListingRepository : IStoryListingRepository
    {
        private readonly IRepository<City> _CityList;
        private readonly IRepository<Country> _CountryList;
        private readonly IRepository<MissionTheme> _ThemeList;
        private readonly IRepository<Skill> _SkillList;
        private readonly IRepository<Story> _StoryList;
        private readonly IRepository<User> _UsersList;
        private readonly IRepository<Mission> _MissionList;
        private readonly IRepository<StoryMedium> _StoryMediaList;


        private readonly IStoryCardRepository _CardList;

        public StoryListingRepository(
            IRepository<City> cityList,
            IRepository<Country> countryList,
            IRepository<MissionTheme> ThemeList,
            IRepository<Skill> SkillList,
            IRepository<Story> StoryList,
            IRepository<User> Users,
            IRepository<Mission> Missions,
            IRepository<StoryMedium> StoryMedia,
            IStoryCardRepository StoryCardList
            )
        {
            _CityList = cityList;
            _CountryList = countryList;
            _ThemeList = ThemeList;
            _SkillList = SkillList;
            _StoryList = StoryList;
            _CardList = StoryCardList;
            _UsersList = Users;
            _MissionList = Missions;
            _StoryMediaList = StoryMedia;
        }
        public StoryListingViewModel GetAllData(string CountryIDs, string CityIDs, string ThemeIDs, string SkillIDs,string SearchText, string UserId, string PageIndex)
        {
            StoryListingViewModel viewModel = new StoryListingViewModel();

            viewModel.Cities = _CityList.GetAll();
            viewModel.Countries = _CountryList.GetAll();
            viewModel.MissionThemes = _ThemeList.GetAll();
            viewModel.Skills = _SkillList.GetAll();
            IEnumerable<Story> AllStories = _StoryList.GetAll().Where(u=> u.Status == "PUBLISHED");
            if(CountryIDs != null && CountryIDs != "")
            {
                AllStories = getStoriesByFilters(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SearchText, UserId);

            }

            viewModel.StoryCards = _CardList.FillData(AllStories);
            viewModel.StoryCount = AllStories.Count();

            foreach (var story in viewModel.StoryCards)
            {
                var user = _UsersList.GetFirstOrDefault(u => u.UserId == story.UserId);
                story.FirstName = user.FirstName;

                story.LastName = user.LastName;
                if(user.Avatar != null)
                {
                    story.UserProfile = user.Avatar;
                }
                else
                {
                    story.UserProfile = "/images/default-user-icon.jpg";
                }
                

                var Currmission = _MissionList.GetFirstOrDefault(u => u.MissionId == story.MissionId).ThemeId;

                story.MissionTheme = _ThemeList.GetFirstOrDefault(u => u.MissionThemeId == Currmission).Title;

                if(_StoryMediaList.ExistUser(u => u.StoryId == story.StoryId))
                {
                    story.StoryMediaPath = _StoryMediaList.GetFirstOrDefault(u => u.StoryId == story.StoryId).Path;
                }
                else
                {
                    story.StoryMediaPath = "/images/Default.jpg";
                }
                

            }
            var pagesize = 3;
            if (PageIndex != null)
            {
                if (PageIndex == "")
                {
                    PageIndex = "1";
                }
                viewModel.StoryCards = viewModel.StoryCards.Skip((Convert.ToInt16(PageIndex) - 1) * pagesize).Take(pagesize).ToList();
            }
            return viewModel;
        }
        public IEnumerable<Story> getStoriesByFilters(string CountryIDs, string CityIDs, string ThemeIDs, string SkillIDs, string SearchText, string UserId)
        {
            SqlConnection connection = new SqlConnection("Data Source=PCI117\\SQL2017;DataBase=CI-Platform;User ID=sa;Password=Tatva@123;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");

            SqlCommand command = new SqlCommand("get_filtered_Stories_procedure", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@country_ids", SqlDbType.VarChar).Value = CountryIDs;
            command.Parameters.Add("@city_ids", SqlDbType.VarChar).Value = CityIDs;
            command.Parameters.Add("@theme_ids", SqlDbType.VarChar).Value = ThemeIDs;
            command.Parameters.Add("@skill_ids", SqlDbType.VarChar).Value = SkillIDs;
            command.Parameters.Add("@searchtext", SqlDbType.VarChar).Value = SearchText;
            //command.Parameters.Add("@user_id", SqlDbType.VarChar).Value = UserId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            List<Story> newStories = new List<Story>();
            foreach (DataRow row in dataTable.Rows)
            {
                Story story = new Story();
                story.StoryId = Convert.ToInt64(row["story_id"]);
                story.MissionId = Convert.ToInt64(row["mission_id"]);
                story.UserId = Convert.ToInt64(row["user_id"]);
                if(row["title"] != DBNull.Value)
                {
                    story.Title = Convert.ToString(row["title"]);
                }
                if (row["description"] != DBNull.Value)
                {
                    story.Description = Convert.ToString(row["description"]);

                }
                story.Status = Convert.ToString(row["status"]);
                if(row["published_at"] != DBNull.Value)
                {
                    story.PublishedAt = Convert.ToDateTime(row["published_at"]);
                }
                story.CreatedAt = Convert.ToDateTime(row["created_at"]);
                if(row["updated_at"] != DBNull.Value)
                {
                    story.UpdatedAt = Convert.ToDateTime(row["updated_at"]);
                }
                if (row["deleted_at"] != DBNull.Value)
                {
                    story.UpdatedAt = Convert.ToDateTime(row["deleted_at"]);
                }

                newStories.Add(story);
            }
            connection.Close();
            return newStories;
        }
        public IEnumerable<City> getCityByCountry(long CountriesId)
        {
            var cities = _CityList.GetAll().Where(u => u.CountryId == CountriesId);
            return cities;
        }
    }
}
