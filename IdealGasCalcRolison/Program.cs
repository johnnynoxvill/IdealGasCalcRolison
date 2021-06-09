/* Thomas Rolison, trolison1@cnm.edu
 * IdealGasCalculatorThomasR.cs
 * 06/04/21
 */


using System;
using System.IO;
using System.Linq;

namespace idealGasCalculator
{
    class Program
    {
        //write about program
        static void DisplayHeader()
        {
            Console.WriteLine("This is a calculator to find the pressure " +
             "exerted by a gas in a container using the following inputs from " +
             "the user: the name of the gas, the volume of the gas container " +
             "in cubic meters, the weight of the gas in grams and the temperature " +
             "of the gas in degrees Celsius");
        }

        static void GetMolecularWeights(ref string[] gasNames, ref double[] molecularWeights, out int count)
        {


            using (var sr = new StreamReader("MolecularWeightsGasesAndVapors.csv"))
            {
                //initialize variables and read first line
                string line;
                sr.ReadLine();
                int totalGases = 0;

                //read through each line and add to arrays
                while ((line = sr.ReadLine()) != null)
                {
                    string[] subs = line.Split(',');

                    string names = subs[0];
                    double num = double.Parse(subs[1]);

                    gasNames[totalGases] = names;
                    molecularWeights[totalGases] = num;

                    totalGases++;

                }
                //get total number of gases in arrays and write info to console
                count = totalGases;

                Console.WriteLine($"The total number of elements is {count}.");

            }
        }

        private static void DisplayGasNames(string[] gasNames, int countGases)
        {
            //display gas names, 3 by line. Not sure how to format this properly, was having trouble with it.
            for (int i = 0; i < 82; i += 3)
            {
                Console.WriteLine($"{gasNames[i]}, {gasNames[i + 1]}, {gasNames[i + 2]}");
            }
        }

        private static double GetMolecularWeightFromName(string gasName, string[] gasNames, double[] molecularWeights, int countGases)
        {
            //initialize variables
            int numOfNameId = 0;
            bool checkUserInput = false;

            while (checkUserInput == false)
            {
                //get user input for name of gas and check if gas is valid input
                Console.WriteLine("What gas would you like to choose?");
                gasName = Console.ReadLine();
                if (gasNames.Contains(gasName))
                {
                    Console.WriteLine($"Gas selected: {gasName}");
                    numOfNameId = Array.IndexOf(gasNames, gasName);
                    checkUserInput = true;
                }
                else
                {
                    Console.WriteLine("Sorry, that's not an element.");
                    checkUserInput = false;
                }
            }
            //write to console mol weight of gas and return variable
            double molWeight = (double)molecularWeights.GetValue(numOfNameId);
            Console.WriteLine($"Molecular weight for {gasName} is {molWeight}");
            return molWeight;

        }

        static double NumberOfMoles(double mass, double molWeight)
        {
            //get weight from user and calculate value of n
            Console.WriteLine("What is the weight of the gas in grams?");
            double v = Convert.ToDouble(Console.ReadLine());
            mass = v;

            double n = mass / molWeight;
            return n;
        }

        static double CelsiusToKelvin(double celsius)
        {
            //get temperature in kelvin from celsius
            Console.WriteLine("What is the temperature of the gas in degrees Celsius");
            double temp = Convert.ToDouble(Console.ReadLine());

            double tempKelvin = temp + 273.15;

            return tempKelvin;
        }

        static double Pressure(double mass, double vol, double temp, double molecularWeight)
        {
            //since R is a constant, initialize as constant
            double RCONSTANT = 8.3145;
            //get volume from user of container and calculate pressure/write to console
            Console.WriteLine($"What is the volumn in cubic meters of the container?");
            vol = Convert.ToDouble(Console.ReadLine());

            double pressure = (NumberOfMoles(mass, molecularWeight) * RCONSTANT * CelsiusToKelvin(temp)) / vol;

            return pressure;

        }

        private static void DisplayPresure(double pressure)
        {
            //calculate and display pressure in pascals
            PaToPSI(pressure);
            Console.WriteLine($"The pressure in pascals is {pressure}");

        }

        static double PaToPSI(double pascals)
        {
            //calculate pressure of psi from pascals and write to console.
            double psi = 0.00014503773;

            double paToPsi = pascals * psi;

            Console.WriteLine($"The pressure in PSI is {paToPsi}");
            return paToPsi;
        }

        static void Main(string[] args)
        {
            //initializing variables
            string[] gasNames = new string[84];
            double[] molecularWeights = new double[84];
            int countGases = 0;
            double mass = 0;
            double vol = 0;
            double temp = 0;
            string gasName = "";
            DisplayHeader();
            bool answer;
            //do loop to check if user wants to continue on getting information
            do
            {
                //run through each method, and loop if user wants to do multiple calculations
                GetMolecularWeights(ref gasNames, ref molecularWeights, out int numberOfGases);

                DisplayGasNames(gasNames, numberOfGases);

                double molWeight = GetMolecularWeightFromName(gasName, gasNames, molecularWeights, countGases);
                double pressure = Pressure(mass, vol, temp, molWeight);

                DisplayPresure(pressure);

                Console.WriteLine("Would you like to calculate another gas? yes or no");
                string userAnswer = Console.ReadLine();
                if (userAnswer == "yes")
                {
                    answer = true;
                }
                else
                {
                    answer = false;
                }

            } while (answer == true);

            Console.WriteLine("Have a nice day!");
        }

    }
}
