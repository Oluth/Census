using Census.Service;
using Census.Service.Debug;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Census.Manager
{
    /// <summary>
    /// Acquires all Citizen data from Cities: Skylines and processes values for mod use. 
    /// <br/>
    /// <br/>
    /// It is preferred to be used instead of in-game managers due to SoC. 
    /// </summary>
    internal class InternalCitizenManager
    {

        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static InternalCitizenManager instance;
        public static InternalCitizenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    DebugService.Log(DebugState.fine, "Create CitizenManager instance.");
                    new InternalCitizenManager();
                }

                DebugService.Log(DebugState.fine, "Return instance of CitizenManager.");
                return instance;
            }
        }

        private InternalCitizenManager() {
            instance = this;
        }

        private CitizenManager refManager = CitizenManager.instance;

        // MAX_CITIZEN_AGE since C:S 1.14. The constant in Citizen is false since then.

        public const int MAX_CITIZEN_AGE = 400;
        public const float REAL_AGEYEARS_PER_INGAME_AGE = 3.5f;

        /// <summary>
        /// Prints a list of all CitizenUnit sizes as a string. For Debug purposes only.
        /// </summary>
        /// <returns>String list of all CitizenUnit sizes.</returns>
        public string GetInhabitantDebugList()
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
        public List<Citizen> GetInhabitantCitizens()
        {
            CitizenUnit[] levelUnits = refManager.m_units.m_buffer;

            uint pop = 0;

            List<Citizen> inhabitantCitizens = new List<Citizen>();
            HashSet<uint> citizenIDs = new HashSet<uint>();

            foreach (CitizenUnit c in levelUnits)
            {
                // CitizenUnit sets are NOT disjoint. This mechanism removes duplicates.
                if (GetCitizenUnitSize(c) > 0 && IsInhabitant(c))
                {
                    uint n = 0;
                    if(c.m_citizen0 > 0 && !citizenIDs.Contains(c.m_citizen0))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen0));
                        citizenIDs.Add(c.m_citizen0);
                        n++;
                    }
                    if (c.m_citizen1 > 0 && !citizenIDs.Contains(c.m_citizen1))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen1));
                        citizenIDs.Add(c.m_citizen1);
                        n++;
                    }
                    if (c.m_citizen2 > 0 && !citizenIDs.Contains(c.m_citizen2))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen2));
                        citizenIDs.Add(c.m_citizen2);
                        n++;
                    }
                    if (c.m_citizen3 > 0 && !citizenIDs.Contains(c.m_citizen3)) 
                    { 
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen3));
                        citizenIDs.Add(c.m_citizen3);
                        n++;
                    }
                    if (c.m_citizen4 > 0 && !citizenIDs.Contains(c.m_citizen4))
                    {
                        inhabitantCitizens.Add(GetCitizen(c.m_citizen4));
                        citizenIDs.Add(c.m_citizen4);
                        n++;
                    }
                    pop += n;
                }
            }

            DebugService.Log(DebugState.warning, "Inhabitant citizens: " + pop);
            return inhabitantCitizens;
        }

        /// <summary>
        /// Returns the number of Citizens in a CitizenUnit object.<br/>Interval between 0 and 5. A value of 0 means that it's a pre-initialized unit that has no occupying Citizens yet.
        /// </summary>
        /// <param name="unit">CitizenUnit object.</param>
        /// <returns></returns>
        private byte GetCitizenUnitSize(CitizenUnit unit)
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

        private Citizen GetCitizen(uint id)
        {
            return refManager.m_citizens.m_buffer[id];
        }

        private CitizenInstance GetCitizenInstance(Citizen c)
        {
            return refManager.m_instances.m_buffer[c.m_instance];
        }

        public bool HasFlag(Citizen c, Citizen.Flags flag)
        {
            return (c.m_flags & flag) == flag;
        }

        public bool HasFlag(CitizenInstance c, CitizenInstance.Flags flag)
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
        public bool IsInhabitant(Citizen c)
        {
            bool isDummyTraffic = HasFlag(c, Citizen.Flags.DummyTraffic);
            bool isCreated = HasFlag(c, Citizen.Flags.Created);
            bool isTourist = HasFlag(c, Citizen.Flags.Tourist);
            bool isMovingIn = HasFlag(c, Citizen.Flags.MovingIn);
            bool isDead = HasFlag(c, Citizen.Flags.Dead);
            bool isInhabitant = !isDummyTraffic && !isTourist && isCreated && !isMovingIn && !isDead;
            return isInhabitant;
        }

        /// <summary>
        /// Evaluates whether at least one member of this CitizenUnit is inhabitant of this level.
        /// </summary>
        /// <param name="cu">CitizenUnit object.</param>
        /// <returns>Inhabitance of this CitizenUnit object.</returns>
        public bool IsInhabitant(CitizenUnit cu)
        {
            if(GetCitizenUnitSize(cu) == 0)
            {
                return false;
            } else
            {
                bool rs = (cu.m_flags & CitizenUnit.Flags.Home) != 0;
                bool c1 = IsInhabitant(GetCitizen(cu.m_citizen0));
                bool c2 = IsInhabitant(GetCitizen(cu.m_citizen1));
                bool c3 = IsInhabitant(GetCitizen(cu.m_citizen2));
                bool c4 = IsInhabitant(GetCitizen(cu.m_citizen3));
                bool c5 = IsInhabitant(GetCitizen(cu.m_citizen4));

                return rs && (c1 || c2 || c3 || c4 || c5);
            }
        }

        public Color32 GetColorByAge(uint age)
        {
            return GetCitizenAgeGroupColor(GetAgeGroupByAge(age));
        }

        public Citizen.AgeGroup GetAgeGroupByAge(uint age)
        {
            Citizen.AgeGroup output;

            if (age <= (double) Citizen.AGE_LIMIT_CHILD / (double) REAL_AGEYEARS_PER_INGAME_AGE)
            {
                output = Citizen.AgeGroup.Child;
            } else if (age <= (double)Citizen.AGE_LIMIT_TEEN / (double)REAL_AGEYEARS_PER_INGAME_AGE)
            {
                output = Citizen.AgeGroup.Teen;
            } else if (age <= (double)Citizen.AGE_LIMIT_YOUNG / (double)REAL_AGEYEARS_PER_INGAME_AGE)
            {
                output = Citizen.AgeGroup.Young;
            } else if (age <= (double)Citizen.AGE_LIMIT_ADULT / (double)REAL_AGEYEARS_PER_INGAME_AGE)
            {
                output = Citizen.AgeGroup.Adult;
            } else
            {
                output = Citizen.AgeGroup.Senior;
            }

            return output;
        }

        public Color32 GetCitizenAgeGroupColor(Citizen.AgeGroup ageGroup)
        {
            Color32 output;

            switch (ageGroup)
            {
                case Citizen.AgeGroup.Child:
                    output = Color.yellow;
                    break;

                case Citizen.AgeGroup.Teen:
                    output = Color.green;
                    break;

                case Citizen.AgeGroup.Young:
                default:
                    output = Color.white;
                    break;

                case Citizen.AgeGroup.Adult:
                    output = Color.red;
                    break;

                case Citizen.AgeGroup.Senior:
                    output = Color.blue;
                    break;
            }

            return output;
        }



    }
}
