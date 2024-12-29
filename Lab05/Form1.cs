using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab05.BUS;
using Lab05.DAL.Entities;

namespace Lab05
{
  
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public Form1()
        {
            InitializeComponent();
        }
        private string avatarFilePath = string.Empty;
        private string sourceFilePath;
        private object studentID;
        private object studentId;
        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {

        }

        private void đăngKýChuyênNghànhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                {
                    var student = studentService.FindById(txtStudentID.Text) ?? new Student();

                    // Update student details
                    student.StudentID = txtStudentID.Text;
                    student.FullName = txtFullName.Text;
                    student.AverageScore = double.Parse(txtGPA.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    // Check if an avatar file has been selected
                    if (!string.IsNullOrEmpty(avatarFilePath))
                    {
                        string avatarFileName = SaveAvatar(avatarFilePath, txtStudentID.Text);
                        if (!string.IsNullOrEmpty(avatarFileName))
                        {
                            student.Avatar = avatarFileName;
                        }
                    }

                    // Save the student to the database
                    studentService.InsertOrUpdateStudent(student);

                    // Refresh the grid or UI
                    BindGrid(studentService.GetAll());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvSINHVIEN.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvSINHVIEN.Rows.Add();
                dgvSINHVIEN.Rows[index].Cells[0].Value = item.StudentID;
                dgvSINHVIEN.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvSINHVIEN.Rows[index].Cells[2].Value =
                    item.Faculty.FacultyName;
                dgvSINHVIEN.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvSINHVIEN.Rows[index].Cells[4].Value = item.Major.Name + "";
                ShowAvatar(item.Avatar);
            }
        }
        private void LoadAvatar(string studentID)
        {
            string folderPath = Path.Combine(Application.StartupPath, "Images");
            var student = studentService.FindById(studentID);
            if (student != null && !string.IsNullOrEmpty(student.Avatar))
            {
                string avatarFilePath = Path.Combine(folderPath, student.Avatar);
                if (File.Exists(avatarFilePath))
                {
                    picAvatar.Image = Image.FromFile(avatarFilePath);
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        private void ShowAvatar(string avatar)
        {
            try
            {
                if (!string.IsNullOrEmpty(avatar) && System.IO.File.Exists(avatar))
                {
                    picAvatar.Image = Image.FromFile(avatar);
                }
                else
                {

                    picAvatar.Image = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi khi hiển thị avatar: " + ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!double.TryParse(txtGPA.Text, out double gpa) || gpa < 0 || gpa > 10)
            {
                MessageBox.Show("Điểm trung bình không hợp lệ! Vui lòng nhập số từ 0 đến 10.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private string SaveAvatar(string avatarFilePath, string text)
        {
            try
            {
                string folderPath = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileExtension = Path.GetExtension(avatarFilePath);
                string targetFilePath = Path.Combine(folderPath, $"{studentID}{fileExtension}");

                if (!File.Exists(avatarFilePath))
                {
                    throw new FileNotFoundException($"Không tìm thấy file: {avatarFilePath}");
                }

                File.Copy(avatarFilePath, targetFilePath, true);
                return $"{studentID}{fileExtension}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu ảnh đại diện: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSINHVIEN);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setGridViewStyle(DataGridView dgvStudent)
        {
            dgvSINHVIEN.BorderStyle = BorderStyle.None;
            dgvSINHVIEN.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvSINHVIEN.CellBorderStyle =
            DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSINHVIEN.BackgroundColor = Color.White;
            dgvSINHVIEN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSINHVIEN.CurrentRow != null)
                {
                    string studentID = dgvSINHVIEN.CurrentRow.Cells[0].Value?.ToString();
                    MessageBox.Show($"StudentID: {studentID}", "Thông báo");

                    var stu = studentService.FindById(studentID);
                    if (stu == null)
                    {
                        MessageBox.Show("Không tìm thấy sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    studentService.Delete(stu);
                    MessageBox.Show("Xóa thành công!", "Thông báo");

                    // Cập nhật lại danh sách
                    BindGrid(studentService.GetAll());
                }
                else
                {
                    MessageBox.Show("Không có dòng nào được chọn!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    avatarFilePath = openFileDialog.FileName;
                    picAvatar.Image = Image.FromFile(avatarFilePath);
                }
            }
        }

        private void đăngKíChuyênNgànhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDKCN frm = new frmDKCN();
            frm.Show();
        }

       
    }
}

