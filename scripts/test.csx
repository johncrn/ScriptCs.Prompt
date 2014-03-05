var prompt = Require<Prompt>();

var num = prompt.For<int>("Enter a number");

Console.WriteLine("Good STuff " + num.ToString());

var dt = prompt.For<DateTime>("Enter a date", defaultVal: DateTime.Now.Date, defaultValLabel: "Today");

Console.WriteLine("a date: " + dt.ToString());

var dt2 = prompt.For<DateTime>("Enter a future date", validator: (input) => 
	{
		if (input <= DateTime.Now) 
			return "input is in the past";
		else 
			return null;
	});