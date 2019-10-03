using WeeklyNotification.App.DAL.Entities;

namespace WeeklyNotification.App.DAL
{
    using Microsoft.EntityFrameworkCore;

    public class ZeuxDbContext : DbContext
    {
        public ZeuxDbContext(DbContextOptions<ZeuxDbContext> options) : base(options) { }
        public DbSet<CustomerMessage> CustomerMessages { get; set; }
        public DbSet<CryptoExchangeRate> CryptoExchangeRates { get; set; }
        public DbSet<FiatDepositInvestmentOrder> FiatDepositInvestmentOrders { get; set; }
        public DbSet<CryptoDepositInvestmentOrder> CryptoDepositInvestmentOrders { get; set; }
        public DbSet<FiatFundInvestmentOrder> FiatFundInvestmentOrders { get; set; }
        public DbSet<CryptoFundInvestmentOrder> CryptoFundInvestmentOrders { get; set; }
        public DbSet<DepositProduct> DepositProducts { get; set; }
        public DbSet<FundProduct> FundProducts { get; set; }
    }
}
