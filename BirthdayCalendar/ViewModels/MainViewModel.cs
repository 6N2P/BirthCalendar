using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BirthdayCalendar.Class;

namespace BirthdayCalendar.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion PropertyChange

        public MainViewModel (MainWindow window)
        {
            Date = DateTime.Now;

            // string path1 = @"C:\Users\Ivanovsv\Downloads\Telegram Desktop\ДР.xlsx";
            string path1 = @"\\srv-fs02.tsm.local\SYSTEM\OIT_DR\DR_Construkciy.xlsx";
            string path2 = @"\\srv-fs02.tsm.local\SYSTEM\OIT_DR\DR_StalMost.xlsx";

#warning как будет актуальный файл, поменять на DR_OOO_STM.xlsx
            string path3 = @"\\srv-fs02.tsm.local\SYSTEM\OIT_DR\DR_OOO_STM1.xlsx";

            ExcelHandler excelHandler=new ExcelHandler();
            
            #region Получает данные из файла на сервере
            ObservableCollection<Employee> employeesConstrukciy = new ObservableCollection<Employee>();
            if (File.Exists(path1))
                employeesConstrukciy = excelHandler.GetEmployeeFail(path1);


            ObservableCollection<Employee> employeesStalMost = new ObservableCollection<Employee>();
            if (File.Exists(path2))
                employeesStalMost = excelHandler.GetEmployeeFail(path2);

            ObservableCollection<Employee> employeesZavod50 = new ObservableCollection<Employee>();
            if (File.Exists(path3))
                employeesZavod50 = excelHandler.GetEmployeeFail(path3);
            #endregion Получает данные из файла на сервере
                        
            #region Добовляет полученные данные в основной список
            employees = new ObservableCollection<Employee>();

            if (employeesConstrukciy != null)
            {
                foreach (var employee in employeesConstrukciy)
                {
                    employees.Add(employee);
                }
            }

            if (employeesStalMost != null)
            {
                foreach (var employee in employeesStalMost)
                {
                    employees.Add(employee);
                }
            }

            if(employeesZavod50 != null)
            {
                foreach (var employee in employeesZavod50)
                {
                    employees.Add(employee);
                }
            }
            
            #endregion Добовляет полученные данные в основной список

            DateBerthd();
            GetEmployeesByMonth(Date);     
        }

        #region Fields        
        ObservableCollection<Employee> employees;
        ObservableCollection<Employee> _employesObs;       
        DateTime _date;
        string _searchName;
        int _countEmployeByView;
        #endregion Fields

        #region Property
        public int CountEmployeByView
        {
            get => _countEmployeByView;
            set
            {
                _countEmployeByView = value;
                OnPropertyChanged("CountEmployeByView");
            }
        }
        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                OnPropertyChanged("SearchName");
            }
        }
        public HashSet<DateTime> Dates { get; } = new HashSet<DateTime>();
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                
                OnPropertyChanged("Date");
               
            }
        }
        public ObservableCollection<Employee> EmployesObs
        {
            get => _employesObs;
            set
            {
                _employesObs = value;
                OnPropertyChanged("EmployesObs");
                CountEmployeByView =_employesObs.Count;
            }
        }
        #endregion Property

        #region Commands
        DelegateCommand _showAllEmployees;
        public DelegateCommand ShowAllEmployees
        {
            get
            {
                return _showAllEmployees ??
                    (_showAllEmployees = new DelegateCommand(obj =>
                    {
                        ShowAllEmployee();
                    }));
            }
        }
        
        DelegateCommand _showEmplotByMonth;
        public DelegateCommand ShowEmplotByMonth
        {
            get
            {
                return _showEmplotByMonth ??
                    (_showEmplotByMonth = new DelegateCommand(obj =>
                    {
                        GetEmployeesByMonth(_date);
                    }));
            }
        }

        DelegateCommand _showEmploeByDate;
        public DelegateCommand ShowEmploeByDate
        {
            get
            {
                return _showEmploeByDate ??
                    (_showEmploeByDate = new DelegateCommand(obj =>
                    {
                        GetEmployeesByDay(_date);
                    }));
            }
        }
        DelegateCommand _searchNameComnand;
        public DelegateCommand SearchNameCommand
        {
            get
            {
                return _searchNameComnand ??
                    (_searchNameComnand = new DelegateCommand(obj =>
                    {
                        DoSearchName();
                    }));
            }
        }
        #endregion Commands

        #region Методы
        /// <summary>
        /// Получает с писок сотрудников у которых ДР в этом месяце
        /// </summary>
        /// <param name="date">Дата чтобы узнать месяц</param>
        void GetEmployeesByMonth(DateTime date)
        {
            var selectEmployes = employees.Where(p => p.DateOfBirth.Month == date.Month).ToList();
            if (EmployesObs != null) EmployesObs.Clear();

            ObservableCollection<Employee> em = new ObservableCollection<Employee>();
            foreach (Employee employee in selectEmployes)
            {
                em.Add(employee);
            }
            EmployesObs = em;
        }
        /// <summary>
        /// Получает список сотрудников у которых ДР в этот день
        /// </summary>
        /// <param name="date">Дата</param>
        void GetEmployeesByDay(DateTime date)
        {
            var selectEmployes = employees.Where(p => p.DateOfBirth.Month == date.Month && p.DateOfBirth.Day == date.Day).ToList();
            if (EmployesObs != null) EmployesObs.Clear();

            ObservableCollection<Employee> em = new ObservableCollection<Employee>();
            foreach (Employee employee in selectEmployes)
            {
                em.Add(employee);
            }
            EmployesObs = em;
        }

        void ShowAllEmployee()
        {
            ObservableCollection<Employee> em = new ObservableCollection<Employee>();
            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    em.Add(employee);
                }
                EmployesObs = em;
            }
        }
        /// <summary>
        /// Получает список дней рождений для отображения в календаре
        /// </summary>
        void DateBerthd()
        {
            //  HashSet<DateTime> dates = new HashSet<DateTime>();

            string year = DateTime.Now.Year.ToString();
            List<DateTime> dateList = new List<DateTime>();
            bool flag = false;
            foreach (var dateBerth in employees)
            {
                string month = dateBerth.DateOfBirth.Month.ToString();
                string day = dateBerth.DateOfBirth.Day.ToString();
                string date = $"{day}.{month}.{year}";
                DateTime dateNew = new DateTime();

                flag = DateTime.TryParse(date, out dateNew);

                if (flag == true) dateList.Add(dateNew);
                flag = false;
            }
            dateList.Sort();
            var listClean = dateList.Distinct().ToHashSet();
            foreach (var listDate in listClean)
            {
                this.Dates.Add(listDate);
            }
        }
        /// <summary>
        /// Поиск по имени
        /// </summary>
        void DoSearchName()
        {
            ObservableCollection<Employee> searchNameOb = new ObservableCollection<Employee>();
            if (!string.IsNullOrEmpty(_searchName))
            {
                var serchEmployee = employees.Where(e => e.Name.Contains(_searchName));
                foreach (var employee in serchEmployee)
                {
                    searchNameOb.Add(employee);
                }
                EmployesObs = searchNameOb;
            }
        } 
        #endregion Методы
    }
}
