using BiblioVelo.Services.Services;
using System.Collections.Generic;
using BiblioVelo.Services.Models;
using BiblioVelo.Services.Repository;
using BiblioVelo.Data.Repository;

namespace BiblioVelo.BusinessLogic.Services
{
    public class ReportsService : IReportsService
    {
        /// <summary>
        /// _reportsRepo
        /// </summary>
        IReportsRepository _reportsRepo = new ReportsRepository();
        /// <summary>
        /// GetReports
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportedReviews> GetReports()
        {
            var listdisputes = _reportsRepo.GetReports();
            return listdisputes;
        }
        /// <summary>
        /// DeleteReviewById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteReviewById(long id)
        {
            return _reportsRepo.DeleteReviewById(id);
             
        }

    }
}
   