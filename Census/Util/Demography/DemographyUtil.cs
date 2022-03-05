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
        public enum AgeBreakdownMode
        {
            Inhabitant_All,
            Inhabitant_Male,
            Inhabitant_Female
        }

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

            IOService.Instance.WriteInFile(output.ToArray<string>(), "ageList.csv");

        }
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

            
            IOService.Instance.WriteInFile(output.ToArray<string>(), "ageBreakdown.txt");
        }

        public static int[] GetAgeBreakdown(AgeBreakdownMode mode)
        {
            int[] ageNumbers = new int[(Citizen.AGE_LIMIT_FINAL + 100) / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE];
            List<Citizen> citizens = InternalCitizenManager.GetInhabitantCitizens();
            DebugService.Log(Service.Debug.DebugState.warning, "Got inhabitant citizens.");

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

            foreach (Citizen c in citizens)
            {
                ageNumbers[c.Age / InternalCitizenManager.REAL_AGEYEARS_PER_INGAME_AGE]++;
            }
            return ageNumbers;
        }

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
            } catch(Exception e) { }

            return total;
        }

    }
}
