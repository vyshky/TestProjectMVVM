using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TestProjectMVVM
{
    public class AddFormView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void PropertyChanging([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        SchoolContext _schoolContext;
        Teacher teacher;
        string facility;
        string fio;
        string group;
        int gpa;
        int id;

        public AddFormView()
        {
            _schoolContext = new SchoolContext();
        }

        public string Facility
        {
            get { return facility; }
            set
            {
                facility = value;
                PropertyChanging("Facility");
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                PropertyChanging("Id");
            }
        }
        public int Gpa
        {
            get { return gpa; }
            set
            {
                gpa = value;
                PropertyChanging("Gpa");
            }
        }
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                PropertyChanging("Group");
            }
        }

        public string Fio
        {
            get { return fio; }
            set
            {
                fio = value;
                PropertyChanging("Fio");
            }
        }

        public ICommand AddButtonStudent
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        teacher = (from teacher in _schoolContext.Teachers where teacher.Id == Id select teacher).FirstOrDefault();

                        if (teacher != null)
                        {
                            Student newStudent = new Student()
                            {
                                FIO = fio,
                                Group = group,
                                GradePointAverage = gpa,
                                Teacher = teacher
                            };
                            _schoolContext.Students.Add(newStudent);
                            _schoolContext.SaveChanges();
                        }
                        else
                            MessageBox.Show("Преподавателя нет в нашей базе данных");
                    }
                    );
            }
        }

        public ICommand AddButtonTeacher
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        Teacher newTeacher = new Teacher()
                        {
                            FIO = fio,
                            Facility = facility
                        };
                        _schoolContext.Teachers.Add(newTeacher);
                        _schoolContext.SaveChanges();
                    }
                    );
            }
        }

        public ICommand UpdateButton
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        if (ViewModel.SelectedItem.GetType() == typeof(Teacher))
                        {
                            var selectedItem = (Teacher)ViewModel.SelectedItem;
                            Teacher teacher = _schoolContext.Teachers.Find(selectedItem.Id);
                            teacher.FIO = fio;
                            teacher.Facility = facility;
                        }
                        else if (ViewModel.SelectedItem.GetType() == typeof(Student))
                        {
                            var selectedItem = (Student)ViewModel.SelectedItem;
                            Student student = _schoolContext.Students.Find(selectedItem.Id);
                            student.FIO = fio;
                            student.Teacher = teacher;
                            student.Group = group;
                            student.GradePointAverage = gpa;
                        }
                        _schoolContext.SaveChanges();
                    }
                    );
            }
        }

        public ICommand CloseButton
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        ViewModel.addFormStudent?.Close();
                        ViewModel.addFormTeacher?.Close();
                        ViewModel.changeFormTeacher?.Close();
                        ViewModel.changeFormStudent?.Close();
                    }
                    );
            }
        }
    }
}
