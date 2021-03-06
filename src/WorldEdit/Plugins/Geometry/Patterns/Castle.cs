﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapeGenerator.Generators.Patterns
{
    public class Castle : ITransformer
    {
        public static string Name = "castle";

        public void Transform(List<Point> points)
        {
            var rnd = new Random();
            var lowOrder = points.Min(p => p.Order);
            var hiOrder = points.Max(p => p.Order);
            for (var order = lowOrder; order <= hiOrder; order++)
            {
                var layer = points.Where(p => p.Order == order);
                if (layer == null || !layer.Any()) continue;
                var lx = layer.Min(p => p.X);
                var ly = layer.Min(p => p.Y);
                var lz = layer.Min(p => p.Z);
                var hx = layer.Max(p => p.X);
                var hy = layer.Max(p => p.Y);
                var hz = layer.Max(p => p.Z);
                var dx = hx - lx + 1;
                var dy = hy - ly + 1;
                var dz = hz - lz + 1;
                var matrix = new Point[dx, dy, dz];

                foreach (var p in layer)
                {
                    matrix[p.X - lx, p.Y - ly, p.Z - lz] = p;
                }

                for (var x = 0; x < dx; x++)
                {
                    for (var z = 0; z < dz; z++)
                    {
                        for (var y = 0; y < dy; y++)
                        {
                            if (matrix[x, y, z] == null) continue;
                            if (matrix[x, y, z].BlockName != "castle") continue;
                            matrix[x, y, z].BlockName = "stone 0";

                            var side = x == 0 || z == 0 || x == dx || z == dz;
                            var corner = x == 0 && z == 0 || x == dx && z == dz;

                            var emptyLeft = x > 0 && (matrix[x - 1, y, z] == null ||
                                            matrix[x - 1, y, z].BlockName.StartsWith("air"));
                            var emptyBack = z > 0 && (matrix[x, y, z - 1] == null ||
                                            matrix[x, y, z - 1].BlockName.StartsWith("air"));
                            var emptyRight = x + 1 < dx && (matrix[x + 1, y, z] == null ||
                                             matrix[x + 1, y, z].BlockName.StartsWith("air"));
                            var emptyFront = z + 1 < dz && (matrix[x, y, z + 1] == null ||
                                             matrix[x, y, z + 1].BlockName.StartsWith("air"));

                            corner = corner || ((emptyLeft || emptyRight) && (emptyFront || emptyBack));
                            side = side || corner || emptyLeft || emptyRight || emptyFront || emptyBack;

                            if (corner)
                            {
                                if (y == 0)
                                {
                                    matrix[x, y, z] = matrix[x, y, z].Clone(1);
                                    matrix[x, y, z].BlockName = "stonebrick " + (rnd.NextDouble() < 0.8D ? "0" : "2");
                                    points.Add(matrix[x, y, z]);
                                    continue;
                                }

                                if (matrix[x, y - 1, z].BlockName.StartsWith("stonebrick"))
                                {
                                    matrix[x, y, z] = matrix[x, y, z].Clone(1);
                                    matrix[x, y, z].BlockName = "stone 0";
                                    points.Add(matrix[x, y, z]);
                                    continue;
                                }

                                matrix[x, y, z] = matrix[x, y, z].Clone(1);
                                matrix[x, y, z].BlockName = "stonebrick " + (rnd.NextDouble() < 0.9D ? "0" : "2");
                                points.Add(matrix[x, y, z]);
                                continue;
                            }

                            if (!side) continue;

                            matrix[x, y, z] = matrix[x, y, z].Clone(1);
                            matrix[x, y, z].BlockName = rnd.NextDouble() < 0.6D
                                ? "stone 0"
                                : $"stonebrick {(rnd.NextDouble() < 0.6D ? (rnd.NextDouble() < 0.8D ? "0" : "1") : "2")}";

                            points.Add(matrix[x, y, z]);
                        }
                    }
                }
            }
        }
    }
}