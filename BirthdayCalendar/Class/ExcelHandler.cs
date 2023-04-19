using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace BirthdayCalendar.Class
{       /// <summary>
        /// Объект для работы с ексель файлом
        /// </summary>
    public class ExcelHandler
    {
        /// <summary>
        /// Объект для работы с ексель файлом
        /// </summary>
        public ExcelHandler() { }
        /// <summary>
        /// Создоёт ObservableCollection<Employee> из ексель файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public ObservableCollection<Employee> GetEmployeeFail (string path)
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
            Employee employee;            

            //Создание объекта приложения excel
            Excel.Application excelApp = new Excel.Application();
            //Сделать ексель видимым
            excelApp.Visible = false;
            //Открыть рабочую книгу
            Excel.Workbook workbook = excelApp.Workbooks.Open(path);
            //Получить первый рабочий лист
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
            //Получает последнюю строку в эксель файле
            int lastRow = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

            int firstRow = 10;
            int countRow = lastRow - firstRow;
            //Заполняет коллекцию
            try
            {
                for (int i = 0; i < countRow; i++)
                {
                    if (worksheet.Cells[firstRow + i, 1].value != "")
                    {
                        DateTime dt = DateTime.Parse(worksheet.Cells[firstRow + i, 4].value);
                        employee = new Employee(worksheet.Cells[firstRow + i, 1].value,
                            worksheet.Cells[firstRow + i, 7].value,
                            worksheet.Cells[firstRow + i, 11].value,
                            dt);
                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
            //Закрыть книгу без изменений
            workbook.Close(false, Type.Missing, Type.Missing);
            //Закрыть приложение ексель
            excelApp.Quit();

            return employees;
        }
    }
}
