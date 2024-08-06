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
    public class UserLectionCompletionService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;

        public UserLectionCompletionService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        IQueryable<UserLectionCompletion> UserLectionCompletions => _db.Set<UserLectionCompletion>().AsQueryable();
        IQueryable<Lection> Lections => _db.Set<Lection>().AsQueryable();
        IQueryable<User> Users => _db.Set<User>().AsQueryable();

        public IEnumerable GetAllCompletions()
        {
            var completions = _mapper.ProjectTo<UserLectionCompletionsDto>(UserLectionCompletions)
                .ToList();
            return completions;
        }

        public IEnumerable GetAllCompletionsByUserId(string UserGuid)
        {
            var userId = Users
                .Where(u => u.UserGuid == UserGuid)
                .Select(u => u.UserID)
                .FirstOrDefault();
            var completions = UserLectionCompletions
                .Where(u => u.UserID == userId)
                .Select(u => new UserLectionCompletionsDto(u.UserID, u.LectionID, u.CompletionDate, u.TimeSpent))
                .ToList();
            return completions;
        }
        
        
        public string AddCompletion(NewUserLectionCompletionCmd cmd)
        {
            var lectionId = Lections
                .Where(l => l.LectionGuid == cmd.LectionGuid)
                .Select(l => l.LectionID)
                .FirstOrDefault();
            var userId = Users
                .Where(u => u.UserGuid == cmd.UserGuid)
                .Select(u => u.UserID)
                .FirstOrDefault();
            var completion = new UserLectionCompletion
            {
                UserID = userId,
                LectionID = lectionId,
                CompletionDate = DateTime.UtcNow,
                TimeSpent = cmd.TimeSpent,
                UserLectionCompletionGuid = Guid.NewGuid().ToString()
            };
            _db.UserLectionCompletions.Add(completion);
            _db.SaveChanges();
            return completion.UserLectionCompletionGuid;
        }

        public IEnumerable LastThreeCompletions(string userGuid)
        {
            var userid = Users
                .Where(u => u.UserGuid == userGuid)
                .Select(u => u.UserID)
                .FirstOrDefault();
            var completions = UserLectionCompletions
                .Where(u => u.UserID == userid)
                .Join(Lections, c => c.LectionID, lection => lection.LectionID,
                    (c, lection) => new { c, lection })
                .Select(u => new UserLectionCompletionsLastDto(u.c.UserID, u.c.LectionID, u.c.CompletionDate, u.c.TimeSpent, u.lection.Title))
                .ToList()
                .OrderByDescending(u => u.CompletionDate)
                .Take(3);
            return completions;
        }
        
        public IEnumerable GetCompletionsTime(int userid)
        {
            var completions = UserLectionCompletions
                .Where(u => u.UserID == userid)
                .GroupBy(u => u.CompletionDate.Month)
                .Select(u => new { Month = u.Key, TimeSpent = u.Sum(t => t.TimeSpent.TotalMinutes) })
                .ToList();
            return completions;
        }

        public bool IsCompleted(string userGuid, string lectionGuid)
        {
            var lectionId = Lections
                .Where(l => l.LectionGuid == lectionGuid)
                .Select(l => l.LectionID)
                .FirstOrDefault();
            var userId = Users.Where(u => u.UserGuid == userGuid)
                .Select(u => u.UserID)
                .FirstOrDefault();
            var completion = UserLectionCompletions
                .Where(u => u.UserID == userId && u.LectionID == lectionId)
                .FirstOrDefault();
            return completion != null;
        }
    }
}
