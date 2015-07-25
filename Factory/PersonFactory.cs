using System;

namespace DataFactory
{
    public class PersonFactory : DataFactoryBase
    {
        /// <summary>
        /// Returns a randomly generated <see cref="DataFactory.Person"/> with an English name
        /// </summary>
        /// <returns></returns>
        public static Person Person()
        {
            var gender = RandomGender;

            var person = new Person
            {
                Title = NameFactory.Title(gender),
                FirstName = NameFactory.FirstName(gender),
                LastName = NameFactory.LastName(),
                IsMale = gender == Gender.Male,
                // 23725 = 65 years
                DateOfBirth = DateTimeGenerator.Date(DateTime.Now.Date.AddDays(-23725), range: TimeSpan.FromDays(23725), roundMinutes: true)
            };
            return person;
        }
    }
}
