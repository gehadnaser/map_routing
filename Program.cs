using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{

    public struct info
    {
        public double distance;
        public double speed;
        public double time;

    };
    public struct query
    {
        public double xstart;
        public double ystart;
        public double xend;
        public double yend;
        public double radius;

    };

    public class getfiles
    {
        public List<string> GetAllFiles(string sDirt)
        {
            List<string> files = new List<string>();

            try
            {
                foreach (string file in Directory.GetFiles(sDirt))
                {
                    files.Add(file);
                }
                foreach (string fl in Directory.GetDirectories(sDirt))
                {
                    files.AddRange(GetAllFiles(fl));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }



            return files;
        }
    }

    class Program
    {
        public static List<nodes> prority_queue = new List<nodes>();


        // public static List<nodes> list_vertix = new List<nodes>();


        public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> dic_edges = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
        public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> dic_temp = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
        public static List<query> list_query = new List<query>();
        public static List<string> files = new List<string>();

        static void Main(string[] args)
        {

            readfolder(@"D:\computer science fuclty\subjects\fuclty projects\algorizm project\algorizim\[2] Medium Cases");



            int size_list = files.Count / 3;
            for (int i = 0; i < size_list; i++) //exact n , n:num of files
            {
                dic_edges.Clear(); //o(1)
                list_query.Clear();  //o(1)
                                     //list_vertix.Clear();  //o(1)

                nodes[] vertix = readfile(files[i], files[i + (2 * size_list)], files[i + size_list]);

                dic_temp.Clear(); //o(1)
                foreach (KeyValuePair<Int32, List<KeyValuePair<Int32, info>>> node in dic_edges) //o(e) , e = num of edges
                {
                    Int32 key = node.Key;
                    List<KeyValuePair<Int32, info>> lis = new List<KeyValuePair<Int32, info>>();
                    for (int c = 0; c < node.Value.Count; c++)
                    {
                        Int32 end_ = node.Value[c].Key;  //o(1)
                        info temp = new info();  //o(1)
                        temp.distance = node.Value[c].Value.distance; //o(1)
                        temp.speed = node.Value[c].Value.speed; //o(1)
                        temp.time = node.Value[c].Value.time; //o(1)


                        lis.Add(new KeyValuePair<Int32, info>(end_, temp)); //o(1)
                    }
                    dic_temp.Add(key, lis); //o(1)
                }


                priorityqueue.getdata(prority_queue, dic_edges, list_query);  //o(1)

                Stopwatch sw1 = new Stopwatch();
                sw1.Start();

                for (int j = 0; j < list_query.Count; j++)
                {

                    prority_queue.Clear();  //kda kda hy3ml over write 3lah

                    for (Int32 k = 0; k < vertix.Count(); k++) //o(v) , v: num of vertix
                    {
                        nodes p = new nodes();//o(1)
                        p.name = vertix[k].name;//o(1)
                        p.parent = vertix[k].parent;//o(1)
                        p.x = vertix[k].x;//o(1)
                        p.y = vertix[k].y;//o(1)
                        p.time = vertix[k].time;//o(1)
                        p.distance = vertix[k].distance;//o(1)
                        p.visited = vertix[k].visited;//o(1)
                        prority_queue.Add(p);//o(1)
                        //prority_queue[list_vertix[k].name] = p;
                    }


                    priorityqueue.getdata(prority_queue, dic_temp, list_query); //o(1)

                    priorityqueue.shortestpath(list_query[j].xstart, list_query[j].ystart, list_query[j].xend, list_query[j].yend, list_query[j].radius, i + 1);


                }

                sw1.Stop(); //o(1)
                Console.WriteLine(sw1.ElapsedMilliseconds);

            }


        }
        public static void readfolder(string foldername)
        {
            getfiles get = new getfiles();
            files = get.GetAllFiles(foldername);

            int size_list = files.Count / 3;


        }

        public static nodes[] readfile(string map, string query, string output)
        {

            FileStream file = new FileStream(@map, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);


            int vertices;

            //int wronganswers = 0;
            int edges;

            string line = sr.ReadLine();
            vertices = int.Parse(line);

            nodes[] list_vertix = new nodes[vertices];

            //prority_queue.Add(dummy);

            for (int i = 0; i < vertices; i++)
            {
                line = sr.ReadLine();
                string[] lineparts = line.Split(' ');
                nodes n = new nodes();
                n.name = Int32.Parse(lineparts[0]);
                n.distance = double.MaxValue;
                n.time = double.MaxValue;
                n.parent = -4;
                n.visited = false;
                n.x = double.Parse(lineparts[1]);
                n.y = double.Parse(lineparts[2]);
                //prority_queue.Add(n);
                //list_vertix.Add(n);t3del


                list_vertix[n.name] = n;

            }


            //dictionary of edges


            line = sr.ReadLine();
            edges = int.Parse(line);


            for (int i = 0; i < edges; i++)//e
            {
                line = sr.ReadLine();
                string[] lineparts = line.Split(' ');

                info edge = new info();

                Int32 key_ = Int32.Parse(lineparts[0]);
                Int32 end = Int32.Parse(lineparts[1]);
                edge.distance = double.Parse(lineparts[2]);
                edge.speed = double.Parse(lineparts[3]);
                edge.time = (edge.distance / edge.speed) * 60; // time minutes


                if (dic_edges.ContainsKey(key_) == true)
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                    dic_edges.TryGetValue(key_, out old_list); // return bool (true or false) (true return value)
                    old_list.Add(new KeyValuePair<Int32, info>(end, edge));
                    dic_edges[key_] = old_list;

                }
                else
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                    old_list.Add(new KeyValuePair<Int32, info>(end, edge));
                    dic_edges.Add(key_, old_list);
                }

                //______________________//
                // bnzbt mn al end ll start

                if (dic_edges.ContainsKey(end) == true)
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                    dic_edges.TryGetValue(end, out old_list); // return bool (true or false) (true return value)
                    old_list.Add(new KeyValuePair<Int32, info>(key_, edge));
                    dic_edges[end] = old_list;
                }
                else
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                    old_list.Add(new KeyValuePair<Int32, info>(key_, edge));
                    dic_edges.Add(end, old_list);
                }


            }

            sr.Close();
            //_____________________________________________________________//

            FileStream file1 = new FileStream(@query, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(file1);

            int num_queries;

            line = sr1.ReadLine();
            num_queries = int.Parse(line);



            for (int i = 0; i < num_queries; i++)
            {
                line = sr1.ReadLine();
                string[] lineparts = line.Split(' ');
                query quer = new query();

                quer.xstart = double.Parse(lineparts[0]);
                quer.ystart = double.Parse(lineparts[1]);
                quer.xend = double.Parse(lineparts[2]);
                quer.yend = double.Parse(lineparts[3]);
                quer.radius = double.Parse(lineparts[4]);


                list_query.Add(quer);

            }
            sr1.Close();
            return list_vertix;//t3deel

        }


    }
}
