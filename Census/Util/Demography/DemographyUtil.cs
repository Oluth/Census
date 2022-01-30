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
            for(int i = 0; i < ageNumbers.Length; i++)
            {
                DebugService.Log(Service.Debug.DebugState.error, "Age " + i + ": " + ageNumbers[i].ToString() + " people.");
            }
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
    }
}
