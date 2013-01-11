MediaManager 2013
===

What's this about
The media manager is first of all an online database that keeps track of movies you have in your collection and lent to or borrowed from other people using this application. 

The solution consists of two projects, a database and a frontend tier.

===
Database layer MediaManager2010
The MM2010 is a typical database layer component that uses LinqToSql to persists the library in a SQL server (Express) database. 

Database generating Sql schema script
To create the database simply run the generate-movies.sql script in the \DB subfolder from inside the SQL Management Studio.

Amazon webservices 
The service component can utilize a web API by Amazon to retrieve information about movies you want to persist. To get this running you need to provide AWS crendentials. Those credentials must be stored in the tblSettings in the movies database.

