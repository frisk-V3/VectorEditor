using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SimpleVectorEditor
{
    public enum ShapeType { Rectangle, Ellipse }

    public class Shape
    {
        public ShapeType Type { get; set; }
        public Rectangle Bounds { get; set; }
        public Color Color { get; set; }
    }

    public class EditorForm : Form
    {
        private List<Shape> shapes = new List<Shape>();
        private ShapeType currentType = ShapeType.Rectangle;
        private Point startPoint;
        private Rectangle currentRect;
        private bool isDrawing = false;

        public EditorForm()
        {
            this.Text = "Vector Editor (Press 'S' to Save SVG)";
            this.DoubleBuffered = true;
            this.Size = new Size(800, 600);
            this.KeyDown += OnKeyDown; // キー入力イベント
            
            // 以下、マウスイベントなどは前回と同じ
            this.MouseDown += (s, e) => { if(e.Button == MouseButtons.Left){ isDrawing = true; startPoint = e.Location; } };
            this.MouseMove += (s, e) => {
                if (isDrawing) {
                    currentRect = new Rectangle(Math.Min(startPoint.X, e.X), Math.Min(startPoint.Y, e.Y), Math.Abs(startPoint.X - e.X), Math.Abs(startPoint.Y - e.Y));
                    this.Invalidate();
                }
            };
            this.MouseUp += (s, e) => {
                if (isDrawing) {
                    shapes.Add(new Shape { Type = currentType, Bounds = currentRect, Color = Color.Blue });
                    isDrawing = false;
                    this.Invalidate();
                }
            };
            this.Paint += OnPaint;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // 'S' キーを押したらSVG保存
            if (e.KeyCode == Keys.S) { ExportToSvg("output.svg"); }
            // 'R' で矩形、'E' で楕円に切り替え
            if (e.KeyCode == Keys.R) currentType = ShapeType.Rectangle;
            if (e.KeyCode == Keys.E) currentType = ShapeType.Ellipse;
        }

        private void ExportToSvg(string filename)
        {
            var svg = new StringBuilder();
            svg.AppendLine("<?xml version=\"1.0\" standalone=\"no\"?>");
            svg.AppendFormat("<svg width=\"{0}\" height=\"{1}\" xmlns=\"http://www.w3.org\">\n", this.Width, this.Height);

            foreach (var shape in shapes)
            {
                string colorHex = string.Format("#{0:X2}{1:X2}{2:X2}", shape.Color.R, shape.Color.G, shape.Color.B);
                if (shape.Type == ShapeType.Rectangle)
                    svg.AppendFormat("  <rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" stroke=\"{4}\" stroke-width=\"2\" fill=\"none\" />\n",
                        shape.Bounds.X, shape.Bounds.Y, shape.Bounds.Width, shape.Bounds.Height, colorHex);
                else
                    svg.AppendFormat("  <ellipse cx=\"{0}\" cy=\"{1}\" rx=\"{2}\" ry=\"{3}\" stroke=\"{4}\" stroke-width=\"2\" fill=\"none\" />\n",
                        shape.Bounds.X + shape.Bounds.Width/2, shape.Bounds.Y + shape.Bounds.Height/2, shape.Bounds.Width/2, shape.Bounds.Height/2, colorHex);
            }
            svg.AppendLine("</svg>");

            File.WriteAllText(filename, svg.ToString());
            MessageBox.Show(filename + " に保存しました！");
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            foreach (var s in shapes) {
                using (var p = new Pen(s.Color, 2)) {
                    if (s.Type == ShapeType.Rectangle) e.Graphics.DrawRectangle(p, s.Bounds);
                    else e.Graphics.DrawEllipse(p, s.Bounds);
                }
            }
        }

        [STAThread] static void Main() { Application.Run(new EditorForm()); }
    }
}
