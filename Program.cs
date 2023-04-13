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
            var userInput = Console.ReadLine().ToUpper();
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
                var choice = PromptForInteger("What would you like to do?\n(1)Sign a band\n(2)View bands\n(3)Add an album to a band\n(4)Add a song to an album\n(5)Fire a band\n(6)Resign a band\n(7)View all albums by Band\n(8)Quit\n");

                switch (choice)
                {
                    case 1:
                        var name = PromptForString("What is the name of the Band? ");
                        var country = PromptForString("What is their country of origin?");
                        var members = PromptForInteger("How many members does the band have? ");
                        var site = PromptForString("What is their website? ");
                        var style = PromptForString("What genre are they? ");
                        var contact = PromptForString("Who is their manager? ");
                        var number = PromptForInteger("What is their manager's phone number? (numbers only) ");

                        var newBand = new Band
                        {
                            Name = name,
                            CountryOfOrigin = country,
                            NumberOfMembers = members,
                            Website = site,
                            Style = style,
                            IsSigned = true,
                            ContactName = contact,
                            ContactPhoneNumber = number
                        };
                        context.Bands.Add(newBand);
                        context.SaveChanges();
                        break;

                    case 2:
                        var viewBands = PromptForString("Which bands would you like to see?\n(S)igned bands\n(U)nsigned bands");
                        if (viewBands == "S")
                        {
                            var signedBand = context.Bands.Where(b => b.IsSigned == true);
                            if (signedBand.Count() < 1)
                            {
                                Console.WriteLine("No bands matching that criteria were found. ");
                            }
                            else
                            {
                                Console.WriteLine($"-SIGNED BANDS-");
                                foreach (var band in signedBand)
                                {
                                    Console.WriteLine($"-{band.Name}-");
                                }
                            }
                        }
                        if (viewBands == "U")
                        {
                            var unsignedBand = context.Bands.Where(b => b.IsSigned == false);
                            if (unsignedBand.Count() < 1)
                            {
                                Console.WriteLine("No bands matching that criteria were found. ");
                            }
                            else
                            {
                                Console.WriteLine($"-UNSIGNED BANDS-");
                                foreach (var band in unsignedBand)
                                {
                                    Console.WriteLine($"-{band.Name}-");
                                }
                            }
                        }
                        break;

                    case 3:
                        var bandToAdd = PromptForString("What band are we adding an album to? ");
                        var foundBand = context.Bands.FirstOrDefault(b => b.Name == bandToAdd);
                        if (foundBand != null)
                        {
                            var albumName = PromptForString("What is the name of the album? ");
                            var warning = PromptForString("Does this album need and advisory warning? [Y/N]");
                            bool warn = true;
                            if (warning == "Y")
                            {
                                warn = true;
                            }
                            if (warning == "N")
                            {
                                warn = false;
                            }
                            var releaseDate = PromptForString("When was this album released? [MM/DD/YYYY]");
                            var parseDate = DateTime.Parse(releaseDate);

                            var newAlbum = new Album
                            {
                                Title = albumName,
                                IsExplicit = warn,
                                ReleaseDate = parseDate,
                                BandId = foundBand.Id,
                            };

                            context.Albums.Add(newAlbum);
                            context.SaveChanges();
                        }
                        else
                            Console.WriteLine($"Sorry {bandToAdd} couldn't be found. ");
                        break;

                    case 4:
                        var albumToAdd = PromptForString("Which album are we adding songs to?");
                        var foundAlbum = context.Albums.FirstOrDefault(a => a.Title == albumToAdd);
                        if (foundAlbum != null)
                        {
                            var track = PromptForInteger("What is the track number? ");
                            var songName = PromptForString("What is the name of the song?");
                            var duration = PromptForString("What is the duration of the song? [mm:ss] ");
                            var parseDuration = TimeOnly.Parse(duration);

                            var newSong = new Song
                            {
                                TrackNumber = track,
                                Title = songName,
                                Duration = parseDuration,
                                AlbumId = foundAlbum.Id
                            };
                            context.Songs.Add(newSong);
                            context.SaveChanges();
                        }
                        else
                            Console.WriteLine($"Sorry {albumToAdd} couldn't be found. ");
                        break;

                    case 5:
                        var bandToFire = PromptForString("Which band would you like to fire?");
                        var bandFired = context.Bands.FirstOrDefault(b => b.Name.ToUpper() == bandToFire);
                        if (bandFired != null)
                        {
                            bandFired.IsSigned = false;
                            context.SaveChanges();
                        }
                        else
                            Console.WriteLine($"Sorry. {bandToFire} was not found. ");
                        break;
                    case 6:
                        var bandToResign = PromptForString("Which band would you like to fire?");
                        var bandResigned = context.Bands.FirstOrDefault(b => b.Name.ToUpper() == bandToResign);
                        if (bandResigned != null)
                        {
                            bandResigned.IsSigned = true;
                            context.SaveChanges();
                        }
                        else
                            Console.WriteLine($"Sorry. {bandToResign} was not found. ");
                        break;
                    case 7:
                        var bandToView = PromptForString("Which band would you like to view?");
                        var existingBand = context.Bands.FirstOrDefault(band => band.Name.ToUpper() == bandToView);
                        if (existingBand != null)
                        {
                            var response = PromptForString("How would you like to view?\n(N)ame of album\n(R)elease date ");
                            if (response == "N")
                            {
                                var byName = context.Albums.Where(a => a.Band == existingBand).OrderBy(a => a.Title).Include(a => a.Songs);
                                foreach (var album in byName)
                                {
                                    Console.WriteLine($"{album.Title} ");
                                    foreach (var song in album.Songs.OrderBy(s => s.TrackNumber))
                                    {
                                        Console.WriteLine($"-{song.TrackNumber}. {song.Title} - {song.Duration.ToString("mm:ss")} ");
                                    }
                                }
                            }
                            if (response == "R")
                            {
                                var byDate = context.Albums.Where(a => a.Band == existingBand).OrderBy(a => a.ReleaseDate).Include(a => a.Songs);
                                foreach (var album in byDate)
                                {
                                    Console.WriteLine($"{album.Title} Released on {album.ReleaseDate.ToString("MM/dd/yyyy")} ");
                                    foreach (var song in album.Songs.OrderBy(s => s.TrackNumber))
                                    {
                                        Console.WriteLine($"-{song.TrackNumber}. {song.Title} - {song.Duration.ToString("mm:ss")} ");
                                    }
                                }
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

