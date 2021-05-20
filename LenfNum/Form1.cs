using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ConcurrencyVisualizer.Instrumentation;

namespace LenfNum {
    public partial class Form1 : Form {
        public static Form1 form1;
        public Form1() {
            InitializeComponent();
            form1 = this;
            //Stopwatch stopwatch = new Stopwatch();
            //LenfLayer l = new LenfLayer(784, 100);
            //LenfNum l1 = new LenfNum(1, 784);

            //stopwatch.Restart();

            //var mm = new LenfNum(l.row, l.column, 0d);
            //for(int i = 0; i < l.row; i++) {
            //    for(int j = 0; j < l.column; j++) {
            //        mm.value[i][j] = Math.Log(l1.value[i][j] == 0 ? 1 - l.value[i][j] : l.value[i][j]);
            //    }
            //}

            //stopwatch.Stop();
            //var time = stopwatch.Elapsed.TotalMilliseconds;


            //l.CalculateOutput(l1);
            //stopwatch.Restart();

            //var sql = LenfNetworkFunction.NormalizeFunction.BatchNormalize[0](l);

            //stopwatch.Stop();
            //var time1 = stopwatch.Elapsed.TotalMilliseconds;
            //int z = 0;




            var sql = File.ReadAllLines("../../../mnist_test.csv").Skip(1).Select(x => x.Split(',')).ToArray();
            var TestAns = sql.Select(x => new LenfNum(short.Parse(x.FirstOrDefault()))).ToArray();
            var TestData = sql.Select(x => new LenfNum(x.Skip(1).Select(x => (int.Parse(x) / 255d)).ToArray())).ToArray();


            LenfConvolutionLayer layer = new LenfConvolutionLayer(5, 5);
            Random r = new Random();
            int index = r.Next(0, 40);
            var a = layer.test(TestData[index]);
            var d = TestAns[index];
            var b = a.Select(x => x.FuncToAll(y => y > 0 ? y : 0).Sum());
            var c = b;

            //List<LenfLayer> layers = new List<LenfLayer>();
            //layers.Add(new LenfDenseLayer(784, 10));
            //LenfNetwork network = new LenfNetwork(layers);

            //var count = 5000;
            //var num = 0;

            //var pack = TestData.Take(count).Zip(TestAns.Take(count), (x, y) => new { x, y });
            //foreach(var t in pack) {
            //    for(int j = 0; j < 1; j++) {
            //        network.Train(t.x, t.y);
            //    }
            //}

            //var a = network.Calculate(TestData[0]);
            //var b = network.Calculate(TestData[1]);
            //int z = 0;

            //var data = TestData.Take(count).Zip(TestAns.Take(count), (x, y) => new { x, y }).OrderBy(x => Guid.NewGuid()).ToArray();

            TrainBtn.Click += (x, y) => {
                ////var SetCount = int.Parse(textBox1.Text);
                //var SetCount = 1;
                //label1.Text = data[num].y.Alllist().First().IndexOf(1) + " ";
                //for(int i = 0; i < SetCount; i++) {
                //    label1.Text += network.Train(data[num].x, data[num].y);
                //    //foreach(var t in TestData.Skip(i).Take(1).Zip(TestAns.Skip(i).Take(1), (x, y) => new { x, y })) {
                //    //    for(int j = 0; j < 1; j++) {
                //    //        network.Train(t.x, t.y);
                //    //    }
                //    //}
                //}
                //Invalidate();
                ////var a = network.Calculate(TestData[5]);
                ////var b = network.Calculate(TestData[18]);
                //num++;
            };

            Paint += (_x, _y) => {
                var weight = a;
                var min = weight.Select(x => x.Min()).Min() / 2;
                var max = weight.Select(x => x.Max()).Max() / 2;
                var x = 10;
                var y = 10;
                for(int i = 0; i < weight.Count(); i++) {
                    for(short j = 0; j < 25; j++) {
                        for(short z = 0; z < 25; z++) {
                            var gray = (int)((weight[i][j, z] / 2 - min) / (max - min) * 255);
                            _y.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(gray, gray, gray)), x + (i % 15) * 30 + z, y + (i / 15) * 30 + j, 1, 1);
                        }
                    }
                }



                //weight = data[num - 1 == -1 ? 0 : (num - 1)].x.Alllist();
                //min = weight.SelectMany(x => x.Select(y => y)).Min() / 2;
                //max = weight.SelectMany(x => x.Select(y => y)).Max() / 2;
                //for(int i = 0; i < weight[0].Count; i++) {
                //    var gray = (int)((weight[0][i] / 2 - min) / (max - min) * 255);
                //    _y.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(gray, gray, gray)), x + i % 28, 50 + i / 28, 1, 1);
                //}


                //try {
                //    //var partial = network.Layers[0].Partial?.Alllist() ?? new List<List<double>>();
                //    //var partialmin = partial?.SelectMany(x => x.Select(y => y)).Min() ?? 0;
                //    //for(int i = 0; i < partial.Count; i++) {
                //    //    for(int j = 0; j < partial[i].Count; j++) {
                //    //        var gray = (int)((partial[i][j] - partialmin) * 255);
                //    //        _y.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(gray, gray, gray)), x + i * 50 + j % 28, y + 20 + j / 28, 1, 1);
                //    //    }
                //    //}
                //} catch {

                //}
            };

        }

        public static Func<LenfNum, LenfNum>[] Softmax = new Func<LenfNum, LenfNum>[] {
            (x) => {
                var total = x.FuncToAll(x => Math.Exp(x)).Sum();
                return x.FuncToAll(x => Math.Exp(x) / total);
            },
            (x) => {
                return x.FuncToAll(x => x - x * x);
            }
        };
    }
}
