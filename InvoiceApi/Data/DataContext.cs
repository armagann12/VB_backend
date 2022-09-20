using InvoiceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<InstitutionModel> InstitutionModels { get; set; }
        public DbSet<InvoiceModel> InvoiceModels { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<CreditCardModel> CreditCardModels { get; set; }

    }
}
