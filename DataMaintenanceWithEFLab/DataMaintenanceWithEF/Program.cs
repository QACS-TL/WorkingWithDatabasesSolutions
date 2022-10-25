using System;
using System.Linq;
using DataMaintenanceWithEF.Models;

namespace DataMaintenanceWithEF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DATA MAINTENANCE
            // Creating a new movie
            Movie mov = new Movie();
            using (MoviesContext db = new MoviesContext())
            {
                mov.Title = "Hairy Spotter and the Potion of Doom";
                mov.Tagline = "Hairy, Germione and Don get up to mischief in a potions class!";
                mov.Overview = "Hairy hops off to Cakewalks and gets into a couple of scrapes but triumphs in the end";
                mov.Runtime = int.MaxValue;
                mov.Homepage = "https://www.youtube.com/watch?v=-_vqx2BsSj0&t=125s";
                db.Movies.Add(mov);
                db.SaveChanges();
            }

            //Run following query in SQL Server Management Studio
            //SELECT * FROM Movie WHERE title Like 'Hairy%' 

            // Updating Movie tag line
            using (MoviesContext db = new MoviesContext())
            {
                Movie m = db.Movies.Single(m => m.MovieId == mov.MovieId);
                m.Tagline += "*";
                db.SaveChanges();
            }

            // Deleting a movie
            using (MoviesContext db = new MoviesContext())
            {
                Movie m = db.Movies.Single(m => m.MovieId == mov.MovieId);
                db.Movies.Remove(m);
                db.SaveChanges();
            }

            // UpdatingMinions Movie Revenue
            string movieTitle = "Minions";
            using (MoviesContext db = new MoviesContext())
            {
                Movie m = db.Movies.Single(m => m.Title == movieTitle);
                m.Revenue = 1160000000;
                db.SaveChanges();
            }

            //  Increase the revenue of all movies with a popularity that is greater than 100 by 10% and by 5% for all the rest
            using (MoviesContext db = new MoviesContext())
            {
                db.Movies
                    .ToList()
                    .ForEach(m =>
                    {
                        if (m.Popularity > 100)
                        {
                            m.Revenue = (long?)(m.Revenue * 1.1);
                        }
                        else
                        {
                            m.Revenue = (long?)(m.Revenue * 1.05);
                        }
                    });
                db.SaveChanges();
            }

            // Add the Hairy Spotter movie back into the database
            mov = new Movie();
            using (MoviesContext db = new MoviesContext())
            {
                mov.Title = "Hairy Spotter and the Potion of Doom";
                mov.Tagline = "Hairy, Germione and Don get up to mischief in a potions class!";
                mov.Overview = "Hairy hops off to Cakewalks and gets into a couple of scrapes but triumphs in the end";
                mov.Runtime = int.MaxValue;
                mov.Homepage = "https://www.youtube.com/watch?v=-_vqx2BsSj0&t=125s";
                //db.Movies.Add(mov);
                db.SaveChanges();
            }

            //  add some cast members to the movie_cast and person tables
            Person person = new Person();
            MovieCast castMember = new MovieCast();
            using (MoviesContext db = new MoviesContext())
            {
                person.PersonName = "Stu Oofedtoi";
                db.People.Add(person);
                //db.SaveChanges();
                castMember.Person = person;
                castMember.Movie = mov;
                castMember.CharacterName = "Hairy Spotter";
                castMember.GenderId = 2;
                db.MovieCasts.Add(castMember);

                person = new Person { PersonName = "Irma Wudendol" };
                db.People.Add(person);
                castMember = new MovieCast { Person = person, Movie = mov, CharacterName = "Germione Stranger", GenderId = 1 };
                db.MovieCasts.Add(castMember);

                person = new Person { PersonName = "Woody Nactor" };
                db.People.Add(person);
                castMember = new MovieCast { Person = person, Movie = mov, CharacterName = "Don Squeezely", GenderId = 2 };
                db.MovieCasts.Add(castMember);

                person = new Person { PersonName = "W. D. Forte" };
                db.People.Add(person);
                castMember = new MovieCast { Person = person, Movie = mov, CharacterName = "Professor Snipe", GenderId = 2 };
                db.MovieCasts.Add(castMember);

                db.SaveChanges();
            }

            List<int> personIds = new List<int>();
            //Check to see if insert s have worked
            using (MoviesContext db = new MoviesContext())
            {
                person = db.People.Single(p => p.PersonName == "Stu Oofedtoi");
                castMember = db.MovieCasts.Single(cm => cm.CharacterName == "Hairy Spotter");

                Console.Write($"{castMember.CharacterName} has a MovieID of {castMember.MovieId} and a PersonID of {castMember.PersonId}. ");
                Console.WriteLine($"and is played by {person.PersonName} that has an ID of {person.PersonId}");
                personIds.Add(person.PersonId);

                person = db.People.Single(p => p.PersonName == "Irma Wudendol");
                castMember = db.MovieCasts.Single(cm => cm.CharacterName == "Germione Stranger");

                Console.Write($"{castMember.CharacterName} has a MovieID of {castMember.MovieId} and a PersonID of {castMember.PersonId}. ");
                Console.WriteLine($"and is played by {person.PersonName} that has an ID of {person.PersonId}");
                personIds.Add(person.PersonId);


                person = db.People.Single(p => p.PersonName == "Woody Nactor");
                castMember = db.MovieCasts.Single(cm => cm.CharacterName == "Don Squeezely");

                Console.Write($"{castMember.CharacterName} has a MovieID of {castMember.MovieId} and a PersonID of {castMember.PersonId}. ");
                Console.WriteLine($"and is played by {person.PersonName} that has an ID of {person.PersonId}");
                personIds.Add(person.PersonId);

                person = db.People.Single(p => p.PersonName == "W. D. Forte");
                castMember = db.MovieCasts.Single(cm => cm.CharacterName == "Professor Snipe");

                Console.Write($"{castMember.CharacterName} has a MovieID of {castMember.MovieId} and a PersonID of {castMember.PersonId}. ");
                Console.WriteLine($"and is played by {person.PersonName} that has an ID of {person.PersonId}");
                personIds.Add(person.PersonId);

            }

            // Deleting a cast member
            using (MoviesContext db = new MoviesContext())
            {
                foreach (int personId in personIds) {
                    MovieCast mcm = db.MovieCasts.Single(cm => cm.MovieId == castMember.MovieId && cm.PersonId == personId);
                    db.MovieCasts.Remove(mcm);
                }
                db.SaveChanges();
            }

            // Deleting a person
            using (MoviesContext db = new MoviesContext())
            {
                foreach (int personId in personIds)
                {
                    Person p1 = db.People.Single(p => p.PersonId == personId);
                    db.People.Remove(p1);
                }
                db.SaveChanges();
            }

            // Deleting a movie
            using (MoviesContext db = new MoviesContext())
            {
                Movie m = db.Movies.Single(m => m.MovieId == mov.MovieId);
                db.Movies.Remove(m);
                db.SaveChanges();
            }


        }
    }
}