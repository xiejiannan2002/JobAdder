using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using CandidateFinder.Business;

namespace CandidateFinderUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IConfiguration _mockConfig;
        private readonly DataProcessing _dtProcessor;

        public UnitTest1()
        {
            //_mockConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var myConfiguration = new Dictionary<string, string>
            {
                {"GetCandidateUri", "http://private-76432-jobadder1.apiary-mock.com/candidates"},
                {"GetJobUri", "http://private-76432-jobadder1.apiary-mock.com/jobs"},
                {"Timeout", "60"}
            };
            _mockConfig = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();          
            _dtProcessor = new DataProcessing(_mockConfig);
        }

        [TestMethod]
        public async Task TestSearchSearchOpenPosition()
        {
            var result = await _dtProcessor.SearchOpenPosition();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Job>));
            Assert.IsTrue(result.ToList().Count > 0);
        }

        [TestMethod]
        public async Task TestSearchOpenPositionFromAPI()
        {
            var result = await _dtProcessor.SearchOpenPositionFromAPI();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Job>));
            Assert.IsTrue(result.ToList().Count > 0);
        }

        [TestMethod]
        public async Task GetCandidates()
        {
            var result = await _dtProcessor.GetCandidates();
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task TestGetJobs()
        {
            var result = await _dtProcessor.GetJobs();
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void TestMatchJobWichCandidate()
        {
            IList<Job> jbs = new List<Job>();
            Job jb = new Job();
            jb.name = "test name";
            jb.company = "test company";
            jb.skills = "mobile, office, excel";
            jbs.Add(jb);

            IList<Candidate> candidates = new List<Candidate>();
            Candidate c1 = new Candidate();
            c1.name = "c1";
            c1.skillTags = "office, excel, communication";
            candidates.Add(c1);
            Candidate c2 = new Candidate();
            c2.name = "c2";
            c2.skillTags = "excel, mobile, word, sales";
            candidates.Add(c2);
            Candidate c3 = new Candidate();
            c3.name = "c3";
            c3.skillTags = "communication, office, admin, word, sales";
            candidates.Add(c3);

            Assert.AreEqual(_dtProcessor.MatchJobWichCandidate(jbs, candidates), true);
        }

        [TestMethod]
        public void TestFindBestCandidate()
        {
            Job jb = new Job();
            jb.name = "test name";
            jb.company = "test company";
            jb.skills = "mobile, office, excel";

            IList<Candidate> candidates = new List<Candidate>();
            Candidate c1 = new Candidate();
            c1.name = "c1";
            c1.skillTags = "office, excel, communication";
            candidates.Add(c1);
            Candidate c2 = new Candidate();
            c2.name = "c2";
            c2.skillTags = "excel, mobile, word, sales";
            candidates.Add(c2);
            Candidate c3 = new Candidate();
            c3.name = "c3";
            c3.skillTags = "communication, office, admin, word, sales";
            candidates.Add(c3);

            var result = _dtProcessor.FindBestCandidate(jb, candidates);
            Assert.AreEqual(result, c2);
        }

        [TestMethod]
        public void TestGetResultFromCache()
        {
            var cache = MemoryCache.Default;
            Assert.AreEqual(_dtProcessor.GetResultFromCache(), cache["SearchingResult"]);
        }

        [TestMethod]
        public void TestSaveResultToCache()
        {
            IList<Job> jbs = new List<Job>();
            jbs.Add(new Job());

            var cache = MemoryCache.Default;
            _dtProcessor.SaveResultToCache(jbs);
            var result = cache["SearchingResult"] as IEnumerable<Job>;
            Assert.AreEqual(result, jbs);
        }
    }
}
