using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ConcurrencyVisualizer.Instrumentation;
using Microsoft.ConcurrencyVisualizer;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Threading;
using System.Data.Odbc;
using System.Xml;
using Lenf;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Sockets;
using System.Numerics;

namespace LenfNum {
    #region LenfNum
    public class LenfNum {
        #region Attribute
        private static Random r = new Random();
        private static double positiveLimit = 1e-8;
        private static double negitiveLimit = -1e-8;
        public short row, column;
        private double[][] _value;
        #endregion

        #region Setter Getter
        public double this[short r, short c] {
            get {
                return _value[r][c];
            }
            set {
                _value[r][c] = value < 0 ? (value < negitiveLimit ? value : negitiveLimit) : (value > positiveLimit ? value : positiveLimit);
            }
        }
        public double[] this[short r] {
            get {
                return _value[r];
            }
            set {
                var lenght = value.Length;
                _value[r] = new double[lenght];
                for(short i = 0; i < lenght; i++) {
                    this[r, i] = value[i];
                }
            }
        }
        #endregion

        #region Constructor
        public LenfNum(short _row, short _column, double OriginValue) {
            row = _row;
            column = _column;
            _value = new double[row][];
            for(short i = 0; i < row; i++) {
                this[i] = Enumerable.Repeat(OriginValue, column).ToArray();
            }
        }
        public LenfNum(short _row, short _column) {
            row = _row;
            column = _column;
            _value = new double[row][];
            for(short i = 0; i < row; i++) {
                this[i] = Enumerable.Repeat(0d, column).ToArray();
                for(short j = 0; j < column; j++) {
                    this[i, j] = (r.NextDouble() - 0.5) / 5d;
                }
            }
        }
        public LenfNum(double[] __value) {
            row = 1;
            column = (short)__value.Count();
            _value = Enumerable.Repeat(__value, 1).ToArray();
        }
        public LenfNum(double[][] __value) {
            row = (short)__value.Count();
            column = (short)__value.First().Count();
            _value = __value;
        }
        public LenfNum(short OneHotposition) {
            var a = Enumerable.Repeat(0d, 10).ToArray();
            a[OneHotposition] = 1;
            _value = Enumerable.Repeat(a, 1).ToArray();
        }
        #endregion

