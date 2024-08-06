using System;

namespace Anikatze.Application.Dtos
{
    public record UserLectionCompletionsTimeDto(
            DateTime CompletionDate, TimeSpan TimeSpent)
        ;
}