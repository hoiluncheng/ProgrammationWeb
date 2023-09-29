using Microsoft.VisualStudio.TestTools.UnitTesting;
using Examen1Formatif.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examen1Formatif.Data;
using Microsoft.EntityFrameworkCore;
using Examen1Formatif.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Examen1Formatif.Services.Tests
{
    [TestClass()]
    public class BillServiceTests
    {

        DbContextOptions<ApplicationDbContext> options;
        public BillServiceTests()
        {
            // TODO On initialise les options de la BD, on utilise une InMemoryDatabase
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                // TODO il faut installer la dépendance [Microsoft.EntityFrameworkCore.InMemory]
                .UseInMemoryDatabase(databaseName: "BillService")
                .Options;
        }
        [TestInitialize]
        public void Init()
        {
            // TODO avoir la durée de vie d'un context la plus petite possible
            using ApplicationDbContext db = new ApplicationDbContext(options);
            // TODO on ajoute des données de tests
            Item[] Items = new Item[] {
          new Item
          {
              Id = 1,
              Name = "Chat Dragon",
              Price = 1.2M
          }, new Item
          {
              Id = 2,
              Name = "Chat Awesome",
              Price = 5.1M
          }, new Item
          {
              Id = 3,
              Name = "Chatton Laser",
              Price = 2.5M
          },
          
        };
            db.AddRange(Items);
            db.SaveChanges();
        }
        [TestCleanup]
        public void Dispose()
        {
            //TODO on efface les données de tests pour remettre la BD dans son état initial
            using ApplicationDbContext db = new ApplicationDbContext(options);
            db.Items.RemoveRange(db.Items);
            db.SaveChanges();
        }
        [TestMethod()]
        public void CreateBillTest()
        {
            //TODO Test classique d'une méthode de service
            using ApplicationDbContext db = new ApplicationDbContext(options);
            BillService service = new BillService(db);
            List<Item> ItemsNull = new List<Item>();
            Assert.IsNull(service.CreateBill("FakeBill", ItemsNull));
        }

        [TestMethod()]
        [ExpectedException(typeof(AreYouInsaneException))]
        public void TestExceptionEqualtoZero() {
            using ApplicationDbContext db = new ApplicationDbContext(options);
            BillService service = new BillService(db);
            Item[] Items = new Item[] {
            new Item {
                    Id = 4,
                    Name = "Fake4",
                    Price = 0.0M
                } 
            };
            service.CreateBill("FakeBill", Items);
        }

        [TestMethod()]
        [ExpectedException(typeof(AreYouInsaneException))]
        public void TestExceptionSmallThanZero()
        {
            using ApplicationDbContext db = new ApplicationDbContext(options);
            BillService service = new BillService(db);
            Item[] Items = new Item[] {
            new Item {
                    Id = 4,
                    Name = "Fake4",
                    Price = -1.0M
                }
            };
            service.CreateBill("FakeBill", Items);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void TestExceptionNameEmpty()
        {
            using ApplicationDbContext db = new ApplicationDbContext(options);
            BillService service = new BillService(db);
            Item[] Items = new Item[] {
            new Item {
                    Id = 4,
                    Name = "Fake4",
                    Price = 1.0M
                }
            };
            service.CreateBill("", Items);
        }
    }
}