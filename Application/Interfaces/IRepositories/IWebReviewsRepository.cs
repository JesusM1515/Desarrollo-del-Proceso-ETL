using Domain.Entities.DWH;
using Domain.Entities.DWH.Dimension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IWebReviewsRepository
    {
        public Task<IEnumerable<EWebReviews>> GetAsync();
    }
}
