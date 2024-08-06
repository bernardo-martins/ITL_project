using System;

namespace Anikatze.Application.Dtos
{
    public record OnlyUserDto(
            string UserName, string Email,
            string UserGuid)
        ;
}