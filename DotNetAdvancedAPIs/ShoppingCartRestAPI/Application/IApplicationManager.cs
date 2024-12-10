using Application.CartAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IApplicationManager
    {
        public ICartProvider CartProvider { get; }
    }
}
