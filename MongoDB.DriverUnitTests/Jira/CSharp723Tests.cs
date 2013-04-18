/* Copyright 2010-2013 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;

namespace MongoDB.DriverUnitTests.Jira
{
    [TestFixture]
    public class CSharp723Tests
    {
        private MongoCollection<IJob> _interfaceCollection;
        private MongoCollection<Job> _classCollection;
        private Guid _id1;
        private Guid _id2;

        public enum JobStatus
        {
            Created = 0,
            Assigning = 1,
            Assigned = 2,
            Started = 3,
            Finished = 4
        }

        public interface IJob
        {
            Guid Id { get; set; }
            JobStatus Status { get; set; }
            bool IsFaulted { get; set; }
            string ErrorMessage { get; set; }
        }

        public class Job : IJob
        {
            public Guid Id { get; set; }
            public JobStatus Status { get; set; }
            public bool IsFaulted { get; set; }
            public string ErrorMessage { get; set; }
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var db = Configuration.TestDatabase;
            _interfaceCollection = db.GetCollection<IJob>(Configuration.TestCollection.Name + "_if");
            _classCollection = db.GetCollection<Job>(Configuration.TestCollection.Name + "_cls");
            _interfaceCollection.RemoveAll();
            _classCollection.RemoveAll();
            CreateData();
        }

        [Test]
        public void TestRetrieveInterfaceWithNoFilter()
        {
            var result = _interfaceCollection.AsQueryable<IJob>().FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual(_id1, result.Id);
            Assert.AreEqual(false, result.IsFaulted);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestRetrieveInterfaceWithEnumFilter()
        {
            var result = _interfaceCollection.AsQueryable<IJob>().FirstOrDefault(x => x.Status == JobStatus.Created);
            Assert.IsNotNull(result);
            Assert.AreEqual(_id1, result.Id);
            Assert.AreEqual(false, result.IsFaulted);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestRetrieveInterfaceWithPrimitiveFilter()
        {
            var result = _interfaceCollection.AsQueryable<IJob>().FirstOrDefault(x => x.IsFaulted == false);
            Assert.IsNotNull(result);
            Assert.AreEqual(_id1, result.Id);
            Assert.AreEqual(false, result.IsFaulted);
        }

        [Test]
        public void TestRetrieveClass()
        {
            var result = _classCollection.AsQueryable<Job>().FirstOrDefault(x => x.Status == JobStatus.Created);
            Assert.IsNotNull(result);
            Assert.AreEqual(_id1, result.Id);
            Assert.AreEqual(false, result.IsFaulted);
        }

        private void CreateData()
        {
            _id1 = Guid.NewGuid();
            _id2 = Guid.NewGuid();
            IJob job1 = new Job()
            {
                Id = _id1,
                Status = JobStatus.Created,
                IsFaulted = false
            };
            IJob job2 = new Job()
            {
                Id = _id2,
                Status = JobStatus.Finished,
                IsFaulted = true
            };
            _interfaceCollection.Insert<IJob>(job1);
            _interfaceCollection.Insert<IJob>(job2);
            _classCollection.Insert<Job>((Job)job1);
            _classCollection.Insert<Job>((Job)job2);
        }

    }
}
