using System.Collections.Generic;
using System.Linq;
using TestIt.Data.Abstract;
using TestIt.Model.DTO;
using TestIt.Model.Entities;

namespace TestIt.Data.Repositories
{
    public class StudentRepository : EntityBaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(TestItContext context)
            : base(context)
        {

        }

        public IEnumerable<StudentTestDto> GetTests(int id)
        {
            var unavailableClassTests = (from a in Context.Exams
                                         where a.StudentId == id
                                         select a.ClassTestsId);

            var tests = (from a in Context.Classes
                         join b in Context.ClassStudents on a.Id equals b.ClassId
                         join c in Context.ClassTests on b.ClassId equals c.ClassId
                         join d in Context.Tests on c.TestId equals d.Id
                         join e in Context.Teachers on a.TeacherId equals e.Id
                         join f in Context.Users on e.UserId equals f.Id
                         where b.StudentId == id && !unavailableClassTests.Contains(c.Id)
                         select new StudentTestDto
                         {
                             ClassName = a.Description,
                             ClassTestId = c.Id,
                             EndDate = c.EndDate,
                             Name = d.Description,
                             TeacherName = f.Name
                         }).ToList();
            
            return tests;
        }
    }
}