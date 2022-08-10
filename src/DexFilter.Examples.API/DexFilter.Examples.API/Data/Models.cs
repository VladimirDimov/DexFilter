namespace DexFilter.Examples.API.Data
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Grade { get; set; }
        public DateTime BirthDate { get; set; }
        public Address Address { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
    }

    public class Address
    {
        public City City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }
    }

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
