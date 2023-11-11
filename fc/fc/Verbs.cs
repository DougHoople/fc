using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace fc
{
    public class Verbiage
    {
        string infinitive;
        string conjugatedVerb;
        PersonalPronoun pronoun;
        Conjugation conjugation;
        bool irregular;

        public Verbiage(string infinitive, string conjugatedVerb, PersonalPronoun pronoun, Conjugation conjugation, bool irregular)
        {
            this.infinitive = infinitive;
            this.conjugatedVerb = conjugatedVerb;
            this.pronoun = pronoun;
            this.conjugation = conjugation;
            this.irregular = irregular;
        }
        public Verbiage(Verbiage v)
        {
            this.infinitive = v.infinitive;
            this.conjugatedVerb = v.conjugatedVerb;
            this.pronoun = v.pronoun;
            this.conjugation = v.conjugation;
            this.irregular = v.irregular;
        }

        public string Infinitive
        {
            get { return infinitive; }
        }
        public string ConjugatedVerb
        {
            get { return conjugatedVerb; }
        }
        public PersonalPronoun Pronoun
        {
            get { return pronoun; }
        }
        public Conjugation Conjugation
        {
            get { return conjugation; }
        }
        public bool IsIrregular
        {
            get { return irregular; }
        }
    }
    public enum Person
    {
        firstPersonSingular,
        secondPersonSingular, // fixup irregular structure before adding in 
        thirdPersonSingular,
        firstPersonPlural,
        secondPersonPlural,   // fixup irregular structure before adding in 
        thirdPersonPlural
    }
    public enum Conjugation
    {
        Present,
        Preterite,
        Imperfect,
        Future,
        Conditional,
        SubjunctivePresent,
        SubjunctiveImperfect,
        SubjunctiveFuture
    }

    public struct History
    {
        int index;
        PersonalPronoun pronoun;
        public History(int index, PersonalPronoun pronoun)
        {
            this.index = index;
            this.pronoun = pronoun;
        }
        public static bool isInHistory(int index, PersonalPronoun pronoun, List<History> historyList)
        {

            // not really the right place for this, but it gets done quietly and automatically
            if (historyList.Count > 20)
                historyList.RemoveAt(0);

            if (index < 0 && pronoun == null)
                return false;

            foreach (History h in historyList)
            {
                if (h.pronoun == pronoun && h.index == index)
                    return true;
            }
            return false;
        }
    }

    public class PersonalPronoun
    {
        public string pronoun;
        public Person person;

        public PersonalPronoun(string pronoun, Person person)
        {
            this.pronoun = pronoun;
            this.person = person;
        }
        public override string ToString()
        {
            return this.pronoun;
        }
    }


    class PersonalPronouns
    {
        private List<PersonalPronoun> m_pronounList;
        private Random m_random;

        public PersonalPronouns()
        {
            m_pronounList = new List<PersonalPronoun>();
            m_random = new Random();

            m_pronounList.Add(new PersonalPronoun("eu", Person.firstPersonSingular));
            m_pronounList.Add(new PersonalPronoun("tu", Person.secondPersonSingular));
            m_pronounList.Add(new PersonalPronoun("você", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("ele", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("ela", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("a gente", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("nós", Person.firstPersonPlural));
            m_pronounList.Add(new PersonalPronoun("vós", Person.secondPersonPlural));
            m_pronounList.Add(new PersonalPronoun("eles", Person.thirdPersonPlural));
            m_pronounList.Add(new PersonalPronoun("elas", Person.thirdPersonPlural));
            m_pronounList.Add(new PersonalPronoun("vocês", Person.thirdPersonPlural));
        }
        public int Count
        {
            get { return m_pronounList.Count; }
        }
        public PersonalPronoun getRandomPersonalPronoun()
        {
#if (compileunwanted)
            private Random rand = new Random(); 
            public T RandomEnum<T>() 
            { 
                T[] values = (T[])Enum.GetValues(typeof(T)); 
                return values[rand.Next(0, values.Length)]; 
            } 
#endif
            Person[] v = (Person[])Enum.GetValues(typeof(Person));
            // override for filtering
            // v = new Person[] { Person.firstPersonSingular, Person.thirdPersonSingular };
            Person rp = v[m_random.Next(0, v.Length)];
            List<PersonalPronoun> pList = this.m_pronounList.FindAll(o => o.person == rp);
            PersonalPronoun pp = pList[m_random.Next(0, pList.Count)];

            return pp;
        }
        public PersonalPronoun getRandomPersonalPronoun(Person p)
        {

            List<PersonalPronoun> pList = this.m_pronounList.FindAll(o => o.person == p);
            PersonalPronoun pp = pList[m_random.Next(0, pList.Count)];

            return pp;
        }

    }
    public class IrregularConjugation
    {
        public Person m_person;
        public string m_conjugatedVerb;

        public IrregularConjugation(Person p, string conjugatedVerb)
        {
            m_conjugatedVerb = conjugatedVerb;
            m_person = p;
        }
    }

    public class IrregularVerb
    {
        public string m_infinitive;
        public List<IrregularConjugation> m_conjugations;
        public IrregularVerb(string infinitive, string[] conjugations)
        {
            m_conjugations = new List<IrregularConjugation>();
            m_infinitive = infinitive;
            m_conjugations.Add(new IrregularConjugation(Person.firstPersonSingular, conjugations[(int)Person.firstPersonSingular]));
            m_conjugations.Add(new IrregularConjugation(Person.secondPersonSingular, conjugations[(int)Person.secondPersonSingular]));
            m_conjugations.Add(new IrregularConjugation(Person.thirdPersonSingular, conjugations[(int)Person.thirdPersonSingular]));
            m_conjugations.Add(new IrregularConjugation(Person.firstPersonPlural, conjugations[(int)Person.firstPersonPlural]));
            m_conjugations.Add(new IrregularConjugation(Person.secondPersonPlural, conjugations[(int)Person.secondPersonPlural]));
            m_conjugations.Add(new IrregularConjugation(Person.thirdPersonPlural, conjugations[(int)Person.thirdPersonPlural]));
        }
    }

    public class Endings
    {
        public char letter;
        public string[] endings;
        public Endings(char letter, string[] endings)
        {
            this.letter = letter;
            this.endings = endings;
        }
    }

    public class Tense
    {
        public Conjugation conjugation;
        public List<Endings> endings;
        public List<IrregularVerb> irregularVerbs;
        public Tense(Conjugation conjugation, Endings[] endings)
        {
            this.endings = new List<Endings>();
            this.irregularVerbs = new List<IrregularVerb>();
            this.conjugation = conjugation;
            foreach (Endings e in endings)
            {
                this.endings.Add(e);
            }
        }
        public void addIrregularVerb(IrregularVerb iv)
        {
            irregularVerbs.Add(iv);
        }
    }
    public class Regulars
    {
        static string[] irregs = new string[]  { "crer"
,"ler"
,"medir"
,"ouvir"
,"pedir"
,"perder"
,"rir"
,"valer"
,"dar"
,"dizer"
,"estar"
,"fazer"
,"haver"
,"ir"
,"poder"
,"pôr"
,"querer"
,"saber"
,"ser"
,"ter"
,"trazer"
,"ver"
,"vir"};

        public static List<string> erRegulars;
        public static List<string> arRegulars;
        public static List<string> irRegulars;
        static Random r = new Random();

        static Regulars()
        {
            erRegulars = new List<string>();
            arRegulars = new List<string>();
            irRegulars = new List<string>();
        }
        public static string SubstituteRegular(string verb)
        {
            string newVerb = verb;

            List<string> l = new List<string>(); // to stop compile error. Should never be active.

            if (IsRegular(verb))
            {

                char ending = verb[verb.Length - 2];
                switch (ending)
                {
                    case 'e':
                        l = erRegulars;
                        break;
                    case 'a':
                        l = arRegulars;
                        break;
                    case 'i':
                        l = irRegulars;
                        break;
                }
                int index;
                if (l.Count > 0)
                {
                    index = r.Next(0, l.Count);
                    newVerb = l.ElementAt(index);
                    if (newVerb == "")
                        newVerb = verb;
                }
            }

            return newVerb;
        }

        public static bool IsRegular(string verb)
        {
            bool isRegular = true;
            foreach (string irreg in irregs)
            {
                if (irreg == verb)
                {
                    isRegular = false;
                    break;
                }
            }
            return isRegular;
        }

        public static void AddRegular(string verb)
        {
            if (IsRegular(verb))
            {
                char ending = verb[verb.Length - 2];
                switch (ending)
                {
                    case 'e':
                        erRegulars.Add(verb);
                        break;
                    case 'i':
                        irRegulars.Add(verb);
                        break;
                    case 'a':
                        arRegulars.Add(verb);
                        break;
                }
            }
        }
        public static void AddRegulars(List<string> infinitives)
        {
            foreach (string infinitive in infinitives)
            {
                AddRegular(infinitive);
            }
        }
    }

    public class Verbs
    {
        static Regulars regulars; // should be replaced with something a bit more function-driven
        static PersonalPronouns personalPronouns;
        static List<string> infinitives = new List<string>();
        static List<string> sentences = new List<string>();
        static string basedir = @"D:\0junk\fc\fc\";
        static List<Tense> Tenses;
        static List<History> historyList;
        static List<Verbiage> mistakes;
        static Random r;

        public static void removeInfinitive(string deleteVerb)
        {
            List<int> indexes = new List<int>();

            for (int i = infinitives.Count - 1; i >= 0; i--)
            {
                string s = infinitives[i];
                if (s == deleteVerb)
                {
                    indexes.Add(i);
                }
            }
            foreach (int i in indexes)
            {
                infinitives.RemoveAt(i);
            }
        }
        public static void recordMistake(Verbiage ve, string status)
        {
            mistakes.Add(new Verbiage(ve));

            StreamWriter SW;

            SW = File.AppendText(basedir + @"mistakes.txt");
            SW.WriteLine((status + " " + ve.Infinitive + ", " + ve.Pronoun.ToString() + " " + ve.ConjugatedVerb).PadRight(30) + " " + DateTime.Now);
            SW.Close();

            for (int i = 0; i < 1; i++) // down to 1 from 10 ! 
                infinitives.Add(ve.Infinitive);

            // re-randomize
            List<string> tmpList = infinitives;
            infinitives = new List<string>();
            int index;
            while (tmpList.Count > 0)
            {
                index = r.Next(0, tmpList.Count);
                infinitives.Add(tmpList.ElementAt(index));
                tmpList.RemoveAt(index);
            }

            return;
        }


        public static void removeEntries(Verbiage ve)
        {

#if (compileunwanted)
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
#endif
            return;
        }

        static List<string> readSeedData()
        {
            List<string> fileStrings = new List<string>();
            StreamReader SR;
            string S;
            string ending;

            SR = File.OpenText(basedir + @"infinitives.txt");
            while ((S = SR.ReadLine()) != null)
            {
                if (S.IndexOf(@"//") != -1)
                {
                    // double-slash anywhere in the line means skip the whole line (for now) 
                    continue;
                }
                S = S.Trim();
                if (S.Length < 3 && S != "ir")
                {
                    Console.WriteLine("skipping " + S + " on loading from file");
                    continue;
                }

                ending = S.Substring(S.Length - 2);

                if (ending != "ar" && ending != "er" && ending != "ir" && S != "pôr")
                {
                    Console.WriteLine("skipping " + S);
                    continue;
                }

#if (pooledregulars)
                if (Regulars.IsRegular(S) && !(S == "falar" || S == "comer" || S == "partir"))
                {
                      Regulars.AddRegular(S);
                }
                else
#endif
                fileStrings.Add(S);

            }
            SR.Close();
            return fileStrings;
        }
        static List<string> readSentences()
        {
            List<string> fileStrings = new List<string>();

            string s = "";
            using (StreamReader rdr = File.OpenText(basedir + @"sentences.txt"))
            {
                s = rdr.ReadToEnd();
            }

            string trim = Regex.Replace(s, @"[\r\t\n]", "");
            trim = Regex.Replace(trim, @"  ", " ");
            string[] sentences = trim.Split(new char[2] { '.', '?' });

            foreach (string tok in sentences)
                fileStrings.Add(tok);

            return fileStrings;
        }

        static PersonalPronoun getPersonalPronoun()
        {
            PersonalPronoun p = personalPronouns.getRandomPersonalPronoun();
            return p;
        }

        static string ThirdPersonPluralPreteriteStem(string infinitive)
        {

            string stem = infinitive.Substring(0, infinitive.Length - 2);

            Tense t = Tenses.Find(o => o.conjugation == Conjugation.Preterite);
            IrregularVerb iv = t.irregularVerbs.Find(o => o.m_infinitive == infinitive);

            if (iv != null)
            {
                IrregularConjugation c = iv.m_conjugations.Find(o => o.m_person == Person.thirdPersonPlural);
                stem = c.m_conjugatedVerb.Substring(0, c.m_conjugatedVerb.Length - 2);
            }

            return stem;
        }

        static string FirstPersonIndicativeStem(string infinitive)
        {

            string stem = infinitive.Substring(0, infinitive.Length - 2);

            Tense t = Tenses.Find(o => o.conjugation == Conjugation.Present);
            IrregularVerb iv = t.irregularVerbs.Find(o => o.m_infinitive == infinitive);

            if (iv != null)
            {
                IrregularConjugation c = iv.m_conjugations.Find(o => o.m_person == Person.firstPersonSingular);
                stem = c.m_conjugatedVerb.Substring(0, c.m_conjugatedVerb.Length - 1);
            }

            return stem;
        }


        static string orthoChanging2(string infinitive, string ending)
        {
            // The purpose of this class of changes is to retain the sound of the 
            // consonant at the end of the stem. Applied across the board. 
            // If it's hard, it's supposed to stay hard. 
            // If it's soft, it's supposed to stay soft. 
            // Good lesson in pronunciation here! 

            string oldStem = infinitive.Substring(0, infinitive.Length - 2);
            string newStem = oldStem;
            if (infinitive.Length < 3)
                return newStem;

            if (infinitive.Substring(infinitive.Length - 3) == "car")
            {
                if (ending[0] == 'e' || ending[0] == 'i')
                {
                    newStem = newStem.Substring(0, newStem.Length - 1) + "qu";
                }
            }
            if (infinitive.Substring(infinitive.Length - 3) == "çar")
            {
                if (ending[0] == 'e' || ending[0] == 'i')
                {
                    newStem = newStem.Substring(0, newStem.Length - 1) + "c";
                }
            }
            if (infinitive.Substring(infinitive.Length - 3) == "gar")
            {
                if (ending[0] == 'e' || ending[0] == 'i')
                {
                    newStem = newStem.Substring(0, newStem.Length - 1) + "gu";
                }
            }
            if (infinitive.Substring(infinitive.Length - 3) == "cer")
            {
                if (ending[0] == 'a' || ending[0] == 'o')
                {
                    newStem = newStem.Substring(0, newStem.Length - 1) + "ç";
                }
            }
            if (infinitive.Substring(infinitive.Length - 3) == "ger" || infinitive.Substring(infinitive.Length - 3) == "gir")
            {
                if (ending[0] == 'a' || ending[0] == 'o')
                {
                    newStem = newStem.Substring(0, newStem.Length - 1) + "j";
                }
            }
            if (infinitive.Length > 3)
            {
                if (infinitive.Substring(infinitive.Length - 4) == "guer" || infinitive.Substring(infinitive.Length - 4) == "guir")
                {
                    if (ending[0] == 'a' || ending[0] == 'o')
                    {
                        newStem = newStem.Substring(0, newStem.Length - 1); // knock off the trailing 'u'
                    }
                }
            }
            if (oldStem != newStem)
                newStem = "*** " + newStem;
            return newStem;
        }

        static string conjugateIrregularVerb(string infinitive, PersonalPronoun pronoun, Conjugation conjugation)
        {
            string conjugatedVerb = null;
            Tense tense = Tenses.Find(o => o.conjugation == conjugation);
            List<IrregularVerb> irregularVerbs = tense.irregularVerbs;
            // not guaranteed to find
            IrregularVerb v = irregularVerbs.Find(o => o.m_infinitive == infinitive);
            if (v != null)
            {
                // guaranteed to find
                IrregularConjugation c = v.m_conjugations.Find(o => o.m_person == pronoun.person);
                conjugatedVerb = c.m_conjugatedVerb;
            }
            return conjugatedVerb;
        }

        static string conjugateRegularVerb(string infinitive, PersonalPronoun pronoun, Conjugation conjugation)
        {
            char ending = infinitive[infinitive.Length - 2];
            string stem = infinitive.Substring(0, infinitive.Length - 2);
            string conjugatedEnding = null;
            string conjugatedVerb;

            Tense t = Tenses.Find(o => o.conjugation == conjugation);
            Endings e = t.endings.Find(o => o.letter == ending);
            string[] endings = e.endings;

            switch (pronoun.person)
            {
                case Person.firstPersonSingular:
                    conjugatedEnding = endings[0];
                    break;
                case Person.secondPersonSingular:
                    conjugatedEnding = endings[1];
                    break;
                case Person.thirdPersonSingular:
                    conjugatedEnding = endings[2];
                    break;
                case Person.firstPersonPlural:
                    conjugatedEnding = endings[3];
                    break;
                case Person.secondPersonPlural:
                    conjugatedEnding = endings[4];
                    break;
                case Person.thirdPersonPlural:
                    conjugatedEnding = endings[5];
                    break;
            }


            string newStem;

            switch (conjugation)
            {
                case Conjugation.SubjunctivePresent:
                    newStem = FirstPersonIndicativeStem(infinitive);
                    // todo: double-check the 1st-person present indicative irregulars 
                    //       to ensure that none that need to be checked 
                    //       get skipped for the ortho change. 
                    if (!isIrregular(infinitive, Conjugation.Present))
                        newStem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
                case Conjugation.SubjunctiveFuture:
                    newStem = ThirdPersonPluralPreteriteStem(infinitive);
                    // todo: double-check the 1st-person present indicative irregulars 
                    //       to ensure that none that need to be checked 
                    //       get skipped for the ortho change. 
                    if (!isIrregular(infinitive, Conjugation.Preterite))
                        newStem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
                case Conjugation.SubjunctiveImperfect:
                    newStem = ThirdPersonPluralPreteriteStem(infinitive);
                    // todo: double-check the 1st-person present indicative irregulars 
                    //       to ensure that none that need to be checked 
                    //       get skipped for the ortho change. 
                    if (!isIrregular(infinitive, Conjugation.Preterite))
                        newStem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
                default:
                    newStem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
            }

            conjugatedVerb = newStem + conjugatedEnding;
            return conjugatedVerb;

        }


        static string conjugateVerb(string infinitive, PersonalPronoun pronoun, Conjugation conjugation)
        {
            string conjugatedVerb = null;

            if (isIrregular(infinitive, conjugation))
                conjugatedVerb = conjugateIrregularVerb(infinitive, pronoun, conjugation);
            else
                conjugatedVerb = conjugateRegularVerb(infinitive, pronoun, conjugation);

            return conjugatedVerb;
        }

        static bool isIrregular(string infinitive, Conjugation conjugation)
        {
            Tense t;
            IrregularVerb iv;
            t = Tenses.Find(o => o.conjugation == conjugation);
            if (t != null)
            {
                iv = t.irregularVerbs.Find(o => o.m_infinitive == infinitive);
                if (iv != null)
                    return true;
            }
            return false;
        }

        static public Verbiage GetAConjugation(Conjugation con)
        {
            string infinitive;
            string conjugatedVerb;
            Conjugation conjugation = con;
            PersonalPronoun pronoun;
            int index;

            if (personalPronouns.Count * infinitives.Count <= historyList.Count)
                // for really small datasets, elminate possible endless loop
                historyList.RemoveRange(0, historyList.Count);

            do
            {
                pronoun = getPersonalPronoun();
                index = r.Next(0, infinitives.Count);
            } while (History.isInHistory(index, pronoun, historyList));

            History h = new History(index, pronoun);
            historyList.Add(h);

            infinitive = infinitives[index];

#if (pooledregulars) 
            infinitive = Regulars.SubstituteRegular(infinitive); 
#endif

            conjugatedVerb = conjugateVerb(infinitive, pronoun, conjugation);

            bool isIrregular = Verbs.isIrregular(infinitive, conjugation);
            Verbiage verbiage = new Verbiage(infinitive, conjugatedVerb, pronoun, conjugation, isIrregular);
            return verbiage;
        }
        static public void StaticConstructor(string[] args)
        {
            if (args[0] != null)
            {
                // if directory and exists, set basedir
                basedir = args[0] + @"\";
            }
            r = new Random();

            regulars = new Regulars();
            personalPronouns = new PersonalPronouns();
            Tenses = new List<Tense>();
            historyList = new List<History>();
            mistakes = new List<Verbiage>();

            Endings[] e = new Endings[3];
            Tense t;

            // Conjugation.Present
            e[0] = new Endings('a', new string[] { "o", "as", "a", "amos", "ais", "am" });
            e[1] = new Endings('e', new string[] { "o", "es", "e", "emos", "eis", "em" });
            e[2] = new Endings('i', new string[] { "o", "es", "e", "imos", "is", "em" });
            t = new Tense(Conjugation.Present, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dou", "dás", "dá", "damos", "dais", "dão" }));
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "digo","diz", "dizes", "dizemos", "dizeis", "dizem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estou", "estás", "está", "estamos", "estais", "estão" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "faço", "fazes", "faz", "fazemos", "fazeis", "fazem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "hei", "hás", "há", "havemos", "heis", "hão" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "vou", "vais", "vai", "vamos", "ides", "vão" }));
            t.addIrregularVerb(new IrregularVerb("poder", new string[] { "posso", "podes", "pode", "podemos", "podeis", "podem" }));
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "ponho", "pões", "põe", "pomos", "pondes", "põem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quero", "queres", "quer", "queremos", "quereis", "querem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "sei", "sabes", "sabe", "sabemos", "sabeis", "sabem" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "sou", "és", "é", "somos", "sois", "são" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tenho", "tens", "tem", "temos", "tendes", "têm" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trago", "trazes", "traz", "trazemos", "trazeis", "trazem" }));
            t.addIrregularVerb(new IrregularVerb("ver", new string[] { "vejo", "vês", "vê", "vemos", "vedes", "vêem" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "venho", "vens", "vem", "vimos", "vindes", "vêm" }));

            t.addIrregularVerb(new IrregularVerb("crer", new string[] { "creio", "crês", "crê", "cremos", "credes", "crêem" }));
            t.addIrregularVerb(new IrregularVerb("ler", new string[] { "leio", "lês", "lê", "lemos", "ledes", "lêem" }));
            t.addIrregularVerb(new IrregularVerb("medir", new string[] { "meço", "medes", "mede", "medimos", "medis", "medem" }));
            t.addIrregularVerb(new IrregularVerb("ouvir", new string[] { "ouço", "ouves", "ouve", "ouvimos", "ouvis", "ouvem" }));
            t.addIrregularVerb(new IrregularVerb("pedir", new string[] { "peço", "pedes", "pede", "pedimos", "pedis", "pedem" }));
            t.addIrregularVerb(new IrregularVerb("despedir", new string[] { "despeço", "despedes", "despede", "despedimos", "despedis", "despedem" }));
            t.addIrregularVerb(new IrregularVerb("perder", new string[] { "perco", "perdes", "perde", "perdemos", "perdeis", "perdem" }));
            t.addIrregularVerb(new IrregularVerb("rir", new string[] { "rio", "ris", "ri", "rimos", "rides", "riem" }));
            t.addIrregularVerb(new IrregularVerb("valer", new string[] { "valho", "vales", "vale", "valemos", "valeis", "valem" }));

            t.addIrregularVerb(new IrregularVerb("boiar", new string[] { "bóio", "boias", "bóia", "boiamos", "boiais", "bóiam" }));
            t.addIrregularVerb(new IrregularVerb("odiar", new string[] { "odeio", "odeias", "odeia", "odiamos", "odiais", "odeiam" }));

            t.addIrregularVerb(new IrregularVerb("cear", new string[] { "ceio", "ceias", "ceia", "ceamos", "ceais", "ceiam" }));
            t.addIrregularVerb(new IrregularVerb("chatear", new string[] { "chateio", "chateias", "chateia", "chateamos", "chateais", "chateiam" }));
            t.addIrregularVerb(new IrregularVerb("passear", new string[] { "passeio", "passeias", "passeia", "passeamos", "passeais", "passeiam" }));
            t.addIrregularVerb(new IrregularVerb("recear", new string[] { "receio", "receias", "receia", "receamos", "receais", "receiam" }));
            t.addIrregularVerb(new IrregularVerb("rechear", new string[] { "recheio", "recheias", "recheia", "recheamos", "recheais", "recheiam" }));

            t.addIrregularVerb(new IrregularVerb("erguer", new string[] { "ergo", "ergues", "ergue", "erguemos", "ergueis", "erguem" }));

            t.addIrregularVerb(new IrregularVerb("aderir", new string[] { "adiro", "aderes", "adere", "aderimos", "aderis", "aderem" }));
            t.addIrregularVerb(new IrregularVerb("advertir", new string[] { "advirto", "advertes", "adverte", "advertimos", "advertis", "advertem" }));
            //          competir isn't in this category after all (I think) 
            t.addIrregularVerb(new IrregularVerb("consentir", new string[] { "consinto", "consentes", "consente", "consentimos", "consentis", "consentem" }));
            //          corrigir is misspelled in Sue Tyson-Ward as corregir 
            t.addIrregularVerb(new IrregularVerb("despir", new string[] { "dispo", "despes", "despe", "despimos", "despis", "despem" }));
            t.addIrregularVerb(new IrregularVerb("divertir", new string[] { "divirto", "divertes", "diverte", "divertimos", "divertis", "divertem" }));
            t.addIrregularVerb(new IrregularVerb("dormir", new string[] { "durmo", "dormes", "dorme", "dormimos", "dormis", "dormem" }));
            t.addIrregularVerb(new IrregularVerb("engolir", new string[] { "engulo", "engoles", "engole", "engolimos", "engolis", "engolem" }));
            t.addIrregularVerb(new IrregularVerb("ferir", new string[] { "firo", "feres", "fere", "ferimos", "feris", "ferem" }));
            t.addIrregularVerb(new IrregularVerb("investir", new string[] { "invisto", "investes", "investe", "investimos", "investis", "investem" }));
            t.addIrregularVerb(new IrregularVerb("mentir", new string[] { "minto", "mentes", "mente", "mentimos", "mentis", "mentem" }));
            t.addIrregularVerb(new IrregularVerb("preferir", new string[] { "prefiro", "preferes", "prefere", "preferimos", "preferis", "preferem" }));
            t.addIrregularVerb(new IrregularVerb("pressentir", new string[] { "pressinto", "pressentes", "pressente", "pressentimos", "pressentis", "pressentem" }));
            t.addIrregularVerb(new IrregularVerb("referir", new string[] { "refiro", "referes", "refere", "referimos", "referis", "referem" }));
            t.addIrregularVerb(new IrregularVerb("repetir", new string[] { "repito", "repetes", "repete", "repetimos", "repetis", "repetem" }));
            t.addIrregularVerb(new IrregularVerb("sentir", new string[] { "sinto", "sentes", "sente", "sentimos", "sentis", "sentem" }));
            t.addIrregularVerb(new IrregularVerb("servir", new string[] { "sirvo", "serves", "serve", "servimos", "servis", "servem" }));
            t.addIrregularVerb(new IrregularVerb("sugerir", new string[] { "sugiro", "sugeres", "sugere", "sugerimos", "sugeris", "sugerem" }));
            t.addIrregularVerb(new IrregularVerb("transferir", new string[] { "transfiro", "transferes", "transfere", "transferimos", "transferis", "transferem" }));
            t.addIrregularVerb(new IrregularVerb("vestir", new string[] { "visto", "vestes", "veste", "vestimos", "vestis", "vestem" }));

            t.addIrregularVerb(new IrregularVerb("cobrir", new string[] { "cubro", "cobres", "cobre", "cobrimos", "cobris", "cobrem" }));
            t.addIrregularVerb(new IrregularVerb("descobrir", new string[] { "descubro", "descobres", "descobre", "descobrimos", "descobris", "descobrem" }));

            t.addIrregularVerb(new IrregularVerb("conseguir", new string[] { "consigo", "consegues", "consegue", "conseguimos", "conseguis", "conseguem" }));
            t.addIrregularVerb(new IrregularVerb("seguir", new string[] { "sigo", "segues", "segue", "seguimos", "seguis", "seguem" }));

            t.addIrregularVerb(new IrregularVerb("acudir", new string[] { "acudo", "acodes", "acode", "acudimos", "acudis", "acodem" }));
            t.addIrregularVerb(new IrregularVerb("cuspir", new string[] { "cuspo", "cospes", "cospe", "cuspimos", "cuspis", "cospem" }));
            t.addIrregularVerb(new IrregularVerb("subir", new string[] { "subo", "sobes", "sobe", "subimos", "subis", "sobem" }));
            t.addIrregularVerb(new IrregularVerb("sacudir", new string[] { "sacudo", "sacodes", "sacode", "sacudimos", "sacudis", "sacodem" }));

            // Conjugation.Preterite
            e[0] = new Endings('a', new string[] { "ei", "aste", "ou", "amos", "astes", "aram" });
            e[1] = new Endings('e', new string[] { "i",  "este", "eu", "emos", "estes", "eram" });
            e[2] = new Endings('i', new string[] { "i",  "iste", "iu", "imos", "istes", "iram" });
            t = new Tense(Conjugation.Preterite, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dei", "deste", "deu", "demos", "destes", "deram" }));
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "disse", "disseste", "disse", "dissemos", "dissestes", "disseram" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estive", "estiveste", "esteve", "estivemos", "estivestes", "estiveram" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "fiz", "fizeste", "fez", "fizemos", "fizestes", "fizeram" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houve", "houveste", "houve", "houvemos", "houvestes", "houveram" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "fui", "foste", "foi", "fomos", "fostes", "foram" }));
            t.addIrregularVerb(new IrregularVerb("poder", new string[] { "pude", "pudeste", "pôde", "pudemos", "pudestes", "puderam" }));
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "pus", "puseste", "pôs", "pusemos", "pusestes", "puseram" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quis", "quiseste", "quis", "quisemos", "quisestes", "quiseram" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "soube", "soubeste", "soube", "soubemos", "soubestes", "souberam" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "fui", "foste", "foi", "fomos", "fostes", "foram" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tive", "tiveste", "teve", "tivemos", "tivestes", "tiveram" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trouxe", "trouxeste", "trouxe", "trouxemos", "trouxestes", "trouxeram" }));
            t.addIrregularVerb(new IrregularVerb("ver", new string[] { "vi", "viste", "viu", "vimos", "vistes", "viram" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "vim","vieste", "veio", "viemos", "viestes", "vieram" }));

            // Conjugation.Imperfect
            e[0] = new Endings('a', new string[] { "ava", "avas", "ava", "ávamos", "áveis", "avam" });
            e[1] = new Endings('e', new string[] { "ia",  "ias", "ia", "íamos", "íeis", "iam" });
            e[2] = new Endings('i', new string[] { "ia", "ias", "ia", "íamos", "íeis", "iam" });
            t = new Tense(Conjugation.Imperfect, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "punha", "punhas", "punha", "púnhamos", "púnheis", "punham" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "era", "eras", "era", "éramos", "éreis", "eram" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tinha", "tinhas", "tinha", "tínhamos", "tínheis", "tinham" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "vinha", "vinhas", "vinha", "vínhamos", "vínheis", "vinham" }));

            // Conjugation.Future
            e[0] = new Endings('a', new string[] { "arei", "arás", "ará", "aremos", "areis", "arão" });
            e[1] = new Endings('e', new string[] { "erei", "erás", "erá", "eremos", "ereis", "erão" });
            e[2] = new Endings('i', new string[] { "irei", "irás", "irá", "iremos", "ireis", "irão" });
            t = new Tense(Conjugation.Future, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "farei", "farás", "fará", "faremos", "fareis", "farão" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trarei", "trarás", "trará", "traremos", "trareis", "trarão" }));

            // Conjugation.Conditional
            e[0] = new Endings('a', new string[] { "aria", "arias", "aria", "aríamos", "aríeis", "ariam" });
            e[1] = new Endings('e', new string[] { "eria", "erias", "eria", "eríamos", "eríeis", "eriam" });
            e[2] = new Endings('i', new string[] { "iria", "irias", "iria", "iríamos", "iríeis", "iriam" });
            t = new Tense(Conjugation.Conditional, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "diria", "dirias", "diria", "diríamos", "diríeis", "diriam" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "faria", "farias", "faria", "faríamos", "faríeis", "fariam" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "traria", "trarias", "traria", "traríamos", "traríeis", "trariam" }));

            // Conjugation.SubjunctivePresent
            e[0] = new Endings('a', new string[] { "e", "es", "e", "emos", "eis", "em" });
            e[1] = new Endings('e', new string[] { "a", "as", "a", "amos", "ais", "am" });
            e[2] = new Endings('i', new string[] { "a", "as", "a", "amos", "ais", "am" });
            t = new Tense(Conjugation.SubjunctivePresent, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "seja", "sejas", "seja", "sejamos", "sejais", "sejam" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "esteja", "estejas", "esteja", "estejamos", "estejais", "estejam" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "haja", "hajas", "haja", "hajamos", "hajais", "hajam" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "saiba", "saiba", "saiba", "saibamos", "saibais", "saibam" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "queira", "queiras", "queira", "queiramos", "queirais", "queiram" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dê", "dê", "dê", "dêmos", "deis", "dêem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "vá", "vás", "vá", "vamos", "vades", "vão" }));

            // Conjugation.SubjunctiveImperfect
            e[0] = new Endings('a', new string[] { "asse", "asses", "asse", "ássemos", "ásseis", "assem" });
            e[1] = new Endings('e', new string[] { "esse", "esse", "esse", "êssemos", "êsseis", "essem" });
            e[2] = new Endings('i', new string[] { "isse", "isse", "isse", "íssemos", "ísseis", "issem" });
            t = new Tense(Conjugation.SubjunctiveImperfect, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "fosse", "fosses", "fosse", "fôssemos", "fôsseis", "fossem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estivesse", "estivesses", "estivesse", "estivéssemos", "estivésseis", "estivessem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houvesse", "houvesses", "houvesse", "houvéssemos", "houvésseis", "houvessem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "soubesse", "soubesses", "soubesse", "soubéssemos", "soubésseis", "soubessem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quisesse", "quisesses", "quisesse", "quiséssemos", "quisésseis", "quisessem" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "desse", "desses", "desse", "déssemos", "désseis", "dessem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "fosse", "fosses", "fosse", "fôssemos", "fôsseis", "fossem" }));

            // Conjugation.SubjunctiveFuture
            e[0] = new Endings('a', new string[] { "ar", "ares", "ar", "armos", "ardes", "arem" });
            e[1] = new Endings('e', new string[] { "er", "eres", "er", "ermos", "erdes", "erem" });
            e[2] = new Endings('i', new string[] { "ir", "ires", "ir", "irmos", "irdes", "irem" });
            t = new Tense(Conjugation.SubjunctiveFuture, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "for", "fores", "for", "formos", "fordes", "forem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estiver", "estiveres", "estiver", "estivermos", "estiverdeshaver", "estiverem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houver", "houveres", "houver", "houvermos", "houverdes", "houverem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "souber", "souberes", "souber", "soubermos", "souberdes", "souberem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quiser", "quiseres", "quiser", "quisermos", "quiserdes", "quiserem" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "der", "deres", "der", "dermos", "derdes", "derem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "for", "fores", "for", "formos", "fordes", "forem" }));
            infinitives = readSeedData();
        }

        static public void Main(string[] args)
        {

            StaticConstructor(args);
            Verbiage verbiage;
            string infinitive;
            // check Form1 constructor for initialization of this variable when 
            // WinForm version is run
            Conjugation conjugation = Conjugation.Preterite;

            while (0 == 0) // hehehe... "while 1 do" ! 
            {
                verbiage = GetAConjugation(conjugation);
                if (verbiage.IsIrregular)
                {
                    Console.WriteLine("Irregular: " + verbiage.Infinitive);
                }
                Console.WriteLine("verb: " + verbiage.Infinitive);
                Console.Write(verbiage.Pronoun + " ");
                Console.ReadLine();


                Console.WriteLine(verbiage.Pronoun + " " + verbiage.ConjugatedVerb);
                Console.WriteLine();

                bool showTables = false;
                if (showTables)
                {
                    infinitive = verbiage.Infinitive;
                    conjugation = verbiage.Conjugation;

                    PersonalPronoun pp;
                    pp = personalPronouns.getRandomPersonalPronoun(Person.firstPersonSingular);
                    Console.WriteLine(("").PadLeft(40) + pp.pronoun.PadRight(8) + conjugateVerb(infinitive, pp, conjugation));
                    pp = personalPronouns.getRandomPersonalPronoun(Person.thirdPersonSingular);
                    Console.WriteLine(("").PadLeft(40) + pp.pronoun.PadRight(8) + conjugateVerb(infinitive, pp, conjugation));
                    pp = personalPronouns.getRandomPersonalPronoun(Person.firstPersonPlural);
                    Console.WriteLine(("").PadLeft(40) + pp.pronoun.PadRight(8) + conjugateVerb(infinitive, pp, conjugation));
                    pp = personalPronouns.getRandomPersonalPronoun(Person.thirdPersonPlural);
                    Console.WriteLine(("").PadLeft(40) + pp.pronoun.PadRight(8) + conjugateVerb(infinitive, pp, conjugation));

                    Console.WriteLine();
                    foreach (Conjugation item in Enum.GetValues(typeof(Conjugation)))
                    {
                        Console.WriteLine(("").PadLeft(40) + (item.ToString() + ":").PadRight(15) + verbiage.Pronoun.pronoun + " " + conjugateVerb(infinitive, verbiage.Pronoun, item));
                    }
                }

#if (SENTENCES)
                int occurenceOffset;
                if (sentences == null)
                {
                    sentences = readSentences();
                }
                foreach (string sentence in sentences)
                {
                    occurenceOffset = sentence.IndexOf(conjugatedVerb);

                    if (occurenceOffset > 0)
                    {
                        string[] foundWords = sentence.Substring(occurenceOffset).Split(new char[4] { ' ', '.', '?', ',' });
                        string foundWord = foundWords[0]; // for the debugger
                        string[] wholeWords = sentence.Split(new char[4] { ' ', '.', '?', ',' });
                        foreach (string word in wholeWords)
                        {
                            if (word == conjugatedVerb)
                            {
                                Console.WriteLine(sentence + ".");
                                Console.WriteLine();
                            }
                        }

                    }
                }
#endif
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}