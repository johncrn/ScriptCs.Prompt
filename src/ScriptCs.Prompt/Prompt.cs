using ScriptCs.Contracts;
using System;
using System.ComponentModel;

namespace ScriptCs.Prompt
{
    public class Prompt : IScriptPackContext
    {
        IConsole _console;
        IConverterFactory _converterFactory;

        public Prompt(IConsole console = null, IConverterFactory converterFactory = null)
        {
            _console = console ?? new SystemConsole();
            _converterFactory = converterFactory ?? new SystemConverterFactory();
        }

        /// <summary>
        /// Prompts the user for a valid value. Will continue to re-prompt until one is entered. 
        /// </summary>
        /// <typeparam name="T">The type of value to be input. Supports structs.</typeparam>
        /// <param name="prompt">The prompt displayed to the user</param>
        /// <param name="defaultVal">The default value returned if no input is entered</param>
        /// <param name="defaultValLabel">If a defaultVal is supplied, this label will be used instead of the value in the prompt. (e.g. you may wish to display TODAY instead of the full date)</param>
        /// <param name="validator">Runs after the type conversion has taken place. If the function returns a string it will be displayed and the user will be reprompted for a valid value. </param>
        /// <param name="converter">Overrides the converter supplied by the converterFactory (which defaults to TypeDescriptor.GetConverter unless overriden in constructor)</param>
        /// <returns>A valid value in the specified type converted from user input.</returns>
        public T For<T>(string prompt, T? defaultVal = null, string defaultValLabel = null, Func<T, string> validator = null, IConverter<T> converter = null) where T : struct
        {
            T result = default(T);
            
            if (converter == null)
            {
                converter = _converterFactory.GetConverter<T>();
            }

            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new NotSupportedException(String.Format("{0} cannot convert from string to {1}", converter.GetType().ToString(), typeof(T).ToString()));
            }

            bool validInput = false;

            do
            {
                string promptLine = !defaultVal.HasValue ? String.Format("{0}: ", prompt) : String.Format("{0} (ENTER for {1}): ", prompt, (!String.IsNullOrWhiteSpace(defaultValLabel) ? defaultValLabel : String.Format("\"{0}\"", defaultVal.ToString())));
                _console.WriteLine(promptLine);
                _console.WriteLine();
                _console.Write("> ");

                var userInput = _console.ReadLine();

                // Return the default value if supplied and user hit ENTER
                if (defaultVal.HasValue && String.IsNullOrWhiteSpace(userInput))
                    return defaultVal.Value;

                bool convertSuccess = false;

                if (String.IsNullOrWhiteSpace(userInput))
                {
                    _console.WriteLine("Please enter a value.");
                }
                else
                {
                    // Otherwise try to convert the string to the specified type
                    try
                    {
                        result = converter.ConvertFromString(userInput);
                        convertSuccess = true;
                    }
                    catch
                    {
                        _console.WriteLine("Invalid input. Cannot convert \"{0}\" to {1}.", userInput, result.GetType().ToString());
                    }
                }
                if (convertSuccess)
                {
                    if (validator == null)
                    {
                        validInput = true;
                    }
                    else
                    {
                        var validatorMessage = validator(result);

                        if (String.IsNullOrWhiteSpace(validatorMessage))
                        {
                            validInput = true;
                        }
                        else
                        {
                            _console.WriteLine(validatorMessage);
                        }
                    }
                }

            } while (!validInput);

            return result;
        }

    }
}