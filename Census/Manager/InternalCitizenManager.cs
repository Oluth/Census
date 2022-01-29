using Census.Service;
using Census.Service.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace Census.Manager
{
    internal static class InternalCitizenManager
    {
        private static CitizenManager refManager = CitizenManager.instance;

        /// <summary>
        /// Prints a list of all CitizenUnit sizes as a string. For Debug purposes only.
        /// </summary>
        /// <returns>String list of all CitizenUnit sizes.</returns>
        public static string GetInhabitantDebugList()
        {
            StringBuilder strg = new StringBuilder();

            Array32<CitizenUnit> levelUnits = refManager.m_units;

            uint pop = 0;

            foreach (CitizenUnit c in levelUnits.m_buffer)
            {
                if(GetCitizenUnitSize(c) > 0 && IsInhabitant(c)) {
                    uint size = GetCitizenUnitSize(c);
                    pop += size;
                    strg.Append(size + ", ");
                }
            }

            return "Population: " + pop + ", " + strg.ToString();
        }

        /// <summary>
        /// Delivers a list of all Citizen instances that are inhabitant to the loaded level.
        /// </summary>
        /// <returns>Citizen list.</returns>
        public static List<Citizen> GetInhabitantCitizens()
        {
            CitizenUnit[] levelUnits = refManager.m_units.m_buffer;

            uint pop = 0;

            List<Citizen> inhabitantCitizens = new List<Citizen>();

            foreach (CitizenUnit c in levelUnits)
            {
                // CitizenUnit sets are NOT disjoint. This mechanism removes duplicates.
                if (GetCitizenUnitSize(c) > 0 && IsInhabitant(c))
                {
                    uint n = 0;
                    if(c.m_citizen0 > 0 && !inhabitantCitizens.Contains(GetCitizen(c.m_citizen0)))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen0));
                        n++;
                    }
                    if (c.m_citizen1 > 0 && !inhabitantCitizens.Contains(GetCitizen(c.m_citizen1)))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen1));
                        n++;
                    }
                    if (c.m_citizen2 > 0 && !inhabitantCitizens.Contains(GetCitizen(c.m_citizen2)))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen2));
                        n++;
                    }
                    if (c.m_citizen3 > 0 && !inhabitantCitizens.Contains(GetCitizen(c.m_citizen3)))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen3));
                        n++;
                    }
                    if (c.m_citizen4 > 0 && !inhabitantCitizens.Contains(GetCitizen(c.m_citizen4)))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen4));
                        n++;
                    }
                    pop += n;
                }
            }
            DebugService.Log(DebugState.error, pop.ToString());
            return inhabitantCitizens;
        }

        /// <summary>
        /// Returns the number of Citizens in a CitizenUnit object.<br/>Interval between 0 and 5. A value of 0 means that it's a pre-initialized unit that has no occupying Citizens yet.
        /// </summary>
        /// <param name="unit">CitizenUnit object.</param>
        /// <returns></returns>
        private static byte GetCitizenUnitSize(CitizenUnit unit)
        {
            byte size = 0;
            if(unit.m_citizen0 > 0)
            {
                size++;
            }
            if(unit.m_citizen1 > 0)
            {
                size++;
            }
            if(unit.m_citizen2 > 0)
            {
                size++;
            }
            if(unit.m_citizen3 > 0)
            {
                size++;
            }
            if(unit.m_citizen4 > 0)
            {
                size++;
            }

            return size;
        }

        private static Citizen GetCitizen(uint id)
        {
            return refManager.m_citizens.m_buffer[id];
        }

        private static CitizenInstance GetCitizenInstance(Citizen c)
        {
            return refManager.m_instances.m_buffer[c.m_instance];
        }

        public static bool HasFlag(Citizen c, Citizen.Flags flag)
        {
            return (c.m_flags & flag) == flag;
        }

        public static bool HasFlag(CitizenInstance c, CitizenInstance.Flags flag)
        {
            return (c.m_flags & flag) == flag;
        }

        /// <summary>
        /// Evaluates whether a particular Citizen is inhabitant of this level.
        /// <br/>
        /// Inhabitance is defined as 'having the Created flag AND not having the DummyTraffic flag AND not being a tourist AND not moving in'.
        /// </summary>
        /// <param name="c">Citizen object.</param>
        /// <returns>Inhabitance of this Citizen.</returns>
        public static bool IsInhabitant(Citizen c)
        {
            bool isDummyTraffic = HasFlag(c, Citizen.Flags.DummyTraffic);
            bool isCreated = HasFlag(c, Citizen.Flags.Created);
            bool isTourist = HasFlag(c, Citizen.Flags.Tourist);
            bool isMovingIn = HasFlag(c, Citizen.Flags.MovingIn);
            bool isInhabitant = !isDummyTraffic && !isTourist && isCreated && !isMovingIn;
            DebugService.Log(DebugState.info, isInhabitant.ToString());
            return isInhabitant;
        }

        /// <summary>
        /// Evaluates whether at least one member of this CitizenUnit is inhabitant of this level.
        /// </summary>
        /// <param name="cu">CitizenUnit object.</param>
        /// <returns>Inhabitance of this CitizenUnit object.</returns>
        public static bool IsInhabitant(CitizenUnit cu)
        {
            if(GetCitizenUnitSize(cu) == 0)
            {
                return false;
            } else
            {
                bool c1 = IsInhabitant(GetCitizen(cu.m_citizen0));
                bool c2 = IsInhabitant(GetCitizen(cu.m_citizen1));
                bool c3 = IsInhabitant(GetCitizen(cu.m_citizen2));
                bool c4 = IsInhabitant(GetCitizen(cu.m_citizen3));
                bool c5 = IsInhabitant(GetCitizen(cu.m_citizen4));

                if (c1)
                {
                    DebugService.Log(DebugState.warning, "ID von 'true':" + cu.m_citizen0);
                }
                if (c2)
                {
                    DebugService.Log(DebugState.warning, "ID von 'true':" + cu.m_citizen1);
                }
                if (c3)
                {
                    DebugService.Log(DebugState.warning, "ID von 'true':" + cu.m_citizen2);
                }
                if (c4)
                {
                    DebugService.Log(DebugState.warning, "ID von 'true':" + cu.m_citizen3);
                }
                if (c5)
                {
                    DebugService.Log(DebugState.warning, "ID von 'true':" + cu.m_citizen4);
                }

                return c1 || c2 || c3 || c4 || c5;
            }
        }

    }
}
