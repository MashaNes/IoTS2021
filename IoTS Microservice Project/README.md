# IoTS Microservice Project
This folder holds an IoTS, microservice oriented application for road condition monitoring.
<br/>
The architecture of the system is shown bellow:  
<br/>
![architecture image](https://github.com/MashaNes/IoTS2021/blob/main/IoTS%20Microservice%20Project/Architecture.png)  
<br/>
<br/>
<br/>
"docker-compose.yml" file should be placed in a directory above the cloned repository. In that directory a "Python docker" subdirectory should be created, with the following structure:  
 <br/>
 "Python docker"  
 {  
	&ensp;&ensp;&ensp;python-sensor-temp  
	&ensp;&ensp;&ensp;{  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;python-sensor-temp-1,  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;python-sensor-temp-2,  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;python-sensor-temp-3,  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;python-sensor-temp-4,  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;python-sensor-temp-5  
		<br/>
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;// Each subfolder should contain the contents of the "repo/IoTS Microservice Project/Microservices/Sensor Device Microservices/Temperature sensor" folder from the repository plus a "road-weather-information-station.csv" file with the following format:  
			&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;StationName, StationLocation, DateTime, RecordId, RoadSurfaceTemperature, AirTemperature  
		&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;// Dataset used: https://www.kaggle.com/city-of-seattle/seattle-road-weather-information-stations  
	&ensp;&ensp;&ensp;}  
 }  
 <br/>
 cassandra-command should have the script from repo/Cassandra/CommandMicroservice.txt executed in cqlsh  
 cassandra-analytics should have the script from repo/Cassandra/AnalyticsMicroservice.txt executed in cqlsh  
 cassandra-data should have the script from repo/Cassandra/DataMicroservice.txt executed in cqlsh  
 <br/>
 Run the command from repo/Siddhi/Docker run.txt to start the Siddhi complex event processing app  
 Run "docker compose build" and "docker compose up" in the directory where "docker-compose.yml" is located    
 Run "npm run serve" in repo/Web dashboard/road-monitor  and access the client on localhost:8080
 
