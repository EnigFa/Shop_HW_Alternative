using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.App.Configurators;

public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
{
    public void Configure(EntityTypeBuilder<CategoryProduct> builder)
    {
        builder.HasKey(cp => new { cp.ProductId, cp.CategoryId });

        builder
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(cp => cp.ProductId);
        builder
            .HasOne(cp => cp.Category)
            .WithMany(c => c.CategoryProducts)
            .HasForeignKey(cp => cp.CategoryId);
    }
}
