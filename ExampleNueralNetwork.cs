using System.Linq.Expressions;
using System;

public class ExampleNueralNetwork {

    /*
        Example Nueral Network in C#
        By Alexander Walford 2023
    */

    float TargetAccuracy = 0.2f; // inverted
    int MaxEpochs = 2;
    int CurrentEpoch = 0;

    int[,] InputData = { {1, 2, 3, 4, 5, 6, 7, 8, 9}, {3, 6, 9, 12, 15, 18, 21, 24, 27} }; // the dataset
    int[,] NeuronDataTable = { { 1, 2, 3}, { 0, 0, 0} };
    int[,] NeuronSuccessTable = { { 1, 2, 3 }, { 0, 0, 0 } };
    int ExpectedNext = 30;

    public static void Main()
    {
        ExampleNueralNetwork net = new ExampleNueralNetwork();
        net.Network();
    }

    public void Network ()
    {
        if (CurrentEpoch < MaxEpochs)
        {
            try {
                Console.WriteLine("EPOCH " + CurrentEpoch);

                // layer 1
                // factoring the input node into 3 nuerons

                Random rnd = new Random();

                int[] nueron_0 = { InputData[1, 0], InputData[1, 1], InputData[1, 2] };
                // attempt to incrment values

                int NextNum = rnd.Next(0, 9);

                if (NextNum == NeuronSuccessTable[0, 0])
                {
                    // check to see if last entry was 0
                    if (NeuronSuccessTable[0, CurrentEpoch] == 0)
                    {
                        // failed
                        Console.WriteLine("FAILED EPOCH: NUERON 0");
                        Network();
                    }
                }

                int NextNum2 = rnd.Next(0, 9);

                if (NextNum2 == NeuronSuccessTable[0, 1])
                {
                    // check to see if last entry was 0
                    if (NeuronSuccessTable[1, CurrentEpoch] == 0)
                    {
                        // failed
                        Console.WriteLine("FAILED EPOCH: NUERON 1");
                        Network();
                    }
                }

                int NextNum3 = rnd.Next(0, 9);

                if (NextNum3 == NeuronSuccessTable[0, 2])
                {
                    // check to see if last entry was 0
                    if (NeuronSuccessTable[2, CurrentEpoch] == 0)
                    {
                        // failed
                        Console.WriteLine("FAILED EPOCH: NUERON 2");
                        Network();
                    }
                }

                nueron_0[0] = nueron_0[0] + NeuronDataTable[CurrentEpoch, 0] + NextNum;
                nueron_0[1] = nueron_0[1] + NeuronDataTable[CurrentEpoch, 0] + NextNum;
                nueron_0[2] = nueron_0[2] + NeuronDataTable[CurrentEpoch, 0] + NextNum;
                NeuronDataTable[CurrentEpoch, 0] = nueron_0[0]; // update table value

                int[] nueron_1 = { InputData[1, 3], InputData[1, 4], InputData[1, 5] };
                // attempt to incrment values
                nueron_1[0] = nueron_1[0] + NeuronDataTable[CurrentEpoch, 1] + NextNum2;
                nueron_1[1] = nueron_1[1] + NeuronDataTable[CurrentEpoch, 1] + NextNum2;
                nueron_1[2] = nueron_1[2] + NeuronDataTable[CurrentEpoch, 1] + NextNum2;
                NeuronDataTable[CurrentEpoch, 1] = nueron_1[0]; // update table value

                int[] nueron_2 = { InputData[1, 6], InputData[1, 7], InputData[1, 8] };
                // attempt to incrment values
                nueron_2[0] = nueron_2[0] + NeuronDataTable[CurrentEpoch, 2] + NextNum3;
                nueron_2[1] = nueron_2[1] + NeuronDataTable[CurrentEpoch, 2] + NextNum3;
                nueron_2[2] = nueron_2[2] + NeuronDataTable[CurrentEpoch, 2] + NextNum3;
                NeuronDataTable[CurrentEpoch, 2] = nueron_2[0]; // update table value

                // now estimate their accuracy
                float Accuracy_0;
                Accuracy_0 = (ExpectedNext - nueron_0.Sum()) / 10;
                Console.WriteLine("Accuracy Nueron 0: -0." + Accuracy_0);
                if (Accuracy_0 > TargetAccuracy)
                {
                    Console.WriteLine("FAIL: " + nueron_0.Sum());
                }
                else if(Accuracy_0 < TargetAccuracy) { 
                    Console.WriteLine("PASS: " + nueron_0.Sum());
                    NeuronSuccessTable[CurrentEpoch, 0] = nueron_1.Sum();
                }


                float Accuracy_1;
                Accuracy_1 = (ExpectedNext - nueron_1.Sum()) / 10;
                Console.WriteLine("Accuracy Nueron 1: -0." + Accuracy_1);
                if (Accuracy_1 > TargetAccuracy)
                {
                    Console.WriteLine("FAIL: " + nueron_1.Sum());
                }
                else if (Accuracy_1 < TargetAccuracy)
                {
                    Console.WriteLine("PASS: " + nueron_1.Sum());
                    NeuronSuccessTable[CurrentEpoch, 1] = nueron_1.Sum();
                }
                else
                {
                    Console.WriteLine("FAIL: " + nueron_1.Sum());
                }

                float Accuracy_2;
                Accuracy_2 = (ExpectedNext - nueron_2.Sum()) / 10;
                Console.WriteLine("Accuracy Nueron 2: -0." + Accuracy_2);
                if (Accuracy_2 > TargetAccuracy)
                {
                    Console.WriteLine("FAIL: " + nueron_2.Sum());
                }
                else if (Accuracy_2 < TargetAccuracy)
                {
                    Console.WriteLine("PASS: " + nueron_2.Sum());
                    NeuronSuccessTable[CurrentEpoch, 2] = nueron_2.Sum();
                }
                else
                {
                    Console.WriteLine("FAIL: " + nueron_2.Sum());
                }

                CurrentEpoch++;
                Network();

            }
            catch (Exception e) {
                Console.WriteLine("ERROR: " + e);
            }
        }
        else
        {
            // we're done! print results and save model
            string data = "";
            Console.WriteLine("Expected Next Value:");
            foreach (int i in NeuronSuccessTable)
            {
                data = i.ToString();
            }
            File.WriteAllText("ProbabilityTable.model", data);
            Console.WriteLine(data);
        }
    }
}
