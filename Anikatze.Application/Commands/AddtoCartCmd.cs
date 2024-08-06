using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anikatze.Application.Commands
{
    public record AddtoCartCmd( int CourseId, 
        string Name, double Price
        );
   
}
