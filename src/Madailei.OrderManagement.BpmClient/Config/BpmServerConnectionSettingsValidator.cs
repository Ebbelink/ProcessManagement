using FluentValidation;

namespace Madailei.ProcessManagement.BpmClient.Config
{
    public class BpmServerConnectionSettingsValidator: AbstractValidator<BpmServerConnectionSettings>
    {
        public BpmServerConnectionSettingsValidator()
        {
            RuleFor(c => c).NotNull();
            RuleFor(c => c.AuthServer).NotEmpty();
            RuleFor(c => c.ClientId).NotEmpty();
            RuleFor(c => c.ClientSecret).NotEmpty();
            RuleFor(c => c.Address).NotEmpty();
        }
    }
}
