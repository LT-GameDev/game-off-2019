using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public class GizmoDrawer : MonoBehaviour
    {
        private List<Gizmo> pinnedGizmos;
        private List<(Gizmo gizmo, int frames)> tempGizmos;

        public GizmoDrawer()
        {
            pinnedGizmos = new List<Gizmo>();
            tempGizmos   = new List<(Gizmo gizmo, int frames)>();
        }

        private void OnDrawGizmos()
        {
            // Draw pinned gizmos
            foreach (var gizmo in pinnedGizmos)
            {
                gizmo.Draw();
            }
            
            // Draw temp gizmos
            for (int i = tempGizmos.Count - 1; i > -1; i--)
            {
                var entry = tempGizmos[i];

                entry.gizmo.Draw();
                entry.frames--;

                if (entry.frames < 1)
                {
                    tempGizmos.RemoveAt(i);
                }
                else
                {
                    tempGizmos[i] = entry;
                }
            }
        }

        public void Draw(Gizmo gizmo, int frames = 60)
        {
            tempGizmos.Add((gizmo, frames));
        }

        public void Pin(Gizmo gizmo)
        {
            pinnedGizmos.Add(gizmo);
        }
        
        #region Singleton
        private static GizmoDrawer _instance;

        public static GizmoDrawer Instane()
        {
            if (!_instance)
            {
                var go = new GameObject("GizmoDrawer");
                _instance = go.AddComponent<GizmoDrawer>();
            }

            return _instance;
        }
        #endregion
    }

    public class Gizmo
    {
        private Action drawMethod;
        private Color color;

        public Gizmo(Action drawMethod, Color color)
        {
            this.drawMethod = drawMethod;
            this.color      = color;
        }

        public void Draw()
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            drawMethod();
            Gizmos.color = oldColor;
        }
    }

    public class SphereGizmo : Gizmo
    {
        public SphereGizmo(Vector3 position, float radius, Color color) 
            : base(() => Gizmos.DrawSphere(position, radius), color)
        { }
    }

    public class CubeGizmo : Gizmo
    {
        public CubeGizmo(Vector3 position, Vector3 size, Color color) 
            : base(() => Gizmos.DrawCube(position, size), color)
        { }
    }

    public class LineGizmo : Gizmo
    {
        public LineGizmo(Vector3 from, Vector3 to, Color color)
            : base(() => Gizmos.DrawLine(from, to), color)
        { }
    }
}