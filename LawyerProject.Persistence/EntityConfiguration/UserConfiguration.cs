using LawyerProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Persistence.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.ObjectId);
            builder.Property(u => u.FirstName).IsRequired()
                .HasMaxLength(50);
            builder.Property(u=>u.LastName).IsRequired()
                .HasMaxLength(50);
            builder.Property(u=>u.UserName).IsRequired()
                .HasMaxLength(50);
            builder.Property(u=>u.Email).IsRequired()
                .HasMaxLength(50);
            builder.Property(u=>u.PhoneNumber).IsRequired()
                .HasMaxLength(25);
            builder.Property(u=>u.PasswordHash).IsRequired()
                .HasMaxLength(250);
            builder.Property(u => u.ProfileImage).IsRequired()
                .HasMaxLength(250);
            builder.Property(u => u.IsAdmin).IsRequired();
            builder.HasMany(u => u.Cases).WithOne(c => c.User).HasForeignKey(u => u.IdUserFK);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.DataState).IsRequired();
            builder.Property(a => a.UpdatedDate).IsRequired(false);
        }
    }
}
