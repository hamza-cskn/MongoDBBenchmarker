# MongoDB Benchmarker

This is a simple tool to benchmark MongoDB performance. It allows to bulk updates, inserts, reads and deletes. It generates random data as you want. For each query it measures the time it took to execute it and the number of operations per second.

## Installing
You need `operations.json` and `operations.log` files to use the application properly. 
* `operations.json` file is a configuration file for bulk operations. You have to configure it for using with Docker. However if you want to use CLI mode, you don't need it.
* `operations.log` file is the log file that will store benchmark results of the executed operations.
### With Clonning Repository
```bash
$ git clone git@github.com:hamza-cskn/MongoDBBenchmarker.git
$ cd MongoDBBenchmarker
$ dotnet run
```

### With Docker
#### Via Compose
Download the `docker-compose.yml` file. Go to directory of the file.
Configure a `operations.json` file. There is an example in the repository.
Create a `operations.log` file. (`touch operations.log` is fine.)
```bash
$ docker-compose up
```
#### Via Command
Configure a `operations.json` file. There is an example in the repository.
Create a `operations.log` file. (`touch operations.log` is fine.)
```bash
$ docker run -v <PATH_TO_CONFIG_FILE>:/app/operations.json:ro \
-v <PATH_TO_LOG_FILE>:/app/operations.log \
--rm mongodb-benchmarker
```

# Features
MongoDB Benchmarker supports Insert, Update, Read and Delete operations.

## Logging
Every operation is stored in operation-log.txt file. Here's example logs.
```log
2023-08-23 21:14:12 - INSERT - Count: 1000, Time: 00:00:00.1622390, Template: {name:"saadasdsa"}
2023-08-23 21:14:33 - INSERT - Count: 100000, Time: 00:00:09.7082600, Template: {name:"saadasdsa"}
2023-08-23 21:14:55 - READ - Count: 101000, Time: 00:00:01.1733350, Filter: { "name" : "saadasdsa" }
2023-08-23 21:15:38 - DELETE - Count: 101000, Time 00:00:16.7651800, Filter: { "name" : "saadasdsa" }
2023-08-23 21:15:42 - READ - Count: 0, Time: 00:00:00.4873030, Filter: { "name" : "saadasdsa" }
2023-08-23 21:24:14 - READ - Count: 5, Time: 00:00:00.5765820, Filter: { "name" : "hamza" }
2023-08-23 21:25:07 - READ - Count: 0, Time: 00:00:00.4542770, Filter: { "name" : "hamza" }
2023-08-23 21:26:25 - READ - Count: 0, Time: 00:00:00.7174220, Filter: { "name" : "Jhon", "age" : 20 }
2023-08-23 21:27:00 - READ - Count: 0, Time: 00:00:00.4747580, Filter: { "name" : "Jhon" }
2023-08-23 21:28:11 - INSERT - Count: 3000, Time: 00:00:00.4328780, Template: {name:"Jhon"}
2023-08-23 21:29:11 - UPDATE - Count: 3000, Time 00:00:00.9136250, Filter: { "name" : "Jhon" }, Updated: { "$set" : { "name" : "notjhon" } }
```

## Placeholders
You can specify placeholders in template documents. The document generator will handle them before bulk insert.
```
{name: "Hamza", age: "%int(10,80)%", resume: "%string(500)%"}
```

### Placeholder List
* **Id:** Iteration number.
  * Format: `%id%`
* **Random String** randomized string with specific length.
  * Format: `%string(n)%`
* **Random Integer** random integer in specific range.
  * Format: `%int(5,10)%`

