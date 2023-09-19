using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumTest.Tests.Unit.Forum
{
    public class ForumConfigurationTests : TestWithSqlite
    {


        /*[Fact]
        public void NameIsRequired()
        {
            var newItem = new ToDoItem();
            DbContext.ToDoItem.Add(newItem);

            Assert.Throws<DbUpdateException>(() => DbContext.SaveChanges());
        }

        [Fact]
        public void AddedItemShouldGetGeneratedId()
        {
            var newItem = new ToDoItem() { Name = "Testitem" };
            DbContext.ToDoItem.Add(newItem);
            DbContext.SaveChanges();

            Assert.NotEqual(Guid.Empty, newItem.Id);
        }

        [Fact]
        public void AddedItemShouldGetPersisted()
        {
            var newItem = new ToDoItem() { Name = "Testitem" };
            DbContext.ToDoItem.Add(newItem);
            DbContext.SaveChanges();

            Assert.Equal(newItem, DbContext.ToDoItem.Find(newItem.Id));
            Assert.Equal(1, DbContext.ToDoItem.Count());
        }*/
    }
}
