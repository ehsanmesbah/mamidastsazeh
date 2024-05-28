using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public interface IReportRepository
    {
        IQueryable<Report> Reports { get; }
        void SaveChanges();
        void Add(Report report);
    }
}
