// Editor.ts
import { Shape } from './Shape';

export class VectorEditor {
    private shapes: Shape[] = [];
    private currentShape: Shape | null = null;
    private isDrawing: boolean = false;
    private startPoint = { x: 0, y: 0 };

    constructor(private canvas: HTMLCanvasElement, private ctx: CanvasRenderingContext2D) {
        this.initEvents();
    }

    private initEvents(): void {
        this.canvas.addEventListener('mousedown', (e) => {
            this.isDrawing = true;
            this.startPoint = { x: e.offsetX, y: e.offsetY };
        });

        this.canvas.addEventListener('mousemove', (e) => {
            if (!this.isDrawing) return;
            const x = Math.min(this.startPoint.x, e.offsetX);
            const y = Math.min(this.startPoint.y, e.offsetY);
            const w = Math.abs(this.startPoint.x - e.offsetX);
            const h = Math.abs(this.startPoint.y - e.offsetY);
            this.currentShape = { type: 'rect', x, y, width: w, height: h, color: 'blue' };
            this.render();
        });

        this.canvas.addEventListener('mouseup', () => {
            if (this.currentShape) this.shapes.push(this.currentShape);
            this.isDrawing = false;
            this.currentShape = null;
            this.render();
        });
    }

    private render(): void {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        [...this.shapes, this.currentShape].forEach(s => {
            if (!s) return;
            this.ctx.strokeStyle = s.color;
            this.ctx.strokeRect(s.x, s.y, s.width, s.height);
        });
    }

    public exportSVG(): void {
        let svg = `<svg xmlns="http://www.w3.org" width="${this.canvas.width}" height="${this.canvas.height}">\n`;
        this.shapes.forEach(s => {
            svg += `  <rect x="${s.x}" y="${s.y}" width="${s.width}" height="${s.height}" fill="none" stroke="${s.color}" stroke-width="2" />\n`;
        });
        svg += '</svg>';
        
        const blob = new Blob([svg], { type: 'image/svg+xml' });
        const a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'vector_art.svg';
        a.click();
    }
}
