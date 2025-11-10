using Domain.Entities.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ISocialCommentsRepository
    {
        public Task<IEnumerable<ESocialComments>> GetSocialCommentsAsync();
    }
}
