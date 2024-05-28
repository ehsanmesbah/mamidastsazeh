using mamidastsazeh.Abstractions;
using mamidastsazeh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Data
{
    public class ReportRepository:IReportRepository
    {
        private readonly mamidastsazehContext _context;
        public ReportRepository(mamidastsazehContext context)
        {
            _context = context;
        }
        public IQueryable<Report> Reports => _context.Report;
        public void SaveChanges()
        {
            _context.SaveChanges();

        }
        public void Add(Report report)
        {
            _context.Add(report);
        }

    }
}
