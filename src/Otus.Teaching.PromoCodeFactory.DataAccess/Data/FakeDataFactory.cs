using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {

        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                Role = Roles.Where(x => x.Name == "Admin").FirstOrDefault(),

                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                Role = Roles.Where(x => x.Name == "PartnerManager").FirstOrDefault(),
                AppliedPromocodesCount = 10
            },
        };

        // костыль. Иначе ошибка трекинга данных
        // instance of entity type cannot be tracked because another instance with same key value is tracked
        static List<Role> RolesEmpty = null;

        public static IEnumerable<Role> Roles => RolesEmpty ?? (RolesEmpty = new List<Role>()
        {
            new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        });

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0");
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = customerId,
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                        CustomerPreferences = new List<CustomerPreference>()
                        {
                            new CustomerPreference()
                            {
                                Id = Guid.NewGuid(),
                                CustomerId = customerId,
                                PreferenceId = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84")
                            },
                            new CustomerPreference()
                            {
                                Id = Guid.NewGuid(),
                                CustomerId = customerId,
                                PreferenceId = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd")
                            }
                        }
                    },
                    new Customer()
                    {
                        Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f1"),
                        Email = "sergey_ivan@mail.ru",
                        FirstName = "Петр",
                        LastName = "Иванов",
                        //TODO: Добавить предзаполненный список предпочтений
                        CustomerPreferences = new List<CustomerPreference>()
                        {
                            new CustomerPreference()
                            {
                                Id = Guid.NewGuid(),
                                CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f1"),
                                PreferenceId = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84")
                            }
                        }
                    },
                    new Customer()
                    {
                        Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f2"),
                        Email = "ivan@mail.ru",
                        FirstName = "Иван",
                        LastName = "Иванов",
                        //TODO: Добавить предзаполненный список предпочтений
                        CustomerPreferences = new List<CustomerPreference>()
                        {
                            new CustomerPreference()
                            {
                                Id = Guid.NewGuid(),
                                CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f2"),
                                PreferenceId = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd")
                            },
                            new CustomerPreference()
                            {
                                Id = Guid.NewGuid(),
                                CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f2"),
                                PreferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c")
                            }
                        }
                    }
                };

                return customers;
            }
        }       
    }        
}