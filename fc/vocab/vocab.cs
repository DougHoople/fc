using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fc
{
    public enum GrammaticalNumber
    {
        singular,
        plural
    }
    public enum Gender
    {
        masculine,
        feminine,
        both
    }

    public class VocabEntry
    {
        public Gender gender;
        public GrammaticalNumber number;
        public string english;
        public string portuguese;
        public string hint;

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

        public string genderToString()
        {
            string returnValue = null;
            switch (gender)
            {
                case Gender.masculine:
                    returnValue = "o";
                    if (number == GrammaticalNumber.plural)
                        returnValue += "s";
                    break;
                case Gender.feminine:
                    returnValue = "a";
                    if (number == GrammaticalNumber.plural)
                        returnValue += "s";
                    break;
                case Gender.both:
                    returnValue = "a/o";
                    break;
            }
            return returnValue;
        }
    }


    public class vocab
    {
        static List<VocabEntry> vocabList = new List<VocabEntry>();
        static List<VocabEntry> mistakes = new List<VocabEntry>();
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

                // this should really be 
                // vocabEntry = new VocabEntry(inputString);
                // lots of leakage and coupling here.
                vocabEntry = new VocabEntry();

                token = S.IndexOf(@",");
                if (token == -1)
                    continue;
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
                        vocabEntry.gender = Gender.both;
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
        static void Main(string[] args)
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
