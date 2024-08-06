using System;

namespace Anikatze.Application.Dtos
{
    public record UserLectionCompletionsLastDto(
            int UserID, int LectionID,
            DateTime CompletionDate, TimeSpan TimeSpent, string Title)
            ;
}
