# CheckWorkOrderCosts
Grabs a list of Work Orders completed yesterday and checks them to see if all costs are there and costs/labor/overhead all add up correctly.

This is accomplished by connecting to an oracle database and then using the Dapper ORM to retrieve the info from the db and map it to our 
class models for processing. There is no need to write to the database so I am only using Query method to return a list of info.
