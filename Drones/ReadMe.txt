
								## REST API DRONE DISPATCH SERVICE  ##

## INTRODUCTION

This service has fleet of **drones** (maximum of 10). A drone is capable of loading and dispatching medications.


## ASSUMPTIONS

* After successful registration, a drone is automatically in **loading state** and available for dispatch.
* Maximum allowable fleet is 10.
* A drone can only load a medication per trip.
* The  **model** of drone is mapped to Ids i.e   Lightweight = 1,   Middleweight = 2,    Cruiserweight = 3,  Heavyweight = 4 .
* The **state** of drone is mapped to Ids i.e  Idle = 0,   Loading = 1,   Loaded = 2,   Delivering = 3,    Delivered = 4,  Returning = 5  .
* Medication delivery status are mapped to Ids i.e   Loaded = 1,  InTransit = 2,  Delivered = 3  .
* After successful loading of medication, drone state changes to **Loaded**, medication delivery status changes to **Loaded** too.
* Drone state and battery level changes in succession, at every 2 minutes or based on time of battery check (battery check drains battery life).
* Medication delivery status changes as drone state changes
* A drone remain unavailable for loading as long as its state is not in **Loading** and its battery level is less than **25%**.


## FUNCTIONALITIES

* This drone dispatch service can:
	- Create drones, 
	- Load drones with medication for delivery
	- Track the drone and medication delivery status
	- Check the remaining battery life of the drone
	- Check availability of drone for dispatch
	- Check the audit log of drone activity
	- Check fleet of all drone 

## GUIDE ON HOW TO BUILD, RUN AND TEST THE API


**Note**
 This project is developed in visual studio 19 using ASP.NetCore API
 API uses in-memory database, default data is preloaded at launch time.
 API can be tested with swagger documenation at https://localhost:5001/swagger

 * In terminal environment, do git clone https://github.com/adewalegbadamosi/dronedispatch.git 
 * Change directory into project folder
 * Open project in IDE , visual studio. and restore dependencies
 * Click build in the top menu and ensure project build successfully
 * Click run button (green button at the top menu) to run in project mode not IIS
 * Project will automatically run in default browser interface
 * Test API with **Swagger documenatation** at https://localhost:5001/swagger
 * While testing, please make use of mapped Ids for **Drone state** and **model** as input parameters

