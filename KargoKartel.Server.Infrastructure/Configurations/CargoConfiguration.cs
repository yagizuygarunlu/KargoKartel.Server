using KargoKartel.Server.Domain.Cargos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KargoKartel.Server.Infrastructure.Configurations
{
    public sealed class CargoConfiguration: IEntityTypeConfiguration<Cargo>
    {
       public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.OwnsOne(c => c.Sender);
            builder.OwnsOne(c => c.Receiver);
            builder.OwnsOne(c => c.ReceiveAddress);

            //Smart Enum conversion for CargoType
            builder.OwnsOne(c => c.CargoInformation, builder =>
            {
                builder
                .Property(p => p.CargoType)
                .HasConversion(t => t.Value, value => CargoType.FromValue(value));
            });

            //Smart Enum conversion for Status
            builder
                .Property(c => c.Status)
                .HasConversion(
                    status => status.Value,
                    value => Status.FromValue(value))
                .HasMaxLength(50);
        }
    }
}
