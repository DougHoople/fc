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
        public string AddTheUmlaut(string portuguese)
        {
            char[] vowelArray = { 'a', 'o', 'u', 'ä', 'ö', 'ü', 'A', 'O', 'U', 'Ü', 'Ö', 'Ä' };

            char[] charArray = portuguese.ToCharArray();
            for ( int i = 0; i < charArray.Length; i++ )
            {
                char c = charArray[i];
                foreach (var vowel in vowelArray)
                {
                    if (c == vowel)
                    {
                        switch (c)
                        {
                            case 'a':
                                c = 'ä';
                                break;
                            case 'o':
                                c = 'ö';
                                break;
                            case 'u':
                                c = 'ü';
                                break;
                            case 'A':
                                c = 'Ä';
                                break;
                            case 'O':
                                c = 'Ö';
                                break;
                            case 'U':
                                c = 'Ü';
                                break;
                        }
                        charArray[i] = c;
                        return new string (charArray);
                    }
                }
            }
            return portuguese;
        }
        public bool TakesAnUmlaut(string portuguese)
        {
            char[] vowelArray = { 'a', 'o', 'u', 'A', 'O', 'U' };

            char[] charArray = portuguese.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                foreach (var vowel in vowelArray)
                {
                    // bump into first vowel.  If it qualifies for an umlaut, then return true;
                    if (c == vowel)
                    {
                        return true; 
                    }
                }
            }
            return false;
        }
        public int SyllableCount(string portuguese)
        {
            char[] vowelArray = { 'a', 'e', 'i', 'o', 'u', 'ä', 'ö', 'ü', 'A', 'E', 'I', 'O', 'U', 'Ü', 'Ö', 'Ä' };

            int count = 0;
            bool isVowel = false;
            bool wasVowel = false;

            char[] charArray = portuguese.ToCharArray();
            foreach (var c in charArray)
            {
                foreach (var vowel in vowelArray)
                {
                    if (c == vowel)
                    { 
                        isVowel = true;
                        break;
                    }
                }
                if (isVowel && !wasVowel)  // new vowel, new syllable
                {
                    count++;
                }
                wasVowel = isVowel;
                isVowel = false;
            }
            return count;
        }
        public bool ShouldBeFeminine(VocabEntry vocabEntry)
        {
            string[] endings = 
                { "schaft"
                    ,"ung"
                    ,"age"
                    ,"ei"
                    ,"heit"
                    ,"keit"
                    ,"ion"
                    ,"tät"
                    ,"ur"
                    ,"ek"
                    ,"eke"
                    ,"ie"
                    ,"ik"
                    ,"in"
                    ,"e"
            };
            return ShouldBeAnything(vocabEntry, endings);
        }

        public bool ShouldBeNeuter(VocabEntry vocabEntry)
        {
            string[] endings =
                {
                     "tum"
                    ,"ment"
                    ,"eum"
                    ,"ium"
                    ,"um"
                    ,"ett"
                    ,"chen"
                    ,"lein"
                    ,"en"  // ??? 
                    ,"o"
                };

            return ShouldBeAnything(vocabEntry, endings);
        }
        public bool ShouldBeMasculine(VocabEntry vocabEntry)
        {
            string[] endings =
                {
                     "el"
                    ,"ig"
                    ,"ich"
                    ,"ling"
                    ,"or"
                    ,"ist"
                    ,"ent"
                    ,"ant"
                    ,"ar"
                    ,"är"
                    ,"ismus"

                    // leaving off "en" and "er" for now
                };

            return ShouldBeAnything(vocabEntry, endings);
        }
        public bool ShouldBeAnything( VocabEntry vocabEntry, string [] endings )
        {
            foreach (var ending in endings)
            {
                if (vocabEntry.portuguese.EndsWith(ending))
                    return true;
            }
            return false;
        }

        public bool Xpected()
        {

            if (ShouldBeFeminine(this) && this.gender == Gender.feminine
                ||
                ShouldBeNeuter(this) && this.gender == Gender.neuter
                ||
               ShouldBeMasculine(this) && this.gender == Gender.masculine)
                return true;

            return false; 
        }
        public string PredictPlural()
        {
            bool umlaut = false;
            string pluralEnding = "";
            string exitLevel;

            exitLevel = "Station1Exception"; 
            if (!StationException(1, this, out umlaut, out pluralEnding))
            {
                exitLevel = "Station1";
                if (!Station1(this, out umlaut, out pluralEnding)) {
                    exitLevel = "Station2Exception";
                    if (!StationException(2, this, out umlaut, out pluralEnding))
                    {
                        exitLevel = "Station2";
                        if (!Station2(this, out umlaut, out pluralEnding)) { 
                                exitLevel = "Station3";
                                if (!Station3(this, out umlaut, out pluralEnding)) { 
                                exitLevel = "Station4aException";
                                if (!StationException(3, this, out umlaut, out pluralEnding)) {
                                    exitLevel = "Station4bException";
                                    if (!StationException(4, this, out umlaut, out pluralEnding)) {
                                        exitLevel = "Station4";
                                        if (!Station4(this, out umlaut, out pluralEnding)) { 
                                            exitLevel = "Station5Exception";
                                            if (!StationException(5, this, out umlaut, out pluralEnding)) { 
                                                exitLevel = "Station5";
                                                // station 5 
                                                pluralEnding = "e";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string plural = this.portuguese;
            if (umlaut)
                plural = AddTheUmlaut(this.portuguese);

            plural += pluralEnding;

            Debug.WriteLine($"Portuguese: {this.portuguese} gives plural: {plural} out of exitLevel {exitLevel}");

            return plural;
        }
    }

    public enum GrammaticalNumber
    {
        singular,
        plural
    }
    public enum Gender
    {
        masculine,
        feminine,
        neuter
    }

    public partial class VocabEntry
    {
        public Gender gender;
        public GrammaticalNumber number;
        public string english;
        public string portuguese;
        public string hint;
        public string predictedPlural;
        public bool xpected;

        public VocabEntry()
        {
        }
        public VocabEntry(VocabEntry ve)
        {
            this.gender = ve.gender;
            this.number = ve.number;
            this.english = ve.english;
            this.portuguese = ve.portuguese;
            this.hint = ve.hint;
        }

        public string GenderToString()
        {
            string returnValue = null;
            switch (gender)
            {
                case Gender.masculine:
                    returnValue = "der";
                    break;
                case Gender.feminine:
                    returnValue = "die";
                    break;
                case Gender.neuter:
                    returnValue = "das";
                    break;
            }
            return returnValue;
        }
        public static Gender StringToGender(string article)
        {
            var returnValue = Gender.neuter;
            switch (article)
            {
                case "der":
                    returnValue = Gender.masculine;
                        break;
                case "die":
                    returnValue = Gender.feminine;
                    break;
                case "das":
                    returnValue = Gender.neuter;
                    break;
            }
            return returnValue;
        }

    }



    public class vocab
    {
        static List<VocabEntry> vocabList = new List<VocabEntry>();
        static List<VocabEntry> mistakes = new List<VocabEntry>();
        static List<VocabEntry> frontrow = new List<VocabEntry>();
        static List<VocabEntry> middlerow = new List<VocabEntry>();
        static List<VocabEntry> backrow = new List<VocabEntry>();
        static List<string> infinitives = new List<string>();
        static List<string> sentences = new List<string>();
        static string basedir = @"C:\0junk\fc\fc\";
        static Random r;

        public static VocabEntry getRandomVocabEntry()
        {
            VocabEntry vocabEntry = vocabList[r.Next(0, vocabList.Count)];
            return vocabEntry;
        }

        public static void recordMistake(VocabEntry ve, string status)
        {
            mistakes.Add(new VocabEntry(ve));

            StreamWriter SW;

            SW = File.AppendText(basedir + @"mistakes.txt");
            SW.WriteLine(status + " " + ve.portuguese.PadRight(30) + " " + DateTime.Now);
            SW.Close();

            for (int i = 0; i < 10; i++)
                vocabList.Add(new VocabEntry(ve));

            // re-randomize
            List<VocabEntry> tmpList = vocabList;
            vocabList = new List<VocabEntry>();
            int index;
            while (tmpList.Count > 0)
            {
                index = r.Next(0, tmpList.Count);
                vocabList.Add(tmpList.ElementAt(index));
                tmpList.RemoveAt(index);
            }
            return;
        }

        public static void removeEntries(VocabEntry ve)
        {
            VocabEntry vocabEntry;
            List<int> indexList = new List<int>();

            for (int i = vocabList.Count - 1; i >= 0; i--)
            {
                vocabEntry = vocabList[i];
                if (vocabEntry.portuguese == ve.portuguese)
                {
                    indexList.Add(i);
                }
            }
            foreach (int i in indexList)
                vocabList.RemoveAt(i);
            return;
        }

        static void readSeedData()
        {
            StreamReader SR;
            string S;
            VocabEntry vocabEntry;
            // char gender;
            List<VocabEntry> tmpVocabList = new List<VocabEntry>();

            SR = File.OpenText(basedir + @"vocab.txt");
            while ((S = SR.ReadLine()) != null)
            {
                int offset = -1;
                int repeatCount = 1;
                string specialInstruction = null;

                if ((offset = S.IndexOf(@"@")) != -1)
                {
                    specialInstruction = S.Substring(offset + 1);
                    repeatCount = Int32.Parse(specialInstruction);
                    S = S.Substring(0, offset);
                }

                if (S.IndexOf(@"//") != -1)
                {
                    // double-slash anywhere in the line means skip the whole line (for now) 
                    continue;
                }
                S = S.Trim();

                if (S.IndexOf(@"[begin]") != -1)
                {
                    // empty it brutally
                    tmpVocabList = new List<VocabEntry>();
                    continue;
                }

                if (S.IndexOf(@"[end]") != -1)
                {
                    // finish off. We only wanted the section
                    break;
                }

                int token;
                // bool both = false;

                // this should really be 
                // vocabEntry = new VocabEntry(inputString);
                // lots of leakage and coupling here.
                vocabEntry = new VocabEntry();

                token = S.IndexOf(@",");
                if (token == -1)
                    continue;
                vocabEntry.portuguese = S.Substring(0, token);
                vocabEntry.english = S.Substring(token + 1);

                if (vocabEntry.english.Equals("der"))
                    vocabEntry.gender = Gender.masculine;
                else if (vocabEntry.english.Equals("die"))
                        vocabEntry.gender = Gender.feminine;
                else if (vocabEntry.english.Equals("das"))
                    vocabEntry.gender = Gender.neuter;

                vocabEntry.xpected = vocabEntry.Xpected();
                vocabEntry.predictedPlural = vocabEntry.PredictPlural();


                for (int i = 0; i < repeatCount; i++)
                    tmpVocabList.Add(vocabEntry);
            }
            SR.Close();

            int index;
            while (tmpVocabList.Count > 0)
            {
                index = r.Next(0, tmpVocabList.Count);
                vocabList.Add(tmpVocabList.ElementAt(index));
                tmpVocabList.RemoveAt(index);
            }
            return;
        }

        static void readSeedDataPortuguese()
        {
            StreamReader SR;
            string S;
            VocabEntry vocabEntry;
            char gender;
            List<VocabEntry> tmpVocabList = new List<VocabEntry>();

            SR = File.OpenText(basedir + @"vocab.txt");
            while ((S = SR.ReadLine()) != null)
            {
                int offset = -1;
                int repeatCount = 1;
                string specialInstruction = null;

                if ((offset = S.IndexOf(@"@")) != -1)
                {
                    specialInstruction = S.Substring(offset + 1);
                    repeatCount = Int32.Parse(specialInstruction);
                    S = S.Substring(0, offset);
                }

                if (S.IndexOf(@"//") != -1)
                {
                    // double-slash anywhere in the line means skip the whole line (for now) 
                    continue;
                }
                S = S.Trim();

                if (S.IndexOf(@"[begin]") != -1)
                {
                    // empty it brutally
                    tmpVocabList = new List<VocabEntry>();
                    continue;
                }

                if (S.IndexOf(@"[end]") != -1)
                {
                    // finish off. We only wanted the section
                    break;
                }

                int token;
                bool both = false;

                token = S.IndexOf(@" ");
                if (S.IndexOf(@"/") == 1 && token == 3)
                {
                    both = true;
                }
                else
                {
                    if (!(token > 0 && token < 3))
                        // broken token. Bad record actually.
                        // looking for 'a', 'o', 'as', 'os'
                        continue;
                }

                string article = S.Substring(0, token);

                // this should really be 
                // vocabEntry = new VocabEntry(inputString);
                // lots of leakage and coupling here.
                vocabEntry = new VocabEntry();

                vocabEntry.number = (token == 1 || both ? GrammaticalNumber.singular : GrammaticalNumber.plural);

                S = S.ToLower();
                if (both)
                    gender = 'b';
                else
                    gender = S[0];

                switch (gender)
                {
                    case 'o':
                        vocabEntry.gender = Gender.masculine;
                        break;
                    case 'a':
                        vocabEntry.gender = Gender.feminine;
                        break;
                    case 'b':
                        vocabEntry.gender = Gender.neuter;
                        break;
                }

                S = S.Substring(token + 1);

                token = S.IndexOf(@",");
                vocabEntry.portuguese = S.Substring(0, token);
                vocabEntry.english = S.Substring(token + 1);

                for (int i = 0; i < repeatCount; i++)
                    tmpVocabList.Add(vocabEntry);
            }
            SR.Close();

            int index;
            while (tmpVocabList.Count > 0)
            {
                index = r.Next(0, tmpVocabList.Count);
                vocabList.Add(tmpVocabList.ElementAt(index));
                tmpVocabList.RemoveAt(index);
            }
            return;
        }

        static public void StaticConstructor(string[] args)
        {
            if (args[0] != null)
            {
                // if directory and exists, set basedir
                basedir = args[0] + @"\";
            }
            r = new Random();

            readSeedData();

        }

        static void MainMain(string article, string portuguese)
        {
            Gender gender = VocabEntry.StringToGender(article);

            var vocabEntry = new VocabEntry { gender = gender, portuguese = portuguese };
            var plural = vocabEntry.PredictPlural();
        }
        static void Main(string[] args)
        {
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfahrt");
            MainMain("die", "Abfallablagerung");
            MainMain("die", "Abfallart");
            MainMain("die", "Abfallart");
            MainMain("die", "Abfallart");
            MainMain("die", "Abfallaufbereitung");
            MainMain("die", "Abfallaufbereitung");
            MainMain("die", "Abfallaufbereitung");
            MainMain("die", "Abfallbeauftragte");
            MainMain("die", "Abfallbehandlungsanlage");
            MainMain("die", "Abfallbehandlungspflichtenverordnung");
            MainMain("die", "Abfallbehandlung");
            MainMain("die", "Abfallbehandlung");
            MainMain("die", "Abfallbehandlung");
            MainMain("die", "Abfallbelastung");
            MainMain("die", "Abfallberatung");
            MainMain("die", "Abfallbeseitigungsanlage");
            MainMain("die", "Abfallbeseitigung");
            MainMain("die", "Abfallbeseitigung");
            MainMain("die", "Abfallbeseitigung");
            MainMain("die", "Abfallbeseitigung");

            MainMain("der", "Schlüssel");
            MainMain("der", "Feigling");
            MainMain("der", "Honig");
            MainMain("der", "Teppich");
            MainMain("der", "Adjutant");
            MainMain("die", "Frau");
            MainMain("das", "Fräulein");
            MainMain("das", "Mädchen");
            MainMain("die", "Zeitung");
            MainMain("der", "Verlust");
            MainMain("der", "Beruf");
            MainMain("der", "Besuch");
            MainMain("der", "Verlag");
            MainMain("das", "Gebäude");
            MainMain("der", "Professor");
            MainMain("der", "Held"     );
            MainMain("der", "Diplomat" );
            MainMain("der", "Polizist" );
            MainMain("der", "Narr"     );


            MainMain("der", "Apfel");
            MainMain("der", "Boden");
            MainMain("der", "Bruder");
            MainMain("der", "Garten");
            MainMain("der", "Hammer");
            MainMain("der", "Mantel");
            MainMain("der", "Nagel");
            MainMain("der", "Schaden");
            MainMain("der", "Schwager");
            MainMain("der", "Vater");
            MainMain("der", "Vogel");
            MainMain("die", "Mutter");
            MainMain("die", "Tochter");
            MainMain("die", "Kloster");
            MainMain("die", "Wasser");
            MainMain("die", "Finsternis");
            MainMain("das", "Zeugnis");
            MainMain("das", "Ärgernis");
            MainMain("die", "Schicksal");
            MainMain("das", "Scheusal");
            MainMain("das", "Floß");

            MainMain("die", "Bibel");
            MainMain("die", "Brezel");
            MainMain("die", "Butter");
            MainMain("die", "Fabel");
            MainMain("die", "Gabel");
            MainMain("die", "Insel");
            MainMain("die", "Kartoffel");
            MainMain("die", "Kugel");
            MainMain("die", "Mandel");
            MainMain("die", "Nadel");
            MainMain("die", "Nudel");
            MainMain("die", "Regel");
            MainMain("die", "Schachtel");
            MainMain("die", "Tafel");
            MainMain("die", "Windel");
            MainMain("die", "Wurzel");
            MainMain("die", "Zwiebel");
            MainMain("das", "Fieber");
            MainMain("das", "Kabel");
            MainMain("das", "Kamel");
            MainMain("das", "Kissen");
            MainMain("das", "Messer");
            MainMain("das", "Mittel");
            MainMain("das", "Möbel");
            MainMain("das", "Rätsel");
            MainMain("das", "Segel");
            MainMain("die", "Angst");
            MainMain("die", "Bank");
            MainMain("die", "Brust");
            MainMain("die", "Frucht");
            MainMain("die", "Hand");
            MainMain("die", "Kraft");
            MainMain("die", "Kuh");
            MainMain("die", "Macht");
            MainMain("die", "Maus");
            MainMain("die", "Nacht");
            MainMain("die", "Nuss");
            MainMain("die", "Schnur");
            MainMain("die", "Stadt");
            MainMain("die", "Wand");
            MainMain("die", "Wurst");
            MainMain("der", "Fleck");
            MainMain("der", "Nerv");
            MainMain("der", "Schmerz");
            MainMain("der", "Staat");
            MainMain("der", "Strahl");
            MainMain("der", "Typ");
            MainMain("der", "Zeh");
            MainMain("das", "Bett");
            MainMain("das", "Fakt");
            MainMain("das", "Hemd");
            MainMain("das", "Herz");
            MainMain("das", "Ohr");
            MainMain("der", "Affe");
            MainMain("der", "Buchstabe");
            MainMain("der", "Friede");
            MainMain("der", "Funke");
            MainMain("der", "Gedanke");
            MainMain("der", "Name");
            MainMain("der", "Same");
            MainMain("das", "Auge");
            MainMain("das", "Ende");
            MainMain("das", "Erbe");
            MainMain("das", "Finale");
            MainMain("das", "Herz");
            MainMain("das", "Interesse");
            MainMain("der", "Arm");
            MainMain("der", "Halm");
            MainMain("der", "Huf");
            MainMain("der", "Hund");
            MainMain("der", "Ort");
            MainMain("der", "Pfad");
            MainMain("der", "Punkt");
            MainMain("der", "Ruf");
            MainMain("der", "Schluck");
            MainMain("der", "Schuh");
            MainMain("der", "Stoff");
            MainMain("der", "Tag");
            MainMain("das", "Bein");
            MainMain("das", "Boot");
            MainMain("das", "Ding");
            MainMain("das", "Haar");
            MainMain("das", "Jahr");
            MainMain("das", "Pferd");
            MainMain("das", "Schaf");
            MainMain("das", "Schiff");
            MainMain("das", "Schwein");
            MainMain("das", "Spiel");
            MainMain("das", "Stück");
            MainMain("das", "Tier");
            MainMain("das", "Tor");
            MainMain("das", "Werk");
            MainMain("das", "Wort");
        }
        static void MainOldPortuguese(string[] args)
        {
            StaticConstructor(args);
            VocabEntry ve = null;
            while (1 == 1)
            {
                //#if (promptedenglish)                
                ve = getRandomVocabEntry();
                Console.WriteLine(ve.english);
                Console.ReadLine();
                Console.WriteLine(ve.portuguese);
                // Console.WriteLine(ve.genderToString() + " " + ve.portuguese);
                // Console.ReadLine();
                ConsoleKeyInfo ki = Console.ReadKey(true);
                ki = Console.ReadKey(true);
                switch (ki.KeyChar)
                {
                    case 'a':
                        Console.WriteLine(ve.english);
                        ki = Console.ReadKey(true);
                        Console.WriteLine();
                        break;
                    case 'm':
                        Console.WriteLine("adding to mistake queue: " + ve.portuguese);
                        recordMistake(ve, "NG");
                        ki = Console.ReadKey(true);
                        Console.WriteLine();
                        break;
                    case 'r':
                        Console.WriteLine("removing all entries with portuguese of: " + ve.portuguese);
                        removeEntries(ve);
                        ki = Console.ReadKey(true);
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                //#endif 

#if (gender)
                ve = getRandomVocabEntry();
                Console.WriteLine(ve.portuguese);
                ConsoleKeyInfo ki = Console.ReadKey(true);
                Console.WriteLine();
                Console.WriteLine(ve.genderToString() + " " + ve.portuguese);
                ki = Console.ReadKey(true);
                switch (ki.KeyChar)
                {
                    case 'a':
                    Console.WriteLine(ve.english);
                    ki = Console.ReadKey(true);
                    Console.WriteLine();
                        break;
                    case 'm':
                    Console.WriteLine("adding to mistake queue: " + ve.portuguese);
                    recordMistake(ve, "NG");
                    ki = Console.ReadKey(true);
                    Console.WriteLine();
                        break;
                    case 'r':
                        Console.WriteLine("removing all entries with portuguese of: " + ve.portuguese);
                        removeEntries(ve);
                        ki = Console.ReadKey(true);
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
#endif
            }
        }
    }
}
