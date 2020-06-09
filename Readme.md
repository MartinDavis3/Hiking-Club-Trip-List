# Hiking Club Trip List

## Project Contributors: Martin J. Davis (MartinDavis3)

## The Project:

Create webpages for handling trip creation, maintenance, display and signup for a fictitious hiking club.

<img src="/HikingClubTripList/wwwroot/img/logo.png" width=294 height=193 title="Wandering Heights Hiking Club Logo" alt="Logo depicting sylised mountains">

# Wandering Heights Hiking Club - Trip Pages

Note: It is not the intention to create a full club website, just the pages required to handle trip creation and signup; specifically:

1. Login – For registered users (club members) entered into the system by an administrator.
2. Home – Brief description of the club with instructions and rules for trip creation and signup.
3. Trips – List of trips, access to trip details and buttons for signup, withdrawal, creation, editing and deletion.
3. Privacy Policy.

## Requirements
1.	List of trips, sorted by date, with date, title, level, leader and if space is still available.
2.	Ability to add new trips.
3.	Detail view showing all trip details, currently signed up participants and with button(s) for signup or withdrawal, as appropriate.
4.	For trip leader, detailed display will allow editing and deletion.
## Business Logic
1.	Anyone can create a trip – they will then automatically be assigned as leader.
2.	Only the leader of a trip can modify it or cancel (delete) it.
3.	Anyone can see all the details of all trips, including the actual names of all participants.
4.	Anyone can sign up for any trip, as long as space is still available.
5.	Participants can withdraw from any trip they are already signed up for.



# List of Citations
1.	ASP.NET Core MVC with EF Core - tutorial series (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-3.1)
2.	Contrast checker: https://webaim.org/resources/contrastchecker/
3.	The code for the tooltip was modified from that given at W3Schools https://www.w3schools.com/css/css_tooltip.asp

# The Application

Since the scope of the project was the handling of the trips list, not management of members, a list of club members seeded into the database (as if entered by an administrator) is used and no registration page is included. A pseudo- login page is provided to allow demonstration of the different actions that different members can perform. This is not a true, secure login and the associated data is not securely stored as it would be for a site configured with a true login.

The application was built using the .NET Core MVC framework and MySQL database. The official Microsoft documentation for .NET Core was referenced extensively during the project. In particular, the ASP.NET Core MVC with EF Core - tutorial series (Ref.1), was used to help setup the initial code and database structure. This was then extensively modified to fulfil the particular business model required for the project application.

The data base structure consists of a fixed, seeded table of members, the table of trips which is accessed by the application to carry out its functions, and a list of signups, which join these two tables.
 
The application structure consists of models for trips, members and signups, and login and a view model for the trip details. There are trip, member and signup controllers, each with a service layer handling database functions. There is some error handling included, notably to trap some errors which could be caused by poorly formed routes, and a simple view which is use to communicate the error to the user.

The layout of the pages is styled using CSS and is fully responsive. The style of the pages is deliberately chosen to be fairly minimalist. The information being provided is fairly simple and the members using the website will know what the information means, so lots of detailed explanation is unnecessary. The aim is to present and allow manipulation of the trip and participation list in a simple, uncluttered manner. The colour scheme was chosen to reflect this calm, simple approach and also the natural theme of a hiking club. Most of the colours were adapted from colours found in the picture on the home page, or other natural sources, adjusting mostly the lightness, and sometimes slightly the saturation, as required to give necessary contrast between elements (WCAG AA in all cases WCAG AAA in most cases, Ref. 2).

# Installation Instructions

