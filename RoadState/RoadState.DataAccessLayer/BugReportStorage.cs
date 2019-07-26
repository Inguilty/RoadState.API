using RoadState.Data;
using RoadState.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class BugReportStorage : IBugReportFinder, IBugReportCreator, IBugReportRater
    {
        private RoadStateContext _context;
        public BugReportStorage(RoadStateContext context)
        {
            this._context = context;
        }

        public void CreateBugReport(BugReport bugReport)
        {
            this._context.BugReports.Add(bugReport);
            this._context.SaveChanges();
        }

        public void RateBugReport(BugReport bugReport, User user, bool hasAgreed)
        {
            this._context.BugReportRates.Add(new BugReportRate
            {
                User = user,
                UserId = user.Id,
                BugReport = bugReport,
                BugReportId = bugReport.Id,
                HasAgreed = hasAgreed,
            });
            this._context.SaveChanges();
        }

        public IEnumerable<BugReport> GetBugReports(Expression<Func<BugReport, bool>> predicate)
        {
            return this._context.BugReports.Where(predicate);
        }
    }
}
