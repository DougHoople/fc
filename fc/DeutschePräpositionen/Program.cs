using System;
using System.Collections;
using System.Collections.Generic;

namespace DeutscheMehrzahl
{
    public enum Gender
    {
        masculine,
        feminine,
        neuter
    }
    public class Word : IEnumerable 
    { 

        public Gender gender { get; set; }
        public string singular { get; set; }
        public string plural { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public static class Words
    {
        public static Word [] words = new Word []
            {
                new Word() { gender = Gender.neuter , singular = "Frau", plural = "Frauen" },
                new Word() { gender = Gender.neuter , singular = "Kind", plural = "Kinder" },
                new Word() { gender = Gender.neuter , singular = "Fräulein", plural = "Fräulein" }, 
                new Word() { gender = Gender.neuter , singular = "Mädchen", plural = "Mädchen" }
            };
    }


    class WordAssessor
    { 

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
