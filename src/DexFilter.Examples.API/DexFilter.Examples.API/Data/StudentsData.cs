namespace DexFilter.Examples.API.Data
{
    using Bogus;

    public class StudentsData
    {
        private const int NumberOfStudents = 1000;

        IQueryable<Student> _students;

        public StudentsData()
        {
        }

        public IQueryable<Student> Students
        {
            get
            {
                _students ??= GenerateStudentsData();

                return _students;
            }
        }

        private IQueryable<Student> GenerateStudentsData()
        {
            var cities = new Faker<ValueContainer<string>>()
                .RuleFor(a => a.Value, f => f.Address.City())
                .Generate(50)
                .Select(a => new City
                {
                    Name = a.Value
                })
                .ToList();

            for (int i = 0; i < cities.Count; i++)
            {
                cities[i].Id = i + 1;
            }

            var subjects = new List<Subject>
            {
                new Subject{ Id = 1, Name = "Mathematics" },
                new Subject{ Id = 2, Name = "Science" },
                new Subject{ Id = 3, Name = "Music" },
                new Subject{ Id = 4, Name = "English" },
                new Subject{ Id = 5, Name = "Geography" },
                new Subject{ Id = 6, Name = "Biology" },
                new Subject{ Id = 7, Name = "Chemistry" },
                new Subject{ Id = 8, Name = "Physics" },
            };

            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);

            var rule = new Faker<Student>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Grade, f => f.Random.Byte(1, 12))
                .RuleFor(s => s.BirthDate, (f, s) => startOfYear.AddYears((s.Grade + 7) * -1).AddDays(f.Random.Int(0, 365)))
                .RuleFor(s => s.Address, f =>
                {
                    var address = f.Address;

                    return new Address
                    {
                        City = f.PickRandom(cities),
                        Street = address.StreetAddress(),
                        PostalCode = address.ZipCode(),
                    };
                })
                .RuleFor(s => s.Subjects, f => subjects
                                                .OrderBy(x => f.Random.Int())
                                                .Take(f.Random.Int(1, subjects.Count))
                                                .ToList());

            return rule.Generate(NumberOfStudents).AsQueryable();
        }

        private class ValueContainer<T>
        {
            public T Value { get; set; }
        }
    }
}
