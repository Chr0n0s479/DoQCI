import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from "@angular/router";

@Component({
  selector: 'app-header',
  imports: [ RouterLink, RouterLinkActive ],
  templateUrl: './header.html',
})
export class Header {
  tools = [
    { id: 'merge', label: 'Unir' },
    { id: 'split', label: 'Dividir' },
    { id: 'reorder', label: 'Reordenar' },
    { id: 'compress', label: 'Comprimir' },
    { id: 'ocr', label: 'Leer Texto' }
  ];
  activeTool: string = "";
  setActive(tool: string) {
    this.activeTool = tool;
  }
  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
}
