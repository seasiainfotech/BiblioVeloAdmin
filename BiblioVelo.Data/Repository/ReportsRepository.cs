using BiblioVelo.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioVelo.Services.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using AutoMapper;
using AutoMapper.Configuration;

namespace BiblioVelo.Data.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        BiblioveloEntities1 _dbEntity = new BiblioveloEntities1();
       
        /// <summary>
        /// GetReports
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportedReviews> GetReports()

        {
            try
            {
                var tblReportedRatings = _dbEntity.tblReportedReviews.ToList();
                if (tblReportedRatings.Count > 0)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblReportedReview, ReportedReviews>();
                    });
                    Mapper.Initialize(new MapperConfigurationExpression() { AllowNullCollections = true, AllowNullDestinationValues = true, CreateMissingTypeMaps = true });
                    var equipModel = Mapper.Instance.Map<List<ReportedReviews>>(tblReportedRatings);
                   foreach(var items in equipModel)
                    {
                        var ratings = _dbEntity.tblRatings.Where(x => x.Id == items.RatingId).FirstOrDefault();
                        if (ratings != null)
                        {
                            var ratedbyID = Convert.ToInt32(ratings.RatedBy);
                            var ratedtoID = Convert.ToInt32(ratings.RatedTo);
                          
                            var ratedbydetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == ratedbyID);
                            var ratedtodetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == ratedtoID);
                            items.Review = ratings.Review;
                            items.RatedBy = ratedbydetails.FirstName + " " + ratedbydetails.LastName;
                            items.RatedTo = ratedtodetails.FirstName + " " + ratedtodetails.LastName;
                        }
                        var reportedbyID = Convert.ToInt32(items.ReportedBy);
                        var reportedbydetails = _dbEntity.tblUsers.FirstOrDefault(x => x.Id == reportedbyID);
                        items.ReportedBy = reportedbydetails.FirstName + " " + reportedbydetails.LastName;
                    }
                    return equipModel;
                }
                return null;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                Common.ExcepLog(dbEx);
                throw;
            }
            catch (Exception ex)
            {
                Common.ExcepLog(ex);
                throw;
            }
        }
        /// <summary>
        /// DeleteReviewById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteReviewById(long id)
        {
            try
            {
                var report = _dbEntity.tblReportedReviews.FirstOrDefault(x => x.Id == id);
                var rating = _dbEntity.tblRatings.FirstOrDefault(x => x.Id == report.RatingId);
                _dbEntity.tblReportedReviews.Remove(report);
                _dbEntity.tblRatings.Remove(rating);
                _dbEntity.SaveChanges();
                return "Deleted";

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
