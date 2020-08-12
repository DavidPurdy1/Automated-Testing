# Console Tests

A library built for Automated testing of Intact using Winium Driver.

This includes a WebApp test result UI to see the results of all tests ran
https://github.com/DavidPurdy1/Testing-DashBoard

-----
# Getting Started
Download from the zip from the release for the test runner

Clone this repo and https://github.com/DavidPurdy1/Testing-DashBoard 

Set any configurations

Run the Testing Dashboard and choose the test you want to run

-----
# Features to Add 
- Add documentation to the jira wiki on stuff like test cases, update running a test, etc....

- Add corrrelation to guid of the document added and test case this could be on a new table linked at the test case level

- Common Error: Once the document is validated the intact window loses focus and then it throws and interruption because it loses focus. Test passes but outcomes a incomplete

- Add a scheduler to have the tests run at a certain time or interval

- Have it be able to enter in a unique document name to track it under the test type

- Create more columns to track the guid of a document or login, create a description of the test that accesses the method summary and the line that it failed on, add a modifier description where they can add details to further communicate a pass or fail. 

- Add annotations (Unimplemented Exeception) 

- Port to winApp driver

# Features Added 

- Creation of Types and definitions and documents

- Logging in or connecting to a remote desktop (Set the path for the application and the user) 

- Adding Documents to InZone (set up the InZone definitions and documents in the right path for it to work)

- Adding Documents through batch review(Set up the definition and type for batch review)

- If failed can take screenshots then adds them to a word document

- Config file so it can be set up on any machine

- Default values for everything, but option to specify exactly what you want for most of the tests. Trying to balance ease and customization of the tests 

- Inzone recoginizes correct definitions when coming in. 

- Fail file to tell you which tests failed and passed

- Logout and log back into intact testing

- Search to determine if something is there

- Support for recognition

- Check for both interruptions and for errors thrown by the application
