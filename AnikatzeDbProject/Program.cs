using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using AnikatzeDbProject.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AnikatzeDbProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            EnsureDatabaseMigratedAndSeeded(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void EnsureDatabaseMigratedAndSeeded(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AnikatzeDbContext>();

                    // Ensure that the database is created.
                    context.Database.EnsureDeleted(); // Delete the existing database
                    context.Database.EnsureCreated(); // Recreate the database

                    // Apply any pending migrations.
                    context.Database.Migrate();

                    // Seed the database.
                    SeedData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                }
            }
        }

        private static void SeedData(AnikatzeDbContext context)
        {
            if (context.Users != null && !context.Users.Any())
            {
                var users = MockDataGenerator.GenerateUsers(50);
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (context.Courses != null && !context.Courses.Any())
            {
                var courses = MockDataGenerator.GenerateCourses(10);
                context.Courses.AddRange(courses);
                context.SaveChanges();
            }

            if (context.PaymentStatuses != null && !context.PaymentStatuses.Any())
            {
                var paymentStatuses = MockDataGenerator.GeneratePaymentStatuses();
                context.PaymentStatuses.AddRange(paymentStatuses);
                context.SaveChanges();
            }

            if (context.Bills != null && !context.Bills.Any())
            {
                var bills = context.Users!.SelectMany(u => MockDataGenerator.GenerateBills(u.UserID, 2)).ToList();
                context.Bills.AddRange(bills);
                context.SaveChanges();
            }

            if (context.Payments != null && !context.Payments.Any())
            {
                var paymentStatuses = context.PaymentStatuses!.ToList();
                var bills = context.Bills!.ToList();
                var payments = bills.SelectMany(b => MockDataGenerator.GeneratePayments(b.BillID, paymentStatuses, 2)).ToList();
                context.Payments.AddRange(payments);
                context.SaveChanges();
            }

            if (context.Lections != null && !context.Lections.Any())
            {
                var courses = context.Courses!.ToList();
                var lections = courses.SelectMany(c => MockDataGenerator.GenerateLections(c.CourseID, 12)).ToList();
                context.Lections.AddRange(lections);
                context.SaveChanges();
            }

            if (context.Quizzes != null && !context.Quizzes.Any())
            {
                var lections = context.Lections!.ToList();
                var quizzes = lections.SelectMany(l => MockDataGenerator.GenerateQuizzes(l.LectionID, 3)).ToList();
                context.Quizzes.AddRange(quizzes);
                context.SaveChanges();
            }

            if (context.QuizQuestions != null && !context.QuizQuestions.Any())
            {
                var quizzes = context.Quizzes!.ToList();
                var quizQuestions = quizzes.SelectMany(q => MockDataGenerator.GenerateQuizQuestions(q.QuizID, 5)).ToList();
                context.QuizQuestions.AddRange(quizQuestions);
                context.SaveChanges();
            }

            if (context.QuizOptions != null && !context.QuizOptions.Any())
            {
                var quizQuestions = context.QuizQuestions!.ToList();
                var quizOptions = quizQuestions.SelectMany(qq => MockDataGenerator.GenerateQuizOptions(qq.QuizQuestionID, 4)).ToList();
                context.QuizOptions.AddRange(quizOptions);
                context.SaveChanges();
            }

            if (context.Carts != null && !context.Carts.Any())
            {
                var carts = context.Users!.SelectMany(u => MockDataGenerator.GenerateCarts(u.UserID)).ToList();
                context.Carts.AddRange(carts);
                context.SaveChanges();
            }

            if (context.CartHistories != null && !context.CartHistories.Any())
            {
                var cartHistories = context.Users!.SelectMany(u => MockDataGenerator.GenerateCartHistories(u.UserID, 2, 4)).ToList();
                context.CartHistories.AddRange(cartHistories);
                context.SaveChanges();
            }

            if (context.CartItems != null && !context.CartItems.Any())
            {
                var carts = context.Carts!.ToList();
                var courses = context.Courses!.ToList();
                var cartItems = carts.SelectMany(c => MockDataGenerator.GenerateCartItems(c.CartID, courses.Select(cs => cs.CourseID).ToList(), 1, 10)).ToList();
                context.CartItems.AddRange(cartItems);
                context.SaveChanges();
            }

            if (context.Reviews != null && !context.Reviews.Any())
            {
                var courses = context.Courses!.ToList();
                var users = context.Users!.ToList();
                var reviews = courses.SelectMany(c => MockDataGenerator.GenerateReviews(c.CourseID, users.Select(u => u.UserID).ToList(), 5)).ToList();
                context.Reviews.AddRange(reviews);
                context.SaveChanges();
            }

            if (context.UserCourses != null && !context.UserCourses.Any())
            {
                var users = context.Users!.ToList();
                var courses = context.Courses!.ToList();
                var userCourses = users.SelectMany(u => MockDataGenerator.GenerateUserCourses(u.UserID, courses.Select(c => c.CourseID).ToList(), 2, 5)).ToList();
                context.UserCourses.AddRange(userCourses);
                context.SaveChanges();
            }

            if (context.UserQuizzes != null && !context.UserQuizzes.Any())
            {
                var users = context.Users!.ToList();
                var quizzes = context.Quizzes!.ToList();
                var userQuizzes = users.SelectMany(u => MockDataGenerator.GenerateUserQuizzes(u.UserID, quizzes.Select(q => q.QuizID).ToList(), 1, 3)).ToList();
                context.UserQuizzes.AddRange(userQuizzes);
                context.SaveChanges();
            }

            if (context.Videos != null && !context.Videos.Any())
            {
                var lections = context.Lections!.ToList();
                var videos = lections.SelectMany(l => MockDataGenerator.GenerateVideos(l.LectionID, 20)).ToList();
                context.Videos.AddRange(videos);
                context.SaveChanges();
            }
        }
    }
}
