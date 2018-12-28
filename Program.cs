using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestRemainingTimeFirst
{
    class Program
    {
        struct Process
        {
            public int PID;
            public int Arrival;
            public int CPU_Burst;
            public int Waits;
        }

        static List<Process> ShortestRemainingTimeFirst_Gantt_Chart(Process[] Processes)
        {
            int Process_Num = Processes.Count();
            int Total_CPU_Burst = 0, Current_Arrival_Time = Int32.MaxValue;

            Process[] Arrived_Processes = new Process[Process_Num];
            List<Process> Gantt_Chart_Processes = new List<Process>();

            // Here, total CPU Burst Time and the arrival time of the first process is determined
            
            for (int i = 0; i < Process_Num; i++)
            {
                Total_CPU_Burst = Total_CPU_Burst + Processes[i].CPU_Burst;

                if (Processes[i].Arrival < Current_Arrival_Time)
                    Current_Arrival_Time = Processes[i].Arrival;

            }

            // Here, the Gantt Chart is generated

            int j = 0;

            while (Total_CPU_Burst > 0)
            {
                for (int i = 0; i < Process_Num; i++)
                {
                    if (Processes[i].Arrival == Current_Arrival_Time)
                    {
                        Arrived_Processes[j].PID = Processes[i].PID;
                        Arrived_Processes[j].Arrival = Processes[i].Arrival;
                        Arrived_Processes[j].CPU_Burst = Processes[i].CPU_Burst;
                        j = j + 1;
                    }
                }

                int Arrived_Process_Assigned_CPU = -1, Min_CPU_Burst = Int32.MaxValue;

                for (int i = 0; i < j; i++)
                {
                    if (Arrived_Processes[i].CPU_Burst > 0 && Arrived_Processes[i].CPU_Burst < Min_CPU_Burst)
                    {
                        Min_CPU_Burst = Arrived_Processes[i].CPU_Burst;
                        Arrived_Process_Assigned_CPU = i;
                    }
                }

                // CPU Use
                Arrived_Processes[Arrived_Process_Assigned_CPU].CPU_Burst = Arrived_Processes[Arrived_Process_Assigned_CPU].CPU_Burst - 1;
                Current_Arrival_Time = Current_Arrival_Time + 1;
                Total_CPU_Burst = Total_CPU_Burst - 1;

                // Hence, add to the Gantt Chart
                Process Arrived_Process_Used_CPU = new Process();
                Arrived_Process_Used_CPU.PID = Arrived_Processes[Arrived_Process_Assigned_CPU].PID;
                Arrived_Process_Used_CPU.CPU_Burst = 1;

                Gantt_Chart_Processes.Add(Arrived_Process_Used_CPU);

            }

            return Gantt_Chart_Processes;
        }

        static double ShortestRemainingTimeFirst_AWT(Process[] Processes, List<Process> Gantt_Chart_Processes)
        {
            int Process_Num = Processes.Count();

            for (int i = 0; i < Process_Num; i++)
            {
                int Current_Slots = 0, Previous_Slots = 0, Current_Slot_Start = -1;
                int Key = 0;

                for (int j = 0; j < Gantt_Chart_Processes.Count(); j++)
                {
                    if ((Key < 2) && (Processes[i].PID == Gantt_Chart_Processes[j].PID))
                    {
                        if (Current_Slot_Start == -1)
                        {
                            Current_Slot_Start = j;
                        }

                        Current_Slots = Current_Slots + 1;
                        Key = 1;
                    }
                    else if ((Key == 1) && (Processes[i].PID != Gantt_Chart_Processes[j].PID))
                    {
                        Key = 2;
                    }
                    else if ((Key == 2) && (Processes[i].PID == Gantt_Chart_Processes[j].PID))
                    {
                        Previous_Slots = Previous_Slots + Current_Slots;
                        Current_Slots = 1;
                        Current_Slot_Start = j;
                        Key = 1;
                    }
                }

                Processes[i].Waits = Current_Slot_Start - Previous_Slots - Processes[i].Arrival;
            }

            double AWT = 0.0;

            for (int i = 0; i < Process_Num; i++)
            {
                AWT = AWT + Processes[i].Waits;
            }

            AWT = AWT / (double) Process_Num;

            return AWT;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of Processes:");
            int Process_Num = Convert.ToInt32(Console.ReadLine());

            Process[] Processes = new Process[Process_Num]; // Array of Processes

            for (int i = 0; i < Process_Num; i++)
            {
                Console.Write("Process ID:");
                Processes[i].PID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Process Arrival:");
                Processes[i].Arrival = Convert.ToInt32(Console.ReadLine());
                Console.Write("Process CPU Burst:");
                Processes[i].CPU_Burst = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("---------------");
            }

            // Gantt Chart is generated in a List as its elements can grow dynamically

            List<Process> Gantt_Chart_Processes = new List<Process>();
            Gantt_Chart_Processes = ShortestRemainingTimeFirst_Gantt_Chart(Processes);
            
            double SRTF_AWT = ShortestRemainingTimeFirst_AWT(Processes, Gantt_Chart_Processes);

            Console.WriteLine("Average Waiting Time according to Shortest Remaining Time First:" + Convert.ToString(SRTF_AWT));
            
            Console.ReadLine();
        }
    }
}