Using PowerShell or a similar git enabled terminal:
1.	Navigate to where you want to place the project (a new folder, HikingClubTripList will be created).
2.	Copy the clone link from the Hiking-Club-Trip-List GitHub repository ( https://github.com/MartinDavis3/Hiking-Club-Trip-List ).
3.	Type the command: git clone <link>

Open VS Community 2019:
1.	Select: Open a project or solution.
2.	Navigate to where the HikingClubTripList folder was created and open it.
3.	Click on the file: HikingClubTripList.sln.
4.	Wait while VS Community loads and prepares the solution.
5.	(If you want to change the name of the database which is created, edit the DefaultConnection string in the file appsettings.json. The default database name is HikingClubTripList.)
6.	(If you do not wish to have the example dataset loaded, delete the file Data/DbInitialiser.)
7.	The Application is ready to be run.

# User Instructions

Navigate the pages as usual using the links at the top.

## Home Page

This gives a brief introduction to the club and also gives brief instructions on how to create, edit and delete trips as well as how to sign up for and withdraw from trips. This is visible without Log In.

## Privacy Page

This gives a very brief statement of club privacy policy. This is also visible without Log In.

## Login

To be able to see the trip list, you must log in on the Login page. A series of 16 users is included in the demonstration data set. These have username and password combinations of simply: user1, pass1; user2, pass2 etc. Once you are logged in you will see the member name in place of Log In. Click on the username to Log Out.

## Trips

The Hiking Trip List shows a summary of all the trips. For each trip the date, title, level, leader name and the spaces remaining on that trip are shown. If there are no spaces remaining the trip is marked full.
1.	To add a new trip, use the link: Add a New Trip at the top of that page. 
2.	To see the details of each trip, use the link: Details to the right of the specific trip listing.

The Trip Details page shows the information which will help you decide if you would like to participate in the trip. This is: date, title, level, distance, elevation gain, a description of the hike, the maximum number of participants, the leader name and the names of the currently signed up participants.

The buttons below the trip description allow you to Delete, Edit, Withdraw or Sign up for a trip. There is also a link at the right: Back to List. The buttons you actually see will depend on the permissions / possibilities you have for this particular trip:
1.	Only the leader of a particular trip can delete or edit that trip.
2.	Obviously, you can only withdraw from a trip if you are already signed up!
3.	Equally, you can only sign up if you are not already participating. Additionally, space must be available on the trip.

## Creating or Editing a Trip
The following data is entered when creating or editing a trip:
1.	The date of the trip (required).
2.	Title (required): Between 5 and 30 characters, including spaces, starting with a capital letter and with no leading or trailing spaces and no full stop at the end.
3.	Level (required, dropdown selection): Beginner, Intermediate, Advanced.
4.	Distance (km): A value between 0 and 100.
5.	Elevation Gain (m): A value between -5000 and 5000.
6.	Description: Maximum 800 characters, including spaces, with no leading or trailing spaces.
7.	Maximum Participants (required): A value between 2 and 12.

When creating a trip, the currently logged in user will be automatically assigned as the leader.

When editing a trip you must save the changes for them to be accepted.

Note: It is possible to change the maximum number of participants to less that the number of actual participant. This is a feature of the application and will not cause problems. Currently signed up participants will remain signed up. However, if any participants withdraw, nobody will be able to sign up until there is less than the new number of maximum participants.

The delete button will let you remove a trip. You will be asked for confirmation, with the option to cancel, before the trip is actually deleted.

## Log Out
Hovering on the name of the currently logged in member in the navigation bar will show a Log Out tooltip (Ref 3). Clicking on the name will then perform the log out.

# Testing Instructions and Test Cases

## Test Login
1.	Check an invalid user cannot log in even with the correct password of another user.
2.	Check a valid user with the wrong password cannot log in.
3.	Check user can be properly logged out.

## Test user input fields

Use the following test data, which is in the form:

Field: “valid data”; “Invalid data 1”, “Invalid data 2”, etc.

Date: “30-Apr-20”; “31-Apr-21”
Title: “Turbine”; “Here”,” Turbine”, “Turbine “, “Turbine.”, “turbine”, “Cougar Canyons Spring Kick-Off”
Distance: “12”; “long”,“-1”,“101”
Elevation Gain: “500”; “high”, “-5001”, “5001”
Description: “Very nice.”;“ Very nice.”, “Very nice. “, “very nice.”, “A nice hike at elevation. Not too long but steep. It is worth the effort though, with great views of the Saskatchewan glacier. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer pharetra metus nec felis varius suscipit. Pellentesque in facilisis dui. Quisque magna arcu, imperdiet ut nunc at, fermentum interdum ante. Nullam sed posuere dolor. Sed quis nibh quis lacus porta ultricies imperdiet nec felis. Suspendisse potenti. Nulla nunc leo, mattis at volutpat non, ornare sed nibh. Vestibulum commodo commodo velit, vitae rutrum nibh fringilla id. Vestibulum in urna libero. Aenean aliquam euismod lacus ut fermentum. Integer convallis tincidunt nisi in cursus. Proin eu vestibulum mi, in pellentesque orci. Nullam blandit dapibus turpis quis finibus. Praesent eget arcu tempor, fauci.”
Maximum Participants: “5”; “5.5”, “1”, “13”

For each entry form.
1.	Enter valid values in all fields and check there are no error messages and data can be submitted.
2.	For each value field in turn:
a.	Enter one of each type of incorrect value in turn.
b.	Check that a meaningful error message is shown in each case.
c.	Check that the form cannot be submitted.
d.	Return the field to a valid value
e.	Continue to next field
3.	In developer tools, disable running of scripts.
4.	For each value field in turn:
a.	Enter one of each type of incorrect value in turn.
b.	Submit the form.
c.	Check that a meaningful error is returned and no records were changed in the database.
d.	Continue to the next field.

## Test Trip Controller
1.	Check Index() returns all data items in correct order.
2.	Check Create correctly adds data to the database and then shows full list with added item.
3.	Check Edit:
a.	Correctly modifies the requested item and displays the modified item.
b.	Edit the URL directly so there is no id and check there is an appropriate error message and no database modification.
c.	Edit the URL directly to an invalid id and check there is an appropriate error message and no database modification.
4.	Check Delete:
a.	Correctly deletes the requested item and returns the full list without the deleted item.
b.	Edit the URL directly so there is no id and check there is an appropriate error message and no database modification.
c.	Edit the URL directly to an invalid id and check there is an appropriate error message and no database modification. 

# Link to Trello Board
https://trello.com/b/LEcsNI0B/capstone-project


