using MongoDB.Bson;
using MongoDB.Driver;
using NoSQLFilmReviews.Models;

namespace NoSQLFilmReviews
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://demo-user:sTqTKzzKL5gm3YQh@cluster0.hgliefa.mongodb.net/?retryWrites=true&w=majority");

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("test");

            // Get list of databases within the cluster connected to
            var databases = client.ListDatabases().ToList();
            foreach (var db in databases)
            {
                Console.WriteLine(db["name"]);
            }

            Film film1 = new Film { Id = 1, Title = "Titanic", Overview = "Blab, blah ,blah", ReleaseDate = new DateTime(1997, 11, 18), Revenue = 1845034188, Reviews = new List<Review>() };
            Film film2 = new Film { Id = 2, Title = "ET", Overview = "An extra terrestrial comes to earth, gets sick, phones home and is rescued", ReleaseDate = new DateTime(2004, 03, 19), Revenue = 72258126, Reviews = new List<Review>() };

            Reviewer reviewer1 = new Reviewer { Id = 100, FirstName = "Mo", LastName = "Vilover", Email = "Mo.Vilover@critic.com", Reviews = new List<Review>() };
            Reviewer reviewer2 = new Reviewer { Id = 101, FirstName = "Phil", LastName = "Emwotcher", Email = "Phil.Emwotcher@critic.com", Reviews = new List<Review>() };
            Reviewer reviewer3 = new Reviewer { Id = 102, FirstName = "Fi", LastName = "Lemkritik", Email = "Fi.Lemkritik@critic.com", Reviews = new List<Review>() };


            database = client.GetDatabase("FilmReviews");
            var reviews = database.GetCollection<Review>("Reviews");
            var films = database.GetCollection<Film>("Films");
            var reviewers = database.GetCollection<Reviewer>("Reviewers");

            reviews.DeleteMany(r => true);
            films.DeleteMany(f => true);
            reviews.DeleteMany(r => true);
            reviewers.DeleteMany(f => true);

            Review review = new Review
            {
                Id = 1000,
                FilmId = film1.Id,
                ReviewerId = reviewer2.Id,
                Commentary = "Truly awful",
                Rating = 2,
            };

            reviews.InsertOne(review);
            film1.Reviews.Add(review);
            reviewer2.Reviews.Add(review);

            review = new Review
            {
                Id = 1001,
                FilmId = film1.Id,
                ReviewerId = reviewer2.Id,
                Commentary = "Gives you a sinking feeling in your stomach",
                Rating = 8,
            };

            reviews.InsertOne(review);
            film1.Reviews.Add(review);
            reviewer2.Reviews.Add(review);

            review = new Review
            {
                Id = 1002,
                FilmId = film1.Id,
                ReviewerId = reviewer3.Id,
                Commentary = "Chilly",
                Rating = 6,
            };
            reviews.InsertOne(review);
            film1.Reviews.Add(review);
            reviewer3.Reviews.Add(review);

            review = new Review
            {
                Id = 1003,
                FilmId = film1.Id,
                ReviewerId = reviewer2.Id,
                Rating = 9,
            };
            reviews.InsertOne(review);
            film2.Reviews.Add(review);
            reviewer2.Reviews.Add(review);

            films.InsertOne(film1);
            films.InsertOne(film2);

            reviewers.InsertOne(reviewer1);
            reviewers.InsertOne(reviewer2);
            reviewers.InsertOne(reviewer3);

            // Get all films with at least 2 reviews
            List<Film> filmsWithReviews = films.Find(f => f.Reviews.Count >= 2).ToList();
            foreach (Film film in filmsWithReviews)
            {
                System.Console.WriteLine(film.Title);
                foreach (Review rev in film.Reviews)
                {
                    Console.WriteLine($"{rev.Commentary} Rating ({rev.Rating})");
                }
            }

            // Get Reviewers, retrieve just firstname, lastname and the number of reviews they've made
            var reviewerDetails = reviewers
                .Find(d => true)
                .Project(d => new
                {
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    ReviewCount = d.Reviews != null ? d.Reviews.Count : 0
                })
                .ToList();
            foreach (var reviewer in reviewerDetails)
            {
                Console.WriteLine($"{reviewer.FirstName} {reviewer.LastName} has written {reviewer.ReviewCount} reviews");
            }

            // Delete an item from a collection
            films.FindOneAndDelete(f => f.Title == "ET");

            films.InsertOne(new Film
            {
                Id = 3,
                Title = "Up",
                Overview = "The ultimate house move",
                ReleaseDate = new DateTime(2012, 04, 21)
            });


            films.InsertOne(new Film
            {
                Id = 4,
                Title = "Where Eagles Dare",
                Overview = "A Wonkymotion remake of the original. Truly awe inspiring.",
                ReleaseDate = new DateTime(2022, 04, 21),
                HomepageURL = "https://www.youtube.com/watch?v=ENrgZ4KAnNw"
            });

            // Retrieve films and show revenue, even though some films don't have them
            foreach (Film film in films.Find(d => true).ToList())
            {
                Console.WriteLine($"{film.Title} has a revenue of ${(film.Revenue != null ? film.Revenue : 0)}, Hompage: {(film.HomepageURL != null ? film.HomepageURL : "No Hompage")}");
            }
        }
    }
}
