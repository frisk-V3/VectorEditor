// Main.ts
import { VectorEditor } from './Editor';

const canvas = document.getElementById('canvas') as HTMLCanvasElement;
const saveBtn = document.getElementById('saveBtn') as HTMLButtonElement;

if (canvas) {
    const ctx = canvas.getContext('2d');
    if (ctx) {
        const editor = new VectorEditor(canvas, ctx);
        
        // 保存ボタンがクリックされたらSVGを出す
        saveBtn?.addEventListener('click', () => {
            editor.exportSVG();
        });
    }
}
