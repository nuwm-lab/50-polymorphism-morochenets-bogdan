#nullable enable // 2. Вмикаємо перевірку на null для підвищення безпеки коду

using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityModel
{
    public class Person
    {
        // 1. Властивості тепер незмінні після створення об'єкта (init замість set)
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Patronymic { get; init; }
        public DateTime DateOfBirth { get; private set; }

        public Person(string firstName, string lastName, string patronymic, int year, int month, int day)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(patronymic))
                throw new ArgumentException("По-батькові не може бути порожнім.", nameof(patronymic));

            try
            {
                DateOfBirth = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentException("Вказана некоректна дата народження.", ex);
            }

            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }
        
        /// <summary>
        /// Обчислює повний вік людини на основі заданої поточної дати.
        /// </summary>
        /// <param name="currentDate">Дата, на яку потрібно обчислити вік.</param>
        /// <returns>Кількість повних років.</returns>
        public int CalculateAge(DateTime currentDate)
        {
            int age = currentDate.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > currentDate.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public int CountCharInLastName(char characterToCount)
        {
            char invariantChar = char.ToUpperInvariant(characterToCount);
            return LastName.Count(c => char.ToUpperInvariant(c) == invariantChar);
        }

        public virtual string GetInfo()
        {
            return $"Людина: {LastName} {FirstName}, вік: {CalculateAge(DateTime.Today)}";
        }
    }

    public class Student : Person
    {
        public int AdmissionYear { get; init; }
        public string Specialty { get; init; }

        public Student(string firstName, string lastName, string patronymic, int year, int month, int day, int admissionYear, string specialty)
            : base(firstName, lastName, patronymic, year, month, day)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Назва спеціальності не може бути порожньою.", nameof(specialty));
            
            // 3. Більш точна валідація: перевіряємо, чи виповнилось 16 років на 1 вересня року вступу
            DateTime admissionDate = new DateTime(admissionYear, 9, 1);
            if (admissionDate < DateOfBirth.AddYears(16))
                throw new ArgumentOutOfRangeException(nameof(admissionYear), $"Неможливий рік вступу ({admissionYear}): студенту має бути не менше 16 років.");

            AdmissionYear = admissionYear;
            Specialty = specialty;
        }

        public override string GetInfo()
        {
            return $"Студент: {LastName} {FirstName}, вік: {CalculateAge(DateTime.Today)}\nСпеціальність: {Specialty}, рік вступу: {AdmissionYear}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // 4. Запускаємо Unit-тести для перевірки коректності логіки
            RunUnitTests();

            try
            {
                Person person = new Person("Тарас", "Шевченко", "Григорович", 1814, 3, 9);
                Student student = new Student("Іван", "Франко", "Якович", 2005, 8, 27, 2022, "Комп'ютерні науки");
                
                RunTasks(person, student);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nПОМИЛКА при створенні об'єкта: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void RunTasks(Person person, Student student)
        {
            Console.WriteLine($"\nВік студента {student.LastName} складає: {student.CalculateAge(DateTime.Today)} років.\n");

            char letterToSearch;
            while (true)
            {
                Console.Write("Введіть ОДНУ літеру для пошуку в прізвищі 'Шевченко' та натисніть Enter: ");
                string? input = Console.ReadLine();

                if (input?.Length == 1 && char.IsLetter(input[0]))
                {
                    letterToSearch = input[0];
                    break;
                }
                Console.WriteLine("Помилка: потрібно ввести рівно одну літеру. Спробуйте ще раз.");
            }

            int letterCount = person.CountCharInLastName(letterToSearch);
            Console.WriteLine($"Кількість літери '{letterToSearch}' в прізвищі '{person.LastName}' складає: {letterCount}.\n");

            Console.WriteLine("--- Демонстрація поліморфізму ---");
            var people = new List<Person> { person, student };
            foreach (var p in people)
            {
                Console.WriteLine(p.GetInfo());
                Console.WriteLine("--------------------");
            }
        }

        /// <summary>
        /// Простий тестовий фреймворк для перевірки ключової логіки.
        /// У реальному проекті для цього використовуються бібліотеки (MSTest, NUnit, xUnit).
        /// </summary>
        private static void RunUnitTests()
        {
            Console.WriteLine("--- Запуск Unit-тестів ---");
            int testsPassed = 0;
            int totalTests = 0;

            Action<string, bool> Assert = (testName, condition) =>
            {
                totalTests++;
                if (condition)
                {
                    testsPassed++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[УСПІХ] {testName}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ПРОВАЛ] {testName}");
                }
                Console.ResetColor();
            };

            // Тести для CalculateAge
            var today = new DateTime(2025, 10, 14);
            var p1 = new Person("Тест1", "Вік", "Точний", 2005, 5, 20); // 20 років
            var p2 = new Person("Тест2", "Вік", "СьогодніДН", 2005, 10, 14); // 20 років
            var p3 = new Person("Тест3", "Вік", "ЗавтраДН", 2005, 10, 15); // 19 років
            Assert("CalculateAge: Стандартний випадок", p1.CalculateAge(today) == 20);
            Assert("CalculateAge: День народження сьогодні", p2.CalculateAge(today) == 20);
            Assert("CalculateAge: День народження завтра", p3.CalculateAge(today) == 19);

            // Тести для CountCharInLastName
            var searchPerson = new Person("Тест", "Шевченко", "Тест", 2000, 1, 1);
            Assert("CountChar: Кілька входжень", searchPerson.CountCharInLastName('е') == 2);
            Assert("CountChar: Чутливість до регістру (пошук 'Е')", searchPerson.CountCharInLastName('Е') == 2);
            Assert("CountChar: Символ відсутній", searchPerson.CountCharInLastName('я') == 0);
            
            Console.WriteLine($"--- Тести завершено. Пройдено: {testsPassed}/{totalTests} ---\n");
            
            if (testsPassed != totalTests)
            {
                 Console.ForegroundColor = ConsoleColor.Red;
                 Console.WriteLine("!!! УВАГА: Деякі тести не пройдено. Подальше виконання може бути некоректним. !!!\n");
                 Console.ResetColor();
            }
        }
    }
}
