using Project10pm.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10pm.API.Test.DataStore
{
    [TestFixture(Description = "Tests for Text Content Repository")]
    internal class TextDataStorageTests
    {
        private TextContentRepo _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = new TextContentRepo();
        }

        [Test]
        public void NewRecord_ValidModel_ReturnsId()
        {
            var content = new TextContent
            {
                RawText = $"2023-06-07"
            };
            var id = _repo.Add(content);
            Assert.That(id, Is.GreaterThan(0));
        }

        [Test]
        public void NewRecord_NullModel_ThrowsException() 
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                _repo.Add(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            });
        }

        [Test]
        public void NewRecord_InvalidModel_ThrowsExcpetion()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _repo.Add(new TextContent());
            });
        }

        [Test]
        public void GetRecord_ValidId_ReturnsRecord()
        {
            var content = new TextContent
            {
                RawText = $"2023-06-07"
            };
            var id = _repo.Add(content);
            var record = _repo.Find(id);
            Assert.That(record.Key, Is.EqualTo(id));
        }

        [Test]
        public void GetRecord_InvalidId_ThrowsException()
        {
            Assert.Throws<Exception>(() =>
            {
                _repo.Find(1);
            });
        }

        [Test]
        public void DeleteRecord_ValidId_ReturnsRecord() 
        {
            var content = new TextContent
            {
                RawText = $"2023-06-07"
            };
            var id = _repo.Add(content);
            var record = _repo.Remove(id);
            Assert.That(record.Key, Is.EqualTo(id));
        }

        [Test]
        public void DeleteRecord_InvalidId_ThrowsException()
        {
            Assert.Throws<Exception>(() =>
            {
                _repo.Remove(1);
            });
        }

        [Test]
        public void RetrieveRecord_DeletedId_ThrowsException()
        {
            var content = new TextContent
            {
                RawText = $"2023-06-07"
            };
            var id = _repo.Add(content);
            var record = _repo.Remove(id);
            Assert.Throws<Exception>(() =>
            {
                _repo.Find(id);
            });
        }

        [Test(Description = "This test is mostly to ensure any mocked repos are handling ids correctly.")]
        public void TwoNewRecords_DeleteFirst_GetSecondReturnsRecord()
        {
            var content1 = new TextContent
            {
                RawText = $"2023-06-06"
            };
            var content2 = new TextContent
            {
                RawText = $"2023-06-07"
            };
            var id1 = _repo.Add(content1);
            var id2 = _repo.Add(content2);
            _repo.Remove(id1);
            var record2 = _repo.Find(id2);
            Assert.That(record2.Key, Is.EqualTo(id2));
        }
    }
}
