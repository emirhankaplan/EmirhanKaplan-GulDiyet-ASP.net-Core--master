using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Evaluation;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IEvaluationCommandService
    {
        Task Add(SaveEvaluationViewModel vm);
        Task Update(SaveEvaluationViewModel vm);
        Task Delete(int id);
    }
}
