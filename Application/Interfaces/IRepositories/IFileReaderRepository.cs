using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IFileReaderRepository <TClass> where TClass : class
    {
       public Task<IEnumerable<TClass>> ReadFileAsync(string filepath, ClassMap classMap);

    }
}
