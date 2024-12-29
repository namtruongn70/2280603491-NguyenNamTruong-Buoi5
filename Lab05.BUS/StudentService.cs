using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05.DAL.Entities;

namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            Model1 context = new Model1();
            return context.Students.ToList();
        }

        public List<Student> GetAllHasNoMajor()
        {
            Model1 context = new Model1();
            return context.Students.Where(p => p.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            Model1 context = new Model1();
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }



        public void InsertUpdate(Student s)
        {
            Model1 context = new Model1();
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }



        public void InsertOrUpdateStudent(Student student)
        {
            using (Model1 context = new Model1())
            {
                var existingStudent = context.Students.FirstOrDefault(p => p.StudentID == student.StudentID);
                if (existingStudent != null)
                {
                    // Cập nhật thông tin sinh viên
                    existingStudent.FullName = student.FullName;
                    existingStudent.AverageScore = student.AverageScore;
                    existingStudent.FacultyID = student.FacultyID;
                    existingStudent.MajorID = student.MajorID;
                    existingStudent.Avatar = student.Avatar;
                }
                else
                {
                    // Thêm sinh viên mới
                    context.Students.Add(student);
                }
                context.SaveChanges();
            }
        }

        public void Delete(Student student)
        {

            using (var context = new Model1())
            {
                // Kiểm tra nếu sinh viên tồn tại
                var existingStudent = context.Students.FirstOrDefault(s => s.StudentID == student.StudentID);
                if (existingStudent != null)
                {
                    context.Students.Remove(existingStudent);
                    context.SaveChanges();
                }
            }
        }




        public List<Student> GetStudentsWithoutMajor(int facultyId)
        {
            using (var context = new Model1())
            {
                return context.Students
                    .Where(p => p.MajorID == null && p.FacultyID == facultyId)
                    .ToList();
            }
        }

        public Student FindById(string studentID)
        {
            try
            {
                Model1 context = new Model1();
                // Giả sử bạn đang dùng một phương thức tìm kiếm từ cơ sở dữ liệu
                Student student = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                return student;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
