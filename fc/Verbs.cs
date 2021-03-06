﻿using System;
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
        //secondPersonSingular, // fixup irregular structure before adding in 
        thirdPersonSingular,
        firstPersonPlural,
        //secondPersonPlural,   // fixup irregular structure before adding in 
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
            m_pronounList.Add(new PersonalPronoun("você", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("ele", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("ela", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("a gente", Person.thirdPersonSingular));
            m_pronounList.Add(new PersonalPronoun("nós", Person.firstPersonPlural));
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
            m_conjugations.Add(new IrregularConjugation(Person.thirdPersonSingular, conjugations[(int)Person.thirdPersonSingular]));
            m_conjugations.Add(new IrregularConjugation(Person.firstPersonPlural, conjugations[(int)Person.firstPersonPlural]));
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
,"cair"
,"sair"
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
            SW.WriteLine((status + " " + ve.Infinitive + ", " + ve.Pronoun.ToString() + " " + ve.ConjugatedVerb + ", " + ve.Conjugation.ToString()).PadRight(50) + " " + DateTime.Now);
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

                if (S.IndexOf(@"[begin]") != -1)
                {
                    // empty it brutally
                    fileStrings = new List<string>();
                    continue;
                }

                if (S.IndexOf(@"[end]") != -1)
                {
                    // finish off. We only wanted the section
                    break;
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

        static string ThirdPersonPluralPreteriteModel(string infinitive)
        {
            // handy for the formation of the Subjunctives Imperfect and Future
            var pronoun = new PersonalPronoun("eles", Person.thirdPersonPlural);
            string stem = conjugateVerb(infinitive, pronoun, Conjugation.Preterite);
            stem = stem.Substring(0, stem.Length - 2);

            return stem;
        }

        static string FirstPersonIndicativeStem(string infinitive)
        {
            // handy for the formation of the Subjuntive Present
            var pronoun = new PersonalPronoun("eu", Person.firstPersonSingular);
            string stem = conjugateVerb(infinitive, pronoun, Conjugation.Present);
            stem = stem.Substring(0, stem.Length - 1);

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
            string conjugatedVerb;
            string newStem;
            string conjugatedEnding = null;

            // set up defaults
            string model = infinitive;
            string stem = model.Substring(0, model.Length - 2); // may get overwritten

            switch (conjugation)
            {
                case Conjugation.SubjunctivePresent:
                    // the "tenho" case
                    // keep the model, get the stem
                    stem = FirstPersonIndicativeStem(infinitive);
                    break;
                case Conjugation.SubjunctiveFuture:
                case Conjugation.SubjunctiveImperfect:
                    // the "comeram" case again
                    // suggest a whole new model and create a stem for it
                    model = ThirdPersonPluralPreteriteModel(infinitive);
                    stem = model.Substring(0, model.Length - 2);
                    break;
            }

            char ending = model[model.Length - 2];

            Tense t = Tenses.Find(o => o.conjugation == conjugation);
            Endings e = t.endings.Find(o => o.letter == ending);
            string[] endings = e.endings;

            switch (pronoun.person)
            {
                case Person.firstPersonSingular:
                    conjugatedEnding = endings[0];
                    break;
                case Person.thirdPersonSingular:
                    conjugatedEnding = endings[1];
                    break;
                case Person.firstPersonPlural:
                    conjugatedEnding = endings[2];
                    break;
                case Person.thirdPersonPlural:
                    conjugatedEnding = endings[3];
                    break;
            }

            switch (conjugation)
            {
                case Conjugation.SubjunctivePresent:
                    if (!isIrregular(infinitive, Conjugation.Present))
                        stem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
                case Conjugation.SubjunctiveFuture:
                case Conjugation.SubjunctiveImperfect:
                    if (!isIrregular(infinitive, Conjugation.Preterite))
                        stem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
                default:
                    stem = orthoChanging2(infinitive, conjugatedEnding);
                    break;
            }



            conjugatedVerb = stem + conjugatedEnding;
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
            e[0] = new Endings('a', new string[] { "o", "a", "amos", "am" });
            e[1] = new Endings('e', new string[] { "o", "e", "emos", "em" });
            e[2] = new Endings('i', new string[] { "o", "e", "imos", "em" });
            t = new Tense(Conjugation.Present, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dou", "dá", "damos", "dão" }));
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "digo", "diz", "dizemos", "dizem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estou", "está", "estamos", "estão" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "faço", "faz", "fazemos", "fazem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "hei", "há", "havemos", "hão" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "vou", "vai", "vamos", "vão" }));
            t.addIrregularVerb(new IrregularVerb("poder", new string[] { "posso", "pode", "podemos", "podem" }));
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "ponho", "põe", "pomos", "põem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quero", "quer", "queremos", "querem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "sei", "sabe", "sabemos", "sabem" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "sou", "é", "somos", "são" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tenho", "tem", "temos", "têm" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trago", "traz", "trazemos", "trazem" }));
            t.addIrregularVerb(new IrregularVerb("ver", new string[] { "vejo", "vê", "vemos", "vêem" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "venho", "vem", "vimos", "vêm" }));

            t.addIrregularVerb(new IrregularVerb("crer", new string[] { "creio", "crê", "cremos", "crêem" }));
            t.addIrregularVerb(new IrregularVerb("ler", new string[] { "leio", "lê", "lemos", "lêem" }));
            t.addIrregularVerb(new IrregularVerb("medir", new string[] { "meço", "mede", "medimos", "medem" }));
            t.addIrregularVerb(new IrregularVerb("ouvir", new string[] { "ouço", "ouve", "ouvimos", "ouvem" }));
            t.addIrregularVerb(new IrregularVerb("pedir", new string[] { "peço", "pede", "pedimos", "pedem" }));
            t.addIrregularVerb(new IrregularVerb("despedir", new string[] { "despeço", "despede", "despedimos", "despedem" }));
            t.addIrregularVerb(new IrregularVerb("perder", new string[] { "perco", "perde", "perdemos", "perdem" }));
            t.addIrregularVerb(new IrregularVerb("rir", new string[] { "rio", "ri", "rimos", "riem" }));
            t.addIrregularVerb(new IrregularVerb("valer", new string[] { "valho", "vale", "valemos", "valem" }));

            t.addIrregularVerb(new IrregularVerb("boiar", new string[] { "bóio", "bóia", "boiamos", "bóiam" }));
            t.addIrregularVerb(new IrregularVerb("odiar", new string[] { "odeio", "odeia", "odiamos", "odeiam" }));

            t.addIrregularVerb(new IrregularVerb("cear", new string[] { "ceio", "ceia", "ceamos", "ceiam" }));
            t.addIrregularVerb(new IrregularVerb("chatear", new string[] { "chateio", "chateia", "chateamos", "chateiam" }));
            t.addIrregularVerb(new IrregularVerb("passear", new string[] { "passeio", "passeia", "passeamos", "passeiam" }));
            t.addIrregularVerb(new IrregularVerb("recear", new string[] { "receio", "receia", "receamos", "receiam" }));
            t.addIrregularVerb(new IrregularVerb("rechear", new string[] { "recheio", "recheia", "recheamos", "recheiam" }));

            t.addIrregularVerb(new IrregularVerb("erguer", new string[] { "ergo", "ergue", "erguemos", "erguem" }));

            t.addIrregularVerb(new IrregularVerb("aderir", new string[] { "adiro", "adere", "aderimos", "aderem" }));
            t.addIrregularVerb(new IrregularVerb("advertir", new string[] { "advirto", "adverte", "advertimos", "advertem" }));
            //          competir isn't in this category after all (I think) 
            t.addIrregularVerb(new IrregularVerb("consentir", new string[] { "consinto", "consente", "consentimos", "consentem" }));
            //          corrigir is misspelled in Sue Tyson-Ward as corregir 
            t.addIrregularVerb(new IrregularVerb("despir", new string[] { "dispo", "despe", "despimos", "despem" }));
            t.addIrregularVerb(new IrregularVerb("divertir", new string[] { "divirto", "diverte", "divertimos", "divertem" }));
            t.addIrregularVerb(new IrregularVerb("dormir", new string[] { "durmo", "dorme", "dormimos", "dormem" }));
            t.addIrregularVerb(new IrregularVerb("engolir", new string[] { "engulo", "engole", "engolimos", "engolem" }));
            t.addIrregularVerb(new IrregularVerb("ferir", new string[] { "firo", "fere", "ferimos", "ferem" }));
            t.addIrregularVerb(new IrregularVerb("investir", new string[] { "invisto", "investe", "investimos", "investem" }));
            t.addIrregularVerb(new IrregularVerb("mentir", new string[] { "minto", "mente", "mentimos", "mentem" }));
            t.addIrregularVerb(new IrregularVerb("preferir", new string[] { "prefiro", "prefere", "preferimos", "preferem" }));
            t.addIrregularVerb(new IrregularVerb("pressentir", new string[] { "pressinto", "pressente", "pressentimos", "pressentem" }));
            t.addIrregularVerb(new IrregularVerb("referir", new string[] { "refiro", "refere", "referimos", "referem" }));
            t.addIrregularVerb(new IrregularVerb("repetir", new string[] { "repito", "repete", "repetimos", "repetem" }));
            t.addIrregularVerb(new IrregularVerb("sentir", new string[] { "sinto", "sente", "sentimos", "sentem" }));
            t.addIrregularVerb(new IrregularVerb("servir", new string[] { "sirvo", "serve", "servimos", "servem" }));
            t.addIrregularVerb(new IrregularVerb("sugerir", new string[] { "sugiro", "sugere", "sugerimos", "sugerem" }));
            t.addIrregularVerb(new IrregularVerb("transferir", new string[] { "transfiro", "transfere", "transferimos", "transferem" }));
            t.addIrregularVerb(new IrregularVerb("vestir", new string[] { "visto", "veste", "vestimos", "vestem" }));

            t.addIrregularVerb(new IrregularVerb("cobrir", new string[] { "cubro", "cobre", "cobrimos", "cobrem" }));
            t.addIrregularVerb(new IrregularVerb("descobrir", new string[] { "descubro", "descobre", "descobrimos", "descobrem" }));

            t.addIrregularVerb(new IrregularVerb("conseguir", new string[] { "consigo", "consegue", "conseguimos", "conseguem" }));
            t.addIrregularVerb(new IrregularVerb("seguir", new string[] { "sigo", "segue", "seguimos", "seguem" }));

            t.addIrregularVerb(new IrregularVerb("acudir", new string[] { "acudo", "acode", "acudimos", "acodem" }));
            t.addIrregularVerb(new IrregularVerb("cuspir", new string[] { "cuspo", "cospe", "cuspimos", "cospem" }));
            t.addIrregularVerb(new IrregularVerb("subir", new string[] { "subo", "sobe", "subimos", "sobem" }));
            t.addIrregularVerb(new IrregularVerb("sacudir", new string[] { "sacudo", "sacode", "sacudimos", "sacodem" }));

            t.addIrregularVerb(new IrregularVerb("cair", new string[] { "caio", "cai", "caímos", "caem" }));
            t.addIrregularVerb(new IrregularVerb("sair", new string[] { "saio", "sai", "saímos", "saem" }));

            // Conjugation.Preterite
            e[0] = new Endings('a', new string[] { "ei", "ou", "amos", "aram" });
            e[1] = new Endings('e', new string[] { "i", "eu", "emos", "eram" });
            e[2] = new Endings('i', new string[] { "i", "iu", "imos", "iram" });
            t = new Tense(Conjugation.Preterite, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("cair", new string[] { "caí", "caiu", "caímos", "caíram" }));
            t.addIrregularVerb(new IrregularVerb("sair", new string[] { "saí", "saiu", "saímos", "saíram" }));

            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dei", "deu", "demos", "deram" }));
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "disse", "disse", "dissemos", "disseram" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estive", "esteve", "estivemos", "estiveram" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "fiz", "fez", "fizemos", "fizeram" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houve", "houve", "houvemos", "houveram" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "fui", "foi", "fomos", "foram" }));
            t.addIrregularVerb(new IrregularVerb("poder", new string[] { "pude", "pôde", "pudemos", "puderam" }));
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "pus", "pôs", "pusemos", "puseram" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quis", "quis", "quisemos", "quiseram" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "soube", "soube", "soubemos", "souberam" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "fui", "foi", "fomos", "foram" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tive", "teve", "tivemos", "tiveram" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trouxe", "trouxe", "trouxemos", "trouxeram" }));
            t.addIrregularVerb(new IrregularVerb("ver", new string[] { "vi", "viu", "vimos", "viram" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "vim", "veio", "viemos", "vieram" }));

            // Conjugation.Imperfect
            e[0] = new Endings('a', new string[] { "ava", "ava", "ávamos", "avam" });
            e[1] = new Endings('e', new string[] { "ia", "ia", "íamos", "iam" });
            e[2] = new Endings('i', new string[] { "ia", "ia", "íamos", "iam" });
            t = new Tense(Conjugation.Imperfect, e);
            Tenses.Add(t);

            t.addIrregularVerb(new IrregularVerb("cair", new string[] { "caía", "caía", "caíamos", "caíam" }));
            t.addIrregularVerb(new IrregularVerb("sair", new string[] { "saía", "saía", "saíamos", "saíam" }));

            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "punha", "punha", "púnhamos", "punham" }));
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "era", "era", "éramos", "eram" }));
            t.addIrregularVerb(new IrregularVerb("ter", new string[] { "tinha", "tinha", "tínhamos", "tinham" }));
            t.addIrregularVerb(new IrregularVerb("vir", new string[] { "vinha", "vinha", "vínhamos", "vinham" }));

            // Conjugation.Future
            e[0] = new Endings('a', new string[] { "arei", "ará", "aremos", "arão" });
            e[1] = new Endings('e', new string[] { "erei", "erá", "eremos", "erão" });
            e[2] = new Endings('i', new string[] { "irei", "irá", "iremos", "irão" });
            t = new Tense(Conjugation.Future, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "farei", "fará", "faremos", "farão" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "trarei", "trará", "traremos", "trarão" }));
            // "unofficial" irregular -- breaks when processed in regular verb conjugation routine
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "porei", "porá", "poremos", "porão" }));

            // Conjugation.Conditional
            e[0] = new Endings('a', new string[] { "aria", "aria", "aríamos", "ariam" });
            e[1] = new Endings('e', new string[] { "eria", "eria", "eríamos", "eriam" });
            e[2] = new Endings('i', new string[] { "iria", "iria", "iríamos", "iriam" });
            t = new Tense(Conjugation.Conditional, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("dizer", new string[] { "diria", "diria", "diríamos", "diriam" }));
            t.addIrregularVerb(new IrregularVerb("fazer", new string[] { "faria", "faria", "faríamos", "fariam" }));
            t.addIrregularVerb(new IrregularVerb("trazer", new string[] { "traria", "traria", "traríamos", "trariam" }));
            // "unofficial" irregular -- breaks when processed in regular verb conjugation routine
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "poria", "poria", "poríamos", "poriam" }));

            // Conjugation.SubjunctivePresent
            e[0] = new Endings('a', new string[] { "e", "e", "emos", "em" });
            e[1] = new Endings('e', new string[] { "a", "a", "amos", "am" });
            e[2] = new Endings('i', new string[] { "a", "a", "amos", "am" });
            t = new Tense(Conjugation.SubjunctivePresent, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "seja", "seja", "sejamos", "sejam" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "esteja", "esteja", "estejamos", "estejam" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "haja", "haja", "hajamos", "hajam" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "saiba", "saiba", "saibamos", "saibam" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "queira", "queira", "queiramos", "queiram" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dê", "dê", "dêmos", "dêem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "vá", "vá", "vamos", "vão" }));
            // "unofficial" irregular -- breaks when processed in regular verb conjugation routine
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "ponha", "ponha", "ponhamos", "ponham" }));

            // Conjugation.SubjunctiveImperfect
            e[0] = new Endings('a', new string[] { "asse", "asse", "ássemos", "assem" });
            e[1] = new Endings('e', new string[] { "esse", "esse", "êssemos", "essem" });
            e[2] = new Endings('i', new string[] { "isse", "isse", "íssemos", "issem" });
            t = new Tense(Conjugation.SubjunctiveImperfect, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "fosse", "fosse", "fôssemos", "fossem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estivesse", "estivesse", "estivéssemos", "estivessem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houvesse", "houvesse", "houvéssemos", "houvessem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "soubesse", "soubesse", "soubéssemos", "soubessem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quisesse", "quisesse", "quiséssemos", "quisessem" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "desse", "desse", "déssemos", "dessem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "fosse", "fosse", "fôssemos", "fossem" }));
            // "unofficial" irregular -- breaks when processed in regular verb conjugation routine
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "pusesse", "pusesse", "puséssemos", "pusessem" }));
            t.addIrregularVerb(new IrregularVerb("cair", new string[] { "caísse", "caísse", "caíssemos", "caíssem" }));
            t.addIrregularVerb(new IrregularVerb("sair", new string[] { "saísse", "saísse", "saíssemos", "saíssem" }));

            // Conjugation.SubjunctiveFuture
            e[0] = new Endings('a', new string[] { "ar", "ar", "armos", "arem" });
            e[1] = new Endings('e', new string[] { "er", "er", "ermos", "erem" });
            e[2] = new Endings('i', new string[] { "ir", "ir", "irmos", "irem" });
            t = new Tense(Conjugation.SubjunctiveFuture, e);
            Tenses.Add(t);
            t.addIrregularVerb(new IrregularVerb("ser", new string[] { "for", "for", "formos", "forem" }));
            t.addIrregularVerb(new IrregularVerb("estar", new string[] { "estiver", "estiver", "estivermos", "estiverem" }));
            t.addIrregularVerb(new IrregularVerb("haver", new string[] { "houver", "houver", "houvermos", "houverem" }));
            t.addIrregularVerb(new IrregularVerb("saber", new string[] { "souber", "souber", "soubermos", "souberem" }));
            t.addIrregularVerb(new IrregularVerb("querer", new string[] { "quiser", "quiser", "quisermos", "quiserem" }));
            t.addIrregularVerb(new IrregularVerb("dar", new string[] { "dar", "dar", "darmos", "darem" }));
            t.addIrregularVerb(new IrregularVerb("ir", new string[] { "for", "for", "formos", "forem" }));
            // "unofficial" irregular -- breaks when processed in regular verb conjugation routine
            t.addIrregularVerb(new IrregularVerb("pôr", new string[] { "puser", "puser", "pusermos", "puserem" }));
            t.addIrregularVerb(new IrregularVerb("cair", new string[] { "caír", "caír", "caírmos", "caírem" }));
            t.addIrregularVerb(new IrregularVerb("sair", new string[] { "saír", "saír", "saírmos", "saírem" }));


            infinitives = readSeedData();
        }

        static private Conjugation primaryConjugation = Conjugation.SubjunctiveImperfect;
        static public Conjugation PrimaryConjugation
        {
            get { return primaryConjugation; }
            set { primaryConjugation = value; }
        }

        public static void SubjunctiveVisualCheck()
        {
            // For console output, set the code page to 1252
            // c:\> chcp 1252
            Conjugation[] conjugations = (Conjugation[])Enum.GetValues(typeof(Conjugation));
            Person[] persons = (Person[])Enum.GetValues(typeof(Person));

            foreach (var infinitive in infinitives)
            {
                System.Console.WriteLine(infinitive);
                foreach (var conjugation in conjugations)
                {
                    switch (conjugation)
                    {
                        case Conjugation.Present:
                            System.Console.WriteLine("  {0}", conjugation);
                            System.Console.WriteLine("     {0}", conjugateVerb(infinitive, new PersonalPronoun("", Person.firstPersonSingular), conjugation));
                            break;
                        case Conjugation.Preterite:
                            System.Console.WriteLine("  {0}", conjugation);
                            System.Console.WriteLine("     {0}", conjugateVerb(infinitive, new PersonalPronoun("", Person.thirdPersonPlural), conjugation));
                            break;
                        case Conjugation.SubjunctivePresent:
                        case Conjugation.SubjunctiveFuture:
                        case Conjugation.SubjunctiveImperfect:
                            System.Console.WriteLine("  {0}", conjugation);
                            foreach (var person in persons)
                            {
                                System.Console.WriteLine("     {0}", conjugateVerb(infinitive, new PersonalPronoun("", person), conjugation));
                            }
                            break;
                    }
                }
            }

        }

        public static void GetEmAll()
        {
            // For console output, set the code page to 1252
            // c:\> chcp 1252
            Conjugation[] conjugations = (Conjugation[])Enum.GetValues(typeof(Conjugation));
            Person[] persons = (Person[])Enum.GetValues(typeof(Person));

            foreach (var infinitive in infinitives)
            {
                System.Console.WriteLine(infinitive);
                foreach (var conjugation in conjugations)
                {
                    System.Console.WriteLine("  {0}", conjugation);
                    foreach (var person in persons)
                    {
                        PersonalPronoun pronoun = new PersonalPronoun("", person); // hope for the best! 
                        System.Console.WriteLine("     {0}", conjugateVerb(infinitive, pronoun, conjugation));
                    }
                }
            }

        }


        static public void Main(string[] args)
        {

            StaticConstructor(args);
            GetEmAll();
            // SubjunctiveVisualCheck(); 
        }

        static public void ArchiveMain(string[] args)
        {

            StaticConstructor(args);

            Verbiage verbiage;
            string infinitive;
            // check Form1 constructor for initialization of this variable when 
            // WinForm version is run
            Conjugation conjugation = Verbs.PrimaryConjugation;

            while (true)
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

                const bool showTables = false;
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
