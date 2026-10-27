using System;
using System.Security.Cryptography;

public class Program
{
    static void Main()
    {
        const string MenuTitle = "===== MAIN MENU - CODEQUEST =====";
        const string MenuOption1 = "1. Train your wizard";
        const string MenuOption2 = "2. Check the dungeon";
        const string MenuOption3 = "3. Loot the mine";
        const string MenuOptionExit = "0. Exit game";
        const string MenuPrompt = "Choose an option (1-3) - (0) to exit: ";

        const string ErrorNotNumber = "Error: you must type a number, not text.";
        const string ErrorTooBig = "Error: that number is too big.";
        const string ErrorRange = "Error: the option must be between 0 and 3.";
        const string ErrorNoInput = "Error: no input received.";

        // Valor centinella: -1 NO és una opció vàlida.
        // El necessitem perquè el compilador no garanteix que 'option' s'assigni
        // dins del try (pot saltar una excepció abans d'arribar a l'assignació).
        int option = -1;
        bool isValid;

        Random rng = new Random();

        do
        {
            isValid = false; // A cada volta suposem que l'entrada NO és vàlida

            Console.WriteLine(MenuTitle);
            Console.WriteLine(MenuOption1);
            Console.WriteLine(MenuOption2);
            Console.WriteLine(MenuOption3);
            Console.WriteLine(MenuOptionExit);
            Console.Write(MenuPrompt);

            try
            {
                // int.Parse pot llançar: FormatException, OverflowException
                // o ArgumentNullException (si ReadLine retorna null amb Ctrl+Z / Ctrl+D)
                option = int.Parse(Console.ReadLine());

                // Validació de rang: si no es compleix, LLANÇEM nosaltres l'excepció
                if (option < 0 || option > 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(option), ErrorRange);
                }

                // Si arribem aquí no ha saltat cap excepció: entrada vàlida
                isValid = true;
                Console.WriteLine($"You chose: {option}");
            }
            catch (FormatException)              // Text que no és un número: "abc", "3.5", ""
            {
                Console.WriteLine(ErrorNotNumber);
            }
            catch (OverflowException)            // Número que no cap en un int: "99999999999"
            {
                Console.WriteLine(ErrorTooBig);
            }
            catch (ArgumentNullException)        // ReadLine ha retornat null
            {
                Console.WriteLine(ErrorNoInput);
            }
            catch (ArgumentOutOfRangeException)  // La que hem llançat nosaltres pel rang
            {
                Console.WriteLine(ErrorRange);
            }
            finally
            {
                // S'executa SEMPRE: hi hagi error o no
                Console.WriteLine("----------------------------");
            }

            if (isValid)
            {
                switch (option)
                {
                    case 1:

                        const string WizardName = "Enter your wizard name: ";
                        const string Day = "Day {0} -> {1}, you meditated {2} hours and now your power is worth {3} points!";
                        const string LevelUp = "* -> {0} reached level {1}!";
                        const string TotalLevel = "{0}'s training has finished with {1} points of power!";

                        const string Result1 = "You repeat on June.\nRank: Raoden the Elantrí.";
                        const string Result2 = "You still confuse wand and spoon.\nRank: Zyn the Bugged.";
                        const string Result3 = "You are a Dark Breeze Summoner.\nRank: Arka Nullpointer";
                        const string Result4 = "Wow! You can summon dragons without burning the lab\n Rank: Elarion the Flamed One.";
                        const string Result5 = "You achieved the Arcane Master rank!\nRank: ITB-Wizard the Gray";


                        Console.Write(WizardName);
                        string? name = Console.ReadLine();


                        int points = 0, level = 1;

                        for (int day = 1; day < 6; day++)
                        {
                            int lastLevel = level;
                            points += rng.Next(0, 11);
                            level = 1 + (points / 10);

                            Console.WriteLine(Day, day, name, rng.Next(1, 25), points);
                            if (level != lastLevel) Console.WriteLine(LevelUp, name, level);
                        }

                        Console.WriteLine(TotalLevel, name, points);

                        if (points >= 40) Console.WriteLine(Result5);
                        else if (points >= 35) Console.WriteLine(Result4);
                        else if (points >= 30) Console.WriteLine(Result3);
                        else if (points >= 20) Console.WriteLine(Result2);
                        else Console.WriteLine(Result1);

                        break;

                    case 2:

                        const string Intro = """
                        Your wizard ventures into the Dungeon of the Dragon RAMón the Almighty to obtain its hidden treasure.
                        Each door is protected by a digital access code. You have three (3) attempts per door.
                        """;
                        const string Door = "Your wizard is in front of the {0} door. ";
                        const string Guess = "Guess the code (1-5): ";
                        const string Wrong = "Wrong! {0} guesses left. ";
                        const string Right = "The dragon respects you. You continue to the next level!";
                        const string BadEnding = "The dragon detected your presence and kicked you out of the server!";
                        const string GoodEnding = "You unlocked the last level. You can keep the dragon's treasure!";

                        bool failed = false;

                        Console.WriteLine(Intro);

                        for (int door = 0; door < 3; door++)
                        {
                            if (!failed)
                            {
                                Console.Write(Door, door == 0 ? "1st" : (door == 1 ? "2nd" : "3rd")); // ah, semantics.

                                int answer = rng.Next(1, 6);
                                bool dunnit = false;

                                for (int attempt = 3; attempt > 0; attempt--)
                                {
                                    if (!dunnit)
                                    {
                                        int guess = 0;
                                        bool valid = false;

                                        // get input
                                        while (!valid)
                                        {
                                            Console.Write(Guess);

                                            try
                                            {
                                                guess = Convert.ToInt16(Console.ReadLine());
                                                valid = true;
                                            }
                                            catch (FormatException) { Console.WriteLine(ErrorNotNumber); }
                                            catch (OverflowException) { Console.WriteLine(ErrorTooBig); }
                                            catch (ArgumentNullException) { Console.WriteLine(ErrorNoInput); }
                                        }
                                        // sighhhh
                                        
                                        if (guess == answer)
                                        {
                                            Console.WriteLine(Right);
                                            dunnit = true;

                                            if (door == 2) Console.WriteLine(GoodEnding);
                                        }
                                        else if (attempt != 1) Console.Write(Wrong, attempt - 1);
                                    }
                                }

                                if (!dunnit)
                                {
                                    Console.WriteLine(BadEnding);
                                    failed = true;
                                }
                            }
                            
                        }

                        break;

                }
                
                Console.Write("\n");
            }

            // Repetim si l'entrada ha estat invàlida O si no han demanat sortir.
            // Comprovem primer !isValid perquè, amb un error, 'option' pot
            // valer 0 d'una volta anterior.
        } while (!isValid || option != 0);

        
    }
}
