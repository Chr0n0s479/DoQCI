import { Component } from '@angular/core';
import { Header } from "../components/header/header";
import { RouterOutlet } from '@angular/router';
import { Footer } from "../components/footer/footer";

@Component({
  selector: 'app-layout',
  imports: [Header, RouterOutlet, Footer],
  templateUrl: './layout.html',
})
export class Layout {

}
