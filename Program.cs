using System;
using System.Linq;

// --- Базовий клас "Людина" ---
public class Person
{
    // Властивості для зберігання даних
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateOfBirth { get; private set; }

    // Конструктор для ініціалізації об'єкта (задання даних)
    public Person(string firstName, string lastName, string patronymic, int year, int month, int day)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        DateOfBirth = new DateTime(year, month, day);
    }

    // Метод для визначення віку
    public virtual int CalculateAge()
    {
        DateTime today = DateTime.Today;
        int age = today.Year - DateOfBirth.Year;

        // Перевірка, чи був день народження в цьому році
        if (DateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }

    // Метод для обчислення кількості зустрічань літери в прізвищі
    public int CountCharInLastName(char characterToCount)
    {
        // Перетворюємо прізвище і шуканий символ в нижній регістр для пошуку без урахування регістру
        return LastName.ToLower().Count(c => c == char.ToLower(characterToCount));
    }
    
    public override string ToString()
    {
        return $"Людина: {LastName} {FirstName} {Patronymic}, Дата народження: {DateOfBirth:dd.MM.yyyy}";
    }
}

// --- Похідний клас "Студент" ---
public class Student : Person
{
    // Додаткові поля для студента
    public int AdmissionYear { get; set; }
    public string Specialty { get; set; }

    // Конструктор, який викликає конструктор базового класу для ініціалізації основних полів
    public Student(string firstName, string lastName, string patronymic, int year, int month, int day, int admissionYear, string specialty)
        : base(firstName, lastName, patronymic, year, month, day) // Виклик конструктора базового класу
    {
        AdmissionYear = admissionYear;
        Specialty = specialty;
    }

    // Метод CalculateAge() та CountCharInLastName() успадковуються автоматично.
    
    public override string ToString()
    {
        return $"Студент: {LastName} {FirstName} {Patronymic}, Дата народження: {DateOfBirth:dd.MM.yyyy}\nСпеціальність: {Specialty}, Рік вступу: {AdmissionYear}";
    }
}

// --- Головна програма ---
class Program
{
    static void Main(string[] args)
    {
        // Встановлюємо кодування консолі для коректного відображення українських літер
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        // 1. Створюємо об'єкти класів „людина” та „студент”
        Person person = new Person("Тарас", "Шевченко", "Григорович", 1814, 3, 9);
        Student student = new Student("Іван", "Франко", "Якович", 2005, 8, 27, 2022, "Комп'ютерні науки");

        Console.WriteLine("--- Створені об'єкти ---");
        Console.WriteLine(person);
        Console.WriteLine(student);
        Console.WriteLine("------------------------\n");

        // 2. За поточною введеною датою визначити вік студента
        int studentAge = student.CalculateAge(); // Викликаємо успадкований метод
        Console.WriteLine($"Сьогоднішня дата: {DateTime.Today:dd.MM.yyyy}");
        Console.WriteLine($"Вік студента {student.FirstName} {student.LastName} складає: {studentAge} років.\n");

        // 3. Визначити кількість зустрічань введеної літери в прізвищі людини
        Console.Write("Введіть літеру для пошуку в прізвищі людини ('Шевченко'): ");
        // Читаємо один символ з консолі
        char letterToSearch = Console.ReadKey().KeyChar;
        Console.WriteLine(); // Перехід на новий рядок після вводу символа

        int letterCount = person.CountCharInLastName(letterToSearch);
        Console.WriteLine($"Кількість літери '{letterToSearch}' в прізвищі '{person.LastName}' складає: {letterCount}.");
    }
}
