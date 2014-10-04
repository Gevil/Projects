using System;
using System.Collections.Generic;
using BenTools.Mathematics;
using BraveNewWorld.Models;

namespace BraveNewWorld.Services
{
    //do I need this? meh
    public class LoadMapParams
    {
        private IEnumerable<Vector> _points;
        private bool _fix;

        public LoadMapParams(IEnumerable<Vector> points, bool fix = false)
        {
            _points = points;
            _fix = fix;
        }

        public IEnumerable<Vector> Points
        {
            get { return _points; }
        }

        public bool Fix
        {
            get { return _fix; }
        }
    }

    public interface IMapService
    {
        void LoadMap(LoadMapParams loadMapParams);
    }

    public class MapService : IMapService
    {
        public void LoadMap(LoadMapParams loadMapParams)
        {
            VoronoiGraph voronoiMap = null;
            for (int i = 0; i < 3; i++)
            {
                voronoiMap = Fortune.ComputeVoronoiGraph(loadMapParams.Points);
                foreach (Vector vector in loadMapParams.Points)
                {
                    double v0 = 0.0d;
                    double v1 = 0.0d;
                    int say = 0;
                    foreach (VoronoiEdge edge in voronoiMap.Edges)
                    {
                        if (edge.LeftData == vector || edge.RightData == vector)
                        {
                            double p0 = (edge.VVertexA[0] + edge.VVertexB[0]) / 2;
                            double p1 = (edge.VVertexA[1] + edge.VVertexB[1]) / 2;
                            v0 += double.IsNaN(p0) ? 0 : p0;
                            v1 += double.IsNaN(p1) ? 0 : p1;
                            say++;
                        }
                    }

                    if (((v0 / say) < 400) && ((v0 / say) > 0))
                    {
                        vector[0] = v0 / say;
                    }

                    if (((v1 / say) < 400) && ((v1 / say) > 0))
                    {
                        vector[1] = v1 / say;
                    }
                }
            }

            voronoiMap = Fortune.ComputeVoronoiGraph(loadMapParams.Points);
            ImproveMapData(voronoiMap, loadMapParams.Fix);
        }

        private void ImproveMapData(VoronoiGraph voronoiMap, bool fix = false)
        {
            IFactory fact = new MapItemFactory();

            foreach (VoronoiEdge edge in voronoiMap.Edges)
            {
                if (fix)
                {
                    if (!newFix(edge))
                        continue;
                }

                Corner c1 = fact.CornerFactory(edge.VVertexA[0], edge.VVertexA[1]);
                Corner c2 = fact.CornerFactory(edge.VVertexB[0], edge.VVertexB[1]);
                Center cntrLeft = fact.CenterFactory(edge.LeftData[0], edge.LeftData[1]);
                Center cntrRight = fact.CenterFactory(edge.RightData[0], edge.RightData[1]);

                c1.AddAdjacent(c2);
                c2.AddAdjacent(c1);

                cntrRight.AddCorner(c1);
                cntrRight.AddCorner(c2);

                cntrLeft.AddCorner(c1);
                cntrLeft.AddCorner(c2);

                Edge e = fact.EdgeFactory(c1, c2, cntrLeft, cntrRight);


                cntrLeft.AddBorder(e);
                cntrRight.AddBorder(e);

                cntrLeft.AddNeighbour(cntrRight);
                cntrRight.AddNeighbour(cntrLeft);

                c1.AddProtrudes(e);
                c2.AddProtrudes(e);
                c1.AddTouches(cntrLeft);
                c1.AddTouches(cntrRight);
                c2.AddTouches(cntrLeft);
                c2.AddTouches(cntrRight);
            }

            foreach (var c in App.AppMap.Centers)
            {
                c.Value.FixBorders();
                //c.SetEdgeAreas();
                c.Value.OrderCorners();
            }

            //IslandHandler.CreateIsland();
        }

