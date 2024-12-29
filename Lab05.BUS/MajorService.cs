using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab05.DAL.Entities;

namespace Lab05.BUS
{
    public class MajorService
    {
        public List<Major> GetAllByFaculty(int facultyID)
        {
            Model1 context = new Model1();
            return context.Majors.Where(p => p.FacultyID == facultyID).ToList();
        }
    
     public void RegisterStudentsToMajor(List<long> selectedStudentIds, int selectedMajorId)
        {

            using (var context = new Model1())
            {
                foreach (var studentId in selectedStudentIds)
                {
                    string studentIdString = studentId.ToString();

                    var student = context.Students.SingleOrDefault(s => s.StudentID == studentIdString);
                    if (student != null && student.MajorID == null)
                    {
                        student.MajorID = selectedMajorId;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
