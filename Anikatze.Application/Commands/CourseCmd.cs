using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anikatze.Application.Commands
{
    public record CourseCmd(
       [StringLength(255, MinimumLength = 1)] string Name,
        [StringLength(255, MinimumLength = 1)] string Description, double Price);
    
}
