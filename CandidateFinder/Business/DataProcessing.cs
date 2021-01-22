using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;

namespace CandidateFinder.Business
{
    public class Candidate
    {
        public string name { get; set; }
        public string skillTags { get; set; }
    }
    public class Job
    {
        public string name { get; set; }
        public string company { get; set; }
        public string skills { get; set; }
        public Candidate candidate { get; set; }
    }
    public class DataProcessing
    {
        private readonly IConfiguration _config;
        private readonly string _getCandidateUri;
        private readonly string _getJobUri;
        private readonly int _timeout;

        public DataProcessing(IConfiguration config)
        {
            _config = config;
            _getCandidateUri = config.GetValue<string>("GetCandidateUri");
            _getJobUri = config.GetValue<string>("GetJobUri");
            _timeout = config.GetValue<int>("Timeout");
        }

        public async Task<IEnumerable<Job>> SearchOpenPosition()
        {
            IEnumerable<Job> result = GetResultFromCache() as IEnumerable<Job>;
            if (result != null)
                return result;
            else
            {
                result = await SearchOpenPositionFromAPI();
                return result;
            }
        }

        public async Task<IEnumerable<Job>> SearchOpenPositionFromAPI()
        {
            var getCandidateTask = GetCandidates();
            var getJobTask = GetJobs();
            await Task.WhenAll(getCandidateTask, getJobTask);
            IList<Candidate> candidates = await getCandidateTask;
            IList<Job> jobs = await getJobTask;
            MatchJobWichCandidate(jobs, candidates);
            SaveResultToCache(jobs);
            return jobs;
        }

        public async Task<IList<Candidate>> GetCandidates()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(this._getCandidateUri);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<IList<Candidate>>();                  
                }
                catch
                {
                    //Write Log                   
                }
            }
            return new List<Candidate>();
        }

        public async Task<IList<Job>> GetJobs()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(this._getJobUri);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<IList<Job>>();
                }
                catch
                {
                    //Write Log
                }
            }
            return new List<Job>();
        }

        public bool MatchJobWichCandidate(IList<Job> jobs, IList<Candidate> candidates)
        {
            try
            {
                foreach (Job job in jobs)
                {
                    job.candidate = FindBestCandidate(job, candidates);
                }
                return true;
            }
            catch
            {
                //Write Log
            }
            return false;
        }

        public Candidate FindBestCandidate(Job job, IList<Candidate> candidates)
        {
            string[] requiredSkills = job.skills.Replace(" ", "").Split(",");
            int countMatchedSkills = 0;
            Candidate best = null;

            foreach(Candidate candidate in candidates)
            {
                string[] ownedSkills = candidate.skillTags.Replace(" ", "").Split(",");
                int matched = requiredSkills.Intersect(ownedSkills, StringComparer.OrdinalIgnoreCase).Count();
                if ((matched > countMatchedSkills) 
                    || (matched == countMatchedSkills && best != null && best.skillTags.Trim().Split(',').Length < ownedSkills.Length))
                {
                    countMatchedSkills = matched;
                    best = candidate;
                }
            }

            return best;
        }

        public object GetResultFromCache()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("SearchingResult"))
            {
                return cache["SearchingResult"];
            }

            return null;
        }

        public void SaveResultToCache(IList<Job> jobs)
        {
            var cache = MemoryCache.Default;
            cache.Remove("SearchingResult");
            if (!cache.Contains("SearchingResult"))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(this._timeout);
                cache.Add("SearchingResult", jobs, expiration);
            }
        }
    }
}
