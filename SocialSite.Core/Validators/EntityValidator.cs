using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace SocialSite.Core.Validators;

public class EntityValidator(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public virtual ValidationResult Validate<TValidator, TModel>(TModel model, params string[] ruleSets)
        where TValidator : AbstractValidator<TModel>
        where TModel : class
    {
        var validator = _serviceProvider.GetService(typeof(TValidator)) as TValidator
            ?? throw new NotSupportedException("Validator of this type was not found.");

        var context = ruleSets?.Length > 0
            ? new ValidationContext<TModel>(model, new PropertyChain(), new RulesetValidatorSelector(ruleSets))
            : new ValidationContext<TModel>(model);

        return validator.Validate(context);
    }
}
