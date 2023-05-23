using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;


namespace DictCC
{
    class Program
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
            neuter
        }

        public partial class NounEntry
        {
            public Gender gender;
            public GrammaticalNumber number;
            public string lemma;
            public string plural;
            public string thefuckisit;

            public NounEntry()
            {
            }
            public NounEntry(NounEntry ve)
            {
                this.gender = ve.gender;
                this.number = ve.number;
                this.lemma = ve.lemma;
                this.plural = ve.plural;
                this.thefuckisit = ve.thefuckisit;
            }
        };

        static List<NounEntry> LookforAllTheNouns(List<NounEntry> nouns)
        {
            // var list = nouns.Where(i => i.lemma.EndsWith("ik") && i.gender.Equals(Gender.feminine));
            // var list = nouns.Where(i => i.lemma.StartsWith("Ge") && i.lemma.EndsWith("e")).ToList();
            var list = nouns.ToList();

            return list; 
        }
        static List<NounEntry> ProcessLinesGermanNouns(List<string> lines)
        {
            var pluralUtilityInputs = new List<NounEntry>();

            var headers = lines.First().Split(',');


            foreach (var line in lines.Skip(1))
            {

                var noun = new NounEntry();
                var splits = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                noun.lemma = splits[0];
                noun.thefuckisit = splits[1];
                noun.plural = splits[16];
                switch (splits[2])
                {
                    case "m":
                        noun.gender = Gender.masculine;
                        break;
                    case "f":
                        noun.gender = Gender.feminine;
                        break;
                    case "n":
                        noun.gender = Gender.neuter;
                        break;
                }

                pluralUtilityInputs.Add(noun);
            }


            return pluralUtilityInputs;
        }
        static List<string> ProcessLinesDictCC( List<string> lines )
        {
            var femNouns = lines.Where(s => s.Contains("{f}")).ToList(); ;

            var pluralUtilityInputs = new List<string>();

            string noun = "";
            foreach (var femNoun in femNouns)
            {
                var splits = femNoun.Split(' ');
                for (var i = 0; i < splits.Length; i++)
                {
                    if (splits[i].StartsWith("{f}"))
                    {
                        noun = splits[i - 1];
                        noun = Regex.Replace(noun, @"^\(.*\)", "");

                        // MainMain("der", "Schlüssel");
                        noun = "MainMain(\"die\", \"" + noun + "\");";
                        pluralUtilityInputs.Add( noun );
                    }
                }

            }
            
            return pluralUtilityInputs;
        }

        static List<string> ReadDictCC()
        {
            var lines = File.ReadAllLines(@"C:\Users\doug.hoople\source\repos\fc-master\fc\dict.cc\cdbosfbbck-20289143196-77i66e.txt");

            var result = new List<string>();
            foreach (var line in lines)
                result.Add(line);
            return result;
        }
        static List<string> ReadGermanNouns()
        {
            var lines = File.ReadAllLines(@"C:\Users\doug.hoople\source\repos\fc-master\fc\german-nouns\nouns.csv");

            var result = new List<string>();
            foreach (var line in lines)
                result.Add(line);
            return result;
        }

        static void Main(string[] args)
        {
            // var lines = ReadDictCC();
            var lines = ReadGermanNouns();
            // lines = ProcessLinesDictCC(lines);
            var nouns = ProcessLinesGermanNouns(lines);
            var filteredNouns = LookforAllTheNouns(nouns);

            lines = new List<string>();
            foreach (var noun in filteredNouns)
            {
                string genderString = "";
                switch (noun.gender)
                {
                    case Gender.masculine:
                        genderString = "der";
                        break;
                    case Gender.feminine:
                        genderString = "die";
                        break;
                    case Gender.neuter:
                        genderString = "das";
                        break;
                }

                lines.Add($"{genderString} {noun.lemma} {noun.plural}");
            }

            // File.WriteAllLines(@"C:\Users\doug.hoople\source\repos\fc-master\fc\dict.cc\lines", lines);
            // File.WriteAllLines(@"C:\Users\doug.hoople\source\repos\fc-master\fc\dict.cc\lines", lines);
            File.WriteAllLines(@"C:\Users\doug.hoople\source\repos\fc-master\fc\german-nouns\lines", lines);

        }
    }
}
