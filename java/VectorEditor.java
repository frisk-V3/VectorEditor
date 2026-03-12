import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.*;
import java.util.*;

public class VectorEditor extends JFrame {
    private java.util.List<Rectangle> shapes = new ArrayList<>();
    private Point start;
    private Rectangle current;

    public VectorEditor() {
        setSize(800, 600);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        JPanel p = new JPanel() {
            protected void paintComponent(Graphics g) {
                super.paintComponent(g);
                for(Rectangle r : shapes) g.drawRect(r.x, r.y, r.width, r.height);
                if(current != null) g.drawRect(current.x, current.y, current.width, current.height);
            }
        };
        p.addMouseListener(new MouseAdapter() {
            public void mousePressed(MouseEvent e) { start = e.getPoint(); }
            public void mouseReleased(MouseEvent e) { shapes.add(current); current = null; }
        });
        p.addMouseMotionListener(new MouseMotionAdapter() {
            public void mouseDragged(MouseEvent e) {
                current = new Rectangle(Math.min(start.x, e.getX()), Math.min(start.y, e.getY()), Math.abs(start.x - e.getX()), Math.abs(start.y - e.getY()));
                repaint();
            }
        });
        addKeyListener(new KeyAdapter() {
            public void keyPressed(KeyEvent e) {
                if(e.getKeyCode() == KeyEvent.VK_S) saveSVG();
            }
        });
        add(p);
    }

    private void saveSVG() {
        try (PrintWriter pw = new PrintWriter("output.svg")) {
            pw.println("<svg xmlns='http://www.w3.org' width='800' height='600'>");
            for(Rectangle r : shapes) 
                pw.printf("  <rect x='%d' y='%d' width='%d' height='%d' fill='none' stroke='black'/>\n", r.x, r.y, r.width, r.height);
            pw.println("</svg>");
            JOptionPane.showMessageDialog(this, "Saved to output.svg");
        } catch (Exception ex) { ex.printStackTrace(); }
    }

    public static void main(String[] args) { new VectorEditor().setVisible(true); }
}
