using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using System.Windows;

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

        public ViewModel()
        {           
            _students = new List<Student>();
            _teacher = new List<Teacher>();
            RefreshView();
        }

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

        public void RefreshView()
        {
            _schoolContext = new SchoolContext();
            Students = (from student in _schoolContext.Students select student).ToList();
            Teachers = (from teacher in _schoolContext.Teachers select teacher).ToList();
        }
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

        public static SelectedItem SelectedItem { get; set; }
    }
}
