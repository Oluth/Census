using Census.Manager;
using Census.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Census.Util.Demography
{

    /// <summary>
    /// Provide the mod's demography business logic based on its managers.
    /// </summary>
    internal static class DemographyUtil
    {
        public enum AgeBreakdownMode
        {
            Inhabitant_All,
            Inhabitant_Male,
            Inhabitant_Female
        }

        /// <summary>
        /// Fetches the recent citizen data and writes the age split into a comma-separated value (.csv) file.
        /// </summary>
        public static void PrintCSVAgeBreakdown()
        {
            List<string> output = new List<string>();

            int[] maleAge = GetAgeBreakdown(AgeBreakdownMode.Inhabitant_Male);
            int[] femaleAge = GetAgeBreakdown(AgeBreakdownMode.Inhabitant_Female);

            output.Add("Age,Male,Female");

            for(int i = 0; i < maleAge.Length; i++)
            {
                output.Add(i.ToString() + "," + maleAge[i].ToString() + "," + femaleAge[i].ToString());
            }

            IOService.Instance.WriteFile(output.ToArray<string>(), "ageList.csv");

        }

        /// <summary>
        /// Fetches the recent citizen data and writes a verbose file with an age split into a comma-separated value (.csv) file.
        /// </summary>
        public static void PrintAgeBreakdown()
        {
            int[] maleAge = GetAgeBreakdown(AgeBreakdownMode.Inhabitant_Male);
            int[] femaleAge = GetAgeBreakdown(AgeBreakdownMode.Inhabitant_Female);
            int[] ageNumbers = GetAgeBreakdown(AgeBreakdownMode.Inhabitant_All);

            int totalMale = GetTotalPopulation(maleAge);
            int totalFemale = GetTotalPopulation(femaleAge);
            int total = totalMale + totalFemale;

            float malePerc = (float) totalMale / (float) total * 100;

            List<string> output = new List<string>();

            output.Add("Total population: " + GetTotalPopulation(ageNumbers) + " (100 %)");
            output.Add("Male population: " + GetTotalPopulation(maleAge) + " (" + Math.Round(malePerc, 2) + " %)");
            output.Add("Female population: " + GetTotalPopulation(femaleAge) + " (" + Math.Round((100-malePerc), 2) + " %)");
            output.Add("");
            output.Add("Mean age: " + Math.Round(GetMeanAge(ageNumbers), 2) + " years.");
            output.Add("Median age: " + GetMedianAge(ageNumbers) + " years.");
            output.Add("");

            string[] ages = new string[ageNumbers.Length];

            for(int i = 0; i < ageNumbers.Length; i++)
            {
                ages[i] = "Age " + i + ": " + ageNumbers[i].ToString() + " people.";
            }

            Array.ForEach(ages, s => output.Add(s));

            
            IOService.Instance.WriteFile(output.ToArray<string>(), "ageBreakdown.txt");
        }

        /// <summary>
        /// Fetches the citizen data and returns an integer array of all age cohorts.
        /// </summary>
        /// <param name="mode">Specification of result.</param>
        /// <returns></returns>
        public static int[] GetAgeBreakdown(AgeBreakdownMode mode)
        {
            int[] ageNumbers = new int[(int) Math.Ceiling(InternalCitizenManager.MAX_CITIZEN_AGE / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE)];
            List<Citizen> citizens = InternalCitizenManager.Instance.GetInhabitantCitizens();
            DebugService.Log(Service.Debug.DebugState.fine, "Got inhabitant citizens.");

            // Filter citizens by defined breakdown mode
            // c.GetCitizenInfo only requires a value if the Citizen struct's m_instance field is unset.
            // This is not the case with Citizens that have already been initialized.
            switch (mode)
            {
                case AgeBreakdownMode.Inhabitant_Male:   
                    citizens = citizens.FindAll(c => c.GetCitizenInfo(0).m_gender == Citizen.Gender.Male);
                    break;
                case AgeBreakdownMode.Inhabitant_Female:
                    citizens = citizens.FindAll(c => c.GetCitizenInfo(0).m_gender == Citizen.Gender.Female);
                    break;
                case AgeBreakdownMode.Inhabitant_All:
                default:
                break;
            }
            DebugService.Log(Service.Debug.DebugState.fine, "Filtered citizens by " + mode.ToString() + ".");

            // Add age of the citizen into array.
            foreach (Citizen c in citizens)
            {
                ageNumbers[(int) Math.Floor(c.Age / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE)]++;
            }
            return ageNumbers;
        }

        /// <summary>
        /// Calculates the average (mean) age of an age array.
        /// </summary>
        /// <param name="quantities">Age array.</param>
        /// <returns>Statistically mean age.</returns>
        /// <exception cref="ArgumentNullException">Please avoid null-pointers. Thank you.</exception>
        public static double GetMeanAge(int[] quantities)
        {
            if (quantities == null)
            {
                throw new ArgumentNullException("Age array is empty!");
            }
            int total = GetTotalPopulation(quantities);
            double avgAge = 0;
            for (int i = 0; i < quantities.Length; i++)
            {
                avgAge += i * quantities[i];
            }
            avgAge /= total;
            return avgAge;
        }

        /// <summary>
        /// Calculates the median age of an age array.
        /// </summary>
        /// <param name="quantities">Age array.</param>
        /// <returns>Statistically median age.</returns>
        /// <exception cref="ArgumentNullException">No null-pointers.</exception>
        /// <exception cref="InvalidOperationException">No arrays with zero length.</exception>
        public static double GetMedianAge(int[] quantities)
        {
            if (quantities == null)
            {
                throw new ArgumentNullException("Age array is empty!");
            }
            int total = GetTotalPopulation(quantities);
            double medianAge = 0;
            int totalCounter = 0;
            int i = 0;

            // shouldn't happen in-game
            if(quantities.Length == 0)
            {
                throw new InvalidOperationException("Invalid age array length.");
            } else
            {
                while (totalCounter + quantities[i] < total / 2)
                {
                    totalCounter += quantities[i];
                    i++;
                }
                if (totalCounter * 2 == quantities.Length)
                {
                    medianAge = i + 0.5;
                } else
                {
                    medianAge = i;
                }
            }
            return medianAge;
        }

        public static int GetMaxQuantity(int[] array)
        {
            int output = 0;
            foreach (int item in array)
            {
                output = item > output ? item : output;
            }
            return output;
        }

        private static int GetTotalPopulation(int[] quantities)
        {
            int total = 0;

            try
            {
                foreach (int quant in quantities)
                {
                    total += quant;
                }
            } catch(Exception e) { 
            
            }

            return total;
        }

    }
}
