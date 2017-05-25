using System;

using Narivia.BusinessLogic.GameManagers;

namespace Narivia.BlitzkriegTester
{
    class MainClass
    {
        const string WORLD_ID = "narivia";
        const string FACTION_ID = "alpalet";
        const string FACTION_TARGET_ID = "caravenna";

        public static void Main(string[] args)
        {
            TestSequencialBlitzkrieg();
            TestParallelizedBlitzkrieg();
        }

        public static void TestSequencialBlitzkrieg()
        {
            Console.WriteLine("==== RUNNING THE SEQUENCIAL TEST");
            DateTime startDate = DateTime.Now;

            GameDomainService game = new GameDomainService();
            game.NewGame(WORLD_ID, FACTION_ID);

            string regionId = string.Empty;

            while (regionId != null)
            {
                regionId = game.Blitzkrieg_Seq(FACTION_ID, FACTION_TARGET_ID);

                Console.WriteLine(regionId);
            }

            DateTime endDate = DateTime.Now;
            DisplayResults(startDate, endDate);
        }

        public static void TestParallelizedBlitzkrieg()
        {
            Console.WriteLine("==== RUNNING THE PARALLEL TEST");
            DateTime startDate = DateTime.Now;

            GameDomainService game = new GameDomainService();
            game.NewGame(WORLD_ID, FACTION_ID);

            string regionId = string.Empty;

            while (regionId != null)
            {
                regionId = game.Blitzkrieg_Parallel(FACTION_ID, FACTION_TARGET_ID);

                Console.WriteLine(regionId);
            }

            DateTime endDate = DateTime.Now;
            DisplayResults(startDate, endDate);
        }

        static void DisplayResults(DateTime startDate, DateTime endDate)
        {
            TimeSpan deltaTime = endDate - startDate;

            Console.WriteLine("Results:");
            Console.WriteLine($"Started at   : {startDate} system time");
            Console.WriteLine($"Ended at     : {endDate} system time");
            Console.WriteLine($"Elpased time : {deltaTime.Milliseconds} ms");
        }
    }
}
