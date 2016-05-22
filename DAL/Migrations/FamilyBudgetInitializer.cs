using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common;
using DAL.Model;

namespace DAL.Migrations
{
    public static class FamilyBudgetInitializer
    {
        private static void InitializeCurrency()
        {
            using (var uow = new UnitOfWork())
            {
                var date = new DateTime(2016, 05, 09);

                var currencies = new List<Currency>
                {
                    new Currency
                    {
                        Name = "American Dollar",
                        Code = "USD",
                        Converter = 19340,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Russian Rouble",
                        Code = "RUB",
                        Converter = 290,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Belarusian Ruble",
                        Code = "BYR",
                        Converter = 1,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Ukraine Hryvnia",
                        Code = "UAH",
                        Converter = 760,
                        UpadeDate = date
                    },
                    new Currency
                    {
                        Name = "Euro",
                        Code = "EUR",
                        Converter = 21970,
                        UpadeDate = date
                    }
                };

                var dbCurrencies = uow.CurrencyRepository.GetAll();

                foreach (var currency in currencies)
                {
                    var dbCurrency =
                        dbCurrencies.FirstOrDefault(
                            c => string.Equals(c.Code, currency.Code, StringComparison.CurrentCultureIgnoreCase));
                    if (dbCurrency == null)
                    {
                        uow.CurrencyRepository.Insert(currency);
                    }
                }
                uow.Commit();
            }
        }

        private static void InitializeCategories()
        {
            using (var uow = new UnitOfWork())
            {
                var categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = "Food",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Products"
                            },
                            new Category
                            {
                                Name = "Meals"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },

                    new Category
                    {
                        Name = "Bills and utilities",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Phone"
                            },
                            new Category
                            {
                                Name = "Water"
                            },
                            new Category
                            {
                                Name = "Electricity"
                            },
                            new Category
                            {
                                Name = "Gas"
                            },
                            new Category
                            {
                                Name = "TV"
                            },
                            new Category
                            {
                                Name = "Internet"
                            },
                            new Category
                            {
                                Name = "Rent"
                            },
                            new Category
                            {
                                Name = "Heating"
                            },
                            new Category
                            {
                                Name = "Credit"
                            },
                            new Category
                            {
                                Name = "Insurance"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Transport",
                        SubCategories = new List<Category>()
                        {
                            new Category
                            {
                                Name = "Taxi"
                            },
                            new Category
                            {
                                Name = "Parking"
                            },
                            new Category
                            {
                                Name = "Fuel"
                            },
                            new Category
                            {
                                Name = "Maintenance"
                            },
                            new Category
                            {
                                Name = "Public transport"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Clothing",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Outerwear"
                            },
                            new Category
                            {
                                Name = "Casual wear"
                            },
                            new Category
                            {
                                Name = "Elegant clothes"
                            },
                            new Category
                            {
                                Name = "Carnival costume"
                            },
                            new Category
                            {
                                Name = "Footwear"
                            },
                            new Category
                            {
                                Name = "Headdress"
                            },
                            new Category
                            {
                                Name = "Jewelry"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Electronics",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Computer"
                            },
                            new Category
                            {
                                Name = "Tablet"
                            },
                            new Category
                            {
                                Name = "Phone"
                            },
                            new Category
                            {
                                Name = "TV"
                            },
                            new Category
                            {
                                Name = "Printer"
                            },
                            new Category
                            {
                                Name = "Camera"
                            },
                            new Category
                            {
                                Name = "Games"
                            },
                            new Category
                            {
                                Name = "Accessories"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "House",
                        SubCategories = new List<Category>()
                        {
                            new Category
                            {
                                Name = "Furniture"
                            },
                            new Category
                            {
                                Name = "Repairs"
                            },
                            new Category
                            {
                                Name = "Sanitary engineering"
                            },
                            new Category
                            {
                                Name = "Appliances"
                            },
                            new Category
                            {
                                Name = "Detergents"
                            },
                            new Category
                            {
                                Name = "Klining"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Entertainment",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Cinema and theater"
                            },
                            new Category
                            {
                                Name = "Concert"
                            },
                            new Category
                            {
                                Name = "Cafes and bars"
                            },
                            new Category
                            {
                                Name = "Holiday"
                            },
                            new Category
                            {
                                Name = "Journey"
                            },
                            new Category
                            {
                                Name = "Games"
                            },
                            new Category
                            {
                                Name = "Excursions"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Beauty and health",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Pharmacy"
                            },
                            new Category
                            {
                                Name = "Doctor"
                            },
                            new Category
                            {
                                Name = "Sport"
                            },
                            new Category
                            {
                                Name = "Cosmetics and perfumery"
                            },
                            new Category
                            {
                                Name = "Beauty saloon"
                            },
                            new Category
                            {
                                Name = "Procedures"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Children",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Games and toys"
                            },
                            new Category
                            {
                                Name = "Clothing"
                            },
                            new Category
                            {
                                Name = "Education"
                            },
                            new Category
                            {
                                Name = "Pocket money"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Education",
                        SubCategories = new List<Category>
                        {
                            new Category
                            {
                                Name = "Literature"
                            },
                            new Category
                            {
                                Name = "Stationery"
                            },
                            new Category
                            {
                                Name = "School"
                            },
                            new Category
                            {
                                Name = "University"
                            },
                            new Category
                            {
                                Name = "College"
                            },
                            new Category
                            {
                                Name = "Sections"
                            },
                            new Category
                            {
                                Name = "Other"
                            }
                        }
                    }
                };

                var dbCategories = uow.CategoryRepository.GetCategories();
                foreach (var category in categories)
                {
                    if (!dbCategories.Any(c => c.Name.Equals(category.Name)))
                    {
                        uow.CategoryRepository.Insert(category);
                    }
                }
              
                uow.Commit();
            }
        }

        public static void Initialize()
        {
            InitializeCurrency();
            InitializeCategories();
        }
    }
}