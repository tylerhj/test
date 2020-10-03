using System.Threading.Tasks;
using DiveCRM.Configuration.Dto;

namespace DiveCRM.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
