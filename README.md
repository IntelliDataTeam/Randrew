# Randrew 1.0
Replacing Humans, one at a time.

<h2>Updates</h2>
<h3>09/22/2015</h3>
-Implemented customizable conditions for each column <br/>
-Moved operations into MainGui from Program for later threading <br/>
-Trimmed some fat <br/>

<h3>08/25/2015</h3>
-Able to update config files from the dev <br/>
-Able to read distinct values from csv files and compare them <br/>
-Reduced processing time for distinct check <br/>
-Able to highlight the error in datagridview <br/>

<h3>08/17/2015</h3>
-Capable of reading and parsing config file for family columns.<br/>
-Able to show data from config file in datagridview.<br/>
<h3>08/07/2015</h3>
-The project will now known as Randrew <br/>
-Switched over to a new framework for reading csv <br/>
-Added bonus personality <br/>
-Improved performance of checks <br/>

<h2>Current Goals</h2>
-<b>Cut down on the distinct values file size (maybe use a new data structure?)</b> <br/>
-Able to edit more than one file consecutively (currently will break if load another file) <br/>
-Add the ability to upload the data right from the program <br/>
---Potentially dangerous as it could run a incorrect query <br/>
---Solution could be to let the user customize and see the query <br/>
-More error messages & notes <br/>
-Create column rule accessibility <br/>
-Encrypt Password <br/>
-Progress Bar <br/>
---Might not be possible since we can't get the line count without greatly affecting performance.<br/>
---Alternative is to have an animation that indicate the process is currently running.<br/>
---Need to get backgroundworker to actively redefining 'Bunny' text field. <br/>
-Implement threading to update GUI while processes are running <br/>

<h2>Current Problems</h2>
<b>Can only edit one file at a time, need to exit out of the program to edit another one </b> <br/>
<b>-Large builds are causing 'outofmemory' errors </b> <br/>
---Unique list run out of memory in trying to store the data for all of the PNs <br/>
<b>-Unique Data Checking is costing too much performance</b> <br/>
---Need to optimize the code (right now is 10x slower with it) <br/>
-Username and Password data are unencrypted <br/>
-Input form are susceptible to sql injection attack <br/>

<h2>Purpose</h2>
The purpose of this program is to assist in the build checking process. The end goal of this project is to use this 
program to eliminate inconsistencies and simple mistakes from the data building process. Accurate and consistence 		data are important for IntelliData since our database is the the important asset that we have. Also, having good 		data cut down on the amount of spring cleanings that we have to do along with cutting down the amount of work 		that the data team have to do.

<h2>Design</h2>
The "language" that this program will be written in is C# because of its simplicity, modern features, and a great
IDE (Visual Studio). C# bring a good balance between performance and simplicity and thus, a good fit for the data 		team.
	
The "form" of this program will be a standalone .EXE. The reasons for this decision because all of the data team
uses Windows, so this type of files are familiar. Also, the decision to not create this program as a part of 			(IntelliPlugin) is because performance won't be tied to excel's performance. Thus, allowing the program to work on 	large datasets much faster.

The "function" of this program is to read the values within each of the columns and make sure that they are 
compliant with the stated rules for those columns. Currently, rules will be hardcoded into the program with extension
to a text file. The text file will contain the distinct values that are deemed acceptable for each column. However, 		the end goal of this program is become completely customizable. Rules are written in text files that the program 		will read from, thus allowing users to change it without having to change the code.
Another goal of this program is to give the user a simple summary of their build data to compare with the data from
the dev. The program should be able to show the user distinct values from each column along, highlighting which value
is new to the dev. Features such as capacitance range in each voltage could help the user spot any error in the 		data.

<h2>Road Map</h2>

	Alpha
		Form
			-A functional GUI.
			-Able to accept user's inputs.
			-Able to output results.
			-Able to work on another computer.
		
		Function
			-Be able to read and manipulate data in a csv file.
			-Check for duplicates in the 'PN' column.
			-Check for Error codes in the data.
			-Get data from the dev.
	
	Beta
		Form
			-A presentable GUI.
			-Output results in a presentable format.
			-Installer must work on another computer with minimum requirements.
			-Cut down on user's inputs.
		
		Function
			-Performance must be noticeable.
			-Able to auto-format data.
			-Compare data with dev data in text file.
			-Able to update exported data from dev.
			-Use regex to check for formats from tol, temprange, features, etc.
			-Show a summary of user's build data.
			-Able to parse rule files.
			-Able to read only from a specific column rather than the entire table.
	
	Release
		???
