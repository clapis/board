﻿using System;
using System.Linq;
using Board.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Database.Configurations
{
    public class JobConfiguration : BaseTypeConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Tags)
                 .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet());

        }
    }

}
