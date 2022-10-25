using System;
using System.Linq;
using EFMoviesDatabaseFirst.Models;

namespace EFMoviesDatabaseFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Display all movies (title, budget and popularity
            Console.WriteLine("\n\nMovies budgets and popularity\n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies.ToList();
                foreach (Movie m in movies)
                {
                    Console.WriteLine($"{m.Title} - Budget:{string.Format(new System.Globalization.CultureInfo("en-GB"), "{0:C}", m.Budget)}, Popularity:{m.Popularity}");
                }
            }

            //Display all movies with 2004 as  year they were released  (display title and budget).
            Console.WriteLine("\n\nMovies released in 2004\n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies
                    .Where(m => m.ReleaseDate != null && ((DateTime)m.ReleaseDate).Year == 2004)
                    .ToList();
                foreach (Movie m in movies)
                {
                    Console.WriteLine($"{m.Title} - Budget:{string.Format(new System.Globalization.CultureInfo("en-GB"), "{0:C}", m.Budget)}");
                }
            }

            //  list of all the cast, ordered by movie_id and then by character_name (display movie_id + character_name).
            Console.WriteLine("\n\nlist of all the cast, ordered by movie_id and then by character_name (display movie_id + character_name)\n");
            using (MoviesContext db = new MoviesContext())
            {
                var cast = db.Movies
                    .Join(db.MovieCasts, m => m.MovieId, mc => mc.MovieId, (m, mc) => new { m, mc })
                    //.Join(db.People, mcp => mcp.mc.PersonId, p => p.PersonId, (mcp, p) => new { mcp, p })
                    .Where(m => m.m.MovieId == 11)
                    .OrderBy(m => m.m.MovieId).ThenBy(mc => mc.mc.CharacterName)
                    .Select(p => new
                    {
                        MovieId = p.m.MovieId,
                        Title = p.m.Title,
                        CharacterName = p.mc.CharacterName
                        //ActorName = p.p.PersonName
                    })
                    .ToList();
                foreach (var cm in cast)
                {
                    System.Console.WriteLine($"{cm.MovieId}: {cm.Title} - {cm.CharacterName}");
                }
            }

            //movies, ordered by budget high to low (display title and budget).
            Console.WriteLine("\n\nmovies, ordered by budget high to low (display title and budget)\n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = (from m in db.Movies
                              orderby m.Budget descending
                              where m.MovieId == 11
                              select new { Title = m.Title, Budget = m.Budget })
                           .ToList();
                foreach (var m in movies)
                {
                    System.Console.WriteLine($"{m.Title} - Budget:{string.Format(new System.Globalization.CultureInfo("en-GB"), "{0:C}", m.Budget)}");
                }
            }

            //  list of all the character_name and associated actor name).
            Console.WriteLine("\n\nmovie characters and associated actors\n");
            using (MoviesContext db = new MoviesContext())
            {
                var cast = db.MovieCasts
                    .Join(db.People, c => c.PersonId, p => p.PersonId, (cp, p) => new { cp, p })
                    .Where(c => c.cp.MovieId == 11)
                    .Select(p => new
                    {
                        CharacterName = p.cp.CharacterName,
                        ActorName = p.p.PersonName
                    })
                    .ToList();
                foreach (var cm in cast)
                {
                    System.Console.WriteLine($"{cm.CharacterName}: {cm.ActorName}");
                }
            }


            //movies and names of people who worked in the directing department whose revenue was less than their budget, (display title and person_name).
            Console.WriteLine("\n\nmovies and names of people who worked in the directing department whose revenue was less than their budget, (display title and person_name)\n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies
                    .Join(db.MovieCrews, m => m.MovieId, mc => mc.MovieId, (m, mc) => new { m, mc })
                    .Join(db.Departments, mc => mc.mc.DepartmentId, d => d.DepartmentId, (mc, d) => new{mc, d})
                    .Join(db.People, mc => mc.mc.mc.PersonId, p => p.PersonId, (mc, p) => new { mc, p })
                    .Where(d => d.mc.d.DepartmentName == "Directing")
                    .Where(m => m.mc.mc.m.Revenue < m.mc.mc.m.Budget)
                    .Select(m => new
                    {
                        Title = m.mc.mc.m.Title,
                        PersonName = (m.p.PersonName)
                    })
                    .ToList();
                foreach (var m in movies)
                {
                    System.Console.WriteLine($"{m.Title} - {m.PersonName}");
                }
            }

            //top 3 directors in terms of how many films they have made (display person_name and total number of movies).
            Console.WriteLine("\n\ntop 3 directors in terms of how many films they have made \n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies
                    .Join(db.MovieCrews, m => m.MovieId, mc => mc.MovieId, (m, mc) => new { m, mc })
                    .Join(db.Departments, mc => mc.mc.DepartmentId, d => d.DepartmentId, (mc, d) => new { mc, d })
                    .Join(db.People, mc => mc.mc.mc.PersonId, p => p.PersonId, (mc, p) => new { mc, p })
                    .Where(d => d.mc.mc.mc.Job == "Director")
                    .GroupBy(n => n.p.PersonName) 
                    .Select(g => new
                    {
                        PersonName = g.Key,
                        Count = g.Count()
                    }).OrderByDescending(m => m.Count).Take(3).ToList();
                foreach (var m in movies)
                {
                    Console.WriteLine($"{m.PersonName} {m.Count} ");
                }
            }


            // directors who have made less than 5 movies (display person_name and total number of movies).
            Console.WriteLine("\n\ndirectors who have made less than 5 movies \n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies
                    .Join(db.MovieCrews, m => m.MovieId, mc => mc.MovieId, (m, mc) => new { m, mc })
                    .Join(db.Departments, mc => mc.mc.DepartmentId, d => d.DepartmentId, (mc, d) => new { mc, d })
                    .Join(db.People, mc => mc.mc.mc.PersonId, p => p.PersonId, (mc, p) => new { mc, p })
                    .Where(d => d.mc.mc.mc.Job == "Director" )
                    .GroupBy(n => n.p.PersonName)
                    .Select(g => new
                    {
                        PersonName = g.Key,
                        Count = g.Count()
                    }).OrderByDescending(m => m.Count).Where(m => m.Count < 5).ToList();
                foreach (var m in movies)
                {
                    Console.WriteLine($"{m.PersonName} {m.Count} ");
                }
            }


            // top 10 directors whose movies have collectively made the most profit (display person_name, total number of movies made and total profit).
            Console.WriteLine("\n directors whose movies have collectively made the most profit  \n");
            using (MoviesContext db = new MoviesContext())
            {
                var movies = db.Movies
                    .Join(db.MovieCrews, m => m.MovieId, mc => mc.MovieId, (m, mc) => new { m, mc })
                    .Join(db.Departments, mc => mc.mc.DepartmentId, d => d.DepartmentId, (mc, d) => new { mc, d })
                    .Join(db.People, mc => mc.mc.mc.PersonId, p => p.PersonId, (mc, p) => new { mc, p })
                    .Where(d => d.mc.mc.mc.Job == "Director")
                    .GroupBy(n => n.p.PersonName)
                    .Select(g => new
                    {
                        PersonName = g.Key,
                        Count = g.Count(),
                        Profit = g.Sum(m => (m.mc.mc.m.Revenue - m.mc.mc.m.Budget))
                    }).OrderByDescending(m => m.Profit).Take(10).ToList();
                foreach (var m in movies)
                {
                   Console.WriteLine($"Director:{m.PersonName} MovieCounmt:{m.Count} Total Profit:${m.Profit}"); //{m.DirectorsMovies.Sum(m => (m.mc.mc.m.Revenue - m.mc.mc.m.Budget))}
                }
            }
 
        }
    }
}