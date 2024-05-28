using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using mamidastsazeh.Models;
namespace mamidastsazeh.Abstractions
{
    public interface IMamiEventRepository 
    {
        IQueryable<MamiEvent> MamiEvents { get;  }
        void SaveChanges();
        void Add(MamiEvent mamiEvent);
        
    }
}
