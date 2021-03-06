using System;
using todo_api.Controllers;
using Xunit;
using Microsoft.EntityFrameworkCore;
using todo_api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FluentAssertions;

namespace todo_api_testcases
{
    public class UnitTest1
    {
        private NotesController _controller;

        public UnitTest1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoApiContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            TodoApiContext context = new TodoApiContext(optionsBuilder.Options);
            _controller = new NotesController(context);
            NoteData(optionsBuilder.Options);
        }

        public void NoteData(DbContextOptions<TodoApiContext> options)
        {
            using (var todocontext = new TodoApiContext(options))
            {
                var note = new List<Note>()
                {
                  new Note()
                    {
                      ID=1,
                      Title="Boeing",
                      PlainText="Aerospace company",
                      Pinned=false,
                      CheckLists=new List<CheckList>()
                      {
                          new CheckList()
                          {
                              CheckListData="Jumbo Jet",
                              Status=true
                          }
                      },
                      Labels = new List<Label>()
                      {
                          new Label()
                          {
                              LabelData ="Dreamliner"
                          }
                      }
                  },

                   new Note()
                  {
                      ID=3,
                      Title="Boeings",
                      PlainText="Aerospace companys",
                      Pinned=false,
                      CheckLists=new List<CheckList>()
                      {
                          new CheckList()
                          {
                              CheckListData="Jumbo Jets",
                              Status=true
                          }
                      },
                      Labels = new List<Label>()
                      {
                          new Label()
                          {
                              LabelData ="Dreamliners"
                          }
                      }
                  }
               };
                todocontext.Note.AddRange(note);
                todocontext.SaveChanges();
            }
        }

        [Fact]
        public async void Test1()
        {
            var result = await _controller.GetNote();
            var SingleNote = result as List<Note>;
            Assert.Equal(2, SingleNote.Count);
        }

        [Fact]
        public async void Test2()
        {
            var result = await _controller.GetNote(1);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var notess = okResult.Value.Should().BeAssignableTo<Note>().Subject;
            notess.ID.Should().Be(1);
        }

        [Fact]
        public async void Test3()
        {
            var result = await _controller.GetNote(false);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var notess = okResult.Value.Should().BeAssignableTo<Note>().Subject;
            notess.Pinned.Should().Be(false);
        }

        [Fact]
        public async void Test5()
        {
            var result = await _controller.GetByTitle("Boeing");
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var notess = okResult.Value.Should().BeAssignableTo<Note>().Subject;
            notess.Title.Should().Be("Boeing");
        }

        [Fact]
        public async void Test6()
        {
            Note note = new Note
            {
                ID = 2,
                Title = "Stackroute",
                PlainText = "Training Centre",
                Pinned = true,
                CheckLists = new List<CheckList>() { new CheckList { CheckListData = "C and C++", Status = false } },
                Labels = new List<Label>() { new Label { LabelData = "C# and JS" } }
            };
            var result = await _controller.PostNote(note);
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var notess = okResult.Value.Should().BeAssignableTo<Note>().Subject;
            notess.ID.Should().Be(2);
        }

        [Fact]
        public async void Test7()
        {
            Note note = new Note
            {
                ID = 3,
                Title = "Airbus",
                PlainText = "Airplane Producer",
                Pinned = true,
                CheckLists = new List<CheckList>() { new CheckList { CheckListData = "A450", Status = false } },
                Labels = new List<Label>() { new Label { LabelData = "A350" } }
            };

            var result = await _controller.PutNote(3, note);
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;

            //var Note = _controller.GetNote(1);
            //note.Title.Should().Be("Airbus");
            //note.Pinned.Should().Be(true);
            //note.PlainText.Should().Be("Airplane Producer");
        }



        [Fact]
        public async void Test8()
        {
            var result = await _controller.DeleteNote(1);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        }

        [Fact]
        public void Test9()
        {
            var result = _controller.DeleteNote("Stackroute");
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        }
    }
}
