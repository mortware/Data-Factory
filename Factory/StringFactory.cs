using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DataFactory
{
    public class StringFactory : DataFactoryBase
    {
        readonly static char[] Consonants = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
        readonly static char[] Vowels = { 'A', 'E', 'I', 'O', 'U' };

        /// <summary>
        /// Gets the Lorem Ipsum dummy text.
        /// </summary>
        public static string LoremText
        {
            get
            {
                return "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            }
        }

        /// <summary>
        /// Returns a randomly generated alphabetic string.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string String(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Rand.NextDouble() + 65)));
                sb.Append(ch);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a randomly generated alphanumeric password.
        /// </summary>
        /// <param name="length">The length of the password to generate.</param>
        /// <returns></returns>
        public static string Password(int length)
        {
            // TODO: Add option to create very secure password

            var sb = new StringBuilder();
            bool useVowel = false;
            for (int i = 0; i < length; i++)
            {
                if (i < 4)
                {
                    if (!useVowel)
                    {
                        sb.Append(i == 0
                                      ? Consonants[Rand.Next(Consonants.Length - 1)].ToString(CultureInfo.InvariantCulture).ToUpper()
                                      : Consonants[Rand.Next(Consonants.Length - 1)].ToString(CultureInfo.InvariantCulture).ToLower());
                        useVowel = true;
                    }
                    else
                    {
                        sb.Append(Vowels[Rand.Next(Vowels.Length - 1)].ToString(CultureInfo.InvariantCulture).ToLower());
                        useVowel = false;
                    }
                }
                else
                    sb.Append(Rand.Next(0, 9));
            var word = PronounceableWord(length - 2);
            word += Rand.Next(0, 9).ToString("D2");
            return word;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a randomly generated readable password. 
        /// </summary>
        /// <param name="length">Length if format is not specified. Defaults to 50% alpha, 50% numeric.</param>
        /// <param name="format">Defines where alpha and numeric characters should be placed within a string (A for alpha, 9 for numeric i.e. AAA99 = PWD12). Overrides the length parameter.</param>
        /// <param name="safeMode">Omits potentially confusing characters i.e. 'B'/'8', or 'I'/'1'</param>
        /// <param name="forceUpper">Converts password to Uppercase</param>
        /// <param name="forceLower">Converts password to Lowercase</param>
        /// <returns></returns>
        public static string Password(int length = 7, string format = "", bool safeMode = false, bool forceUpper = false, bool forceLower = false)
        {
            // Assert format contains only A's or 9's
            if (!string.IsNullOrEmpty(format))
            {
                if (format.Any())
                {

                }
            }

            // Assert only either forceUpper OR forceLower can be specified
            if (forceUpper && forceLower)
                throw new ArgumentException("Only one parameter can be true for 'forceUpper' OR 'forceLower'");

            char[] consonants = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' };
            char[] vowels = { 'A', 'E', 'I', 'O', 'U' };
            char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            char[] safeConsonants = { 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'T', 'V', 'W', 'X', 'Y', 'Z' };
            char[] safeVowels = { 'A', 'E' };
            char[] safeNumbers = { '2', '3', '4', '5', '7', '9' };

            var sb = new StringBuilder();

            // Determine password format
            char[] passwordFormat;
            if (!string.IsNullOrEmpty(format))
                passwordFormat = format.ToArray();
            else
            {
                var formatChars = new List<char>();
                for (int i = 0; i < length; i++)
                {
                    formatChars.Add(i > length / 2 ? '9' : 'A');
                }
                passwordFormat = formatChars.ToArray();
            }


            var cons = safeMode ? safeConsonants : consonants;  // Consonants to use
            var vows = safeMode ? safeVowels : vowels;          // Vowels to use
            var nums = safeMode ? safeNumbers : numbers;        // Numbers to use

            var useVowel = false;
            foreach (var c in passwordFormat)
            {
                switch (c)
                {
                    case 'A':
                        // Create random alpha character
                        var s = useVowel
                            ? vows[Rand.Next(vows.Length - 1)].ToString(CultureInfo.InvariantCulture)
                            : cons[Rand.Next(cons.Length - 1)].ToString(CultureInfo.InvariantCulture);
                        useVowel = !useVowel;

                        // Randomly upper/lower the character
                        if (!forceUpper && !forceLower)
                            s = Rand.NextDouble() > 0.5 ? s.ToUpper() : s.ToLower();

                        sb.Append(s);
                        break;
                    case '9':
                        sb.Append(nums[Rand.Next(nums.Length - 1)].ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }

            var password = sb.ToString();

            // Force casing if specified
            if (forceUpper)
                password = password.ToUpper();
            if (forceLower)
                password = password.ToLower();

            return password;
        }

        /// <summary>
        /// Returns randomly generated 'Lorem Ipsum' text.
        /// </summary>
        /// <param name="minWords">The minimum number of words per sentence.</param>
        /// <param name="maxWords">The maximum number of words per sentence.</param>
        /// <param name="minSentences">The minimum number of sentences.</param>
        /// <param name="maxSentences">The maximum number of sentences.</param>
        /// <param name="numParagraphs">The total number of paragraphs.</param>
        /// <returns></returns>
        public static string Lorem(int minWords = 10, int maxWords = 15, int minSentences = 1, int maxSentences = 5, int numParagraphs = 1)
        {
            int numSentences = Rand.Next(maxSentences - minSentences) + minSentences + 1;
            int numWords = Rand.Next(maxWords - minWords) + minWords + 1;

            string result = string.Empty;

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        string word = Data.LoremWords[Rand.Next(Data.LoremWords.Length)];
                        if (w == 0)
                            word = word.ToProper();
                        else if (w > 0 && w < numWords - 1)
                            word += " ";

                        result += word;
                    }
                    result += ". ";
                }
                result += Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// Returns a randomly generated word that is formed so that it can be 
        /// pronounced - Consonant, vowel, consonant, vowel etc. (i.e. Daku, Luguf)
        /// </summary>
        /// <param name="length">Length of the word to generate</param>
        /// <param name="vowelFirst">Word starts with a vowel, defaults to consonant</param>
        /// <returns></returns>
        public static string PronounceableWord(int length = 4, bool vowelFirst = false)
        {
            var sb = new StringBuilder();
            bool useVowel = vowelFirst;
            for (int i = 0; i < length; i++)
            {
                if (!useVowel)
                {
                    sb.Append(i == 0
                                  ? Consonants[Rand.Next(Consonants.Length - 1)].ToString(CultureInfo.InvariantCulture).ToUpper()
                                  : Consonants[Rand.Next(Consonants.Length - 1)].ToString(CultureInfo.InvariantCulture).ToLower());
                    useVowel = true;
                }
                else
                {
                    sb.Append(Vowels[Rand.Next(Vowels.Length - 1)].ToString(CultureInfo.InvariantCulture).ToLower());
                    useVowel = false;
                }
            }

            return sb.ToString();
        }
    }
}
