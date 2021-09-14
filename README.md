# Json.Net.Curl
## Simple .Net lib for file and http operations. You can store/retrive JSON files from local and remote location   

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

## Supported .Net framework

- .Net core 2.2, 3.0, 3.1
- .Net 5
- .Net Framwrowk 4.5+

## Features

- Get Save JSON on  local file system
- Get Save JSON on  remote location   
- Async method support 

## Get Examples
```sh
// get JObject from local file system 
var json = Json.Net.Curl.Get(@"data\JObjectUnitTest1.json");
var json = await Json.Net.Curl.GetAsync(@"data\JObjectUnitTest1.json")

// get JObject from Server  
var json = await Json.Net.Curl.GetAsync("http://myserver.com/data.json");

// get JArray  from local file system 
var jsonArr = Json.Net.Curl.GetJArray(@"data\JArrUnitTest1.json");
var jsonArr = await Json.Net.Curl.GetJArrayAsync(@"data\JArrUnitTest1.json")

// get JArray from Server  
var jsonArr = await Json.Net.Curl.GetJArrayAsync("http://myserver.com/data_arr.json");

// get Object 

 public class Apple
 {
        public string fruit { get; set; }
        public string size { get; set; }
        public string color { get; set; }
 }
 var json = await Json.Net.Curl.GetAsync<Apple>(@"data\JObjectUnitTest1.json");

 //Get Json from Web using authorization
 var json = await Json.Net.Curl.GetAsync<Apple>(@"http://myserver.com/data.json", new Dictionary<string, string>() {["Authorization"] = "Basic dXNlcjpic2Ux" });
  
 
// Get File List 
 var list = Json.Net.Curl.List(@"directory_path");


```

## Save/Post/Put Examples
```sh
// Save to local file system
Json.Net.Curl.Save(@"data\JObjectSave1.json", new JObject() { 
["id"] = "1",
["user"] = "user1",
["pass"] = "AAA@@@!!!!"
});
   
   
 // Save/send to rest endpoint
Json.Net.Curl.Save(@"http://myserver.com/add", new JObject() { 
["id"] = "1",
["user"] = "user1",
["pass"] = "AAA@@@!!!!"
});

// Save Json Array 

 await Json.Net.Curl.SaveJArrayAsync(@"data\JObjectSaveJArrayAsync1.json", new JArray(){ new JObject()
    {
    ["id"] = "1",
    ["user"] = "User1",
    ["pass"] = "AAA@@@!!!!"
    } 
 });
            
```


## NuGet
https://www.nuget.org/packages/Json.Net.Curl
```sh
Install-Package Json.Net.Curl 
```

