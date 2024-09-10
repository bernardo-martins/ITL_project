using System;

namespace Anikatze.Application.Dtos
{
    public record AllUsersDto(string UserGuid, string Username, string Email, string password)
    {
        public AllUsersDto()
        {
        }
    };
}
