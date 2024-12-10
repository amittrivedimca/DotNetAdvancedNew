using Application.CategoryAL;
using Application.ProductAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IApplicationManager
    {
        ICategoryProvider CategoryProvider { get; }
        IProductProvider ProductProvider { get; }
    }
}
