using FluentValidation;

using RPSSL.Application.Game;
using RPSSL.UI.Api.v1.Play.Models;

namespace RPSSL.UI.Api.v1.Play.Validation;

public sealed class PostPlayBodyValidator : AbstractValidator<PostPlayBody>
{
    private readonly IRuleBook _ruleBook;

    public PostPlayBodyValidator(IRuleBook ruleBook)
    {
        _ruleBook = ruleBook ?? throw new ArgumentNullException(nameof(ruleBook));

        DefineValidationRules();
    }

    private void DefineValidationRules()
    {
        RuleFor(x => x.Player).NotNull();

        When(x => x.Player != null, () =>
        {
            RuleFor(x => x.Player)
                .Must(x => _ruleBook.Choices.Any(choice => choice.Id == x.Value))
                .WithMessage(x => $"Failed to find a choice with id '${x.Player}'.");
        });
    }
}
