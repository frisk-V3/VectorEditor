// Shape.ts
export interface Shape {
    type: 'rect' | 'ellipse';
    x: number;
    y: number;
    width: number;
    height: number;
    color: string;
}
