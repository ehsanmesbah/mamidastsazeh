using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IHashId
    {
        string Encode(int id);
        int Decode(string hashId);

    }
}
