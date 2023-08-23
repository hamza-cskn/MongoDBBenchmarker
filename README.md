# MongoDB Benchmarker

This is a simple tool to benchmark MongoDB performance. It allows to bulk updates, inserts, reads and deletes. It generates random data as you want. For each query it measures the time it took to execute it and the number of operations per second.

## Installing

### With Clonning Repository
```
$ git clone git@github.com:hamza-cskn/MongoDBBenchmarker.git
$ cd MongoDBBenchmarker
$ dotnet run .
```

### With Docker
not supported yet

## Usage

<img width="457" alt="image" src="https://github.com/hamza-cskn/MongoDBBenchmarker/assets/36128276/d889097c-4442-43c8-974e-47ea0e0aa87d">

### Placeholders
* **Id:** Iteration number
  * Format: `%id%`
* **Random String** randomized string with specific length
  * Format: `%string(n)%`
* **Random Integer** random integer in specific range.
  * Format: `%int(5,10)%`
