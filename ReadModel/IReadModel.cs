using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadModel
{
    public interface IReadModel
    {
        object Get(string identifier);
    }
}
