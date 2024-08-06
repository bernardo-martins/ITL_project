using System;

namespace Anikatze.Application.Dtos
{
    public record LectionDto(
            string LectionGuid, string Title,
            string? VideoID, string Text)
        ;
}