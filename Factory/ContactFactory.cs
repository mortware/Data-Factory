using System;
using System.Text.RegularExpressions;

namespace DataFactory
{
    public class ContactFactory : DataFactoryBase
    {
        /// <summary>
        /// Returns a randomly generated <see cref="DataFactory.Contact"/> based in the UK, with full contact information
        /// </summary>
        /// <returns></returns>
        public static Contact Contact()
        {
            var gender = RandomGender;

            var contact = new Contact
            {
                Title = NameFactory.Title(gender),
                FirstName = NameFactory.FirstName(gender),
                LastName = NameFactory.LastName(),
                Organisation = NameFactory.Organisation(),
                IsMale = gender == Gender.Male,
                // 23725 = 65 years
                DateOfBirth = DateTimeGenerator.Date(DateTime.Now.Date.AddDays(-23725), range: TimeSpan.FromDays(23725), roundMinutes: true)
            };

            var email = string.Format("{0}@{1}.{2}", contact.FirstName, contact.Organisation, Properties.Resources.EmailDomains.Split(',').GetRandom()).Trim().ToLower();

            contact.Email = Regex.Replace(email, @"[^\w0-9-_]", "");

            contact.Phone = Phone();
            contact.Mobile = Phone(true);

            var townAndCounty = AddressFactory.LocalityAndRegion();
            contact.Street = AddressFactory.FirstLine();
            contact.Town = townAndCounty.Key;
            contact.County = townAndCounty.Value;
            contact.Postcode = AddressFactory.Postcode();

            return contact;
        }

        /// <summary>
        /// Returns a randomly generated (UK-based) phone number
        /// </summary>
        /// <param name="mobile">Specify whether it should be a mobile number</param>
        /// <param name="internationalise">Specify if the number should include the internationalised dialing prefix (+44)</param>
        /// <returns></returns>
        public static string Phone(bool mobile = false, bool internationalise = false)
        {
            string[] landlinePrefix = { "020", "023", "024", "028", "029" };

            string prefix;

            // xxxx
            if (mobile)
                prefix = string.Format("07{0:D2}", Rand.Next(100));
            else
                prefix = landlinePrefix.GetRandom() + Rand.Next(10);

            prefix += string.Format("{0:D7}", Rand.Next(10000000));
            // xxxxxxxxxxx

            prefix = prefix.Insert(mobile ? 8 : 7, " "); // Insert the second space (i.e. XXXXXXXX_XXX)
            prefix = prefix.Insert(mobile ? 5 : 4, " "); // Insert the first space

            prefix = prefix.Remove(0, 1); // Remove the first zero
            prefix = prefix.Insert(0, internationalise ? "+44 " : "0"); // Add zero or +44 to the beginning


            return prefix;
        }

        /// <summary>
        /// Returns a randomly generated email address with fictional organisation domain
        /// </summary>
        /// <returns></returns>
        public static string Email()
        {
            var domain = NameFactory.Organisation();
            domain = Regex.Replace(domain, @"[^\w0-9-_]", "");

            var email = string.Format("{0}@{1}.{2}",
                NameFactory.FirstName(),
                domain,
                Properties.Resources.EmailDomains.Split(',').GetRandom())
                .Trim().ToLower();

            return email;
        }
    }
}
