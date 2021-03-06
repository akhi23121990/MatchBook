﻿using Matchbook.Model;
using Matchbook.Tests.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matchbook.Tests
{


    public interface IFakeDBcontext
    {

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubAccount> SubAccounts { get; set; }
        public DbSet<ClearingAccount> ClearingAccounts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLink> OrderLink { get; set; }

        int SaveChanges();
    }

    //class FakeDBContext
    //{
    //    Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> list) where T : class, new()
    //    {
    //        IQueryable<T> queryableList = list.AsQueryable();
    //        Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
    //        dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
    //        dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
    //        dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
    //        dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());
    //        dbSetMock.Setup(x => x.Create()).Returns(new T());

    //        return dbSetMock;
    //    }
    //}

}
