using BiblioVelo.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioVelo.Services.Repository
{
    public interface IReportsRepository
    {
        IEnumerable<ReportedReviews> GetReports();
        string DeleteReviewById(long id);

    }
}
