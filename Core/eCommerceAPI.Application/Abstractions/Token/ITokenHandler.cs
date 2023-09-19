using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T = eCommerceAPI.Application.DTOs;

namespace eCommerceAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        T::Token CreateToken(int seconds);
    }
}
