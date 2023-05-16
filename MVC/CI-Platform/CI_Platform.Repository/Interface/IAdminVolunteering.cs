using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminVolunteering
    {
        public IEnumerable<AdminTimesheetViewModel> GetVolunteeringDetails(string SearchText, int PageIndex);

        public void UpdateStatus(long TimesheetId, string Status);
    }
}
