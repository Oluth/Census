using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Census.Service
{
    internal static class VersionService
    {
        private static Version censusVersion;

        public static Version CensusVersion
        {
            get
            {
                if (censusVersion.Equals(new Version()))
                {
                    censusVersion = new Version();

                    // SET RECENT VERSION HERE!
                    censusVersion.major = 0;
                    censusVersion.minor = 0;
                    censusVersion.patch = 0;

                    censusVersion.isBeta = false;
                    censusVersion.betaIncrement = 0;

                    censusVersion.isAlpha = true;
                    censusVersion.alphaIncrement = 3;

                    censusVersion.isHotfix = false;
                    censusVersion.hotfixIncrement = 0;

                    // Set metadata to null if none is given.
                    censusVersion.metadata = null;
                }

                return censusVersion;
            }
        }
        /// <summary>
        /// Semantic versioning according to <a href="https://semver.org/"https://semver.org/</a>.
        /// </summary>
        public struct Version : IEquatable<Version>
        {
            public bool isAlpha;
            public bool isBeta;
            public bool isHotfix;
            public int alphaIncrement;
            public int betaIncrement;
            public int hotfixIncrement;

            public int major;
            public int minor;
            public int patch;

            public string metadata;

            public bool Equals(Version other)
            {
                bool f1 = alphaIncrement == other.alphaIncrement && betaIncrement == other.betaIncrement;
                bool f2 = isAlpha == other.isAlpha && isBeta == other.isBeta && isHotfix == other.isHotfix;
                bool f3 = major == other.major && minor == other.minor && patch == other.patch;

                return f3 && f2 && f1;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(major).Append(".").Append(minor).Append(".").Append(patch);

                if (isBeta)
                {
                    sb.Append("-beta.").Append(betaIncrement);
                }
                if (isAlpha)
                {
                    sb.Append("-alpha.").Append(alphaIncrement);
                }
                if (isHotfix)
                {
                    sb.Append("-hotfix.").Append(hotfixIncrement);
                }

                if (isAlpha || isBeta)
                {
                    sb.Append(" EXPERIMENTAL");
                } else
                {
                    sb.Append(" STABLE");
                }

                return  sb.ToString();
            }

            public string ToDetailedString()
            {
                return this.ToString() + "+" + metadata;
            }
        }
    }
}
