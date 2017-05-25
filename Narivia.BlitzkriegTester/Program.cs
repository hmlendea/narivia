using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.BusinessLogic.GameManagers;

namespace Narivia.BlitzkriegTester
{
    class MainClass
    {
        const string WORLD_ID = "narivia";
        const string FACTION_ID = "alpalet";
        const string FACTION_TARGET_ID = "caravenna";
        const int TEST_COUNT = 15;

        public static void Main(string[] args)
        {
            TestSequencialBlitzkrieg();
            TestParallelizedBlitzkrieg();
        }

        static void TestSequencialBlitzkrieg()
        {
            Console.WriteLine("==== RUNNING THE SEQUENCIAL TEST");
            List<Tuple<DateTime, DateTime>> results = new List<Tuple<DateTime, DateTime>>();

            for (int i = 0; i < TEST_COUNT; i++)
            {
                Console.Write($"  {i + 1} : ");
                DateTime startDate = DateTime.Now;
                CallSequencialBlitzkrieg();
                DateTime endDate = DateTime.Now;

                results.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
            }

            DisplayResults(results);
        }

        static void TestParallelizedBlitzkrieg()
        {
            Console.WriteLine("==== RUNNING THE PARALLEL TEST");
            List<Tuple<DateTime, DateTime>> results = new List<Tuple<DateTime, DateTime>>();

            for (int i = 0; i < TEST_COUNT; i++)
            {
                Console.Write($"  {i + 1} : ");
                DateTime startDate = DateTime.Now;
                CallParallelizedBlitzkrieg();
                DateTime endDate = DateTime.Now;

                results.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
            }

            DisplayResults(results);
        }

        static void CallSequencialBlitzkrieg()
        {
            GameDomainService game = new GameDomainService();
            game.NewGame(WORLD_ID, FACTION_ID);

            string regionId = string.Empty;

            while (regionId != null)
            {
                regionId = game.Blitzkrieg_Seq(FACTION_ID, FACTION_TARGET_ID);
                Console.Write($"{regionId}->");
            }
            Console.WriteLine();

            game = null;
        }

        static void CallParallelizedBlitzkrieg()
        {
            GameDomainService game = new GameDomainService();
            game.NewGame(WORLD_ID, FACTION_ID);

            string regionId = string.Empty;

            while (regionId != null)
            {
                regionId = game.Blitzkrieg_Parallel(FACTION_ID, FACTION_TARGET_ID);
                Console.Write($"{regionId}->");
            }
            Console.WriteLine();
        }

        static void DisplayResults(List<Tuple<DateTime, DateTime>> results)
        {
            Console.WriteLine($"==> Results: ({results.Count})");

            List<TimeSpan> durations = results.Select(x => x.Item2 - x.Item1).ToList();

            for (int i = 1; i < TEST_COUNT; i++)
            {
                Console.WriteLine($"  {i} : {durations[i].Milliseconds} ms");
            }

            Console.WriteLine($"Average timespan: {Math.Round(durations.Average(x => x.Milliseconds))} ms");
        }
    }
}
