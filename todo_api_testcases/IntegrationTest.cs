using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using todo_api;
using todo_api.Models;
using Xunit;

namespace todo_api_testcases
{
    public class IntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task IntegrationTest1Async()
        {
            var note = new Note
            {
                ID = 1,
                Title = "Boeing",
                PlainText = "Aerospace company",
                Pinned = false,
                CheckLists = new List<CheckList>()
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
            };
            var content = JsonConvert.SerializeObject(note);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/notes", stringContent);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var JOArray = JObject.Parse(responseString);
            var JId = JOArray["id"].ToString();
            JId.Should().Be("1");


            //Get 
            var response1 = await _client.GetAsync("/api/notes");
            response1.EnsureSuccessStatusCode();
            var responseString1 = await response.Content.ReadAsStringAsync();

            var JOArray1 = JObject.Parse(responseString1);
            var JId1 = JOArray1["id"].ToString();
            JId1.Should().Be("1");


            //GetByID
            var response2 = await _client.GetAsync("/api/notes/1");
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response.Content.ReadAsStringAsync();

            var JOArray2 = JObject.Parse(responseString2);
            var JId2 = JOArray2["id"].ToString();
            JId2.Should().Be("1");


            //Put
            var notes = new Note
            {
                ID = 1,
                Title = "Stackroute",
                PlainText = "Training Centre",
                Pinned = true,
                CheckLists = new List<CheckList>()
                  {
                      new CheckList()
                      {
                          CheckListData="C and C++",
                          Status=false
                      }
                  },
                Labels = new List<Label>()
                  {
                      new Label()
                      {
                          LabelData ="JS and C#"
                      }
                  }
            };
            var content3 = JsonConvert.SerializeObject(notes);
            var stringContent3 = new StringContent(content, Encoding.UTF8, "application/json");

            var response3 = await _client.PutAsync("/api/notes/1", stringContent);
            response.EnsureSuccessStatusCode();


            //Delete
            var response4 = await _client.DeleteAsync("/api/notes/1");
            response.EnsureSuccessStatusCode();

        }
    }
}
