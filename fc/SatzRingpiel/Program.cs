using System;

namespace SatzRingpiel
{


    class Program
    {
        static string[] ringspiel = {
                "Steht dort nicht ein einfacher Mann, der namenlos ist?"
                ,"Der wird mir mit schlichten Worten sagen können, was zur Psychologie des Krieges gehört."
                ,"Seine Aufgabe ist es, den Spagat am Mörser anzuziehen – scheinbar nur eine einfache Dienstleistung und doch,"
                ,"welche unabsehbaren Folgen,"
                ,"für den übermütigen Feind sowohl wie für das Vaterland,"
                ,"knüpfen sich nicht an diesen Moment!"
                ,"Ob er sich dessen bewusst ist? "
                ,"Ob er auch seelisch auf der Höhe dieser Aufgabe steht? "
                ,"Freilich, die im Hinterland sitzen und von Spagat nichts weiter wissen als dass er auszugehen droht,"
                ,"sie ahnen auch nicht, zu welchen heroischen Möglichkeiten gerade der einfache Mann an der Front,"
                ,"der den Spagat am Mörser anzieht – (Sie wendet sich an einen Kanonier)"
            };


        private static int GetNextRandomNumber(int min, int max )
        {
            Random r = new Random();
            int rInt = r.Next(min, max); //for ints

            return rInt;
        }


        private static string GetNextSatz()
        {
            return ringspiel[GetNextRandomNumber(0, ringspiel.Length )]; 
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(GetNextSatz());
                Console.ReadLine();
            }
        }
    }
}
