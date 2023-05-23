using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fc
{
    public partial class VocabEntry
    {

        public bool StationException(int station, VocabEntry vocabEntry, out bool umlaut, out string pluralEnding)
        {
            var stationExceptions = new Dictionary<string, StationExceptionEntry>();

            switch (station)
            {
                case 1:
                    stationExceptions = station1Exceptions;
                    break;
                case 2:
                    stationExceptions = station2Exceptions;
                    break;
                case 3:
                    stationExceptions = station4aExceptions;
                    break;
                case 4:
                    stationExceptions = station4bExceptions;
                    break;
                case 5:
                    stationExceptions = station5Exceptions;
                    break;
            }

            if (stationExceptions.ContainsKey(vocabEntry.portuguese))
            {
                var entry = stationExceptions[vocabEntry.portuguese];
                umlaut = entry.umlaut;
                pluralEnding = entry.pluralEnding;
                return true;
            }
            umlaut = false;
            pluralEnding = "";
            return false;
        }
        public bool Station1(VocabEntry vocabEntry, out bool umlaut, out string pluralEnding)
        {
            umlaut = false;
            pluralEnding = "";

            if (vocabEntry.gender == Gender.neuter)
            {
                if (vocabEntry.portuguese.StartsWith("Ge") && vocabEntry.portuguese.EndsWith("e"))
                {
                    umlaut = false;
                    pluralEnding = "";
                    return true;
                }
            }

            string[] endings1 =
                {
                    "ant"
                };
            if (vocabEntry.gender == Gender.masculine)
            {
                foreach (var ending in endings1)
                {
                    if (vocabEntry.portuguese.EndsWith(ending))
                    {
                        umlaut = false;
                        pluralEnding = "en";
                        return true;
                    }
                }
            }
            string[] endings2neuter =
                {
                     "chen"
                    ,"lein"
                    ,"er"
                    ,"el"
                    ,"en"
                };

            string[] endings2masc =
                {
                     "er"
                    ,"el"
                    ,"en"
                };
            string[] endings2fem =
                {
                };

            string[] endings;

            if (vocabEntry.gender == Gender.neuter)
                endings = endings2neuter;
            else
                if (vocabEntry.gender == Gender.masculine)
                endings = endings2masc;
            else
                endings = endings2fem;

            foreach (var ending in endings)
            {
                if (vocabEntry.portuguese.EndsWith(ending))
                {
                    umlaut = false;
                    pluralEnding = "";
                    return true;
                }
            }

            string[] endings3masc =
            {
                     "é"
                    ,"i"
                    ,"u"
            };
            string[] endings3neuter =
            {
                     "o"
            };
            string[] endings3fem =
                {
                     "a"
                    ,"y"
                };
            if (vocabEntry.gender == Gender.neuter)
                endings = endings3neuter;
            else
                if (vocabEntry.gender == Gender.masculine)
                endings = endings3masc;
            else
                endings = endings3fem;

            foreach (var ending in endings)
            {
                if (vocabEntry.portuguese.EndsWith(ending))
                {
                    umlaut = false;
                    pluralEnding = "s";
                    return true;
                }
            }

            string[] endings4 =
            {
                     "ig"
                    ,"ich"
                    ,"ling"
                    ,"eur"
                    ,"ier"
                    ,"ör"
                };

            if (vocabEntry.gender == Gender.feminine || vocabEntry.gender == Gender.neuter)
            {
                if (vocabEntry.portuguese.EndsWith("nis"))
                {
                    umlaut = false;
                    pluralEnding = "se";
                    return true;
                }
                if (vocabEntry.portuguese.EndsWith("sal"))
                {
                    umlaut = false;
                    pluralEnding = "e";
                    return true;
                }
            }
            else
            {
                // masculine
                foreach (var ending in endings4)
                {
                    if (vocabEntry.portuguese.EndsWith(ending))
                    {
                        umlaut = false;
                        pluralEnding = "e";
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Station2(VocabEntry vocabEntry, out bool umlaut, out string pluralEnding)
        {
            umlaut = false;
            pluralEnding = "";

            if (vocabEntry.gender == Gender.feminine)
            {
                if (vocabEntry.portuguese.EndsWith("e") || vocabEntry.portuguese.EndsWith("r") || vocabEntry.portuguese.EndsWith("l"))
                    pluralEnding = "n";
                else
                    if (vocabEntry.portuguese.EndsWith("n"))
                    pluralEnding = "nen";
                else
                    pluralEnding = "en";
                return true;
            }
            else
            {
                if (vocabEntry.portuguese.EndsWith("e"))
                {
                    pluralEnding = "n";
                    return true;
                }
            }

            // add masculine weak nouns once you have a handle on them. 
            // most important difficult corner case of them all. 
            return false;
        }
        public bool Station3(VocabEntry vocabEntry, out bool umlaut, out string pluralEnding)
        {
            // English/French loanwords that have not been "Germanized" 
            // Probably needs a raw list of all of them. 
            umlaut = false;
            pluralEnding = "";
            string[] foreignLoanwords =
            {
                // needs further additions
                 "Auto"
                ,"Baby"
                ,"Kamera"
                ,"Radio"
            };
            foreach (var foreignLoanword in foreignLoanwords)
            {
                if (vocabEntry.portuguese == foreignLoanword)
                {
                    umlaut = false;
                    pluralEnding = "s";
                    return true;
                }
            }

            umlaut = false;
            pluralEnding = "";

            return false;
        }
        public bool Station4(VocabEntry vocabEntry, out bool umlaut, out string pluralEnding)
        {
            umlaut = false;
            pluralEnding = "";

            if (vocabEntry.gender == Gender.feminine)  // last stop for feminines !! doesn't actually come here
            {
                umlaut = true;
                pluralEnding = "e";
                return true;
            }

            if (SyllableCount(vocabEntry.portuguese) == 1)
            {
                if (vocabEntry.gender == Gender.masculine) 
                 {
                    umlaut = true;
                    pluralEnding = "e";
                    return true;
                } 
                else if (vocabEntry.gender == Gender.neuter)
                {
                    umlaut = true;
                    pluralEnding = "er";
                    return true;
                }
            }
            umlaut = false;
            pluralEnding = "";

            return false;
        }
    }
}