using eCommerceAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<Vm_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Ad alanı boş geçilemez!")
                .MinimumLength(5)
                .MaximumLength(180)
                    .WithMessage("Ad  alanı 5-185 karakter arası olmalıdır!");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Fiyat alanı boş geçilemez!")
                .Must(f => f >= 0)
                    .WithMessage("Fiyat bilgisi negatif olamaz!");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stok alanı boş geçilemez!")
                .Must(f => f >= 0)
                    .WithMessage("Stok bilgisi negatif olamaz!");
        }
    }
}
