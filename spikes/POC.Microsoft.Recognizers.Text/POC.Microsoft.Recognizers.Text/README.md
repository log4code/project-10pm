## Objectives 
(silent coding session)

- Experiment with MS text 
	regognizers for date/time
	parsing

## Notes

- Culture is VERY important
- May return a reference to anything that resembles a date
	- "Today"
	- "current date"
	- "next Monday"
- Sample result:
``` json
 {
  "Text": "next monday",
  "Start": 12, //start index
  "End": 22, //end index
  "TypeName": "datetimeV2.date",
  "Resolution": {
    "values": [
      {
        "timex": "2023-06-12",
        "type": "date",
        "value": "2023-06-12"
      }
    ]
  }
}
```


## Questions

- How to disregard date recognition that isn't relevant (today, current date, etc.)
- How to go from "next Monday" to a real date?
	- This is possible using the 'Resolution' property of the results:
``` json
 {
  "Text": "next monday",
  "Start": 12,
  "End": 22,
  "TypeName": "datetimeV2.date",
  "Resolution": {
    "values": [
      {
        "timex": "2023-06-12",
        "type": "date",
        "value": "2023-06-12"
      }
    ]
  }
}
```

## Possible strategies
- Diff recognized 'real' dates from text to get informational context
	- "The test is next Monday" => "next Monday", "The test is"