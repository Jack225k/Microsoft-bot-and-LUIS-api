using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstBot.model
{
        public class TrendingMovie
        {
            public int watchers { get; set; }

        public string url { get; set; }
        public Movie movie { get; set; }
        }

        public class PopularMovies
        {
            public int mostviewed { get; set; }
            public Movie movie { get; set; }
        }

    public class Movie
        {
            public string title { get; set; }
            public int year { get; set; }
            public Ids ids { get; set; }
        }

        public class Ids
        {
            public int trakt { get; set; }
            public string slug { get; set; }
            public string imdb { get; set; }
            public int tmdb { get; set; }
        }
    }