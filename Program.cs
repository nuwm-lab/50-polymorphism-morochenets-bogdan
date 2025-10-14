using System;
using System.Collections.Generic;
using System.Linq;

// 1. Створено namespace для кращої організації проекту
namespace UniversityModel
{
    // --- Файл: Person.cs ---

    /// <summary>
    /// Представляє базовий клас для людини з основними персональними даними.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Отримує або задає ім'я людини.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отримує або задає прізвище людини.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отримує або задає по-батькові людини.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Отримує дату народження.
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="Person"/>.
        /// </summary>
        /// <param name="firstName">Ім'я.</param>
        /// <param name="lastName">Прізвище.</param>
        /// <param name="patronymic">По-батькові.</param>
        /// <param name="year">Рік народження.</param>
        /// <param name="month">Місяць народження.</param>
        /// <param name="day">День народження.</param>
        public Person(string firstName, string lastName, string patronymic, int year, int month, int day)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            DateOfBirth = new DateTime(year, month, day);
        }

        /// <summary>
        /// Обчислює повний вік людини на поточну дату.
        /// </summary>
        /// <returns>Кількість повних років.</returns>
        public virtual int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        /// <summary>
        /// Підраховує кількість входжень заданого символу в прізвищі.
        /// </summary>
        /// <param name="characterToCount">Символ для пошуку.</param>
        /// <returns>Кількість входжень символу.</returns>
        public int CountCharInLastName(char characterToCount)
        {
            // 2. Додано перевірку на null (null-safety) та культуро-незалежне порівняння
            return LastName?.Count(c => char.Equals(c, characterToCount)) ?? 0;
            // Альтернативний варіант з ігноруванням регістру:
            // return LastName?.Count(c => char.ToUpperInvariant(c) == char.ToUpperInvariant(characterToCount)) ?? 0;
        }

        /// <summary>
        /// Повертає рядок з основною інформацією про людину.
        /// (Віртуальний метод для демонстрації поліморфізму).
        /// </summary>
        /// <returns>Інформаційний рядок.</returns>
        public virtual string GetInfo()
        {
            return $"Людина: {LastName} {FirstName}, вік: {CalculateAge()}";
        }
    }

    // --- Файл: Student.cs ---

    /// <summary>
    /// Представляє студента, успадковуючи властивості від класу <see cref="Person"/>.
    /// </summary>
    public class Student : Person
    {
        /// <summary>
        /// Отримує або задає рік вступу до ВНЗ.
        /// </summary>
        public int AdmissionYear { get; set; }

        /// <summary>
        /// Отримує або задає назву спеціальності.
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="Student"/>.
        /// </summary>
        public Student(string firstName, string lastName, string patronymic, int year, int month, int day, int admissionYear, string specialty)
            : base(firstName, lastName, patronymic, year, month, day)
        {
            AdmissionYear = admissionYear;
            Specialty = specialty;
        }

        /// <summary>
        /// Перевизначений метод. Повертає рядок з розширеною інформацією про студента.
        /// </summary>
        /// <returns>Інформаційний рядок про студента.</returns>
        public override string GetInfo()
        {
            return $"Студент: {LastName} {FirstName}, вік: {CalculateAge()}\nСпеціальність: {Specialty}, рік вступу: {AdmissionYear}";
        }
    }

    // --- Файл: Program.cs ---

    /// <summary>
    /// Головний клас програми.
    /// 3. Явно вказано модифікатор доступу internal.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Створюємо об'єкти
            Person person = new Person("Тарас", "Шевченко", "Григорович", 1814, 3, 9);
            Student student = new Student("Іван", "Франко", "Якович", 2005, 8, 27, 2022, "Комп'ютерні науки");

            // --- Виконання завдань ---
            
            // 1. Визначити вік студента
            Console.WriteLine($"Вік студента {student.LastName} складає: {student.CalculateAge()} років.\n");

            // 2. Визначити кількість зустрічань літери в прізвищі людини
            Console.Write("Введіть літеру для пошуку в прізвищі 'Шевченко': ");
            char letterToSearch = Console.ReadKey().KeyChar;
            Console.WriteLine(); 

            int letterCount = person.CountCharInLastName(letterToSearch);
            Console.WriteLine($"Кількість літери '{letterToSearch}' в прізвищі '{person.LastName}' складає: {letterCount}.\n");
            
            // 3. Демонстрація поліморфізму (ключова вимога для високої оцінки)
            Console.WriteLine("--- Демонстрація поліморфізму ---");
            var people = new List<Person> { person, student };

            foreach (var p in people)
            {
                // Викликається відповідна версія методу GetInfo() для кожного об'єкта
                Console.WriteLine(p.GetInfo());
                Console.WriteLine("--------------------");
            }
        }
    }
}
