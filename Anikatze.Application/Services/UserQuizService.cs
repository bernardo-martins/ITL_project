using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Anikatze.Application.Commands;
using Anikatze.Application.Dtos;

namespace Anikatze.Application.Services
{
    public class UserQuizService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;

        public UserQuizService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        IQueryable<UserQuiz> UserQuizzes => _db.Set<UserQuiz>().AsQueryable();
        IQueryable<Quiz> Quizzes => _db.Set<Quiz>().AsQueryable();
        IQueryable<User> Users => _db.Set<User>().AsQueryable();

        public string AddUserQuizCompletion(NewUserQuizCmd cmd)
        {   
            var userId = Users.Where(u => u.UserGuid == cmd.UserGuid).Select(u => u.UserID).FirstOrDefault();
            var quizId = Quizzes.Where(q => q.QuizGuid == cmd.QuizGuid).Select(q => q.QuizID).FirstOrDefault();
            if(UserQuizzes.Any(uq => uq.UserID == userId && uq.QuizID == quizId))
            {
                throw new Exception("User already completed this quiz");
            }
            
            var newUserQuiz = new UserQuiz
            {
                UserQuizGuid = Guid.NewGuid().ToString(), // Generiere eine neue GUID für das UserQuiz
                UserID = userId,
                QuizID = quizId,
            };
            _db.UserQuizzes.Add(newUserQuiz);
            _db.SaveChanges();
            return newUserQuiz.UserQuizGuid;
        }

        public IEnumerable<UserQuizDto> GetAllUserQuizzesByUserId(string userGuid)
        {
            var userId = Users.Where(u => u.UserGuid == userGuid).Select(u => u.UserID).FirstOrDefault();
            var userQuizzes = UserQuizzes.Where(uq => uq.UserID == userId).Join(Quizzes, userquiz => userquiz.QuizID,
                quiz => quiz.QuizID, (userquiz, quiz) => new { userquiz, quiz }).Select(f => new UserQuizDto(
                f.userquiz.UserID,
                f.quiz.QuizGuid,
                f.userquiz.UserQuizID,
                f.userquiz.UserQuizGuid
            ));
            var userQuizDtos = _mapper.Map<IEnumerable<UserQuizDto>>(userQuizzes);
            return userQuizDtos;
        }
        
        public object CheckUserQuizzesWithLectionQuiz(int userId, int lectionId)
        {
            var completedQuizIds = UserQuizzes
                .Where(uq => uq.UserID == userId)
                .Select(uq => uq.QuizID)
                .ToList();

            var lectionQuizzes = Quizzes
                .Where(q => q.LectionID == lectionId && !completedQuizIds.Contains(q.QuizID))
                .Select(f => new { f.QuizID })
                .ToList();

            if (lectionQuizzes.Count == 1)
            {
                return lectionQuizzes[0];
            }
            
            return lectionQuizzes;
        }
    }
}