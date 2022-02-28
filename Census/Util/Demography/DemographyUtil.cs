using Census.Manager;
using Census.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Census.Util.Demography
{
    internal static class DemographyUtil
    {
        public enum BreakdownMode
        {
            Inhabitant
        }

        public static void PrintAgeBreakdown()
        {
            int[] ageNumbers = GetAgeBreakdown(BreakdownMode.Inhabitant);

            List<string> output = new List<string>();

            output.Add("Average age: " + GetAverageAge(ageNumbers) + " years.");
            output.Add("");

            string[] ages = new string[ageNumbers.Length];

            for(int i = 0; i < ageNumbers.Length; i++)
            {
                ages[i] = "Age " + i + ": " + ageNumbers[i].ToString() + " people.";
            }

            Array.ForEach(ages, s => output.Add(s));

            
            IOService.Instance.WriteInFile(output.ToArray<string>(), "ageBreakdown.txt");
        }

        public static int[] GetAgeBreakdown(BreakdownMode mode)
        {
            int[] ageNumbers = new int[(Citizen.AGE_LIMIT_FINAL + 100) / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE];
            switch (mode)
            {
                default:
                    List<Citizen> citizens = InternalCitizenManager.GetInhabitantCitizens();
                    foreach (Citizen c in citizens)
                    {
                        ageNumbers[c.Age / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE]++;
                    }
                    break;
            }
            return ageNumbers;
        }

        public static double GetAverageAge(int[] quantities)
        {
            int total = 0;

            if(quantities == null)
            {
                throw new ArgumentNullException("Age array is empty!");
            }
            double avgAge = 0;
            for (int i = 0; i < quantities.Length; i++)
            {
                avgAge += i * quantities[i];
                total += quantities[i];
            }
            avgAge /= total;
            return avgAge;
        }
    }
}