        #region Operator
        public static LenfNum operator +(LenfNum augend, LenfNum addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] + addend[i, j];
                }
            }
            return d;
        }
        public static LenfNum operator +(LenfNum augend, double addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] + addend;
                }
            }
            return d;
        }
        public static LenfNum operator -(LenfNum augend, LenfNum addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] - addend[i, j];
                }
            }
            return d;
        }
        public static LenfNum operator -(LenfNum augend, double addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] - addend;
                }
            }
            return d;
        }
        public static LenfNum operator *(LenfNum augend, LenfNum addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] * addend[i, j];
                }
            }
            return d;
        }
        public static LenfNum operator *(LenfNum augend, double addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] * addend;
                }
            }
            return d;
        }
        public static LenfNum operator /(LenfNum augend, LenfNum addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i][j] = augend[i, j] / addend[i, j];
                }
            }
            return d;
        }
        public static LenfNum operator /(LenfNum augend, double addend) {
            short row = augend.row;
            short column = augend.column;
            var d = new LenfNum(row, column);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = augend[i, j] / addend;
                }
            }
            return d;
        }
        #endregion

        #region Overriding
        public override string ToString() {
            return String.Join("\r\n", _value.Select(x => String.Join(", ", x.Select(y => y))));
        }
        #endregion

        #region NormalFunction
        public int Count() {
            return row * column;
        }
        public double Sum() {
            var sum = 0d;
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    sum += this[i, j];
                }
            }
            return sum;
        }
        public double Min() {
            var m = double.PositiveInfinity;
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    m = this[i, j] < m ? this[i, j] : m;
                }
            }
            return m;
        }
        public double Max() {
            var m = double.NegativeInfinity;
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    m = this[i, j] > m ? this[i, j] : m;
                }
            }
            return m;
        }
        public LenfNum FuncToAll(Func<double, double> func) {
            var d = new LenfNum(row, column, 0d);
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    d[i, j] = func(this[i, j]);
                }
            }
            return d;
        }
        public LenfNum FuncForAll(Func<double, double> func) {
            for(short i = 0; i < row; i++) {
                for(short j = 0; j < column; j++) {
                    this[i, j] = func(this[i, j]);
                }
            }
            return this;
        }
        public List<List<double>> Alllist() {
            return _value.Select(x => x.Select(y => y).ToList()).ToList();
        }
        #endregion

        #region Matrix
        public LenfNum Transposed() {
            var sql = new LenfNum(column, row, 0d);
            for(short i = 0; i < column; i++) {
                for(short j = 0; j < row; j++) {
                    sql[i, j] = this[j, i];
                }
            }
            return sql;
        }
        public LenfNum Multiply(LenfNum Multier) {
            if(Multier == null)
                return this;
            if(column != Multier.row)
                throw new Exception("Can't multiply AAAAA");

            var c = row;
            var d = Multier.column;
            var e = column;
            var f = new LenfNum(c, d, 0d);

            for(short i = 0; i < c; i++) {
                for(short j = 0; j < d; j++) {
                    for(short k = 0; k < e; k++) {
                        f[i, j] += this[i, k] * Multier[k, j];
                    }
                }
            }

            return f;
        }
        #endregion
    }
    #endregion
    #region LenfLayer
    public abstract class LenfLayer {
        public short Get, Give;
        public double LearningRate;
        public LenfNum Input, Partial;

        public LenfLayer(short get, short give, double learningRate) {
            Get = get;
            Give = give;
            LearningRate = -learningRate;
        }

        public abstract LenfNum CalculateOutput(LenfNum input);
        public abstract LenfNum BackPropagation(LenfNum lastLayer);
        public abstract void GradientDescent();
    }
    #endregion
    #region InputLayer
    public class LenfInterLayer : LenfLayer {
        public LenfInterLayer(short get, short give, double learningRate) : base(get, give, learningRate) {

        }

        public override LenfNum BackPropagation(LenfNum lastLayer) {
            throw new NotImplementedException();
        }

        public override LenfNum CalculateOutput(LenfNum input) {
            throw new NotImplementedException();
        }

        public override void GradientDescent() {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region DenseLayer
    public class LenfDenseLayer : LenfLayer {
        public LenfNum Weight, Bias, BeforeActive, AfterActive;
        public Func<LenfDenseLayer, LenfNum>[] ActivityFunction = LenfNetworkFunction.ActivityFunction.Softmax;
        public LenfDenseLayer(short get, short give, double learningRate = 0.1, Func<LenfLayer, LenfNum>[] activityFunction = null) : base(get, give, learningRate) {
            ActivityFunction = activityFunction ?? ActivityFunction;
            Weight = new LenfNum(get, give);
            Bias = new LenfNum(1, give, 0);
        }

        public override LenfNum CalculateOutput(LenfNum input) {
            Input = input;
            BeforeActive = input.Multiply(Weight) + Bias;
            AfterActive = ActivityFunction[0](this);
            return AfterActive;
        }
        public override LenfNum BackPropagation(LenfNum lastLayer) {
            Partial = lastLayer * ActivityFunction[1](this);
            return Partial.Multiply(Weight.Transposed());
        }
        public override void GradientDescent() {
            Weight += Input.Transposed().Multiply(Partial) * LearningRate;
            Bias += Partial * LearningRate / 10;
        }
    }
    #endregion
    #region NormalizeLayer
    //Warning
    public class LenfNormalizeLayer : LenfLayer {
        public double Weight = 1, Bias = 0;

        public LenfNum BeforeNormal, AfterNormal;
        public Func<LenfNormalizeLayer, LenfNum>[] NormalizeFunction = LenfNetworkFunction.NormalizeFunction.BatchNormalize;

        public LenfNormalizeLayer(short get, short give, double learningRate = 0.5, Func<LenfLayer, LenfNum>[] normalizeFunction = null) : base(get, give, learningRate) {
            NormalizeFunction = normalizeFunction ?? NormalizeFunction;
        }

        public override LenfNum CalculateOutput(LenfNum input) {
            BeforeNormal = input;
            AfterNormal = NormalizeFunction[0](this);
            return AfterNormal * Weight + Bias;
        }
        public override LenfNum BackPropagation(LenfNum lastLayer) {
            Partial = lastLayer;
            return NormalizeFunction[1](this) * Weight;
        }
        public override void GradientDescent() {
            Weight += (Partial * AfterNormal * LearningRate).Sum();
            Bias += (Partial * LearningRate).Sum();
        }
    }
    #endregion
    #region ConvolutionLayer
    public class LenfConvolutionLayer : LenfLayer {
        public short Height, Width, FilterLenght;

        public LenfNum[] filter;
        public LenfNum[] result;

        public LenfConvolutionLayer(short get, short give, short height, short width, short filterLenght, double learningRate = 0.5) : base(get, give, learningRate) {
            Height = height;
            Width = width;
            FilterLenght = filterLenght;
            filter = new LenfNum[] {
                #region CustomFilter
                //// right
                //new LenfNum(new double[][] {new double[] { 1, -1, -1, -1, -1 }, new double[] { 1, -1, -1, -1, -1 }, new double[] { 1, -1, -1, -1, -1 }, new double[] { 1, -1, -1, -1, -1 }, new double[] { 1, -1, -1, -1, -1 } }),
                //// left 
                //new LenfNum(new double[][] { new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 } }),
                //// bottom
                //new LenfNum(new double[][] { new double[] { 1, 1, 1, 1, 1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 } }),
                //// top
                //new LenfNum(new double[][] { new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] {-1, -1, -1, -1, -1 }, new double[] { 1, 1, 1, 1, 1 } }),
                //// left up
                //new LenfNum(new double[][] { new double[] {1, 1, 1, 1, 1 }, new double[] {1, -1, -1, -1, -1 }, new double[] {1, -1, -1, -1, -1 }, new double[] {1, -1, -1, -1, -1 }, new double[] { 1, -1, -1, -1, -1 } }),
                //// left down
                //new LenfNum(new double[][] { new double[] {1, -1, -1, -1, -1 }, new double[] {1, -1, -1, -1, -1 }, new double[] {1, -1, -1, -1, -1 }, new double[] {1, -1, -1, -1, -1 }, new double[] { 1, 1, 1, 1, 1 } }),
                //// right up
                //new LenfNum(new double[][] { new double[] {1, 1, 1, 1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] { -1, -1, -1, -1, 1 } }),
                //// right down
                //new LenfNum(new double[][] { new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] {-1, -1, -1, -1, 1 }, new double[] { 1, 1, 1, 1, 1 } }),
                //// cross
                //new LenfNum(new double[][] { new double[] {-1, -1, 1, -1, -1 }, new double[] {-1, -1, 1, -1, -1 }, new double[] { 1, 1, 1, 1, 1 }, new double[] {-1, -1, 1, -1, -1 }, new double[] {-1, -1, 1, -1, -1 } }),
                //// X
                //new LenfNum(new double[][] { new double[] { 1, -1, -1, -1, 1 }, new double[] { -1, 1, -1, 1, -1 }, new double[] { -1, -1, 1, -1, -1 }, new double[] { -1, 1, -1, 1, -1 }, new double[] { 1, -1, -1, -1, 1 } }),
                //// O
                //new LenfNum(new double[][] { new double[] {2, -1, -1, -1,2 }, new double[] { -1, -1, -1, -1, -1 }, new double[] { -1, -1, -1, -1, -1 }, new double[] { -1, -1, -1, -1, -1 }, new double[] {2, -1, -1, -1,2 } })
                #endregion
            };
            result = new LenfNum[filter.Length];
        }
        public LenfNum[] test(LenfNum input) {
            short height = (short)(Height - FilterLenght + 1);
            short width = (short)(Width - FilterLenght + 1);

            for(short count = 0; count < filter.Length; count++) {

                for(short i = 0; i < height; i++) {
                    for(short j = 0; j < width; j++) {
                        double r = 0;
                        for(short k = 0; k < 5; k++) {
                            for(short l = 0; l < 5; l++) {
                                r += input[0, (short)((i + k) * Width + (j + l))] * filter[count][k, l];
                            }
                        }

                    }
                }
            }
            return result;
        }
        public override LenfNum BackPropagation(LenfNum lastLayer) {
            throw new NotImplementedException();
        }
        public override LenfNum CalculateOutput(LenfNum input) {
            short lenght = 28 - 5 + 1;
            for(short count = 0; count < filter.Length; count++) {
                result[count] = new LenfNum((short)(lenght + 1), (short)(lenght + 1), 0);
                for(short i = 0; i < lenght; i++) {
                    for(short j = 0; j < lenght; j++) {
                        double r = 0;
                        for(short k = 0; k < 5; k++) {
                            for(short l = 0; l < 5; l++) {
                                r += input[0, (short)((i + k) * 28 + (j + l))] * filter[count][k, l];
                            }
                        }
                        result[count][i, j] = r;
                    }
                }
            }
            throw new NotImplementedException();
            //return result;
        }
        public override void GradientDescent() {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Network
    public class LenfNetwork {
        public List<LenfLayer> Layers = new List<LenfLayer>();

        public Func<LenfNum, LenfNum, LenfNum>[] CostFunction = LenfNetworkFunction.CostFunction.CrossEntropy;
        public LenfNetwork(List<LenfLayer> layers, Func<LenfNum, LenfNum, LenfNum>[] costFunction = null) {
            CostFunction = costFunction ?? CostFunction;

            if(!layers.Select(x => x.Get).Skip(1).Zip(layers.Select(x => x.Give).Take(layers.Count - 1), (x, y) => x == y).All(x => x))
                throw new Exception("Layer should be concat(?");

            Layers.AddRange(layers);
        }
        #region FunctionAddLayer
        //public void AddLayer(LenfLayer layer, short? firstInputCount = null) {
        //    layer.Input = Layers.Count == 0 ? new LenfNum(1, firstInputCount ?? throw new Exception("First Layer should have input size")) : new LenfNum(Layers.Last().give,layer.get);
        //    Layers.Add(layer);
        //}
        //public void AddRangeLayers(List<LenfLayer> layers, short? firstInputCount = null) {
        //    foreach(var layer in layers) {
        //        AddLayer(layer, firstInputCount);
        //    }
        //}
        #endregion
        public LenfNum Calculate(LenfNum CheckData) {
            var finalOutput = GetFinalOutput(CheckData);
            var totalCost = CostFunction[0](finalOutput * -1, CheckData).Sum();
            return finalOutput;
        }
        public double Train(LenfNum TrainData, LenfNum CheckData) {
            var finalOutput = GetFinalOutput(TrainData) * -1;
            var totalCost = -CostFunction[0](finalOutput * -1, CheckData).Sum();
            BackPropagation(CostFunction[1](finalOutput, CheckData));
            return totalCost;
        }
        public LenfNum GetFinalOutput(LenfNum TrainData) {
            foreach(var layer in Layers) {
                TrainData = layer.CalculateOutput(TrainData);
            }
            return TrainData;
        }
        public void BackPropagation(LenfNum CostData) {
            foreach(var layer in Layers.ToArray().Reverse()) {
                CostData = layer.BackPropagation(CostData);
            }
            foreach(var layer in Layers) {
                layer.GradientDescent();
            }
        }
    }
    #endregion
    #region Function
    public static class LenfNetworkFunction {
        public static class ActivityFunction {
            public static Func<LenfDenseLayer, LenfNum>[] Sigmoid = new Func<LenfDenseLayer, LenfNum>[] {
                (x) => {
                    return x.BeforeActive.FuncToAll(x => 1 / (1 + Math.Exp(-x)));
                },
                (x) => {
                    return x.AfterActive.FuncToAll(x => x * (1 - x));
                }
            };

            public static Func<LenfDenseLayer, LenfNum>[] Softmax = new Func<LenfDenseLayer, LenfNum>[] {
                (x) => {
                    var total = x.BeforeActive.FuncToAll(x => Math.Exp(x)).Sum();
                    return x.BeforeActive.FuncToAll(x => Math.Exp(x) / total);
                },
                (x) => {
                    return x.AfterActive.FuncToAll(x => x - x * x);
                }
            };

            public static Func<LenfDenseLayer, LenfNum>[] ReLU = new Func<LenfDenseLayer, LenfNum>[] {
                (x) => {
                    return x.BeforeActive.FuncToAll(x => x > 0 ? x : -0.8 * x);
                },
                (x) => {
                    return x.AfterActive.FuncToAll(x => x > 0 ? 1 : -0.8);
                }
            };
        }
        public static class NormalizeFunction {
            public static Func<LenfNormalizeLayer, LenfNum>[] MinMaxNormalize = new Func<LenfNormalizeLayer, LenfNum>[] {
                (x) => {
                    var max = x.BeforeNormal.Max();
                    return x.BeforeNormal.FuncToAll(y => y / max);
                },
                (x) => {
                    var max = x.BeforeNormal.Max();
                    return x.BeforeNormal.FuncToAll(y => 1 / max);
                }
            };
            public static Func<LenfNormalizeLayer, LenfNum>[] BatchNormalize = new Func<LenfNormalizeLayer, LenfNum>[] {
                (x) => {
                    var mean = x.BeforeNormal.Sum() / x.BeforeNormal.Count();
                    var variance = Math.Sqrt(x.BeforeNormal.FuncToAll(x=>Math.Pow(x-mean,2)).Sum() / x.BeforeNormal.Count());
                    return x.BeforeNormal.FuncToAll(x => (x - mean) / (variance + 1e-8));
                },
                (x) => {
                    var mean = x.BeforeNormal.Sum() / x.BeforeNormal.Count();
                    var variance = Math.Sqrt(x.BeforeNormal.FuncToAll(x=>Math.Pow(x-mean,2)).Sum() / x.BeforeNormal.Count());
                    return (x.AfterNormal * x.AfterNormal + 1 - x.BeforeNormal.Count()) / x.BeforeNormal.Count() / -variance;
                }
            };
        }
        public static class CostFunction {
            public static Func<LenfNum, LenfNum, LenfNum>[] CrossEntropy = new Func<LenfNum, LenfNum, LenfNum>[] {
                (x, y) => {
                    var row = x.row;
                    var column = x.column;
                    var d = new LenfNum(row,column);
                    for(short i = 0; i < row; i++) {
                        for(short j = 0; j < column; j++) {
                            d[i][j] = Math.Log( y[i][j] == 0 ? ( 1 - x[i][j] ) : x[i][j] );
                        }
                    }
                    return d;
                },
                (x, y) => {
                    var row = x.row;
                    var column = x.column;
                    var d = new LenfNum(row,column);
                    for(short i = 0; i < row; i++) {
                        for(short j = 0; j < column; j++) {
                            d[i][j] = 1 / ( y[i][j] == 0 ? ( 1 - x[i][j] ) : x[i][j] );
                        }
                    }
                    return d;
                }
            };
        }
    }
    #endregion
}