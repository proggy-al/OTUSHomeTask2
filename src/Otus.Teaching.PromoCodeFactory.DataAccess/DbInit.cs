using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DbInit(DatabaseContext context) : IDbInit
    {
        public void Init()
        {
            // Пересоздаем БД.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            // Заливаем данные.             
            context.Roles.AddRange(FakeDataFactory.Roles);
            context.Preferences.AddRange(FakeDataFactory.Preferences);
            context.Customers.AddRange(FakeDataFactory.Customers);
            context.Employees.AddRange(FakeDataFactory.Employees);            
            context.SaveChanges();
        }
    }
}
