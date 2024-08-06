using System;

namespace Anikatze.Application.Dtos
{
    public record QuizDto(
            string Title, string QuizGuid, List<QuestionDto> Questions)
        ;
}