using System;

namespace DataFactory
{
    public class VehicleFactory : DataFactoryBase
    {
        private static readonly string[] MakeModels = { "Honda Civic", "Ford Focus", "Vauxhall Zafira", "Peugeot 206", "Peugeot 4008", "Toyota Auris", "Nissan Qashqai", "Vauxhall Astra", "Mini Cooper", "Lotus Elise", "Range Rover Evoque", "Jaguar XF", "Land Rover", "Jaguar F-Type", "Aston Martin V8 Vantage", "Rolls Royce Phantom", "BMW 120D", "Citroen DS3", "Renault Scenic", "Audi A6", "Citroen Zsara Picasso", "Ford Galaxy", "Renault Megane", "Ford Fiesta", "Suzuki Swift" };
        private static readonly string[] Manufacturers = { "Ford", "Vauxhall", "Mini", "BMW", "Audi", "Renault", "Peugoet", "Toyota", "Lotus", "Citroen", "Suzuki", "Nissan" };

        private static readonly char[] RegistrationCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'V', 'W', 'X', 'Y' };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxAge">Maximum age of vehicle (in years)</param>
        /// <returns></returns>
        public static Vehicle Vehicle(int maxAge = 10)
        {
            var mm = MakeModels.GetRandom().Split(' ');
            var registered = DateTime.Now.AddMinutes(-Rand.Next(maxAge * 365 * 24 * 60));
            var registration = Registration(maxAge, registered);

            // Mileage - based on average mileage per UK car between 2002 and 2013
            var annual = Rand.Next(7900, 9200);
            var mileage = Convert.ToInt32((DateTime.Now - registered).TotalDays * (annual / 365.0));

            var v = new Vehicle
            {
                Manufacturer = mm[0],
                Model = mm[1],
                Registration = registration,
                Registered = registered,
                Mileage = mileage
            };
            return v;
        }

        public static string Registration(int maxAge = 15, DateTime? registered = null)
        {
            var regDate = registered.HasValue
                ? registered.Value
                : DateTime.Now.AddMinutes(Rand.Next(-(maxAge * 365 * 24 * 60)));

            var reg = string.Format("{0}{1}", RegistrationCharacters.GetRandom(), RegistrationCharacters.GetRandom());
            var year = regDate.Year - 2000;

            if (RandomBoolean) year += 50;

            reg += year.ToString("D2");
            reg += " ";
            reg += string.Format("{0}{1}{2}", RegistrationCharacters.GetRandom(), RegistrationCharacters.GetRandom(), RegistrationCharacters.GetRandom());
            return reg;
        }
    }
}
