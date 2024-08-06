using System;

namespace Anikatze.Application.Dtos
{
    public record LectionsDto(
            int CourseId, int LectionId, string LectionGuid,
            string Title)
        ;
}