using Microsoft.Xna.Framework;

namespace MonoGizmo
{
    public sealed class GizmoOptions
    {
        public float GizmoScale = 1.0f;

        #region GizmoSize
        public float OriginSize = 10.0f;
        public float TranslateLength = 50.0f;
        public float TranslateSize = 3.5f;
        public float RotationRadius = 30.0f;
        public float RotationThickness = 4.0f;
        public float ScaleLength = 75.0f;
        public float ScaleSize = 4.0f;
        #endregion

        #region Transform Step
        public float TranslateStep = 1.0f;
        public float ScaleStep = 5.0f;
        public float RotationStep = 1.0f;
        #endregion

        #region Colors
        public Color XAxisColor = new Color(255, 23, 68);
        public Color YAxisColor = new Color(41, 98, 254);
        public Color XYAxisColor = new Color(126, 111, 255);
        public Color RotationColor = new Color(141, 219, 4);
        #endregion
    }
}
