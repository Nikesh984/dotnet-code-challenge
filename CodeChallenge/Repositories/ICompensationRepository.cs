using System.ComponentModel.Design.Serialization;
using System.Threading.Tasks;
using CodeChallenge.Models;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Create(Compensation compensation);
        Compensation GetCompensationByEmployeeId(string id);
    }
}