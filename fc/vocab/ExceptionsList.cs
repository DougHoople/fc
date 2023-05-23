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
        public class StationExceptionEntry
        {
            public bool umlaut;
            public string pluralEnding;
        }
        public Dictionary<string, StationExceptionEntry> stationExceptions = new Dictionary<string, StationExceptionEntry>()
        {
             // Laura Bennett's 1a exceptions (er/el/en nouns taking umlaut)  
             { "Mutter", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
        };
        public Dictionary<string, StationExceptionEntry> station1Exceptions = new Dictionary<string, StationExceptionEntry>()
        {
             // Laura Bennett's 1a exceptions (er/el/en nouns taking umlaut)  

            // the big two 
             { "Mutter", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Tochter", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }

            // the lesser big two 
            ,{ "Kloster", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Wasser", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }

            // der 
            ,{ "Apfel", new StationExceptionEntry()  {  umlaut = true, pluralEnding = "" } }
            ,{ "Boden", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Garten", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Bruder", new StationExceptionEntry()  {  umlaut = true, pluralEnding = "" } }
            ,{ "Hammer", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Mantel", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Nagel", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Schaden", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Schwager", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Vater", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }
            ,{ "Vogel", new StationExceptionEntry()  { umlaut = true, pluralEnding = "" } }


            // outlier (gets misread because of "el" over in neut mono 4
            ,{ "Spiel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }


            // Laura Bennett's 1b exceptions (er/el/en fem nouns taking n and neuter nouns just mostly hanging out )  
           // ,{ "Bibel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Brezel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Butter", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } }
           // ,{ "Fabel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Gabel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Insel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Kartoffel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Kugel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Mandel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Nadel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Nudel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Regel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Schachtel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Schüssel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Tafel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Windel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Wurzel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
           // ,{ "Zwiebel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }

            // plus one to not get caught in the Station1 vowel trap until it gets gender-generalized
            ,{ "Frau", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }

           // ,{ "Fieber", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } } // nx
           // ,{ "Kabel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } }  // nx
            ,{ "Kamel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
           // ,{ "Kissen", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } } // nx
           // ,{ "Messer", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } } // nx
           // ,{ "Mittel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } } // nx
           // ,{ "Möbel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } }  // nx
           // ,{ "Rätsel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } } // nx
           // ,{ "Segel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "" } }  // nx

        };
        public Dictionary<string, StationExceptionEntry> station2Exceptions = new Dictionary<string, StationExceptionEntry>()
        {
            // Laura Bennett's 2 exceptions (fem mono nouns that should fall through to 4 for -e & umlaut ) 
             { "Angst", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Bank", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Brust", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Frucht", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Hand", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Kraft", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Kuh", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Macht", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Maus", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Nacht", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Nuss", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Schnur", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Stadt", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Wand", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }
            ,{ "Wurst", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }

            // male persons & animals, needs further attention
            ,{ "Professor", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Held", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Diplomat", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Polizist", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Narr", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }


            // plus a weird one-off
            ,{ "Floß", new StationExceptionEntry()  { umlaut = true, pluralEnding = "e" } }


        };
        public Dictionary<string, StationExceptionEntry> station4aExceptions = new Dictionary<string, StationExceptionEntry>()
        {
             // Laura Bennett's 4a exceptions (masc & neuter mono that take "en" instead of "e" or "er", should have been picked up in 2?   )  
             // der
             { "Fleck", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Nerv", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Schmerz", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Staat", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Strahl", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Typ", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Zeh", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }

            // das 
            ,{ "Bett", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Elektron", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }  // needs more scientific terms
            ,{ "Fakt", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Hemd", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Herz", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }
            ,{ "Ohr", new StationExceptionEntry()  { umlaut = false, pluralEnding = "en" } }

            // der
            ,{ "Affe", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Buchstabe", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Friede", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Funke", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Gedanke", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Löwe", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Name", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Same", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            // das
            ,{ "Auge", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Ende", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Erbe", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Finale", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }
            ,{ "Interesse", new StationExceptionEntry()  { umlaut = false, pluralEnding = "n" } }

        };
        public Dictionary<string, StationExceptionEntry> station4bExceptions = new Dictionary<string, StationExceptionEntry>()
        {
            // Laura Bennett's 4 exceptions (mono masc and neuter that fall through to the default  )  
            // der 
             { "Arm", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Halm", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Huf", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Hund", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Ort", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Pfad", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Punkt", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Ruf", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Schluck", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Schuh", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Stoff", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Tag", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            // das 
            ,{ "Bein", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Boot", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Ding", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Haar", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Jahr", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Pferd", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Schaf", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Schiff", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Schwein", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Spiel", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Stück", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Tier", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Tor", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Werk", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
            ,{ "Wort", new StationExceptionEntry()  { umlaut = false, pluralEnding = "e" } }
        };
        public Dictionary<string, StationExceptionEntry> station5Exceptions = new Dictionary<string, StationExceptionEntry>()
        {
        };
    }
}