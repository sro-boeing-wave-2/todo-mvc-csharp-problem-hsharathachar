using System;
using todo_api.Controllers;
using Xunit;

namespace todo_api_testcases
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var controller = new NotesController();
            var result = controller.Get();
            Assert.Equal(, result);
        }
    }
}