        private bool newFix(VoronoiEdge edge)
        {
            double x1 = edge.VVertexA[0];
            double y1 = edge.VVertexA[1];

            double x2 = edge.VVertexB[0];
            double y2 = edge.VVertexB[1];



            //if both ends are in map, not much to do
            if ((DotInMap(x1, y1) && DotInMap(x2, y2)))
                return true;
            
            //if one end is out of map
            if ((DotInMap(x1, y1) && !DotInMap(x2, y2)) || (!DotInMap(x1, y1) && DotInMap(x2, y2)))
            {
                double b = 0.0d, slope = 0.0d;

                //and that point is actually a number ( not going to infinite ) 
                if (!(double.IsNaN(x2) || double.IsNaN(y2)))
                {
                    slope = ((y2 - y1) / (x2 - x1));

                    b = edge.VVertexA[1] - (slope * edge.VVertexA[0]);

                    // y = ( slope * x ) + b


                    if (edge.VVertexA[0] < 0)
                        edge.VVertexA = new Vector(0, b);

                    if (edge.VVertexA[0] > App.MapSize)
                        edge.VVertexA = new Vector(App.MapSize, (App.MapSize * slope) + b);

                    if (edge.VVertexA[1] < 0)
                        edge.VVertexA = new Vector((-b / slope), 0);

                    if (edge.VVertexA[1] > App.MapSize)
                        edge.VVertexA = new Vector((App.MapSize - b) / slope, App.MapSize);



                    if (edge.VVertexB[0] < 0)
                        edge.VVertexB = new Vector(0, b);

                    if (edge.VVertexB[0] > App.MapSize)
                        edge.VVertexB = new Vector(App.MapSize, (App.MapSize * slope) + b);

                    if (edge.VVertexB[1] < 0)
                        edge.VVertexB = new Vector((-b / slope), 0);

                    if (edge.VVertexB[1] > App.MapSize)
                        edge.VVertexB = new Vector((App.MapSize - b) / slope, App.MapSize);

                }
                else
                {
                    //and if that end is actually not a number ( going go infinite )
                    if (double.IsNaN(x2) || double.IsNaN(y2))
                    {
                        var x3 = (edge.LeftData[0] + edge.RightData[0]) / 2;
                        var y3 = (edge.LeftData[1] + edge.RightData[1]) / 2;

                        slope = ((y3 - y1) / (x3 - x1));

                        slope = Math.Abs(slope);

                        b = edge.VVertexA[1] - (slope * edge.VVertexA[0]);

                        // y = ( slope * x ) + b
                        var i = 0.0d;

                        if(x3 < y3)
                        {
                            if(App.MapSize - x3 > y3)
                            {
                                i = b;
                                if (i > 0 && i < 400)
                                    edge.VVertexB = new BenTools.Mathematics.Vector(0, i);

                            }
                            else
                            {
                                i = (App.MapSize - b) / slope;
                                if (i > 0 && i < 400)
                                    edge.VVertexB = new BenTools.Mathematics.Vector(i, App.MapSize);

                            }
                        }
                        else
                        {
                            if (App.MapSize - x3 > y3)
                            {
                                i = (-b / slope);
                                if (i > 0 && i < 400)
                                    edge.VVertexB = new BenTools.Mathematics.Vector(i, 0);
                            }
                            else
                            {
                                i = (App.MapSize * slope) + b;
                                if (i > 0 && i < 400)
                                    edge.VVertexB = new BenTools.Mathematics.Vector(App.MapSize, i);

                            }
                        }

                        //if (x3 < App.MapSize / 4)
                        //{
                        //    i = b;
                        //    if (i > 0 && i < 400)
                        //        edge.VVertexB = new BenTools.Mathematics.Vector(0, i);

                        //    //left
                        //}

                        //if (x3 > App.MapSize * 3 / 4)
                        //{
                        //    i = (App.MapSize * slope) + b;
                        //    if (i > 0 && i < 400)
                        //        edge.VVertexB = new BenTools.Mathematics.Vector(App.MapSize, i);

                        //    //right
                        //}

                        //if (y3 > App.MapSize * 3 / 4)
                        //{
                        //    i = (App.MapSize - b) / slope;
                        //    if (i > 0 && i < 400)
                        //        edge.VVertexB = new BenTools.Mathematics.Vector(i, App.MapSize);

                        //    //bottom
                        //}

                        //if (y3 < App.MapSize / 4)
                        //{
                        //    i = (-b / slope);
                        //    if (i > 0 && i < 400)
                        //        edge.VVertexB = new BenTools.Mathematics.Vector(i, 0);

                        //    //top
                        //}

                        
                    }
                }
                return true;
            }
            return false;
        }

        private bool DotInMap(double x, double y)
        {
            return (x > 0 && x < 400) && (y > 0 && y < 400);
        }

    }
}
