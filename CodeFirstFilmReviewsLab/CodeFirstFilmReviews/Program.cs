﻿using EFCodeFirstFilmReviewsDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCodeFirstFilmReviewsDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (FilmReviewContext db = new FilmReviewContext())
            {
                db.Database.Migrate();
            }
            using (FilmReviewContext db = new FilmReviewContext())
            {
                db.Films.Add(new Film 
                    { Title = "Titanic", 
                      Overview = "Would you like ice with that?", 
                      ReleaseDate = new DateTime(1997, 11, 18), 
                      Revenue = 1845034188 
                    });
                db.Films.Add(new Film 
                    { Title = "ET", 
                      Overview = "Phone Home", 
                      ReleaseDate = new DateTime(2004, 03, 19), 
                      Revenue = 72258126 
                    });
                db.Reviewers.Add(new Reviewer { FirstName = "Mo", LastName = "Vilover", Email = "Mo.Vilover@critic.com" });
                db.Reviewers.Add(new Reviewer { FirstName = "Phil", LastName = "Emwotcher", Email = "Phil.Emwotcher@critic.com" });
                db.SaveChanges();
            }

            using (FilmReviewContext db = new FilmReviewContext())
            {
                db.Database.Migrate();
            }
            using (FilmReviewContext db = new FilmReviewContext())
            {
                db.Reviews.Add(new Review { FilmId = 1, ReviewerId = 2, Commentary = "Truly awful", Rating = 2 });
                db.SaveChanges();
            }

            using (FilmReviewContext db = new FilmReviewContext())
            {
                Film film = db.Films.Single(d => d.Title == "Titanic");
                Reviewer reviewer = db.Reviewers.Single(r => r.LastName == "EmWotcher");
                db.Reviews.Add(new Review
                {
                    Commentary = "Gives you a sinking feeling in your stomach",
                    Rating = 8,
                    Film = film,
                    Reviewer = reviewer
                });
                db.Reviews.Add(new Review
                {
                    Commentary = "Makes you want to phone home",
                    Rating = 6,
                    Film = db.Films.Single(d => d.Title == "ET"),
                    Reviewer = db.Reviewers.Single(r => r.LastName == "Vilover")
                });
                db.Reviews.Add(new Review
                {
                    Commentary = "It'd 'Be Good' if it wasn't for those pesky kids ",
                    Rating = 9,
                    Film = db.Films.Single(d => d.Title == "ET"),
                    Reviewer = db.Reviewers.Single(r => r.LastName == "EmWotcher")
                });
                db.SaveChanges();
            }
        }
    }
}
