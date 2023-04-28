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
    public class AdminSkillRepository : IAdminSkillRepository
    {
        public readonly CiPlatformContext _db;
        public readonly IRepository<Skill> _Skills;

        public AdminSkillRepository( CiPlatformContext db,
            IRepository<Skill> skills)
        {
            _db = db;
            _Skills = skills;
        }

        public IEnumerable<AdminSkillTableViewModel> GetSkills(string SearchText, int PageIndex)
        {
            List<AdminSkillTableViewModel> Skills = new List<AdminSkillTableViewModel>();
            IEnumerable<Skill> skilldb = (from skill in _db.Skills
                                                    where skill.SkillName!.Contains(SearchText) && skill.DeletedAt == null
                                                    select skill).ToList();

            foreach(Skill skill in skilldb)
            {
                AdminSkillTableViewModel newmodel = new AdminSkillTableViewModel
                {
                    SkillId = skill.SkillId,
                    Name = skill.SkillName!,
                    Status = skill.Status,
                    SkillCount = skilldb.Count(),
                };
                Skills.Add(newmodel);
            }
            var pagesize = 9;
            Skills = Skills.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            
            return Skills;
        }

        public string SaveSkill(int SkillId,string Name, string Status)
        {
            if(SkillId == 0)
            {
                if(_Skills.ExistUser(u => u.SkillName!.Replace(" ","") == Name.Trim().Replace(" ", "") && u.DeletedAt == null)){
                    return "Skill Already Exist";
                }else if(_Skills.ExistUser(u => u.SkillName!.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.DeletedAt != null))
                {
                    Skill skill = _Skills.GetFirstOrDefault(u => u.SkillName!.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.DeletedAt != null);
                    skill.DeletedAt = null;
                    skill.UpdatedAt = DateTime.Now;
                    _Skills.Update(skill);
                    _Skills.Save();
                    return "Added";
                }
                Skill newSkill = new Skill
                {
                    SkillName = Name,
                    Status = Status,
                };
                _Skills.AddNew(newSkill);
                _Skills.Save();
                return "Added";
            }
            else
            {
                if (_Skills.ExistUser(sk => sk.SkillId == SkillId))
                {
                    Skill skill = _Skills.GetFirstOrDefault(sk => sk.SkillId == SkillId);
                    if (skill.Status == Status && skill.SkillName == Name) return "Skill not changed";

                    else if (_Skills.ExistUser(u => u.SkillName!.Replace(" ", "") == Name.Trim().Replace(" ", "") && u.SkillId != SkillId && u.DeletedAt == null))
                    {
                        return "Skill Already Exist";
                    }
                    if (skill != null)
                    {
                        skill.Status = Status;
                        skill.SkillName = Name.Trim();
                        skill.UpdatedAt = DateTime.Now;
                        _Skills.Update(skill);
                        _Skills.Save();
                        return "Updated";
                    }
                }
                return "";

            }
        }

        public void DeleteSkill(int SkillId)
        {
            if (_Skills.ExistUser(sk => sk.SkillId == SkillId))
            {
                Skill skill = _Skills.GetFirstOrDefault(sk => sk.SkillId == SkillId);
                if(skill != null)
                {
                    skill.UpdatedAt = DateTime.Now;
                    skill.DeletedAt = DateTime.Now;
                    _Skills.Update(skill);
                    _Skills.Save();
                }
            }
        }
    }
}
