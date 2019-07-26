using Microsoft.EntityFrameworkCore;
using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public interface IBugReportRater
    {
        void RateBugReport(BugReport bugReport, User user, bool hasAgreed);
    }

    public interface IBugReportFinder
    {
        IEnumerable<BugReport> GetBugReports(Expression<Func<BugReport, bool>> predicate);
    }

    public interface IBugReportCreator
    {
        void CreateBugReport(BugReport bugReport);
    }

    public class BugReportStorage : IBugReportCreator, IBugReportFinder, IBugReportRater
    {
        private RoadStateContext _context;
        public BugReportStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public void CreateBugReport(BugReport bugReport)
        {
            this._context.BugReports.AddAsync(bugReport);
            this._context.SaveChangesAsync();
        }

        public void RateBugReport(BugReport bugReport, User user, bool hasAgreed)
        {
            this._context.BugReportRates.AddAsync(new BugReportRate
            {
                User = user,
                UserId = user.Id,
                BugReport = bugReport,
                BugReportId = bugReport.Id,
                HasAgreed = hasAgreed,
            });
            this._context.SaveChangesAsync();
        }

        public IEnumerable<BugReport> GetBugReports(Expression<Func<BugReport, bool>> predicate)
        {
            return this._context.BugReports.Include(x => x.Author).Where(predicate);
        }
    }
}
