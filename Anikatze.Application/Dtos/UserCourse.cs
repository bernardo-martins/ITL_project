using System;

namespace Anikatze.Application.Dtos
{
    public record UserCourseDto(
            int UserID, string Username,
            string CourseGuid, string CourseDescription)
            ;
}