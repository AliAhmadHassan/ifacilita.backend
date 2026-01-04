using RoboAntiCaptchaModel.Dto;
using System.Threading.Tasks;

namespace RoboAntiCaptchaDomain.Interfaces
{
    public interface IProcessServiceBase<TConfig, TEntity>
    {
        Task<ValidationField> ExecuteWrite();

        Task<bool> ExecuteRead(bool confirmSubmit = true);

        Task<ValidationField> Submit();

        Task<bool> Clear();

        Task<bool> FillForm();
    }
}
