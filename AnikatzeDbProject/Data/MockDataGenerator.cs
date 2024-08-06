using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using AnikatzeDbProject.Model;

namespace AnikatzeDbProject.Data
{
    public static class MockDataGenerator
    {
        private static int _currentId = 1;

        public static List<User> GenerateUsers(int count)
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.UserID, f => _currentId++)
                .RuleFor(u => u.UserGuid, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName());

            return faker.Generate(count);
        }

        public static List<Course> GenerateCourses(int count)
        {
            var faker = new Faker<Course>()
                .RuleFor(c => c.CourseID, f => _currentId++)
                .RuleFor(c => c.CourseGuid, f => Guid.NewGuid().ToString())
                .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                .RuleFor(c => c.Description, f => f.Lorem.Paragraph());

            return faker.Generate(count);
        }

        public static List<Lection> GenerateLections(int courseId, int count)
        {
            var faker = new Faker<Lection>()
                .RuleFor(l => l.LectionID, f => _currentId++)
                .RuleFor(l => l.LectionGuid, f => Guid.NewGuid().ToString())
                .RuleFor(l => l.Title, f => f.Lorem.Sentence())
                .RuleFor(l => l.Text, f => f.Lorem.Paragraphs())
                .RuleFor(l => l.CourseID, f => courseId);

            return faker.Generate(count);
        }

        public static List<Quiz> GenerateQuizzes(int lectionId, int count)
        {
            var faker = new Faker<Quiz>()
                .RuleFor(q => q.QuizID, f => _currentId++)
                .RuleFor(q => q.QuizGuid, f => Guid.NewGuid().ToString())
                .RuleFor(q => q.Title, f => f.Lorem.Sentence())
                .RuleFor(q => q.LectionID, f => lectionId);

            return faker.Generate(count);
        }

        public static List<QuizQuestion> GenerateQuizQuestions(int quizId, int count)
        {
            var faker = new Faker<QuizQuestion>()
                .RuleFor(qq => qq.QuizQuestionID, f => _currentId++)
                .RuleFor(qq => qq.QuizQuestionGuid, f => Guid.NewGuid().ToString())
                .RuleFor(qq => qq.QuizID, f => quizId)
                .RuleFor(qq => qq.QuestionText, f => f.Lorem.Sentence());

            return faker.Generate(count);
        }

        public static List<QuizOption> GenerateQuizOptions(int quizQuestionId, int count)
        {
            var faker = new Faker<QuizOption>()
                .RuleFor(qo => qo.QuizOptionID, f => _currentId++)
                .RuleFor(qo => qo.QuizOptionGuid, f => Guid.NewGuid().ToString())
                .RuleFor(qo => qo.QuizQuestionID, f => quizQuestionId)
                .RuleFor(qo => qo.OptionText, f => f.Lorem.Word())
                .RuleFor(qo => qo.IsCorrect, f => f.Random.Bool(0.2f));

            return faker.Generate(count);
        }

        public static List<Bill> GenerateBills(int userId, int count)
        {
            var faker = new Faker<Bill>()
                .RuleFor(b => b.BillID, f => _currentId++)
                .RuleFor(b => b.BillGuid, f => Guid.NewGuid().ToString())
                .RuleFor(b => b.UserID, f => userId)
                .RuleFor(b => b.IssuedAt, f => f.Date.Past().ToUniversalTime()); // Ensure UTC

            return faker.Generate(count);
        }

        public static List<Cart> GenerateCarts(int userId)
        {
            var faker = new Faker<Cart>()
                .RuleFor(c => c.CartID, f => _currentId++)
                .RuleFor(c => c.CartGuid, f => Guid.NewGuid().ToString())
                .RuleFor(c => c.UserID, f => userId)
                .RuleFor(c => c.CreatedAt, f => f.Date.Past().ToUniversalTime()); // Ensure UTC

            return faker.Generate(1);
        }

        public static List<CartHistory> GenerateCartHistories(int userId, int minCount, int maxCount)
        {
            var count = new Random().Next(minCount, maxCount + 1);
            var faker = new Faker<CartHistory>()
                .RuleFor(ch => ch.CartHistoryID, f => _currentId++)
                .RuleFor(ch => ch.CartHistoryGuid, f => Guid.NewGuid().ToString())
                .RuleFor(ch => ch.UserID, f => userId)
                .RuleFor(ch => ch.ArchivedAt, f => f.Date.Past().ToUniversalTime()); // Ensure UTC

            return faker.Generate(count);
        }

        public static List<CartItem> GenerateCartItems(int cartId, List<int> courseIds, int minCount, int maxCount)
        {
            var count = new Random().Next(minCount, maxCount + 1);
            var faker = new Faker<CartItem>()
                .RuleFor(ci => ci.CartItemID, f => _currentId++)
                .RuleFor(ci => ci.CartItemGuid, f => Guid.NewGuid().ToString())
                .RuleFor(ci => ci.CartID, f => cartId)
                .RuleFor(ci => ci.CourseID, f => f.PickRandom(courseIds))
                .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 5))
                .RuleFor(ci => ci.AddedAt, f => f.Date.Past().ToUniversalTime()); // Ensure UTC

            return faker.Generate(count);
        }

        public static List<Review> GenerateReviews(int courseId, List<int> userIds, int count)
        {
            var faker = new Faker<Review>()
                .RuleFor(r => r.ReviewID, f => _currentId++)
                .RuleFor(r => r.ReviewGuid, f => Guid.NewGuid().ToString())
                .RuleFor(r => r.CourseID, f => courseId)
                .RuleFor(r => r.UserID, f => f.PickRandom(userIds))
                .RuleFor(r => r.Content, f => f.Lorem.Paragraph())
                .RuleFor(r => r.Rating, f => f.Random.Int(1, 5));

            return faker.Generate(count);
        }

        public static List<UserCourse> GenerateUserCourses(int userId, List<int> courseIds, int minCount, int maxCount)
        {
            var count = new Random().Next(minCount, maxCount + 1);
            var selectedCourses = new HashSet<int>();

            while (selectedCourses.Count < count)
            {
                selectedCourses.Add(courseIds[new Random().Next(courseIds.Count)]);
            }

            var selectedCoursesList = selectedCourses.ToList(); // Convert to list

            var faker = new Faker<UserCourse>()
                .RuleFor(uc => uc.UserID, f => userId)  // Ensure userId is correctly assigned using lambda
                .RuleFor(uc => uc.CourseID, f => f.PickRandom(selectedCoursesList))  // Correct usage with f
                .RuleFor(uc => uc.UserCourseGuid, f => Guid.NewGuid().ToString());  // Correct usage with f

            return faker.Generate(selectedCourses.Count);
        }

        public static List<UserQuiz> GenerateUserQuizzes(int userId, List<int> quizIds, int minCount, int maxCount)
        {
            var count = new Random().Next(minCount, maxCount + 1);
            var faker = new Faker<UserQuiz>()
                .RuleFor(uq => uq.UserQuizID, f => _currentId++)
                .RuleFor(uq => uq.UserQuizGuid, f => Guid.NewGuid().ToString())
                .RuleFor(uq => uq.UserID, f => userId)
                .RuleFor(uq => uq.QuizID, f => f.PickRandom(quizIds));

            return faker.Generate(count);
        }

        public static List<Video> GenerateVideos(int lectionId, int count)
        {
            var faker = new Faker<Video>()
                .RuleFor(v => v.VideoID, f => _currentId++)
                .RuleFor(v => v.VideoGuid, f => Guid.NewGuid().ToString())
                .RuleFor(v => v.Title, f => f.Lorem.Sentence())
                .RuleFor(v => v.LectionID, f => lectionId);

            return faker.Generate(count);
        }

        public static List<Payment> GeneratePayments(int billId, List<PaymentStatus> paymentStatuses, int count)
        {
            var faker = new Faker<Payment>()
                .RuleFor(p => p.PaymentID, f => _currentId++)
                .RuleFor(p => p.PaymentGuid, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.BillID, f => billId)
                .RuleFor(p => p.PaymentStatusID, f => f.PickRandom(paymentStatuses).PaymentStatusID)
                .RuleFor(p => p.Amount, f => f.Finance.Amount(100, 1000))
                .RuleFor(p => p.PaidAt, f => f.Date.Past().ToUniversalTime()); // Ensure UTC

            return faker.Generate(count);
        }
        //von niklas
        public static List<UserLectionCompletion> GenerateUserLectionCompletions(List<User> users, List<Lection> lections, List<UserCourse> userCourses)
        {
            var completions = new List<UserLectionCompletion>();
            var random = new Random();

            foreach (var user in users)
            {
                var userCourseIds = userCourses
                    .Where(uc => uc.UserID == user.UserID)
                    .Select(uc => uc.CourseID)
                    .ToList();

                var completedLections = new HashSet<int>();

                while (completedLections.Count < random.Next(5, 9))
                {
                    var courseId = userCourseIds[random.Next(userCourseIds.Count)];
                    var lectionsInCourse = lections.Where(l => l.CourseID == courseId).ToList();

                    if (lectionsInCourse.Count > 0)
                    {
                        var lection = lectionsInCourse[random.Next(lectionsInCourse.Count)];

                        if (!completedLections.Contains(lection.LectionID))
                        {
                            var faker = new Faker<UserLectionCompletion>()
                                .RuleFor(ulc => ulc.UserLectionCompletionID, f => _currentId++)
                                .RuleFor(ulc => ulc.UserID, f => user.UserID)
                                .RuleFor(ulc => ulc.LectionID, f => lection.LectionID)
                                .RuleFor(ulc => ulc.CompletionDate, f => f.Date.Past().ToUniversalTime())
                                .RuleFor(ulc => ulc.TimeSpent, f => TimeSpan.FromMinutes(random.Next(1, 120)));

                            completions.Add(faker.Generate());
                            completedLections.Add(lection.LectionID);
                        }
                    }
                }
            }

            return completions;
        }
            //von niklas ende
            public static List<PaymentStatus> GeneratePaymentStatuses()
        {
            var statuses = new List<PaymentStatus>
            {
                new PaymentStatus { PaymentStatusID = _currentId++, PaymentStatusGuid = Guid.NewGuid().ToString(), Status = "Pending" },
                new PaymentStatus { PaymentStatusID = _currentId++, PaymentStatusGuid = Guid.NewGuid().ToString(), Status = "Completed" },
                new PaymentStatus { PaymentStatusID = _currentId++, PaymentStatusGuid = Guid.NewGuid().ToString(), Status = "Failed" }
            };

            return statuses;
        }
    }
}
