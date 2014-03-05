ScriptCs.Prompt
==============================

A [ScriptCS](https://github.com/scriptcs/scriptcs) script pack for interactive console scripts to prompt for and validate user input. 

## Usage

The For method prompts the user for input and returns the user input converted into the specified type. 

```cs
var prompt = Require<Prompt>();

var num = prompt.For<int>("Enter a number");

Console.WriteLine("You entered " + num.ToString());

var dt = prompt.For<DateTime>("Enter a date", defaultVal: DateTime.Now.Date, defaultValLabel: "Today");

Console.WriteLine("a date: " + dt.ToString());

var dt2 = prompt.For<DateTime>("Enter a future date", validator: (input) => 
	{
		if (input <= DateTime.Now) 
			return "input is in the past";
		else 
			return null;
	});
```

### For Method Parameters

* `prompt` **required**: The prompt displayed to the user
* `defaultVal` *optional*: The default value returned if no input is entered
* `defaultValLabel` *optional*: If a defaultVal is supplied, this label will be used instead of the value in the prompt. (e.g. you may wish to display TODAY instead of the full date)
* `validator` *optional*: Runs after the type conversion has taken place. If the function returns a string it will be displayed and the user will be reprompted for a valid value.
* `converter` *optional*: Overrides the converter supplied by the converterFactory (which defaults to TypeDescriptor.GetConverter unless overriden in constructor)