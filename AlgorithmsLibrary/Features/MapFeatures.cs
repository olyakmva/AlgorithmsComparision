using System;

namespace AlgorithmsLibrary.Features
{
/*1. Длину
2. Длину базовой линии
3. Высоту
4. Площадь
5. Отношение длины и базовой линии(извилистость)
6. Отношение высоты и базовой линии(пропорция)
7. Отношение периметра и площади(компактность). 
Периметр получается сложением длины и базовой линии изгиба/тройки.

Для данных характеристик нужно получить средние значения(14 шт: 7 для изгибов
и 7 для троек), а для площади — еще и сумму(2 шт — по 1 для изгибов и троек). */

    public class MapFeatures
    {
        public double TotalArea { get; set; }
        public double ShapeProminence { get; set; }
        public BaseFeatures Average { get; set; }
        public BaseFeatures Max { get; set; }
        public BaseFeatures Min { get; set; }
        public BaseFeatures Sko { get; set; }

        public MapFeatures()
        {
            Average = new BaseFeatures();
            Max = new BaseFeatures();
            Min = new BaseFeatures();
            Sko = new BaseFeatures();
        }

        public override string ToString()
        {
            string s = string.Format("{0};{1};{2}{3}{4}{5}",
                 TotalArea, ShapeProminence, Average, Max, Min, Sko);
            return s;
        }
        public static string GetDescription(string prefix)
        {
            string s = string.Format("{0}TotalArea;{0}ShapeProminence;{1}{2}{3}{4}", prefix, BaseFeatures.GetDescription(prefix+"Average"),
                BaseFeatures.GetDescription(prefix+"Max"), BaseFeatures.GetDescription(prefix+"Min"),
                BaseFeatures.GetDescription(prefix+"Sko"));
            return s;
        }
    }

    public class BaseFeatures
    {
        public double Length { get; set; }
        public double BaseLineLength { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Area { get; set; }
        public double Sinuosity { get; set; }
        public double HeightBaselineRatio { get; set; }
        public double HeightWidthRatio { get; set; }
        public double Compactness { get; set; }
        

        public override string ToString()
        {
            string s = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};",
                Length, BaseLineLength, Height, Width, Area, Sinuosity, 
                HeightBaselineRatio, HeightWidthRatio, Compactness);
            return s;
        }

        public static string GetDescription(string prefix)
        {
            string s = prefix+ "Length;"+prefix+"BaseLineLength;"
                +prefix+"Height;"+prefix+"Width;"+prefix+"Area;"+
                prefix +"Sinuosity;"+prefix+"HeightBaselineRatio;"+
                prefix +"HeightWidthRatio;"+prefix +"Compactness;";
            return s;
        }
    }

    public static class Compactness
    {
        public static double Get(double area, double perimetr)
        {
            var c = 4 * Math.PI * area / (perimetr * perimetr);
            return c;
        }
    }

}
