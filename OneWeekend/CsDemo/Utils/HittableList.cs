using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Utils
{
    internal class HittableList : IHittable
    {
        public List<IHittable> Objects { get; }

        public HittableList()
        {
            this.Objects = new List<IHittable>();
        }

        public HittableList(IEnumerable<IHittable> objects)
        {
            this.Objects = objects.ToList();
        }

        public bool Hit(Ray ray, float min, float max, ref HitRecord hitRecord)
        {
            var hitAnything = false;
            var closestSoFar = max;     // t的最大值
            var tempRecord = new HitRecord();
            hitRecord = default;

            foreach (var @object in Objects)
            {
                if (@object.Hit(ray, min, closestSoFar, ref tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.DirectionFactor;
                    hitRecord = tempRecord;
                }
            }

            return hitAnything;
        }
    }
}
