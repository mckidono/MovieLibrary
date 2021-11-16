using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using NLog.Web;

namespace MovieLibrary{
    public class File{
        private string filePath;

        private List<Movie> movies;

        private StreamReader reader;

        private StreamWriter writer;

        private bool isCreated;

        private NLog.Logger logger;

         public File(string filePath = null, List<Movie> movies = null, StreamReader reader = null,
            StreamWriter writer = null, Logger logger = null)
        {
            this.filePath = filePath;
            this.movies = movies;
            this.reader = reader;
            this.writer = writer;
            this.logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        }

        public bool IsCreated(string title)
        {
            if (this.movies.ConvertAll(movie1 => movie1.Title.ToUpper()).Contains(title.ToUpper()))
            {
                this.logger.Error($"{title} already exists");
                this.isCreated = false;
                return false;
            }

            this.isCreated = true;
            return true;
        }

        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        public List<Movie> Movies
        {
            get => movies;
            set => movies = value;
        }

        public Logger Logger
        {
            get => logger;
            set => logger = value;
        }

        public File(string path)
        {
            this.logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            this.filePath = path;
            movies = new List<Movie>();
            try
            {
                StreamReader reader = new StreamReader(filePath);
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    Movie movie = new Movie();
                    string line = reader.ReadLine();
                    int index = line.IndexOf('"');
                    if (index == -1)
                    {
                        string[] movieData = line.Split(',');
                        movie.MovieID = Int32.Parse(movieData[0]);
                        movie.Title = movieData[1];
                        movie.Genres = movieData[2].Split('|').ToList();
                    }
                    else
                    {
                        movie.MovieID = Int32.Parse(line.Substring(0, index - 1));
                        line = line.Substring(index + 1);
                        index = line.IndexOf('"');
                        movie.Title = line.Substring(0, index);
                        line = line.Substring(index + 2);
                        movie.Genres = line.Split('|').ToList();
                    }

                    movies.Add(movie);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                this.logger.Error(e.Message);
            }
        }


        public void AddMovie(Movie movie)
        {
            try
            {
                movie.MovieID = movies.Max(movie2 => movie2.MovieID) + 1;
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine($"{movie.MovieID},{movie.Title},{string.Join("|", movie.Genres)}");
                writer.Close();
                movies.Add(movie);
            }
            catch (Exception e)
            {
                this.logger.Error(e.Message);
            }
        }
    }
}
