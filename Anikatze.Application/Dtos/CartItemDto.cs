using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anikatze.Application.Dtos
{
    public record CartItemDto(
        int CartItemId, string Name, double Price);
    
}
