using System;
using System.IO;
using NLog;
using NLog.Web;

namespace MovieLibrary
{    class Program
    {
        private static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
             string csvPath = "movies.csv";

            File csv = new File(csvPath);

            string input = "";
            do
            {
                Console.WriteLine("1) Add Movie  \n  2) Print Movies  \n  Enter to quit");
                input = Console.ReadLine();
                if (input == "1")
                {
                    Movie movie = new Movie();
                    Console.Write("Title: ");
                    movie.Title = Console.ReadLine();
                    if (csv.IsCreated(movie.Title))
                    {
                        do
                        {
                            Console.WriteLine("Genre (NA when done)");
                            input = Console.ReadLine();
                            if (input != "NA" && input.Length > 0)
                            {
                                movie.Genres.Add(input);
                            }
                        } while (input != "NA");
                        if (movie.Genres.Count == 0)
                        {
                            movie.Genres.Add("n/a");
                        }
                        csv.AddMovie(movie);
                    }
                }
                else if (input == "2")
                {
                    foreach (Movie mov in csv.Movies)
                    {
                        Console.WriteLine(mov.Print());
                    }
                }
            } while (input == "1" || input == "2");
        }
    }
}
