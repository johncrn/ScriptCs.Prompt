using System;
using Xunit;
using ScriptCs.Prompt;
using Moq;
using Should;
using Should.Core;
using Should.Fluent;

namespace ScriptCs.Prompt.Tests
{
    public class PromptTests
    {
        public class TheForMethod
        {
            [Fact]
            public void ValidInput_ShouldReturnInt()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).Returns("1234");
                
                var prompt = new Prompt(moqConsole.Object);
                
                var result = prompt.For<int>("Enter a valid number");

                result.GetType().Should().Equal(typeof(int));
                result.Should().Equal(1234);
            }

            [Fact]
            public void InvalidInput_ShouldPromptAgain()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).ReturnsInOrder("asdf1234","1234");

                var prompt = new Prompt(moqConsole.Object);

                var result = prompt.For<int>("Enter a valid number");
                
                moqConsole.Verify(c => c.ReadLine(), Times.Exactly(2)); 
                result.GetType().Should().Equal(typeof(int));
                result.Should().Equal(1234);
            }

            [Fact]
            public void ValidInput_ShouldReturnDateTime()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).Returns("10 feb 1970");

                var prompt = new Prompt(moqConsole.Object);

                var result = prompt.For<DateTime>("Enter a valid date");

                result.GetType().Should().Equal(typeof(DateTime));
                result.Should().Equal(new DateTime(1970, 2, 10));
            }

            [Fact]
            public void BlankInputWithDefault_ShouldReturnDefault()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).Returns("");

                var prompt = new Prompt(moqConsole.Object);

                var today = DateTime.Now.Date;

                var result = prompt.For<DateTime>("Enter a valid date", defaultVal: today, defaultValLabel:"TODAY");

                result.GetType().Should().Equal(typeof(DateTime));
                result.Should().Equal(today);
            }

            [Fact]
            public void ValidInput_ShouldReturnBoolean()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).ReturnsInOrder("Y","true");

                var prompt = new Prompt(moqConsole.Object);

                var result = prompt.For<bool>("Good? Y/N");

                result.GetType().Should().Equal(typeof(bool));
                result.Should().Equal(true);
            }

            [Fact]
            public void ValidInputThatFailsCustomValidation_ShouldPromptAgain()
            {
                var moqConsole = new Mock<IConsole>();
                var futureDate = DateTime.Now.AddDays(10).Date;
                moqConsole.Setup(c => c.ReadLine()).ReturnsInOrder("10 feb 1970", futureDate.ToString());

                var prompt = new Prompt(moqConsole.Object);

                var result = prompt.For<DateTime>("Enter a future date", validator:
                    (input) =>
                    {
                        if (input <= DateTime.Now)
                        {
                            return "Date cannot be in the past";
                        }
                        return null;
                    }
                    );

                result.GetType().Should().Equal(typeof(DateTime));
                result.Should().Equal(futureDate);
            }

            [Fact]
            public void ValidInputWithCustomConverter_ShouldReturnBool()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).Returns("YES");

                var prompt = new Prompt(moqConsole.Object);
                var boolConverter = new BoolConverter();

                var result = prompt.For<bool>("Good? Y/N", converter: boolConverter);

                result.GetType().Should().Equal(typeof(bool));
                result.Should().Equal(true);
            }

            [Fact]
            public void ConverterThanCantConvertFromString_ShouldThrowException()
            {
                var moqConsole = new Mock<IConsole>();
                moqConsole.Setup(c => c.ReadLine()).Returns("YES");

                var prompt = new Prompt(moqConsole.Object);
                var brokenConverter = new BrokenConverter();

                Assert.Throws<NotSupportedException>(() => prompt.For<int>("will fail", converter: brokenConverter));
            }

            [Fact]
            public void BlankInput_ShouldNotBeValidated()
            {
                var moqConsole = new Mock<IConsole>();
                var futureDate = DateTime.Now.AddDays(10).Date;
                moqConsole.Setup(c => c.ReadLine()).ReturnsInOrder("", futureDate.ToString());

                var prompt = new Prompt(moqConsole.Object);

                var result = prompt.For<DateTime>("Enter a date");

                result.GetType().Should().Equal(typeof(DateTime));
                result.Should().Equal(futureDate);
            }

        }
    }
}
