using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RhythmsGonnaGetYou
{
    class Program
    {
        static string PromptForString(string prompt)
        {
            Console.WriteLine(prompt);
            var userInput = Console.ReadLine();
            return userInput;
        }
        static int PromptForInteger(string prompt)
        {
            var isThisGoodInput = false;
            while (isThisGoodInput == false)
            {
                Console.Write(prompt);
                int userInput;
                isThisGoodInput = Int32.TryParse(Console.ReadLine(), out userInput);
                if (isThisGoodInput)
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("I'm sorry that is not a valid response. ");
                }
            }
            return 0;
        }

        static void Main(string[] args)
        {
            var context = new RecordLabelContext();
            var keepGoing = true;
            while (keepGoing)
            {
                var choice = PromptForInteger("What would you like to do?\n(1)Sign a band\n(2)View bands\n(3)Add an album to a band\n(4)Add a song to an album\n(5)Fire a band\n(6)Resign a band\n(7)View all albums by Band\n(8)Quit\n ");

                switch (choice)
                {
                    case 1:
                    case 2:
                        var viewBands = PromptForString("Which bands would you like to see?\n(S)igned bands\n(U)nsigned bands");
                        if (viewBands == "S")
                        {

                        }
                        if (viewBands == "U")
                        {

                        }
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        var bandToView = PromptForString("Which band would you like to view?");
                        var existingBand = context.Bands.FirstOrDefault(band => band.Name == bandToView);
                        if (existingBand != null)
                        {
                            var response = PromptForString("How would you like to view?\n(N)ame of album\n(R)elease date ");
                            if (response == "N")
                            {

                            }
                            if (response == "R")
                            {

                            }
                        }
                        else
                        {
                            Console.WriteLine($"{bandToView} was not found. ");
                        }
                        break;
                    case 8:
                        keepGoing = false;
                        break;

                }
            }



        }

    }
}

