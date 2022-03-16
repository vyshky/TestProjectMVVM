using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace TestProjectMVVM
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyChanging([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        List<Student> _students;
        List<Teacher> _teacher;
        SchoolContext _schoolContext;
        public static AddFormStudent addFormStudent;
        public static AddFormTeacher addFormTeacher;
        public static ChangeFormStudent changeFormStudent;
        public static ChangeFormTeacher changeFormTeacher;
        string _findText;
        bool _chBoxSortStudentsName;
        bool _chBoxSortStudentsGroup;
        bool _chBoxSortTeachersName;
        bool _chBoxSortTeachersFacility;

        public ViewModel()
        {
            _students = new List<Student>();
            _teacher = new List<Teacher>();
            RefreshView();
        }

        //Сортировка 
        ////////////////////////////////////////////////////////////////////////////////////
        public bool ChBoxSortStudentsName
        {
            get
            {
                return _chBoxSortStudentsName;
            }
            set
            {
                _chBoxSortStudentsName = value;
                SortStudents();

            }
        }
        public bool ChBoxSortStudentsGroup
        {
            get
            {
                return _chBoxSortStudentsGroup;
            }
            set
            {
                _chBoxSortStudentsGroup = value;
                SortStudents();
            }
        }
        public bool ChBoxSortTeachersName
        {
            get
            {
                return _chBoxSortTeachersName;
            }
            set
            {
                _chBoxSortTeachersName = value;
                SortTeachers();

            }
        }
        public bool ChBoxSortTeachersFacility
        {
            get
            {
                return _chBoxSortTeachersFacility;
            }
            set
            {
                _chBoxSortTeachersFacility = value;
                SortTeachers();
            }
        }

        public void SortStudents()
        {
            if (_chBoxSortStudentsName && _chBoxSortStudentsGroup)
            {
                Students = (from student in _schoolContext.Students
                            orderby student.FIO, student.Group ascending
                            select student).ToList();
            }
            else if (_chBoxSortStudentsName && _chBoxSortStudentsGroup == false)
            {
                Students = (from student in _schoolContext.Students
                            orderby student.FIO ascending
                            select student).ToList();
            }
            else if (_chBoxSortStudentsName == false && _chBoxSortStudentsGroup)
            {
                Students = (from student in _schoolContext.Students
                            orderby student.Group ascending
                            select student).ToList();
            }
            else
            {
                Students = (from student in _schoolContext.Students
                            select student).ToList();
            }
        }

        public void SortTeachers()
        {
            if (_chBoxSortTeachersName && _chBoxSortTeachersFacility)
            {
                Teachers = (from teacher in _schoolContext.Teachers
                            orderby teacher.FIO, teacher.Facility ascending
                            select teacher).ToList();
            }
            else if (_chBoxSortTeachersName && _chBoxSortTeachersFacility == false)
            {
                Teachers = (from teacher in _schoolContext.Teachers
                            orderby teacher.FIO ascending
                            select teacher).ToList();
            }
            else if (_chBoxSortTeachersName == false && _chBoxSortTeachersFacility)
            {
                Teachers = (from teacher in _schoolContext.Teachers
                            orderby teacher.Facility ascending
                            select teacher).ToList();
            }
            else
            {
                Teachers = (from teacher in _schoolContext.Teachers
                            select teacher).ToList();
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////
     



        // Поиск
        ////////////////////////////////////////////////////////////////////////////////////
        public string FindStudent
        {
            set
            {
                _findText = value;
                FindStudents(_findText);
            }
        }
        public string FindTeacher
        {
            set
            {
                _findText = value;
                FindTeachers(_findText);
            }
        }

        void FindStudents(string text = "")
        {
            Students = (from student in _schoolContext.Students
                        where student.FIO.Contains(text)
                        select student).ToList();
        }
        void FindTeachers(string text = "")
        {
            Teachers = (from student in _schoolContext.Teachers
                        where student.FIO.Contains(text)
                        select student).ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////


        // добавить удалить изменить
        ////////////////////////////////////////////////////////////////////////////////////
        public ICommand AddStudent
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        addFormStudent = new AddFormStudent();
                        addFormStudent.ShowDialog();
                        RefreshView();
                    }
                    );
            }
        }
        public ICommand AddTeacher
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        addFormTeacher = new AddFormTeacher();
                        addFormTeacher.ShowDialog();
                        RefreshView();
                    }
                    );
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new ButtonsCommand(
                () =>
                {
                    if (SelectedItem != null && SelectedItem.GetType() == typeof(Teacher))
                    {
                        Teachers.Remove((Teacher)SelectedItem);
                        _schoolContext.Teachers.Remove((Teacher)SelectedItem);
                        _schoolContext.SaveChanges();
                        Teachers = new List<Teacher>(Teachers);
                    }
                    else if (SelectedItem != null && SelectedItem.GetType() == typeof(Student))
                    {
                        Students.Remove((Student)SelectedItem);
                        _schoolContext.Students.Remove((Student)SelectedItem);
                        _schoolContext.SaveChanges();
                        Students = new List<Student>(Students);
                    }
                }
                );
            }
        }

        public ICommand ChangeButton
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        if (SelectedItem == null)
                        {
                            MessageBox.Show("Вы не выбрали элемент для изменения");
                        }
                        else if (SelectedItem.GetType() == typeof(Teacher))
                        {
                            changeFormTeacher = new ChangeFormTeacher();
                            changeFormTeacher.ShowDialog();
                            RefreshView();
                        }
                        else if (SelectedItem.GetType() == typeof(Student))
                        {
                            changeFormStudent = new ChangeFormStudent();
                            changeFormStudent.ShowDialog();
                            RefreshView();
                        }
                    }
                    );
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////


        // обновить view
        public void RefreshView()
        {
            _schoolContext = new SchoolContext();
            Students = (from student in _schoolContext.Students select student).ToList();
            Teachers = (from teacher in _schoolContext.Teachers select teacher).ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////


        // листы 
        public List<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                PropertyChanging();
            }
        }
        public List<Teacher> Teachers
        {
            get { return _teacher; }
            set
            {
                _teacher = value;
                PropertyChanging();
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////


        // селектор
        public static SelectedItem SelectedItem { get; set; }
        ////////////////////////////////////////////////////////////////////////////////////
    }
}
