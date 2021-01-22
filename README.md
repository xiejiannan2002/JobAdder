# JobAdder
1. Introduction 
   It is a .Net Core & Angular Project

   1. front end angular component: \JobAdder\CandidateFinder\ClientApp\src\app\home
   2. web api (receive request from angular page): \JobAdder\CandidateFinder\Controllers\HomeController.cs
   3. main logic & business: \JobAdder\CandidateFinder\Business\DataProcessing.cs
   4. configuration file: \JobAdder\CandidateFinder\appsettings.json
   5. independent testing framework: \JobAdder\CandidateFinderUnitTest


2. How to run the program
   1. download the sourcecode
   2. unzip it
   3. open CandidateFinder.sln in visual studio 2019
   4. build the project (may take few minutes, as it needs to restore npm package)
   5. run the project, you will see the result in browser
   6. (Note: such as API Uri has been configed at appsettings.json)
   
   
3. Something more
   1. peformance: 
      a. using cache to avoid requesting data from web api every time 
	  b. async function to avoid waiting more times on front time
   2. reliability: TDD, writing independent test cases
   3. scalability: everything is configurable at appsettings.json
   4. Clarification: there is one thing which requirement doesn't specified clearly
      if two or more candidates have the same amount of matched skills, then who is the most qualified candidate?
      My solution is: if both candidates have same amount of skills required by the job, then choose the candidate who has more tag sikills
   
   
4. Improvement need to be done to make it as a mature product
   1. write log
   2. write more test case, and inject fake data(such as Configuration) into test method
   3. write script to deployed to docker

