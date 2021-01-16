using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Archeology_Tool
{
    class Tool
    {
        static Collection[] collections;

        static void Main(string[] args) {
            collections = LoadCollections();
            Menu();   
        }

        // Function that starts a new screen
        static void ClearScreen() {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("***** RUNESCAPE ARCHEOLOGY TOOL *****\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        // Function that asks user to press enter to continue
        static void PressEnter() {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

        // Function for the main menu
        static void Menu() {
            while (true) {
                ClearScreen();
                
                // Main menu header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Welcome at the Runescape Archeology Tool!\n\nYou can here: \n- Store your artefacts \n- Check your collection progress.\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Make an option:\n" +
                                  "1) Collections \n" +
                                  "2) Exit \n");

                string userInput = Console.ReadLine();
                int choice;
                // Checks user input 
                if (int.TryParse(userInput, out choice)) {
                    if (choice == 1) {
                        // Go to collections menu
                        CollectionsMenu();
                        break;
                    } else if (choice == 2) {
                        // Quit application
                        break;
                    } else {
                        // Error message for invalid input
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Choose number 1 or 2");
                        Console.ForegroundColor = ConsoleColor.White;
                        PressEnter();
                    }
                } else {
                    // Error message for invalid input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter a number");
                    Console.ForegroundColor = ConsoleColor.White;
                    PressEnter();
                }
            }
        }

        // Function for the collections menu
        static void CollectionsMenu() {
            while (true) {
                ClearScreen();
                int totalCollections = collections.Length;

                // Collections menu header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("~~ COLLECTIONS ~~\n");
                Console.ForegroundColor = ConsoleColor.White;

                // Show all possible collections:
                //      If-else is to check at which collection the application is to improve the layout of the menu
                for (int count = 1; count < totalCollections; count++) { 
                    if (count < 10)
                        Console.WriteLine(" " + count + ") " + collections[count - 1].GetCollectionName());
                    else
                        Console.WriteLine(count + ") " + collections[count - 1].GetCollectionName());
                    if (count == 3 || count == 7 || count == 10 || count == 13 || count == 17 || count == 21 || count == 25)
                        Console.WriteLine("");
                }
                Console.WriteLine("\n 0) Go back\n");

                string userInput = Console.ReadLine();
                int choice;
                // Checks user input
                if (int.TryParse(userInput, out choice)) {
                    if (choice > 0 && choice <= totalCollections - 1) {
                        // Go to selected collection
                        ManageCollection(collections[choice-1]);
                        break;
                    } else if (choice == 0) {
                        // Go back to main menu
                        Menu();
                        break;
                    } else {
                        // Error message for invalid input
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Choose a number from 1 to 24");
                        Console.ForegroundColor = ConsoleColor.White;
                        PressEnter();
                    }
                } else {
                    // Error message for invalid input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choose a number");
                    Console.ForegroundColor = ConsoleColor.White;
                    PressEnter();
                }
            }
        }

        // Function to show the current collection
        static void ManageCollection(Collection currentCollection) {
            while (true) {
                ClearScreen();
                bool isComplete;
                int completeCounter = 0;

                // Check if all the artefacts exists at least once
                for (int i = 0; i < currentCollection.GetAllArtefacts().Length; i++) {
                    if (currentCollection.GetArtefact(i).GetRepairedAmount() > 0) {
                        completeCounter++;
                    }
                }
                isComplete = (completeCounter == currentCollection.GetAllArtefacts().Length);

                // Collection header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("~~ " + currentCollection.GetCollectionName() + " ~~\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Archeology Level: {currentCollection.GetCollectionLevel()}\n");

                // Get all artefacts of the current collection
                int artefactCounter = 0;
                foreach (CollectionItem artefact in currentCollection.GetAllArtefacts()) {
                    artefactCounter++;
                    Console.WriteLine($"{artefactCounter})\n{artefact.GetDamagedName()} : {artefact.GetDamagedAmount()}\n{artefact.GetName()} : {artefact.GetRepairedAmount()}\n");
                }

                // Output when collection is ready to hand in
                if (isComplete) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nCollection complete!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // User input information
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nC = Change item state, H = Hand in collection, S = Save and go back\n");
                Console.ForegroundColor = ConsoleColor.White;

                string userInput = Console.ReadLine().ToLower();
                // Checks user input
                if (userInput == "c") {
                    Console.WriteLine("Choose item to change: ");
                    string numberInput = Console.ReadLine();
                    int number;
                    if (int.TryParse(numberInput, out number)) {
                        if (number > 0 && number <= artefactCounter) {
                            // Ask if user wants to add or repair item
                            //      number-- is to define the correct array index position
                            number--;
                            Console.WriteLine("Add(+) or Repair(-): ");
                            string symbolInput = Console.ReadLine();
                            if (symbolInput == "+") {
                                // Add new damaged artefact
                                currentCollection.GetArtefact(number).AddDamagedArtefact();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Succesfully added damaged artefact!");
                                Console.ForegroundColor = ConsoleColor.White;
                                PressEnter();
                            } else if (symbolInput == "-") {
                                // Check if there is at least one damaged artefact
                                if (currentCollection.GetArtefact(number).GetDamagedAmount() != 0) {
                                    // Repair damaged artefact
                                    currentCollection.GetArtefact(number).RepairDamagedArtefact();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Succesfully repaired artefact!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    PressEnter();

                                } else {
                                    // Error message for invalid input
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("You don't have any artefacts to repair...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    PressEnter();
                                }
                            } else {
                                // Error message for invalid input
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Choose + or -");
                                Console.ForegroundColor = ConsoleColor.White;
                                PressEnter();
                            }
                        } else {
                            // Error message for invalid input
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Choose a number from 1 to " + artefactCounter);
                            Console.ForegroundColor = ConsoleColor.White;
                            PressEnter();
                        }
                    } else {
                        // Error message for invalid input
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Choose a number");
                        Console.ForegroundColor = ConsoleColor.White;
                        PressEnter();
                    }
                } else if (userInput == "h") {
                    if (isComplete) {
                        // Hand in collection: 
                        //      Each repaired artefact -1
                        currentCollection.HandInCollection();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You succesfully handed in the collection!");
                        Console.ForegroundColor = ConsoleColor.White;
                        PressEnter();
                    } else {
                        // Error message for invalid input
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Collection is not complete");
                        Console.ForegroundColor = ConsoleColor.White;
                        PressEnter();
                    }
                } else if (userInput == "s") {
                    // Save collections to JSON and go back to main menu
                    SaveCollections();
                    CollectionsMenu();
                    break;
                } else {
                    // Error message for invalid input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choose C, H or B");
                    Console.ForegroundColor = ConsoleColor.White;
                    PressEnter();
                }
            }
        }

        // Function that gets the collection data from JSON
        private static Collection[] LoadCollections() {
            using (StreamReader reader = new StreamReader(@"../../../data/collections.json", Encoding.GetEncoding("iso-8859-1"))) {
                string json = reader.ReadToEnd();
                List<Collection> collections = JsonConvert.DeserializeObject<List<Collection>>(json);
                Collection[] collectionArr = collections.ToArray();
                return collectionArr;
            }
        }

        // Function that saves the collection data to JSON
        private static void SaveCollections() {
            string json = JsonConvert.SerializeObject(collections, Formatting.Indented);
            File.WriteAllText(@"../../../data/collections.json", json);
        }
    }
}
