using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IsmsTokenRepository
    {
        IQueryable<SmsToken> SmsTokens { get; }
        public int SaveChanges();
        public void Add(SmsToken entity);
    }

}
