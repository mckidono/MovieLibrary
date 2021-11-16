using System.Collections.Generic;

namespace MovieLibrary
{
    public class Movie
    {
        private List<string> genres;
        private int movieID;
        private string title;

        public Movie(int movieID = default, string title = null, List<string> genres = null)
        {
            this.movieID = movieID;
            this.title = title;
            this.genres = new List<string>();
        }

        public int MovieID
        {
            get => movieID;
            set => movieID = value;
        }

        public List<string> Genres
        {
            get => genres;
            set => genres = value;
        }

        public string Title
        {
            get => title;
            set
            {
                if (value.IndexOf(',') != -1)
                {
                    this.title = ($"\"{value}\"");
                }
                else
                {
                    this.title = value;
                }
            }
        }

        public string Print()
        {
            return $"MovieID: {this.movieID}" +
                   $"\nTitle: {this.title}" +
                   $"\nGenres: {string.Join(", ", this.genres)}" +
                   $"\n";
        }
    }
} 