using System;

namespace Anikatze.Application.Commands
{
    public record NewUserLectionCompletionCmd(
            string LectionGuid, TimeSpan TimeSpent, string UserGuid )
        ;
}