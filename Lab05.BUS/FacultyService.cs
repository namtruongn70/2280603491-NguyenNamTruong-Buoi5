using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05.DAL.Entities;

namespace Lab05.BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            Model1 context = new Model1();
            return context.Faculties.ToList();
        }

        public void InsertOrUpdateFaculty(Faculty faculty)
        {
            using (var context = new Model1())
            {
                if (faculty.FacultyID == 0) // Thêm mới
                {
                    context.Faculties.Add(faculty);
                }
                else // Cập nhật
                {
                    var existingFaculty = context.Faculties.Find(faculty.FacultyID);
                    if (existingFaculty != null)
                    {
                        existingFaculty.FacultyName = faculty.FacultyName;
                        // Cập nhật các thuộc tính khác nếu cần
                    }
                }
                context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }

        public void DeleteFaculty(int facultyId)
        {
            using (var context = new Model1())
            {
                var faculty = context.Faculties.Find(facultyId);
                if (faculty != null)
                {
                    context.Faculties.Remove(faculty);
                    context.SaveChanges();
                }
            }
        }
    }
}
