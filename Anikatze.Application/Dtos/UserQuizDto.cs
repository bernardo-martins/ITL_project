using System;

namespace Anikatze.Application.Dtos
{
    public record UserQuizDto(
        int UserID,
        string QuizGuid,
        int UserQuizID,
        string UserQuizGuid
        );
}
