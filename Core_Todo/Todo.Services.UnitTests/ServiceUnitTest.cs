using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Moq;
using System.Linq;
using Todo.Data;
using Todo.Repositories;
using Todo.Services;
using Todo.Mappers.Profiles;
using Todo.Models;
using Todo.Common.Enumerations;

namespace BoilerPlate.Services.UnitTest
{
    [TestClass]
    public class ServiceUnitTest
    {
        [TestMethod]
        public async Task GetByIdAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository,mapper.Object);
            var obj = await manager.GetAsync(11);
            Assert.IsTrue(obj != null);
        }
        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository, mapper.Object);
            var obj = await manager.GetAsync(1);
            Assert.IsTrue(obj != null);
        }
        [TestMethod]
        public async Task AddAsyncTest()
        {
            try
            {
                Mapper.Initialize(m =>
                {
                    var profiles = typeof(UserTaskProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
                    foreach (var profile in profiles)
                    {
                        m.AddProfile(Activator.CreateInstance(profile) as Profile);
                    }
                });
                var context = new DataContext();
                var repository = new UserTaskRepository(context);
                var manager = new UserTaskManager(repository, Mapper.Configuration.CreateMapper());
                var obj = await manager.AddAsync(new UserTaskModel()
                {
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow.AddDays(-1),
                    Title = "Test",
                    Description = "Test Description"
                });
                Assert.IsTrue(obj != null);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [TestMethod]
        public async Task DeleteByIdAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository, mapper.Object);
            await manager.DeleteAsync(1);
            var obj = await manager.GetAsync(1);
            Assert.IsTrue(obj == null);
        }
        [TestMethod]
        public async Task DeleteByObjectAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository, mapper.Object);
            var obj = await manager.GetAsync(1);
            await manager.DeleteAsync(obj);
            var result = await manager.GetAsync(1);
            Assert.IsTrue(result == null);
        }
        [TestMethod]
        public async Task SearchAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository, mapper.Object);
            string id = "546542D6-59C3-4D8E-9C14-A550E0473885";
            var filters = new List<FilterInfo>() { new FilterInfo() { Field = "Id", Op = FilterOperators.Equals, Value = new Guid(id) } };
            var searchRequest = new SearchRequest() { Filters = filters, Page = 1, PageSize = 10, Sort = "Id" };
            var obj = await manager.SearchAsync(searchRequest);
            Assert.IsTrue(obj != null);
        }
        [TestMethod]
        public async Task UpadteAsyncTest()
        {
            var context = new DataContext();
            var mapper = new Mock<IMapper>();
            var repository = new UserTaskRepository(context);
            var manager = new UserTaskManager(repository, mapper.Object);
            var obj = await manager.GetAsync(1);
            obj.DateModified = DateTime.UtcNow;
            var data = await manager.UpdateAsync(obj);
            Assert.IsTrue(data != null);
        }
    }
}
