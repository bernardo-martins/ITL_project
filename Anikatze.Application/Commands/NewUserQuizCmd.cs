using System;

namespace Anikatze.Application.Commands
{
    public record NewUserQuizCmd(
            string UserGuid, string QuizGuid)
        ;
}