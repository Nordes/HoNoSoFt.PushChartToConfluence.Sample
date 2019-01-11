# HoNoSoFt.PushChartToConfluence.Sample
This repository exists for the sole purpose of the article available at https://blog.honosoft.com/.

## What is it doing
* Generate a graph (chart) using Chart.js
* Push the chart (canvas) into a Confluence page.

## Configuration
Don't forget to configure the project, otherwise you won't be able to use it. The code has been done using the basic authentication. In case you want to create, please read the documentation from Atlassian.

```json
  "Confluence": {
    "Username": "username",
    "Password": "password",
    "baseApiUri": "https://confluence.on.yoursite.com/rest/api/"
  }
```

## In case you want to quick prototype in c#
Here's some useful link:

* To convert your curl request to c# => https://github.com/olsh/curl-to-csharp
* Convert JSON to C#  ==> https://app.quicktype.io/#l=cs&

## Author
* @Nordes (Nordès Ménard-Lamarre)

## License
MIT