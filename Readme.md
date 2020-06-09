# Hiking Club Trip List

## Project Contributors: Martin J. Davis (MartinDavis3)

## The Project:

Create webpages for handling trip creation, maintenance, display and signup for a fictitious hiking club.

<img src="/HikingClubTripList/wwwroot/img/logo.png" width=294 height=193 title="Wandering Heights Hiking Club Logo" alt="Logo depicting sylised mountains">

# Wandering Heights Hiking Club - Trip Pages

Note: It is not the intention to create a full club website, just the pages required to handle trip creation and signup; specifically:
</br>
•	Login – For registered users (club members) entered into the system by an administrator.
•	Home – Brief description of the club with instructions and rules for trip creation and signup.
•	Trips – List of trips, access to trip details and buttons for signup, withdrawal, creation, editing and deletion.
•	Privacy Policy.



List of Citations
1.	ASP.NET Core MVC with EF Core - tutorial series (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-3.1)
2.	Contrast checker: https://webaim.org/resources/contrastchecker/
3.	The code for the tooltip was modified from that given at W3Schools https://www.w3schools.com/css/css_tooltip.asp






Table: Test Values
Property	Required / Optional	Constraints (value or no. of chars.)	Valid Example	Counter examples
		Min 	Max 	Other		
Date	Required				30-Apr-20	31-Apr-21	
Title	Required	5	30	a	Turbine	Here	 Turbine	Turbine 	Turbine.	turbine	[> 30 characters]
Distance	Optional	0 	100 		12	long	-1	101	
Elevation Gain	Optional	-5000 	5000		500	high	-5001	5001	
Description	Optional	0 	800	b	Very nice.	 Very nice.	Very nice. 	very nice.	[> 800 characters]
Maximum Participants	Required	2 	12 		5	5.5			

a.	Starts with a capital letter and with no leading or trailing spaces and no full stop at the end.
b.	Starts with a capital letter and with no leading or trailing spaces.
