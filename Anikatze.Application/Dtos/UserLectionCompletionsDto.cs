using System;

namespace Anikatze.Application.Dtos
{
    public record UserLectionCompletionsDto(
            int UserID, int LectionID,
            DateTime CompletionDate, TimeSpan TimeSpent)
            ;
}