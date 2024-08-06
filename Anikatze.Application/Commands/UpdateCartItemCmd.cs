using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anikatze.Application.Commands
{
    public record UpdateCartItemCmd(
        int CourseGuid, string Name, double Price);

    
}
