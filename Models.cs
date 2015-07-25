using System;

namespace DataFactory
{
    public class Person
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string FullName { get { return string.Format("{0} {1} {2}", Title, FirstName, LastName); } }
    }

    public class Contact : Person
    {
        public string Organisation { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
    }

    public class Vehicle
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public DateTime Registered { get; set; }
        public int Mileage { get; set; }
    }
}
