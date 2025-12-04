using Application.Interfaces;
using Application.Services;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            ISurveys pruebaread = new CSVSurveysServices();

            pruebaread.GetSurveysAsync().Wait();
        }
    }
}
