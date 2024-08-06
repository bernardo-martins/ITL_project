using Microsoft.EntityFrameworkCore;
using AnikatzeDbProject.Model;
using System.Linq;
using System.Collections.Generic;

namespace AnikatzeDbProject.Data
{
    public class AnikatzeDbContext : DbContext
    {
        public AnikatzeDbContext(DbContextOptions<AnikatzeDbContext> options)
            : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Bill>? Bills { get; set; }
        public DbSet<Cart>? Carts { get; set; }
        public DbSet<CartHistory>? CartHistories { get; set; }
        public DbSet<CartItem>? CartItems { get; set; }
        public DbSet<Course>? Courses { get; set; }
        public DbSet<Lection>? Lections { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<PaymentStatus>? PaymentStatuses { get; set; }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<QuizOption>? QuizOptions { get; set; }
        public DbSet<QuizQuestion>? QuizQuestions { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<UserCourse>? UserCourses { get; set; }
        public DbSet<UserQuiz>? UserQuizzes { get; set; }
        public DbSet<Video>? Videos { get; set; }
        public DbSet<UserLectionCompletion>? UserLectionCompletions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many relationship: User <-> Course
            modelBuilder.Entity<UserCourse>().HasKey(uc => new { uc.UserID, uc.CourseID });
            modelBuilder.Entity<UserCourse>().HasOne(uc => uc.User).WithMany(u => u.UserCourses).HasForeignKey(uc => uc.UserID);
            modelBuilder.Entity<UserCourse>().HasOne(uc => uc.Course).WithMany(c => c.UserCourses).HasForeignKey(uc => uc.CourseID);


            //von niklas
            modelBuilder.Entity<UserLectionCompletion>()
                .HasKey(ulc => ulc.UserLectionCompletionID);

            modelBuilder.Entity<UserLectionCompletion>()
                .HasOne(ulc => ulc.User)
                .WithMany(u => u.UserLectionCompletions)
                .HasForeignKey(ulc => ulc.UserID);

            modelBuilder.Entity<UserLectionCompletion>()
                .HasOne(ulc => ulc.Lection)
                .WithMany(l => l.UserLectionCompletions)
                .HasForeignKey(ulc => ulc.LectionID);

            modelBuilder.Entity<UserLectionCompletion>()
                .HasIndex(ulc => new { ulc.UserID, ulc.LectionID })
                .IsUnique();
            //von niklas ende

            // Relationships for User
            modelBuilder.Entity<User>().HasMany(u => u.Bills).WithOne(b => b.User).HasForeignKey(b => b.UserID);
            modelBuilder.Entity<User>().HasOne(u => u.Cart).WithOne(c => c.User).HasForeignKey<Cart>(c => c.UserID);
            modelBuilder.Entity<User>().HasMany(u => u.Reviews).WithOne(r => r.User).HasForeignKey(r => r.UserID);
            modelBuilder.Entity<User>().HasMany(u => u.UserQuizzes).WithOne(uq => uq.User).HasForeignKey(uq => uq.UserID);

            // Relationships for Course
            modelBuilder.Entity<Course>().HasMany(c => c.Reviews).WithOne(r => r.Course).HasForeignKey(r => r.CourseID);
            modelBuilder.Entity<Course>().HasMany(c => c.CartItems).WithOne(ci => ci.Course).HasForeignKey(ci => ci.CourseID);
            modelBuilder.Entity<Course>().HasMany(c => c.UserCourses).WithOne(uc => uc.Course).HasForeignKey(uc => uc.CourseID);

            // Relationships for Lection
            modelBuilder.Entity<Lection>().HasMany(l => l.Quizzes).WithOne(q => q.Lection).HasForeignKey(q => q.LectionID);
            modelBuilder.Entity<Lection>().HasMany(l => l.Videos).WithOne(v => v.Lection).HasForeignKey(v => v.LectionID);

            // Relationships for Quiz
            modelBuilder.Entity<Quiz>().HasMany(q => q.QuizQuestions).WithOne(qq => qq.Quiz).HasForeignKey(qq => qq.QuizID);
            modelBuilder.Entity<Quiz>().HasMany(q => q.UserQuizzes).WithOne(uq => uq.Quiz).HasForeignKey(uq => uq.QuizID);
            modelBuilder.Entity<QuizQuestion>().HasMany(qq => qq.QuizOptions).WithOne(qo => qo.QuizQuestion).HasForeignKey(qo => qo.QuizQuestionID);

            // Relationships for Payment
            modelBuilder.Entity<Payment>().HasOne(p => p.Bill).WithMany(b => b.Payments).HasForeignKey(p => p.BillID);
            modelBuilder.Entity<Bill>().HasMany(b => b.Payments).WithOne(p => p.Bill).HasForeignKey(p => p.BillID);
            modelBuilder.Entity<Payment>().HasOne(p => p.PaymentStatus).WithMany(ps => ps.Payments).HasForeignKey(p => p.PaymentStatusID);

            // Relationships for Cart
            modelBuilder.Entity<Cart>().HasMany(c => c.CartItems).WithOne(ci => ci.Cart).HasForeignKey(ci => ci.CartID);
            modelBuilder.Entity<CartHistory>().HasMany(ch => ch.CartItems).WithOne(ci => ci.CartHistory).HasForeignKey(ci => ci.CartHistoryID);

            // Relationships for UserQuiz
            modelBuilder.Entity<UserQuiz>().HasKey(uq => uq.UserQuizID);
            modelBuilder.Entity<UserQuiz>().HasOne(uq => uq.User).WithMany(u => u.UserQuizzes).HasForeignKey(uq => uq.UserID);
            modelBuilder.Entity<UserQuiz>().HasOne(uq => uq.Quiz).WithMany(q => q.UserQuizzes).HasForeignKey(uq => uq.QuizID);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var users = MockDataGenerator.GenerateUsers(50);
            var courses = MockDataGenerator.GenerateCourses(10);
            var lections = courses.SelectMany(c => MockDataGenerator.GenerateLections(c.CourseID, 12)).ToList();
            var quizzes = lections.SelectMany(l => MockDataGenerator.GenerateQuizzes(l.LectionID, 3)).ToList();
            var quizQuestions = quizzes.SelectMany(q => MockDataGenerator.GenerateQuizQuestions(q.QuizID, 5)).ToList();
            var quizOptions = quizQuestions.SelectMany(qq => MockDataGenerator.GenerateQuizOptions(qq.QuizQuestionID, 4)).ToList();
            var paymentStatuses = MockDataGenerator.GeneratePaymentStatuses();
            var bills = users.SelectMany(u => MockDataGenerator.GenerateBills(u.UserID, 2)).ToList();
            var payments = bills.SelectMany(b => MockDataGenerator.GeneratePayments(b.BillID, paymentStatuses, 2)).ToList();
            var carts = users.Select(u => MockDataGenerator.GenerateCarts(u.UserID).First()).ToList();
            var cartHistories = users.SelectMany(u => MockDataGenerator.GenerateCartHistories(u.UserID, 2, 4)).ToList();
            var cartItems = carts.SelectMany(c => MockDataGenerator.GenerateCartItems(c.CartID, courses.Select(cs => cs.CourseID).ToList(), 1, 10)).ToList();
            var reviews = courses.SelectMany(c => MockDataGenerator.GenerateReviews(c.CourseID, users.Select(u => u.UserID).ToList(), 5)).ToList();
            

            // Generate unique UserCourses
            var userCourses = new List<UserCourse>();
            var userCourseSet = new HashSet<(int UserId, int CourseId)>();
            foreach (var user in users)
            {
                var userCourseList = MockDataGenerator.GenerateUserCourses(user.UserID, courses.Select(c => c.CourseID).ToList(), 2, 5);
                foreach (var userCourse in userCourseList)
                {
                    if (userCourseSet.Add((userCourse.UserID, userCourse.CourseID)))
                    {
                        userCourses.Add(userCourse);
                    }
                }
            }
            var completions = MockDataGenerator.GenerateUserLectionCompletions(users, lections, userCourses);

            var userQuizzes = users.SelectMany(u => MockDataGenerator.GenerateUserQuizzes(u.UserID, quizzes.Select(q => q.QuizID).ToList(), 1, 3)).ToList();
            var videos = lections.SelectMany(l => MockDataGenerator.GenerateVideos(l.LectionID, 20)).ToList();

            modelBuilder.Entity<User>().HasData(users.ToArray());
            modelBuilder.Entity<Course>().HasData(courses.ToArray());
            modelBuilder.Entity<Lection>().HasData(lections.ToArray());
            modelBuilder.Entity<Quiz>().HasData(quizzes.ToArray());
            modelBuilder.Entity<QuizQuestion>().HasData(quizQuestions.ToArray());
            modelBuilder.Entity<QuizOption>().HasData(quizOptions.ToArray());
            modelBuilder.Entity<Bill>().HasData(bills.ToArray());
            modelBuilder.Entity<Payment>().HasData(payments.ToArray());
            modelBuilder.Entity<PaymentStatus>().HasData(paymentStatuses.ToArray());
            modelBuilder.Entity<Cart>().HasData(carts.ToArray());
            modelBuilder.Entity<CartHistory>().HasData(cartHistories.ToArray());
            modelBuilder.Entity<CartItem>().HasData(cartItems.ToArray());
            modelBuilder.Entity<Review>().HasData(reviews.ToArray());
            modelBuilder.Entity<UserCourse>().HasData(userCourses.ToArray());
            modelBuilder.Entity<UserQuiz>().HasData(userQuizzes.ToArray());
            modelBuilder.Entity<Video>().HasData(videos.ToArray());
            modelBuilder.Entity<UserLectionCompletion>().HasData(completions.ToArray());
        }

    }
}
