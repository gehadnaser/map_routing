using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    public struct nodes
    {
        public Int32 name;
        public double x;
        public double y;
        public double distance;
        public double time;
        public bool visited;
        public Int32 parent;
    }
    class priorityqueue
    {


        public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> d_edges = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
        public static List<query> list_query = new List<query>();
        public static List<nodes> priority_queue = new List<nodes>();
        public static List<nodes> list_vertics = new List<nodes>();
        



        public static void getdata(List<nodes> p_queue, Dictionary<Int32, List<KeyValuePair<Int32, info>>> edges, List<query> query) //o(1)
        {


            list_vertics = p_queue; //o(1)
            d_edges = edges; //o(1)

            list_query = query; //o(1)

        }

        public static Int32 Count { get { return priority_queue.Count; } }

        public static void insert_heap(nodes node) //o(log v)
        {
            priority_queue.Add(node);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (priority_queue[p].time <= node.time) { break; }

               priority_queue[i] = priority_queue[p];
                i = p;
            }

            if (Count > 0)priority_queue[i] = node;
        }
        public static void Min_heap(int i)//o(log v)
        {
            int r = 2 * i + 1; //o(1)
            int l = 2 * i; //o(1)
            int minimum;
            if (l <= priority_queue.Count - 1 && priority_queue[l].time < priority_queue[i].time) //o(1)
            {
                minimum = l; //o(1)
            }
            else { minimum = i; } //o(1)
            if (r <= priority_queue.Count - 1 && priority_queue[r].time < priority_queue[minimum].time) //o(1)
            {
                minimum = r;//o(1)
            }
            if (minimum != i)
            {
                swap(i, minimum); //o(1)
                Min_heap(minimum); //calculate recurance equation
            }

        }

        public static void swap(int i, int minimum) //o(1)
        {
            nodes newnode = new nodes();
            newnode = priority_queue[minimum]; //o(1)
            priority_queue[minimum] = priority_queue[i]; //o(1)
            priority_queue[i] = newnode; //o(1)
        }
        public static nodes heap_extract_min() //o(log v)
        {
           nodes min = Peek();
            nodes root = priority_queue[Count - 1];
            priority_queue.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                Int32 a = i * 2 + 1;
                Int32 b = i * 2 + 2;
                Int32 c = b < Count && priority_queue[b].time < priority_queue[a].time ? b : a;

                if (priority_queue[c].time >= root.time) break;
                priority_queue[i] = priority_queue[c];
                i = c;
            }

            if (Count > 0) priority_queue[i] = root;
            return min;
        }
        public static nodes Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return priority_queue[0];
        }


        public static void heap_dec_key()  //o(log v)
        {


            int index = priority_queue.Count - 1; //o(1)
            while (index > 1 && priority_queue[index / 2].time > priority_queue[index].time) //o(log v )
            {
                swap(index, index / 2); //o(1)
                index = index / 2; //o(1)
            }


        }



        public static void shortestpath(double xstart, double ystart, double xend, double yend, double r, int filenum)
        {


            Stopwatch sw2 = new Stopwatch();
            sw2.Start();

            nodes final = new nodes();  //o(1)

            final.name = list_vertics.Count;
            final.x = xend; //o(1)
            final.y = yend; //o(1)
            final.time = double.MaxValue; //o(1)
            final.distance = double.MaxValue; //o(1)
            final.visited = false;
            final.parent = -4; //o(1)




            nodes start = new nodes(); //o(1)
            start.name = -1; //o(1) 
            start.x = xstart; //o(1)
            start.y = ystart; //o(1)
            start.time = 0; //o(1)
            start.distance = 0; //o(1)
            start.parent = -5; //o(1)


            nodes dummy = new nodes(); //o(1)
            dummy.name = -3; //o(1)
            dummy.x = -1; //o(1)
            dummy.y = -1; //o(1)
            dummy.time = double.MinValue; //o(1)
            dummy.distance = double.MinValue; //o(1)
            dummy.parent = -3; //o(1)

            //priority_queue.Add(dummy); //o(1)

            priority_queue.Add(start); //o(1)

            List<Int32> final_neg = new List<Int32>();
            for (int i = 0; i < list_vertics.Count; i++) //o(v)
            {

                double start_node_dis, end_node_dis;
                double xstart_xnode, xend_xnode;
                double ystart_ynode, yend_ynode;
                //----------------------start-------------------------
                xstart_xnode = start.x - list_vertics[i].x; //o(1)
                ystart_ynode = start.y - list_vertics[i].y; //o(1)
                start_node_dis = Math.Sqrt((xstart_xnode * xstart_xnode) + (ystart_ynode * ystart_ynode)); //o(1)
                if (start_node_dis <= r / 1000)  //o(1)
                {
                    /*------------add start neighbours------------- */
                    info edge = new info();
                    int key_ = -1;
                    int end = list_vertics[i].name;
                    edge.distance = start_node_dis; //o(1)
                    edge.speed = 5; //o(1)
                    edge.time = (edge.distance / edge.speed) * 60; // time minutes //o(1)


                    if (d_edges.ContainsKey(key_) == true) //o(1)
                    {
                        List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                        d_edges.TryGetValue(key_, out old_list); //o(1)
                        old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                        d_edges[key_] = old_list; //o(1)

                    }
                    else
                    {
                        List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                        old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                        d_edges.Add(key_, old_list); //o(1)
                    }

                    /*5lst edaft al node l geran al start*/


                }

                /*end*/


                xend_xnode = final.x - list_vertics[i].x; //o(1)
                yend_ynode = final.y - list_vertics[i].y; //o(1)
                end_node_dis = Math.Sqrt((xend_xnode * xend_xnode) + (yend_ynode * yend_ynode)); //o(1)
                if (end_node_dis <= r / 1000) //o(1)
                {

                    /*hdeef al end tkon geranhom*/

                    info edge = new info();
                    Int32 key_ = list_vertics[i].name; //o(1)
                    final_neg.Add(key_);//o(1)
                    Int32 end = final.name; //o(1)
                    edge.distance = end_node_dis; //o(1)
                    edge.speed = 5; //o(1)
                    edge.time = (edge.distance / edge.speed) * 60; // time minutes //o(1)


                    if (d_edges.ContainsKey(key_) == true) //o(1)
                    {
                        List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                        d_edges.TryGetValue(key_, out old_list); //o(1)
                        old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                        d_edges[key_] = old_list; //o(1)

                    }
                    else //o(1)
                    {
                        List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                        old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                        d_edges.Add(key_, old_list); //o(1)
                    }

                    /*kda dafo al final b2t garthom*/
                }//end if


            }//end loop

            //add final
            list_vertics.Add(final);//o(1) //m7d4 y7tha fo2 al for loop

            Int32 final_count = 0;

            while (priority_queue.Count > 0) //o(e log v)
            {
                nodes extractnode = new nodes();
                extractnode = heap_extract_min(); //o(log v)

                if (extractnode.name == list_vertics.Count - 1) //o(1)
                {
                    break;
                }

                List<KeyValuePair<Int32, info>> neighbours = new List<KeyValuePair<Int32, info>>();


                d_edges.TryGetValue(extractnode.name, out neighbours); //o(1)
                Int32 j;
                /*34an case 67 al drba fe al large 
                     al 3ayz ygrb w y4of by7sl ah y7t breakpoint gowa al if de 3la 7tt al console w ym4i continue*/
                /*
              /* if (extractnode.name == 13722 || extractnode.name == 13717 || extractnode.name == list_vertics.Count-1) 
                {
                        Console.WriteLine("l2ato");
                }
                */
                for (int k = 0; k < neighbours.Count; k++) // num of neighbours of extract node 
                {
                    /*34an case 67 al drba fe al large 
                     al 3ayz ygrb w y4of by7sl ah y7t breakpoint gowa al if de 3la 7tt al console w ym4i continue*/
                    /*
                    if (neighbours[k].Key == list_vertics.Count - 1 || neighbours[k].Key == 13722 || neighbours[k].Key == 13717)
                    {
                        Console.WriteLine("l2ato");
                    }
                    */

                    j = neighbours[k].Key;

                    if (list_vertics[j].time >= neighbours[k].Value.time + extractnode.time)//o(log v)
                    {

                        //update
                        nodes n = new nodes();
                        n.time = neighbours[k].Value.time + extractnode.time; //o(1)
                        n.distance = neighbours[k].Value.distance + extractnode.distance; //o(1)
                        n.parent = extractnode.name; //o(1)
                        n.name = list_vertics[j].name; //o(1)
                        n.x = list_vertics[j].x; //o(1)
                        n.y = list_vertics[j].y; //o(1)
                        list_vertics[j] = n; //o(1)

                        insert_heap(list_vertics[j]); //o(log v)


                    }


                }


            }//end while


            sw2.Stop(); //o(1)
            Console.WriteLine(sw2.ElapsedMilliseconds+"ms");



            double walked = 0; //o(1)



            FileStream finalfile = new FileStream(@"C:\Users\gehad\Desktop\algorizim project virsion1\New folder" + filenum + ".txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(finalfile);


            Stack<Int32> stack_nodes = new Stack<Int32>();

            walked = list_vertics[list_vertics.Count - 1].distance - list_vertics[list_vertics[list_vertics.Count - 1].parent].distance;
            Int32 index = list_vertics.Count - 1;

            while (true)
            {
                if (list_vertics[index].parent != -1)
                {
                    stack_nodes.Push(list_vertics[index].parent);
                    index = list_vertics[index].parent;
                }
                else
                {
                    walked += list_vertics[index].distance;
                    break;
                }


            }
            //Console.WriteLine("the path");
            while (stack_nodes.Count != 0)
            {
                //Console.Write(stack_nodes.Pop()+" => ");
                sw.Write(stack_nodes.Pop() + " ");
                //Console.Write(stack_nodes.Pop() + " ");

            }

            /*-------------------------------------------------------------------------*/
            sw.WriteLine();

            // Console.WriteLine();

            // walked += min_dist; //o(1)
            //Console.WriteLine("start--> node : "+ min_dist);
            /* Console.WriteLine("total time : " + Math.Round(list_vertics[list_vertics.Count - 1].time, 2)); //o(1)
             Console.WriteLine("total distance : " + Math.Round(list_vertics[list_vertics.Count - 1].distance, 2)); //o(1)
             Console.WriteLine("walked distance : " + Math.Round(walked,2)); //o(1)
             Console.WriteLine("car distance : " + Math.Round((list_vertics[list_vertics.Count - 1].distance - walked), 2)); //o(1)



             Console.WriteLine("____________________ ");
             */


            sw.WriteLine(Math.Round(list_vertics[list_vertics.Count - 1].time, 2) + " mins");
            sw.WriteLine(Math.Round(list_vertics[list_vertics.Count - 1].distance, 2) + " km");
            sw.WriteLine(Math.Round(walked, 2) + " km");
            sw.WriteLine(Math.Round((list_vertics[list_vertics.Count - 1].distance - walked), 2) + " km");//car distance

            //sw.WriteLine(sw2.ElapsedMilliseconds + " ms");
            sw.WriteLine();
            sw.Close();



            priority_queue.Clear(); //o(1)
            d_edges.Remove(-1);//ms7t al start
                               /*hms7 final mn kol node*/


            for (int i = 0; i < final_neg.Count; i++)
            {

                List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                d_edges.TryGetValue(final_neg[i], out old_list); //o(1)
                old_list.RemoveAt(old_list.Count - 1);//o(1)

                d_edges[final_neg[i]] = old_list; //o(1)
            }

            final_neg.Clear();

        }

    }
}
