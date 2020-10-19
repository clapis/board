using Board.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Database.Configurations
{
    public class ReceivedPaymentConfiguration : BaseTypeConfiguration<ReceivedPayment>
    {
        public override void Configure(EntityTypeBuilder<ReceivedPayment> builder)
        {
            base.Configure(builder);
        }
    }
}
