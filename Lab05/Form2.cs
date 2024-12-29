using Lab05.BUS;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05
{
    public partial class frmDKCN : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();

        public frmDKCN()
        {
            InitializeComponent();
        }
        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {

            this.cmbFaculty.DataSource = listFaculties;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";

        }


        private void BindGrid(List<Student> listStudent)
        {
            dgvCHUYENNGANH.Rows.Clear();

            foreach (var item in listStudent)
            {
                int index = dgvCHUYENNGANH.Rows.Add();
                // Cột "Chọn" không cần gán giá trị ban đầu (checkbox mặc định là false)
                dgvCHUYENNGANH.Rows[index].Cells[1].Value = item.StudentID;
                dgvCHUYENNGANH.Rows[index].Cells[2].Value = item.FullName;
                if (item.Faculty != null)
                    dgvCHUYENNGANH.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
                dgvCHUYENNGANH.Rows[index].Cells[4].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvCHUYENNGANH.Rows[index].Cells[5].Value = item.Major.Name + "";
            }
        }

      

        private void FillMajorCombobox(List<Major> listMajor)
        {

            try
            {
                if (listMajor != null && listMajor.Count > 0)
                {
                    cmbMajor.DataSource = listMajor;
                    cmbMajor.DisplayMember = "MajorName";
                    cmbMajor.ValueMember = "MajorID";
                }
                else
                {
                    cmbMajor.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi điền thông tin ngành học: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {

                var listFaculties = facultyService.GetAll();
                FillFacultyCombobox(listFaculties);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> selectedStudentIds = new List<long>();

                foreach (DataGridViewRow row in dgvCHUYENNGANH.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        if (row.Cells[1].Value != null)
                        {
                            string studentIdString = row.Cells[1].Value.ToString();
                            if (!string.IsNullOrEmpty(studentIdString))
                            {
                                long studentId = long.Parse(studentIdString);
                                selectedStudentIds.Add(studentId);
                            }
                            else
                            {
                                MessageBox.Show("MSSV không hợp lệ.");
                            }
                        }
                    }
                }

                if (selectedStudentIds.Count > 0)
                {
                    var selectedMajorId = 0;
                    if (cmbMajor.SelectedValue != null)
                    {
                        if (int.TryParse(cmbMajor.SelectedValue.ToString(), out selectedMajorId))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Chuyên ngành không hợp lệ.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn chuyên ngành.");
                        return;
                    }

                    majorService.RegisterStudentsToMajor(selectedStudentIds, selectedMajorId);

                    MessageBox.Show("Đăng ký thành công!");

                    var selectedFacultyId = Convert.ToInt32(cmbFaculty.SelectedValue);
                    var listStudents = studentService.GetAllHasNoMajor(selectedFacultyId);
                    BindGrid(listStudents);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để đăng ký.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message);
            }
        }

        private void cmbFaculty_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            Faculty selectedFaculty = cmbFaculty.SelectedItem as Faculty;

            if (selectedFaculty != null)
            {
                var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                FillMajorCombobox(listMajor);

                var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindGrid(listStudents);
            }
        }
    }
    }

