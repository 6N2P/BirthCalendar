using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayCalendar.Class
{
    public class Employee
    {
        public string Name { get; set; }        
        public string Division { get; set; }
        public string Position { get; set; }        
        public DateTime DateOfBirth { get; set; }

        public Employee() { }
        public Employee(string name, string division, string position, DateTime dateOfBirth)
        {
            Name = name;            
            Division = division;
            Position = position;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            string result = $"Ф.И.О: {Name}; Подразделенин: {Division}; Должность: {Position}; Дата рождения: {DateOfBirth.ToString("d")}";
            return result;
        }
    }
}
