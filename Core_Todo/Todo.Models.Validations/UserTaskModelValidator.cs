using FluentValidation;

namespace Todo.Models.Validations
{
    public class UserTaskModelValidator : AbstractValidator<UserTaskModel>
    {
        public UserTaskModelValidator()
        {
            RuleFor(reg => reg.Title).NotEmpty().WithName("Title").WithMessage("{PropertyName} is required");
            RuleFor(reg => reg.Description).NotEmpty().WithMessage("Description is required");
            //RuleFor(model => model).Custom((model, context) => { //MY SUPER CUSTOM VALIDATION };)
        }
    }
}
