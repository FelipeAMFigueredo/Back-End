using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestIt.API.ViewModels.User;
using TestIt.Business;
using TestIt.Model.Entities;
using TestIt.Utils.Email;

namespace TestIt.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService userService;
        private ITeacherService teacherService;
        private IStudentService studentService;
        private IEmailService emailService;

        public UserController(IUserService userService, ITeacherService teacherService, IStudentService studentService, IEmailService emailService)
        {
            this.userService = userService;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.emailService = emailService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return userService.Get(); //TODO: UserViewModel
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = userService.GetSingle(id);

            if (user != null)
                return new OkObjectResult(user); //TODO: UserViewModel
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateUserViewModel viewModel)
        {
            OkObjectResult result;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = Mapper.Map<User>(viewModel);

            user.Active = true;
            userService.Save(user);

            if (viewModel.Type == 1)
            {
                var teacherId = CreateStudent(user);

                result = Ok(new { teacherId = teacherId, userId = user.Id });
            }
            else
            {
                var studentId = CreateStudent(user);

                result = Ok(new { studentId = studentId, userId = user.Id });
            }

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CreateUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = Mapper.Map<User>(viewModel);

            var sucess = userService.Update(id, user);

            if (sucess)
                return new NoContentResult();
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User user = userService.GetSingle(id);

            if (user == null)
                return new NotFoundResult();
            else
            {
                var tId = GetTeacherId(user.Id);
                var sId = GetStudentId(user.Id);
                
                if (tId != 0)
                    teacherService.Delete(tId);
                if (sId != 0)
                    studentService.Delete(sId);
                
                userService.Delete(id);

                return new NoContentResult();
            }
        }

        [HttpGet("exists/{email}")]
        public int UserExists(string email)
        {
            return userService.Exists(email);
        }


        private int CreateStudent(User user)
        {
            var student = new Student()
            {
                User = user
            };

            studentService.Save(student);

            emailService.SendSignUp(user.Email, user.Name, user.Id);

            return student.Id;
        }

        private int CreateTeacher(User user)
        {
            var teacher = new Teacher()
            {
                User = user
            };

            teacherService.Save(teacher);

            return teacher.Id;
        }

        private void DeleteTeacher(int id)
        {
            teacherService.Delete(id);
        }

        private void DeleteStudent(int id)
        {
            studentService.Delete(id);
        }

        private int GetTeacherId (int id)
        {
            Teacher t = teacherService.GetByUser(id);

            return t != null ? t.Id : 0;
        }

        private int GetStudentId(int id)
        {
            Student t = studentService.GetByUser(id);

            return t != null ? t.Id : 0;
        }
    }
}