using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFactory
{
    public class NameFactory : DataFactoryBase
    {
        readonly static char[] Consonants = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        readonly static char[] Vowels = { 'A', 'E', 'I', 'O', 'U' };

        /// <summary>
        /// Returns a random title/salutation with optional gender
        /// </summary>
        /// <param name="gender">The gender of the title</param>
        /// <returns></returns>
        public static string Title(Gender gender = Gender.Either)
        {
            switch (gender)
            {
                case Gender.Male:
                    return Properties.Resources.TitlesMale.Split(',').GetRandom();
                case Gender.Female:
                    return Properties.Resources.TitlesFemale.Split(',').GetRandom();
                default:
                    return Properties.Resources.TitlesMale.Split(',').Concat(Properties.Resources.TitlesFemale.Split(',')).GetRandom();
            }
        }

        /// <summary>
        /// Returns a random first name with optional gender
        /// </summary>
        /// <param name="gender">The gender of the random name to generate</param>
        /// <returns></returns>
        public static string FirstName(Gender gender = Gender.Either)
        {
            switch (gender)
            {
                case Gender.Male:
                    return Properties.Resources.NamesMale.Split(',').GetRandom();
                case Gender.Female:
                    return Properties.Resources.NamesFemale.Split(',').GetRandom();
                default:
                    return Properties.Resources.NamesMale.Split(',').Concat(Properties.Resources.NamesFemale.Split(',')).GetRandom();
            }
        }

        /// <summary>
        /// Returns a random last name
        /// </summary>
        /// <returns></returns>
        public static string LastName()
        {
            return Properties.Resources.LastNames.Split(',').GetRandom();
        }

        /// <summary>
        /// Returns a random (first and last) name
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string Name(Gender gender = Gender.Either)
        {
            return String.Format("{0} {1}", FirstName(gender), LastName());
        }

        /// <summary>
        /// Returns a random full (title, first and last) name
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string FullName(Gender gender = Gender.Either)
        {
            return String.Format("{0} {1} {2}", Title(gender), FirstName(gender), LastName());
        }

        /// <summary>
        /// Returns a randomly generated organisation/company name
        /// </summary>
        /// <returns></returns>
        public static string Organisation()
        {
            var funcs = new List<Func<string>>
            {
                () => string.Format("{0}{1}{2} {3}", 
                    Consonants.GetRandom(), 
                    Consonants.GetRandom(), 
                    Consonants.GetRandom(),
                    Properties.Resources.OrganisationCompanySuffixes.Split(',').GetRandom()),
                () =>
                    string.Format("{0}{1} {2}", 
                    Consonants.GetRandom(), 
                    Consonants.GetRandom(),
                    Properties.Resources.OrganisationNameDescriptions.Split(',').GetRandom()),
                () =>
                    string.Format("{0}{1} {2}", 
                    Consonants.GetRandom(), 
                    Consonants.GetRandom(),
                    Properties.Resources.OrganisationCompanySuffixes.Split(',').GetRandom()),
                () =>
                    string.Format("{0} & {1}",
                    Properties.Resources.LastNames.Split(',').GetRandom(),
                    Properties.Resources.LastNames.Split(',').GetRandom()),
                () =>
                    string.Format("{0} & {1} {2}",
                    Properties.Resources.LastNames.Split(',').GetRandom(),
                    Properties.Resources.LastNames.Split(',').GetRandom(),
                    Properties.Resources.OrganisationNameDescriptions.Split(',').GetRandom()),
                () =>
                    string.Format("{0} & {1}",
                    Properties.Resources.LastNames.Split(',').GetRandom(),
                    StringFactory.PronounceableWord(5)),
                () =>
                    string.Format("{0} {1}",
                    StringFactory.PronounceableWord(5),
                    Properties.Resources.OrganisationNameDescriptions.Split(',').GetRandom()),
                () =>
                    string.Format("{0} {1} {2}",
                    Properties.Resources.LastNames.Split(',').GetRandom(),
                    Properties.Resources.OrganisationNameDescriptions.Split(',').GetRandom(),
                    Properties.Resources.OrganisationCompanySuffixes.Split(',').GetRandom())
            };

            return funcs.GetRandom()();
        }

        /// <summary>
        /// Returns a random corporate Job Title
        /// </summary>
        /// <returns></returns>
        public static string JobTitle()
        {
            return Properties.Resources.JobTitles.Split(',').GetRandom();
        }
    }
}
