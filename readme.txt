Instruction:
It is a .Net Core & Angular Project

the web page is at: CandidateFinder\CandidateFinder\ClientApp\src\app\home
the main logic is at: CandidateFinder\CandidateFinder\ClientApp\src\app\service
the css is at: CandidateFinder\CandidateFinder\ClientApp\src\assets\css

Once you download the code, go to the "CandidateFinder\CandidateFinder\ClientApp" folder
run "npm i" to generate "node_modules" folder
then you can run "ng serve --open" to view the app via browser

alternatively, you can open "CandidateFinder.sln" in Visual Studio 2019
build and run the project to view the app via browser

-------------------------------------------------------------------------------------------------------

Clarification:
there is one thing which requirement doesn't specified clearly
if 2 or more candidates have the same amount of matched skills, then who is the most qualified candidate?
My solution is: if both candidates have same amount of skills required by the job, then show the candidate who has more tag sikills

-------------------------------------------------------------------------------------------------------

Improvement:
Due to the time limitation (4 hours), there are some areas need to improve in future
1. write independent test project and write test case first
2. make parameters configurable, such as web api URL, how many items to show in each page, the best idea is put those into configuration file, rather than hard code.
3. move bussiness logic into C# business layer. Ideally, the data, business, and UI should be presented into different layer


-------------------------------------------------------------------------------------------------------
Commit Histroy
v1: submit the projects, basic function has been implemented.
v2: update css 
    update readme
	update the algorithm to match job and candidate