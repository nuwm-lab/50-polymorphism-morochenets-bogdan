using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityModel
{
    // --- Файл: Person.cs ---

    /// <summary>
    /// Представляє базовий клас для людини з основними персональними даними.
    /// </summary>
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="Person"/> з валідацією вхідних даних.
        /// </summary>
        /// <exception cref="ArgumentException">Викидається, якщо рядкові параметри порожні або дата некоректна.</exception>
        public Person(string firstName, string lastName, string patronymic, int year, int month, int day)
        {
            // --- 1. ВАЛІДАЦІЯ КОНСТРУКТОРА ---
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
                // Перетворюємо системний виняток у більш зрозумілий для користувача
                throw new ArgumentException("Вказана некоректна дата народження (неправильний день, місяць або рік).", ex);
            }

            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }
        
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
        /// Підраховує кількість входжень заданого символу в прізвищі (без урахування регістру).
        /// </summary>
        public int CountCharInLastName(char characterToCount)
        {
            // --- 2. ПОЛІПШЕНЕ ПОРІВНЯННЯ СИМВОЛІВ ---
            char invariantChar = char.ToUpperInvariant(characterToCount);
            return LastName?.Count(c => char.ToUpperInvariant(c) == invariantChar) ?? 0;
        }

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
        public int AdmissionYear { get; set; }
        public string Specialty { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="Student"/>.
        /// </summary>
        /// <param name="firstName">Ім'я.</param>
        /// <param name="lastName">Прізвище.</param>
        /// <param name="patronymic">По-батькові.</param>
        /// <param name="year">Рік народження.</param>
        /// <param name="month">Місяць народження.</param>
        /// <param name="day">День народження.</param>
        /// <param name="admissionYear">Рік вступу до ВНЗ.</param>
        /// <param name="specialty">Назва спеціальності.</param>
        /// <exception cref="ArgumentException">Викидається, якщо спеціальність порожня.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Викидається, якщо рік вступу некоректний.</exception>
        public Student(string firstName, string lastName, string patronymic, int year, int month, int day, int admissionYear, string specialty)
            : base(firstName, lastName, patronymic, year, month, day) // Валідація базового класу викликається автоматично
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Назва спеціальності не може бути порожньою.", nameof(specialty));
            
            // Рік вступу не може бути раніше, ніж людині виповнилось 16 років
            if (admissionYear < DateOfBirth.Year + 16)
                throw new ArgumentOutOfRangeException(nameof(admissionYear), $"Неможливий рік вступу ({admissionYear}) для народженого в {DateOfBirth.Year} році.");

            AdmissionYear = admissionYear;
            Specialty = specialty;
        }

        public override string GetInfo()
        {
            return $"Студент: {LastName} {FirstName}, вік: {CalculateAge()}\nСпеціальність: {Specialty}, рік вступу: {AdmissionYear}";
        }
    }

    // --- Файл: Program.cs ---

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // Створюємо об'єкти. Якщо дані будуть невірні, тут виникне виняток.
                Person person = new Person("Тарас", "Шевченко", "Григорович", 1814, 3, 9);
                Student student = new Student("Іван", "Франко", "Якович", 2005, 8, 27, 2022, "Комп'ютерні науки");

                // Виконання завдань
                RunTasks(person, student);
            }
            catch (ArgumentException ex)
            {
                // --- 4. ОБРОБКА ВИНЯТКІВ ---
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nПОМИЛКА при створенні об'єкта: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Виконує основні завдання програми.
        /// </summary>
        private static void RunTasks(Person person, Student student)
        {
            Console.WriteLine($"Вік студента {student.LastName} складає: {student.CalculateAge()} років.\n");

            // --- 3. НАДІЙНА ОБРОБКА ВВОДУ ---
            char letterToSearch;
            while (true)
            {
                Console.Write("Введіть ОДНУ літеру для пошуку в прізвищі 'Шевченко' та натисніть Enter: ");
                string input = Console.ReadLine();

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
    }
}
