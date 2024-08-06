using System;

namespace Anikatze.Application.Dtos
{
    public record QuestionDto(
            string QuestionText, string QuestionGuid, List<string> Answers)
        ;
}