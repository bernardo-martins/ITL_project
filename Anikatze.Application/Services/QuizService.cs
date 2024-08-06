using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Anikatze.Application.Dtos;
using static System.Collections.Specialized.BitVector32;
using Anikatze.Application.Commands;

namespace Anikatze.Application.Services
{
    public class QuizService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;

        public QuizService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public IQueryable<Quiz> Quizzes => _db.Set<Quiz>().AsQueryable();
        public IQueryable<QuizQuestion> QuizQuestions => _db.Set<QuizQuestion>().AsQueryable();
        public IQueryable<QuizOption> QuizOptions => _db.Set<QuizOption>().AsQueryable();
        public IQueryable<Lection> Lections => _db.Set<Lection>().AsQueryable();


        public IEnumerable<QuizDto> GetQuizzesByLectionGuid(string lectionGuid)
        {
            var lectionid = Lections.Where(l => l.LectionGuid == lectionGuid).Select(l => l.LectionID).FirstOrDefault();
            var quizData = Quizzes
                .Where(q => q.LectionID == lectionid)
                .Join(QuizQuestions, quiz => quiz.QuizID, question => question.QuizID, (quiz, question) => new { quiz, question })
                .Join(QuizOptions, qq => qq.question.QuizQuestionID, option => option.QuizQuestionID, (qq, option) => new { qq.quiz, qq.question, option })
                .ToList();

            var quizzes = quizData
                .GroupBy(x => x.quiz.QuizGuid)
                .Select(g => new QuizDto(
                    g.First().quiz.Title,
                    g.Key,
                    g.GroupBy(q => q.question.QuizQuestionGuid)
                        .Select(qg => new QuestionDto(
                            qg.First().question.QuestionText,
                            qg.Key,
                            qg.Select(x => x.option.OptionText).ToList()
                            
                        )).ToList()
                )).ToList();

            return quizzes;
        }

        public List<bool> CheckUserAnswers(string quizGuid, Dictionary<string, string> userAnswers)
        {
            var correctAnswers = QuizQuestions
                .Where(q => q.QuizQuestionGuid == quizGuid)
                .Join(QuizOptions, question => question.QuizQuestionID, option => option.QuizQuestionID, (question, option) => new { question, option })
                .Where(x => x.option.IsCorrect)
                .ToList();

            Console.WriteLine("Correct Answers:");
            foreach (var correct in correctAnswers)
            {
                Console.WriteLine($"QuestionID: {correct.question.QuizQuestionID}, Answer: {correct.option.OptionText}");
            }

            var result = new List<bool>();

            foreach (var answer in userAnswers)
            {
                Console.WriteLine($"User Answer - QuestionID: {answer.Key}, Answer: {answer.Value}");
                var correctAnswer = correctAnswers.FirstOrDefault(x => x.question.QuizQuestionGuid == answer.Key);
                if (correctAnswer == null || correctAnswer.option.OptionText != answer.Value)
                {
                    result.Add(false);
                }
                else
                {
                    result.Add(true);
                }
            }

            return result;
        }
        

    }
}
